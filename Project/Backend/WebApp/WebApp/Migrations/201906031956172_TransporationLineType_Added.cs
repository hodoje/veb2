namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransporationLineType_Added : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BenefitUserTypes", newName: "UserTypeBenefits");
            DropPrimaryKey("dbo.UserTypeBenefits");
            CreateTable(
                "dbo.TransporationLineTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TransportationLines", "TransporationLineZoneId", c => c.Int(nullable: false));
            AddColumn("dbo.TransportationLines", "TransporationLineZone_Id", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.UserTypeBenefits", new[] { "UserType_Id", "Benefit_Id" });
            CreateIndex("dbo.TransportationLines", "TransporationLineZone_Id");
            AddForeignKey("dbo.TransportationLines", "TransporationLineZone_Id", "dbo.TransporationLineTypes", "Id");
            DropColumn("dbo.Benefits", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Benefits", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.TransportationLines", "TransporationLineZone_Id", "dbo.TransporationLineTypes");
            DropIndex("dbo.TransportationLines", new[] { "TransporationLineZone_Id" });
            DropPrimaryKey("dbo.UserTypeBenefits");
            DropColumn("dbo.TransportationLines", "TransporationLineZone_Id");
            DropColumn("dbo.TransportationLines", "TransporationLineZoneId");
            DropTable("dbo.TransporationLineTypes");
            AddPrimaryKey("dbo.UserTypeBenefits", new[] { "Benefit_Id", "UserType_Id" });
            RenameTable(name: "dbo.UserTypeBenefits", newName: "BenefitUserTypes");
        }
    }
}
