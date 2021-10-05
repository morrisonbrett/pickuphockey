namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePaymentPreferenceAndInfo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BuySells", "PaymentPreference");
            DropColumn("dbo.BuySells", "PaymentInfo");
            DropColumn("dbo.AspNetUsers", "PaymentPreference");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "PaymentPreference", c => c.Int(nullable: false));
            AddColumn("dbo.BuySells", "PaymentInfo", c => c.String());
            AddColumn("dbo.BuySells", "PaymentPreference", c => c.Int(nullable: false));
        }
    }
}
