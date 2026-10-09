using System;
using System.Collections.Generic;
using AnimalShelterApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnimalShelterApi.Data;

public partial class AnimalShelterDbContext : DbContext
{
    public AnimalShelterDbContext(DbContextOptions<AnimalShelterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adopter> Adopters { get; set; }

    public virtual DbSet<AdoptionApplication> AdoptionApplications { get; set; }

    public virtual DbSet<Animal> Animals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adopter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Adopters__3214EC07AFBBB3E8");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<AdoptionApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Adoption__3214EC074CE2B6C6");

            entity.Property(e => e.ApplicationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Adopter).WithMany(p => p.AdoptionApplications)
                .HasForeignKey(d => d.AdopterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_Adopters");

            entity.HasOne(d => d.Animal).WithMany(p => p.AdoptionApplications)
                .HasForeignKey(d => d.AnimalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_Animals");
        });

        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Animals__3214EC0734C6D461");

            entity.Property(e => e.Breed).HasMaxLength(100);
            entity.Property(e => e.HealthStatus).HasMaxLength(100);
            entity.Property(e => e.IsAdoptable).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Species).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
