using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class Teachers
    {
        [Key] 
        public int TeacherID { get; set; }        
        public string? Title { get; set; }
        public string? FirstName{ get; set; }
        public string? Surname{ get; set; }
        public string? Sex { get; set; }
    }

}












