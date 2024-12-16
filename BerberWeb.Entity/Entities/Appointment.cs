using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Entity.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public DateTimeOffset StartDate { get; set; } // DateTime yerine DateTimeOffset
        public DateTimeOffset FinishDate { get; set; } // DateTime yerine DateTimeOffset
        public bool IsAvailable { get; set; } = true;
        public int? AppUserId { get; set; } //Personeli temsil ediyor.
        public AppUser AppUser { get; set; }
        public List<AppointmentRegister> AppointmentRegisters { get; set; }



    }
}
