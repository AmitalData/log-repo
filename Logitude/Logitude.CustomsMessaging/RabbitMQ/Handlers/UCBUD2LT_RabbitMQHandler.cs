using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
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

namespace Logitude.CustomsMessaging.RabbitMQ.Handlers
{
    public class UCBUD2LT_RabbitMQHandler : CustomAnalyzerQueueBase
    {
        private Customs.Def.EntityPMs.DeclarationPM _DeclarationPM;

        public UCBUD2LT_RabbitMQHandler(QueueDetails queueDetails)
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
                
                DCAInUCBUD2LTWithResponseContentHeader customsResponse;
                using (TextReader reader = new StringReader(communicationsData))
                {


                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);

                    XmlNodeList elemList = doc.GetElementsByTagName("Body");

                    customsResponse = (DCAInUCBUD2LTWithResponseContentHeader)serializer.Deserialize(new StringReader(elemList[0].InnerXml));


                }



                if (string.IsNullOrWhiteSpace(customsResponse.DocumentTypeCode))
                {
                    res.ErrorMessage = $"bad communicationsData  mySTBMessage.DocumentTypeCode is null";
                    res.MyCommStatusEnum = Customs.Def.ClosedTable.CommStatusEnum.F;
                    return res;
                }

                var qsDeclarationQueryService = new DeclarationQueryService(customsResponse.tenant);

                var xml = XmlGenericUtil<LOGIDOCS>.SerializeObject(
              new LOGIDOCS()
              {
                  LogitudeDocs = new LogitudeDocs[] {
                     new LogitudeDocs(){
                         COM_ID =customsResponse.DocumentsFilingId,
                         DOC_ID =customsResponse.DocumentTypeCode,
                         Id =customsResponse.DeclarationId , Tenant= customsResponse.tenant.ToString()
                     }
               }
              }
          );

                //Direct(mySTBMessage, xml);
                try
                {
                    var rs = new UniCourierBatchSendUCBUD2LT_MsgResponseService();
                    rs.Update(
                        customsResponse,
                        new Common.RequestParams.GenericRequestParams()
                        {
                            Tenant = customsResponse.tenant,
                            RequestName = $" UD2LT   קישור מסמך לטיקט" + customsResponse.DocumentsFilingCode + " "
                        });

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message + "--" + ex.StackTrace);
                }
                _DeclarationPM = qsDeclarationQueryService.GetSingle(customsResponse.DeclarationId, true, false);
                res.EntityID = customsResponse.DeclarationId;
                res.EntityReference = _DeclarationPM.CustomFileNo;
                res.MyCommStatusEnum = Customs.Def.ClosedTable.CommStatusEnum.D;
                // res.ErrorMessage = messageOut;


            }

            catch (BusinessErrorException ee)
            {
                res.ErrorMessage = ee.ToString();
                res.MyCommStatusEnum = Customs.Def.ClosedTable.CommStatusEnum.D;
                return res;
            }
            catch (Exception ee)
            {
                ////res.ErrorMessage = ee.Message + "--" + ee.StackTrace;
                //res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                // return res;
                throw new Exception(ee.Message + "--" + ee.StackTrace);
            }
            return res;
        }

        private static void Direct(DCAInUCBUD2LTWithResponseContentHeader mySTBMessage, string xml)
        {
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
            try
            {
                unifreightGenericService.ProccessGenericRequest(xml, ref moreParams, out messageOut);

            }
            catch (Exception ex)
            {
                // res.ErrorMessage = ex.Message;
                //res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.W;
                //res.EntityID = mySTBMessage.DeclarationId;
                //res.EntityReference = _DeclarationPM.CustomFileNo;
                throw new Exception(ex.Message + "--" + ex.StackTrace);
            }
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

  

}
