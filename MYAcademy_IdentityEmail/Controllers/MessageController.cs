using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MYAcademy_IdentityEmail.Context;
using MYAcademy_IdentityEmail.Entities;

namespace MYAcademy_IdentityEmail.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult>  InBox()
        {
            var user = await _userManager.GetUserAsync(User);
            var value = _context.Messages.Where(x => x.ReceiverEmail == user.Email).ToList();

            return View(value);
        }
        public async Task<IActionResult> SendBox() 
        {


            var value = await _userManager.GetUserAsync(User);
            var messageList = _context.Messages.Where(x => x.SenderEmail == value.Email).ToList();
            return View(messageList);

        }
     
        
        public IActionResult CreateBox()
        {
            return View();
        }

        
        
        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> CreateMessage(Message m)
        {
            var values = await _userManager.GetUserAsync(User);
            m.SendDate = DateTime.Now;
            m.SenderEmail = values.Email;
            m.IsRead = false;
            m.CategoryID = 1;
            _context.Messages.Add(m);
            _context.SaveChanges();
            return RedirectToAction("SendBox");
        }




    }

}
