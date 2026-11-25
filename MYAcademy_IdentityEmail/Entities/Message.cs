using System.ComponentModel.DataAnnotations;

namespace MYAcademy_IdentityEmail.Entities
{
    public class Message
    {
        [Key]
       
        public int MessageID { get; set; }
        
        
        [EmailAddress(ErrorMessage ="Lütfen doğru e-mail girin")]
        public string ReceiverEmail { get; set; }
       
        [EmailAddress(ErrorMessage = "Lütfen doğru e-mail girin")]
        public string SenderEmail { get; set; }
        
        public string Subject { get; set; }
        public DateTime SendDate { get; set; }
        public string MessageDetail { get; set; }
        public bool IsRead { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
