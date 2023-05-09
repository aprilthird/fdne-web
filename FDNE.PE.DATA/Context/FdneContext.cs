using FDNE.PE.DATA.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.DATA.Context
{
    public class FdneContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<DisciplineSeason> DisciplineSeasons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<RiderClub> RiderClubs { get; set; }
        public DbSet<Horse> Horses { get; set; }
        public DbSet<HorseClub> HorseClubs { get; set; }
        public DbSet<Binomial> Binomials { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventJudge> EventJudges { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<FederationContact> FederationContact { get; set; }
        public DbSet<FederationInformation> FederationInformation { get; set; }
        public DbSet<FederationOrganization> FederationOrganization { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<FederationGuidelines> FederationGuidelines { get; set; }
        public DbSet<FederationOfficers> FederationOfficers { get; set; }
        public DbSet<Officers> Officers { get; set; }
        public DbSet<FederationFEI> FederationFEIs { get; set; }
        public DbSet<FEIFile> FEIFiles { get; set; }
        public DbSet<FederationHistory> FederationHistories { get; set; }
        public DbSet<HistoryPerYear> HistoryPerYears { get; set; }
        public DbSet<ClubAdministrator> ClubAdministrators { get; set; }
        public DbSet<CoachingSystem> CoachingSystems { get; set; }
        public DbSet<DisciplinePortal> DisciplinePortals { get; set; }

        public FdneContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static FdneContext Create()
        {
            return new FdneContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Ranking>()
                .HasRequired(x => x.DisciplineSeason)
                .WithMany(x => x.Tournaments)
                .HasForeignKey(x => new { x.SeasonId, x.DisciplineId })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>()
                .HasRequired(x => x.DisciplineSeason)
                .WithMany(x => x.Events)
                .HasForeignKey(x => new { x.SeasonId, x.DisciplineId })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Result>()
                .HasRequired(x => x.Binomial)
                .WithMany(x => x.Results)
                .HasForeignKey(x => new { x.RankingId, x.ClubId, x.RiderId, x.HorseId })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Binomial>()
                .HasRequired(x => x.Rider)
                .WithMany(x => x.Binomials)
                .HasForeignKey(x => x.RiderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Binomial>()
                .HasRequired(x => x.HorseClub)
                .WithMany(x => x.Binomials)
                .HasForeignKey(x => new { x.HorseId, x.ClubId })
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Binomial>()
            //    .HasRequired(x => x.HorseClub)
            //    .WithMany(x => x.Binomials)
            //    .HasForeignKey(x => new { x.ClubId, x.HorseId })
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<RiderClub>().HasKey(x => new { x.RiderId, x.ClubId });

            modelBuilder.Entity<RiderClub>()
                .HasRequired(x => x.Rider)
                .WithMany(x => x.RiderClubs)
                .HasForeignKey(x => x.RiderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Binomial>()
                .HasRequired(x => x.RiderClub)
                .WithMany(x => x.Binomials)
                .HasForeignKey(x => new { x.ClubId, x.RiderId })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Binomial>()
                .HasRequired(x => x.HorseClub)
                .WithMany(x => x.Binomials)
                .HasForeignKey(x => new { x.ClubId, x.HorseId })
                .WillCascadeOnDelete(false);

            ////modelBuilder.Entity<Binomial>()
            ////    .HasRequired(x => x.RiderClub)
            ////    .WithMany(x => x.Binomials)
            ////    .HasForeignKey(x => new { x.RiderId, x.ClubId })
            ////    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Result>()
                .HasRequired(x => x.Rider)
                .WithMany(x => x.Results)
                .HasForeignKey(x => x.RiderId)
                .WillCascadeOnDelete(false);
        }
    }
}
