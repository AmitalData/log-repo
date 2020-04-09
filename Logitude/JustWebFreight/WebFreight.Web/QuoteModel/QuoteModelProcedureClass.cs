using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.Tools.EntityService;
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
        public static void ExecuteDailyJobAutomaticallyClosing()
        {
            int tenant = 0;

            string strConnString = GetConnection(tenant);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_QuotesAutomaticallyClosingDailyJob", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }

        public static void ExecuteDailyAutomaticallyClosing()
        {
            IQuotesContext quotesContext;
            IQuotesContext quotesContext_Loop;
            DateTime todayDate = DateTime.Now;
            QuoteQuery quoteQuery;
            QuoteService quoteService;
            QuotePM quotePM;
            QuoteStageRepository quoteStageRepository;
            QuoteStage quoteStage;

            quotesContext = QuotesContext.GetContext(0);
            IQueryable<Quote> allQuotes = (from d in quotesContext.Quotes
                                           where d.IsAutomaticallyClosed && !d.IsClosed && d.AutomaticallyCloseDate != null
                                           && System.Data.Entity.DbFunctions.TruncateTime(d.AutomaticallyCloseDate) == System.Data.Entity.DbFunctions.TruncateTime(todayDate)
                                           select d);

            var list = allQuotes.ToList();
            foreach (Quote item in allQuotes)
            {
                quotesContext_Loop = QuotesContext.GetContext(item.Tenant);
                quoteQuery = new QuoteQuery(item.Tenant);
                var email = "system@tenant" + item.Tenant + ".com";
                quoteService = new QuoteService(quotesContext_Loop, item.Tenant, email);
                quoteStageRepository = new QuoteStageRepository(item.Tenant);
                quoteStage = quoteStageRepository.GetSingleQuoteStageByCode("QTDC", item.Tenant);
                quotePM = quoteQuery.GetSinglePM(item.Id, item.Tenant);
                quotePM.IsClosed = true;
                quotePM.QuoteClosingReasonCode = "XQ";
                quotePM.StageId = quoteStage != null ? quoteStage.Id : null;
                quotePM.StageDueDate = CalculateStageDueDate(quoteStage, todayDate, quotePM.StageDueDate);
                quoteService.Update(quotePM, true);
            }
        }

        private static DateTime? CalculateStageDueDate(QuoteStage quoteStage, DateTime todayDate, DateTime? stageDueDate)
        {
            var dueDate = stageDueDate;
            if (quoteStage != null)
            {
                if (quoteStage.MaxDays != null)
                {
                    dueDate = todayDate.AddDays(quoteStage.MaxDays.Value);
                }
            }
            return dueDate;
        }
    }
}