
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using WebFreight.Web.CustomWebServices.SignChunks.Common;
using WebFreight.Web.Security;


namespace WebFreight.Web.WcfApi
{
    public partial class ExternalTasksQueueWcfService
    {

        const bool ON_PREMISE= false;

        public ResponseExportSignTask GetExportSignTaskFromQueue(ExportReqSignData exportReqSignData)
        {
            
            
            QueueResponse queueResponse = null;
            var stringBuilder = new StringBuilder();
            
            try
            {

                stringBuilder.AppendLine($"Tenant{exportReqSignData.Tenant}|Company:{exportReqSignData.isCompanySignOn}|Personal{exportReqSignData.isPersonalSignOn}|{exportReqSignData.CurrentSignCertificate}");
                if (!ON_PREMISE)
                {
                    SecurityUtility.AuthenticationOnTenant(exportReqSignData.Tenant);
                }


                

                var currentSignCertificate = SignCertificateClass.Get(exportReqSignData.CurrentSignCertificate);
                //{"InterfaceTypeCode":"8347","Tenant":"1","CorrelationId":"ef284e9d-7228-4a99-97e8-49e5b808af91"}
                MessagingServiceFactoryHelper.InitContainer();

                if (exportReqSignData.ToCheckSignCertificate)
                {
                    string PersonalTenantCommaDelimitedList = SignQueue.GetTenantCommaDelimitedList(exportReqSignData.CurrentSignCertificate);
                    string CompanyTenant = SignQueue.GetCompanyTenant(exportReqSignData.CurrentSignCertificate);


                    return new ResponseExportSignTask()
                    {
                        currTenant = exportReqSignData.Tenant,
                        ResultSignCertificateCheck = new SignCertificateCheck()
                        {
                            isCompanySignOn = !string.IsNullOrWhiteSpace(CompanyTenant),
                            isPersonalSignOn = !string.IsNullOrWhiteSpace(PersonalTenantCommaDelimitedList)
                        }
                    };
                }
                else
                {
                    if (exportReqSignData.isPersonalSignOn)
                    {
                        string queueName = "PersonalSign_" + exportReqSignData.Tenant + "_" + currentSignCertificate.PersonId;
                        DbQueueService queueservice = new DbQueueService(queueName, exportReqSignData.Tenant);
                        stringBuilder.AppendLine("queueName:" + queueName);
                        queueResponse = queueservice.ReceiveDetail(TimeSpan.FromMinutes(4), true);

                        if (queueResponse.RetryNumber > 10)
                        {
                            stringBuilder.AppendLine($"queueResponse.RetryNumber {queueResponse.RetryNumber} > 10");
                            queueservice.Complete();
                            return new ResponseExportSignTask() { };
                        }
                        if (queueResponse.MessageId != null)
                        {



                            string correlationId = queueResponse.MessageValues["CorrelationId"].ToString();
                            string InterfaceTypeCode = queueResponse.MessageValues["InterfaceTypeCode"].ToString();
                            stringBuilder.AppendLine($"Q:{queueResponse.MessageId}:Rtry:{queueResponse.RetryNumber}|CRSid:{correlationId}");
                            var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(InterfaceTypeCode);



                            var BytesToSign = anaO.PasiveSignGetBytesToSign(exportReqSignData.Tenant, correlationId /*CustomsRequestsSheetId*/);
                            stringBuilder.AppendLine($"PasiveSignGetBytesToSign()|Bytes2Sign:{BytesToSign.Length}");
                            return new ResponseExportSignTask()
                            {
                                currTenant = exportReqSignData.Tenant,
                                queueId = queueResponse.MessageId,
                                CustomsRequestsSheetId = correlationId,
                                InterfaceTypeCode = InterfaceTypeCode,
                                ReceiveBytesToSign = BytesToSign
                            };


                        }
                    }


                    if (exportReqSignData.isCompanySignOn)
                    {
                        if (CustomsSettingQueryService.GetSettingByTenant(exportReqSignData.Tenant).CustomsAgentId != currentSignCertificate.CustomsAgentId)
                        {
                            throw new Exception($"GetSettingByTenant({exportReqSignData.Tenant}).CustomsAgentId != signServer.customsAgentId {currentSignCertificate.CustomsAgentId} ");
                        }
                        string queueName = "CompanySign_" + exportReqSignData.Tenant + "_" + currentSignCertificate.CustomsAgentId;
                        DbQueueService queueservice = new DbQueueService(queueName, exportReqSignData.Tenant);//QueueServiceManager.GetQueueService(queueName, 0);
                        stringBuilder.AppendLine("queueName:" + queueName);
                        queueResponse = queueservice.ReceiveDetail(TimeSpan.FromMinutes(4), true);

                        if (queueResponse.RetryNumber > 10)
                        {
                            stringBuilder.AppendLine($"queueResponse.RetryNumber {queueResponse.RetryNumber} > 10");
                            queueservice.Complete();
                            
                            return new ResponseExportSignTask() { };

                        }
                        if (queueResponse.MessageId != null)
                        {
                            //{"InterfaceTypeCode":"8347","Tenant":"1","CorrelationId":"ef284e9d-7228-4a99-97e8-49e5b808af91"}
                            //int.TryParse(queueResponse.MessageValues["Tenant"].ToString(), out tenant);

                            string correlationId = queueResponse.MessageValues["CorrelationId"].ToString();
                            string InterfaceTypeCode = queueResponse.MessageValues["InterfaceTypeCode"].ToString();
                            stringBuilder.AppendLine($"Q:{queueResponse.MessageId}:Rtry:{queueResponse.RetryNumber}|CRSid:{correlationId}");
                            var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(InterfaceTypeCode);
                            if (anaO.CurrentCustomsCommandWR != CustomsCommandEnum.CustomsCommandSignRequestWR)
                            {
                                //anaO.
                            }



                            var BytesToSign = anaO.PasiveSignGetBytesToSign(exportReqSignData.Tenant, correlationId /*CustomsRequestsSheetId*/);
                            stringBuilder.AppendLine($"PasiveSignGetBytesToSign()|Bytes2Sign:{BytesToSign.Length}");
                            return new ResponseExportSignTask() { currTenant = exportReqSignData.Tenant, queueId = queueResponse.MessageId, CustomsRequestsSheetId = correlationId, InterfaceTypeCode = InterfaceTypeCode, ReceiveBytesToSign = BytesToSign };


                        }
                    }

                }


                stringBuilder.AppendLine($"noQ");
                return new ResponseExportSignTask() {   };

            }
            catch (Exception ex)
            {
                var responseExportSignTask = new ResponseExportSignTask();
                responseExportSignTask.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                responseExportSignTask.HasError = true;
                responseExportSignTask.ErrorMessage = ex.Message;
                responseExportSignTask.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    responseExportSignTask.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                stringBuilder.AppendLine($"HandleException:{ex.Message}");
                ExceptionHandler.HandleException(ex, DateTime.Now, exportReqSignData?.Tenant?? 0, "", "GetExportSignTaskFromQueue", "", null);
                return responseExportSignTask; ;

            }
            finally
            {



                LogitudeSettings.HandleLogMe
                /*Logger.LogMe*/(stringBuilder.Replace(Environment.NewLine, "|").ToString(), false, "ExportSignTaskFromQueue_" + (exportReqSignData?.Tenant ?? 0).ToString()
                , GetStopLogAt());
                try
                {

                    var dbSignQueueService = new CloudExportDbSignQueueService();
                    dbSignQueueService.UpsertSignStation(exportReqSignData.CurrentSignCertificate, exportReqSignData.isPersonalSignOn, exportReqSignData.isCompanySignOn);


                }
                catch 
                {

                    
                }
            }
        }

        public Response MarkExportSignTaskAsDone(int tenant, String queueId, String customsRequestsSheetId, string interfaceTypeCode, byte[] signBytes)
        {
            Response response = new Response();
            StringBuilder stringBuilder= new StringBuilder();
            try
            {
                
                if (!ON_PREMISE)
                {
                    //SecurityUtility.AuthenticationOnTenant(exportReqSignData.Tenant);
                }

                MessagingServiceFactoryHelper.InitContainer();
                var mySendSheetSignModel = new SendSheetSignModel();
                mySendSheetSignModel.CustomsRequestsSheetId = customsRequestsSheetId;
                mySendSheetSignModel.Tenant = tenant;
                mySendSheetSignModel.CustomRequestSignedByteArryPasiveSign = signBytes;
                mySendSheetSignModel.CurrentSignCertificateName = "CurrentSignCertificate";

                mySendSheetSignModel.ExportTaskQueueId = queueId;


                MessagingServiceFactoryHelper.InitContainer();
                var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(interfaceTypeCode);
                //anaO.CompleteResponseSignBytes(tenant, CustomsRequestsSheetId, mySignBytes);
                stringBuilder.AppendLine($"CRSid:{customsRequestsSheetId}|");
                var responseData = anaO.SendSheet(tenant, customsRequestsSheetId, mySendSheetSignModel);
                var responseDataBase = (responseData as Logitude.CustomsMessaging.Common.ResponseData.ResponseDataBase);
                if (responseDataBase?.HasException == true)
                {
                    stringBuilder.AppendLine($"SendSheet:HandleException:{responseDataBase.UserMessage}");
                    ExceptionHandler.HandleException(new Exception(message: responseDataBase.UserMessage), DateTime.Now, tenant, "", "MarkExportSignTaskAsDone", "", null);
                }
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

               
                stringBuilder.AppendLine($"HandleException:{ex.Message}");
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "MarkExportSignTaskAsDone", "", null);
                return response;
            }
            finally
            {
                LogitudeSettings.HandleLogMe
                /*Logger.LogMe*/(stringBuilder.Replace(Environment.NewLine,"|").ToString(), false, "MarkExportSignTaskAsDone_" + tenant.ToString(), 
                GetStopLogAt());
                
            }

            return response;
        }

        public Response MarkExportSignTaskAsFail(int tenant, String queueId, String customsRequestsSheetId, string interfaceTypeCode, string ErrorMessage)
        {
            throw new NotImplementedException();
        }


        DateTime GetStopLogAt()
        {
            DateTime stopLogAt = new DateTime(2023, 03, 01);
            try
            {
                string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["ExportSignQ.LogUntilDateyyyyMMdd"];
                if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                {
                    stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        style: DateTimeStyles.None);
                }
            }
            catch (Exception)
            {

            }
            return stopLogAt;

        }
    }
}