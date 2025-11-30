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
        public ActionResult CreateStudent(StudentModel model, long? course) // Accept nullable long in case no course was selected
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
    }
}