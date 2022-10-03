namespace pickuphockey.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RegularSet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "RegularSetId", c => c.Int(nullable: true));

            CreateTable(
                "dbo.RegularSet",
                c => new
                {
                    RegularSetId = c.Int(nullable: false, identity: true),
                    DayOfWeek = c.Int(nullable: false),
                    CreateDateTime = c.DateTime(nullable: false),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.RegularSetId)
                .Index(t => t.RegularSetId);

            CreateTable(
                "dbo.Regulars",
                c => new
                {
                    RegularSetId = c.Int(nullable: false),
                    UserId = c.String(maxLength: 128),
                    TeamAssignment = c.Int(nullable: false),
                    PositionPreference = c.Int(nullable: false)
                })
                .PrimaryKey(t => new { t.RegularSetId, t.UserId })
                .ForeignKey("dbo.RegularSet", t => t.RegularSetId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Sessions", "RegularSetId", "dbo.RegularSet");
            DropForeignKey("dbo.Regulars", "RegularSetId", "dbo.RegularSet");

            DropColumn("dbo.Sessions", "RegularSetId");

            DropTable("dbo.RegularSet");
            DropTable("dbo.Regulars");
        }
    }
}
