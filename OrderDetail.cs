using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarMagazine2025_42.Data.Models.Order
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int BikeId { get; set; }

        // Помечаем [ForeignKey], чтобы EF точно понимал, на что ссылаемся
        [ForeignKey("BikeId")]
        public virtual Bike? Bike { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
    }
}