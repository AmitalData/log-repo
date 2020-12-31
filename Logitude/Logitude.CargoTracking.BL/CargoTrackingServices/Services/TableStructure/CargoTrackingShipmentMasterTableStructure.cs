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
            List<string> columnsForCopy = new List<string>();
            columnsForCopy.Add("Id");
            columnsForCopy.Add("Master");
            columnsForCopy.Add("MainCarriageATD");
            columnsForCopy.Add("MainCarriageETD");
            columnsForCopy.Add("MainCarriageATA");
            columnsForCopy.Add("MainCarriageETA");
            columnsForCopy.Add("Tenant");
            columnsForCopy.Add("AutomaticLastUpdateDate");


            return string.Join(",", columnsForCopy.ToArray());
        }


    }
}
