using Hospital.Core.Models;
using Hospital.Core.Repositories;
using Hospital.Core.Services;
using Hospital.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Service.Services
{
    public class DoctorService : Service<Doctor>, IDoctorService
    {
        public DoctorService(IGenericRepository<Doctor> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
