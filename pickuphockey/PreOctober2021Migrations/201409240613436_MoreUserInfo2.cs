namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreUserInfo2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TeamAssignment", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TeamAssignment");
        }
    }
}
