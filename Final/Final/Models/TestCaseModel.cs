using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Final.Models
{
    public class TestCaseModel
    {
        [Key]
        public int TestCaseID { get; set; }
        public string Title { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Steps { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Comments { get; set; }
        public Nullable<int> StatusD { get; set; }
        public Nullable<int> UATID { get; set; }

        public virtual UATModel UAT { get; set; }

        public virtual StatusModel Status { get; set; }
        //public virtual UATModel UAT { get; set; }


        //public virtual ICollection<StatusModel> Status { get; set; }
        //public virtual ICollection<UATModel> UAT { get; set; }
    }
}