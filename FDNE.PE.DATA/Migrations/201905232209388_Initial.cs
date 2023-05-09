namespace FDNE.PE.DATA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Acronym = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        DisciplineId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disciplines", t => t.DisciplineId, cascadeDelete: true)
                .Index(t => t.DisciplineId);
            
            CreateTable(
                "dbo.Disciplines",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        UrlImage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Acronym = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DisciplineSeasons",
                c => new
                    {
                        SeasonId = c.Guid(nullable: false),
                        DisciplineId = c.Guid(nullable: false),
                        Regulation = c.String(),
                    })
                .PrimaryKey(t => new { t.SeasonId, t.DisciplineId })
                .ForeignKey("dbo.Disciplines", t => t.DisciplineId, cascadeDelete: true)
                .ForeignKey("dbo.Seasons", t => t.SeasonId, cascadeDelete: true)
                .Index(t => t.SeasonId)
                .Index(t => t.DisciplineId);
            
            CreateTable(
                "dbo.Seasons",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Year = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        SeasonId = c.Guid(nullable: false),
                        DisciplineId = c.Guid(nullable: false),
                        CategoryId = c.Guid(nullable: false),
                        LevelId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.DisciplineSeasons", t => new { t.SeasonId, t.DisciplineId })
                .ForeignKey("dbo.Levels", t => t.LevelId)
                .Index(t => new { t.SeasonId, t.DisciplineId })
                .Index(t => t.CategoryId)
                .Index(t => t.LevelId);
            
            CreateTable(
                "dbo.Levels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        DisciplineId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disciplines", t => t.DisciplineId, cascadeDelete: true)
                .Index(t => t.DisciplineId);
            
            CreateTable(
                "dbo.FederationInformations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Mision = c.String(nullable: false),
                        Vision = c.String(nullable: false),
                        AboutUs = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Horses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        ClubId = c.Guid(),
                        Sex = c.Byte(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        BelongsToUs = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NumericIdentifier = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Body = c.String(),
                        ImageUrl = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Riders",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClubId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        PaternalSurname = c.String(nullable: false),
                        MaternalSurname = c.String(nullable: false),
                        Dni = c.String(nullable: false, maxLength: 9),
                        Sex = c.Byte(nullable: false),
                        BirthDate = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Riders", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Riders", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.Horses", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.Tournaments", "LevelId", "dbo.Levels");
            DropForeignKey("dbo.Levels", "DisciplineId", "dbo.Disciplines");
            DropForeignKey("dbo.Tournaments", new[] { "SeasonId", "DisciplineId" }, "dbo.DisciplineSeasons");
            DropForeignKey("dbo.Tournaments", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.DisciplineSeasons", "SeasonId", "dbo.Seasons");
            DropForeignKey("dbo.DisciplineSeasons", "DisciplineId", "dbo.Disciplines");
            DropForeignKey("dbo.Categories", "DisciplineId", "dbo.Disciplines");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Riders", new[] { "ClubId" });
            DropIndex("dbo.Riders", new[] { "UserId" });
            DropIndex("dbo.Horses", new[] { "ClubId" });
            DropIndex("dbo.Levels", new[] { "DisciplineId" });
            DropIndex("dbo.Tournaments", new[] { "LevelId" });
            DropIndex("dbo.Tournaments", new[] { "CategoryId" });
            DropIndex("dbo.Tournaments", new[] { "SeasonId", "DisciplineId" });
            DropIndex("dbo.DisciplineSeasons", new[] { "DisciplineId" });
            DropIndex("dbo.DisciplineSeasons", new[] { "SeasonId" });
            DropIndex("dbo.Categories", new[] { "DisciplineId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Riders");
            DropTable("dbo.News");
            DropTable("dbo.Horses");
            DropTable("dbo.FederationInformations");
            DropTable("dbo.Levels");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Seasons");
            DropTable("dbo.DisciplineSeasons");
            DropTable("dbo.Clubs");
            DropTable("dbo.Disciplines");
            DropTable("dbo.Categories");
        }
    }
}
