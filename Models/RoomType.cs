using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetablez.Models
{
    public class RoomType
    {
        [Key]
        public int RoomTypeID { set; get; }
        public string Description { set; get; }


    }
}
