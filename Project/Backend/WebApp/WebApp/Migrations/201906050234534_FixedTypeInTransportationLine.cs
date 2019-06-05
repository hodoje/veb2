namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedTypeInTransportationLine : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TransportationLines", name: "TransporationLineTypeId", newName: "TransportationLineTypeId");
            RenameIndex(table: "dbo.TransportationLines", name: "IX_TransporationLineTypeId", newName: "IX_TransportationLineTypeId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TransportationLines", name: "IX_TransportationLineTypeId", newName: "IX_TransporationLineTypeId");
            RenameColumn(table: "dbo.TransportationLines", name: "TransportationLineTypeId", newName: "TransporationLineTypeId");
        }
    }
}
