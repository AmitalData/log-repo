
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
            //if (LastUpdate!=null)
            //{
            //    condition = " where (AutomaticLastUpdateDate > '"+ LastUpdate+"')";
            //}
            if (buildWhereConditionArg.Condition != null && buildWhereConditionArg.CargoTrackingArguments != null)
            {
                condition = " where "+ buildWhereConditionArg.Condition;
            }
            else if (buildWhereConditionArg.Condition != null && buildWhereConditionArg.CargoTrackingArguments == null)
            {
                 
                   condition += " and "+ buildWhereConditionArg.Condition;
            }
            if (buildWhereConditionArg.CargoTrackingArguments != null)
            {
                if(buildWhereConditionArg.TableName == "CargoTrackingShipments" || buildWhereConditionArg.TableName == "CargoTrackingShipmentSearches")
                {
                    if (buildWhereConditionArg.CargoTrackingArguments.Tenant!=null)
                    {
                       condition = " where Tenant=" + buildWhereConditionArg.CargoTrackingArguments.Tenant + " and CreateDateTime >= '" + buildWhereConditionArg.CargoTrackingArguments.FromDate + "' and CreateDateTime <= '" + buildWhereConditionArg.CargoTrackingArguments.ToDate+"'";

                    }
                    else 
                    {
                        condition += " and CreateDateTime >= '" + buildWhereConditionArg.CargoTrackingArguments.FromDate + "' and CreateDateTime <= '" + buildWhereConditionArg.CargoTrackingArguments.ToDate+"'";

                    }

                }
                else
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
            }

 
            return condition;


        }
        


    }

}
