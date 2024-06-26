using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Batch;
using Logitude.Accounting.BL.CoreBL.BuildTenant.MumpsOpenReconcile;
using Logitude.Accounting.BL.CoreBL.Fix;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.BL.CoreBL.Testers;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Utils;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Update.PatchDistribution;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Logitude.Update.SandBox
{
    public partial class FormAccountingTester : Form
    {
        public FormAccountingTester()
        {
            InitializeComponent();
            LoggedContactResolver.RegisterLoggedContactUtil();

        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int tenant = int.Parse(_TBTenant.Text);
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var CSV = openFileDialog1.FileName;
            string csvText = File.ReadAllText(CSV);
            var openRecoDataValidation = new OpenRecoDataValidationService();
            openRecoDataValidation.GetDataAndValid(csvText);

            var allrows = openRecoDataValidation.GoodRows;

            var goodRowsGoodAccount = allrows.Where(r => !openRecoDataValidation.BadAccounts.Contains(r.InternalNumber)).ToList();

            LogMessagingUtil.Instance.AppendLine($"GoodRows {allrows.Count} ::: goodRowsGoodAccount {goodRowsGoodAccount.Count()}");


            RealOpenAmountService.Get(tenant);
            LogMessagingUtil.Instance.AppendLine($"RealOpenAmount.MyList.Count {RealOpenAmountService.MyList.Count} ");
            if (MessageBox.Show("to continue?", "", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
            {
                return;
            }


            foreach (var itemGroug in goodRowsGoodAccount.GroupBy(r => r.InternalNumber))
            {

                
                    var updateOpenReconcile = new UpdateOpenReconcileService();
                    updateOpenReconcile.DoAccount(tenant, itemGroug.ToList());
                
            }

            ///List<MMPSDataM> list100 = Do100(tenant, goodRowsGoodAccount);
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("done !!!!!!!!!");
        }

        private static List<MMPSDataM> Do100(int tenant, List<MMPSDataM> goodRowsGoodAccount)
        {
            var list100 = new List<MMPSDataM>();
            foreach (var itemGroug in goodRowsGoodAccount.GroupBy(r => r.InternalNumber))
            {

                list100.AddRange(itemGroug.ToList());
                if (list100.Count > 100)
                {
                    var updateOpenReconcile = new UpdateOpenReconcileService();
                    updateOpenReconcile.DoAccount100(tenant, list100);
                    list100.Clear();
                }
            }
            if (list100.Count > 0)
            {
                var updateOpenReconcile = new UpdateOpenReconcileService();
                updateOpenReconcile.DoAccount100(tenant, list100);

            }

            return list100;
        }

        private void tESTADHOKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //NewMethod();return;

            var fixJournaRecolService = new FixJournaRecolService();

            fixJournaRecolService.Fix("1-16314448", 10, false);
            return;

            //var changeGLAccount2IsMultiCurrencyService = new ChangeGLAccount2IsMultiCurrencyService();
            //changeGLAccount2IsMultiCurrencyService.Change2MultiCurrency("1-18459", 1255 );
            //return;
            ///RevaluationBatch revaluationBatch = new RevaluationBatch();
            //revaluationBatch.RunAllOpenRevaluations(28);
            //return;
            List<string> Last_journalBufferKeys = new List<string>();
            JournalApproveService.WorkWithoutQueue(62, "1-16309384", ref Last_journalBufferKeys);

            return;

            var myGateWayTester = new GateWayTester();
            myGateWayTester.ImmediateYearTransferthod(20, 62);
            return;

            YearTest();
            return;
            var myReverseEngineerCashBook = new ReverseEngineerCashBook(69);
            myReverseEngineerCashBook.CheckDbIntegrity();
            return;
            GLaccountCreateTester();

            return;
        }

        private static void NewMethod()
        {
            var accountingContext = AccountingContext.GetContext(3);
            var myLedgerTransactionUpdateService = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, IContext>(), 3);

            var listTransactionId = new List<string>() { "1-159389115", "1-126188886" };
            myLedgerTransactionUpdateService.UpdateInReconcileProgress(listTransactionId, 3, false);
            return;
        }

        public  void YearTest()
        {
            var parameterArgs = new BatchYearlyFIXParams() { MyFixType="", Year=2015, Tenant=3 };
            var accountingContext = AccountingContext.GetContext(parameterArgs.Tenant);
            try
            {
                int maxMonth = 12;
                if (false && parameterArgs.Year == DateTime.Now.Year)
                {
                    maxMonth = DateTime.Now.Month;
                }
                for (int month = 1; month <= maxMonth; month++)
                {
                    DateTime dateTime = new DateTime(parameterArgs.Year, month, 1);
                    switch (parameterArgs.MyFixType)
                    {
                        case "ReverseEngineerTotalByMonthService":
                        default:
                            {
                                try
                                {
                                    var s = new ReverseEngineerTotalByMonthService(dateTime, parameterArgs.Tenant, null);
                                    s.FixDbIntegrityFromLedgeToTotal();
                                }
                                catch (Exception E) when  (E.Message == ReverseEngineerTotalByMonth_ControlAccountService.const_isokNothingDone )
                                {

                                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("const_isokNothingDone");
                                    //throw;
                                }

                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "BatchYearlyFIXService", new DateTime(2021, 7, 1));
                throw;

            }

        }
        private static void GLaccountCreateTester()
        {
            string json1 = "{'Id':null,'Tenant':'4','InternalNumber':'020700004','AccountTypeCode':'3','DisplayNumber':'020700004','ExternalDisplayNumber':'020700004','EncodeBase64NVARCHARFieldsBy':'windows-1255','LocalName':'4/fx+CD26fjp6iD55eXp9Q==','EnglishName':'Dachser Spedition Ag - swiss','IsMultiCurrency':false,'Inactive':null,'ChartOfAccountsId':'bla','ChartOfAccountsTypeCode':'4','CurrencyCode':'NIS','CurrencyId':null,'RevenueExpenseType':'3','IsControlAccount':0,'ChartOfAccountType':'4','ChartOfAccountsCode':'0207','ParentAccountByCurrency':null,'ReconcileMethodCode':'0','utomaticReconcileId':null,'PreviousLocalName':null,'PreviousLocalNameChangeDate':null,'PreviousEnglishName':null,'PreviousEnglishNameChangeDate':null,'PreviousNo':null,'PreviousNoChangeDate':null,'PreviousChartOfAccountId':null,'PreviousChartOfAccountChangeDate':null,'BalanceInLocCurrencyId':null,'RevaluationEnable':null,'SearchFields':null,'IsVATExempt':false}";


            json1 = "{'Id':null,'Tenant':'4','InternalNumber':'020702161','AccountTypeCode':'3','DisplayNumber':'020702161','ExternalDisplayNumber':'020702161','EncodeBase64NVARCHARFieldsBy':'windows-1255','LocalName':'4uzl4ewg4Ozs6eDw8SDs5eLp8ejp9 / Eg9OjkIODsIOjpIOPp','EnglishName':'Global Alliance Logistics Pte Ltd\','IsMultiCurrency':false,'Inactive':null,'ChartOfAccountsId':'bla','ChartOfAccountsTypeCode':'4','CurrencyCode':'NIS','CurrencyId':null,'RevenueExpenseType':'3','IsControlAccount':0,'ChartOfAccountType':'4','ChartOfAccountsCode':'0207','ParentAccountByCurrency':null,'ReconcileMethodCode':'0','utomaticReconcileId':null,'PreviousLocalName':null,'PreviousLocalNameChangeDate':null,'PreviousEnglishName':null,'PreviousEnglishNameChangeDate':null,'PreviousNo':null,'PreviousNoChangeDate':null,'PreviousChartOfAccountId':null,'PreviousChartOfAccountChangeDate':null,'BalanceInLocCurrencyId':null,'RevaluationEnable':null,'SearchFields':null,'IsVATExempt':false}";


            GLAccountPM entityPM
            = LogitudeXmlSerializer.JsonConvertDeserializeTObject<GLAccountPM>(json1);
            var MyContext = AccountingContext.GetContext(entityPM.Tenant);
            GLAccountUpdateService service = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPM, true);
        }

        private void agingFixRepoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int tenant = 1;
            string reconcileId = "1-100";

            var agingReportRebulidTesterService = new AgingReportRebulidTesterService();
            agingReportRebulidTesterService.RebulidReconcile(tenant, reconcileId);
        }

        private void loadBigJournalFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int tenant = int.Parse(_TBTenant.Text);
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            string jsonText = File.ReadAllText(openFileDialog1.FileName);
            JournalPM entityPM = LogitudeXmlSerializer.JsonConvertDeserializeTObject<JournalPM>(jsonText);
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
                JournalUpdateService service = new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                service.Update(entityPM, true);
                scope.Complete();
            }
        }

        private void fixJournalToolStripMenuItem_Click(object sender, EventArgs e)
        {

                        
            string Id = "1-15275662";
            int Tenant = 29;
            string JournalNumber = "1046993";
            var accountingContext = AccountingContext.GetContext(Tenant);
            var myLedgerTransactionUpdateService = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, IContext>(), Tenant);

            var listTransactionId = new List<string>() {"1-126134019","1-126160652","1-126160668","1-126160730","1-93363452" };
            myLedgerTransactionUpdateService.UpdateInReconcileProgress(listTransactionId, Tenant, false);
            



            return;

            var fixJournaRecolService = new FixJournaRecolService();
            fixJournaRecolService.FixByJournalNumber(JournalNumber, Tenant);
        }

        private void intgrityCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountingIntegrityInParam param = new AccountingIntegrityInParam()
            {
                Tenant = 118 ,
                FromMonthInclusive =  new DateTime( DateTime.Now.Year,1,1),
                ToMonthInclusive = DateTime.Now,

            };
            string serializeObjectstring = "";
            try
            {



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
            }
        }
    }
}
