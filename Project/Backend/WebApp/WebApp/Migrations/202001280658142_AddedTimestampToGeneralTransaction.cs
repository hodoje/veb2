namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimestampToGeneralTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneralTransactions", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneralTransactions", "Timestamp");
        }
    }
}
