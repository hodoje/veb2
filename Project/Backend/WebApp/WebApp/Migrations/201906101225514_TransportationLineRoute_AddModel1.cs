namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransportationLineRoute_AddModel1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportationLineRoutes", "RoutePoint", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportationLineRoutes", "RoutePoint");
        }
    }
}
