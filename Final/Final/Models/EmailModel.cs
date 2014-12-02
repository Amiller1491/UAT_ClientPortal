using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Final.Models
{
    public class EmailModel
    {
        public int MailId { get; set; }

        public string Mail_To { get; set; }

        public string Mail_From { get; set; }

        public string Mail_Subject { get; set; }

        public string Mail_Contents { get; set; }

        public DateTime Mail_Date { get; set; }

        //[ForeignKey("Anganwadi")]
        //public int AnganwadiId { get; set; }
        //public virtual Anganwadi Anganwadi { get; set; }
    }
}
