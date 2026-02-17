using Logitude.Customs.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs
{
    public interface IMessagingServiceInterfaceType
    {

#if false
        void SendBatchStateMachine(int tenant, string correlationId, ref CustomsStepEnum processState); ///,CancellationToken ct) 
#endif
        Nullable<CustomsCommandEnum> CurrentCustomsCommandWR { get; set; }
        OverrideControllerModel MyOverrideControllerModel { get; set; }

        string ConvertStepDataToJSON(int StepNumber,string DocumentDataXml );

        string TestSendXml(string customXmlRequest, out string exceptionMessage);


        object SendSheet(int tenant, string customsRequestsSheetId, SendSheetSignModel SignRecievedModel = null);
        object ReQueue(int tenant, string customsRequestsSheetId);

        //string DcaReceivedCustomResponseCorrelationCrashIfNotValid(Logitude.Customs.BL.EntityPMs.InterfaceManagementPM messageDCA, int tenant, string selectedFile, byte[] messageBytes, bool pseudo = false);
        string DcaReceivedCustomResponseCorrelation(Logitude.Customs.Def.EntityPMs.InterfaceManagementPM messageDCA, int tenant,
            DCAFileModel selectedDCAFile, string fileContents //byte[] messageBytesbyte[] messageBytes
            , bool pseudo = false);


        string DCAServerUploadStatus(
            int tenant,
            DCAFileModel selectedDCAFile,
            Logitude.CustomsMessaging.Common.DCAParams.DCAServerUploadStatus MyDCAServerUploadStatus);
            
        byte[] PasiveSignGetBytesToSign(int tenant, string CustomsRequestsSheetId);
        //bool CompleteResponseSignBytes(int tenant, string CustomsRequestsSheetId, Byte[] mySignBytes);
    }    
}
