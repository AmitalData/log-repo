using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentContainersEntityBehaviour : IServiceBehaviour
    {
        private ShipmentServiceInitializer initializer;
        private IShipmentsContext shipmentsContext;
        private ContainerService containerService;
        private ContainerQuery containerQuery;
        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.shipmentsContext = this.initializer.ShipmentContext;
            this.containerService = new ContainerService(this.shipmentsContext, this.initializer.Tenant);
            this.containerQuery = new ContainerQuery(this.initializer.Tenant);
            this.HandleBehaviour();
        }

        public void HandleBehaviour()
        {
            HandelContainers();
        }

        private void HandelContainers()
        {
            if (this.ShouldUpdateContainers())
            {
                this.HandelShipmentMasterDataFieldsChanges();
                this.HandelShipmentPackagesChangeSets();
            }
        }

        private void HandelShipmentMasterDataFieldsChanges()
        {
            if (CheckIfShipmentMasterDataFieldsUpdated()) {
                this.UpdateShipmentPackagesChangeSetOperation();
            }
        }

        private bool CheckIfShipmentMasterDataFieldsUpdated()
        {
            if (this.initializer.EntityMasterData == null)
                return false;
            if (this.initializer.EntityPM.MainCarriageCarrierId != this.initializer.EntityMasterData.MainCarriageCarrierId)
                return true;
            if (this.initializer.EntityPM.MainCarriageCarrierNumber != this.initializer.EntityMasterData.MainCarriageCarrierNumber)
                return true;
            if (this.initializer.EntityPM.Master != this.initializer.EntityMasterData.Master)
                return true;
            if (this.initializer.EntityPM.MainCarriageVesselId != this.initializer.EntityMasterData.MainCarriageVesselId)
                return true;
            if (this.initializer.EntityPM.MainCarriageETA != this.initializer.EntityMasterData.MainCarriageETA)
                return true;
            if (this.initializer.EntityPM.MainCarriageETD != this.initializer.EntityMasterData.MainCarriageETD)
                return true;
            if (this.initializer.EntityPM.MainCarriageATA != this.initializer.EntityMasterData.MainCarriageATA)
                return true;
            if (this.initializer.EntityPM.MainCarriageATD != this.initializer.EntityMasterData.MainCarriageATD)
                return true;
            return false;
        }
        private void UpdateShipmentPackagesChangeSetOperation()
        {
            if (initializer.ShipmentPackagesChangeSet != null)
            {
                foreach (ShipmentPackagePM itemPM in initializer.ShipmentPackagesChangeSet)
                {
                    if (itemPM.ChangeSetOp == ChangeSetOperation.None)
                    {
                        itemPM.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
            }
        }
        private void HandelShipmentPackagesChangeSets()
        {
            if (initializer.ShipmentPackagesChangeSet != null)
            {
                foreach (ShipmentPackagePM itemPM in initializer.ShipmentPackagesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateContainer(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                if (string.IsNullOrEmpty(itemPM.ContainerEntityId))
                                {
                                    this.CreateContainer(itemPM);
                                }
                                else
                                {
                                    this.UpdateContainer(itemPM);
                                }

                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteContainer(itemPM);
                                break;
                            }
                        default: { break; }
                    }
                }
            }
        }

        private bool ShouldUpdateContainers()
        {
            if (!FeatureToggleHelper.HasFeatureToggle("OIC", this.initializer.Tenant))
                return false;

            if (initializer.EntityPM.TransportModeId != "O" &&
               (initializer.EntityPM.ShipmentTypeId.ToLower() != "fcl" || initializer.EntityPM.ShipmentTypeId.ToLower() != "fcld"))
                return false;

            return true;
        }

        private void CreateContainer(ShipmentPackagePM shipmentPackage)
        {
            ContainerPM containerPM = new ContainerPM();
            MapContainerPMFields(containerPM, shipmentPackage, true);
            containerService.Create(containerPM);
            UpdateShipmentPackage(containerPM.Id, shipmentPackage.Id);            
        }

        private void UpdateContainer(ShipmentPackagePM shipmentPackage)
        {
            var container = CheckIfContainerExists(shipmentPackage);
            if (container != null)
            {
                MapContainerPMFields(container, shipmentPackage, false);
                containerService.Update(container);
            }
        }

        private void MapContainerPMFields(ContainerPM container, ShipmentPackagePM shipmentPackage, bool isNew)
        {
            if (isNew == true)
            {
                container.Tenant = this.initializer.Tenant;
                container.CreateDate = TenantServerConfigration.GetCurrentDateTime(this.initializer.Tenant);
                container.CreatedByUserId = this.initializer.LoggedContactId;
                container.UpdateDate = TenantServerConfigration.GetCurrentDateTime(this.initializer.Tenant);
                container.UpdatedByUserId = this.initializer.LoggedContactId;
                container.ShipmentPackagesId = shipmentPackage.Id;
            }
            container.MainCarriageCarrierId = this.initializer.EntityPM.MainCarriageCarrierId;
            container.MainCarriageCarrierNumber = this.initializer.EntityPM.MainCarriageCarrierNumber;
            container.MainCarriageVesselId = this.initializer.EntityPM.MainCarriageVesselId;
            container.MainCarriageATA = this.initializer.EntityPM.MainCarriageATA;
            container.MainCarriageATD = this.initializer.EntityPM.MainCarriageATD;
            container.MainCarriageETA = this.initializer.EntityPM.MainCarriageETA;
            container.MainCarriageETD = this.initializer.EntityPM.MainCarriageETD;
            container.Master = this.initializer.EntityPM.Master;
            container.ContainerNumber = shipmentPackage.ContainerNumber;
            container.ShipmentId = shipmentPackage.ShipmentId;
        }

        private void DeleteContainer(ShipmentPackagePM shipmentPackage)
        {
            var container = CheckIfContainerExists(shipmentPackage);
            if (container != null)
            {
                containerService.Delete(container);
            }
        }

        private ContainerPM CheckIfContainerExists(ShipmentPackagePM shipmentPackage)
        {
            var containerPM = containerQuery.GetContainerByShipmentPackagesId(shipmentPackage.Id, shipmentPackage.Tenant);
            return containerPM;
        }

        private void UpdateShipmentPackage(string containerId, string shipmentPackageId)
        {
            if (containerId != null)
            {
                ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(this.initializer.ShipmentContext);
                ShipmentPackage shipmentPackage = shipmentPackageRepository.GetSingleShipmentPackage(shipmentPackageId, this.initializer.Tenant);
                
                if (shipmentPackage != null)
                {
                    this.UpdateStandaloneShipmentPackage(containerId);
                    this.UpdatePickupDeliveryPackage(shipmentPackage, containerId);
                    shipmentPackage.ContainerEntityId = containerId;
                    shipmentPackageRepository.Update(shipmentPackage);
                    shipmentPackageRepository.SubmitChanges();
                }                
            }
        }
        private void UpdateStandaloneShipmentPackage(string containerEntityId)
        {
            Shipment standaloneShipment = initializer.Repository.GetSingleShipment(initializer.EntityPM.StandaloneShipmentId, initializer.Tenant);
            if(standaloneShipment != null)
            {
                List<ShipmentPackage> shipmentPackages = initializer.ShipmentPackageRepository.GetShipmentPackagesForShipmentTenant(standaloneShipment.Id, initializer.Tenant).ToList();
                if(shipmentPackages != null)
                {
                    ShipmentPackage shipmentPackage = shipmentPackages.FirstOrDefault();
                    if(shipmentPackage != null)
                    {
                        shipmentPackage.ContainerEntityId = containerEntityId;
                        initializer.ShipmentPackageRepository.Update(shipmentPackage);
                    }
                }
            }
        }

        private void UpdatePickupDeliveryPackage(ShipmentPackage shipmentPackage, string containerId)
        {
            ShipmentPickUpDeliveryPackageRepository shipmentPickUpDeliveryPackageRepository = new ShipmentPickUpDeliveryPackageRepository(this.initializer.ShipmentContext);
            IQueryable<ShipmentPickUpDeliveryPackage> shipmentPickupDeliveryPackages = this.GetPickupDeliveryPackagesForThisShipment(shipmentPackage.ShipmentId, shipmentPickUpDeliveryPackageRepository);

            ShipmentPickUpDeliveryPackage pickUpDeliveryPackage = shipmentPickupDeliveryPackages.Where(d => d.ContainerNumber == shipmentPackage.ContainerNumber).FirstOrDefault();
            if (pickUpDeliveryPackage != null)
            {
                pickUpDeliveryPackage.ContainerEntityId = containerId;
                shipmentPickUpDeliveryPackageRepository.Update(pickUpDeliveryPackage);
            }            
        }
        private IQueryable<ShipmentPickUpDeliveryPackage> GetPickupDeliveryPackagesForThisShipment(string shipmentId, ShipmentPickUpDeliveryPackageRepository shipmentPickUpDeliveryPackageRepository)
        {
            IQueryable<ShipmentPickUpDeliveryPackage> packages = null;
            ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(this.initializer.ShipmentContext);
            List<ShipmentPickUpDelivery> pickUpDeliveries = shipmentPickUpDeliveryRepository.GetShipmentPickUpDeliveryForShipment(shipmentId, this.initializer.Tenant);

            if (pickUpDeliveries != null)
            {
                List<string> pickupDeliveryIds = pickUpDeliveries.Select(d => d.Id).ToList();
                if (pickupDeliveryIds != null)
                {
                    packages = shipmentPickUpDeliveryPackageRepository.GetPickUpDeliveryPackagesByIdsList(pickupDeliveryIds, this.initializer.Tenant);
                }
            }

            return packages;
        }
    }
}
