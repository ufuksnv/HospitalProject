using Hospital.Core.Models;
using Hospital.Core.Services;
using Hospital.Service.Services;
using HospitalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace HospitalProject.Controllers
{
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IService<Appointment> _appointmentService;
        private readonly IEmailService _emailService;
        private readonly IFileProvider _fileProvider;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IService<Appointment> appointmentService, IEmailService emailService, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appointmentService = appointmentService;
            _emailService = emailService;
            _fileProvider = fileProvider;
        }


        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            var userViewModel = new UserViewModel
            {
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture,               

            };

            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {
            
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);

            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                BirthDay = currentUser.BirthDay,             
            };

            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);

            currentUser.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.PhoneNumber = request.Phone;
            currentUser.BirthDay = request.BirthDay;
            

            //addPicture
            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                string randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "userpictures").PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;

            }
            //addPictureEnd


            var updateToUserResult = await _userManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                foreach (IdentityError item in updateToUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    return View();
                }
            }

           // await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);

            TempData["SuccessMessage"] = "Bilgileriniz güncellenmiştir";


            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                BirthDay = currentUser.BirthDay,              
            };

            return View(userEditViewModel);
        }

        [HttpGet]
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);
            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, request.PasswordOld, request.PasswordNew);

            if (!resultChangePassword.Succeeded)
            {
                foreach (IdentityError item in resultChangePassword.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    return View();
                }
            }

            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);

            TempData["SuccessMessage"] = "Şifreniz değiştirilmiştir";

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> AppointmentList()
        {
            var request = await _appointmentService.GetAllAsync();
            return View(request);
        }

        public async Task<IActionResult> TakeAppointment(int id)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            var UserId = currentUser.Id;
            var UserMail = currentUser.Email;

            var request = await _appointmentService.GetByIdAsync(id);
            request.AcceptanceDate = DateTime.Now;
            request.Title = "Alınmış Randevu";
            request.AppointmentStatus = false;
            request.AppUserId = UserId;

            await _appointmentService.UpdateAsync(request);

            await _emailService.SendTakeAppointmentEmail(UserMail);

            return RedirectToAction("AppointmentList", "Member");



        }





        //public async Task<IActionResult> AppointmentListByMember()
        //{
        //    var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
        //    var UserId = currentUser.Id;

        //    var values = _appointmentService
        //    return View(values);

        //}

    }
}
