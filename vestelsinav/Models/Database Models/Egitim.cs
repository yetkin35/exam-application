namespace vestelsinav.Models.Database_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Egitim")]
    public partial class Egitim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Egitim()
        {
            Kullanici_Hareket = new HashSet<Kullanici_Hareket>();
            Soru = new HashSet<Soru>();
        }

        public int egitimID { get; set; }

        public int egitimSuresi { get; set; }

        [Required]
        [StringLength(50)]
        public string konuBasligi { get; set; }

        public int soruSayisi { get; set; }

        [StringLength(50)]
        public string cevapAnahtari { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kullanici_Hareket> Kullanici_Hareket { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Soru> Soru { get; set; }
    }
}
