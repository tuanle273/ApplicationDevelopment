using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appdev.Models
{
    public class Enroll
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [Required]
        public string TraineeId { get; set; }
        [ForeignKey("TraineeId")]
        public Trainee Trainee { get; set; }
    }
}