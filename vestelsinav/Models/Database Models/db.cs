namespace vestelsinav.Models.Database_Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class db : DbContext
    {
        public db()
            : base("name=db")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Egitim> Egitim { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<Kullanici_Hareket> Kullanici_Hareket { get; set; }
        public virtual DbSet<Soru> Soru { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.isim)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.soyisim)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.sifre)
                .IsUnicode(false);

            modelBuilder.Entity<Egitim>()
                .Property(e => e.konuBasligi)
                .IsUnicode(false);

            modelBuilder.Entity<Egitim>()
                .Property(e => e.cevapAnahtari)
                .IsUnicode(false);

            modelBuilder.Entity<Egitim>()
                .HasMany(e => e.Soru)
                .WithRequired(e => e.Egitim)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .Property(e => e.isim)
                .IsUnicode(false);

            modelBuilder.Entity<Kullanici>()
                .Property(e => e.soyisim)
                .IsUnicode(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Kullanici_Hareket)
                .WithRequired(e => e.Kullanici)
                .HasForeignKey(e => e.kullaniciID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici_Hareket>()
                .Property(e => e.verdigiCevaplar)
                .IsUnicode(false);

            modelBuilder.Entity<Soru>()
                .Property(e => e.soruIcerigi)
                .IsUnicode(false);

            modelBuilder.Entity<Soru>()
                .Property(e => e.dogruCevap)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
