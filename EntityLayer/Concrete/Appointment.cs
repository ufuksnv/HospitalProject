using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public string? Title { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public bool AppointmentStatus { get; set; }

        public string? Id { get; set; }
        public AppUser? AppUser { get; set; }

    }
}
