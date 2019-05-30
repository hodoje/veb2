namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Benefits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DiscountCoefficient = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        TicketTypePricelistId = c.Int(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        TicketType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.TicketTypePricelists", t => t.TicketTypePricelistId, cascadeDelete: true)
                .ForeignKey("dbo.TicketTypes", t => t.TicketType_Id)
                .Index(t => t.TicketTypePricelistId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.TicketType_Id);
            
            CreateTable(
                "dbo.TicketTypePricelists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BasePrice = c.Double(nullable: false),
                        TicketTypeId = c.Int(nullable: false),
                        PricelistId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pricelists", t => t.PricelistId, cascadeDelete: true)
                .ForeignKey("dbo.TicketTypes", t => t.TicketTypeId, cascadeDelete: true)
                .Index(t => t.TicketTypeId)
                .Index(t => t.PricelistId);
            
            CreateTable(
                "dbo.Pricelists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DayOfTheWeeks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOfTheWeekId = c.Int(nullable: false),
                        Timetable = c.String(),
                        TransportationLineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DayOfTheWeeks", t => t.DayOfTheWeekId, cascadeDelete: true)
                .ForeignKey("dbo.TransportationLines", t => t.TransportationLineId, cascadeDelete: true)
                .Index(t => t.DayOfTheWeekId)
                .Index(t => t.TransportationLineId);
            
            CreateTable(
                "dbo.TransportationLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineNum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransportationLineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportationLines", t => t.TransportationLineId, cascadeDelete: true)
                .Index(t => t.TransportationLineId);
            
            CreateTable(
                "dbo.StationTransportationLines",
                c => new
                    {
                        Station_Id = c.Int(nullable: false),
                        TransportationLine_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Station_Id, t.TransportationLine_Id })
                .ForeignKey("dbo.Stations", t => t.Station_Id, cascadeDelete: true)
                .ForeignKey("dbo.TransportationLines", t => t.TransportationLine_Id, cascadeDelete: true)
                .Index(t => t.Station_Id)
                .Index(t => t.TransportationLine_Id);
            
            AddColumn("dbo.AspNetUsers", "BenefitId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "BenefitId");
            AddForeignKey("dbo.AspNetUsers", "BenefitId", "dbo.Benefits", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "TransportationLineId", "dbo.TransportationLines");
            DropForeignKey("dbo.StationTransportationLines", "TransportationLine_Id", "dbo.TransportationLines");
            DropForeignKey("dbo.StationTransportationLines", "Station_Id", "dbo.Stations");
            DropForeignKey("dbo.Schedules", "TransportationLineId", "dbo.TransportationLines");
            DropForeignKey("dbo.Schedules", "DayOfTheWeekId", "dbo.DayOfTheWeeks");
            DropForeignKey("dbo.TicketTypePricelists", "TicketTypeId", "dbo.TicketTypes");
            DropForeignKey("dbo.Tickets", "TicketType_Id", "dbo.TicketTypes");
            DropForeignKey("dbo.Tickets", "TicketTypePricelistId", "dbo.TicketTypePricelists");
            DropForeignKey("dbo.TicketTypePricelists", "PricelistId", "dbo.Pricelists");
            DropForeignKey("dbo.Tickets", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "BenefitId", "dbo.Benefits");
            DropIndex("dbo.StationTransportationLines", new[] { "TransportationLine_Id" });
            DropIndex("dbo.StationTransportationLines", new[] { "Station_Id" });
            DropIndex("dbo.Vehicles", new[] { "TransportationLineId" });
            DropIndex("dbo.Schedules", new[] { "TransportationLineId" });
            DropIndex("dbo.Schedules", new[] { "DayOfTheWeekId" });
            DropIndex("dbo.TicketTypePricelists", new[] { "PricelistId" });
            DropIndex("dbo.TicketTypePricelists", new[] { "TicketTypeId" });
            DropIndex("dbo.Tickets", new[] { "TicketType_Id" });
            DropIndex("dbo.Tickets", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Tickets", new[] { "TicketTypePricelistId" });
            DropIndex("dbo.AspNetUsers", new[] { "BenefitId" });
            DropColumn("dbo.AspNetUsers", "BenefitId");
            DropTable("dbo.StationTransportationLines");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Stations");
            DropTable("dbo.TransportationLines");
            DropTable("dbo.Schedules");
            DropTable("dbo.DayOfTheWeeks");
            DropTable("dbo.TicketTypes");
            DropTable("dbo.Pricelists");
            DropTable("dbo.TicketTypePricelists");
            DropTable("dbo.Tickets");
            DropTable("dbo.Benefits");
        }
    }
}
