namespace vestelsinav.Models.Database_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Soru")]
    public partial class Soru
    {
        public int soruID { get; set; }

        public int egitimID { get; set; }

        [Required]
        public string soruIcerigi { get; set; }

        public string aSecenegi { get; set; }

        public string bSecenegi { get; set; }

        public string cSecenegi { get; set; }

        public string dSecenegi { get; set; }

        [Required]
        [StringLength(1)]
        public string dogruCevap { get; set; }

        public virtual Egitim Egitim { get; set; }
    }
}
