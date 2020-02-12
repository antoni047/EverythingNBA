using Microsoft.EntityFrameworkCore;

using EverythingNBA.Models;
using EverythingNBA.Models.MappingTables;

namespace EverythingNBA.Data
{
    public class EverythingNBADbContext : DbContext
    {
        public EverythingNBADbContext()
        {

        }

        public EverythingNBADbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<AllStarTeam> AllStarTeams { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<CloudinaryImage> CloudinaryImages { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Playoff> Playoffs { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<GameStatistic> GameStatistics { get; set; }
        public DbSet<SeasonStatistic> SeasonStatistics { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<PlayerSeasonStatistic> PlayerSeasonStatistics { get; set; }
        public DbSet<AllStarTeamsPlayers> AllStarTeamsPlayers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }

            //optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<AllStarTeamsPlayers>()
                .HasKey(ap => new { ap.AllStarTeamId, ap.PlayerId });

            ////Many-To-Many
            //builder.Entity<AllStarTeamsPlayers>()
            //    .HasOne(ap => ap.AllStarTeam)
            //    .WithMany(a => a.Players)
            //    .HasForeignKey(ap => ap.AllStarTeamId);

            //builder.Entity<AllStarTeamsPlayers>()
            //   .HasOne(ap => ap.Player)
            //   .WithMany(a => a.AllStarTeams)
            //   .HasForeignKey(ap => ap.PlayerId);

            ////One-To-Many/Many-To-One

            builder.Entity<GameStatistic>()
                .HasOne(g => g.Game)
                .WithMany(g => g.PlayerStats)
                .HasForeignKey(p => p.GameId);

            //builder.Entity<SingleGameStatistic>()
            //    .HasOne(ss => ss.Player)
            //    .WithMany(p => p.SingleGameStatistics)
            //    .HasForeignKey(ss => ss.PlayerId);

            builder.Entity<Game>()
                .HasOne(g => g.Team2)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(g => g.Team2Id);

            //builder.Entity<Game>()
            //    .HasOne(g => g.Winner)
            //    .WithMany(t => t.GamesWon)
            //    .HasForeignKey(g => g.WinnerId);

            builder.Entity<Game>()
                .HasOne(g => g.TeamHost)
                .WithMany(t => t.HomeGames)
                .HasForeignKey(g => g.TeamHostId);

            //builder.Entity<Game>()
            //    .HasOne(g => g.Season)
            //    .WithMany(s => s.Games)
            //    .HasForeignKey(g => g.SeasonId);

            //builder.Entity<Player>()
            //    .HasOne(p => p.Team)
            //    .WithMany(t => t.Players)
            //    .HasForeignKey(p => p.TeamId);

            builder.Entity<Season>()
                .HasOne(s => s.TitleWinner)
                .WithMany(t => t.TitlesWon)
                .HasForeignKey(s => s.TitleWinnerId);

            //builder.Entity<Award>()
            //   .HasOne(a => a.Winner)
            //   .WithMany(p => p.Awards)
            //   .HasForeignKey(a => a.WinnerId);

            builder.Entity<SeasonStatistic>()
                .HasOne(ss => ss.Season)
                .WithMany(s => s.SeasonStatistics)
                .HasForeignKey(ss => ss.SeasonId);

            builder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId);

            ////One-To-One
            builder.Entity<Season>()
               .HasOne(s => s.Playoff)
               .WithOne(p => p.Season)
               .HasForeignKey<Playoff>(p => p.SeasonId);
        }
    }
}
