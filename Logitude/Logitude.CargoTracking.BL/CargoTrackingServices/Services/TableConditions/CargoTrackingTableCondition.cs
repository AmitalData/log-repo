
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class CargoTrackingTableBuildWhereCondition
    {

        public static string BuildWhereCondition(BuildWhereConditionArgs buildWhereConditionArg , bool IsClosedTable=false)
        {
            string condition = " where (AutomaticLastUpdateDate > '" + buildWhereConditionArg.LastUpdate + "')";

            if (buildWhereConditionArg.CargoTrackingArguments != null)
            {

                if (buildWhereConditionArg.CargoTrackingArguments.Tenant != null && !IsClosedTable)
                {
                    condition = " where Tenant = " + buildWhereConditionArg.CargoTrackingArguments.Tenant;
                }
                else
                {
                    condition = "";
                }


            }


            return condition;


        }
        


    }

}
