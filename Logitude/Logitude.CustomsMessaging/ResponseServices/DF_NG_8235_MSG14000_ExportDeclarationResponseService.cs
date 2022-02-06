
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using Logitude.Customs.BL.BL;
using System.Xml.Serialization;
using System.Xml.Linq;
using UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference;
using Logitude.CustomsMessaging.MessagingServices;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_8235_MSG14000_ExportDeclarationResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg, AmendmentRequestParams>
    {
        public override void OnRequestFail(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse, AmendmentRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }
            base.OnRequestFail(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(
            DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse, AmendmentRequestParams requestParams)
        {

            /// itzik test     TestTrans(requestParams);
            return this.MyResponseData;
        }


        public override void Update(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse, AmendmentRequestParams requestParams)
        {

            DF_NG_8237_ExportDeclerationAmendmentReplyResponseService dF_NG_8237_ImportDeclerationAmendmentReplyResponseService = new DF_NG_8237_ExportDeclerationAmendmentReplyResponseService();
            this.MyResponseData = dF_NG_8237_ImportDeclerationAmendmentReplyResponseService.Update8237(Cast8235Msg(customResponse), requestParams);
        }

        public UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg Cast8235Msg(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse)
        {


            string DeclarationString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                //ns.Add("q", "http://malam.com/customs/DealFile/Declaration/DF_NG_2751_MSG10000_ExportDeclaration");
                var serializer = new XmlSerializer(customResponse.GetType());
                serializer.Serialize(stringwriter, customResponse, namespaces: ns);
                DeclarationString = stringwriter.ToString();
            }
            DeclarationString = DeclarationString.Replace("xmlns:q", "xmlns");
            DeclarationString = DeclarationString.Replace("<q:", "<");
            DeclarationString = DeclarationString.Replace("</q:", "</");

            using (var stringReader = new System.IO.StringReader(DeclarationString))
            {
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg));

                try
                {
                    return serializer.Deserialize(stringReader) as UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg;


                }
                catch (System.Exception ex)
                {
                    return null;
                }

            }


        }






    }
}
