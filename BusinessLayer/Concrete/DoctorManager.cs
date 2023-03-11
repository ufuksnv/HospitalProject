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
    public class DoctorManager : IDoctorService
    {
        IDoctorRepository _doctorRepository;

        public DoctorManager(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public void TAdd(Doctor t)
        {
            _doctorRepository.Insert(t);
        }

        public void TDelete(Doctor t)
        {
            _doctorRepository.Delete(t);
        }

        public Doctor TGetByID(int id)
        {
            return _doctorRepository.GetByID(id);
        }

        public List<Doctor> TGetList()
        {
            return _doctorRepository.GetList();
        }

        public void TUpdate(Doctor t)
        {
            _doctorRepository.Update(t);
        }
    }
}
