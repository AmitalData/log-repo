using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.ChangingTimeServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;
using Simplog.Data.InfrastructureModel.Repositories;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class CH_NG_191_MSG2_ChangingTimeRequestMessagingService :
        MessagingServiceBase<
        CH_NG_191_MSG2_ChangingTimeRequestParams,
        CH_NG_192_MSG3_ApproveChangeTimeResponseData,
        //CH_NG_191_MSG2_ChangingTimeRequestChangingTimeRequest,
        CH_NG_191_MSG2_ChangingTimeRequest,
        CH_NG_192_MSG3_ApproveChangeTimeRequest,
        CH_NG_191_MSG2_ChangingTimeRequestService,
        CH_NG_192_MSG3_ApproveChangeTimeResponseService, RequestHeader>
    {


        public override string MainInterfaceCode { get { return "191"; } }

        protected override CH_NG_192_MSG3_ApproveChangeTimeRequest CallWS(
            CH_NG_191_MSG2_ChangingTimeRequest customRequest,
            CH_NG_191_MSG2_ChangingTimeRequestParams checkParams,
            out string exceptionMessage)
        {
#if USE_ErrorHandlerUtil
            try
            {
#endif
            CH_NG_192_MSG3_ApproveChangeTimeRequest curResponse192;
                        
            //var ExternalId = Guid.NewGuid().ToString();
            int requestType;
            int.TryParse(checkParams.RequestType, out requestType);

            // var changingTimeManager = new ChangingTimeManager();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            //CH_NG_191_MSG2_ChangingTimeRequest myChangingTimeRequest = new CH_NG_191_MSG2_ChangingTimeRequest() { RequestContentHeader = new RequestContentHeader() };
            //BuildRequestContentHeaderB4Sign(customRequest);
            //myChangingTimeRequest.ChangingTimeRequest = customRequest;
            //myMP.MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.TestMode;

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IPhysicalCheck>().PhysicalCheckChangingTime(this.RequestsSheetExternalId,
                base.CustomsSetting.CustomsAgentId,
                //myChangingTimeRequest,
                customRequest,
                ref this._IIGGatewayMoreParams,
                out curResponse192);
            }
            exceptionMessage = null;
            return curResponse192;



#if USE_ErrorHandlerUtil
            
                var ExeptionDescription = "";
                if (curResponse192 != null)
                {
                    if (curResponse192.ResponseContentHeader.Exception != null)
                    {
                        foreach (var rec in curResponse192.ResponseContentHeader.Exception)
                        {

                            if (!String.IsNullOrWhiteSpace(ExeptionDescription))
                            {
                                ExeptionDescription += Environment.NewLine;
                            }
                            ExeptionDescription += rec.ExeptionDescription;

                        }


                        exceptionMessage = ExeptionDescription;

                    }
                }

      
            }
            catch (System.Exception ex)
            {
                exceptionMessage = ex.Message;
                return null;
            }
#endif

        }


        protected override CH_NG_192_MSG3_ApproveChangeTimeRequest CallWSTest(CH_NG_191_MSG2_ChangingTimeRequest customRequest, CH_NG_191_MSG2_ChangingTimeRequestParams requestParams)
        {
            CH_NG_192_MSG3_ApproveChangeTimeRequest curResponse192 = new CH_NG_192_MSG3_ApproveChangeTimeRequest();
            curResponse192.ApproveChangeTimeRequest = new CH_NG_192_MSG3_ApproveChangeTimeRequestApproveChangeTimeRequest();
            curResponse192.ResponseContentHeader = new ResponseContentHeader();

            switch (requestParams.TestCase.Code)
            {

                case "Choose Specific Time":
                    {
                        curResponse192.ApproveChangeTimeRequest.newDate = requestParams.QueueDate;
                        curResponse192.ApproveChangeTimeRequest.newDateSpecified = true;
                        curResponse192.ApproveChangeTimeRequest.requestType = 2;
                        break;
                    }
                case "Choose Automatic Time":
                    {
                        Random dayRand = new Random();
                        Random monthRand = new Random();
                        Random yearRand = new Random();
                        Random hoursRand = new Random();
                        Random minRand = new Random();
                        int day = dayRand.Next(1, 30);
                        int month = monthRand.Next(1, 12);
                        int year = yearRand.Next(2013, 2014);
                        int hour = hoursRand.Next(23);
                        int minutes = minRand.Next(59);
                        DateTime date = new DateTime(year, month, day, hour, minutes, 0);
                        curResponse192.ApproveChangeTimeRequest.newDate = date;
                        curResponse192.ApproveChangeTimeRequest.newDateSpecified = true;
                        curResponse192.ApproveChangeTimeRequest.requestType = 3;
                        break;
                    }
                case "Choose Specific Time Fail":
                    {
                        List<UnifreightIIG.Common.ChangingTimeServiceReference.Exception> exceptions=new List<UnifreightIIG.Common.ChangingTimeServiceReference.Exception>();
                        exceptions.Add(new UnifreightIIG.Common.ChangingTimeServiceReference.Exception(){ EnglishDescription="This is a test exception to show the failure of choosing a specific time!", ExeptionDescription="This is a test exception to show the failure of choosing a specific time!"});
                        curResponse192.ResponseContentHeader.Exception = exceptions.ToArray();
                        
                        break;
                    }
                case "Choose Automatic Time Fail":
                    {
                        List<UnifreightIIG.Common.ChangingTimeServiceReference.Exception> exceptions = new List<UnifreightIIG.Common.ChangingTimeServiceReference.Exception>();
                        exceptions.Add(new UnifreightIIG.Common.ChangingTimeServiceReference.Exception() { EnglishDescription = "This is a test exception to show the failure of choosing an automatic time!", ExeptionDescription = "This is a test exception to show the failure of choosing an automatic time!" });
                        curResponse192.ResponseContentHeader.Exception = exceptions.ToArray();
                        break;
                    }
            }
            
            return curResponse192;

        }

        protected override CH_NG_191_MSG2_ChangingTimeRequestParams CreateDefaultRequestParamsFromCustomsResponse(CH_NG_192_MSG3_ApproveChangeTimeRequest customsResponse)
        {
            var tableName = "Customs.PhysicalCheck";

            var myGenericRequestParams = new CH_NG_191_MSG2_ChangingTimeRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }
    }
}
