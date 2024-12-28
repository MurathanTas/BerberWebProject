using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Entity.Entities
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public string ServiceName { get; set; }

        public string? ServiceImgUrl { get; set; }

        public decimal? Price { get; set; }
        public TimeSpan? Duration { get; set; }

        public List<ServicePersonel> ServicePersonels { get; set; }
        public List<Randevu> Randevus { get; set; }



    }
}
