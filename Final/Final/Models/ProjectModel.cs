using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class ProjectModel
    {
        [Key]
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }

        public virtual UserModel User { get; set; }

        //public virtual ICollection<UserModel> User { get; set; }
        public virtual ICollection<UATModel> UAT { get; set; }

    }
}