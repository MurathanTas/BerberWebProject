﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.Entity.Entities
{
    public class Portfolio
    {
        [Key]
        public int PortfolioID { get; set; }
        public string ?Name { get; set; }
        public string ?ImageUrl { get; set; }
        public string ?ImageUrl2 { get; set; }
        public string ?Image1 { get; set; }
        public string ?Image2 { get; set; }
        public string ?Image3 { get; set; }
        public string ?Image4 { get; set; }
        public int Value { get; set; }
    }
}