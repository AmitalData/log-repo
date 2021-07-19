using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Data.Entity;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ContainerQuery
    {
        ContainerRepository repository;
        public ContainerQuery(int tenant)
        {
            repository = new ContainerRepository(tenant);
        }
        public ContainerQuery(ContainerRepository myRepository)
        {
            this.repository = myRepository;
        }

        public ContainerPM GetSinglePM(string id, int tenant)
        {
            ContainerPM result = null;
            Container entityPoco = repository.GetSingleContainer(id, tenant);
            if (entityPoco != null)
            {
                result = new ContainerPM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    CreateDate = entityPoco.CreateDate,
                    CreatedByUserId = entityPoco.CreatedByUserId,
                    UpdateDate = entityPoco.UpdateDate,
                    UpdatedByUserId = entityPoco.UpdatedByUserId,
                    MainCarriageCarrierId = entityPoco.MainCarriageCarrierId,
                    MainCarriageCarrierNumber = entityPoco.MainCarriageCarrierNumber,
                    MainCarriageATA = entityPoco.MainCarriageATA,
                    MainCarriageATD = entityPoco.MainCarriageATD,
                    MainCarriageETA = entityPoco.MainCarriageETA,
                    MainCarriageETD = entityPoco.MainCarriageETD,
                    ContainerNumber = entityPoco.ContainerNumber,
                    MainCarriageVesselId = entityPoco.MainCarriageVesselId,
                    ShipmentPackagesId = entityPoco.ShipmentPackagesId,
                    SearchFields = entityPoco.SearchFields,
                    DischargeDate = entityPoco.DischargeDate,
                    Master = entityPoco.Master,
                    CarrierName = entityPoco.CarrierCard != null ? entityPoco.CarrierCard.EnglishName : "",
                    VesselName = entityPoco.VesselCard != null ? entityPoco.VesselCard.EnglishName : "",
                    ShipmentId = entityPoco.ShipmentId,
                    ActualEmptyPickupDate = entityPoco.ActualEmptyPickupDate,
                    EstimatedEmptyPickupDate = entityPoco.EstimatedEmptyPickupDate,
                    EstimatedGateInDate = entityPoco.EstimatedGateInDate,
                    ActualGateInDate = entityPoco.ActualGateInDate,
                    CurrentStatus = entityPoco.CurrentStatus,
                    CurrentStatusDate = entityPoco.CurrentStatusDate,
                    HasContainerException = entityPoco.HasContainerException,
                    CurrentLocation = entityPoco.CurrentLocation,
                    EmptyPickupLocation = entityPoco.EmptyPickupLocation,
                    DepartureLocation = entityPoco.DepartureLocation,
                    DestinationLocation = entityPoco.DestinationLocation,
                };
            }

            return result;
        }

        public List<ContainerPM> GetContainers(string id , int tenant)
        {
            return (from a in repository.context.Containers.Include("CarrierCard").Include("VesselCard")
                    where a.Id == id && a.Tenant == tenant
                    select new ContainerPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        CreatedByUserId = a.CreatedByUserId,
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserId = a.UpdatedByUserId,
                        MainCarriageCarrierId = a.MainCarriageCarrierId,
                        MainCarriageCarrierNumber = a.MainCarriageCarrierNumber,
                        MainCarriageATA = a.MainCarriageATA,
                        MainCarriageATD = a.MainCarriageATD,
                        MainCarriageETA = a.MainCarriageETA,
                        MainCarriageETD = a.MainCarriageETD,
                        ContainerNumber = a.ContainerNumber,
                        MainCarriageVesselId = a.MainCarriageVesselId,
                        ShipmentPackagesId = a.ShipmentPackagesId,
                        SearchFields = a.SearchFields,
                        DischargeDate = a.DischargeDate,
                        Master = a.Master,
                        CarrierName = a.CarrierCard != null ? a.CarrierCard.EnglishName : "",
                        VesselName = a.VesselCard != null ? a.VesselCard.EnglishName : "",
                        ShipmentId = a.ShipmentId,
                        ActualEmptyPickupDate = a.ActualEmptyPickupDate,
                        EstimatedEmptyPickupDate = a.EstimatedEmptyPickupDate,
                        EstimatedGateInDate = a.EstimatedGateInDate,
                        ActualGateInDate = a.ActualGateInDate,
                        CurrentStatus = a.CurrentStatus,
                        CurrentStatusDate = a.CurrentStatusDate,
                        HasContainerException = a.HasContainerException,
                        CurrentLocation = a.CurrentLocation,
                        EmptyPickupLocation = a.EmptyPickupLocation,
                        DepartureLocation = a.DepartureLocation,
                        DestinationLocation = a.DestinationLocation,
                    }).ToList();
        }

        public IQueryable<ContainerList> GetIQueryableEntityList(IQueryable<Container> iQueryable)
        {
            IQueryable<ContainerList> result = from entity in iQueryable.Include("CarrierCard").Include("VesselCard")
                                                 select new ContainerList()
                                                 {
                                                     Id = entity.Id,
                                                     Tenant = entity.Tenant,
                                                     CreateDate = entity.CreateDate,
                                                     CreatedByUserId = entity.CreatedByUserId,
                                                     UpdateDate = entity.UpdateDate,
                                                     UpdatedByUserId = entity.UpdatedByUserId,
                                                     MainCarriageCarrierId = entity.MainCarriageCarrierId,
                                                     MainCarriageCarrierNumber = entity.MainCarriageCarrierNumber,
                                                     MainCarriageATA = entity.MainCarriageATA,
                                                     MainCarriageATD = entity.MainCarriageATD,
                                                     MainCarriageETA = entity.MainCarriageETA,
                                                     MainCarriageETD = entity.MainCarriageETD,
                                                     ContainerNumber = entity.ContainerNumber,
                                                     MainCarriageVesselId = entity.MainCarriageVesselId,
                                                     ShipmentPackagesId = entity.ShipmentPackagesId,
                                                     SearchFields = entity.SearchFields,
                                                     DischargeDate = entity.DischargeDate,
                                                     Master = entity.Master,
                                                     CarrierName = entity.CarrierCard != null ? entity.CarrierCard.EnglishName : "",
                                                     VesselName = entity.VesselCard != null ? entity.VesselCard.EnglishName : "",
                                                     ShipmentId = entity.ShipmentId,
                                                     ActualEmptyPickupDate = entity.ActualEmptyPickupDate,
                                                     EstimatedEmptyPickupDate = entity.EstimatedEmptyPickupDate,
                                                     EstimatedGateInDate = entity.EstimatedGateInDate,
                                                     ActualGateInDate = entity.ActualGateInDate,
                                                     CurrentStatus = entity.CurrentStatus,
                                                     CurrentStatusDate = entity.CurrentStatusDate,
                                                     HasContainerException = entity.HasContainerException,
                                                     CurrentLocation = entity.CurrentLocation,
                                                     EmptyPickupLocation = entity.EmptyPickupLocation,
                                                     DepartureLocation = entity.DepartureLocation,
                                                     DestinationLocation = entity.DestinationLocation,
                                                 };
            return result;
        }

        public ContainerPM GetContainerByShipmentPackagesId(string shipmentPackageId, int tenant)
        {
            ContainerPM containerPM = null;
            Container container = repository.GetContainerByShipmentPackagesId(shipmentPackageId, tenant);
            if (container != null)
            {
                containerPM = new ContainerPM()
                {
                    Id = container.Id,
                    Tenant = container.Tenant,
                    CreateDate = container.CreateDate,
                    CreatedByUserId = container.CreatedByUserId,
                    UpdateDate = container.UpdateDate,
                    UpdatedByUserId = container.UpdatedByUserId,
                    MainCarriageCarrierId = container.MainCarriageCarrierId,
                    MainCarriageCarrierNumber = container.MainCarriageCarrierNumber,
                    MainCarriageATA = container.MainCarriageATA,
                    MainCarriageATD = container.MainCarriageATD,
                    MainCarriageETA = container.MainCarriageETA,
                    MainCarriageETD = container.MainCarriageETD,
                    ContainerNumber = container.ContainerNumber,
                    MainCarriageVesselId = container.MainCarriageVesselId,
                    ShipmentPackagesId = container.ShipmentPackagesId,
                    SearchFields = container.SearchFields,
                    DischargeDate = container.DischargeDate,
                    Master = container.Master,
                    ShipmentId = container.ShipmentId,
                    ActualEmptyPickupDate = container.ActualEmptyPickupDate,
                    EstimatedEmptyPickupDate = container.EstimatedEmptyPickupDate,
                    EstimatedGateInDate = container.EstimatedGateInDate,
                    ActualGateInDate = container.ActualGateInDate,
                    CurrentStatus = container.CurrentStatus,
                    CurrentStatusDate = container.CurrentStatusDate,
                    HasContainerException = container.HasContainerException,
                    CurrentLocation = container.CurrentLocation,
                    EmptyPickupLocation = container.EmptyPickupLocation,
                    DepartureLocation = container.DepartureLocation,
                    DestinationLocation = container.DestinationLocation,
                };
            }
            return containerPM;
        }

        public ContainerPM GetContainerByNumberAndShipmentIdAndTenant(string containerNumber, string shipmentId, int tenant)
        {
            ContainerPM result = null;
            Container entityPoco = (from container in repository.context.Containers
                                    where container.Tenant == tenant && container.ContainerNumber == containerNumber && container.ShipmentId == shipmentId
                                    select container).FirstOrDefault();

            if(entityPoco != null)
            {
                result = new ContainerPM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    CreateDate = entityPoco.CreateDate,
                    CreatedByUserId = entityPoco.CreatedByUserId,
                    UpdateDate = entityPoco.UpdateDate,
                    UpdatedByUserId = entityPoco.UpdatedByUserId,
                    MainCarriageCarrierId = entityPoco.MainCarriageCarrierId,
                    MainCarriageCarrierNumber = entityPoco.MainCarriageCarrierNumber,
                    MainCarriageATA = entityPoco.MainCarriageATA,
                    MainCarriageATD = entityPoco.MainCarriageATD,
                    MainCarriageETA = entityPoco.MainCarriageETA,
                    MainCarriageETD = entityPoco.MainCarriageETD,
                    ContainerNumber = entityPoco.ContainerNumber,
                    MainCarriageVesselId = entityPoco.MainCarriageVesselId,
                    ShipmentPackagesId = entityPoco.ShipmentPackagesId,
                    SearchFields = entityPoco.SearchFields,
                    DischargeDate = entityPoco.DischargeDate,
                    Master = entityPoco.Master,
                    CarrierName = entityPoco.CarrierCard != null ? entityPoco.CarrierCard.EnglishName : "",
                    VesselName = entityPoco.VesselCard != null ? entityPoco.VesselCard.EnglishName : "",
                    ShipmentId = entityPoco.ShipmentId,
                    ActualEmptyPickupDate = entityPoco.ActualEmptyPickupDate,
                    EstimatedEmptyPickupDate = entityPoco.EstimatedEmptyPickupDate,
                    EstimatedGateInDate = entityPoco.EstimatedGateInDate,
                    ActualGateInDate = entityPoco.ActualGateInDate,
                    CurrentStatus = entityPoco.CurrentStatus,
                    CurrentStatusDate = entityPoco.CurrentStatusDate,
                    HasContainerException = entityPoco.HasContainerException,
                    CurrentLocation = entityPoco.CurrentLocation,
                    EmptyPickupLocation = entityPoco.EmptyPickupLocation,
                    DepartureLocation = entityPoco.DepartureLocation,
                    DestinationLocation = entityPoco.DestinationLocation,
                };
            }

            return result;
        }
    }
}
