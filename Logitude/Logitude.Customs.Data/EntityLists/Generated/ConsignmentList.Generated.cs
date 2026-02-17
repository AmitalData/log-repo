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
   public partial class ConsignmentList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int? ConsignmentNumber  { get; set; }
       [DataMember]
       public int? SequenceNumeric  { get; set; }
       [DataMember]
       public string CargoTypeCode  { get; set; }
       [DataMember]
       public string CargoTypeName  { get; set; }
       [DataMember]
       public DateTime? ManifestDate  { get; set; }
       [DataMember]
       public string ManifestNumber  { get; set; }
       [DataMember]
       public string SecondCargoID  { get; set; }
       [DataMember]
       public string ThirdCargoID  { get; set; }
       [DataMember]
       public DateTime? UnloadDate  { get; set; }
       [DataMember]
       public string UnloadPortCode  { get; set; }
       [DataMember]
       public string UnloadPortName  { get; set; }
       [DataMember]
       public string CargoDescription  { get; set; }
       [DataMember]
       public string IsLastReleaseFromWarehous  { get; set; }
       [DataMember]
       public string LoadingPortCode  { get; set; }
       [DataMember]
       public string OriginCountryCode  { get; set; }
       [DataMember]
       public string OriginCountryName  { get; set; }
       [DataMember]
       public string StorageSiteCode  { get; set; }
       [DataMember]
       public string StorageSiteName  { get; set; }
       [DataMember]
       public string ReceiverWarehouseCode  { get; set; }
       [DataMember]
       public string ReceiverWarehouseName  { get; set; }
       [DataMember]
       public string DeliveryPlaceName  { get; set; }
   }

}
	 