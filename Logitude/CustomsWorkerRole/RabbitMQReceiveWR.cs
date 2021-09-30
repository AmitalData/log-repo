
using CustomsWorkerRole.L2U;

using Logitude.Server.Tools.Models;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnifreightIIG.UServer;
using CustomsWorkerRole.Queue;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System.Net.Http;
using Logitude.Customs.BL.Messaging.Maman;
using Microsoft.Practices.Unity;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.FTP;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.Repositories;
using Unifreight.Data.AmitalModel;
using Logitude.Server.Tools.Utils;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Logitude.CustomsMessaging.ResponseServices;
using System.Xml.Serialization;
using Logitude.CustomsMessaging.MessagingServices;

namespace CustomsWorkerRole
{

    public class RabbitMQReceiveWR
        : CustomsWorkerEntryPoint
    {
        QueueDescription _QueueDescription;
        QueueClient _QueueClient;
        public override void Run()
        {

            while (true)
            {

                if (!General.IsUpdating())
                {
                    try
                    {

                        WorkOnce();
                        Thread.Sleep(TimeSpan.FromSeconds(_SeedTenant));
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "RabbitMQReceiveWR : Run() Method", null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }


            }

        }
        bool _OnStartDone = false;
        private ICommonDataContext _ICommonDataContext;

        private CommunicationLogRepository _CommunicationLogRep;
        private int _Tenant;

        private CommunicationLog _WaitingCommLog;

        private DateTime _LastCreateFtpDefinition;
        private List<CustomsPartnerFtpPM> _FtpDefinitions;
        private int _SeedTenant = 1;


        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;
                DoneItemsInRange = new Dictionary<DateTime, int>();

                if (WorkerRoleServiceLocator.PleaseShutDown) return true;
            


                var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "connectToTicket",
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
                        DCAInUCBUD2LTWithResponseContentHeader result = new DCAInUCBUD2LTWithResponseContentHeader();
                        using (TextReader reader = new StringReader(message))
                        {
                            result = (DCAInUCBUD2LTWithResponseContentHeader)serializer.Deserialize(reader);
                        }


                        uniCourierBatchSendUCBUD2LT_MsgResponseService.RealUpdate2(result);

                        Console.WriteLine(" [x] Received {0}", message);
                    };

                    channel.BasicConsume(queue: "connectToTicket",
                                         autoAck: true,
                                         consumer: consumer);
                }

                }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "amital send data worker role start", null, null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.


            return base.OnStart();
        }

        private void CreateFtpDefinitionsEvery10Min()
        {
            //if (DateTime.Now.Subtract(_LastCreateFtpDefinition) < TimeSpan.FromMinutes(15))
            //{
            //    return;
            //}
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                //ProccessReceivedMessage();


                _LastCreateFtpDefinition = DateTime.Now;
                _FtpDefinitions = new List<CustomsPartnerFtpPM>();
                var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
                var ftpIncustomsPartnerFtpDetails = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.TypeCode == CustomsPartnerFtpDetails.TypeCode_In)
                    .Where(r => r.ViaMethod == "FTP")
                    .ToList();

                ftpIncustomsPartnerFtpDetails.ForEach(ftpIncustomsPartnerFtpDetail =>
                {

                    var myCustomsPartnerFtpQueryService = new CustomsPartnerFtpQueryService(_SeedTenant);
                    var pmCustomsPartnerFtps = myCustomsPartnerFtpQueryService.GetAllTenantBy(ftpIncustomsPartnerFtpDetail.Code /*CustomsPartnerFtpDetails.InterfaceName_ECSPCL*/,
                        ftpIncustomsPartnerFtpDetail.Partner,
                        ftpIncustomsPartnerFtpDetail.TypeCode);


                    foreach (var pmCustomsPartnerFtp in pmCustomsPartnerFtps)
                    {
                        //if (pmCustomsPartnerFtp != null)
                        {


                            FTPDetailRepository ftpDetailsRepository = new FTPDetailRepository(_SeedTenant);
                            FTPDetail ftpDetail = ftpDetailsRepository.GetSingleFTPDetail(pmCustomsPartnerFtp.FtpDetailsId, pmCustomsPartnerFtp.Tenant);
                            if (ftpDetail != null)
                            {

                                pmCustomsPartnerFtp.MyFtpDetail = ftpDetail;
                                _FtpDefinitions.Add(pmCustomsPartnerFtp);
                            }
                        }

                    }
                });
            }
        }

        public override void WorkOnce()
        {

            try
            {
                OnStart();

              //  WorkUntilQEmpty_Db();


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }


        }

        //private void WorkUntilQEmpty_Db()
        //{
        //    CreateFtpDefinitionsEvery10Min();

        //    while (!WorkerRoleServiceLocator.PleaseShutDown)
        //    {

        //        foreach (CustomsPartnerFtpPM ftpDef in _FtpDefinitions)
        //        {
        //            LastActivity = DateTime.UtcNow;
        //            if (ftpDef.MyFtpDetail.UseSFTP)
        //            {
        //                DownloadSFTPFiles(ftpDef);
        //            }
        //            else
        //            {
        //                DownloadFTPFiles(ftpDef);
        //            }
        //            if (WorkerRoleServiceLocator.PleaseShutDown)
        //            {
        //                break;
        //            }
        //        }


        //        //Thread.Sleep(TimeSpan.FromSeconds(5));

        //        Thread.Sleep(TimeSpan.FromSeconds(15));//not using soo mach 
        //        break;


        //    }
        //}
        static List<string> _BadFileNamesCache = new List<string>();
        static DateTime _LastClearCacheBadFileNames = DateTime.MinValue;

 
        public static void SaveAnalyzeQueue(InterfaceDetails defInterfaceDetails, string fileName, byte[] fileData, int tenant)
        {
            //var analyzeQueueUtil = new AnalyzeQueueUtil();
            //analyzeQueueUtil.SaveMessageToAnalyzeQueue(fileName, fileData, tenant, "", defInterfaceDetails, null);
        }

        private void ClearBadFileNamesCache()
        {
            _LastClearCacheBadFileNames = DateTime.Now;
            _BadFileNamesCache.Clear();
        }

        public static void SaveAnalyzeQueueFromCode(int tenant, string interfaceCode, string fileName, byte[] fileData)
        {
            //var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            //var defInterfaceDetails = customsPartnerFtpDetails.GetAllInterfaceDetails()
            //        .Where(r => r.Code == interfaceCode).First();
            //SaveAnalyzeQueue(defInterfaceDetails, fileName, fileData, tenant);
        }
    }



}
