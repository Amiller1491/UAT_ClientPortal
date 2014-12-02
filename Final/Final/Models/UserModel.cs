using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
        //[Required] gave problem in editing :RABIN
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public Nullable<int> RoleID { get; set; }

        public virtual RoleModel Role { get; set; }

        public virtual ICollection<ProjectModel> Project { get; set; }
        //public virtual ICollection<RoleModel> Role { get; set; }
    }
}