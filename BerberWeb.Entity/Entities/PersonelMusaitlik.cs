using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Entity.Entities
{
    public class PersonelMusaitlik
    {
        public int PersonelMusaitlikId { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public DateTimeOffset BaslangicSaati { get; set; }
        public DateTimeOffset BitisSaati { get; set; }

    }
}
