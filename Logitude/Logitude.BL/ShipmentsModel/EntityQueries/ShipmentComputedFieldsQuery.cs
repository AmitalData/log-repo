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
                        ContainersNumbersAndTypesArray = a.ContainersNumbersAndTypesArray,
                        FirstPickupATD = a.FirstPickupATD,
                        FirstPickupATA = a.FirstPickupATA,
                        FinalDeliveryETD = a.FinalDeliveryETD,
                        FinalDeliveryETA = a.FinalDeliveryETA,
                        FinalDeliveryATD = a.FinalDeliveryATD,
                        FinalDeliveryATA = a.FinalDeliveryATA,
                        OperationallyClosedByUserId = a.OperationallyClosedByUserId,
                        NumberOfDeliveries = a.NumberOfDeliveries,
                        LastPickupETA = a.LastPickupETA,
                        LastPickupETD = a.LastPickupETD,
                        LastPickupATA = a.LastPickupATA,
                        LastPickupATD = a.LastPickupATD,
                        DeliveryToPortId = a.DeliveryToPortId,
                        DeliveryFrom = a.DeliveryFrom,
                        DeliveryTo = a.DeliveryTo,
                        PickupFrom = a.PickupFrom,
                        PickupTo = a.PickupTo,
                        CreatedFromDigital = a.CreatedFromDigital,
                        DeliveryTruckerId = a.DeliveryTruckerId,
                        DeliveryTruckerNumber = a.DeliveryTruckerNumber,
                        DeliveryDriver = a.DeliveryDriver,
                        DeliveryTrailerNumber = a.DeliveryTrailerNumber,
                        DeliveryNotes = a.DeliveryNotes,
                        PickupTruckerId = a.PickupTruckerId,
                        PickupTruckerNumber = a.PickupTruckerNumber,
                        PickupDriver = a.PickupDriver,
                        PickupTrailerNumber = a.PickupTrailerNumber,
                        PickupNotes = a.PickupNotes,
                        DeliveryDate = a.DeliveryDate,
                        OnHandDate = a.OnHandDate,
                        PODDate = a.PODDate,
                        BookingConfirmationSent = a.BookingConfirmationSent,
                        PreAlertSent = a.PreAlertSent,
                        ArrivalNoticeSent = a.ArrivalNoticeSent,
                        DeliveryNoticeSent = a.DeliveryNoticeSent,
                        ExpectedArrivalNoticeSent = a.ExpectedArrivalNoticeSent,
                        T1Received = a.T1Received,
                        AccountingClosedByUserId = a.AccountingClosedByUserId,
                        PackagesQuantityAndType = a.PackagesQuantityAndType,
                        IsDocumentsNeedApprove = a.IsDocumentsNeedApprove,
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
                                                                 ContainersNumbersAndTypesArray = a.ContainersNumbersAndTypesArray,
                                                                 FirstPickupATD = a.FirstPickupATD,
                                                                 FirstPickupATA = a.FirstPickupATA,
                                                                 FinalDeliveryETD = a.FinalDeliveryETD,
                                                                 FinalDeliveryETA = a.FinalDeliveryETA,
                                                                 FinalDeliveryATD = a.FinalDeliveryATD,
                                                                 FinalDeliveryATA = a.FinalDeliveryATA,
                                                                 OperationallyClosedByUserId = a.OperationallyClosedByUserId,
                                                                 NumberOfDeliveries = a.NumberOfDeliveries,
                                                                 LastPickupETA = a.LastPickupETA,
                                                                 LastPickupETD = a.LastPickupETD,
                                                                 LastPickupATA = a.LastPickupATA,
                                                                 LastPickupATD = a.LastPickupATD,
                                                                 DeliveryToPortId = a.DeliveryToPortId,
                                                                 DeliveryFrom = a.DeliveryFrom,
                                                                 DeliveryTo = a.DeliveryTo,
                                                                 PickupFrom = a.PickupFrom,
                                                                 PickupTo = a.PickupTo,
                                                                 OperationallyClosedByUserName = a.OperationallyClosedByUser.Contact.Name,
                                                                 CreatedFromDigital = a.CreatedFromDigital,
                                                                 DeliveryTruckerId = a.DeliveryTruckerId,
                                                                 DeliveryTruckerNumber = a.DeliveryTruckerNumber,
                                                                 DeliveryDriver = a.DeliveryDriver,
                                                                 DeliveryTrailerNumber = a.DeliveryTrailerNumber,
                                                                 DeliveryNotes = a.DeliveryNotes,
                                                                 PickupTruckerId = a.PickupTruckerId,
                                                                 PickupTruckerNumber = a.PickupTruckerNumber,
                                                                 PickupDriver = a.PickupDriver,
                                                                 PickupTrailerNumber = a.PickupTrailerNumber,
                                                                 PickupNotes = a.PickupNotes,
                                                                 DeliveryDate = a.DeliveryDate,
                                                                 OnHandDate = a.OnHandDate,
                                                                 PODDate = a.PODDate,
                                                                 BookingConfirmationSent = a.BookingConfirmationSent,
                                                                 PreAlertSent = a.PreAlertSent,
                                                                 ArrivalNoticeSent = a.ArrivalNoticeSent,
                                                                 DeliveryNoticeSent = a.DeliveryNoticeSent,
                                                                 ExpectedArrivalNoticeSent = a.ExpectedArrivalNoticeSent,
                                                                 T1Received = a.T1Received,
                                                                 AccountingClosedByUserId = a.AccountingClosedByUserId,
                                                                 MainCarriageATA = a.MainCarriageATA,
                                                                 MainCarriageETD = a.MainCarriageETD,
                                                                 MainCarriageETA = a.MainCarriageETA,
                                                                 MainCarriageATD = a.MainCarriageATD,
                                                                 PackagesQuantityAndType = a.PackagesQuantityAndType,
                                                                 IsDocumentsNeedApprove = a.IsDocumentsNeedApprove,
                                                             });
            return result;
        }
    }
}