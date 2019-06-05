namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransportationLineTypeNameFix : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TransporationLineTypes", newName: "TransportationLineTypes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TransportationLineTypes", newName: "TransporationLineTypes");
        }
    }
}
