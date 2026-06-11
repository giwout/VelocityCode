using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarMagazine2025_42.Data.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Название категории обязательно")]
        [StringLength(50)]
        [DisplayName("Название категории")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(250)]
        [DisplayName("Описание")]
        public string Description { get; set; } = string.Empty;

        // Навигационное свойство для связи один-ко-многим
        // Использование virtual позволяет Entity Framework реализовать Lazy Loading
        public virtual List<Bike> Bikes { get; set; } = new List<Bike>();
    }
}