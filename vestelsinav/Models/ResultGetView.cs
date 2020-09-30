using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vestelsinav.Models.Database_Models;

namespace vestelsinav.Models
{
    public class ResultGetView
    {
        public Kullanici k { get; set; }

        public List<Soru> soruList{ get; set; }

        public string verdigiCevaplar { get; set; }
    }
}