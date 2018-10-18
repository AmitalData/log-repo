using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public enum CustomsStepEnum : int
    {
        StartRequestParams = 0,
        CustomRequest = 1,
        CustomRequestSign = 2,//Have Sign Ver (If Needed)
        CustomRequestSignPersonal = 2,//Have Sign Ver (If Needed)

        DCAInProgressUploading = 11,// Sent via DCA Get ServerJobId
        DCAInProgressUploaded = 12,

        ReceivedCustomResponseCorrelation = 20,// SendWS or ReviveFromDownloadDcaMessagesWR
        AnalyzeResponseData = 30// DCASent Or  AnalyzeResponse
    }
    
    
}
