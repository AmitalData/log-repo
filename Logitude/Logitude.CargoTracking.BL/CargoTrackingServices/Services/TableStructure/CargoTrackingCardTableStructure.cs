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
            List<string> columnsForCopy = new List<string>();
            columnsForCopy.Append("Id");
            columnsForCopy.Append("Tenant");
            columnsForCopy.Append("Code");
            columnsForCopy.Append("LocalName");
            columnsForCopy.Append("EnglishName");
            columnsForCopy.Append("AutomaticLastUpdateDate");


            return string.Join(",", columnsForCopy.ToArray());
        }


    }
}
