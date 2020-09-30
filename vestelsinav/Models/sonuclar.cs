using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace vestelsinav.Models
{
    public class sonuclar
    {
        SqlConnection baglanti = new SqlConnection();
        List<sonuclar> sonucListesi = new List<sonuclar>();
        public int sicilNo { get; set; }
        public string isim { get; set; }
        public string soyisim { get; set; }


        sonuclar s = null;

        public List<sonuclar> SonucAl()
        {
            baglanti.ConnectionString = "data source=TELEFON4137\\SQLEXPRESS;Initial Catalog=VESTELSINAV; User Id=sa; password=asd.1234";
            baglanti.Open();

            SqlCommand komut = new SqlCommand("select * from Kullanici", baglanti);
            DataTable tablo = new DataTable();
            SqlDataAdapter adaptor = new SqlDataAdapter(komut);
            adaptor.Fill(tablo);
            int i = Convert.ToInt32(tablo.Rows.Count);
            for (int j = 0; j < i; j++)
            {
                s = new Models.sonuclar();
                s.sicilNo = Convert.ToInt32(tablo.Rows[j][1]);
                s.isim = Convert.ToString(tablo.Rows[j][2]);
                s.soyisim = Convert.ToString(tablo.Rows[j][3]);
                sonucListesi.Add(s);

            }
            return sonucListesi;



        }
    }
}