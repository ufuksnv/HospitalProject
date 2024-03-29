﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HospitalProject.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Lütfen Mail giriniz !")]
        [Display(Name = "Email :")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        public string? Email { get; set; }
    }
}
