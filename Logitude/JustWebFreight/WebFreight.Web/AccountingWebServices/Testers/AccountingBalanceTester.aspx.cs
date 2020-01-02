//test task 46490!!
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Devart.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Logitude.Accounting.BL.CoreBL.Dashboard;
//using Logitude.Accounting.BL.CoreBL.BuildTenant;
using Logitude.Accounting.Data.EntityPOCOs;
using System.Diagnostics;
using Logitude.Accounting.BL.CoreBL.BuildTenant;
using static Logitude.Accounting.Data.EntityListQueryServices.ARPaymentChequeListQueryService;
using System.Configuration;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.CoreBL.BankAccountPages;
using Logitude.Accounting.BL;
using Logitude.Accounting.BL.CoreBL.FunctionalTests;
using Logitude.Accounting.BL.CoreBL.Batch;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Validators;
//using Logitude.Accounting.BL.CoreBL.ReverseEngineer;

namespace WebFreight.Web.AccountingWebServices.Testers
{
    public partial class AccountingBalanceTester : System.Web.UI.Page
    {
        public enum MyLastAction
        {
            none,
            _ButtonReverseTotal_Click,
            _ButtonReverseTrans_Click,
            _ButtonJournalApprove_Click,
            _ButtonJournalApproveQueue_Click,
            _ButtonCreateNewJournal_Click,
            _ButtonAging_Click,
            _ButtonLedgerTransactionBalance_Click,
            _ButtonCheckBalance_Click,
            _ButtonGetReconcile_Click,
            _ButtonReverseGLBalance_Click,
            _ButtonCurrBalanceByType_Click,
            _ButtonTreeMapCOA_Click,
            _ButtonDueLocalBalance_Click,
            _ButtonReverseDueDate_Click,
            _ButtonLedgerTransactionCardIndex_Click,
            _ButtonYearTransfer_Click,
            _ButtonSysCheckTotalSumIsZero_Click,
            _ButtonSysCheckGLAccJL2Total_Click,
            _ButtonBuildTenant_Click,
            _ButtonSysCheckLdegerTransSumIsZero_Click,
            _ButtonARPaymentCheque_Click,
            _WorkWithoutQueueStatus4_Click,
            _ButtonIsApprovedJournalTOTZero_Click,
            _ButtonReverseTotalFIX_Click,
            _ButtonReverseGLBalanceFIX_Click,
            _AccountingIntegrityService_Click,
            _ButtonBalanceByCollector_Click,
            _ButtonLoadBankPages_Click,
            _ButtonGetSystem1000_Click,
            _ButtonLoadSystem1000_Click,
            _ButtonYearTransferCancel_Click,
            _ButtonExternalReconcile_click,
            _ButtonCardIndexNew_Click
        }

        //DateTime _MyDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            LogMessagingUtil.Instance.Clear();
            //var testIt = new AccountingModel.WebInjection.XLSUtil();
            //testIt.GetDataTableFromWorkSheet(null,"");
            //var s = new ClearAccountingDB();
            //s.ClearDB(1148);
            //var a = new ReconcileOpenAmountService();
            //var l = a.GetLedgerOpenAmountDiff(69, 2019);

            //ExternalReconcileAdjustBankFees();
            //var myWorker = new JournalApproveService.JournalApproveWorker();
            //myWorker.CreateBatchAccountingIntegrityCheck();
            try
            {



                //AuthenticationUtil.Impersonate(1, 
                //    //"jalal@mail.com", "jalal"
                //    "admin@fnarsoft.com", "admin@fnarsoft.com"
                //    );

                if (ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString == "Logitude2-5_Global,sa,Saas256,.")
                //if (Environment.MachineName.ToUpper().Contains("ITZIK"))
                {
                    AuthenticationUtil.Impersonate(1,
                   //"jalal@mail.com", "jalal"
                   "angular@fnarsoft.com", "angular@fnarsoft.com"
                   );

                }
                else
                {
                    AuthenticationUtil.Impersonate(989,
                        //"jalal@mail.com", "jalal"
                        "basel@amital.co.il", "basel@amital.co.il"
                        );

                }
            }
            catch (Exception)
            {

            }
            ///CreateJournalReconcile();

            if (!Page.IsPostBack)
            {
                //Expression<Func<TEntity, TEntity>> evaluator = new { Test="222", ff=DateTime.Now };
                //Expression<Func<class, TEntity>> evaluator = new { Test = "222", ff = DateTime.Now };

                //var r = new AutomaticReconcileService();
                //r.AutomaticReconcile("1-33", 1);
                //_MyDate = DateTime.Now;
                //_TextBoxDate.Text = "0";
                //_LabelDate.Text = _MyDate.ToString();
            }

        }

        private void ExternalReconcileAdjustBankFees()
        {
            var accountingContext = AccountingContext.GetContext(1071);
            IExternalReconcileDataProvider externalReconcileDataProvider = new ExternalReconcileDataProvider(accountingContext);
            var a = new ExternalReconcileAdjustBankFeesService();
            a.MustInit(externalReconcileDataProvider);
            a.CreateJournalWithExtReconcile(1071, new List<string>() { "1-12487" }, "1-216674", "Notes ",DateTime.Now);
            var aa = a.TheNewJournal;

            var us = new JournalUpdateService(AccountingContext.GetContext(a.TheNewJournal.Tenant), new Dictionary<string, IContext>(), a.TheNewJournal.Tenant);
            us.Update(a.TheNewJournal, true);
            _LabelResult.Text = JsonConvert.SerializeObject(a.TheNewJournal); ;

        }

        private void CreateJournalReconcile()
        {
            var jPM = new JournalPM()
            {
                //CreateDate= DateTime.Now()
            };
        }

        public static string GetHost()
        {

            if (Environment.CommandLine.ToLower().Contains("w3wp.exe")) return LogitudeSettings.LogitudeURL;
            return "";
        }

        protected void _ButtonCheckBalance_Click(object sender, EventArgs e)
        {
            AccountBalanceParam param = null;
            AccountBalanceParam paramDefault = new AccountBalanceParam()
            {
                Tenant = 989,
                accoutingDate = DateTime.Now.AddMonths(-1),
                GLAccountId = "1-1",
                verbose = true
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonCheckBalance_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = LogitudeXmlSerializer.DeserializeObject<AccountBalanceParam>(_TextBoxParam.Text);


                var ac = new AccountBalanceByDateCodeService(null, param.Tenant, param.GLAccountId, null);
                ac.ReSetAccountList(param.IncludeChildAccounts, param.IncludeRelatedCurrenciesAccount);
                
                ac.CalculateBalance(param.OpenBalancePlease_ReCalcYearTransfer,  GLAccountTotalDateTypeValues.Accountingdate, param.accoutingDate,
                    false,
                    param.includeAccoutingDateLTransaction,
                    
                    param.verbose);
                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<CurrencySum>>(ac.AccountBalance.Totals);
                //ac.AccountBalance.Totals

                ReloadGrid(SerializeObjectByte);
                ac.AccountBalance.Totals = null;

                var SerializeObjectByte1 = LogitudeXmlSerializer.SerializeObject<AccountBalanceM>(ac.AccountBalance);
                _LabelResult.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByte1);

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonCheckBalance_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<AccountBalanceParam>(param);
                _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }




        }

        protected void _ButtonCurrBalanceByType_Click(object sender, EventArgs e)
        {

            ParamBasic param = null;
            ParamBasic paramDefault = new ParamBasic()
            {
                MyTenant = 989,
                MyDate = DateTime.Now.AddMonths(-1),
                MyGLAccId = "1-1",

            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonCurrBalanceByType_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = LogitudeXmlSerializer.DeserializeObject<ParamBasic>(_TextBoxParam.Text);
                var myAllCardServiceDS = new GLAccountDashboard();
                var dic = myAllCardServiceDS.GetCardsLocalBalanceGByChartOfAccountsTypeCode(param.MyTenant,true,null);

                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<ChartOfAccountBalanceM>>(dic);
                ReloadGrid(SerializeObjectByte);

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonCurrBalanceByType_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<ParamBasic>(param);
                _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }

        protected void _ButtonReverseGLBalance_Click(object sender, EventArgs e)
        {

            ParamBasic param = null;
            ParamBasic paramDefault = new ParamBasic()
            {
                MyTenant = 989,
                MyDate = DateTime.Now.AddMonths(-1),
                MyGLAccId = "1-1",

            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonReverseGLBalance_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = LogitudeXmlSerializer.DeserializeObject<ParamBasic>(_TextBoxParam.Text);
                var s = new ReverseEngineerGLAccountBalance(/*param.MyDate,*/ param.MyTenant);
                s.CheckDbIntegrity();

                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<GLAccountBalanceDTO>>(s.CompareReport.GLAccountBalanceList);
                ReloadGrid(SerializeObjectByte);

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonReverseGLBalance_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<ParamBasic>(param);
                _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }


        protected void _AccountingIntegrityService_Click(object sender, EventArgs e)
        {
            AccountingIntegrityInParam param = null;
            var paramDefault = new AccountingIntegrityInParam()
            {
                Tenant = 1064,
                FromMonthInclusive = DateTime.Now.AddMonths(-3),
                ToMonthInclusive = DateTime.Now,

            };
            string serializeObjectstring = "";
            try
            {

                if (GetMyLastAction() != MyLastAction._AccountingIntegrityService_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject<AccountingIntegrityInParam>(_TextBoxParam.Text);
                

                var accountingIntegrityService = new AccountingIntegrityService();

                string errorMessage = accountingIntegrityService.CheckParams(param);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                var res = accountingIntegrityService.CheckIntegrity(param);

                serializeObjectstring = JsonConvert.SerializeObject(res);



            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._AccountingIntegrityService_Click.ToString();
                if (param == null)
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(paramDefault);
                }
                else
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(param);
                }

                _LabelLog.Text = serializeObjectstring ?? LogMessagingUtil.Instance.ToString();
            }
        }

        protected void _ButtonReverseGLBalanceFIX_Click(object sender, EventArgs e)
        {
            dynamic param = null;
            var paramDefault = new
            {
                MyTenant = 989,
                MyDate = DateTime.Now.AddMonths(-1),
                MyGLAccId = "1-1",
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonReverseGLBalanceFIX_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);

                var MyTenant = (int)param.MyTenant;
                var MyDate = (DateTime)param.MyDate;

                var s = new ReverseEngineerGLAccountBalance(/*MyDate,*/ MyTenant);
                s.FIXCheckDbIntegrity();

                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<GLAccountBalanceDTO>>(s.CompareReport.GLAccountBalanceList);
                ReloadGrid(SerializeObjectByte);


            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonReverseGLBalanceFIX_Click.ToString();
                if (param == null)
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(paramDefault);
                }
                else
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(param);
                }

                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }

        protected void _ButtonReverseTotalFIX_Click(object sender, EventArgs e)
        {

            ParamBasic param = null;
            ParamBasic paramDefault = new ParamBasic()
            {
                MyTenant = 989,
                MyDate = DateTime.Now.AddMonths(-1),
                MyGLAccId = "1-131321",

            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonReverseTotalFIX_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = LogitudeXmlSerializer.DeserializeObject<ParamBasic>(_TextBoxParam.Text);
                var s = new ReverseEngineerTotalByMonthService(param.MyDate, param.MyTenant, param.MyGLAccId);
                s.FixDbIntegrityFromLedgeToTotal();

                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<GLAccountTotalByMonthsDTO>>(s.CompareReport.GLAccountTotalByMonthsList);
                ReloadGrid(SerializeObjectByte);

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonReverseTotalFIX_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<ParamBasic>(param);
                _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }
        protected void _ButtonReverseTotal_Click(object sender, EventArgs e)
        {

            ParamBasic param = null;
            ParamBasic paramDefault = new ParamBasic()
            {
                MyTenant = 989,
                MyDate = DateTime.Now.AddMonths(-1),
                MyGLAccId = "1-131321",

            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonReverseTotal_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = LogitudeXmlSerializer.DeserializeObject<ParamBasic>(_TextBoxParam.Text);
                var s = new ReverseEngineerTotalByMonthService(param.MyDate, param.MyTenant, param.MyGLAccId);
                s.CheckDbIntegrity();

                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<GLAccountTotalByMonthsDTO>>(s.CompareReport.GLAccountTotalByMonthsList);
                ReloadGrid(SerializeObjectByte);

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonReverseTotal_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<ParamBasic>(param);
                _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }

        protected void _ButtonReverseTrans_Click(object sender, EventArgs e)
        {





            ParamBasic param = null;
            ParamBasic paramDefault = new ParamBasic()
            {
                MyTenant = 989,
                MyDate = DateTime.Now.AddMonths(-1),
                MyGLAccId = "1-1",

            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonReverseTrans_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = LogitudeXmlSerializer.DeserializeObject<ParamBasic>(_TextBoxParam.Text);
                var s = new ReverseEngineerLedgerTransactionService(param.MyDate, param.MyTenant);
                s.CheckDbIntegrity();



                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<JournalLineLedgerDTO>>(s.CompareReport.rows);

                //_LabelHaveApproveJournal.Text = ac.AccountBalance.HaveAccountingQueued.ToString(); 
                ReloadGrid(SerializeObjectByte);

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonReverseTrans_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<ParamBasic>(param);
                _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);

            }
        }

        private void ReloadGrid(byte[] SerializeObjectByte)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(new MemoryStream(SerializeObjectByte));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds.Tables[0].DefaultView;


            }
            else
            {
                GridView1.DataSource = null;
            }
            _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            GridView1.DataBind();
        }


        static QueueClient _QueueClient;
        protected void _ButtonJournalApproveQueue_Click(object sender, EventArgs e)
        {
            var myWorker = new JournalApproveService.JournalApproveWorker();
            var sw = Stopwatch.StartNew();
            myWorker.WorkUntilQEmptyQueueDB();
            sw.Stop();
            _LabelLog.Text = $"Tot:{sw.Elapsed}" + LogMessagingUtil.Instance.ToString();
        }
        public string GetDefaultJornalPM()
        {
            var param = JournalTesterClass.GetDefaultJornalPM(DateTime.Now);
            return JsonConvert.SerializeObject(param);
        }

        public string GetDefaultGLAccountPM()
        {
            var repo = new GLAccountRepository(989);
            var pm = repo.GetAll(989).FirstOrDefault();
            var param = new GLAccountPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = 989,
                DisplayNumber = "1122",
                EnglishName = "GLAccount1",

            };
            return JsonConvert.SerializeObject(pm);
        }
        protected void _ButtonCreateNewJournal_Click(object sender, EventArgs e)
        {

            Logitude.Accounting.Def.EntityPMs.JournalPM paramDefault = null;
            Logitude.Accounting.Def.EntityPMs.JournalPM param = JournalTesterClass.GetDefaultJornalPM(DateTime.Now);
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonCreateNewJournal_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                //param = LogitudeXmlSerializer.DeserializeObject<JournalPM>(_TextBoxParam.Text);
                //var myJournalsController = new JournalsController()

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonCreateNewJournal_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                //var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<JournalPM>(param);
                //_TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
                _TextBoxParam.Text = JsonConvert.SerializeObject(param);
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }


#if true
        protected void _ButtonBalanceByCollector_Click(object sender, EventArgs e)
        {
            BalanceGroupByCollectorReportParam myCollectorReportParam = null;
            var myAgingReportParamDefault = new BalanceGroupByCollectorReportParam()
            {

                //ChartOfAccountIdV1NotInUse = "",
                VendorCustomerId = "",
                Tenant = 1064,
                //CurrenciesDetailedV1NotInUse = null,
                CollectorId = "1-81443",
                GroupByDate = AgingReportParam.DateEnum.DueDate,
                GroupByDate_Options = Enum.GetNames(typeof(AgingReportParam.DateEnum)).ToList().Aggregate((b4, aftr) => string.Concat(b4, ";", aftr)),

                AccountTypeCode = AgingReportParam.Aging4AccountTypeCodeEnum.Customer2,
                AccountTypeCode_Options = "Customer2;Vendor3",
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonBalanceByCollector_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    //   myAgingReportParam = UpdateDefaultLedgerTransBalance(myAgingReportParam);
                    return;
                }
                myCollectorReportParam = LogitudeXmlSerializer.DeserializeObject<BalanceGroupByCollectorReportParam>(_TextBoxParam.Text);

                var collectorReport = new BalanceGroupByCollectorService(myCollectorReportParam);
                var res = collectorReport.RunReport();

                var xmlMyPeriodList = LogitudeXmlSerializer.SerializeObjectToXmlString(res);
                _LabelResult.Text = xmlMyPeriodList;

                ReloadGrid(System.Text.Encoding.UTF8.GetBytes(xmlMyPeriodList));
            }
            catch
            {
                myCollectorReportParam = null;
                throw;
            }
            finally
            {

                _MyLastAction.Value = MyLastAction._ButtonBalanceByCollector_Click.ToString();
                if (myCollectorReportParam == null)
                {
                    myCollectorReportParam = myAgingReportParamDefault;
                }
                var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<BalanceGroupByCollectorReportParam>(myCollectorReportParam);
                _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
                //_LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }

        }


#endif
        protected void _ButtonAging_Click(object sender, EventArgs e)
        {
            AgingReportParam myAgingReportParam = null;
            var myAgingReportParamDefault = new AgingReportParam()
            {
                AgingForDate = DateTime.Now,
                NumberOfmonthsbackwards = 6,
                //ChartOfAccountIdV1NotInUse = "",
                VendorCustomerId = "1-1",
                Tenant = 989,
                //CurrenciesDetailedV1NotInUse = null,
                Category1Id = "",
                Category2Id = "",
                Category3Id = "",
                Category4Id = "",
                Category5Id = "",
                CollectorId = "",
                SalesmanId = "",
                AgingMethod = AgingReportParam.MethodEnum.TotalByMonthMethod.ToString(),
                AgingMethod_Options = Enum.GetNames(typeof(AgingReportParam.MethodEnum)).ToList().Aggregate((b4, aftr) => string.Concat(b4, ";", aftr)),
                GroupByDate = AgingReportParam.DateEnum.DueDate,
                GroupByDate_Options = Enum.GetNames(typeof(AgingReportParam.DateEnum)).ToList().Aggregate((b4, aftr) => string.Concat(b4, ";", aftr)),

                Aging4AccountTypeCode = AgingReportParam.Aging4AccountTypeCodeEnum.Customer2,
                Aging4AccountTypeCode_Options = Enum.GetNames(typeof(AgingReportParam.Aging4AccountTypeCodeEnum)).ToList().Aggregate((b4, aftr) => string.Concat(b4, ";", aftr)),
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonAging_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    //   myAgingReportParam = UpdateDefaultLedgerTransBalance(myAgingReportParam);
                    return;
                }
                myAgingReportParam = LogitudeXmlSerializer.DeserializeObject<AgingReportParam>(_TextBoxParam.Text);

                var agingReport = new AgingReportService(myAgingReportParam);
                var xml = agingReport.RunReport();
                var MyPeriodList = agingReport.MyPeriodList;
                var xmlMyPeriodList = LogitudeXmlSerializer.SerializeObjectToXmlString(agingReport.MyPeriodList);
                _LabelResult.Text = xmlMyPeriodList;

                ReloadGrid(System.Text.Encoding.UTF8.GetBytes(xml));
            }
            catch
            {
                myAgingReportParam = null;
                throw;
            }
            finally
            {

                _MyLastAction.Value = MyLastAction._ButtonAging_Click.ToString();
                if (myAgingReportParam == null)
                {
                    myAgingReportParam = myAgingReportParamDefault;
                }
                var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<AgingReportParam>(myAgingReportParam);
                _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
                //_LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }

        }

        public MyLastAction GetMyLastAction()
        {
            _LabelResult.Text = "";
            MyLastAction myLastAction = MyLastAction.none;
            Enum.TryParse<MyLastAction>(_MyLastAction.Value, out myLastAction);
            return myLastAction;
        }

        protected void _ButtonLedgerTransactionBalance_Click(object sender, EventArgs e)
        {
            LedgerTransactionBalanceFilter myLedgerTransactionBalanceFilter = null;

            if (GetMyLastAction() != MyLastAction._ButtonLedgerTransactionBalance_Click)
            {
                myLedgerTransactionBalanceFilter = UpdateDefaultLedgerTransBalance(myLedgerTransactionBalanceFilter);
                return;
            }
            if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
            {
                myLedgerTransactionBalanceFilter = UpdateDefaultLedgerTransBalance(myLedgerTransactionBalanceFilter);
                return;
            }
            myLedgerTransactionBalanceFilter =
                LogitudeXmlSerializer.DeserializeObject<LedgerTransactionBalanceFilter>(_TextBoxParam.Text);

            _MyLastAction.Value = MyLastAction._ButtonLedgerTransactionBalance_Click.ToString();
            var accountingContext = AccountingContext.GetContext(myLedgerTransactionBalanceFilter.Tenant);
            var ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext, myLedgerTransactionBalanceFilter);
            ledgerTransactionBalanceService.Run();

            //_LabelResult.Text = "ledgerTransactionBalance" +ledgerTransactionBalanceService.Response.StartBalanceLocal + " " + ledgerTransactionBalanceService.Response.EndBalanceLocal;

            myLedgerTransactionBalanceFilter.CallBack = new LedgerTransactionBalanceFilterCallBack()
            {
                //EndBalanceForeign = ledgerTransactionBalanceService.Response.EndBalanceForeign,
                StartBalanceForeignList = ledgerTransactionBalanceService.Response.StartBalanceForeignList,
                EndBalanceLocal = ledgerTransactionBalanceService.Response.EndBalanceLocal,
                Have1CurrencyIdInPeriod = ledgerTransactionBalanceService.Response.Have1CurrencyIdInPeriod,
                MaxCreateAt = ledgerTransactionBalanceService.Response.MaxCreateAt,

                //StartBalanceForeign = ledgerTransactionBalanceService.Response.StartBalanceForeign,
                EndBalanceForeignList = ledgerTransactionBalanceService.Response.EndBalanceForeignList,
                AllIdAccounts = ledgerTransactionBalanceService.Response.AllIdAccounts,
                OpenBalanceForYearInLocalCurrency = ledgerTransactionBalanceService.Response.OpenBalanceForYearInLocalCurrency,
                HaveAccountingQueued = ledgerTransactionBalanceService.Response.HaveAccountingQueued,
                StartBalanceLocal = ledgerTransactionBalanceService.Response.StartBalanceLocal,
                TotalRowCount = ledgerTransactionBalanceService.Response.TotalRowCount,
                YearTransferLedgerTransactionIds = ledgerTransactionBalanceService.Response.YearTransferLedgerTransactionIds,
                SearchFields = ledgerTransactionBalanceService.Response.SearchFields,
                OmitAllBalance = ledgerTransactionBalanceService.Response.OmitAllBalance,
                //BeginOfYearLocalAmountBalance = ledgerTransactionBalanceService.Response.BeginOfYearLocalAmountBalance,
                SuppressCumulativeDueMultiCurrencyInPeriod = ledgerTransactionBalanceService.Response.SuppressCumulativeDueMultiCurrencyInPeriod
            };
            var SerializeObjectByteParam2 = LogitudeXmlSerializer.SerializeObject<LedgerTransactionBalanceFilter>(myLedgerTransactionBalanceFilter);
            _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam2);

            var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<LedgerTransactionList>>(ledgerTransactionBalanceService.Response.MyLedgerTransactionList);
            ReloadGrid(SerializeObjectByte);
            _LabelLog.Text = LogMessagingUtil.Instance.ToString();

        }



        protected void _ButtonLedgerTransactionCardIndex_Click(object sender, EventArgs e)
        {
            LedgerTransactionCardIndexFilter myLedgerTransactionCardIndexFilter = null;

            if (GetMyLastAction() != MyLastAction._ButtonLedgerTransactionCardIndex_Click)
            {
                myLedgerTransactionCardIndexFilter = UpdateDefaultLedgerTransCardIndex(myLedgerTransactionCardIndexFilter);
                return;
            }
            if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
            {
                myLedgerTransactionCardIndexFilter = UpdateDefaultLedgerTransCardIndex(myLedgerTransactionCardIndexFilter);
                return;
            }
            myLedgerTransactionCardIndexFilter =
                LogitudeXmlSerializer.DeserializeObject<LedgerTransactionCardIndexFilter>(_TextBoxParam.Text);

            _MyLastAction.Value = MyLastAction._ButtonLedgerTransactionCardIndex_Click.ToString();
            var accountingContext = AccountingContext.GetContext(myLedgerTransactionCardIndexFilter.Tenant);
            var ledgerTransactionCardIndexService = new LedgerTransactionCardIndexService(accountingContext, myLedgerTransactionCardIndexFilter);
            ledgerTransactionCardIndexService.Run();

            //_LabelResult.Text = "ledgerTransactionCardIndex" +ledgerTransactionCardIndexService.Response.StartCardIndexLocal + " " + ledgerTransactionCardIndexService.Response.EndCardIndexLocal;

            myLedgerTransactionCardIndexFilter.CallBack = new LedgerTransactionCardIndexFilterCallBack()
            {
                //EndCardIndexForeign = ledgerTransactionCardIndexService.Response.EndCardIndexForeign,
                //  StartCardIndexForeignList = ledgerTransactionCardIndexService.Response.StartCardIndexForeignList,
                //   EndCardIndexLocal = ledgerTransactionCardIndexService.Response.EndCardIndexLocal,
                //   Have1CurrencyIdInPeriod = ledgerTransactionCardIndexService.Response.Have1CurrencyIdInPeriod,
                //   MaxCreateAt = ledgerTransactionCardIndexService.Response.MaxCreateAt,

                //StartCardIndexForeign = ledgerTransactionCardIndexService.Response.StartCardIndexForeign,
                //   EndCardIndexForeignList = ledgerTransactionCardIndexService.Response.EndCardIndexForeignList,
                AllIdAccounts = ledgerTransactionCardIndexService.Response.AllIdAccounts,
                //    OpenCardIndexForYearInLocalCurrency = ledgerTransactionCardIndexService.Response.OpenCardIndexForYearInLocalCurrency,
                HaveAccountingQueued = ledgerTransactionCardIndexService.Response.HaveAccountingQueued,
                //    StartCardIndexLocal = ledgerTransactionCardIndexService.Response.StartCardIndexLocal,
                TotalRowCount = ledgerTransactionCardIndexService.Response.TotalRowCount,
                SearchFields = ledgerTransactionCardIndexService.Response.SearchFields,
                OmitAllCardIndex = ledgerTransactionCardIndexService.Response.OmitAllCardIndex,
                //BeginOfYearLocalAmountCardIndex = ledgerTransactionCardIndexService.Response.BeginOfYearLocalAmountCardIndex,
                //   SuppressCumulativeDueMultiCurrencyInPeriod = ledgerTransactionCardIndexService.Response.SuppressCumulativeDueMultiCurrencyInPeriod
            };
            var SerializeObjectByteParam2 = LogitudeXmlSerializer.SerializeObject<LedgerTransactionCardIndexFilter>(myLedgerTransactionCardIndexFilter);
            _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam2);

            var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<LedgerTransactionList>>(ledgerTransactionCardIndexService.Response.MyLedgerTransactionList);
            ReloadGrid(SerializeObjectByte);
            _LabelLog.Text = LogMessagingUtil.Instance.ToString();

        }


        protected void _ButtonARPaymentCheque_Click(object sender, EventArgs e)
        {
            ARPaymentChequeFilter myARPaymentChequeFilter = null;

            if (GetMyLastAction() != MyLastAction._ButtonARPaymentCheque_Click)
            {
                myARPaymentChequeFilter = UpdateDefaultLedgerTransCardIndex(myARPaymentChequeFilter);
                return;
            }
            if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
            {
                myARPaymentChequeFilter = UpdateDefaultLedgerTransCardIndex(myARPaymentChequeFilter);
                return;
            }
            myARPaymentChequeFilter =
                LogitudeXmlSerializer.DeserializeObject<ARPaymentChequeFilter>(_TextBoxParam.Text);

            _MyLastAction.Value = MyLastAction._ButtonARPaymentCheque_Click.ToString();
            var accountingContext = AccountingContext.GetContext(myARPaymentChequeFilter.Tenant);
            var aRPaymentChequeRedemptionService = new ARPaymentChequeRedemptionService(accountingContext, myARPaymentChequeFilter);
            aRPaymentChequeRedemptionService.Run();

            //_LabelResult.Text = "aRPaymentCheque" +aRPaymentChequeService.Response.StartCardIndexLocal + " " + aRPaymentChequeService.Response.EndCardIndexLocal;

            myARPaymentChequeFilter.CallBack = new ARPaymentChequeFilterCallBack()
            {
                //EndCardIndexForeign = aRPaymentChequeService.Response.EndCardIndexForeign,
                //  StartCardIndexForeignList = aRPaymentChequeService.Response.StartCardIndexForeignList,
                //   EndCardIndexLocal = aRPaymentChequeService.Response.EndCardIndexLocal,
                //   Have1CurrencyIdInPeriod = aRPaymentChequeService.Response.Have1CurrencyIdInPeriod,
                //   MaxCreateAt = aRPaymentChequeService.Response.MaxCreateAt,

                //StartCardIndexForeign = aRPaymentChequeService.Response.StartCardIndexForeign,
                //   EndCardIndexForeignList = aRPaymentChequeService.Response.EndCardIndexForeignList,
                AllIdCheques = aRPaymentChequeRedemptionService.Response.AllIdCheques,
                //    OpenCardIndexForYearInLocalCurrency = aRPaymentChequeService.Response.OpenCardIndexForYearInLocalCurrency,
                //    StartCardIndexLocal = aRPaymentChequeService.Response.StartCardIndexLocal,
                TotalRowCount = aRPaymentChequeRedemptionService.Response.TotalRowCount,
                SearchFields = aRPaymentChequeRedemptionService.Response.SearchFields,
                OmitAllCheque = aRPaymentChequeRedemptionService.Response.OmitAllCheque,
                //BeginOfYearLocalAmountCardIndex = aRPaymentChequeService.Response.BeginOfYearLocalAmountCardIndex,
                //   SuppressCumulativeDueMultiCurrencyInPeriod = aRPaymentChequeService.Response.SuppressCumulativeDueMultiCurrencyInPeriod
            };
            var SerializeObjectByteParam2 = LogitudeXmlSerializer.SerializeObject<ARPaymentChequeFilter>(myARPaymentChequeFilter);
            _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam2);

            var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<ARPaymentChequeList>>(aRPaymentChequeRedemptionService.Response.MyARPaymentChequeList);
            ReloadGrid(SerializeObjectByte);
            _LabelLog.Text = LogMessagingUtil.Instance.ToString();

        }


        private ARPaymentChequeFilter UpdateDefaultLedgerTransCardIndex(ARPaymentChequeFilter myARPaymentChequeFilter)
        {
            _MyLastAction.Value = MyLastAction._ButtonARPaymentCheque_Click.ToString();
            myARPaymentChequeFilter = new ARPaymentChequeFilter()
            {
                Tenant = 1,
                From = DateTime.Now.AddMonths(-1),
                To = DateTime.Now,
                CurrencyId = (new AccountingSettingResolver()).ResolveAccountingCurrencyId(1),

                SearchFields = "",
                PageStartAtRecordIndex = 0,
                PageSize = 100,
                CallBack = new ARPaymentChequeFilterCallBack()
                {
                    TotalRowCount = 0,
                }

            };
            var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<ARPaymentChequeFilter>(myARPaymentChequeFilter);
            _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
            return myARPaymentChequeFilter;
        }



        private LedgerTransactionBalanceFilter UpdateDefaultLedgerTransBalance(LedgerTransactionBalanceFilter myLedgerTransactionBalanceFilter)
        {
            _MyLastAction.Value = MyLastAction._ButtonLedgerTransactionBalance_Click.ToString();
            myLedgerTransactionBalanceFilter = new LedgerTransactionBalanceFilter()
            {
                Tenant = 989,
                From = DateTime.Now.AddMonths(-1),
                To = DateTime.Now,
                CurrencyId = (new AccountingSettingResolver()).ResolveAccountingCurrencyId(1),
                DateTypeCode = "1",
                GLAccountId = "1-1",

                SearchFields = "",
                PageStartAtRecordIndex = 0,
                PageSize = 100,
                CallBack = new LedgerTransactionBalanceFilterCallBack()
                {

                }

            };
            var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<LedgerTransactionBalanceFilter>(myLedgerTransactionBalanceFilter);
            _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
            return myLedgerTransactionBalanceFilter;
        }


        private LedgerTransactionCardIndexFilter UpdateDefaultLedgerTransCardIndex(LedgerTransactionCardIndexFilter myLedgerTransactionCardIndexFilter)
        {
            _MyLastAction.Value = MyLastAction._ButtonLedgerTransactionCardIndex_Click.ToString();
            myLedgerTransactionCardIndexFilter = new LedgerTransactionCardIndexFilter()
            {
                Tenant = 1,
                From = DateTime.Now.AddMonths(-1),
                To = DateTime.Now,
                CurrencyId = (new AccountingSettingResolver()).ResolveAccountingCurrencyId(1),

                GLAccountId = "1-1",
                Category1Id = "1-12",
                Category2Id = "",
                Category3Id = "",
                Category4Id = "",
                Category5Id = "",
                AccountTypeCode = "",
                ChartOfAccountsId = "1-11",
                DateTypeCode = "1",
                SearchFields = "",
                IncludeChildAccounts = true,
                IsReconciled = false,
                PageStartAtRecordIndex = 0,
                PageSize = 100,
                CallBack = new LedgerTransactionCardIndexFilterCallBack()
                {
                    TotalRowCount = 0,
                }

            };
            var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<LedgerTransactionCardIndexFilter>(myLedgerTransactionCardIndexFilter);
            _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
            return myLedgerTransactionCardIndexFilter;
        }



        protected void _ButtonCreateRandomJournal_Click(object sender, EventArgs e)
        {

            var sb = new StringBuilder();
            var tester = new JournalTesterClass();
            for (int i = 0; i < 1000; i++)
            {
                sb.Append(tester.InsertRandomJournal()).Append(",");
            }
            _LabelResult.Text = sb.ToString();
        }

        protected void _ButtonGetReconcile_Click(object sender, EventArgs e)
        {

            var queryOperations = new QueryOperations()
            {
                ObjectTableName = "LedgerTransaction",
                PageIndex = 0,
                PageSize = 10,
                QuerySection = "LedgerTransaction",
                SortByColumnName = "AccountingDate",
                SortDirectin = "Descending",

            };
            queryOperations.SetFilter("AccountId", "1-1", false, "Equals", null, false);
            queryOperations.SetFilter("AccountingDate", null, false, "Contains", null, false);
            //queryOperations.SetFilter("IsOpen", true, true, "Equals", null, false);
            queryOperations.SetFilter("MaxCreateAt", null, true, "LessThanOrEqual", null, false);



            //ReconcileParam param = null;
            //ReconcileParam paramDefault = new ReconcileParam()
            //{
            //    Tenant = 989,
            //    accoutingDate = DateTime.Now.AddMonths(-1),
            //    GLAccountId = "1-1",
            //    verbose = true
            //};
            //try
            //{

            //    if (GetMyLastAction() != MyLastAction._ButtonCheckBalance_Click)
            //    {
            //        return;
            //    }
            //    if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
            //    {
            //        return;
            //    }

            //    param = LogitudeXmlSerializer.DeserializeObject<AccountBalanceParam>(_TextBoxParam.Text);

            //    var ac = new AccountBalanceService(param.Tenant, param.GLAccountId, param.IncludeChildAccounts, param.IncludeRelatedCurrenciesAccount);
            //    ac.GetBalance(param.accoutingDate, param.includeAccoutingDateLTransaction, param.verbose);
            //    var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<CurrencySum>>(ac.AccountBalance.Totals);
            //    //ac.AccountBalance.Totals

            //    ReloadGrid(SerializeObjectByte);
            //    ac.AccountBalance.Totals = null;

            //    var SerializeObjectByte1 = LogitudeXmlSerializer.SerializeObject<AccountBalanceM>(ac.AccountBalance);
            //    _LabelResult.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByte1);

            //}
            //catch (Exception)
            //{
            //    param = null;
            //    throw;
            //}
            //finally
            //{
            //    _MyLastAction.Value = MyLastAction._ButtonCheckBalance_Click.ToString();
            //    if (param == null)
            //    {
            //        param = paramDefault;
            //    }
            //    var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<ReconcileParam>(param);
            //    _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
            //    _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            //}
        }


        protected void _ButtonTreeMapCOA_Click(object sender, EventArgs e)
        {


            //ParamBasic param = null;
            //ParamBasic paramDefault = new ParamBasic()
            //{
            //    MyTenant = 989,
            //    MyDate = DateTime.Now.AddMonths(-1),
            //    MyGLAccId = "1-1",


            //};
            dynamic param = null;
            var paramDefault = new
            {
                MyTenant = 989,
                MyCollector = "",
                OptionByBalance = "AccountingDateBalance1,DueDateBalance2",
                ByBalance = "AccountingDateBalance1",
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonTreeMapCOA_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }


                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);

                var tenant = (int)param.MyTenant;
                Response.Clear();
                Response.Redirect("TreeMapGLAccountBanlanceByCOA.aspx?tenant=" + param.MyTenant + "&MyCollector=" + param.MyCollector + "&ByBalance=" + param.ByBalance);
                //var myAllCardServiceDS = new GLAccountDashboard();
                //var dic = myAllCardServiceDS.GetCardsLocalBalanceGByChartOfAccountsTypeCode(tenant);

                //var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<ChartOfAccountBalanceM>>(dic);
                //ReloadGrid(SerializeObjectByte);

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonTreeMapCOA_Click.ToString();
                if (param == null)
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(paramDefault);
                }
                else
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(param);
                }
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }

        }

        protected void _ButtonDueLocalBalance_Click(object sender, EventArgs e)
        {
            dynamic param = null;
            var paramDefault = new
            {
                MyTenant = 1064,


            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonDueLocalBalance_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }


                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);

                var tenant = (int)param.MyTenant;
                Response.Clear();
                var myDueLocalBalanceService = new DueLocalBalanceService();
                myDueLocalBalanceService.ReBuild(tenant, "");

                //var myAllCardServiceDS = new GLAccountDashboard();
                //var dic = myAllCardServiceDS.GetCardsLocalBalanceGByChartOfAccountsTypeCode(tenant);

                //var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<ChartOfAccountBalanceM>>(dic);
                //ReloadGrid(SerializeObjectByte);

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonDueLocalBalance_Click.ToString();
                if (param == null)
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(paramDefault);
                }
                else
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(param);
                }
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }

        protected void _ButtonReverseDueDate_Click(object sender, EventArgs e)
        {
            dynamic param = null;
            var paramDefault = new
            {
                MyTenant = 1064,
                AccountId = "1-216569"

            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonReverseDueDate_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }


                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);

                var tenant = (int)param.MyTenant;
                string AccountId = param.AccountId;
                Response.Clear();
                var myDueLocalBalanceService = new DueLocalBalanceService();
                var listDiff = myDueLocalBalanceService.ReverseEngineer(tenant, AccountId);

                //var myAllCardServiceDS = new GLAccountDashboard();
                //var dic = myAllCardServiceDS.GetCardsLocalBalanceGByChartOfAccountsTypeCode(tenant);

                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<DueLocalBalanceDiffM>>(listDiff);
                ReloadGrid(SerializeObjectByte);

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonReverseDueDate_Click.ToString();
                if (param == null)
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(paramDefault);
                }
                else
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(param);
                }
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }

        protected void ButtonYearTransfer_Click(object sender, EventArgs e)
        {



            dynamic param = null;

            var paramDefault = new
            {
                Tenant = 989,
                YY = 17,
                Immediate =false
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonYearTransfer_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);
                int YY = param.YY;
                int tenant = param.Tenant;
                bool Immediate = param.Immediate;
                JournalPM journal = null;
                if (Immediate)
                {
                    journal = ImmediateYearTransferthod(YY, tenant);
                }
                else
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        var accountingContext = AccountingContext.GetContext(tenant);
                        string userId = AuthenticationUtil.ResolveUserId(tenant);
                        ICheckAndQYearTransferService yearTransferService = new YearTransferService();
                        string taskiD = yearTransferService.Check_CreateQBatchTaskYearTransfer(YY, tenant, userId);

                        scope.Complete();
                    }
                }
                
            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonYearTransfer_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = JsonConvert.SerializeObject(param);
                _TextBoxParam.Text = SerializeObjectByteParam;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }


        }

        private JournalPM ImmediateYearTransferthod(int YY, int tenant)
        {
            JournalPM journal;
            using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(10)))
            {
                var accountingContext = AccountingContext.GetContext(tenant);
                string userId = AuthenticationUtil.ResolveUserId(tenant);
                IYearTransferService yearTransferService = new YearTransferService();
                journal = yearTransferService.ProccessJournal(accountingContext, YY, tenant, userId);
                if (journal != null)
                {
                    //var parser = new JournalApproveParser(journal, false,
                    //AccountingValidationContextServiceProvider.NewJournalValidatorContextByAContext(accountingContext, journal)
                    //);
                    //parser.ParseIt();
                    bool toComplete = false;
                    if (!toComplete)
                    {
                        throw new Exception("ddd");
                    }
                    scope.Complete();
                }


            }
            if (journal != null)
            {
                var journalJson = JsonConvert.SerializeObject(journal);
                _LabelResult.Text = journalJson;
            }
            else
            {
                _LabelResult.Text = "No journal";
            }

            return journal;
        }

        protected void ButtonYearTransferCancel_Click(object sender, EventArgs e)
        {
            dynamic param = null;

            var paramDefault = new
            {
                Tenant = 989,
                YY = 17,
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonYearTransferCancel_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);
                int YY = param.YY;
                int tenant = param.Tenant;
                JournalPM journal = null;
                
                using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(10)))
                {
                    var accountingContext = AccountingContext.GetContext(tenant);
                    string userId = AuthenticationUtil.ResolveUserId(tenant);
                    ICancelYearTransferService yearTransferService = new YearTransferService();
                    journal = yearTransferService.CancelYear(accountingContext, YY, tenant/*, userId*/);
                    if (journal != null)
                    {
                        //var parser = new JournalApproveParser(journal, false,
                        //AccountingValidationContextServiceProvider.NewJournalValidatorContextByAContext(accountingContext, journal)
                        //);
                        //parser.ParseIt();
                        bool toComplete = true;
                        if (!toComplete)
                        {
                            throw new Exception("ddd");
                        }
                        scope.Complete();
                    }


                }
                if (journal != null)
                {
                    var journalJson = JsonConvert.SerializeObject(journal);
                    _LabelResult.Text = journalJson;
                }
                else
                {
                    _LabelResult.Text = "No journal";
                }
            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonYearTransferCancel_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = JsonConvert.SerializeObject(param);
                _TextBoxParam.Text = SerializeObjectByteParam;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }


        }


        protected void ButtonGetSystem1000_Click(object sender, EventArgs e)
        {



            dynamic param = null;

            var paramDefault = new
            {
                Tenant = 989,
                Email = "itzik@amital.co.il"
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonGetSystem1000_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);
                int tenant = param.Tenant;
                string Email = param.Email;
                string flatFile = "";
                //         using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(10)))
                //         {
                var accountingContext = AccountingContext.GetContext(tenant);
                ISystem1000Service System1000Service = new System1000Service();
                var flatFiles = System1000Service.GetSystem1000FlatFile(accountingContext, tenant);
                if (flatFiles.Count > 0)
                {
                    System1000Service.EmailIt(Email, flatFiles, tenant);
                }
                //    if (!String.IsNullOrWhiteSpace(flatFile))
                //    {
                //        bool toComplete = false;
                //        if (!toComplete)
                //        {
                //            throw new Exception("ddd");
                //        }
                //        scope.Complete();
                //    }


                //}
                flatFile = string.Join(",", flatFiles);
                if (!String.IsNullOrWhiteSpace(flatFile))
                {
                    // var flatFileJson = JsonConvert.SerializeObject(flatFile);
                    _LabelResult.Text = flatFile; // flatFileJson;


                }
                else
                {
                    _LabelResult.Text = "[No flat file]";
                }
            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonGetSystem1000_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = JsonConvert.SerializeObject(param);
                _TextBoxParam.Text = SerializeObjectByteParam;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }


        }



        protected void ButtonLoadSystem1000_Click(object sender, EventArgs e)
        {

            string param = "";
            string paramDefault = "Please insert page, you can add a header  //Tenant=1071";

            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonLoadSystem1000_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                string fileSystem1000 = _TextBoxParam.Text;

                var mySystem1000FlatFileAnalyser = new System1000FlatFileAnalyser();
                mySystem1000FlatFileAnalyser.Analyse(null, fileSystem1000);

                _LabelResult.Text = JsonConvert.SerializeObject(mySystem1000FlatFileAnalyser.MyResultLoadFlatFile); ;

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonLoadSystem1000_Click.ToString();
                if (string.IsNullOrWhiteSpace(param))
                {
                    param = paramDefault;
                }

                _TextBoxParam.Text = param;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }






        protected void _ButtonSysCheckTotalSumIsZero_Click(object sender, EventArgs e)
        {


            dynamic param = null;

            var paramDefault = new
            {
                Tenant = 989,
                //YYYY = 2016,
                //CheckControlAccountMode=false
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonSysCheckTotalSumIsZero_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);
                //int YYYY = param.YYYY;
                //bool CheckControlAccountMode = param.CheckControlAccountMode;
                int tenant = param.Tenant;

                var accountingContext = AccountingContext.GetContext(tenant);
                var systemCheckTotals = new SystemCheckTotals();
                systemCheckTotals.TotalSumMustBeZero(tenant);
                var json = systemCheckTotals.TotalSumPerAccountGroupByDateTypeDiff(tenant);

                _LabelResult.Text = json;
                //var journalJson = JsonConvert.SerializeObject(journal);
                //_LabelResult.Text = journalJson;

                //var parser = new JournalApproveParser(journal, false,
                //AccountingValidationContextServiceProvider.NewJournalValidatorContextByAContext(accountingContext, journal)
                //);
                //parser.ParseIt();
            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonSysCheckTotalSumIsZero_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = JsonConvert.SerializeObject(param);
                _TextBoxParam.Text = SerializeObjectByteParam;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }


        protected void _ButtonSysCheckLdegerTransSumIsZero_Click(object sender, EventArgs e)
        {


            dynamic param = null;

            var paramDefault = new
            {
                Tenant = 989,
                YYYY = 2017,
                //CheckControlAccountMode=false
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonSysCheckLdegerTransSumIsZero_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);
                int YYYY = param.YYYY;
                //bool CheckControlAccountMode = param.CheckControlAccountMode;
                int tenant = param.Tenant;

                var accountingContext = AccountingContext.GetContext(tenant);
                var systemCheckTotals = new SystemCheckTotals();
                systemCheckTotals.LedgerTransactionSumMustBeZero(tenant, YYYY);



                //var journalJson = JsonConvert.SerializeObject(journal);
                //_LabelResult.Text = journalJson;

                //var parser = new JournalApproveParser(journal, false,
                //AccountingValidationContextServiceProvider.NewJournalValidatorContextByAContext(accountingContext, journal)
                //);
                //parser.ParseIt();
            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonSysCheckLdegerTransSumIsZero_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = JsonConvert.SerializeObject(param);
                _TextBoxParam.Text = SerializeObjectByteParam;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }
        protected void _ButtonJournalApprove_Click(object sender, EventArgs e)
        {
            dynamic param = null;
            var paramDefault = new
            {
                Tenant = 989,
                JournalId = "1-55235"
                //YYYY = 2016,
                //CheckControlAccountMode=false
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonJournalApprove_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);
                //int YYYY = param.YYYY;
                //bool CheckControlAccountMode = param.CheckControlAccountMode;
                int tenant = param.Tenant;
                string JournalId = param.JournalId;





                List<string> Last_journalBufferKeys = null;
                JournalApproveService.WorkWithoutQueue(tenant, JournalId, ref Last_journalBufferKeys);



            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonJournalApprove_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = JsonConvert.SerializeObject(param);
                _TextBoxParam.Text = SerializeObjectByteParam;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }

        }



        protected void _WorkWithoutQueueStatus4_Click(object sender, EventArgs e)
        {
            dynamic param = null;
            var paramDefault = new
            {
                Tenant = 989,
                JournalId = "1-55235"
                //YYYY = 2016,
                //CheckControlAccountMode=false
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._WorkWithoutQueueStatus4_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);
                //int YYYY = param.YYYY;
                //bool CheckControlAccountMode = param.CheckControlAccountMode;
                int tenant = param.Tenant;
                string JournalId = param.JournalId;





                List<string> Last_journalBufferKeys = null;
                JournalApproveService.WorkWithoutQueueStatus4(tenant, JournalId, ref Last_journalBufferKeys);



            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._WorkWithoutQueueStatus4_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = JsonConvert.SerializeObject(param);
                _TextBoxParam.Text = SerializeObjectByteParam;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }

        }


        protected void _ButtonIsApprovedJournalTOTZero_Click(object sender, EventArgs e)
        {


            dynamic param = null;

            var paramDefault = new
            {
                Tenant = 989,
                YearAndMonth = DateTime.Now.Date,

                JournalId = "",
                Remarks = "Tenat is must other not "

                //CheckControlAccountMode=false
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonIsApprovedJournalTOTZero_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);

                DateTime? YearAndMonth = null;
                if (param.YearAndMonth.ToString() != "")
                {
                    YearAndMonth = param.YearAndMonth;
                }

                //bool CheckControlAccountMode = param.CheckControlAccountMode;
                int tenant = param.Tenant;
                string JournalId = param.JournalId;

                var accountingContext = AccountingContext.GetContext(tenant);
                var systemCheck = new SystemCheckApprovedJournals();
                var errorrows = systemCheck.StartCheck(tenant, YearAndMonth, JournalId);



                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<string>>(errorrows);

                //_LabelHaveApproveJournal.Text = ac.AccountBalance.HaveAccountingQueued.ToString(); 
                ReloadGrid(SerializeObjectByte);



            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonIsApprovedJournalTOTZero_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = JsonConvert.SerializeObject(param);
                _TextBoxParam.Text = SerializeObjectByteParam;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }
        protected void _ButtonSysCheckGLAccJL2Total_Click(object sender, EventArgs e)
        {


            dynamic param = null;

            var paramDefault = new
            {
                Tenant = 989,
                GLAccountId = "1-55235"
                //YYYY = 2016,
                //CheckControlAccountMode=false
            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonSysCheckGLAccJL2Total_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);
                //int YYYY = param.YYYY;
                //bool CheckControlAccountMode = param.CheckControlAccountMode;
                int tenant = param.Tenant;
                string gLAccountId = param.GLAccountId;

                var accountingContext = AccountingContext.GetContext(tenant);
                var systemCheckTotals = new SystemCheckGLAccount();
                var errorrows = systemCheckTotals.StartCheck(gLAccountId, tenant);



                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<JournalLineLedgerDTO>>(errorrows);

                //_LabelHaveApproveJournal.Text = ac.AccountBalance.HaveAccountingQueued.ToString(); 
                ReloadGrid(SerializeObjectByte);


                //var journalJson = JsonConvert.SerializeObject(journal);
                //_LabelResult.Text = journalJson;

                //var parser = new JournalApproveParser(journal, false,
                //AccountingValidationContextServiceProvider.NewJournalValidatorContextByAContext(accountingContext, journal)
                //);
                //parser.ParseIt();
            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonSysCheckGLAccJL2Total_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = JsonConvert.SerializeObject(param);
                _TextBoxParam.Text = SerializeObjectByteParam;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }


        protected void ButtonBuildTenant_Click(object sender, EventArgs e)
        {


            dynamic param = null;

            var paramDefault = new
            {
                Tenant = 1051,
                YYYY = 2019,
                BuildAccountingTenant = new BuildAccountingTenantParam()
                {
                    //OtherAccounts = true,
                    VatAccounts = true,
                    ControlAccounts = true,
                    ExchangeRateDiff = true,
                    RevenueExpense = true,
                    TaxWithholding = true
                },
                //BuildFullAccountingSetting = true,
                //BuildFullAccountingSettingVAT = true,
                BuildGLAccountEachType = 30,
                BuildJournalEachMonth = 20,


            };
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonBuildTenant_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);
                //int YYYY = param.YYYY;
                //bool CheckControlAccountMode = param.CheckControlAccountMode;
                int tenant = param.Tenant;
                int YYYY = param.YYYY;
                //bool BuildFullAccountingSetting = param.BuildFullAccountingSetting;
                //bool BuildFullAccountingSettingVAT = param.BuildFullAccountingSettingVAT;
                BuildAccountingTenantParam BuildAccountingTenant = new BuildAccountingTenantParam()
                {
                    ControlAccounts = param.BuildAccountingTenant.ControlAccounts,
                    ExchangeRateDiff = param.BuildAccountingTenant.ExchangeRateDiff,
                    RevenueExpense = param.BuildAccountingTenant.RevenueExpense,
                    TaxWithholding = param.BuildAccountingTenant.TaxWithholding,
                    VatAccounts = param.BuildAccountingTenant.VatAccounts,
                };
                if (BuildAccountingTenant.ControlAccounts)
                {
                    BuildAccountingTenant.ChartOfAccountsId = null;
                    BuildAccountingTenant.ControlAccountId= null;
                    //BuildAccountingTenant.CreateCustomerControlAccountId = true;
                    //BuildAccountingTenant.CreateVendorControlAccountId = true;
                    //BuildAccountingTenant.CreateFileControlAccountId = true;
                    //BuildAccountingTenant.CreateOceanExportJobControlAccountId = true;
                    //BuildAccountingTenant.CreateOceanImportJobControlAccountId = true;
                    //BuildAccountingTenant.CreateAirImportJobControlAccountId = true;

                }
                BuildAccountingTenant.CheckAndInsertPoco = true;
                int BuildGLAccountEachType = param.BuildGLAccountEachType;
                int BuildJournalEachMonth = param.BuildJournalEachMonth;

                SetHttpAuth(tenant);
                var accountingContext = AccountingContext.GetContext(tenant);

                FullAccountingSetting fullSetting = null; ;

                var chartOfAccountProvider = new ChartOfAccountProvider();
                var displayNumberProvider = new DisplayNumberProvider();
                if (BuildAccountingTenant!=null/*BuildFullAccountingSetting*/)
                {
                    var BTFullAccountingService = new FullAccountingProvider(chartOfAccountProvider, displayNumberProvider);
                    fullSetting = BTFullAccountingService.Insert(tenant, accountingContext, BuildAccountingTenant);
                }
                //if (BuildFullAccountingSettingVAT)
                //{
                //    if (fullSetting == null)
                //    {
                //        var repo = new FullAccountingSettingRepository(tenant);
                //        fullSetting = repo.GetSingleFullAccountingSetting(tenant);
                //        if (fullSetting == null)
                //        {
                //            throw new Exception("BuildFullAccountingSettingVAT - but not BuildFullAccountingSetting");
                //        }
                //    }
                //    var BTFullAccountingService = new FullAccountingProvider(chartOfAccountProvider, displayNumberProvider);

                //    BTFullAccountingService.CreateVatGLAccount(tenant, accountingContext, fullSetting);
                //}
                CacheManager.ClearCacheItems();




                if (BuildGLAccountEachType > 0)
                {
                    var dummyTenantProviderArg = new DummyTenantProviderArg()
                    {
                        CreateJobs = BuildGLAccountEachType,
                        CreateCustomers = BuildGLAccountEachType,
                        CreateExpanse = BuildGLAccountEachType,
                        CreateFiles = BuildGLAccountEachType,
                        CreateRevenue = BuildGLAccountEachType,
                        CreateVendors = BuildGLAccountEachType,

                    };
                    var g = new DummyTenantProvider();
                    g.GenrateGLAccount(dummyTenantProviderArg, accountingContext, chartOfAccountProvider, displayNumberProvider, fullSetting, tenant);
                    accountingContext.SaveChanges();
                }
                CacheManager.ClearCacheItems();
                if (BuildJournalEachMonth > 0)
                {
                    var g = new DummyTenantProvider();
                    g.GenrateJournals(BuildJournalEachMonth, accountingContext, YYYY, tenant);
                    accountingContext.SaveChanges();
                }





            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonBuildTenant_Click.ToString();
                if (param == null)
                {
                    param = paramDefault;
                }
                var SerializeObjectByteParam = JsonConvert.SerializeObject(param);
                _TextBoxParam.Text = SerializeObjectByteParam;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }


        protected void _ButtonLoadBankPages_Click(object sender, EventArgs e)
        {



            string param = "";
            string paramDefault = "Please insert page U Can Add Header  //Tenant=1071";

            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonLoadBankPages_Click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                //param = JsonConvert.DeserializeObject(_TextBoxParam.Text);
                //int YYYY = param.YYYY;
                //bool CheckControlAccountMode = param.CheckControlAccountMode;
                //int tenant = param.Tenant;
                string FileBankPages = _TextBoxParam.Text;// param.FileBankPages;
                                                          //SetHttpAuth(tenant);

                var myBankAccountPageAnalyzer = new BankAccountPageAnalyzer();
                myBankAccountPageAnalyzer.Analyze(null, FileBankPages);

                _LabelResult.Text = JsonConvert.SerializeObject(myBankAccountPageAnalyzer.MyResultLoadBankPage); ;

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonLoadBankPages_Click.ToString();
                if (string.IsNullOrWhiteSpace(param))
                {
                    param = paramDefault;
                }

                _TextBoxParam.Text = param;
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();
            }
        }




        protected void _ButtonExternalReconcile_click(object sender, EventArgs e)
        {



            dynamic param = null;

            var paramDefault = new
            {
                Tenant = 1064,

                LedgerTransactionId = "1-3069551",
                ReconcileExternalPageLineId = "1-1313",//"1-12225"
            };
            
            try
            {

                if (GetMyLastAction() != MyLastAction._ButtonExternalReconcile_click)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    return;
                }

                param = JsonConvert.DeserializeObject(_TextBoxParam.Text);

                int Tenant = param.Tenant;
                string LedgerTransactionId = param.LedgerTransactionId;
                string ReconcileExternalPageLineId = param.ReconcileExternalPageLineId;

                
                var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
                myExternalReconcileJournalService.MustInit(new ExternalReconcileDataProvider( AccountingContext.GetContext(Tenant)));
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(Tenant, LedgerTransactionId, ReconcileExternalPageLineId);
                var us = new JournalUpdateService(AccountingContext.GetContext(Tenant), new Dictionary<string, IContext>(),Tenant);
                us.Update(myExternalReconcileJournalService.TheJournalPM, true);
                _LabelResult.Text = JsonConvert.SerializeObject(myExternalReconcileJournalService.TheJournalPM); ;

            }
            catch (Exception)
            {
                param = null;
                throw;
            }
            finally
            {
                _MyLastAction.Value = MyLastAction._ButtonExternalReconcile_click.ToString();
                if (param == null)
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(paramDefault);
                }
                else
                {
                    _TextBoxParam.Text = JsonConvert.SerializeObject(param);
                }

                _LabelLog.Text = LogMessagingUtil.Instance.ToString();

                
            }
        }


        private void SetHttpAuth(int tenant)
        {
            var email = AuthenticationUtil.SystemIdentityName(tenant);
            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(
                email
                /*authToken.Email*/), new string[0]);
        }

        private CardIndexReportParams UpdateDefaultCardIndexNew(CardIndexReportParams myLedgerTransactionBalanceFilter)
        {
            _MyLastAction.Value = MyLastAction._ButtonLedgerTransactionBalance_Click.ToString();
            myLedgerTransactionBalanceFilter = new CardIndexReportParams()
            {
                Tenant = 62,
                From = DateTime.Now.AddMonths(-1),
                To = DateTime.Now,
                CurrencyId = (new AccountingSettingResolver()).ResolveAccountingCurrencyId(1),
                DateTypeCode = "1",
                GLAccountId = "1-1533",

                SearchFields = "",
                PageStartAtRecordIndex = 0,
                PageSize = 20000,
                Category1Id = "",
                Category2Id = "",
                Category3Id = "",
                Category4Id = "",
                Category5Id = "",
                ChartOfAccountsId = "1-186",
                AccountTypeCode = "",
                IsReconciled=null,

            };
            var SerializeObjectByteParam = LogitudeXmlSerializer.SerializeObject<CardIndexReportParams>(myLedgerTransactionBalanceFilter);
            _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam);
            return myLedgerTransactionBalanceFilter;
        }
        protected void _ButtonCardIndexNew_Click(object sender, EventArgs e)
        {

           
                CardIndexReportParams myCardIndexReportParams = null;

                if (GetMyLastAction() != MyLastAction._ButtonLedgerTransactionBalance_Click)
                {
                    myCardIndexReportParams = UpdateDefaultCardIndexNew(myCardIndexReportParams) as CardIndexReportParams;
                    return;
                } 
                if (string.IsNullOrWhiteSpace(_TextBoxParam.Text))
                {
                    myCardIndexReportParams = UpdateDefaultCardIndexNew(myCardIndexReportParams) as CardIndexReportParams;
                    return;
                }
                myCardIndexReportParams =
                    LogitudeXmlSerializer.DeserializeObject<CardIndexReportParams>(_TextBoxParam.Text);

                _MyLastAction.Value = MyLastAction._ButtonLedgerTransactionBalance_Click.ToString();
                var accountingContext = AccountingContext.GetContext(myCardIndexReportParams.Tenant);
                var CardIndexReportService = new CardIndexReportService(accountingContext, myCardIndexReportParams);
                CardIndexReportService.Run();

                //_LabelResult.Text = "ledgerTransactionBalance" +ledgerTransactionBalanceService.Response.StartBalanceLocal + " " + ledgerTransactionBalanceService.Response.EndBalanceLocal;

                
                var SerializeObjectByteParam2 = LogitudeXmlSerializer.SerializeObject<CardIndexReportParams>(myCardIndexReportParams);
                _TextBoxParam.Text = System.Text.Encoding.UTF8.GetString(SerializeObjectByteParam2);

                var SerializeObjectByte = LogitudeXmlSerializer.SerializeObject<List<LedgerTransactionBalanceResponse>>(CardIndexReportService.CardIndexs);
                ReloadGrid(SerializeObjectByte);
                _LabelLog.Text = LogMessagingUtil.Instance.ToString();

            }
        }

    public class ReconcileParam
    {
    }

    public class ParamBasic
    {
        public int MyTenant { get; set; }
        public DateTime MyDate { get; set; }

        public string MyGLAccId { get; set; }
    }

    class JournalTesterClass
    {
        static Random _Rand = new Random(DateTime.Now.TimeOfDay.TotalMinutes.GetHashCode());


        //static Hashtable _
        static List<string> _Client = null;
        private static List<string> _Vendor;
        private static List<string> _Revenue;
        private int tenant = 1;

        public static string GetClientCard(int tenant)
        {
            if (_Client == null)
            {
                //var qs = new GLAccountQueryService(tenant);
                //2	לקוח	Client	0
                var repo = new GLAccountRepository(tenant);

                _Client = repo.GetGLAccountId(tenant, ((int)GLAccountTypePM.GLAccountTypeEnum.Client).ToString(), "", 100);
            }
            if (_Client.Count < 1)
            {
                throw new Exception("if (_Client.Count < 1)");
            }
            var pos = _Rand.Next(_Client.Count);
            return _Client.ElementAt(pos);
        }
        public static string GetVendorCard(int tenant)
        {
            if (_Vendor == null)
            {
                //var qs = new GLAccountQueryService(tenant);
                //2	לקוח	Client	0
                var repo = new GLAccountRepository(tenant);

                _Vendor = repo.GetGLAccountId(tenant, ((int)GLAccountTypePM.GLAccountTypeEnum.Vendor).ToString(), "", 100);
            }
            if (_Vendor.Count < 1)
            {
                throw new Exception("if (_Vendor.Count < 1)");
            }
            var pos = _Rand.Next(_Vendor.Count);
            return _Vendor.ElementAt(pos);
        }
        public static string GetExpenseCard(int tenant)
        {
            if (_Expense == null)
            {
                //var qs = new GLAccountQueryService(tenant);
                //2	לקוח	Client	0
                var repo = new GLAccountRepository(tenant);

                _Expense = repo.GetGLAccountId(tenant, "", ((int)RevenueExpenseTypePM.RevenueExpenseTypeEnum.Expense).ToString(), 100);
            }
            if (_Expense.Count < 1)
            {
                throw new Exception("if (_Expense.Count < 1)");
            }
            var pos = _Rand.Next(_Expense.Count);
            return _Expense.ElementAt(pos);
        }

        public static string GetRevenueCard(int tenant)
        {
            if (_Revenue == null)
            {
                //var qs = new GLAccountQueryService(tenant);
                //2	לקוח	Client	0
                var repo = new GLAccountRepository(tenant);

                _Revenue = repo.GetGLAccountId(tenant, "", ((int)RevenueExpenseTypePM.RevenueExpenseTypeEnum.Revenue).ToString(), 100);
            }
            if (_Revenue.Count < 1)
            {
                throw new Exception("if (_Revenue.Count < 1)");
            }
            var pos = _Rand.Next(_Revenue.Count);
            return _Revenue.ElementAt(pos);
        }
        public enum JLCreditDebitVatProfile : int
        {
            CreditDebitInTwoLine = 1,
            CreditDebitOneLine = 2,
            DebitCreditAndVatdeduction = 3

        }

        public string InsertRandomJournal()
        {
            int Tenant = 989;
            var row = _Rand.Next(2, 100);
            var accDate1 = DateTime.Now.AddDays(_Rand.Next(-100, 0));

            using (var scope = TransactionFactory.GetTransaction())
            {
                bool f = true;

                Logitude.Accounting.Def.EntityPMs.JournalPM j = null;

                j = new Logitude.Accounting.Def.EntityPMs.JournalPM()
                {
                    AccountingDate = accDate1,
                    Tenant = tenant,
                    JournalNumber = "1003",
                    //StatusCode = "2",
                    StatusCodeEnum = Logitude.Accounting.Def.EntityPMs.JournalStatusTypePM.StatusCodeEnum.Approved,
                    UpdatedByUserId = "1-1",
                    UpdateDate = DateTime.Now,
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    CreateDate = DateTime.Now,
                    CreatedByUserId = "1-1",
                };
                bool typeVendorClientOrRevExpense = false;
                j.JournalLines = new List<JournalLinePM>();
                string creditCard = "";
                string debitCard = "";
                int lineId = 0;
                for (int i = 0; i < row; i++)
                {

                    if (typeVendorClientOrRevExpense)
                    {
                        creditCard = GetClientCard(tenant);
                        debitCard = GetVendorCard(tenant);
                    }
                    else
                    {
                        creditCard = GetRevenueCard(tenant);
                        debitCard = GetExpenseCard(tenant);
                    }
                    DateTime DocumentDate = DateTime.Now.AddDays(_Rand.Next(-600, 180));
                    DateTime DueDate = DateTime.Now.AddDays(_Rand.Next(-600, 180));
                    decimal localAm = _Rand.Next(201, 5000000) / 100m;
                    decimal foreignAm = localAm / 4;

                    JLCreditDebitVatProfile journalLinesProfile = (JLCreditDebitVatProfile)Enum.ToObject(typeof(JLCreditDebitVatProfile), _Rand.Next(3));

                    switch (journalLinesProfile)
                    {
                        case JLCreditDebitVatProfile.CreditDebitOneLine:
                            j.JournalLines.Add(new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                            {
                                AccountingDate = j.AccountingDate,
                                //ActionName = "4", 
                                ActionTypeCodeEnum = MyJournalActionTypeEnum.DebitAndCredit,
                                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                CreditAccountId = creditCard,
                                DebitAccountId = debitCard,
                                DocumentDate = DocumentDate,
                                DueDate = DueDate,

                                ForeignAmount = foreignAm,
                                LocalAmount = localAm,
                                CurrencyId = GetCurrencyId("USD"),

                                JournalId = j.Id,
                                Line = lineId++,
                                Tenant = tenant
                            });
                            break;
                        case JLCreditDebitVatProfile.DebitCreditAndVatdeduction:
                            j.JournalLines.Add(new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                            {
                                AccountingDate = j.AccountingDate,
                                //ActionName = "4", 
                                ActionTypeCodeEnum = MyJournalActionTypeEnum.DebitCreditAndVatdeduction,
                                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                CreditAccountId = creditCard,
                                DebitAccountId = debitCard,
                                DocumentDate = DocumentDate,
                                DueDate = DueDate,

                                ForeignAmount = foreignAm,
                                LocalAmount = localAm,
                                CurrencyId = GetCurrencyId("USD"),

                                JournalId = j.Id,
                                Line = lineId++,
                                Tenant = tenant
                            });
                            break;
                        case JLCreditDebitVatProfile.CreditDebitInTwoLine:
                        default:

                            j.JournalLines.Add(new JournalLinePM()
                            {
                                AccountingDate = j.AccountingDate,
                                //ActionName = "1", 
                                ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit,
                                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                CreditAccountId = creditCard,
                                DebitAccountId = null,

                                DocumentDate = DocumentDate,
                                DueDate = DueDate,

                                ForeignAmount = foreignAm,
                                LocalAmount = localAm,
                                CurrencyId = GetCurrencyId("USD"),

                                JournalId = j.Id,
                                Line = lineId++,
                                Tenant = tenant

                            });
                            j.JournalLines.Add(
                               new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                               {
                                   AccountingDate = j.AccountingDate,
                                   //ActionName = "2", 
                                   ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit,
                                   ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                   //CreditAccountId = "35", 

                                   CreditAccountId = null,
                                   DebitAccountId = GetDebitAccountId(),
                                   DocumentDate = DocumentDate,
                                   DueDate = DueDate,
                                   ForeignAmount = foreignAm,
                                   LocalAmount = localAm,
                                   CurrencyId = GetCurrencyId("USD"),
                                   JournalId = j.Id,
                                   Line = 2,
                                   Tenant = 989
                               });
                            break;
                    }
                    typeVendorClientOrRevExpense = !typeVendorClientOrRevExpense;


                }

                var myAccountingContext = AccountingContext.GetContext(1);
                var us = new Logitude.Accounting.BL.EntityUpdateServices.JournalUpdateService(myAccountingContext, new Dictionary<string, IContext>(), 1);
                us.Update(j, true);
                scope.Complete();
                return j.Id;
            }
        }




        public void NewJournalMethod(DateTime accDate, string CreditAccountId = null)
        {
            using (var scope = TransactionFactory.GetTransaction())
            {
                bool f = true;

                Logitude.Accounting.Def.EntityPMs.JournalPM j = GetDefaultJornalPM(accDate);

                var myAccountingContext = AccountingContext.GetContext(1);
                var us = new Logitude.Accounting.BL.EntityUpdateServices.JournalUpdateService(myAccountingContext, new Dictionary<string, IContext>(), 1);
                us.Update(j, true);
                scope.Complete();

            }
        }

        public static JournalPM GetDefaultJornalPM(DateTime accDate)
        {
            Logitude.Accounting.Def.EntityPMs.JournalPM j = null;
            var refExt = DateTime.Now.Ticks.ToString();
            j = new Logitude.Accounting.Def.EntityPMs.JournalPM()
            {
                AccountingDate = accDate,
                Tenant = 989,
                JournalNumber = "1003",
                //StatusCode = "2",
                TypeCode = "0",//0,Manual,ידנית
                AccountingEntityCode = "1",//1	פקודת יומן	Journal
                StatusCodeEnum = Logitude.Accounting.Def.EntityPMs.JournalStatusTypePM.StatusCodeEnum.Approved,
                UpdatedByUserId = "1-1",
                UpdateDate = DateTime.Now,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                CreateDate = DateTime.Now,
                CreatedByUserId = "1-1",
                AccountingEntityReference = "AER" + refExt,
                AccountingEntityId = "1-69"



            };



            j.JournalLines = new List<Logitude.Accounting.Def.EntityPMs.JournalLinePM>
                    {
                        new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                    {
                      AccountingDate = j.AccountingDate,
                      //ActionName = "1", 
                      ActionTypeCodeEnum= MyJournalActionTypeEnum.Credit,
                      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                      CreditAccountId = GetCreditAccountId(),
                      //DebitAccountId = "35",
                      DebitAccountId = null,

                      CurrencyId =GetCurrencyId("USD"),
                      DocumentDate=DateTime.Now,
                      DueDate= DateTime.Now,
                      ForeignAmount = 25,
                      LocalAmount = 100,
                      ExchangeRate=4,
                      JournalId=j.Id,
                      Line=1,
                      Tenant=j.Tenant
                      ,
                      Reference1="ref1" +refExt ,
                      Reference2="ref2" +refExt ,
                      Reference3="ref3" +refExt ,
                      Notes ="Notes" +refExt ,
                    },
                        new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                    {
                      AccountingDate = j.AccountingDate,
                      //ActionName = "2", 
                      ActionTypeCodeEnum= MyJournalActionTypeEnum.Debit,
                      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert, 
                      //CreditAccountId = "35", 
                      CurrencyId =GetCurrencyId("USD"),
                      CreditAccountId = null,
                      DebitAccountId = GetDebitAccountId(),
                      DocumentDate=DateTime.Now,
                      DueDate= DateTime.Now,
                      ForeignAmount = 25,
                      LocalAmount = 100,
                      ExchangeRate=4,
                      JournalId=j.Id,
                      Line=2,
                      Tenant=j.Tenant
                      ,
                      Reference1="ref1" +refExt ,
                      Reference2="ref2" +refExt ,
                      Reference3="ref3" +refExt ,
                      Notes ="Notes" +refExt ,
                    },

                  new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                    {
                      AccountingDate = j.AccountingDate,
                      //ActionName = "4", 
                      ActionTypeCodeEnum= MyJournalActionTypeEnum.DebitCreditAndVatdeduction,
                      ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                      CreditAccountId = GetCreditAccountId(),
                      DebitAccountId = GetDebitAccountId(),
                      DocumentDate=DateTime.Now,
                      DueDate= DateTime.Now,

                      ForeignAmount = 25,
                      LocalAmount = 100,
                      ExchangeRate=4,
                       CurrencyId =GetCurrencyId("USD"),

                      JournalId=j.Id,
                      Line=3,
                      Tenant=j.Tenant

                      ,
                      Reference1="ref1" +refExt ,
                      Reference2="ref2" +refExt ,
                      Reference3="ref3" +refExt ,
                      Notes ="Notes" +refExt ,
                    }
                    };
            return j;
        }

        private static string GetCurrencyId(string p)
        {
            return "1-4438";
        }

        private static string GetDebitAccountId()
        {
            return "1-64";
        }

        private static string GetCreditAccountId()
        {
            return "1-63";
        }

        public static List<string> _Expense { get; set; }
    }

}
