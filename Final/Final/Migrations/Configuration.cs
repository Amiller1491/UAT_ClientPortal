namespace Final.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Final.Models;
    using System.Collections.Generic;
    using System.Web.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<Final.Models.UATContext>
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = true;
            //AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Final.Models.UATContext context)
        {
            //  This method will be called after migrating to the latest version.



            context.Role.AddOrUpdate(r => r.Name,
                    new RoleModel { Name = "Admin" },
                    new RoleModel { Name = "Client" }
                );

            context.Status.AddOrUpdate(r => r.StatusType,
                new StatusModel { StatusType = "Sent" },
                new StatusModel { StatusType = "Viewed" },
                new StatusModel { StatusType = "Approved" },
                new StatusModel { StatusType = "Rejected" },
                new StatusModel { StatusType = "In Progress" }

            );
            //
            context.TestCase.AddOrUpdate(r => r.Title,
                new TestCaseModel
                {
                    Title = "CornerstoneTestCase",
                    Steps = "Step 1: Habba babba",
                    Comments = "I could only get past step 1",
                    //StatusD = 4,
                    //UATID = 1
                }

            );
            //
            //
            context.UAT.AddOrUpdate(r => r.Name,

               new UATModel
               {
                   Name = "CornerstoneUAT",
                   Version = "1.1",
                   Started = DateTime.Parse("4-1-2013"),
                   LastModified = DateTime.Parse("4-3-2013"),
                   //StatusD = 2,
                   //TestCaseID = 1,
               }

            );
            //
            context.Project.AddOrUpdate(r => r.Name,
               new ProjectModel
               {
                   Name = "Cornerstone",
                   //UATID = 1,
               }

            );
            //
            context.User.AddOrUpdate(r => r.FirstName,

                new UserModel
                {
                    FirstName = "Lance",
                    LastName = "Ugalde",
                    Email = "lanceugalde@gmail.com",
                    Phone = "985-210-5234",
                    Password = Crypto.HashPassword("youtube"),
                    //ProjectID = 1,
                    //RoleID = 1,
                }

            );






            //var projects = new List<ProjectModel>
            //{
            //    new ProjectModel { Name = "Cornerstone"},
            //   //new ProjectModel { Name = "BergenStock"},
            //   //new ProjectModel { Name = "Polydrip"},
            //   //new ProjectModel { Name = "MCS"},
            //    //new ProjectModel { Name = "Olaf Solutions", UATID = 4 }
            //};
            //projects.ForEach(s => context.Project.Add(s));
            //context.SaveChanges();



        }
    }
}
