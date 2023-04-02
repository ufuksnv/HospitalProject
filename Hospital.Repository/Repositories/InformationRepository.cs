using Hospital.Core.Models;
using Hospital.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repository.Repositories
{
    public class InformationRepository : GenericRepository<Information>, IInformationRepository
    {
        public InformationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
