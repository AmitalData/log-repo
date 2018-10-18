using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Helpers
{
    public class DashBoardBookingClass
    {
        private static int counter = 0;
        public DashBoardBookingClass()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }
        public string FFRStatusName { get; set; }
        public int Count { get; set; }
    }
}
