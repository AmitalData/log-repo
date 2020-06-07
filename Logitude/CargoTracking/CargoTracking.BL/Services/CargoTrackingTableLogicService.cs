using CargoTracking.CargoTracking.BL.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTracking.CargoTracking.BL.Services
{
    public class CargoTrackingTableLogicService
    {

        public static  void SetTableLogic(DataRow TableRow,string TableName)
        {
            if (TableName == "CargoTrackingPorts")
            {
                if ( TableRow["Id"].Equals("1-100"))
                {
                    TableRow.SetField("EnglishName", "Mutaz");
                }
            }
           

        }
          
      
    }
 
}
