using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public class CargoTrackingShipmentMasterTableStructure
    {
        public string GetColumnsForCopy()
        {
            List<string> ColumnsForCopy = new List<string>();
            ColumnsForCopy.Add("Id");
            ColumnsForCopy.Add("Master");
            ColumnsForCopy.Add("MainCarriageATD");
            ColumnsForCopy.Add("MainCarriageETD");
            ColumnsForCopy.Add("MainCarriageATA");
            ColumnsForCopy.Add("MainCarriageETA");
            ColumnsForCopy.Add("Tenant");
            ColumnsForCopy.Add("AutomaticLastUpdateDate");


            return string.Join(",", ColumnsForCopy.ToArray());
        }


    }
}
