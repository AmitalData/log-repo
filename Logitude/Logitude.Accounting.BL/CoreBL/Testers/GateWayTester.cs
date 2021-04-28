using Logitude.Accounting.BL.CoreBL.BuildTenant;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.CoreBL.Reports.Aging;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Logitude.Accounting.BL.CoreBL.Testers
{
    public class GateWayTester
    {
        public GateWayTesterResult TestIt(string operationId,int tenant, string _TextBoxParam)
        {
            LogMessagingUtil.Instance.Clear();
            switch (operationId)
            {
                case "_ButtonReverseTotal_Click":
                    {
                       return _ButtonReverseTotal_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "_ButtonReverseTrans_Click":
                    {
                        return _ButtonReverseTrans_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "_ButtonReverseTotalFIX_Click":
                    {
                        return _ButtonReverseTotalFIX_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "_ButtonReverseGLBalanceFIX_Click":
                    {
                        return _ButtonReverseGLBalanceFIX_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "_ButtonReverseTotalFIXControl_Click":
                    {
                        return _ButtonReverseTotalFIXControl_Click(tenant, _TextBoxParam);
                    }
                    break;
                    
                    case "_ButtonFixDueLocalBalance_Click":
                    {
                        return _ButtonFixDueLocalBalance_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "_ButtonReverseDueDate_Click":
                    {
                        return _ButtonReverseDueDate_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "ButtonLoadSystem1000_Click":
                    {
                        return ButtonLoadSystem1000_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "ButtonLoadConsolTaxRep_Click":
                    {
                        return ButtonLoadConsolTaxRep_Click(tenant, _TextBoxParam);
                    }
                    break;

                case "WorkWithoutQueue_Click":
                    {
                        return WorkWithoutQueue_Click(tenant, _TextBoxParam);
                    }
                    break;
                    
                    case "JournalApproveQueue_Click":
                    {
                        return JournalApproveQueue_Click(tenant, _TextBoxParam);
                    }
                    break;

                case "_TrailReport_Click":
                    {
                        return _TrailReport_Click(tenant, _TextBoxParam);
                    }
                    break;

                case "Aging_Click":
                    {
                        return Aging_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "RebuildFIXGLAccountAgingData_Click":
                    {
                        return RebuildFIXGLAccountAgingData_Click(tenant, _TextBoxParam);
                    }
                    break;
                
                case "CardIndexNew_Click":
                    {
                        return CardIndexNew_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "YearTransfer_Click":
                    {
                        return YearTransfer_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "YearTransferCancel_Click":
                    {
                        return YearTransferCancel_Click(tenant, _TextBoxParam);
                    }
                    break;
                
                case "BuildTenant_Click":
                    {
                        return BuildTenant_Click(tenant, _TextBoxParam);
                    }
                    break;

                default:
                    return new GateWayTesterResult()
                    {
                        ExceptionMess =
                        $"No operationId  {operationId}"
                    };
                    break;
            }
        }

        private GateWayTesterResult RebuildFIXGLAccountAgingData_Click(int tenant, string textBoxParam)
        {
            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                dynamic myAgingReportParam = LogitudeXmlSerializer.JsonConvertDeserializeObject(textBoxParam);
                string aging4AccountTypeCode = myAgingReportParam.Aging4AccountTypeCode;//: 'Customer2', 
                string MyGLAccId = myAgingReportParam.MyGLAccId;
                //using (

                var dailyRebuildAgingService = new DailyRebuildAgingService();
                var diff= dailyRebuildAgingService.RebuildAging4AccountTypeCode(tenant, aging4AccountTypeCode, MyGLAccId);



                string xml = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<GLAccountAgingDataPM>>(diff);



                gateWayTesterResult.JsonOut = xml;

                //gateWayTesterResult.Log = xmlMyPeriodList;


                //gateWayTesterResult.JsonOut = xml;


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }

     

        private GateWayTesterResult Aging_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                var myAgingReportParam = LogitudeXmlSerializer.JsonConvertDeserializeTObject<AgingReportParam>(textBoxParam);
                //using (
                var agingReport = new AgingReportService(myAgingReportParam);
                var xml = agingReport.RunReport();
                //var MyPeriodList = agingReport.MyPeriodList;
                var xmlMyPeriodList = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<PeriodMExtended>>(agingReport.MyPeriodExtendedList);

                gateWayTesterResult.Log = xmlMyPeriodList;


                gateWayTesterResult.JsonOut = xml;


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }


        private GateWayTesterResult _TrailReport_Click(int tenant, string textBoxParam)         
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<TrailReportParam>(textBoxParam);
                //using (
                var trailReportService = TrailReportFactory.CreateNew(param);//)



                var res = trailReportService.Execute();

                gateWayTesterResult.Log = trailReportService.DbLog;
                

                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<TrailReportM>>(res);


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }



        
            
        private GateWayTesterResult BuildTenant_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                var accountingContext = AccountingContext.GetContext(tenant);
                FullAccountingSetting fullSetting = null; ;

                var chartOfAccountProvider = new ChartOfAccountProvider();
                var displayNumberProvider = new DisplayNumberProvider();
              

                dynamic param = LogitudeXmlSerializer.JsonConvertDeserializeObject(textBoxParam);
                int YYYY = param.YYYY;
                //using (
                int BuildGLAccountEachType = param.BuildGLAccountEachType;
                int BuildJournalEachMonth = param.BuildJournalEachMonth;
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

                //gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<JournalPM>(journal);




            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
        private GateWayTesterResult YearTransferCancel_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                dynamic param = LogitudeXmlSerializer.JsonConvertDeserializeObject(textBoxParam);
                //using (
                int YY = param.YY;
                //int tenant = param.Tenant;
                using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(10)))
                {
                    var accountingContext = AccountingContext.GetContext(tenant);
                    string userId = AuthenticationUtil.ResolveUserId(tenant);
                    ICancelYearTransferService yearTransferService = new YearTransferService();
                    JournalPM journal = yearTransferService.CancelYear(accountingContext, YY, tenant/*, userId*/);
                    if (journal != null)
                    {
                     
                        
                        bool toComplete = true;
                        if (!toComplete)
                        {
                            throw new Exception("!toComplete");
                        }
                        scope.Complete();
                    }

                    gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<JournalPM>(journal);
                }



            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
        private GateWayTesterResult YearTransfer_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                dynamic param = LogitudeXmlSerializer.JsonConvertDeserializeObject(textBoxParam);
                //using (
                int YY = param.YY;
                //int tenant = param.Tenant;
                bool Immediate = param.Immediate;
                JournalPM journal = null;
                if (Immediate)
                {
                    journal = ImmediateYearTransferthod(YY, tenant);
                    gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<JournalPM>(journal);
                }
                else
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        var accountingContext = AccountingContext.GetContext(tenant);
                        string userId = AuthenticationUtil.ResolveUserId(tenant);
                        ICheckAndQYearTransferService yearTransferService = new YearTransferService();
                        string taskiD = yearTransferService.Check_CreateQBatchTaskYearTransfer(YY, tenant, userId);
                        gateWayTesterResult.JsonOut = $"{{taskiD : {taskiD} }}";
                        scope.Complete();
                    }
                }
              


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
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
                    bool toComplete = true;
                    if (!toComplete)
                    {
                        throw new Exception("!toComplete");
                    }

                    scope.Complete();
                }


            }
           

            return journal;
        }

        private GateWayTesterResult CardIndexNew_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<CardIndexReportParams>(textBoxParam);
                //using (
                var accountingContext = AccountingContext.GetContext(tenant);
                var CardIndexReportService = new CardIndexReportService(accountingContext, param);
                CardIndexReportService.Run();

                //gateWayTesterResult.Log = trailReportService.DbLog;


                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<LedgerTransactionBalanceResponse>>(CardIndexReportService.CardIndexs);


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
        
        private GateWayTesterResult JournalApproveQueue_Click(int tenant, string textBoxParam)

        {
            dynamic param = null;
            var gateWayTesterResult = new GateWayTesterResult();


            try
            {


                param = LogitudeXmlSerializer.JsonConvertDeserializeObject(textBoxParam);
                //int tenantFrom = param.Tenant;

                var myWorker = new JournalApproveService.JournalApproveWorker();
                var sw = Stopwatch.StartNew();
                HttpContext.Current.Items["workerrolename"] = (string)param.workerrolename;
                string selectedQueue = null;
                if ((bool)param.ConversionJournal)
                {
                    selectedQueue = JournalApproveService.K_AccountingConversionJournalApproveWR;
                }
                myWorker.WorkUntilQEmptyQueueDB(TimeSpan.FromSeconds((int)param.TimeOutinSec), selectedQueue);
                sw.Stop();
                LogMessagingUtil.Instance.AppendLine($"Tot:{sw.Elapsed}");


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {


                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }

        private GateWayTesterResult WorkWithoutQueue_Click(int tenant, string textBoxParam)
        
        {
            dynamic param = null;
            var gateWayTesterResult = new GateWayTesterResult();


            try
            {


                param = LogitudeXmlSerializer.JsonConvertDeserializeObject(textBoxParam);
                int tenantFrom = param.Tenant;
                string JournalId = param.JournalId;
                List<string> Last_journalBufferKeys = null;
                JournalApproveService.WorkWithoutQueue(tenantFrom, JournalId, ref Last_journalBufferKeys);

            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {


                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }


        
               
        private GateWayTesterResult ButtonLoadSystem1000_Click(int tenant, string textBoxParam)
        {
            var gateWayTesterResult = new GateWayTesterResult();
            try
            {
                //dynamic param = LogitudeXmlSerializer.JsonConvertDeserializeObject(textBoxParam);
                // var tenant = (int)param.MyTenant;
                string fileSystem1000 = textBoxParam;
                ///Response.Clear();
                var mySystem1000FlatFileAnalyser = new System1000FlatFileAnalyser();
                mySystem1000FlatFileAnalyser.Analyse(null, fileSystem1000, null);

                
                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax(mySystem1000FlatFileAnalyser.MyResultLoadFlatFile); ;


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {
                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
        private GateWayTesterResult _ButtonReverseDueDate_Click(int tenant, string textBoxParam)
        {
            var gateWayTesterResult = new GateWayTesterResult();
            try
            {
                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<ParamBasic>(textBoxParam);
                // var tenant = (int)param.MyTenant;
                string AccountId = param.MyGLAccId;
                ///Response.Clear();
                var myDueLocalBalanceService = new DueLocalBalanceService();
                var listDiff = myDueLocalBalanceService.ReverseEngineer(tenant, AccountId);
                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<DueLocalBalanceDiffM>>(listDiff);

                
            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {
                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
        private GateWayTesterResult _ButtonFixDueLocalBalance_Click(int tenant, string textBoxParam)
        {
            var gateWayTesterResult = new GateWayTesterResult();
            try
            {
                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<ParamBasic>(textBoxParam);
                var myDueLocalBalanceService = new DueLocalBalanceService();
                myDueLocalBalanceService.ReBuild(tenant, "");
            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {
                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
        private GateWayTesterResult _ButtonReverseTotalFIXControl_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<ParamBasic>(textBoxParam);

                if (param.ChangeSupplier2Customer)
                {
                    var theWholePeriodReverseEngineerTotalByMonth_ControlAccountService = new TheWholePeriodReverseEngineerTotalByMonth_ControlAccountService();
                    theWholePeriodReverseEngineerTotalByMonth_ControlAccountService.ChangeSupplier2Customer(param.MyDate, param.MyTenant);

                }
                else
                {
                    
                    var s = new ReverseEngineerTotalByMonth_ControlAccountService(param.MyDate, param.MyTenant, param.MyGLAccId);
                    s.FixDbIntegrityFromLedgeToTotal(/*param.ChangeSupplier2Customer*/);
                    gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<GLAccountTotalByMonthsDTO>>(s.CompareReport.GLAccountTotalByMonthsList);
                }
                


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {


                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
        private GateWayTesterResult _ButtonReverseTotalFIX_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {


                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<ParamBasic>(textBoxParam);
                var s = new ReverseEngineerTotalByMonthService(param.MyDate, param.MyTenant, param.MyGLAccId);
                s.FixDbIntegrityFromLedgeToTotal();

                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<GLAccountTotalByMonthsDTO>>(s.CompareReport.GLAccountTotalByMonthsList);


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {


                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
        private GateWayTesterResult _ButtonReverseGLBalanceFIX_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {


                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<ParamBasic>(textBoxParam);
                var MyTenant = (int)param.MyTenant;
                var MyDate = (DateTime)param.MyDate;

                var s = new ReverseEngineerGLAccountBalance(/*MyDate,*/ MyTenant);
                s.FIXCheckDbIntegrity();

                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<GLAccountBalanceDTO>>(s.CompareReport.GLAccountBalanceList);


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {


                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
        private GateWayTesterResult _ButtonReverseTrans_Click(int tenant, string textBoxParam)
        {
         
            var gateWayTesterResult = new GateWayTesterResult();


            try
            {


                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<ParamBasic>(textBoxParam);
                var s = new ReverseEngineerLedgerTransactionService(param.MyDate, param.MyTenant);
                s.CheckDbIntegrity();

                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<JournalLineLedgerDTO>>(s.CompareReport.rows);


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {


                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }



        private GateWayTesterResult ButtonLoadConsolTaxRep_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {


                    string fileConsolidatedTaxReport = textBoxParam;


                    var myConsolidatedTaxReportFlatFileAnalyser = new ConsolidatedTaxReportFlatFileAnalyser();
                    myConsolidatedTaxReportFlatFileAnalyser.Analyse(null, null, fileConsolidatedTaxReport);

                        gateWayTesterResult.JsonOut = "TaxRep Ok";




            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }



        GateWayTesterResult _ButtonReverseTotal_Click(int tenant ,string _TextBoxParam)
        {
            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                

                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<ParamBasic>(_TextBoxParam);
                var s = new ReverseEngineerTotalByMonthService(param.MyDate, param.MyTenant, param.MyGLAccId);
                s.CheckDbIntegrity();
                
                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<GLAccountTotalByMonthsDTO>>(s.CompareReport.GLAccountTotalByMonthsList);
                ///ReloadGrid(SerializeObjectByte);

            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {


                gateWayTesterResult.Log= LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
    }



    public class GateWayTesterResult
    {
        public string JsonOut { get; set; }
        public string Log { get; set; }
        public string ExceptionMess { get; set; }

    }
    public class ParamBasic
    {
        public int MyTenant { get; set; }
        public DateTime MyDate { get; set; }

        public string MyGLAccId { get; set; }

        public bool ChangeSupplier2Customer { get; set; }

    }
}
