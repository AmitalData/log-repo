using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.QuoteModel.EntityLists;
using Simplog.Data.QuoteModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.QuoteModel.EntityPOCOs;


namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class FollowUpQuery
    {
        FollowUpRepository repository;
        public FollowUpQuery()
        {
            repository = new FollowUpRepository(); 
        }

        public FollowUpQuery(int tenant)
        {
            repository = new FollowUpRepository(tenant);
        }

        public FollowUpQuery(FollowUpRepository followUpRepository)
        {
            repository = followUpRepository;
        }

        public IQueryable<FollowUpPM> GetFollowUpPMs(int tenant)
        {
            IQueryable<FollowUpPM> followups = (from follow in repository.context.FollowUps.Include("OwnerUser.Contact")
                                                where follow.Tenant == tenant
                                                select new FollowUpPM()
                                                {
                                                    Tenant = follow.Tenant,
                                                    Date = follow.Date,
                                                    Id = follow.Id,
                                                    OwnerUserName = follow.OwnerUser.Contact.EnglishName,
                                                    AutomationId = follow.AutomationId,
                                                    DocumentTypeId = follow.DocumentTypeId,
                                                    Area = follow.Area,
                                                });
            return followups;
        }

        public FollowUpPM GetSinglePM(string id, int tenant)
        {
            FollowUpPM followups = (from follow in repository.context.FollowUps.Include("OwnerUser.Contact").Include("EventType")
                                    where follow.Id == id
                                    && follow.Tenant == tenant
                                    select new FollowUpPM()
                                    {
                                        Tenant = follow.Tenant,
                                        Date = follow.Date,
                                        Done = follow.Done,
                                        DoneDateTime = follow.DoneDateTime,
                                        DoneNote = follow.DoneNote,
                                        //EntityTypeId = follow.EntityTypeId,
                                        ExternalDocumentId = follow.DocumentsFilingId,
                                        // FollowUpTypeId = follow.FollowUpTypeId,
                                        Id = follow.Id,
                                        InternalDocumentId = follow.InternalDocumentId,
                                        IsNew = follow.IsNew,
                                        JobId = follow.JobId,
                                        LegType = follow.LegType,
                                        Notes = follow.Notes,
                                        ShipmentId = follow.ShipmentId,
                                        // FollowUpTypeName = follow.FollowUpType.Name,
                                        //EntityDateId = follow.FollowUpType.EntityDateId
                                        EventTypeId = follow.EventTypeId,
                                        EventTypeFollowUpName = follow.EventType.FollowUpEnglishName,
                                        ManualActivatedFollowUp = follow.EventType.ManualActivatedFollowUp,
                                        OwnerUserId = follow.OwnerUserId,
                                        OwnerUserName = follow.OwnerUser.Contact.EnglishName,
                                        AutomationId = follow.AutomationId,
                                        DocumentTypeId = follow.DocumentTypeId,
                                        Area = follow.Area,

                                    }).FirstOrDefault();
            return followups;
        }

        public List<ShipmentList> GetShipmentFollowUpsByShipmentId(string shipmentId, int tenant)
        {
            IShipmentFollowUpDataViewContext dataViewContext = ShipmentFollowUpDataViewContext.GetContext(tenant);
            IQueryable<ShipmentList> shipmentLists = null;
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);

            shipmentLists = from a in dataViewContext.ShipmentFollowUpDataViews
                            where a.Tenant == tenant && a.Id == shipmentId
                            select new ShipmentList()
                            {
                                AWBPrint = a.AWBPrint,
                                ShipmentViewId = a.Id + a.Id,
                                Id = a.Id,
                                DirectionName = a.DirectionName,
                                TransportModeName = a.TransportModeName,
                                Shipper = a.ShipperName,
                                Consignee = a.ConsigneeName,
                                DirectionId = a.DirectionId,
                                FollowUpDate = a.FollowUpDate,
                                FollowUpType = a.FollowUpType,
                                FollowUpId = a.Id,
                                FromPort = a.MainCarriageFromPortCode,
                                House = a.House,
                                CreateDateTime = a.CreateDateTime,
                                ShipmentNumber = a.ShipmentNumber,
                                ShipmentType = a.ShipmentTypeName,
                                ToPort = a.MainCarriageToPortCode,
                                TransportModeId = a.TransportModeId,
                                Field3 = a.Field3,
                                Field4 = a.Field4,
                                Field5 = a.Field5,
                                Field6 = a.Field6,
                                Field7 = a.Field7,
                                Field9 = a.Field9,
                                Field8 = a.Field8,
                                Field10 = a.Field10,
                                ChargeableWeightInKG = a.ChargeableWeightInKG,
                                ShipperReference1 = a.ShipperReference1,
                                FollowUpNotes = a.FollowUpNotes,
                                FromPortName = a.MainCarriageFromPortName,
                                FromPortCountry = a.MainCarriageFromPortCountryName,
                                ToPortCountry = a.MainCarriageToPortCountryName,
                                ToPortName = a.MainCarriageToPortName,
                                ShipmentPayableStatusCode = a.ShipmentPayableStatusCode,
                                ShipmentReceivableStatusCode = a.ShipmentReceivableStatusCode,
                                FollowUpOwner = a.FollowUpOwner,
                                FollowUpOwnerId = a.FollowUpOwnerId,
                                BranchId = a.BranchId,
                                DepartmentId = a.DepartmentId,
                                MainCarriageETA = a.MainCarriageETA,
                                MasterShipmentDataId = a.MasterShipmentDataId,
                                BranchName = a.BranchName,
                                CustomerName = a.CustomerName,
                                GrossWeightInKG = a.GrossWeightInKG,
                                ShipmentLevelCode = a.ShipmentLevelCode,
                                VolumetricWeight = a.VolumetricWeight,
                                MasterShipmentNumber = a.MasterShipmentNumber,
                                FromPortId = a.MainCarriageFromPortId,
                                ToPortId = a.MainCarriageToPortId,
                                ARInvoiceIssued = a.ARInvoiceIssued,
                                CreditNoteIssued = a.CreditNoteIssued,
                                PackagesQuantity = a.PackagesQuantity,
                                IsAccountingClosed = a.IsAccountingClosed,
                                IsOperationalClosed = a.IsOperationalClosed,
                                FHLStatusCode = a.FHLStatusCode,
                                FHLStatusName = a.FHLStatusName,
                                FHLStatusDate = a.FHLStatusDate,
                                FWBStatusCode = a.FWBStatusCode,
                                FWBStatusName = a.FWBStatusName,
                                FWBStatusDate = a.FWBStatusDate,
                                CargonautFHLStatusCode = a.CargonautFHLStatusCode,
                                CargonautFHLStatusName = a.CargonautFHLStatusName,
                                CargonautFHLStatusDate = a.CargonautFHLStatusDate,
                                CargonautFWBStatusCode = a.CargonautFWBStatusCode,
                                CargonautFWBStatusName = a.CargonautFWBStatusName,
                                CargonautFWBStatusDate = a.CargonautFWBStatusDate,
                                NumberOfInsidePackages = a.NumberOfInsidePackages,
                                NumberOfInsidePackagesDetails = a.NumberOfInsidePackagesDetails,
                                ConsolidatorId = a.ConsolidatorId,
                                ConsolidatorName = a.ConsolidatorName,
                                ConsolidatorNote = a.ConsolidatorNote,
                                ConsolidatorAddressId = a.ConsolidatorAddressId,
                                ConsolidatorContactId = a.ConsolidatorContactId,
                                ConsolidatorReference = a.ConsolidatorReference,
                                ManifestReason = a.ManifestReason,
                                ManifestStatusCode = a.ManifestStatusCode,
                                AirlinePrefix = a.AirlinePrefix,
                                FromPortCountryCode = a.FromPortCountryCode,
                                ToPortCountryCode = a.ToPortCountryCode,
                                StatusId = a.MasterShipmentDataId != null ? (a.ShipmentMasterDataStatusWeight > a.ShipmentStatusWeight ? a.ShipmentMasterDataStatusId : a.ShipmentStatusId) : (a.ShipmentStatusId),
                                StatusDate = a.MasterShipmentDataId != null ? (a.ShipmentMasterDataStatusWeight > a.ShipmentStatusWeight ? a.ShipmentMasterDataStatusDate : a.ShipmentStatusDate) : (a.ShipmentStatusDate),
                                StatusName = a.MasterShipmentDataId != null ? (a.ShipmentMasterDataStatusWeight > a.ShipmentStatusWeight ? a.ShipmentMasterDataStatusName : a.ShipmentStatusName) : (a.ShipmentStatusName),
                                StatusLocation = a.MasterShipmentDataId != null ? (a.ShipmentMasterDataStatusWeight > a.ShipmentStatusWeight ? a.ShipmentMasterDataStatusLocation : a.ShipmentStatusLocation) : (a.ShipmentStatusLocation),
                                CustomsDeclarationNumber = a.CustomsDeclarationNumber,
                                OperationalDate = a.OperationalDate,
                                CutoffDate = a.CutoffDate,
                                NumberOfHouses = a.NumberOfHouses,
                            };

            return shipmentLists.ToList();
        }

        public IQueryable<ShipmentList> GetFollowUpsByTenantForMasterFilter(int tenant, string cardId)
        {
            IQueryable<ShipmentList> shipmentLists = null;
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            IShipmentFollowUpDataViewContext dataViewContext = ShipmentFollowUpDataViewContext.GetContext(tenant);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);

            if (!string.IsNullOrEmpty(cardId))
            {

                //shipmentLists = from a in shipmentsContext.FollowUps

                //            where a.Tenant == tenant && (a.Shipment.AgentId == cardId || a.Shipment.ClientId == cardId)
                //            select a;
            }

            else
            {
                shipmentLists = from a in dataViewContext.ShipmentFollowUpDataViews
                                where a.Tenant == tenant && (a.ShipmentLevelCode == "D" || a.ShipmentLevelCode == "C")
                                select new ShipmentList()
                                {
                                    AWBPrint = a.AWBPrint,
                                    ShipmentViewId = a.Id + a.Id,
                                    Id = a.Id,
                                    DirectionName = a.DirectionName,
                                    TransportModeName = a.TransportModeName,
                                    Shipper = a.ShipperName,
                                    Consignee = a.ConsigneeName,
                                    DirectionId = a.DirectionId,
                                    FollowUpDate = a.FollowUpDate,
                                    FollowUpType = a.FollowUpType,
                                    FollowUpId = a.Id,
                                    FromPort = a.MainCarriageFromPortCode,
                                    House = a.House,
                                    CreateDateTime = a.CreateDateTime,
                                    ShipmentNumber = a.ShipmentNumber,
                                    ShipmentType = a.ShipmentTypeName,
                                    ToPort = a.MainCarriageToPortCode,
                                    TransportModeId = a.TransportModeId,
                                    Field3 = a.Field3,
                                    Field4 = a.Field4,
                                    Field5 = a.Field5,
                                    Field6 = a.Field6,
                                    Field7 = a.Field7,
                                    Field9 = a.Field9,
                                    Field8 = a.Field8,
                                    Field10 = a.Field10,
                                    ChargeableWeightInKG = a.ChargeableWeightInKG,
                                    ShipperReference1 = a.ShipperReference1,
                                    FollowUpNotes = a.FollowUpNotes,
                                    FromPortName = a.MainCarriageFromPortName,
                                    FromPortCountry = a.MainCarriageFromPortCountryName,
                                    ToPortCountry = a.MainCarriageToPortCountryName,
                                    ToPortName = a.MainCarriageToPortName,
                                    ShipmentPayableStatusCode = a.ShipmentPayableStatusCode,
                                    ShipmentReceivableStatusCode = a.ShipmentReceivableStatusCode,
                                    FollowUpOwner = a.FollowUpOwner,
                                    FollowUpOwnerId = a.FollowUpOwnerId,
                                    BranchId = a.BranchId,
                                    DepartmentId = a.DepartmentId,
                                    MainCarriageETA = a.MainCarriageETA,
                                    MasterShipmentDataId = a.MasterShipmentDataId,
                                    BranchName = a.BranchName,
                                    CustomerName = a.CustomerName,
                                    GrossWeightInKG = a.GrossWeightInKG,
                                    ShipmentLevelCode = a.ShipmentLevelCode,
                                    VolumetricWeight = a.VolumetricWeight,
                                    MasterShipmentNumber = a.MasterShipmentNumber,
                                    FromPortId = a.MainCarriageFromPortId,
                                    ToPortId = a.MainCarriageToPortId,
                                    ARInvoiceIssued = a.ARInvoiceIssued,
                                    CreditNoteIssued = a.CreditNoteIssued,
                                    IsAccountingClosed = a.IsAccountingClosed,
                                    IsOperationalClosed = a.IsOperationalClosed,
                                    PackagesQuantity = a.PackagesQuantity,
                                    FHLStatusCode = a.FHLStatusCode,
                                    FHLStatusName = a.FHLStatusName,
                                    FHLStatusDate = a.FHLStatusDate,
                                    FWBStatusCode = a.FWBStatusCode,
                                    FWBStatusName = a.FWBStatusName,
                                    FWBStatusDate = a.FWBStatusDate,
                                    CargonautFHLStatusCode = a.CargonautFHLStatusCode,
                                    CargonautFHLStatusName = a.CargonautFHLStatusName,
                                    CargonautFHLStatusDate = a.CargonautFHLStatusDate,
                                    CargonautFWBStatusCode = a.CargonautFWBStatusCode,
                                    CargonautFWBStatusName = a.CargonautFWBStatusName,
                                    CargonautFWBStatusDate = a.CargonautFWBStatusDate,
                                    NumberOfInsidePackages = a.NumberOfInsidePackages,
                                    NumberOfInsidePackagesDetails = a.NumberOfInsidePackagesDetails,
                                    ConsolidatorId = a.ConsolidatorId,
                                    ConsolidatorName = a.ConsolidatorName,
                                    ConsolidatorNote = a.ConsolidatorNote,
                                    ConsolidatorAddressId = a.ConsolidatorAddressId,
                                    ConsolidatorContactId = a.ConsolidatorContactId,
                                    ConsolidatorReference = a.ConsolidatorReference,
                                    ManifestReason = a.ManifestReason,
                                    ManifestStatusCode = a.ManifestStatusCode,
                                    AirlinePrefix = a.AirlinePrefix,
                                    FromPortCountryCode = a.FromPortCountryCode,
                                    ToPortCountryCode = a.ToPortCountryCode,
                                    StatusId = a.MasterShipmentDataId != null ? (a.ShipmentMasterDataStatusWeight > a.ShipmentStatusWeight ? a.ShipmentMasterDataStatusId : a.ShipmentStatusId) : (a.ShipmentStatusId),
                                    StatusDate = a.MasterShipmentDataId != null ? (a.ShipmentMasterDataStatusWeight > a.ShipmentStatusWeight ? a.ShipmentMasterDataStatusDate : a.ShipmentStatusDate) : (a.ShipmentStatusDate),
                                    StatusName = a.MasterShipmentDataId != null ? (a.ShipmentMasterDataStatusWeight > a.ShipmentStatusWeight ? a.ShipmentMasterDataStatusName : a.ShipmentStatusName) : (a.ShipmentStatusName),
                                    StatusLocation = a.MasterShipmentDataId != null ? (a.ShipmentMasterDataStatusWeight > a.ShipmentStatusWeight ? a.ShipmentMasterDataStatusLocation : a.ShipmentStatusLocation) : (a.ShipmentStatusLocation),
                                    CustomsDeclarationNumber = a.CustomsDeclarationNumber,
                                    OperationalDate = a.OperationalDate,
                                    CutoffDate = a.CutoffDate,
                                    NumberOfHouses = a.NumberOfHouses,
                                };

            }

            if (shipmentLists == null)
            {
                return null;
            }

            return shipmentLists.OrderBy(d => !d.FollowUpDate.HasValue).ThenBy(d => d.FollowUpDate);
        }
        
        public QuoteList GetQuoteFollowUpsByQuoteId(string quoteId, int tenant)
        {
            IQuoteFollowUpDataViewContext quoteFollowUpDataViewContext = QuoteFollowUpDataViewContext.GetContext(tenant);

            QuoteList quoteLists = null;

            QuoteFollowUpDataView q = quoteFollowUpDataViewContext.QuoteFollowUpDataViews.Where(d => d.Id == quoteId && d.Tenant == tenant).FirstOrDefault();

            quoteLists = new QuoteList()
                         {
                             IsClosed = q.IsClosed,
                             QuoteViewId = q.Id + q.FollowUpId,
                             Id = q.Id,
                             Shipper = q.Shipper,
                             Consignee = q.Consignee,
                             DirectionId = q.DirectionId,
                             FollowUpDate = q.FollowUpDate,
                             FollowUpType = q.FollowUpType,
                             FollowUpId = q.FollowUpId,
                             FromPort = q.FromPortCode,
                             OpenDate = q.OpenDate,
                             QuoteNumber = q.QuoteNumber,
                             ShipmentType = q.ShipmentTypeName,
                             ToPort = q.ToPortCode,
                             TransportModeId = q.TransportModeId,
                             Field1 = q.Field1,
                             Field2 = q.Field2,
                             Field3 = q.Field3,
                             Field4 = q.Field4,
                             Field5 = q.Field5,
                             Field6 = q.Field6,
                             Field7 = q.Field7,
                             Field9 = q.Field9,
                             Field8 = q.Field8,
                             Field10 = q.Field10,
                             ChargeableWeight = q.ChargeableWeight,
                             ShipperReference1 = q.ShipperReference1,
                             BranchId = q.BranchId,
                             DepartmentId = q.DepartmentId,
                             ExpirationDate = q.ExpirationDate,
                             FollowUpNotes = q.FollowUpNotes,
                             FromPortName = q.FromPortName,
                             FromPortCountry = q.FromPortName,
                             ToPortCountry = q.ToPortCountry,
                             ToPortName = q.ToPortName,
                             LastModified = q.LastModified,
                             CreatedByUser = q.CreatedByUser,
                             UpdatedByUser = q.UpdatedByUser,
                             FollowUpOwner = q.FollowUpOwner,
                             FollowUpOwnerId = q.FollowUpOwnerUserId,
                             QuoteTypeName = q.QuoteTypeName,
                             QuoteTypeCode = q.QuoteTypeCode,
                             FollowUpTypeId = q.FollowUpTypeId,
                             IsCancelled = q.IsCancelled,
                             MainCarriageCarrierId = q.MainCarriageCarrierId,
                             MainCarriageCarrierName = q.MainCarriageCarrierName,
                             StageId = q.StageId,
                             StageName = q.StageName,
                             RatingCode = q.RatingCode,
                             LastActivityDate = q.LastActivityDate,
                             LastActivitySubject = q.LastActivitySubject,
                             LastActivityTypeCode = q.LastActivityTypeCode,
                             NextActivityDate = q.NextActivityDate,
                             NextActivitySubject = q.NextActivitySubject,
                             NextActivityTypeCode = q.NextActivityTypeCode,
                             RatingName = q.RatingCode == "C" ? "Cold" : (q.RatingCode == "H" ? "Hot" : q.RatingCode == "N" ? "Neutral" : "Warm"),
                             LastActivityTypeName = q.LastActivityTypeCode == "CL" ? "Call" : (q.LastActivityTypeCode == "TS" ? "Task" : (q.LastActivityTypeCode == "AP" ? "Appointment" : (q.LastActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                             NextActivityTypeName = q.NextActivityTypeCode == "CL" ? "Call" : (q.NextActivityTypeCode == "TS" ? "Task" : (q.NextActivityTypeCode == "AP" ? "Appointment" : (q.NextActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                             OpportunityId = q.OpportunityId,
                             IsAutomaticallyClosed = q.IsAutomaticallyClosed,
                             AutomaticallyCloseDays = q.AutomaticallyCloseDays,
                             AutomaticallyCloseDate = q.AutomaticallyCloseDate,
                             UpdateDate = q.UpdateDate,
                             SalesmanUserId = q.SalesmanUserId,
                             BusinessUnitId = q.BusinessUnitId,
                             BusinessUnitName = q.BusinessUnitName,
                             QuoteClosingReasonCode = q.QuoteClosingReasonCode,
                             QuoteClosingReasonName = q.QuoteClosingReasonName,
                             ProductCode = q.ProductCode,
                             IncotermCode = q.IncotermCode,
                         };
           
            return quoteLists;
        }

        public IQueryable<QuoteList> GetFollowUpsForQuotes(int tenant)
        {
            IQuoteFollowUpDataViewContext quoteFollowUpDataViewContext = QuoteFollowUpDataViewContext.GetContext(tenant);
            IQuotesContext quotesContext = QuotesContext.GetContext(tenant);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
            IQueryable<QuoteList> quoteLists = null;

            quoteLists = from q in quoteFollowUpDataViewContext.QuoteFollowUpDataViews
                         where q.Tenant == tenant
                         select new QuoteList()
                         {
                             IsClosed = q.IsClosed,
                             QuoteViewId = q.Id + q.FollowUpId,
                             Id = q.Id,
                             Shipper = q.Shipper,
                             Consignee = q.Consignee,
                             DirectionId = q.DirectionId,
                             FollowUpDate = q.FollowUpDate,
                             FollowUpType = q.FollowUpType,
                             FollowUpId = q.FollowUpId,
                             FromPort = q.FromPortCode,
                             OpenDate = q.OpenDate,
                             QuoteNumber = q.QuoteNumber,
                             ShipmentType = q.ShipmentTypeName,
                             ToPort = q.ToPortCode,
                             TransportModeId = q.TransportModeId,
                             Field1 = q.Field1,
                             Field2 = q.Field2,
                             Field3 = q.Field3,
                             Field4 = q.Field4,
                             Field5 = q.Field5,
                             Field6 = q.Field6,
                             Field7 = q.Field7,
                             Field9 = q.Field9,
                             Field8 = q.Field8,
                             Field10 = q.Field10,
                             ChargeableWeight = q.ChargeableWeight,
                             ShipperReference1 = q.ShipperReference1,
                             BranchId = q.BranchId,
                             DepartmentId = q.DepartmentId,
                             ExpirationDate = q.ExpirationDate,
                             FollowUpNotes = q.FollowUpNotes,
                             FromPortName = q.FromPortName,
                             FromPortCountry = q.FromPortName,
                             ToPortCountry = q.ToPortCountry,
                             ToPortName = q.ToPortName,
                             LastModified = q.LastModified,
                             CreatedByUser = q.CreatedByUser,
                             UpdatedByUser = q.UpdatedByUser,
                             FollowUpOwner = q.FollowUpOwner,
                             FollowUpOwnerId = q.FollowUpOwnerUserId,
                             QuoteTypeName = q.QuoteTypeName,
                             QuoteTypeCode = q.QuoteTypeCode,
                             FollowUpTypeId = q.FollowUpTypeId,
                             IsCancelled = q.IsCancelled,
                             MainCarriageCarrierId = q.MainCarriageCarrierId,
                             MainCarriageCarrierName = q.MainCarriageCarrierName,
                             StageId = q.StageId,
                             StageName = q.StageName,
                             RatingCode = q.RatingCode,
                             LastActivityDate = q.LastActivityDate,
                             LastActivitySubject = q.LastActivitySubject,
                             LastActivityTypeCode = q.LastActivityTypeCode,
                             NextActivityDate = q.NextActivityDate,
                             NextActivitySubject = q.NextActivitySubject,
                             NextActivityTypeCode = q.NextActivityTypeCode,
                             RatingName = q.RatingCode == "C" ? "Cold" : (q.RatingCode == "H" ? "Hot" : q.RatingCode == "N" ? "Neutral" : "Warm"),
                             LastActivityTypeName = q.LastActivityTypeCode == "CL" ? "Call" : (q.LastActivityTypeCode == "TS" ? "Task" : (q.LastActivityTypeCode == "AP" ? "Appointment" : (q.LastActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                             NextActivityTypeName = q.NextActivityTypeCode == "CL" ? "Call" : (q.NextActivityTypeCode == "TS" ? "Task" : (q.NextActivityTypeCode == "AP" ? "Appointment" : (q.NextActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                             OpportunityId = q.OpportunityId,
                             IsAutomaticallyClosed = q.IsAutomaticallyClosed,
                             AutomaticallyCloseDays = q.AutomaticallyCloseDays,
                             AutomaticallyCloseDate = q.AutomaticallyCloseDate,
                             UpdateDate = q.UpdateDate,
                             SalesmanUserId = q.SalesmanUserId,
                             BusinessUnitId = q.BusinessUnitId,
                             BusinessUnitName = q.BusinessUnitName,
                             QuoteClosingReasonCode = q.QuoteClosingReasonCode,
                             QuoteClosingReasonName = q.QuoteClosingReasonName,
                             ProductCode = q.ProductCode,
                             IncotermCode = q.IncotermCode,
                         };

            return quoteLists.OrderBy(d => !d.FollowUpDate.HasValue).ThenBy(d => d.FollowUpDate);
        }

        public IQueryable<QuoteFollowUpDataView> GetDataViewFollowUpsForQuotes(int tenant)
        {
            IQuoteFollowUpDataViewContext dataViewEntities = QuoteFollowUpDataViewContext.GetContext(tenant);
            IQueryable<QuoteFollowUpDataView> result = (from a in dataViewEntities.QuoteFollowUpDataViews where a.Tenant == tenant && a.IsCancelled == false select a);
            return result;
        }

    }
}