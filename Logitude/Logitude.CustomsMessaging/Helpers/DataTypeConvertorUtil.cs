using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Helpers
{
    public class DataTypeConvertorUtil
    {


        public static string Convert(Nullable<DateTime> dateTime)
        {
            if (dateTime.HasValue)
            {
                return Convert(dateTime.Value);
            }
            return "";
        }
        public static string Convert(DateTime dateTime)
        {
            return dateTime.ToString("O").Substring(0, 19);
        }
        public static string Convert(decimal myDecimal)
        {
            return myDecimal.ToString("#,###,##0.00");
        }
    }
}
