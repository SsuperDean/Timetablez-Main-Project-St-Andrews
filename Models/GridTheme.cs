using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetablez.Models
{
    public class GridTheme
    {
        public Color HeaderBackColor { get; set; }
        public Color HeaderForeColor { get; set; }
        public Color HeaderSelectionBackColor { get; set; }
        public Color HeaderSelectionForeColor { get; set; }

        public Color RowBackColor { get; set; }
        public Color AlternatingRowBackColor { get; set; }
        public Color SelectionBackColor { get; set; }
    }
}
