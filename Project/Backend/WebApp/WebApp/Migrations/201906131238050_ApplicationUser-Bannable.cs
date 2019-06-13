namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserBannable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Banned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Banned");
        }
    }
}
