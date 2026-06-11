using CarMagazine2025_42.Data.Context;
using CarMagazine2025_42.Data.Models;
using CarMagazine2025_42.Data.Models.Cart;
using CarMagazine2025_42.Data.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarMagazine2025_42.Controllers
{
    [Authorize] // Весь контроллер доступен только авторизованным
    public class OrderController : Controller
    {
        private readonly BikeDbContext _db;
        private readonly ShopCart _shopCart;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(BikeDbContext db, ShopCart shopCart, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _shopCart = shopCart;
            _userManager = userManager;
        }

        [AllowAnonymous] // Оформление доступно всем
        [HttpGet]
        public IActionResult Checkout() => View();

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Order order)
        {
            ModelState.Remove("OrderDetails"); // Игнорируем коллекцию деталей при валидации формы

            if (!ModelState.IsValid) return View(order);

            var items = _shopCart.GetCartItems();
            if (items == null || !items.Any())
            {
                ModelState.AddModelError("", "Ваша корзина пуста.");
                return View(order);
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (User.Identity.IsAuthenticated)
                        order.UserId = _userManager.GetUserId(User);

                    order.OrderTime = DateTime.Now;
                    _db.Orders.Add(order);
                    _db.SaveChanges();

                    foreach (var item in items)
                    {
                        var detail = new OrderDetail
                        {
                            BikeId = item.Bike.BikeId,
                            OrderId = order.Id,
                            Price = item.Bike.Price
                        };
                        _db.OrderDetails.Add(detail);
                    }

                    _db.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Ошибка при оформлении заказа.");
                    return View(order);
                }
            }

            _shopCart.ClearCart();
            return RedirectToAction("Complete");
        }

        public IActionResult Complete() => View();

        public IActionResult MyOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _db.Orders.Where(o => o.UserId == userId).OrderByDescending(o => o.OrderTime).ToList();
            return View(orders);
        }

        // НОВЫЙ МЕТОД: Просмотр состава конкретного заказа
        public IActionResult OrderDetails(int orderId)
        {
            // Отладочный лог: выведем все ID, чтобы увидеть, есть ли там нужный нам
            var allIds = _db.Orders.Select(o => o.Id).ToList();

            var order = _db.Orders
                           .Include(o => o.OrderDetails)
                           .ThenInclude(od => od.Bike)
                           .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return Content($"Заказ ID: {orderId} не найден. Доступные ID в БД: {string.Join(", ", allIds)}");
            }

            // Проверка прав
            if (User.IsInRole("Admin") || order.UserId == _userManager.GetUserId(User))
                return View(order);

            return Forbid();
        }
    }
}