namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketType : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BenefitUserTypes", newName: "UserTypeBenefits");
            DropPrimaryKey("dbo.UserTypeBenefits");
            AddColumn("dbo.TicketTypes", "CardDuration", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.UserTypeBenefits", new[] { "UserType_Id", "Benefit_Id" });
            DropColumn("dbo.Benefits", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Benefits", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.UserTypeBenefits");
            DropColumn("dbo.TicketTypes", "CardDuration");
            AddPrimaryKey("dbo.UserTypeBenefits", new[] { "Benefit_Id", "UserType_Id" });
            RenameTable(name: "dbo.UserTypeBenefits", newName: "BenefitUserTypes");
        }
    }
}
