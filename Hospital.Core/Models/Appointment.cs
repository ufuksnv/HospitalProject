using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public string? Title { get; set; } 
        public string? DoctorName { get; set; } 
        public DateTime AppointmentDate { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public bool AppointmentStatus { get; set; }

       
        
        public AppUser? AppUser { get; set; }
        public string? AppUserId { get; set; }
    }
}
