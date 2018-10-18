using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CustomRequestStep
    {
        public CustomsStepEnum MyCustomsStepEnum { get; set; }
        public const int StartRequestParams = 0;
        public const int CustomRequest = 1;
        public const int CustomRequestSign = 2;//Have Sign Ver (If Needed)
        

        public const int DCAInProgressUploading = 11;// Sent via DCA Get ServerJobId
        public const int DCAInProgressUploaded = 12;

        public const int ReceivedCustomResponseCorrelation = 20;// SendWS or ReviveFromDownloadDcaMessagesWR
        public const int AnalyzeResponseData = 30;// DCASent Or  AnalyzeResponse
    }
}
