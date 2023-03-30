	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.CustomFilters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.CRM.Data.BusinessUnitFilters;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class OpportunityListQueryService
    {
	    private IQueryable<OpportunityList> GetIqueryableList(IQueryable<Opportunity> iQueryable)
        {
            if (iQueryable.Count() > 0)
            {
                int tenant = iQueryable.First().Tenant;

                OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
                iQueryable = filter.RunFilter(iQueryable);
            }

            IQueryable<OpportunityList> query = (from a in iQueryable.Include("Customer").Include("LeadPartner").Include("Rating").Include("Stage").Include("Owner").Include("NextActivityType").Include("ActivityType").Include("BusinessUnit").Include("Contact").Include("OpportunityClosingReason").Include("LeadSource").Include("OpportunityType").Include("LastStage").Include("LeadUser")
                                                 select new OpportunityList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     Subject = a.Subject,
                                                     CustomerId = a.CustomerId,
                                                     LeadSourceId = a.LeadSourceId,
                                                     ContactId = a.ContactId,
                                                     EstimatedClosingDate = a.EstimatedClosingDate,
                                                     StageId = a.StageId,
                                                     Probability = a.Probability,
                                                     ValueField = a.ValueField == null ? 0 : a.ValueField,
                                                     CreateDate = a.CreateDate,
                                                     CreatedByUserId = a.CreatedByUserId,
                                                     UpdateDate = a.UpdateDate,
                                                     UpdatedByUserId = a.UpdatedByUserId,
                                                     RatingCode = a.RatingCode,
                                                     IsClosed = a.IsClosed,
                                                     ActualClosingDate = a.ActualClosingDate,
                                                     ClosingDescription = a.ClosingDescription,
                                                     SearchFields = a.SearchFields,
                                                     CustomerName = a.Customer == null ? "" : a.Customer.EnglishName,
                                                     StageName = a.Stage == null ? "" : a.Stage.Name,
                                                     RatingName = a.Rating == null ? "" : a.Rating.Name,
                                                     OpportunityTypeId = a.OpportunityType == null ? "" : a.OpportunityType.Id,
                                                     OwnerName = a.Owner == null ? "" : a.Owner.Contact.EnglishName,
                                                     NumberOfShipments = a.NumberOfShipments,
                                                     LastStageDate = a.LastStageDate,
                                                     LastCompletedActivityDate = a.LastCompletedActivityDate,
                                                     LastCompletedActivityTypeCode = a.LastCompletedActivityTypeCode,
                                                     LastCompletedActivityTypeName = a.ActivityType != null ? a.ActivityType.Name : null,
                                                     LastActivitySubject = a.LastActivitySubject,
                                                     NextActivityDate = a.NextActivityDate,
                                                     NextActivityTypeCode = a.NextActivityTypeCode,
                                                     NextActivitySubject = a.NextActivitySubject,
                                                     NextActivityTypeName = a.NextActivityType != null ? a.NextActivityType.Name : null,
                                                     CountryName = a.Customer != null ? a.Customer.CountryName : null,
                                                     IndustryName = a.Customer != null ? (a.Customer.Customer != null ? (a.Customer.Customer.Industry != null ? a.Customer.Customer.Industry.Name : null) : null) : null,
                                                     Field1 = a.Field1,
                                                     Field2 = a.Field2,
                                                     Field3 = a.Field3,
                                                     Field4 = a.Field4,
                                                     Field5 = a.Field5,
                                                     Field6 = a.Field6,
                                                     Field7 = a.Field7,
                                                     Field8 = a.Field8,
                                                     Field9 = a.Field9,
                                                     Field10 = a.Field10,
                                                     Field11 = a.Field11,
                                                     Field12 = a.Field12,
                                                     Field13 = a.Field13,
                                                     Field14 = a.Field14,
                                                     Field15 = a.Field15,
                                                     Field16 = a.Field16,
                                                     Field17 = a.Field17,
                                                     Field18 = a.Field18,
                                                     Field19 = a.Field19,
                                                     Field20 = a.Field20,
                                                     Field21 = a.Field21,
                                                     Field22 = a.Field22,
                                                     Field23 = a.Field23,
                                                     Field24 = a.Field24,
                                                     Field25 = a.Field25,
                                                     Field26 = a.Field26,
                                                     Field27 = a.Field27,
                                                     Field28 = a.Field28,
                                                     Field29 = a.Field29,
                                                     Field30 = a.Field30,
                                                     Field31 = a.Field31,
                                                     Field32 = a.Field32,
                                                     Field33 = a.Field33,
                                                     Field34 = a.Field34,
                                                     Field35 = a.Field35,
                                                     Field36 = a.Field36,
                                                     Field37 = a.Field37,
                                                     Field38 = a.Field38,
                                                     Field39 = a.Field39,
                                                     Field40 = a.Field40,
                                                     StageDueDate = a.StageDueDate,
                                                     LeadDescription = a.LeadDescription,
                                                     Notes = a.Notes,
                                                     BusinessUnitId = a.BusinessUnitId,
                                                     BusinessUnitName = a.BusinessUnit == null ? null : a.BusinessUnit.Name,
                                                     StageProbability = a.Stage == null ? 0 : a.Stage.Probability,
                                                     RatingIndexOrder = a.Rating == null ? 0 : a.Rating.IndexOrder,
                                                     LeadUserId = a.LeadUserId,
                                                     LeadPartnerId = a.LeadPartnerId,
                                                     LeadPartnerName = a.LeadPartner == null ? "" : a.LeadPartner.EnglishName,
                                                     AgentId = a.AgentId,
                                                     ForeignClientId = a.ForeignClientId,
                                                     ContactName = a.Contact != null ? a.Contact.EnglishName : null,
                                                     ContactPhone = a.Contact != null ? a.Contact.BusinessPhone : null,
                                                     OwnerId = a.OwnerId,
                                                     ClosingReasonId = a.ClosingReasonId,
                                                     ClosingReasonName = a.OpportunityClosingReason == null ? null : a.OpportunityClosingReason.Name,
                                                     LeadSourceName = a.LeadSource == null ? null : a.LeadSource.Name,
                                                     ContactEmail = a.Contact != null ? a.Contact.Email : null,
                                                     ActivityWatch = a.Customer == null ? false : (a.Customer.Customer == null ? false : a.Customer.Customer.ActivityWatch),
                                                     OpportunityTypeName = a.OpportunityType == null ? null : a.OpportunityType.Name,
                                                     NumberOfShipmentsForeground = a.NumberOfShipments > 0 ? "#FF282E30" : "#FFE53030",
                                                     IsCancelled = a.IsCancelled,
                                                     NumberOfConnectedQuotes = a.NumberOfConnectedQuotes,
                                                     LastStageIdBeforeClosure = a.LastStageIdBeforeClosure,
                                                     LastStageBeforeClosureName = a.LastStage == null ? "" : a.LastStage.Name,
                                                     UserName = a.LeadUser == null ? null : a.LeadUser.Contact.EnglishName,
                                                     ClientId = a.ClientId,
                                                     LeadOrigin = a.LeadOrigin,
                                                     Campaign = a.Campaign,
                                                 });
            return query;
		}

        public List<OpportunityList> GetRecentEntityLists(string ownerId, string businessUnitId, int tenant, string userId, string objectTableId)
        {
            List<OpportunityList> entityList = new List<OpportunityList>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            OpportunityRepository entityRepository = new OpportunityRepository(tenant);
            IQueryable<Opportunity> entities = entityRepository.GetAllFromIdList(ids, tenant);

            OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
            entities = filter.RunFilter(entities);

            if (!string.IsNullOrEmpty(ownerId))
            {
                entities = entities.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                entities = entities.Where(d => d.BusinessUnitId == businessUnitId);
            }

            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                Opportunity a = (from d in entities.Include("Customer").Include("LeadPartner").Include("Rating").Include("Stage").Include("BusinessUnit").Include("Contact").Include("OpportunityClosingReason").Include("LeadSource").Include("OpportunityType")
                                       where d.Id == lastActivity.EntityId
                                       select d).FirstOrDefault();

                if (a != null)
                {
                    OpportunityList list = new OpportunityList()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Subject = a.Subject,
                        CustomerId = a.CustomerId,
                        LeadSourceId = a.LeadSourceId,
                        ContactId = a.ContactId,
                        EstimatedClosingDate = a.EstimatedClosingDate,
                        StageId = a.StageId,
                        Probability = a.Probability,
                        ValueField = a.ValueField == null ? 0 : a.ValueField,
                        CreateDate = a.CreateDate,
                        CreatedByUserId = a.CreatedByUserId,
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserId = a.UpdatedByUserId,
                        RatingCode = a.RatingCode,
                        IsClosed = a.IsClosed,
                        ActualClosingDate = a.ActualClosingDate,
                        ClosingDescription = a.ClosingDescription,
                        SearchFields = a.SearchFields,
                        CustomerName = a.Customer == null ? "" : a.Customer.EnglishName,
                        StageName = a.Stage == null ? "" : a.Stage.Name,
                        RatingName = a.Rating == null ? "" : a.Rating.Name,
                        LastActivityDate = lastActivity.ActivityDate,
                        LastActivityTypeName = lastActivity.ActivityType.Name,
                        LastActivityByUserName = lastActivity.User.Contact.EnglishName,
                        OpportunityTypeId = a.OpportunityType == null ? "" : a.OpportunityType.Id,
                        NumberOfShipments = a.NumberOfShipments,                       
                        Field1 = a.Field1,
                        Field2 = a.Field2,
                        Field3 = a.Field3,
                        Field4 = a.Field4,
                        Field5 = a.Field5,
                        Field6 = a.Field6,
                        Field7 = a.Field7,
                        Field8 = a.Field8,
                        Field9 = a.Field9,
                        Field10 = a.Field10,
                        Field11 = a.Field11,
                        Field12 = a.Field12,
                        Field13 = a.Field13,
                        Field14 = a.Field14,
                        Field15 = a.Field15,
                        Field16 = a.Field16,
                        Field17 = a.Field17,
                        Field18 = a.Field18,
                        Field19 = a.Field19,
                        Field20 = a.Field20,
                        Field21 = a.Field21,
                        Field22 = a.Field22,
                        Field23 = a.Field23,
                        Field24 = a.Field24,
                        Field25 = a.Field25,
                        Field26 = a.Field26,
                        Field27 = a.Field27,
                        Field28 = a.Field28,
                        Field29 = a.Field29,
                        Field30 = a.Field30,
                        Field31 = a.Field31,
                        Field32 = a.Field32,
                        Field33 = a.Field33,
                        Field34 = a.Field34,
                        Field35 = a.Field35,
                        Field36 = a.Field36,
                        Field37 = a.Field37,
                        Field38 = a.Field38,
                        Field39 = a.Field39,
                        Field40 = a.Field40,
                        StageDueDate = a.StageDueDate,
                        LeadDescription = a.LeadDescription,
                        Notes = a.Notes,
                        BusinessUnitId = a.BusinessUnitId,
                        BusinessUnitName = a.BusinessUnit == null ? null : a.BusinessUnit.Name,
                        StageProbability = a.Stage == null ? 0 : a.Stage.Probability,
                        RatingIndexOrder = a.Rating == null ? 0 : a.Rating.IndexOrder,
                        LeadUserId = a.LeadUserId,
                        LeadPartnerId = a.LeadPartnerId,
                        LeadPartnerName = a.LeadPartner == null ? "" : a.LeadPartner.EnglishName,
                        AgentId = a.AgentId,
                        ForeignClientId = a.ForeignClientId,
                        ContactName = a.Contact != null ? a.Contact.EnglishName : null,
                        ContactPhone = a.Contact != null ? a.Contact.BusinessPhone : null,
                        OwnerId = a.OwnerId,
                        LastStageDate = a.LastStageDate,
                        ClosingReasonId = a.ClosingReasonId,
                        ClosingReasonName = a.OpportunityClosingReason == null ? null : a.OpportunityClosingReason.Name,
                        LeadSourceName = a.LeadSource == null ? null : a.LeadSource.Name,
                        ContactEmail = a.Contact != null ? a.Contact.Email : null,
                        ActivityWatch = a.Customer == null ? false : (a.Customer.Customer == null ? false : a.Customer.Customer.ActivityWatch),
                        OpportunityTypeName = a.OpportunityType == null ? null : a.OpportunityType.Name,
                        NumberOfShipmentsForeground = a.NumberOfShipments > 0 ? "#FF282E30" : "#FFE53030",
                        IsCancelled = a.IsCancelled,
                        ClientId = a.ClientId,
                        LeadOrigin = a.LeadOrigin,
                        Campaign = a.Campaign,
                    };

                    ContactRepository rep = new ContactRepository(tenant);
                    Contact contact = rep.GetSingleContact(a.OwnerId, tenant);
                    if (contact != null)
                    {
                        list.OwnerName = contact.EnglishName;
                    }
                    else
                    {
                        list.OwnerName = "";
                    }

                    entityList.Add(list);
                }
            }

            return entityList;
        }

        public List<OpportunityList> GetOpportunitiesByCustomerId(string customerId, int tenant)
        {
            IQueryable<OpportunityList> query = (from a in context.Opportunities.Include("Customer").Include("LeadPartner").Include("Rating").Include("Stage").Include("BusinessUnit").Include("Contact").Include("OpportunityClosingReason").Include("LeadSource").Include("OpportunityType")
                                                 where a.Tenant == tenant && a.CustomerId == customerId
                                                 select new OpportunityList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     Subject = a.Subject,
                                                     CustomerId = a.CustomerId,
                                                     LeadSourceId = a.LeadSourceId,
                                                     ContactId = a.ContactId,
                                                     EstimatedClosingDate = a.EstimatedClosingDate,
                                                     StageId = a.StageId,
                                                     Probability = a.Probability,
                                                     ValueField = a.ValueField,
                                                     CreateDate = a.CreateDate,
                                                     CreatedByUserId = a.CreatedByUserId,
                                                     UpdateDate = a.UpdateDate,
                                                     UpdatedByUserId = a.UpdatedByUserId,
                                                     RatingCode = a.RatingCode,
                                                     IsClosed = a.IsClosed,
                                                     ActualClosingDate = a.ActualClosingDate,
                                                     ClosingDescription = a.ClosingDescription,
                                                     SearchFields = a.SearchFields,
                                                     CustomerName = a.Customer == null ? "" : a.Customer.EnglishName,
                                                     StageName = a.Stage == null ? "" : a.Stage.Name,
                                                     RatingName = a.Rating == null ? "" : a.Rating.Name,
                                                     OpportunityTypeId = a.OpportunityType == null ? "" : a.OpportunityType.Id,
                                                     NumberOfShipments = a.NumberOfShipments,                                                     
                                                     OwnerName = a.Owner == null ? "" : a.Owner.Contact.EnglishName,
                                                     Field1 = a.Field1,
                                                     Field2 = a.Field2,
                                                     Field3 = a.Field3,
                                                     Field4 = a.Field4,
                                                     Field5 = a.Field5,
                                                     Field6 = a.Field6,
                                                     Field7 = a.Field7,
                                                     Field8 = a.Field8,
                                                     Field9 = a.Field9,
                                                     Field10 = a.Field10,
                                                     Field11 = a.Field11,
                                                     Field12 = a.Field12,
                                                     Field13 = a.Field13,
                                                     Field14 = a.Field14,
                                                     Field15 = a.Field15,
                                                     Field16 = a.Field16,
                                                     Field17 = a.Field17,
                                                     Field18 = a.Field18,
                                                     Field19 = a.Field19,
                                                     Field20 = a.Field20,
                                                     Field21 = a.Field21,
                                                     Field22 = a.Field22,
                                                     Field23 = a.Field23,
                                                     Field24 = a.Field24,
                                                     Field25 = a.Field25,
                                                     Field26 = a.Field26,
                                                     Field27 = a.Field27,
                                                     Field28 = a.Field28,
                                                     Field29 = a.Field29,
                                                     Field30 = a.Field30,
                                                     Field31 = a.Field31,
                                                     Field32 = a.Field32,
                                                     Field33 = a.Field33,
                                                     Field34 = a.Field34,
                                                     Field35 = a.Field35,
                                                     Field36 = a.Field36,
                                                     Field37 = a.Field37,
                                                     Field38 = a.Field38,
                                                     Field39 = a.Field39,
                                                     Field40 = a.Field40,
                                                     StageDueDate = a.StageDueDate,
                                                     LeadDescription = a.LeadDescription,
                                                     Notes = a.Notes,
                                                     BusinessUnitId = a.BusinessUnitId,
                                                     BusinessUnitName = a.BusinessUnit == null ? null : a.BusinessUnit.Name,
                                                     StageProbability = a.Stage == null ? 0 : a.Stage.Probability,
                                                     RatingIndexOrder = a.Rating == null ? 0 : a.Rating.IndexOrder,
                                                     LeadUserId = a.LeadUserId,
                                                     LeadPartnerId = a.LeadPartnerId,
                                                     LeadPartnerName = a.LeadPartner == null ? "" : a.LeadPartner.EnglishName,
                                                     AgentId = a.AgentId,
                                                     ForeignClientId = a.ForeignClientId,
                                                     ContactName = a.Contact != null ? a.Contact.EnglishName : null,
                                                     ContactPhone = a.Contact != null ? a.Contact.BusinessPhone : null,
                                                     OwnerId = a.OwnerId,
                                                     ClosingReasonId = a.ClosingReasonId,
                                                     ClosingReasonName = a.OpportunityClosingReason == null ? null : a.OpportunityClosingReason.Name,
                                                     LeadSourceName = a.LeadSource == null ? null : a.LeadSource.Name,
                                                     ContactEmail = a.Contact != null ? a.Contact.Email : null,
                                                     ActivityWatch = a.Customer == null ? false : (a.Customer.Customer == null ? false : a.Customer.Customer.ActivityWatch),
                                                     OpportunityTypeName = a.OpportunityType == null ? null : a.OpportunityType.Name,
                                                     NumberOfShipmentsForeground = a.NumberOfShipments > 0 ? "#FF282E30" : "#FFE53030",
                                                     IsCancelled = a.IsCancelled,
                                                     ClientId = a.ClientId,
                                                     LeadOrigin = a.LeadOrigin,
                                                     Campaign = a.Campaign,
                                                 });

            OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
            query = filter.RunFilter(query);

            return query.ToList();
        }

        private IQueryable<Opportunity> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Opportunity> iQueryable,int tenant)
        {
            return OpportunityCustomFilter.GetFilteredQuery(queryOperations, iQueryable, tenant);
        }

        public IQueryable<OpportunityList> GetOpportunitiesByTenant(int tenant)
        {
            IQueryable<OpportunityList> query = (from a in context.Opportunities.Include("Customer").Include("LeadPartner").Include("Rating").Include("Stage").Include("LastStage").Include("BusinessUnit").Include("Contact").Include("OpportunityClosingReason").Include("LeadSource").Include("OpportunityType")
                                                 where a.Tenant == tenant
                                                 select new OpportunityList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     Subject = a.Subject,
                                                     CustomerId = a.CustomerId,
                                                     CustomerName = a.Customer == null ? "" : a.Customer.EnglishName,
                                                     StageId = a.StageId,
                                                     StageName = a.Stage == null ? "" : a.Stage.Name, 
                                                     CreateDate = a.CreateDate,
                                                     UpdateDate = a.UpdateDate,
                                                     IsClosed = a.IsClosed,
                                                     RatingCode = a.RatingCode,                                                     
                                                     RatingName = a.Rating == null ? "" : a.Rating.Name,
                                                     OpportunityTypeId = a.OpportunityType == null ? "" : a.OpportunityType.Id,
                                                     OwnerName = a.Owner == null ? "" : a.Owner.Contact.EnglishName,
                                                     LastStageDate = a.LastStageDate,
                                                     LastStageIdBeforeClosure = a.LastStageIdBeforeClosure,
                                                     LastStageName = a.LastStage != null ? a.LastStage.Name : null,
                                                     CountryName = a.Customer != null ? a.Customer.CountryName : null,                                                     
                                                     Field1 = a.Field1,
                                                     Field2 = a.Field2,
                                                     Field3 = a.Field3,
                                                     Field4 = a.Field4,
                                                     Field5 = a.Field5,
                                                     Field6 = a.Field6,
                                                     Field7 = a.Field7,
                                                     Field8 = a.Field8,
                                                     Field9 = a.Field9,
                                                     Field10 = a.Field10,
                                                     Field11 = a.Field11,
                                                     Field12 = a.Field12,
                                                     Field13 = a.Field13,
                                                     Field14 = a.Field14,
                                                     Field15 = a.Field15,
                                                     Field16 = a.Field16,
                                                     Field17 = a.Field17,
                                                     Field18 = a.Field18,
                                                     Field19 = a.Field19,
                                                     Field20 = a.Field20,
                                                     Field21 = a.Field21,
                                                     Field22 = a.Field22,
                                                     Field23 = a.Field23,
                                                     Field24 = a.Field24,
                                                     Field25 = a.Field25,
                                                     Field26 = a.Field26,
                                                     Field27 = a.Field27,
                                                     Field28 = a.Field28,
                                                     Field29 = a.Field29,
                                                     Field30 = a.Field30,
                                                     Field31 = a.Field31,
                                                     Field32 = a.Field32,
                                                     Field33 = a.Field33,
                                                     Field34 = a.Field34,
                                                     Field35 = a.Field35,
                                                     Field36 = a.Field36,
                                                     Field37 = a.Field37,
                                                     Field38 = a.Field38,
                                                     Field39 = a.Field39,
                                                     Field40 = a.Field40,
                                                     StageDueDate = a.StageDueDate,
                                                     LeadDescription = a.LeadDescription,
                                                     Notes = a.Notes,
                                                     BusinessUnitId = a.BusinessUnitId,
                                                     BusinessUnitName = a.BusinessUnit == null ? null : a.BusinessUnit.Name,
                                                     StageProbability = a.Stage == null ? 0 : a.Stage.Probability,
                                                     RatingIndexOrder = a.Rating == null ? 0 : a.Rating.IndexOrder,
                                                     LeadUserId = a.LeadUserId,
                                                     LeadPartnerId = a.LeadPartnerId,
                                                     LeadPartnerName = a.LeadPartner == null ? "" : a.LeadPartner.EnglishName,
                                                     AgentId = a.AgentId,
                                                     ForeignClientId = a.ForeignClientId,
                                                     ContactName = a.Contact != null ? a.Contact.EnglishName : null,
                                                     ContactPhone = a.Contact != null ? a.Contact.BusinessPhone : null,
                                                     OwnerId = a.OwnerId,
                                                     ClosingReasonId = a.ClosingReasonId,
                                                     ClosingReasonName = a.OpportunityClosingReason == null ? null : a.OpportunityClosingReason.Name,
                                                     LeadSourceName = a.LeadSource == null ? null : a.LeadSource.Name,
                                                     ContactEmail = a.Contact != null ? a.Contact.Email : null,
                                                     ActivityWatch = a.Customer == null ? false : (a.Customer.Customer == null ? false : a.Customer.Customer.ActivityWatch),
                                                     OpportunityTypeName = a.OpportunityType == null ? null : a.OpportunityType.Name,
                                                     NumberOfShipmentsForeground = a.NumberOfShipments > 0 ? "#FF282E30" : "#FFE53030",
                                                     IsCancelled = a.IsCancelled,
                                                     ClientId = a.ClientId,
                                                     LeadOrigin = a.LeadOrigin,
                                                     Campaign = a.Campaign,
                                                 });

            OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
            query = filter.RunFilter(query);

            return query;
        }

        private IQueryable<Opportunity> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Opportunity> iQueryable, int tenant)
        {
            OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
            iQueryable = filter.RunFilter(iQueryable);

            return iQueryable;
        }

        public IQueryable<OpportunityJoinOpportunityStageList> GetOppJoinStageListData(int tenant)
        {
            IQueryable<OpportunityJoinOpportunityStageList> dataList =
                (from opportunity in context.Opportunities.Include("Customer").Include("Rating").Include("Stage").Include("BusinessUnit").Include("Contact")
                 join opportunityStage in context.OpportunityStages
                 on opportunity.Id equals opportunityStage.OpportunityId into JoinedData                 
                 from jd in JoinedData.DefaultIfEmpty()                 
                 where opportunity.Tenant == tenant
                 select new OpportunityJoinOpportunityStageList()
                 {
                     Id = opportunity.Id + (!string.IsNullOrEmpty(jd.Id) ? jd.Id : ""),
                     OpportunityId = opportunity.Id,
                     Probability = opportunity.Probability,
                     CreateDate = opportunity.CreateDate,
                     StageName = opportunity.Stage.Name,
                 });

            return dataList;
        }

        public OpportunityList GetSingleOpportunityByQuote(string id, int tenant)
        {
            OpportunityList query = (from a in context.Opportunities.Include("Owner").Include("Owner.Contact").Include("Stage")
                                            where a.Tenant == tenant && a.Id == id
                                            select new OpportunityList()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,                                                
                                                Subject = a.Subject,
                                                OwnerId = a.OwnerId,
                                                OwnerName = a.Owner == null ? null : (a.Owner.Contact == null ? null : a.Owner.Contact.EnglishName),
                                                StageName = a.Stage == null ? null : a.Stage.Name,
                                                EstimatedClosingDate = a.EstimatedClosingDate,
                                                StageId = a.StageId,                                                
                                                CreateDate = a.CreateDate,                                                
                                                UpdateDate = a.UpdateDate,
                                                UpdatedByUserId = a.UpdatedByUserId,                                                
                                                IsClosed = a.IsClosed,
                                                IsCancelled = a.IsCancelled,                                                                                             
                                                BusinessUnitId = a.BusinessUnitId,
                                                ClientId = a.ClientId,
                                                LeadOrigin = a.LeadOrigin,
                                                Campaign = a.Campaign,
                                            }).FirstOrDefault();
            return query;
        }
	}
}
	