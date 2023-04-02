using FluentValidation;
using Hospital.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Service.ValidationRules
{
    public class AppointmentValidator : AbstractValidator<Appointment>
    {
        public AppointmentValidator()
        {
            RuleFor(x => x.DoctorName).NotEmpty().WithMessage("Lütfen Doktor Adını Boş Bırakmayınız!");
            RuleFor(x => x.AppointmentDate).NotEmpty().WithMessage("Lütfen Randevu Tarihini Boş Bırakmayınız!");
        }
    }
}
