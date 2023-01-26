
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.ILSWS;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class ILSWSQHAWBSplitterService : CustomAnalyzerQueueBase
    {
        public ILSWSQHAWBSplitterService(InterfaceDetails MyInterfaceDetails)
            : base(MyInterfaceDetails)
        {

        }

        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            var courierDeclarationQueryService = new CourierDeclarationQueryService(_CommunicationLog.Tenant);

            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var defInterfaceName_ILSWSQHAWB = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_ECSWSTHR_RESPONE).First();


            string courierMasterId = null, objectTableID = null, entityReference = null, decID = null;
            communicationsData = communicationsData ?? "";
            communicationsData = communicationsData.Trim();

            // Encode the XML string in a UTF-8 byte array
            byte[] encodedString = Encoding.UTF8.GetBytes(communicationsData);

            // Put the byte array into a stream and rewind it to the beginning
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;

            // Build the XmlDocument from the MemorySteam of UTF-8 encoded bytes
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(communicationsData);


            //var myXElementMamanBaldarSTB = XElement.Parse(communicationsData);
            //var listSTBMessage = myXElementMamanBaldarSTB.Descendants("STBMessage").ToList();
            //foreach (var itemSTBMessage in listSTBMessage)
            var listSTBMessage = new List<string>();
            foreach (XmlNode itemChildNode in xmlDoc.DocumentElement.ChildNodes)
            {

                var currSTBMessage = itemChildNode.OuterXml;
                listSTBMessage.Add(currSTBMessage);
            }

            decID = Get1stDeclarationId(listSTBMessage, courierDeclarationQueryService);

            if (!string.IsNullOrWhiteSpace(decID))
            {
                var courierDeclaration = courierDeclarationQueryService.GetCourierDeclarationByDeclarationId(decID, _CommunicationLog.Tenant);
                if (courierDeclaration != null)
                {
                    courierMasterId = courierDeclaration.CourierMasterId;
                    objectTableID = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
                    entityReference = courierDeclaration.ExternalDeclarationNumber;


                }
            }
            var res = new AnalyzeResultModel()
            {
                ErrorMessage = null,
                MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D,
                EntityID = courierMasterId,
                ObjectTableID = objectTableID,
                EntityReference = entityReference,
            };

            listSTBMessage.ForEach(currSTBMessage =>
            {
                var analyzeQueueUtil = new AnalyzeQueueUtil();
                var new_analyze = analyzeQueueUtil
                  .SaveMessageToAnalyzeQueue(this._AnalyzeQueue.FileName, Encoding.UTF8.GetBytes(currSTBMessage), this._CommunicationLog.Tenant, "", defInterfaceName_ILSWSQHAWB, res);

            });

           


            return res;
        }
        //private static XElement GetXElement(XElement myXElementSTBMessage, string field)
        //{
        //    XElement ele = myXElementSTBMessage.Element(field);
        //    if (ele == null)
        //    {
        //        throw new Exception($"XElement {field} not exist ");
        //    }

        //    return ele;
        //}

        //private static List<CourierSWSHAWBResponse> GetSWSHAWBResponse(string communicationsData)
        //{

        //    var listSWSHAWBMessage = new List<CourierSWSHAWBResponse>();
        //    var myXElementSWSHAWBResponse = XElement.Parse(communicationsData);
        //    if (myXElementSWSHAWBResponse != null)
        //    {
        //        var ArrayOfSWSHAWBResponse = myXElementSWSHAWBResponse.Elements("CourierHawbFeedback");
        //        if (ArrayOfSWSHAWBResponse != null)
        //        {
        //            foreach (var item in ArrayOfSWSHAWBResponse)
        //            {
        //                var mySWSHAWBResponse = new CourierSWSHAWBResponse();
        //                mySWSHAWBResponse.CourierCompanyVat = (string)GetXElement(item, "CourierCompanyVat");
        //                mySWSHAWBResponse.CourierHawbNumber = (string)GetXElement(item, "CourierHawbNumber");
        //                mySWSHAWBResponse.StatusCode = (string)GetXElement(item, "StatusCode");
        //                mySWSHAWBResponse.ErrorCode = (string)GetXElement(item, "ErrorCode");
        //                mySWSHAWBResponse.ErrorDescription = (string)GetXElement(item, "ErrorDescription");
        //                listSWSHAWBMessage.Add(mySWSHAWBResponse);
        //            }
        //        }
        //        else
        //        {
        //            var mySWSHAWBResponse = new CourierSWSHAWBResponse();
        //            mySWSHAWBResponse.CourierCompanyVat = (string)GetXElement(myXElementSWSHAWBResponse, "CourierCompanyVat");
        //            mySWSHAWBResponse.CourierHawbNumber = (string)GetXElement(myXElementSWSHAWBResponse, "CourierHawbNumber");
        //            mySWSHAWBResponse.StatusCode = (string)GetXElement(myXElementSWSHAWBResponse, "StatusCode");
        //            mySWSHAWBResponse.ErrorCode = (string)GetXElement(myXElementSWSHAWBResponse, "ErrorCode");
        //            mySWSHAWBResponse.ErrorDescription = (string)GetXElement(myXElementSWSHAWBResponse, "ErrorDescription");
        //            listSWSHAWBMessage.Add(mySWSHAWBResponse);
        //        }
        //    }
        //    return listSWSHAWBMessage;
        //}
        private string Get1stDeclarationId(List<string> listSTBMessage, CourierDeclarationQueryService courierDeclarationQueryService)
        {
            string decID = null;

            DeclarationQueryService qs = new DeclarationQueryService(_CommunicationLog.Tenant);

            listSTBMessage
                .ToList().Any(// any stop first true !!!!
xml =>
{
    var xElementGWMessageSTBData = XElement.Parse(xml);
    string myBaldarAwb = (string)xElementGWMessageSTBData.Element("CourierHawbNumber");
    var idList = qs.GetListByCourierHAWB(myBaldarAwb, _CommunicationLog.Tenant);
    if (idList.Count == 0)
    {
        //decID = idList.FirstOrDefault();
        return false;
    }

    if (idList.Count > 0)
    {
        decID = idList.FirstOrDefault();
        return true;
    }
    return false;
}
);
            return decID;
        }
    }
}
