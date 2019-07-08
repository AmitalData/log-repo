
using Logitude.AmitalMessaging.Utils;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnifreightIIG.Common.RetrieveImportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2754_MSG10004_RetrieveImportDeclarationResponseService
        : ResponseServiceBase<DeclarationRestoreResponseData, DF_NG_2754_MSG10004_ImportDeclarationResponse, DeclarationRestoreRequestParams>
    {
        private DF_NG_2754_MSG10004_ImportDeclarationResponseService _DF_NG_2754_MSG10004_ImportDeclarationResponseService;
        private DeclarationRestoreResponseData _MyDefaultResponseData;
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption { get; set; }

        public override void Update(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, DeclarationRestoreRequestParams requestParams)
        {

            if (customResponse.ResponseContentHeader != null &&
                customResponse.Response == null &&
                customResponse.AddAGlobalScannedAttachmentToEntityResponse == null &&
                customResponse.CollateralRequestDetails == null)
            {
                _MyDefaultResponseData = new DeclarationRestoreResponseData()
                {
                    ApplicationID = customResponse.ResponseContentHeader.ApplicationID.ToString(),
                    Succeeded = true,
                    UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription,
                    HasException = false,
                    ResponseStatusXML = GetDummyXml(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription+customResponse.ResponseContentHeader.Remark,requestParams.IsAngularClient),
                };
                return;
            }
            //[XmlType(AnonymousType = true, Namespace = "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ImportDeclaration")]
            //[XmlType(AnonymousType = true, Namespace = "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ImportDeclaration")]
            UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse ser = null;
            var xml = XmlGenericUtil<DF_NG_2754_MSG10004_ImportDeclarationResponse>.SerializeObject(customResponse);
            ser = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse>.DeSerializeObject(xml);
            _DF_NG_2754_MSG10004_ImportDeclarationResponseService = new DF_NG_2754_MSG10004_ImportDeclarationResponseService();
            //ITZIK+MIRT  _DF_NG_2754_MSG10004_ImportDeclarationResponseService._ResponseHeaderExeption = _ResponseHeaderExeption;
            _DF_NG_2754_MSG10004_ImportDeclarationResponseService._IsRetrieveDeclarationResponse = true;
            _DF_NG_2754_MSG10004_ImportDeclarationResponseService.Update(ser, requestParams);

            //<--- Yuval Chalup 13.05.2015 TASK-11915
            if (customResponse.ResponseContentHeader != null &&
                customResponse.Response != null &&
                customResponse.AddAGlobalScannedAttachmentToEntityResponse == null &&
                customResponse.CollateralRequestDetails == null)
            {
                _MyDefaultResponseData = new DeclarationRestoreResponseData();
                if (customResponse.Response != null)
                {
                    if (requestParams.AppicationId != null && requestParams.DeclarationId == null) requestParams.DeclarationId = requestParams.AppicationId; // moran 10.1.16 Task 19724 + add to condition --><--
                    if (customResponse.Response.Declaration != null && _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.HasException != true)
                    {
                        xml = XmlGenericUtil<Declaration>.SerializeObject(customResponse.Response.Declaration);
                        _MyDefaultResponseData.ResponseStatusXML = xml;
                        if (requestParams.IsAngularClient)
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(xml);

                            _MyDefaultResponseData.ResponseStatusXML = //JsonConvert.SerializeObject(customResponse.Response.Declaration);
                                JsonConvert.SerializeXmlNode(doc);
                        }
                        
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(_DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage))
                        {
                            _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.HasException = false; // moran 10.1.16 Task 19724
                            _MyDefaultResponseData.ResponseStatusXML = GetDummyXml(_DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage,requestParams.IsAngularClient);
                        }
                    }
                }
            }
            //Yuval Chalup 13.05.2015 TASK-11915 --->
        }
    

        public override DeclarationRestoreResponseData GetResponse(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, DeclarationRestoreRequestParams requestParams)
        {
            if (_DF_NG_2754_MSG10004_ImportDeclarationResponseService == null || _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData == null)
            {
                return _MyDefaultResponseData;
            }
            var response = new DeclarationRestoreResponseData()
            {
                ApplicationID = _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.ApplicationID,
                UserMessage = _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage,
                HasException = _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.HasException,
                Succeeded = _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.Succeeded,
            };

            if (!string.IsNullOrWhiteSpace(_DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage) && _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage.Contains("נתוני ההצהרה לא עודכנו"))
            {
                response.IsShowUserMessage = true;
            }

            //<--- Yuval Chalup 13.05.2015 TASK-11915
            if (customResponse.ResponseContentHeader != null &&
                customResponse.Response != null &&
                customResponse.AddAGlobalScannedAttachmentToEntityResponse == null &&
                customResponse.CollateralRequestDetails == null)
            {
                if (this._MyDefaultResponseData != null)
                {
                    response.ResponseStatusXML = this._MyDefaultResponseData.ResponseStatusXML;
                    return response;
                }

                if (customResponse.Response != null)
                {
                    if (customResponse.Response.Declaration != null)
                    {
                        if (requestParams.IsAngularClient)
                        {
                            _MyDefaultResponseData.ResponseStatusXML = JsonConvert.SerializeObject(customResponse.Response.Declaration);
                        }
                        else
                        {
                            string xml = XmlGenericUtil<Declaration>.SerializeObject(customResponse.Response.Declaration);
                            _MyDefaultResponseData.ResponseStatusXML = xml;
                        }
                        
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(_DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage))
                        {
                            _MyDefaultResponseData.ResponseStatusXML = GetDummyXml(_DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage, requestParams.IsAngularClient);
                        }
                    }
                }
            }
            //Yuval Chalup 13.05.2015 TASK-11915 --->

            return response;
        }


        private string GetDummyXml(string message,bool toJson)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            if (toJson)
            {
                return JsonConvert.SerializeObject(myDummyXml);
            }
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }

        public class GeneralMessage
        {
            public string Message { get; set; }
        }
    }
}
