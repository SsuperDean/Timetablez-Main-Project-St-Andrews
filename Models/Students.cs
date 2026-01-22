using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class Students
    {
        [Key] 
        public int StudentID { get; set; }        
        public string? StudentCode { get; set; }
        public string? Title { get; set; }
        public string? FirstName{ get; set; }
        public string? Surname{ get; set; }
        public string? Sex { get; set; }
    }

}












