namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoteFlags : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuySells", "SellerNoteFlagged", c => c.Boolean(nullable: false));
            AddColumn("dbo.BuySells", "BuyerNoteFlagged", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BuySells", "BuyerNoteFlagged");
            DropColumn("dbo.BuySells", "SellerNoteFlagged");
        }
    }
}
