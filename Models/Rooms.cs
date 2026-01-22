using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class Rooms
    {
        [Key] 
        public int RoomID { get; set; }        
        public int RoomTypeID { get; set; }
        public string? RoomCode{ get; set; }
        public int NoDesks { get; set; }
        public int NoPCs { get; set; }
        public bool Enabled { get; set; }
    }

}












