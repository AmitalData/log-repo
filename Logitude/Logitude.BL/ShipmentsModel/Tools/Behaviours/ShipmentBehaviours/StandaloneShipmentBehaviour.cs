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
        private ShipmentQuery shipmentQuery;
        private ShipmentPickUpQuery shipmentPickUpQuery;
        private ShipmentDeliveryQuery shipmentDeliveryQuery;

        private int tenant;
        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.shipmentsContext = this.initializer.ShipmentContext;
            this.shipmentPM = this.initializer.EntityPM;
            this.tenant = this.initializer.Tenant;
            this.shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(this.shipmentsContext);
            this.shipmentPickUpDeliveryPackageRepository = new ShipmentPickUpDeliveryPackageRepository(this.shipmentsContext);
            this.shipmentPickUpQuery = new ShipmentPickUpQuery(this.shipmentPickUpDeliveryRepository);
            this.shipmentDeliveryQuery = new ShipmentDeliveryQuery(this.shipmentPickUpDeliveryRepository);
            this.shipmentQuery = new ShipmentQuery(this.initializer.Repository);
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
                        case ChangeSetOperation.Update:
                            {

                                if (itemPM.IsConnectedToStandalone)
                                {
                                    this.UpdateStandAloneShipmentOnPickUpConnection(itemPM);
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
                                    this.UpdateStandAloneShipmentOnDeliveryConnection(itemPM);
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

        private void UpdateShipment(ShipmentPM shipmentPM)
        {
            ShipmentService shipmentService = new ShipmentService(this.shipmentsContext, shipmentPM, "");
            shipmentService.Update(true);
        }

        private void UpdateStandAloneShipmentOnPickUpConnection(ShipmentPickUpPM shipmentPickUpPM)
        {
            if (shipmentPickUpPM == null)
            {
                return;
            }

            ShipmentPM stanAloneShipmentPM = this.GetStandAloneShipmentPM(shipmentPickUpPM.StandaloneShipmentId);
            if (stanAloneShipmentPM == null)
            {
                return;
            }

            this.MapStandaloneShipmentFields(stanAloneShipmentPM, shipmentPickUpPM.Id, "Pickup");
            this.MapStandAlonePackagesOnPickDeliveryConnection(stanAloneShipmentPM, shipmentPickUpPM.Id, shipmentPickUpPM.ShipmentPickUpDeliveryPackages);
            this.UpdateConnectedStanadAloneShipmentService(stanAloneShipmentPM);
        }

        private void UpdateStandAloneShipmentOnDeliveryConnection(ShipmentDeliveryPM shipmentDeliveryPM)
        {
            if(shipmentDeliveryPM == null)
            {
                return;
            }

            ShipmentPM stanAloneShipmentPM = this.GetStandAloneShipmentPM(shipmentDeliveryPM.StandaloneShipmentId);
            if (stanAloneShipmentPM == null)
            {
                return;
            }
            this.MapStandaloneShipmentFields(stanAloneShipmentPM, shipmentDeliveryPM.Id, "Delivery");
            this.MapStandAlonePackagesOnPickDeliveryConnection(stanAloneShipmentPM, shipmentDeliveryPM.Id, shipmentDeliveryPM.ShipmentPickUpDeliveryPackages);
            this.UpdateConnectedStanadAloneShipmentService(stanAloneShipmentPM);
        }

        private void UpdateConnectedStanadAloneShipmentService(ShipmentPM stanAloneShipmentPM)
        {
            this.UpdateShipment(stanAloneShipmentPM);            
        }
        private void MapStandAlonePackagesOnPickDeliveryConnection(ShipmentPM stanAloneShipmentPM, string pickupDeliveryId, List<ShipmentPickUpDeliveryPackagePM> connectedPickupDeliveryPackages)
        {
            List<ShipmentPickUpDeliveryPackagePM> shipmentPickUpDeliveryPackages = connectedPickupDeliveryPackages.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            if (stanAloneShipmentPM.ShipmentPackages.Count != 0)
            {
                foreach (ShipmentPackagePM shipmentPackagePM in stanAloneShipmentPM.ShipmentPackages)
                {
                    shipmentPackagePM.ChangeSetOp = ChangeSetOperation.Delete;
                }
            }
            if (shipmentPickUpDeliveryPackages.Count() != 0)
            {
                ShipmentPickUpDeliveryPackage itemPoco = shipmentPickUpDeliveryPackageRepository.GetSingleShipmentPickUpDeliveryPackage(shipmentPickUpDeliveryPackages.FirstOrDefault().Id);
                if (itemPoco != null)
                {
                    this.CreateStandaloneShipmentPackage(itemPoco, stanAloneShipmentPM.Id);
                }
            }
        }

        private void MapStandaloneShipmentFields(ShipmentPM standAloneShipmentPM, string pickupDeliveryId, string forwarderPickUpDeliveryType)
        {
            ShipmentPickUpDelivery shipmentPickUpDelivery = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, pickupDeliveryId);
            standAloneShipmentPM.IsStandalonePickupDelivery = true;
            standAloneShipmentPM.ForwarderStandaloneShipmentId = this.shipmentPM.Id;
            standAloneShipmentPM.StandalonePickupDeliveryId = pickupDeliveryId;
            standAloneShipmentPM.ForwarderPickUpDeliveryType = forwarderPickUpDeliveryType;
            standAloneShipmentPM.MainCarriageCarrierId = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.CarrierId : null;
            standAloneShipmentPM.MainCarriageCarrierNumber = shipmentPickUpDelivery!=null? shipmentPickUpDelivery.CarrierNumber : null;
            standAloneShipmentPM.Driver = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.Driver : null;
            standAloneShipmentPM.TruckNumber = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.TruckNumber : null;
            standAloneShipmentPM.TrailerNumber = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.TrailerNumber : null;
            standAloneShipmentPM.MainCarriageETD = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.ETD : null;
            standAloneShipmentPM.MainCarriageETA = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.ETA : null;
            standAloneShipmentPM.MainCarriageATD = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.ATD : null;
            standAloneShipmentPM.MainCarriageATA = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.ATA : null;
        }
        private ShipmentPM GetStandAloneShipmentPM(string shipmentId)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(initializer.Repository);
            ShipmentPM stanAloneShipmentPM = shipmentQuery.GetSinglePM(shipmentId, this.tenant);
            return stanAloneShipmentPM;
        }

        // -------- standalone side ----
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
            ShipmentPM forwarderShipment = this.GetForwarderShipmentPM();
            if (shipmentPickUpDelivery == null || forwarderShipment == null)
            {
                return;
            }

            if (shipmentPickUpDelivery.PickUpDeliveryTypeCode == "PICK")
            {
                UpdatePickUpStandaloneFieldsOnShipmentUpdate(this.GetShipmentPickUpPMByStandAloneShipmentId(forwarderShipment, shipmentPickUpDelivery.Id));
                this.UpdateShipment(forwarderShipment);
            }
            else if (shipmentPickUpDelivery.PickUpDeliveryTypeCode == "DELV")
            {
                UpdateDeliveryStandaloneFieldsOnShipmentUpdate(this.GetShipmentDeliveryPMByStandAloneShipmentId(forwarderShipment, shipmentPickUpDelivery.Id));
                this.UpdateShipment(forwarderShipment);
            }
        }

        private void UpdatePickUpStandaloneFieldsOnShipmentUpdate(ShipmentPickUpPM shipmentPickUpPM)
        {
            if (shipmentPickUpPM == null)
            {
                return;
            }
            this.MapPickUpFieldsFromStandaloneShipment(shipmentPickUpPM);
            this.HandelStandalonePackagesChangeSets(shipmentPickUpPM.Id);
        }

        private void UpdateDeliveryStandaloneFieldsOnShipmentUpdate(ShipmentDeliveryPM shipmentDeliveryPM)
        {
            if (shipmentDeliveryPM == null)
            {
                return;
            }
            this.MapDeliveryFieldsFromStandaloneShipment(shipmentDeliveryPM);
            this.HandelStandalonePackagesChangeSets(shipmentDeliveryPM.Id);
        }

        private void MapPickUpFieldsFromStandaloneShipment(ShipmentPickUpPM shipmentPickUpPM)
        {
            shipmentPickUpPM.ChangeSetOp = ChangeSetOperation.Update;
            shipmentPickUpPM.FromPartnerCardId = shipmentPM.MainCarriageFromPartnerId;
            shipmentPickUpPM.FromAddressId = shipmentPM.MainCarriageFromAddressId;
            shipmentPickUpPM.ToPartnerCardId = shipmentPM.MainCarriageToPartnerId;
            shipmentPickUpPM.ToAddressId = shipmentPM.MainCarriageToAddressId;
            shipmentPickUpPM.CarrierId = shipmentPM.MainCarriageCarrierId;
            shipmentPickUpPM.CarrierNumber = shipmentPM.MainCarriageCarrierNumber;
            shipmentPickUpPM.Driver = shipmentPM.Driver;
            shipmentPickUpPM.TruckNumber = shipmentPM.TruckNumber;
            shipmentPickUpPM.TrailerNumber = shipmentPM.TrailerNumber;
            shipmentPickUpPM.ETD = shipmentPM.MainCarriageETD;
            shipmentPickUpPM.ETA = shipmentPM.MainCarriageETA;
            shipmentPickUpPM.ATD = shipmentPM.MainCarriageATD;
            shipmentPickUpPM.ATA = shipmentPM.MainCarriageATA;
        }

        private void MapDeliveryFieldsFromStandaloneShipment(ShipmentDeliveryPM shipmentDeliveryPM)
        {
            shipmentDeliveryPM.ChangeSetOp = ChangeSetOperation.Update;
            shipmentDeliveryPM.FromPartnerCardId = shipmentPM.MainCarriageFromPartnerId;
            shipmentDeliveryPM.FromAddressId = shipmentPM.MainCarriageFromAddressId;
            shipmentDeliveryPM.ToPartnerCardId = shipmentPM.MainCarriageToPartnerId;
            shipmentDeliveryPM.ToAddressId = shipmentPM.MainCarriageToAddressId;
            shipmentDeliveryPM.CarrierId = shipmentPM.MainCarriageCarrierId;
            shipmentDeliveryPM.CarrierNumber = shipmentPM.MainCarriageCarrierNumber;
            shipmentDeliveryPM.Driver = shipmentPM.Driver;
            shipmentDeliveryPM.TruckNumber = shipmentPM.TruckNumber;
            shipmentDeliveryPM.TrailerNumber = shipmentPM.TrailerNumber;
            shipmentDeliveryPM.ETD = shipmentPM.MainCarriageETD;
            shipmentDeliveryPM.ETA = shipmentPM.MainCarriageETA;
            shipmentDeliveryPM.ATD = shipmentPM.MainCarriageATD;
            shipmentDeliveryPM.ATA = shipmentPM.MainCarriageATA;
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

        private void HandelStandalonePackagesChangeSets(string shipmentPickUpDeliveryId)
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
                                this.ValidateUpdatingForwarderShipmentPackages(itemPM.ContainerNumber, itemPM.ContainerEntityId);
                                this.UpdateForwarderShipmentPackages(itemPM);
                                this.UpdateStanadAlonePickupDeliveryPackages(shipmentPickUpDeliveryId, itemPM);
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
            if (!string.IsNullOrEmpty(this.shipmentPM.ForwarderStandaloneShipmentId) && !string.IsNullOrEmpty(this.shipmentPM.ForwarderPickUpDeliveryType)
                && string.IsNullOrEmpty(this.shipmentPM.StandalonePickupDeliveryId))
            {
                Shipment forwarderShipment = this.GetForwarderShipment();
                ShipmentPickUpDelivery shipmentPickUpDelivery = new ShipmentPickUpDelivery();
                this.MapForwarderShipmentNewPickUpDelivery(shipmentPickUpDelivery, forwarderShipment);
                MapPickUpDeliveryFieldsFromStandaloneShipment(shipmentPickUpDelivery);
                shipmentPickUpDeliveryRepository.Add(shipmentPickUpDelivery);
                this.shipmentPM.StandalonePickupDeliveryId = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.Id : null;
                this.shipmentPM.StandalonePickupDeliveryNumber = shipmentPickUpDelivery != null ? shipmentPickUpDelivery.PickUpDeliveryNumber : null;
                initializer.Repository.Update(forwarderShipment);
            }
        }
        private Shipment GetForwarderShipment()
        {
            Shipment  forwarderShipment = this.initializer.Repository.GetSingleShipment(this.shipmentPM.ForwarderStandaloneShipmentId,tenant);
            return forwarderShipment;
        }
        private ShipmentPM GetForwarderShipmentPM()
        {
            ShipmentPM forwarderShipment = this.shipmentQuery.GetSinglePM(this.shipmentPM.ForwarderStandaloneShipmentId, tenant);
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
        private void ValidateUpdatingForwarderShipmentPackages(string containerNumber,string ContainerEntityId)
        {
            List<ShipmentPackage> shipmentPackages = this.initializer.ShipmentPackageRepository.GetShipmentPackagesForShipmentTenant(this.shipmentPM.ForwarderStandaloneShipmentId, tenant).ToList();
            if (shipmentPackages != null)
            {
                bool isContainerNumberExist = shipmentPackages.Any(d=>d.ContainerNumber == containerNumber && d.ContainerEntityId != ContainerEntityId);
                if (isContainerNumberExist)
                {
                    throw new ApplicationException("Cannot have 2 containers with same number");
                }
            }
        }

        private ShipmentPickUpPM GetShipmentPickUpPMByStandAloneShipmentId(ShipmentPM shipmentPM, string pickUpPMId)
        {
            if (shipmentPM.ShipmentPickUps == null || string.IsNullOrEmpty(pickUpPMId))
                return null;

            ShipmentPickUpPM shipmentPickUpPM = shipmentPM.ShipmentPickUps.Find(d => d.Id == pickUpPMId);
            return shipmentPickUpPM;
        }

        private ShipmentDeliveryPM GetShipmentDeliveryPMByStandAloneShipmentId(ShipmentPM shipmentPM, string deliveryPM)
        {
            if (shipmentPM.ShipmentDeliveries == null || string.IsNullOrEmpty(deliveryPM))
                return null;

            ShipmentDeliveryPM shipmentDeliveryPM = shipmentPM.ShipmentDeliveries.Find(d => d.Id == deliveryPM);
            return shipmentDeliveryPM;
        }
    }
}
