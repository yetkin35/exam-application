namespace vestelsinav.Models.Database_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kullanici")]
    public partial class Kullanici
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kullanici()
        {
            Kullanici_Hareket = new HashSet<Kullanici_Hareket>();
        }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        [Required]
        [StringLength(50)]
        public string sifre { get; set; }

        [Key]
        public int sicilNo { get; set; }

        [Required]
        [StringLength(50)]
        public string isim { get; set; }

        [Required]
        [StringLength(50)]
        public string soyisim { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kullanici_Hareket> Kullanici_Hareket { get; set; }
    }
}
