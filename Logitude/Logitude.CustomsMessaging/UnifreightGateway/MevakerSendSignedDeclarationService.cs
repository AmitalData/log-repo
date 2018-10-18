

using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class MevakerSendSignedDeclarationService : UnifreightGenericService
    {

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.Sivug.MevakerSendSingDeclarationService.Upsert()";
        private Stopwatch _Stopwatch;
        private GenericRequestParams _GenericRequestParams;
        private string _PersonId;

        public MevakerSendSignedDeclarationService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string xmlList,
              ref string MoreParams,
              out string MessageOut
            )
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "MevakerSendSingDeclarationService ";

            DeserilazeObject(xmlList);

            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            CheckParamValid();
            CheckLock();

            if (!CheckSignServerOn())
            {
                MyGenericResponseObj.ExceptionLevel = "!CheckSignServerOn()";
                MyGenericResponseObj.ErrorDescription = "Sign server not available";
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                return;
            }

            AppendLogLine("CheckParamValid:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "GetContext";

            INF_MSG_GenericResponseData responseData;
            var myDF_MSG10000_ImportDeclarationMessagingService = new DF_MSG10000_ImportDeclarationMessagingService();
            responseData = myDF_MSG10000_ImportDeclarationMessagingService.Send(this._GenericRequestParams);          
            MyGenericResponseObj.CorrelationId = responseData.CustomsRequestsSheetId;
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
        }

        private bool CheckSignServerOn()
        {
            var LoggingUserId = AuthenticationUtil.ResolveUserId(this._GenericRequestParams.Tenant);
            
            var availableSignServer = SignQueue.Instance.
                GetAvailableSignServer(this._GenericRequestParams.Tenant, Server.Tools.ExternalServices.SignQueueByType.SignQueueByPersonId , this._PersonId);
            return !String.IsNullOrWhiteSpace(availableSignServer);

        }

        private void CheckLock()
        {
            //throw new NotImplementedException();
        }

        private void CheckParamValid()
        {
            //throw new NotImplementedException();
        }

        private void DeserilazeObject(string xmlList)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("MevakerSendSingDeclarationService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlList))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlList.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlList.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlList);
            }

            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            var dic = UnifreightListsUtil.Deserialize(xmlList);

            int tenant = this.ResolvedTenant();
            if (tenant == null)
            {
                throw new Exception("Tenant is missing !!!");
            }

            this._GenericRequestParams = new GenericRequestParams()
            {
                Tenant = tenant,
                LoggingEntityId = UnifreightListsUtil.GetValue(ref dic, "DeclarationId"),
                AppicationId = UnifreightListsUtil.GetValue(ref dic, "DeclarationId"),
                LoggingUserId = AuthenticationUtil.ResolveUserId(tenant),
                RequestVIA = SendRequestVIA.WebServiceBatch,
                LoggingEntityReference = UnifreightListsUtil.GetValue(ref dic, "DeclarationNumber"),        //TO COMPLETE
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                LoggingEnabled = true
            };

            this._PersonId = UnifreightListsUtil.GetValue(ref dic, "PersonId");
            if (string.IsNullOrWhiteSpace(this._PersonId))
            {
                this._PersonId = SignQueue.Instance.GetUserPersonID(_GenericRequestParams.LoggingUserId, _GenericRequestParams.Tenant);
            }
        }

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {

            return
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<ArrayOfEntry xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
 <Entry>
  <Key>DeclarationId</Key>
  <Value>1-2482</Value>
 </Entry>
 <Entry>
  <Key>LoggingUserId</Key>
  <Value>MORAN</Value>
 </Entry>
 <Entry>
  <Key>CFIFILEMFileNo</Key>
  <Value>616200307</Value>
 </Entry>
 <Entry>
  <Key>CCUFILEMFileNo</Key>
  <Value>50012482</Value>
 </Entry>
</ArrayOfEntry>
";
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }
    }
}

