using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataService
{
    public class DayOfWeekClass
    {
        public DayOfWeekClass(DayOfWeek dayOfWeek, int numberOfDay)
        {
            this.DayOfWeek = dayOfWeek;
            this.NumberOfDay = numberOfDay;
        }
        public DayOfWeek DayOfWeek { get; set; }
        public int NumberOfDay { get; set; }
    }
}
