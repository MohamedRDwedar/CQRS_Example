using Microsoft.EntityFrameworkCore;
using TestPlanning.Common.Models;

namespace TestPlanning.Experiment.Context
{
    public class ExperimentContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                @"Server=localhost;Port=5432;Database=experiment;User Id=postgres;Password=12345;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("experiment_schema");
            modelBuilder.Entity<ExperimentModel>().ToTable("experiments");
            modelBuilder.Entity<ExperimentModel>().HasKey(b => b.Id);
            modelBuilder.Entity<ExperimentModel>().Property(b => b.Id).HasColumnName("id");
            modelBuilder.Entity<ExperimentModel>().Property(b => b.Name).HasColumnName("name");
            modelBuilder.Entity<ExperimentModel>().Property(b => b.MethodId).HasColumnName("methodid");
            modelBuilder.Entity<ExperimentModel>().Property(b => b.TimeStamp).HasColumnName("timestamp");

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<ExperimentModel> Experiments { get; set; }
    }
}