namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Session_DateRename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "SessionDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Sessions", "SessionDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sessions", "SessionDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Sessions", "SessionDate");
        }
    }
}
