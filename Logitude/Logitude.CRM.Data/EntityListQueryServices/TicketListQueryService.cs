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
using Logitude.CRM.Data.CustomFilters;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.BusinessUnitFilters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class TicketListQueryService
    {
	    private IQueryable<TicketList> GetIqueryableList(IQueryable<Ticket> iQueryable)
        {
            int tenant = 0;
            if (iQueryable != null && iQueryable.Count() > 0)
            {
                tenant = iQueryable.First().Tenant;
            }

            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            IQueryable<TicketList> query = (from a in iQueryable.Include("Owner").Include("Company").Include("Contact").Include("Stage").Include("TicketType").Include("Severity").Include("MainClassification").Include("SecondaryClassification").Include("BusinessUnit").Include("NextActivityType").Include("ActivityType")
                                            select new TicketList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     TicketNumber = a.TicketNumber,
                                                     Subject = a.Subject,
                                                     ContactId = a.ContactId,
                                                     StageId = a.StageId,
                                                     CreateDate = a.CreateDate,
                                                     CreatedByContactId = a.CreatedByContactId,
                                                     UpdateDate = a.UpdateDate,
                                                     UpdatedByUserId = a.UpdatedByUserId,
                                                     CompanyId = a.CompanyId,
                                                     MainClassificationId = a.MainClassificationId,
                                                     SecondaryClassificationId = a.SecondaryClassificationId,
                                                     SeverityId = a.SeverityId,
                                                     TicketTypeId = a.TicketTypeId,
                                                     IsClosed = a.IsClosed,
                                                     IsCancelled = a.IsCancelled,
                                                     SearchFields = a.SearchFields,
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
                                                     OwnerId = a.OwnerId,
                                                     OwnerName = a.Owner == null ? "" : a.Owner.Contact.EnglishName,
                                                     CompanyName = a.Company == null ? "" : a.Company.EnglishName,
                                                     StageName = a.Stage == null ? "" : a.Stage.Name,
                                                     StageCode = a.Stage == null ? "" : a.Stage.Code,
                                                     TypeName = a.TicketType == null ? "" : a.TicketType.Name,
                                                     SeverityName = a.Severity == null ? "" : a.Severity.Name,
                                                     SeverityCode = a.Severity == null ? "" : a.Severity.Code,
                                                     MainClassificationName = a.MainClassification == null ? "" : a.MainClassification.Name,
                                                     SecondaryClassificationName = a.SecondaryClassification == null ? "" : a.SecondaryClassification.Name,
                                                     ContactName = a.Contact != null ? a.Contact.EnglishName : null,
                                                     CCs = a.CCs,
                                                     Bcc = a.Bcc,
                                                     CreatedByContactName = a.CreatedByContact != null ? a.CreatedByContact.EnglishName : null,
                                                     ActivityWatch = a.Company == null ? false : (a.Company.Customer == null ? false : a.Company.Customer.ActivityWatch),
                                                     RankCode = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Code),
                                                     RankId = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Id),
                                                     RankName = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Name),
                                                     SeverityPriority = a.Severity == null ? 0 : a.Severity.Severity,
                                                     BusinessUnitId = a.BusinessUnitId,
                                                     FirstResponseDue = a.FirstResponseDue,
                                                     FirstResponseTime = a.FirstResponseTime,
                                                     FullResolvedTime = a.FullResolvedTime,
                                                     ResolveWithinDue = a.ResolveWithinDue,
                                                     ContactEmail = a.Contact != null ? a.Contact.Email : null,
                                                     OpenEscalation = a.OpenEscalation,
                                                     InternalUsers = a.InternalUsers,
                                                     ClosureDescription = a.ClosureDescription,
                                                     Closewithoutnotifying = a.Closewithoutnotifying,
                                                     LastCompletedActivityDate = a.LastCompletedActivityDate,
                                                     LastCompletedActivityTypeCode = a.LastCompletedActivityTypeCode,
                                                     LastCompletedActivitySubject = a.LastCompletedActivitySubject,
                                                     NextActivityDate = a.NextActivityDate,
                                                     NextActivityTypeCode = a.NextActivityTypeCode,
                                                     NextActivitySubject = a.NextActivitySubject,
                                                     NextActivityTypeName = a.NextActivityType != null ? a.NextActivityType.Name : null,
                                                     LastCompletedActivityTypeName = a.ActivityType != null ? a.ActivityType.Name : null,
                                                     TicketDescription = a.TicketDescription,
                                                     ShipmentId  = a.ShipmentId,
                                                     ShipmentNumber = a.ShipmentNumber,
                                                     UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                                                     FirstResolveDate = a.FirstResolveDate,
                                                     EmployeeGroupId = a.EmployeeGroupId,

                                                     IsResolveDue = (a.FirstResolveDate != null) ? false : true,
                                                     ResolveColor = (a.FirstResolveDate != null) ? "Green" : (a.ResolveWithinDue > todayDateTime ? "Orange" : "Red"),
                                                     IsResolveExamination = (a.FirstResolveDate != null && a.ResolveWithinDue < a.FirstResolveDate) ? true : false,


                                                     IsResponseDue = (a.FirstResponseTime != null) ? false : (true),
                                                     ResponseColor = (a.FirstResponseTime != null) ? "Green" : (a.FirstResponseDue > todayDateTime ? "Orange" : "Red"),
                                                     IsResponseExamination = (a.FirstResponseTime != null && a.FirstResponseDue < a.FirstResponseTime) ? true : false,

                                                     TicketFirstResponseTime = (a.FirstResponseTime != null) ? a.FirstResponseTime : a.FirstResponseDue,
                                                     TicketFirstResolveTime = (a.FirstResolveDate != null) ? a.FirstResolveDate : a.ResolveWithinDue,
                                                     Source =a.Source,
                                                     SourceName = a.TicketSource != null ? a.TicketSource.Name : null,
                                                     CreatedbyType = a.CreatedbyType,
                                                     CreatedbyTypeName = a.TicketCreatedByType != null ? a.TicketCreatedByType.Name : null,
                                                     FirstCloseDate = a.FirstCloseDate,
                                                     LastCloseDate = a.LastCloseDate,
                                                     OpenPeriodMinutes = a.OpenPeriodMinutes,
                                                     OpenDate = a.OpenDate,
                                                     OwnerEmail = a.Owner != null && a.Owner.Contact != null ? a.Owner.Contact.Email : null,
                                                     EmployeeGroupName = a.EmployeeGroup != null && a.EmployeeGroup.Name != null ? a.EmployeeGroup.Name : null,
                                                     QuoteId = a.QuoteId,
                                                     QuoteNumber= a.QuoteNumber,
                                                     SLAId = a.SLAId,
                                                     EntityNumber = a.ShipmentNumber != null ? a.ShipmentNumber: a.QuoteNumber,
                                                     LastCorrespondence = a.LastCorrespondence,
                                            });
            return query;
		}

		private IQueryable<Ticket> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Ticket> iQueryable,int tenant)
        {
            return TicketCustomFilter.GetFilteredQuery(queryOperations, iQueryable, tenant);
		}

		private IQueryable<Ticket> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Ticket> iQueryable,int tenant)
        {
            //TicketBusinessUnitFilter filter = new TicketBusinessUnitFilter(tenant);
            //iQueryable = filter.RunFilter(iQueryable);
			return iQueryable;
		}

        public List<TicketList> GetRecentEntityLists(string ownerId, string employeeGroupId, int tenant, string userId, string objectTableId)
        {
            List<TicketList> entityList = new List<TicketList>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            TicketRepository entityRepository = new TicketRepository(tenant);
            IQueryable<Ticket> entities = entityRepository.GetAllFromIdList(ids, tenant);

            //TicketBusinessUnitFilter filter = new TicketBusinessUnitFilter(tenant);
            //entities = filter.RunFilter(entities);

            if (!string.IsNullOrEmpty(ownerId) && ownerId != "null")
            {
                entities = entities.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(employeeGroupId) && employeeGroupId != "null")
            {
                //entities = entities.Where(d => d.BusinessUnitId == businessUnitId);
            }

            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                Ticket a = (from d in entities.Include("Owner").Include("Company").Include("Contact").Include("Stage").Include("Severity").Include("MainClassification").Include("SecondaryClassification").Include("BusinessUnit").Include("NextActivityType").Include("ActivityType")
                            where d.Id == lastActivity.EntityId
                                 select d).FirstOrDefault();

                if (a != null)
                {
                    TicketList list = new TicketList()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        TicketNumber = a.TicketNumber,
                        Subject = a.Subject,
                        ContactId = a.ContactId,
                        StageId = a.StageId,
                        CreateDate = a.CreateDate,
                        CreatedByContactId = a.CreatedByContactId,
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserId = a.UpdatedByUserId,
                        CompanyId = a.CompanyId,
                        MainClassificationId = a.MainClassificationId,
                        SecondaryClassificationId = a.SecondaryClassificationId,
                        SeverityId = a.SeverityId,
                        TicketTypeId = a.TicketTypeId,
                        IsClosed = a.IsClosed,
                        IsCancelled = a.IsCancelled,
                        SearchFields = a.SearchFields,
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
                        OwnerId = a.OwnerId,
                        LastActivityDate = lastActivity.ActivityDate,
                        LastActivityTypeName = lastActivity.ActivityType.Name,
                        LastActivityByUserName = lastActivity.User.Contact.EnglishName,
                        CompanyName = a.Company == null ? "" : a.Company.EnglishName,
                        StageName = a.Stage == null ? "" : a.Stage.Name,
                        SeverityName = a.Severity == null ? "" : a.Severity.Name,
                        SeverityCode = a.Severity == null ? "" : a.Severity.Code,
                        MainClassificationName = a.MainClassification == null ? "" : a.MainClassification.Name,
                        SecondaryClassificationName = a.SecondaryClassification == null ? "" : a.SecondaryClassification.Name,
                        ContactName = a.Contact != null ? a.Contact.EnglishName : null,
                        CCs = a.CCs,
                        Bcc = a.Bcc,
                        CreatedByContactName = a.Contact != null ? a.Contact.EnglishName : null,
                        ActivityWatch = a.Company == null ? false : (a.Company.Customer == null ? false : a.Company.Customer.ActivityWatch),
                        RankCode = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Code),
                        RankId = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Id),
                        RankName = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Name),
                        SeverityPriority = a.Severity == null ? 0 : a.Severity.Severity,
                        BusinessUnitId=a.BusinessUnitId,
                        ContactEmail = a.Contact != null ? a.Contact.Email : null,
                        InternalUsers = a.InternalUsers,
                        ClosureDescription = a.ClosureDescription,
                        Closewithoutnotifying = a.Closewithoutnotifying,
                        LastCompletedActivityDate = a.LastCompletedActivityDate,
                        LastCompletedActivityTypeCode = a.LastCompletedActivityTypeCode,
                        LastCompletedActivitySubject = a.LastCompletedActivitySubject,
                        NextActivityDate = a.NextActivityDate,
                        NextActivityTypeCode = a.NextActivityTypeCode,
                        NextActivitySubject = a.NextActivitySubject,
                        NextActivityTypeName = a.NextActivityType != null ? a.NextActivityType.Name : null,
                        LastCompletedActivityTypeName = a.ActivityType != null ? a.ActivityType.Name : null,
                        TicketDescription = a.TicketDescription,
                        ShipmentId = a.ShipmentId,
                        ShipmentNumber = a.ShipmentNumber,
                        FirstResolveDate = a.FirstResolveDate,
                        EmployeeGroupId = a.EmployeeGroupId,
                        Source = a.Source,
                        SourceName = a.TicketSource != null ? a.TicketSource.Name : null,
                        CreatedbyType = a.CreatedbyType,
                        CreatedbyTypeName = a.TicketCreatedByType != null ? a.TicketCreatedByType.Name : null,
                        OpenPeriodMinutes = a.OpenPeriodMinutes,
                        OpenDate = a.OpenDate,
                        OwnerEmail = a.Owner != null && a.Owner.Contact != null? a.Owner.Contact.Email : null,
                        QuoteId = a.QuoteId,
                        QuoteNumber = a.QuoteNumber,
                        EntityNumber = a.ShipmentNumber != null ? a.ShipmentNumber : a.QuoteNumber,
                    };

                    ContactRepository rep = new ContactRepository(tenant);
                    Contact ownerContact = rep.GetSingleContact(a.OwnerId, tenant);
                    Contact updatedByUserContact = rep.GetSingleContact(a.UpdatedByUserId, tenant);
                    list.OwnerName = "";
                    list.UpdatedByUserName = ""; 
                    if (ownerContact != null)
                    {
                        list.OwnerName = ownerContact.EnglishName;
                    }

                    if (updatedByUserContact != null)
                    {
                        list.UpdatedByUserName = updatedByUserContact.EnglishName;
                    }

                    TicketTypeRepository repType = new TicketTypeRepository(tenant);
                    TicketType type = repType.GetSingle(a.TicketTypeId, tenant);
                    if (type != null)
                    {
                        list.TypeName = type.Name;
                    }
                    else
                    {
                        list.TypeName = "";
                    }

                    entityList.Add(list);
                }
            }

            return entityList;
        }

        public List<TicketList> GetTicketListByShipmentIdList(string shipmentId, int tenant)
        {
            IQueryable<TicketList> query = (from a in context.Tickets.Include("Owner").Include("Company").Include("Contact").Include("Stage").Include("TicketType").Include("Severity").Include("MainClassification").Include("SecondaryClassification").Include("BusinessUnit").Include("NextActivityType").Include("ActivityType")
                                            where a.Tenant == tenant && a.ShipmentId == shipmentId
                                            select new TicketList()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                TicketNumber = a.TicketNumber,
                                                Subject = a.Subject,
                                                ContactId = a.ContactId,
                                                StageId = a.StageId,
                                                CreateDate = a.CreateDate,
                                                CreatedByContactId = a.CreatedByContactId,
                                                UpdateDate = a.UpdateDate,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                CompanyId = a.CompanyId,
                                                MainClassificationId = a.MainClassificationId,
                                                SecondaryClassificationId = a.SecondaryClassificationId,
                                                SeverityId = a.SeverityId,
                                                TicketTypeId = a.TicketTypeId,
                                                IsClosed = a.IsClosed,
                                                IsCancelled = a.IsCancelled,
                                                SearchFields = a.SearchFields,
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
                                                OwnerId = a.OwnerId,
                                                OwnerName = a.Owner == null ? "" : a.Owner.Contact.EnglishName,
                                                CompanyName = a.Company == null ? "" : a.Company.EnglishName,
                                                StageName = a.Stage == null ? "" : a.Stage.Name,
                                                StageCode = a.Stage == null ? "" : a.Stage.Code,
                                                TypeName = a.TicketType == null ? "" : a.TicketType.Name,
                                                SeverityName = a.Severity == null ? "" : a.Severity.Name,
                                                SeverityCode = a.Severity == null ? "" : a.Severity.Code,
                                                MainClassificationName = a.MainClassification == null ? "" : a.MainClassification.Name,
                                                SecondaryClassificationName = a.SecondaryClassification == null ? "" : a.SecondaryClassification.Name,
                                                ContactName = a.Contact != null ? a.Contact.EnglishName : null,
                                                CCs = a.CCs,
                                                Bcc = a.Bcc,
                                                CreatedByContactName = a.Contact != null ? a.Contact.EnglishName : null,
                                                ActivityWatch = a.Company == null ? false : (a.Company.Customer == null ? false : a.Company.Customer.ActivityWatch),
                                                RankCode = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Code),
                                                RankId = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Id),
                                                RankName = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Name),
                                                SeverityPriority = a.Severity == null ? 0 : a.Severity.Severity,
                                                BusinessUnitId = a.BusinessUnitId,
                                                FirstResponseDue = a.FirstResponseDue,
                                                FirstResponseTime = a.FirstResponseTime,
                                                FullResolvedTime = a.FullResolvedTime,
                                                ResolveWithinDue = a.ResolveWithinDue,
                                                ContactEmail = a.Contact != null ? a.Contact.Email : null,
                                                OpenEscalation = a.OpenEscalation,
                                                InternalUsers = a.InternalUsers,
                                                ClosureDescription = a.ClosureDescription,
                                                Closewithoutnotifying = a.Closewithoutnotifying,
                                                LastCompletedActivityDate = a.LastCompletedActivityDate,
                                                LastCompletedActivityTypeCode = a.LastCompletedActivityTypeCode,
                                                LastCompletedActivitySubject = a.LastCompletedActivitySubject,
                                                NextActivityDate = a.NextActivityDate,
                                                NextActivityTypeCode = a.NextActivityTypeCode,
                                                NextActivitySubject = a.NextActivitySubject,
                                                NextActivityTypeName = a.NextActivityType != null ? a.NextActivityType.Name : null,
                                                LastCompletedActivityTypeName = a.ActivityType != null ? a.ActivityType.Name : null,
                                                TicketDescription = a.TicketDescription,
                                                ShipmentId = a.ShipmentId,
                                                ShipmentNumber = a.ShipmentNumber,
                                                UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                                                FirstResolveDate = a.FirstResolveDate,
                                                EmployeeGroupId = a.EmployeeGroupId,
                                                Source = a.Source,
                                                SourceName = a.TicketSource != null ? a.TicketSource.Name : null,
                                                CreatedbyType = a.CreatedbyType,
                                                CreatedbyTypeName = a.TicketCreatedByType != null ? a.TicketCreatedByType.Name : null,
                                                OpenPeriodMinutes = a.OpenPeriodMinutes,
                                                OpenDate = a.OpenDate,
                                                OwnerEmail = a.Owner != null && a.Owner.Contact != null ? a.Owner.Contact.Email : null,
                                                QuoteId = a.QuoteId,
                                                QuoteNumber = a.QuoteNumber,
                                                EntityNumber = a.ShipmentNumber != null ? a.ShipmentNumber : a.QuoteNumber,
                                            });
            return query.ToList();
        }

        public List<TicketList> GetTicketListByQuoteIdList(string quoteId, int tenant)
        {
            IQueryable<TicketList> query = (from a in context.Tickets.Include("Owner").Include("Company").Include("Contact").Include("Stage").Include("TicketType").Include("Severity").Include("MainClassification").Include("SecondaryClassification").Include("BusinessUnit").Include("NextActivityType").Include("ActivityType")
                                            where a.Tenant == tenant && a.QuoteId == quoteId
                                            select new TicketList()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                TicketNumber = a.TicketNumber,
                                                Subject = a.Subject,
                                                ContactId = a.ContactId,
                                                StageId = a.StageId,
                                                CreateDate = a.CreateDate,
                                                CreatedByContactId = a.CreatedByContactId,
                                                UpdateDate = a.UpdateDate,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                CompanyId = a.CompanyId,
                                                MainClassificationId = a.MainClassificationId,
                                                SecondaryClassificationId = a.SecondaryClassificationId,
                                                SeverityId = a.SeverityId,
                                                TicketTypeId = a.TicketTypeId,
                                                IsClosed = a.IsClosed,
                                                IsCancelled = a.IsCancelled,
                                                SearchFields = a.SearchFields,
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
                                                OwnerId = a.OwnerId,
                                                OwnerName = a.Owner == null ? "" : a.Owner.Contact.EnglishName,
                                                CompanyName = a.Company == null ? "" : a.Company.EnglishName,
                                                StageName = a.Stage == null ? "" : a.Stage.Name,
                                                StageCode = a.Stage == null ? "" : a.Stage.Code,
                                                TypeName = a.TicketType == null ? "" : a.TicketType.Name,
                                                SeverityName = a.Severity == null ? "" : a.Severity.Name,
                                                SeverityCode = a.Severity == null ? "" : a.Severity.Code,
                                                MainClassificationName = a.MainClassification == null ? "" : a.MainClassification.Name,
                                                SecondaryClassificationName = a.SecondaryClassification == null ? "" : a.SecondaryClassification.Name,
                                                ContactName = a.Contact != null ? a.Contact.EnglishName : null,
                                                CCs = a.CCs,
                                                Bcc = a.Bcc,
                                                CreatedByContactName = a.Contact != null ? a.Contact.EnglishName : null,
                                                ActivityWatch = a.Company == null ? false : (a.Company.Customer == null ? false : a.Company.Customer.ActivityWatch),
                                                RankCode = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Code),
                                                RankId = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Id),
                                                RankName = a.Company == null ? null : (a.Company.Customer == null ? null : a.Company.Customer.Rank.Name),
                                                SeverityPriority = a.Severity == null ? 0 : a.Severity.Severity,
                                                BusinessUnitId = a.BusinessUnitId,
                                                FirstResponseDue = a.FirstResponseDue,
                                                FirstResponseTime = a.FirstResponseTime,
                                                FullResolvedTime = a.FullResolvedTime,
                                                ResolveWithinDue = a.ResolveWithinDue,
                                                ContactEmail = a.Contact != null ? a.Contact.Email : null,
                                                OpenEscalation = a.OpenEscalation,
                                                InternalUsers = a.InternalUsers,
                                                ClosureDescription = a.ClosureDescription,
                                                Closewithoutnotifying = a.Closewithoutnotifying,
                                                LastCompletedActivityDate = a.LastCompletedActivityDate,
                                                LastCompletedActivityTypeCode = a.LastCompletedActivityTypeCode,
                                                LastCompletedActivitySubject = a.LastCompletedActivitySubject,
                                                NextActivityDate = a.NextActivityDate,
                                                NextActivityTypeCode = a.NextActivityTypeCode,
                                                NextActivitySubject = a.NextActivitySubject,
                                                NextActivityTypeName = a.NextActivityType != null ? a.NextActivityType.Name : null,
                                                LastCompletedActivityTypeName = a.ActivityType != null ? a.ActivityType.Name : null,
                                                TicketDescription = a.TicketDescription,
                                                ShipmentId = a.ShipmentId,
                                                ShipmentNumber = a.ShipmentNumber,
                                                UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                                                FirstResolveDate = a.FirstResolveDate,
                                                EmployeeGroupId = a.EmployeeGroupId,
                                                Source = a.Source,
                                                SourceName = a.TicketSource != null ? a.TicketSource.Name : null,
                                                CreatedbyType = a.CreatedbyType,
                                                CreatedbyTypeName = a.TicketCreatedByType != null ? a.TicketCreatedByType.Name : null,
                                                OpenPeriodMinutes = a.OpenPeriodMinutes,
                                                OpenDate = a.OpenDate,
                                                OwnerEmail = a.Owner != null && a.Owner.Contact != null ? a.Owner.Contact.Email : null,
                                                QuoteId = a.QuoteId,
                                                QuoteNumber = a.QuoteNumber,
                                                EntityNumber = a.ShipmentNumber != null ? a.ShipmentNumber : a.QuoteNumber,
                                            });
            return query.ToList();
        }

        public int GetTicketsCountByShipmentId(string shipmentId, int tenant)
        {
            return (from a in context.Tickets where a.Tenant == tenant && a.ShipmentId == shipmentId select a).Count();
        }
    }
}
	