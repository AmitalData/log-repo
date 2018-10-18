using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.Utils;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public abstract class MessagingServiceBase20131016<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestService, TResponseService> 
        : Logitude.CustomsMessaging.MessagingServices.IMessagingServiceBase<TRequestParams,TResponseData,TCustomsResponse>
        where TCustomsRequest :class ,new()
        where TCustomsResponse : class ,IINF_MSG_Generic
        where TRequestService : RequestServiceBase<TCustomsRequest, TRequestParams>, new()
        where TRequestParams : RequestParamsBase
        where TResponseService : ResponseServiceBase<TResponseData, TCustomsResponse, TRequestParams>, new()
        where TResponseData : ResponseDataBase, new()
    {


        
        protected TRequestParams RequestParams;
        protected IResponseHeaderOrFault _ResponseHeader;
        private string _CorrelationId;
        abstract protected TCustomsResponse CallWS(TCustomsRequest customRequest, TRequestParams requestParams, out string exceptionMessage);

        virtual protected TCustomsResponse CallWSTest(TCustomsRequest customRequest, TRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = "No test is implemented please make sure that you implemented the CallWSTest and that you don't call the overridden base method";
            return null;
        }

        virtual protected void PreCallWS(TCustomsRequest customRequest, TRequestParams requestParams)
        {
            
        }
        
        


        public TResponseData Send(TRequestParams requestParams)
        {

            Stopwatch stopwatch = null;
            
            try
            {

                string tempXml = "";
                string exceptionMessage = null;
                string requestCommunicationLogId = null;
                string responseCommunicationLogId = null;

                TCustomsResponse customsResponse = default(TCustomsResponse);
                TRequestService requestService = new TRequestService();

                stopwatch = Stopwatch.StartNew();
                TCustomsRequest customsRequest = requestService.GetRequest(requestParams);
                stopwatch.Stop();
                Debug.WriteLine("MessagingServiceBase:requestService.GetRequest:" + stopwatch.Elapsed.ToString());
                

                if (requestParams.LoggingEnabled)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        LogRequest(customsRequest, requestParams, out requestCommunicationLogId);
                        scope.Complete();
                    }
                }


                try
                {

                    using (TransactionScope scope = new TransactionScope())
                    {

                        stopwatch = Stopwatch.StartNew();
                        PreCallWS(customsRequest, requestParams);
                        stopwatch.Stop();
                        Debug.WriteLine("MessagingServiceBase:PreCallWS:" + stopwatch.Elapsed.ToString());
                        scope.Complete();
                    }

                    LogMessagingUtil.Instance.Clear();
                    tempXml = UnifreightIIG.Common.Utils.XmlGenericUtil<TCustomsRequest>.SerilazeObject(customsRequest);
                    LogMessagingUtil.Instance.AppendLine("customsRequest:").AppendLine(tempXml);
                    Debug.WriteLine("customsRequest:");
                    Debug.WriteLine(tempXml);


                    stopwatch = Stopwatch.StartNew();
                    if (requestParams.TestCase != null && requestParams.TestCase.Type == "customservice" && requestParams.TestCase.Code != "Real Logic")
                    {
                        customsResponse = CallWSTest(customsRequest, requestParams, out exceptionMessage);
                    }
                    else
                    {
                        customsResponse = CallWS(customsRequest, requestParams, out exceptionMessage);
                        Debug.WriteLine("MessagingServiceBase:CallWS:" + stopwatch.Elapsed.ToString());
                        this._CorrelationId = _ResponseHeader.CorrelationId;

                        Debug.WriteLine("responseHeader.CorrelationId =" + _ResponseHeader.CorrelationId);
                        {
                            LogMessagingUtil.Instance.AppendLine("responseHeader.CorrelationId =" + _ResponseHeader.CorrelationId);
                            tempXml = UnifreightIIG.Common.Utils.XmlGenericUtil<TCustomsResponse>.SerilazeObject(customsResponse);
                            LogMessagingUtil.Instance.AppendLine("customsResponse:").AppendLine(tempXml);
                            //Debug.WriteLine("customsResponse:");
                            //Debug.WriteLine(xml);
                        }

                        UnifreightIIGFault.ThrowIIGBLException(
                            _ResponseHeader,
                            customsResponse.GetResponseContentHeader() as IResponseContentHeader
                            );
                    }

                    stopwatch.Stop();






                    tempXml = UnifreightIIG.Common.Utils.XmlGenericUtil<TCustomsResponse>.SerilazeObject(customsResponse);
                    LogMessagingUtil.Instance.AppendLine("CustomsResponse:").AppendLine(tempXml);
                    Debug.WriteLine("CustomsResponse:");
                    Debug.WriteLine(tempXml);





                    //    UnifreightIIGFault.ThrowIIGBLException(
                    //responseHeader,customsResponse.ResponseContentHeader as IResponseContentHeader);
                }
                catch (Exception ex)
                {
                    var defaultMessage = "Sending request to IIG Server Failed ";

                    exceptionMessage = ErrorHandlerUtil.CreateNew().ToFormattedMessage(ex);
                    if (String.IsNullOrWhiteSpace(exceptionMessage))
                    {
                        exceptionMessage = defaultMessage;
                    }
                    LogMessagingUtil.Instance.AppendLine("exceptionMessage:" + exceptionMessage);

                    if (requestParams.LoggingEnabled && !string.IsNullOrWhiteSpace(requestCommunicationLogId))
                    {
                        if (requestParams.LoggingEnabled)
                        {
                            using (TransactionScope scope = new TransactionScope())
                            {
                                UpdateCommunicationLog(requestParams, requestCommunicationLogId, exceptionMessage);
                                scope.Complete();
                            }

                        }
                    }
                    Debug.WriteLine(exceptionMessage);
                    return new TResponseData() { ExceptionMessage = exceptionMessage, HasException = true };
                }



                _ResponseService = new TResponseService();

                try
                {

                    using (TransactionScope scope = new TransactionScope())
                    {
                        
                        stopwatch = Stopwatch.StartNew();
                        Update(customsResponse, requestParams);
                        stopwatch.Stop();
                        Debug.WriteLine("MessagingServiceBase:Update:" + stopwatch.Elapsed.ToString());

                        scope.Complete();
                    }    
                    
                }
                
                catch (Exception ex)
                {
                    exceptionMessage = ex.Message;


                    using (TransactionScope scope = new TransactionScope())
                    {
                        UpdateCommunicationLog(requestParams, requestCommunicationLogId, exceptionMessage);
                        scope.Complete();
                    }    
                    
                    return new TResponseData() { ExceptionMessage = exceptionMessage, HasException = true };
                }

                if (requestParams.LoggingEnabled)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        LogResponse(customsResponse, requestParams, out responseCommunicationLogId);
                        scope.Complete();
                    }

                }

                
                var ResponseData = _ResponseService.GetResponse(customsResponse, requestParams);
                
                return ResponseData;
            }
            catch (Exception exception1)
            {
                Debug.WriteLine(exception1.ToString());
                return new TResponseData() { ExceptionMessage = "Unexpected Exception " + exception1.Message, HasException = true };
            }
        }


        private void UpdateCommunicationLog(TRequestParams requestParams, string requestCommunicationLogId, string exceptionMessage)
        {
            int tenant = requestParams.Tenant;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            CommunicationLog commLog = communicationLogRepository.GetSingleCommunicationLog(requestCommunicationLogId, tenant);
            commLog.ExceptionMessage = exceptionMessage;
            commLog.CorrelationID = this._CorrelationId;
            commLog.Logs = LogMessagingUtil.Instance.ToString(8000) ;

            commLog.CommunicationStatusTypeCode = "F";
            commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
            communicationLogRepository.Update(commLog);
            communicationLogRepository.SubmitChanges();

        }
        TResponseService _ResponseService;
        public void Update(TCustomsResponse customsResponse, TRequestParams requestParams)
        {
            try
            {
                _ResponseService.Update(customsResponse, requestParams);
            }
            catch (DbEntityValidationException ex)
            {
                throw ExceptionFormatUtil.GetFormated(ex);
            }

            catch (Exception)
            {

                throw;
            }

        }

        private void LogRequest(TCustomsRequest customRequest, TRequestParams requestParams, out string communicationLogId)
        {
            Document document = LogMessage(requestParams, requestParams.RequestName, "O", out communicationLogId);
            SerializeCustomRequest(customRequest, requestParams, document);
        }

        private void LogResponse(TCustomsResponse customResponse, TRequestParams requestParams, out string communicationLogId)
        {
            Document document = LogMessage(requestParams, requestParams.ResponseName, "I", out communicationLogId);
            SerializeCustomResponse(customResponse, requestParams, document);
        }

        private Document LogMessage(TRequestParams requestParams, string subject, string InOut, out string communicationLogId)
        {
            int tenant = requestParams.Tenant;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentrepository = new DocumentRepository(commonContext);

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = 999,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "customs",
            };
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                To = "Customs",
                InOut = InOut,
                EntityId = requestParams.LoggingEntityId,
                ObjectTableId = requestParams.LoggingObjectTableId,
                Subject = subject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "D",
                CreatedByUserId = requestParams.LoggingUserId,
                DocumentId = document.Id,
                EntityReference = requestParams.LoggingEntityReference,
                CorrelationID = this._CorrelationId,
                Logs = LogMessagingUtil.Instance.ToString(8000),
                LastStatusDateUTC = DateTime.UtcNow,
                CreateDateUTC = DateTime.UtcNow,
                DoneDateUTC=DateTime.UtcNow,
                DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant),
            };
            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            communicationLogId = commLog.Id;
            return document;
        }

        private void SerializeCustomRequest(TCustomsRequest customRequest, TRequestParams requestParams, Document document)
        {
            MemoryStream memstream = new MemoryStream();

            XmlSerializer ser = new XmlSerializer(typeof(TCustomsRequest));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "http://www.champ.aero/GCCS/CargoXML");
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,


            };

            XmlWriter writer = XmlTextWriter.Create(memstream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");
            ser.Serialize(writer, customRequest, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            content = content.Replace(" />", "/>");
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            string filename = document.Id + "." + document.Extension;
            var blobContainer = StorageAcountDetails.GetCurrentContainer(requestParams.Tenant);
            var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));
            using (Stream blobstream = blobfile.OpenWrite())
            {
                blobstream.Write(memstream.ToArray(), 0, (int)memstream.Length);
            }
        }

        private void SerializeCustomResponse(TCustomsResponse customResponse, TRequestParams requestParams, Document document)
        {
            MemoryStream memstream = new MemoryStream();

            XmlSerializer ser = new XmlSerializer(typeof(TCustomsResponse));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "http://www.champ.aero/GCCS/CargoXML");
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,


            };

            XmlWriter writer = XmlTextWriter.Create(memstream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");
            ser.Serialize(writer, customResponse, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            content = content.Replace(" />", "/>");
            byte[] bytearray = Encoding.ASCII.GetBytes(content);
            string filename = document.Id + "." + document.Extension;
            var blobContainer = StorageAcountDetails.GetCurrentContainer(requestParams.Tenant);
            var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));
            using (Stream blobstream = blobfile.OpenWrite())
            {
                blobstream.Write(memstream.ToArray(), 0, (int)memstream.Length);
            }
        }

        public virtual string TestSendXml(string customXmlRequest,out string exceptionMessage)
        {
            exceptionMessage = "";
            var customXmlResponseXmlOut = "";
            //var  myRequest = 
            TCustomsRequest customsRequest = default(TCustomsRequest);
            XmlSerializer ser = new XmlSerializer(typeof(TCustomsRequest));
            try
            {
                using (TextReader tr = new StringReader(customXmlRequest))
                {
                    customsRequest = (TCustomsRequest)ser.Deserialize(tr);
                }
            }
            catch (Exception)
            {
                string text = "";
                customsRequest = new TCustomsRequest();
                using (var stream = new MemoryStream())
                {
                    var myXmlSerializer = new XmlSerializer(typeof(TCustomsRequest));
                    myXmlSerializer.Serialize(stream, customsRequest);
                    // convert stream to string
                    stream.Position = 0;
                    StreamReader reader = new StreamReader(stream);
                    text = reader.ReadToEnd();
                }

                throw new Exception("Bad customXmlRequest!! Try this :" + Environment.NewLine + text);
            }
            

            TRequestParams requestParams = default(TRequestParams);

            TCustomsResponse customsResponse = default(TCustomsResponse);
            try
            {
                customsResponse = this.CallWS(customsRequest, requestParams, out exceptionMessage);
            }
            catch (Exception ex)
            {
                
                exceptionMessage = ErrorHandlerUtil.CreateNew().ToFormattedMessage(ex);
                return "";

            }
            


           


            

             MemoryStream memStream = null;
            XmlTextWriter xmlWriter = null;
            try
            {



                //ser = new XmlSerializer(T); //, SerializeObject.TargetNamespace);
                ser = new XmlSerializer(typeof(TCustomsResponse)); //, SerializeObject.TargetNamespace);

                memStream = new MemoryStream();

                xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8);
                xmlWriter.Namespaces = true;
                xmlWriter.Formatting = Formatting.Indented;
                ser.Serialize(xmlWriter, customsResponse);//, SerializeObject.GetNamespaces());
                xmlWriter.Close();
                memStream.Close();
                
                customXmlResponseXmlOut = Encoding.UTF8.GetString(memStream.GetBuffer());
                customXmlResponseXmlOut = customXmlResponseXmlOut.Substring(customXmlResponseXmlOut.IndexOf(Convert.ToChar(60)));
                customXmlResponseXmlOut = customXmlResponseXmlOut.Substring(0, (customXmlResponseXmlOut.LastIndexOf(Convert.ToChar(62)) + 1));
                
            }
            finally
            {
                if (xmlWriter != null) xmlWriter.Close();
                if (memStream != null) memStream.Dispose();

            }
            return customXmlResponseXmlOut;

            
        }





        
    }
}
