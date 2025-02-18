using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace Logitude.Accounting.BL.Utils
{
    public class Redempt_a_ChequeBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private List<string> _badList;
        private int _ChequesMade = 0;
        private bool _retry;
        private bool _errors = false;

        public Redempt_a_ChequeBatch()
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
        public void RunRedempt_a_Cheque(Redempt_a_ChequeArg Redempt_a_ChequeArg)
        {
            try
            {
                int tenant = Redempt_a_ChequeArg.Tenant;
                string myARChequeId = Redempt_a_ChequeArg.ARChequeId;

                _badList = new List<string>();
                _ChequesMade = 0;


                IAccountingContext accContext = AccountingContext.GetContext(tenant);
                ARPaymentChequeQueryService aPaymentChequeQueryService = new ARPaymentChequeQueryService(accContext);


                // Check the ARCheque 
                ARPaymentChequePM aRPaymentChequePM = null;
                if (String.IsNullOrWhiteSpace(myARChequeId))
                {
                    this.AddErrorRow($"A.R.ChequeId Id is empty");
                    _errors = true;
                }
                else
                {
                    aRPaymentChequePM = aPaymentChequeQueryService.GetSingle(myARChequeId, true, false);
                    if (aRPaymentChequePM == null)
                    {
                        this.AddErrorRow($"ARPaymentCheque id={myARChequeId} is not found in tenant {tenant}");
                        _errors = true;
                    }

                    else if (aRPaymentChequePM.StatusCode != ARPaymentChequeStatusValues.InBank && aRPaymentChequePM.StatusCode != ARPaymentChequeStatusValues.InBankAccount)
                    {
                        this.AddErrorRow($"ARPaymentCheque id={myARChequeId} number={aRPaymentChequePM.ChequeNumber} is not deposited in a bank");
                        _errors = true;
                    }


                }

                if (!_errors)
                {
                    try
                    {
                        PostDatedChequesRedemptionBatch postDatedChequesRedemptionBatch = new PostDatedChequesRedemptionBatch();
                        postDatedChequesRedemptionBatch.RunOnePayableARPaymentCheque(myARChequeId, tenant);
                        _ChequesMade += 1;
                    }
                    catch (Exception e)
                    {
                        throw;
                    }

                }



                string updated = _ChequesMade > 0 ? "Cheque Redeemed, " : "";

                _ResponseText = $"{updated} Made Chequess: {_ChequesMade}, Errors/Messages: {String.Join(", \n", _badList.ToArray())}";
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

    public class Redempt_a_ChequeArg
    {
        public int Tenant { get; set; }
        public string ARChequeId { get; set; }

    }
}