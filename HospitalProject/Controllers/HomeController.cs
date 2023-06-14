using Hospital.Core.Models;
using Hospital.Core.Services;
using Hospital.Service.Services;
using HospitalProject.Models;
using HospitalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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
                Random random = new Random();
                int randomKey = random.Next(100000, 1000000);

                AppUser user = new AppUser()
                {
                    UserName = request.UserName,
                    PhoneNumber = request.Phone,
                    Email = request.Email,
                    ConfirmCode = randomKey
                };

                var identityResult = await _userManager.CreateAsync(user, request.Password);

                if (identityResult.Succeeded)
                {
                    await _emailService.SendConfirmCodeEmail(randomKey, request.Email!);
                    //TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıyla gerçekleşmiştir";
                    TempData["Mail"] = request.Email;

                    //return RedirectToAction(nameof(HomeController.SignUp));
                    return RedirectToAction(nameof(HomeController.CodeConfirm));
                }

                foreach (IdentityError item in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

            }
            return View(request);
        }

        [HttpGet]
        public IActionResult CodeConfirm()
        {
            var value = TempData["Mail"];
            ViewBag.v = value;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CodeConfirm(ConfirmMailViewModel confirmMailViewModel)
        {
            var user = await _userManager.FindByEmailAsync(confirmMailViewModel.Mail);
            if (user.ConfirmCode == confirmMailViewModel.ConfirmCode)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return RedirectToAction(nameof(HomeController.SignIn));
            }
            return View();
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

            if (hasUser.EmailConfirmed == false)
            {
                TempData["ConfirmMessage"] = "Lütfen hesabınızı onaylayınız.";
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

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            string userId = TempData["userId"]!.ToString();
            string token = TempData["token"]!.ToString();

            if (userId == null || token == null)
            {
                throw new Exception("Bir hata meydana geldi");
            }

            var hasUser = await _userManager.FindByIdAsync(userId);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(hasUser, token, request.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla yenilenmiştir";
            }
            else
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return View();
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


        public async Task<IActionResult> ExternalResponse(string ReturnUrl = "/")
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("SignIn");
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                if (result.Succeeded)
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    AppUser user = new AppUser();

                    user.Email = info.Principal.FindFirst(ClaimTypes.Email)!.Value;
                    string ExternalUserId = info.Principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                    if (info.Principal.HasClaim(x => x.Type == ClaimTypes.Name))
                    {
                        string userName = info.Principal.FindFirst(ClaimTypes.Name)!.Value;
                        userName = userName.Replace(' ', '-').ToLower() + ExternalUserId.Substring(0, 5).ToString();
                        user.UserName = userName;
                    }
                    else
                    {
                        user.UserName = info.Principal.FindFirst(ClaimTypes.Email)!.Value;
                    }

                    IdentityResult createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        IdentityResult loginResult = await _userManager.AddLoginAsync(user, info);

                        if (loginResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, true);
                            return Redirect(ReturnUrl);
                        }
                    }
                    else
                    {
                        foreach (IdentityError item in createResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, item.Description);
                        }
                    }

                }
            }

            return RedirectToAction("Error");
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