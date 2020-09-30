using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vestelsinav.Models
{
    public class ResultView
    {
        public int sicilNo { get; set; }

        public int egitimID { get; set; }

        public int toplamSoruSayisi { get; set; }

        public int dogruSayisi { get; set; }

        public string verdigiCevaplar { get; set; }

        public double bitirmeSuresi { get; set; }

        public string isim { get; set; }

        public string soyisim { get; set; }
    }
}