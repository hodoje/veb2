namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPriceList : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pricelists", "ToDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pricelists", "ToDate", c => c.DateTime(nullable: false));
        }
    }
}
