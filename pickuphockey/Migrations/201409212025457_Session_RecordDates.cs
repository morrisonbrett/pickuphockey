namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Session_RecordDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "CreateDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "UpdateDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "UpdateDateTime");
            DropColumn("dbo.Sessions", "CreateDateTime");
        }
    }
}
