using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarMagazine2025_42.Data.Models
{
    public class Bike
    {
        public int BikeId { get; set; }

        [Required(ErrorMessage = "Введите название велосипеда")]
        [DisplayName("Название")]
        [StringLength(100)]
        public string BikeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите бренд")]
        [DisplayName("Бренд")]
        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [DisplayName("Краткое описание")]
        public string ShortDescription { get; set; } = string.Empty;

        [DisplayName("Полное описание")]
        public string LongDescription { get; set; } = string.Empty;

        [DisplayName("Ссылка на фото")]
        public string Img { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите цену")]
        [Range(1, 1000000, ErrorMessage = "Цена должна быть от 1 до 1 000 000")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Цена")]
        public decimal Price { get; set; }

        [DisplayName("Размер рамы")]
        public string FrameSize { get; set; } = string.Empty;

        [DisplayName("Диаметр колес")]
        public double WheelDiameter { get; set; }

        [DisplayName("Материал рамы")]
        public string FrameMaterial { get; set; } = string.Empty;

        [DisplayName("Избранный товар")]
        public bool IsFavourite { get; set; }

        [DisplayName("В наличии")]
        public bool Available { get; set; }

        [DisplayName("Категория")]
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }


    }
}