 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class CargoTrackingTableCondition
    {

        public static string Condition(string TableName, string LastUpdate,CargoTrackingArguments CargoTrackingArguments = null, string Condition = null)
        {
            string condition = " where (AutomaticLastUpdateDate > '" + LastUpdate + "')";
            //if (LastUpdate!=null)
            //{
            //    condition = " where (AutomaticLastUpdateDate > '"+ LastUpdate+"')";
            //}
            if (Condition != null && CargoTrackingArguments!=null)
            {
                condition = " where "+Condition;
            }
            else if (Condition != null && CargoTrackingArguments == null)
            {
                 
                   condition += " and "+ Condition;
            }
            if (CargoTrackingArguments != null)
            {
                if(TableName== "CargoTrackingShipments" || TableName == "CargoTrackingShipmentSearches")
                {
                    if (CargoTrackingArguments.Tenant!=null)
                    {
                       condition = " where Tenant=" + CargoTrackingArguments.Tenant + " and CreateDateTime >= '" + CargoTrackingArguments.FromDate + "' and CreateDateTime <= '" + CargoTrackingArguments.ToDate+"'";

                    }
                    else 
                    {
                        condition += " and CreateDateTime >= '" + CargoTrackingArguments.FromDate + "' and CreateDateTime <= '" + CargoTrackingArguments.ToDate+"'";

                    }

                }
                else
                {
                    if (CargoTrackingArguments.Tenant != null)
                    {
                        condition = " where Tenant = " + CargoTrackingArguments.Tenant;
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
