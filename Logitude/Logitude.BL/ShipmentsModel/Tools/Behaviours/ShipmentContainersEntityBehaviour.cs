using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentContainersEntityBehaviour
    {
        private ShipmentServiceInitializer initializer;
        private IShipmentsContext shipmentsContext;
        private ContainerService containerService;
        public ShipmentContainersEntityBehaviour(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.shipmentsContext = this.initializer.ShipmentContext;
            this.containerService = new ContainerService(this.shipmentsContext, this.initializer.Tenant);
        }

        public void HandleBehaviour()
        {
            HandelContainers();
        }

        private void HandelContainers()
        {
            if (this.ValidHandelContainers())
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

        private bool ValidHandelContainers()
        {
            if (!FeatureToggleHelper.HasFeatureToggle("OIC", this.initializer.Tenant))
                return false;

            if (initializer.EntityPM.TransportModeId != "O" &&
               (initializer.EntityPM.ShipmentTypeId.ToLower() != "fcl" || initializer.EntityPM.ShipmentTypeId.ToLower() != "fcld"))
                return false;

            if (initializer.ShipmentPackagesChangeSet == null)
                return false;

            return true;
        }

        private void CreateContainer(ShipmentPackagePM shipmentPackage)
        {
            ContainerPM containerPM = new ContainerPM
            {
                Tenant = this.initializer.Tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(this.initializer.Tenant),
                CreatedByUserId = this.initializer.LoggedContactId,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(this.initializer.Tenant),
                UpdatedByUserId = this.initializer.LoggedContactId,
                MainCarriageCarrierId = this.initializer.EntityMasterData.MainCarriageCarrierId,
                MainCarriageCarrierNumber = this.initializer.EntityMasterData.MainCarriageCarrierNumber,
                MainCarriageVesselId = this.initializer.EntityMasterData.MainCarriageVesselId,
                MainCarriageATA = this.initializer.EntityMasterData.MainCarriageATA,
                MainCarriageATD = this.initializer.EntityMasterData.MainCarriageATD,
                MainCarriageETA = this.initializer.EntityMasterData.MainCarriageETA,
                MainCarriageETD = this.initializer.EntityMasterData.MainCarriageETD,
                Master = this.initializer.EntityMasterData.Master,
                ContainerNumber = shipmentPackage.ContainerNumber,
                ShipmentPackagesId = shipmentPackage.Id
            };
            containerService.Create(containerPM);
        }

        private void UpdateContainer(ShipmentPackagePM shipmentPackage)
        {
            bool isContainerExists = CheckIfContainerExists(shipmentPackage);
            if (isContainerExists == true)
            {

            }
        }

        private void DeleteContainer(ShipmentPackagePM shipmentPackage)
        {
            bool isContainerExists = CheckIfContainerExists(shipmentPackage);
            if (isContainerExists == true)
            {

            }
        }

        private bool CheckIfContainerExists(ShipmentPackagePM shipmentPackage)
        {
            return shipmentsContext.Containers.Any(container => container.ContainerNumber == shipmentPackage.ContainerNumber
                                                 && container.Tenant == shipmentPackage.Tenant && container.ShipmentPackagesId == shipmentPackage.Id);
        }
    }
}
