using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class StudentCourses
    {
        [Key]
        public int StudentCourseID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public bool Accessible { get; set; }
    }

}












