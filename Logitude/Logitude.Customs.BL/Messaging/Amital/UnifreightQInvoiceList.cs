using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Logitude.Customs.BL.Messaging.Amital
{
    public class UnifreightQInvoiceList
    {

        public AllInvoices GetInvoice(int tenant, string fileNo, out string ErrMessage)
        {
            ErrMessage = "";

            string P_MESSAGE = "";
            string xmlStatusList = "";


            try
            {

                string statusList = "", Subject = " file "+ fileNo;
                var myParams = new Hashtable();


               

                myParams.Add("componentname", "GDSHMAINXML");
                myParams.Add("Operation", "GetQInvoice");
                myParams.Add("Subject", "GetQInvoice:" + Subject);
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
                Debug.WriteLine(UnifreightTester);
                if (!String.IsNullOrWhiteSpace(resXML))
                {
                    //var response = UnifreightListsUtil.Deserialize(resXML);
                    //xmlStatusList = UnifreightListsUtil.GetHtmlDecodeValue(ref response, "StatusList");
                    string alexGiveBadXML = $"<AllInvoices>{resXML}</AllInvoices>";
                    var StatusItemlist = LogitudeXmlSerializer.DeserializeObject<AllInvoices>(alexGiveBadXML);
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

        public AllInvoices GetInvoice()
        {

            var resXML = @"<AllInvoices><Statuses>
<StatusData>
  <Code>OPN</Code>
  <Name>פתיחה</Name>
  <Date>29.03.20</Date>
  <Time>10:05</Time>
  <Comments/>
</StatusData>
<StatusData>
  <Code>ETA</Code>
  <Name>תאריך הגעה משוער</Name>
  <Date>01.06.19</Date>
  <Time>00:00</Time>
</StatusData>
<StatusData>
  <Code>MWB</Code>
  <Name>בדיקה</Name>
  <Date>26.03.20</Date>
  <Time>00:00</Time>
  <Comments>MAWB = 235-40044422</Comments>
</StatusData>
<StatusData>
  <Code>ATA</Code>
  <Name>הגעה</Name>
  <Date>29.03.20</Date>
  <Time>00:00</Time>
  <Comments>הערך הישן הוא 01.06.19,
הערך החדש הוא 29.03.20</Comments>
</StatusData>
</Statuses>
<InvoiceLines>
<InvoiceLine>
  <ServiceCode>1</ServiceCode>
  <ServiceName>הובלה ימית/אוירית</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>2457</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>2457</AmountForeign>
</InvoiceLine>
<InvoiceLine>
  <ServiceCode>1</ServiceCode>
  <ServiceName>הובלה ימית/אוירית</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>111</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>111</AmountForeign>
</InvoiceLine>
<InvoiceLine>
  <ServiceCode>23</ServiceCode>
  <ServiceName>בדיקה פיזית/פירוט</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>15.5</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>15.5</AmountForeign>
</InvoiceLine>
<InvoiceLine>
  <ServiceCode>COM</ServiceCode>
  <ServiceName>עמלה</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>10</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>10</AmountForeign>
</InvoiceLine>
</InvoiceLines>
<IntegratedInvoices>
<IntegratedInvoice>
  <InvoiceNumber>ci200026</InvoiceNumber>
  <ForwarderFile>A00002928</ForwarderFile>
  <InvoiceCurrency>NIS</InvoiceCurrency>
  <InvoiceAmount>17685</InvoiceAmount>
</IntegratedInvoice>
</IntegratedInvoices>
<Invoices>
<Invoice>
  <InvoiceBillTo>COM</ServiceCode>
  <InvoiceType>עמלה</ServiceName>
  <InvoiceDate>L</PayType>
  <InvoiceCurrency>NIS</Currency>
  <InvoiceAmount>10</AmountForeign>
</Invoice>
</Invoices>
<Messages>
 <MessagesData>
  <E>חסר תאור בשורה של סעיף MMN1</E>
 </MessagesData>
 <MessagesData>
  <W>בכרטיס 10008559 מספר עוסק מורשה חייב להיות באורך 9</W>
 </MessagesData>
</Messages>
</AllInvoices>
 ";
            if (!String.IsNullOrWhiteSpace(resXML))
            {
                using (var stringReader = new System.IO.StringReader(resXML))
                {
                    var serializer = new XmlSerializer(typeof(AllInvoices));


                   var x=  serializer.Deserialize(stringReader) as AllInvoices;
                    return x;
                }
                

                //var response = UnifreightListsUtil.Deserialize(resXML);
                //var xmlStatusList = UnifreightListsUtil.GetHtmlDecodeValue(ref response, "InvoiceList");
                //var StatusItemlist = LogitudeXmlSerializer.DeserializeObject<List<Invoices>>(xmlStatusList);
                //return StatusItemlist;


            }


            return null;

        }

        public class AllInvoices
        {

            public List<StatusData> Statuses;
            public List<InvoiceLine> InvoiceLines;
            public List<IntegratedInvoice> IntegratedInvoices;
            public List<Invoice> Invoices;
            public List<Messages> MessagesData;


        }
        public class Invoice
        {
            public string InvoiceBillTo { get; set; }
            public string InvoiceType { get; set; }
            public string InvoiceDate { get; set; }
            public string InvoiceCurrency { get; set; }
            public decimal InvoiceAmount { get; set; }

        }
        public class StatusData
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string Comments { get; set; }

        }

        public class InvoiceLine
        {
            public string ServiceCode { get; set; }
            public string ServiceName { get; set; }
            public string PayType { get; set; }
            public string AmountNIS { get; set; }
            public string AmountForeign { get; set; }
            public string Currency { get; set; }


        }



        public class IntegratedInvoice
        {
            public string InvoiceNumber { get; set; }
            public string ForwarderFile { get; set; }
            public string InvoiceCurrency { get; set; }
            public decimal InvoiceAmount { get; set; }
        }
        public class Messages
        {
            public string W { get; set; }
            public string E { get; set; }
        }


    }
}
