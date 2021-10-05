namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePlayerJersey : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "PlayerJersey");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "PlayerJersey", c => c.String());
        }
    }
}
