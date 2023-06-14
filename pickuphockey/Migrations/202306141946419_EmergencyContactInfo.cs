namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmergencyContactInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmergencyName", c => c.String());
            AddColumn("dbo.AspNetUsers", "EmergencyPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EmergencyPhone");
            DropColumn("dbo.AspNetUsers", "EmergencyName");
        }
    }
}
