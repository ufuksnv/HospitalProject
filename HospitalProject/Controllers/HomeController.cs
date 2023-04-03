using Hospital.Core.Models;
using Hospital.Core.Services;
using Hospital.Service.Services;
using HospitalProject.Models;
using HospitalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HospitalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IService<Doctor> _doctorService;
        private readonly IService<Comment> _commentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, IService<Doctor> service, IService<Comment> commentService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _doctorService = service;
            _commentService = commentService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

       // kalanlar : üye randevu al, üye randevuarı gör, admin randevu hazırla, yorumları pasif yap
       // üye mesaj atar ve randevu tarihini verir, admin randevu bilgilerini girer(vazgeçtim)

        //kalanlar: randevu ekleme tamamlandı, üye randevuları görebiliyor, üye randevuları alma eklenecek(güncelleme alım tarihi şimdi statu false title müsait değil)
        //yorumları pasif yap eklenebilir


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    UserName = request.UserName,
                    PhoneNumber = request.Phone,
                    Email = request.Email
                };

                var identityResult = await _userManager.CreateAsync(user, request.Password);

                if (identityResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıyla gerçekleşmiştir";
                    return RedirectToAction(nameof(HomeController.SignUp));
                }

                foreach (IdentityError item in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

            }
            return View(request);
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request)
        {

            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if (hasUser == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı adınız veya parolanız hatalı lütfen tekrar deneyiniz.";
                return View();
            }

            var SignInresult = await _signInManager.PasswordSignInAsync(hasUser, request.Password, request.RememberMe, true);

            if (SignInresult.Succeeded)
            {
                return RedirectToAction("Index", "Member");
            }
            if (SignInresult.IsLockedOut)
            {
                TempData["LockedOutMessage"] = "3 dakika sonra tekrar deneyiniz";
                return View();
            }
            TempData["ErrorMessage"] = "Kullanıcı adınız veya parolanız hatalı lütfen tekrar deneyiniz.";
            return View(request);



        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if (hasUser == null)
            {
                TempData["ForgetErrorMessage"] = "Bu email adresine sahip kullanıcı bulunamadı.";
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new
            { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            //
            await _emailService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);

            TempData["success"] = "Şifre yenileme linki, eposta adresinize gönderilmiştir.";
            return RedirectToAction(nameof(ForgetPassword));
        }


        public async Task <IActionResult> DoctorList()
        {
            var request = await _doctorService.GetAllAsync();
            return View(request);
        }

        public async Task<IActionResult> CommentList()
        {
            var request = await _commentService.GetAllAsync();
            return View(request);
        }


        [HttpGet]
        public PartialViewResult PartialAddComment()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> PartialAddComment(Comment p)
        {
            p.CommentStatus = true;
            await _commentService.AddAsync(p);
            return RedirectToAction("CommentList", "Home");

        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}