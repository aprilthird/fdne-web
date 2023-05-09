namespace FDNE.PE.DATA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateGallery : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ImageUrl = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Galleries");
        }
    }
}
