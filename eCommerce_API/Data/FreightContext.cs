using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using eCommerce_API.Models;

namespace eCommerce_API.Data
{
    public partial class FreightContext : DbContext
    {
        public FreightContext()
        {
        }

        public FreightContext(DbContextOptions<FreightContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<FreightSettings> FreightSettings { get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //             optionsBuilder.UseSqlServer("Server=192.168.1.248\\sql2014;Database=wanfang_cloud14;User Id=eznz;password=9seqxtf7");
                //             optionsBuilder.UseSqlServer("Server=localhost;Database=rst374_cloud12;User Id=;password=;Trusted_Connection=True");
                //optionsBuilder.UseSqlServer("Server=192.168.1.218\\sqlexpress;Database=rst374_cloud12;User Id=eznz;password=9seqxtf7");
                //             optionsBuilder.UseSqlServer("Server=192.168.1.218\\sql2008;Database=onestopshop08;User Id=eznz;password=9seqxtf7");
                optionsBuilder.UseSqlServer("Server=192.168.10.204\\sql2014;Database=wanfang_cloud14;User Id=eznz;password=9seqxtf7");
                //               optionsBuilder.UseSqlServer("Server=192.168.1.218\\sql2008;Database=acq_new20;User Id=eznz;password=9seqxtf7");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.ToTable("settings");

                entity.HasIndex(e => e.Cat)
                    .HasName("IDX_settings_cat");

                entity.HasIndex(e => e.Hidden)
                    .HasName("IDX_settings_hidden");

                entity.HasIndex(e => e.Name)
                    .HasName("IDX_settings_name");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Access).HasColumnName("access");

                entity.Property(e => e.BoolValue)
                    .IsRequired()
                    .HasColumnName("bool_value")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.Cat)
                    .HasColumnName("cat")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Hidden)
                    .IsRequired()
                    .HasColumnName("hidden")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(1024)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FreightSettings>(entity =>
            {
                entity.ToTable("freight_settings");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("(1)");

                entity.Property(e => e.Region)
                    .HasColumnName("region")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Freight)
                    .HasColumnName("freight")
                    .HasColumnType("money")
                     .HasDefaultValueSql("(0)");
                entity.Property(e => e.FreeshippingActiveAmount)
                    .HasColumnName("freeshipping_active_amount")
                    .HasColumnType("money")
                     .HasDefaultValueSql("(0)");
            });
        }
    }
}
