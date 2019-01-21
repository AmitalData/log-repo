using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class MamanStatusAvailabilitySplitterService : CustomAnalyzerQueueBase
    {

        public MamanStatusAvailabilitySplitterService(InterfaceDetails MyInterfaceDetails)
            :base(MyInterfaceDetails)
        {

        }

        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            string courierMasterId = null, objectTableID = null, entityReference=null;
            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var defInterfaceName_ECSTS_Splited = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_ECSTB_Splited).First();


            var defInterfaceName_ECSTS_real = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_ECSTB).First();
            LogMessagingUtil.Instance.AppendLine("MamanStatusAvailabilitySplitterService");
            communicationsData = communicationsData ?? "";
            communicationsData=communicationsData.Trim();


            // Encode the XML string in a UTF-8 byte array
            byte[] encodedString = Encoding.UTF8.GetBytes(communicationsData);

            // Put the byte array into a stream and rewind it to the beginning
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;

            // Build the XmlDocument from the MemorySteam of UTF-8 encoded bytes
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ms);


            //var myXElementMamanBaldarSTB = XElement.Parse(communicationsData);
            //var listSTBMessage = myXElementMamanBaldarSTB.Descendants("STBMessage").ToList();
            //foreach (var itemSTBMessage in listSTBMessage)
            var listSTBMessage = new List<string>();
            foreach (XmlNode itemChildNode in xmlDoc.DocumentElement.ChildNodes)
                
            {

                var currSTBMessage = itemChildNode.OuterXml;
                listSTBMessage.Add(currSTBMessage);
                var analyzeQueueUtil = new AnalyzeQueueUtil();
                 var new_analyze =analyzeQueueUtil
                    .SaveMessageToAnalyzeQueue(this._AnalyzeQueue.FileName, Encoding.UTF8.GetBytes(currSTBMessage), this._CommunicationLog.Tenant,defInterfaceName_ECSTS_Splited);

                LogMessagingUtil.Instance.AppendLine($"new_analyze  CommunicationLogId = {new_analyze.CommunicationLogId}");
            }
            var qs = new DeclarationQueryService(_CommunicationLog.Tenant);
            var courierDeclarationQueryService = new CourierDeclarationQueryService(_CommunicationLog.Tenant);
            //do
            //{

            //} while (true);

            string decID = null;
            listSTBMessage
                .ToList().Any(// any stop first true !!!!
                xml =>
            {
                var xElementGWMessageSTBData = XElement.Parse(xml);
                string myBaldarAwb = (string)xElementGWMessageSTBData.Element("BaldarAwb");
                var idList = qs.GetListByCourierHAWB(myBaldarAwb, _CommunicationLog.Tenant);
                if (idList.Count == 0)
                {
                    //decID = idList.FirstOrDefault();
                    return false;
                }

                if (idList.Count == 1)
                {
                    decID = idList.FirstOrDefault();
                    return true;
                }
                string myBaldarOpenDate = (string)xElementGWMessageSTBData.Element("BaldarOpenDate");
                var consignmentQueryService = new ConsignmentQueryService(_CommunicationLog.Tenant);
                string myDeclarationId = consignmentQueryService.GetDeclarationIdBythirdCargoID(myBaldarOpenDate, _CommunicationLog.Tenant, idList);


                if (!string.IsNullOrWhiteSpace(myDeclarationId))
                {
                    decID = myDeclarationId;

                    return true;
                }

                return false;

            }
            );
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
            

            return new AnalyzeResultModel()
            {
                ErrorMessage = null,
                MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D,
                EntityID = courierMasterId,
                ObjectTableID = objectTableID,
                EntityReference = entityReference,
            };
        }


 
    }
}
