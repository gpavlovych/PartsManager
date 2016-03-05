using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PartManagementWebApp.Models;

namespace PartManagementWebApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PartManagementWebApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PartManagementWebApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            if (!(context.Users.Any(u => u.UserName == "pavlheo@gmail.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser {UserName = "pavlheo@gmail.com", PhoneNumber = "0797697898"};
                userManager.Create(userToInsert, "abc123");

                context.Parts.AddOrUpdate(
                    new Part {Id = 1, PartNumber = "LM317", Owner = userToInsert },
                    new Part {Id = 2, PartNumber = "AT90USB162", Owner = userToInsert },
                    new Part {Id = 3, PartNumber = "AD8551", Owner = userToInsert });
            }
        }
    }
}
