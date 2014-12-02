using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public class StatusModel
    {
        [Key]
        public int StatusD { get; set; }
        public string StatusType { get; set; }
             

        public virtual ICollection<TestCaseModel> TestCase { get; set; }
        public virtual ICollection<UATModel> UAT { get; set; }
    }
}
