using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appdev.Models
{
    public class Trainee : User
    {
        public DateTime DateOfBirth { get; set; }
        public string Education { get; set; }
    }
}