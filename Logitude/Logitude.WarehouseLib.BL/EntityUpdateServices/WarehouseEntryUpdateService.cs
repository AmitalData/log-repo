using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.Helpers;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.WarehouseLib.BL.EntityUpdateServices
{

    public partial class WarehouseEntryUpdateService
    {

        protected override void OnCreating(WarehouseEntryPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("WarehouseEntry", entityPM.Tenant);
                entityPM.EntryNumber = TableCounter.GetNumber(entityPM.Tenant, "WAEC", null, null).ToString();
                this.BuildActivityLog("N", entityPM);

            }
        }

        private void UpdateShipment(WarehouseEntryPM entityPM)
        {
            ShipmentRepository myShipmentRepository = new ShipmentRepository(entityPM.Tenant);
            Shipment myShipment = myShipmentRepository.GetSingleShipment(entityPM.ShipmentId, entityPM.Tenant);
            if (myShipment != null && string.IsNullOrEmpty(myShipment.WarehouseLegWarehouseId))
            {
                myShipment.WarehouseLegWarehouseId = entityPM.WarehouseId;
                myShipment.WarehouseLegExpectedEntryDate = entityPM.ExpectedEntryDate;
                myShipment.WarehouseLegActualEntryDate = entityPM.ActualEntryDate;
                myShipmentRepository.Update(myShipment);
                myShipmentRepository.SubmitChanges();
            }
        }



        private void AddTraceEvent(List<string> eventCodeList, string userId)
        {
            if (eventCodeList != null && eventCodeList.Count > 0)
            {
                foreach (string eventCode in eventCodeList)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = EntityPM.Tenant,
                        EventTypeCode = eventCode,
                        UserId = userId,
                        EntityId = EntityPM.Id,
                        ObjectTableName = "WarehouseEntry",
                    });
                }
            }

        }




        protected override void OnUpdating(WarehouseEntryPM entityPM, WarehouseEntry entityPOCO)
        {
            AddTraceEvents(entityPM, entityPOCO);

            if (entityPM.StatusCode != entityPOCO.StatusCode)
            {
                entityPM.LastStatusUpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }

            base.OnUpdating(entityPM, entityPOCO);
        }

        private void AddTraceEvents(WarehouseEntryPM entityPM, WarehouseEntry entityPOCO)
        {
            List<string> eventCodeLists = new List<string>();
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert) eventCodeLists.Add("CREN");
            else eventCodeLists.Add("UPEN");
            if (entityPM.ExpectedEntryDate != entityPOCO.ExpectedEntryDate) eventCodeLists.Add("EXEN");
            if (entityPM.ActualEntryDate != entityPOCO.ActualEntryDate) eventCodeLists.Add("ENEN");

            var eventTracerArgs = new EventTracerArgs() { Tenant = entityPM.Tenant, UserId = entityPM.UpdatedByUserId, EntityId = entityPM.Id, ObjectTableName = "WarehouseEntry", };
            WarehouseEntryReleaseHelper warehouseEntryReleaseHelper = new WarehouseEntryReleaseHelper();
            warehouseEntryReleaseHelper.AddTraceEvents(eventCodeLists, eventTracerArgs);

        }

        protected override void OnUpdating(WarehouseEntryPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                string x = "";
                if (!string.IsNullOrEmpty(entityPM.CustomerRef1) && !string.IsNullOrEmpty(entityPM.CustomerRef2)) x = ",";
                entityPM.References = entityPM.CustomerRef1 + x + entityPM.CustomerRef2;

                WarehouseEntryStatusRepository warehouseEntryStatusRepository = new WarehouseEntryStatusRepository(entityPM.Tenant);
                WarehouseEntryStatus warehouseEntryStatus = warehouseEntryStatusRepository.GetSingle(entityPM.StatusCode);
                if (warehouseEntryStatus != null) entityPM.StatusName = warehouseEntryStatus.Name;

                this.BuildActivityLog("U", entityPM);
                this.ValidatePorts(entityPM);
                this.ValidateInlandDomestic(entityPM);
            }
        }

        protected override void UpdateComposition(WarehouseEntryPM entityPM)
        {
            WarehouseEntryPackageUpdateService warehouseEntryPackageUpdateService = new WarehouseEntryPackageUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            warehouseEntryPackageUpdateService.UpdateMulti(entityPM.WarehouseEntryPackages, entityPM.DeletedWarehouseEntryPackages, entityPM, false);

            base.UpdateComposition(entityPM);
        }

        
        private void BuildActivityLog(string typeCode, WarehouseEntryPM entityPM)
        {

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("WarehouseEntry", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant,true);
            if (loggedContact != null)
            {
                ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, typeCode, loggedContact.Id);
                if (typeCode == "U") entityPM.UpdatedByUserId = loggedContact.Id;


            }
        }
        private void ValidatePorts(WarehouseEntryPM entityPM)
        {
            var isInlandDomestic = entityPM.TransportModeId == "I" && entityPM.DirectionId == "D" ? true : false;
            if (!isInlandDomestic)
            {
                string msg = "Both Ports must be in the same country since the direction is Domestic";
                string fromCountryId = null;
                bool fromCountryIsEC = false;
                string toCountryId = null;
                bool toCountryIsEC = false;

                PortQuery myPortQuery = new PortQuery(entityPM.Tenant);
                if (entityPM.DirectionId == "D")
                {
                    if (!string.IsNullOrEmpty(entityPM.FromPortId) && !string.IsNullOrEmpty(entityPM.ToPortId))
                    {
                        PortPM FromPortList = myPortQuery.GetSinglePM(entityPM.FromPortId, entityPM.Tenant);
                        PortPM ToPortList = myPortQuery.GetSinglePM(entityPM.ToPortId, entityPM.Tenant);

                        if (FromPortList != null)
                        {
                            fromCountryId = FromPortList.CountryId;
                            fromCountryIsEC = FromPortList.CountryEC;
                        }

                        if (ToPortList != null)
                        {
                            toCountryId = ToPortList.CountryId;
                            toCountryIsEC = ToPortList.CountryEC;
                        }

                        if (fromCountryId != toCountryId)
                        {
                            if (fromCountryIsEC == false || toCountryIsEC == false)
                            {
                                throw new ApplicationException(msg);
                            }
                        }
                    }
                }
            }
        }
        private void ValidateInlandDomestic(WarehouseEntryPM entityPM)
        {
            var msg = "Both Addresses must be in the same country since the direction is Domestic";
            var isInlandDomestic = entityPM.TransportModeId == "I" && entityPM.DirectionId == "D" ? true : false;
            if (isInlandDomestic)
            {
                AddressQuery myAddressQuery = new AddressQuery(entityPM.Tenant);
                if (entityPM.ShipmentLevelCode != "C")
                {
                    string fromCountryId = null;
                    bool fromCountryIsEC = false;
                    string toCountryId = null;
                    bool toCountryIsEC = false;

                    AddressList ShipperAddressList = myAddressQuery.GetSingleAddressList(entityPM.FromAddressId, entityPM.Tenant);
                    AddressList ConsigneeAddressList = myAddressQuery.GetSingleAddressList(entityPM.ToAddressId, entityPM.Tenant);


                    if (ShipperAddressList != null)
                    {
                        fromCountryId = ShipperAddressList.CountryId;
                        fromCountryIsEC = ShipperAddressList.CountryEC;
                    }

                    if (ConsigneeAddressList != null)
                    {
                        toCountryId = ConsigneeAddressList.CountryId;
                        toCountryIsEC = ConsigneeAddressList.CountryEC;
                    }
                    if (!string.IsNullOrEmpty(entityPM.ShipperId) && !string.IsNullOrEmpty(entityPM.ConsigneeId))
                    {
                        if (fromCountryId != toCountryId)
                        {
                            if (fromCountryIsEC == false || toCountryIsEC == false)
                            {
                                throw new ApplicationException(msg);
                            }
                        }
                    }
                }
            }

        }
       

    }
}
