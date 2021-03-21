 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Data.Helpers;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class ActivityRepository:IRepository<Activity>
   {
        
		public List<Activity> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }

        public IQueryable<Activity> GetAllFromIdList(List<string> ids, int tenant)
        {
            IQueryable<Activity> entities = (from a in context.Activities where a.Tenant == tenant && ids.Contains(a.Id) select a);
            return entities;
        }

        public IQueryable<Activity> GetActivitiesByOpportunityId(string opportunityId, int tenant)
        {
            return (from a in context.Activities where a.Tenant == tenant && a.OpportunityId == opportunityId select a);
        }

        public IQueryable<Activity> GetActivitiesByQuoteId(string quoteId, int tenant)
        {
            return (from a in context.Activities where a.Tenant == tenant && a.QuoteId == quoteId select a);
        }

        public IQueryable<Activity> GetActivitiesByCustomerId(string customerId, int tenant)
        {
            return (from a in context.Activities where a.Tenant == tenant && a.CustomerId == customerId select a);
        }

        public Activity GetSingleByOutlookId(string outlookId, int tenant)
        {
            return (from a in context.Activities
                    where a.OutlookId == outlookId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public Activity GetSingleByOwnerId(string id, string ownerId, int tenant)
        {
            return (from a in context.Activities
                    where a.Id == id && a.OwnerId == ownerId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Activity> GetActivitiesByTicketId(string ticketId, int tenant)
        {
            return (from a in context.Activities where a.Tenant == tenant && a.TicketId == ticketId select a);
        }

        public IQueryable<Activity> GetExtendedActivitiesByShipmentId(string shipmentId, int tenant)
        {
            return (from a in context.Activities where a.Tenant == tenant && a.ShipmentId == shipmentId && a.ActivityTypeCode == "TX" select a);
        }

        public IQueryable<Activity> GetExtendedActivitiesByQuoteId(string quoteId, int tenant)
        {
            return (from a in context.Activities where a.Tenant == tenant && a.QuoteId == quoteId && a.ActivityTypeCode == "TX" select a);
        }

        public IQueryable<Activity> GetExtendedActivities(int tenant)
        {
            return (from a in context.Activities where a.Tenant == tenant && a.ActivityTypeCode == "TX" select a);
        }
    }
}
   