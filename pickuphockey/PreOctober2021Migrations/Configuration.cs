using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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

        void AddAdminRole(DbContext context)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            rm.Create(new IdentityRole("Admin"));
        }

        static void AddAdminUser(DbContext context, string email)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = um.FindByEmail(email);

            if (user != null)
                um.AddToRole(user.Id, "Admin");
        }

        protected override void Seed(ApplicationDbContext context)
        {
            AddAdminRole(context);

            var listofAdmins = System.Configuration.ConfigurationManager.AppSettings["ListOfAdmins"];
            if (!string.IsNullOrEmpty(listofAdmins))
            {
                var listofAdminsList = listofAdmins.Split(',');

                foreach (var a in listofAdminsList)
                    AddAdminUser(context, a);
            }

        //    context.Sessions.AddOrUpdate(p => p.SessionDate,
        //        new Session
        //        {
        //            SessionDate = new DateTime(2014, 9, 24),
        //            CreateDateTime = DateTime.UtcNow,
        //            UpdateDateTime = DateTime.UtcNow
        //        },
        //        new Session
        //        {
        //            SessionDate = new DateTime(2014, 9, 26),
        //            CreateDateTime = DateTime.UtcNow,
        //            UpdateDateTime = DateTime.UtcNow
        //        },
        //        new Session
        //        {
        //            SessionDate = new DateTime(2014, 10, 1),
        //            CreateDateTime = DateTime.UtcNow,
        //            UpdateDateTime = DateTime.UtcNow
        //        },
        //        new Session
        //        {
        //            SessionDate = new DateTime(2014, 10, 3),
        //            CreateDateTime = DateTime.UtcNow,
        //            UpdateDateTime = DateTime.UtcNow
        //        },
        //        new Session
        //        {
        //            SessionDate = new DateTime(2014, 10, 8),
        //            CreateDateTime = DateTime.UtcNow,
        //            UpdateDateTime = DateTime.UtcNow
        //        },
        //        new Session
        //        {
        //            SessionDate = new DateTime(2014, 10, 10),
        //            CreateDateTime = DateTime.UtcNow,
        //            UpdateDateTime = DateTime.UtcNow
        //        },
        //        new Session
        //        {
        //            SessionDate = new DateTime(2014, 10, 15),
        //            CreateDateTime = DateTime.UtcNow,
        //            UpdateDateTime = DateTime.UtcNow
        //        },
        //        new Session
        //        {
        //            SessionDate = new DateTime(2014, 10, 17),
        //            CreateDateTime = DateTime.UtcNow,
        //            UpdateDateTime = DateTime.UtcNow
        //        }
        //        );
        }
    }
}
