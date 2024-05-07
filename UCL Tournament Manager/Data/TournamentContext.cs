using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UCL_Tournament_Manager.Models;

namespace UCL_Tournament_Manager.Data
{
    public class TournamentContext : DbContext
    {
        // DbSets for each model
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Match> Matches { get; set; }

        // Configure the connection string
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=UCL_Tournament;Trusted_Connection=True;");
            }
        }

        // Customize relationships, if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-many relationship between Tournament and Groups
            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Groups)
                .WithOne(g => g.Tournament)
                .HasForeignKey(g => g.TournamentId);

            // One-to-many relationship between Group and Teams
            modelBuilder.Entity<Group>()
                .HasMany(g => g.Teams)
                .WithOne(t => t.Group)
                .HasForeignKey(t => t.GroupId);

            // One-to-many relationship between Group and Matches
            modelBuilder.Entity<Group>()
                .HasMany(g => g.Matches)
                .WithOne(m => m.Group)
                .HasForeignKey(m => m.GroupId);
        }
    }
}
