using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Entity.Entities
{
    public class ServicePersonel
    {

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

    }
}
