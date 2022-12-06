namespace pickuphockey.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            return;
            
#pragma warning disable CS0162 // Unreachable code detected
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
#pragma warning restore CS0162 // Unreachable code detected
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        SessionId = c.Int(nullable: false, identity: true),
                        SessionDate = c.DateTime(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.SessionId);
            
            CreateTable(
                "dbo.BuySells",
                c => new
                    {
                        BuySellId = c.Int(nullable: false, identity: true),
                        SessionId = c.Int(nullable: false),
                        BuyerUserId = c.String(maxLength: 128),
                        SellerUserId = c.String(maxLength: 128),
                        SellerNote = c.String(),
                        TeamAssignment = c.Int(nullable: false),
                        BuyerNote = c.String(),
                        PaymentSent = c.Boolean(nullable: false),
                        PaymentReceived = c.Boolean(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BuySellId)
                .ForeignKey("dbo.AspNetUsers", t => t.BuyerUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.SellerUserId)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.SessionId)
                .Index(t => t.BuyerUserId)
                .Index(t => t.SellerUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        TeamAssignment = c.Int(nullable: false),
                        NotificationPreference = c.Int(nullable: false),
                        PayPalEmail = c.String(),
                        Active = c.Boolean(nullable: false),
                        Preferred = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            return;

#pragma warning disable CS0162 // Unreachable code detected
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
#pragma warning restore CS0162 // Unreachable code detected
            DropForeignKey("dbo.ActivityLogs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BuySells", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.BuySells", "SellerUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BuySells", "BuyerUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ActivityLogs", "SessionId", "dbo.Sessions");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.BuySells", new[] { "SellerUserId" });
            DropIndex("dbo.BuySells", new[] { "BuyerUserId" });
            DropIndex("dbo.BuySells", new[] { "SessionId" });
            DropIndex("dbo.ActivityLogs", new[] { "UserId" });
            DropIndex("dbo.ActivityLogs", new[] { "SessionId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.BuySells");
            DropTable("dbo.Sessions");
            DropTable("dbo.ActivityLogs");
        }
    }
}
