namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuyDayMinimum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "BuyDayMinimum", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "BuyDayMinimum");
        }
    }
}
