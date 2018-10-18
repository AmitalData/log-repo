using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.CommonIIGInterface;

namespace Logitude.CustomsMessaging.Testers.Messages
{

    [XmlRoot(Namespace = "http://amital.com/customs/Test/UniDebug01_Msg", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Test/UniDebug01_Msg")]
     public class UniDebug01_Msg : IINF_MSG_Generic
    {
         public UniDebug01_Msg()
         {

         }
         public DefaultResponseContentHeader ResponseContentHeader { get; set; }

         //public string CustomFile { get; set; }
         
         public string Remarks { get; set; }

         public bool  ToFailAnalyze{ get; set; }

         public IResponseContentHeader GetResponseContentHeader()
         {
             return this.ResponseContentHeader;
         }

         public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

         public string DeclarationID { get; set; } //DeclarationNumber

         public string BigField { get; set; }
    }
     public class DefaultResponseContentHeader:IResponseContentHeader
     {

         public int ApplicationID {get ;set;}
         

         public IException[] GetException()
         {
             var l = new List<DefaultResponseContentHeaderIException>();
             return l.ToArray();
             
         }

         public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

         public string Remark {get ;set;}
         public DateTime TransmitionDateTime {get ;set;}
     }
     class DefaultResponseContentHeaderIException : IException
     {
         public string BindingXpathField {get ;set;}
         public string EnglishDescription {get ;set;}
         public int ExceptionLevel {get ;set;}

         public string[] ExceptionParms {get ;set;}
         public string ExeptionDescription {get ;set;}

         public int ExeptionType {get ;set;}
         public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
     }
     public class ESBResponseXmlClass
     {
         public string Get(string CorrelationId, string ExternalId, string body)
         {
             var xml =
 @"
<ns0:ESBResponse xmlns:ns0=""http://MalamTeam.Inf.ESB.Schemas.ESBResponse"">
  <ns1:ResponseHeader xmlns:ns1=""http://MalamTeam.Inf.ESB.Schemas.ResponseHeader"">
    <ns1:CorrelationId>@CorrelationId@</ns1:CorrelationId>
    <ns1:Status>Success</ns1:Status>
    <ns1:ErrorDescription></ns1:ErrorDescription>
    <ns1:ErrorCode>None</ns1:ErrorCode>
    <ns1:ExternalId>@ExternalId@</ns1:ExternalId>
  </ns1:ResponseHeader>
  <Body>
@Body@
  </Body>
</ns0:ESBResponse>";
             return xml
                 .Replace("@CorrelationId@", CorrelationId)
                 .Replace("@ExternalId@", ExternalId)
                 .Replace("@Body@", body);

         }
         public bool IsValid()
         {
             return true;
         }

     }
}
