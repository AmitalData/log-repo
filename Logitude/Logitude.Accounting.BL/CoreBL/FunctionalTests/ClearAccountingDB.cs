using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.FunctionalTests
{
    public class ClearAccountingDB
    {
        public void ClearDB(int tenant)
        {
            if (tenant!= 1148)
            {
                throw new Exception("tenant!= 1148");
            }
            using (var scope = TransactionFactory.GetNewTransaction())
            {


                var accountingContext = AccountingContext.GetContext(tenant);


                (accountingContext as DbContextBase)
                    .DeleteWhere<ReconciliationLine>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
        .DeleteWhere<Reconciliation>(rec => rec.Tenant == tenant);
                (accountingContext as DbContextBase)
        .DeleteWhere<JournalReconcile>(rec => rec.Tenant == tenant);
                (accountingContext as DbContextBase)
        .DeleteWhere<ExternalReconciliationLine>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
        .DeleteWhere<ExternalReconciliation>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<LedgerTransaction>(rec => rec.Tenant == tenant);
                (accountingContext as DbContextBase)
    .DeleteWhere<JournalLine>(rec => rec.Tenant == tenant);
                (accountingContext as DbContextBase)
    .DeleteWhere<JournalMoreData>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<JournalAdditionalData>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<Journal>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<GLAccountTotalByMonth>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<ReconcileExternalPageLine>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<ReconcileExternalPage>(rec => rec.Tenant == tenant);

                
                //            (accountingContext as DbContextBase)
                //.DeleteWhere<AccountingPeriod>(rec => rec.Tenant == tenant);

                var exec = $"update GLAccountMoreDatas set BalanceInLocalCurrency=0.00,LocalBalanceInDue=0.00,NextDueDate='',TotalOpenChequesInLocalCur=0.00,TotFutureOpenChequesInLocalCur=0.00 where Tenant ={tenant}";

                CommandExecuteNonQuery(AccountingContext.GetContext(tenant).GetConnection(), exec);


                (accountingContext as DbContextBase)
    .DeleteWhere<BankDepositLine>(rec => rec.Tenant == tenant);
                (accountingContext as DbContextBase)
    .DeleteWhere<BankDeposit>(rec => rec.Tenant == tenant);



                (accountingContext as DbContextBase)
    .DeleteWhere<ARPaymentCheque>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<ARInvoicePayment>(rec => rec.Tenant == tenant);


                (accountingContext as DbContextBase)
    .DeleteWhere<ARPayment>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<ARInvoiceLine>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<APInvoicePayment>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<ARInvoiceTotalVAT>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<ARInvoice>(rec => rec.Tenant == tenant);


                (accountingContext as DbContextBase)
    .DeleteWhere<APInvoiceLine>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<APInvoiceTotalVAT>(rec => rec.Tenant == tenant);

                (accountingContext as DbContextBase)
    .DeleteWhere<APInvoice>(rec => rec.Tenant == tenant);
                scope.Complete();
            }
        }

        public static void CommandExecuteNonQuery(DbConnection conn, string cmd)
        {
            

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms != "oracle")
            {
                
                using (var cn = conn as SqlConnection)
                {
                    Debug.WriteLine($"CommandExecuteNonQuery({cmd})");


                    var command = new SqlCommand(cmd, cn);

                    cn.Open();
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            else
            {
                throw new System.Exception("Context is not  4 oracle ");
            }


        }
    }
}

