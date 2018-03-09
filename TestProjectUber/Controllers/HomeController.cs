using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProjectUber.DbLayer;
using TestProjectUber.Models;
using TestProjectUber.Services;

namespace TestProjectUber.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICoursesContext _context;

        public HomeController(ICoursesContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            foreach (var key in TempData.Keys)
                foreach (var e in TempData[key] as ModelErrorCollection)
                    ModelState.AddModelError(key, e.ErrorMessage);

            if (!ModelState.IsValid)
                ViewBag.ValidationTitle = "Validation on server failed:";

            var model = new HomeIndexViewModel();
            model.Courses = GetCoursesViewModelList();
            model.CalendarEntriesJson = GetCalendarEntriesViewModelList();
            SetCalendarStructureData();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdateCourse(HomeIndexViewModel model)
        {
            if(model.CourseId == 0)
            {
                var result = _context.Courses.Where(c => c.Name == model.CourseName).FirstOrDefault();
                if (result != null)
                {
                    ModelState.AddModelError("CourseName", "Course with this name already exists");
                }
            }
            if (ModelState.IsValid)
            {
                if (model.CourseId == 0)
                    CreateCourse(model);
                else
                    UpdateCourse(model);
            }
            else
            {
                TempData["CourseId"] = ModelState["CourseId"].Errors;
                TempData["CourseName"] = ModelState["CourseName"].Errors;
                TempData["CourseValue"] = ModelState["CourseValue"].Errors;
                TempData["SelectedTimesJson"] = ModelState["SelectedTimesJson"].Errors;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteCourse(int courseId)
        {
            var course = _context.Courses.Where(c => c.Id == courseId).FirstOrDefault();

            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
            else
                ModelState.AddModelError("CourseId", "This course wasn't found in database");

            return RedirectToAction("Index");
        }
        
        private List<CourseViewModel> GetCoursesViewModelList()
        {
            var list = new List<CourseViewModel>();

            var courses = _context.Courses.Include(c => c.CalendarEntries.Select(ce => ce.Hour)).ToList();

            foreach (var course in courses)
            {
                var newCourse = new CourseViewModel()
                {
                    Id = course.Id,
                    Name = course.Name,
                    SetValue = course.Value
                };

                foreach (var day in Enum.GetNames(typeof(Enumerations.Days)))
                {
                    var entries = course.CalendarEntries.Where(ce => ce.Day.ToString() == day);
                    if (entries.FirstOrDefault() != null)
                    {
                        string times = string.Empty;
                        foreach (var e in entries)
                        {
                            times += e.Hour.Value + ", ";
                        }
                        times = times.Substring(0, times.Length - 2);
                        newCourse.Schedule.Add(day, times);
                    }
                }
                list.Add(newCourse);
            }

            return list;
        }

        private string GetCalendarEntriesViewModelList()
        {
            var list = new List<CalendarEntryViewModel>();

            var entries = _context.CalendarEntries.Include(ce => ce.Course).Include(ce => ce.Hour).ToList();

            foreach (var entry in entries)
            {
                list.Add(new CalendarEntryViewModel()
                {
                    Day = GetShortDay(entry.Day),
                    Hour = GetShortHour(entry.Hour.Value),
                    Course = entry.Course.Name
                });
            }

            return JsonConvert.SerializeObject(list);
        }

        private string GetShortDay(Enumerations.Days day)
        {
            return day.ToString().Substring(0, 3);
        }

        private string GetShortHour(string hour)
        {
            hour = hour.Substring(0, 2);
            if (hour == "09")
                hour = "9";
            return hour;
        }

        private void SetCalendarStructureData()
        {
            List<string> days = new List<string>();
            foreach (Enumerations.Days day in Enum.GetValues(typeof(Enumerations.Days)))
            {
                days.Add(GetShortDay(day));
            }
            ViewBag.CalendarDays = days;

            List<Tuple<string, string>> hours = new List<Tuple<string, string>>();
            foreach(var hour in _context.Hours)
            {
                hours.Add(new Tuple<string, string>(hour.Value, GetShortHour(hour.Value)));
            }
            ViewBag.CalendarHours = hours;
        }

        private void CreateCourse(HomeIndexViewModel model)
        {
            Course course = new Course();
            course.Name = model.CourseName;
            course.Value = model.CourseValue * 100;
            _context.Courses.Add(course);
            _context.SaveChanges();

            var selectedTimes = JsonConvert.DeserializeAnonymousType(model.SelectedTimesJson, new[] { new { Day = 0, Hour = 0 } });
            foreach (var item in selectedTimes)
            {
                _context.CalendarEntries.Add(new CalendarEntry()
                {
                    Day = (Enumerations.Days)item.Day,
                    HourId = item.Hour,
                    CourseId = course.Id
                });
            }
            _context.SaveChanges();
        }

        private void UpdateCourse(HomeIndexViewModel model)
        {
            Course course = _context.Courses.Where(c => c.Id == model.CourseId).FirstOrDefault();
            if(course != null)
            {
                course.Name = model.CourseName;
                course.Value = model.CourseValue * 100;

                List<CalendarEntry> currentSelectedTimes = _context.CalendarEntries.Where(ce => ce.CourseId == model.CourseId).ToList();
                var newSelectedTimes = JsonConvert.DeserializeAnonymousType(model.SelectedTimesJson, new[] { new { Day = 0, Hour = 0 } }).ToList();

                List<CalendarEntry> toDelete = new List<CalendarEntry>();
                foreach (var item in currentSelectedTimes)
                {
                    var result = newSelectedTimes.Where(t => t.Day == (int)item.Day && t.Hour == item.HourId).FirstOrDefault();
                    if(result != null)
                    {
                        toDelete.Add(item);
                        newSelectedTimes.Remove(result);
                    }
                }
                foreach (var item in toDelete)
                {
                    currentSelectedTimes.Remove(item);
                }

                _context.CalendarEntries.RemoveRange(currentSelectedTimes);
                
                foreach (var item in newSelectedTimes)
                {
                    _context.CalendarEntries.Add(new CalendarEntry()
                    {
                        Day = (Enumerations.Days)item.Day,
                        HourId = item.Hour,
                        CourseId = course.Id
                    });
                }
                _context.SaveChanges();
            }
            else
                ModelState.AddModelError("CourseId", "This course wasn't found in database");
        }

    }
}