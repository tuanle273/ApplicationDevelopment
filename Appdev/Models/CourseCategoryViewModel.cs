using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appdev.Models
{
    public class CourseCategoryViewModel
    {
        public Course Course { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}