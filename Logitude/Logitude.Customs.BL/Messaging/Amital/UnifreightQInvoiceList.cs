using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Customs.BL.Messaging.Amital
{
    public class UnifreightQInvoiceList
    {


        public Invoices GetInvoice()
        {

            var resXML = @"<Invoices><Statuses>
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

</Invoices>
 ";
            if (!String.IsNullOrWhiteSpace(resXML))
            {
                using (var stringReader = new System.IO.StringReader(resXML))
                {
                    var serializer = new XmlSerializer(typeof(Invoices));


                   var x=  serializer.Deserialize(stringReader) as  Invoices ;
                    return x;
                }
                

                //var response = UnifreightListsUtil.Deserialize(resXML);
                //var xmlStatusList = UnifreightListsUtil.GetHtmlDecodeValue(ref response, "InvoiceList");
                //var StatusItemlist = LogitudeXmlSerializer.DeserializeObject<List<Invoices>>(xmlStatusList);
                //return StatusItemlist;


            }


            return null;

        }

        public class Invoices
        {

            public List<StatusData> Statuses;
            public List<InvoiceLine> InvoiceLines;
            public List<IntegratedInvoice> IntegratedInvoices;


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
            public decimal AmountNIS { get; set; }
            public decimal AmountForeign { get; set; }
            public string Currency { get; set; }


        }



        public class IntegratedInvoice
        {
            public string InvoiceNumber { get; set; }
            public string ForwarderFile { get; set; }
            public string InvoiceCurrency { get; set; }
            public decimal InvoiceAmount { get; set; }




        }
    }
}
