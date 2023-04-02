using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Information
    {
        [Key]
        public int HospitalId { get; set; }
        public string? Address { get; set; }
        public string? FacebookAddress { get; set; }
        public int PhoneNumber { get; set; }
    }
}
