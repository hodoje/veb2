namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGeneralTransactionModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeneralTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.String(),
                        DateCreated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Status = c.String(),
                        PayerEmail = c.String(),
                        Price = c.String(),
                        Currency = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GeneralTransactions");
        }
    }
}
