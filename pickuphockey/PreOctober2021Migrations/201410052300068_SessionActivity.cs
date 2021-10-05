namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessionActivity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        ActivityLogId = c.Int(nullable: false, identity: true),
                        SessionId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CreateDateTime = c.DateTime(nullable: false),
                        Activity = c.String(),
                    })
                .PrimaryKey(t => t.ActivityLogId)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.SessionId)
                .Index(t => t.UserId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityLogs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ActivityLogs", "SessionId", "dbo.Sessions");
            DropIndex("dbo.ActivityLogs", new[] { "UserId" });
            DropIndex("dbo.ActivityLogs", new[] { "SessionId" });
            DropTable("dbo.ActivityLogs");
        }
    }
}
