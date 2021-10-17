using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CommunicationLogExtendedService
    {
        public static string AddCommunicationLog(CommunicationLogExtendedArgs communicationLogExtendedArgs, int tenant)
        {
            ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
            ObjectTablePM table = tablesQuery.GetObjectTableByName(communicationLogExtendedArgs.ObjectTableName, 0);

            CommunicationsParams logParams = new CommunicationsParams()
            {
                Tenant = tenant,
                From = communicationLogExtendedArgs.From,
                To = communicationLogExtendedArgs.To,
                CommunicationLogTypeCode = communicationLogExtendedArgs.CommunicationLogTypeCode,
                Priority = communicationLogExtendedArgs.Priority,
                InOut = communicationLogExtendedArgs.InOut,
                Status = communicationLogExtendedArgs.CommunicationStatusTypeCode,
                LoggingObjectTableId = table.Id,
                LoggingEntityId = communicationLogExtendedArgs.EntityId,
                Subject = communicationLogExtendedArgs.Subject,
                FolderName = communicationLogExtendedArgs.FolderName,
                ByteData = Encoding.UTF8.GetBytes(communicationLogExtendedArgs.MessageBody),
                FileExtension = communicationLogExtendedArgs.FileExtension,
                Logs = communicationLogExtendedArgs.Logs,
                ExceptionMessage = communicationLogExtendedArgs.ExceptionMessage
            };

            return Communications.AddCommunicationLog(logParams);
        }
    }
}
