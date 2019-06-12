namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeStampAddedToModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTypes", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.AspNetUsers", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Tickets", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.TicketTypePricelists", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Pricelists", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.TicketTypes", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.DayOfTheWeeks", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Schedules", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.TransportationLines", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.TransportationLineRoutes", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Stations", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.TransportationLineTypes", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportationLineTypes", "Timestamp");
            DropColumn("dbo.Stations", "Timestamp");
            DropColumn("dbo.TransportationLineRoutes", "Timestamp");
            DropColumn("dbo.TransportationLines", "Timestamp");
            DropColumn("dbo.Schedules", "Timestamp");
            DropColumn("dbo.DayOfTheWeeks", "Timestamp");
            DropColumn("dbo.TicketTypes", "Timestamp");
            DropColumn("dbo.Pricelists", "Timestamp");
            DropColumn("dbo.TicketTypePricelists", "Timestamp");
            DropColumn("dbo.Tickets", "Timestamp");
            DropColumn("dbo.AspNetUsers", "Timestamp");
            DropColumn("dbo.UserTypes", "Timestamp");
        }
    }
}
