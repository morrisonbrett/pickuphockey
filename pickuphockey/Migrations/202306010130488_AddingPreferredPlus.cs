namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPreferredPlus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PreferredPlus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PreferredPlus");
        }
    }
}
