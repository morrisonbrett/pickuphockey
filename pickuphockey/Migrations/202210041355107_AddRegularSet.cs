namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegularSet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegularSets",
                c => new
                    {
                        RegularSetId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        DayOfWeek = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RegularSetId);
            
            CreateTable(
                "dbo.Regulars",
                c => new
                    {
                        RegularSetId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        TeamAssignment = c.Int(nullable: false),
                        PositionPreference = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RegularSetId, t.UserId });
            
            AddColumn("dbo.Sessions", "RegularSetId", c => c.Int());
            CreateIndex("dbo.Sessions", "RegularSetId");
            AddForeignKey("dbo.Sessions", "RegularSetId", "dbo.RegularSets", "RegularSetId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sessions", "RegularSetId", "dbo.RegularSets");
            DropIndex("dbo.Sessions", new[] { "RegularSetId" });
            DropColumn("dbo.Sessions", "RegularSetId");
            DropTable("dbo.Regulars");
            DropTable("dbo.RegularSets");
        }
    }
}
