#if mergepilot20141214
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
using UnifreightIIG.Common.MessageLib.Deficit;
using UnifreightIIG.Common.Utils;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageAnalyzerService
    : MessageAnalyzerServiceBase<GenericRequestParams, 
        INF_MSG_GenericResponseData, 
        DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage, 
        Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageResponseService>, 
        IMessageAnalyzerService
    {
        public Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.Deficit.Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage.xsd")
        { }



        DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage _Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage;

        public override string GetObjectTableName()
        {
            return "Customs.PhysicalCheck";
        }

        void Validate1()
        {

            if (_Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage == null)
            {
                throw new System.Exception("Loadxml 1st ");
            }


            var xsdStream = ManifestResourceUtil.GetManifestResourceStreamXSD<DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage>(_Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage, MessageXSD);
            string targetNamespace = XmlGenericUtil<DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage>.GetDefaultNamespace(typeof(DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage));

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
            DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage deficitRequirementsMessage = null;

            deficitRequirementsMessage = _CustomResponse;
            var tenant = 1;
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();

            Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageResponseService customResponseService = new Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageResponseService();
            customResponseService.Update(deficitRequirementsMessage, requestParams);
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
#endif