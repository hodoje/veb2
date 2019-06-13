namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserProfileProgressionUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RegistrationStatus", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "IsSuccessfullyRegistered");
            DropColumn("dbo.AspNetUsers", "ProfileInProcessing");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ProfileInProcessing", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsSuccessfullyRegistered", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "RegistrationStatus");
        }
    }
}
