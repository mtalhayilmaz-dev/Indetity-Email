using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MYAcademy_IdentityEmail.Entities;
using MYAcademy_IdentityEmail.Models;

namespace MYAcademy_IdentityEmail.Controllers
{
    
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager = null)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userLoginViewModel);
            }

            var user = await _userManager.FindByNameAsync(userLoginViewModel.Username);

            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError("", "E-posta adresiniz henüz doğrulanmamış. Lütfen mailinize gelen kodu onaylayın.");
                return View(userLoginViewModel);
            }

            // 3. parametre (true): Beni hatırla / kalıcı çerez
            // 4. parametre (true): Başarısız girişlerde hesap kilitleme aktif
            var result = await _signInManager.PasswordSignInAsync(
                userLoginViewModel.Username,
                userLoginViewModel.Password,
                isPersistent: true,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return RedirectToAction("InBox", "Message");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Çok fazla yanlış deneme hesabınız kilitlendi.");
                return View(userLoginViewModel);
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "Email onayı yapılmamış veya giriş yasak.");
                return View(userLoginViewModel);
            }

            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
            return View(userLoginViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Login");
        }
    
    
    }
}
