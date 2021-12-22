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
   public partial class ExportStorageList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public string ExportFileNo  { get; set; }
       [DataMember]
       public string StorageStatus  { get; set; }
       [DataMember]
       public string CargoTypeCode  { get; set; }
       [DataMember]
       public DateTime OpenDate  { get; set; }
       [DataMember]
       public string CargoType  { get; set; }
       [DataMember]
       public string CustomsStatus  { get; set; }
       [DataMember]
       public string ExporterID  { get; set; }
       [DataMember]
       public string ShipCode  { get; set; }
       [DataMember]
       public string FirstCargoID  { get; set; }
       [DataMember]
       public string SecondCargoID  { get; set; }
       [DataMember]
       public string ThirdCargoID  { get; set; }
       [DataMember]
       public string DeclarationStatusTypeName  { get; set; }
       [DataMember]
       public string CargoTypeName  { get; set; }
       [DataMember]
       public string StorageStatusName  { get; set; }
       [DataMember]
       public string ExporterName  { get; set; }
       [DataMember]
       public string ShipName  { get; set; }
       [DataMember]
       public string StorErrorXML  { get; set; }
       [DataMember]
       public string StorageNo  { get; set; }
       [DataMember]
       public string ExportDealIdentification  { get; set; }
       [DataMember]
       public string CargoTypeCodeName  { get; set; }
       [DataMember]
       public string DeclarationStatusTypeCode  { get; set; }
   }

}
	 