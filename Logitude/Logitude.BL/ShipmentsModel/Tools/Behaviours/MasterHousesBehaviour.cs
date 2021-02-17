using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class MasterHousesBehaviour
    {
        private ShipmentServiceInitializer initializer;
        private bool isNewEntity;
        private List<Shipment> allHouses;
        private List<InsideShipmentPackagePM> masterInsidePackages;
        private ShipmentPM iHousePM;

        public MasterHousesBehaviour(ShipmentServiceInitializer initializer, List<Shipment> allHouses, bool isNewEntity)
        {
            this.initializer = initializer;
            this.allHouses = allHouses;
            this.isNewEntity = isNewEntity;
        }
        public void ApplyUpdatingMasterHouses()
        {
            if (!this.isNewEntity && initializer.EntityPM.ShipmentLevelCode == "C")
            {
                UpdateHousesRoutings();
                UpdateMasterHousesPackagesLCLContainerType();
                UpdateMasterHousesWithConcurrencyGuid();
                UpdateMasterNewConnectedHouses();
                UpdateMasterDisconnectedHouses();
            }
        }
        private void UpdateHousesRoutings()
        {
            if (allHouses.Count > 0)
            {
                foreach (Shipment item in allHouses)
                {
                    item.NextETA = initializer.EntityPOCO.NextETA;
                    item.NextETD = initializer.EntityPOCO.NextETD;
                    item.NextLeg = initializer.EntityPOCO.NextLeg;
                    item.NextLegCode = initializer.EntityPOCO.NextLegCode;

                    if (string.IsNullOrEmpty(item.AgentId))
                    {
                        item.AgentComputed = initializer.EntityPM.AgentId;
                    }

                    initializer.Repository.Update(item);
                }
                initializer.Repository.SubmitChanges();
            }
        }
        private void UpdateMasterHousesWithConcurrencyGuid()
        {
            if (initializer.ShipmentConsoleShipmentsChangeSet != null && (initializer.IsUpdatingHouses || initializer.IsUpdatingHousesFinalArrivalDate))
            {
                List<string> ids = initializer.ShipmentConsoleShipmentsChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Select(s => s.Id).ToList();
                if (ids.Count > 0)
                {
                    ShipmentQuery iShipmentQuery = new ShipmentQuery(initializer.Repository);
                    foreach (string id in ids)
                    {
                        iHousePM = iShipmentQuery.GetSinglePM(id, initializer.Tenant);
                        if (iHousePM != null)
                        {
                            MapMasterHouseFields();
                            MapMasterHouseFromPortFields();
                            MapMasterHouseToPortFields();
                            UpdateShipment();
                        }
                    }
                }
            }
        }
        private void MapMasterHouseFields()
        {
            iHousePM.IsCancelled = initializer.EntityPM.IsCancelled;
            iHousePM.CancelledDate = this.initializer.EntityPM.CancelledDate;
            iHousePM.IsOperationalClosed = this.initializer.EntityPM.IsOperationalClosed;
            iHousePM.OperationalClosedByUserId = this.initializer.EntityPM.OperationalClosedByUserId;
            iHousePM.OperationalCloseDate = this.initializer.EntityPM.OperationalCloseDate;
            iHousePM.FirstOperationalCloseDate = this.initializer.EntityPM.FirstOperationalCloseDate;
            iHousePM.IsAccountingClosed = this.initializer.EntityPM.IsAccountingClosed;
            iHousePM.AccountingCloseDate = this.initializer.EntityPM.AccountingCloseDate;
            iHousePM.FirstAccountingCloseDate = this.initializer.EntityPM.FirstAccountingCloseDate;
        }
        private void MapMasterHouseFromPortFields()
        {
            if (iHousePM.FromPortId != this.initializer.EntityPM.MainCarriageFromPortId)
            {
                iHousePM.FromPortId = this.initializer.EntityPM.MainCarriageFromPortId;

                if (iHousePM.PreCarriageFromPortId != null && iHousePM.PreCarriageToPortId != null)
                {
                    iHousePM.PreCarriageToPortId = iHousePM.FromPortId;
                }
            }
        }
        private void MapMasterHouseToPortFields()
        {
            if (iHousePM.ToPortId != this.initializer.EntityPM.MainCarriageFinalDestinationPortId)
            {
                iHousePM.ToPortId = this.initializer.EntityPM.MainCarriageFinalDestinationPortId;

                if (iHousePM.OnCarriageFromPortId != null && iHousePM.OnCarriageToPortId != null)
                {
                    iHousePM.OnCarriageFromPortId = iHousePM.ToPortId;
                }
            }
        }
        private void UpdateMasterNewConnectedHouses()
        {
            if (initializer.ConnectedHousesIds.Count > 0)
            {
                UpdateMasterHouses(initializer.ConnectedHousesIds);
            }
        }
        private void UpdateMasterDisconnectedHouses()
        {
            if (initializer.DeletedHousesIds.Count > 0)
            {
                UpdateMasterHouses(initializer.DeletedHousesIds);

                foreach (string myShipmentId in initializer.DeletedHousesIds)
                {
                    this.RunRegistryDateProcedure(myShipmentId);
                    this.RunFirstApprovalDateProcedure(myShipmentId);
                    if (initializer.EntityPM != null && !initializer.EntityPM.IsHybrid)
                    {
                        UpdateShipmentProfitClass.UpdateProfitFunction(myShipmentId, initializer.Tenant, false);
                    }
                }
            }
        }
        private void UpdateShipment(bool mapComposition = false)
        {
            ShipmentService iShipmentService = new ShipmentService(initializer.ShipmentContext, iHousePM, initializer.LoggedContactEmail);
            iShipmentService.Update(mapComposition);
        }
        private void UpdateMasterHouses(List<string> ids)
        {
            if (ids.Count > 0)
            {
                ShipmentQuery iShipmentQuery = new ShipmentQuery(initializer.Repository);

                foreach (string id in ids)
                {
                    iHousePM = iShipmentQuery.GetSinglePM(id, initializer.Tenant);

                    if (iHousePM != null)
                    {
                        UpdateShipment();
                    }
                }
            }
        }
        private void RunRegistryDateProcedure(string myShipmentId)
        {
            RunStoredProcedureClass.UpdateShipmentRegistryDate(myShipmentId, initializer.EntityPM.Tenant);
        }
        private void RunFirstApprovalDateProcedure(string myShipmentId)
        {
            RunStoredProcedureClass.UpdateShipmentFirstApprovalDate(myShipmentId, initializer.EntityPM.Tenant);
        }
        private void UpdateMasterHousesPackagesLCLContainerType()
        {
            if (this.initializer.EntityPM.IsGroupageHousesUpdated)
            {
                masterInsidePackages = new List<InsideShipmentPackagePM>();
                masterInsidePackages.AddRange(this.initializer.EntityPM.ShipmentPackages.SelectMany(n => n.InsideShipmentPackages.Where(a => a.OriginalShipmentPackageId != null).ToList()).ToList());
                List<string> housesPackagesIds = masterInsidePackages.Select(s => s.OriginalShipmentPackageId).ToList();
                List<string> housesShipmentsIds = initializer.Repository.GetShipmentsFromPackagesIds(housesPackagesIds, initializer.Tenant).Select(a => a.Id).ToList();
                ShipmentQuery iShipmentQuery = new ShipmentQuery(initializer.Repository);
                foreach (string houseId in housesShipmentsIds)
                {
                    iHousePM = iShipmentQuery.GetSinglePM(houseId, this.initializer.Tenant);
                    UpdateHousePackagesLCLCOntainerType();
                    UpdateShipment(true);
                }

                initializer.EntityPM.IsGroupageHousesUpdated = false;
            }
        }
        private void UpdateHousePackagesLCLCOntainerType()
        {
            foreach (ShipmentPackagePM package in iHousePM.ShipmentPackages)
            {
                var houseConnected = masterInsidePackages.Where(a => a.OriginalShipmentPackageId == package.Id).FirstOrDefault();
                if (houseConnected != null)
                {
                    ShipmentPackagePM masterPackage = this.initializer.EntityPM.ShipmentPackages.Where(a => a.Id == houseConnected.ShipmentPackageId).FirstOrDefault();
                    if (masterPackage.PackageTypeId != package.LCLContainerTypeId)
                    {
                        package.LCLContainerTypeId = masterPackage.PackageTypeId;
                        package.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
            }
        }
    }
}
