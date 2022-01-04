using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using Logitude.Customs.BL.Messaging.U2L.CommDec;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.CustomsMessaging.RabbitMQ.Handlers
{
    public class UCUW2L_RabbitMQHandler : CustomAnalyzerQueueBase
    {
        //private DeclarationPM _DeclarationPM;

        public UCUW2L_RabbitMQHandler(QueueDetails queueDetails)
            : base(queueDetails)
        {

        }


        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            var res = new AnalyzeResultModel();
            try
            {

                res.ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                LogMessagingUtil.Instance.AppendLine("UCUW2L_OpenDeclarationsQService");
                //XmlSerializer serializer = new XmlSerializer(typeof(DCAInUCUW2LResponseContentHeader));
                //DCAInUCUW2LResponseContentHeader mySTBMessage;
                string mySTBMessage = "";
                int tenant = 0;
                communicationsData = HttpUtility.HtmlDecode(communicationsData);
                using (TextReader reader = new StringReader(communicationsData))
                {
                    var x = reader.ReadLine();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);
                    XmlNodeList elemList = doc.GetElementsByTagName("LOGICOMMDEC");
                    XmlNodeList te = doc.GetElementsByTagName("Tenant");
                    mySTBMessage = elemList[0].OuterXml;
                    tenant = int.Parse(te[0].InnerText);
                    //mySTBMessage = (DCAInUCUW2LResponseContentHeader)serializer.Deserialize(reader1);
                }

                var UCUW2LResponseService = new DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceResponseService();
                UCUW2LResponseService.Update(
                    new MessagingServices.DCAInUCUW2LResponseContentHeader()
                    {
                        LOGICOMMDEC = mySTBMessage
                    },
                 new MessagingServices.DCAInUCUW2LRequestParams()
                 {

                     Tenant = tenant,
                     LoggingUserId = null,


                 }
                );
                string decId = UCUW2LResponseService.MyResponseData.ApplicationID;
                string customFileNo = UCUW2LResponseService?.MyRequestSheetParam.CustomFileNo;
                res.EntityID = UCUW2LResponseService?.MyRequestSheetParam.EntityId1;
                res.EntityReference = customFileNo;

                if (UCUW2LResponseService.MyResponseData.Succeeded)
                {
                    res.MyCommStatusEnum = Customs.Def.ClosedTable.CommStatusEnum.D;
                }
                else
                {
                    res.MyCommStatusEnum = Customs.Def.ClosedTable.CommStatusEnum.W;
                }
                
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
        protected  AnalyzeResultModel AnalyzeDataOld(string communicationsData)
        {
            var res = new AnalyzeResultModel();
            try
            {

                res.ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                LogMessagingUtil.Instance.AppendLine("UCUW2L_OpenDeclarationsQService");
                //XmlSerializer serializer = new XmlSerializer(typeof(DCAInUCUW2LResponseContentHeader));
                //DCAInUCUW2LResponseContentHeader mySTBMessage;
                string mySTBMessage = "";
                int tetant=0;
                communicationsData = HttpUtility.HtmlDecode(communicationsData);
                using (TextReader reader = new StringReader(communicationsData))
                {
                    var x = reader.ReadLine();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);
                    XmlNodeList elemList = doc.GetElementsByTagName("LOGICOMMDEC");
                    XmlNodeList te = doc.GetElementsByTagName("Tenant");
                    mySTBMessage = elemList[0].OuterXml;
                    tetant = int.Parse(te[0].InnerText);
                    //mySTBMessage = (DCAInUCUW2LResponseContentHeader)serializer.Deserialize(reader1);
                }


                var unifreightGenericService = new Do_CommDecService();

                try
                {
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
                    moreParams = moreParams.Replace("@TENANT@", tetant.ToString());
                    moreParams = moreParams.Replace("@UNIFREIGHT_USER_ID@", "AMITAL");
                    string error = "";
                    string decId = "";
                    string courierMasterID = "";
                    string customFileNo = "";
                    unifreightGenericService.ProccessGenericRequestReal(mySTBMessage, tetant, null, null, ref moreParams, out error, out customFileNo, out decId, out courierMasterID);
                    res.EntityID = decId;
                    res.EntityReference = customFileNo;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "--" + ex.StackTrace);
                }
                
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
       
       

    }

    



  

}
