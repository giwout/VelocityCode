using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarMagazine2025_42.Data.Models.Order
{
    public class Order
    {
        [BindNever]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [StringLength(25)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите фамилию")]
        [StringLength(25)]
        public string Sname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите адрес доставки")]
        [StringLength(100)]
        public string Adres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите корректный Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [BindNever]
        public DateTime OrderTime { get; set; }

        [BindNever]
        public string? UserId { get; set; }

        // Инициализируем список, чтобы избежать NullReferenceException
        public virtual List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}