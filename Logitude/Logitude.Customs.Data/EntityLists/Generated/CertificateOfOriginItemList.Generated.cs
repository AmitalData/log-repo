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
   public partial class CertificateOfOriginItemList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string CertificateOfOriginId  { get; set; }
       [DataMember]
       public int? ItemSerial  { get; set; }
       [DataMember]
       public string ItemId  { get; set; }
       [DataMember]
       public string OriginCriterionCode  { get; set; }
       [DataMember]
       public string MarksAndNumbers  { get; set; }
       [DataMember]
       public int? PackageQuantity  { get; set; }
       [DataMember]
       public string PackageType  { get; set; }
       [DataMember]
       public string ItemDescription  { get; set; }
       [DataMember]
       public int? Weight  { get; set; }
       [DataMember]
       public string MeasureType  { get; set; }
       [DataMember]
       public string InvoiceConnect  { get; set; }
       [DataMember]
       public string ContainerIsoCode  { get; set; }
       [DataMember]
       public string PackingTypeName  { get; set; }
       [DataMember]
       public string MeasureTypeName  { get; set; }
       [DataMember]
       public string OriginCriterionCodeName  { get; set; }
   }

}
	 