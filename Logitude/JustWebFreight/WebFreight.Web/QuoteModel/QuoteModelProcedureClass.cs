using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
//test
namespace WebFreight.Web.QuoteModel
{
    public class QuoteModelProcedureClass
    {
        public static void ExecuteDailyAutomaticallyClosing()
        {
            IQuotesContext quotesContext;
            IQuotesContext quotesContext_Loop;
            DateTime todayDate = DateTime.Now;
            QuoteQuery quoteQuery;
            QuoteService quoteService;
            QuotePM quotePM;

            quotesContext = QuotesContext.GetContext(0);
            IQueryable<Quote> allQuotes = (from d in quotesContext.Quotes
                                           where d.IsAutomaticallyClosed && !d.IsClosed && d.AutomaticallyCloseDate != null
                                           && System.Data.Entity.DbFunctions.TruncateTime(d.AutomaticallyCloseDate) <= System.Data.Entity.DbFunctions.TruncateTime(todayDate)
                                           select d);

            var list = allQuotes.ToList();
            foreach (Quote item in allQuotes)
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
            }
        }
    }
}