using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime? BirthDay { get; set; }
        public int? Size { get; set; }
        public int? weight { get; set; }

        //public List<Appointment>? Appointments { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
