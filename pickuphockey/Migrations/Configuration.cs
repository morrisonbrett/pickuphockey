namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using pickuphockey.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<pickuphockey.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "pickuphockey.Models.ApplicationDbContext";
        }

        protected override void Seed(pickuphockey.Models.ApplicationDbContext context)
        {
            context.Sessions.AddOrUpdate(p => p.SessionDate,
                new Session
                {
                    SessionDate = new DateTime(2014, 8, 29),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                },
                new Session
                {
                    SessionDate = new DateTime(2014, 9, 12),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                },
                new Session
                {
                    SessionDate = new DateTime(2014, 9, 19),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                },
                new Session
                {
                    SessionDate = new DateTime(2014, 9, 24),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                },
                new Session
                {
                    SessionDate = new DateTime(2014, 9, 26),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                },
                new Session
                {
                    SessionDate = new DateTime(2014, 10, 1),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                },
                new Session
                {
                    SessionDate = new DateTime(2014, 10, 3),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                }
                );
        }
    }
}
