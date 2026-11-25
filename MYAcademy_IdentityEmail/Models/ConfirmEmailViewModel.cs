using System.ComponentModel.DataAnnotations;

namespace MYAcademy_IdentityEmail.Models
{
    public class ConfirmEmailViewModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Doğrulama kodunu boş geçemezsin.")]
        public int ValidationCode { get; set; }
    }
}
