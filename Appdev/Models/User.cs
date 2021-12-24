using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appdev.Models
{
    public class User : ApplicationUser
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}