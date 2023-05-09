namespace FDNE.PE.DATA.Migrations
{
    using FDNE.PE.DATA.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    
    internal sealed class Configuration : DbMigrationsConfiguration<FDNE.PE.DATA.Context.FdneContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FDNE.PE.DATA.Context.FdneContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.DocumentTypes.Any())
            {
                context.DocumentTypes.AddOrUpdate(new DocumentType[]
                {
                    new DocumentType() { Name = "Documento Nacional de Identidad o Libreta Electoral", Acronym = "DNI / L.E.", Length = 8, ExactLength = true, IsNumeric = true },
                    new DocumentType() { Name = "Carnet de Extranjería", Acronym = "CARNET EXT.", Length = 12, ExactLength = false, IsNumeric = false },
                    new DocumentType() { Name = "Registro Único de Contribuyentes", Acronym = "RUC", Length = 11, ExactLength = true, IsNumeric = true },
                    new DocumentType() { Name = "Pasaporte", Acronym = "PASAPORTE", Length = 12, ExactLength = false, IsNumeric = false },
                    new DocumentType() { Name = "Partida de Nacimiento-Identidad", Acronym = "P. NAC.", Length = 15, ExactLength = false, IsNumeric = false },
                    new DocumentType() { Name = "Otros", Acronym = "OTROS", Length = 15, ExactLength = false, IsNumeric = false }
                });
                context.SaveChanges();
            }
            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                roleManager.Create(new IdentityRole("Admin"));
                roleManager.Create(new IdentityRole("Jinete"));
                roleManager.Create(new IdentityRole("Jurado"));
            }

            if (!context.Users.Any())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var documentType = context.DocumentTypes.FirstOrDefault();
                var admin = new ApplicationUser() { UserName = "admin", Email = "admin@fdne.com", Name = "admin", PaternalSurname = "admin", MaternalSurname = "admin", DocumentTypeId = documentType.Id, Document = "99988877", PhoneNumber = "87127312" };
                var rider = new ApplicationUser() { UserName = "jinete", Email = "jinete@fdne.com", Name = "Luis", PaternalSurname = "Lopez", MaternalSurname = "Almandroz", DocumentTypeId = documentType.Id, Document = "99988855", PhoneNumber = "87127413" };
                var judge = new ApplicationUser() { UserName = "jurado", Email = "jurado@fdne.com", Name = "Erick", PaternalSurname = "Carbajal", MaternalSurname = "Velasquez", DocumentTypeId = documentType.Id, Document = "99988866", PhoneNumber = "87127514" };
                var result1 = userManager.Create(admin, "Admin.123");
                var result2 = userManager.Create(rider, "Jinete.123");
                var result3 = userManager.Create(judge, "Jurado.123");
            }

            if (context.Users.Any(x => !x.Roles.Any()))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var admin = userManager.FindByEmail("admin@fdne.com");
                if (!userManager.GetRoles(admin.Id).Any())
                    userManager.AddToRole(admin.Id, "Admin");
                var rider = userManager.FindByEmail("jinete@fdne.com");
                if (!userManager.GetRoles(rider.Id).Any())
                    userManager.AddToRole(rider.Id, "Jinete");
                var judge = userManager.FindByEmail("jurado@fdne.com");
                if (!userManager.GetRoles(judge.Id).Any())
                    userManager.AddToRole(judge.Id, "Jurado");
            }
            if (!context.Disciplines.Any())
            {
                context.Disciplines.AddOrUpdate(new Discipline[] {
                    new Discipline() { Name = "Adiestramiento", Description = "Disciplina de Adistramiento", UrlImage = "" },
                    new Discipline() { Name = "Enduro", Description = "Disciplina de Enduro", UrlImage = "" },
                    new Discipline() { Name = "Prueba Completa", Description = "Disciplina de Prueba Completa", UrlImage = "" },
                    new Discipline() { Name = "Salto", Description = "Disciplina de Salto", UrlImage = "" }
                });
                context.SaveChanges();
            }
            if (!context.Seasons.Any())
            {
                context.Seasons.AddOrUpdate(new Season[] {
                    new Season() { Year = 2019, StartDate = DateTime.Parse("2019-01-10"), EndDate = DateTime.Parse("2019-12-20") }
                });
                context.SaveChanges();
            }
            if (!context.DisciplineSeasons.Any())
            {
                var season = context.Seasons.FirstOrDefault();
                var disciplines = context.Disciplines.ToList();
                context.DisciplineSeasons.AddOrUpdate(new DisciplineSeason[]
                {
                    new DisciplineSeason() { SeasonId = season.Id, DisciplineId = disciplines.First(x => x.Name == "Adiestramiento").Id },
                    new DisciplineSeason() { SeasonId = season.Id, DisciplineId = disciplines.First(x => x.Name == "Enduro").Id },
                    new DisciplineSeason() { SeasonId = season.Id, DisciplineId = disciplines.First(x => x.Name == "Prueba Completa").Id },
                    new DisciplineSeason() { SeasonId = season.Id, DisciplineId = disciplines.First(x => x.Name == "Salto").Id }
                });
                context.SaveChanges();
            }
            if (!context.Categories.Any())
            {
                var disciplines = context.Disciplines.ToList();
                context.Categories.AddOrUpdate(new Category[]
                {
                    new Category() { DisciplineId = disciplines.First(x => x.Name == "Adiestramiento" ).Id, Name = "Adulto", Acronym = "ADL" }
                });
                context.SaveChanges();
            }
            if (!context.Clubs.Any())
            {
                context.Clubs.AddOrUpdate(new Club[]
                {
                   new Club() { Id = Guid.NewGuid(), Name = "Club Hípico", Acronym = "CHH", Address = "Av. Javier Prado Este 3542, San Borja 15037, Perú", Email = "clubhipico@email.com", IsActive = true, PhoneNumber = "981821329", Latitude = -12.086164208278365, Longitude = -76.987954038048144 }
                });
                context.SaveChanges();
            }
            if (!context.Riders.Any())
            {
                var user = context.Users.FirstOrDefault(x => x.UserName == "jinete");
                if (user == null)
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var documentType = context.DocumentTypes.FirstOrDefault();
                    user = new ApplicationUser() { UserName = "jinete", Email = "jinete@fdne.com", Name = "jinete", PaternalSurname = "jinete", MaternalSurname = "jinete", DocumentTypeId = documentType.Id, Document = "99988855", PhoneNumber = "87127413" };
                    userManager.Create(user, "Jinete.123");
                    userManager.AddToRole(user.Id, "Jinete");
                }
                var clubs = context.Clubs.ToList();
                context.Riders.AddOrUpdate(new Rider[]
                {
                   new Rider() { UserId = user.Id, User = user, IsActive = true}
                });
                context.SaveChanges();
            }
            if (!context.RiderClubs.Any())
            {
                var rider = context.Riders.FirstOrDefault();
                var club = context.Clubs.FirstOrDefault();
                context.RiderClubs.AddOrUpdate(new RiderClub
                {
                    RiderId = rider.UserId,
                    ClubId = club.Id
                });
                context.SaveChanges();
            }
            if (!context.Horses.Any())
            {
                context.Horses.AddOrUpdate(new Horse[] {
                    new Horse()
                    {
                        BelongsToUs = false,
                        IsActive = true,
                        Name = "Sugar",
                        Sex = 0
                    }
                });
                context.SaveChanges();
            }
            if (!context.HorseClubs.Any())
            {
                var horse = context.Horses.FirstOrDefault();
                var club = context.Clubs.FirstOrDefault();
                context.HorseClubs.AddOrUpdate(new HorseClub
                {
                    HorseId = horse.Id,
                    ClubId = club.Id
                });
                context.SaveChanges();
            }
        }

    }
}
