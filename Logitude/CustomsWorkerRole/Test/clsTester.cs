using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.Utils;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Customs.BL.Messaging.Maman;
using Simplog.Server.Infrastructure.Helpers;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.Messaging.ILSWS;
using Logitude.Customs.BL.TraceEvents;

namespace CustomsWorkerRole.Test
{
    public class clsTester
    {

        public void TestAsDataSet(string closedTableId)
        {
            var messageService = new SYSTBL_NG_9000_MSG_SystemTableRequestMessageService();
            var req = new Logitude.CustomsMessaging.Common.RequestParams.SystemTableRequestParams()
            {
                TableId = closedTableId,
                Tenant = 1,//_CustomsSetting.Tenant ,
                RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive,
                AsTableData = true,
                ForAnatSaveAsDATASET = true

            };

            var debugIt = true;
            if (debugIt)
            {
                req.RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive;
            }

            // req.RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive;    
            
            var res = messageService.Send(req);
            
        }

        public static void RequestSheetRepushQService(string customsRequestsSheetId, int tenant)
        {
            throw new Exception("to do ");
        }

        public static void SendDCA19666()
        {
            var o = new Logitude.CustomsMessaging.MessagingServices.SYSTBL_NG_9000_MSG_SystemTableRequestMessageService();

            var res = o.Send(new Logitude.CustomsMessaging.Common.RequestParams.SystemTableRequestParams()
            {
                //SuppressSplitWR = true,
                Tenant = 1,
                AsTableData = false,
                TableId = "1966",
                RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.DCABatch
            });

        }
        public static void TestNull()
        {
            var customsDocumentsTicketRepository = new CustomsDocumentsTicketRepository(1);
            /*
SELECT TOP 1000 [Id]
      ,[Tenant]
      ,[ParentEntityCode]
      ,[ParentEntityId]
      ,[Child1EntityCode]
      ,[Child1EntityId]
      ,[Child2EntityCode]
      ,[Child2EntityId]
      ,[Child3EntityCode]
      ,[Child3EntityId]
      ,[CustomDocumentId]
      ,[RequiredDocID]
      ,[DocumentTypeCode]
  FROM [AmitalPilot2_Main].[Customs].[CustomsDocumentPointers]
  where 
  iD='1-142' AND Tenant=1
  AND ParentEntityCode ='Declaration' AND ParentEntityId='1-106'
  AND Child1EntityCode  ='SupplierInvoice' AND Child1EntityId='1087'
  AND Child2EntityCode  is null AND Child2EntityId is null
             */
            var params1 = new GetTicketsParams()
            {
                ParentEntityId = "1-106",
                ParentEntityCode = "Declaration",

                Child1EntityCode = "SupplierInvoice",
                Child1EntityId = "1087",

                Child2EntityCode = null,
                Child2EntityId = null,
                Child3EntityCode = null,
                Child3EntityId = null

            };
            customsDocumentsTicketRepository.GetCustomsDocumentTickets(params1, 1);


        }

        public static void RepushQ(string customsRequestsSheetsId)
        {
            //var CustomsRequestsSheetService = CustomsRequestsSheetService<
        }

        public static void SpeedTest(string IIGServiceAddress, int tenant)
        {
            Logitude.CustomsMessaging.Helpers.SystemTables.SpeedTest(IIGServiceAddress, tenant);
        }
        public static void reqSheetStatistic()
        {

        }
        public static void DebugRQStep(string mainInterfaceCode, int tenant, string correlationId,
            string myCustomsCommand)
        {
            CustomsCommandEnum myCustomsCommandEnum;

            if (!Enum.TryParse<CustomsCommandEnum>(myCustomsCommand, out myCustomsCommandEnum))
            {
                throw new Exception("myCustomsCommand bad  " + myCustomsCommand);
            }
            Logitude.CustomsMessaging.MessagingServices.MessagingServiceFactoryHelper.ResolveAndExecute(
                mainInterfaceCode, tenant, correlationId, myCustomsCommandEnum,
                new OverrideControllerModel()
                {
                    DebugMode = true
                });
        }

        public static void ReQueue(string mainInterfaceCode, int tenant, string correlationId
            //,string myCustomsCommand
            )
        {
            //CustomsCommandEnum myCustomsCommandEnum;

            //if (!Enum.TryParse<CustomsCommandEnum>(myCustomsCommand, out myCustomsCommandEnum))
            //{
            //    throw new Exception("myCustomsCommand bad  " + myCustomsCommand);
            //}
            Logitude.CustomsMessaging.MessagingServices.MessagingServiceFactoryHelper.ResolveAndReQueue(
                mainInterfaceCode, tenant, correlationId, //myCustomsCommandEnum,
                new OverrideControllerModel()
                {
                    DebugMode = true
                });
        }



        public static void BuildDcaAggregrateFile(string path)
        {

            var iBig = 1024 * 1024 * 5;
            var big = new String("x"[0], iBig);
            var failedL = new List<int>() { 9 };
            var entityL = new List<int>() { 5, 6 };
            for (int i = 0; i < 10; i++)
            {
                var DeclarationID = "16021000274892";
                if (entityL.Contains(i))
                {
                    DeclarationID = "16041002498911";
                }

                var transmitionDateTime = DateTime.Now;
                var transTime = "2016-04-19_13-35-13-481";

                transTime = transmitionDateTime.ToString("s").Replace("T", "_").Replace(":", "-");
                transTime += "-";
                transTime += transmitionDateTime.Millisecond.ToString();

                var response = new UniDebug01_Msg()
                {
                    BigField = big,
                    DeclarationID = DeclarationID,
                    Remarks = transTime,
                    ResponseContentHeader = new DefaultResponseContentHeader()
                    {
                        //ApplicationID="16021007405812",
                        TransmitionDateTime = transmitionDateTime
                    },
                    ToFailAnalyze = failedL.Contains(i)
                };

                var body = XmlGenericUtil<UniDebug01_Msg>.SerializeObject(response);
                //
                body = body.Substring(body.IndexOf(Environment.NewLine));
                var myESBResponseXmlClass = new ESBResponseXmlClass();
                var extrenalId = "62833ff7-1cd3-4faa-85a6-a4312ae4797a";
                extrenalId = Guid.NewGuid().ToString();
                var xml = myESBResponseXmlClass.Get(Guid.NewGuid().ToString(), extrenalId, body);

                var fileName = "UnifreightTester01_Out.IL941079089." + transTime + "." + extrenalId + ".PLT.xml";
                File.WriteAllText(Path.Combine(path, fileName), xml);
            }



        }

        public static string GetDeclarationXml(int tenant, string DecId)
        {
            try
            {
                
                var DeclarationQueryService = new DeclarationQueryService(tenant);


                var pm = DeclarationQueryService.GetSingle(DecId, true, false);
                Logitude.Server.Tools.EntityPM.SuppressCreateNotifyPropertyChangeValues = true;
                var xml = XmlGenericUtil<DeclarationPM>.SerializeObject(pm);

                return xml;
            }
            finally
            {
                Logitude.Server.Tools.EntityPM.SuppressCreateNotifyPropertyChangeValues = false;
            }


        }

        public static void PutDeclarationXml(string xmlCheck)
        {
            var decPm = XmlGenericUtil<DeclarationPM>.DeSerializeObject(xmlCheck);

        }

        public static void SendDeclarationsThatCanResendInBatch()
        {
            //12 or 13
            var declarationQueryService = new DeclarationQueryService(1);
            var declarationRepository = new DeclarationRepository(1);
            var list = declarationRepository.GetDeclarationsThatCanResend(1, 300);
            Debug.WriteLine("GetDeclarationsThatCanResend:Retrieve:" + list.Count().ToString());
            int succ = 0;
            foreach (var item in list)
            {
                try
                {
                    var o = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();

                    var res = o.Send(new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams()
                    {
                        //SuppressSplitWR = true,
                        Tenant = 1,
                        AppicationId = item.Id,
                        RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch
                    });
                    succ++;
                }
                finally
                {

                }
            }
            Debug.WriteLine("GetDeclarationsThatCanResend:succ:" + succ.ToString());
        }


        public static void GetDeclarationMandatoryTicketList(string parentEntityId, string parentEntityCode)
        {
            try
            {


                ICustomContext customContext = CustomContext.GetContext(1);
                //CustomsDocumentQueryService customsDocumentQuery = new CustomsDocumentQueryService(customContext);
                //List<CustomsDocumentPM> documents = customsDocumentQuery.GetDeclarationMandatoryTicketList(parentEntityId, parentEntityCode, tenant);
                using (var s = (customContext as DbContextBase).CreateLogger())//check genrated sql 
                {
                    //return Request.CreateResponse(HttpStatusCode.OK, documents);
                    var query = new DeclarationQueryService(customContext);
                    var myCustomsDocumentsTicketPMList = query.GetDeclarationMandatoryTicket(parentEntityId, 1);
                    Debug.WriteLine(s.ToString());
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public static string CheckWSCourierStatistic(int tenant, bool multiThreard)
        {
            var declarationCourierStatusQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationCourierStatusQueryService(tenant);
            var counts = declarationCourierStatusQueryService.GetQueriesCounts(tenant,null,multiThreard);
            var jsonSetting = ProxyUtil.JsonConvertSerialize(counts);
            return jsonSetting;
        }

        public static void TestUnifreightFUStatusTaskService()
        {
            var unifreightFUStatusTaskService = new UnifreightFUStatusTaskService();
            unifreightFUStatusTaskService.UpsertFUStatusLE2U(3, "1-10", new UnifreightFUStatusParam()
            {
                Entname = "CFIFILEM",
                PrimaryNum = "60515808",
                Mode = UnifreightEventMode.@new,
                StatusCode = "SMG",
                EventDateTime = new DateTime(2021,03,09),
                OwnerUnifreightUserCode = FUOwnerUnifreightUserCode.SWISS
            });
        }
        public static void TestUpdateLOGITUDE_FILE()
        {
            int tenant = 2;
            var repo = new CFIFILEMRepository(tenant);
            var res = repo.UpdateLOGITUDE_FILE(tenant,197, "1-3434");
            repo.SubmitChanges();
            //throw new NotImplementedException();

        }

        public static void GetListByCourierHAWB()
        {
            var repo = new DeclarationRepository(1);
            var res=repo.GetListByCourierHAWB("xmd",1);
            //throw new NotImplementedException();
        }

        public static void MultiProccessTestLockTab()
        {
            int nTasks = 0;
            object o = nTasks;
            List<Task> tasks = new List<Task>();
            IDisposable processLockReleaseToken = null;


            int counter = 1;
            string myKey = "Key," + counter.ToString();
            using (var tran = TransactionFactory.GetTransaction())
            {
                using (processLockReleaseToken = ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(1, true, myKey, "blabla"))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(30));

                    Console.WriteLine("{0} tasks started and executed.", nTasks);
                }

            }

        }
        //public static void TestLockTab()
        //{
        //    int nTasks = 0;
        //    object o = nTasks;
        //    List<Task> tasks = new List<Task>();
        //    IDisposable processLockReleaseToken = null;
        //    try
        //    {
        //        int counter = 1;
        //        string myKey = "Key," + counter.ToString();
        //        processLockReleaseToken = ProcessLockTableUtil.Instance.LockItAndGetReleaseToken(myKey, "blabla");


        //        for (int ctr = 0; ctr < 3; ctr++)
        //            tasks.Add(Task.Run(() =>
        //            { // Instead of doing some work, just sleep.
        //                Thread.Sleep(250);
        //                // Increment the number of tasks.
        //                if (ctr > 1)
        //                {
        //                    counter++;
        //                }
        //                myKey = "Key," + counter.ToString();
        //                try
        //                {
        //                    using (var processLockReleaseToken1 = ProcessLockTableUtil.Instance.LockItAndGetReleaseToken(myKey, "blabla"))
        //                    {
        //                        Thread.Sleep(25);

        //                    }
        //                }
        //                catch (Exception eeee)
        //                {

        //                    Console.WriteLine(eeee.ToString());
        //                }



        //            }));
        //        Task.WaitAll(tasks.ToArray());
        //        Console.WriteLine("{0} tasks started and executed.", nTasks);
        //    }
        //    catch (AggregateException e)
        //    {
        //        String msg = String.Empty;
        //        foreach (var ie in e.InnerExceptions)
        //        {
        //            Console.WriteLine("{0}", ie.GetType().Name);
        //            if (!msg.Contains(ie.Message))
        //                msg += ie.Message + Environment.NewLine;
        //        }
        //        Console.WriteLine("\nException Message(s):");
        //        Console.WriteLine(msg);
        //    }
        //    finally
        //    {
        //        processLockReleaseToken.Dispose();
        //    }

        //}

        public void RestoreAlDec()
        {
            int i = 1;
            bool err = false;
            //Debug.WriteLine("RestoreAlDec");
            //Debug.WriteLine("תיקים ששולמו ושסוג תהליך שלהם מתחיל ב-407");
            Debug.WriteLine("תיקים ששולמו ");
            try
            {
                var declarationRepository = new DeclarationRepository(1);
                var alreadyPay = declarationRepository.GetAll(1)
                    //CustomerId = '1-3878' AND IsCancelled = 0
                    .Where(r => r.CustomerId == "1-3878")
                    .Where(r => !r.IsCancelled)


                    .Where(r => r.PaymentDate.HasValue)


                    //.Where(r => r.ProcedureCurrentCode.StartsWith("407"))
                    .Select(
                    poco => new
                    {
                        Id = poco.Id,
                        DeclarationNumber = poco.DeclarationNumber,
                        CustomsFile = poco.CustomFileNo
                    });



                var payList = alreadyPay.ToList();
                Debug.WriteLine("payList.Count" + payList.Count().ToString());

                //var supplierInvoiceItemRepository = new SupplierInvoiceItemRepository(1);
                //var qHaveMoreThen1 = (from a in supplierInvoiceItemRepository.GetAll(1)
                //                      where payList.Contains(a.DeclarationId)
                //                      group a by a.DeclarationId into g
                //                     where g.Count()>1
                //                     select g.Key
                //                    );

                //Debug.WriteLine(qHaveMoreThen1.ToTraceString());

                //var haveMoreThen1List =qHaveMoreThen1.ToList();
                //Debug.WriteLine("haveMoreThen1List.Count" + haveMoreThen1List.Count().ToString());
                foreach (var poco in payList)

                {


                    Debug.WriteLine($"Dec = {poco.Id} CustFile {poco.CustomsFile}");

                    var ms = new Logitude.CustomsMessaging.MessagingServices.DF_NG_8373_Web05_RetrieveImportDeclarationMessagingService();
                    ms.Send(
                        new Logitude.CustomsMessaging.Common.RequestParams.DeclarationRestoreRequestParams()
                        {
                            Tenant = 1,
                            RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch,
                            InterfaceTypeCode = "8373",
                            IsAngularClient = true,



                            DeclarationId = poco.Id,
                            AppicationId = poco.Id,
                            DeclarationNumber = poco.DeclarationNumber,
                            CustomsFile = poco.CustomsFile

                        }
                        );
                    i++;
                }

            }
            catch (Exception ee)
            {
                err = true;
                Debug.WriteLine("RestoreAlDec:" + ee.ToString());

            }
            finally
            {
                if (!err)
                {
                    Debug.WriteLine("Done !!!!:" + i.ToString());
                }
            }
        }

        public void Send8373()
        {
            //            Debug.WriteLine(@"לפתח תוכנית תיקון שתריץ מסר סטטוס הצהרה
            //האוכולוסיה לחיפוש - הצהרות שיש להם תאריך תשלום ואין להם תאריך התרה
            //עבור כל הצהרה יבוצע שאילתא לסטטוס הצהרה");

            Debug.WriteLine(@"שיחזור נתוני הצהרה    
PaymentDate  מלפני 3  ימים ");
            int i = 1;
            bool err = false;
            //Debug.WriteLine("RestoreAlDec");
            try
            {
                DateTime myDate = DateTime.Now.Subtract(TimeSpan.FromDays(3)).Date;
                var declarationRepository = new DeclarationRepository(1);
                var noHatra = declarationRepository.GetAll(1)
                    .Where(r => r.PaymentDate.HasValue && r.PaymentDate > myDate)
                    //.Where(r => !r.HatraDate.HasValue)
                    .Select(
                    poco => new
                    {
                        Id = poco.Id,
                        DeclarationNumber = poco.DeclarationNumber,
                        CustomsFile = poco.CustomFileNo
                    });



                var noHatraList = noHatra.ToList();
                Debug.WriteLine("Count" + noHatraList.Count().ToString());

                //var supplierInvoiceItemRepository = new SupplierInvoiceItemRepository(1);
                //var qHaveMoreThen1 = (from a in supplierInvoiceItemRepository.GetAll(1)
                //                      where payList.Contains(a.DeclarationId)
                //                      group a by a.DeclarationId into g
                //                     where g.Count()>1
                //                     select g.Key
                //                    );

                //Debug.WriteLine(qHaveMoreThen1.ToTraceString());

                //var haveMoreThen1List =qHaveMoreThen1.ToList();
                //Debug.WriteLine("haveMoreThen1List.Count" + haveMoreThen1List.Count().ToString());
                foreach (var poco in noHatraList)

                {


                    Debug.WriteLine($"Dec = {poco.Id} CustFile {poco.CustomsFile}");

                    var ms = new Logitude.CustomsMessaging.MessagingServices.DF_NG_8373_Web05_RetrieveImportDeclarationMessagingService();
                    ms.Send(
                         new Logitude.CustomsMessaging.Common.RequestParams.DeclarationRestoreRequestParams()
                         {
                             LoggingEnabled = true,
                             //LoggingUserId = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant),
                             CustomsFile = poco.CustomsFile,
                             DeclarationNumber = poco.DeclarationNumber,
                                             //CargoTypeCode = myDeclarationPM.Consignments[0].CargoTypeCode,
                                             //ManifestNumber = myDeclarationPM.Consignments[0].ManifestNumber,
                                             //SecondCargoID = myDeclarationPM.Consignments[0].SecondCargoID,
                                             //ThirdCargoID = myDeclarationPM.Consignments[0].ThirdCargoID,
                             Tenant = 1,
                             RequestName = "Declaration Status Search (from תוכנית לתיקון תאריך התרה)",
                             ResponseName = "Declaration Status Search (from תוכנית לתיקון תאריך התרה)",
                                             //TestCase = SelectedTest,
                             //CargoRadio = false,
                             //DeclarationRadio = true,
                             //OldReshimonRadio = false,
                             //OldReshimonNumber = null,
                             LoggingEntityId = poco.Id,
                             //RequestOrigin = "DeclarationStatusRequestViewModel",
                             RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch,
                         }
                        );
                    i++;
                }

            }
            catch (Exception ee)
            {
                err = true;
                Debug.WriteLine("RestoreAlDec:" + ee.ToString());

            }
            finally
            {
                if (!err)
                {
                    Debug.WriteLine("Done !!!!:" + i.ToString());
                }
            }
        }

        public void FtpTester(int tenant)
        {
            using (var scop = TransactionFactory.GetTransaction())
            {


                byte[] bytearray = Encoding.UTF8.GetBytes(
                   @"Hello world
שלום עולם 2 " + DateTime.Now.ToLongTimeString());
                //http://192.116.221.103:584/Courier58/api/couriermasters/getsingle?id=1-106

                var myFTPMamanService = new FTPOutMamanSubManifestService();
                myFTPMamanService.BuildCommunicationLog(bytearray, tenant, "1-1255463");//02004004

                var myFTPOutMaman2470ReleaseGoodService = new FTPOutMaman2470ReleaseGoodService();
                myFTPOutMaman2470ReleaseGoodService.BuildCommunicationLog(bytearray, tenant, "1-1255463", $"maman{Guid.NewGuid().ToString()}", false);



                string dec = "1-1255463";
                //CustomsPartnerFtpDetails.InterfaceName_ECSWSTHR_REQUEST
                var fTPOutMawbSWSService = new FTPOutMawbSWSService();
                Guid g = Guid.NewGuid();
                string filename = "02004004" + "_" + g;
                fTPOutMawbSWSService.BuildCommunicationLog(bytearray, tenant, dec, CustomsPartnerFtpDetails.InterfaceName_ECSWSTHR_REQUEST, filename);
                    
                    
                g = Guid.NewGuid();
                filename = "02004004" + "_" + g;
                fTPOutMawbSWSService.BuildCommunicationLog(bytearray, tenant, dec, CustomsPartnerFtpDetails.InterfaceName_ECSWSTHR_REQUEST, filename);
                scop.Complete();
                    //output  ftp://192.168.10.88/FTP_MAMAN/	
            }
        }
    }


    public class UploadMulti
    {
        Stopwatch sw = null;
        public void TestMulti()
        {
            sw = Stopwatch.StartNew();
            Task.Factory.StartNew(() =>
            {


                //Parallel.For(1, 100, i =>
                Task1000MS(0, 48, 57);

            });

            //return;
            Task.Factory.StartNew(() =>
            {
                //Parallel.For(1, 100, i =>
                Task1000MS(320, 65, 89);
                
            });

            Task.Factory.StartNew(() =>
            {
                Task1000MS(660, 97, 121);
            });
        }

        private void Task1000MS(int startcounter, int charstart, int charend)
        {
            int counter = startcounter;//0/310//620
            while (sw.Elapsed < TimeSpan.FromMinutes(5))
            {

                var sw1 = Stopwatch.StartNew();
                for (int i = 48; i <= 57; i++)
                {
                    sw1 = Stopwatch.StartNew();
                    try
                    {


                        Test1(startcounter,i, counter);
                        
                    }
                    catch (Exception ee)
                    {

                        Debug.WriteLine($"error  {i}:{counter}  " + ee.ToString());

                    }
                    finally
                    {

                        sw1.Stop();
                        counter++;
                        var wait = TimeSpan.FromSeconds(1).TotalMilliseconds - sw1.ElapsedMilliseconds;
                        if (wait > 0)
                        {

                            Thread.Sleep((int)wait);
                        }

                    }
                }
            }
        }

        public void Test1(int start,int seedChar,int extraSize)
        {
#if false
fileInfo
{Logitude.Server.Tools.BlobFileInfo}
    AesKey: null
    ContainerName: "tenant1"
    Extension: "pdf"
    ExternalContainerName: null
    FileName: "1-24037866"
    FileSize: 156796
    FolderName: "docsin"
    HasExternalContainer: false
    IsDecrypted: false
    IsEncrypted: false
    Tenant: 1
    UCreateDate: {11-07-18 09:07:41}
    UDocumentsFilingId: "dq-qklxeeego6mi1ilbafa00000000"
    UDocumentsId: "1-24037866"
    UFileVer: null
    UMode: true
    USuppressWriteDueSameMD5Hash: false
    tenant: 1

#endif
            var DateTimeTommarow = DateTime.Now.AddDays(1);
            var baseChar = Convert.ToChar(seedChar);
            //int size = 100 * 1024 + (seed * 1024);
            var content = string.Concat(
                new string("x"[0], 100 * 1024),
                Environment.NewLine,
                new string(baseChar, extraSize * 1024)
                );
            byte[] mybytes = Encoding.UTF8.GetBytes(content);

            var guid = Guid.NewGuid();
            var baseChar_guid = start + "_"+extraSize.ToString() + "_" + baseChar + "__" + guid.ToString();
            
            var myUnifreightFillingService = new UnifreightFillingService();
            myUnifreightFillingService.UploadByTenantComId(new Logitude.Server.Tools.BlobFileInfo()
            {
                //ContainerName = "tenant1",
                Extension = ".tst.txt",
                FileName = baseChar_guid,
                FileSize = mybytes.Length,
                FolderName = "docsin",
                Tenant = 1,
                UCreateDate = DateTimeTommarow,
                UDocumentsFilingId = baseChar_guid,
                UFileVer = null,
                USuppressWriteDueSameMD5Hash = false,
                UDocumentsId = baseChar_guid,
                UMode = true,


            },
            baseChar_guid,
            DateTimeTommarow,
            mybytes);
        }


    }
}