using FitFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using static System.Collections.Specialized.BitVector32;

namespace FitFlow.Infrastructure.Persistence;

public class FitFlowDbContext : DbContext
{
    public FitFlowDbContext(DbContextOptions<FitFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Domain.Entities.Section> Sections => Set<Domain.Entities.Section>();
    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();
    public DbSet<Visit> Visits => Set<Visit>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150);
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Specialization).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.Property(x => x.Price).HasPrecision(10, 2);

            entity.HasOne(x => x.Client)
                .WithMany(x => x.Memberships)
                .HasForeignKey(x => x.ClientId);
        });

        modelBuilder.Entity<Domain.Entities.Section>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);

            entity.HasOne(x => x.Trainer)
                .WithMany(x => x.Sections)
                .HasForeignKey(x => x.TrainerId);
        });

        modelBuilder.Entity<TrainingSession>(entity =>
        {
            entity.HasOne(x => x.Section)
                .WithMany(x => x.TrainingSessions)
                .HasForeignKey(x => x.SectionId);

            entity.HasOne(x => x.Trainer)
                .WithMany(x => x.TrainingSessions)
                .HasForeignKey(x => x.TrainerId);
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasOne(x => x.Client)
                .WithMany(x => x.Visits)
                .HasForeignKey(x => x.ClientId);

            entity.HasOne(x => x.Membership)
                .WithMany(x => x.Visits)
                .HasForeignKey(x => x.MembershipId);

            entity.HasOne(x => x.TrainingSession)
                .WithMany(x => x.Visits)
                .HasForeignKey(x => x.TrainingSessionId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(x => x.Amount).HasPrecision(10, 2);

            entity.HasOne(x => x.Client)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.ClientId);

            entity.HasOne(x => x.Membership)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.MembershipId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}