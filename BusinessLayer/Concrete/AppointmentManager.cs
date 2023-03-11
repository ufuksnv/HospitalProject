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
    public class AppointmentManager : IAppointmentService
    {
        IAppointmentRepository _appointmentRepository;

        public AppointmentManager(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public void TAdd(Appointment t)
        {
            _appointmentRepository.Insert(t);
        }

        public void TDelete(Appointment t)
        {
            _appointmentRepository.Delete(t);
        }

        public Appointment TGetByID(int id)
        {
            return _appointmentRepository.GetByID(id);
        }

        public List<Appointment> TGetList()
        {
            return _appointmentRepository.GetList();
        }

        public void TUpdate(Appointment t)
        {
            _appointmentRepository.Update(t);
        }
    }
}
