using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic
{
    public class CargoTrackingShipmentsLogicService
    {
        public static string GetMappingFields(string FieldName)
        {
            List<string> MappingFields = new List<string>();
            MappingFields.Add(GetTextMapping("ShipmentMasterDatas", "Master", "MasterShipmentDataId"));
            MappingFields.Add(GetTextMapping("Shipment", "PickupEstimationDate", "FirstPickupETD"));
            MappingFields.Add(GetTextMapping("Shipment", "ClearanceDate", "CustomsClearanceDate"));
            MappingFields.Add(GetTextMapping("Shipment", "CreateDate", "CreateDateTime"));
            MappingFields.Add(GetTextMapping("ShipmentMasterDatas", "DepartureEstimationDate", "MainCarriageETD"));
            MappingFields.Add(GetTextMapping("ShipmentMasterDatas", "ArrivalEstimationDate", "MainCarriageETA"));
            MappingFields.Add(GetTextMapping("ShipmentComputedFields", "DeliveredEstimationDate", "FinalDeliveryETA"));
            MappingFields.Add(GetTextMapping("ShipmentComputedFields", "PickupDate", "FirstPickupATD"));
            MappingFields.Add(GetTextMapping("ShipmentComputedFields", "PickupDone", "FirstPickupATD", "PickupDone = True , If FirstPickupATD not null and less then Todate PickupDon"));
            MappingFields.Add(GetTextMapping("Shipment", "FromWarehouseDone", "WarehouseLegActualEntryDate", "FromWarehouseDone = True , If DirectionId  = 'E' on Shipment and WarehouseLegActualEntryDate  not null and less then Todate PickupDon"));
            MappingFields.Add(GetTextMapping("Shipment", "FromWarehouseDate", "WarehouseLegActualEntryDate", "FromWarehouseDate = WarehouseLegActualEntryDate If DirectionId  = 'E' on Shipment"));
            MappingFields.Add(GetTextMapping("Shipment", "FromWarehouseEstimationDate", "WarehouseLegExpectedEntryDate", "FromWarehouseEstimationDate = WarehouseLegExpectedEntryDate If DirectionId  = 'E' on Shipment"));
            MappingFields.Add(GetTextMapping("Shipment", "FromWarehouseEstimationDate", "WarehouseLegExpectedEntryDate", "FromWarehouseEstimationDate = WarehouseLegExpectedEntryDate If DirectionId  = 'E' on Shipment"));
            MappingFields.Add(GetTextMapping("Shipment", "FromWarehouseNotes", "WarehouseLegRemarks", "FromWarehouseNotes = WarehouseLegRemarks If DirectionId  = 'E' on Shipment"));
            MappingFields.Add(GetTextMapping("ShipmentMasterDatas", "DepartureDate", "MainCarriageATD"));
            MappingFields.Add(GetTextMapping("CargoTrackingShipments", "DepartureDone", "DepartureDate" , "DepartureDone = true if DepartureDate is not null"));
            MappingFields.Add(GetTextMapping("ShipmentMasterDatas", "ArrivalDate", "MainCarriageATA"));
            MappingFields.Add(GetTextMapping("CargoTrackingShipments", "ArrivalDone", "ArrivalDate", "ArrivalDone = true if ArrivalDate is not null"));
            MappingFields.Add(GetTextMapping("CargoTrackingShipments", "ArrivalDone", "ArrivalDate", "ArrivalDone = true if ArrivalDate is not null"));
            MappingFields.Add(GetTextMapping("Shipment", "ToWarehouseDate", "WarehouseLegActualEntryDate", "ToWarehouseDate = WarehouseLegActualEntryDate If DirectionId  = 'E' on Shipment"));
            MappingFields.Add(GetTextMapping("CargoTrackingShipments", "ToWarehouseDone", "ToWarehouseDate", "ToWarehouseDone  = true if ToWarehouseDate is not null"));
            MappingFields.Add(GetTextMapping("Shipments", "ToWarehouseEstimationDate", "WarehouseLegExpectedEntryDate", "ToWarehouseEstimationDate  = WarehouseLegExpectedEntryDate if DirectionId  = 'E' on Shipment"));
            MappingFields.Add(GetTextMapping("Shipments", "ToWarehouseNotes", "WarehouseLegRemarks", "ToWarehouseNotes  = WarehouseLegRemarks if DirectionId  = 'E' on Shipment"));
            MappingFields.Add(GetTextMapping("Shipments", "DeliveredDate", "FinalDeliveryATA"));
            MappingFields.Add(GetTextMapping("Shipments", "CustomsPaymentDate", "DeclarationDate"));
            MappingFields.Add(GetTextMapping("Shipments", "ClearanceDate", "CustomsClearanceDate"));
            MappingFields.Add(GetTextMapping("CargoTrackingShipments", "DeliveredDone", "DeliveredDate", "DeliveredDone = true if DeliveredDate is not null"));
            MappingFields.Add(GetTextMapping("CargoTrackingShipments", "CustomsPaymentDone", "CustomsPaymentDate", "CustomsPaymentDone = true if CustomsPaymentDate is not null"));
            MappingFields.Add(GetTextMapping("CargoTrackingShipments", "ClearanceDone", "ClearanceDate", "ClearanceDone = true if ClearanceDate is not null"));
            MappingFields.Add(GetTextMapping("Shipments", "CustomerReference", "CustomerReference1,CustomerReference2", "CustomerReference = CustomerReference1+','+CustomerReference2"));
            MappingFields.Add(GetTextMapping("Shipments", "CustomerReference", "CustomerReference1,CustomerReference2", "CustomerReference = CustomerReference1+','+CustomerReference2"));
            MappingFields.Add(GetTextMapping("Shipments", "EntityId", "Id"));
            MappingFields.Add("IsMainRecord Is Always True From Incremental if shipment is Forwarding and not has a cusstom Shipment");
            MappingFields.Add("EntityType Is 'C' if shipment is cusstom or 'F' If Forwarding");
            MappingFields.Add(GetTextMapping("Shipments", "ForwardingShipmentHeaderId", "Id" , "ForwardingShipmentHeaderId = Id From Shipment If Shipment is Custom and has Forwarding shipment"));
            MappingFields.Add(GetTextMapping("Shipments", "CustomsShipmentHeaderId", "Id", "CustomsShipmentHeaderId = Id From Shipment If Shipment is Forwarding and has Custom shipment"));
            MappingFields.Add(GetTextMapping("CargoTrackingShipments", "CurrentMilestoneCode", "Depened on All Milestone", "This Sorting (First Done) = > DeliveredDone,ClearanceDone,CustomsPaymentDone,CustomsPaymentDone,ToWarehouseDone,ArrivalDone,DepartureDone,FromWarehouseDone,PickupDone"));
            MappingFields.Add(GetTextMapping("CargoTrackingShipments", "CurrentMilestoneDate", "Depened on All Milestone", "This Sorting (First Done) = > DeliveredDate,ClearanceDate,CustomsPaymentDate,CustomsPaymentDate,ToWarehouseDate,ArrivalDate,DepartureDate,FromWarehouseDate,PickupDate"));

            string Notes = "";
            Notes += MappingFields.Where(s => s.Contains(FieldName)).FirstOrDefault();

            return Notes;

        }
        public static void SetTableLogic(DataRow TableRow, int ConditionNumber)
        {
            CompareNullabelFirstPickupETADateTime(TableRow);
            TableRow.SetField("Master", TableRow["MasterShipmentDataId"]);
            TableRow.SetField("PickupEstimationDate", TableRow["FirstPickupETD"]);
            TableRow.SetField("ClearanceDate", TableRow["CustomsClearanceDate"]);
            TableRow.SetField("CreateDate", TableRow["CreateDateTime"]);
            TableRow.SetField("DepartureEstimationDate", TableRow["MainCarriageETD"]);
            TableRow.SetField("ArrivalEstimationDate", TableRow["MainCarriageETA"]);
            TableRow.SetField("DeliveredEstimationDate", TableRow["FinalDeliveryETA"]);
            TableRow.SetField("PickupDate", TableRow["FirstPickupATD"]);
            SetFromWarehouseDoneField(TableRow);
            SetPickupDoneField(TableRow);
            SetFromWarehouseDate(TableRow);
            SetFromWarehouseEstimationDate(TableRow);
            SetFromWarehouseNotes(TableRow);
            TableRow.SetField("DepartureDate", TableRow["MainCarriageATD"]);
            SetDepartureDone(TableRow);
            TableRow.SetField("ArrivalDate", TableRow["MainCarriageATA"]);
            SetArrivalDone(TableRow);
            SetToWarehouseDate(TableRow);
            SetToWarehouseDone(TableRow);
            SetToWarehouseEstimationDate(TableRow);
            SetToWarehouseNotes(TableRow);
            TableRow.SetField("DeliveredDate", TableRow["FinalDeliveryATA"]);
            SetDeliveredDone(TableRow);
            TableRow.SetField("CustomsPaymentDate", TableRow["DeclarationDate"]);
            SetCustomsPaymentDone(TableRow);
            TableRow.SetField("ClearanceDate", TableRow["CustomsClearanceDate"]);
            SetClearanceDone(TableRow);
            SetCustomerReference(TableRow);
            SetClearance(TableRow);
            if (ConditionNumber == 1)
            {
                SetFieldsForCustomShipment(TableRow);
            }
            else if (ConditionNumber == 2)
            {
                SetFieldsForForwardingShipment(TableRow);
            }
            SetCurrentMilestone(TableRow);

        }

        private static void SetCustomerReference(DataRow TableRow)
        {
            if (!TableRow["CustomerReference1"].Equals(null) && !TableRow["CustomerReference1"].Equals("") && TableRow["CustomerReference1"].GetType().Name != "DBNull" && !TableRow["CustomerReference2"].Equals(null) && !TableRow["CustomerReference2"].Equals("") && TableRow["CustomerReference2"].GetType().Name != "DBNull")
            {
                TableRow.SetField("CustomerReference", TableRow["CustomerReference1"] + "," + TableRow["CustomerReference2"]);
            }
            else if (!TableRow["CustomerReference1"].Equals(null) && TableRow["CustomerReference1"].GetType().Name != "" && TableRow["CustomerReference1"].GetType().Name != "DBNull")
            {
                TableRow.SetField("CustomerReference", TableRow["CustomerReference1"]);
            }
            else if (!TableRow["CustomerReference2"].Equals(null) && TableRow["CustomerReference2"].GetType().Name != "" && TableRow["CustomerReference2"].GetType().Name != "DBNull")
            {
                TableRow.SetField("CustomerReference", TableRow["CustomerReference2"]);
            }
        }

        private static void SetClearance(DataRow TableRow)
        {
            if (!TableRow["CustomsClearanceDate"].Equals(null) && TableRow["CustomsClearanceDate"].GetType().Name != "DBNull")
            {
                TableRow.SetField("ClearanceDone", true);
            }
            else
            {
                TableRow.SetField("ClearanceDone", false);

            }
        }
        private static void SetCurrentMilestone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "DeliveredDone") && !TableRow["DeliveredDone"].Equals("False"))
            {
                TableRow.SetField("CurrentMilestoneCode", "11");
                TableRow.SetField("CurrentMilestoneDate", TableRow["DeliveredDate"]);

            }
            else if (!IsFieldNullOrEmpty(TableRow, "ClearanceDone") && !TableRow["ClearanceDone"].Equals("False"))
            {
                TableRow.SetField("CurrentMilestoneCode", "9");
                TableRow.SetField("CurrentMilestoneDate", TableRow["ClearanceDate"]);

            }
            else if (!IsFieldNullOrEmpty(TableRow, "CustomsPaymentDone") && !TableRow["CustomsPaymentDone"].Equals("False"))
            {
                TableRow.SetField("CurrentMilestoneCode", "8");
                TableRow.SetField("CurrentMilestoneDate", TableRow["CustomsPaymentDate"]);

            }
 
            else if (!IsFieldNullOrEmpty(TableRow, "ToWarehouseDone") && !TableRow["ToWarehouseDone"].Equals("False"))
            {
                TableRow.SetField("CurrentMilestoneCode", "6");
                TableRow.SetField("CurrentMilestoneDate", TableRow["ToWarehouseDate"]);

            }
            else if (!IsFieldNullOrEmpty(TableRow, "ArrivalDone") && !TableRow["ArrivalDone"].Equals("False"))
            {
                TableRow.SetField("CurrentMilestoneCode", "5");
                TableRow.SetField("CurrentMilestoneDate", TableRow["ArrivalDate"]);

            }
            else if (!IsFieldNullOrEmpty(TableRow, "DepartureDone") && !TableRow["DepartureDone"].Equals("False"))
            {
                TableRow.SetField("CurrentMilestoneCode", "4");
                TableRow.SetField("CurrentMilestoneDate", TableRow["DepartureDate"]);

            }
            else if (!IsFieldNullOrEmpty(TableRow, "FromWarehouseDone") && !TableRow["FromWarehouseDone"].Equals("False"))
            {
                TableRow.SetField("CurrentMilestoneCode", "3");
                TableRow.SetField("CurrentMilestoneDate", TableRow["FromWarehouseDate"]);

            }
            else if (!IsFieldNullOrEmpty(TableRow, "PickupDone") && !TableRow["PickupDone"].Equals("False"))
            {
                TableRow.SetField("CurrentMilestoneCode", "2");
                TableRow.SetField("CurrentMilestoneDate", TableRow["PickupDate"]);

            }
 
        }
        private static void SetFieldsForCustomShipment(DataRow TableRow)
        {
            TableRow.SetField("EntityType", "C");
            TableRow.SetField("EntityId", TableRow["Id"]);
            TableRow.SetField("IsMainRecord", true);
            TableRow.SetField("ForwardingShipmentHeaderId", TableRow["ForwardingIdForCustom"]);


           
 
        }

        private static void SetFieldsForForwardingShipment(DataRow TableRow)
        {
            TableRow.SetField("EntityType", "F");
            TableRow.SetField("EntityId", TableRow["Id"]);

            if (!TableRow["CustomFileId"].Equals(null) && !TableRow["CustomFileId"].Equals("") && TableRow["CustomFileId"].GetType().Name != "DBNull")
            {
                TableRow.SetField("CustomsShipmentHeaderId", TableRow["CustomFileId"]);
                TableRow.SetField("IsMainRecord", false);
                //ForwardingShipments.Add((string)TableRow["Id"], (string)TableRow["CustomFileId"]);
            }
            else
            {
                TableRow.SetField("IsMainRecord", true);

            }
        }

        private static void SetFromWarehouseDoneField(DataRow TableRow)
        {
            TableRow.SetField("FromWarehouseDone", false);

            if (TableRow["DirectionId"].Equals("E"))
            {
                if (!IsFieldNullOrEmpty(TableRow, "WarehouseLegActualEntryDate"))
                {
                    DateTime? WarehouseLegActualEntryDate = (DateTime?)(TableRow["WarehouseLegActualEntryDate"]);
                    DateTime? todayDate = DateTime.Today.Date;

                    if (WarehouseLegActualEntryDate != null && WarehouseLegActualEntryDate.Value.Date <= todayDate.Value.Date)
                    {
                        TableRow.SetField("FromWarehouseDone", true);
                    }
                }
            }

        }

        private static void SetPickupDoneField(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "PickupDate"))
            {
                TableRow.SetField("PickupDone", true);
            }
            else
            {
                TableRow.SetField("PickupDone", false);

            }


        }


        private static void SetDepartureDone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "DepartureDate"))
            {
                TableRow.SetField("DepartureDone", true);
            }
            else
            {
                TableRow.SetField("DepartureDone", false);

            }


        }

        private static void SetToWarehouseDone(DataRow TableRow)
        {
            TableRow.SetField("ToWarehouseDone", false);
            if (TableRow["DirectionId"].Equals("I"))
            {
                if (!IsFieldNullOrEmpty(TableRow, "ToWarehouseDate"))
                {
                    TableRow.SetField("ToWarehouseDone", true);
                }

            }


        }

        private static void SetClearanceDone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "ClearanceDate"))
            {
                TableRow.SetField("ClearanceDone", true);
            }
            else
            {
                TableRow.SetField("ClearanceDone", false);

            }


        }

        private static void SetCustomsPaymentDone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "CustomsPaymentDate"))
            {
                TableRow.SetField("CustomsPaymentDone", true);
            }
            else
            {
                TableRow.SetField("CustomsPaymentDone", false);

            }


        }

        private static void SetDeliveredDone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "DeliveredDate"))
            {
                TableRow.SetField("DeliveredDone", true);
            }
            else
            {
                TableRow.SetField("DeliveredDone", false);

            }


        }

        private static void SetArrivalDone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "ArrivalDate"))
            {
                TableRow.SetField("ArrivalDone", true);
            }
            else
            {
                TableRow.SetField("ArrivalDone", false);

            }


        }


        private static void SetFromWarehouseDate(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("E"))
            {
                TableRow.SetField("FromWarehouseDate", TableRow["WarehouseLegActualEntryDate"]);
            }


        }


        private static void SetFromWarehouseEstimationDate(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("E"))
            {
                TableRow.SetField("FromWarehouseEstimationDate", TableRow["WarehouseLegExpectedEntryDate"]);
            }


        }

        private static void SetToWarehouseDate(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("I"))
            {
                TableRow.SetField("ToWarehouseDate", TableRow["WarehouseLegActualEntryDate"]);
            }


        }


        private static void SetToWarehouseEstimationDate(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("I"))
            {
                TableRow.SetField("ToWarehouseEstimationDate", TableRow["WarehouseLegExpectedEntryDate"]);
            }


        }



        private static void SetToWarehouseNotes(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("I"))
            {
                TableRow.SetField("ToWarehouseNotes", TableRow["WarehouseLegRemarks"]);
            }


        }

        private static void SetFromWarehouseNotes(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("E"))
            {
                TableRow.SetField("FromWarehouseNotes", TableRow["WarehouseLegRemarks"]);
            }


        }

        private static bool IsFieldNullOrEmpty(DataRow TableRow, string CoulmnName)
        {
            bool IsNull = false;
            if (TableRow[CoulmnName].Equals(null) || TableRow[CoulmnName].Equals("") || TableRow[CoulmnName].GetType().Name == "DBNull")
            {
                IsNull = true;
            }

            return IsNull;


        }

        private static void CompareNullabelFirstPickupETADateTime(DataRow TableRow)
        {
            var FirstPickupETA = TableRow["FirstPickupETA"].GetType();
            if (FirstPickupETA.FullName == "System.DBNull")
            {
                TableRow.SetField("PickupDone", false);
            }
            else
            {
                DateTime? lastPostDate = (DateTime?)(TableRow["FirstPickupETA"]);
                DateTime? todayDate = DateTime.Today.Date;

                if (FirstPickupETA != null && lastPostDate.Value.Date < todayDate.Value.Date)
                {
                    TableRow.SetField("PickupDone", true);
                }
                else
                {
                    TableRow.SetField("PickupDone", false);

                }
            }

        }


        
        private static string GetTextMapping(string TableNmae, string FieldFromMapping, string FieldName ,string ExtrNote=null)
        {
            string Note = FieldFromMapping + " Mapping From => " + Environment.NewLine + "Table Name: " + TableNmae + Environment.NewLine + "Field Name: " + FieldName;
            if (ExtrNote!=null)
            {
                Note += Environment.NewLine + ExtrNote;
            }
            return Note;

        }
    }
}
