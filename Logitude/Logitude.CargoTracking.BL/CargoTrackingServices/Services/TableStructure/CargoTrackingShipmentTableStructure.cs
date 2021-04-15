using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public class CargoTrackingShipmentTableStructure
    {
        public string GetColumnsForCopy()
        {
            List<string> columnsForCopy = new List<string>();
            columnsForCopy.Add("Id");
            columnsForCopy.Add("Tenant");
            columnsForCopy.Add("CustomFileNumber");
            columnsForCopy.Add("ForwarderShipmentNumber");
            columnsForCopy.Add("CustomsDeclarationNumber");
            columnsForCopy.Add("ShipperName");
            columnsForCopy.Add("CustomerId");
            columnsForCopy.Add("TransportModeId");
            columnsForCopy.Add("DirectionId");
            columnsForCopy.Add("MasterShipmentDataId");
            columnsForCopy.Add("House");
            columnsForCopy.Add("ShipmentNumber");
            columnsForCopy.Add("FromPortId");
            columnsForCopy.Add("ToPortId");
            columnsForCopy.Add("ShipperId");
            columnsForCopy.Add("ConsigneeId");
            columnsForCopy.Add("GrossWeight");
            columnsForCopy.Add("Volume");
            columnsForCopy.Add("CustomConnectToShipment");
            columnsForCopy.Add("AutomaticLastUpdateDate");
            columnsForCopy.Add("ShipmentPickUpIndex");
            columnsForCopy.Add("FirstPickupETA");
            columnsForCopy.Add("ShipmentLevelCode");
            columnsForCopy.Add("CustomsClearanceDate");
            columnsForCopy.Add("CustomFileId");
            columnsForCopy.Add("CreateDateTime");
            columnsForCopy.Add("SecurityKey");
            columnsForCopy.Add("ConsigneeName");
            columnsForCopy.Add("CustomerReference1");
            columnsForCopy.Add("CustomerReference2");
            columnsForCopy.Add("FirstPickupETD");
            columnsForCopy.Add("WarehouseLegActualEntryDate");
            columnsForCopy.Add("WarehouseLegExpectedEntryDate");
            columnsForCopy.Add("WarehouseLegRemarks");
            columnsForCopy.Add("DeclarationDate");
            columnsForCopy.Add("IsCancelled");
            columnsForCopy.Add("SearchFields");
            columnsForCopy.Add("PackagesQuantity");

            columnsForCopy.Add("AssignedToTruckerDate");
            columnsForCopy.Add("AssginedToCustomsAgentDate");
            columnsForCopy.Add("GrossWeightUnitCode");
            columnsForCopy.Add("ShipmentTypeId");


            return string.Join(",", columnsForCopy.ToArray());
        }

        public string GetDummyColumnsForCopy()
        {
            List<string> ColumnsForCopy = new List<string>();
            ColumnsForCopy.Add("IsPublic");
            ColumnsForCopy.Add("ReferenceType");

            return string.Join(",", ColumnsForCopy.ToArray());
        }
   }
}
