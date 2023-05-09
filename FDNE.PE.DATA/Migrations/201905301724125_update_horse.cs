namespace FDNE.PE.DATA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_horse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Horses", "UrlToImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Horses", "UrlToImage");
        }
    }
}
