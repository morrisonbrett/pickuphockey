namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuySell2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuySells", "PaymentPreference", c => c.Int(nullable: false));
            AddColumn("dbo.BuySells", "TeamAssignment", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BuySells", "TeamAssignment");
            DropColumn("dbo.BuySells", "PaymentPreference");
        }
    }
}
