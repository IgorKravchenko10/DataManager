using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Models
{
    public class ColumnData
    {
        public int Column { get; set; }

        public List<double> Values { get; set; }

        public ColumnData()
        {
            this.Values = new List<double>();
        }
    }
}
