using System.ComponentModel.DataAnnotations;

namespace MYAcademy_IdentityEmail.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage ="Bu Alanı Boş Bırakamazsın")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bu Alanı Boş Bırakamazsın")]
        public string Password{ get; set; }

    }
}
