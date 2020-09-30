namespace vestelsinav.Models.Database_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Kullanici_Hareket
    {
        [Key]
        public int hareketID { get; set; }

        public DateTime? bitirmeTarihi { get; set; }

        public DateTime? baslamaTarihi { get; set; }

        public int? egitimID { get; set; }

        [StringLength(50)]
        public string verdigiCevaplar { get; set; }

        public int kullaniciID { get; set; }

        public virtual Egitim Egitim { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
