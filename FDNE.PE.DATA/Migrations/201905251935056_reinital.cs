namespace FDNE.PE.DATA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reinital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Binomials",
                c => new
                    {
                        TournamentId = c.Guid(nullable: false),
                        RiderId = c.String(nullable: false, maxLength: 128),
                        HorseId = c.Guid(nullable: false),
                        Rider_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TournamentId, t.RiderId, t.HorseId })
                .ForeignKey("dbo.Horses", t => t.HorseId, cascadeDelete: true)
                .ForeignKey("dbo.Riders", t => t.Rider_UserId)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.TournamentId)
                .Index(t => t.HorseId)
                .Index(t => t.Rider_UserId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SeasonId = c.Guid(nullable: false),
                        DisciplineId = c.Guid(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        ClubId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Acronym = c.String(nullable: false),
                        HasTwoDates = c.Boolean(nullable: false),
                        DoublesScore = c.Boolean(nullable: false),
                        Tournament_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clubs", t => t.ClubId, cascadeDelete: true)
                .ForeignKey("dbo.DisciplineSeasons", t => new { t.SeasonId, t.DisciplineId })
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id)
                .Index(t => new { t.SeasonId, t.DisciplineId })
                .Index(t => t.ClubId)
                .Index(t => t.Tournament_Id);
            
            CreateTable(
                "dbo.ContestJudges",
                c => new
                    {
                        ContestId = c.Guid(nullable: false),
                        JudgeId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ContestId, t.JudgeId })
                .ForeignKey("dbo.Contests", t => t.ContestId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.JudgeId, cascadeDelete: true)
                .Index(t => t.ContestId)
                .Index(t => t.JudgeId);
            
            CreateTable(
                "dbo.Contests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EventId = c.Guid(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        TournamentId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.TournamentId);
            
            AddColumn("dbo.Seasons", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.FederationOrganizations", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.FederationOrganizations", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContestJudges", "JudgeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Contests", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Contests", "EventId", "dbo.Events");
            DropForeignKey("dbo.ContestJudges", "ContestId", "dbo.Contests");
            DropForeignKey("dbo.Events", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Events", new[] { "SeasonId", "DisciplineId" }, "dbo.DisciplineSeasons");
            DropForeignKey("dbo.Events", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.Binomials", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Binomials", "Rider_UserId", "dbo.Riders");
            DropForeignKey("dbo.Binomials", "HorseId", "dbo.Horses");
            DropIndex("dbo.Contests", new[] { "TournamentId" });
            DropIndex("dbo.Contests", new[] { "EventId" });
            DropIndex("dbo.ContestJudges", new[] { "JudgeId" });
            DropIndex("dbo.ContestJudges", new[] { "ContestId" });
            DropIndex("dbo.Events", new[] { "Tournament_Id" });
            DropIndex("dbo.Events", new[] { "ClubId" });
            DropIndex("dbo.Events", new[] { "SeasonId", "DisciplineId" });
            DropIndex("dbo.Binomials", new[] { "Rider_UserId" });
            DropIndex("dbo.Binomials", new[] { "HorseId" });
            DropIndex("dbo.Binomials", new[] { "TournamentId" });
            AlterColumn("dbo.FederationOrganizations", "Name", c => c.String());
            AlterColumn("dbo.FederationOrganizations", "Title", c => c.String());
            DropColumn("dbo.AspNetUsers", "IsActive");
            DropColumn("dbo.Seasons", "IsActive");
            DropTable("dbo.Contests");
            DropTable("dbo.ContestJudges");
            DropTable("dbo.Events");
            DropTable("dbo.Binomials");
        }
    }
}
