namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuySell : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BuySells",
                c => new
                    {
                        BuySellId = c.Int(nullable: false, identity: true),
                        SessionId = c.Int(nullable: false),
                        BuyerUserId = c.String(maxLength: 128),
                        SellerUserId = c.String(maxLength: 128),
                        SellerNote = c.String(),
                        BuyerNote = c.String(),
                        PaymentSent = c.Boolean(nullable: false),
                        PaymentReceived = c.Boolean(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BuySellId)
                .ForeignKey("dbo.AspNetUsers", t => t.BuyerUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.SellerUserId)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.SessionId)
                .Index(t => t.BuyerUserId)
                .Index(t => t.SellerUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BuySells", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.BuySells", "SellerUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BuySells", "BuyerUserId", "dbo.AspNetUsers");
            DropIndex("dbo.BuySells", new[] { "SellerUserId" });
            DropIndex("dbo.BuySells", new[] { "BuyerUserId" });
            DropIndex("dbo.BuySells", new[] { "SessionId" });
            DropTable("dbo.BuySells");
        }
    }
}
