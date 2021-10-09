using Microsoft.EntityFrameworkCore;
using soccer.Data.Entities;

namespace soccer.Data
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<GroupDetail> GroupDetails { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Team>().HasIndex(t => t.Name).IsUnique();
        }
    }
}
