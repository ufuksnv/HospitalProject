using Hospital.Core.Models;
using Hospital.Core.Repositories;
using Hospital.Core.Services;
using Hospital.Core.UnitOfWorks;
using Hospital.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Service.Services
{
    public class AppointmentService : Service<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
      
        public AppointmentService(IAppointmentRepository appointmentRepository,IGenericRepository<Appointment> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _appointmentRepository = appointmentRepository;
        }

       

    }
}
