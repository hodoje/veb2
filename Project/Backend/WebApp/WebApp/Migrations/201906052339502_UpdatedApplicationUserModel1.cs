namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedApplicationUserModel1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsSuccessfullyRegistered", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "DocumentImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DocumentImage");
            DropColumn("dbo.AspNetUsers", "IsSuccessfullyRegistered");
        }
    }
}
