using ASP_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_CRUD.Lib
{
    public class Factory
    {
        Repository repository = new Repository(new UniversityExampleEntities());
        public List<StudentModel> GetTopNStudents(int count)
        {
            try
            {
                return TransformStudentToStudentView(repository.GetEntities<Student>().OrderByDescending(s => s.Scholarship).Take(count).ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Lector> GetNLectors(int count)
        {
            try
            {
                return repository.GetEntities<Lector>().OrderByDescending(l => l.HireDate).Take(count).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<StudentModel> TransformStudentToStudentView(List<Student> students)
        {
            List<Course> courses = repository.GetEntities<Course>().ToList();
            return students.Select(s => new StudentModel
            {
                ID_student = s.ID_student,
                Name = s.Name,
                Surname = s.Surname,
                BirthYear = s.BirthYear,
                Gender = s.Gender,
                Course = (s.ID_course != null) ? courses.Where(c => c.ID_course == s.ID_course).FirstOrDefault().Name : string.Empty,
                Scholarship = (s.Scholarship != null) ? s.Scholarship : 0
            }).ToList();
        }
        public bool DeleteStudent(long idStudent)
        {
            try
            {
                return repository.DeleteEntity<Student>(idStudent);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting student: " + ex.Message);
            }
        }
        public StudentModel AddStudent(StudentModel student)
        {
            try
            {
                var entity = new Student
                {
                    Name = student.Name,
                    Surname = student.Surname,
                    BirthYear = student.BirthYear,
                    Gender = student.Gender,
                    ID_course = student.ID_course,
                    Scholarship = student.Scholarship
                };
                var createdEntity = repository.AddEntity(entity);
                student.ID_student = createdEntity.ID_student;
                return student;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding student: " + ex.Message);
            }
        }
        public List<Course> GetAllCourses()
        {
            try
            {
                return repository.GetEntities<Course>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving courses: " + ex.Message);
            }
        }
        public List<Faculty> GetAllFaculties()
        {
            try
            {
                return repository.GetEntities<Faculty>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving faculties: " + ex.Message);
            }
        }
    }
}