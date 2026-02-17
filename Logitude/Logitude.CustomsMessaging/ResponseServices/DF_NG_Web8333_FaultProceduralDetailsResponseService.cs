
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Storage;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using UnifreightIIG.Common.MessageLib.Docs;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using System.Collections;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.FaultProceduralDetailsServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{

    public class DF_NG_Web8333_FaultProceduralDetailsResponseService : ResponseServiceBase<
         FaultProceduralResponseData, DF_NG_Web8333_FaultProceduralDetailsResponse, FaultProceduralRequestParams>
    {
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        DeclarationPM _MyDeclarationPM;
        ConsignmentPM _MyConsignmentPM;
        private GTRTRANQueryService _GTRTRANQueryService;
        private Dictionary<string, IList> _MyLocalCache = new Dictionary<string, IList>();
        private AmitalContext _AmitalContext;

        public override void Update(DF_NG_Web8333_FaultProceduralDetailsResponse customResponse, FaultProceduralRequestParams requestParams)
        {
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

            this.MyResponseData = new FaultProceduralResponseData();

            //Checking foe Exceptions
            if (customResponse.ResponseContentHeader.Exception != null || _ResponseHeaderExeption != null)
            {
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                }
                else
                {
                    this.MyResponseData.UserMessage = _ResponseHeaderExeption.ErrorDescription;
                }
                this.MyResponseData.HasException = true;
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage); 
                return;
            }

            if (customResponse.ResponseContentHeader != null)
            {
                if (customResponse.ResponseContentHeader.ApplicationID == 0)
                {
                    if (customResponse.ResponseContentHeader.Exception != null)
                    {
                        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                        {
                            var errMess = "No Declaration details in the Response " + requestParams.DeclarationNumber;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        else
                        {
                            var errMess = "No Declaration details in the Response " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". " + requestParams.DeclarationNumber;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                        return;
                    }                   
                }
            }

            if (customResponse.FaultGeneralDetail == null)
            {
                if (customResponse.ResponseContentHeader.Exception == null)
                {
                    if (customResponse.ResponseContentHeader.Exception != null)
                    {
                        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                        {
                            var errMess = "No Declaration details in the Response " + requestParams.DeclarationNumber;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        else
                        {
                            var errMess = "No Declaration details in the Response " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". " + requestParams.DeclarationNumber;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                    }
                }
                this.MyResponseData.Succeeded = true; 
                this.MyResponseData.HasException = true;
                return;
            }

            //customResponse.FaultGeneralDetail = customResponse.FaultGeneralDetail ?? new DF_NG_Web8333_FaultProceduralDetailsResponseFaultGeneralDetail[] { new DF_NG_Web8333_FaultProceduralDetailsResponseFaultGeneralDetail() };
            //string xml = XmlGenericUtil<DF_NG_Web8333_FaultProceduralDetailsResponseFaultGeneralDetail[]>.SerializeObject(customResponse.FaultGeneralDetail);
            //MyResponseData.ResponseStatusXML = xml;

            List<FaultGeneralDetailResult> faultGeneralDetailList = new List<FaultGeneralDetailResult>();
            foreach (var faultProceduralDetailsItem in customResponse.FaultGeneralDetail)
            {
                //FaultGeneralDetail
                FaultGeneralDetailResult faultProceduralDetail = new FaultGeneralDetailResult();
                faultProceduralDetail.ProceduralFaultID = faultProceduralDetailsItem.ProceduralFaultID.ToString();
                faultProceduralDetail.CustomsHouse = faultProceduralDetailsItem.CustomsHouse;
                faultProceduralDetail.CustomsHouseName = faultProceduralDetailsItem.CustomsHouseName;
                faultProceduralDetail.DeclarationId = faultProceduralDetailsItem.declarationId;
                faultProceduralDetail.ProceduralFaultCode = faultProceduralDetailsItem.proceduralFaultCode.ToString();
                faultProceduralDetail.ProceduralFaultCodeName = faultProceduralDetailsItem.proceduralFaultCodeName;
                faultProceduralDetail.CreateDate = faultProceduralDetailsItem.CreateDate.Date.ToString("dd/MM/yyyy");
                faultProceduralDetail.AgentExternalID = faultProceduralDetailsItem.AgentExternalID;               
                faultProceduralDetail.ExternalID = faultProceduralDetailsItem.ImporterOrEporter_ExternalID;
                faultProceduralDetail.ProceduralFaultStatus = faultProceduralDetailsItem.proceduralFaultStatus.ToString();
                faultProceduralDetail.ProceduralFaultStatusName = faultProceduralDetailsItem.proceduralFaultStatusName;
                faultProceduralDetail.CustomsHouse = faultProceduralDetailsItem.CustomsHouse;

                //FaultAdittionalInformation
                List<FaultAdittionalInformationResult> faultAdittionalInformationList = new List<FaultAdittionalInformationResult>();
                FaultAdittionalInformationResult faultAdittionalInformation = new FaultAdittionalInformationResult();
                faultAdittionalInformation.AgentInDeclarationName = faultProceduralDetailsItem.AgentInDeclarationName;
                faultAdittionalInformation.AgentResponsibilityID = faultProceduralDetailsItem.FaultAdittionalInformation.AgentResponsibilityID;
                faultAdittionalInformation.AgentResponsibilityName = faultProceduralDetailsItem.FaultAdittionalInformation.AgentResponsibilityName;
                faultAdittionalInformation.ResponsibilityID = faultProceduralDetailsItem.FaultAdittionalInformation.Importer_ExporterResponsibilityID;
                faultAdittionalInformation.ResponsibilityName = faultProceduralDetailsItem.FaultAdittionalInformation.Importer_ExporterResponsibilityName;
                faultProceduralDetail.ImporterExporterResponsibilityName = faultProceduralDetailsItem.FaultAdittionalInformation.Importer_ExporterResponsibilityName;
                faultAdittionalInformation.ProceduralFaultInputProcess = faultProceduralDetailsItem.FaultAdittionalInformation.proceduralFaultInputProcess.ToString();
                faultAdittionalInformation.ProceduralFaultInputProcessName = faultProceduralDetailsItem.FaultAdittionalInformation.proceduralFaultInputProcessName;
                faultAdittionalInformation.RansomViolationType = faultProceduralDetailsItem.FaultAdittionalInformation.ransomViolationType.ToString();
                faultAdittionalInformation.RansomViolationTypeName = faultProceduralDetailsItem.FaultAdittionalInformation.ransomViolationTypeName;
                faultAdittionalInformation.RansomViolationSum = String.Format("{0:N2}", faultProceduralDetailsItem.FaultAdittionalInformation.ransomViolationSum);
                faultAdittionalInformation.FelonyType = faultProceduralDetailsItem.FaultAdittionalInformation.FelonyType.ToString();
                faultAdittionalInformation.FelonyTypeName = faultProceduralDetailsItem.FaultAdittionalInformation.FelonyTypeName;
                faultAdittionalInformation.Severity = faultProceduralDetailsItem.FaultAdittionalInformation.Severity.ToString();
                faultAdittionalInformation.SeverityName = faultProceduralDetailsItem.FaultAdittionalInformation.SeverityName;
                faultAdittionalInformation.Scoring = faultProceduralDetailsItem.Scoring.ToString("N2");
                faultAdittionalInformation.ExporterImporterInDeclarationName = faultProceduralDetailsItem.Exporter_ImporterInDeclarationName;
                faultAdittionalInformationList.Add(faultAdittionalInformation);

                faultProceduralDetail.FaultAdittionalInformationList = faultAdittionalInformationList;
                faultGeneralDetailList.Add(faultProceduralDetail);
            }

            MyResponseData.ApplicationID = requestParams.DeclarationNumber;
            MyResponseData.FaultGeneralDetailList = faultGeneralDetailList;
            MyResponseData.Succeeded = true;
            MyResponseData.HasException = false;
            MyResponseData.UserMessage = "שליחת מסר ליקויים בוצעה בהצלחה";
        }

        private string GetDummyXml(string message)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }

        public override FaultProceduralResponseData GetResponse(DF_NG_Web8333_FaultProceduralDetailsResponse customResponse, FaultProceduralRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private string GetErrosXmlFromResponseHeaderExeption()
        {
            return _ResponseHeaderExeption.ErrorDescription;
        }

        public class GeneralMessage
        {
            public string Message { get; set; }
        }
    }
}




