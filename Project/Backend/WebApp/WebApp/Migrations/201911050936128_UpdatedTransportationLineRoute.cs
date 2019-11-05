namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTransportationLineRoute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportationLineRoutes", "SequenceNo", c => c.Int(nullable: false));
            DropColumn("dbo.TransportationLineRoutes", "RoutePoint");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransportationLineRoutes", "RoutePoint", c => c.Int(nullable: false));
            DropColumn("dbo.TransportationLineRoutes", "SequenceNo");
        }
    }
}
