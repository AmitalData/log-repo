using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Customs.Data.EntityLists
{
   [DataContract]
   public partial class LogisticActionRequestList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public DateTime RequestDate  { get; set; }
       [DataMember]
       public string ExportFileNo  { get; set; }
       [DataMember]
       public string ExporterIdentifierType  { get; set; }
       [DataMember]
       public string ExporterNumber  { get; set; }
       [DataMember]
       public string PassportCountry  { get; set; }
       [DataMember]
       public string PassportNumber  { get; set; }
       [DataMember]
       public string RequestType  { get; set; }
       [DataMember]
       public string RequestReason  { get; set; }
       [DataMember]
       public string DeliverySiteID  { get; set; }
       [DataMember]
       public string CargoIdentifierType  { get; set; }
       [DataMember]
       public string CargoIdentifierKey1  { get; set; }
       [DataMember]
       public string CargoIdentifierKey2  { get; set; }
       [DataMember]
       public string CargoIdentifierKey3  { get; set; }
       [DataMember]
       public string PackagingTypeCode  { get; set; }
       [DataMember]
       public decimal Quantity  { get; set; }
       [DataMember]
       public string RequestNumber  { get; set; }
       [DataMember]
       public string ResponseStatusCode  { get; set; }
       [DataMember]
       public string OperationalStatus  { get; set; }
       [DataMember]
       public string Direction  { get; set; }
       [DataMember]
       public string TransportmodeId  { get; set; }
       [DataMember]
       public string DecisionRmarks  { get; set; }
       [DataMember]
       public string CustomsUserName  { get; set; }
       [DataMember]
       public double IsClosed  { get; set; }
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public string DeclarationNumber  { get; set; }
       [DataMember]
       public string RequestCancelStatus  { get; set; }
   }

}
	 