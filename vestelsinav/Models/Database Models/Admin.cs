namespace vestelsinav.Models.Database_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        public int sicilNo { get; set; }

        [Required]
        [StringLength(50)]
        public string isim { get; set; }

        [Required]
        [StringLength(50)]
        public string soyisim { get; set; }

        [Required]
        [StringLength(50)]
        public string sifre { get; set; }
    }
}
