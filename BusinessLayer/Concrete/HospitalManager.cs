using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class HospitalManager : IHospitalService
    {
        IHospitalRepository _hospitalRepository;

        public HospitalManager(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        public void TAdd(Hospital t)
        {
            _hospitalRepository.Insert(t);
        }

        public void TDelete(Hospital t)
        {
            _hospitalRepository.Delete(t);
        }

        public Hospital TGetByID(int id)
        {
           return _hospitalRepository.GetByID(id);
        }

        public List<Hospital> TGetList()
        {
            return _hospitalRepository.GetList();
        }

        public void TUpdate(Hospital t)
        {
            _hospitalRepository.Update(t);
        }
    }
}
