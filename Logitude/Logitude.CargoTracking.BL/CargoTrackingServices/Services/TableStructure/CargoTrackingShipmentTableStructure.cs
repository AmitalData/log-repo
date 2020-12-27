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
            List<string> ColumnsForCopy = new List<string>();
            ColumnsForCopy.Add("Id");
            ColumnsForCopy.Add("Tenant");
            ColumnsForCopy.Add("CustomFileNumber");
            ColumnsForCopy.Add("ForwarderShipmentNumber");
            ColumnsForCopy.Add("CustomsDeclarationNumber");
            ColumnsForCopy.Add("ShipperName");
            ColumnsForCopy.Add("CustomerId");
            ColumnsForCopy.Add("TransportModeId");
            ColumnsForCopy.Add("DirectionId");
            ColumnsForCopy.Add("MasterShipmentDataId");
            ColumnsForCopy.Add("House");
            ColumnsForCopy.Add("ShipmentNumber");
            ColumnsForCopy.Add("FromPortId");
            ColumnsForCopy.Add("ToPortId");
            ColumnsForCopy.Add("ShipperId");
            ColumnsForCopy.Add("ConsigneeId");
            ColumnsForCopy.Add("GrossWeight");
            ColumnsForCopy.Add("Volume");
            ColumnsForCopy.Add("CustomConnectToShipment");
            ColumnsForCopy.Add("AutomaticLastUpdateDate");
            ColumnsForCopy.Add("ShipmentPickUpIndex");
            ColumnsForCopy.Add("FirstPickupETA");
            ColumnsForCopy.Add("ShipmentLevelCode");
            ColumnsForCopy.Add("CustomsClearanceDate");
            ColumnsForCopy.Add("CustomFileId");
            ColumnsForCopy.Add("CreateDateTime");
            ColumnsForCopy.Add("SecurityKey");
            ColumnsForCopy.Add("ConsigneeName");
            ColumnsForCopy.Add("CustomerReference1");
            ColumnsForCopy.Add("CustomerReference2");
            ColumnsForCopy.Add("FirstPickupETD");
            ColumnsForCopy.Add("WarehouseLegActualEntryDate");
            ColumnsForCopy.Add("WarehouseLegExpectedEntryDate");
            ColumnsForCopy.Add("WarehouseLegRemarks");
            ColumnsForCopy.Add("DeclarationDate");
            ColumnsForCopy.Add("IsCancelled");
            ColumnsForCopy.Add("SearchFields");
            ColumnsForCopy.Add("PackagesQuantity");


            return string.Join(",", ColumnsForCopy.ToArray());
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
