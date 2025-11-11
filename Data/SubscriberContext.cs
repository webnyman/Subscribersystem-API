using Microsoft.EntityFrameworkCore;
using Subscribersystem_API.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Subscribersystem_API.Data
{
    public class SubscriberContext : DbContext
    {
        public SubscriberContext(DbContextOptions<SubscriberContext> options) : base(options) { }

        public DbSet<Subscriber> Subscribers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscriber>(entity =>
            {
                entity.ToTable("tbl_subscribers");

                entity.HasKey(e => e.SubscriptionNumber)
                      .HasName("PK_tbl_subscribers");

                entity.Property(e => e.SubscriptionNumber)
                      .HasColumnName("sub_prenr")
                      .HasMaxLength(20);

                entity.Property(e => e.PersonalNumber)
                      .HasColumnName("sub_personnr")
                      .HasMaxLength(20);

                entity.Property(e => e.FirstName)
                      .HasColumnName("sub_fornamn")
                      .HasMaxLength(50);

                entity.Property(e => e.LastName)
                      .HasColumnName("sub_efternamn")
                      .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                      .HasColumnName("sub_telefon")
                      .HasMaxLength(20);

                entity.Property(e => e.DeliveryAddress)
                      .HasColumnName("sub_adr_utm")
                      .HasMaxLength(100);

                entity.Property(e => e.PostalCode)
                      .HasColumnName("sub_postnr")
                      .HasMaxLength(10);

                entity.Property(e => e.City)
                      .HasColumnName("sub_ort")
                      .HasMaxLength(50);
            });
        }
    }
}
