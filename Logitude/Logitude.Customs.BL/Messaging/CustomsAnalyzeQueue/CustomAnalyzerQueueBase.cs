using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Def.ClosedTable;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
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
        private CommunicationLog _CommunicationLog;

        protected StringBuilder _SBLog;

        //int  _SeedTenant=1;
        public CustomAnalyzerQueueBase(InterfaceDetails MyInterfaceDetails)
        {
            _SBLog = new StringBuilder();
            this._InterfaceDetails = MyInterfaceDetails;
            
        }
        public void Run(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {







            try
            {

                this._AnalyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
                _CommunicationLogRepository = new CommunicationLogRepository(this._AnalyzeQueue.Tenant);
                if (analyzeQueue == null)
                {
                    throw new Exception("AnalyzeQueue analyzeQueue is must ");
                }
                if (String.IsNullOrWhiteSpace(this._AnalyzeQueue.CommunicationLogId))
                {
                    throw new Exception("this._AnalyzeQueue.CommunicationLogId is null");
                }





                _CommunicationLog = Communications.GetCommunicationLog(this._AnalyzeQueue.Tenant, this._AnalyzeQueue.CommunicationLogId);
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
                string AnalyzeErrorProblem = this.AnalyzeData(communicationsData);
                if (String.IsNullOrWhiteSpace(AnalyzeErrorProblem))
                {
                    AnalyzeDone();
                }
                else
                {
                    //Update analyze queue
                    AnalyzeFailed(AnalyzeErrorProblem);
                }
            }

            catch (Exception ex)
            {
                AnalyzeFailed(ex.ToString());

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomAnalyzerQueueBase : Run() Method", null);

            }
        }

        //protected abstract StringBuilder GetStringBuilderLogger();
        protected abstract string AnalyzeData(string communicationsData);
        //public abstract TCustomRequest GetRequest(TRequestParams requestParams);


        private void AnalyzeDone()
        {
            var myCommunicationLogRepository = new CommunicationLogRepository(_AnalyzeQueue.Tenant);

            _CommunicationLog.Retries++;
            _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.D.ToString();
            _CommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(_AnalyzeQueue.Tenant);
            _CommunicationLog.Logs = LogMessagingUtil.Instance.ToString().GetLast((8000 - 1));
            myCommunicationLogRepository.Update(_CommunicationLog);
            myCommunicationLogRepository.SubmitChanges();



            _AnalyzeQueue.Status = "D";
            _AnalyzeQueue.ErrorMessage = null;
            analyzeQueueRepository.Update(_AnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }



        private void AnalyzeFailed(string ErrorMessage)
        {
            if (_CommunicationLog!=null)
            {
                _CommunicationLog.Retries++;
                _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.F.ToString();
                _CommunicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(_AnalyzeQueue.Tenant);
                _CommunicationLogRepository.Update(_CommunicationLog);
                _CommunicationLogRepository.SubmitChanges();

            }



            _AnalyzeQueue.Status = "F";
            _AnalyzeQueue.ErrorMessage = ErrorMessage;
            _AnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(_AnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(_AnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
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
}
