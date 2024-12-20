using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Entity.Entities
{
    public class Randevu
    {
        public int RandevuId { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }   
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }
        public DateTimeOffset StartDate { get; set; } // DateTime yerine DateTimeOffset
        public DateTimeOffset FinishDate { get; set; } // DateTime yerine DateTimeOffset
        public bool Onay { get; set; } = false;
        public bool Ret { get; set; } = false;
        public string Detay { get; set; }


    }
}
