using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public class CargoTrackingCardTableStructure
    {
        public string GetColumnsForCopy()
        {
            List<string> ColumnsForCopy = new List<string>();
            ColumnsForCopy.Append("Id");
            ColumnsForCopy.Append("Tenant");
            ColumnsForCopy.Append("Code");
            ColumnsForCopy.Append("LocalName");
            ColumnsForCopy.Append("EnglishName");
            ColumnsForCopy.Append("AutomaticLastUpdateDate");


            return string.Join(",", ColumnsForCopy.ToArray());
        }


    }
}
