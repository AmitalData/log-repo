using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {

        [Query(HasSideEffects = true)]
        public IQueryable<ContainerFollowUpList> GetContainerFollowUpFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IShipmentsContext MyContext = ShipmentsContext.GetContext(tenant);
            ShipmentPackageRepository entityRepository = new ShipmentPackageRepository(MyContext);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentPackage> entityPocos = entityRepository.GetShipmentPackages(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            entityPocos = filter.GetFilteredQuery<ShipmentPackage>(nonListQueryOperation, entityPocos);
            int skippedEntities = queryOperations.PageIndex;

            IQueryable<ContainerFollowUpList> query2 = (from f in entityPocos.Include("PackageType")
                                                             join db_Shipments in MyContext.Shipments.Include("Direction").Include("TransportMode").Include("ShipmentLevel").Include("ShipmentType").Include("ShipperCard").Include("ConsigneeCard").Include("CustomerCard").Include("CustomerCard.PrimaryContact").Include("ShipmentMasterData").Include("ShipmentMasterData.MainCarriageCarrierCard").Include("ShipmentMasterData.MainCarriageVessel")
                                                             on f.ShipmentId equals db_Shipments.Id into PackagesShipments
                                                             from myShipment in PackagesShipments
                                                             where f.Tenant == tenant && myShipment.Tenant == tenant
                                                             && myShipment.IsCancelled == false
                                                             select new ContainerFollowUpList()
                                                             {
                                                                 Id = f.Id + ":" + f.ShipmentId,
                                                                 Tenant = f.Tenant,
                                                                 ShipmentId = f.ShipmentId,
                                                                 ShipperSeal = f.ShipperSeal,
                                                                 Volume = f.Volume,
                                                                 ContainerNumber = f.ContainerNumber,
                                                                 IsDangerous = f.IsDangerous,
                                                                 Description = f.Description,
                                                                 MarksAndNumbers = f.MarksAndNumbers,
                                                                 ContainerTypeName = f.PackageType == null ? null : f.PackageType.EnglishName,
                                                                 IsDeliveryFU = f.IsDeliveryFU,
                                                                 DeliveryId = f.DeliveryId,
                                                                 DeliveryETD = f.DeliveryETD,
                                                                 DeliveryATD = f.DeliveryATD,
                                                                 DeliveryATA = f.DeliveryATA,
                                                                 DeliveryETA = f.DeliveryETA,
                                                                 DeliveryFrom = f.DeliveryFrom,
                                                                 DeliveryTo = f.DeliveryTo,
                                                                 DeliveryDeparture = f.DeliveryATD != null ? f.DeliveryATD : f.DeliveryETD,
                                                                 DeliveryArrival = f.DeliveryATA != null ? f.DeliveryATA : f.DeliveryETA,
                                                                 IsEmptyContainerReturnFU = f.IsEmptyContainerReturnFU,
                                                                 EmptyContainerReturnId = f.EmptyContainerReturnId,
                                                                 EmptyContainerReturnETD = f.EmptyContainerReturnETD,
                                                                 EmptyContainerReturnATD = f.EmptyContainerReturnATD,
                                                                 EmptyContainerReturnETA = f.EmptyContainerReturnETA,
                                                                 EmptyContainerReturnATA = f.EmptyContainerReturnATA,
                                                                 EmptyContainerReturnFrom = f.EmptyContainerReturnFrom,
                                                                 EmptyContainerReturnTo = f.EmptyContainerReturnTo,
                                                                 ReturnDeparture = f.EmptyContainerReturnATD != null ? f.EmptyContainerReturnATD : f.EmptyContainerReturnETD,
                                                                 ReturnArrival = f.EmptyContainerReturnATA != null ? f.EmptyContainerReturnATA : f.EmptyContainerReturnETA,

                                                                 DirectionId = myShipment.DirectionId,
                                                                 TransportModeId = myShipment.TransportModeId,
                                                                 ShipmentNumber = myShipment.ShipmentNumber,
                                                                 House = myShipment.House,
                                                                 ShipmentLevelCode = myShipment.ShipmentLevelCode,
                                                                 StatusId = myShipment.StatusId,
                                                                 ConsigneeReference = (myShipment.ConsigneeReference1 == null || myShipment.ConsigneeReference1 == "") ? myShipment.ConsigneeReference2 : ((myShipment.ConsigneeReference2 == null || myShipment.ConsigneeReference2 == "") ? myShipment.ConsigneeReference1 : myShipment.ConsigneeReference1 + "," + myShipment.ConsigneeReference2),
                                                                 DirectionName = myShipment.Direction == null ? null : myShipment.Direction.Name,
                                                                 TransportModeName = myShipment.TransportMode == null ? null : myShipment.TransportMode.Name,
                                                                 ShipmentLevelName = myShipment.ShipmentLevel == null ? null : myShipment.ShipmentLevel.Name,
                                                                 ShipmentType = myShipment.ShipmentType == null ? null : myShipment.ShipmentType.Name,
                                                                 ShipperName = myShipment.ShipperCard == null ? null : myShipment.ShipperCard.EnglishName,
                                                                 ConsigneeName = myShipment.ConsigneeCard == null ? null : myShipment.ConsigneeCard.EnglishName,
                                                                 CustomerName = myShipment.CustomerCard == null ? null : myShipment.CustomerCard.EnglishName,
                                                                 LongMaster = myShipment.ShipmentMasterData == null ? null : (myShipment.TransportModeId == "A" ? (!string.IsNullOrEmpty(myShipment.ShipmentMasterData.AirlinePrefix) && !string.IsNullOrEmpty(myShipment.ShipmentMasterData.Master) ? myShipment.ShipmentMasterData.AirlinePrefix + "-" + myShipment.ShipmentMasterData.Master : "") : myShipment.ShipmentMasterData.Master),
                                                                 CarrierName = myShipment.ShipmentMasterData == null ? null : (myShipment.ShipmentMasterData.MainCarriageCarrierCard == null ? null : myShipment.ShipmentMasterData.MainCarriageCarrierCard.EnglishName),
                                                                 CustomerContactName = myShipment.CustomerCard == null ? null : (myShipment.CustomerCard.PrimaryContact == null ? null : myShipment.CustomerCard.PrimaryContact.EnglishName),

                                                                 ShipperId = myShipment.ShipperId,
                                                                 ConsigneeId = myShipment.ConsigneeId,
                                                                 CustomerId = myShipment.CustomerId,
                                                                 CarrierId = myShipment.ShipmentMasterData == null ? null : myShipment.ShipmentMasterData.MainCarriageCarrierId,
                                                                 SearchFields = myShipment.SearchFields,

                                                                 ShipmentNotes = myShipment.Notes,
                                                                 VesselName = myShipment.ShipmentMasterData == null ? null : (myShipment.ShipmentMasterData.MainCarriageVessel == null ? null : myShipment.ShipmentMasterData.MainCarriageVessel.EnglishName),
                                                                 IsCancelled = myShipment.IsCancelled,
                                                                 ShipmentTypeId = myShipment.ShipmentTypeId,
                                                                 DeliveryTransportModeCode = f.DeliveryTransportModeCode,
                                                                 DeliveryTransportModeName = f.DeliveryTransportMode != null ? f.DeliveryTransportMode.Name : "",
                                                                 ECRTransportModeCode = f.ECRTransportModeCode,
                                                                 ECRTransportModeName = f.ECRTransportMode != null ? f.ECRTransportMode.Name : ""
                                                             });

            ContainerFollowUpCustomFilter customfilters = new ContainerFollowUpCustomFilter(tenant);
            query2 = customfilters.GetFilteredQuery(queryOperations, query2);

            query2 = filter.GetFilteredQuery<ContainerFollowUpList>(listQueryOperation, query2);


            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ContainerFollowUpList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ContainerFollowUp", tenant).ToList();

                ObjectField objectField = (from a in entityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<ContainerFollowUpList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ContainerFollowUpList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ContainerFollowUpList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ContainerFollowUpList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ContainerFollowUpList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedEntities);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetContainerFollowUpFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IShipmentsContext MyContext = ShipmentsContext.GetContext(tenant);
            ShipmentPackageRepository entityRepository = new ShipmentPackageRepository(MyContext);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentPackage> entityPocos = entityRepository.GetShipmentPackages(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            entityPocos = filter.GetFilteredQuery<ShipmentPackage>(nonListQueryOperation, entityPocos);

            IQueryable<ContainerFollowUpList> query2 = (from f in entityPocos.Include("PackageType")
                                                        join db_Shipments in MyContext.Shipments.Include("Direction").Include("TransportMode").Include("ShipmentLevel").Include("ShipmentType").Include("ShipperCard").Include("ConsigneeCard").Include("CustomerCard").Include("CustomerCard.PrimaryContact").Include("ShipmentMasterData").Include("ShipmentMasterData.MainCarriageCarrierCard").Include("ShipmentMasterData.MainCarriageVessel")
                                                        on f.ShipmentId equals db_Shipments.Id into PackagesShipments
                                                        from myShipment in PackagesShipments
                                                        where f.Tenant == tenant && myShipment.Tenant == tenant
                                                        && myShipment.IsCancelled == false
                                                        select new ContainerFollowUpList()
                                                        {
                                                            Id = f.Id + ":" + f.ShipmentId,
                                                            Tenant = f.Tenant,
                                                            ShipmentId = f.ShipmentId,
                                                            ShipperSeal = f.ShipperSeal,
                                                            Volume = f.Volume,
                                                            ContainerNumber = f.ContainerNumber,
                                                            IsDangerous = f.IsDangerous,
                                                            Description = f.Description,
                                                            MarksAndNumbers = f.MarksAndNumbers,
                                                            ContainerTypeName = f.PackageType == null ? null : f.PackageType.EnglishName,
                                                            IsDeliveryFU = f.IsDeliveryFU,
                                                            DeliveryId = f.DeliveryId,
                                                            DeliveryETD = f.DeliveryETD,
                                                            DeliveryATD = f.DeliveryATD,
                                                            DeliveryATA = f.DeliveryATA,
                                                            DeliveryETA = f.DeliveryETA,
                                                            DeliveryFrom = f.DeliveryFrom,
                                                            DeliveryTo = f.DeliveryTo,
                                                            DeliveryDeparture = f.DeliveryATD != null ? f.DeliveryATD : f.DeliveryETD,
                                                            DeliveryArrival = f.DeliveryATA != null ? f.DeliveryATA : f.DeliveryETA,
                                                            IsEmptyContainerReturnFU = f.IsEmptyContainerReturnFU,
                                                            EmptyContainerReturnId = f.EmptyContainerReturnId,
                                                            EmptyContainerReturnETD = f.EmptyContainerReturnETD,
                                                            EmptyContainerReturnATD = f.EmptyContainerReturnATD,
                                                            EmptyContainerReturnETA = f.EmptyContainerReturnETA,
                                                            EmptyContainerReturnATA = f.EmptyContainerReturnATA,
                                                            EmptyContainerReturnFrom = f.EmptyContainerReturnFrom,
                                                            EmptyContainerReturnTo = f.EmptyContainerReturnTo,
                                                            ReturnDeparture = f.EmptyContainerReturnATD != null ? f.EmptyContainerReturnATD : f.EmptyContainerReturnETD,
                                                            ReturnArrival = f.EmptyContainerReturnATA != null ? f.EmptyContainerReturnATA : f.EmptyContainerReturnETA,

                                                            DirectionId = myShipment.DirectionId,
                                                            TransportModeId = myShipment.TransportModeId,
                                                            ShipmentNumber = myShipment.ShipmentNumber,
                                                            House = myShipment.House,
                                                            ShipmentLevelCode = myShipment.ShipmentLevelCode,
                                                            StatusId = myShipment.StatusId,
                                                            ConsigneeReference = (myShipment.ConsigneeReference1 == null || myShipment.ConsigneeReference1 == "") ? myShipment.ConsigneeReference2 : ((myShipment.ConsigneeReference2 == null || myShipment.ConsigneeReference2 == "") ? myShipment.ConsigneeReference1 : myShipment.ConsigneeReference1 + "," + myShipment.ConsigneeReference2),
                                                            DirectionName = myShipment.Direction == null ? null : myShipment.Direction.Name,
                                                            TransportModeName = myShipment.TransportMode == null ? null : myShipment.TransportMode.Name,
                                                            ShipmentLevelName = myShipment.ShipmentLevel == null ? null : myShipment.ShipmentLevel.Name,
                                                            ShipmentType = myShipment.ShipmentType == null ? null : myShipment.ShipmentType.Name,
                                                            ShipperName = myShipment.ShipperCard == null ? null : myShipment.ShipperCard.EnglishName,
                                                            ConsigneeName = myShipment.ConsigneeCard == null ? null : myShipment.ConsigneeCard.EnglishName,
                                                            CustomerName = myShipment.CustomerCard == null ? null : myShipment.CustomerCard.EnglishName,
                                                            LongMaster = myShipment.ShipmentMasterData == null ? null : (myShipment.TransportModeId == "A" ? (!string.IsNullOrEmpty(myShipment.ShipmentMasterData.AirlinePrefix) && !string.IsNullOrEmpty(myShipment.ShipmentMasterData.Master) ? myShipment.ShipmentMasterData.AirlinePrefix + "-" + myShipment.ShipmentMasterData.Master : "") : myShipment.ShipmentMasterData.Master),
                                                            CarrierName = myShipment.ShipmentMasterData == null ? null : (myShipment.ShipmentMasterData.MainCarriageCarrierCard == null ? null : myShipment.ShipmentMasterData.MainCarriageCarrierCard.EnglishName),
                                                            CustomerContactName = myShipment.CustomerCard == null ? null : (myShipment.CustomerCard.PrimaryContact == null ? null : myShipment.CustomerCard.PrimaryContact.EnglishName),

                                                            ShipperId = myShipment.ShipperId,
                                                            ConsigneeId = myShipment.ConsigneeId,
                                                            CustomerId = myShipment.CustomerId,
                                                            CarrierId = myShipment.ShipmentMasterData == null ? null : myShipment.ShipmentMasterData.MainCarriageCarrierId,
                                                            SearchFields = myShipment.SearchFields,

                                                            ShipmentNotes = myShipment.Notes,
                                                            VesselName = myShipment.ShipmentMasterData == null ? null : (myShipment.ShipmentMasterData.MainCarriageVessel == null ? null : myShipment.ShipmentMasterData.MainCarriageVessel.EnglishName),
                                                            IsCancelled = myShipment.IsCancelled,
                                                            ShipmentTypeId = myShipment.ShipmentTypeId,
                                                            DeliveryTransportModeCode = f.DeliveryTransportModeCode,
                                                            DeliveryTransportModeName = f.DeliveryTransportMode != null ? f.DeliveryTransportMode.Name : "",
                                                            ECRTransportModeCode = f.ECRTransportModeCode,
                                                            ECRTransportModeName = f.ECRTransportMode != null ? f.ECRTransportMode.Name : ""
                                                        });

            ContainerFollowUpCustomFilter customfilters = new ContainerFollowUpCustomFilter(tenant);
            query2 = customfilters.GetFilteredQuery(queryOperations, query2);

            query2 = filter.GetFilteredQuery<ContainerFollowUpList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

    }
}