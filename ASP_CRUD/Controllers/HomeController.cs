using ASP_CRUD.Lib;
using ASP_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace ASP_CRUD.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        private List<StudentModel> StudentsData
        {
            get
            {
                if (HttpContext.Cache["StudentsData"] == null)
                {
                    HttpContext.Cache["StudentsData"] = factory.GetTopNStudents(100);
                }
                return (List<StudentModel>)HttpContext.Cache["StudentsData"];
            }
            set
            {
                HttpContext.Cache["StudentsData"] = value;
            }
        }
        private List<Lector> LectorData
        {
            get
            {
                if (HttpContext.Cache["LectorData"] == null)
                {
                    HttpContext.Cache["LectorData"] = factory.GetNLectors(100);
                }
                return (List<Lector>)HttpContext.Cache["LectorData"];
            }
            set
            {
                HttpContext.Cache["LectorData"] = value;
            }
        }
        Factory factory = new Factory();
        public ActionResult Students(bool UseCache = true)
        {
            if (!UseCache)
            {
                StudentsData = factory.GetTopNStudents(100);
            }
            ViewBag.Message = "Your students page.";
            return View(StudentsData);
        }
        public ActionResult Lecturers(bool UseCache = true)
        {
            if (!UseCache)
            {
                LectorData = factory.GetNLectors(100);
            }
            ViewBag.Message = "Your lectors page.";
            return View(LectorData);
        }
        public ActionResult DeleteStudent(long idStudent)
        {
            try
            {
                bool deleted = factory.DeleteStudent(idStudent);
                if (deleted)
                {
                    StudentModel student = StudentsData.Where(s => s.ID_student == idStudent).FirstOrDefault();
                    if (student != null)
                    {
                        StudentsData.Remove(student);
                    }
                }
                return RedirectToAction("Students");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error deleting student: " + ex.Message;
                return View("../Shared/Error");
            }
        }
        public ActionResult DeleteLecturer(long idLector)
        {
            try
            {
                bool deleted = factory.DeleteLecturer(idLector);
                if (deleted)
                {
                    Lector lector = LectorData.Where(l => l.ID_lector == idLector).FirstOrDefault();
                    if (lector != null)
                    {
                        LectorData.Remove(lector);
                    }
                }
                return RedirectToAction("Lecturers");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error deleting lector: " + ex.Message;
                return View("../Shared/Error");
            }
        }
        [HttpGet]
        public ActionResult CreateStudent()
        {
            return PartialView("StudentRegistration", new StudentModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudent(StudentModel model, long? course)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("StudentRegistration", model);
            }

            try
            {
                int year = int.Parse(Request["BirthYear_Year"]);
                int month = int.Parse(Request["BirthYear_Month"]);
                int day = int.Parse(Request["BirthYear_Day"]);

                model.BirthYear = new DateTime(year, month, day);
                model.ID_course = course;

                factory.AddStudent(model);

                StudentsData = factory.GetTopNStudents(100);

                return RedirectToAction("Students");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, error = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult CreateLecturer()
        {
            return PartialView("LecturerRegistration", new Lector());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLecturer(Lector model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("LecturerRegistration", model);
            }
            try
            {
                int year = int.Parse(Request["HireDate_Year"]);
                int month = int.Parse(Request["HireDate_Month"]);
                int day = int.Parse(Request["HireDate_Day"]);
                model.HireDate = new DateTime(year, month, day);
                
                factory.AddLecturer(model);
                LectorData = factory.GetNLectors(100);
                return RedirectToAction("Lecturers");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, error = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult EditStudent(long idStudent)
        {
            Student student = factory.GetStudentById(idStudent);

            DateTime birthDate;
            if (!student.BirthYear.HasValue)
            {
                birthDate = DateTime.Today;
            }
            else
            {
                birthDate = student.BirthYear.Value;
            }

            ViewBag.YearItems = Enumerable.Range(DateTime.Now.Year - 100, 101)
                .Reverse()
                .Select(y => new SelectListItem
                {
                    Value = y.ToString(),
                    Text = y.ToString(),
                    Selected = (birthDate.Year == y)
                });

            ViewBag.MonthItems = Enumerable.Range(1, 12)
                .Select(m => new SelectListItem
                {
                    Value = m.ToString(),
                    Text = m.ToString(),
                    Selected = (birthDate.Month == m)
                });

            ViewBag.DayItems = Enumerable.Range(1, 31)
                .Select(d => new SelectListItem
                {
                    Value = d.ToString(),
                    Text = d.ToString(),
                    Selected = (birthDate.Day == d)
                });

            return PartialView("EditStudent", student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("EditStudent", student);
            }
            try
            {
                int year = int.Parse(Request["BirthYear_Year"]);
                int month = int.Parse(Request["BirthYear_Month"]);
                int day = int.Parse(Request["BirthYear_Day"]);

                student.BirthYear = new DateTime(year, month, day);

                factory.UpdateStudent(student);
                StudentsData = factory.GetTopNStudents(100);
                return RedirectToAction("Students");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}