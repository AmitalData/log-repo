using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentCarrierStatusQuery
    {  
        ShipmentCarrierStatusRepository repository;
         
        public ShipmentCarrierStatusQuery(int tenant)
        {
            repository = new ShipmentCarrierStatusRepository(tenant);
        }

        public ShipmentCarrierStatusQuery(ShipmentCarrierStatusRepository ShipmentCarrierStatusRepository)
        {
            repository = ShipmentCarrierStatusRepository;
        }

        public ShipmentCarrierStatusPM GetSinglePM(string id, int tenant)
        {
            ShipmentCarrierStatusPM ShipmentCarrierStatus = (from a in repository.Context.ShipmentCarrierStatuses.Include("LocationPort").Include("AWBStatus") 
                                                             where a.Id == id && a.Tenant == tenant
                                                             select new ShipmentCarrierStatusPM()
                                                             {
                                                                 Id = a.Id,
                                                                 Tenant = a.Tenant,
                                                                 Weight = a.Weight,
                                                                 Details = a.Details,
                                                                 EventDate = a.EventDate != null ? a.EventDate : a.ReceivingDate,
                                                                 FlightNumber = a.FlightNumber,
                                                                 FromPortId = a.FromPortId,
                                                                 Partial = a.Partial,
                                                                 Pieces = a.Pieces,
                                                                 ReceivingDate = a.ReceivingDate,
                                                                 RecordHash = a.RecordHash,
                                                                 ShipmentId = a.ShipmentId,
                                                                 Status = a.Status,
                                                                 ToPortId = a.ToPortId,
                                                                 Location = a.Location,
                                                                 LocationCode = a.LocationPort == null ? "" : a.LocationPort.Code,
                                                                 LocationName = a.LocationPort == null ? "" : a.LocationPort.EnglishName,
                                                                 AirlineName = a.AirlineName,
                                                                 DepartureDate = a.DepartureDate,
                                                                 ArrivalDate = a.ArrivalDate,
                                                                 TimeOfArrivalInfo = a.TimeOfArrivalInfo,
                                                                 TimeOfDepartureInfo = a.TimeOfDepartureInfo,
                                                             }).FirstOrDefault();
            return ShipmentCarrierStatus;
        }

        public IQueryable<ShipmentCarrierStatusPM> GetShipmentCarrierStatusPMsByTenant(int tenant)
        {
            var result = from a in repository.Context.ShipmentCarrierStatuses.Include("LocationPort").Include("AWBStatus") 
                                 where a.Tenant == tenant
                                 select new ShipmentCarrierStatusPM()
                                 {
                                     Id = a.Id,
                                     Tenant = a.Tenant,
                                     Weight = a.Weight,
                                     Details = a.Details,
                                     EventDate = a.EventDate != null ? a.EventDate : a.ReceivingDate,
                                     FlightNumber = a.FlightNumber,
                                     FromPortId = a.FromPortId,
                                     Partial = a.Partial,
                                     Pieces = a.Pieces,
                                     ReceivingDate = a.ReceivingDate,
                                     RecordHash = a.RecordHash,
                                     ShipmentId = a.ShipmentId,
                                     Status = a.Status,
                                     ToPortId = a.ToPortId,
                                     Location = a.Location,
                                     LocationCode = a.LocationPort == null ? "" : a.LocationPort.Code,
                                     LocationName = a.LocationPort == null ? "" : a.LocationPort.EnglishName,
                                     AirlineName = a.AirlineName,
                                     DepartureDate = a.DepartureDate,
                                     ArrivalDate = a.ArrivalDate,
                                     TimeOfArrivalInfo = a.TimeOfArrivalInfo,
                                     TimeOfDepartureInfo = a.TimeOfDepartureInfo,
                                 };
            return result;
        }

        public IQueryable<ShipmentCarrierStatusPM> GetShipmentCarrierStatusPMsByShipmentId(Shipment shipment)
        {
            IQueryable<ShipmentCarrierStatusPM> myResult = null;
            IQueryable<ShipmentCarrierStatus> iQueryable = null;

            if (shipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipment.MasterShipmentDataId))
            {
                iQueryable = (from a in repository.Context.ShipmentCarrierStatuses.Include("LocationPort").Include("AWBStatus")
                              where a.Tenant == shipment.Tenant
                              && (a.ShipmentId == shipment.Id || a.ShipmentId == shipment.MasterShipmentDataId)
                              select a);
            }

            else
            {
                iQueryable = (from a in repository.Context.ShipmentCarrierStatuses.Include("LocationPort").Include("AWBStatus")
                              where a.Tenant == shipment.Tenant 
                              && a.ShipmentId == shipment.Id
                              select a);
            }

            myResult = (from a in iQueryable
                        select new ShipmentCarrierStatusPM()
                        {
                            Id = a.Id,
                            Tenant = a.Tenant,
                            Weight = a.Weight,
                            Details = a.Details,
                            EventDate = a.EventDate != null ? a.EventDate : a.ReceivingDate,
                            FlightNumber = a.FlightNumber,
                            FromPortId = a.FromPortId,
                            Partial = a.Partial,
                            Pieces = a.Pieces,
                            ReceivingDate = a.ReceivingDate,
                            RecordHash = a.RecordHash,
                            ShipmentId = a.ShipmentId,
                            Status = a.Status,
                            ToPortId = a.ToPortId,
                            Location = a.Location,
                            StatusName = a.AWBStatus == null ? "" : a.AWBStatus.Name,
                            LocationCode = a.LocationPort == null ? "" : a.LocationPort.Code,
                            LocationName = a.LocationPort == null ? "" : a.LocationPort.EnglishName,
                            AirlineName = a.AirlineName,
                            DepartureDate = a.DepartureDate,
                            ArrivalDate = a.ArrivalDate,
                            TimeOfArrivalInfo = a.TimeOfArrivalInfo,
                            TimeOfDepartureInfo = a.TimeOfDepartureInfo,
                        }).OrderBy(a => a.Id);


            return myResult;
        }

        public IQueryable<ShipmentCarrierStatusList> GetIQueryableEntityList(IQueryable<ShipmentCarrierStatus> iQueryable)
        {
            IQueryable<ShipmentCarrierStatusList> result = from a in iQueryable.Include("LocationPort").Include("AWBStatus") 
                                                           select new ShipmentCarrierStatusList()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  Weight = a.Weight,
                                                  Details = a.Details,
                                                  EventDate = a.EventDate!=null?a.EventDate:a.ReceivingDate,
                                                  FlightNumber = a.FlightNumber,
                                                  FromPortId = a.FromPortId,
                                                  Partial = a.Partial,
                                                  Pieces = a.Pieces,
                                                  ReceivingDate = a.ReceivingDate,
                                                  RecordHash = a.RecordHash,
                                                  ShipmentId = a.ShipmentId,
                                                  Status = a.Status,
                                                  ToPortId = a.ToPortId, 
                                                  Location=a.Location,
                                                  StatusName = a.AWBStatus == null ? "" : a.AWBStatus.Name,
                                                  LocationCode = a.LocationPort == null ? "" : a.LocationPort.Code,
                                                  LocationName = a.LocationPort == null ? "" : a.LocationPort.EnglishName,
                                                  AirlineName = a.AirlineName,
                                                  DepartureDate = a.DepartureDate,
                                                  ArrivalDate = a.ArrivalDate,
                                                  TimeOfArrivalInfo = a.TimeOfArrivalInfo,
                                                  TimeOfDepartureInfo = a.TimeOfDepartureInfo,
                                              };
            return result;
        }

        public IQueryable<ShipmentCarrierStatusList> GetShipmentCarrierStatusLists(string shipmentId, int tenant)
        {
            //shipmentRepository = new ShipmentRepository(tenant);
            Shipment shipment = (from d in repository.Context.Shipments where d.Id == shipmentId select d).FirstOrDefault();

            IQueryable<ShipmentCarrierStatusList> iQueryable = null;

            if (shipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipment.MasterShipmentDataId))
            {
                iQueryable = (from a in repository.Context.ShipmentCarrierStatuses.Include("LocationPort").Include("AWBStatus")
                              where a.Tenant == shipment.Tenant
                              && (a.ShipmentId == shipment.Id || a.ShipmentId == shipment.MasterShipmentDataId)
                              select new ShipmentCarrierStatusList()
                              {
                                  Id = a.Id,
                                  Tenant = a.Tenant,
                                  Weight = a.Weight,
                                  Details = a.Details,
                                  EventDate = a.EventDate != null ? a.EventDate : a.ReceivingDate,
                                  FlightNumber = a.FlightNumber,
                                  FromPortId = a.FromPortId,
                                  Partial = a.Partial,
                                  Pieces = a.Pieces,
                                  ReceivingDate = a.ReceivingDate,
                                  RecordHash = a.RecordHash,
                                  ShipmentId = a.ShipmentId,
                                  Status = a.Status,
                                  ToPortId = a.ToPortId,
                                  Location = a.Location,
                                  StatusName = a.AWBStatus == null ? "" : a.AWBStatus.Name,
                                  LocationCode = a.LocationPort == null ? "" : a.LocationPort.Code,
                                  LocationName = a.LocationPort == null ? "" : a.LocationPort.EnglishName,
                                  AirlineName = a.AirlineName,
                                  DepartureDate = a.DepartureDate,
                                  ArrivalDate = a.ArrivalDate,
                                  TimeOfArrivalInfo = a.TimeOfArrivalInfo,
                                  TimeOfDepartureInfo = a.TimeOfDepartureInfo,
                              });
            }

            else
            {
                iQueryable = (from a in repository.Context.ShipmentCarrierStatuses.Include("LocationPort").Include("AWBStatus")
                              where a.Tenant == shipment.Tenant
                              && a.ShipmentId == shipment.Id
                              select new ShipmentCarrierStatusList()
                              {
                                  Id = a.Id,
                                  Tenant = a.Tenant,
                                  Weight = a.Weight,
                                  Details = a.Details,
                                  EventDate = a.EventDate != null ? a.EventDate : a.ReceivingDate,
                                  FlightNumber = a.FlightNumber,
                                  FromPortId = a.FromPortId,
                                  Partial = a.Partial,
                                  Pieces = a.Pieces,
                                  ReceivingDate = a.ReceivingDate,
                                  RecordHash = a.RecordHash,
                                  ShipmentId = a.ShipmentId,
                                  Status = a.Status,
                                  ToPortId = a.ToPortId,
                                  Location = a.Location,
                                  StatusName = a.AWBStatus == null ? "" : a.AWBStatus.Name,
                                  LocationCode = a.LocationPort == null ? "" : a.LocationPort.Code,
                                  LocationName = a.LocationPort == null ? "" : a.LocationPort.EnglishName,
                                  AirlineName = a.AirlineName,
                                  DepartureDate = a.DepartureDate,
                                  ArrivalDate = a.ArrivalDate,
                                  TimeOfArrivalInfo = a.TimeOfArrivalInfo,
                                  TimeOfDepartureInfo = a.TimeOfDepartureInfo,
                              });
            }

            return iQueryable;
        }
    }
}