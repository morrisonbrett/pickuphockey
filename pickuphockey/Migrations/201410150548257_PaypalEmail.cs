namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayPalEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PayPalEmail", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PayPalEmail");
        }
    }
}
