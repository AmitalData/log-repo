using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{

    public partial class BIReportsExecutionLogQueryService : EntityQueryService<BIReportsExecutionLog, BIReportsExecutionLogKeys, BIReportsExecutionLogPM, object, BIReportsExecutionLogKeys>
    {
  
        public BIReportsExecutionLogList GetBIReportsExecutionLogList(string id, int tenant)
        {
            BIReportsExecutionLogList bIReportsExecutionLogList = (from a in context.BIReportsExecutionLogs
                                                                   where a.Tenant == tenant
                                                                   && a.Id == id
                                                                   select new BIReportsExecutionLogList()
                                                                   {
                                                                       Tenant = a.Tenant,
                                                                       Id = a.Id,
                                                                       BIReportId = a.BIReportId,
                                                                       StatusCode = a.StatusCode,
                                                                       ExceptionMessage = a.ExceptionMessage,
                                                                       CreateDate = a.CreateDate,
                                                                       CreatedByUserId = a.CreatedByUserId,
                                                                       DoneDate = a.DoneDate,
                                                                       
                                                                   }).FirstOrDefault();

            if (bIReportsExecutionLogList != null) bIReportsExecutionLogList.ExceptionMessage = GetUnderStandableMessageFromMessageException(bIReportsExecutionLogList.ExceptionMessage);

            return bIReportsExecutionLogList;
        }

        private string GetUnderStandableMessageFromMessageException(string exceptionMessage)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                string[] lines = exceptionMessage.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                result = lines[0];
            }
            return result;
        }
    }
}
