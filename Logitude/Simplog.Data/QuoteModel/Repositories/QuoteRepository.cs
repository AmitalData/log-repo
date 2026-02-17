using System.Collections.Generic;
using System.Linq;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteRepository : IRepository<Quote>
    {
        IQuotesContext quotesContext;

        public QuoteRepository(IQuotesContext context)
        {
            quotesContext = context;
        }

        public QuoteRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }

        public QuoteRepository()
        {
            quotesContext = new QuotesContext();
        }

        public string GetQuoteNumber(string id)
        {
            return (from a in context.Quotes where a.Id == id select a.QuoteNumber).FirstOrDefault();
        }

        public string GetQuoteId(string quoteNumber, int tenant)
        {
            return (from a in context.Quotes where a.QuoteNumber == quoteNumber && a.Tenant == tenant select a.Id).FirstOrDefault();
        }

        public int GetQuotesCount(int tenant)
        {
            return (from record in context.Quotes where record.Tenant == tenant && record.IsCancelled == false select record).Count();
        }
        
        public IQueryable<Quote> GetQuotes(int tenant)
        {
            return (from d in context.Quotes.Include("Stage").Include("Rating") where d.Tenant == tenant select d);
        }

        public IQueryable<Quote> GetSentQuotes(int tenant)
        {
            return (from d in context.Quotes.Include("Stage").Include("Rating") where d.Tenant == tenant && d.Stage.Name == "Sent" && !d.IsCancelled select d);
        }

        public IQueryable<Quote> GetAcceptedQuotes(int tenant)
        {
            return (from d in context.Quotes.Include("Stage").Include("Rating") where d.Tenant == tenant && d.Stage.Name == "Accepted" && !d.IsCancelled select d);
        }

        public IQueryable<Quote> GetDeclinedQuotes(int tenant)
        {
            return (from d in context.Quotes.Include("Stage").Include("Rating") where d.Tenant == tenant && d.Stage.Name == "Declined" && !d.IsCancelled select d);
        }

        public Quote GetSingleQuote(string id, int tenant)
        {
            return (from record in context.Quotes.Include("Incoterm").Include("FromPort").Include("Stage").Include("Rating").Include("QuoteType").Include("TransportMode").Include("Direction").Include("ToPort").Include("ShipmentType").Include("ToPort.Country").Include("FromPort.Country").Include("CreatedByUser.Contact").Include("MainCarriageCarrierCard").Include("Department").Include("Branch").Include("FromPartnerAddress").Include("ToPartnerAddress").Include("FromPartnerAddress.Country").Include("ToPartnerAddress.Country").Include("FreelancerCard").Include("BusinessUnit").Include("QuoteClosingReason").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AgentCard").Include("NotifyCard")
                    where record.Id == id && record.Tenant == tenant 
                    select record).FirstOrDefault();
        }

        public Quote GetSingleQuoteWithOutInCluded(string id, int tenant)
        {
            return (from record in context.Quotes
                    where record.Id == id && record.Tenant == tenant
                    select record).FirstOrDefault();
        }

        public Quote GetSingleQuoteByNumber(string number, int tenant)
        {
            return (from record in context.Quotes.Include("Incoterm").Include("FromPort").Include("Stage").Include("Rating").Include("QuoteType").Include("TransportMode").Include("Direction").Include("ToPort").Include("ShipmentType").Include("ToPort.Country").Include("FromPort.Country").Include("CreatedByUser.Contact").Include("MainCarriageCarrierCard").Include("Department").Include("Branch").Include("FromPartnerAddress").Include("ToPartnerAddress").Include("FromPartnerAddress.Country").Include("ToPartnerAddress.Country").Include("FreelancerCard").Include("BusinessUnit").Include("QuoteClosingReason").Include("SalesmanUser").Include("SalesmanUser.Contact")
                    where record.QuoteNumber == number && record.Tenant == tenant
                    select record).FirstOrDefault();
        }
        public IQueryable<Quote> GetQuoteByTenant(int tenant, bool isClosed, string cardId)
        {
            IQueryable<Quote> quotes = null;
            if (!string.IsNullOrEmpty(cardId))
            {
                quotes = from a in context.Quotes                             
                            where a.Tenant == tenant && a.IsClosed == isClosed && (a.ShipperId == cardId)
                            select a;
            }
            else
            {
                quotes = from a in context.Quotes
                            where a.Tenant == tenant && a.IsClosed == isClosed
                            select a;

            }

            return quotes;
        }

        public IQueryable<Quote> GetQuoteByTenant(int tenant, string cardId)
        {
            IQueryable<Quote> quotes = null;

            if (!string.IsNullOrEmpty(cardId))
            {
                quotes = from a in context.Quotes
                            where a.Tenant == tenant && (a.ShipperId == cardId)
                            select a;
            }
            else
            {
                quotes = from a in context.Quotes
                            where a.Tenant == tenant
                            select a;
            }

            return quotes;
        }

        public IQueryable<Quote> GetFirst20QuotesByTenant(int tenant, string quoteId)
        {
            //string l = "1000";
            //string y = "1001";
            //int comparator=l.CompareTo(y);
            IQueryable<Quote> quotes = (from a in context.Quotes
                                              where a.Tenant == tenant && a.Id.CompareTo(quoteId) <= 1 && a.Id.CompareTo(quoteId) >= 0
                                              select a).Take(1);
            //Quote sss = (from a in Quotes
            //                where a.Id == QuoteId
            //                select a).FirstOrDefault();
            return quotes;
        }

        public Quote GetFirstQuote(int tenant)
        {
            return (from a in context.Quotes
                    where a.Tenant==tenant
                    select a).FirstOrDefault();
        }

        public int GetAllQuotesCountForCustomer(int tenant,string customerId)
        {
            int count = (from a in context.Quotes
                         where a.Tenant == tenant&&a.CustomerId==customerId&&a.IsCancelled==false
                         select a).Count();
            return count;
        }

        public int GetOpenQuotesCountForCustomer(int tenant, string customerId)
        {
            int count = (from a in context.Quotes
                         where a.Tenant == tenant && a.IsClosed == false && a.CustomerId == customerId && a.IsCancelled == false
                         select a).Count();
            return count;
        }

        public void Add(Quote entity)
        {
            context.Quotes.Add(entity);
        }

        public void Remove(Quote entity)
        {
            context.Quotes.Attach(entity);
            context.Quotes.Remove(entity);
        }

        public void Update(Quote entity)
        {
            try
            {
                context.Quotes.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<Quote> All()
        {
            return context.Quotes.ToList();
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
        
        internal List<Quote> GetQuotesByShipmentData
            (
            string clientId,
            string fromPortId,
            string toPortId,
            string directionId,
            string transportModeId,
            string shipmentTypeId,
            int tenant
            )
        {
            List<Quote> list = (from a in context.Quotes
                                where a.Tenant == tenant
                                && a.ShipperId == clientId
                                && a.FromPortId == fromPortId
                                && a.ToPortId == toPortId                                 
                                && a.DirectionId == directionId                                  
                                && a.TransportModeId == transportModeId                                 
                                && a.ShipmentTypeId == shipmentTypeId
                                
                                select a).ToList();

            return list;
        }





        public List<Quote> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Quote GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<QuoteFollowUpDataView> GetQuoteFollowUpDataViewByTenant(int tenant)
        {
            IQuoteFollowUpDataViewContext dataViewEntities = QuoteFollowUpDataViewContext.GetContext(tenant);
            IQueryable<QuoteFollowUpDataView> result = (from a in dataViewEntities.QuoteFollowUpDataViews where a.Tenant == tenant && a.IsCancelled == false select a);
            return result;
        }

        public QuoteFollowUpDataView GetSingleQuoteFollowUpDataView(string iQuoteId, int tenant)
        {
            IQuoteFollowUpDataViewContext dataViewEntities = QuoteFollowUpDataViewContext.GetContext(tenant);
            QuoteFollowUpDataView result = (from f in dataViewEntities.QuoteFollowUpDataViews where f.Id == iQuoteId && f.Tenant == tenant select f).FirstOrDefault();
            return result;
        }

        public IQueryable<Quote> GetQuotesByCustomerId(string customerId, int tenant)
        {
            return from a in context.Quotes
                   where a.CustomerId == customerId && a.Tenant == tenant
                   select a;
        }
    }
}
