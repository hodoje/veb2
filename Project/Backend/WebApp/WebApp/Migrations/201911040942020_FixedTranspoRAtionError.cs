namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedTranspoRAtionError : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TransportationLineRoutes", name: "TransporationLineId", newName: "TransportationLineId");
            RenameIndex(table: "dbo.TransportationLineRoutes", name: "IX_TransporationLineId", newName: "IX_TransportationLineId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TransportationLineRoutes", name: "IX_TransportationLineId", newName: "IX_TransporationLineId");
            RenameColumn(table: "dbo.TransportationLineRoutes", name: "TransportationLineId", newName: "TransporationLineId");
        }
    }
}
