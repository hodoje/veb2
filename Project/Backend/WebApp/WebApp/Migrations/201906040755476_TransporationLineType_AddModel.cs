namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransportationLineType_AddModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BenefitUserTypes", newName: "UserTypeBenefits");
            DropPrimaryKey("dbo.UserTypeBenefits");
            CreateTable(
                "dbo.TransportationLineTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TransportationLines", "TransportationLineTypeId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.UserTypeBenefits", new[] { "UserType_Id", "Benefit_Id" });
            CreateIndex("dbo.TransportationLines", "TransportationLineTypeId");
            AddForeignKey("dbo.TransportationLines", "TransportationLineTypeId", "dbo.TransportationLineTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.Benefits", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Benefits", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.TransportationLines", "TransportationLineTypeId", "dbo.TransportationLineTypes");
            DropIndex("dbo.TransportationLines", new[] { "TransportationLineTypeId" });
            DropPrimaryKey("dbo.UserTypeBenefits");
            DropColumn("dbo.TransportationLines", "TransportationLineTypeId");
            DropTable("dbo.TransportationLineTypes");
            AddPrimaryKey("dbo.UserTypeBenefits", new[] { "Benefit_Id", "UserType_Id" });
            RenameTable(name: "dbo.UserTypeBenefits", newName: "BenefitUserTypes");
        }
    }
}
