using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProjectUber.Models
{
    public class CourseViewModel
    {
        private long _value;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get { return "$" + (_value / 100); } }
        public long SetValue { set { _value = value; } }
        public Dictionary<string,string> Schedule { get; set; }

        public CourseViewModel()
        {
            Schedule = new Dictionary<string, string>();
        }
    }
}