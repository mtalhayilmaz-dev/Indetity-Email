using System.ComponentModel.DataAnnotations;

namespace MYAcademy_IdentityEmail.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Bu alanı boş geçemezsin")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bu alanı boş geçemezsin")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Bu alanı boş geçemezsin")]
        [EmailAddress(ErrorMessage = "Doğru bir email adresi girin")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bu alanı boş geçemezsin")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bu alanı boş geçemezsin")]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
        [MaxLength(30, ErrorMessage = "Şifre en fazla 30 karakter olabilir.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$",
            ErrorMessage = "Şifre en az 1 Büyük harf, 1 küçük harf, 1 rakam ve 1 özel karakter içermelidir.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bu alanı boş geçemezsin")]
        [Compare("Password", ErrorMessage = "Şifreler birbiriyle uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
    }
}