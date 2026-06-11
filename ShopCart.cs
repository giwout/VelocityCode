using CarMagazine2025_42.Data.Context;
using CarMagazine2025_42.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarMagazine2025_42.Data.Models.Cart
{
    public class ShopCart
    {
        private readonly BikeDbContext _db;
        public ShopCart(BikeDbContext db) { _db = db; }

        public string ShopCartId { get; set; } = string.Empty;
        public List<ShopCartItem> ListShopItems { get; set; } = new List<ShopCartItem>();

        public static ShopCart GetCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session
                ?? throw new InvalidOperationException("Сессия не найдена");
            var context = services.GetRequiredService<BikeDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShopCart(context) { ShopCartId = cartId };
        }

        public void AddToCart(Bike bike)
        {
            _db.ShopCartItems.Add(new ShopCartItem { ShopCartId = ShopCartId, Bike = bike, Price = bike.Price });
            _db.SaveChanges();
        }

        public List<ShopCartItem> GetCartItems()
        {
            return _db.ShopCartItems.Where(c => c.ShopCartId == ShopCartId).Include(s => s.Bike).ToList();
        }

        public void RemoveFromCart(Bike bike)
        {
            // Ищем конкретный элемент корзины для текущей сессии
            var item = _db.ShopCartItems.FirstOrDefault(s => s.ShopCartId == ShopCartId && s.Bike.BikeId == bike.BikeId);
            if (item != null)
            {
                _db.ShopCartItems.Remove(item);
                _db.SaveChanges();
            }
        }

        public void ClearCart()
        {
            var items = _db.ShopCartItems.Where(c => c.ShopCartId == ShopCartId);
            _db.ShopCartItems.RemoveRange(items);
            _db.SaveChanges();
        }
    }
}