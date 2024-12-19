using BerberWeb.Business.Abstract;
using BerberWeb.DataAccess.Abstract;
using BerberWeb.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Business.Concrete
{
    public class AppointmentManager : GenericManager<Appointment>, IAppointmentService
    {
        public AppointmentManager(IGenericDal<Appointment> repository) : base(repository)
        {
        }
    }
}
