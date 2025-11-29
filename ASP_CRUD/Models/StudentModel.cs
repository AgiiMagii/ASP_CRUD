using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_CRUD.Models
{
    public class StudentModel
    {
        public long ID_student { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthYear { get; set; }
        public string Course { get; set; }
        public long? ID_course { get; set; }
        public Nullable<decimal> Scholarship { get; set; }
    }
}