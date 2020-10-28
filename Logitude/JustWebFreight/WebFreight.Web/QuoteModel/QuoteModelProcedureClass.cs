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

        public static bool IsExistsQuoteAutomaticallyClosingDataHistory(int tenant, DateTime? startDateTime)
        {
            bool isExists = false;

            string strConnString = GetConnection(tenant);

            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("select top 1 Id from QuoteAutomaticallyClosingDataHistory where Tenant = @Tenant and CONVERT(date,StartDateTime) = CONVERT(date,@StartDateTime)", cn);

                cmd.Parameters.AddWithValue("@Tenant", tenant);
                cmd.Parameters.AddWithValue("@StartDateTime", startDateTime);

                cn.Open();

                var iResult = cmd.ExecuteScalar();
                if (iResult != null)
                {
                    isExists = true;
                }

                cn.Close();
            }

            return isExists;
        }

        public static void ExecuteSingleQuoteAutomaticallyClosing(int tenant)
        {
            IQuotesContext quotesContext;
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            QuoteQuery quoteQuery;
            QuoteService quoteService;
            QuotePM quotePM;
            IQuotesContext quotesContext_Loop;
            quotesContext = QuotesContext.GetContext(tenant);
            IQueryable<Quote> allQuotes = (from d in quotesContext.Quotes
                                           where d.IsAutomaticallyClosed && !d.IsClosed && d.AutomaticallyCloseDate != null
                                           && System.Data.Entity.DbFunctions.TruncateTime(d.AutomaticallyCloseDate) <= System.Data.Entity.DbFunctions.TruncateTime(todayDate)
                                           && d.Tenant == tenant
                                           select d);

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
                quotePM.QuoteClosingReasonId = quoteClosing != null ? quoteClosing.Id: null;
                quotePM.QuoteClosingReasonCode = "XQ";
                quotePM.ActionType = "Decline";
                quoteService.Update(quotePM, true);
            }
        }

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
                quotePM = quoteQuery.GetSinglePM(item.Id, item.Tenant);
                quotePM.IsClosed = true;
                var quoteClosingReasonRepository = new QuoteClosingReasonRepository(item.Tenant);
                var quoteClosing = quoteClosingReasonRepository.GetSingleQuoteClosingReasonByCode("XQ", item.Tenant);
                quotePM.QuoteClosingReasonId = quoteClosing.Id;
                quotePM.QuoteClosingReasonCode = "XQ";
                quotePM.ActionType = "Decline";
                quoteService.Update(quotePM, true);
            }
        }

        public static void InsertQuoteAutomaticallyClosingDataHistory(int tenant, DateTime? startDateTime, DateTime? endDateTime, bool hasException, string exceptionMessage)
        {
            string strConnString = GetConnection(tenant);

            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"insert into QuoteAutomaticallyClosingDataHistory(Tenant, StartDateTime, EndDateTime, HasException, ExceptionMessage) VALUES(@Tenant, @StartDateTime, @EndDateTime, @HasException, @ExceptionMessage)";

                    cmd.Parameters.AddWithValue("@Tenant", tenant);
                    cmd.Parameters.AddWithValue("@StartDateTime", startDateTime);
                    cmd.Parameters.AddWithValue("@EndDateTime", endDateTime);
                    cmd.Parameters.AddWithValue("@HasException", hasException);
                    if (exceptionMessage == null)
                    {
                        cmd.Parameters.AddWithValue("@ExceptionMessage", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ExceptionMessage", exceptionMessage);
                    }
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
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