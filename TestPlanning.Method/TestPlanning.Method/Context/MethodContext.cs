using Microsoft.EntityFrameworkCore;
using TestPlanning.Common.Models;

namespace TestPlanning.Method.Context
{
    public class MethodContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                @"Server=localhost;Port=5432;Database=method;User Id=postgres;Password=12345;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("method_schema");
            modelBuilder.Entity<MethodModel>().ToTable("methods");
            modelBuilder.Entity<MethodModel>().HasKey(b => b.Id);
            modelBuilder.Entity<MethodModel>().Property(b => b.Id).HasColumnName("id"); 
            modelBuilder.Entity<MethodModel>().Property(b => b.Name).HasColumnName("name");
            modelBuilder.Entity<MethodModel>().Property(b => b.TimeStamp).HasColumnName("timestamp");
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<MethodModel> Methods { get; set; }
    }
}
