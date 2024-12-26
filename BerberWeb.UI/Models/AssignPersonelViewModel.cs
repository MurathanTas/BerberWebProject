using BerberWeb.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BerberWeb.UI.Models
{
    public class AssignPersonelViewModel
    {
        public int ServiceId { get; set; }
        public int SelectedPersonelId { get; set; }
        public List<SelectListItem> Personels { get; set; }
        public List<AppUser> AssignedPersonels { get; set; } = new List<AppUser>();  
    }
}
