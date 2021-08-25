using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
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
                this.HandelShipmentPickUpsChangeSets();
                this.HandelShipmentDeliveriesChangeSets();
                this.HandelDeletedShipmentPickUpsChangeSets();
                this.HandelDeletedShipmentDeliveriesChangeSets();


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

            return false; 
        }

        private void HandelShipmentPickUpsChangeSets()
        {
            ShipmentPickUpPM updatedShipmentPickUp = this.GetUpdatedShipmentPickUpPMByContainerEntityId();
            List<ContainerPM> containersToBeUpdated = this.GetPickUpContainersToBeUpdatedByContainerEntityId(updatedShipmentPickUp);

            if (updatedShipmentPickUp == null || containersToBeUpdated.Count == 0)
                return;

            this.UpdateContainerFieldsFromPickUp(updatedShipmentPickUp, containersToBeUpdated);
        }

        private void HandelShipmentDeliveriesChangeSets()
        {
            ShipmentDeliveryPM updatedShipmentDeliveryPM = this.GetUpdatedShipmentDeliveryPMByContainerEntityId();
            List<ContainerPM> containersToBeUpdated = this.GetDeliveryContainersToBeUpdatedByContainerEntityId(updatedShipmentDeliveryPM);

            if (updatedShipmentDeliveryPM == null || containersToBeUpdated.Count == 0)
                return;

            this.UpdateContainerFieldsFromDelivery(updatedShipmentDeliveryPM, containersToBeUpdated);
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
            container.Master = this.initializer.EntityPM.Master;
            container.ContainerNumber = shipmentPackage.ContainerNumber;
            container.ShipmentId = shipmentPackage.ShipmentId;
            container.ShipmentPreCarriageFromId = this.initializer.EntityPM?.PreCarriageFromPortId;
            container.ShipmentPreCarriageToId = this.initializer.EntityPM?.PreCarriageToPortId;
            container.ShipmentMainCarriageFromId = this.initializer.EntityPM?.MainCarriageFromPortId;
            container.ShipmentMainCarriageToId = this.initializer.EntityPM?.MainCarriageToPortId;
            container.ShipmentTransshipment1FromId = this.initializer.EntityPM?.Transshipment1FromPortId;
            container.ShipmentTransshipment1ToId = this.initializer.EntityPM?.Transshipment1ToPortId;
            container.ShipmentTransshipment2FromId = this.initializer.EntityPM?.Transshipment2FromPortId;
            container.ShipmentTransshipment2ToId = this.initializer.EntityPM?.Transshipment2ToPortId;
            container.ShipmentTransshipment3FromId = this.initializer.EntityPM?.Transshipment3FromPortId;
            container.ShipmentTransshipment3ToId = this.initializer.EntityPM?.Transshipment3ToPortId;
            container.ShipmentOnCarriageFromId = this.initializer.EntityPM?.OnCarriageFromPortId;
            container.ShipmentOnCarriageToId = this.initializer.EntityPM?.OnCarriageToPortId;
            this.MapContainerFieldsFromShipmentPickup(container);
            this.MapContainerFieldsFromShipmentDelivery(container);
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

        private void MapContainerFieldsFromShipmentPickup(ContainerPM entityPM)
        {
            ShipmentPickUpPM shipmentPickUpPM = GetShipmentPickUpPMByContainerEntityId(entityPM);
            if (shipmentPickUpPM == null)
                return;

            entityPM.ShipmentFirstPickupFrom = this.GetFirstPickupFromAddress(shipmentPickUpPM);
            entityPM.ShipmentFirstPickupTo = this.GetFirstPickupToAddress(shipmentPickUpPM);
        }

        private void MapContainerFieldsFromShipmentDelivery(ContainerPM entityPM)
        {
            ShipmentDeliveryPM shipmentDeliveryPM = GetShipmentDeliveryPMByContainerEntityId(entityPM);
            if (shipmentDeliveryPM == null)
                return;

            entityPM.ShipmentLastDeliveryFrom = GetLastDeliveryFromAddress(shipmentDeliveryPM);
            entityPM.ShipmentLastDeliveryTo = GetLastDeliveryToAddress(shipmentDeliveryPM);
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
                if (!string.IsNullOrEmpty(shipmentPickUpDeliveryPackagesPM.ContainerEntityId))
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
            foreach (ShipmentPickUpDeliveryPackagePM shipmentPickUpDeliveryPackagesPM in shipmentPickUpPM.ShipmentPickUpDeliveryPackages)
            {
                if (!string.IsNullOrEmpty(shipmentPickUpDeliveryPackagesPM.ContainerEntityId))
                {
                    containerPMs.Add(containerQuery.GetSinglePM(shipmentPickUpDeliveryPackagesPM.ContainerEntityId, initializer.Tenant));
                }
            }
            return containerPMs;
        }

        private void UpdateContainerFieldsFromPickUp(ShipmentPickUpPM updatedShipmentPickUp, List<ContainerPM> containerPMs)
        {
            foreach (ContainerPM containerPM in containerPMs)
            {
                this.UpdateContainerFieldsFromPickUpFields(containerPM,updatedShipmentPickUp);
            }
        }

        private void UpdateContainerFieldsFromDelivery(ShipmentDeliveryPM updatedShipmentDeliveryPM, List<ContainerPM> containerPMs)
        {
            foreach (ContainerPM containerPM in containerPMs)
            {
                this.UpdateContainerFieldsFromDeliveryFields(containerPM, updatedShipmentDeliveryPM);
            }
        }

        private void UpdateContainerFieldsFromPickUpFields(ContainerPM containerPM, ShipmentPickUpPM updatedShipmentPickUp)
        {
            if (!IsContainerUpdatedBefore(containerPM))
            {
                containerPM.ShipmentFirstPickupFrom = this.GetFirstPickupFromAddress(updatedShipmentPickUp);
                containerPM.ShipmentFirstPickupTo = this.GetFirstPickupToAddress(updatedShipmentPickUp);
                containerService.Update(containerPM);
            }
        }

        private void UpdateContainerFieldsFromDeliveryFields(ContainerPM containerPM, ShipmentDeliveryPM updatedShipmentDeliveryPM)
        {
            if (!IsContainerUpdatedBefore(containerPM))
            {
                containerPM.ShipmentLastDeliveryFrom = GetLastDeliveryFromAddress(updatedShipmentDeliveryPM);
                containerPM.ShipmentLastDeliveryTo = GetLastDeliveryToAddress(updatedShipmentDeliveryPM);
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
                List<ContainerPM> containers = this.GetPickUpContainersToBeUpdatedByContainerEntityId(shipmentPickUpPM);
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
                List<ContainerPM> containers = this.GetDeliveryContainersToBeUpdatedByContainerEntityId(shipmentDeliveryPM);
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
                containerService.Update(containerPM);
            }
        }
    }
}
