using Logitude.Customs.Def.ClosedTable;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.ExternalServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Logitude.Customs.BL.Messaging.Customs
{

    public class DefaultMessageController : IMessageController
    {

        public static int MaxToRetry { get { return (1 + 3 + 4); } }
        public bool ToRetry(CustomsStepEnum CustomsProcessState, int retries)
        {
            switch (CustomsProcessState)
            {
                case CustomsStepEnum.StartRequestParams:
                    if (retries > 1)
                    {
                        return false;
                    }
                    break;
                case CustomsStepEnum.CustomRequest:
                    if (retries > 3)
                    {
                        return false;
                    }
                    break;
                case CustomsStepEnum.CustomRequestSign:
                    if (retries > 4)
                    {
                        return false;
                    }
                    break;

                case CustomsStepEnum.DCAInProgressUploading://maybe UNifreight DCA Server DOWN
                    if (retries > 2)
                    {
                        return false;
                    }
                    break;
                case CustomsStepEnum.DCAInProgressUploaded: //Cyberark DCA Server  upload  every 1 min // or is down 
                    if (retries > 2)
                    {
                        return false;
                    }
                    break;
                case CustomsStepEnum.ReceivedCustomResponseCorrelation:
                    if (retries > 4)
                    {
                        return false;
                    }
                    break;
                case CustomsStepEnum.AnalyzeResponseData:
                    if (retries > 3)
                    {
                        return false;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }


       
        public void BuildRealSteps(InterfaceTenantDefinitionManagementPM InterfaceTenantDefinitionWithManagment,ref SendRequestVIA requestVIA ,bool ForcePersonalSign)
        {
             RealSteps = new List<CustomsStepEnum>(Enum.GetValues(typeof(CustomsStepEnum)).OfType<CustomsStepEnum>());

            if (ForcePersonalSign)
            {
                this.SignStepName=SignQueue.CustomRequestSignPersonal ;
                RealSteps.Remove(CustomsStepEnum.CustomRequestSign);
            }
            else
            {
                switch (InterfaceTenantDefinitionWithManagment.InterfaceManagement.SignatureBy)
                {

                    case SignQueueByType.SignQueueByCustomsAgentId:
                        this.SignStepName = SignQueue.CustomRequestSign;
                        RealSteps.Remove(CustomsStepEnum.CustomRequestSignPersonal);
                        break;
                    case SignQueueByType.SignQueueByPersonId:
                        this.SignStepName = SignQueue.CustomRequestSignPersonal;
                        
                        RealSteps.Remove(CustomsStepEnum.CustomRequestSign);
                        break;
                    default:

                        RealSteps.Remove(CustomsStepEnum.CustomRequestSign);
                        RealSteps.Remove(CustomsStepEnum.CustomRequestSignPersonal);
                        break;
                }
            }
            

                    
            

            requestVIA =CalcRequestVIA = Via(InterfaceTenantDefinitionWithManagment, requestVIA);
            if (CalcRequestVIA != SendRequestVIA.DCABatch)
            {
                RealSteps.Remove(CustomsStepEnum.DCAInProgressUploading);
                RealSteps.Remove(CustomsStepEnum.DCAInProgressUploaded);
                //SentDCAInProgress,// Sent via DCA Get ServerJobId
                //SentDCAFileUpload,
            }
            if (CalcRequestVIA == SendRequestVIA.DCABatch && InterfaceTenantDefinitionWithManagment.InterfaceManagement.INOUT == InOutType.In)
            {
                RealSteps.Remove(CustomsStepEnum.CustomRequest);
                RealSteps.Remove(CustomsStepEnum.CustomRequestSign);
                RealSteps.Remove(CustomsStepEnum.CustomRequestSignPersonal);
                RealSteps.Remove(CustomsStepEnum.DCAInProgressUploading);
                RealSteps.Remove(CustomsStepEnum.DCAInProgressUploaded);
            }
            //return RealSteps;
        }

        private static SendRequestVIA Via(InterfaceTenantDefinitionManagementPM InterfaceTenantDefinitionWithManagment, SendRequestVIA requestVIA)
        {
            SendRequestVIA currRequestVIA = requestVIA;
            if (currRequestVIA == SendRequestVIA.Default)
            {
                if (InterfaceTenantDefinitionWithManagment.Interactive == InteractiveMode.DCABatchOutIn)
                {
                    currRequestVIA = SendRequestVIA.DCABatch;
                }
                if (InterfaceTenantDefinitionWithManagment.Interactive == InteractiveMode.WebServiceBatch)
                {
                    currRequestVIA = SendRequestVIA.WebServiceBatch;
                }
                if (InterfaceTenantDefinitionWithManagment.Interactive == InteractiveMode.WebServiceInteractive)
                {
                    currRequestVIA = SendRequestVIA.WebServiceInteractive;
                }
            }
            return currRequestVIA;
        }
        public static SendRequestVIA Via(int tenant ,string interfaceCode, SendRequestVIA requestVIA)
        {
            var interfaceTenantDefinitionQueryService = new InterfaceTenantDefinitionQueryService(tenant);
            

            var _InterfaceTenantDefinitionManagement = interfaceTenantDefinitionQueryService.GetWithInterfaceManagementDefinition(tenant,interfaceCode
                ).FirstOrDefault();
            return Via(_InterfaceTenantDefinitionManagement, requestVIA);
        }

        public CustomsCommandEnum ConvertToWR(CustomsStepEnum customsStepEnum)
        {
            switch (customsStepEnum)
            {
                
                case CustomsStepEnum.CustomRequest:
                    return CustomsCommandEnum.CustomsCommandGetCustomRequestWR;
                    break;
                case CustomsStepEnum.CustomRequestSign:
                    return CustomsCommandEnum.CustomsCommandSignRequestWR;
                    break;
                case CustomsStepEnum.DCAInProgressUploading:
                    return CustomsCommandEnum.CustomsCommandSendDCAWR;
                    break;
                case CustomsStepEnum.DCAInProgressUploaded:
                    return CustomsCommandEnum.CustomsCommandSendDCAUploadStatusWR;
                    break;
                case CustomsStepEnum.ReceivedCustomResponseCorrelation:
                    return CustomsCommandEnum.CustomsCommandSendWSReceiveCorrelationWR;
                    break;
                case CustomsStepEnum.AnalyzeResponseData:
                    return CustomsCommandEnum.CustomsCommandAnalyzeResponseWR;
                    break;
                
                case CustomsStepEnum.StartRequestParams:
                default:
                    throw new Exception("CustomsCommandEnum ConvertToWR(CustomsStepEnum customsStepEnum)-Not Valid WR"); 
                    break; break;
            }
        }

        public SendRequestVIA CalcRequestVIA { get; private set; }


        public string SignStepName { get; private set; }
        public List<CustomsStepEnum> RealSteps { get; set; }
    }


    public interface IMessageController
    {
        bool ToRetry(CustomsStepEnum CustomsProcessState, int retries);
        void BuildRealSteps(InterfaceTenantDefinitionManagementPM InterfaceTenantDefinitionWithManagment, ref SendRequestVIA requestVIA, bool ForceSign);

        CustomsCommandEnum ConvertToWR(CustomsStepEnum nxtEnum);
        //SendRequestVIA CalcRequestVIA { get; private set; }
        List<CustomsStepEnum> RealSteps { get; }
        string SignStepName { get;  }
    }
}
