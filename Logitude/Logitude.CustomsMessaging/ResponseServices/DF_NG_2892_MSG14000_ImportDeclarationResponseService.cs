using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnifreightIIG.Common.ImportDeclarationAmendmentServiceReference;
using UnifreightIIG.Common.MessageLib.Collateral;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
using Logitude.Customs.BL.BL;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2892_MSG14000_ImportDeclarationResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        private bool _FastDelete;
        //private List<SupplierInvoiceItemsTaxesModPM> _SupplierInvoiceItemsTaxesModificationPMList;
        //public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        public bool _IsRetrieveDeclarationResponse { get; set; }
        decimal? totGeneralTaxCalc = 0;
        decimal? totPurchaseCalc = 0;
        decimal? totVatCalc = 0;
        decimal? generalTax = 0;
        decimal? purchase = 0;
        decimal? vat = 0;

        DeclarationError _MyDeclarationError;
        decimal? _TotalBtlCoverageNISSum = 0;

        public override void OnRequestFail(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customResponse, GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }
            base.OnRequestFail(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(
            DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customResponse, GenericRequestParams requestParams)
        {

            /// itzik test     TestTrans(requestParams);
            return this.MyResponseData;
        }

 
        public override void Update(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customResponse, GenericRequestParams requestParams)
        {

            DF_NG_5117_ImportDeclerationAmendmentReplyResponseService dF_NG_5117_ImportDeclerationAmendmentReplyResponseService = new DF_NG_5117_ImportDeclerationAmendmentReplyResponseService();
            this.MyResponseData=  dF_NG_5117_ImportDeclerationAmendmentReplyResponseService.Update5117(Cast5117Msg(customResponse), requestParams);
         }
 
        public UnifreightIIG.Common.MessageLib.ID.DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg Cast5117Msg(UnifreightIIG.Common.ImportDeclarationAmendmentServiceReference.DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customResponse)
        {


            string DeclarationString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                ns.Add("q", "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ImportDeclaration");
                var serializer = new XmlSerializer(customResponse.GetType());
                serializer.Serialize(stringwriter, customResponse,namespaces: ns);
                DeclarationString = stringwriter.ToString();
            }
            DeclarationString = DeclarationString.Replace("xmlns:q", "xmlns");
            DeclarationString = DeclarationString.Replace("<q:", "<");
            DeclarationString = DeclarationString.Replace("</q:", "</");

             using (var stringReader = new System.IO.StringReader(DeclarationString))
            {
                var serializer = new XmlSerializer(typeof( UnifreightIIG.Common.MessageLib.ID.DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg));

                try {  
                   return  serializer.Deserialize(stringReader) as UnifreightIIG.Common.MessageLib.ID.DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg;
 
               
                }
                catch ( System.Exception ex)
                {
                    return null;
                 }
 
            }


        }






    }
}
