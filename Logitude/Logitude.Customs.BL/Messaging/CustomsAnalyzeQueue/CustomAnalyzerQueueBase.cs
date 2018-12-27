using Logitude.Customs.BL.CloseTables;
using Logitude.Server.Tools;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public abstract class CustomAnalyzerQueueBase
    {
        private AnalyzeQueue _AnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        CommunicationLogRepository myCommunicationLogRepository;
        private MemoryStream myMemoryStream;
        int Tenant;
        protected InterfaceDetails _InterfaceDetails;
        int  _SeedTenant=1;
        public CustomAnalyzerQueueBase(InterfaceDetails MyInterfaceDetails)
        {
            this._InterfaceDetails = MyInterfaceDetails;
        }
        public void Run(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                throw new Exception("AnalyzeQueue analyzeQueue is must ");


            }

            if (String.IsNullOrWhiteSpace(this._AnalyzeQueue.CommunicationLogId))
            {
                throw new Exception("this._AnalyzeQueue.CommunicationLogId is null");
            }





            var comm = Communications.GetCommunicationLog(_SeedTenant, this._AnalyzeQueue.CommunicationLogId);
            if (comm == null)
            {
                throw new Exception("Cannnot GetCommunicationLog");
            }
            var communicationsData = Communications.GetData(comm); ;
            if (string.IsNullOrWhiteSpace(communicationsData))
            {
                throw new Exception("communicationsData is null");
            }


            this.Tenant = analyzeQueue.Tenant;
            this._AnalyzeQueue = analyzeQueue;
            this.analyzeQueueRepository = analyzeQueueRepository;
            this.myCommunicationLogRepository = new CommunicationLogRepository(this.Tenant);
        
        
      
            try
            {

                

                string AnalyzeErrorProblem = this.AnalyzeData(communicationsData);
                if (String.IsNullOrWhiteSpace(AnalyzeErrorProblem))
                {
                    AnalyzeDone();
                }
                else
                {
                    //Update analyze queue
                    AnalyzeFailed();
                }
            }

            catch (Exception ex)
            {
                _AnalyzeQueue.Status = "F";
                _AnalyzeQueue.ErrorMessage = "Artemus Analyzer failed: " + ex.Message;
                _AnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(_AnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(_AnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomAnalyzerQueueBase : Run() Method", null);

            }
        }

        protected abstract string AnalyzeData(string communicationsData);
        //public abstract TCustomRequest GetRequest(TRequestParams requestParams);


        private void AnalyzeDone()
        {
            _AnalyzeQueue.Status = "D";
            _AnalyzeQueue.ErrorMessage = null;
            analyzeQueueRepository.Update(_AnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }

        private void AnalyzeFailed()
        {
            _AnalyzeQueue.Status = "F";
            _AnalyzeQueue.ErrorMessage = "Shipment does not  exist";
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
