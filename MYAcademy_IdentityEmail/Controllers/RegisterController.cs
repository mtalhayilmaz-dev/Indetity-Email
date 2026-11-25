using MailKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using MYAcademy_IdentityEmail.Entities;
using MYAcademy_IdentityEmail.Models;
using System.Xml.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MYAcademy_IdentityEmail.Controllers
{
   
    public class RegisterController : Controller
    {
        //Kullanıcı üye olsun diye yapıyoruz

        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public RegisterController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> SignUp(UserRegisterViewModel userRegisterViewModel)
        {
            Random random = new Random();
            int validationCode = random.Next(100000, 1000000);
            AppUser appUser = new AppUser()
            {
                Name = userRegisterViewModel.Name,
                Surname = userRegisterViewModel.Surname,
                Email = userRegisterViewModel.Email,
                UserName = userRegisterViewModel.Username,
                ValidCode = validationCode,
                EmailVerificationCodeExpiry = DateTime.Now.AddMinutes(3),
            };

            var result = await _userManager.CreateAsync(appUser, userRegisterViewModel.Password);


            if (result.Succeeded)
            {
                var host = _configuration["SmtpSettings:Host"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]!);
                var senderName = _configuration["SmtpSettings:SenderName"];
                var senderEmail = _configuration["SmtpSettings:SenderEmail"];
                var password = _configuration["SmtpSettings:Password"];

                MimeMessage mimeMessage = new MimeMessage();
                MailboxAddress mailboxAddressFrom = new MailboxAddress(senderName, senderEmail);
                mimeMessage.From.Add(mailboxAddressFrom);

                MailboxAddress mailboxAddressTo = new MailboxAddress(userRegisterViewModel.Name + " " + userRegisterViewModel.Surname, userRegisterViewModel.Email);
                mimeMessage.To.Add(mailboxAddressTo);


                mimeMessage.Subject = "Dogrulama Kodu";



                var bodyBuilder = new BodyBuilder();

                bodyBuilder.TextBody = "Dogrulama Kodunuz: " + validationCode + " 3 dakika için gecerlidir";

                mimeMessage.Body = bodyBuilder.ToMessageBody();




                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Connect(host, port, false);
                smtpClient.Authenticate(senderEmail, password);
                smtpClient.Send(mimeMessage);
                smtpClient.Disconnect(true);




                return RedirectToAction("ConfirmEmail", "Register", new { email = appUser.Email });
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);

            }

            return View(userRegisterViewModel);

        }

        [HttpGet]
        public IActionResult ConfirmEmail(string email)
        {
            var model = new ConfirmEmailViewModel
            {
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Bulunamadı");
                return View(model);
            }
            if (user.EmailVerificationCodeExpiry == null || DateTime.Now > user.EmailVerificationCodeExpiry)
            {
                ModelState.AddModelError("", "Doğrulama kodunun süresi doldu.");
                return View(model);
            }

            if (user.ValidCode != model.ValidationCode)
            {
                ModelState.AddModelError("", "Doğrulama kodu hatalı.");
                return View(model);

            }

            user.EmailConfirmed = true;
            user.ValidCode = null;
            user.EmailVerificationCodeExpiry = null;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("SignIn", "Login");

        }




        [HttpPost]
        public async Task<IActionResult> ResendCode(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("SignUp");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return RedirectToAction("SignUp");
            }

            Random random = new Random();
            int newCode = random.Next(100000, 1000000);

            user.ValidCode = newCode;
            user.EmailVerificationCodeExpiry = DateTime.Now.AddMinutes(3);

            await _userManager.UpdateAsync(user);

            var host = _configuration["SmtpSettings:Host"];
            var port = int.Parse(_configuration["SmtpSettings:Port"]!);
            var senderName = _configuration["SmtpSettings:SenderName"];
            var senderEmail = _configuration["SmtpSettings:SenderEmail"];
            var password = _configuration["SmtpSettings:Password"];

            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddressFrom = new MailboxAddress(senderName, senderEmail);
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress($"{user.Name} {user.Surname}", user.Email);
            mimeMessage.To.Add(mailboxAddressTo);

            mimeMessage.Subject = "Yeni Dogrulama Kodu";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Yeni Dogrulama Kodunuz: {newCode} (3 dakika icinde gecerlidir.)";
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();
            smtpClient.Connect(host, port, false);
            smtpClient.Authenticate(senderEmail, password);
            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);

           
            return RedirectToAction("ConfirmEmail", "Register", new { email = user.Email });
        }





    }

}