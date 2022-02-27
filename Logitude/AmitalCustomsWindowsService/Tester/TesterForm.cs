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
using System.Windows.Forms;
using AmitalCustomsWindowsService.Tester.CustomMessage;
using Unifreight.Data.AmitalModel;
using CustomsWorkerRole;
using CustomsWorkerRole.Test;
using AmitalCustomsWindowsService.Utils;
using AmitalCustomsWindowsService.Tester.SU;
using System.Xml.Linq;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.Messaging.Maman;
using System.Threading;
using Logitude.Customs.BL.Messaging;
using System.Net;
using CommunicationWorkerRole;
 using Logitude.Server.Tools.Helpers;
using WebFreight.Web.CustomWebServices;
using Logitude.CustomsMessaging.U2L.CommDec;
using Logitude.CustomsMessaging.ResponseServices;
using System.Xml.Serialization;
using Logitude.CustomsMessaging.MessagingServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Xml;
using Logitude.CustomsMessaging.RabbitMQ;
using Logitude.Customs.BL.CloseTables;
using Simplog.Global.Data.GlobalModel.Repositories;
//using System.Windows.Interactivity;

namespace AmitalCustomsWindowsService.Tester
{
    public partial class TesterForm : Form
    {
        public TesterForm()
        {
            InitializeComponent();
            TraceListener debugListener = new MyTraceListener(this.textBoxLogger);
            Debug.Listeners.Add(debugListener);
            _CBWorkerRole.Items.Add("CustomsCommandGetCustomRequestWR");
            _CBWorkerRole.Items.Add("CustomsCommandSignRequestWR");
            _CBWorkerRole.Items.Add("CustomsCommandSendDCAWR");
            _CBWorkerRole.Items.Add("CustomsCommandSendDCAUploadStatusWR");
            _CBWorkerRole.Items.Add("CustomsCommandSendWSReceiveCorrelationWR");
            _CBWorkerRole.Items.Add("CustomsCommandDownloadDcaReceiveCorrelationWR");
            _CBWorkerRole.Items.Add("CustomsCommandAnalyzeResponseWR");
            _CBWorkerRole.Items.Add("SendWEBAPIMessage2MamanWR");
            _CBWorkerRole.Items.Add("FTPToAnalyzeQueueWR");
            _CBWorkerRole.Items.Add("CustomsAnalyzeQueueWR");
            _CBWorkerRole.Items.Add("CustomsSchedularWR");
            _CBWorkerRole.Items.Add("RabbitMQReceiveWR");

            Debug.WriteLine("Env:");
            Debug.WriteLine(LogitudeSettings.LogitudeURL);

            var t = new Thread(GetENV);
            t.Start();
            //GetENV();an
            ///customsMessagingSheetWRToolStripMenuItem_Click(this, null);
        }

        private static void GetENV()
        {
            
            var pmCustomsSetting = Logitude.Customs.BL.EntityQueryServices.CustomsSettingQueryService.GetSettingByTenant(1);
            var jsonSetting = ProxyUtil.JsonConvertSerialize(pmCustomsSetting);
            Debug.WriteLine(jsonSetting);
        }

        private void BlobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setBlob();
        }

        private void setBlob()
        {
            string filename = Guid.NewGuid().ToString() + ".xml";
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            Logitude.Server.Tools.Communications.SetBolb(1, filename, "Amital", GetByte());
            stopwatch.Stop();
            Debug.WriteLine("SetBlob:" + filename + ":Took:" + stopwatch.Elapsed.ToString());
        }

        private byte[] GetByte()
        {
            return File.ReadAllBytes(toolStripTextBoxBolbXml.Text); 
            
        }

        private void multiBlobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                 setBlob();
            }
        }

        

        private void toDcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new Send380Form())
            {
                frm.ShowDialog();
            }
        }

        

        private void stToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (
            var cntxt = AmitalContext.GetContext(GetTenant());//)
            {
                var rec = cntxt.CCUTAXES.FirstOrDefault();
            }
        }

        private void updateCloseTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //    var d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsWorkerRole.UpdateClosedTablesWR>(1, 1, true) { ServiceStarted = true };
          //  d.ExecuteTask(); 
        }

        

        

        private void customsMessagingSheetWRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsWorkerRole.CustomsMessagingSheetWR>(10, 1, checkBoxDebugMode.Checked) { ServiceStarted = true };
            d.ExecuteTask(); 
        }

        private void sendDataToExternalServicesWRToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsWorkerRole.SendDataToExternalServicesWR>(10, 1, checkBoxDebugMode.Checked) { ServiceStarted = true };
            d.ExecuteTask(); 
        }

        async Task DownloadDcaMessageSheetWRAsync()
        {

            var d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsWorkerRole.DownloadDcaMessageSheetWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text, GetTenant()) { ServiceStarted = true, FromTesterForm=true };
            d.ExecuteTask();

        }

        private async void downloadDcaMessageSheetWRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsWorkerRole.DownloadDcaMessageSheetWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text, GetTenant()) { ServiceStarted = true, FromTesterForm = true };
            d.ExecuteTask();
        }

        private int GetTenant()
        {
            int i;
            if (int.TryParse(_TBTenant.Text, out i))
            {
                return i;
            }
            throw new Exception("Please insert tenant in setting !!!");
            
        }

        private void _CBInterfaceID_Click(object sender, EventArgs e)
        {

        }

        private void statisticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var AllQ = CustomsWorkerRole.Utils.ServiceBusUtil.ShowAll();
                Debug.WriteLine(AllQ);
            }
            catch (Exception eee)
            {

                Debug.WriteLine("CustomsWorkerRole.Utils.ServiceBusUtil.ShowAll failed :" + eee.ToString());
            }

        }

        

       
       
       

        private void testItToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dynamic d;
            switch (_CBWorkerRole.Text)
            {
                case "CustomsSchedularWR":
                    {
                        d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsSchedularWR>(
                                        10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text)
                        { ServiceStarted = true, };
                    }
                    break;
                case "CustomsCommandGetCustomRequestWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsCommandGetCustomRequestWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text) { ServiceStarted = true, };
                    break;
                case "CustomsCommandSignRequestWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsCommandSignRequestWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text) { ServiceStarted = true, };
                    break;
                case "CustomsCommandSendDCAWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsCommandSendDCAWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text) { ServiceStarted = true, };
                    break;
                case "CustomsCommandSendDCAUploadStatusWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsCommandSendDCAUploadStatusWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text) { ServiceStarted = true, };
                    break;
                case "CustomsCommandSendWSReceiveCorrelationWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsCommandSendWSReceiveCorrelationWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text) { ServiceStarted = true, };
                    break;
                case "CustomsCommandDownloadDcaReceiveCorrelationWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsCommandDownloadDcaReceiveCorrelationWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text) { ServiceStarted = true, };
                    break;
                case "CustomsCommandAnalyzeResponseWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsCommandAnalyzeResponseWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text) { ServiceStarted = true, };
                    break;
                case "SendWEBAPIMessage2MamanWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<SendWEBAPIMessage2MamanWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text)
                    { ServiceStarted = true, };
                    break;
                case "FTPToAnalyzeQueueWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<FTPToAnalyzeQueueWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text)
                    { ServiceStarted = true, };
                    break;


                case "RabbitMQReceiveWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<RabbitMQReceiveWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text)
                    { ServiceStarted = true, };
                    break;

                case "CustomsAnalyzeQueueWR":
                    d = new AmitalCustomsWindowsService.BL.WorkerOnce<CustomsAnalyzeQueueWR>(
                10, 1, checkBoxDebugMode.Checked, _CBInterfaceID.Text)
                    { ServiceStarted = true, };
                    break;
                    
                default:
                    return;
            }

            //d.WorkerQueueType = checkBoxMQ.Checked ? Logitude.Server.Tools.WorkerQueueType.RabbitMQ : Logitude.Server.Tools.WorkerQueueType.DB;
            if (!String.IsNullOrWhiteSpace(textBoxOverrideRMQ.Text))
            {
                d.OverrideRMQ = textBoxOverrideRMQ.Text;
            }
            Logitude.Server.Tools.WorkerRoleServiceLocator.PleaseShutDown = false;
            if (checkBoxDebugMode.Checked)
            {
                Task.Run(async () => {

                    var sw = Stopwatch.StartNew();
                    while (sw.Elapsed<TimeSpan.FromSeconds(120))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2));
                        Application.DoEvents();
                    }    
                    
                    Logitude.Server.Tools.WorkerRoleServiceLocator.PleaseShutDown = true;

                });
            }
            d.ExecuteTask(); 

        }

        private void viaUpdateCloseTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //var wr = new UpdateClosedTablesWR();
                //wr.WorkOnce();
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString()); 
            }
            
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rabbitMQReceiveWR = new RabbitMQReceiveWR();
            var customRabbitMQQueue = new CustomRabbitMQQueue();
            var allQueueDetails = customRabbitMQQueue.GetAllQueueDetails()
             .Where(r => r.AnalyzeQueueService != AnalyzeMQQueueServiceEnum.none)
            .ToList();
            AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
            string log = "";
            bool success = false;
            var c=allQueueDetails.FirstOrDefault(r => r.Code == "uw2l");
            rabbitMQReceiveWR.Exec(
                customRabbitMQQueue, c,
                analyzeQueueRepository,
                "NYC1MMYLAEOS6QWZ44GZPA00000000",3,"", out log, out success);
            ;
            return;
            clsTester.TestUpdateLOGITUDE_FILE();
            //clsTester.GetListByCourierHAWB();

            return;
            clsTester.MultiProccessTestLockTab();
            string customsResponseXml = File.ReadAllText(@"C:\Users\itzik\Desktop\zevel\1-43468729.xml");
            WebFreight.Web.CustomWebServices.Testers.Tester.DeSerializeObject3052(customsResponseXml);
            customsResponseXml = File.ReadAllText(@"C:\Users\itzik\Desktop\zevel\1-20980020.xml");
            WebFreight.Web.CustomWebServices.Testers.Tester.DeSerializeObject3052(customsResponseXml);

            //http://lodmpn05/DSVWebFreightDebug/api/DeclarationWebService/GetDeclarationMandatoryTicketList/?parentEntityId=1-92241&parentEntityCode=Declaration
            //DbContextBaseUtil.ToLog = true;
            //WebFreight.Web.CustomWebServices.Testers.Tester.TestNOWait();

            //clsTester.GetDeclarationMandatoryTicketList(parentEntityId: "1-92241", parentEntityCode: "Declaration");
            //clsTester.TestLockTab();
            //return;
            clsTester.TestNull();
        }

        private void signUpWorkerRoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{


                string email; string Company;
                using (var signUpForm = new SignUpForm())
                {
                    if (signUpForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                    email = signUpForm.Email;
                    Company = signUpForm.Company;

                }

                var signUpWorkerRole = new CommunicationWorkerRole.SignUpWorkerRoleWinService();
                var dardcODED = true;
                if (dardcODED)
                {
                    var password = signUpWorkerRole.CreatTenant(email, Company);
                    Logger.LogMe("CreatTenant:email=" + email + ":Pass=" + password, false);
                }
                else
                {
                    signUpWorkerRole.WorkOnceSuppressClearQ();
                }
                Debug.WriteLine("signUpWorkerRole.WorkOnce END !!");
            //}
            //catch (Exception ee)
            //{
            //    Logger.LogMe(ee.ToString(), true); 
            //    //throw;
            //}
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var parameters = new CommunicationWorkerRole.EmailParameters()
            {
                From = "admin@fnarsoft.com",
                To = "jalal@logitudeworld.com",//;itzik@amital.co.il;YaronC@AMITAL.CO.IL",
                Cc = "",
                Bcc = "",
                Subject = "SignUp complete successfully for " ,
                Body = "emailbody",
            };
                    parameters.To += ";itzik@amital.co.il;YaronC@AMITAL.CO.IL";
                    parameters.Tenant = 92;
            Debug.WriteLine(parameters.To);
            CommunicationWorkerRole.EmailingHelper.SendEmail(parameters);
        }

        private void send1966ByDCAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsTester.SendDCA19666(); 
        }

        private void logErrorSmtpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                throw new Exception("logErrorSmtpToolStripMenuItem_Click");
            }
            catch (Exception e1)
            {

                Logger.LogMe(e1.ToString(), true);  
            }
        }

        private void communicationLogWorkerRoleWinServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var wr = new CommunicationWorkerRole.CommunicationLogWorkerRoleWinService();
            wr.WorkOnce();
            Debug.WriteLine("CommunicationWorkerRole.WorkOnce END !!");
        }

        private void repushQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var customsRequestsSheetsId =_txCustomsRequestsSheetsId.Text;
            clsTester.RepushQ(customsRequestsSheetsId);
        }

        private void downLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void passwordCheckServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var passwordCheckService = new PasswordCheckService();
        }

        private void _tsComboBoxTenant_Click(object sender, EventArgs e)
        {

        }

        private void downloadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return ;
            }
            var dcaFile = openFileDialog1.FileName;
            var bytsDcaFile = File.ReadAllBytes(dcaFile);
            var base64InnerUTF8 =Base64InnerUTF8(dcaFile, new StringBuilder());
            openFileDialog1.Dispose();
            dcaFile=Path.GetFileName(dcaFile);

            
            CustomsWorkerRole.DCA.DCATester.DownloadFile(GetTenant(), dcaFile, 
                //bytsDcaFile
                base64InnerUTF8 
                );
            
        }
        private static string Base64InnerUTF8(string CurFileName, StringBuilder sbLog)
        {
            var myEncoding = System.Text.Encoding.UTF8;
            sbLog.AppendLine("!defaultEncoding Change to UTF8= " + myEncoding.EncodingName);

            sbLog.AppendLine("ReadAllText (UTF8) ..");
            string txt = File.ReadAllText(CurFileName, myEncoding);
            var checkParse = false;
            if (checkParse)
            {
                sbLog.AppendLine("XDocument.Parse(txt)..");
                var myXDocument = XDocument.Parse(txt);
            }
            sbLog.AppendLine("ToBase64_EncodeUTF8 ..");
            var base64String = ToBase64_EncodeByEncoding(txt, myEncoding);

            return base64String;
        }

        static string ToBase64_EncodeByEncoding(string data, Encoding myEncoding)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];

                //Standard

                encData_byte = myEncoding.GetBytes(data);

                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }
        private void IIGGatewayServiceURLStripMenuItem_Click(object sender, EventArgs e)
        {


            CustomsWorkerRole.Test.clsTester.SpeedTest(this.IIGGatewayServiceURLStripMenuItem.Text, GetTenant());

            

            Task[] taskArray = new Task[10 ];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) =>
                {
                    
                    //data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                    //Console.WriteLine("Task #{0} created at {1} on thread #{2}.",
                    //                  data.Name, data.CreationTime, data.ThreadNum);
                    for (int j = 0; j < 2; j++)
                    {
                        CustomsWorkerRole.Test.clsTester.SpeedTest(this.IIGGatewayServiceURLStripMenuItem.Text, GetTenant());    
                    }
                },
                                                     i);
            }
            Task.WaitAll(taskArray);     
        }
        LoadTesterForm _LoadTest;

        

        private void loadTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_LoadTest==null)
            {
                _LoadTest = new LoadTesterForm();
                _LoadTest.Initialize(GetTenant()); 
                _LoadTest.Show();    
            }
            

        }

        

        

        

        private void testSpeedTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IIGGatewayServiceURLStripMenuItem_Click(sender, e);
        }

        private void debugStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (_CBWorkerRole.Text)
            {
                case "CustomsSchedularWR":
                    {
                        var customsSchedularWR = new CustomsSchedularWR();
                        customsSchedularWR.WorkOnce();
                    }
                    break;
                case "CustomsAnalyzeQueueWR":
                    {
                        var customsAnalyzeQueueWR = new CustomsAnalyzeQueueWR();
                        customsAnalyzeQueueWR.CheckParamsAndExec(_TBID.Text, _CBInterfaceID.Text, GetTenant());

                    }
                    break;
                case "SendWEBAPIMessage2MamanWR":
                    {
                        var SendWEBAPIMessage2MamanWR = new SendWEBAPIMessage2MamanWR();
                        SendWEBAPIMessage2MamanWR.DebugStep(_TBID.Text, _CBInterfaceID.Text, GetTenant());

                    }
                    break;
                default:
                    CustomsWorkerRole.Test.clsTester.DebugRQStep(
                _CBInterfaceID.Text, GetTenant(), _TBID.Text,
                _CBWorkerRole.Text);
                    break;
            }
            
            


        }

        private void reqSheetStatisticToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var s = new ReqSheetStatisticClass();
            s.GetStatisticSub();
            ReqSheetStatisticClass.DoIt(
                (emailbody, subj) =>
                {
                    var parameters = new CommunicationWorkerRole.EmailParameters()
                    {
                        From = "admin@fnarsoft.com",
                        SwitchFromWithUserNameIfValid = true,
                        To = "itzik@amital.co.il;YaronC@AMITAL.CO.IL;bbwrweim@mailparser.io",
                        Cc = "",
                        Bcc = "",
                        Subject = subj,
                        Body ="ReqSheetStatistic " + Environment.MachineName + "/ " + Environment.UserDomainName +
                        emailbody,
                    };
                    //parameters.To += ";itzik@amital.co.il;YaronC@AMITAL.CO.IL";
                    //parameters.Tenant = 92;
                    Debug.WriteLine(parameters.To);
                    Debug.WriteLine(emailbody);
                    
                    CommunicationWorkerRole.EmailingHelper.SendEmail(parameters);
                },true);
        }

        private void memLeakToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void grantCCUToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Move to Logitude.Update");
                
                //Debug.WriteLine(CustomsWorkerRole.Test.clsTester.GrantCCUTo(GetTenant()));
            }
            catch (Exception ee)
            {

                Debug.WriteLine(ee.ToString());
                MessageBox.Show("maybe tenant not exist !!!");
            }
            
        }

        private void buildDcaAggregrateTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            CustomsWorkerRole.Test.clsTester.BuildDcaAggregrateFile(@"c:\temp\");
            
        }

        private void frizGetDecXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tenant = GetTenant();
            var DecId = "1-2642";
            var xml = CustomsWorkerRole.Test.clsTester.GetDeclarationXml(tenant, DecId);
            var file= Path.Combine(@"c:\",DecId +".xml");
            File.WriteAllText(file, xml);
            Debug.WriteLine(file); 

            var checkXml = @"C:\Program Files (x86)\Microsoft Visual Studio 11.0\DeclarationPM.FromXsd.xml";
            var xmlCheck=File.ReadAllText(checkXml);
            CustomsWorkerRole.Test.clsTester.PutDeclarationXml(xmlCheck);

        }

        private void speedTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rePushAnalyzeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            CustomsWorkerRole.Test.clsTester.ReQueue(
                _CBInterfaceID.Text, GetTenant(), _TBID.Text
                //_CBWorkerRole.Text
                );
        }

        private void loadTestAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var createCustomFile = new Logitude.CustomsMessaging.Testers.LoadTest.CreateCustomFileService();
            
            
            //var fileNo = createCustomFile.SendHybridInterface(createCustomFile.GetShipmentAM());

            //Debug.WriteLine("Created File :" + fileNo);

            //var resDec = createCustomFile.DeclarationUpsert(fileNo);
            //var declarationId = createCustomFile.GetDeclarationId(fileNo);

            //createCustomFile.DeletSI(declarationId);
            //createCustomFile.PutCopyDeclaration(declarationId, "");
            //createCustomFile.SendDeclaration(declarationId);
            //createCustomFile.PostPrintRequestRequest(declarationId);
            //DeclarartionController.PutCopyDeclaration



        }

        private void sendInBatchToolStripMenuItem_Click(object sender, EventArgs e)
        {

            CustomsWorkerRole.Test.clsTester.SendDeclarationsThatCanResendInBatch();
        }

        private void restoreDecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tst = new CustomsWorkerRole.Test.clsTester();
            tst.RestoreAlDec();

        }

        private void Send8373ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tst = new CustomsWorkerRole.Test.clsTester();
            tst.Send8373();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DbContextBaseUtil.ToLog = checkBox1.Checked;
        }

        private void clearCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logitude.BL.Helpers.TableLastUpdateClass.UpdateCacheTableHistory();
            Logitude.BL.Helpers.TableLastUpdateClass.UpdateSystemMetaDataHistory();
        }

        private void uploadMultiToolStripMenuItem_Click(object sender, EventArgs e)
        {

           
        }

        private void checkUniqueUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void UploadMultiDoit_Click(object sender, EventArgs e)
        {
            var uploadMulti = new CustomsWorkerRole.Test.UploadMulti();
            uploadMulti.TestMulti();
        }

        private void toolStripMenuItemcheckUniqueUpload_Click(object sender, EventArgs e)
        {
            string input = checkUniqueUploadTextBox1.Text;
            var di = new System.IO.DirectoryInfo(input);
            if (di.Exists)
            {
                var fiList = di.GetFiles("*").ToList();
                var notUniqeNames =
                    //(from fi in fiList
                    //                 group fi by fi.Length into g
                    //                 where g.Count() > 1
                    //                 select //g.Select ( r=>r.Name)                                     //g.SelectMany(g1 => g1.Name)
                    //                g.SelectMany( r=>r.Name)

                    //     ).ToList();
                    fiList
                    .GroupBy(r => r.Length)
                    .Where(g => g.Count() > 1)
                    .SelectMany(r => r)
                    .Select( fi=> fi.Name)
                    .ToList()
                    ;
               
                    
                    ;
                notUniqeNames.ForEach(r =>
                {
                    Debug.WriteLine(r);
                }
                );

            }
            else
            {
                Debug.WriteLine("folder Not Exist ");
            }
        }

        private void fTPCommunicationWorkerRoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool testIt = false;
            if (testIt)
            {
                int Tenant = GetTenant();
                var tst = new CustomsWorkerRole.Test.clsTester();
                tst.FtpTester(Tenant);
            }
            var d = new AmitalCustomsWindowsService.BL.WorkerOnce<CommunicationWorkerRole.FTPCommunicationWorkerRoleWinService>(10, 1, checkBoxDebugMode.Checked) { ServiceStarted = true };
            d.ExecuteTask();
        }
        private void asDataSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tst = new CustomsWorkerRole.Test.clsTester();
            tst.TestAsDataSet("1306");

        }

        private void _CBWorkerRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void hAWBALDARMamanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebAPINetworkCredentialMessage.OVSUpdateHawbStatusTester();
            //WebAPI2BearerMamanMessage.OVSUpdateHawbStatusTesterNotWork();

            //MamanBaldarTest();

        }

        private static void MamanBaldarTest()
        {
            var wr = new SendWEBAPIMessage2MamanWR();
            string data =
                @"{""BaldarCode"":""2026"",""BaldarAwb"":""baldarAWb35"",""AirlineAwbPref"":""001"",""Master"":22222211,""Awb8"":88888888,""HawbExtnd"":""abcd1234 update"",""AirlineCode"":""1X"",""FltNo"":null,""FltDate"":null,""LandTime"":null,""DecNoOfPackags"":1,""DecWeight"":100.1,""DolarValue"":200.12345,""StoreTypeReq"":""67"",""Description"":""Description1 - 2026 update"",""CustomerName"":""Miriam"",""CustomerAddress"":""Ein Gedi"",""CustomerPhone"":""026765544"",""DestLineDesc"":""DestLineDesc"",""BaldarMessageTime"":""2018 - 10 - 16T17: 38:33.1365366 + 03:00"",""BaldarHp"":""2323231"",""OpenBaldarAwbDate"":""2018 - 10 - 15T17: 38:33.1365366 + 03:00"",""ResponseStatusCode"":null,""ResponseStatusMsg"":null}";
            var service = new WebAPI2BearerMamanMessage(new CourierWEBAPICommSettings()
            {
                DeclarationId = "",
                username = "F_unitedf",
                password = "Unit2019",
                //URIToken = @"https://maman.wsfreeze.co.il/WebAPIExt/Token", //HTTP/1.1;
                //URIBaldarCreateECTHRMessgae = @"https://maman.wsfreeze.co.il/WebAPIExt/api/baldar/CreateECTHRMessgae",

                //URIToken = @"http://localhost:52013/api/Token",
                //URIBaldarCreateECTHRMessgae = @"http://localhost:52013/api/MamanCreateECTHRMessgae",

                URIToken = @"http://192.116.221.103/WebApp3PartySimulator/api/Token",
                URIMethod = @"http://192.116.221.103/WebApp3PartySimulator/api/MamanCreateECTHRMessgae",


                Tenant = 1


            });


            var res = service.PostIt(data);
        }

        private void mamanCreateECSpclMessgaeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var wr = new SendWEBAPIMessage2MamanWR();
            string data =
                @"{
  ""ActionCode"": ""sample string 1"",
  ""BaldarAwb"": ""sample string 2"",
  ""BaldarHp"": ""sample string 3"",
  ""OpenBaldarAwbDate"": ""2018-12-12T15:40:40.6169976+02:00"",
  ""ResponseStatusCode"": 1,
  ""ResponseStatusMsg"": ""sample string 4"",
  ""SpLabel1"": ""sample string 5"",
  ""SpLabel2"": ""sample string 6"",
  ""SpLabel3"": ""sample string 7"",
  ""SpLabel4"": ""sample string 8"",
  ""SpLabel5"": ""sample string 9"",
  ""SpSpclCode"": ""sample string 10""
}";
            var service = new WebAPI2BearerMamanMessage(new CourierWEBAPICommSettings()
            {
                DeclarationId = "",
                username = "F_unitedf",
                password = "Unit2019",
                URIToken = @"https://maman.wsfreeze.co.il/WebAPIExt/Token", //HTTP/1.1;
                URIMethod = @"https://maman.wsfreeze.co.il/WebAPIExt/api/baldar/CreateECSpclMessgae",
                Tenant = 1

            });
            var res = service.PostIt(data);

            CourierGWMessageECSpclMamanRequestService.TestSend();





        }

        private void TesterForm_Load(object sender, EventArgs e)
        {
            WebFreight.Web.CustomWebServices.Testers.Tester.GetSingleDeclarationPMByNumber();

            AmitalContext.TestIt();

            if (DBWorkerService.IsOldDB())
            {
                MessageBox.Show("DBWorkerService.IsOldDB !!! - Please do Logitute.Update> Update DB !!!");
            }
            ;

        }

        private void buildMamanBaldarSTBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var AirlineIdMAWB=_tstbMamanBaldarSTB.Text;
            AirlineIdMAWB = AirlineIdMAWB.Trim();
            if (string.IsNullOrEmpty(AirlineIdMAWB))
            {
                MessageBox.Show("AirlineId-MAWB is must");
            }
            var my = new Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue.MamanStatusAvailabilityTesterService();
            var list = my.Tester(AirlineIdMAWB);
            string dir = @"C:\inetpub\wwwroot\FTP_MAMAN";
            if (!Directory.Exists(dir))
            {
                dir = Path.Combine(Path.GetTempPath(), "FTP_MAMAN");
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }

            }
            System.Diagnostics.Process.Start(dir);
            foreach (var item in list)
            {
                var f = Path.Combine(dir, item.Key);
                File.WriteAllText(f, item.Value);
            }
        }

        private void textBoxLogger_TextChanged(object sender, EventArgs e)
        {

        }

        private void downloadFTPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string interfaceID = _CBInterfaceID.Text;
            if (string.IsNullOrWhiteSpace(interfaceID))
            {
                MessageBox.Show("_CBInterfaceID.Text is null");
                return;
            }
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            try
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                var fileName = openFileDialog1.FileName;
                var bytsDcaFile = File.ReadAllBytes(fileName);
                fileName = Path.GetFileName(fileName);

                int Tenant = GetTenant();

                FTPToAnalyzeQueueWR.SaveAnalyzeQueueFromCode(Tenant, interfaceID, fileName, bytsDcaFile);
            }
            finally
            {
                openFileDialog1.Dispose();
            }
            


            
            
        }

        private void commDecServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < 9; i++)
            {
                try
                {
                    string MoreParams = "";
                    string MessageOut = "";
                    var ListEntry = new Dictionary<string, string>();
                    ListEntry.Add("tenant", "1");
                    ListEntry.Add("UNIFREIGHT_USER_ID", "ITZIK");




                    var s = new CommDecService();

                    string AssemblyQualifiedName = "Logitude.Customs.BL.Messaging.U2L.CommDec.CommDecService";
                    string DataIn1 = s.GetExampleDataIn1();
                    var rep="0987800" + i.ToString();
                    DataIn1 = DataIn1.Replace("09878498", rep);
                    string DataIn2 = "";
                    MoreParams = UnifreightListsUtil.Serialize(ListEntry);
                    string DataOut1 = "";
                    string DataOut2 = "";
                    string SUCCESS = "";

                    var gw = new UnifreightGatewayService();
                    gw.ProccessRequest(
                        AssemblyQualifiedName,
                DataIn1,
                DataIn2,
                out DataOut1,
                out DataOut2,
                out SUCCESS,
                ref MoreParams,
                out MessageOut);



                }
                catch (Exception E)
                {
                    //throw;
                }
            }
        }

        private void sendToolStripMenuItem_Click(object sender, EventArgs e)
        {
//            var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123" };
//            using (var connection = factory.CreateConnection())
//            using (var channel = connection.CreateModel())
//            {

//                //var factory = new ConnectionFactory() { HostName = "localhost" };
//                //using (var connection = factory.CreateConnection())
//                //using (var channel = connection.CreateModel())
//                //{
//                channel.QueueDeclare(queue: "connectToTicket",
//                                     durable: false,
//                                     exclusive: false,
//                                     autoDelete: false,
//                                     arguments: null);

//                string message = @"<DCAInUCBUD2LTWithResponseContentHeader xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns='http://amital.com/customs/Prod/DCAInUCBUD2LTWithResponseContentHeader'>
//<ResponseContentHeader>
//<ApplicationID>0</ApplicationID>
//<TransmitionDateTime>2021-09-12T18:31:55.5180688+03:00</TransmitionDateTime>
//</ResponseContentHeader>
//<tenant>3</tenant>
//<LoggingUserId>1-7</LoggingUserId>
//<DeclarationId>1-1479599</DeclarationId>
//<MyMoreParams/>
//<DocumentsFilingCode>E526108</DocumentsFilingCode>
//<DocumentsFilingId>PATLCHNAXUSVBJPZLLS+8A00000000</DocumentsFilingId>
//<DocumentTypeCode>CWB</DocumentTypeCode>
//</DCAInUCBUD2LTWithResponseContentHeader>"; ;
//                var body = Encoding.UTF8.GetBytes(message);

//                channel.BasicPublish(exchange: "",
//                                     routingKey: "connectToTicket",
//                                     basicProperties: null,
//                                     body: body);
//                Console.WriteLine(" [x] Sent {0}", message);
//            }

        }

        private void recivedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var args = new Dictionary<string, object>();
                //lazy = store messages to disk => no lost messages in case on rabbitmq restart 
                args.Add("x-queue-mode", "lazy");
                //channel.QueueDeclare(queue: "ucbud2lt__",
                //                    durable: true,
                //                    exclusive: false,
                //                    autoDelete: false,
                //                    arguments: args);


                channel.QueueDeclare(queue: "ucbud2lt__",
                                                           durable: false,
                                                           exclusive: false,
                                                           autoDelete: false,
                                                           arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    UniCourierBatchSendUCBUD2LT_MsgResponseService uniCourierBatchSendUCBUD2LT_MsgResponseService = new UniCourierBatchSendUCBUD2LT_MsgResponseService();
                    XmlSerializer serializer = new XmlSerializer(typeof(DCAInUCBUD2LTWithResponseContentHeader));
                    DCAInUCBUD2LTWithResponseContentHeader mySTBMessage;
                    using (TextReader reader = new StringReader(message))
                    {


                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader);

                        //Display all the book titles.
                        XmlNodeList elemList = doc.GetElementsByTagName("Body");
                        //XmlNodeList elemList2 = elemList.GetElementsByTagName("ResponseContentHeader");

                        //  mySTBMessage = GetSTBMessage(elemList[0].LastChild.InnerXml);

                        mySTBMessage = (DCAInUCBUD2LTWithResponseContentHeader)serializer.Deserialize(new StringReader(elemList[0].InnerXml));

                        //    dynamic test = XmlGenericUtil<dynamic>.DeSerializeObject(elemList[0].InnerXml);//serializer.Deserialize(reader);
                        //    result = (DCAInUCBUD2LTWithResponseContentHeader)test.body.DCAInUCBUD2LTWithResponseContentHeader;
                    }

                    uniCourierBatchSendUCBUD2LT_MsgResponseService.RealUpdate2(mySTBMessage);
             //   channel.BasicAck(ea.DeliveryTag, false);
                    //Console.WriteLine(" [x] Received {0}", message);
                };

                channel.BasicConsume(queue: "ucbud2lt__",
                                     autoAck: true,
                                     consumer: consumer);

                // Console.WriteLine(" Press [enter] to exit.");
                // Console.ReadLine();
            }
        }

        private void checkBoxMQ_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
