namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Preferred : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Preferred", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Preferred");
        }
    }
}
