using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Interfaces;

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
                                this.UpdateContainer(itemPM);
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
    }
}
