namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMobileLast4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MobileLast4", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MobileLast4");
        }
    }
}
