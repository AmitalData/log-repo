 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Amital.QuoteOPM.Data.Repsitories
{
   public partial class QuoteOPRepository:IRepository<QuoteOP>
   {
        
		public List<QuoteOP> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public IQueryable<QuoteOP> GetQuotes(int tenant)
        {
            return (from d in context.QuoteOPs.Include("Stage").Include("Rating") where d.Tenant == tenant select d);
        }
        public string GetQuoteNumber(string id)
        {
            return (from a in context.QuoteOPs where a.Id == id select a.QuoteNumber).FirstOrDefault();
        }

        public string GetQuoteId(string quoteNumber, int tenant)
        {
            return (from a in context.QuoteOPs where a.QuoteNumber == quoteNumber && a.Tenant == tenant select a.Id).FirstOrDefault();
        }

        public int GetQuotesCount(int tenant)
        {
            return (from record in context.QuoteOPs where record.Tenant == tenant && record.IsCancelled == false select record).Count();
        }

      

        public IQueryable<QuoteOP> GetAllIncludeStage(int tenant)
        {
            return from a in context.QuoteOPs.Include("Stage")
                   where a.Tenant == tenant
                   select a;
        }

        



        public IQueryable<QuoteOP> GetSentQuotes(int tenant)
        {
            return (from d in context.QuoteOPs.Include("Stage").Include("Rating") where d.Tenant == tenant && d.Stage.Name == "Sent" && !d.IsCancelled select d);
        }

        public IQueryable<QuoteOP> GetAcceptedQuotes(int tenant)
        {
            return (from d in context.QuoteOPs.Include("Stage").Include("Rating") where d.Tenant == tenant && d.Stage.Name == "Accepted" && !d.IsCancelled select d);
        }

        public IQueryable<QuoteOP> GetDeclinedQuotes(int tenant)
        {
            return (from d in context.QuoteOPs.Include("Stage").Include("Rating") where d.Tenant == tenant && d.Stage.Name == "Declined" && !d.IsCancelled select d);
        }

        public QuoteOP GetSingleQuote(string id, int tenant)
        {

            var q = (from record in context.QuoteOPs.Include("Incoterm").Include("FromPort").Include("Stage").Include("Rating").Include("QuoteOPType").Include("TransportMode").Include("Direction").Include("ToPort")
                     //.Include("ShipmentType")
                     .Include("ToPort.Country").Include("FromPort.Country").Include("CreatedByUser.Contact").Include("MainCarriageCarrierCard").Include("Department").Include("Branch").Include("FromPartnerAddress").Include("ToPartnerAddress").Include("FromPartnerAddress.Country").Include("ToPartnerAddress.Country").Include("FreelancerCard").Include("BusinessUnit").Include("QuoteOPClosingReason").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AgentCard").Include("NotifyCard")
                     where record.Id == id && record.Tenant == tenant
                     select record);
            return q.FirstOrDefault();

            //return (from record in context.QuoteOPs.Include("Incoterm").Include("FromPort").Include("Stage").Include("Rating").Include("QuoteOPType").Include("TransportMode").Include("Direction").Include("ToPort").Include("ShipmentType").Include("ToPort.Country").Include("FromPort.Country").Include("CreatedByUser.Contact").Include("MainCarriageCarrierCard").Include("Department").Include("Branch").Include("FromPartnerAddress").Include("ToPartnerAddress").Include("FromPartnerAddress.Country").Include("ToPartnerAddress.Country").Include("FreelancerCard").Include("BusinessUnit").Include("QuoteOPClosingReason").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AgentCard").Include("NotifyCard")
            //        where record.Id == id && record.Tenant == tenant
            //        select record).FirstOrDefault();
        }

        public QuoteOP GetSingleQuoteWithOutInCluded(string id, int tenant)
        {
            return (from record in context.QuoteOPs
                    where record.Id == id && record.Tenant == tenant
                    select record).FirstOrDefault();
        }

        public QuoteOP GetSingleQuoteByNumber(string number, int tenant)
        {
            return (from record in context.QuoteOPs.Include("Incoterm").Include("FromPort").Include("Stage").Include("Rating").Include("QuoteOPType").Include("TransportMode").Include("Direction").Include("ToPort").Include("ShipmentType").Include("ToPort.Country").Include("FromPort.Country").Include("CreatedByUser.Contact").Include("MainCarriageCarrierCard").Include("Department").Include("Branch").Include("FromPartnerAddress").Include("ToPartnerAddress").Include("FromPartnerAddress.Country").Include("ToPartnerAddress.Country").Include("FreelancerCard").Include("BusinessUnit").Include("QuoteOPClosingReason").Include("SalesmanUser").Include("SalesmanUser.Contact")
                    where record.QuoteNumber == number && record.Tenant == tenant
                    select record).FirstOrDefault();
        }
        public IQueryable<QuoteOP> GetQuoteByTenant(int tenant, bool isClosed, string cardId)
        {
            IQueryable<QuoteOP> quotes = null;
            if (!string.IsNullOrEmpty(cardId))
            {
                quotes = from a in context.QuoteOPs
                         where a.Tenant == tenant && a.IsClosed == isClosed && (a.ShipperId == cardId)
                         select a;
            }
            else
            {
                quotes = from a in context.QuoteOPs
                         where a.Tenant == tenant && a.IsClosed == isClosed
                         select a;

            }

            return quotes;
        }
        public IQueryable<QuoteOP> GetQuotesByCustomerId(string customerId, int tenant)
        {
            return from a in context.QuoteOPs
                   where a.CustomerId == customerId && a.Tenant == tenant
                   select a;
        }

        public IQueryable<QuoteOP> GetQuoteByTenant(int tenant, string cardId)
        {
            IQueryable<QuoteOP> quotes = null;

            if (!string.IsNullOrEmpty(cardId))
            {
                quotes = from a in context.QuoteOPs
                         where a.Tenant == tenant && (a.ShipperId == cardId)
                         select a;
            }
            else
            {
                quotes = from a in context.QuoteOPs
                         where a.Tenant == tenant
                         select a;
            }

            return quotes;
        }

        public IQueryable<QuoteOP> GetFirst20QuotesByTenant(int tenant, string quoteId)
        {
            //string l = "1000";
            //string y = "1001";
            //int comparator=l.CompareTo(y);
            IQueryable<QuoteOP> quotes = (from a in context.QuoteOPs
                                        where a.Tenant == tenant && a.Id.CompareTo(quoteId) <= 1 && a.Id.CompareTo(quoteId) >= 0
                                        select a).Take(1);
            //Quote sss = (from a in Quotes
            //                where a.Id == QuoteId
            //                select a).FirstOrDefault();
            return quotes;
        }

        public QuoteOP GetFirstQuote(int tenant)
        {
            return (from a in context.QuoteOPs
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public int GetAllQuotesCountForCustomer(int tenant, string customerId)
        {
            int count = (from a in context.QuoteOPs
                         where a.Tenant == tenant && a.CustomerId == customerId && a.IsCancelled == false
                         select a).Count();
            return count;
        }

        public int GetOpenQuotesCountForCustomer(int tenant, string customerId)
        {
            int count = (from a in context.QuoteOPs
                         where a.Tenant == tenant && a.IsClosed == false && a.CustomerId == customerId && a.IsCancelled == false
                         select a).Count();
            return count;
        }

    }

}
   