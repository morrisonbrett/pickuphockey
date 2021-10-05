namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuySells", "PaymentInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BuySells", "PaymentInfo");
        }
    }
}
