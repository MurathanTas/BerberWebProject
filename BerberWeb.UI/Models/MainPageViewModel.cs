using BerberWeb.Entity.Entities;

namespace BerberWeb.UI.Models
{
    public class MainPageViewModel
    {
        public About ?About { get; set; }
        public IEnumerable<Service> ?Services { get; set; }
        public Contact ?Contact { get; set; }
    }
}
