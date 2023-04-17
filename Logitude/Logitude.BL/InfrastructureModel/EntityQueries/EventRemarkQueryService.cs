using System;
using System.Linq;
using Logitude.Accounting.Data;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System.Web;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
        public partial class EventRemarkQueryService
        {

            EventRemarkRepository repository;
            IAccountingContext context;
            public EventRemarkQueryService(int tenant)
            {
                context = AccountingContext.GetContext(tenant);
                repository = new EventRemarkRepository(repository.context);
            }

            public EventRemarkQueryService(EventRemarkRepository repository)
            {
                this.repository = repository;
            }

            public EventRemarkQueryService(IAccountingContext context)
            {
                this.repository = new EventRemarkRepository(repository.context);
                this.context = context;
            }

            public EventRemarkPM GetSingle(string id, int tenant = 0)
            {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "EventRemarkPM" + id;
                EventRemarkPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var eventRemarks = (from a in repository.context.EventRemarks
                                          select new EventRemarkPM() { Id = a.Id, });
                    }
                    else
                    {
                        entity = (EventRemarkPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.EventRemarks
                              where a.Id == id
                              select new EventRemarkPM() { Id = a.Id}).FirstOrDefault();
                }
            }
            return null;
        }
        public IQueryable<EventRemarkList> GetIQueryableEntityList(IQueryable<EventRemark> iQueryable)
        {
            IQueryable<EventRemarkList> result = from entity in iQueryable
                                               select new EventRemarkList()
                                               {
                                                  
                                                   Id = entity.Id,
                                                   SearchFields = entity.SearchFields
                                               };
            return result;
        }
        public IQueryable<EventRemarkList> GetEventRemarksByEventTypeID(string EventTypeID)
        {
            var eventRemarks = (from a in repository.context.EventRemarks
                                where a.EventTypeId == EventTypeID
                                select new EventRemarkList() { 
                                    Id = a.Id,
                                    Tenant= a.Tenant,
                                    CreateDate= a.CreateDate,
                                    CreatedByUserId= a.CreatedByUserId,
                                    UpdateDate= a.UpdateDate,
                                    UpdatedByUserId=a.UpdatedByUserId,
                                    SearchFields= a.SearchFields,
                                    EventTypeId= a.EventTypeId,
                                    PartnerTypeId =a.PartnerTypeId,
                                    IsChoose =a.IsChoose,
                                });
            return eventRemarks;
        }

    }



}
