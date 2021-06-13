using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class StandaloneShipmentBehaviour : IServiceBehaviour
    {
        private ShipmentServiceInitializer initializer;
        private IShipmentsContext shipmentsContext;
        private ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository;
        private ShipmentPickUpDeliveryPackageRepository shipmentPickUpDeliveryPackageRepository;
        private ShipmentPM shipmentPM;
        private int tenant;
        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.shipmentsContext = this.initializer.ShipmentContext;
            this.shipmentPM = this.initializer.EntityPM;
            this.tenant = this.initializer.Tenant;
            this.shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(this.shipmentsContext);
            this.shipmentPickUpDeliveryPackageRepository = new ShipmentPickUpDeliveryPackageRepository(this.shipmentsContext);

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if(shipmentPM.IsStandalonePickupDelivery)
            {
                this.HandleBehaviourFromStandaloneSide();
            }

            else
            {
                this.HandleBehaviourFromForwarderSide();
            }
        }

        private void HandleBehaviourFromForwarderSide()
        {
            this.HandleBehaviourFromPickupForwarderSide();
            this.HandleBehaviourFromDeliveryForwarderSide(); 
        }
        private void HandleBehaviourFromPickupForwarderSide()
        {
            if (initializer.ShipmentPickUpsChangeSet != null)
            {
                foreach (ShipmentPickUpPM itemPM in initializer.ShipmentPickUpsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Update:                            {

                                if (itemPM.IsConnectedToStandalone)
                                {
                                    this.UpdateStandAloneShipmentOnPickDeliveryConnection(itemPM.StandaloneShipmentId, itemPM.Id, itemPM.ShipmentPickUpDeliveryPackages);
                                    itemPM.IsConnectedToStandalone = false;
                                }

                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                if (!string.IsNullOrEmpty(itemPM.StandaloneShipmentId))
                                {
                                    this.ResetStandAloneShipmentFields(itemPM.StandaloneShipmentId);
                                }

                                break;
                            }
                    }
                }
            }
        }
        private void HandleBehaviourFromDeliveryForwarderSide()
        {
            if (initializer.ShipmentDeliveriesChangeSet != null)
            {
                foreach (ShipmentDeliveryPM itemPM in initializer.ShipmentDeliveriesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Update:
                            {                                
                                if (itemPM.IsConnectedToStandalone)
                                {
                                    this.UpdateStandAloneShipmentOnPickDeliveryConnection(itemPM.StandaloneShipmentId, itemPM.Id, itemPM.ShipmentPickUpDeliveryPackages);
                                    itemPM.IsConnectedToStandalone = false;
                                }

                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                if (!string.IsNullOrEmpty(itemPM.StandaloneShipmentId))
                                {
                                    this.ResetStandAloneShipmentFields(itemPM.StandaloneShipmentId);
                                }

                                break;
                            }
                    }
                }
            }
        }
        private void ResetStandAloneShipmentFields(string shipmentId)
        {
            ShipmentPM stanAloneShipmentPM = this.GetStandAloneShipmentPM(shipmentId);
            if (stanAloneShipmentPM != null)
            {
                stanAloneShipmentPM.IsStandalonePickupDelivery = false;
                stanAloneShipmentPM.StandalonePickupDeliveryId = null;
                this.UpdateShipment(stanAloneShipmentPM);                
            }
        }

        private void UpdateShipment(ShipmentPM stanAloneShipmentPM)
        {
            ShipmentService shipmentService = new ShipmentService(this.shipmentsContext, stanAloneShipmentPM, "");
            shipmentService.Update();
        }

        private void UpdateStandAloneShipmentOnPickDeliveryConnection(string shipmentId, string pickupDeliveryId, List<ShipmentPickUpDeliveryPackagePM> connectedPickupDeliveryPackages)
        {
            ShipmentPM stanAloneShipmentPM = this.GetStandAloneShipmentPM(shipmentId);
            if (stanAloneShipmentPM != null)
            {
                this.MapStandaloneShipmentFields(stanAloneShipmentPM, pickupDeliveryId);
                this.MapStandAlonePackagesOnPickDeliveryConnection(stanAloneShipmentPM, pickupDeliveryId, connectedPickupDeliveryPackages);
                this.UpdateConnectedStanadAloneShipmentService(stanAloneShipmentPM);
            }
        }
        private void UpdateConnectedStanadAloneShipmentService(ShipmentPM stanAloneShipmentPM)
        {
            this.UpdateShipment(stanAloneShipmentPM);            
        }
        private void MapStandAlonePackagesOnPickDeliveryConnection(ShipmentPM stanAloneShipmentPM, string pickupDeliveryId, List<ShipmentPickUpDeliveryPackagePM> connectedPickupDeliveryPackages)
        {
            List<ShipmentPickUpDeliveryPackagePM> shipmentPickUpDeliveryPackages = connectedPickupDeliveryPackages.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            
            if (shipmentPickUpDeliveryPackages.Count() != 0 && stanAloneShipmentPM.ShipmentPackages.Count == 0)
            {
                ShipmentPickUpDeliveryPackage itemPoco = shipmentPickUpDeliveryPackageRepository.GetSingleShipmentPickUpDeliveryPackage(shipmentPickUpDeliveryPackages.FirstOrDefault().Id);
                if (itemPoco != null)
                {
                    this.CreateStandaloneShipmentPackage(itemPoco, stanAloneShipmentPM.Id);
                }
            }

            else if (shipmentPickUpDeliveryPackages.Count() == 0 && stanAloneShipmentPM.ShipmentPackages.Count != 0)
            {
                this.CreateStandaloneShipmentPackageFromForwarder(stanAloneShipmentPM.ShipmentPackages.FirstOrDefault(), pickupDeliveryId);
            }
        }
        private void CreateStandaloneShipmentPackageFromForwarder(ShipmentPackagePM shipmentPackagePM, string pickupDeliveryId)
        {
            ShipmentPickUpDeliveryPackage shipmentPickUpDeliveryPackage = new ShipmentPickUpDeliveryPackage()
            {
                Tenant = tenant,
                ContainerEntityId = shipmentPackagePM.ContainerEntityId,
                ContainerNumber = shipmentPackagePM.ContainerNumber,
                Description = shipmentPackagePM.Description,
                PackageTypeId = shipmentPackagePM.PackageTypeId,
                Quantity = shipmentPackagePM.Quantity,
                Volume = shipmentPackagePM.Volume,
                Weight = shipmentPackagePM.Weight,
                Id = IdCounter.GetNumber("ShipmentPickUpDeliveryPackage", tenant).ToString(),
                ShipmentPickUpDeliveryId = pickupDeliveryId,
            };

            shipmentPickUpDeliveryPackageRepository.Add(shipmentPickUpDeliveryPackage);
        }
        private void MapStandaloneShipmentFields(ShipmentPM stanAloneShipmentPM, string pickupDeliveryId)
        {
            stanAloneShipmentPM.IsStandalonePickupDelivery = true;
            stanAloneShipmentPM.StandalonePickupDeliveryId = pickupDeliveryId;
            stanAloneShipmentPM.MainCarriageCarrierNumber = null;
            stanAloneShipmentPM.Driver = null;
            stanAloneShipmentPM.TruckNumber = null;
            stanAloneShipmentPM.TrailerNumber = null;
            stanAloneShipmentPM.MainCarriageETD = null;
            stanAloneShipmentPM.MainCarriageETA = null;
            stanAloneShipmentPM.MainCarriageATD = null;
            stanAloneShipmentPM.MainCarriageATA = null;
        }
        private ShipmentPM GetStandAloneShipmentPM(string shipmentId)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(initializer.Repository);
            ShipmentPM stanAloneShipmentPM = shipmentQuery.GetSinglePM(shipmentId, this.tenant);
            return stanAloneShipmentPM;
        }

        // standalone side
        private void HandleBehaviourFromStandaloneSide()
        {
            if(initializer.IsNewEntity)
            {
                this.UpdatePickUpDeliveryStandaloneFieldsOnShipmentCreation();
            }

            else
            {
                if (shipmentPM.IsCancelled)
                {
                    this.CancleStandaloneShipmentConnection();
                }
                else
                {
                    this.UpdatePickUpDeliveryStandaloneFieldsOnShipmentUpdate();
                }
            }
        }
        private void UpdatePickUpDeliveryStandaloneFieldsOnShipmentCreation()
        {
            if (!string.IsNullOrEmpty(shipmentPM.StandalonePickupDeliveryId))
            {
                ShipmentPickUpDelivery shipmentPickUpDelivery = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, shipmentPM.StandalonePickupDeliveryId);
                if (shipmentPickUpDelivery != null)
                {
                    shipmentPickUpDelivery.StandaloneShipmentId = shipmentPM.Id;
                    shipmentPickUpDelivery.StandaloneShipmentNumber = shipmentPM.ShipmentNumber;

                    IQueryable<ShipmentPickUpDeliveryPackage> pickUpDeliveryPackages = shipmentPickUpDeliveryPackageRepository.GetPackagesByDeliveryId(shipmentPickUpDelivery.Id, tenant);
                    if (pickUpDeliveryPackages.Count() == 1)
                    {
                        this.CreateStandaloneShipmentPackage(pickUpDeliveryPackages.FirstOrDefault());
                    }
                    shipmentPickUpDeliveryRepository.Update(shipmentPickUpDelivery);
                }
            }
        }
        private void UpdatePickUpDeliveryStandaloneFieldsOnShipmentUpdate()
        {
            ShipmentPickUpDelivery shipmentPickUpDelivery = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDeliveryByStandaloneShipmentId(shipmentPM.Id, tenant);
            if (shipmentPickUpDelivery != null)
            {
                this.MapPickUpDeliveryFieldsFromStandaloneShipment(shipmentPickUpDelivery);
                this.HandelStandalonePackagesChangeSets(shipmentPickUpDelivery);
                shipmentPickUpDeliveryRepository.Update(shipmentPickUpDelivery);
            }
        }
        private void MapPickUpDeliveryFieldsFromStandaloneShipment(ShipmentPickUpDelivery shipmentPickUpDelivery)
        {
            shipmentPickUpDelivery.FromPartnerCardId = shipmentPM.MainCarriageFromPartnerId;
            shipmentPickUpDelivery.FromAddressId = shipmentPM.MainCarriageFromAddressId;
            shipmentPickUpDelivery.ToPartnerCardId = shipmentPM.MainCarriageToPartnerId;
            shipmentPickUpDelivery.ToAddressId = shipmentPM.MainCarriageToAddressId;
            shipmentPickUpDelivery.CarrierId = shipmentPM.MainCarriageCarrierId;
            shipmentPickUpDelivery.CarrierNumber = shipmentPM.MainCarriageCarrierNumber;
            shipmentPickUpDelivery.Driver = shipmentPM.Driver;
            shipmentPickUpDelivery.TruckNumber = shipmentPM.TruckNumber;
            shipmentPickUpDelivery.TrailerNumber = shipmentPM.TrailerNumber;
            shipmentPickUpDelivery.ETD = shipmentPM.MainCarriageETD;
            shipmentPickUpDelivery.ETA = shipmentPM.MainCarriageETA;
            shipmentPickUpDelivery.ATD = shipmentPM.MainCarriageATD;
            shipmentPickUpDelivery.ATA = shipmentPM.MainCarriageATA;
        }
        private void HandelStandalonePackagesChangeSets(ShipmentPickUpDelivery shipmentPickUpDelivery)
        {
            if (initializer.ShipmentPackagesChangeSet != null)
            {
                foreach (ShipmentPackagePM itemPM in initializer.ShipmentPackagesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.initializer.IsPackageCreatedFromStandaloneShipment = true;
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateStanadAlonePickupDeliveryPackages(shipmentPickUpDelivery.Id, itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteStanadAlonePickupDeliveryPackages();
                                break;
                            }
                        default: { break; }
                    }
                }
            }
        }
        private void CancleStandaloneShipmentConnection()
        {
            ShipmentPickUpDelivery shipmentPickUpDelivery = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDeliveryByStandaloneShipmentId(shipmentPM.Id, tenant);
            if (shipmentPickUpDelivery != null)
            {
                shipmentPickUpDelivery.StandaloneShipmentId = null;
                shipmentPickUpDelivery.StandaloneShipmentNumber = null;
                shipmentPickUpDeliveryRepository.Update(shipmentPickUpDelivery);
            }

            shipmentPM.IsStandalonePickupDelivery = false;
            shipmentPM.StandalonePickupDeliveryId = null;
        }
        private void DeleteStanadAlonePickupDeliveryPackages()
        {
            ShipmentPickUpDelivery shipmentPickUpDelivery = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDeliveryByStandaloneShipmentId(shipmentPM.Id, tenant);
            if (shipmentPickUpDelivery != null)
            {
                IQueryable<ShipmentPickUpDeliveryPackage> shipmentPickUpDeliveryPackages = shipmentPickUpDeliveryPackageRepository.GetPackagesByDeliveryId(shipmentPickUpDelivery.Id, tenant);
                if (shipmentPickUpDeliveryPackages != null)
                {
                    ShipmentPickUpDeliveryPackage shipmentPickUpDeliveryPackage = shipmentPickUpDeliveryPackages.FirstOrDefault();
                    if (shipmentPickUpDeliveryPackage != null)
                    {
                        shipmentPickUpDeliveryPackageRepository.Remove(shipmentPickUpDeliveryPackage);
                    }
                }
            }
        }
        private void CreateStandaloneShipmentPackage(ShipmentPickUpDeliveryPackage pickUpDeliveryPackage, string shipmentId = null) // dynamic
        {
            ShipmentPackage shipmentPackage = new ShipmentPackage()
            {
                Tenant = tenant,
                ContainerEntityId = pickUpDeliveryPackage.ContainerEntityId,
                ContainerNumber = pickUpDeliveryPackage.ContainerNumber,
                Description = pickUpDeliveryPackage.Description,
                PackageTypeId = pickUpDeliveryPackage.PackageTypeId,
                Quantity = pickUpDeliveryPackage.Quantity,
                Volume = pickUpDeliveryPackage.Volume,
                Weight = pickUpDeliveryPackage.Weight,
                Id = IdCounter.GetNumber("ShipmentPackage", tenant).ToString(),
                ShipmentId = !string.IsNullOrEmpty(shipmentId) ? shipmentId : shipmentPM.Id,
            };

            if (initializer.IsFCLEntity && shipmentPackage.Quantity == null)
            {
                shipmentPackage.Quantity = 1;
            }

            this.initializer.ShipmentPackageRepository.Add(shipmentPackage);
        }        
        private void UpdateStanadAlonePickupDeliveryPackages(string pickupDeliveryId, ShipmentPackagePM shipmentPackagePM)
        {
            IQueryable<ShipmentPickUpDeliveryPackage> shipmentPickUpDeliveryPackages = shipmentPickUpDeliveryPackageRepository.GetPackagesByDeliveryId(pickupDeliveryId, tenant);
            if (shipmentPickUpDeliveryPackages != null)
            {
                ShipmentPickUpDeliveryPackage shipmentPickUpDeliveryPackage = shipmentPickUpDeliveryPackages.FirstOrDefault();
                if (shipmentPickUpDeliveryPackage != null)
                {
                    shipmentPickUpDeliveryPackage.ContainerEntityId = shipmentPackagePM.ContainerEntityId;
                    shipmentPickUpDeliveryPackage.ContainerNumber = shipmentPackagePM.ContainerNumber;
                    shipmentPickUpDeliveryPackage.Description = shipmentPackagePM.Description;
                    shipmentPickUpDeliveryPackage.PackageTypeId = shipmentPackagePM.PackageTypeId;
                    shipmentPickUpDeliveryPackage.Quantity = shipmentPackagePM.Quantity;
                    shipmentPickUpDeliveryPackage.Volume = shipmentPackagePM.Volume;
                    shipmentPickUpDeliveryPackage.Weight = shipmentPackagePM.Weight;
                    shipmentPickUpDeliveryPackage.ShipperSeal = shipmentPackagePM.ShipperSeal;
                    shipmentPickUpDeliveryPackageRepository.Update(shipmentPickUpDeliveryPackage);
                }
            }
        }
    }
}
