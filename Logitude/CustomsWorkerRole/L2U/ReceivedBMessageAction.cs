using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomsWorkerRole.Queue;

namespace CustomsWorkerRole.L2U
{
    abstract class ReceivedBMessageAction
    {
        BrokeredMessage _ReceivedBrokeredMessage;
        protected int _Tenant = 0;
        protected string _CommunicationLogId;
        ICommonDataContext _Context;
        protected  CommunicationLog _WaitingCommLog;

        public ReceivedBMessageAction(BrokeredMessage receivedBrokeredMessage)
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

        public void ProccessReceivedMessage()
        {

            LogMessagingUtil.Instance.Clear();
            Parse();
            _Context = CommonDataContext.GetContext(_Tenant);
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(_Context);
            CommunicationLogRepository commLogrepository = new CommunicationLogRepository(_Context);
            LogMessagingUtil.Instance.AppendLine("ProccessReceivedMessage()")
                    .Append("CommunicationLogId:").Append(_CommunicationLogId).Append(",Tenant").Append(_Tenant);
            _WaitingCommLog = communicationLogRep.GetSingleCommunicationLog(_CommunicationLogId, _Tenant);
            if (_WaitingCommLog == null)
            {
                
                var myEx =new Exception("GetSingleCommunicationLog(_CommunicationLogId:" + _CommunicationLogId + " , _Tenant:" + _Tenant.ToString() + ") == null");
                ExceptionHandler.HandleException(myEx, DateTime.Now, _Tenant, "", "WorkerRole", "", null);
                _ReceivedBrokeredMessage.SafeComplete(); //Stop Try !!
                return;
            }
            var ExceptionMessage="";
            var Retries = _ReceivedBrokeredMessage.GetProperty<int>( QueueExt.QueuePropertyNames.Retries, 0);
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
                    LogMessagingUtil.Instance.Append("DoAction..");
                    if (DoAction(xmlfile))
                    {
                        //cl.ExceptionMessage 
                        //cl.ExternalDocument  
                        //cl.Logs  
                        //cl.CorrelationID  
                        _WaitingCommLog.CommunicationStatusTypeCode = "D";
                        //LogDoneItemInMemory();
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
                LogMessagingUtil.Instance.AppendLine(":" + _WaitingCommLog.CommunicationStatusTypeCode);
                commLogrepository.Update(_WaitingCommLog);
                commLogrepository.SubmitChanges();

                _ReceivedBrokeredMessage.SafeComplete();
            }
            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc, DateTime.Now, _Tenant, "", "WorkerRole", "",null);
                _WaitingCommLog.Retries++;
                var i=_ReceivedBrokeredMessage.GetProperty<int>(QueueExt.QueuePropertyNames.Retries, 0);

                _ReceivedBrokeredMessage.SetProperty<int>(QueueExt.QueuePropertyNames.Retries, ++i);
                var s = "ProccessReceivedMessage()Exception:" + exc.Message;
                _WaitingCommLog.ExceptionMessage =   s.Substring(0,Math.Min(7999,s.Length));
                commLogrepository.Update(_WaitingCommLog);
                commLogrepository.SubmitChanges();
                _ReceivedBrokeredMessage.SafeAbandon();
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
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = _WaitingCommLog.DocumentId,
                FolderName = _WaitingCommLog.Document.Folder,
                Extension = _WaitingCommLog.Document.Extension,
                Tenant = _WaitingCommLog.Tenant,
                

            };
            byte[] datainByte = storageservice.Read(fileInfo);
            if (datainByte != null)
            {
                 
                Encoding encoding = Encoding.UTF8;
                xmlfile = encoding.GetString(datainByte);
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("Bolb is null! response ..");
                 
            }
            
            return xmlfile;
        }

        public abstract bool DoAction(string actionParams);
        
       
    }
}
