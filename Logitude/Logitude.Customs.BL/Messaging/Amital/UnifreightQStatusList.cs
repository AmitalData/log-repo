using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Amital
{
    public class UnifreightQStatusList
    {

        public enum StatusPartnerEnum
        {
            MVKR, MSVG
        }
        public List<StatusItem> GetStatusList(int tenant, string fileNo, StatusPartnerEnum statusPartnerEnum,
             out string ErrMessage)
        {
            ErrMessage = "";

            string P_MESSAGE = "";
            string xmlStatusList = "";


            try
            {

                string statusList = "", Subject = "";
                var myParams = new Hashtable();


                switch (statusPartnerEnum)
                {
                    case StatusPartnerEnum.MVKR:
                        statusList = "INA;INC"; Subject = "הערות סטטוס - מבקר";
                        break;
                    case StatusPartnerEnum.MSVG:
                        statusList = "SVC;SVR"; Subject = "הערות סטטוס - מסווג";
                        break;
                }

                myParams.Add("componentname", "GAQHSIVUG");
                myParams.Add("Operation", "GetStatusList");
                myParams.Add("Subject", "GAQHSIVUG:" + Subject);
                myParams.Add("StatusList", statusList);
                //if (!String.IsNullOrWhiteSpace(unifreigtUser))
                //{
                //    myParams.Add("$$GSC_USER_ID", unifreigtUser);
                //}
                myParams["CFIHMAIN:Xml"] = "";
                myParams.Add("FileNo", fileNo);

                string myParamsXML = UnifreightListsUtil.Serialize(myParams);
                string UnifreightTester = "";
                var resXML = SendMessageToUServerUtil.SendMessageToUServer(tenant, myParamsXML, out P_MESSAGE, out UnifreightTester);
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug(UnifreightTester);
                if (!String.IsNullOrWhiteSpace(resXML))
                {
                    var response = UnifreightListsUtil.Deserialize(resXML);
                    xmlStatusList = UnifreightListsUtil.GetHtmlDecodeValue(ref response, "StatusList");
                    var StatusItemlist = LogitudeXmlSerializer.DeserializeObject<List<StatusItem>>(xmlStatusList);
                    return StatusItemlist;


                }
                else
                {
                    ErrMessage = "UServer:Message =" + P_MESSAGE;
                    return (null);
                }
            }
            catch (Exception e)
            {
                ErrMessage = P_MESSAGE + e.ToString();

                return (null);
            }



        }

        public class StatusItem
        {
            public string StatusId { get; set; }
            public string StatusName { get; set; }

            public DateTime StatusDate { get; set; }

            public string StatusComment { get; set; }



        }
    }
}
