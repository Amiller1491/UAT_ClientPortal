using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    
    public class UATContext : DbContext


    {
        public DbSet<ProjectModel> Project { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<RoleModel> Role { get; set; }
        public DbSet<StatusModel> Status { get; set; }
        public DbSet<TestCaseModel> TestCase { get; set; }
        public DbSet<UATModel> UAT { get; set; }
        //public DbSet<AccountModel> Account { get; set; }
    }
}