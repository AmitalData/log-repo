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
   public partial class ExportStorgeCargoList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string CargoTypeCode  { get; set; }
       [DataMember]
       public string Manifest  { get; set; }
       [DataMember]
       public string SecondCargoID  { get; set; }
       [DataMember]
       public string ThirdCargoID  { get; set; }
       [DataMember]
       public string CargoDescription  { get; set; }
       [DataMember]
       public string CargoType  { get; set; }
       [DataMember]
       public string HandlingCode  { get; set; }
       [DataMember]
       public decimal DangerousGoodsIndication  { get; set; }
       [DataMember]
       public decimal CodeBreaksIndication  { get; set; }
       [DataMember]
       public decimal DamageCode  { get; set; }
       [DataMember]
       public string ForeignCurrencyType  { get; set; }
       [DataMember]
       public decimal ForeignCurrencyAmoun  { get; set; }
       [DataMember]
       public decimal GoodsValueNIS  { get; set; }
       [DataMember]
       public string PackageType  { get; set; }
       [DataMember]
       public decimal Quantity  { get; set; }
       [DataMember]
       public string MarksNumbers  { get; set; }
       [DataMember]
       public decimal WeightInPortMandatory  { get; set; }
       [DataMember]
       public decimal Weight  { get; set; }
       [DataMember]
       public decimal VolumeSize  { get; set; }
       [DataMember]
       public string LicensePlateNumber  { get; set; }
       [DataMember]
       public string CustomsItem  { get; set; }
   }

}
	 