using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using UnifreightIIG.Common.MessageLib.Gurntee;
using UnifreightIIG.Common.Utils;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class GRNT_MSG15_createGurateeRequestInfoAnalyzerService
    : MessageAnalyzerServiceBase<GenericRequestParams, 
        INF_MSG_GenericResponseData, 
        GRNT_MSG15_createGurateeRequestInfo, 
        GRNT_MSG15_createGurateeRequestInfoResponseService>, 
        IMessageAnalyzerService
    {
        public GRNT_MSG15_createGurateeRequestInfoAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.PhysicalCheck.GRNT_MSG15_createGurateeRequestInfo.xsd")
        { }



        GRNT_MSG15_createGurateeRequestInfo _GRNT_MSG15_createGurateeRequestInfo;

        public override string GetObjectTableName()
        {
            return "Customs.PhysicalCheck";
        }

        void Validate1()
        {

            if (_GRNT_MSG15_createGurateeRequestInfo == null)
            {
                throw new System.Exception("Loadxml 1st ");
            }


            var xsdStream = ManifestResourceUtil.GetManifestResourceStreamXSD<GRNT_MSG15_createGurateeRequestInfo>(_GRNT_MSG15_createGurateeRequestInfo, MessageXSD);
            string targetNamespace = XmlGenericUtil<GRNT_MSG15_createGurateeRequestInfo>.GetDefaultNamespace(typeof(GRNT_MSG15_createGurateeRequestInfo));

            XmlSchemaSet schemas = new XmlSchemaSet();


            schemas.Add(targetNamespace, XmlReader.Create(xsdStream));
            // new StringReader(xsdMarkup)));

            XDocument doc1 = XDocument.Parse(Xml);




            Debug.WriteLine("Validating doc1");
            bool errors = false;
            doc1.Validate(schemas, (o, e) =>
            {
                Debug.WriteLine("{0}", e.Message);
                errors = true;
            });
            Debug.WriteLine("doc1 {0}", errors ? "did not validate" : "validated");

        }


        public override Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            GRNT_MSG15_createGurateeRequestInfo createGurateeRequestInfo = null;

            createGurateeRequestInfo = _CustomResponse;
            var tenant = 1;
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();

            GRNT_MSG15_createGurateeRequestInfoResponseService customResponseService = new GRNT_MSG15_createGurateeRequestInfoResponseService();
            customResponseService.Update(createGurateeRequestInfo, requestParams);
            if (customResponseService.MyResponseData == null)
            {
                //??
                return null;
            }
            reData.ExceptionMessage = customResponseService.MyResponseData.ExceptionMessage;
            reData.ApplicationID = customResponseService.MyResponseData.ApplicationID;
            reData.HasException = customResponseService.MyResponseData.HasException;
            reData.Succeeded = customResponseService.MyResponseData.Succeeded;
            return reData;
        }

        private int CustomsAgentToTenant(int? CustomsAgent)
        {
            //  TODO: CustomsAgentToTenant- yaron how to know tenent     <customsAgent>510120041</customsAgent>
            return 1;
        }

        public override int ResolveTenant()
        {
            //return CustomsAgentToTenant(_CustomResponse.NoticeToClient.customsAgent);
            return 1;
        }

        public override string GetLoggingEntityReference()
        {
            //return _CustomResponse.NoticeToClient.checkId.ToString();
            return "";
        }
    }

}


