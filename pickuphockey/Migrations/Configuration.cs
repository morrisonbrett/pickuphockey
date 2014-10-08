namespace pickuphockey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "pickuphockey.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.Sessions.AddOrUpdate(p => p.SessionDate,
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
                },
                new Session
                {
                    SessionDate = new DateTime(2014, 10, 8),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                },
                new Session
                {
                    SessionDate = new DateTime(2014, 10, 10),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                },
                new Session
                {
                    SessionDate = new DateTime(2014, 10, 15),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                },
                new Session
                {
                    SessionDate = new DateTime(2014, 10, 17),
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow
                }
                );
        }
    }
}
