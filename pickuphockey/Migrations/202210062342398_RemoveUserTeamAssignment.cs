namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUserTeamAssignment : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "TeamAssignment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "TeamAssignment", c => c.Int(nullable: false));
        }
    }
}
