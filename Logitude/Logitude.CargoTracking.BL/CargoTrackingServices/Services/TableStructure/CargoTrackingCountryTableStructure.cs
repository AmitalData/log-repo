using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public class CargoTrackingCountryTableStructure
    {
        public string GetColumnsForCopy()
        {
            List<string> columnsForCopy = new List<string>();
            columnsForCopy.Add("Id");
            columnsForCopy.Add("LocalName");
            columnsForCopy.Add("Code");
            columnsForCopy.Add("EnglishName");
            columnsForCopy.Add("Tenant");
            columnsForCopy.Add("AutomaticLastUpdateDate");


            return string.Join(",", columnsForCopy.ToArray());
        }


    }
}
