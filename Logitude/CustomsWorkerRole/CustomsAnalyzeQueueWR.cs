


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

namespace CustomsWorkerRole
{
    /* 
     * ///couriernet_global _global _global
Insert into BATCHSERVICESDEFINITIONS (CODE,CLASSNAME) values ('CustomsAnalyzeQueueWR','CustomsAnalyzeQueueWR');
Insert into BATCHSERVICESDEFINITIONMODS (CODE,INACTIVE,NUMBEROFTHREADS) values ('CustomsAnalyzeQueueWR',0,1);
     */

    //public class SendWebAPI2MamanGWMessageECTHRDataWR
    public class CustomsAnalyzeQueueWR
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
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CustomsAnalyzeQueueWR : Run() Method", null);
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
        //private DbQueueService _IQueueService;
        private ICommonDataContext _ICommonDataContext;

        private CommunicationLogRepository _CommunicationLogRep;
        private int _Tenant;

        private CommunicationLog _WaitingCommLog;

        private DateTime _LastCreateFtpDefinition;
        private List<InterfaceDetails> _CustomsPartnerAnalyzeQueueService;
        private int _SeedTenant = 1;

        //private QueueResponse _ReceivedBrokeredMessage;

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;



                var myClass = this.GetType().Name;



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




        public override void WorkOnce()
        {

            try
            {
                OnStart();

                WorkUntilQEmpty_Db();


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }


        }

        private void WorkUntilQEmpty_Db()
        {

            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var _CustomsAnalyzeQueueServices = customsPartnerFtpDetails.GetAllInterfaceDetails()
            .Where(r => r.TypeCode == CustomsPartnerFtpDetails.TypeCode_In)
            .Where(r => r.AnalyzeQueueService != AnalyzeQueueServiceEnum.none)
            .ToList();

            while (true)
            {

                foreach (InterfaceDetails @interface in _CustomsAnalyzeQueueServices)
                {

                    {
                        try
                        {
                            AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
                            var from = @interface.Partner + "," + @interface.Code;
                            
                            LastActivity = DateTime.UtcNow;
                            while (true)
                            {

                                AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue(from);
                                if (analyzeQueue == null)
                                {
                                    break;
                                }
                                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                                {
                                    var serviceAnalyzer = customsPartnerFtpDetails.GetCustomAnalyzerQueueService(@interface);
                                    //ArtemusAnalyzer analyzer = new Artemus(analyzeQueue, analyzeQueueRepository);
                                    serviceAnalyzer.Run(analyzeQueue, analyzeQueueRepository);
                                    scope.Complete();
                                }
                                LogDoneItemInMemory();
                            }


                            Thread.Sleep(500);


                        }
                        catch (Exception e)
                        {
                            ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ArtemusAnalyzerWorkerRole : Run() Method", null);
                            Thread.Sleep(5000);
                        }
                    }
                }


                //Thread.Sleep(TimeSpan.FromSeconds(5));

                Thread.Sleep(TimeSpan.FromSeconds(1));//not using soo mach 
                break;


            }
        }

    }

}
