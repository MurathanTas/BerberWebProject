using BerberWeb.Entity.Entities;

namespace BerberWeb.UI.Models
{
    public class ServicePersonelViewModel
    {
        public string ServiceName { get; set; }
        public List<AppUser> AssignedPersonels { get; set; }
        public int serviceID { get; set; }

    }
}
