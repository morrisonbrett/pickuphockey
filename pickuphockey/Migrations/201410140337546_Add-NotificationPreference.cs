namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationPreference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NotificationPreference", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NotificationPreference");
        }
    }
}
