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
   public partial class CB_PropertiesDetailsHistoryList
   {
          [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public int CustomsItemID  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? EndDate  { get; set; }
       [DataMember]
       public string EntityStatusID  { get; set; }
       [DataMember]
       public int ChangeRequestTypePriority  { get; set; }
       [DataMember]
       public bool IsCarItem  { get; set; }
       [DataMember]
       public bool IsConditionalExemptionItem  { get; set; }
       [DataMember]
       public bool IsCustomsItemDiscount  { get; set; }
       [DataMember]
       public bool IsEntitlementDiscount  { get; set; }
       [DataMember]
       public bool IsGreenIndex  { get; set; }
       [DataMember]
       public bool IsHybridCar  { get; set; }
       [DataMember]
       public bool IsImporterDiscount  { get; set; }
       [DataMember]
       public bool IsIndexedLinked  { get; set; }
       [DataMember]
       public bool IsNotAutonomiaUpdate  { get; set; }
       [DataMember]
       public bool IsRawMaterial  { get; set; }
       [DataMember]
       public bool IsWholesalePrice  { get; set; }
       [DataMember]
       public string VatDiscountReason  { get; set; }
       [DataMember]
       public int? MaxSupervisionPeriod  { get; set; }
       [DataMember]
       public string MeasurementUnitID  { get; set; }
       [DataMember]
       public string ConditionalExemptionTypeID  { get; set; }
       [DataMember]
       public string FuelTypeID  { get; set; }
       [DataMember]
       public bool IsElectronic  { get; set; }
       [DataMember]
       public string CarEngineVolumeID  { get; set; }
       [DataMember]
       public string CarWeightID  { get; set; }
       [DataMember]
       public decimal VatDiscountRate  { get; set; }
       [DataMember]
       public string Discount_CustomItemGroupTypeID  { get; set; }
       [DataMember]
       public bool IsCarDiscount  { get; set; }
       [DataMember]
       public string DiscountRegularityRequiremType  { get; set; }

       [Key]
       [DataMember]
       public string CB_ID  { get; set; }
   }

}
	 