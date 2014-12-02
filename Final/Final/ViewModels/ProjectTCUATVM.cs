using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Final.Models;

namespace Final.ViewModels
{
    public class ProjectTCUATVM
    {
        public List<ProjectModel> Project { get; set; }
        public List<UATModel> UAT{ get; set; }
        public List<TestCaseModel> TestCase { get; set; }
        public List<UserModel> User { get; set; }

    }
}