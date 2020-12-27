using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public class CargoTrackingPortTableStructure
    {
        public string GetColumnsForCopy()
        {
            List<string> ColumnsForCopy = new List<string>();
            ColumnsForCopy.Add("Id");
            ColumnsForCopy.Add("Tenant");
            ColumnsForCopy.Add("Code");
            ColumnsForCopy.Add("CountryId");
            ColumnsForCopy.Add("EnglishName");
            ColumnsForCopy.Add("AutomaticLastUpdateDate");


            return string.Join(",", ColumnsForCopy.ToArray());
        }


    }
}
