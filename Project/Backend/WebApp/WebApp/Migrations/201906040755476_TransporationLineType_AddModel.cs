namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransporationLineType_AddModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BenefitUserTypes", newName: "UserTypeBenefits");
            DropPrimaryKey("dbo.UserTypeBenefits");
            CreateTable(
                "dbo.TransporationLineTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TransportationLines", "TransporationLineTypeId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.UserTypeBenefits", new[] { "UserType_Id", "Benefit_Id" });
            CreateIndex("dbo.TransportationLines", "TransporationLineTypeId");
            AddForeignKey("dbo.TransportationLines", "TransporationLineTypeId", "dbo.TransporationLineTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.Benefits", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Benefits", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.TransportationLines", "TransporationLineTypeId", "dbo.TransporationLineTypes");
            DropIndex("dbo.TransportationLines", new[] { "TransporationLineTypeId" });
            DropPrimaryKey("dbo.UserTypeBenefits");
            DropColumn("dbo.TransportationLines", "TransporationLineTypeId");
            DropTable("dbo.TransporationLineTypes");
            AddPrimaryKey("dbo.UserTypeBenefits", new[] { "Benefit_Id", "UserType_Id" });
            RenameTable(name: "dbo.UserTypeBenefits", newName: "BenefitUserTypes");
        }
    }
}
