using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
using WebFreight.Web.CustomWebServices.BL;
using WebFreight.Web.CustomWebServices.Contracts;
using Logitude.Customs.Def.Contracts;
//using Logitude.AmitalMessaging.Utils;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.Server.Tools.Models;
using System.Threading.Tasks;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.Messaging.Customs;
using Simplog.Server.Infrastructure.Helpers;


namespace WebFreight.Web.CustomWebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GatewayService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GatewayService.svc or GatewayService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UnifreightGatewayService : IGatewayService, IDisposable
    {

        static readonly List<string> _AllTypes;
        static readonly List<string> _AllIUnifreightGenericService;

        static readonly IUnityContainer _UnityContainer;

        static int _Ocounter = 1;
        int _HashCode = -1;
        StringBuilder _sbGatewayLog = null;
        Stopwatch _swGatewayLog = null;
        static UnifreightGatewayService()
        {
            _UnityContainer = new UnityContainer();
            _AllTypes = new List<string>();
            _AllIUnifreightGenericService = new List<string>();
            string curr = "";

            curr = "Logitude.CustomsMessaging.UnifreightGateway.UServerGetRequestService";
            _AllTypes.Add(curr);
            _UnityContainer.RegisterType<UnifreightGatewayProxy, Logitude.CustomsMessaging.UnifreightGateway.UServerGetRequestService>(curr);

            curr = "Logitude.CustomsMessaging.UnifreightGateway.UServerSetResponseService";
            _AllTypes.Add(curr);
            _UnityContainer.RegisterType<UnifreightGatewayProxy, Logitude.CustomsMessaging.UnifreightGateway.UServerSetResponseService>(curr);



            curr = "Logitude.CustomsMessaging.UnifreightGateway.InsertImportDeclarationService";
            _AllTypes.Add(curr);
            _UnityContainer.RegisterType<UnifreightGatewayProxy, Logitude.CustomsMessaging.UnifreightGateway.InsertImportDeclarationService>(curr);



            curr = "Logitude.Customs.BL.Messaging.U2L.ImportDeclaration.DeclarationUpsertService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.ImportDeclaration.DeclarationUpsertService>(curr);

            //curr = "Logitude.Accounting.BL.Messaging.GLAccountUpsertService";
            //_AllIUnifreightGenericService.Add(curr);
            //_UnityContainer.RegisterType<UnifreightGenericService, Logitude.Accounting.BL.Messaging.GLAccountUpsertService>(curr);

            
            curr = "Logitude.Customs.BL.Messaging.U2L.Sivug.SivugUpsertService";
            _AllIUnifreightGenericService.Add(curr);
            //_UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.Sivug.SivugUpsertService>(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.CustomsMessaging.U2L.Sivug.SivugUpsertService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.Tsrufa.TsrufaService"; // moran 23.7.15 - Task 13902
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.Tsrufa.TsrufaService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.Entry.EntryService"; // moran 15.11.15 - AMI-54599
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.Entry.EntryService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.Release.ReleaseService"; // moran 19.11.15 - AMI-54986
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.Release.ReleaseService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.PayHand.PayHandService"; // moran 31.1.16 - AMI-55700
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.PayHand.PayHandService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.Scheduler.SchedulerService"; // moran 18.2.16 - Task 11138
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.Scheduler.SchedulerService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.CellFile.CellFileUpsertService"; // moran 27.4.16 - AMI-55991
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.CellFile.CellFileUpsertService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.FritzDeclaration.FritzDeclarationService"; // moran 15.6.16 - AMI-57051
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.FritzDeclaration.FritzDeclarationService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.CommDec.CommDecService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.CommDec.CommDecService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.Reshimon.ReshimonService"; 
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.Reshimon.ReshimonService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments.DeclarationDocumentsService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments.DeclarationDocumentsService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.Courier.CourierService"; 
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.Courier.CourierService>(curr);

            curr = "Logitude.CustomsMessaging.UnifreightGateway.SendDirectMessageService";
            _AllTypes.Add(curr);
            _UnityContainer.RegisterType<UnifreightGatewayProxy, Logitude.CustomsMessaging.UnifreightGateway.SendDirectMessageService>(curr);
            AddSendDirectMessageService();

            curr = "Logitude.CustomsMessaging.UnifreightGateway.Testres.MritTestService";
            _AllTypes.Add(curr);
            _UnityContainer.RegisterType<UnifreightGatewayProxy, Logitude.CustomsMessaging.UnifreightGateway.Testres.MritTestService>(curr);
            AddSendDirectMessageService();
            
            curr = "Logitude.CustomsMessaging.UnifreightGateway.AnalyzeQueueMessageService";
            _AllTypes.Add(curr);
            _UnityContainer.RegisterType<UnifreightGatewayProxy, Logitude.CustomsMessaging.UnifreightGateway.AnalyzeQueueMessageService>(curr);

            //var messagingServiceTestSendXml =_UnityContainer.Resolve<Logitude.CustomsMessaging.MessagingServices.IMessagingServiceTestSendXml>(curr);
            //messagingServiceTestSendXml.TestSendXml(

            curr = "Logitude.CustomsMessaging.Testers.SubmitDeclarationTestService";
            _AllTypes.Add(curr);
            _UnityContainer.RegisterType<UnifreightGatewayProxy, Logitude.CustomsMessaging.Testers.SubmitDeclarationTestService>(curr);

            curr = "Logitude.CustomsMessaging.Testers.PaymentTestService";
            _AllTypes.Add(curr);
            _UnityContainer.RegisterType<UnifreightGatewayProxy, Logitude.CustomsMessaging.Testers.PaymentTestService>(curr);

            curr = "Logitude.CustomsMessaging.UnifreightGateway.UGEnvironmentService";
            _AllTypes.Add(curr);
            _UnityContainer.RegisterType<UnifreightGatewayProxy, Logitude.CustomsMessaging.UnifreightGateway.UGEnvironmentService>(curr);

            curr = "Logitude.CustomsMessaging.UnifreightGateway.CustomsRequestsSheetInProgressService";
            _AllTypes.Add(curr);
            _UnityContainer.RegisterType<UnifreightGatewayProxy, Logitude.CustomsMessaging.UnifreightGateway.CustomsRequestsSheetInProgressService>(curr);

            curr = "Logitude.CustomsMessaging.UnifreightGateway.SivugUpsertBatchService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.CustomsMessaging.UnifreightGateway.SivugUpsertBatchService>(curr);

            curr = "Logitude.CustomsMessaging.UnifreightGateway.SivugUpsertDcaReceivedService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.CustomsMessaging.UnifreightGateway.SivugUpsertDcaReceivedService>(curr);

            curr = "Logitude.CustomsMessaging.UnifreightGateway.MevakerSendSignedDeclarationService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.CustomsMessaging.UnifreightGateway.MevakerSendSignedDeclarationService>(curr);

            curr = "Logitude.CustomsMessaging.UnifreightGateway.CacheManagerService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.CustomsMessaging.UnifreightGateway.CacheManagerService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.CommMasterCourier.CommMasterCourierService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.CommMasterCourier.CommMasterCourierService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.Courier.CourierPendingReasonService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.Courier.CourierPendingReasonService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.CourierStatus.CourierStatusService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.CourierStatus.CourierStatusService>(curr);

            curr = "Logitude.Customs.BL.Messaging.U2L.CommDecReferantData.CommDecReferantDataService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.Customs.BL.Messaging.U2L.CommDecReferantData.CommDecReferantDataService>(curr);

            curr = "Logitude.CustomsMessaging.UnifreightGateway.ExportCloudSSOService";
            _AllIUnifreightGenericService.Add(curr);
            _UnityContainer.RegisterType<UnifreightGenericService, Logitude.CustomsMessaging.UnifreightGateway.ExportCloudSSOService>(curr);
        }

        private static void AddSendDirectMessageService()
        {
            var curr = "DF_MSG10000_ImportDeclarationMessagingService";
            _UnityContainer.RegisterType
                <IMessagingServiceInterfaceType,
                Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService>(curr);
            curr = "CH_NG_191_MSG2_ChangingTimeRequestMessagingService";
            _UnityContainer.RegisterType
                <IMessagingServiceInterfaceType,
                Logitude.CustomsMessaging.MessagingServices.CH_NG_191_MSG2_ChangingTimeRequestMessagingService>(curr);


            curr = "CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService";
            _UnityContainer.RegisterType
                <IMessagingServiceInterfaceType,
                Logitude.CustomsMessaging.MessagingServices.CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService>(curr);

            curr = "TSH_MSG6_AgentPaymentMessageService";
            _UnityContainer.RegisterType
                <IMessagingServiceInterfaceType,
                Logitude.CustomsMessaging.MessagingServices.TSH_MSG6_AgentPaymentMessageService>(curr);
            curr = "TSH_NG_3053_MSG8_AgentPaymentRequestMessageService";
            _UnityContainer.RegisterType
                <IMessagingServiceInterfaceType,
                Logitude.CustomsMessaging.MessagingServices.TSH_NG_3053_MSG8_AgentPaymentRequestMessageService>(curr);

            curr = "VE_MSG010_VendorInsertUpdateDeleteMessagingService";
            _UnityContainer.RegisterType
                <IMessagingServiceInterfaceType,
                Logitude.CustomsMessaging.MessagingServices.VE_MSG010_VendorInsertUpdateDeleteMessagingService>(curr);

            curr = "VE_MSG051_VendorSearchByCustomsAgentMessagingService";
            _UnityContainer.RegisterType
                <IMessagingServiceInterfaceType,
                Logitude.CustomsMessaging.MessagingServices.VE_MSG051_VendorSearchByCustomsAgentMessagingService>(curr);


            curr = "D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService";
            _UnityContainer.RegisterType
                <IMessagingServiceInterfaceType,
                Logitude.CustomsMessaging.MessagingServices.D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService>(curr);


            curr = "DF_NG_2755_MSG12001_SubmitDeclarationMessagingService";
            _UnityContainer.RegisterType
                <IMessagingServiceInterfaceType,
                Logitude.CustomsMessaging.MessagingServices.DF_NG_2755_MSG12001_SubmitDeclarationMessagingService>(curr);



        }

        public UnifreightGatewayService()
        {
            _sbGatewayLog = new StringBuilder();
            _HashCode = _Ocounter++;
            _sbGatewayLog.AppendLine("UnifreightGatewayService:Request=" + _HashCode.ToString());
            _swGatewayLog = new Stopwatch();
            _swGatewayLog.Start();

        }


        public string GetState()
        {
            return GatewayServiceState.GetState();
        }


        public void ProccessBASE64Request(
                  string AssemblyQualifiedName,
                  string BASE64DataIn1,
                  string BASE64DataIn2,
            string BASE64DataIn3,
                  out string BASE64DataOut1,
                  out string BASE64DataOut2,
            out string BASE64DataOut3,
                  out string SUCCESS,
                  ref string MoreParams,
                  out string MessageOut
                  )
        {


            BASE64DataOut1 = BASE64DataOut2 = BASE64DataOut3 = SUCCESS = MessageOut = "";



        }



        public void ProccessRequest(
            string AssemblyQualifiedName,
            string DataIn1,
            string DataIn2,
            out string DataOut1,
            out string DataOut2,
            out string SUCCESS,
            ref string MoreParams,
            out string MessageOut
            )
        {

            AssemblyQualifiedName = AssemblyQualifiedName ?? "";
            if (AssemblyQualifiedName.Equals( "SivugTST", StringComparison.OrdinalIgnoreCase))
            {
                GetSivugTest(ref AssemblyQualifiedName, ref DataIn1, ref MoreParams);
            }
            DataOut1 = DataOut2 = SUCCESS = MessageOut = "";

            SUCCESS = false.ToString();
            string MessageOutWS = "";
            IUnifreightGatewayProxy myWSProxy = null;
            UnifreightGenericService unifreightGenericService = null;
            try
            {

                _sbGatewayLog.AppendLine("Proccess( " + AssemblyQualifiedName + " )");





                if (_UnityContainer.IsRegistered(typeof(UnifreightGenericService), AssemblyQualifiedName))
                {
                    unifreightGenericService = _UnityContainer.Resolve<UnifreightGenericService>(AssemblyQualifiedName);
                    if (unifreightGenericService != null)
                    {
                        ProccessGenericRequest(unifreightGenericService, DataIn1, ref DataOut1, ref MoreParams, ref MessageOutWS);
                        SUCCESS = true.ToString();
                        ///
                        return;
                    }
                }

                myWSProxy = _UnityContainer.Resolve<UnifreightGatewayProxy>(AssemblyQualifiedName /*"ImportDeclarationService" */);
                if (myWSProxy == null)
                {
                    _sbGatewayLog.Insert(0, "PlugInWSProxy1.GetInstanceIWSProxy(AssemblyQualifiedName)== null are u reference Class that is Assambly missing ??");
                    return;
                }
                myWSProxy.MyUnity = (object)_UnityContainer;


                myWSProxy.ProccessRequest(DataIn1,
                DataIn2,
                out DataOut1,
                out DataOut2,
                out SUCCESS,
                ref MoreParams,
                out MessageOutWS);



                ///_UnityContainer.Resolve <Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService>("DF_MSG10000_ImportDeclarationMessagingService"); 

            }
            catch (DbEntityValidationException ex)
            {
                var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                _sbGatewayLog.Insert(0, "ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                Debug.WriteLine("ProccessRequest():Exception " + FormatedException.ToString(), true);
            }
            catch (Exception e)
            {
                _sbGatewayLog.Insert(0, "ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                Debug.WriteLine("ProccessRequest():Exception " + e.ToString(), true);
            }
            finally
            {

                DataIn1 = DataIn1 ?? "";
                DataIn2 = DataIn2 ?? "";
                DataOut1 = DataOut1 ?? "";
                DataOut2 = DataOut2 ?? "";

                Debug.WriteLine(
                    "GatewayService:Request=" + _HashCode.ToString() +
string.Format(
@"DataIn1={0}
DataIn2={1}
DataOut1={2}
DataOut2={3}                  
SUCCESS={4}"
, string.Concat(DataIn1.Take(100)), string.Concat(DataIn2.Take(100)), string.Concat(DataOut1.Take(100)), string.Concat(DataOut2.Take(100)), SUCCESS), false);
                if (myWSProxy != null)
                {
                    _sbGatewayLog.AppendLine(myWSProxy.GetLog());
                }
                if (SUCCESS == true.ToString())
                {
                    GatewayServiceState.AddSuccess();
                    _sbGatewayLog.AppendLine(MessageOutWS);

                }
                else
                {
                    _sbGatewayLog.Insert(0, MessageOutWS);
                }

                MessageOut = _sbGatewayLog.ToString();
                if (myWSProxy != null)
                {
                    (myWSProxy as IDisposable).Dispose();
                }
            }

        }

        
        /// <summary>
        /// UnifreightGatewayService.ProccessGenericRequest
        /// </summary>
        /// <param name="unifreightGenericService"></param>
        /// <param name="DataIn1"></param>
        /// <param name="DataOut1"></param>
        /// <param name="MoreParams"></param>
        /// <param name="MessageOutWS"></param>
        private void ProccessGenericRequest
            (UnifreightGenericService unifreightGenericService, string DataIn1, ref string DataOut1, ref string MoreParams, ref string MessageOutWS)
        {
            string newCommunication = "";
            unifreightGenericService.MyUnity = (object)_UnityContainer;
            _sbGatewayLog.AppendLine("unifreightGenericService.ProccessRequest .. ");
            try
            {
                AuthenticationUtil.DebugUsers();

                int iTenanat = -999; string contactEmail = "";
                if (AuthenticationUtil.UnifreightImpersonate(MoreParams, out iTenanat, out contactEmail))
                {
                    _sbGatewayLog.AppendLine("UnifreightImpersonate email-" + contactEmail + " tenent-" + iTenanat);

                    unifreightGenericService.SetIdentityName(contactEmail);
                }
                else
                {
                    _sbGatewayLog.AppendLine("UnifreightImpersonate  Failed ");
                }
                unifreightGenericService.SetTenant(iTenanat);
                if (false)
                {
                    AuthenticationUtil.DebugUsers();


                    // Wait for all tasks to complete.
                    Task[] tasks = new Task[10];
                    for (int i = 0; i < 10; i++)
                    {
                        //System.Threading.Tasks.Task.Factory.StartNew(() => {  ; });
                        tasks[i] = Task.Factory.StartNew(() => AuthenticationUtil.DebugUsers());
                    }
                    Task.WaitAll(tasks);
                }




                using (TransactionScope scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))//new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(10)))
                {
                    unifreightGenericService.ProccessGenericRequest(DataIn1, ref MoreParams, out MessageOutWS);

                    if (unifreightGenericService.MyGenericResponseObj.StatusType == Logitude.AmitalMessaging.Infrastructure.GenericResponseObj.StatusEnum.Success)
                    {
                        scope.Complete();
                    }
                }
                //DataOut1 = XmlGenericUtil<GenericResponseObj>.SerializeObject(unifreightGenericService.MyGenericResponseObj);
            }
            catch (BusinessErrorException businessErrorException)
            {
                unifreightGenericService.MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                unifreightGenericService.MyGenericResponseObj.Message = businessErrorException.Message;
                unifreightGenericService.MyGenericResponseObj.InnerException = businessErrorException.ToString();
            }
            catch (DbEntityValidationException ex)
            {
                var formatedException = ExceptionFormatUtil.GetFormated(ex);
                //_sbLog.Insert(0, "ProccessRequest():Exception " + formatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                Debug.WriteLine("ProccessRequest():Exception " + formatedException.ToString(), true);
                unifreightGenericService.MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                unifreightGenericService.MyGenericResponseObj.Message = "Error while DeclarationUpdateService.Update " + formatedException.Message;
                unifreightGenericService.MyGenericResponseObj.ErrorDescription = formatedException.ToString();
                if (formatedException.InnerException != null)
                {
                    unifreightGenericService.MyGenericResponseObj.InnerException = formatedException.InnerException.ToString();
                }
            }
            catch (Exception e)
            {

                unifreightGenericService.MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.TecinicalFailure;
                unifreightGenericService.MyGenericResponseObj.Message = "Exception: " + e.Message;
                unifreightGenericService.MyGenericResponseObj.ErrorDescription = e.ToString();
                if (e.InnerException != null)
                {
                    unifreightGenericService.MyGenericResponseObj.InnerException = e.InnerException.ToString();
                    if (e.InnerException.InnerException != null)
                    {
                        unifreightGenericService.MyGenericResponseObj.InnerException += e.InnerException.InnerException.ToString();
                    }
                }

            }

            finally
            {
                unifreightGenericService.MyGenericResponseObj.Log = unifreightGenericService.GetLog();
                DataOut1 = XmlGenericUtil<GenericResponseObj>.SerializeObject(unifreightGenericService.MyGenericResponseObj);

                DataOut1 = LogCommunication(unifreightGenericService, DataIn1, DataOut1);

            }


            //unifreightGenericService.MyGenericResponseObj.




        }

        private static string LogCommunication(UnifreightGenericService unifreightGenericService, string DataIn1, string DataOut1)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DataIn1) && string.IsNullOrWhiteSpace(DataOut1))
                {
                    return DataOut1;
                }

                unifreightGenericService.MyCommunicationsParams.From = unifreightGenericService.MyCommunicationsParams.From ?? "Amital";
                unifreightGenericService.MyCommunicationsParams.To = unifreightGenericService.MyCommunicationsParams.To ?? "Logitude";
                unifreightGenericService.MyCommunicationsParams.CommunicationLogTypeCode = unifreightGenericService.MyCommunicationsParams.CommunicationLogTypeCode ?? "T";
                unifreightGenericService.MyCommunicationsParams.FolderName = unifreightGenericService.MyCommunicationsParams.FolderName ?? "Amital";

                unifreightGenericService.MyCommunicationsParams.Subject = unifreightGenericService.MyCommunicationsParams.Subject ?? "ProccessGenericRequest";

                if (unifreightGenericService.MyGenericResponseObj.StatusType ==
                    GenericResponseObj.StatusEnum.Success)
                {
                    unifreightGenericService.MyCommunicationsParams.Status = "D";
                }
                else
                {
                    unifreightGenericService.MyCommunicationsParams.Status = "F";
                }


                unifreightGenericService.MyCommunicationsParams.LoggingUserId = unifreightGenericService.MyCommunicationsParams.LoggingUserId
                    ?? AuthenticationUtil.ResolveUserId(unifreightGenericService.MyCommunicationsParams.Tenant);

                unifreightGenericService.MyCommunicationsParams.InOut = "I";
                unifreightGenericService.MyCommunicationsParams.CommunicationLogTypeCode = "T";


                unifreightGenericService.MyCommunicationsParams.ByteData = Encoding.UTF8.GetBytes(DataIn1);
                unifreightGenericService.MyCommunicationsParams.Logs = unifreightGenericService.GetLog();
                var comm = Communications.AddCommunicationLog(unifreightGenericService.MyCommunicationsParams);
                unifreightGenericService.MyGenericResponseObj.CorrelationId = comm;

                DataOut1 = XmlGenericUtil<GenericResponseObj>.SerializeObject(unifreightGenericService.MyGenericResponseObj);
            }

            catch (Exception)
            {


            }
            return DataOut1;
        }




        public void GetAllAssemblyQualifiedName(
            out string AllAssemblyQualifiedName, out string AllAssemblyDescription,
            ref string MoreParams,
            out string MessageOut)
        {
            MessageOut = AllAssemblyDescription = AllAssemblyQualifiedName = "";
            try
            {
                StringBuilder sbAllAssemblyQualifiedName = new StringBuilder();
                StringBuilder sbAllAssemblyDescription = new StringBuilder();
#if false
                PlugInWSProxy PlugInWSProxy1 = new PlugInWSProxy(GatewayService.GetExecutablePath());

                var AllServicesKey = PlugInWSProxy1.GetAllServicesKey();
                foreach (var item in AllServicesKey)
                {
                    sbAllAssemblyQualifiedName.AppendLine(item);
                    sbAllAssemblyDescription.AppendLine(item);
                }
#else



                foreach (var assemblyQualifiedName in _AllTypes)
                {
                    using (var myWSProxy = _UnityContainer.Resolve<UnifreightGatewayProxy>(assemblyQualifiedName /*"ImportDeclarationService" */))
                    {
                        //sbAllAssemblyQualifiedName.AppendLine(myWSProxy.GetType().AssemblyQualifiedName);
                        sbAllAssemblyQualifiedName.AppendLine(assemblyQualifiedName);
                        sbAllAssemblyDescription.AppendLine(myWSProxy.UniDescription + " " + myWSProxy.UniVersion);
                    }
                }
                foreach (var assemblyQualifiedName in _AllIUnifreightGenericService)
                {
                    using (var myWSProxy = _UnityContainer.Resolve<UnifreightGenericService>(assemblyQualifiedName /*"ImportDeclarationService" */))
                    {
                        //sbAllAssemblyQualifiedName.AppendLine(myWSProxy.GetType().AssemblyQualifiedName);
                        sbAllAssemblyQualifiedName.AppendLine(assemblyQualifiedName);
                        sbAllAssemblyDescription.AppendLine(myWSProxy.UniDescription + " " + myWSProxy.UniVersion);
                    }

                }
                //TYFetchContainerManifestNumber myTYFetchContainerManifestNumber = new TYFetchContainerManifestNumber();


#endif





                AllAssemblyDescription = sbAllAssemblyDescription.ToString();
                AllAssemblyQualifiedName = sbAllAssemblyQualifiedName.ToString();
            }
            catch (Exception e)
            {
                MessageOut = "Exception " + e.ToString();
            }
        }

        public void GetExampleProcessRequest(string AssemblyQualifiedName,
            out string DataIn1, out string DataIn2, out string DataOut1, out string DataOut2, ref string MoreParams, out string MessageOut)
        {
            ///throw new NotImplementedException();
            DataIn1 = DataIn2 = DataOut1 = DataOut2 = MessageOut = "";

            try
            {

                _sbGatewayLog.AppendLine("GetExampleProcessRequest( " + AssemblyQualifiedName + " )");

                _sbGatewayLog.AppendLine("CreateInstanceWSProxy .. ");
                //var myWSProxy = CreateInstanceWSProxy(AssemblyQualifiedName);
                UnifreightGatewayProxy myWSProxy = null;
                if (_UnityContainer.IsRegistered(typeof(UnifreightGatewayProxy), AssemblyQualifiedName))
                {
                    myWSProxy = _UnityContainer.Resolve<UnifreightGatewayProxy>(AssemblyQualifiedName /*"ImportDeclarationService" */);
                }
                else
                {
                    var UnifreightGenericService = _UnityContainer.Resolve<UnifreightGenericService>(AssemblyQualifiedName);
                    if (UnifreightGenericService != null)
                    {
                        myWSProxy = UnifreightGenericService;
                    }
                }




                DataIn1 = myWSProxy.GetExampleDataIn1();
                DataIn2 = myWSProxy.GetExampleDataIn2();
                DataOut1 = myWSProxy.GetExampleDataout1();
                DataOut2 = myWSProxy.GetExampleDataout2();
                //DataOut2 = MessageOut;
            }
            catch (Exception e)
            {
                MessageOut = "Exception " + e.ToString();
            }

        }


        public void Dispose()
        {
            _swGatewayLog.Stop();
            _sbGatewayLog.AppendLine("Dispose:ElapsedMilliseconds=" + _swGatewayLog.ElapsedMilliseconds.ToString());
            Debug.WriteLine(_sbGatewayLog.ToString(), false);
        }


        private static void GetSivugTest(ref string AssemblyQualifiedName, ref string DataIn1, ref string MoreParams)
        {
            AssemblyQualifiedName = "Logitude.Customs.BL.Messaging.U2L.Sivug.SivugUpsertService";
            MoreParams = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<ArrayOfEntry xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n <Entry>\n  <Key>TENANT</Key>\n  <Value>1</Value>\n </Entry>\n <Entry>\n  <Key>UNIFREIGHT_USER_ID</Key>\n  <Value>ITZIK</Value>\n </Entry>\n</ArrayOfEntry>\n";

            DataIn1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<LOGISIVUG>\n <SIVUG>\n  .........";
        }

    }
}
