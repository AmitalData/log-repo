using Logitude.Base.Models.UserTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLogitude.ShipmentTests.Services
{
    public class DateHelper
    {
        public static DateTime FillTodayDate(string Date)
        {
            if (Date.ToUpper() == "TODAY")
            {
                return DateTime.Now;
            }
            return DateTime.Now; 
        }
    }
}
