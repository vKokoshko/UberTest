using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProjectUber.DbLayer;

namespace TestProjectUber.Models
{
    public class HomeIndexViewModel
    {
        public List<CourseViewModel> Courses { get; set; }
        
        public string CalendarEntriesJson { get; set; }

        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please fill the course name")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Please set the course value")]
        [Range(1, int.MaxValue, ErrorMessage = "Course value can't be 0 or negative")]
        public int CourseValue { get; set; }

        [Required(ErrorMessage = "Please choose at least one time at schedule")]
        [StringLength(2000, MinimumLength = 3, ErrorMessage = "Please choose at least one time at schedule")]
        public string SelectedTimesJson { get; set; }

        public HomeIndexViewModel()
        {
            Courses = new List<CourseViewModel>();
        }
    }
}