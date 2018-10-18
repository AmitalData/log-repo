using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.AccountingMessaging.Common
{
    public enum AccountingStepEnum : int
    {
        StartRequestParams = 0,
        AccountingRequest = 1,
        AccountingRequestSign = 2,//Have Sign Ver (If Needed)
        AccountingRequestSignPersonal = 2,//Have Sign Ver (If Needed)

        DCAInProgressUploading = 11,// Sent via DCA Get ServerJobId
        DCAInProgressUploaded = 12,

        ReceivedAccountingResponseCorrelation = 20,// SendWS or ReviveFromDownloadDcaMessagesWR
        AnalyzeResponseData = 30// DCASent Or  AnalyzeResponse
    }
    
    
}
