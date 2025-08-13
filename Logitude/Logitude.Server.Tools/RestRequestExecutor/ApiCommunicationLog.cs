using Logitude.Server.Tools;
using System;
using System.Threading.Tasks;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;

namespace Logitude.Server.Tools.RestRequestExecutor
{
    public class ApiCommunicationLog
    {
        public async Task<string> AddCommunicationLogAsync(
    string requestPayload,          // what you SENT  (can be empty on reply)
    string responsePayload,         // what you RECEIVED (can be empty on send)
    int tenant,
    StatusTypeCommunication statusTypeCode,
    ApiCommunicationConstants comm)
        {
            try
            {
                string rawBlob = comm.InOut.Equals("O", StringComparison.OrdinalIgnoreCase)
                                 ? requestPayload       
                                 : responsePayload;     

                CommunicationsParams logParams = new CommunicationsParams
                {
                    Tenant = tenant,
                    CommunicationLogTypeCode = ApiCommunicationConstants.TypeQueue.ToString(),
                    Status = ((char)statusTypeCode).ToString(),
                    QueueName = $"{ApiCommunicationConstants.DefaultFolder}{tenant}1",
                    Priority = 1,
                    InOut = comm.InOut,
                    LoggingEntityId = comm.EntityId,
                    LoggingObjectTableId = comm.ObjectTableId,
                    Subject = comm.Subject,
                    FolderName = ApiCommunicationConstants.DefaultFolder,

                    ByteData = LogitudeXmlSerializer.SerializeObject(rawBlob),
                    Logs = responsePayload
                };

                return await Task.Run(() => Communications.AddCommunicationLog(logParams));
            }
            catch (Exception ex)
            {
                throw new CommunicationLogException(
                    $"Failed to save communication log Tenant {tenant}", ex);
            }
        }

    }
}
