using Logitude.AmitalMessaging.Customs.CustomFile.Sivug;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.CommonIIGInterface;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class Unifreight_L2US01_US2L01_SivugMessagingService
         : MessagingServiceBase<
        Unifreight_L2US01RequestParam,  //DecId + uni.fileNumber 
        Unifreight_L2US01ResponseData,
        Unifreight_L2US01RequestParam,
        LOGISIVUGWithResponseContentHeader,
        Unifreight_L2US01RequestParamRequestService,
        Unifreight_LOGISIVUGResponseService, DCAInRequestHeader>
    {

        //protected override Unifreight_L2US01ResponseData GetIIGBLExceptionFromReponseHeader(LOGISIVUGWithResponseContentHeader customsResponse)
        //{
        //    return base.GetIIGBLExceptionFromReponseHeader(customsResponse);
        //}


        
        protected override bool ToValidateCustomsRequestB4Send()
        {
            return false;
        }
        protected override LOGISIVUGWithResponseContentHeader CallWS(Unifreight_L2US01RequestParam customRequest, Unifreight_L2US01RequestParam requestParams, out string exceptionMessage)
        {
            exceptionMessage = "";
            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
                Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess, 
                //"CWSFLOGIFILE"
                //"CWSFLOGISIVUG"
                "CFIUSIVUG"
                , 
                //"DeclarationUpsertPut"
                //"DeclarationGetUS2L01"
                "SivugUpsertBatchGet"
                )
            {
                Tenant = customRequest.Tenant,
                objectTableName = "Customs.Declaration",
                CommunicationLoggingEntityReference = customRequest.CFIFILEMFileNo,
                EntityId = customRequest.CFIFILEMFileNo,
                UserId = customRequest.LoggingUserId,
                CommunicationSubject = "Unifreight_L2US01_US2L01_SivugMessagingService",
                
            };

            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, Unifreight_L2US01RequestParam>(
                amitalCustomFileCommunicationModel, requestParams);
            bool syn = true;
            if (DateTime.Now < new DateTime(2014, 04, 01))
            {
                syn = false;// due uroter 54 use 9501 instead 96 
            }
            //await Task.Delay(10);
            if (DateTime.Now < new DateTime(2017, 01, 15))
            {
                System.Threading.Thread.Sleep(5000); 
            }
            
            var info = myUServerCommunicationService.Send(syn);

            LogMessagingUtil.Instance.AppendLine("UServerCommunicationCustomFileService CommunicationMessage =" + info.ImmediatelyMessage ?? "NULL");
            LogMessagingUtil.Instance.AppendLine("UServerCommunicationCustomFileService CommunicationLogId =" + info.CommunicationLogId ?? "NULL");
            if (info.GenericResponseObj.StatusType != GenericResponseObj.StatusEnum.Success)
            {
            }
            if (String.IsNullOrWhiteSpace(info.GenericResponseObj.ResponseXml) )
            {
                throw new Exception("Urouter did not get respons");
            }
            
            var responseLOGISIVUG = XmlGenericUtil<LOGISIVUG>.DeSerializeObject(info.GenericResponseObj.ResponseXml);
            
            _ResponseHeader = new UnifreightIIG.Common.SystemTableServiceReference.ResponseHeader()
            {
                CorrelationId = info.CommunicationLogId ,
                Status = "Success"
            };//IResponseHeaderOrFault

            var myLOGISIVUGWithResponseContentHeader = new LOGISIVUGWithResponseContentHeader()
            {
                MyLOGISIVUG = responseLOGISIVUG
            };
            return myLOGISIVUGWithResponseContentHeader;

            ;
        }

        public override string MainInterfaceCode
        {
            get { return "US2L01"; }
        }


        
        protected override Unifreight_L2US01RequestParam CreateDefaultRequestParamsFromCustomsResponse(LOGISIVUGWithResponseContentHeader customsResponse)
        { // moran 4.7.16 - Bug 21904
            var tableName = "Customs.Declaration";
            var decId = "";
            if (customsResponse != null && customsResponse.MyLOGISIVUG != null && customsResponse.MyLOGISIVUG.SIVUG != null)
            {
                decId = customsResponse.MyLOGISIVUG.SIVUG[0].LOGITUDEFILE;
            }

            var myGenericRequestParams = new Unifreight_L2US01RequestParam()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                LoggingEntityId = decId,
                DeclarationId = decId,
                RequestName = "מסר סיווג פריטים ",
                ResponseName = "מסר סיווג פריטים "
                
            };
            return myGenericRequestParams;
        }


        protected override void BuildRequestContentHeaderB4Sign(Unifreight_L2US01RequestParam customRequest)
        {
            //base.BuildRequestContentHeaderB4Sign(customRequest);
        }
    }




    [XmlRoot(Namespace = "http://amital.com/customs/Prod/LOGISIVUGWithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/LOGISIVUGWithResponseContentHeader")]
    public class LOGISIVUGWithResponseContentHeader : IINF_MSG_Generic
    {

        public IResponseContentHeader GetResponseContentHeader()
        {

            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }
        public LOGISIVUG MyLOGISIVUG { get; set; }
        public string MyMoreParams { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }


    //[XmlRoot(Namespace = "http://amital.com/customs/Prod/LOGISIVUGWithResponseContentHeader", IsNullable = false)]
    //[XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/LOGISIVUGWithResponseContentHeader")]
    //public class Unifreight_L2US01RequestParam : RequestParamsBase
    //{

    //    public string DeclarationId { get; set; }
    //    public string CFIFILEMFileNo { get; set; }
    //    public string CCUFILEmFileNo { get; set; }
    //}
}