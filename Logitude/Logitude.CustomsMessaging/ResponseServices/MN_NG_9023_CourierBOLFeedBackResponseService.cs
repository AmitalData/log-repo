
                                                                //Yuval Chalup 23.06.2015 TASK-13278
using System;
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
using UnifreightIIG.Common.ConstraintApprovalRequestServiceReference;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using System.Collections;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.CourierBOLQueryServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{

    public class MN_NG_9023_CourierBOLFeedBackResponseService : ResponseServiceBase<
         CourierBOLQueryResponseData, MN_NG_9023_CourierBOLFeedBack_Message, CourierBOLQueryRequestParams>
    {
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        private Dictionary<string, IList> _MyLocalCache = new Dictionary<string, IList>();
        private AmitalContext _AmitalContext;

        public override void Update(MN_NG_9023_CourierBOLFeedBack_Message customResponse, CourierBOLQueryRequestParams requestParams)
        {
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

            this.MyResponseData = new CourierBOLQueryResponseData();

            if (!string.IsNullOrWhiteSpace(requestParams.CourierBOL) || !string.IsNullOrWhiteSpace(requestParams.CourierVAT))
            {
                if (this.MyRequestSheetParam == null)
                {
                    this.MyRequestSheetParam = new RequestSheetParam();
                }
            }

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
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage); 
                return;
            }

            if (customResponse.ResponseContentHeader != null)
            {
                {
                    if (customResponse.ResponseContentHeader.Exception != null)
                    {
                        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                        {
                            var errMess = "No Courier BOL details in the Response for " + requestParams.CourierBOL + requestParams.CourierVAT;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        else
                        {
                            var errMess = "No Courier BOL details in the Response for " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". For " + requestParams.CourierBOL + requestParams.CourierVAT;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                        return;
                    }
                }
            }

            if (customResponse.CourierCargos == null)
            {
                if (customResponse.ResponseContentHeader.Exception == null)
                {
                    if (customResponse.ResponseContentHeader.Exception != null)
                    {
                        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                        {
                            var errMess = "No Courier BOL details in the Response for " + requestParams.CourierBOL + requestParams.CourierVAT;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        else
                        {
                            var errMess = "No Courier BOL details in the Response" + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". For " + requestParams.CourierBOL + requestParams.CourierVAT;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                    }
                }
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                return;
            }

            if (customResponse.CourierCargos != null)
            {
                if (customResponse.CourierCargos.Count() > 0)
                {
                    MyResponseData.CourierBOLDetailsList = new List<CourierBOLQueryResponseData.CourierBOLDetailsResult>();
                    foreach (var courierCargo in customResponse.CourierCargos)
                    {
                        CourierBOLQueryResponseData.CourierBOLDetailsResult newCourierBOLDetailsResult = new CourierBOLQueryResponseData.CourierBOLDetailsResult()
                        {
                            cargoIdentifierKey1 = courierCargo.cargoIdentifierKey1,
                            cargoIdentifierKey2 = courierCargo.cargoIdentifierKey2,
                            cargoIdentifierKey3 = courierCargo.cargoIdentifierKey3,
                            cargoIdentifierType = courierCargo.cargoIdentifierType,
                        };
                        MyResponseData.CourierBOLDetailsList.Add(newCourierBOLDetailsResult);
                    }
                }
            }
         
            LogMessagingUtil.Instance.AppendLine("Analyze Credit Query response for " + requestParams.CourierBOL + requestParams.CourierVAT);

            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "שליחת שאילתא לשטרי מטען בלדר בוצעה בהצלחה";
        }

        public override CourierBOLQueryResponseData GetResponse(MN_NG_9023_CourierBOLFeedBack_Message customResponse, CourierBOLQueryRequestParams requestParams)
        {
            return this.MyResponseData;
        }

         private string GetErrosXmlFromResponseHeaderExeption()
        {
            return _ResponseHeaderExeption.ErrorDescription;
        }

         private string GetDummyXml(string message)
         {
             var myDummyXml = new GeneralMessage() { Message = message };
             var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
             return xml;
         }
         
        public class GeneralMessage
        {
            public string Message { get; set; }
            //public MN_NG_9023_CourierBOLFeedBack_Message Response { get; set; }
        }
    }
    
}




