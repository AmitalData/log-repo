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
  <Code>99L</Code>
  <Name>הערות פנימיות אוריין</Name>
  <Date>29.08.23</Date>
  <Time>14:57</Time>
  <Comments>בטיפול סימונה דדיה</Comments>
 </StatusData>
 <StatusData>
  <Code>TRG</Code>
  <Name>נמסר למוביל</Name>
  <Date>30.08.23</Date>
  <Time>12:00</Time>
  <Comments>ORIAN</Comments>
 </StatusData>
 <StatusData>
  <Code>POD</Code>
  <Name>נמסר לקוח</Name>
  <Date>31.08.23</Date>
  <Time>11:29</Time>
  <Comments>from=נתבג               
to=נהריה               
weight=0.02
volume=
quantity=
trans.type=
warehouse=</Comments>
 </StatusData>
</Statuses>
<InvoiceLines>
 <InvoiceLine>
  <ServiceCode>T4</ServiceCode>
  <ServiceName>מעמ רשימון</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>1440</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>1440</AmountForeign>
  <LineNumber>56</LineNumber>
  <Wip>N</Wip>
 </InvoiceLine>
 <InvoiceLine>
  <ServiceCode>T6</ServiceCode>
  <ServiceName>אגרת מחשב</ServiceName>
  <PayType>E</PayType>
  <AmountNIS>20</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>20</AmountForeign>
  <LineNumber>57</LineNumber>
  <Wip>N</Wip>
 </InvoiceLine>
 <InvoiceLine>
  <ServiceCode>T16</ServiceCode>
  <ServiceName>אגרת ביטחון</ServiceName>
  <PayType>E</PayType>
  <AmountNIS>46</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>46</AmountForeign>
  <LineNumber>58</LineNumber>
  <Wip>N</Wip>
 </InvoiceLine>
 <InvoiceLine>
  <ServiceCode>SWIS</ServiceCode>
  <ServiceName>סוויספורט</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>119.86</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>119.86</AmountForeign>
  <LineNumber>59</LineNumber>
  <Wip>N</Wip>
 </InvoiceLine>
 <InvoiceLine>
  <ServiceCode>GLX</ServiceCode>
  <ServiceName>שער עולמי</ServiceName>
  <PayType>L</PayType>
  <AmountNIS>65</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>65</AmountForeign>
  <LineNumber>51</LineNumber>
  <Wip>N</Wip>
 </InvoiceLine>
 <InvoiceLine>
  <ServiceCode>INL</ServiceCode>
  <ServiceName>הובלה יבשתית</ServiceName>
  <PayType>E</PayType>
  <AmountNIS>2306</AmountNIS>
  <Currency>NIS</Currency>
  <AmountForeign>2306</AmountForeign>
  <LineNumber>55</LineNumber>
  <Wip>N</Wip>
 </InvoiceLine>
</InvoiceLines>
<IntegratedInvoices>
 <IntegratedInvoice>
  <InvoiceNumber>632304231</InvoiceNumber>
  <ForwarderFile>A01151559</ForwarderFile>
  <InvoiceCurrency>EUR</InvoiceCurrency>
  <BillTo>Schenker Deutschland AG</BillTo>
  <InvoiceAmount>754.84</InvoiceAmount>
 </IntegratedInvoice>
</IntegratedInvoices>
<Invoices>
 <Invoice>
  <InvoiceBillTo>Agrolan Ltd</InvoiceBillTo>
  <InvoiceBillToCard>10219814</InvoiceBillToCard>
  <InvoiceDate>31.08.2023</InvoiceDate>
  <InvoiceType>Client Invoice</InvoiceType>
  <InvoiceTypeCode>L</InvoiceTypeCode>
  <InvoiceCurrency>NIS</InvoiceCurrency>
  <InvoiceAmount>1624.86</InvoiceAmount>
 </Invoice>
</Invoices>
<Messages/>
<GeneralDetails>
 <Forwarder>ORIAN</Forwarder>
 <TypeOfDelivery/>
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
            public string InvoiceAmount { get; set; }

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
            public string BillTo { get; set; }


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
