using Logitude.Server.Tools;
using System;
using System.Threading.Tasks;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;

namespace CommunicationWorkerRole.RestRequestExecutor
{
    public class ApiCommunicationLog
    {              
        public async Task<string> AddCommunicationLogAsync(string request, string response, int tenant, StatusTypeCommunication statusTypeCode)
        {
            try
            {
                CommunicationsParams logParams = new CommunicationsParams()
                {
                    Tenant = tenant,
                    CommunicationLogTypeCode = CommunicationConstants.TypeQueue.ToString(),
                    Status = ((char)statusTypeCode).ToString(),
                    QueueName = $"{CommunicationConstants.DefaultFolder}{tenant}1",
                    Priority = 1,
                    InOut = CommunicationConstants.InOut.ToString(),
                    Subject = CommunicationConstants.SubjectCustomerActivation,
                    FolderName = CommunicationConstants.DefaultFolder,
                    ByteData = LogitudeXmlSerializer.SerializeObject(request),
                    Logs=response                    
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
