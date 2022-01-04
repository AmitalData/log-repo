using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Def.ClosedTable;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public abstract class CustomAnalyzerQueueBase
    {
        protected AnalyzeQueue _AnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        CommunicationLogRepository _CommunicationLogRepository;
        

        protected InterfaceDetails _InterfaceDetails;
        protected QueueDetails _QueueDetails;

        protected CommunicationLog _CommunicationLog;

        protected StringBuilder _SBLog;
        private AnalyzeResultModel _AnalyzeResultModel;

        //int  _SeedTenant=1;
        public CustomAnalyzerQueueBase(InterfaceDetails MyInterfaceDetails)
        {
            _SBLog = new StringBuilder();
            this._InterfaceDetails = MyInterfaceDetails;
            
        }

        public CustomAnalyzerQueueBase(QueueDetails queueDetails)
        {
            _SBLog = new StringBuilder();
            this._QueueDetails = queueDetails;

        }

        public void Run( AnalyzeQueueRepository analyzeQueueRepository, int tenant, string candidateCommunicationLogId , string message, QueueDetails queue, out string log , out bool success)
        {
            success = false;
            log = "none";
            try
            {
                this.analyzeQueueRepository = analyzeQueueRepository;

                if (String.IsNullOrWhiteSpace(candidateCommunicationLogId))
                {
                    
                        var _CommunicationsParams = new CommunicationsParams()
                        {

                            Tenant = tenant,

                            //LoggingObjectTableId = objectTableId,
                            //LoggingEntityId = entityId,

                            Subject = queue.Name,
                            //LoggingEntityReference = documentsFilingPM.ExternalEntityReference,
                           // LoggingUserId = LoggingUserId,
                            //CorrelationID = documentsFilingPM.Id,

                            Status = "W",
                            To = "RabbitMQ",
                            CommunicationLogTypeCode = "T",
                            FolderName = "RabbitMQ",
                            From = "Logitude",
                            InOut = "O",

                        };


                        var messageByte = Encoding.UTF8.GetBytes(message);
                        _CommunicationsParams.ByteData = messageByte;
                          candidateCommunicationLogId = Communications.AddCommunicationLog(_CommunicationsParams);

                    
                     //   throw new Exception("CommunicationLogId is null");
                }

                try
                {
                    _CommunicationLog = Communications.GetCommunicationLog(tenant, candidateCommunicationLogId);
                    if (_CommunicationLog == null)
                    {
                        _CommunicationLog = Communications.GetCommunicationLogByCorrelationID(tenant, candidateCommunicationLogId);
                    }
                }
                catch (Exception ex)
                {
                    log = "Cannnot GetCommunicationLog : " + ex.Message;

                    throw new Exception("Cannnot GetCommunicationLog");
                }

                if (_CommunicationLog == null)
                {


                    var _CommunicationsParams = new CommunicationsParams()
                    {

                        Tenant = tenant,

                        //LoggingObjectTableId = objectTableId,
                        //LoggingEntityId = entityId,

                        Subject = queue.Name,
                        //LoggingEntityReference = documentsFilingPM.ExternalEntityReference,
                        // LoggingUserId = LoggingUserId,
                        //CorrelationID = documentsFilingPM.Id,
                        CorrelationID = candidateCommunicationLogId,
                        Status = "W",
                        To = "RabbitMQ",
                        CommunicationLogTypeCode = "T",
                        FolderName = "RabbitMQ",
                        From = "Logitude",
                        InOut = "O",

                    };


                    var messageByte = Encoding.UTF8.GetBytes(message);
                    _CommunicationsParams.ByteData = messageByte;
                    candidateCommunicationLogId = Communications.AddCommunicationLog(_CommunicationsParams);

                    _CommunicationLog = Communications.GetCommunicationLog(tenant, candidateCommunicationLogId);

                }

                if (_CommunicationLog.Retries == 5)
                { success = true;
                    log = "_CommunicationLog.Retries == 5";

                    return;

                 }
                log = "_CommunicationLog != null";

                var communicationsData = message;  

                try
                {
                    _AnalyzeResultModel = this.AnalyzeData(communicationsData);
                 }
                catch (Exception ex )
                {
                    log = ex.Message + ex.StackTrace;
                    _AnalyzeResultModel = _AnalyzeResultModel ?? new AnalyzeResultModel();
                    _AnalyzeResultModel.MyCommStatusEnum = CommStatusEnum.W;
                    _AnalyzeResultModel.ErrorMessage = log;

                    success = false;
                     throw new Exception(log);
                }


                log = _AnalyzeResultModel.ErrorMessage;

                //;
                _AnalyzeResultModel = _AnalyzeResultModel ?? new AnalyzeResultModel();
                LogMessagingUtil.Instance.AppendLine(ProxyUtil.JsonConvertSerialize(_AnalyzeResultModel));
               
                if( _CommunicationLog == null)
                {
                    _CommunicationLog = Communications.GetCommunicationLog(tenant, candidateCommunicationLogId);

                }
                if (_CommunicationLog != null)
                {
                    UpdateAnlayzeDone();
                    success = true;
                }
                else
                {
                    log = log + "---" + "cannot update ";
                    success = false;
                    UpdateAnlayzeRetry();

                }

            }

            catch (Exception ex)
            {
                _AnalyzeResultModel = _AnalyzeResultModel ?? new AnalyzeResultModel()
                {
                    ErrorMessage = ex.ToString(),
                    MyCommStatusEnum = CommStatusEnum.W
                };
                //AnalyzeFailed(ex.ToString());
               // UpdateAnlayzeDone();
                UpdateAnlayzeRetry();

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomAnalyzerQueueBase : Run() Method", null);

            }
        }

        public void Run(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository, int tenant)
        {







            try
            {

                this._AnalyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
                
                if (analyzeQueue == null)
                {
                    throw new Exception("AnalyzeQueue analyzeQueue is must ");
                }
                if (String.IsNullOrWhiteSpace(this._AnalyzeQueue.CommunicationLogId))
                {
                    throw new Exception("this._AnalyzeQueue.CommunicationLogId is null");
                }



                

                _CommunicationLog = Communications.GetCommunicationLog(tenant, this._AnalyzeQueue.CommunicationLogId);
                if (_CommunicationLog == null)
                {
                    throw new Exception("Cannnot GetCommunicationLog");
                }
                var communicationsData = Communications.GetData(_CommunicationLog); ;
                if (string.IsNullOrWhiteSpace(communicationsData))
                {
                    throw new Exception("communicationsData is null");
                }



                LogMessagingUtil.Instance.Clear();
                _AnalyzeResultModel = this.AnalyzeData(communicationsData);
                ;
                _AnalyzeResultModel = _AnalyzeResultModel ?? new AnalyzeResultModel(); 
                LogMessagingUtil.Instance.AppendLine(ProxyUtil.JsonConvertSerialize(_AnalyzeResultModel));
                UpdateAnlayzeQ();
            }

            catch (Exception ex)
            {
                _AnalyzeResultModel = _AnalyzeResultModel ?? new AnalyzeResultModel()
                {
                    ErrorMessage = ex.ToString(),
                    MyCommStatusEnum = CommStatusEnum.F
                };
                UpdateAnlayzeQ();
                //AnalyzeFailed(ex.ToString());

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomAnalyzerQueueBase : Run() Method", null);

            }
        }

        //protected abstract StringBuilder GetStringBuilderLogger();
        protected abstract AnalyzeResultModel AnalyzeData(string communicationsData);
        //public abstract TCustomRequest GetRequest(TRequestParams requestParams);


        private void UpdateAnlayzeQ()
        {
            _AnalyzeQueue = _AnalyzeQueue ?? new AnalyzeQueue();
            if (_CommunicationLog != null)
            {


                var myCommunicationLogRepository = new CommunicationLogRepository(_CommunicationLog.Tenant);
                var myCommunicationLog = myCommunicationLogRepository.GetSingleCommunicationLog(_CommunicationLog.Id, _CommunicationLog.Tenant);
                myCommunicationLog.Retries++;
                myCommunicationLog.CommunicationStatusTypeCode = _AnalyzeResultModel.MyCommStatusEnum.ToString();
                myCommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(_AnalyzeQueue.Tenant);
                myCommunicationLog.Logs = _AnalyzeResultModel.ErrorMessage ?? "" + Environment.NewLine + LogMessagingUtil.Instance.ToString().GetLast((8000 - 1));
                myCommunicationLog.ExceptionMessage = _AnalyzeQueue.ErrorMessage;
                myCommunicationLog.EntityReference = _AnalyzeResultModel.EntityReference;
                if (!string.IsNullOrWhiteSpace(_AnalyzeResultModel.EntityID) &&
                    !string.IsNullOrWhiteSpace(_AnalyzeResultModel.ObjectTableID))
                {

                    myCommunicationLog.ObjectTableId = _AnalyzeResultModel.ObjectTableID;
                    myCommunicationLog.EntityId = _AnalyzeResultModel.EntityID;
                    LogMessagingUtil.Instance.AppendLine($".ObjectTableId = {_AnalyzeResultModel.ObjectTableID}");
                    LogMessagingUtil.Instance.AppendLine($".EntityId = {_AnalyzeResultModel.EntityID}");
                    LogMessagingUtil.Instance.AppendLine($".EntityReference = {_AnalyzeResultModel.EntityReference}");
                    

                }
                myCommunicationLogRepository.Update(myCommunicationLog);
                myCommunicationLogRepository.SubmitChanges();

            }
            else
            {
                LogMessagingUtil.Instance.AppendLine($"(_CommunicationLog != null)");
            }

            _AnalyzeQueue.Status = _AnalyzeResultModel.MyCommStatusEnum.ToString();
            string mm = _AnalyzeResultModel.ErrorMessage ?? "";
            mm = mm.Substring(0, Math.Min(mm.Length, 2000 - 1));
            _AnalyzeQueue.ErrorMessage = mm;// _AnalyzeResultModel.ErrorMessage;
            analyzeQueueRepository.Update(_AnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
        private void UpdateAnlayzeRetry()
        {
            _AnalyzeQueue = _AnalyzeQueue ?? new AnalyzeQueue();
            if (_CommunicationLog != null)
            {


                var myCommunicationLogRepository = new CommunicationLogRepository(_CommunicationLog.Tenant);
                var myCommunicationLog = myCommunicationLogRepository.GetSingleCommunicationLog(_CommunicationLog.Id, _CommunicationLog.Tenant);
                myCommunicationLog.Retries++;
                myCommunicationLog.CommunicationStatusTypeCode = _AnalyzeResultModel.MyCommStatusEnum.ToString();
                if (myCommunicationLog.Retries == 5) 
                    myCommunicationLog.CommunicationStatusTypeCode = "F";
   ;            myCommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(_AnalyzeQueue.Tenant);
                myCommunicationLog.Logs = _AnalyzeResultModel.ErrorMessage ?? "" + Environment.NewLine + LogMessagingUtil.Instance.ToString().GetLast((8000 - 1));
                myCommunicationLog.ExceptionMessage = _AnalyzeQueue.ErrorMessage;
                myCommunicationLog.EntityReference = _AnalyzeResultModel.EntityReference;
                if (!string.IsNullOrWhiteSpace(_AnalyzeResultModel.EntityID) &&
                    !string.IsNullOrWhiteSpace(_AnalyzeResultModel.ObjectTableID))
                {

                    myCommunicationLog.ObjectTableId = _AnalyzeResultModel.ObjectTableID;
                    myCommunicationLog.EntityId = _AnalyzeResultModel.EntityID;
                    LogMessagingUtil.Instance.AppendLine($".ObjectTableId = {_AnalyzeResultModel.ObjectTableID}");
                    LogMessagingUtil.Instance.AppendLine($".EntityId = {_AnalyzeResultModel.EntityID}");
                    LogMessagingUtil.Instance.AppendLine($".EntityReference = {_AnalyzeResultModel.EntityReference}");


                }
                myCommunicationLogRepository.Update(myCommunicationLog);
                myCommunicationLogRepository.SubmitChanges();

            }
            else
            {
                LogMessagingUtil.Instance.AppendLine($"(_CommunicationLog != null)");
            }

            //_AnalyzeQueue.Status = _AnalyzeResultModel.MyCommStatusEnum.ToString();
            //string mm = _AnalyzeResultModel.ErrorMessage ?? "";
            //mm = mm.Substring(0, Math.Min(mm.Length, 2000 - 1));
            //_AnalyzeQueue.ErrorMessage = mm;// _AnalyzeResultModel.ErrorMessage;
            //analyzeQueueRepository.Update(_AnalyzeQueue);
            //analyzeQueueRepository.SubmitChanges();
        }

        private void UpdateAnlayzeDone()
        {
            _AnalyzeQueue = _AnalyzeQueue ?? new AnalyzeQueue();
            if (_CommunicationLog != null)
            {


                var myCommunicationLogRepository = new CommunicationLogRepository(_CommunicationLog.Tenant);
                var myCommunicationLog = myCommunicationLogRepository.GetSingleCommunicationLog(_CommunicationLog.Id, _CommunicationLog.Tenant);
                myCommunicationLog.Retries++;
                myCommunicationLog.CommunicationStatusTypeCode = _AnalyzeResultModel.MyCommStatusEnum.ToString();
                myCommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(_AnalyzeQueue.Tenant);
                myCommunicationLog.Logs = _AnalyzeResultModel.ErrorMessage ?? "" + Environment.NewLine + LogMessagingUtil.Instance.ToString().GetLast((8000 - 1));
                myCommunicationLog.ExceptionMessage = _AnalyzeQueue.ErrorMessage;
                myCommunicationLog.EntityReference = _AnalyzeResultModel.EntityReference;
                if (!string.IsNullOrWhiteSpace(_AnalyzeResultModel.EntityID) &&
                    !string.IsNullOrWhiteSpace(_AnalyzeResultModel.ObjectTableID))
                {

                    myCommunicationLog.ObjectTableId = _AnalyzeResultModel.ObjectTableID;
                    myCommunicationLog.EntityId = _AnalyzeResultModel.EntityID;
                    LogMessagingUtil.Instance.AppendLine($".ObjectTableId = {_AnalyzeResultModel.ObjectTableID}");
                    LogMessagingUtil.Instance.AppendLine($".EntityId = {_AnalyzeResultModel.EntityID}");
                    LogMessagingUtil.Instance.AppendLine($".EntityReference = {_AnalyzeResultModel.EntityReference}");


                }
                myCommunicationLogRepository.Update(myCommunicationLog);
                myCommunicationLogRepository.SubmitChanges();

            }
            else
            {
                LogMessagingUtil.Instance.AppendLine($"(_CommunicationLog != null)");
            }

            //_AnalyzeQueue.Status = _AnalyzeResultModel.MyCommStatusEnum.ToString();
            //string mm = _AnalyzeResultModel.ErrorMessage ?? "";
            //mm = mm.Substring(0, Math.Min(mm.Length, 2000 - 1));
            //_AnalyzeQueue.ErrorMessage = mm;// _AnalyzeResultModel.ErrorMessage;
            //analyzeQueueRepository.Update(_AnalyzeQueue);
            //analyzeQueueRepository.SubmitChanges();
        }


        //private void AnalyzeDone()
        //{
        //    var myCommunicationLogRepository = new CommunicationLogRepository(_AnalyzeQueue.Tenant);

        //    _CommunicationLog.Retries++;
        //    _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.D.ToString();
        //    _CommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(_AnalyzeQueue.Tenant);
        //    _CommunicationLog.Logs = LogMessagingUtil.Instance.ToString().GetLast((8000 - 1));
        //    myCommunicationLogRepository.Update(_CommunicationLog);
        //    myCommunicationLogRepository.SubmitChanges();



        //    _AnalyzeQueue.Status = "D";
        //    _AnalyzeQueue.ErrorMessage = null;
        //    analyzeQueueRepository.Update(_AnalyzeQueue);
        //    analyzeQueueRepository.SubmitChanges();
        //}



        //private void AnalyzeFailed()
        //{
        //    if (_CommunicationLog!=null)
        //    {
        //        _CommunicationLog.Retries++;
        //        _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.F.ToString();
        //        _CommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(_AnalyzeQueue.Tenant);
        //        _CommunicationLog.Logs = LogMessagingUtil.Instance.ToString().GetLast((8000 - 1));
        //        _CommunicationLog.ObjectTableId
        //        _CommunicationLogRepository.Update(_CommunicationLog);

        //        _CommunicationLogRepository.SubmitChanges();

        //    }



        //    _AnalyzeQueue.Status = "F";
        //    _AnalyzeQueue.ErrorMessage = ErrorMessage;
        //    _AnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(_AnalyzeQueue.Tenant);
        //    analyzeQueueRepository.Update(_AnalyzeQueue);
        //    analyzeQueueRepository.SubmitChanges();
        //}
#if false
        private void OnCatchAnalyzingError(Exception ex)
        {
            myAnalyzeQueue.ErrorMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");
            myAnalyzeQueue.StackTrace = (ex.StackTrace != null ? Environment.NewLine + "Stack Trace: " + ex.StackTrace : "");
            myAnalyzeQueue.ErrorMessage = myAnalyzeQueue.ErrorMessage.Length > 7950 ? myAnalyzeQueue.ErrorMessage.Substring(0, 7950) : myAnalyzeQueue.ErrorMessage;
            myAnalyzeQueue.StackTrace = myAnalyzeQueue.StackTrace.Length > 7950 ? myAnalyzeQueue.StackTrace.Substring(0, 7950) : myAnalyzeQueue.StackTrace;

            if (ex.Message.StartsWith("--"))
            {
                myAnalyzeQueue.Status = "F";
            }

            else
            {
                myAnalyzeQueue.Retries++;

                if (myAnalyzeQueue.Retries >= 5)
                {
                    myAnalyzeQueue.Status = "F";

                    if (myAnalyzeQueue.ConnectedToTenant)
                    {
                        CommunicationLog commLog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, Tenant);
                        if (commLog != null)
                        {
                            commLog.CommunicationStatusTypeCode = "F";
                            commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                            commLog.LastStatusDateUTC = DateTime.UtcNow;
                            commLog.ExceptionMessage = myAnalyzeQueue.ErrorMessage;

                            if (myAnalyzeQueue.StackTrace != null)
                            {
                                commLog.ExceptionMessage = commLog.ExceptionMessage + Environment.NewLine + "Stack Trace: " + myAnalyzeQueue.StackTrace;
                            }

                            myCommunicationLogRepository.Update(commLog);
                            myCommunicationLogRepository.SubmitChanges();
                        }
                    }
                }
            }
            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }



#endif
    }
    public class AnalyzeResultModel
    {
        public AnalyzeResultModel()
        {
            MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;//default !!
        }
        public string ErrorMessage { get; set; }
        public CommStatusEnum MyCommStatusEnum { get; set; }
        public string EntityID { get; set; }
        public string ObjectTableID { get; set; }
        public string EntityReference { get; set; }
    }
}
