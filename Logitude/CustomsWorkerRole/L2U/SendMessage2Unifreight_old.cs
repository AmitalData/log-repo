using CustomsWorkerRole.L2U;
using Logitude.Customs.BL.DummyData;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
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


namespace CustomsWorkerRole.L2U
{
    class SendMessage2Unifreight_old 
    {
        BrokeredMessage _ReceivedBrokeredMessage;
        //AmitalStandardCommunicationModel _AmitalCommunicationModel = null;
        int _Tenant = 0;
        string _CommunicationLogId;
        ICommonDataContext _Context;
        CommunicationLog _WaitingCommLog;

        public SendMessage2Unifreight_old(BrokeredMessage receivedBrokeredMessage)
        {
            _ReceivedBrokeredMessage=receivedBrokeredMessage;
        }
        

        bool Parse()
        {
            
            _CommunicationLogId = _ReceivedBrokeredMessage.Properties["CommunicationLogId"].ToString();

            int.TryParse(_ReceivedBrokeredMessage.Properties["Tenant"].ToString(), out _Tenant);
            
            //if (_ReceivedBrokeredMessage.Properties.ContainsKey("AmitalStandardCommunicationModel.UnifaceMethodType"))
            //{
            //    Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod operationMethod =
            //    (Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod)_ReceivedBrokeredMessage
            //    .Properties["AmitalStandardCommunicationModel.UnifaceMethodType"];

            //    _AmitalCommunicationModel = new AmitalStandardCommunicationModel(
            //        operationMethod,
            //        (string)_ReceivedBrokeredMessage.Properties["AmitalStandardCommunicationModel.UnifaceComponentName"],
            //    (string)_ReceivedBrokeredMessage.Properties["AmitalStandardCommunicationModel.UnifaceOperation"]);
            //}
            return true;
        }

        public void SendDataToAmital()
        {

            LogMessagingUtil.Instance.Clear();
            Parse();
            _Context = CommonDataContext.GetContext(_Tenant);
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(_Context);
            CommunicationLogRepository commLogrepository = new CommunicationLogRepository(_Context);
            LogMessagingUtil.Instance.AppendLine("SendDataToAmital()")
                    .Append("CommunicationLogId:").Append(_CommunicationLogId).Append(",Tenant").Append(_Tenant);
            _WaitingCommLog = communicationLogRep.GetSingleCommunicationLog(_CommunicationLogId, _Tenant);
            var ExceptionMessage="";
            
            if (_WaitingCommLog.Retries == null)
            {
                _WaitingCommLog.Retries = 0;
            }
            try
            {
                if (_WaitingCommLog.Retries < 5)
                {
                    var xmlfile = GetCommDataFromBlob();
                    if (string.IsNullOrWhiteSpace(xmlfile))
                    {
                        
                    }
                    if (SendMessageToUServer(xmlfile))
                    {
                        //cl.ExceptionMessage 
                        //cl.ExternalDocument  
                        //cl.Logs  
                        //cl.CorrelationID  
                        _WaitingCommLog.CommunicationStatusTypeCode = "D";
                    }
                    else
                    {
                        _WaitingCommLog.CommunicationStatusTypeCode = "F";
                    }
                }

                else
                {
                    _WaitingCommLog.CommunicationStatusTypeCode = "F";
                }

                //if (_Context != null)
                //{

                commLogrepository.Update(_WaitingCommLog);
                commLogrepository.SubmitChanges();
                //}


            }

            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc, DateTime.Now, _Tenant, "", "WorkerRole", "");

                _WaitingCommLog.Retries++;

                _WaitingCommLog.ExceptionMessage = "SendDataToAmital()Exception:" + exc.Message;
                commLogrepository.Update(_WaitingCommLog);
                commLogrepository.SubmitChanges();

                //throw;

            }
        }

        public string GetCommDataFromBlob()
        {
            string xmlfile = "";

            CommunicationAttachmentRepository communicationAttachmentRep = new CommunicationAttachmentRepository(_WaitingCommLog.Tenant);

            List<CommunicationAttachment> attachmentsList = 
                communicationAttachmentRep
                .GetCommunicationAttachmentsForCommLog(_WaitingCommLog.Id, _WaitingCommLog.Tenant).ToList();//(from attach in context.CommunicationAttachments


            if (_WaitingCommLog == null)
            {
                throw new Exception("CommunicationAttachment is empty WaitingCommLog.Id=" + _WaitingCommLog.Id + ",Tenant=" + _WaitingCommLog.Tenant);
            }

            string filename;
            if (_WaitingCommLog.Document == null)
            {
                throw new Exception("CommunicationAttachment WaitingCommLog.Document== null WaitingCommLog.Id=" + _WaitingCommLog.Id + ",Tenant=" + _WaitingCommLog.Tenant);
            }

            filename = _WaitingCommLog.DocumentId + "." + _WaitingCommLog.Document.Extension;

            //CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(_WaitingCommLog.Tenant);
            //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, _WaitingCommLog.Document.Folder));


            //using (MemoryStream memstream = new MemoryStream())
            //{
            //    blobfile.DownloadToStream(memstream);
            //    Encoding encoding = Encoding.UTF8;
            //    xmlfile = encoding.GetString(memstream.ToArray());
            //}


            string filePath = "tenant" + _WaitingCommLog.Tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), _WaitingCommLog.Document.Folder);
            LogMessagingUtil.Instance.AppendLine("Try Read Bolb :" + filePath);

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            Logitude.Server.Tools.BlobServiceReference.Response response = storageservice.Read(filePath);
            if (response.Result != null)
            {
                byte[]  datainByte = response.Result as byte[];

                Encoding encoding = Encoding.UTF8;
                xmlfile = encoding.GetString(datainByte);
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("Bolb is null! response ..");
                LogMessagingUtil.Instance.AppendLine(response.ErrorMessage);
            }
            
            return xmlfile;
        }

        private bool SendMessageToUServer(string urouterParams)
        {
            //implement the code to send the xml file to amital;

            string P_MOREPARAMS = "";
            string P_XML_DATA = "";
            string P_MESSAGE = "";
            if (String.IsNullOrWhiteSpace(urouterParams))
            {

                throw new ArgumentNullException("SendFileToAmitalService():xmlfile is null");
            }

            var setting = UnifreightIIGCommonUtil.GetTenantSetting(_Tenant);
            var myUServerDNS = setting.UServerDNS;
            var myUServerPort = setting.UServerPort;
            var myUServerUtil = new UServerUtil(myUServerDNS, myUServerPort); ;

            
            myUServerUtil.DoIt(urouterParams, ref P_MOREPARAMS, out P_XML_DATA, out P_MESSAGE);
            //_WaitingCommLog.Logs += "UServer did not return response ";
            _WaitingCommLog.Logs += P_XML_DATA + Environment.NewLine;
            _WaitingCommLog.Logs += myUServerUtil.UnifreightTester;
            if (String.IsNullOrWhiteSpace(P_XML_DATA))
            {
                _WaitingCommLog.Logs+= "UServer did not return response ";
                return false;    
            }
            else
            {
                //todo: Create New commincation/ANALYZQUEUE ?? 
            }

            
            // var UNIQUE_ENVIRONMENT_ID = UnifaceAssociativeListUtil.GetValue(P_XML_DATA, "UNIQUE_ENVIRONMENT_ID");

            return true;
        }

       
    }
}
