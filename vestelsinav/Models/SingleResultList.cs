using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vestelsinav.Models
{
    public class SingleResultList
    {
        public int egitimID { get; set; }

        public string sinavBasligi { get; set; }

        public int dogruSayisi { get; set; }

        public int toplamSoruSayisi { get; set; }
    }
}