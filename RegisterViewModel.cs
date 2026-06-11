using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarMagazine2025_42.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email обязателен для заполнения")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        [DisplayName("Email адрес")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DisplayName("Подтверждение пароля")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}