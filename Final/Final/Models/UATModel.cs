using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Final.Models
{
    public class UATModel
    {
        [Key]
        public int UATID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Intro { get; set; }
        public System.DateTime Started { get; set; }
        public System.DateTime LastModified { get; set; }
        public int ProjectID { get; set; }
        public Nullable<int> StatusD { get; set; }
        //public Nullable<int> TestCaseID { get; set; }

        public virtual ProjectModel Project { get; set; }
        public virtual StatusModel Status { get; set; }  
        //public virtual TestCaseModel TestCase { get; set; }

       //public virtual ICollection<StatusModel> Status { get; set; }
       public virtual ICollection<TestCaseModel> TestCase { get; set; }

    }
}