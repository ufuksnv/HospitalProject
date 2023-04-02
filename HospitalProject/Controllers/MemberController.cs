using Hospital.Core.Models;
using Hospital.Core.Services;
using Hospital.Service.Services;
using HospitalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HospitalProject.Controllers
{
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IService<Appointment> _appointmentService;
        

        public MemberController( SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IService<Appointment> appointmentService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appointmentService = appointmentService;
            
        }
       

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            var userViewModel = new UserViewModel
            {
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                PhoneNumber = currentUser.PhoneNumber,
            };

            return View(userViewModel);
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

            var request = await _appointmentService.GetByIdAsync(id);
            request.AcceptanceDate = DateTime.Now;
            request.Title = "Alınmış Randevu";
            request.AppointmentStatus = false;
            request.AppUserId = UserId;

            await _appointmentService.UpdateAsync(request);

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
