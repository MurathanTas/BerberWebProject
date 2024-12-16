using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Entity.Entities
{
    public class AppointmentRegister
    {
        public int AppointmentRegisterId { get; set; }
        public int AppUserId { get; set; } // Customer ı temsil ediyor
        public AppUser AppUser { get; set; }
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

    }
}
