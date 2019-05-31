namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Benefit_UserType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.Vehicles", "TransportationLineId", "dbo.TransportationLines");
            DropForeignKey("dbo.Tickets", "TicketTypePricelistId", "dbo.TicketTypePricelists");
            DropForeignKey("dbo.Schedules", "TransportationLineId", "dbo.TransportationLines");
            DropIndex("dbo.AspNetUsers", new[] { "BenefitId" });
            DropIndex("dbo.Tickets", new[] { "TicketTypePricelistId" });
            DropIndex("dbo.Schedules", new[] { "TransportationLineId" });
            DropIndex("dbo.Vehicles", new[] { "TransportationLineId" });
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BenefitUserTypes",
                c => new
                    {
                        Benefit_Id = c.Int(nullable: false),
                        UserType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Benefit_Id, t.UserType_Id })
                .ForeignKey("dbo.Benefits", t => t.Benefit_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserTypes", t => t.UserType_Id, cascadeDelete: true)
                .Index(t => t.Benefit_Id)
                .Index(t => t.UserType_Id);
            
            AddColumn("dbo.Benefits", "Name", c => c.String());
            AddColumn("dbo.Benefits", "CoefficientDiscount", c => c.Double());
            AddColumn("dbo.Benefits", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetUsers", "UserTypeId", c => c.Int());
            AddColumn("dbo.Stations", "Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.Stations", "Latitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Tickets", "TicketTypePricelistId", c => c.Int());
            AlterColumn("dbo.Schedules", "TransportationLineId", c => c.Int());
            CreateIndex("dbo.Schedules", "TransportationLineId");
            CreateIndex("dbo.Tickets", "TicketTypePricelistId");
            CreateIndex("dbo.AspNetUsers", "UserTypeId");
            AddForeignKey("dbo.AspNetUsers", "UserTypeId", "dbo.UserTypes", "Id");
            AddForeignKey("dbo.Tickets", "TicketTypePricelistId", "dbo.TicketTypePricelists", "Id");
            AddForeignKey("dbo.Schedules", "TransportationLineId", "dbo.TransportationLines", "Id");
            DropColumn("dbo.Benefits", "DiscountCoefficient");
            DropColumn("dbo.AspNetUsers", "BenefitId");
            DropTable("dbo.Vehicles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransportationLineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "BenefitId", c => c.Int());
            AddColumn("dbo.Benefits", "DiscountCoefficient", c => c.Double(nullable: false));
            DropForeignKey("dbo.Schedules", "TransportationLineId", "dbo.TransportationLines");
            DropForeignKey("dbo.Tickets", "TicketTypePricelistId", "dbo.TicketTypePricelists");
            DropForeignKey("dbo.BenefitUserTypes", "UserType_Id", "dbo.UserTypes");
            DropForeignKey("dbo.BenefitUserTypes", "Benefit_Id", "dbo.Benefits");
            DropForeignKey("dbo.AspNetUsers", "UserTypeId", "dbo.UserTypes");
            DropIndex("dbo.BenefitUserTypes", new[] { "UserType_Id" });
            DropIndex("dbo.BenefitUserTypes", new[] { "Benefit_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "UserTypeId" });
            DropIndex("dbo.Tickets", new[] { "TicketTypePricelistId" });
            DropIndex("dbo.Schedules", new[] { "TransportationLineId" });
            AlterColumn("dbo.Schedules", "TransportationLineId", c => c.Int(nullable: false));
            AlterColumn("dbo.Tickets", "TicketTypePricelistId", c => c.Int(nullable: false));
            DropColumn("dbo.Stations", "Latitude");
            DropColumn("dbo.Stations", "Longitude");
            DropColumn("dbo.AspNetUsers", "UserTypeId");
            DropColumn("dbo.Benefits", "Discriminator");
            DropColumn("dbo.Benefits", "CoefficientDiscount");
            DropColumn("dbo.Benefits", "Name");
            DropTable("dbo.BenefitUserTypes");
            DropTable("dbo.UserTypes");
            CreateIndex("dbo.Vehicles", "TransportationLineId");
            CreateIndex("dbo.Schedules", "TransportationLineId");
            CreateIndex("dbo.Tickets", "TicketTypePricelistId");
            CreateIndex("dbo.AspNetUsers", "BenefitId");
            AddForeignKey("dbo.Schedules", "TransportationLineId", "dbo.TransportationLines", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tickets", "TicketTypePricelistId", "dbo.TicketTypePricelists", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Vehicles", "TransportationLineId", "dbo.TransportationLines", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "BenefitId", "dbo.Benefits", "Id");
        }
    }
}
