using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
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
                stanAloneShipmentPM.ForwarderStandaloneShipmentId = null;
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
            ShipmentPickUpDelivery shipmentPickUpDelivery = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, pickupDeliveryId);
            stanAloneShipmentPM.IsStandalonePickupDelivery = true;
            stanAloneShipmentPM.ForwarderStandaloneShipmentId = this.shipmentPM.Id;
            stanAloneShipmentPM.StandalonePickupDeliveryId = pickupDeliveryId;
            stanAloneShipmentPM.MainCarriageCarrierNumber = shipmentPickUpDelivery!=null? shipmentPickUpDelivery.CarrierNumber : null;
            stanAloneShipmentPM.Driver = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.Driver : null; 
            stanAloneShipmentPM.TruckNumber = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.TruckNumber : null;
            stanAloneShipmentPM.TrailerNumber = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.TrailerNumber : null;
            stanAloneShipmentPM.MainCarriageETD = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.ETD : null;
            stanAloneShipmentPM.MainCarriageETA = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.ETA : null;
            stanAloneShipmentPM.MainCarriageATD = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.ATD : null;
            stanAloneShipmentPM.MainCarriageATA = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.ATA : null;
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
                this.CreatForwarderShipmentPickUpDelivery(); 
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
                                this.UpdateForwarderShipmentPackages(itemPM);
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
            shipmentPM.ForwarderStandaloneShipmentId = null;
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
        private void CreatForwarderShipmentPickUpDelivery()
        {
            if (!string.IsNullOrEmpty(this.shipmentPM.ForwarderStandaloneShipmentId) && !string.IsNullOrEmpty(this.shipmentPM.ForwarderPickUpDeliveryType))
            {
                Shipment forwarderShipment = this.GetForwarderShipmentPM();
                ShipmentPickUpDelivery shipmentPickUpDelivery = new ShipmentPickUpDelivery();
                this.MapForwarderShipmentNewPickUpDelivery(shipmentPickUpDelivery, forwarderShipment);
                MapPickUpDeliveryFieldsFromStandaloneShipment(shipmentPickUpDelivery);
                shipmentPickUpDeliveryRepository.Add(shipmentPickUpDelivery);
                initializer.Repository.Update(forwarderShipment);
            }
        }
        private Shipment GetForwarderShipmentPM()
        {
            Shipment  forwarderShipment = this.initializer.Repository.GetSingleShipment(this.shipmentPM.ForwarderStandaloneShipmentId,tenant);
            return forwarderShipment;
        }

        private void MapForwarderShipmentNewPickUpDelivery(ShipmentPickUpDelivery shipmentPickUpDelivery,Shipment forwarderShipment)
        {
            shipmentPickUpDelivery.Id = IdCounter.GetNumber("ShipmentPickUpDelivery", tenant).ToString();
            shipmentPickUpDelivery.Tenant = tenant;
            shipmentPickUpDelivery.PickUpDeliveryTypeCode = GetShipmentPickUpDeliveryType();
            shipmentPickUpDelivery.StandaloneShipmentId = this.shipmentPM.Id;
            shipmentPickUpDelivery.StandaloneShipmentNumber = this.shipmentPM.ShipmentNumber;
            shipmentPickUpDelivery.PickUpDeliveryNumber = GetShipmentPickUpDeliveryNumber(forwarderShipment);
            shipmentPickUpDelivery.FromAddressCity = this.shipmentPM.FromAddressCity;
            shipmentPickUpDelivery.FromAddressCountryId = this.shipmentPM.FromAddressCountryId;
            shipmentPickUpDelivery.ToAddressCountryId = this.shipmentPM.ToAddressCountryId;
            shipmentPickUpDelivery.ToAddressCity = this.shipmentPM.ToAddressCity;
            shipmentPickUpDelivery.ShipmentId = forwarderShipment.Id;
            shipmentPickUpDelivery.PickUpDeliveryFromTypeCode = "PART";
            shipmentPickUpDelivery.PickUpDeliveryToTypeCode = "PART";
        }

        private dynamic GetShipmentPickUpDeliveryType()
        {
            string pickUpTypeCode = "";
            if (this.shipmentPM.ForwarderPickUpDeliveryType == "Delivery")
            {
                pickUpTypeCode = "DELV";
            }
            else if (this.shipmentPM.ForwarderPickUpDeliveryType == "Pickup")
            {
                 pickUpTypeCode = "PICK";
            }
            return pickUpTypeCode;
        }

        private string GetShipmentPickUpDeliveryNumber(Shipment shipment)
        {
            if (this.shipmentPM.ForwarderPickUpDeliveryType == "Delivery")
            {
                shipment.ShipmentDeliveryIndex += 1;
                return (shipment.ShipmentNumber + "/" + shipment.ShipmentDeliveryIndex);
            }
            else if (this.shipmentPM.ForwarderPickUpDeliveryType == "Pickup")
            {
                shipment.ShipmentPickUpIndex += 1;      
                return (shipment.ShipmentNumber + "/" + shipment.ShipmentPickUpIndex);
            }
            return "";
        }

        private void UpdateForwarderShipmentPackages(ShipmentPackagePM shipmentPackagePM)
        {
            List<ShipmentPackage> shipmentPackages = this.initializer.ShipmentPackageRepository.GetShipmentsPackagesByContainerIdAndTenant(this.shipmentPM.Id, shipmentPackagePM.ContainerEntityId, tenant);
            if(shipmentPackages != null)
            {
                foreach(ShipmentPackage shipmentPackage in shipmentPackages)
                {
                    if (shipmentPackage != null)
                    {
                        shipmentPackage.ContainerEntityId = shipmentPackagePM.ContainerEntityId;
                        shipmentPackage.ContainerNumber = shipmentPackagePM.ContainerNumber;
                        shipmentPackage.Description = shipmentPackagePM.Description;
                        shipmentPackage.PackageTypeId = shipmentPackagePM.PackageTypeId;
                        shipmentPackage.Quantity = shipmentPackagePM.Quantity;
                        shipmentPackage.Volume = shipmentPackagePM.Volume;
                        shipmentPackage.Weight = shipmentPackagePM.Weight;
                        shipmentPackage.ShipperSeal = shipmentPackagePM.ShipperSeal;
                        this.initializer.ShipmentPackageRepository.Update(shipmentPackage);
                    }
                }
            }
        }
    }
}
