
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class UCBUD2LT_ConnectDocToTicketQService : CustomAnalyzerQueueBase
    {
        private DeclarationPM _DeclarationPM;

        public UCBUD2LT_ConnectDocToTicketQService(QueueDetails queueDetails)
            : base(queueDetails)
        {

        }

        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            var res = new AnalyzeResultModel();
            try
            {

                res.ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                LogMessagingUtil.Instance.AppendLine("UCBUD2LT_ConnectDocToTicketQService");
                XmlSerializer serializer = new XmlSerializer(typeof(DCAInUCBUD2LTWithResponseContentHeader));
                DCAInUCBUD2LTWithResponseContentHeader mySTBMessage;
                using (TextReader reader = new StringReader(communicationsData))
                {
                     

                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);

                     XmlNodeList elemList = doc.GetElementsByTagName("Body");
                  
                   mySTBMessage = (DCAInUCBUD2LTWithResponseContentHeader)serializer.Deserialize(new StringReader(elemList[0].InnerXml));

                    
                }



                 if (string.IsNullOrWhiteSpace(mySTBMessage.DocumentTypeCode))
                {
                    res.ErrorMessage = $"bad communicationsData  mySTBMessage.DocumentTypeCode is null";
                    res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                    return res;
                }

                var qsDeclarationQueryService = new DeclarationQueryService(mySTBMessage.tenant);

                var xml = XmlGenericUtil<LOGIDOCS>.SerializeObject(
              new LOGIDOCS()
              {
                  LogitudeDocs = new LogitudeDocs[] {
                     new LogitudeDocs(){
                         COM_ID =mySTBMessage.DocumentsFilingId,
                         DOC_ID =mySTBMessage.DocumentTypeCode,
                         Id =mySTBMessage.DeclarationId , Tenant= mySTBMessage.tenant.ToString()
                     }
               }
              }
          );


                var unifreightGenericService = new DeclarationDocumentsService();
                string moreParams =
    @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<ArrayOfEntry xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
 <Entry>
  <Key>MODE</Key>
  <Value></Value>
 </Entry>
 <Entry>
  <Key>TENANT</Key>
  <Value>@TENANT@</Value>
 </Entry>
 <Entry>
  <Key>UNIFREIGHT_USER_ID</Key>
  <Value>@UNIFREIGHT_USER_ID@</Value>
 </Entry>
</ArrayOfEntry>";
                moreParams = moreParams.Replace("@TENANT@", mySTBMessage.tenant.ToString());
                moreParams = moreParams.Replace("@UNIFREIGHT_USER_ID@", "AMITAL");
                string messageOut = "";
                unifreightGenericService.ProccessGenericRequest(xml, ref moreParams, out messageOut);
                _DeclarationPM = qsDeclarationQueryService.GetSingle(mySTBMessage.DeclarationId, true, false);
                res.EntityID = mySTBMessage.DeclarationId;
                res.EntityReference = _DeclarationPM.CustomFileNo;
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;



            }

            catch (BusinessErrorException ee)
            {
                res.ErrorMessage = ee.ToString();
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;
            }
            catch (Exception ee)
            {
                throw;
            }
            return res;
        }
        void UpdateAVA(string theDecId, int EventQty)
        {
            string AcceptanceStatusCode = "";
            var totPackageQuantity = _DeclarationPM.Consignments.SelectMany(r => r.ConsignmentPackages).Sum(p => p.PackageQuantity);
            if (EventQty == totPackageQuantity)
            {
                AcceptanceStatusCode = "1";
            }
            else if (EventQty < totPackageQuantity)
            {
                AcceptanceStatusCode = "2";
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("EventQty > totPackageQuantity  ??? >>throw new Exception- Eitan confirm ?!?!? ");
                throw new BusinessErrorException("EventQty > totPackageQuantity  ???");
            }
            _DeclarationPM.AcceptanceStatusCode = AcceptanceStatusCode;

            var customContext = CustomContext.GetContext(_CommunicationLog.Tenant);
            var declarationUpdateService = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), _CommunicationLog.Tenant);
            _DeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            declarationUpdateService.Update(_DeclarationPM, true);
            LogMessagingUtil.Instance.AppendLine("Declaration Payment Date " + _DeclarationPM.PaymentDate + " Declaration Total Tax " + _DeclarationPM.TotalTax);
            if (_DeclarationPM.PaymentDate.HasValue && (_DeclarationPM.TotalTax == null || _DeclarationPM.TotalTax == 0))
            {
                var declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_CommunicationLog.Tenant);
                var currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(theDecId, true, false);

                currentDeclarationCourierStatusPM.CourierPaymentStatusCode = "P";

                var declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), _CommunicationLog.Tenant);
                currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                LogMessagingUtil.Instance.AppendLine("Update Declaration Courier Status CourierPaymentStatusCode=" + currentDeclarationCourierStatusPM.CourierPaymentStatusCode);
            }

        }


        private static DCAInUCBUD2LTWithResponseContentHeader GetSTBMessage(string communicationsData)
        {


            var mySTBMessage = new DCAInUCBUD2LTWithResponseContentHeader();
            var myXElementSTBMessage = XElement.Parse(communicationsData);

            mySTBMessage.DocumentsFilingId = (string)GetXElement(myXElementSTBMessage, "DocumentsFilingId");//<CourierHawbNumber>177553172644104455</CourierHawbNumber>
            mySTBMessage.DocumentTypeCode = (string)GetXElement(myXElementSTBMessage, "DocumentTypeCode");//<CourierHawbDate>241118</CourierHawbDate>
            mySTBMessage.DeclarationId = (string)GetXElement(myXElementSTBMessage, "DeclarationId");//<EventCode>1234</EventCode>
            mySTBMessage.tenant = (int)GetXElement(myXElementSTBMessage, "tenant");//<EventTime>2019-01-01T10:14:35.433269+02:00</EventTime>
        
            return mySTBMessage;
        }

        private static XElement GetXElement(XElement myXElementSTBMessage, string field)
        {



            XElement ele = myXElementSTBMessage.Element(field);
            if (ele == null)
            {
                throw new Exception($"XElement {field} not exist ");
            }

            return ele;
        }

    }

    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCBUD2LTWithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCBUD2LTWithResponseContentHeader")]
    public class DCAInUCBUD2LTWithResponseContentHeader 
    {

      
        public int tenant { get; set; }
        public string LoggingUserId { get; set; }
        public string DeclarationId { get; set; }
        //public string master { get; set; }

        public LOGIDOCS MyLOGIDOCS { get; set; }

        public string MyMoreParams { get; set; }
        public string DocumentsFilingCode { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DOCUMENTTYPEID { get; set; }
        public string DocumentTypeCode { get; set; }

 
    }

}
