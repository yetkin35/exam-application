using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vestelsinav.Models
{
    public class PostModel
    {
        public int hareketID { get; set; }

        public int egitimID { get; set; }

        public int soruID { get; set; }

        public int kacinciSoru { get; set; }

        public char secenek { get; set; }

        public bool ileriYon { get; set; }

        public bool isDone { get; set; }

    }
}