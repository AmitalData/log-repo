using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
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
        private ContainerRepository containerRepository;
        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.shipmentsContext = this.initializer.ShipmentContext;
            this.containerService = new ContainerService(this.shipmentsContext, this.initializer.Tenant);
            this.containerRepository = new ContainerRepository(this.shipmentsContext);
            this.containerQuery = new ContainerQuery(containerRepository);
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
                this.SendAutomaticallyOceanOnsightsRequest();
                this.HandelShipmentPickUpsChangeSets();
                this.HandelShipmentDeliveriesChangeSets();
                this.HandelDeletedShipmentPickUpsChangeSets();
                this.HandelDeletedShipmentDeliveriesChangeSets();
                this.HandelCancelationShipment();
            }
        }
        private void HandelCancelationShipment()
        {
            if (this.initializer.EntityPM.IsCancelled != this.initializer.EntityPOCO.IsCancelled)
            {
                this.UpdateShipmentContainers(this.initializer.EntityPM.IsCancelled);
            }
        }

        private void UpdateShipmentContainers(bool isCancelled)
        {
            List<ContainerPM> containers = GetShipmentContainers(this.initializer.EntityPM);
            foreach (ContainerPM containerPM in containers)
            {
                this.MapContainerCancelledFields(containerPM, isCancelled);
            }
        }

        private void MapContainerCancelledFields(ContainerPM containerPM, bool isCancelled)
        {
            containerPM.IsCancelled = isCancelled;
            containerPM.CancelledDate = null;
            if (isCancelled == true)
            {
                containerPM.CancelledDate = TenantServerConfigration.GetCurrentDateTime(this.initializer.Tenant);
            }

            containerService.Update(containerPM);
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
            if (this.initializer.EntityPM.PreCarriageFromPortId != this.initializer.EntityMasterData.PreCarriageFromPortId)
                return true;
            if (this.initializer.EntityPM.PreCarriageToPortId != this.initializer.EntityMasterData.PreCarriageToPortId)
                return true;
            if (this.initializer.EntityPM.MainCarriageToPortId != this.initializer.EntityMasterData.MainCarriageToPortId)
                return true;
            if (this.initializer.EntityPM.MainCarriageFromPortId != this.initializer.EntityMasterData.MainCarriageFromPortId)
                return true;
            if (this.initializer.EntityPM.Transshipment1ToPortId != this.initializer.EntityMasterData.Transshipment1ToPortId)
                return true;
            if (this.initializer.EntityPM.Transshipment1FromPortId != this.initializer.EntityMasterData.Transshipment1FromPortId)
                return true;
            if (this.initializer.EntityPM.Transshipment2ToPortId != this.initializer.EntityMasterData.Transshipment2ToPortId)
                return true;
            if (this.initializer.EntityPM.Transshipment2FromPortId != this.initializer.EntityMasterData.Transshipment2FromPortId)
                return true;
            if (this.initializer.EntityPM.Transshipment3ToPortId != this.initializer.EntityMasterData.Transshipment3ToPortId)
                return true;
            if (this.initializer.EntityPM.Transshipment3FromPortId != this.initializer.EntityMasterData.Transshipment3FromPortId)
                return true;
            if (this.initializer.EntityPM.OnCarriageToPortId != this.initializer.EntityMasterData.OnCarriageToPortId)
                return true;
            if (this.initializer.EntityPM.OnCarriageFromPortId != this.initializer.EntityMasterData.OnCarriageFromPortId)
                return true;
            if (this.initializer.EntityPM.StatusId != this.initializer.EntityMasterData.StatusId)
                return true;
            if (this.initializer.EntityPM.CustomsClearanceDate != this.initializer.EntityPOCO.CustomsClearanceDate)
                return true;
            if (this.initializer.EntityPM.FreightRelease != this.initializer.EntityPOCO.FreightRelease)
                return true;

            return false;
        }

        private void HandelShipmentPickUpsChangeSets()
        {
            ShipmentPickUpPM updatedShipmentPickUp = this.GetUpdatedShipmentPickUpPMByContainerEntityId();
            List<ContainerPM> containersToBeUpdated = this.GetPickUpContainersToBeUpdatedByContainerEntityId(updatedShipmentPickUp);
            List<ContainerPM> containersToBeUpdatedFromDeleteedPackages = this.GetPickUpContainersToBeDeletedByContainerEntityId(updatedShipmentPickUp);

            if (updatedShipmentPickUp != null && containersToBeUpdated.Count > 0)
                this.UpdateContainerFieldsFromPickUp(updatedShipmentPickUp, containersToBeUpdated, false);

            if (updatedShipmentPickUp != null && containersToBeUpdatedFromDeleteedPackages.Count > 0)
                this.UpdateContainerFieldsFromPickUp(updatedShipmentPickUp, containersToBeUpdatedFromDeleteedPackages, true);
        }

        private void HandelShipmentDeliveriesChangeSets()
        {
            ShipmentDeliveryPM updatedShipmentDeliveryPM = this.GetUpdatedShipmentDeliveryPMByContainerEntityId();
            List<ContainerPM> containersToBeUpdated = this.GetDeliveryContainersToBeUpdatedByContainerEntityId(updatedShipmentDeliveryPM);
            List<ContainerPM> containersToBeUpdatedFromDeleteedPackages = this.GetDeliveryContainersToBeDeletedByContainerEntityId(updatedShipmentDeliveryPM);

            if (updatedShipmentDeliveryPM != null && containersToBeUpdated.Count > 0)
                this.UpdateContainerFieldsFromDelivery(updatedShipmentDeliveryPM, containersToBeUpdated, false);

            if (updatedShipmentDeliveryPM != null && containersToBeUpdatedFromDeleteedPackages.Count > 0)
                this.UpdateContainerFieldsFromDelivery(updatedShipmentDeliveryPM, containersToBeUpdatedFromDeleteedPackages, true);
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
                        case ChangeSetOperation.None:
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
            if (!IsOceanInsightFeatureToggleExistInTenant( this.initializer.Tenant))
                return false;

            if (initializer.EntityPM.TransportModeId != "O")
                return false;

            if (initializer.EntityPM.ShipmentTypeId.ToLower() != "fcl" && initializer.EntityPM.ShipmentTypeId.ToLower() != "fcld")
                return false;

            return true;
        }


        private bool IsOceanInsightFeatureToggleExistInTenant(int tenant)
        {
            string ocaenInsightFeatureToggleCode = "OIC";
            bool isOceanInsightFeatureToggleExist = false;

            isOceanInsightFeatureToggleExist = (from a in initializer.IInfrastructureContext.FeatureToggles
                                                where a.ToggleCode == ocaenInsightFeatureToggleCode
                                                && (a.TenantNumber == tenant || (tenant >= a.FromTenantNumber && tenant <= a.ToTenantNumber))
                                                && !a.Inactive
                                                && a.Tenant == 0
                                                select a).Any();
            return isOceanInsightFeatureToggleExist;
        }

        private void CreateContainer(ShipmentPackagePM shipmentPackage)
        {
            var container = containerQuery.GetCancelledContainerByShipmentId(initializer.EntityPM.Id, shipmentPackage.ContainerNumber, initializer.EntityPM.Tenant);
            if (container != null)
            {
                this.ActivateCancelledContainer(container, shipmentPackage);
            }
            else
            {
                this.CreateNewContainer(shipmentPackage);
            }
        }
        private void ActivateCancelledContainer(ContainerPM container, ShipmentPackagePM shipmentPackage)
        {
            container.ShipmentPackagesId = shipmentPackage.Id;
            container.IsCancelled = false;
            container.CancelledDate = null;
            MapContainerPMFields(container, shipmentPackage, false);
            containerService.Update(container);
            UpdateShipmentPackage(container.Id, shipmentPackage.Id);
        }
        private void CreateNewContainer(ShipmentPackagePM shipmentPackage)
        {
            if (string.IsNullOrEmpty(shipmentPackage.ContainerEntityId))
            {
                ContainerPM containerPM = new ContainerPM();
                MapContainerPMFields(containerPM, shipmentPackage, true);
                containerService.Create(containerPM);
                UpdateShipmentPackage(containerPM.Id, shipmentPackage.Id);
            }
            else
            {
                UpdateContainer(shipmentPackage);
            }
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
                container.ShipmentPackagesId = shipmentPackage.Id;
                container.ShipmentCreateDate = this.initializer.EntityPM?.CreateDateTime;
            }
            container.UpdateDate = TenantServerConfigration.GetCurrentDateTime(this.initializer.Tenant);
            container.UpdatedByUserId = this.initializer.LoggedContactId;
            container.MainCarriageCarrierId = this.initializer.EntityPM.MainCarriageCarrierId;
            container.MainCarriageCarrierNumber = this.initializer.EntityPM.MainCarriageCarrierNumber;
            container.MainCarriageVesselId = this.initializer.EntityPM.MainCarriageVesselId;
            container.Master = this.initializer.EntityPM.Master;
            container.ContainerNumber = shipmentPackage.ContainerNumber;
            container.ShipmentId = shipmentPackage.ShipmentId;
            container.ShipmentPreCarriageFromId = this.initializer.EntityPM?.PreCarriageFromPortId;
            container.ShipmentPreCarriageToId = this.initializer.EntityPM?.PreCarriageToPortId;
            container.ShipmentMainCarriageFromId = this.initializer.EntityPM?.MainCarriageFromPortId;
            container.ShipmentMainCarriageToId = this.initializer.EntityPM?.MainCarriageFinalDestinationPortId;
            container.ShipmentTransshipment1FromId = this.initializer.EntityPM?.Transshipment1FromPortId;
            container.ShipmentTransshipment1ToId = this.initializer.EntityPM?.Transshipment1ToPortId;
            container.ShipmentTransshipment2FromId = this.initializer.EntityPM?.Transshipment2FromPortId;
            container.ShipmentTransshipment2ToId = this.initializer.EntityPM?.Transshipment2ToPortId;
            container.ShipmentTransshipment3FromId = this.initializer.EntityPM?.Transshipment3FromPortId;
            container.ShipmentTransshipment3ToId = this.initializer.EntityPM?.Transshipment3ToPortId;
            container.ShipmentOnCarriageFromId = this.initializer.EntityPM?.OnCarriageFromPortId;
            container.ShipmentOnCarriageToId = this.initializer.EntityPM?.OnCarriageToPortId;
            container.ShipmentStatusId = this.initializer.EntityPM?.StatusId;
            container.CustomsReleaseDate = this.initializer.EntityPM?.CustomsClearanceDate;
            container.CarrierReleaseDate = this.initializer.EntityPM?.FreightRelease;
            container.ShipmentPreCarriageETA = this.initializer.EntityPM?.PreCarriageETA;
            container.ShipmentPreCarriageETD = this.initializer.EntityPM?.PreCarriageETD;
            container.ShipmentPreCarriageATA = this.initializer.EntityPM?.PreCarriageATA;
            container.ShipmentPreCarriageATD = this.initializer.EntityPM?.PreCarriageATD;
            container.ShipmentMainCarriageETA = this.initializer.EntityPM?.MainCarriageETA;
            container.ShipmentMainCarriageETD = this.initializer.EntityPM?.MainCarriageETD;
            container.ShipmentMainCarriageATA = this.initializer.EntityPM?.MainCarriageATA;
            container.ShipmentMainCarriageATD = this.initializer.EntityPM?.MainCarriageATD;
            container.ShipmentTransshipment1ETA = this.initializer.EntityPM?.Transshipment1ETA;
            container.ShipmentTransshipment1ETD = this.initializer.EntityPM?.Transshipment1ETD;
            container.ShipmentTransshipment1ATA = this.initializer.EntityPM?.Transshipment1ATA;
            container.ShipmentTransshipment1ATD = this.initializer.EntityPM?.Transshipment1ATD;
            container.ShipmentTransshipment2ETA = this.initializer.EntityPM?.Transshipment2ETA;
            container.ShipmentTransshipment2ETD = this.initializer.EntityPM?.Transshipment2ETD;
            container.ShipmentTransshipment2ATA = this.initializer.EntityPM?.Transshipment2ATA;
            container.ShipmentTransshipment2ATD = this.initializer.EntityPM?.Transshipment2ATD;
            container.ShipmentTransshipment3ETA = this.initializer.EntityPM?.Transshipment3ETA;
            container.ShipmentTransshipment3ETD = this.initializer.EntityPM?.Transshipment3ETD;
            container.ShipmentTransshipment3ATA = this.initializer.EntityPM?.Transshipment3ATA;
            container.ShipmentTransshipment3ATD = this.initializer.EntityPM?.Transshipment3ATD;
            container.ShipmentOnCarriageETA = this.initializer.EntityPM?.OnCarriageETA;
            container.ShipmentOnCarriageETD = this.initializer.EntityPM?.OnCarriageETD;
            container.ShipmentOnCarriageATA = this.initializer.EntityPM?.OnCarriageATA;
            container.ShipmentOnCarriageATD = this.initializer.EntityPM?.OnCarriageATD;
            container.ShipmentOriginAgentId = this.initializer.EntityPM?.AgentId;
            container.ShipmentDestinationAgentId = this.initializer.EntityPM?.FreightForwarderId;
            container.ShipmentNumber = this.initializer.EntityPM?.ShipmentNumber;
            container.ContainersCount = this.initializer.EntityPM?.NumberOfContainers;
            container.HandlerId = this.initializer.EntityPM?.HandlerUserId;
            container.CustomerId = this.initializer.EntityPM?.CustomerId;
            container.OPClosed = this.initializer.EntityPM != null ? this.initializer.EntityPM.IsOperationalClosed : false;
            container.ShipmentTypeId = this.initializer.EntityPM?.ShipmentTypeId;
            container.PODReceivedOnDate = this.initializer.EntityPM?.PODReceivedDate;

            this.MapContainerFieldsFromShipmentPickup(container);
            this.MapContainerFieldsFromShipmentDelivery(container);
        }

        private void SendAutomaticallyOceanOnsightsRequest()
        {
            if (IsSendAutomaticallyOceanOnsightsRequestByContainer())
            {
                var allUpdatedContainers = initializer.ShipmentPackagesChangeSet.Where(a => a.ContainerNumber != null);
                foreach (var container in allUpdatedContainers)
                {
                    this.SendAutomaticallyOceanOnsightsRequestByContainer(container.ContainerEntityId);
                }
            }
        }

        private bool IsSendAutomaticallyOceanOnsightsRequestByContainer()
        {
            if (initializer.ShipmentPackagesChangeSet != null)
            {
                var isContainerUpdated = initializer.ShipmentPackagesChangeSet
                      .Where(a => a.ChangeSetOp == ChangeSetOperation.Update || a.ChangeSetOp == ChangeSetOperation.Insert)
                      .Any(a => a.ContainerNumber != null);

                if (string.IsNullOrEmpty(this.initializer.EntityPM.Master) && isContainerUpdated)
                {
                    return true;
                }

                if(!string.IsNullOrEmpty(this.initializer.EntityPM.Master) && !this.initializer.IsFirstFourDigitsOfMasterNumberAreLetters())
                {
                    return true;
                }
            }

            return false;
        }
        private void SendAutomaticallyOceanOnsightsRequestByContainer(string containerId)
        {
            if (FeatureToggleHelper.HasFeatureToggle("AOI", this.initializer.Tenant))
            { 
                // Container 
                ContainerStatusesHelper myHelper = new ContainerStatusesHelper(this.initializer.EntityPM.Id, containerId, true, this.initializer.Tenant, this.initializer.ShipmentContext);
                if (myHelper.Validate() && myHelper.IsLogitudeOceanInsightsRequestExistForConatiner())
                {
                    myHelper.SendContainerStatusRequest();
                }
            }
        }

        private void DeleteContainer(ShipmentPackagePM shipmentPackage)
        {
            var container = CheckIfContainerExists(shipmentPackage);
            if (container != null)
            {
                container.IsCancelled = true;
                container.ShipmentPackagesId = null;
                container.CancelledDate = TenantServerConfigration.GetCurrentDateTime(this.initializer.Tenant);
                containerService.Update(container);
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
                    initializer.ShipmentPackagesChangeSet.Where(a=>a.Id == shipmentPackage.Id).FirstOrDefault().ContainerEntityId = containerId;
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

        private void MapContainerFieldsFromShipmentPickup(ContainerPM entityPM)
        {
            ShipmentPickUpPM shipmentPickUpPM = GetShipmentPickUpPMByContainerEntityId(entityPM);
            if (shipmentPickUpPM == null && (!string.IsNullOrEmpty(entityPM.ShipmentFirstPickupFrom)
                || !string.IsNullOrEmpty(entityPM.ShipmentFirstPickupTo)))
            {
                entityPM.ShipmentFirstPickupTo = null;
                entityPM.ShipmentFirstPickupFrom = null;
                entityPM.ShipmentPickupETA = null;
                entityPM.ShipmentPickupETD = null;
                entityPM.ShipmentPickupATA = null;
                entityPM.ShipmentPickupATD = null;
                return;
            }

            if (shipmentPickUpPM == null)
            {
                return;
            }

            entityPM.ShipmentFirstPickupFrom = this.GetFirstPickupFromAddress(shipmentPickUpPM);
            entityPM.ShipmentFirstPickupTo = this.GetFirstPickupToAddress(shipmentPickUpPM);
            entityPM.ShipmentPickupETA = shipmentPickUpPM?.ETA;
            entityPM.ShipmentPickupETD = shipmentPickUpPM?.ETD;
            entityPM.ShipmentPickupATA = shipmentPickUpPM?.ATA;
            entityPM.ShipmentPickupATD = shipmentPickUpPM?.ATD;
        }

        private void MapContainerFieldsFromShipmentDelivery(ContainerPM entityPM)
        {
            ShipmentDeliveryPM shipmentDeliveryPM = GetShipmentDeliveryPMByContainerEntityId(entityPM);
            if (shipmentDeliveryPM == null && (!string.IsNullOrEmpty(entityPM.ShipmentLastDeliveryFrom)
                || !string.IsNullOrEmpty(entityPM.ShipmentLastDeliveryTo)))
            {
                entityPM.ShipmentLastDeliveryFrom = null;
                entityPM.ShipmentLastDeliveryTo = null;
                entityPM.ShipmentDeliveryETA = null;
                entityPM.ShipmentDeliveryETD = null;
                entityPM.ShipmentDeliveryATA = null;
                entityPM.ShipmentDeliveryATD = null;
                return;
            }
            
            if (shipmentDeliveryPM == null)
            {
                return;
            }

            entityPM.ShipmentLastDeliveryFrom = GetLastDeliveryFromAddress(shipmentDeliveryPM);
            entityPM.ShipmentLastDeliveryTo = GetLastDeliveryToAddress(shipmentDeliveryPM);
            entityPM.ShipmentDeliveryETA = shipmentDeliveryPM?.ETA;
            entityPM.ShipmentDeliveryETD = shipmentDeliveryPM?.ETD;
            entityPM.ShipmentDeliveryATA = shipmentDeliveryPM?.ATA;
            entityPM.ShipmentDeliveryATD = shipmentDeliveryPM?.ATD;
        }

        private ShipmentPickUpPM GetShipmentPickUpPMByContainerEntityId(ContainerPM entityPM)
        {
            foreach (ShipmentPickUpPM shipmentPickUpPM in initializer.EntityPM.ShipmentPickUps)
            {
                if (IsPickUpHaveContainerEntityId(shipmentPickUpPM, entityPM.Id))
                {
                    return shipmentPickUpPM;
                }
            }
            return null;
        }

        private bool IsPickUpHaveContainerEntityId(ShipmentPickUpPM shipmentPickUpPM, string containerId)
        {
            if (shipmentPickUpPM.ShipmentPickUpDeliveryPackages != null)
            {
                ShipmentPickUpDeliveryPackagePM shipmentPickUpDeliveryPackagePM = shipmentPickUpPM.ShipmentPickUpDeliveryPackages.
                    Find(a => !string.IsNullOrEmpty(a.ContainerEntityId) && containerId == a.ContainerEntityId);

                if (shipmentPickUpDeliveryPackagePM != null)
                    return true;
            }
            return false;
        }

        private ShipmentDeliveryPM GetShipmentDeliveryPMByContainerEntityId(ContainerPM entityPM)
        {
            foreach (ShipmentDeliveryPM shipmentDeliveryPM in initializer.EntityPM.ShipmentDeliveries)
            {
                if (IsDeliveryHaveContainerEntityId(shipmentDeliveryPM, entityPM.Id))
                {
                    return shipmentDeliveryPM;
                }
            }
            return null;
        }

        private bool IsDeliveryHaveContainerEntityId(ShipmentDeliveryPM shipmentDeliveryPM,string containerId)
        {
            if (shipmentDeliveryPM.ShipmentPickUpDeliveryPackages != null)
            {
                ShipmentPickUpDeliveryPackagePM shipmentPickUpDeliveryPackagePM = shipmentDeliveryPM.ShipmentPickUpDeliveryPackages.
                    Find(a => !string.IsNullOrEmpty(a.ContainerEntityId) && containerId == a.ContainerEntityId);
                if (shipmentPickUpDeliveryPackagePM != null)
                    return true;
            }
            return false;
        }
        

        private string GetFirstPickupFromAddress(ShipmentPickUpPM shipmentPickUpPM)
        {
            if(shipmentPickUpPM.PickUpDeliveryFromTypeCode == "PART")
            {
                return GetPartnerCardAddress(shipmentPickUpPM.FromPartnerCardId);
            }
            else if (shipmentPickUpPM.PickUpDeliveryFromTypeCode == "PORT")
            {
                return GetPortCombinedCode(shipmentPickUpPM.FromPortId);
            }
            else if (shipmentPickUpPM.PickUpDeliveryFromTypeCode == "CASL")
            {
                return shipmentPickUpPM.FromAddressCity + ", " + shipmentPickUpPM.FromAddressCountryName;
            }
            return "";
        }

        private string GetFirstPickupToAddress(ShipmentPickUpPM shipmentPickUpPM)
        {
            if (shipmentPickUpPM.PickUpDeliveryToTypeCode == "PART")
            {
                return GetPartnerCardAddress(shipmentPickUpPM.ToPartnerCardId);
            }
            else if (shipmentPickUpPM.PickUpDeliveryToTypeCode == "PORT")
            {
                return GetPortCombinedCode(shipmentPickUpPM.ToPortId);
            }
            else if (shipmentPickUpPM.PickUpDeliveryToTypeCode == "CASL")
            {
                return shipmentPickUpPM.ToAddressCity + ", " + shipmentPickUpPM.ToAddressCountryName;
            }
            return "";
        }

        private string GetLastDeliveryFromAddress(ShipmentDeliveryPM shipmentDeliveryPM)
        {
            if (shipmentDeliveryPM.PickUpDeliveryFromTypeCode == "PART")
            {
                return GetPartnerCardAddress(shipmentDeliveryPM.FromPartnerCardId);
            }
            else if (shipmentDeliveryPM.PickUpDeliveryFromTypeCode == "PORT")
            {
                return GetPortCombinedCode(shipmentDeliveryPM.FromPortId);
            }
            else if (shipmentDeliveryPM.PickUpDeliveryFromTypeCode == "CASL")
            {
                return shipmentDeliveryPM.FromAddressCity + ", " + shipmentDeliveryPM.FromAddressCountryName;
            }
            return "";
        }

        private string GetLastDeliveryToAddress(ShipmentDeliveryPM shipmentDeliveryPM)
        {
            if (shipmentDeliveryPM.PickUpDeliveryToTypeCode == "PART")
            {
                return GetPartnerCardAddress(shipmentDeliveryPM.ToPartnerCardId);
            }
            else if (shipmentDeliveryPM.PickUpDeliveryToTypeCode == "PORT")
            {
                return GetPortCombinedCode(shipmentDeliveryPM.ToPortId);
            }
            else if (shipmentDeliveryPM.PickUpDeliveryToTypeCode == "CASL")
            {
                return shipmentDeliveryPM.ToAddressCity + ", " + shipmentDeliveryPM.ToAddressCountryName;
            }
            return "";
        }

        private string GetPartnerCardAddress(string partnerId)
        {
            Card partner = initializer.CardRepository.GetSingleCard(partnerId, initializer.Tenant);
            if (partner == null)
                return "";

            return partner.EnglishName + ", " + partner.CityName + ", "+partner.CountryName;
        }

        private string GetPortCombinedCode(string portId)
        {
            Port port = initializer.PortRepository.GetSinglePort(portId, initializer.Tenant);
            if (port == null)
                return "";

            return port.CombinedCode;
        }

        private ShipmentPickUpPM GetUpdatedShipmentPickUpPMByContainerEntityId()
        {
            ShipmentPickUpPM shipmentPickUpPM = initializer.EntityPM.ShipmentPickUps.Find(d => d.ChangeSetOp == ChangeSetOperation.Update
            && d.ShipmentPickUpDeliveryPackages != null && (d.ShipmentPickUpDeliveryPackages.Any(a => !string.IsNullOrEmpty(a.ContainerEntityId))
            ));
            return shipmentPickUpPM;
        }

        private ShipmentDeliveryPM GetUpdatedShipmentDeliveryPMByContainerEntityId()
        {
            ShipmentDeliveryPM shipmentDeliveryPM = initializer.EntityPM.ShipmentDeliveries.Find(d => d.ChangeSetOp == ChangeSetOperation.Update
            && d.ShipmentPickUpDeliveryPackages != null && (d.ShipmentPickUpDeliveryPackages.Any(a => !string.IsNullOrEmpty(a.ContainerEntityId))
            ));
            return shipmentDeliveryPM;
        }


        private List<ContainerPM> GetDeliveryContainersToBeUpdatedByContainerEntityId(ShipmentDeliveryPM shipmentDeliveryPM)
        {         
            List<ContainerPM> containerPMs = new List<ContainerPM>(); 
            if (shipmentDeliveryPM == null)
            {
                return containerPMs;
            }
            foreach (ShipmentPickUpDeliveryPackagePM shipmentPickUpDeliveryPackagesPM in shipmentDeliveryPM.ShipmentPickUpDeliveryPackages)
            {
                if (!string.IsNullOrEmpty(shipmentPickUpDeliveryPackagesPM.ContainerEntityId) && shipmentPickUpDeliveryPackagesPM.ChangeSetOp != ChangeSetOperation.Delete)
                {
                    containerPMs.Add(this.containerQuery.GetSinglePM(shipmentPickUpDeliveryPackagesPM.ContainerEntityId, initializer.Tenant));
                }
            }
            return containerPMs;
        }

        private List<ContainerPM> GetDeliveryContainersToBeDeletedByContainerEntityId(ShipmentDeliveryPM shipmentDeliveryPM)
        {
            List<ContainerPM> containerPMs = new List<ContainerPM>();
            if (shipmentDeliveryPM == null)
            {
                return containerPMs;
            }
            foreach (ShipmentPickUpDeliveryPackagePM shipmentPickUpDeliveryPackagesPM in shipmentDeliveryPM.ShipmentPickUpDeliveryPackages)
            {
                if (!string.IsNullOrEmpty(shipmentPickUpDeliveryPackagesPM.ContainerEntityId) && shipmentPickUpDeliveryPackagesPM.ChangeSetOp == ChangeSetOperation.Delete)
                {
                    containerPMs.Add(this.containerQuery.GetSinglePM(shipmentPickUpDeliveryPackagesPM.ContainerEntityId, initializer.Tenant));
                }
            }
            return containerPMs;
        }

        private List<ContainerPM> GetPickUpContainersToBeDeletedByContainerEntityId(ShipmentPickUpPM shipmentPickUpPM)
        {
            List<ContainerPM> containerPMs = new List<ContainerPM>();
            if (shipmentPickUpPM == null)
            {
                return containerPMs;
            }
            foreach (ShipmentPickUpDeliveryPackagePM shipmentPickUpDeliveryPackagesPM in shipmentPickUpPM.ShipmentPickUpDeliveryPackages)
            {
                if (!string.IsNullOrEmpty(shipmentPickUpDeliveryPackagesPM.ContainerEntityId) && shipmentPickUpDeliveryPackagesPM.ChangeSetOp == ChangeSetOperation.Delete)
                {
                    containerPMs.Add(this.containerQuery.GetSinglePM(shipmentPickUpDeliveryPackagesPM.ContainerEntityId, initializer.Tenant));
                }
            }
            return containerPMs;
        }

        private List<ContainerPM> GetPickUpContainersToBeUpdatedByContainerEntityId(ShipmentPickUpPM shipmentPickUpPM)
        {
            List<ContainerPM> containerPMs = new List<ContainerPM>();
            if(shipmentPickUpPM == null)
            {
                return containerPMs;
            }
            foreach (ShipmentPickUpDeliveryPackagePM shipmentPickUpDeliveryPackagePM in shipmentPickUpPM.ShipmentPickUpDeliveryPackages)
            {
                if (!string.IsNullOrEmpty(shipmentPickUpDeliveryPackagePM.ContainerEntityId) && shipmentPickUpDeliveryPackagePM.ChangeSetOp != ChangeSetOperation.Delete)
                {
                    containerPMs.Add(containerQuery.GetSinglePM(shipmentPickUpDeliveryPackagePM.ContainerEntityId, initializer.Tenant));
                }
            }
            return containerPMs;
        }

        private void UpdateContainerFieldsFromPickUp(ShipmentPickUpPM updatedShipmentPickUp, List<ContainerPM> containerPMs, bool isPackageDeleted)
        {
            foreach (ContainerPM containerPM in containerPMs)
            {
                this.UpdateContainerFieldsFromPickUpFields(containerPM,updatedShipmentPickUp, isPackageDeleted);
            }
        }

        private void UpdateContainerFieldsFromDelivery(ShipmentDeliveryPM updatedShipmentDeliveryPM, List<ContainerPM> containerPMs ,bool isPackageDeleted)
        {
            foreach (ContainerPM containerPM in containerPMs)
            {
                this.UpdateContainerFieldsFromDeliveryFields(containerPM, updatedShipmentDeliveryPM, isPackageDeleted);
            }
        }

        private void UpdateContainerFieldsFromPickUpFields(ContainerPM containerPM, ShipmentPickUpPM updatedShipmentPickUp, bool isPackageDeleted)
        {
            if (!IsContainerUpdatedBefore(containerPM))
            {
                containerPM.ShipmentFirstPickupFrom = isPackageDeleted ? null : this.GetFirstPickupFromAddress(updatedShipmentPickUp);
                containerPM.ShipmentFirstPickupTo = isPackageDeleted ? null : this.GetFirstPickupToAddress(updatedShipmentPickUp);
                containerPM.ShipmentPickupETA = isPackageDeleted ? null : updatedShipmentPickUp?.ETA;
                containerPM.ShipmentPickupETD = isPackageDeleted ? null : updatedShipmentPickUp?.ETD;
                containerPM.ShipmentPickupATA = isPackageDeleted ? null : updatedShipmentPickUp?.ATA;
                containerPM.ShipmentPickupATD = isPackageDeleted ? null : updatedShipmentPickUp?.ATD;

                containerService.Update(containerPM);
            }
        }

        private void UpdateContainerFieldsFromDeliveryFields(ContainerPM containerPM, ShipmentDeliveryPM updatedShipmentDeliveryPM, bool isPackageDeleted)
        {
            if (!IsContainerUpdatedBefore(containerPM))
            {
                containerPM.ShipmentLastDeliveryFrom = isPackageDeleted ? null : GetLastDeliveryFromAddress(updatedShipmentDeliveryPM);
                containerPM.ShipmentLastDeliveryTo = isPackageDeleted ? null : GetLastDeliveryToAddress(updatedShipmentDeliveryPM);
                containerPM.ShipmentDeliveryETA  = isPackageDeleted ? null : updatedShipmentDeliveryPM?.ETA;
                containerPM.ShipmentDeliveryETD  = isPackageDeleted ? null : updatedShipmentDeliveryPM?.ETD;
                containerPM.ShipmentDeliveryATA  = isPackageDeleted ? null : updatedShipmentDeliveryPM?.ATA;
                containerPM.ShipmentDeliveryATD = isPackageDeleted ? null : updatedShipmentDeliveryPM?.ATD;
                containerService.Update(containerPM);
            }
        }

        private bool IsContainerUpdatedBefore(ContainerPM containerPM)
        {
            return initializer.EntityPM.ShipmentPackages != null && initializer.EntityPM.ShipmentPackages.Any(d =>
                 d.ChangeSetOp == ChangeSetOperation.Update && d.ContainerEntityId == containerPM.Id);
        }

        private void HandelDeletedShipmentPickUpsChangeSets()
        {
            if (initializer.EntityPM.ShipmentPickUps == null)
                return;

            List<ShipmentPickUpPM> shipmentPickUpsPM = initializer.EntityPM.ShipmentPickUps.FindAll(d => d.ChangeSetOp == ChangeSetOperation.Delete
            && d.ShipmentPickUpDeliveryPackages != null && (d.ShipmentPickUpDeliveryPackages.Any(a => !string.IsNullOrEmpty(a.ContainerEntityId))));
            if (shipmentPickUpsPM != null && shipmentPickUpsPM.Count > 0)
            {
                this.UpdateContainerFieldsFromDeletedPickUps(shipmentPickUpsPM);
            }
        }

        private void UpdateContainerFieldsFromDeletedPickUps(List<ShipmentPickUpPM> shipmentPickUpsPM)
        {
            foreach (ShipmentPickUpPM shipmentPickUpPM in shipmentPickUpsPM)
            {
                List<ContainerPM> containers = this.GetPickUpContainersToBeDeletedByContainerEntityId(shipmentPickUpPM);
                this.UpdateContainerFieldsFromDeletedPickUpsFields(containers);
            }
        }

        private void UpdateContainerFieldsFromDeletedPickUpsFields(List<ContainerPM> containers)
        {
            if (containers == null)
                return;
            
            foreach(ContainerPM containerPM in containers)
            {
                containerPM.ShipmentFirstPickupFrom = null;
                containerPM.ShipmentFirstPickupTo = null;
                containerPM.ShipmentPickupETA = null;
                containerPM.ShipmentPickupETD = null;
                containerPM.ShipmentPickupATA = null;
                containerPM.ShipmentPickupATD = null;
                containerService.Update(containerPM);
            }
        }

        private void HandelDeletedShipmentDeliveriesChangeSets()
        {
            if (initializer.EntityPM.ShipmentDeliveries == null)
                return;

            List<ShipmentDeliveryPM> shipmentDeliveriesPM = initializer.EntityPM.ShipmentDeliveries.FindAll(d => d.ChangeSetOp == ChangeSetOperation.Delete
            && d.ShipmentPickUpDeliveryPackages != null && (d.ShipmentPickUpDeliveryPackages.Any(a => !string.IsNullOrEmpty(a.ContainerEntityId))));
            if (shipmentDeliveriesPM != null && shipmentDeliveriesPM.Count > 0)
            {
                this.UpdateContainerFieldsFromDeletedDeliveries(shipmentDeliveriesPM);
            }
        }

        private void UpdateContainerFieldsFromDeletedDeliveries(List<ShipmentDeliveryPM> shipmentDeliveriesPM)
        {
            foreach (ShipmentDeliveryPM shipmentDeliveryPM in shipmentDeliveriesPM)
            {
                List<ContainerPM> containers = this.GetDeliveryContainersToBeDeletedByContainerEntityId(shipmentDeliveryPM);
                this.UpdateContainerFieldsFromDeletedDeliveriesFields(containers);
            }
        }

        private void UpdateContainerFieldsFromDeletedDeliveriesFields(List<ContainerPM> containers)
        {
            if (containers == null)
                return;

            foreach (ContainerPM containerPM in containers)
            {
                containerPM.ShipmentLastDeliveryFrom = null;
                containerPM.ShipmentLastDeliveryTo = null;
                containerPM.ShipmentDeliveryETA = null;
                containerPM.ShipmentDeliveryETD = null;
                containerPM.ShipmentDeliveryATA = null;
                containerPM.ShipmentDeliveryATD = null;
                containerService.Update(containerPM);
            }
        }

        public static void UpdateConatinarStatus(ShipmentPM shipmentPM,bool isEntityStatusUpdated ,IShipmentsContext shipmentContext)
        {
            if (shipmentPM == null)
                return;

            if (!isEntityStatusUpdated)
                return;

            List<ContainerPM> containers = GetShipmentContainers(shipmentPM);
            ContainerService containerService = GetContainerService(shipmentContext, shipmentPM.Tenant);

            foreach (ContainerPM containerPM in containers)
            {
                containerPM.ShipmentStatusId = shipmentPM.StatusId;
                containerService.Update(containerPM);
            }
        }

        private static List<ContainerPM> GetShipmentContainers(ShipmentPM shipmentPM)
        {
            List<ContainerPM> containerPMs = new List<ContainerPM>();
            if (shipmentPM == null)
            {
                return containerPMs;
            }
            ContainerQuery containerQuery = new ContainerQuery(shipmentPM.Tenant);
            foreach (ShipmentPackagePM shipmentPackagePM in shipmentPM.ShipmentPackages)
            {
                if (!string.IsNullOrEmpty(shipmentPackagePM.ContainerEntityId))
                {
                    containerPMs.Add(containerQuery.GetSinglePM(shipmentPackagePM.ContainerEntityId, shipmentPM.Tenant));
                }
            }
            return containerPMs;
        }

        private static ContainerService GetContainerService(IShipmentsContext shipmentContext, int tenant)
        {
            return new ContainerService(shipmentContext, tenant);
        }
    }
}
