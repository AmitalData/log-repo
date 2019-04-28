using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.QuoteModel;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteQuery
    {
        QuoteRepository repository;

        public QuoteQuery()
        {
            repository = new QuoteRepository();
        }
        public QuoteQuery(int tenant)
        {
            repository = new QuoteRepository(tenant);
        }
        public QuoteQuery(QuoteRepository quoteQuery)
        {
            repository = quoteQuery;
        }

        public QuotePM GetSinglePM(string id, int tenant)
        {
            Quote entityPOCO = (from a in repository.context.Quotes.Include("Incoterm").Include("Stage").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("FromPartnerCard").Include("ToPartnerCard").Include("ToPort.Country").Include("FromPort.Country").Include("FromPort").Include("Direction").Include("TransportMode").Include("QuoteType").Include("AgentCard").Include("SaleCurrency")
                                where a.Id == id && a.Tenant == tenant
                                select a).FirstOrDefault();

            QuotePM entityPM = this.MapPOCOToPM(entityPOCO);


            QuotePM securedPM = new QuotePM();
            SecuredMapping.GetMappedPM(entityPM, securedPM, "Quote", tenant);

            QuotePM myResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);
            myResult = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), myResult, tenant);

            return myResult;
        }

        public QuotePM GetSinglePMByQuoteNumber(string quoteNumber, int tenant)
        {
            Quote entityPOCO = (from a in repository.context.Quotes.Include("Incoterm").Include("Stage").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("FromPartnerCard").Include("ToPartnerCard").Include("ToPort.Country").Include("FromPort.Country").Include("FromPort").Include("Direction").Include("TransportMode").Include("QuoteType").Include("SaleCurrency")
                                where a.QuoteNumber == quoteNumber && a.Tenant == tenant
                                select a).FirstOrDefault();

            QuotePM entityPM = this.MapPOCOToPM(entityPOCO);


            QuotePM securedPM = new QuotePM();
            SecuredMapping.GetMappedPM(entityPM, securedPM, "Quote", tenant);

            QuotePM myResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);
            myResult = ProductPermitionsFilter.AddUserProductRestrictionFilters(new QueryOperations(), myResult, tenant);

            return myResult;
        }

        public IQueryable<QuoteList> GetIQueryableEntityList(IQueryable<Quote> iQueryable)
        {
            if (iQueryable.Count() > 0)
            {
                int tenant = iQueryable.First().Tenant;

                QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
                iQueryable = businessUnitFilter.RunFilter(iQueryable);

                iQueryable = BranchPermitionsFilter.AddUserBranchRestrictionFilters<Quote>(new QueryOperations(), iQueryable, tenant);
                iQueryable = ProductPermitionsFilter.AddUserProductRestrictionFilters<Quote>(new QueryOperations(), iQueryable, tenant);
            }

            IQueryable<QuoteList> result = from f in iQueryable.Include("Incoterm").Include("FromPort").Include("Stage").Include("QuoteType").Include("TransportMode").Include("Direction").Include("ToPort").Include("ShipmentType").Include("ToPort.Country").Include("FromPort.Country").Include("CreatedByUser.Contact").Include("MainCarriageCarrierCard").Include("Department").Include("Branch").Include("FromPartnerAddress").Include("ToPartnerAddress").Include("FromPartnerAddress.Country").Include("ToPartnerAddress.Country").Include("FreelancerCard").Include("BusinessUnit").Include("QuoteClosingReason").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AgentCard").Include("NotifyCard").Include("MoveType")
                                           select new QuoteList()
                                           {
                                               IsClosed = f.IsClosed,
                                               EstimateProfit = f.EstimateProfit,
                                               EstimateProfitEdited = f.EstimateProfitEdited,
                                               DirectionName = f.Direction == null ? "" : f.Direction.Name,
                                               Id = f.Id,
                                               Shipper = f.ShipperName,
                                               Consignee = f.ConsigneeName,
                                               TransportModeName = f.TransportMode == null ? "" : f.TransportMode.Name,
                                               QuoteViewId = f.Id,
                                               CustomerName = f.CustomerName,
                                               ShipmentType = f.ShipmentType == null ? "" : f.ShipmentType.Name,
                                               Field1 = f.Field1,
                                               Field2 = f.Field2,
                                               Field3 = f.Field3,
                                               Field4 = f.Field4,
                                               Field5 = f.Field5,
                                               Field6 = f.Field6,
                                               Field7 = f.Field7,
                                               Field9 = f.Field9,
                                               Field8 = f.Field8,
                                               Field10 = f.Field10,
                                               LastVersionNumber = f.LastVersionNumber,
                                               ShipperReference1 = f.ShipperReference1,
                                               LastModified = f.LastModified,
                                               CreatedByUser = f.CreatedByUser != null && f.CreatedByUser.Contact != null ? f.CreatedByUser.Contact.EnglishName : "",
                                               QuoteTypeCode = f.QuoteTypeCode,
                                               DirectionId = f.DirectionId,
                                               TransportModeId = f.TransportModeId,
                                               ShipmentTypeId = f.ShipmentTypeId,
                                               ShipperId = f.ShipperId,
                                               FromPortId = f.FromPortId,
                                               ToPortId = f.ToPortId,
                                               QuoteNumber = f.QuoteNumber,
                                               OpenDate = f.OpenDate,
                                               ExpirationDate = f.ExpirationDate,
                                               MainCarriageCarrierId = f.MainCarriageCarrierId,
                                               MainCarriageCarrierName = f.MainCarriageCarrierCard == null ? "" : f.MainCarriageCarrierCard.EnglishName,
                                               QuoteTypeName = f.QuoteType == null ? "" : f.QuoteType.Name,
                                               CarrierName = f.MainCarriageCarrierCard == null ? "" : f.MainCarriageCarrierCard.EnglishName,
                                               ChargeableWeight = f.ChargeableWeight,
                                               GrossWeight = f.GrossWeight,
                                               GrossWeightInKG = f.GrossWeightInKG,
                                               GrossWeightPerTon = f.GrossWeightPerTon,
                                               IsCancelled = f.IsCancelled,
                                               SearchFields = f.SearchFields,
                                               Notes = f.Notes,
                                               BranchId = f.BranchId,
                                               DepartmentId = f.DepartmentId,
                                               BranchName = f.Branch == null ? null : f.Branch.EnglishName,
                                               MoveTypeName = f.MoveType == null ? null : f.MoveType.MoveTypeEnglishName,
                                               DepartmentName = f.Department == null ? null : f.Department.EnglishName,
                                               NumberOfContainers = f.NumberOfContainers,
                                               FromPartnerId = f.FromPartnerId,
                                               ToPartnerId = f.ToPartnerId,
                                               FromPartnerAddressId = f.FromPartnerAddressId,
                                               ToPartnerAddressId = f.ToPartnerAddressId,
                                               NumberOfPackages = f.NumberOfPackages,
                                               QuoteClosingReasonCode = f.QuoteClosingReasonCode,
                                               QuoteClosingReasonName = f.QuoteClosingReason == null ? null : f.QuoteClosingReason.Name,
                                               SentDate = f.SentDate,
                                               AcceptedDate = f.AcceptedDate,
                                               DeclinedDate = f.DeclinedDate,
                                               UsageCount = f.UsageCount,
                                               LastUsageDate = f.LastUsageDate,
                                               FreelancerId = f.FreelancerId,
                                               FreelancerAddressId = f.FreelancerAddressId,
                                               FreelancerContactId = f.FreelancerContactId,
                                               FreelancerName = f.FreelancerCard != null ? f.FreelancerCard.EnglishName : null,
                                               BusinessUnitId = f.BusinessUnitId,
                                               BusinessUnitName = f.BusinessUnit == null ? "" : f.BusinessUnit.Name,
                                               SalesmanUserId = f.SalesmanUserId,
                                               Salesman = f.SalesmanUser == null ? "" : (f.SalesmanUser.Contact == null ? "" : f.SalesmanUser.Contact.EnglishName),
                                               SalesmanName = f.SalesmanUser == null ? "" : (f.SalesmanUser.Contact == null ? "" : f.SalesmanUser.Contact.EnglishName),
                                               StageId = f.StageId,
                                               StageName = f.Stage == null ? "" : f.Stage.Name,
                                               StageMaxDays = f.Stage == null ? 0 : f.Stage.MaxDays,
                                               StageDueDate = f.StageDueDate,
                                               RatingCode = f.RatingCode,
                                               RatingName = f.Rating == null ? "" : f.Rating.Name,
                                               RatingIndexOrder = f.Rating == null ? 0 : f.Rating.IndexOrder,
                                               LastActivityDate = f.LastActivityDate,
                                               LastActivitySubject = f.LastActivitySubject,
                                               LastActivityTypeCode = f.LastActivityTypeCode,
                                               NextActivityDate = f.NextActivityDate,
                                               NextActivitySubject = f.NextActivitySubject,
                                               NextActivityTypeCode = f.NextActivityTypeCode,
                                               LastActivityTypeName = f.LastActivityTypeCode == "CL" ? "Call" : (f.LastActivityTypeCode == "TS" ? "Task" : (f.LastActivityTypeCode == "AP" ? "Appointment" : (f.LastActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                                               NextActivityTypeName = f.NextActivityTypeCode == "CL" ? "Call" : (f.NextActivityTypeCode == "TS" ? "Task" : (f.NextActivityTypeCode == "AP" ? "Appointment" : (f.NextActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                                               OpportunityId = f.OpportunityId,
                                               IsAutomaticallyClosed = f.IsAutomaticallyClosed,
                                               AutomaticallyCloseDate = f.AutomaticallyCloseDate,
                                               AutomaticallyCloseDays = f.AutomaticallyCloseDays,
                                               UpdateDate = f.UpdateDate,
                                               UpdatedByUserId = f.UpdatedByUserId,
                                               IncotermCode = f.Incoterm == null ? "" : f.Incoterm.Code,

                                               FromPortName = f.FromPort == null ? "" : f.FromPort.EnglishName,
                                               ToPortName = f.ToPort == null ? "" : f.ToPort.EnglishName,

                                               FromPort = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                                               (f.FromPartnerAddress != null ? f.FromPartnerAddress.City : "")
                                               :
                                               (f.FromPort != null ? f.FromPort.Code : ""),

                                               FromCountryCode = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                                               ((f.FromPartnerAddress != null && f.FromPartnerAddress.Country != null ? f.FromPartnerAddress.Country.Code : ""))
                                               :
                                               ((f.FromPort != null && f.FromPort.Country != null ? f.FromPort.Country.Code : "")),

                                               FromPortCountry = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                                               ((f.FromPartnerAddress != null && f.FromPartnerAddress.Country != null ? f.FromPartnerAddress.Country.EnglishName : ""))
                                               :
                                               ((f.FromPort != null && f.FromPort.Country != null ? f.FromPort.Country.EnglishName : "")),

                                               ToPort = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                                               (f.ToPartnerAddress != null ? f.ToPartnerAddress.City : "")
                                               :
                                               (f.ToPort != null ? f.ToPort.Code : ""),

                                               ToCountryCode = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                                               ((f.ToPartnerAddress != null && f.ToPartnerAddress.Country != null ? f.ToPartnerAddress.Country.Code : ""))
                                               :
                                               ((f.ToPort != null && f.ToPort.Country != null ? f.ToPort.Country.Code : "")),

                                               ToPortCountry = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                                               ((f.ToPartnerAddress != null && f.ToPartnerAddress.Country != null ? f.ToPartnerAddress.Country.EnglishName : ""))
                                               :
                                               ((f.ToPort != null && f.ToPort.Country != null ? f.ToPort.Country.EnglishName : "")),

                                               Routing = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                                               ((f.FromPartnerAddress == null ? "" : f.FromPartnerAddress.City) + " > " + (f.ToPartnerAddress == null ? "" : f.ToPartnerAddress.City))
                                               :
                                               ((f.FromPort == null ? "" : f.FromPort.Code) + " > " + (f.ToPort == null ? "" : f.ToPort.Code)),

                                               Subject = f.Subject,
                                               IsSubjectEdited = f.IsSubjectEdited,
                                               IsFixedPrice = f.IsFixedPrice,
                                               ProductCode = f.ProductCode,
                                               TransitTime = f.TransitTime,
                                               DepartureFrequency = f.DepartureFrequency,
                                               ETD = f.ETD,
                                               ETA = f.ETA,
                                               AgentId = f.AgentId,
                                               AgentAddressId = f.AgentAddressId,
                                               AgentContactId = f.AgentContactId,
                                               AgentReference1 = f.AgentReference1,
                                               AgentReference2 = f.AgentReference2,
                                               AgentName = f.AgentCard == null ? null : f.AgentCard.EnglishName,
                                               LastStageDate = f.LastStageDate,
                                               TEU = f.TEU,
                                               IsSaleCurrencySameAsCost = f.IsSaleCurrencySameAsCost,
                                               IsChargesByVAT = f.IsChargesByVAT,
                                               ValueOfGoods = f.ValueOfGoods,
                                               TotalPerContainer = f.TotalPerContainer,
                                               IsQuoteDataExternal = f.IsQuoteDataExternal,
                                               IsQuoteDocumentExternal = f.IsQuoteDocumentExternal,
                                               QuotationSections = f.QuotationSections,
                                               NotifyId = f.NotifyId,
                                               NotifyAddressId = f.NotifyAddressId,
                                               NotifyContactId = f.NotifyContactId,
                                               NotifyName = f.NotifyCard == null ? null : f.NotifyCard.EnglishName,
                                               NotifyNote = f.NotifyCard == null ? null : f.NotifyCard.Notes,
                                               NumberOfFollowUps = f.NumberOfFollowUps,
                                               CustomerId = f.CustomerId,
                                               IsDangerous = f.IsDangerous,
                                           };
            return result;
        }

        public List<QuoteList> GetRecentEntityLists(string ownerId, string businessUnitId, int tenant, string userId, string objectTableId)
        {
            List<QuoteList> entityList = new List<QuoteList>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            repository = new QuoteRepository(tenant);
            IQueryable<Quote> entities = entities = repository.GetQuotes(tenant);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            entities = businessUnitFilter.RunFilter(entities);

            if (!string.IsNullOrEmpty(ownerId))
            {
                entities = entities.Where(d => d.SalesmanUserId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                entities = entities.Where(d => d.BusinessUnitId == businessUnitId);
            }

            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                Quote f = (from d in entities.Include("Incoterm").Include("FromPort").Include("Stage").Include("Rating").Include("QuoteType").Include("TransportMode").Include("Direction").Include("ToPort").Include("ShipmentType").Include("ToPort.Country").Include("FromPort.Country").Include("CreatedByUser.Contact").Include("MainCarriageCarrierCard").Include("Department").Include("Branch").Include("FromPartnerAddress").Include("ToPartnerAddress").Include("FromPartnerAddress.Country").Include("ToPartnerAddress.Country").Include("FreelancerCard").Include("BusinessUnit").Include("QuoteClosingReason").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AgentCard").Include("MoveType")
                           where d.Id == lastActivity.EntityId
                           select d).FirstOrDefault();

                if (f != null)
                {
                    QuoteList list = new QuoteList()
                    {
                        Id = f.Id,
                        QuoteViewId = f.Id,
                        IsClosed = f.IsClosed,
                        IsCancelled = f.IsCancelled,
                        LastModified = f.LastModified,
                        QuoteNumber = f.QuoteNumber,
                        BranchId = f.BranchId,
                        DepartmentId = f.DepartmentId,
                        BranchName = f.Branch == null ? null : f.Branch.EnglishName,
                        MoveTypeName = f.MoveType == null ? null : f.MoveType.MoveTypeEnglishName,
                        DepartmentName = f.Department == null ? null : f.Department.EnglishName,
                        DirectionId = f.DirectionId,
                        TransportModeId = f.TransportModeId,
                        DirectionName = f.Direction == null ? "" : f.Direction.Name,
                        TransportModeName = f.TransportMode == null ? "" : f.TransportMode.Name,
                        ShipmentTypeId = f.ShipmentTypeId,
                        ShipmentType = f.ShipmentType == null ? "" : f.ShipmentType.Name,
                        QuoteTypeCode = f.QuoteTypeCode,
                        QuoteTypeName = f.QuoteType.Name,
                        EstimateProfit = f.EstimateProfit,
                        EstimateProfitEdited = f.EstimateProfitEdited,
                        GrossWeight = f.GrossWeight,
                        GrossWeightInKG = f.GrossWeightInKG,
                        GrossWeightPerTon = f.GrossWeightPerTon,
                        ChargeableWeight = f.ChargeableWeight,
                        NumberOfContainers = f.NumberOfContainers,
                        NumberOfPackages = f.NumberOfPackages,
                        QuoteClosingReasonCode = f.QuoteClosingReasonCode,
                        QuoteClosingReasonName = f.QuoteClosingReason == null ? null : f.QuoteClosingReason.Name,
                        SentDate = f.SentDate,
                        AcceptedDate = f.AcceptedDate,
                        DeclinedDate = f.DeclinedDate,
                        UsageCount = f.UsageCount,
                        LastUsageDate = f.LastUsageDate,
                        FreelancerId = f.FreelancerId,
                        FreelancerAddressId = f.FreelancerAddressId,
                        FreelancerContactId = f.FreelancerContactId,
                        FreelancerName = f.FreelancerCard != null ? f.FreelancerCard.EnglishName : null,
                        BusinessUnitId = f.BusinessUnitId,
                        BusinessUnitName = f.BusinessUnit == null ? "" : f.BusinessUnit.Name,
                        SalesmanUserId = f.SalesmanUserId,
                        Salesman = f.SalesmanUser == null ? "" : (f.SalesmanUser.Contact == null ? "" : f.SalesmanUser.Contact.EnglishName),
                        SalesmanName = f.SalesmanUser == null ? "" : (f.SalesmanUser.Contact == null ? "" : f.SalesmanUser.Contact.EnglishName),
                        Field1 = f.Field1,
                        Field2 = f.Field2,
                        Field3 = f.Field3,
                        Field4 = f.Field4,
                        Field5 = f.Field5,
                        Field6 = f.Field6,
                        Field7 = f.Field7,
                        Field9 = f.Field9,
                        Field8 = f.Field8,
                        Field10 = f.Field10,
                        OpenDate = f.OpenDate,
                        ExpirationDate = f.ExpirationDate,
                        SearchFields = f.SearchFields,
                        LastVersionNumber = f.LastVersionNumber,
                        MainCarriageCarrierId = f.MainCarriageCarrierId,
                        CarrierName = f.MainCarriageCarrierCard == null ? "" : f.MainCarriageCarrierCard.EnglishName,
                        MainCarriageCarrierName = f.MainCarriageCarrierCard == null ? "" : f.MainCarriageCarrierCard.EnglishName,
                        CreatedByUser = f.CreatedByUser == null ? "" : (f.CreatedByUser.Contact == null ? "" : f.CreatedByUser.Contact.EnglishName),
                        LastQuoteActivityDate = lastActivity.ActivityDate,
                        LastQuoteActivityTypeName = lastActivity.ActivityType.Name,
                        LastActivityByUserName = lastActivity.User.Contact.EnglishName,
                        Notes = f.Notes,
                        ShipperId = f.ShipperId,
                        Shipper = f.ShipperName,
                        ShipperReference1 = f.ShipperReference1,
                        Consignee = f.ConsigneeName,
                        CustomerName = f.CustomerName,
                        CustomerId = f.CustomerId,
                        FromPortId = f.FromPortId,
                        ToPortId = f.ToPortId,
                        FromPartnerId = f.FromPartnerId,
                        ToPartnerId = f.ToPartnerId,
                        FromPartnerAddressId = f.FromPartnerAddressId,
                        ToPartnerAddressId = f.ToPartnerAddressId,
                        StageId = f.StageId,
                        StageName = f.Stage == null ? "" : f.Stage.Name,
                        StageMaxDays = f.Stage == null ? null : f.Stage.MaxDays,
                        StageDueDate = f.StageDueDate,
                        RatingCode = f.RatingCode,
                        RatingName = f.Rating == null ? "" : f.Rating.Name,
                        RatingIndexOrder = f.Rating == null ? 0 : f.Rating.IndexOrder,
                        LastActivityDate = f.LastActivityDate,
                        LastActivitySubject = f.LastActivitySubject,
                        LastActivityTypeCode = f.LastActivityTypeCode,
                        NextActivityDate = f.NextActivityDate,
                        NextActivitySubject = f.NextActivitySubject,
                        NextActivityTypeCode = f.NextActivityTypeCode,
                        LastActivityTypeName = f.LastActivityTypeCode == "CL" ? "Call" : (f.LastActivityTypeCode == "TS" ? "Task" : (f.LastActivityTypeCode == "AP" ? "Appointment" : (f.LastActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                        NextActivityTypeName = f.NextActivityTypeCode == "CL" ? "Call" : (f.NextActivityTypeCode == "TS" ? "Task" : (f.NextActivityTypeCode == "AP" ? "Appointment" : (f.NextActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                        OpportunityId = f.OpportunityId,
                        IsAutomaticallyClosed = f.IsAutomaticallyClosed,
                        AutomaticallyCloseDate = f.AutomaticallyCloseDate,
                        AutomaticallyCloseDays = f.AutomaticallyCloseDays,
                        UpdateDate = f.UpdateDate,
                        UpdatedByUserId = f.UpdatedByUserId,
                        IncotermCode = f.Incoterm == null ? "" : f.Incoterm.Code,
                        FromPortName = f.FromPort == null ? "" : f.FromPort.EnglishName,
                        ToPortName = f.ToPort == null ? "" : f.ToPort.EnglishName,

                        FromPort = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                        (f.FromPartnerAddress != null ? f.FromPartnerAddress.City : "")
                        :
                        (f.FromPort != null ? f.FromPort.Code : ""),

                        FromCountryCode = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                        ((f.FromPartnerAddress != null && f.FromPartnerAddress.Country != null ? f.FromPartnerAddress.Country.Code : ""))
                        :
                        ((f.FromPort != null && f.FromPort.Country != null ? f.FromPort.Country.Code : "")),

                        FromPortCountry = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                        ((f.FromPartnerAddress != null && f.FromPartnerAddress.Country != null ? f.FromPartnerAddress.Country.EnglishName : ""))
                        :
                        ((f.FromPort != null && f.FromPort.Country != null ? f.FromPort.Country.EnglishName : "")),

                        ToPort = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                        (f.ToPartnerAddress != null ? f.ToPartnerAddress.City : "")
                        :
                        (f.ToPort != null ? f.ToPort.Code : ""),

                        ToCountryCode = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                        ((f.ToPartnerAddress != null && f.ToPartnerAddress.Country != null ? f.ToPartnerAddress.Country.Code : ""))
                        :
                        ((f.ToPort != null && f.ToPort.Country != null ? f.ToPort.Country.Code : "")),

                        ToPortCountry = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                        ((f.ToPartnerAddress != null && f.ToPartnerAddress.Country != null ? f.ToPartnerAddress.Country.EnglishName : ""))
                        :
                        ((f.ToPort != null && f.ToPort.Country != null ? f.ToPort.Country.EnglishName : "")),

                        Routing = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                        ((f.FromPartnerAddress == null ? "" : f.FromPartnerAddress.City) + " > " + (f.ToPartnerAddress == null ? "" : f.ToPartnerAddress.City))
                        :
                        ((f.FromPort == null ? "" : f.FromPort.Code) + " > " + (f.ToPort == null ? "" : f.ToPort.Code)),

                        Subject = f.Subject,
                        IsSubjectEdited = f.IsSubjectEdited,
                        ProductCode = f.ProductCode,
                        TransitTime = f.TransitTime,
                        DepartureFrequency = f.DepartureFrequency,
                        ETD = f.ETD,
                        ETA = f.ETA,
                        AgentId = f.AgentId,
                        AgentAddressId = f.AgentAddressId,
                        AgentContactId = f.AgentContactId,
                        AgentReference1 = f.AgentReference1,
                        AgentReference2 = f.AgentReference2,
                        AgentName = f.AgentCard == null ? null : f.AgentCard.EnglishName,
                        LastStageDate = f.LastStageDate,
                        TEU = f.TEU,
                        IsSaleCurrencySameAsCost = f.IsSaleCurrencySameAsCost,
                        IsChargesByVAT = f.IsChargesByVAT,
                        ValueOfGoods = f.ValueOfGoods,
                        TotalPerContainer = f.TotalPerContainer,
                        IsQuoteDataExternal = f.IsQuoteDataExternal,
                        IsQuoteDocumentExternal = f.IsQuoteDocumentExternal,
                        QuotationSections = f.QuotationSections,
                        NumberOfFollowUps = f.NumberOfFollowUps,
                    };

                    ContactRepository rep = new ContactRepository(tenant);
                    Contact contact = rep.GetSingleContact(f.SalesmanUserId, tenant);
                    if (contact != null)
                    {
                        list.Salesman = contact.EnglishName;
                    }

                    else
                    {
                        list.Salesman = "";
                    }

                    entityList.Add(list);
                }
            }

            if (entityList.Count > 0)
            {
                entityList = BranchPermitionsFilter.AddUserBranchRestrictionFilters<QuoteList>(new QueryOperations(), entityList.AsQueryable<QuoteList>(), tenant).ToList();
                entityList = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteList>(new QueryOperations(), entityList.AsQueryable<QuoteList>(), tenant).ToList();
            }

            return entityList;
        }
        public List<ChartingDataClass> GetQuotesChartDataCustom(DateTime?FromDate,DateTime?ToDate, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();
          

            IQueryable<Quote> dataSource =
                (from d in repository.context.Quotes.Include("Stage")
                 where d.Tenant == tenant
                 && d.SalesmanUserId != null
                 && d.IsCancelled == false
                 && d.IsClosed == true
                 select d);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            dataSource = businessUnitFilter.RunFilter(dataSource);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.SalesmanUserId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSource = dataSource.Where(d => d.BusinessUnitId == businessUnitId);
            }

            //QTAC : Accepted
            //QTDC : Declined
            dataSource = dataSource.Where(d => d.Stage.Code == "QTAC" || d.Stage.Code == "QTDC");

          
                dataSource =
                    dataSource.Where(d =>
                    (d.Stage.Code == "QTAC" && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) <= ToDate)
                    ||
                    (d.Stage.Code == "QTDC" && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) <= ToDate)
                    );
            

            myResult = (from d in dataSource
                        where d.StageId != null
                        group d by new { d.Stage } into g
                        select new ChartingDataClass()
                        {
                            Id = g.Key.Stage.Code,
                            GroupedId = g.Key.Stage.Id,
                            DataTypeCode = g.Key.Stage.Code,
                            StringProperty = g.Key.Stage.Name,
                            IntegerProperty = g.Count(),
                            TypeIndex = g.Key.Stage.Code == "QTAC" ? 0 : 1,
                        }).ToList();

            return myResult;
        }

        public List<ChartingDataClass> GetQuotesChartData(string code, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date1 = helper.Date1;
            DateTime? date2 = helper.Date2;
            int days = helper.Days;

            IQueryable<Quote> dataSource =
                (from d in repository.context.Quotes.Include("Stage")
                 where d.Tenant == tenant
                 && d.SalesmanUserId != null
                 && d.IsCancelled == false
                 && d.IsClosed == true
                 select d);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            dataSource = businessUnitFilter.RunFilter(dataSource);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.SalesmanUserId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSource = dataSource.Where(d => d.BusinessUnitId == businessUnitId);
            }

            //QTAC : Accepted
            //QTDC : Declined
            dataSource = dataSource.Where(d => d.Stage.Code == "QTAC" || d.Stage.Code == "QTDC");

            if (days >= 0)
            {
                dataSource =
                    dataSource.Where(d =>
                    (d.Stage.Code == "QTAC" && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) == todayDate)
                    ||
                    (d.Stage.Code == "QTDC" && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) == todayDate)
                    );
            }

            else if (days == -1)
            {
                dataSource =
                    dataSource.Where(d =>
                    (d.Stage.Code == "QTAC" && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) == date1)
                    ||
                    (d.Stage.Code == "QTDC" && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) == date1)
                    );
            }

            else
            {
                dataSource =
                    dataSource.Where(d =>
                    (d.Stage.Code == "QTAC" && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) <= date2)
                    ||
                    (d.Stage.Code == "QTDC" && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) <= date2)
                    );
            }

            myResult = (from d in dataSource
                        where d.StageId != null
                        group d by new { d.Stage } into g
                        select new ChartingDataClass()
                        {
                            Id = g.Key.Stage.Code,
                            GroupedId = g.Key.Stage.Id,
                            DataTypeCode = g.Key.Stage.Code,
                            StringProperty = g.Key.Stage.Name,
                            IntegerProperty = g.Count(),
                            TypeIndex = g.Key.Stage.Code == "QTAC" ? 0 : 1,
                            Day = days,
                        }).ToList();

            return myResult;
        }

        public List<ChartingDataClass> GetQuotesGroupBySalesman(string code, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date1 = helper.Date1;
            DateTime? date2 = helper.Date2;
            int days = helper.Days;

            IQueryable<Quote> dataSourceQuery =
                (from d in repository.context.Quotes.Include("SalesmanUser").Include("Stage")
                 where d.Tenant == tenant
                 && d.SalesmanUserId != null
                 && !d.IsCancelled
                 select d);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            dataSourceQuery = businessUnitFilter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.SalesmanUserId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (fieldCode == "C")
            {
                if (days >= 0)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) == todayDate);
                }

                else if (days == -1)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) == date1);
                }

                else
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) <= date2);
                }

                if (dataSourceQuery != null)
                {
                    if (isTopTen)
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                             select new ChartingDataClass()
                             {
                                 Id = g.Key.SalesmanUserId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 Day = days,
                                 OwnerId = g.Key.SalesmanUserId,
                                 BusinessUnitId = businessUnitId,
                                 Code=code,
                             })
                             .OrderByDescending(o => o.IntegerProperty)
                             .Take(10)
                             .ToList();
                    }

                    else
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                             select new ChartingDataClass()
                             {
                                 Id = g.Key.SalesmanUserId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 Day = days,
                                 OwnerId = g.Key.SalesmanUserId,
                                 BusinessUnitId = businessUnitId,
                                 Code=code
                             })
                             .ToList();
                    }
                }
            }

            else if (fieldCode == "P")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.IsClosed == false);

                if (dataSourceQuery != null)
                {
                    if (isTopTen)
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                             select new ChartingDataClass()
                             {
                                 Id = g.Key.SalesmanUserId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 Day = days,
                                 OwnerId = g.Key.SalesmanUserId,
                                 BusinessUnitId = businessUnitId,
                             })
                             .OrderByDescending(o => o.IntegerProperty)
                             .Take(10)
                             .ToList();
                    }

                    else
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                             select new ChartingDataClass()
                             {
                                 Id = g.Key.SalesmanUserId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 Day = days,
                                 OwnerId = g.Key.SalesmanUserId,
                                 BusinessUnitId = businessUnitId,
                             })
                             .ToList();
                    }
                }
            }

            else if (fieldCode == "S")
            {
                // Entity Status
                //QTAC : Accepted
                //QTDC : Declined

                dataSourceQuery = dataSourceQuery.Where(d => d.IsClosed == true && (d.Stage.Code == "QTAC" || d.Stage.Code == "QTDC"));

                if (days >= 0)
                {
                    dataSourceQuery =
                        dataSourceQuery.Where(d =>
                        (d.Stage.Code == "QTAC" && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) == todayDate)
                        ||
                        (d.Stage.Code == "QTDC" && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) == todayDate)
                        );
                }

                else if (days == -1)
                {
                    dataSourceQuery =
                        dataSourceQuery.Where(d =>
                        (d.Stage.Code == "QTAC" && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) == date1)
                        ||
                        (d.Stage.Code == "QTDC" && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) == date1)
                        );
                }

                else
                {
                    dataSourceQuery =
                        dataSourceQuery.Where(d =>
                        (d.Stage.Code == "QTAC" && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) <= date2)
                        ||
                        (d.Stage.Code == "QTDC" && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) <= date2)
                        );
                }

                if (dataSourceQuery != null)
                {
                    IQueryable<Quote> data_Won = dataSourceQuery.Where(d => d.Stage.Code == "QTAC");
                    IQueryable<Quote> data_Lost = dataSourceQuery.Where(d => d.Stage.Code == "QTDC");

                    List<ChartingDataClass> myData = new List<ChartingDataClass>();

                    if (isTopTen)
                    {
                        myData =
                           (from d in dataSourceQuery
                            group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                            select new ChartingDataClass()
                            {
                                Id = g.Key.SalesmanUserId,
                                StringProperty = g.Key.EnglishName,
                                IntegerProperty = g.Count(),
                            })
                            .OrderByDescending(o => o.IntegerProperty)
                            .Take(10)
                            .ToList();
                    }

                    else
                    {
                        myData =
                           (from d in dataSourceQuery
                            group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                            select new ChartingDataClass()
                            {
                                Id = g.Key.SalesmanUserId,
                                StringProperty = g.Key.EnglishName,
                                IntegerProperty = g.Count(),
                            })
                            .ToList();
                    }

                    foreach (ChartingDataClass item in myData)
                    {
                        myResult.Add(new ChartingDataClass()
                        {
                            Id = item.Id + ":QTAC",
                            DataTypeCode = "QTAC",
                            StringProperty = item.StringProperty,
                            IntegerProperty = data_Won.Where(d => d.SalesmanUserId == item.Id).Count(),
                            Day = days,
                            OwnerId = ownerId,
                            BusinessUnitId = businessUnitId,
                        });

                        myResult.Add(new ChartingDataClass()
                        {
                            Id = item.Id + ":QTDC",
                            DataTypeCode = "QTDC",
                            StringProperty = item.StringProperty,
                            IntegerProperty = data_Lost.Where(d => d.SalesmanUserId == item.Id).Count(),
                            Day = days,
                            OwnerId = ownerId,
                            BusinessUnitId = businessUnitId,
                        });
                    }
                }
            }

            return myResult;
        }

        public List<ChartingDataClass> GetQuotesGroupBySalesmanCustom(DateTime? FromDate,DateTime? ToDate, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();


            IQueryable<Quote> dataSourceQuery =
                (from d in repository.context.Quotes.Include("SalesmanUser").Include("Stage")
                 where d.Tenant == tenant
                 && d.SalesmanUserId != null
                 && !d.IsCancelled
                 select d);

            QuoteBusinessUnitFilter businessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            dataSourceQuery = businessUnitFilter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.SalesmanUserId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (fieldCode == "C")
            {
                                            
            dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) <= ToDate);
                
                if (dataSourceQuery != null)
                {
                    if (isTopTen)
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                             select new ChartingDataClass()
                             {
                                 Id = g.Key.SalesmanUserId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.SalesmanUserId,
                                 BusinessUnitId = businessUnitId,
                             })
                             .OrderByDescending(o => o.IntegerProperty)
                             .Take(10)
                             .ToList();
                    }

                    else
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                             select new ChartingDataClass()
                             {
                                 Id = g.Key.SalesmanUserId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.SalesmanUserId,
                                 BusinessUnitId = businessUnitId,
                             })
                             .ToList();
                    }
                }
            }

            else if (fieldCode == "P")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.IsClosed == false);

                if (dataSourceQuery != null)
                {
                    if (isTopTen)
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                             select new ChartingDataClass()
                             {
                                 Id = g.Key.SalesmanUserId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.SalesmanUserId,
                                 BusinessUnitId = businessUnitId,
                             })
                             .OrderByDescending(o => o.IntegerProperty)
                             .Take(10)
                             .ToList();
                    }

                    else
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                             select new ChartingDataClass()
                             {
                                 Id = g.Key.SalesmanUserId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.SalesmanUserId,
                                 BusinessUnitId = businessUnitId,
                             })
                             .ToList();
                    }
                }
            }

            else if (fieldCode == "S")
            {
                // Entity Status
                //QTAC : Accepted
                //QTDC : Declined

                dataSourceQuery = dataSourceQuery.Where(d => d.IsClosed == true && (d.Stage.Code == "QTAC" || d.Stage.Code == "QTDC"));
                
                    dataSourceQuery =
                        dataSourceQuery.Where(d =>
                        (d.Stage.Code == "QTAC" && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) <= ToDate)
                        ||
                        (d.Stage.Code == "QTDC" && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) <= ToDate)
                        );
                

                if (dataSourceQuery != null)
                {
                    IQueryable<Quote> data_Won = dataSourceQuery.Where(d => d.Stage.Code == "QTAC");
                    IQueryable<Quote> data_Lost = dataSourceQuery.Where(d => d.Stage.Code == "QTDC");

                    List<ChartingDataClass> myData = new List<ChartingDataClass>();

                    if (isTopTen)
                    {
                        myData =
                           (from d in dataSourceQuery
                            group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                            select new ChartingDataClass()
                            {
                                Id = g.Key.SalesmanUserId,
                                StringProperty = g.Key.EnglishName,
                                IntegerProperty = g.Count(),
                            })
                            .OrderByDescending(o => o.IntegerProperty)
                            .Take(10)
                            .ToList();
                    }

                    else
                    {
                        myData =
                           (from d in dataSourceQuery
                            group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                            select new ChartingDataClass()
                            {
                                Id = g.Key.SalesmanUserId,
                                StringProperty = g.Key.EnglishName,
                                IntegerProperty = g.Count(),
                            })
                            .ToList();
                    }

                    foreach (ChartingDataClass item in myData)
                    {
                        myResult.Add(new ChartingDataClass()
                        {
                            Id = item.Id + ":QTAC",
                            DataTypeCode = "QTAC",
                            StringProperty = item.StringProperty,
                            IntegerProperty = data_Won.Where(d => d.SalesmanUserId == item.Id).Count(),
                            OwnerId = ownerId,
                            BusinessUnitId = businessUnitId,
                        });

                        myResult.Add(new ChartingDataClass()
                        {
                            Id = item.Id + ":QTDC",
                            DataTypeCode = "QTDC",
                            StringProperty = item.StringProperty,
                            IntegerProperty = data_Lost.Where(d => d.SalesmanUserId == item.Id).Count(),
                            OwnerId = ownerId,
                            BusinessUnitId = businessUnitId,
                        });
                    }
                }
            }

            return myResult;
        }

        public List<ChartingDataClass> GetStageFunnelData(string ownerId, string businessUnitId, int tenant, string RecordsTypeCode)
        {
            IQueryable<Quote> dataSourceQuery =
                (from d in repository.context.Quotes.Include("Stage")
                 where d.Tenant == tenant
                 && !d.IsClosed
                 && !d.IsCancelled
                 && d.StageId != null
                 && d.Stage.Rank > 0
                 select d);

            QuoteBusinessUnitFilter filter = new QuoteBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (RecordsTypeCode == "C")
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.CreatedByUserId == ownerId);
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.SalesmanUserId == ownerId);
                }

                if (!string.IsNullOrEmpty(businessUnitId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
                }
            }

            List<ChartingDataClass> result =
                (from d in dataSourceQuery
                 group d by new { d.StageId, d.Stage.Name, d.Stage.Rank } into g
                 select new ChartingDataClass()
                 {
                     Id = g.Key.StageId,
                     LabelProperty = g.Key.Name,
                     DecimalProperty = g.Count(),
                     IntegerProperty = g.Key.Rank,
                     GroupedId = g.Key.StageId,
                 }).ToList();

            return result;
        }

        public string GetQuoteAutomaticSubject(QuotePM entityPM)
        {
            string mySubject = null;

            int tenant = entityPM.Tenant;

            bool isInlandDomestic = (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I");

            AddressRepository addressRepository = new AddressRepository(tenant);

            if (!string.IsNullOrEmpty(entityPM.IncotermId))
            {
                IncotermRepository incotermRepository = new IncotermRepository(tenant);
                Incoterm incoterm = incotermRepository.GetSingleIncoterm(entityPM.IncotermId, tenant);
                if (incoterm != null)
                {
                    mySubject = incoterm.Code;
                }
            }

            if (isInlandDomestic)
            {
                #region
                if (!string.IsNullOrEmpty(entityPM.FromPartnerAddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(entityPM.FromPartnerAddressId, tenant);
                    if (myAddress != null)
                    {
                        if (!string.IsNullOrEmpty(myAddress.City))
                        {
                            mySubject = string.IsNullOrEmpty(mySubject) ? myAddress.City : mySubject + " " + myAddress.City;
                        }

                        else if (!string.IsNullOrEmpty(myAddress.ZipCode))
                        {
                            mySubject = string.IsNullOrEmpty(mySubject) ? myAddress.ZipCode : mySubject + " " + myAddress.ZipCode;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.ToPartnerAddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(entityPM.ToPartnerAddressId, tenant);
                    if (myAddress != null)
                    {
                        if (!string.IsNullOrEmpty(myAddress.City))
                        {
                            mySubject = string.IsNullOrEmpty(mySubject) ? myAddress.City : mySubject + " > " + myAddress.City;
                        }

                        else if (!string.IsNullOrEmpty(myAddress.ZipCode))
                        {
                            mySubject = string.IsNullOrEmpty(mySubject) ? myAddress.ZipCode : mySubject + " > " + myAddress.ZipCode;
                        }
                    }
                }
                #endregion
            }

            else
            {
                #region
                if (entityPM.IncludePickUp)
                {
                    if (!string.IsNullOrEmpty(entityPM.PickUpAddressId))
                    {
                        Address myAddress = addressRepository.GetSingleAddress(entityPM.PickUpAddressId, tenant);
                        if (myAddress != null)
                        {
                            if (!string.IsNullOrEmpty(myAddress.City))
                            {
                                mySubject = string.IsNullOrEmpty(mySubject) ? myAddress.City : mySubject + " " + myAddress.City;
                            }

                            else if (!string.IsNullOrEmpty(myAddress.ZipCode))
                            {
                                mySubject = string.IsNullOrEmpty(mySubject) ? myAddress.ZipCode : mySubject + " " + myAddress.ZipCode;
                            }
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(entityPM.FromAddressCity))
                        {
                            mySubject = string.IsNullOrEmpty(mySubject) ? entityPM.FromAddressCity : mySubject + " " + entityPM.FromAddressCity;
                        }

                        else if (!string.IsNullOrEmpty(entityPM.FromAddressZipCode))
                        {
                            mySubject = string.IsNullOrEmpty(mySubject) ? entityPM.FromAddressZipCode : mySubject + " " + entityPM.FromAddressZipCode;
                        }
                    }
                }

                else if (!string.IsNullOrEmpty(entityPM.FromPortId))
                {
                    PortPM myPort = PortQuery.GetSinglePort(entityPM.Tenant, entityPM.FromPortId, true);
                    if (myPort != null)
                    {
                        mySubject = string.IsNullOrEmpty(mySubject) ? myPort.Code : mySubject + " " + myPort.Code;
                    }
                }

                if (entityPM.IncludeDelivery)
                {
                    if (!string.IsNullOrEmpty(entityPM.DeliveryAddressId))
                    {
                        Address myAddress = addressRepository.GetSingleAddress(entityPM.DeliveryAddressId, tenant);
                        if (myAddress != null)
                        {
                            if (!string.IsNullOrEmpty(myAddress.City))
                            {
                                mySubject = string.IsNullOrEmpty(mySubject) ? myAddress.City : mySubject + " > " + myAddress.City;
                            }

                            else if (!string.IsNullOrEmpty(myAddress.ZipCode))
                            {
                                mySubject = string.IsNullOrEmpty(mySubject) ? myAddress.ZipCode : mySubject + " > " + myAddress.ZipCode;
                            }
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(entityPM.ToAddressCity))
                        {
                            mySubject = string.IsNullOrEmpty(mySubject) ? entityPM.ToAddressCity : mySubject + " > " + entityPM.ToAddressCity;
                        }

                        else if (!string.IsNullOrEmpty(entityPM.ToAddressZipCode))
                        {
                            mySubject = string.IsNullOrEmpty(mySubject) ? entityPM.ToAddressZipCode : mySubject + " > " + entityPM.ToAddressZipCode;
                        }
                    }
                }

                else if (!string.IsNullOrEmpty(entityPM.ToPortId))
                {
                    PortPM myPort = PortQuery.GetSinglePort(entityPM.Tenant, entityPM.ToPortId, true);
                    if (myPort != null)
                    {
                        mySubject = string.IsNullOrEmpty(mySubject) ? myPort.Code : mySubject + " > " + myPort.Code;
                    }
                }

                #endregion
            }

            return mySubject;
        }

        private QuotePM MapPOCOToPM(Quote entityPOCO)
        {
            QuotePM entityPM = new QuotePM()
            {
                Id = entityPOCO.Id,
                Tenant = entityPOCO.Tenant,
                ShipmentTypeId = entityPOCO.ShipmentTypeId,
                IsFreightBySteps = entityPOCO.IsFreightBySteps,
                ConcurrencyGUID = entityPOCO.ConcurrencyGUID,
                ExpirationDate = entityPOCO.ExpirationDate,
                ExpirationDays = entityPOCO.ExpirationDays,
                IsFixedPrice = entityPOCO.IsFixedPrice,
                SentDate = entityPOCO.SentDate,
                AcceptedDate = entityPOCO.AcceptedDate,
                DeclinedDate = entityPOCO.DeclinedDate,
                Subject = entityPOCO.Subject,
                IsSubjectEdited = entityPOCO.IsSubjectEdited,
                ProductCode = entityPOCO.ProductCode,
                RatingCode = entityPOCO.RatingCode,
                RatingName = entityPOCO.RatingCode == "C" ? "Cold" : (entityPOCO.RatingCode == "H" ? "Hot" : entityPOCO.RatingCode == "N" ? "Neutral" : "Warm"),
                LastActivityDate = entityPOCO.LastActivityDate,
                LastActivitySubject = entityPOCO.LastActivitySubject,
                LastActivityTypeCode = entityPOCO.LastActivityTypeCode,
                NextActivityDate = entityPOCO.NextActivityDate,
                NextActivitySubject = entityPOCO.NextActivitySubject,
                NextActivityTypeCode = entityPOCO.NextActivityTypeCode,
                OpportunityId = entityPOCO.OpportunityId,
                IsAutomaticallyClosed = entityPOCO.IsAutomaticallyClosed,
                AutomaticallyCloseDate = entityPOCO.AutomaticallyCloseDate,
                AutomaticallyCloseDays = entityPOCO.AutomaticallyCloseDays,
                UpdateDate = entityPOCO.UpdateDate,
                UpdatedByUserId = entityPOCO.UpdatedByUserId,
                IsClosed = entityPOCO.IsClosed,
                DirectionName = entityPOCO.Direction.Name,
                TransportModeName = entityPOCO.TransportMode.Name,
                DescriptionOfGoods = entityPOCO.DescriptionOfGoods,
                DirectionId = entityPOCO.DirectionId,
                LastModified = entityPOCO.LastModified,
                Notes = entityPOCO.Notes,
                OpenDate = entityPOCO.OpenDate,
                CreatedByUserId = entityPOCO.CreatedByUserId,
                BusinessUnitId = entityPOCO.BusinessUnitId,
                QuoteNumber = entityPOCO.QuoteNumber,
                BranchId = entityPOCO.BranchId,
                DepartmentId = entityPOCO.DepartmentId,
                EstimateProfit = entityPOCO.EstimateProfit,
                EstimateProfitEdited = entityPOCO.EstimateProfitEdited,
                LastVersionNumber = entityPOCO.LastVersionNumber,
                PackageType1Id = entityPOCO.PackageType1Id,
                PackageType2Id = entityPOCO.PackageType2Id,
                PackageType3Id = entityPOCO.PackageType3Id,
                PackageType4Id = entityPOCO.PackageType4Id,
                PackageType5Id = entityPOCO.PackageType5Id,
                PackageType1Quantity = entityPOCO.PackageType1Quantity,
                PackageType2Quantity = entityPOCO.PackageType2Quantity,
                PackageType3Quantity = entityPOCO.PackageType3Quantity,
                PackageType4Quantity = entityPOCO.PackageType4Quantity,
                PackageType5Quantity = entityPOCO.PackageType5Quantity,
                IsDangerous = entityPOCO.IsDangerous,
                QuoteTypeCode = entityPOCO.QuoteTypeCode,
                QuoteTypeName = entityPOCO.QuoteType.Name,
                IsByContainer = entityPOCO.IsByContainer,
                IsByKG = entityPOCO.IsByKG,
                MainCarriageCarrierId = entityPOCO.MainCarriageCarrierId,
                IsCancelled = entityPOCO.IsCancelled,
                SaleCurrencyId = entityPOCO.SaleCurrencyId,
                SaleCurrencyCode = entityPOCO.SaleCurrency == null ? "" : entityPOCO.SaleCurrency.Code,
                ExchangeRate = entityPOCO.ExchangeRate,
                UsageCount = entityPOCO.UsageCount,
                LastUsageDate = entityPOCO.LastUsageDate,
                QuoteClosingReasonCode = entityPOCO.QuoteClosingReasonCode,
                VolumeUnitCode = entityPOCO.VolumeUnitCode,
                DimensionsUnitCode = entityPOCO.DimensionsUnitCode,
                GrossWeightUnitCode = entityPOCO.GrossWeightUnitCode,
                ChargeableWeightUnitCode = entityPOCO.ChargeableWeightUnitCode,
                Volume = entityPOCO.Volume,
                VolumetricWeight = entityPOCO.VolumetricWeight,
                GrossWeight = entityPOCO.GrossWeight,
                GrossWeightInKG = entityPOCO.GrossWeightInKG,
                GrossWeightPerTon = entityPOCO.GrossWeightPerTon,
                ChargeableWeight = entityPOCO.ChargeableWeight,
                TransportModeId = entityPOCO.TransportModeId,
                Ratio = entityPOCO.Ratio,
                DimFactor = entityPOCO.DimFactor,
                NumberOfPackages = entityPOCO.NumberOfPackages,
                NumberOfContainers = entityPOCO.NumberOfContainers,
                FromPortId = entityPOCO.FromPortId,
                ToPortId = entityPOCO.ToPortId,
                QuoteCustomerTypeCode = entityPOCO.QuoteCustomerTypeCode,
                CustomerId = entityPOCO.CustomerId,
                CustomerName = entityPOCO.CustomerName,
                CustomerContactId = entityPOCO.CustomerContactId,
                CustomerReference1 = entityPOCO.CustomerReference1,
                CustomerReference2 = entityPOCO.CustomerReference2,
                ShipperId = entityPOCO.ShipperId,
                ShipperName = entityPOCO.ShipperName,
                ShipperContactId = entityPOCO.ShipperContactId,
                ShipperReference1 = entityPOCO.ShipperReference1,
                ShipperReference2 = entityPOCO.ShipperReference2,
                ConsigneeId = entityPOCO.ConsigneeId,
                ConsigneeName = entityPOCO.ConsigneeName,
                ConsigneeContactId = entityPOCO.ConsigneeContactId,
                ConsigneeReference1 = entityPOCO.ConsigneeReference1,
                ConsigneeReference2 = entityPOCO.ConsigneeReference2,
                FreelancerId = entityPOCO.FreelancerId,
                FreelancerAddressId = entityPOCO.FreelancerAddressId,
                FreelancerContactId = entityPOCO.FreelancerContactId,
                IncotermId = entityPOCO.IncotermId,
                IncotermCode = entityPOCO.Incoterm == null ? null : entityPOCO.Incoterm.Code,
                IncotermName = entityPOCO.Incoterm == null ? null : entityPOCO.Incoterm.Name,
                SalesmanUserId = entityPOCO.SalesmanUserId,
                SalesmanName = entityPOCO.SalesmanUser == null ? null : (entityPOCO.SalesmanUser.Contact == null ? null : entityPOCO.SalesmanUser.Contact.EnglishName),
                StageId = entityPOCO.StageId,
                StageDueDate = entityPOCO.StageDueDate,
                StageName = entityPOCO.Stage == null ? null : entityPOCO.Stage.Name,
                StageMaxDays = entityPOCO.Stage == null ? null : entityPOCO.Stage.MaxDays,
                FromPartnerId = entityPOCO.FromPartnerId,
                FromPartnerAddressId = entityPOCO.FromPartnerAddressId,
                FromPartnerName = entityPOCO.FromPartnerCard == null ? null : entityPOCO.FromPartnerCard.EnglishName,
                ToPartnerId = entityPOCO.ToPartnerId,
                ToPartnerAddressId = entityPOCO.ToPartnerAddressId,
                ToPartnerName = entityPOCO.ToPartnerCard == null ? null : entityPOCO.ToPartnerCard.EnglishName,
                Field1 = new CustomFieldClass("Field1", "Quote", entityPOCO.Field1),
                Field2 = new CustomFieldClass("Field2", "Quote", entityPOCO.Field2),
                Field3 = new CustomFieldClass("Field3", "Quote", entityPOCO.Field3),
                Field4 = new CustomFieldClass("Field4", "Quote", entityPOCO.Field4),
                Field5 = new CustomFieldClass("Field5", "Quote", entityPOCO.Field5),
                Field6 = new CustomFieldClass("Field6", "Quote", entityPOCO.Field6),
                Field7 = new CustomFieldClass("Field7", "Quote", entityPOCO.Field7),
                Field8 = new CustomFieldClass("Field8", "Quote", entityPOCO.Field8),
                Field9 = new CustomFieldClass("Field9", "Quote", entityPOCO.Field9),
                Field10 = new CustomFieldClass("Field10", "Quote", entityPOCO.Field10),
                TransitTime = entityPOCO.TransitTime,
                DepartureFrequency = entityPOCO.DepartureFrequency,
                ETD = entityPOCO.ETD,
                ETA = entityPOCO.ETA,
                AgentId = entityPOCO.AgentId,
                AgentAddressId = entityPOCO.AgentAddressId,
                AgentContactId = entityPOCO.AgentContactId,
                AgentReference1 = entityPOCO.AgentReference1,
                AgentReference2 = entityPOCO.AgentReference2,
                AgentName = entityPOCO.AgentCard == null ? null : entityPOCO.AgentCard.EnglishName,
                MoveTypeId = entityPOCO.MoveTypeId,
                TEU = entityPOCO.TEU,
                IsSaleCurrencySameAsCost = entityPOCO.IsSaleCurrencySameAsCost,
                ValueOfGoods = entityPOCO.ValueOfGoods,
                ValueOfGoodsCurrencyId = entityPOCO.ValueOfGoodsCurrencyId,
                IsChargesByVAT = entityPOCO.IsChargesByVAT,
                QuoteTemplateId = entityPOCO.QuoteTemplateId,
                TotalPerContainer = entityPOCO.TotalPerContainer,
                IsQuoteDataExternal = entityPOCO.IsQuoteDataExternal,
                IsQuoteDocumentExternal = entityPOCO.IsQuoteDocumentExternal,
                LastStageDate = entityPOCO.LastStageDate,
                QuotationSections = entityPOCO.QuotationSections,
                NumberOfFollowUps = entityPOCO.NumberOfFollowUps,
                SameOrFixed = entityPOCO.IsSaleCurrencySameAsCost ? "Same as Cost Currency" : "Fixed",
                QuoteLevel = entityPOCO.ShipmentTypeId,
            };

            int tenant = entityPOCO.Tenant;
            string entityId = entityPOCO.Id;

            ICommonDataContext myCommonContext= CommonDataContext.GetContext(tenant);
            IQuotesContext myQuotesContext= QuotesContext.GetContext(tenant);

            FollowUpRepository followUpsRepository = new FollowUpRepository(tenant);
            PortRepository portRepository = new PortRepository(myCommonContext);
            CardRepository cardsRepository = new CardRepository(myCommonContext);
            CountryRepository countryRepository = new CountryRepository(myCommonContext);
            AddressRepository addressRepository = new AddressRepository(myCommonContext);

            QuoteChargeRepository quoteChargeRepository = new QuoteChargeRepository(myQuotesContext);
            QuotePackageRepository quotePackageRepository = new QuotePackageRepository(myQuotesContext);
            QuoteTotalVATRepository myTotalVATRepository = new QuoteTotalVATRepository(myQuotesContext);
            QuoteChargeQuery quoteChargeQuery = new QuoteChargeQuery(quoteChargeRepository);
            QuotePackageQuery quotePackageQuery = new QuotePackageQuery(quotePackageRepository);
            QuoteTotalVATQuery myTotalVATQuery = new QuoteTotalVATQuery(myTotalVATRepository);

            bool isInlandDomestic = (entityPOCO.DirectionId == "D" && entityPOCO.TransportModeId == "I");

            #region From | To Location
            if (isInlandDomestic)
            {
                if (!string.IsNullOrEmpty(entityPOCO.FromPartnerAddressId))
                {
                    Address fromAddress = addressRepository.GetSingleAddress(entityPOCO.FromPartnerAddressId, tenant);
                    if (fromAddress != null)
                    {
                        entityPM.FromCountryId = fromAddress.CountryId;

                        string fromLocation = "";

                        if (!string.IsNullOrEmpty(fromAddress.City))
                        {
                            fromLocation = fromAddress.City;
                        }

                        if (fromAddress.Country != null)
                        {
                            entityPM.FromCountryIsEC = fromAddress.Country.EC;

                            if (string.IsNullOrEmpty(fromLocation))
                            {
                                fromLocation = fromAddress.Country.Code;
                            }

                            else
                            {
                                fromLocation += " " + fromAddress.Country.Code;
                            }
                        }

                        entityPM.FromLocation = fromLocation;
                    }
                }

                if (!string.IsNullOrEmpty(entityPOCO.ToPartnerAddressId))
                {
                    Address toAddress = addressRepository.GetSingleAddress(entityPOCO.ToPartnerAddressId, tenant);
                    if (toAddress != null)
                    {
                        entityPM.ToCountryId = toAddress.CountryId;

                        string toLocation = "";

                        if (!string.IsNullOrEmpty(toAddress.City))
                        {
                            toLocation = toAddress.City;
                        }

                        if (toAddress.Country != null)
                        {
                            entityPM.ToCountryIsEC = toAddress.Country.EC;

                            if (string.IsNullOrEmpty(toLocation))
                            {
                                toLocation = toAddress.Country.Code;
                            }

                            else
                            {
                                toLocation += " " + toAddress.Country.Code;
                            }
                        }

                        entityPM.ToLocation = toLocation;
                    }
                }
            }

            else
            {
                PortQuery portQuery = new PortQuery(portRepository);

                PortPM fromPort = portQuery.GetSinglePM(entityPOCO.FromPortId, tenant);
                if (fromPort != null)
                {
                    entityPM.FromCountryId = fromPort.CountryId;
                    entityPM.FromLocation = fromPort.Code + " " + fromPort.EnglishName;

                    entityPM.FromCountryIsEC = fromPort.CountryEC;
                }

                PortPM toPort = portQuery.GetSinglePM(entityPOCO.ToPortId, tenant);
                if (toPort != null)
                {
                    entityPM.ToCountryId = toPort.CountryId;
                    entityPM.ToLocation = toPort.Code + " " + toPort.EnglishName;

                    entityPM.ToCountryIsEC = toPort.CountryEC;
                }
            }
            #endregion

            #region Routings

            if (entityPOCO.FromPort != null)
            {
                entityPM.FromPort = entityPOCO.FromPort.Code;
                entityPM.FromPortName = entityPOCO.FromPort.EnglishName;

                if (entityPOCO.FromPort.Country != null)
                {
                    entityPM.FromPortCountry = entityPOCO.FromPort.Country.EnglishName;
                }
            }

            if (entityPOCO.ToPort != null)
            {
                entityPM.ToPort = entityPOCO.ToPort.Code;
                entityPM.ToPortName = entityPOCO.ToPort.EnglishName;

                if (entityPOCO.ToPort.Country != null)
                {
                    entityPM.ToPortCountry = entityPOCO.ToPort.Country.EnglishName;
                }
            }

            if (isInlandDomestic)
            {
                if (!string.IsNullOrEmpty(entityPOCO.FromPartnerAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPOCO.FromPartnerAddressId, tenant);
                    if (address != null)
                    {
                        if (!string.IsNullOrEmpty(address.CountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                            if (country != null)
                            {
                                entityPM.FromCountryCode = country.Code;
                                entityPM.FromCountryName = country.EnglishName;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(entityPOCO.ToPartnerAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPOCO.ToPartnerAddressId, tenant);
                    if (address != null)
                    {
                        if (!string.IsNullOrEmpty(address.CountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                            if (country != null)
                            {
                                entityPM.ToCountryCode = country.Code;
                                entityPM.ToCountryName = country.EnglishName;
                            }
                        }
                    }
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(entityPOCO.FromPortId))
                {
                    Port port = portRepository.GetSinglePort(tenant, entityPOCO.FromPortId);
                    if (port != null)
                    {
                        if (!string.IsNullOrEmpty(port.CountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(port.CountryId, tenant);
                            if (country != null)
                            {
                                entityPM.FromCountryCode = country.Code;
                                entityPM.FromCountryName = country.EnglishName;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(entityPOCO.ToPortId))
                {
                    Port port = portRepository.GetSinglePort(tenant, entityPOCO.ToPortId);
                    if (port != null)
                    {
                        if (!string.IsNullOrEmpty(port.CountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(port.CountryId, tenant);
                            if (country != null)
                            {
                                entityPM.ToCountryCode = country.Code;
                                entityPM.ToCountryName = country.EnglishName;
                            }
                        }
                    }
                }
            }
            #endregion

            #region Partners

            if (!string.IsNullOrEmpty(entityPOCO.CustomerId))
            {
                CustomerQuery customerQuery = new CustomerQuery(tenant);
                CustomerList list = customerQuery.GetSingleCustomerList(entityPOCO.CustomerId, tenant);

                if (list != null)
                {
                    entityPM.CustomerNote = list.Notes;
                    entityPM.CustomerRankName = list.RankName;
                }
            }

            #region Shipper


            if (!string.IsNullOrEmpty(entityPOCO.ShipperId))
            {
                Card loadedCard = CardRepository.GetSingleCard(entityPOCO.ShipperId, tenant, true);
                entityPM.ShipperNote = loadedCard.Notes;

                if (loadedCard.PartnerTypeId == "PO")
                {
                    entityPM.IsPotentialShipper = true;
                }

                Address addressMain = addressRepository.GetSingleAddressByCardIdAndTypeId(entityPOCO.ShipperId, "M", tenant);
                Address addressPick = addressRepository.GetSingleAddressByCardIdAndTypeId(entityPOCO.ShipperId, "P", tenant);

                if (addressMain != null)
                {
                    entityPM.ShipperMainAddressId = addressMain.Id;
                }

                if (addressPick != null)
                {
                    entityPM.ShipperPickAddressId = addressPick.Id;
                }
            }
            #endregion

            #region Consignee


            if (entityPOCO.ConsigneeId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(entityPOCO.ConsigneeId, tenant, false);
                entityPM.ConsigneeNote = loadedCard.Notes;

                if (loadedCard.PartnerTypeId == "PO")
                {
                    entityPM.IsPotentialConsignee = true;
                }

                Address addressMain = addressRepository.GetSingleAddressByCardIdAndTypeId(entityPOCO.ConsigneeId, "M", tenant);
                Address addressPick = addressRepository.GetSingleAddressByCardIdAndTypeId(entityPOCO.ConsigneeId, "P", tenant);

                if (addressMain != null)
                {
                    entityPM.ConsigneeMainAddressId = addressMain.Id;
                }

                if (addressPick != null)
                {
                    entityPM.ConsigneePickAddressId = addressPick.Id;
                }
            }
            #endregion

            #region Freelancer

            if (!string.IsNullOrEmpty(entityPOCO.FreelancerId))
            {
                Card loadedCard = CardRepository.GetSingleCard(entityPOCO.FreelancerId, tenant, true);
                entityPM.FreelancerName = loadedCard.EnglishName;

                Address adr = addressRepository.GetMainAddressByCardId(entityPOCO.FreelancerId, entityPOCO.Tenant);
                if (adr != null)
                {
                    entityPM.FreelancerAddressId = adr.Id;
                }
            }
            #endregion

            #region Notify
            entityPM.NotifyId = entityPOCO.NotifyId;
            entityPM.NotifyAddressId = entityPOCO.NotifyAddressId;
            entityPM.NotifyContactId = entityPOCO.NotifyContactId;
            if (!string.IsNullOrEmpty(entityPOCO.NotifyId))
            {
                Card loadedCard = CardRepository.GetSingleCard(entityPOCO.NotifyId, entityPOCO.Tenant, true);
                entityPM.NotifyName = loadedCard.EnglishName;
                entityPM.NotifyNote = loadedCard.Notes;

                if (!string.IsNullOrEmpty(entityPOCO.NotifyAddressId))
                {
                    Address notify1Address = addressRepository.GetSingleAddress(entityPM.NotifyAddressId, tenant);
                    if (notify1Address != null)
                    {
                        entityPM.NotifyAddress1 = notify1Address.Address1;
                        entityPM.NotifyAddress2 = notify1Address.Address2;
                        entityPM.NotifyCity = notify1Address.City;
                        entityPM.NotifyCountryId = notify1Address.CountryId;
                        entityPM.NotifyStateId = notify1Address.StateId;
                        entityPM.NotifyZipCode = notify1Address.ZipCode;
                    }
                }
            }
            #endregion

            #endregion

            #region Pickup Delivery
            entityPM.IncludePickUp = entityPOCO.IncludePickUp;
            entityPM.IncludeDelivery = entityPOCO.IncludeDelivery;
            entityPM.PickUpAddressId = entityPOCO.FromAddressId;
            entityPM.DeliveryAddressId = entityPOCO.ToAddressId;
            entityPM.FromAddressCity = entityPOCO.FromAddressCity;
            entityPM.FromAddressZipCode = entityPOCO.FromAddressZipCode;
            entityPM.FromAddressCountryId = entityPOCO.FromAddressCountryId;
            entityPM.ToAddressCity = entityPOCO.ToAddressCity;
            entityPM.ToAddressZipCode = entityPOCO.ToAddressZipCode;
            entityPM.ToAddressCountryId = entityPOCO.ToAddressCountryId;

            if (entityPM.IncludePickUp)
            {
                if (!string.IsNullOrEmpty(entityPM.PickUpAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPM.PickUpAddressId, tenant);
                    if (address != null)
                    {
                        entityPM.PickUpAddress = this.GetAddress(address)
                            + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                            + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                    }
                }
            }

            if (entityPM.IncludeDelivery)
            {
                if (!string.IsNullOrEmpty(entityPM.DeliveryAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPM.DeliveryAddressId, tenant);
                    if (address != null)
                    {
                        entityPM.DeliveryAddress = this.GetAddress(address)
                            + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                            + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                    }
                }
            }


            if (entityPOCO.IncludePickUp)
            {
                #region Pickup Location
                if (!string.IsNullOrEmpty(entityPOCO.FromAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPOCO.FromAddressId, tenant);
                    if (address != null)
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(address.City))
                        {
                            location = address.City;
                        }

                        if (address.Country != null)
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = address.Country.Code;
                            }

                            else
                            {
                                location += " " + address.Country.Code;
                            }
                        }

                        if (!string.IsNullOrEmpty(address.ZipCode))
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = address.ZipCode;
                            }

                            else
                            {
                                location += " - " + address.ZipCode;
                            }
                        }

                        entityPM.PickupLocation = location;
                    }
                }

                else
                {
                    string location = "";

                    if (!string.IsNullOrEmpty(entityPOCO.FromAddressCity))
                    {
                        location = entityPOCO.FromAddressCity;
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.FromAddressCountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(entityPOCO.FromAddressCountryId, tenant);
                        if (country != null)
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = country.Code;
                            }

                            else
                            {
                                location += " " + country.Code;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.FromAddressZipCode))
                    {
                        if (string.IsNullOrEmpty(location))
                        {
                            location = entityPOCO.FromAddressZipCode;
                        }

                        else
                        {
                            location += " - " + entityPOCO.FromAddressZipCode;
                        }
                    }

                    entityPM.PickupLocation = location;
                }
                #endregion
            }
            
            if (entityPOCO.IncludeDelivery)
            {
                #region Delivery Location
                if (!string.IsNullOrEmpty(entityPOCO.ToAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPOCO.ToAddressId, tenant);
                    if (address != null)
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(address.City))
                        {
                            location = address.City;
                        }

                        if (address.Country != null)
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = address.Country.Code;
                            }

                            else
                            {
                                location += " " + address.Country.Code;
                            }
                        }


                        if (!string.IsNullOrEmpty(address.ZipCode))
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = address.ZipCode;
                            }

                            else
                            {
                                location += " - " + address.ZipCode;
                            }
                        }

                        entityPM.DeliveryLocation = location;
                    }
                }

                else
                {
                    string location = "";

                    if (!string.IsNullOrEmpty(entityPOCO.ToAddressCity))
                    {
                        location = entityPOCO.ToAddressCity;
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.ToAddressCountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(entityPOCO.ToAddressCountryId, tenant);
                        if (country != null)
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = country.Code;
                            }

                            else
                            {
                                location += " " + country.Code;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.ToAddressZipCode))
                    {
                        if (string.IsNullOrEmpty(location))
                        {
                            location = entityPOCO.ToAddressZipCode;
                        }

                        else
                        {
                            location += " - " + entityPOCO.ToAddressZipCode;
                        }
                    }

                    entityPM.DeliveryLocation = location;
                }
                #endregion
            }
            #endregion

            #region FollowUps
            List<FollowUp> followupList = followUpsRepository.GetFollowUpsByQuoteId(entityId, tenant);
            foreach (FollowUp follow in followupList)
            {
                QuoteFollowUpPM followUpPM = new QuoteFollowUpPM()
                {
                    Tenant = follow.Tenant,
                    Date = follow.Date,
                    Done = follow.Done,
                    DoneDateTime = follow.DoneDateTime,
                    DoneNote = follow.DoneNote,
                    ExternalDocumentId = follow.DocumentsFilingId,
                    Id = follow.Id,
                    InternalDocumentId = follow.InternalDocumentId,
                    IsNew = follow.IsNew,
                    JobId = follow.JobId,
                    LegType = follow.LegType,
                    Note = follow.Notes,
                    QuoteId = follow.QuoteId,
                    EventTypeId = follow.EventTypeId,
                    EventTypeFollowUpName = follow.EventType.FollowUpEnglishName,
                    ManualActivatedFollowUp = follow.EventType.ManualActivatedFollowUp,
                    OwnerUserId = follow.OwnerUserId,
                    OwnerUserName = follow.OwnerUser.Contact.EnglishName,
                    Area = follow.Area,
                    DocumentTypeId = follow.DocumentTypeId,
                    AutomationId = follow.AutomationId,
                };
                entityPM.FollowUps.Add(followUpPM);
            }
            #endregion

            entityPM.QuoteCharges = quoteChargeQuery.GetQuoteChargesPMsByQuoteId(entityId, tenant);           
            entityPM.TotalVATs = myTotalVATQuery.GetTotalVATs(entityId, tenant);

            if (entityPM.ShipmentTypeId == "FCLD" || entityPM.ShipmentTypeId == "FTL")
            {
                if (entityPM.PackageType1Id != null || entityPM.PackageType1Quantity != null)
                {
                    QuotePackagePM quotePackage1 = new QuotePackagePM();
                    quotePackage1.QuoteId = entityId;
                    quotePackage1.PackageTypeId = entityPM.PackageType1Id;
                    quotePackage1.Quantity = entityPM.QuoteTypeCode == "P" ? 1 : entityPM.PackageType1Quantity;
                    entityPM.QuotePackages.Add(quotePackage1);
                }

                if (entityPM.PackageType2Id != null || entityPM.PackageType2Quantity != null)
                {
                    QuotePackagePM quotePackage2 = new QuotePackagePM();
                    quotePackage2.QuoteId = entityId;
                    quotePackage2.PackageTypeId = entityPM.PackageType2Id;
                    quotePackage2.Quantity = entityPM.QuoteTypeCode == "P" ? 1 : entityPM.PackageType2Quantity;
                    entityPM.QuotePackages.Add(quotePackage2);
                }

                if (entityPM.PackageType3Id != null || entityPM.PackageType3Quantity != null)
                {
                    QuotePackagePM quotePackage3 = new QuotePackagePM();
                    quotePackage3.QuoteId = entityId;
                    quotePackage3.PackageTypeId = entityPM.PackageType3Id;
                    quotePackage3.Quantity = entityPM.QuoteTypeCode == "P" ? 1 : entityPM.PackageType3Quantity;
                    entityPM.QuotePackages.Add(quotePackage3);
                }

                if (entityPM.PackageType4Id != null || entityPM.PackageType4Quantity != null)
                {
                    QuotePackagePM quotePackage4 = new QuotePackagePM();
                    quotePackage4.QuoteId = entityId;
                    quotePackage4.PackageTypeId = entityPM.PackageType4Id;
                    quotePackage4.Quantity = entityPM.QuoteTypeCode == "P" ? 1 : entityPM.PackageType4Quantity;
                    entityPM.QuotePackages.Add(quotePackage4);
                }

                if (entityPM.PackageType5Id != null || entityPM.PackageType5Quantity != null)
                {
                    QuotePackagePM quotePackage5 = new QuotePackagePM();
                    quotePackage5.QuoteId = entityId;
                    quotePackage5.PackageTypeId = entityPM.PackageType5Id;
                    quotePackage5.Quantity = entityPM.QuoteTypeCode == "P" ? 1 : entityPM.PackageType5Quantity;
                    entityPM.QuotePackages.Add(quotePackage5);
                }
            }

            else
            {
                entityPM.QuotePackages = quotePackageQuery.GetQuotePackagesForQuotePMIDTenant(entityId, tenant);
            }

            #region Summery Fields
            if (entityPM.QuoteCharges != null)
            {
                entityPM.CostTotalAmountInLocalCurrency = entityPM.QuoteCharges.Sum(d => d.CostTotalAmountLocal);
                entityPM.CostTotalAmountInSaleCurrency = entityPM.QuoteCharges.Sum(d => d.CostAmountInSaleCurrency);
                entityPM.SaleTotalAmountInLocalCurrency = entityPM.QuoteCharges.Where(d => d.IsAllIN == false).Sum(d => d.SaleTotalAmountLocal);
                entityPM.SaleTotalAmountInSaleCurrency = entityPM.QuoteCharges.Where(d => d.IsAllIN == false).Sum(d => d.SaleAmountInSaleCurrency);
                entityPM.EstimateProfitInSaleCurrency = MethodHelper.Round((entityPM.SaleTotalAmountInSaleCurrency - entityPM.CostTotalAmountInSaleCurrency), 2);
            }
            #endregion

            entityPM.TotalReceivablesAmount = 0;
            foreach (QuoteChargePM receviable in entityPM.QuoteCharges)
            {
                if (receviable.SaleTotalAmountLocal != null)
                {
                    entityPM.TotalReceivablesAmount += receviable.SaleTotalAmountLocal;
                }
            }

            #region QuoteCostCharges || QuoteSaleCharges
            foreach (QuoteChargePM item in entityPM.QuoteCharges)
            {
                bool isCostChargeAddable = true;

                if (entityPM.TransportModeId.ToUpper() == "A"
                    ||
                    (entityPM.TransportModeId.ToUpper() == "O" && entityPM.ShipmentTypeId.ToUpper() == "LCLD")
                    ||
                    (entityPM.TransportModeId.ToUpper() == "I" && entityPM.ShipmentTypeId.ToUpper() == "LTL")
                    )
                {
                    if (item.CostUnitPrice == null)
                    {
                        isCostChargeAddable = false;
                    }
                }
                else
                {
                    if (item.CostUnitPrice == null
                        && item.CostContainerType1UnitPrice == null
                        && item.CostContainerType2UnitPrice == null
                        && item.CostContainerType3UnitPrice == null
                        && item.CostContainerType4UnitPrice == null
                        && item.CostContainerType5UnitPrice == null
                        )
                    {
                        isCostChargeAddable = false;
                    }
                }

                if (isCostChargeAddable)
                {
                    QuoteCostChargePM costChargePM = new QuoteCostChargePM()
                    {
                        Id = item.Id,
                        Tenant = item.Tenant,
                        QuoteId = item.QuoteId,
                        ChargesTypeId = item.ChargesTypeId,
                        ChargesTypeCode = item.ChargesTypeCode,
                        ChargesTypeName = item.ChargesTypeName,
                        ChargesGroupCode = item.ChargesGroupCode,
                        CurrencyId = item.CostCurrencyId,
                        CurrencyCode = item.CostCurrencyCode,
                        SaleExchangeRate = item.SaleExchangeRate,
                        MarkUpTypeCode = item.MarkUpTypeCode,
                        MarkUpValue = item.MarkUpValue,
                        Notes = item.Notes,
                        UpdatedByUserId = item.UpdatedByUserId,
                        UpdateDate = item.UpdateDate,
                        ValueDate = item.ValueDate,
                        QuoteTypeCode = item.QuoteTypeCode,
                        ContainerType1MarkUpTypeCode = item.ContainerType1MarkUpTypeCode,
                        ContainerType2MarkUpTypeCode = item.ContainerType2MarkUpTypeCode,
                        ContainerType3MarkUpTypeCode = item.ContainerType3MarkUpTypeCode,
                        ContainerType4MarkUpTypeCode = item.ContainerType4MarkUpTypeCode,
                        ContainerType5MarkUpTypeCode = item.ContainerType5MarkUpTypeCode,
                        ContainerType1MarkUpValue = item.ContainerType1MarkUpValue,
                        ContainerType2MarkUpValue = item.ContainerType2MarkUpValue,
                        ContainerType3MarkUpValue = item.ContainerType3MarkUpValue,
                        ContainerType4MarkUpValue = item.ContainerType4MarkUpValue,
                        ContainerType5MarkUpValue = item.ContainerType5MarkUpValue,
                        CostMeasurementId = item.CostMeasurementId,
                        CostMeasurementCode = item.CostMeasurementCode,
                        CostMeasurementShortName = item.CostMeasurementShortName,
                        CostQuantity = item.CostQuantity,
                        CostUnitPrice = item.CostUnitPrice,
                        CostTotalAmount = item.CostTotalAmount,
                        CostTotalAmountLocal = item.CostTotalAmountLocal,
                        CostContainerType1UnitPrice = item.CostContainerType1UnitPrice,
                        CostContainerType2UnitPrice = item.CostContainerType2UnitPrice,
                        CostContainerType3UnitPrice = item.CostContainerType3UnitPrice,
                        CostContainerType4UnitPrice = item.CostContainerType4UnitPrice,
                        CostContainerType5UnitPrice = item.CostContainerType5UnitPrice,
                        VatTypeId = item.VatTypeId,
                        VatPercentage = item.VatPercentage,
                        VatTypeName = item.VatTypeName,
                        UOMPercentage = item.CostMeasurementCode == "PRVL" || item.CostMeasurementCode == "PRFR" ? "%" : "",
                        CostMinAmount = item.CostMinAmount,
                        CostMaxAmount = item.CostMaxAmount,
                        SaleMinAmount = item.SaleMinAmount,
                        SaleMaxAmount = item.SaleMaxAmount,
                    };

                    entityPM.QuoteCostCharges.Add(costChargePM);
                }

                bool isSaleChargeAddable = !(item.IsAllIN);

                if (entityPM.TransportModeId.ToUpper() == "A"
                    ||
                    (entityPM.TransportModeId.ToUpper() == "O" && entityPM.ShipmentTypeId.ToUpper() == "LCLD")
                    ||
                    (entityPM.TransportModeId.ToUpper() == "I" && entityPM.ShipmentTypeId.ToUpper() == "LTL")
                    )
                {
                    if (item.IsChargeBySteps)
                    {
                        if (item.QuoteChargePriceSteps.Count == 0)
                        {
                            isSaleChargeAddable = false;
                        }
                    }

                    else
                    {
                        if (item.SaleUnitPrice == null)
                        {
                            isSaleChargeAddable = false;
                        }
                    }
                }

                else
                {
                    if (item.IsChargeBySteps)
                    {
                        if (item.QuoteChargePriceSteps.Count == 0)
                        {
                            isSaleChargeAddable = false;
                        }
                    }

                    else
                    {
                        if (item.SaleUnitPrice == null
                            && item.SaleContainerType1UnitPrice == null
                            && item.SaleContainerType2UnitPrice == null
                            && item.SaleContainerType3UnitPrice == null
                            && item.SaleContainerType4UnitPrice == null
                            && item.SaleContainerType5UnitPrice == null
                            )
                        {
                            isSaleChargeAddable = false;
                        }
                    }
                }

                if (isSaleChargeAddable)
                {
                    QuoteSaleChargePM saleChargePM = new QuoteSaleChargePM()
                    {
                        Id = item.Id,
                        Tenant = item.Tenant,
                        QuoteId = item.QuoteId,
                        ChargesTypeId = item.ChargesTypeId,
                        ChargesTypeCode = item.ChargesTypeCode,
                        ChargesTypeName = item.ChargesTypeName,
                        ChargesTypeDescription = item.ChargesTypeDescription,
                        ChargesGroupCode = item.ChargesGroupCode,
                        CurrencyId = item.SaleCurrencyId,
                        CurrencyCode = item.SaleCurrencyCode,
                        SaleExchangeRate = item.SaleExchangeRate,
                        MarkUpTypeCode = item.MarkUpTypeCode,
                        MarkUpValue = item.MarkUpValue,
                        ChargesTypeLocalName = item.ChargesTypeLocalName,
                        Notes = item.Notes,
                        UpdatedByUserId = item.UpdatedByUserId,
                        UpdateDate = item.UpdateDate,
                        ValueDate = item.ValueDate,
                        IsAllIN = item.IsAllIN ? "True" : "False",
                        QuoteTypeCode = item.QuoteTypeCode,
                        ContainerType1MarkUpTypeCode = item.ContainerType1MarkUpTypeCode,
                        ContainerType2MarkUpTypeCode = item.ContainerType2MarkUpTypeCode,
                        ContainerType3MarkUpTypeCode = item.ContainerType3MarkUpTypeCode,
                        ContainerType4MarkUpTypeCode = item.ContainerType4MarkUpTypeCode,
                        ContainerType5MarkUpTypeCode = item.ContainerType5MarkUpTypeCode,
                        ContainerType1MarkUpValue = item.ContainerType1MarkUpValue,
                        ContainerType2MarkUpValue = item.ContainerType2MarkUpValue,
                        ContainerType3MarkUpValue = item.ContainerType3MarkUpValue,
                        ContainerType4MarkUpValue = item.ContainerType4MarkUpValue,
                        ContainerType5MarkUpValue = item.ContainerType5MarkUpValue,
                        SaleMeasurementId = item.SaleMeasurementId,
                        SaleMeasurementCode = item.SaleMeasurementCode,
                        SaleMeasurementShortName = item.SaleMeasurementShortName,
                        SaleMeasurementLocalName = item.SaleMeasurementLocalName,
                        SaleQuantity = item.SaleQuantity,
                        SaleUnitPrice = item.SaleUnitPrice,
                        SaleTotalAmount = item.SaleTotalAmount,
                        SaleTotalAmountLocal = item.SaleTotalAmountLocal,
                        SaleContainerType1UnitPrice = item.SaleContainerType1UnitPrice,
                        SaleContainerType2UnitPrice = item.SaleContainerType2UnitPrice,
                        SaleContainerType3UnitPrice = item.SaleContainerType3UnitPrice,
                        SaleContainerType4UnitPrice = item.SaleContainerType4UnitPrice,
                        SaleContainerType5UnitPrice = item.SaleContainerType5UnitPrice,
                        VatTypeId = item.VatTypeId,
                        VatPercentage = item.VatPercentage,
                        VatTypeName = item.VatTypeName,
                        VatAmount = item.VatAmount,
                        UOMPercentage = item.SaleMeasurementCode == "PRVL" || item.SaleMeasurementCode == "PRFR" ? "%" : "",
                        SaleUnitPriceInSaleCurrency = item.SaleUnitPriceInSaleCurrency,
                        SaleUnitPrice1InSaleCurrency = item.SaleUnitPrice1InSaleCurrency,
                        SaleUnitPrice2InSaleCurrency = item.SaleUnitPrice2InSaleCurrency,
                        SaleUnitPrice3InSaleCurrency = item.SaleUnitPrice3InSaleCurrency,
                        SaleUnitPrice4InSaleCurrency = item.SaleUnitPrice4InSaleCurrency,
                        SaleUnitPrice5InSaleCurrency = item.SaleUnitPrice5InSaleCurrency,
                        SaleAmountInSaleCurrency = item.SaleAmountInSaleCurrency,
                        CostMinAmount = item.CostMinAmount,
                        CostMaxAmount = item.CostMaxAmount,
                        SaleMinAmount = item.SaleMinAmount,
                        SaleMaxAmount = item.SaleMaxAmount,
                    };

                    if (item.IsChargeBySteps)
                    {
                        if (item.QuoteChargePriceSteps.Count > 0)
                        {
                            string myPriceBreaks = "";

                            foreach (QuotePriceStepsPM itemStep in item.QuoteChargePriceSteps)
                            {
                                string formattedValue = "";
                                if (itemStep.SaleUnitPrice != null)
                                {
                                    formattedValue = itemStep.SaleUnitPrice.Value.ToString();
                                    if (formattedValue.Contains("."))
                                    {
                                        string[] digits = formattedValue.Split('.');
                                        if (digits[1] == "0" || digits[1] == "00")
                                        {
                                            formattedValue = digits[0];
                                        }

                                        if (string.IsNullOrEmpty(formattedValue))
                                        {
                                            formattedValue = itemStep.SaleUnitPrice.Value.ToString("#,##0." + new string('0', 2));
                                        }
                                    }
                                }

                                if (string.IsNullOrEmpty(myPriceBreaks))
                                {
                                    myPriceBreaks += "+" + itemStep.Step + " kg: " + formattedValue;
                                }

                                else
                                {
                                    myPriceBreaks += "\r";//Environment.NewLine;
                                    myPriceBreaks += "+" + itemStep.Step + " kg: " + formattedValue;
                                }
                            }

                            saleChargePM.PriceBreaks = myPriceBreaks;
                        }
                    }

                    entityPM.QuoteSaleCharges.Add(saleChargePM);
                }
            }
            #endregion

            #region Quote Sales Totals
            if (entityPM.IsSaleCurrencySameAsCost)
            {
                entityPM.QuoteSalesTotals = (from d in entityPM.QuoteCharges
                                             group d by d.SaleCurrencyCode into g
                                             select new QuoteSalesTotalPM()
                                             {
                                                 CurrencyCode = g.Key,
                                                 Amount = g.Sum(s=>s.SaleTotalAmount),
                                             }).ToList();
            }

            else
            {
                entityPM.QuoteSalesTotals.Add(new QuoteSalesTotalPM() { CurrencyCode = entityPM.SaleCurrencyCode, Amount = entityPM.SaleTotalAmountInSaleCurrency });
            }

            string mySalesTotalAmounts = "";
            foreach (QuoteSalesTotalPM item in entityPM.QuoteSalesTotals)
            {
                double? myAmountField = 0;
                if (item.Amount != null)
                {
                    myAmountField = item.Amount;
                }

                string myLineText = String.Format("{0:#,0.00}", myAmountField) + " " + item.CurrencyCode;
                if (string.IsNullOrEmpty(mySalesTotalAmounts))
                {
                    mySalesTotalAmounts = myLineText;
                }

                else
                {
                    mySalesTotalAmounts += Environment.NewLine;
                    mySalesTotalAmounts += myLineText;
                }
            }

            entityPM.SalesTotalAmounts = mySalesTotalAmounts;
            #endregion

            #region TotalVATPerQuote
            if (entityPM.IsChargesByVAT)
            {
                if (entityPM.QuoteCharges.Count > 0)
                {
                    entityPM.TotalVATPerQuote = (from item in myQuotesContext.QuoteTotalVATs.Include("VatType")
                                                 where item.Tenant == tenant && item.QuoteId == entityPM.Id
                                                 select new QuoteVATsTotalPM()
                                                 {
                                                     Id = item.Id,
                                                     VatTypeId = item.VatTypeId,
                                                     VatTypeName = item.VatType == null ? "" : item.VatType.EnglishName,
                                                     VatPercentage = item.VatPercent,
                                                     AmountInSaleCurrency = item.QuoteCurrencyVATAmount,
                                                     AmountInLocalCurrency = item.LocalCurrencyVATAmount,
                                                 }).ToList();
                }
            }
            #endregion

            string totalContainers = "";
            string container = "";
            string containerTypeCode = "";
            PackageType packageType;
            if (entityPM.PackageType1Id != null)
            {
                packageType = PackageTypeRepository.GetSinglePackageType(entityPM.PackageType1Id, tenant, true);

                containerTypeCode = packageType.Code;

                if (containerTypeCode.Length > 2)
                {
                    containerTypeCode = packageType.Code.Insert(2, "'");
                }
                
                container = entityPM.PackageType1Quantity + " x " + containerTypeCode;
                totalContainers = totalContainers + container;
            }

            if (entityPM.PackageType2Id != null)
            {
                packageType = PackageTypeRepository.GetSinglePackageType(entityPM.PackageType2Id, tenant, true);

                containerTypeCode = packageType.Code;

                if (containerTypeCode.Length > 2)
                {
                    containerTypeCode = packageType.Code.Insert(2, "'");
                }

                container = entityPM.PackageType2Quantity + " x " + containerTypeCode;
                totalContainers = totalContainers + ", " + container;
            }

            if (entityPM.PackageType3Id != null)
            {
                packageType = PackageTypeRepository.GetSinglePackageType(entityPM.PackageType3Id, tenant, true);

                containerTypeCode = packageType.Code;

                if (containerTypeCode.Length > 2)
                {
                    containerTypeCode = packageType.Code.Insert(2, "'");
                }

                container = entityPM.PackageType3Quantity + " x " + containerTypeCode;
                totalContainers = totalContainers + ", " + container;
            }

            if (entityPM.PackageType4Id != null)
            {
                packageType = PackageTypeRepository.GetSinglePackageType(entityPM.PackageType4Id, tenant, true);

                containerTypeCode = packageType.Code;

                if (containerTypeCode.Length > 2)
                {
                    containerTypeCode = packageType.Code.Insert(2, "'");
                }

                container = entityPM.PackageType4Quantity + " x " + containerTypeCode;
                totalContainers = totalContainers + ", " + container;
            }

            if (entityPM.PackageType5Id != null)
            {
                packageType = PackageTypeRepository.GetSinglePackageType(entityPM.PackageType5Id, tenant, true);

                containerTypeCode = packageType.Code;

                if (containerTypeCode.Length > 2)
                {
                    containerTypeCode = packageType.Code.Insert(2, "'");
                }

                container = entityPM.PackageType5Quantity + " x " + containerTypeCode;
                totalContainers = totalContainers + ", " + container;
            }

            entityPM.TotalContainers = totalContainers;

            #region DocumentVersions

            QuoteDocumentVersionRepository rep = new QuoteDocumentVersionRepository(tenant);
            QuoteDocumentVersionQuery quoteDocumentVersionQuery = new QuoteDocumentVersionQuery(rep);
            entityPM.QuoteDocumentVersions = quoteDocumentVersionQuery.GetQuoteDocumentVersionPMsByQuoteId(entityId, tenant).ToList();

            #endregion

            if (!string.IsNullOrEmpty(entityPM.PickUpAddressId))
            {
                entityPM.ETDLabel = "ETD Origin City";
            }
            else
            {
                entityPM.ETDLabel = "ETD Port of Loading";
            }

            if (!string.IsNullOrEmpty(entityPM.DeliveryAddressId))
            {
                entityPM.ETALabel = "ETA Delivery City";
            }
            else
            {
                entityPM.ETALabel = "ETA Port of Destination";
            }

            double? mySaleAmountLocal = entityPM.QuoteCharges.Where(d => d.IsAllIN == false).Sum(s => s.SaleTotalAmountLocal);
            double? myTotalVATLocal = entityPM.TotalVATs.Sum(s => s.LocalCurrencyVATAmount);
            double? mySaleAmount = 0;
            double? myTotalVAT = entityPM.TotalVATs.Sum(s => s.QuoteCurrencyVATAmount);

            mySaleAmountLocal = MethodHelper.Round(mySaleAmountLocal, 2);
            mySaleAmount = MethodHelper.Round(mySaleAmountLocal / entityPM.ExchangeRate, 2);
            myTotalVATLocal = MethodHelper.Round(myTotalVATLocal, 2);
            myTotalVAT = MethodHelper.Round(myTotalVAT, 2);

            entityPM.TotalSaleIncludingVATAmountInSaleCurrency = mySaleAmount + myTotalVAT;
            entityPM.TotalSaleIncludingVATAmountInLocalCurrency = mySaleAmountLocal + myTotalVATLocal;

            return entityPM;
        }

        private string GetAddress(Address address)
        {
            string resultAddress = "";

            if (address != null)
            {
                resultAddress = address.Address1 != null ? address.Address1 : "";

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.Address2;
                }

                if (!string.IsNullOrEmpty(address.City))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.City;
                }

                if (address.State != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + " " + (address.State.LocalName != null ? address.State.LocalName : "");
                    }

                    else
                    {
                        resultAddress = resultAddress + " " + (address.State.EnglishName != null ? address.State.EnglishName : "");
                    }
                }

                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    resultAddress = resultAddress + " " + address.ZipCode;
                }

                if (address.Country != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.LocalName;
                    }

                    else
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.EnglishName;
                    }
                }
            }

            return resultAddress;
        }



    }
}