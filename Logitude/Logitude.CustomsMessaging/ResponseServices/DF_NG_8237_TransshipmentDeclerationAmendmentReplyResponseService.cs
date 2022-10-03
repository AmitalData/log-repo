using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Fault;
 using UnifreightIIG.Common.MessageLib.Collateral;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System.Xml.Serialization;
using Logitude.Customs.BL.TraceEvents;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
 using UnifreightIIG.Common.MessageLib.Ransom;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.Messaging.Customs;
using UnifreightIIG.Common.TransshipmenDeclarationAmendmentRequestMsgRequestServiceReference;
using Logitude.Customs.BL.Utils;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_8237_TransshipmentDeclerationAmendmentReplyResponseService : ResponseServiceBase<ExportDeclarationAmendmentResponseData, DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg, AmendmentRequestParams>
    {
        public override void Update(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse, AmendmentRequestParams requestParams)
        {
            UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg castCustomResponse =
               Serializer.CastXML<UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg, DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg>(customResponse);

            var service = new DF_NG_8237_ExportDeclerationAmendmentReplyResponseService();
            service.Update(castCustomResponse, requestParams);

            MyResponseData = service.MyResponseData;
        }

        public override ExportDeclarationAmendmentResponseData GetResponse(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse, AmendmentRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
