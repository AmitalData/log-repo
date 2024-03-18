using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server; 
using Logitude.Server.Tools; 
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure.DataContracts; 
using Logitude.Customs.Def.Validators;
  
namespace Logitude.Customs.Def.EntityPMs
{
   [CustomValidation(typeof(CustomsClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class CB_PropertiesDetailsHistoryPM : EntityPM
   {
   	  private string iD ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private string customsItemID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsItemID  
	   {
	    
	     get
		{
		   return customsItemID;
		 }
		 set
		 {
		   if(customsItemID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsItemID",OldValue=customsItemID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsItemID=value;
		   }
			
		 }
	   }
	  private DateTime? startDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartDate  
	   {
	    
	     get
		{
		   return startDate;
		 }
		 set
		 {
		   if(startDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDate",OldValue=startDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startDate=value;
		   }
			
		 }
	   }
	  private DateTime? endDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndDate  
	   {
	    
	     get
		{
		   return endDate;
		 }
		 set
		 {
		   if(endDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndDate",OldValue=endDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endDate=value;
		   }
			
		 }
	   }
	  private string entityStatusID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityStatusID  
	   {
	    
	     get
		{
		   return entityStatusID;
		 }
		 set
		 {
		   if(entityStatusID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityStatusID",OldValue=entityStatusID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityStatusID=value;
		   }
			
		 }
	   }
	  private int changeRequestTypePriority ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ChangeRequestTypePriority  
	   {
	    
	     get
		{
		   return changeRequestTypePriority;
		 }
		 set
		 {
		   if(changeRequestTypePriority != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChangeRequestTypePriority",OldValue=changeRequestTypePriority,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   changeRequestTypePriority=value;
		   }
			
		 }
	   }
	  private bool isCarItem ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCarItem  
	   {
	    
	     get
		{
		   return isCarItem;
		 }
		 set
		 {
		   if(isCarItem != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCarItem",OldValue=isCarItem,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCarItem=value;
		   }
			
		 }
	   }
	  private bool isConditionalExemptionItem ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConditionalExemptionItem  
	   {
	    
	     get
		{
		   return isConditionalExemptionItem;
		 }
		 set
		 {
		   if(isConditionalExemptionItem != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConditionalExemptionItem",OldValue=isConditionalExemptionItem,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConditionalExemptionItem=value;
		   }
			
		 }
	   }
	  private bool isCustomsItemDiscount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomsItemDiscount  
	   {
	    
	     get
		{
		   return isCustomsItemDiscount;
		 }
		 set
		 {
		   if(isCustomsItemDiscount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomsItemDiscount",OldValue=isCustomsItemDiscount,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomsItemDiscount=value;
		   }
			
		 }
	   }
	  private bool isEntitlementDiscount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsEntitlementDiscount  
	   {
	    
	     get
		{
		   return isEntitlementDiscount;
		 }
		 set
		 {
		   if(isEntitlementDiscount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsEntitlementDiscount",OldValue=isEntitlementDiscount,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isEntitlementDiscount=value;
		   }
			
		 }
	   }
	  private bool isGreenIndex ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsGreenIndex  
	   {
	    
	     get
		{
		   return isGreenIndex;
		 }
		 set
		 {
		   if(isGreenIndex != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsGreenIndex",OldValue=isGreenIndex,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isGreenIndex=value;
		   }
			
		 }
	   }
	  private bool isHybridCar ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsHybridCar  
	   {
	    
	     get
		{
		   return isHybridCar;
		 }
		 set
		 {
		   if(isHybridCar != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsHybridCar",OldValue=isHybridCar,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isHybridCar=value;
		   }
			
		 }
	   }
	  private bool isImporterDiscount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsImporterDiscount  
	   {
	    
	     get
		{
		   return isImporterDiscount;
		 }
		 set
		 {
		   if(isImporterDiscount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsImporterDiscount",OldValue=isImporterDiscount,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isImporterDiscount=value;
		   }
			
		 }
	   }
	  private bool isIndexedLinked ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsIndexedLinked  
	   {
	    
	     get
		{
		   return isIndexedLinked;
		 }
		 set
		 {
		   if(isIndexedLinked != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsIndexedLinked",OldValue=isIndexedLinked,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isIndexedLinked=value;
		   }
			
		 }
	   }
	  private bool isNotAutonomiaUpdate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsNotAutonomiaUpdate  
	   {
	    
	     get
		{
		   return isNotAutonomiaUpdate;
		 }
		 set
		 {
		   if(isNotAutonomiaUpdate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsNotAutonomiaUpdate",OldValue=isNotAutonomiaUpdate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isNotAutonomiaUpdate=value;
		   }
			
		 }
	   }
	  private bool isRawMaterial ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsRawMaterial  
	   {
	    
	     get
		{
		   return isRawMaterial;
		 }
		 set
		 {
		   if(isRawMaterial != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsRawMaterial",OldValue=isRawMaterial,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isRawMaterial=value;
		   }
			
		 }
	   }
	  private bool isWholesalePrice ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsWholesalePrice  
	   {
	    
	     get
		{
		   return isWholesalePrice;
		 }
		 set
		 {
		   if(isWholesalePrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsWholesalePrice",OldValue=isWholesalePrice,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isWholesalePrice=value;
		   }
			
		 }
	   }
	  private string vatDiscountReason ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VatDiscountReason  
	   {
	    
	     get
		{
		   return vatDiscountReason;
		 }
		 set
		 {
		   if(vatDiscountReason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatDiscountReason",OldValue=vatDiscountReason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vatDiscountReason=value;
		   }
			
		 }
	   }
	  private int? maxSupervisionPeriod ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? MaxSupervisionPeriod  
	   {
	    
	     get
		{
		   return maxSupervisionPeriod;
		 }
		 set
		 {
		   if(maxSupervisionPeriod != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MaxSupervisionPeriod",OldValue=maxSupervisionPeriod,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   maxSupervisionPeriod=value;
		   }
			
		 }
	   }
	  private string measurementUnitID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MeasurementUnitID  
	   {
	    
	     get
		{
		   return measurementUnitID;
		 }
		 set
		 {
		   if(measurementUnitID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeasurementUnitID",OldValue=measurementUnitID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   measurementUnitID=value;
		   }
			
		 }
	   }
	  private string conditionalExemptionTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConditionalExemptionTypeID  
	   {
	    
	     get
		{
		   return conditionalExemptionTypeID;
		 }
		 set
		 {
		   if(conditionalExemptionTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConditionalExemptionTypeID",OldValue=conditionalExemptionTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   conditionalExemptionTypeID=value;
		   }
			
		 }
	   }
	  private string fuelTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FuelTypeID  
	   {
	    
	     get
		{
		   return fuelTypeID;
		 }
		 set
		 {
		   if(fuelTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FuelTypeID",OldValue=fuelTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fuelTypeID=value;
		   }
			
		 }
	   }
	  private bool isElectronic ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsElectronic  
	   {
	    
	     get
		{
		   return isElectronic;
		 }
		 set
		 {
		   if(isElectronic != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsElectronic",OldValue=isElectronic,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isElectronic=value;
		   }
			
		 }
	   }
	  private string carEngineVolumeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarEngineVolumeID  
	   {
	    
	     get
		{
		   return carEngineVolumeID;
		 }
		 set
		 {
		   if(carEngineVolumeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarEngineVolumeID",OldValue=carEngineVolumeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carEngineVolumeID=value;
		   }
			
		 }
	   }
	  private string carWeightID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarWeightID  
	   {
	    
	     get
		{
		   return carWeightID;
		 }
		 set
		 {
		   if(carWeightID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarWeightID",OldValue=carWeightID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carWeightID=value;
		   }
			
		 }
	   }
	  private decimal vatDiscountRate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal VatDiscountRate  
	   {
	    
	     get
		{
		   return vatDiscountRate;
		 }
		 set
		 {
		   if(vatDiscountRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatDiscountRate",OldValue=vatDiscountRate,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   vatDiscountRate=value;
		   }
			
		 }
	   }
	  private string discount_CustomsItemGroupTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Discount_CustomsItemGroupTypeID  
	   {
	    
	     get
		{
		   return discount_CustomsItemGroupTypeID;
		 }
		 set
		 {
		   if(discount_CustomsItemGroupTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Discount_CustomsItemGroupTypeID",OldValue=discount_CustomsItemGroupTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   discount_CustomsItemGroupTypeID=value;
		   }
			
		 }
	   }
	  private bool isCarDiscount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCarDiscount  
	   {
	    
	     get
		{
		   return isCarDiscount;
		 }
		 set
		 {
		   if(isCarDiscount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCarDiscount",OldValue=isCarDiscount,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCarDiscount=value;
		   }
			
		 }
	   }
	  private string discountRegularityRequirementType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DiscountRegularityRequirementType  
	   {
	    
	     get
		{
		   return discountRegularityRequirementType;
		 }
		 set
		 {
		   if(discountRegularityRequirementType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DiscountRegularityRequirementType",OldValue=discountRegularityRequirementType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   discountRegularityRequirementType=value;
		   }
			
		 }
	   }
   }
   
}
	 