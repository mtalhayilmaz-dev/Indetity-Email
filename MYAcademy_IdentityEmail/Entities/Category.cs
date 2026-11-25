using System.ComponentModel.DataAnnotations;

namespace MYAcademy_IdentityEmail.Entities
{
    public class Category
    {
        [Key]
        public int categoryID { get; set; }

        [Required(ErrorMessage ="Kategori Adı Boş Bırakılamaz")]
        public string CategoryName { get; set; }
      
        public List<Message> Messages  { get; set; }
    }
    




}
