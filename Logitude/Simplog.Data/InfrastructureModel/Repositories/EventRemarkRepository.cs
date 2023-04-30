using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
        public partial class EventRemarkRepository : IRepository<AdvancedQueryFilter>
    {

            private IWebFreightContext webFreightContext;
        public EventRemarkRepository(int tenant)
            {
                webFreightContext = WebFreightContext.GetContext(tenant);
            }

            public EventRemarkRepository(IWebFreightContext context)
            {
                 webFreightContext = context;
            }

            public IWebFreightContext context
            {
                get { return webFreightContext; }
            }

        public EventRemark GetSingle(string id, int tenant)
            {
                return (from a in context.EventRemarks
                        where a.Id == id && a.Tenant == tenant
                        select a).FirstOrDefault();
            }
        public EventRemark GetEventRemarkByPartnerTypeId(string PartnerTypeId, string EventTypeId, int tenant)
        {
            var eventRemark = (from a in context.EventRemarks
                               where a.PartnerTypeId == PartnerTypeId && a.EventTypeId== EventTypeId && a.Tenant == tenant
                               select a).FirstOrDefault();
            return eventRemark;
        }

        public IQueryable<EventRemark> GetAll(int tenant)
            {
                return from a in context.EventRemarks
                       where a.Tenant == tenant
                       select a;
            }

            public EventRemark GetSingle(EntityKeyFields entityKeys)
            {
                //EventRemarkKeys keys = entityKeys as EventRemarkKeys;
                //return (from a in context.EventRemarks
                //        where a.Id == keys.Id
                //        select a).FirstOrDefault();
                throw new System.NotImplementedException();
            }

            partial void onAdd();//Partial Methods Definition in Generated
            public void Add(EventRemark entity)
            {
                onAdd();
                context.EventRemarks.Add(entity);
            }

            public void Remove(EventRemark entity)
            {
                context.EventRemarks.Attach(entity);
                context.EventRemarks.Remove(entity);
            }

            partial void onUpdate();//Partial Methods Definition in Generated
            public void Update(EventRemark entity)
            {
                onUpdate();
                context.EventRemarks.Attach(entity);
                context.SetAsModified(entity);
            }

            public List<EventRemark> All()
            {
                return context.EventRemarks.ToList();
            }

            public void SubmitChanges()
            {
                context.SaveChanges();
            }

        public void Add(AdvancedQueryFilter entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(AdvancedQueryFilter entity)
        {
            throw new NotImplementedException();
        }

        public void Update(AdvancedQueryFilter entity)
        {
            throw new NotImplementedException();
        }

        List<AdvancedQueryFilter> IRepository<AdvancedQueryFilter>.All()
        {
            throw new NotImplementedException();
        }

        public List<AdvancedQueryFilter> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        AdvancedQueryFilter IRepository<AdvancedQueryFilter>.GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }

}
