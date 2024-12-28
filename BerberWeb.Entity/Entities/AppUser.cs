using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Entity.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public List<ServicePersonel> ServicePersonels { get; set; }

        public List<Randevu> Randevus { get; set; }
        public List<PersonelMusaitlik> PersonelMusaitliks { get; set; }


    }
}
