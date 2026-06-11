using CarMagazine2025_42.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CarMagazine2025_42.Data.Models.Cart
{
    public class ShopCartItem
    {
        public int Id { get; set; }

        // Поле для связи с сессией корзины
        public string ShopCartId { get; set; } = string.Empty;

        // Внешний ключ для связи с таблицей Bike
        public int BikeId { get; set; }

        // Навигационное свойство для EF
        public virtual Bike? Bike { get; set; }

        // Цена товара на момент добавления в корзину (важно, если цена изменится в каталоге)
        [Range(0, 9999999)]
        public decimal Price { get; set; }
    }
}