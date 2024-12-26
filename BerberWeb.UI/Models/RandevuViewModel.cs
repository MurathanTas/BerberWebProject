using BerberWeb.Entity.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BerberWeb.UI.Models
{
    public class RandevuViewModel
    {
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int AppUserId { get; set; }
        [Required(ErrorMessage = "Personel seçimi zorunludur.")]
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }
        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [DataType(DataType.DateTime, ErrorMessage = "Geçerli bir tarih giriniz.")]
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset FinishDate { get; set; }
        public TimeSpan? Duration { get; set; }


        [ValidateNever]
        public List<SelectListItem> Services { get; set; }

        [ValidateNever]
        public List<SelectListItem> Personels { get; set; }
    }
}
