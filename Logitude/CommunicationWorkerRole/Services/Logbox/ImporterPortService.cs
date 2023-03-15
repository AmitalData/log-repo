using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.Logbox
{
    class ImporterPortService
    {
        public string URI = "";
        private APILogsPM LogPM;
        private IWebFreightContext webFreightContext;
        private int tenant;
        private string correlationId;
        APILogsService apiLogsService;
        QueueResponse Response;

        public ImporterPortService(QueueResponse queueResponse)
        {
            this.tenant = int.Parse(queueResponse.MessageValues["Tenant"].ToString());
            URI = CustomerTenantsURLService.Get();
            webFreightContext = WebFreightContext.GetContext(tenant);
            correlationId = queueResponse.MessageId;
            this.Response = queueResponse;
        }
        public void Run(string portId, int tenant)
        {
            //Authenticate
            string token = APICredentialsAuthenticationService.Authenticate(URI);
            //APILOG
            LogPM = GetLogPM();

            PortQuery portQuery = new PortQuery(tenant);
            PortPM portPM = portQuery.GetSinglePM(portId, tenant);

            LogPM.Refrence = portPM.Code;

            //APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, "Start Sending Port", LogitudeXmlSerializer.SerializeObjectToXmlString(port), null, null, "");
            //else
            //    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(port), null, null, "");

            //string documentsFilingObjectTableId = GetDocumentsFilingObjectTableId();
            //LogPM.ObjectTableId = documentsFilingObjectTableId;
            //LogPM.Tenant = tenant;
            //LogPM.Subject = "Start Building Queues For Documents Batch ImporterShipmentDocuments Controller";
            //if (isNewLog)
            //{
            //    CreateAPILog(LogPM);
            //}

            //PortQuery portQuery = new PortQuery(tenant);
            //PortPM portPM = portQuery.GetSinglePM(portId, tenant);
            if (portPM == null) return;
            //Reference

            
            // we need to get logbox api (portCode,CountryCode)   -> true/false
            //isPortExist

            //    new PortAM();
            // if false -> create of ports -> create api (details)
            // else update

            // pass portAM to create 
        }
        private APILogsPM GetLogPM()
        {
            APILogsRepository aPILogsRepository = new APILogsRepository(webFreightContext);
            APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(correlationId, tenant);
            bool IsNewLog = false;
            APILogsPM LogPM;
            if (Log == null)
            {
                IsNewLog = true;
                LogPM = CreateNewLog();
            }

            IsNewLog = false;
            LogPM = UpdateLog(Log);
            if (IsNewLog)
            {
                //LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                LogPM.QueueType = "Port";
                apiLogsService.Create(LogPM);
            }
            return LogPM;
        }

        private static APILogsPM UpdateLog(APILogs Log)
        {
            return new APILogsPM()
            {
                Id = Log.Id,
                CorrelationId = Log.CorrelationId,
                CreateDate = Log.CreateDate,
                CreateDateUTC = Log.CreateDateUTC,
                Direction = Log.Direction,
                EntityId = Log.EntityId,
                LastUpdateDate = Log.LastUpdateDate,
                LastUpdateDateUTC = Log.LastUpdateDateUTC,
                NumberOfRetries = Log.NumberOfRetries++,
                ObjectTableId = Log.ObjectTableId,
                ExpirationDate = Log.ExpirationDate,
                Refrence = Log.Refrence,
                Status = "I",
                Tenant = Log.Tenant,
                QueueMessageMoreDetailsId = Log.QueueMessageMoreDetailsId
            };
        }

        private APILogsPM CreateNewLog()
        {
            return new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", tenant),
                CorrelationId = correlationId,
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "O",
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ExpirationDate = DateTime.Now.AddDays(90),
                Status = "I",
                QueueMessageMoreDetailsId = correlationId
            };
        }
    }
}
