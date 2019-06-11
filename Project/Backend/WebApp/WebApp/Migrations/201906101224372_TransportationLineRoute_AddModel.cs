namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransportationLineRoute_AddModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StationTransportationLines", "Station_Id", "dbo.Stations");
            DropForeignKey("dbo.StationTransportationLines", "TransportationLine_Id", "dbo.TransportationLines");
            DropIndex("dbo.StationTransportationLines", new[] { "Station_Id" });
            DropIndex("dbo.StationTransportationLines", new[] { "TransportationLine_Id" });
            CreateTable(
                "dbo.TransportationLineRoutes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransporationLineId = c.Int(nullable: false),
                        StationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stations", t => t.StationId, cascadeDelete: true)
                .ForeignKey("dbo.TransportationLines", t => t.TransporationLineId, cascadeDelete: true)
                .Index(t => t.TransporationLineId)
                .Index(t => t.StationId);
            
            DropTable("dbo.StationTransportationLines");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StationTransportationLines",
                c => new
                    {
                        Station_Id = c.Int(nullable: false),
                        TransportationLine_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Station_Id, t.TransportationLine_Id });
            
            DropForeignKey("dbo.TransportationLineRoutes", "TransporationLineId", "dbo.TransportationLines");
            DropForeignKey("dbo.TransportationLineRoutes", "StationId", "dbo.Stations");
            DropIndex("dbo.TransportationLineRoutes", new[] { "StationId" });
            DropIndex("dbo.TransportationLineRoutes", new[] { "TransporationLineId" });
            DropTable("dbo.TransportationLineRoutes");
            CreateIndex("dbo.StationTransportationLines", "TransportationLine_Id");
            CreateIndex("dbo.StationTransportationLines", "Station_Id");
            AddForeignKey("dbo.StationTransportationLines", "TransportationLine_Id", "dbo.TransportationLines", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StationTransportationLines", "Station_Id", "dbo.Stations", "Id", cascadeDelete: true);
        }
    }
}
