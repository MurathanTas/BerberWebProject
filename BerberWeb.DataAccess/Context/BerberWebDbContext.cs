using BerberWeb.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.DataAccess.Context
{
    public class BerberWebDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public BerberWebDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ara tablonun birincil anahtarını belirleme
            modelBuilder.Entity<ServicePersonel>()
                .HasKey(sp => new { sp.ServiceId, sp.AppUserId });

            // İlişkileri tanımlama
            modelBuilder.Entity<ServicePersonel>()
                .HasOne(sp => sp.Service)
                .WithMany(s => s.ServicePersonels)
                .HasForeignKey(sp => sp.ServiceId);

            modelBuilder.Entity<ServicePersonel>()
                .HasOne(sp => sp.AppUser)
                .WithMany(u => u.ServicePersonels)
                .HasForeignKey(sp => sp.AppUserId);


        }

        public DbSet<About> About { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServicePersonel> ServicePersonels { get; set; }
        public DbSet<PersonelMusaitlik> PersonelMusaitliks { get; set; }
        public DbSet<Randevu> Randevus { get; set; }
        public DbSet<Personel> Personels { get; set; }
        public DbSet<Deneme> Denemes { get; set; }
    }
}
