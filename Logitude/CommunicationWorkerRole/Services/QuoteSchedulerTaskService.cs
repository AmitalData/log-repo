using CommunicationWorkerRole.Tasks;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommunicationWorkerRole.Services
{
    public class QuoteSchedulerTaskService
    {
        TaskManagerBase currentTask;

        private string successMsg = "Total quotes closed: ";
        private string failedMsg = "Total failed closures: ";
        private static int successCounter = 0;
        private static int failedCounter = 0;
        private static Dictionary<int, QuoteNumbers> successQuotesDetails { get; set; }
        private static Dictionary<int, QuoteNumbers> failedQuotesDetails { get; set; }

        public QuoteSchedulerTaskService(TaskManagerBase task)
        {
            this.currentTask = task;
            successCounter = 0;
            failedCounter = 0;
            successQuotesDetails = new Dictionary<int, QuoteNumbers>();
            failedQuotesDetails = new Dictionary<int, QuoteNumbers>();
        }

        public void ExecuteDailyAutomaticallyClosing(int tenant)
        {

            IQuotesContext quotesContext;
            IQuotesContext quotesContext_Loop;
            DateTime todayDate = DateTime.Now;
            QuoteQuery quoteQuery;
            QuoteService quoteService;
            QuotePM quotePM;
            quotesContext = QuotesContext.GetContext(tenant);
            IQueryable<Quote> allQuotes = (from d in quotesContext.Quotes
                                           where d.IsAutomaticallyClosed && !d.IsClosed && !d.IsCancelled && d.AutomaticallyCloseDate != null
                                           && System.Data.Entity.DbFunctions.TruncateTime(d.AutomaticallyCloseDate) <= System.Data.Entity.DbFunctions.TruncateTime(todayDate)
                                           select d);
            foreach (Quote item in allQuotes)
            {
                try
                {
                    quotesContext_Loop = QuotesContext.GetContext(item.Tenant);
                    quoteQuery = new QuoteQuery(item.Tenant);
                    var email = "system@tenant" + item.Tenant + ".com";
                    quoteService = new QuoteService(quotesContext_Loop, item.Tenant, email);
                    quotePM = quoteQuery.GetSinglePMForWorkerRole(item.Id, item.Tenant);
                    quotePM.IsClosed = true;
                    var quoteClosingReasonRepository = new QuoteClosingReasonRepository(item.Tenant);
                    var quoteClosing = quoteClosingReasonRepository.GetSingleQuoteClosingReasonByCode("XQ", item.Tenant);
                    quotePM.QuoteClosingReasonId = quoteClosing != null ? quoteClosing.Id : null;
                    quotePM.QuoteClosingReasonCode = "XQ";
                    quotePM.ActionType = "Decline";
                    quoteService.Update(quotePM, true);
                    this.AddLogMessage_Success(item);
                }
                catch (Exception ex)
                {
                    this.AddLogMessage_Failed(item);
                    continue;
                }
            }

            this.BuildLogMessageInfo_Success();
            this.BuildLogMessageInfo_Failed();
        }

        private void BuildLogMessageInfo_Failed()
        {
            this.failedMsg = this.failedMsg + failedCounter + Environment.NewLine;
            foreach (var item in failedQuotesDetails.ToArray())
            {
                this.failedMsg = this.failedMsg + "Tenant " + item.Key + ":" + item.Value.Count + "(" + item.Value.Numbers+")" + Environment.NewLine;
            }

            if (this.currentTask != null)
            {
                currentTask.LogInfo(this.failedMsg);
            }
        }

        private void BuildLogMessageInfo_Success()
        {
            this.successMsg = this.successMsg + successCounter + Environment.NewLine;
            foreach (var item in successQuotesDetails.ToArray())
            {
                this.successMsg = this.successMsg + "Tenant " + item.Key + ":" + item.Value.Count + "(" + item.Value.Numbers + ")" + Environment.NewLine;
            }

            if (this.currentTask != null)
            {
                currentTask.LogInfo(this.successMsg);
            }
        }

        private void AddLogMessage_Success(Quote quote)
        {
            if (successQuotesDetails.ContainsKey(quote.Tenant))
            {
                QuoteNumbers quoteNumbers = successQuotesDetails[quote.Tenant];
                successQuotesDetails[quote.Tenant] = new QuoteNumbers
                {
                    Count = quoteNumbers.Count + 1, 
                    Numbers = quoteNumbers.Numbers + "," + quote.QuoteNumber
                };
            }
            else
            {
                successQuotesDetails.Add(quote.Tenant, new QuoteNumbers { Count = 1, Numbers = quote.QuoteNumber });
            }
            successCounter = successCounter + 1;
        }

        private void AddLogMessage_Failed(Quote quote)
        {
            if (failedQuotesDetails.ContainsKey(quote.Tenant))
            {
                QuoteNumbers quoteNumbers = failedQuotesDetails[quote.Tenant];
                failedQuotesDetails[quote.Tenant] = new QuoteNumbers
                {
                    Count = quoteNumbers.Count + 1,
                    Numbers = quoteNumbers.Numbers + "," + quote.QuoteNumber
                };
            }
            else
            {
                failedQuotesDetails.Add(quote.Tenant, new QuoteNumbers { Count = 1, Numbers = quote.QuoteNumber });
            }
            failedCounter = failedCounter + 1;
        }
    }

    public struct QuoteNumbers
    {
        public int Count;
        public string Numbers;
    }
}
