

//using Logitude.AmitalMessaging.Utils;
//using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
//using Logitude.Customs.BL.BL;
//using Logitude.Customs.BL.CloseTables;
//using Logitude.Customs.BL.EntityQueryServices;
//using Logitude.Customs.BL.EntityUpdateServices;
//using Logitude.Customs.BL.Messaging.Maman;
//using Logitude.Customs.BL.Messaging.U2L.CommDec;
//using Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments;
//using Logitude.Customs.BL.TraceEvents;
//using Logitude.Customs.Data;
//using Logitude.Customs.Data.Repsitories;
//using Logitude.Customs.Def.EntityPMs;
//using Logitude.Server.Tools.Helpers;
//using Logitude.Server.Tools.Models;
//using Simplog.Data.CommonDataModel.Repositories;
//using Simplog.Data.InfrastructureModel.Repositories;
//using Simplog.Server.Infrastructure;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using System.Xml;
//using System.Xml.Linq;
//using System.Xml.Serialization;

//namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
//{
//    public class UCUW2L_OpenDeclarationsQService : CustomAnalyzerQueueBase
//    {
//        //private DeclarationPM _DeclarationPM;

//        public UCUW2L_OpenDeclarationsQService(QueueDetails queueDetails)
//            : base(queueDetails)
//        {

//        }

//        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
//        {
//            var res = new AnalyzeResultModel();
//            try
//            {

//                res.ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
//                LogMessagingUtil.Instance.AppendLine("UCUW2L_OpenDeclarationsQService");
//                //XmlSerializer serializer = new XmlSerializer(typeof(DCAInUCUW2LResponseContentHeader));
//                //DCAInUCUW2LResponseContentHeader mySTBMessage;
//                string mySTBMessage = "";
//                int tetant=0;
//                communicationsData = HttpUtility.HtmlDecode(communicationsData);
//                using (TextReader reader = new StringReader(communicationsData))
//                {
//                    var x = reader.ReadLine();
//                    XmlDocument doc = new XmlDocument();
//                    doc.Load(reader);
//                    XmlNodeList elemList = doc.GetElementsByTagName("LOGICOMMDEC");
//                    XmlNodeList te = doc.GetElementsByTagName("Tenant");
//                    mySTBMessage = elemList[0].OuterXml;
//                    tetant = int.Parse(te[0].InnerText);
//                    //mySTBMessage = (DCAInUCUW2LResponseContentHeader)serializer.Deserialize(reader1);
//                }


//                var unifreightGenericService = new Do_CommDecService();

//                try
//                {
//                    string moreParams =
//     @"<?xml version=""1.0"" encoding=""utf-8"" ?>
//<ArrayOfEntry xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
// <Entry>
//  <Key>MODE</Key>
//  <Value></Value>
// </Entry>
// <Entry>
//  <Key>TENANT</Key>
//  <Value>@TENANT@</Value>
// </Entry>
// <Entry>
//  <Key>UNIFREIGHT_USER_ID</Key>
//  <Value>@UNIFREIGHT_USER_ID@</Value>
// </Entry>
//</ArrayOfEntry>";
//                    moreParams = moreParams.Replace("@TENANT@", tetant.ToString());
//                    moreParams = moreParams.Replace("@UNIFREIGHT_USER_ID@", "AMITAL");
//                    string error = "";
//                    string decId = "";
//                    string courierMasterID = "";
//                    string customFileNo = "";
//                    unifreightGenericService.ProccessGenericRequestReal(mySTBMessage, tetant, null, null, ref moreParams, out error, out customFileNo, out decId, out courierMasterID);
//                    res.EntityID = decId;
//                    res.EntityReference = customFileNo;
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception(ex.Message + "--" + ex.StackTrace);
//                }
                
//                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;
//                // res.ErrorMessage = messageOut;
//            }
//            catch (BusinessErrorException ee)
//            {
//                res.ErrorMessage = ee.ToString();
//                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;
//                return res;
//            }
//            catch (Exception ee)
//            {
//                ////res.ErrorMessage = ee.Message + "--" + ee.StackTrace;
//                //res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
//               // return res;
//                throw new Exception(ee.Message + "--" + ee.StackTrace);
//            }
//            return res;
//        }
       
       

//    }

//    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCUW2LResponseContentHeader", IsNullable = false)]
//    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCUW2LResponseContentHeader")]
//    public class DCAInUCUW2LResponseContentHeader 
//    {
//        public int tenant { get; set; }
//        public string LOGICOMMDEC { get; set; }
//        //public string MoreParams { get; set; }
       
//    }



  

//}
