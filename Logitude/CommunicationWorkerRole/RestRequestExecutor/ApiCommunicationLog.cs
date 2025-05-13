using Logitude.Server.Tools;
using System;
using System.Threading.Tasks;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;

namespace CommunicationWorkerRole.RestRequestExecutor
{
    public class ApiCommunicationLog
    {              
        public string AddCommunicationLog(string request, string response, int tenant, StatusTypeCommunication statusTypeCode)
        {
            try
            {
                CommunicationsParams logParams = new CommunicationsParams()
                {
                    Tenant = tenant,
                    CommunicationLogTypeCode = "Q",
                    Status = ((char)statusTypeCode).ToString(),
                    QueueName = "externaltasksqueue" + tenant + 1,
                    Priority = 1,
                    InOut = "O",
                    Subject = "Customer ready for activation",
                    FolderName = "ExternalTasksQueue",
                    ByteData = LogitudeXmlSerializer.SerializeObject(request),
                    Logs=response                    
                };
                
                var result = Communications.AddCommunicationLog(logParams);
                return result;
            }
            catch (Exception ex)
            {                
                throw new Exception("Failed to save communication log", ex);
            }            
        }
        public void UpdateCommunicationLogStatus( string communicationLogId,
            int tenant,
            StatusTypeCommunication statusTypeCode,
            string response, 
            object exceptionMessage)
        {
            try
            {
                Communications.UpdateCommunicationLogStatus(communicationLogId,
               tenant,
               null,
               ((char)statusTypeCode).ToString(),
               response,
               exceptionMessage);

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update communication log communicationLogId={communicationLogId} ", ex);
            }
             
            
        }
    }
}
