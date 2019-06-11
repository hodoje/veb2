namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Tickets", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Tickets", "ApplicationUserId");
            RenameColumn(table: "dbo.Tickets", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            AlterColumn("dbo.Tickets", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tickets", "ApplicationUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tickets", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Tickets", "ApplicationUserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Tickets", name: "ApplicationUserId", newName: "ApplicationUser_Id");
            AddColumn("dbo.Tickets", "ApplicationUserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "ApplicationUser_Id");
        }
    }
}
