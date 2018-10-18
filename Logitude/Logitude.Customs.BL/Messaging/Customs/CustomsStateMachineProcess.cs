using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.QueueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs
{
    
   
    public enum CustomsCommandEnum:int
    {
        CustomsCommandGetCustomRequestWR =10,
        CustomsCommandSignRequestWR = 20,

        //DCA Stages
        CustomsCommandSendDCAWR = 30,
        CustomsCommandSendDCAUploadStatusWR = 31,



        CustomsCommandDownloadDcaReceiveCorrelationWR = 40,
        
        
        //WS Stage
        CustomsCommandSendWSReceiveCorrelationWR = 50,

        CustomsCommandAnalyzeResponseWR = 60,

    }
    public class CustomsRCmmand : RCmmand<CustomsStepEnum, CustomsCommandEnum>
    {

        public CustomsRCmmand(CustomsCommandEnum command, Func<object,bool> execute)
            : base(command, execute)
        {

        }
#if false
        public CustomsRCmmand(CustomsCommandEnum command, Action<object> execute)
            :base(command, execute)
        {

        }
#endif

    }

    public class CustomsStateMachineProcess : StateMachineProcess<CustomsStepEnum, CustomsCommandEnum, CustomsRCmmand>
    {
        public CustomsStateMachineProcess(List<CustomsRCmmand> myRCmmands, Dictionary<StateTransition, CustomsStepEnum> transitions)
            :base(myRCmmands, transitions)
        {

        }
        
    }
}
