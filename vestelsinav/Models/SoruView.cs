using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace vestelsinav.Models
{
    public class SoruView
    {
        public int soruID { get; set; }

        public int egitimID { get; set; }

        public string soruIcerigi { get; set; }

        public string aSecenegi { get; set; }

        public string bSecenegi { get; set; }

        public string cSecenegi { get; set; }

        public string dSecenegi { get; set; }

        [StringLength(1)]
        public string dogruCevap { get; set; }

        public int hareketID { get; set; }

        public char kayitliCevap { get; set; }

        public int kacinciSoru { get; set; }

        public bool sonMu { get; set; }
    }
}