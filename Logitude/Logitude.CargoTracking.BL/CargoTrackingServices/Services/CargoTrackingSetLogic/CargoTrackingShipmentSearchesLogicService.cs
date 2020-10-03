using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic
{
    public class CargoTrackingShipmentSearchesLogicService
    {
        public static void SetTableLogic(DataRow TableRow,  int ConditionNumber)
        {
            SetShipmentDate(TableRow, ConditionNumber);
        }

        private static void SetShipmentDate(DataRow TableRow, int ConditionNumber)
        {
            TableRow.SetField("ShipmentDate", TableRow["CreateDateTime"]);
            TableRow.SetField("ShipmentId", TableRow["Id"]);
            TableRow.SetField("IsPublic", true);

        }

        public static string GetMappingFields(string FieldName)
        {
            List<string> MappingFields = new List<string>();
            MappingFields.Add(GetTextMapping("Shipment", "ShipmentDate", "CreateDateTime"));
            MappingFields.Add(GetTextMapping("Shipment", "ShipmentId", "Id"));
            MappingFields.Add("IsPublic Is Always True From Incremental Service");

            string Notes = "";
            Notes += MappingFields.Where(s => s.Contains(FieldName)).FirstOrDefault();

            return Notes;

        }
         private static string GetTextMapping(string TableNmae , string FieldFromMapping, string FieldName )
        {
            string Note = FieldFromMapping+ " Mapping From => " +Environment.NewLine+"Table Name: "+ TableNmae+ Environment.NewLine+"Field Name: "+ FieldName;
            return Note;
           
        }
    }
}
