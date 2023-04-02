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
    public class InformationService : Service<Information>, IInformationService
    {
        public InformationService(IGenericRepository<Information> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
