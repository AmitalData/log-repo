using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Fix
{
    public class ChangeGLAccount2IsMultiCurrencyService
    {
        public void Change2MultiCurrency(string gLAccountId,int tenant)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            var gLAccountQueryService = new GLAccountQueryService(accountingContext);
            var gLAccountUpdateService = new GLAccountUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            //var gLAccountMoreDataUpdateService = new GLAccountMoreDataUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var gLAccountPm = gLAccountQueryService.GetSingle(gLAccountId, true, false);

            if (gLAccountPm.IsMultiCurrency.GetValueOrDefault())
            {
                throw new Exception($"gLAccountId:{gLAccountId} Already Multi Currency no need to change");
            }
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tPM = tenantQuery.GetSinglePM(tenant);
            string accountingCurrencyId = tPM.CurrencyId;
            if (string.IsNullOrWhiteSpace(accountingCurrencyId))
            {
                throw new Exception($"accountingCurrencyId:{gLAccountId} is must !!");
            }
            using (var scope = TransactionFactory.GetNewTransaction( TimeSpan.FromMinutes(5)))
            {

                //IsReconciledIs0OpenAmountCurrencyId
                string resetLedgerTransaction =
                    //$"update LedgerTransactions set IsReconciled=0,OpenAmountCurrencyId=(select id from Currencies where code='NIS' and tenant = {tenant}),OpenAmount=LocalAmountDebit-LocalAmountCredit where AccountId='{gLAccountId}' and tenant = {tenant}";
                    $"update LedgerTransactions set IsReconciled=0,OpenAmountCurrencyId='{accountingCurrencyId}',OpenAmount=LocalAmountDebit-LocalAmountCredit where AccountId='{gLAccountId}' and tenant = {tenant}";
                int resetLedgerTransactionRes =  CommandExecuteNonQuery(AccountingContext.GetContext(tenant).GetConnection(), resetLedgerTransaction);


                string deleteAllRecoLines = 
                    $"delete ReconciliationLines where tenant = {tenant} and ReconciliationId in (select id from Reconciliations where tenant = {tenant} and AccountId = '{gLAccountId}')";
                CommandExecuteNonQuery(AccountingContext.GetContext(tenant).GetConnection(), deleteAllRecoLines);
                string deleteAllRecos =
                    $"delete Reconciliations where AccountId = '{gLAccountId}' and tenant = {tenant}";
                int deleteAllRecosRes =CommandExecuteNonQuery(AccountingContext.GetContext(tenant).GetConnection(), deleteAllRecos);



                



                //ReconciliationUpdateService.DeleteReconciliationsOfGLaccount(accountingContext, gLAccountId,tenant);
                ///update GLAccounts set IsMultiCurrency=1 ,CurrencyId=null,ReconcileMethodCode=0 where id='1-1544136' and tenant = 74
                gLAccountPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                string remark = $"Changed from {gLAccountPm.CurrencyCode} to Multi Currency/n updated {resetLedgerTransactionRes} transaction , deleted {deleteAllRecosRes} reconciliations";
                gLAccountPm.IsMultiCurrency = true;
                gLAccountPm.CurrencyId = null;
                gLAccountPm.CurrencyCode= null;// override fillforeign
                gLAccountPm.ReconcileMethodCode = ((int)Logitude.Accounting.Def.EntityPMs.ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString();
                
                gLAccountPm.Change2MultiCurrencyNotes = remark;
                gLAccountUpdateService.Update(gLAccountPm, true);

                //Fix BalanceinForeign & Due(GLAMoreData) - it should same as LOCAL
                string fixGLAMoreData =
                  $@"update GLAccountMoreDatas set 
BalanceInForeignCurrency = BalanceInLocalCurrency,
ForeignBalanceInDue = LocalBalanceInDue
where Tenant = {tenant} and AccountId = '{gLAccountId}' ";
                CommandExecuteNonQuery(AccountingContext.GetContext(tenant).GetConnection(), fixGLAMoreData);


                accountingContext.SaveChanges();
                scope.Complete();
            }

        }


        public static int CommandExecuteNonQuery(DbConnection conn, string cmd)
        {

            int rowsaffected = -1;
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms != "oracle")
            {

                using (var cn = conn as SqlConnection)
                {
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"CommandExecuteNonQuery({cmd})");


                    var command = new SqlCommand(cmd, cn);

                    cn.Open();
                    rowsaffected=command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            else
            {
                throw new System.Exception("Context is not  4 oracle ");
            }

            return rowsaffected;
        }
    }
}
