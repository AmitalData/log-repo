using System;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentComputedFieldsQuery
    {
        ShipmentComputedFieldsRepository repository;

        public ShipmentComputedFieldsQuery(int tenant)
        {
            repository = new ShipmentComputedFieldsRepository(tenant);
        }


        public ShipmentComputedFieldsQuery(ShipmentComputedFieldsRepository repository)
        {
            this.repository = repository;
        }


        public ShipmentComputedFieldsPM GetSinglePM(string id)
        {
            return (from a in repository.context.ShipmentComputedFields
                    where a.Id == id
                    select new ShipmentComputedFieldsPM()
                    {
                        Tenant = a.Tenant,
                        IsMissingDocuments = a.IsMissingDocuments,
                        DocumentsSearchFields = a.DocumentsSearchFields,
                        MissingDocumentsCount = a.MissingDocumentsCount,
                        MissingDocumentsNames = a.MissingDocumentsNames,
                        IsRequestedDocuments = a.IsRequestedDocuments,
                        RequestedDocumentsCount = a.RequestedDocumentsCount,
                        NumberOfHouses = a.NumberOfHouses,
                        IsDigitalSignRequired = a.IsDigitalSignRequired,
                        IsDepositionRequired = a.IsDepositionRequired,
                        ImporterDepositionRequestDetails = a.ImporterDepositionRequestDetails,
                        Commodity = a.Commodity,
                        FirstPickupLocation = a.FirstPickupLocation,
                        ContainersNumbers = a.ContainersNumbers,
                        FirstPickupATD = a.FirstPickupATD,
                        FirstPickupATA = a.FirstPickupATA,
                        FinalDeliveryETD = a.FinalDeliveryETD,
                        FinalDeliveryETA = a.FinalDeliveryETA,
                        FinalDeliveryATD = a.FinalDeliveryATD,
                        FinalDeliveryATA = a.FinalDeliveryATA,
                        OperationallyClosedByUserId = a.OperationallyClosedByUserId,
                        NumberOfDeliveries = a.NumberOfDeliveries,
                        ImportDeclarationDate = a.ImportDeclarationDate,
                        ImportDeclarationNumber = a.ImportDeclarationNumber,
                        LastPickupETA = a.LastPickupETA,
                        LastPickupETD = a.LastPickupETD,
                        LastPickupATA = a.LastPickupATA,
                        LastPickupATD = a.LastPickupATD,
                        DeliveryToCity = a.DeliveryToCity,
                        DeliveryToPortId = a.DeliveryToPortId,
                        ContainsDangerousGoods = a.ContainsDangerousGoods,
                        DeliveryFrom = a.DeliveryFrom,
                        DeliveryTo = a.DeliveryTo,
                        PickupFrom = a.PickupFrom,
                        PickupTo = a.PickupTo,
                    }).FirstOrDefault();
        }

        public IQueryable<ShipmentComputedFieldsList> GetIQueryableEntityList(IQueryable<ShipmentComputedFields> iQueryable)
        {
            IQueryable<ShipmentComputedFieldsList> result = (from a in iQueryable
                                                             select new ShipmentComputedFieldsList()
                                                             {
                                                                 Id = a.Id,
                                                                 Tenant = a.Tenant,
                                                                 IsMissingDocuments = a.IsMissingDocuments,
                                                                 DocumentsSearchFields = a.DocumentsSearchFields,
                                                                 MissingDocumentsCount = a.MissingDocumentsCount,
                                                                 MissingDocumentsNames = a.MissingDocumentsNames,
                                                                 IsRequestedDocuments = a.IsRequestedDocuments,
                                                                 RequestedDocumentsCount = a.RequestedDocumentsCount,
                                                                 NumberOfHouses = a.NumberOfHouses,
                                                                 IsDigitalSignRequired = a.IsDigitalSignRequired,
                                                                 IsDepositionRequired = a.IsDepositionRequired,
                                                                 ImporterDepositionRequestDetails = a.ImporterDepositionRequestDetails,
                                                                 Commodity = a.Commodity,
                                                                 FirstPickupLocation = a.FirstPickupLocation,
                                                                 ContainersNumbers = a.ContainersNumbers,
                                                                 FirstPickupATD = a.FirstPickupATD,
                                                                 FirstPickupATA = a.FirstPickupATA,
                                                                 FinalDeliveryETD = a.FinalDeliveryETD,
                                                                 FinalDeliveryETA = a.FinalDeliveryETA,
                                                                 FinalDeliveryATD = a.FinalDeliveryATD,
                                                                 FinalDeliveryATA = a.FinalDeliveryATA,
                                                                 OperationallyClosedByUserId = a.OperationallyClosedByUserId,
                                                                 NumberOfDeliveries = a.NumberOfDeliveries,
                                                                 ImportDeclarationDate = a.ImportDeclarationDate,
                                                                 ImportDeclarationNumber = a.ImportDeclarationNumber,
                                                                 LastPickupETA = a.LastPickupETA,
                                                                 LastPickupETD = a.LastPickupETD,
                                                                 LastPickupATA = a.LastPickupATA,
                                                                 LastPickupATD = a.LastPickupATD,
                                                                 DeliveryToCity = a.DeliveryToCity,
                                                                 DeliveryToPortId = a.DeliveryToPortId,
                                                                 ContainsDangerousGoods = a.ContainsDangerousGoods,
                                                                 DeliveryFrom = a.DeliveryFrom,
                                                                 DeliveryTo = a.DeliveryTo,
                                                                 PickupFrom = a.PickupFrom,
                                                                 PickupTo = a.PickupTo,
                                                                 OperationallyClosedByUserName = a.OperationallyClosedByUser.Contact.Name,
                                                             });
            return result;
        }
    }
}