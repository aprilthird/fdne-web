namespace FDNE.PE.DATA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FederationOrganizations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FederationOrganizations");
        }
    }
}
