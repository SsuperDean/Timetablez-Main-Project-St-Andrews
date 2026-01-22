using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class ExamBoards
    {
        [Key] 
        public int ExamBoardID { get; set; }
        public string? Name{ get; set; }
    }

}












