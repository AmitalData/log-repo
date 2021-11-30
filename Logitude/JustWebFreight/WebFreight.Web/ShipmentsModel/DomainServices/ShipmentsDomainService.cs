using System;
using System.Collections.Generic;

using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using WebFreight.Web.ReportsWebServices;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.DataContracts;
using System.Data.Entity.Core;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    // TODO: Create methods containing your application logic.
   //[RequiresAuthentication()]
    [EnableClientAccess()]
    public partial class ShipmentsDomainService : LogitudeDomainService
    {
        private IShipmentsContext objectContext;
        private ShipmentRepository shipmentRepository;
        private ShipmentQuery shipmentQuery;

        public ShipmentsDomainService()
        {

        }

        public ShipmentsDomainService(IShipmentsContext context)
        {

        }

        public void InsertShipmentPM(ShipmentPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Shipment", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
            }

            ShipmentService service = new ShipmentService(objectContext, entityPM, ServiceContext.User.Identity.Name);
            service.Create();

            //if (this.ChangeSet != null)
            //{
            //    this.ChangeSet.Associate(entityPM, service.entityPoco, MapShipmentPMToShipment);
            //}            
        }

        public void UpdateShipmentPM(ShipmentPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Shipment", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
            }

            #region OrderPackages
            List<ShipmentOrderPackagePM> shipmentOrderPackagesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentOrderPackages).Cast<ShipmentOrderPackagePM>().ToList();
            foreach (ShipmentOrderPackagePM itemPM in shipmentOrderPackagesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region Packages
            List<ShipmentPackagePM> shipmentPackagesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentPackages).Cast<ShipmentPackagePM>().ToList();
            foreach (ShipmentPackagePM itemPM in shipmentPackagesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;

                            itemPM.InsideShipmentPackagesChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.InsideShipmentPackages).Cast<InsideShipmentPackagePM>().ToList();
                            foreach (InsideShipmentPackagePM insideItemPM in itemPM.InsideShipmentPackagesChangeSet)
                            {
                                switch (ChangeSet.GetChangeOperation(insideItemPM))
                                {
                                    case ChangeOperation.Insert: { insideItemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                                    case ChangeOperation.Update: { insideItemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                                    case ChangeOperation.Delete: { insideItemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                                    default: { insideItemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                                }
                            }

                            itemPM.ShipmentPackageItemsChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.ShipmentPackageItems).Cast<ShipmentPackageItemPM>().ToList();
                            foreach (ShipmentPackageItemPM packageItem in itemPM.ShipmentPackageItemsChangeSet)
                            {
                                switch (ChangeSet.GetChangeOperation(packageItem))
                                {
                                    case ChangeOperation.Insert: { packageItem.ChangeSetOp = ChangeSetOperation.Insert; break; }
                                    case ChangeOperation.Update: { packageItem.ChangeSetOp = ChangeSetOperation.Update; break; }
                                    case ChangeOperation.Delete: { packageItem.ChangeSetOp = ChangeSetOperation.Delete; break; }
                                    default: { packageItem.ChangeSetOp = ChangeSetOperation.None; break; }
                                }
                            }
                            
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region Commodities
            List<ShipmentCommodityPM> shipmentCommoditiesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentCommodities).Cast<ShipmentCommodityPM>().ToList();
            foreach (ShipmentCommodityPM itemPM in shipmentCommoditiesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;

                            itemPM.CommodityPackages = ChangeSet.GetAssociatedChanges(itemPM, d => d.CommodityPackages).Cast<CommodityPackagePM>().ToList();
                            foreach (CommodityPackagePM insideItemPM in itemPM.CommodityPackages)
                            {
                                switch (ChangeSet.GetChangeOperation(insideItemPM))
                                {
                                    case ChangeOperation.Insert: { insideItemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                                    case ChangeOperation.Update: { insideItemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                                    case ChangeOperation.Delete: { insideItemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                                    default: { insideItemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                                }
                            }

                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region PickUps
            List<ShipmentPickUpPM> shipmentPickUpsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentPickUps).Cast<ShipmentPickUpPM>().ToList();
            foreach (ShipmentPickUpPM itemPM in shipmentPickUpsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;

                            itemPM.ShipmentPickUpPackagesChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.ShipmentPickUpDeliveryPackages).Cast<ShipmentPickUpDeliveryPackagePM>().ToList();
                            foreach (ShipmentPickUpDeliveryPackagePM insideItemPM in itemPM.ShipmentPickUpPackagesChangeSet)
                            {
                                switch (ChangeSet.GetChangeOperation(insideItemPM))
                                {
                                    case ChangeOperation.Insert: { insideItemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                                    case ChangeOperation.Update: { insideItemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                                    case ChangeOperation.Delete: { insideItemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                                    default: { insideItemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                                }
                            }

                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region Deliveries
            List<ShipmentDeliveryPM> shipmentDeliveriesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentDeliveries).Cast<ShipmentDeliveryPM>().ToList();
            foreach (ShipmentDeliveryPM itemPM in shipmentDeliveriesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;

                            itemPM.ShipmentDeliveryPackagesChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.ShipmentPickUpDeliveryPackages).Cast<ShipmentPickUpDeliveryPackagePM>().ToList();
                            foreach (ShipmentPickUpDeliveryPackagePM insideItemPM in itemPM.ShipmentDeliveryPackagesChangeSet)
                            {
                                switch (ChangeSet.GetChangeOperation(insideItemPM))
                                {
                                    case ChangeOperation.Insert: { insideItemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                                    case ChangeOperation.Update: { insideItemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                                    case ChangeOperation.Delete: { insideItemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                                    default: { insideItemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                                }
                            }

                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region Payables
            List<ShipmentPayablePM> shipmentPayablesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentPayables).Cast<ShipmentPayablePM>().ToList();
            foreach (ShipmentPayablePM itemPM in shipmentPayablesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;

                            itemPM.ChildShipmentPayablesChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.ChildShipmentPayables).Cast<ShipmentPayablePM>().ToList();
                            foreach (ShipmentPayablePM insideItemPM in itemPM.ChildShipmentPayablesChangeSet)
                            {
                                switch (ChangeSet.GetChangeOperation(insideItemPM))
                                {
                                    case ChangeOperation.Insert: { insideItemPM.ChildChangeOp = ChangeSetOperation.Insert; break; }
                                    case ChangeOperation.Update: { insideItemPM.ChildChangeOp = ChangeSetOperation.Update; break; }
                                    case ChangeOperation.Delete: { insideItemPM.ChildChangeOp = ChangeSetOperation.Delete; break; }
                                    default: { insideItemPM.ChildChangeOp = ChangeSetOperation.None; break; }
                                }
                            }

                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region Receivables
            List<ShipmentReceivablePM> shipmentReceivablesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentReceivables).Cast<ShipmentReceivablePM>().ToList();
            foreach (ShipmentReceivablePM itemPM in shipmentReceivablesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region AWBPrintOnlies
            List<ShipmentAWBPrintOnlyPM> shipmentAWBPrintOnliesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentAWBPrintOnlies).Cast<ShipmentAWBPrintOnlyPM>().ToList();
            foreach (ShipmentAWBPrintOnlyPM itemPM in shipmentAWBPrintOnliesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region ConsoleShipments
            List<ConsoleShipmentPM> shipmentConsoleShipmentsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentConsoleShipments).Cast<ConsoleShipmentPM>().ToList();
            foreach (ConsoleShipmentPM itemPM in shipmentConsoleShipmentsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region FollowUps
            List<ShipmentFollowUpPM> shipmentFollowUpsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.FollowUps).Cast<ShipmentFollowUpPM>().ToList();
            foreach (ShipmentFollowUpPM itemPM in shipmentFollowUpsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region CarrierStatuses
            List<ShipmentCarrierStatusPM> shipmentCarrierStatusesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentCarrierStatuses).Cast<ShipmentCarrierStatusPM>().ToList();
            foreach (ShipmentCarrierStatusPM itemPM in shipmentCarrierStatusesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region OCIs
            List<AWBOCIPM> aWBOCIPMChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.AWBOCIPMs).Cast<AWBOCIPM>().ToList();
            foreach (AWBOCIPM itemPM in aWBOCIPMChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region Assemblies
            List<ShipmentAssemblyPM> shipmentAssembliesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentAssemblies).Cast<ShipmentAssemblyPM>().ToList();
            foreach (ShipmentAssemblyPM itemPM in shipmentAssembliesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region ShipmentStoragePricings
            List<ShipmentStoragePricingPM> shipmentStoragePricingsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentStoragePricings).Cast<ShipmentStoragePricingPM>().ToList();
            foreach (ShipmentStoragePricingPM itemPM in shipmentStoragePricingsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region ShipmentProductItems
            List<ShipmentProductItemPM> shipmentProductItemsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentProductItems).Cast<ShipmentProductItemPM>().ToList();
            foreach (ShipmentProductItemPM itemPM in shipmentProductItemsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region ShipmentUnassignedFields
            List<ShipmentUnassignedFieldPM> shipmentUnassignedFieldsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ShipmentUnassignedFields).Cast<ShipmentUnassignedFieldPM>().ToList();
            foreach (ShipmentUnassignedFieldPM itemPM in shipmentUnassignedFieldsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            ShipmentService service = new ShipmentService(objectContext, entityPM, ServiceContext.User.Identity.Name);
            service.SetChangeSet(shipmentPackagesChangeSet, shipmentOrderPackagesChangeSet, shipmentPickUpsChangeSet, shipmentDeliveriesChangeSet, shipmentReceivablesChangeSet, shipmentPayablesChangeSet, shipmentFollowUpsChangeSet, shipmentAWBPrintOnliesChangeSet, shipmentConsoleShipmentsChangeSet, shipmentCarrierStatusesChangeSet, aWBOCIPMChangeSet, shipmentCommoditiesChangeSet, shipmentAssembliesChangeSet, shipmentStoragePricingsChangeSet, shipmentProductItemsChangeSet, shipmentUnassignedFieldsChangeSet);
            service.Update();

            //if (this.ChangeSet != null)
            //{
            //    this.ChangeSet.Associate(entityPM, service.entityPoco, MapShipmentPMToShipment);
            //}            
        }

        public void MapShipmentPMToShipment(ShipmentPM entityPM, Shipment entityPoco)
        {
            // Ayman: this code has been moved to shipment service
        }

        protected override bool PersistChangeSet()
        {            
            try
            {
                objectContext.SaveChanges();
            }

            catch (OptimisticConcurrencyException ex)
            {
                throw new Exception("Sorry you can't update this record right now it's being updated by another user");
            }

            return base.PersistChangeSet();
        }
    }
}



