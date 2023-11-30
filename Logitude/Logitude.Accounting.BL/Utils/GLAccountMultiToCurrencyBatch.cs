using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Accounting.Data.Repositories;
using System.Diagnostics;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Remoting.Contexts;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Accounting.BL.CloseTables;
using Microsoft.SqlServer.Server;

namespace Logitude.Accounting.BL.Utils
{
    public class GLAccountMultiToCurrencyBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private List<string> _badList;
        private int _AccMade = 0; 
        private int _RecosMade = 0;
        private int _TransactionsMade = 0;
        private bool _retry; 
        private bool _errors = false;

        public GLAccountMultiToCurrencyBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }
        public void RunGLAccountMultiToCurrency(GLAccountMultiToCurrencyArg gLAccountMultiToCurrencyArg)
        {
            try
            {
                int tenant = gLAccountMultiToCurrencyArg.Tenant;
                string myGLAccountId = gLAccountMultiToCurrencyArg.AccountId;
                string toCurrencyId = gLAccountMultiToCurrencyArg.ToCurrencyId;
                bool toMulti = toCurrencyId == "MULTI";

                _badList = new List<string>();
                _TransactionsMade = 0;
                BatchTaskExecutionPM batchTaskExecutionPM = gLAccountMultiToCurrencyArg.BatchTask;
                BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = null;
                if (batchTaskExecutionPM != null)
                {
                    batchTaskExecutionUpdateService = GetBatchTaskUpdateServiceInstance(tenant);
                }

                IAccountingContext accContext = AccountingContext.GetContext(tenant);
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(accContext);
                LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(accContext);
                CurrencyQueryService currencyQueryService = new CurrencyQueryService(tenant);


                //  string lastCheckedId = "";
                bool only_part_2 = false;

                // Check Account 
                GLAccountPM gLAccountPM = null;
                if (String.IsNullOrWhiteSpace(myGLAccountId))
                {
                    this.AddErrorRow($"GLAccount Id is empty");
                    _errors = true;
                }
                else
                {
                    gLAccountPM = gLAccountQueryService.GetSinglePM(myGLAccountId, tenant);
                    //  gLAccountQueryService.GetComposition(new GLAccountKeys() { Id = myGLAccountId }, gLAccountPM);
                    if (gLAccountPM == null)
                    {
                        this.AddErrorRow($"GLAccount id={gLAccountPM} is not found in tenant {tenant}");
                        _errors = true;
                    }
                    else if (gLAccountPM.IsControlAccount.HasValue && gLAccountPM.IsControlAccount.Value == true)
                    {
                        this.AddErrorRow($"GLAccount id={gLAccountPM} display={gLAccountPM.DisplayNumber} is a control account");
                        _errors = true;
                    }
                    else if (toMulti && (!gLAccountPM.IsMultiCurrency.HasValue || gLAccountPM.IsMultiCurrency.Value == false))
                    {
                        this.AddErrorRow($"GLAccount id={gLAccountPM} display={gLAccountPM.DisplayNumber} is not a multi-currency account");
                        _errors = true;
                    }
                    else if (toMulti && (gLAccountPM.IsMultiCurrency.HasValue && gLAccountPM.IsMultiCurrency.Value == true))
                    {
                        only_part_2 = true;
                    }
                    else if (!toMulti && (!gLAccountPM.IsMultiCurrency.HasValue || gLAccountPM.IsMultiCurrency.Value == false) && !String.IsNullOrEmpty(gLAccountPM.CurrencyId) && gLAccountPM.CurrencyId != toCurrencyId)
                    {
                        string code = null;
                        var accCurrency = currencyQueryService.GetCurrencyById(gLAccountPM.CurrencyId, tenant);
                        if (accCurrency == null)
                        {
                            code = gLAccountPM.CurrencyId;
                        }
                        else
                        {
                            code = accCurrency.Code;
                        }

                        this.AddErrorRow($"GLAccount id={gLAccountPM} display={gLAccountPM.DisplayNumber} is a {code} account");
                        _errors = true;
                    }
                }

                // Check Currency 
                Currency currency = null;
                if (!_errors && !toMulti)
                {
                    if (String.IsNullOrWhiteSpace(toCurrencyId))
                    {
                        this.AddErrorRow($"Currency Id is empty");
                        _errors = true;
                    }
                    else
                    {
                        currency = currencyQueryService.GetCurrencyById(toCurrencyId, tenant);
                        if (currency == null)
                        {
                            this.AddErrorRow($"Currency id={currency} is not found in tenant {tenant}");
                            _errors = true;
                        }

                    }
                }
                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM tPM = tenantQuery.GetSinglePM(tenant);
                string accountingCurrencyId = tPM.CurrencyId; 
                
                string recoMethod = null;
                if (toMulti)
                {
                    recoMethod = ReconcileMethodValues.LocalCurrency;
                }
                else
                {
                    recoMethod = toCurrencyId != accountingCurrencyId ? ReconcileMethodValues.ForeignCurrency : ReconcileMethodValues.LocalCurrency;
                }
                // Check Ledger Transactions 
                if (!_errors && !toMulti)
                {
                    bool othercurr = ledgerTransactionQueryService.CheckIfLedgerTransactionOtherCurrencyExist(myGLAccountId, toCurrencyId, tenant);
                    if (othercurr)
                    {
                        this.AddErrorRow($"GLAccount {gLAccountPM.DisplayNumber} Id={gLAccountPM} has transactions not in {currency.Code}");
                      //  _errors = true;
                        only_part_2 = true;
                        recoMethod = gLAccountPM.ReconcileMethodCode;
                    }
                }

                string strConnString = TenantServerConfigration.GetDbConnection(tenant);

                // Change Currency 
                if (!_errors && !only_part_2 && !toMulti)
                {
                    if ((!gLAccountPM.IsMultiCurrency.HasValue || gLAccountPM.IsMultiCurrency.Value == false) && gLAccountPM.CurrencyId == toCurrencyId)
                    {
                        // already changed to 'toCurrencyId'
                    }
                    else if (gLAccountPM.IsMultiCurrency.HasValue && gLAccountPM.IsMultiCurrency.Value == true)
                    {

                        using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(1)))
                        {
                            try
                            {
                                _AccMade = Update_Acc(tenant, myGLAccountId, toCurrencyId, recoMethod, strConnString);
                                scope.Complete();
                            }
                            catch (Exception e)
                            {
                                throw;
                            }
                        }
                    }


                }

                if (!_errors || only_part_2)
                {
                    // Cancel Reconciliations
                    using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(3)))
                    {
                        try
                        {
                            _RecosMade = Update_Reco(tenant, myGLAccountId, strConnString);
                            scope.Complete();
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                    if (recoMethod == ReconcileMethodValues.LocalCurrency)  
                    {
                        // Open Transactions - Local
                        using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(4)))
                        {
                            try
                            {
                                _TransactionsMade = Update_LT_Local(tenant, myGLAccountId, accountingCurrencyId, strConnString);
                                scope.Complete();
                            }
                            catch (Exception e)
                            {
                                throw;
                            }
                        }
                    }
                    else //(recoMethod == ReconcileMethodValues.ForeignCurrency) 
                    {
                        // Open Transactions - Foreign
                        using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(4)))
                        {
                            try
                            {
                                _TransactionsMade = Update_LT_Foreign(tenant, myGLAccountId, strConnString);
                                scope.Complete();
                            }
                            catch (Exception e)
                            {
                                throw;
                            }
                        }
                    }
                }



                string updated = _AccMade > 0 ? "Account Updated, " : "";

                _ResponseText = $"{updated} Made Reconciliations: {_RecosMade}, Transactions: {_TransactionsMade}, Errors/Messages: {String.Join(", \n", _badList.ToArray())}";
            }
            catch
            {
                throw;
            }
        }

        private void AddErrorRow(String errorLine)
        {
            this._badList.Add(errorLine);
        }

        private static int Update_Acc(int tenant, string accountId, string toCurrencyId, string recoMethod, string strConnString)
        {


            using (SqlConnection connection = new SqlConnection(strConnString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE GLAccounts SET IsMultiCurrency = 0, CurrencyId= @V_currency, ReconcileMethodCode= @V_recoMethod " +
                        "WHERE Tenant = @V_tenant and Id = @V_AccountId";


                    command.CommandType = CommandType.Text;

                    command.Parameters.Add("@V_tenant", SqlDbType.Int);
                    command.Parameters["@V_tenant"].Value = tenant;


                    command.Parameters.Add("@V_AccountId", SqlDbType.VarChar);
                    command.Parameters["@V_AccountId"].Value = accountId;

                    command.Parameters.Add("@V_currency", SqlDbType.VarChar);
                    command.Parameters["@V_currency"].Value = toCurrencyId;

                    command.Parameters.Add("@V_recoMethod", SqlDbType.VarChar);
                    command.Parameters["@V_recoMethod"].Value = recoMethod;

                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                    return rows;
                }
            }

        }

        private static int Update_Reco(int tenant, string accountId, string strConnString)
        {


            using (SqlConnection connection = new SqlConnection(strConnString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE Reconciliations SET IsCancelled = 1 " +
                        "WHERE Tenant = @V_tenant and AccountId = @V_AccountId and IsCancelled = 0";


                    command.CommandType = CommandType.Text;

                    command.Parameters.Add("@V_tenant", SqlDbType.Int);
                    command.Parameters["@V_tenant"].Value = tenant;


                    command.Parameters.Add("@V_AccountId", SqlDbType.VarChar);
                    command.Parameters["@V_AccountId"].Value = accountId;

                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                    return rows;
                }
            }

        }

        private static int Update_LT_Local(int tenant, string accountId, string accountingCurrencyId, string strConnString)
        {


            using (SqlConnection connection = new SqlConnection(strConnString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {

                    command.CommandText =
                        "UPDATE LedgerTransactions SET IsReconciled = 0, OpenAmountCurrencyId = @V_accountingCurrencyId, OpenAmount = LocalAmountDebit - LocalAmountCredit " +
                        "WHERE Tenant = @V_tenant and AccountId = @V_AccountId " +
                        "AND ((IsReconciled = 1) OR (OpenAmountCurrencyId <> @V_accountingCurrencyId) OR (OpenAmount <> LocalAmountDebit - LocalAmountCredit))";


                    command.CommandType = CommandType.Text;

                    command.Parameters.Add("@V_tenant", SqlDbType.Int);
                    command.Parameters["@V_tenant"].Value = tenant;

                    command.Parameters.Add("@V_AccountId", SqlDbType.VarChar);
                    command.Parameters["@V_AccountId"].Value = accountId;

                    command.Parameters.Add("@V_accountingCurrencyId", SqlDbType.VarChar);
                    command.Parameters["@V_accountingCurrencyId"].Value = accountingCurrencyId;

                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                    return rows;
                }
            }

        }

        private static int Update_LT_Foreign(int tenant, string accountId, string strConnString)
        {


            using (SqlConnection connection = new SqlConnection(strConnString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {

                    command.CommandText =
                        "UPDATE LedgerTransactions SET IsReconciled = 0, OpenAmountCurrencyId = CurrencyId, OpenAmount = ForeignAmountDebit - ForeignAmountCredit " +
                        "WHERE Tenant = @V_tenant AND AccountId = @V_AccountId " +
                        "AND ((IsReconciled = 1) OR (OpenAmountCurrencyId <> CurrencyId) OR (OpenAmount <> ForeignAmountDebit - ForeignAmountCredit))";


                    command.CommandType = CommandType.Text;

                    command.Parameters.Add("@V_tenant", SqlDbType.Int);
                    command.Parameters["@V_tenant"].Value = tenant;

                    command.Parameters.Add("@V_AccountId", SqlDbType.VarChar);
                    command.Parameters["@V_AccountId"].Value = accountId;

                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                    return rows;
                }
            }

        }

        private BatchTaskExecutionUpdateService GetBatchTaskUpdateServiceInstance(int tenant)
        {
            IInfrastructureContext context = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            return batchTaskExecutionUpdateService;
        }


    }

    public class GLAccountMultiToCurrencyArg
    {
        public int Tenant { get; set; }
        public string AccountId { get; set; }
        //public string AccountDisplayNumber { get; set; }
        public string ToCurrencyId { get; set; }
        //public string ToCurrencyCode { get; set; }
        public bool Batch { get; set; }
        public BatchTaskExecutionPM BatchTask { get; set; }
    }
}