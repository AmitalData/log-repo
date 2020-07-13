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
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using UnifreightIIG.Common.MessageLib.Fault;
 using UnifreightIIG.Common.MessageLib.Collateral;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System.Reflection;
using System.Xml.Serialization;
using Logitude.Customs.BL.TraceEvents;
using Simplog.Data.CommonDataModel.Repositories;
using Declaration = UnifreightIIG.Common.MessageLib.ID.Declaration;
using System.Diagnostics;
using Response = UnifreightIIG.Common.MessageLib.ID.Response;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using ResponseError = UnifreightIIG.Common.MessageLib.ID.ResponseError;
using DeclarationGoodsShipment = UnifreightIIG.Common.MessageLib.ID.DeclarationGoodsShipment;
using DeclarationGoodsShipmentCustomsValuation = UnifreightIIG.Common.MessageLib.ID.DeclarationGoodsShipmentCustomsValuation;
using UnifreightIIG.Common.MessageLib.Ransom;
using Logitude.BL.CommonDataModel.EntityQueries;
using UnifreightIIG.Common.MessageLib.DeclarationCancel;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsgResponseService : 
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsg, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        DeclarationPM _MyDeclarationPMOrg;

        private DeclarationPrintResponseData _SendDeclarationPrintResponse;
 

      
        public override void Update(DF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsg customResponse, GenericRequestParams requestParams)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();
            DeclarationCorrectionsPointerService myDeclarationCorrectionsPointerService = new DeclarationCorrectionsPointerService();
            string error = "";




            FeatureQuery featureQuery = new FeatureQuery();

            var features = featureQuery.GetAllowedFeaturesForLoggedUser(requestParams.LoggingUserId, requestParams.Tenant);

            var feature = features.Features.FirstOrDefault(x => x.Code == "DECLARATIONAMENDMENT");
            if (feature != null)
            {
                 

            }



         }
       
 
        public UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseStatus CastStatus(UnifreightIIG.Common.MessageLib.ID.ResponseStatus declaration)
        {


            string DeclarationString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(declaration.GetType());
                serializer.Serialize(stringwriter, declaration);
                DeclarationString = stringwriter.ToString();
            }



            using (var stringReader = new System.IO.StringReader(DeclarationString))
            {
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseStatus));
                return serializer.Deserialize(stringReader) as UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseStatus;
            }
        }

        public UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration CastDeclaration(UnifreightIIG.Common.MessageLib.ID.Declaration declaration )
        {


            string DeclarationString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(declaration.GetType());
                serializer.Serialize(stringwriter, declaration);
                DeclarationString = stringwriter.ToString();
            }



            using (var stringReader = new System.IO.StringReader(DeclarationString))
            {
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration));
                return serializer.Deserialize(stringReader) as UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration;
            }
        }


        public UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseError[] CastError(UnifreightIIG.Common.MessageLib.ID.ResponseError[] responseError)
        {


            string ErrorString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(responseError.GetType());
                serializer.Serialize(stringwriter, responseError);
                ErrorString = stringwriter.ToString();
            }



            using (var stringReader = new System.IO.StringReader(ErrorString))
            {
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseError[]));
                return serializer.Deserialize(stringReader) as UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseError[];
            }
        }

 
        public override INF_MSG_GenericResponseData GetResponse(DF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        
     }
}
