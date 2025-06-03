using Logitude.Server.Tools;
using System;
using System.Threading.Tasks;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;

namespace Logitude.Server.Tools.RestRequestExecutor
{
    public class ApiCommunicationLog
    {              
        public async Task<string> AddCommunicationLogAsync(string request,
            string response,
            int tenant,
            StatusTypeCommunication statusTypeCode,
            ApiCommunicationConstants communicationsRequest)
        {
            try
            {

                CommunicationsParams logParams = new CommunicationsParams()
                {
                    Tenant = tenant,
                    CommunicationLogTypeCode = ApiCommunicationConstants.TypeQueue.ToString(),
                    Status = ((char)statusTypeCode).ToString(),
                    QueueName = $"{ApiCommunicationConstants.DefaultFolder}{tenant}1",
                    Priority = 1,
                    InOut = ApiCommunicationConstants.InOut.ToString(),
                    LoggingEntityId = communicationsRequest.EntityId,
                    LoggingObjectTableId = communicationsRequest.ObjectTableId,                    
                    Subject = communicationsRequest.Subject,
                    FolderName = ApiCommunicationConstants.DefaultFolder,
                    ByteData = LogitudeXmlSerializer.SerializeObject(request),
                    Logs = response
                };                
                
                return await Task.Run(() => Communications.AddCommunicationLog(logParams));
                
            }
            catch (Exception ex)
            {
                throw new CommunicationLogException($"Failed to save communication log Tenant {tenant}", ex);
            }            
        }        
    }
}
