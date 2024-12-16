using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Entity.Entities
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
