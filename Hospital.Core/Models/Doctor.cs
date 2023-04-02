using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? Degree { get; set; }
        public string? DoctorsSchool { get; set; }

    }
}
