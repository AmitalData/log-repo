using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnifreightIIG.Common.MessageLib.PhysicalCheck;
using UnifreightIIG.Common.Utils;

namespace CustomsWorkerRole.MessageAnalyzer
{
    public class CH_NG_190_MSG1_NoticeToClientAnalyzerService
        : MessageAnalyzerServiceBase<CH_NG_190_MSG1_NoticeToClient>, IMessageAnalyzerService
    {
         public CH_NG_190_MSG1_NoticeToClientAnalyzerService()
           :base("UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd")
        {}
        
       

        CH_NG_190_MSG1_NoticeToClient _CH_NG_190_MSG1_NoticeToClient;
        //const string MyXSD = "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd";

        public override string GetObjectTableName()
        {
            return "Customs.PhysicalChecks";
        }

        void Validate1()
        {

            if (_CH_NG_190_MSG1_NoticeToClient == null)
            {
                throw new Exception("Loadxml 1st ");
            }


            var xsdStream = ManifestResourceUtil.GetManifestResourceStreamXSD<CH_NG_190_MSG1_NoticeToClient>(_CH_NG_190_MSG1_NoticeToClient, MessageXSD);
            string targetNamespace = XmlGenericUtil<CH_NG_190_MSG1_NoticeToClient>.GetDefaultNamespace(typeof(CH_NG_190_MSG1_NoticeToClient));

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


        public override Logitude.CustomsMessaging.ResponseData.INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(CH_NG_190_MSG1_NoticeToClient));
            CH_NG_190_MSG1_NoticeToClient physicalCheckMessage = null;///(CH_NG_190_MSG1_NoticeToClient)serializer.Deserialize(memstream);
            physicalCheckMessage = _CustomResponse;
            var tenant = 1;
            var reData = new Logitude.CustomsMessaging.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();
            ///InsertPhysicalCheck(physicalCheckMessage);
            CH_NG_190_MSG1_NoticeToClientResponseService customResponseService = new CH_NG_190_MSG1_NoticeToClientResponseService();
            customResponseService.Update(physicalCheckMessage, requestParams);
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




        //public override string GetMessageXSD()
        //{
        //    return "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_190_MSG1_NoticeToClient.xsd";
        //}

        public override int ResolveTenant()
        {
            return  CustomsAgentToTenant(_CustomResponse.NoticeToClient.customsAgent);
        }
    }
}
