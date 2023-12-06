namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LockerRoom13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LockerRoom13", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LockerRoom13");
        }
    }
}
