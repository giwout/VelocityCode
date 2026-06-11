using CarMagazine2025_42.Data.Context;
using CarMagazine2025_42.Data.Models.Cart;
using CarMagazine2025_42.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarMagazine2025_42.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly BikeDbContext _db;
        private readonly ShopCart _shopCart;

        public ShopCartController(BikeDbContext db, ShopCart shopCart)
        {
            _db = db;
            _shopCart = shopCart;
        }

        public IActionResult Index()
        {
            // Берем актуальные данные из корзины
            var items = _shopCart.GetCartItems();
            _shopCart.ListShopItems = items;

            var viewModel = new ShopCartViewModel
            {
                ShopCart = _shopCart,
                Sum = items.Sum(x => x.Price)
            };

            return View(viewModel);
        }

        public IActionResult AddToCart(int bikeId)
        {
            var item = _db.Bikes.FirstOrDefault(i => i.BikeId == bikeId);
            if (item != null)
            {
                _shopCart.AddToCart(item);
                // ВОТ ЭТА СТРОЧКА ДАЕТ УВЕДОМЛЕНИЕ:
                TempData["Success"] = $"Велосипед {item.BikeName} успешно добавлен в корзину!";
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int bikeId)
        {
            var item = _db.Bikes.FirstOrDefault(i => i.BikeId == bikeId);
            if (item != null)
            {
                _shopCart.RemoveFromCart(item);
                TempData["Success"] = "Товар удален из корзины.";
            }
            return RedirectToAction("Index");
        }

    }
}