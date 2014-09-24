namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessionNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "Note");
        }
    }
}
