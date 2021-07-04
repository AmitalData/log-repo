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

                string statusList = "", Subject = " file " + fileNo;
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
                    // return GetInvoice();
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

            var resXML = @"<Statuses>
 <StatusData>
  <Code>OPN</Code>
  <Name>פתיחה</Name>
  <Date>07.05.20</Date>
  <Time>12:40</Time>
  <Comments/>
 </StatusData>
</Statuses>
<InvoiceLines>
 <InvoiceLine>
  <ServiceCode>56</ServiceCode>
  <ServiceName/>
  <PayType>L</PayType>
  <AmountNIS>-100</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign></AmountForeign>
  <Wip>N</Wip>
<LineNumber>1</LineNumber>
 </InvoiceLine>
 <InvoiceLine>
  <ServiceCode>COM</ServiceCode>
  <ServiceName>עמלה</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>-344</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>344</AmountForeign>
  <Wip>N</Wip>
<LineNumber>2</LineNumber>
 </InvoiceLine>
 <InvoiceLine>
  <ServiceCode>T16</ServiceCode>
  <ServiceName>אגרת ביטחון</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>41</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>41</AmountForeign>
  <Wip>Y</Wip>
<LineNumber>3</LineNumber>
 </InvoiceLine>
 <InvoiceLine>
  <ServiceCode>T6</ServiceCode>
  <ServiceName>אגרת מחשב</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>36</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>36</AmountForeign>
  <Wip>Y</Wip>
<LineNumber>4</LineNumber>
 </InvoiceLine>
 <InvoiceLine>
  <ServiceCode>TAX</ServiceCode>
  <ServiceName>מס</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>-207144</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>-207144</AmountForeign>
  <Wip>Y</Wip>
<LineNumber>5</LineNumber>
 </InvoiceLine>
</InvoiceLines>
<IntegratedInvoices/>
<Invoices>
 <Invoice>
  <InvoiceBillTo>אודליה</InvoiceBillTo>
  <InvoiceBillToCard>10013234</InvoiceBillToCard>
  <InvoiceDate>31.01.2020</InvoiceDate>
  <InvoiceType>Client Invoice</InvoiceType>
  <InvoiceCurrency>NIS</InvoiceCurrency>
  <InvoiceAmount>207665</InvoiceAmount>
 </Invoice>
</Invoices>
<Messages>
 <MessagesData>
  <E>חסר תאור בשורה של סעיף 56</E>
 </MessagesData>
<MessagesData>
<E>אריק בדיקה 1</E>
</MessagesData>
<MessagesData>
<W>אריק שגיאה1</W>
</MessagesData>
</Messages>
<GeneralDetails>
<Forwarder>XXX</Forwarder>
<TypeOfDelivery>2</TypeOfDelivery>
</GeneralDetails>
";
            if (!String.IsNullOrWhiteSpace(resXML))
            {
                string alexGiveBadXML = $"<AllInvoices>{resXML}</AllInvoices>";
                var StatusItemlist = LogitudeXmlSerializer.DeserializeObject<AllInvoices>(alexGiveBadXML);
                return StatusItemlist;

                // string alexGiveBadXML = $"<AllInvoices>{resXML}</AllInvoices>";
                // var StatusItemlist = LogitudeXmlSerializer.DeserializeObject<AllInvoices>(alexGiveBadXML);

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
            public List<MessagesData> Messages;
            public GeneralDetails GeneralDetails;

        }
        public class Invoice
        {
            public string InvoiceBillTo { get; set; }
            public string InvoiceType { get; set; }
            public string InvoiceDate { get; set; }
            public string InvoiceBillToCard { get; set; }
            public string InvoiceTypeCode { get; set; }
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
            public string Wip { get; set; }
            public string Currency { get; set; }
            public string LineNumber { get; set; }

        }



        public class IntegratedInvoice
        {
            public string InvoiceNumber { get; set; }
            public string ForwarderFile { get; set; }
            public string InvoiceCurrency { get; set; }
            public decimal InvoiceAmount { get; set; }
        }
        public class MessagesData
        {
            public string W { get; set; }
            public string E { get; set; }
        }

        public class GeneralDetails
        {
            public string Forwarder { get; set; }
            public string TypeOfDelivery { get; set; }

        }


    }
}
