using Logitude.BL.Helpers;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.EntityChanges;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ContainerService
    {
        bool isNewEntity;
        private int tenant;
        private ContainerPM containerPm;
        private IShipmentsContext shipmentsContext;
        private ContainerRepository entityRepository;        
        private Container containerPoco { get; set; }
        public ContainerService(IShipmentsContext shipmentsContext, int tenant)
        {
            this.tenant = tenant;
            this.shipmentsContext = shipmentsContext;
            this.entityRepository = new ContainerRepository(shipmentsContext);
        }
        public void Create(ContainerPM entityPM)
        {
            this.isNewEntity = true;
            this.containerPm = entityPM;
            this.containerPm.Id = IdCounter.GetNumber("Container", tenant).ToString();
            this.containerPoco = new Container { Id = this.containerPm.Id, Tenant = this.containerPm.Tenant };
            RunAutomation("OnCreate", entityPM);
            ContainerValidating.Validate(this.containerPm, this.containerPoco, isNewEntity);
            ContainerTracing containerTracing = new ContainerTracing(entityPM, containerPoco, isNewEntity);
            containerTracing.Trace();
            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity);
            this.GetForeignFields_Status(entityPM, containerPoco);
            entityRepository.Add(containerPoco);
            entityRepository.SubmitChanges();
            AddShipmentUpdateKafkaQueueMessage("CToolContainerCreate");
            MapShipmentConcurrencyFields();
        }
        public void Update(ContainerPM entityPM, ContainersExternal containersExternal = null)
        {
            this.isNewEntity = false;
            this.containerPm = entityPM;
            containerPm.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.SetUpdatedByUser();
            this.containerPoco = entityRepository.GetSingleContainer(entityPM.Id, tenant);
            this.MapContainerClosedDate(entityPM, containerPoco);
            
            if (!string.IsNullOrEmpty(containerPm.EmptyReturnLocationPortId) && (containerPm.EstimatedEmptyReturn != containerPoco.EstimatedEmptyReturn || containerPm.ActualEmptyReturn != containerPoco.ActualEmptyReturn))
            {
                this.HandleEmptyReturnLeg();
            }

            ContainerValidating.Validate(this.containerPm, this.containerPoco, isNewEntity);
            ContainerTracing containerTracing = new ContainerTracing(entityPM, containerPoco, isNewEntity);
            containerTracing.Trace();
            
            if (!entityPM.IsUpdateByAutomation)
            {
                RunAutomation("OnUpdate", entityPM);
            }

            this.HandleContainersExternalData(entityPM, containersExternal);
            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity);
            this.GetForeignFields_Status(entityPM, containerPoco);
            entityRepository.Update(containerPoco);
            entityRepository.SubmitChanges();

            AddShipmentUpdateKafkaQueueMessage("CToolContainerUpdate");
            MapShipmentConcurrencyFields();
        }
        private void SetUpdatedByUser()
        {
            bool setUser = true;
            if(containerPm.IsUpdatedFromAPI && !string.IsNullOrEmpty(containerPm.UpdatedByUserId))
            {
                setUser = false;
            }

            if (!setUser)
            {
                return;
            }

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "system@tenant" + tenant + ".com";

            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            Contact contact = contactRep.GetSingleContactByEmail(email, tenant);
            if (contact != null)
            {
                containerPm.UpdatedByUserId = contact.Id;
            }
        }
        private void HandleContainersExternalData(ContainerPM entityPM, ContainersExternal containersExternal)
        {
            if(containersExternal == null)
            {
                return;
            }
            if (!containersExternal.IsFromOceanInsights)
                return;
            ContainersExternalDataBehaviour containersExternalDataBehaviour = new ContainersExternalDataBehaviour(entityPM, shipmentsContext, containersExternal);
            containersExternalDataBehaviour.Handle();
        }
        private void GetForeignFields_Status(ContainerPM entityPM, Container entityPoco)
        {
            entityPM.StatusName = null;
            if (entityPoco.StatusId == null)
            {
                return;
            }
            EntityStatus iEntityStatus = EntityStatusRepository.GetSingleEntityStatus(entityPoco.StatusId, entityPoco.Tenant, true);
            if (iEntityStatus != null)
            {
                entityPM.StatusName = iEntityStatus.Name;
            }
        }
        private void MapContainerClosedDate(ContainerPM containerPM, Container container)
        {
            if (containerPM.IsClosed != container.IsClosed)
            {
                if (containerPM.IsClosed)
                {
                    containerPM.ClosedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                }
                else
                {
                    containerPM.ClosedDate = null;
                }
            }
        }
        private void AddShipmentUpdateKafkaQueueMessage(string queueName)
        {
            if (!FeatureToggleHelper.HasFeatureToggle("CTL", containerPm.Tenant))
            {
                return;
            }
            AddKafkaQueueMessage(queueName);
        }
        private void AddKafkaQueueMessage(string queueName)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue(queueName, 0);
            var queueMessage = new Dictionary<string, string>() {
                { "ContainerId", containerPm.Id },
                { "Tenant", tenant.ToString()}};

            queueservice.Send(queueMessage, tenant);
        }
        public void Delete(ContainerPM entityPM)
        {
            this.containerPm = entityPM;
            this.containerPoco = entityRepository.GetSingleContainer(entityPM.Id, tenant);
            ContainerTracing containerTracing = new ContainerTracing(entityPM, containerPoco, isNewEntity);
            containerTracing.Trace();
            entityRepository.Remove(containerPoco);
            entityRepository.SubmitChanges();
        }
        private void RunAutomation(string processType, ContainerPM entityPM)
        {
            var mainEntityChangeService = new MainEntityChangeService(new EntityChangeArgs() { EntityPM = entityPM, ProcessType = processType, ObjectTableName = "Container", EntityId = entityPM.Id, Tenant = entityPM.Tenant, StartDate = DateTime.Now });
            mainEntityChangeService.AddEntityChange();
        }
        private void MapShipmentConcurrencyFields()
        {
            if (string.IsNullOrEmpty(this.containerPm.ShipmentId))
            {
                return;
            }
            this.containerPm.ShipmentConcurrencyGUID = entityRepository.GetConcurrencyGUIDByShipmentId(this.containerPm.ShipmentId, this.containerPm.Tenant);
            this.containerPm.ShipmentNewConcurrencyGUID = Guid.NewGuid().ToString();
        }

        private void HandleEmptyReturnLeg()
        {
            if (!SecurityUtility.CheckFeature("Shipment", "Area.ContainersFU", tenant)) return;

            ShipmentPM shipmentPM = this.GetShipment();
            if (shipmentPM == null || shipmentPM.DirectionId != "I") return;

            ShipmentDeliveryPM emptyReturn = this.GetEmptyReturnLeg(shipmentPM);
            if (emptyReturn != null)
            {
                UpdateEmptyReturnLeg(emptyReturn);
            }

            else
            {
                ShipmentPackagePM shipmentPackage = shipmentPM.ShipmentPackages.Where(d => d.ContainerEntityId == containerPm.Id).FirstOrDefault();
                if (shipmentPackage == null) return;

                emptyReturn = CreateEmptyReturnLeg(shipmentPackage, shipmentPM);
                ShipmentPickUpDeliveryPackagePM deliveryPackage = this.CreateEmptyReturnPackage(shipmentPackage);
                if (deliveryPackage != null)
                {
                    this.AddPackageHarmonizes(deliveryPackage, shipmentPackage);
                    emptyReturn.ShipmentPickUpDeliveryPackages.Add(deliveryPackage);
                }
                shipmentPM.ShipmentDeliveries.Add(emptyReturn);
            }

            UpdateShipment(shipmentPM);            
        }

        private ShipmentPM GetShipment()
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            return shipmentQuery.GetSinglePM(containerPm.ShipmentId, tenant);
        }
        private ShipmentDeliveryPM GetEmptyReturnLeg(ShipmentPM shipmentPM)
        {
            ShipmentDeliveryPM shipmentDelivery = null;

            ShipmentPickUpDeliveryPackageRepository pickUpDeliveryPackageRepository = new ShipmentPickUpDeliveryPackageRepository(shipmentsContext);
            List<ShipmentPickUpDeliveryPackage> packages = pickUpDeliveryPackageRepository.GetShipmentPickUpDeliveryPackagesByContainerIdAndTenant(containerPm.Id, tenant);
            if (packages != null && packages.Count > 0)
            {
                List<string> deliveryPackagesIds = packages.Select(s => s.ShipmentPickUpDeliveryId).ToList();
                shipmentDelivery = shipmentPM.ShipmentDeliveries.Where(a => deliveryPackagesIds.Contains(a.Id) && a.PickUpDeliveryTypeCode == "EMPT").FirstOrDefault();
            }

            return shipmentDelivery;
        }
        private void UpdateEmptyReturnLeg(ShipmentDeliveryPM emptyReturn)
        {
            emptyReturn.ETA = containerPm.EstimatedEmptyReturn;
            emptyReturn.ATA = containerPm.ActualEmptyReturn;
            emptyReturn.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        }
        private ShipmentDeliveryPM CreateEmptyReturnLeg(ShipmentPackagePM shipmentPackage, ShipmentPM shipmentPM)
        {
            ShipmentDeliveryPM emptyReturn = this.CreateEmptyReturnInstance(shipmentPackage.Id);
            this.ComputeEmptyReturnFromProperties(emptyReturn, shipmentPM);            

            PortRepository portRepository = new PortRepository(tenant);
            Port toPort = portRepository.GetSinglePort(containerPm.EmptyReturnLocationPortId, tenant);
            emptyReturn.ToAddress = "Port Of: " + toPort?.EnglishName;

            if (!string.IsNullOrEmpty(emptyReturn.FromPortId))
            {
                Port fromPort = portRepository.GetSinglePort(emptyReturn.FromPortId, tenant);
                emptyReturn.FromAddress = "Port Of: " + fromPort?.EnglishName;
            }

            return emptyReturn;
        }
        private ShipmentDeliveryPM CreateEmptyReturnInstance(string connectedPackageId)
        {
            return new ShipmentDeliveryPM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                PickUpDeliveryTypeCode = "EMPT",
                ETA = containerPm.EstimatedEmptyReturn,
                ATA = containerPm.ActualEmptyReturn,
                PickUpDeliveryToTypeCode = "PORT",
                ToPortId = containerPm.EmptyReturnLocationPortId,
                FullResponsibility = true,
                ConnectedPackageId = connectedPackageId,
            };
        }
        private ShipmentPickUpDeliveryPackagePM CreateEmptyReturnPackage(ShipmentPackagePM shipmentPackage)
        {
            return new ShipmentPickUpDeliveryPackagePM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                ContainerEntityId = containerPm.Id,
                ContainerNumber = containerPm.ContainerNumber,
                PackageTypeId = shipmentPackage.PackageTypeId,
                Weight = shipmentPackage.Weight,
                Description = shipmentPackage.Description,
                PackageTypeName = shipmentPackage.PackageTypeName,
                Quantity = shipmentPackage.Quantity,
                Volume = shipmentPackage.Volume,
                ShipperSeal = shipmentPackage.ShipperSeal,
                Width = shipmentPackage.Width,
                Height = shipmentPackage.Height,
                Length = shipmentPackage.Length,
                Harmonize = shipmentPackage.Harmonize,
                OriginalShipmentPackageId = shipmentPackage.Id,
                IsMultiHarmonize = shipmentPackage.IsMultiHarmonize,
            };
        }
        private void AddPackageHarmonizes(ShipmentPickUpDeliveryPackagePM deliveryPackage, ShipmentPackagePM shipmentPackage)
        {
            foreach( ShipmentPackageHarmonizePM harmonizeItem in shipmentPackage.ShipmentPackageHarmonizes)
            {
                PickUpDeliveryPackageHarmonizePM harmonize = new PickUpDeliveryPackageHarmonizePM();
                harmonize.Harmonize = harmonizeItem.Harmonize;
                harmonize.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                deliveryPackage.PickUpDeliveryPackageHarmonizes.Add(harmonize);
            };
        }
        private void ComputeEmptyReturnFromProperties(ShipmentDeliveryPM emptyReturn, ShipmentPM shipmentPM)
        {
            ShipmentDeliveryPM lastDelivery = shipmentPM.ShipmentDeliveries.Where(d => d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(o => o.PickUpDeliveryNumber).FirstOrDefault();
            if(lastDelivery != null)
            {
                this.MapLocationFromLastDelivery(lastDelivery, emptyReturn);
                emptyReturn.PickUpDeliveryFromTypeCode = lastDelivery.PickUpDeliveryToTypeCode;                
            }

            else if (!string.IsNullOrEmpty(shipmentPM.WarehouseLegWarehouseId))
            {
                emptyReturn.PickUpDeliveryFromTypeCode = "PART";
                emptyReturn.FromPartnerCardId = shipmentPM.WarehouseLegWarehouseId;
                emptyReturn.FromAddressId = shipmentPM.WarehouseLegAddressId;
            }

            else if (!string.IsNullOrEmpty(shipmentPM.OnCarriageToPortId))
            {
                emptyReturn.PickUpDeliveryFromTypeCode = "PORT";
                emptyReturn.FromPortId = shipmentPM.OnCarriageToPortId;
            }

            else 
            {
                emptyReturn.PickUpDeliveryFromTypeCode = "PORT";
                emptyReturn.FromPortId = shipmentPM.MainCarriageToPortId;
            }
        }        
        private void MapLocationFromLastDelivery(ShipmentDeliveryPM lastDelivery, ShipmentDeliveryPM emptyReturn)
        {
            switch (lastDelivery.PickUpDeliveryFromTypeCode)
            {
                case "PART":
                    {
                        emptyReturn.FromPartnerCardId = lastDelivery.ToPartnerCardId;
                        emptyReturn.FromAddressId = lastDelivery.ToAddressId;
                        break;
                    }

                case "PORT":
                    {
                        emptyReturn.FromPortId = lastDelivery.ToPortId;
                        emptyReturn.FromAddress = lastDelivery.ToAddress;
                        break;
                    }

                case "CASL":
                    {
                        emptyReturn.FromAddressCity = lastDelivery.ToAddressCity;
                        emptyReturn.FromAddressZipCode = lastDelivery.ToAddressZipCode;
                        emptyReturn.FromAddressCountryId = lastDelivery.ToAddressCountryId;
                        break;
                    }
            }
        }
        private void UpdateShipment(ShipmentPM shipmentPM)
        {
            string systemEmail = "system@tenant" + tenant + ".com";
            ShipmentService service = new ShipmentService(shipmentsContext, shipmentPM, systemEmail);
            service.SetChangeSet(shipmentPM.ShipmentPackages, shipmentPM.ShipmentOrderPackages, shipmentPM.ShipmentPickUps, shipmentPM.ShipmentDeliveries, shipmentPM.ShipmentReceivables, shipmentPM.ShipmentPayables, shipmentPM.FollowUps, shipmentPM.ShipmentAWBPrintOnlies, shipmentPM.ShipmentConsoleShipments, shipmentPM.ShipmentCarrierStatuses, shipmentPM.AWBOCIPMs, shipmentPM.ShipmentCommodities, shipmentPM.ShipmentAssemblies, shipmentPM.ShipmentStoragePricings, shipmentPM.ShipmentProductItems, shipmentPM.ShipmentUnassignedFields);
            service.Update();
        }
    }
}
