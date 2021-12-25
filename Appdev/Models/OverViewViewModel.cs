using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appdev.Models
{
    public class OverViewViewModel
    {
        public IEnumerable<Assign> TrainerList { get; set; }
        public IEnumerable<Enroll> TraineeList { get; set; }
    }
}