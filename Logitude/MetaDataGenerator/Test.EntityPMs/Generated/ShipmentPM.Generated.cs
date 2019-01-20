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
using Test.Validators;
  
namespace Test.EntityPMs
{
   [CustomValidation(typeof(EntityPMsClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class ShipmentPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Id  
	   {
	    
	     get
		{
		   return id;
		 }
		 set
		 {
		   if(id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Id",OldValue=id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   id=value;
		   }
			
		 }
	   }
	  private string concurrencyGUID ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConcurrencyGUID  
	   {
	    
	     get
		{
		   return concurrencyGUID;
		 }
		 set
		 {
		   if(concurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConcurrencyGUID",OldValue=concurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   concurrencyGUID=value;
		   }
			
		 }
	   }
	  private string countryForStatisticsId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CountryForStatisticsId  
	   {
	    
	     get
		{
		   return countryForStatisticsId;
		 }
		 set
		 {
		   if(countryForStatisticsId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryForStatisticsId",OldValue=countryForStatisticsId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   countryForStatisticsId=value;
		   }
			
		 }
	   }
	  private string currentUserId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrentUserId  
	   {
	    
	     get
		{
		   return currentUserId;
		 }
		 set
		 {
		   if(currentUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrentUserId",OldValue=currentUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currentUserId=value;
		   }
			
		 }
	   }
	  private int tenant ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public int Tenant  
	   {
	    
	     get
		{
		   return tenant;
		 }
		 set
		 {
		   if(tenant != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Tenant",OldValue=tenant,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   tenant=value;
		   }
			
		 }
	   }
	  private string basketId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BasketId  
	   {
	    
	     get
		{
		   return basketId;
		 }
		 set
		 {
		   if(basketId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BasketId",OldValue=basketId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   basketId=value;
		   }
			
		 }
	   }
	  private string lastStatusLogDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastStatusLogDate  
	   {
	    
	     get
		{
		   return lastStatusLogDate;
		 }
		 set
		 {
		   if(lastStatusLogDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastStatusLogDate",OldValue=lastStatusLogDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastStatusLogDate=value;
		   }
			
		 }
	   }
	  private string customConnectToShipment ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomConnectToShipment  
	   {
	    
	     get
		{
		   return customConnectToShipment;
		 }
		 set
		 {
		   if(customConnectToShipment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomConnectToShipment",OldValue=customConnectToShipment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customConnectToShipment=value;
		   }
			
		 }
	   }
	  private string customFileId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomFileId  
	   {
	    
	     get
		{
		   return customFileId;
		 }
		 set
		 {
		   if(customFileId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomFileId",OldValue=customFileId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customFileId=value;
		   }
			
		 }
	   }
	  private string lTCWEdited ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LTCWEdited  
	   {
	    
	     get
		{
		   return lTCWEdited;
		 }
		 set
		 {
		   if(lTCWEdited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LTCWEdited",OldValue=lTCWEdited,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lTCWEdited=value;
		   }
			
		 }
	   }
	  private int shipmentPickUpIndex ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ShipmentPickUpIndex  
	   {
	    
	     get
		{
		   return shipmentPickUpIndex;
		 }
		 set
		 {
		   if(shipmentPickUpIndex != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentPickUpIndex",OldValue=shipmentPickUpIndex,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   shipmentPickUpIndex=value;
		   }
			
		 }
	   }
	  private int shipmentDeliveryIndex ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ShipmentDeliveryIndex  
	   {
	    
	     get
		{
		   return shipmentDeliveryIndex;
		 }
		 set
		 {
		   if(shipmentDeliveryIndex != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentDeliveryIndex",OldValue=shipmentDeliveryIndex,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   shipmentDeliveryIndex=value;
		   }
			
		 }
	   }
	  private string quoteId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteId  
	   {
	    
	     get
		{
		   return quoteId;
		 }
		 set
		 {
		   if(quoteId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteId",OldValue=quoteId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteId=value;
		   }
			
		 }
	   }
	  private string bookingId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingId  
	   {
	    
	     get
		{
		   return bookingId;
		 }
		 set
		 {
		   if(bookingId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingId",OldValue=bookingId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingId=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId  
	   {
	    
	     get
		{
		   return updatedByUserId;
		 }
		 set
		 {
		   if(updatedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserId",OldValue=updatedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserId=value;
		   }
			
		 }
	   }
	  private DateTime lastUpdateDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime LastUpdateDate  
	   {
	    
	     get
		{
		   return lastUpdateDate;
		 }
		 set
		 {
		   if(lastUpdateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastUpdateDate",OldValue=lastUpdateDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   lastUpdateDate=value;
		   }
			
		 }
	   }
	  private string profitCurrencyId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProfitCurrencyId  
	   {
	    
	     get
		{
		   return profitCurrencyId;
		 }
		 set
		 {
		   if(profitCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProfitCurrencyId",OldValue=profitCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   profitCurrencyId=value;
		   }
			
		 }
	   }
	  private string nextLegCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NextLegCode  
	   {
	    
	     get
		{
		   return nextLegCode;
		 }
		 set
		 {
		   if(nextLegCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextLegCode",OldValue=nextLegCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nextLegCode=value;
		   }
			
		 }
	   }
	  private string masterShipmentDataId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MasterShipmentDataId  
	   {
	    
	     get
		{
		   return masterShipmentDataId;
		 }
		 set
		 {
		   if(masterShipmentDataId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MasterShipmentDataId",OldValue=masterShipmentDataId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   masterShipmentDataId=value;
		   }
			
		 }
	   }
	  private string field1 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field1  
	   {
	    
	     get
		{
		   return field1;
		 }
		 set
		 {
		   if(field1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field1",OldValue=field1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field1=value;
		   }
			
		 }
	   }
	  private string field2 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field2  
	   {
	    
	     get
		{
		   return field2;
		 }
		 set
		 {
		   if(field2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field2",OldValue=field2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field2=value;
		   }
			
		 }
	   }
	  private string field3 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field3  
	   {
	    
	     get
		{
		   return field3;
		 }
		 set
		 {
		   if(field3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field3",OldValue=field3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field3=value;
		   }
			
		 }
	   }
	  private string field4 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field4  
	   {
	    
	     get
		{
		   return field4;
		 }
		 set
		 {
		   if(field4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field4",OldValue=field4,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field4=value;
		   }
			
		 }
	   }
	  private string field5 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field5  
	   {
	    
	     get
		{
		   return field5;
		 }
		 set
		 {
		   if(field5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field5",OldValue=field5,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field5=value;
		   }
			
		 }
	   }
	  private string field6 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field6  
	   {
	    
	     get
		{
		   return field6;
		 }
		 set
		 {
		   if(field6 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field6",OldValue=field6,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field6=value;
		   }
			
		 }
	   }
	  private string field7 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field7  
	   {
	    
	     get
		{
		   return field7;
		 }
		 set
		 {
		   if(field7 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field7",OldValue=field7,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field7=value;
		   }
			
		 }
	   }
	  private string field8 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field8  
	   {
	    
	     get
		{
		   return field8;
		 }
		 set
		 {
		   if(field8 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field8",OldValue=field8,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field8=value;
		   }
			
		 }
	   }
	  private string field9 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field9  
	   {
	    
	     get
		{
		   return field9;
		 }
		 set
		 {
		   if(field9 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field9",OldValue=field9,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field9=value;
		   }
			
		 }
	   }
	  private string field10 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field10  
	   {
	    
	     get
		{
		   return field10;
		 }
		 set
		 {
		   if(field10 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field10",OldValue=field10,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field10=value;
		   }
			
		 }
	   }
	  private string isFSRSent ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsFSRSent  
	   {
	    
	     get
		{
		   return isFSRSent;
		 }
		 set
		 {
		   if(isFSRSent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFSRSent",OldValue=isFSRSent,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isFSRSent=value;
		   }
			
		 }
	   }
	  private string exceptionDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExceptionDate  
	   {
	    
	     get
		{
		   return exceptionDate;
		 }
		 set
		 {
		   if(exceptionDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExceptionDate",OldValue=exceptionDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exceptionDate=value;
		   }
			
		 }
	   }
	  private string computedStatusId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ComputedStatusId  
	   {
	    
	     get
		{
		   return computedStatusId;
		 }
		 set
		 {
		   if(computedStatusId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ComputedStatusId",OldValue=computedStatusId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   computedStatusId=value;
		   }
			
		 }
	   }
	  private string computedStatusDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ComputedStatusDate  
	   {
	    
	     get
		{
		   return computedStatusDate;
		 }
		 set
		 {
		   if(computedStatusDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ComputedStatusDate",OldValue=computedStatusDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   computedStatusDate=value;
		   }
			
		 }
	   }
	  private string foreignPartnerCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForeignPartnerCountryCode  
	   {
	    
	     get
		{
		   return foreignPartnerCountryCode;
		 }
		 set
		 {
		   if(foreignPartnerCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignPartnerCountryCode",OldValue=foreignPartnerCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   foreignPartnerCountryCode=value;
		   }
			
		 }
	   }
	  private string consigneeAddressOneTime ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeAddressOneTime  
	   {
	    
	     get
		{
		   return consigneeAddressOneTime;
		 }
		 set
		 {
		   if(consigneeAddressOneTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeAddressOneTime",OldValue=consigneeAddressOneTime,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeAddressOneTime=value;
		   }
			
		 }
	   }
	  private string shipperAddressOneTime ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperAddressOneTime  
	   {
	    
	     get
		{
		   return shipperAddressOneTime;
		 }
		 set
		 {
		   if(shipperAddressOneTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperAddressOneTime",OldValue=shipperAddressOneTime,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperAddressOneTime=value;
		   }
			
		 }
	   }
	  private string freelancerId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreelancerId  
	   {
	    
	     get
		{
		   return freelancerId;
		 }
		 set
		 {
		   if(freelancerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreelancerId",OldValue=freelancerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freelancerId=value;
		   }
			
		 }
	   }
	  private string freelancerAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreelancerAddressId  
	   {
	    
	     get
		{
		   return freelancerAddressId;
		 }
		 set
		 {
		   if(freelancerAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreelancerAddressId",OldValue=freelancerAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freelancerAddressId=value;
		   }
			
		 }
	   }
	  private string freelancerContactId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreelancerContactId  
	   {
	    
	     get
		{
		   return freelancerContactId;
		 }
		 set
		 {
		   if(freelancerContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreelancerContactId",OldValue=freelancerContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freelancerContactId=value;
		   }
			
		 }
	   }
	  private string fromPortId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortId  
	   {
	    
	     get
		{
		   return fromPortId;
		 }
		 set
		 {
		   if(fromPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortId",OldValue=fromPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortId=value;
		   }
			
		 }
	   }
	  private string toPortId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortId  
	   {
	    
	     get
		{
		   return toPortId;
		 }
		 set
		 {
		   if(toPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortId",OldValue=toPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortId=value;
		   }
			
		 }
	   }
	  private string noFreightFile ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NoFreightFile  
	   {
	    
	     get
		{
		   return noFreightFile;
		 }
		 set
		 {
		   if(noFreightFile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NoFreightFile",OldValue=noFreightFile,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   noFreightFile=value;
		   }
			
		 }
	   }
	  private string forwarderShipmentNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForwarderShipmentNumber  
	   {
	    
	     get
		{
		   return forwarderShipmentNumber;
		 }
		 set
		 {
		   if(forwarderShipmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForwarderShipmentNumber",OldValue=forwarderShipmentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   forwarderShipmentNumber=value;
		   }
			
		 }
	   }
	  private string customerShipmentNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerShipmentNumber  
	   {
	    
	     get
		{
		   return customerShipmentNumber;
		 }
		 set
		 {
		   if(customerShipmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerShipmentNumber",OldValue=customerShipmentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerShipmentNumber=value;
		   }
			
		 }
	   }
	  private string forwarderPartnerId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForwarderPartnerId  
	   {
	    
	     get
		{
		   return forwarderPartnerId;
		 }
		 set
		 {
		   if(forwarderPartnerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForwarderPartnerId",OldValue=forwarderPartnerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   forwarderPartnerId=value;
		   }
			
		 }
	   }
	  private string statusId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusId  
	   {
	    
	     get
		{
		   return statusId;
		 }
		 set
		 {
		   if(statusId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusId",OldValue=statusId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusId=value;
		   }
			
		 }
	   }
	  private string statusName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusName  
	   {
	    
	     get
		{
		   return statusName;
		 }
		 set
		 {
		   if(statusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusName",OldValue=statusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusName=value;
		   }
			
		 }
	   }
	  private string shipmentNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentNumber  
	   {
	    
	     get
		{
		   return shipmentNumber;
		 }
		 set
		 {
		   if(shipmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentNumber",OldValue=shipmentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentNumber=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SearchFields  
	   {
	    
	     get
		{
		   return searchFields;
		 }
		 set
		 {
		   if(searchFields != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SearchFields",OldValue=searchFields,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   searchFields=value;
		   }
			
		 }
	   }
	  private string branchName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BranchName  
	   {
	    
	     get
		{
		   return branchName;
		 }
		 set
		 {
		   if(branchName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BranchName",OldValue=branchName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   branchName=value;
		   }
			
		 }
	   }
	  private string shipperReference1 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperReference1  
	   {
	    
	     get
		{
		   return shipperReference1;
		 }
		 set
		 {
		   if(shipperReference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperReference1",OldValue=shipperReference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperReference1=value;
		   }
			
		 }
	   }
	  private string shipperReference2 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperReference2  
	   {
	    
	     get
		{
		   return shipperReference2;
		 }
		 set
		 {
		   if(shipperReference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperReference2",OldValue=shipperReference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperReference2=value;
		   }
			
		 }
	   }
	  private string consigneeReference2 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeReference2  
	   {
	    
	     get
		{
		   return consigneeReference2;
		 }
		 set
		 {
		   if(consigneeReference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeReference2",OldValue=consigneeReference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeReference2=value;
		   }
			
		 }
	   }
	  private string consigneeReference1 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeReference1  
	   {
	    
	     get
		{
		   return consigneeReference1;
		 }
		 set
		 {
		   if(consigneeReference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeReference1",OldValue=consigneeReference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeReference1=value;
		   }
			
		 }
	   }
	  private string house ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string House  
	   {
	    
	     get
		{
		   return house;
		 }
		 set
		 {
		   if(house != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="House",OldValue=house,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   house=value;
		   }
			
		 }
	   }
	  private string branchId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BranchId  
	   {
	    
	     get
		{
		   return branchId;
		 }
		 set
		 {
		   if(branchId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BranchId",OldValue=branchId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   branchId=value;
		   }
			
		 }
	   }
	  private string incotermId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IncotermId  
	   {
	    
	     get
		{
		   return incotermId;
		 }
		 set
		 {
		   if(incotermId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncotermId",OldValue=incotermId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   incotermId=value;
		   }
			
		 }
	   }
	  private string salesmanUserId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SalesmanUserId  
	   {
	    
	     get
		{
		   return salesmanUserId;
		 }
		 set
		 {
		   if(salesmanUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SalesmanUserId",OldValue=salesmanUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   salesmanUserId=value;
		   }
			
		 }
	   }
	  private string createDateTime ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreateDateTime  
	   {
	    
	     get
		{
		   return createDateTime;
		 }
		 set
		 {
		   if(createDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDateTime",OldValue=createDateTime,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createDateTime=value;
		   }
			
		 }
	   }
	  private string departmentId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepartmentId  
	   {
	    
	     get
		{
		   return departmentId;
		 }
		 set
		 {
		   if(departmentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartmentId",OldValue=departmentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   departmentId=value;
		   }
			
		 }
	   }
	  private string customerId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerId  
	   {
	    
	     get
		{
		   return customerId;
		 }
		 set
		 {
		   if(customerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerId",OldValue=customerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerId=value;
		   }
			
		 }
	   }
	  private string customerName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerName  
	   {
	    
	     get
		{
		   return customerName;
		 }
		 set
		 {
		   if(customerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerName",OldValue=customerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerName=value;
		   }
			
		 }
	   }
	  private string shipperId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperId  
	   {
	    
	     get
		{
		   return shipperId;
		 }
		 set
		 {
		   if(shipperId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperId",OldValue=shipperId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperId=value;
		   }
			
		 }
	   }
	  private string shipper ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Shipper  
	   {
	    
	     get
		{
		   return shipper;
		 }
		 set
		 {
		   if(shipper != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Shipper",OldValue=shipper,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipper=value;
		   }
			
		 }
	   }
	  private string consigneeId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeId  
	   {
	    
	     get
		{
		   return consigneeId;
		 }
		 set
		 {
		   if(consigneeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeId",OldValue=consigneeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeId=value;
		   }
			
		 }
	   }
	  private string consignee ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Consignee  
	   {
	    
	     get
		{
		   return consignee;
		 }
		 set
		 {
		   if(consignee != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Consignee",OldValue=consignee,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consignee=value;
		   }
			
		 }
	   }
	  private string agentId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentId  
	   {
	    
	     get
		{
		   return agentId;
		 }
		 set
		 {
		   if(agentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentId",OldValue=agentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentId=value;
		   }
			
		 }
	   }
	  private string routing ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Routing  
	   {
	    
	     get
		{
		   return routing;
		 }
		 set
		 {
		   if(routing != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Routing",OldValue=routing,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   routing=value;
		   }
			
		 }
	   }
	  private string incotermCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IncotermCode  
	   {
	    
	     get
		{
		   return incotermCode;
		 }
		 set
		 {
		   if(incotermCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncotermCode",OldValue=incotermCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   incotermCode=value;
		   }
			
		 }
	   }
	  private string shipperName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperName  
	   {
	    
	     get
		{
		   return shipperName;
		 }
		 set
		 {
		   if(shipperName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperName",OldValue=shipperName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperName=value;
		   }
			
		 }
	   }
	  private string consigneeName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeName  
	   {
	    
	     get
		{
		   return consigneeName;
		 }
		 set
		 {
		   if(consigneeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeName",OldValue=consigneeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeName=value;
		   }
			
		 }
	   }
	  private string freightForwarderId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreightForwarderId  
	   {
	    
	     get
		{
		   return freightForwarderId;
		 }
		 set
		 {
		   if(freightForwarderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreightForwarderId",OldValue=freightForwarderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freightForwarderId=value;
		   }
			
		 }
	   }
	  private string directionId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DirectionId  
	   {
	    
	     get
		{
		   return directionId;
		 }
		 set
		 {
		   if(directionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DirectionId",OldValue=directionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   directionId=value;
		   }
			
		 }
	   }
	  private string mainCarriageFromPortId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFromPortId  
	   {
	    
	     get
		{
		   return mainCarriageFromPortId;
		 }
		 set
		 {
		   if(mainCarriageFromPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFromPortId",OldValue=mainCarriageFromPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFromPortId=value;
		   }
			
		 }
	   }
	  private string transportModeId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeId  
	   {
	    
	     get
		{
		   return transportModeId;
		 }
		 set
		 {
		   if(transportModeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeId",OldValue=transportModeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeId=value;
		   }
			
		 }
	   }
	  private string preCarriageETD ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageETD  
	   {
	    
	     get
		{
		   return preCarriageETD;
		 }
		 set
		 {
		   if(preCarriageETD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageETD",OldValue=preCarriageETD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageETD=value;
		   }
			
		 }
	   }
	  private string mainCarriageETA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageETA  
	   {
	    
	     get
		{
		   return mainCarriageETA;
		 }
		 set
		 {
		   if(mainCarriageETA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageETA",OldValue=mainCarriageETA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageETA=value;
		   }
			
		 }
	   }
	  private string master ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Master  
	   {
	    
	     get
		{
		   return master;
		 }
		 set
		 {
		   if(master != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Master",OldValue=master,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   master=value;
		   }
			
		 }
	   }
	  private string longMaster ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LongMaster  
	   {
	    
	     get
		{
		   return longMaster;
		 }
		 set
		 {
		   if(longMaster != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LongMaster",OldValue=longMaster,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   longMaster=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierId  
	   {
	    
	     get
		{
		   return mainCarriageCarrierId;
		 }
		 set
		 {
		   if(mainCarriageCarrierId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierId",OldValue=mainCarriageCarrierId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierId=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierNumber  
	   {
	    
	     get
		{
		   return mainCarriageCarrierNumber;
		 }
		 set
		 {
		   if(mainCarriageCarrierNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierNumber",OldValue=mainCarriageCarrierNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierNumber=value;
		   }
			
		 }
	   }
	  private string mainCarriageETD ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageETD  
	   {
	    
	     get
		{
		   return mainCarriageETD;
		 }
		 set
		 {
		   if(mainCarriageETD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageETD",OldValue=mainCarriageETD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageETD=value;
		   }
			
		 }
	   }
	  private string mainCarriageATA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageATA  
	   {
	    
	     get
		{
		   return mainCarriageATA;
		 }
		 set
		 {
		   if(mainCarriageATA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageATA",OldValue=mainCarriageATA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageATA=value;
		   }
			
		 }
	   }
	  private string mainCarriageATD ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageATD  
	   {
	    
	     get
		{
		   return mainCarriageATD;
		 }
		 set
		 {
		   if(mainCarriageATD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageATD",OldValue=mainCarriageATD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageATD=value;
		   }
			
		 }
	   }
	  private string fromPort ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPort  
	   {
	    
	     get
		{
		   return fromPort;
		 }
		 set
		 {
		   if(fromPort != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPort",OldValue=fromPort,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPort=value;
		   }
			
		 }
	   }
	  private string toPort ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPort  
	   {
	    
	     get
		{
		   return toPort;
		 }
		 set
		 {
		   if(toPort != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPort",OldValue=toPort,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPort=value;
		   }
			
		 }
	   }
	  private string shipmentType ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentType  
	   {
	    
	     get
		{
		   return shipmentType;
		 }
		 set
		 {
		   if(shipmentType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentType",OldValue=shipmentType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentType=value;
		   }
			
		 }
	   }
	  private string followUpType ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FollowUpType  
	   {
	    
	     get
		{
		   return followUpType;
		 }
		 set
		 {
		   if(followUpType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowUpType",OldValue=followUpType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   followUpType=value;
		   }
			
		 }
	   }
	  private string followUpTypeId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FollowUpTypeId  
	   {
	    
	     get
		{
		   return followUpTypeId;
		 }
		 set
		 {
		   if(followUpTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowUpTypeId",OldValue=followUpTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   followUpTypeId=value;
		   }
			
		 }
	   }
	  private string followUpDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FollowUpDate  
	   {
	    
	     get
		{
		   return followUpDate;
		 }
		 set
		 {
		   if(followUpDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowUpDate",OldValue=followUpDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   followUpDate=value;
		   }
			
		 }
	   }
	  private string followUpNotes ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FollowUpNotes  
	   {
	    
	     get
		{
		   return followUpNotes;
		 }
		 set
		 {
		   if(followUpNotes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowUpNotes",OldValue=followUpNotes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   followUpNotes=value;
		   }
			
		 }
	   }
	  private string followUpOwner ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FollowUpOwner  
	   {
	    
	     get
		{
		   return followUpOwner;
		 }
		 set
		 {
		   if(followUpOwner != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowUpOwner",OldValue=followUpOwner,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   followUpOwner=value;
		   }
			
		 }
	   }
	  private string followUpOwnerId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FollowUpOwnerId  
	   {
	    
	     get
		{
		   return followUpOwnerId;
		 }
		 set
		 {
		   if(followUpOwnerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowUpOwnerId",OldValue=followUpOwnerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   followUpOwnerId=value;
		   }
			
		 }
	   }
	  private string agentReference2 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentReference2  
	   {
	    
	     get
		{
		   return agentReference2;
		 }
		 set
		 {
		   if(agentReference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentReference2",OldValue=agentReference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentReference2=value;
		   }
			
		 }
	   }
	  private string agentReference1 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentReference1  
	   {
	    
	     get
		{
		   return agentReference1;
		 }
		 set
		 {
		   if(agentReference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentReference1",OldValue=agentReference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentReference1=value;
		   }
			
		 }
	   }
	  private string openPayablesInLocalCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpenPayablesInLocalCurrency  
	   {
	    
	     get
		{
		   return openPayablesInLocalCurrency;
		 }
		 set
		 {
		   if(openPayablesInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenPayablesInLocalCurrency",OldValue=openPayablesInLocalCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   openPayablesInLocalCurrency=value;
		   }
			
		 }
	   }
	  private string openPayablesInProfitCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpenPayablesInProfitCurrency  
	   {
	    
	     get
		{
		   return openPayablesInProfitCurrency;
		 }
		 set
		 {
		   if(openPayablesInProfitCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenPayablesInProfitCurrency",OldValue=openPayablesInProfitCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   openPayablesInProfitCurrency=value;
		   }
			
		 }
	   }
	  private string accountedPayablesInLocalCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountedPayablesInLocalCurrency  
	   {
	    
	     get
		{
		   return accountedPayablesInLocalCurrency;
		 }
		 set
		 {
		   if(accountedPayablesInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountedPayablesInLocalCurrency",OldValue=accountedPayablesInLocalCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountedPayablesInLocalCurrency=value;
		   }
			
		 }
	   }
	  private string accountedPayablesInProfitCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountedPayablesInProfitCurrency  
	   {
	    
	     get
		{
		   return accountedPayablesInProfitCurrency;
		 }
		 set
		 {
		   if(accountedPayablesInProfitCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountedPayablesInProfitCurrency",OldValue=accountedPayablesInProfitCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountedPayablesInProfitCurrency=value;
		   }
			
		 }
	   }
	  private string orderVolumetricWeight ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OrderVolumetricWeight  
	   {
	    
	     get
		{
		   return orderVolumetricWeight;
		 }
		 set
		 {
		   if(orderVolumetricWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OrderVolumetricWeight",OldValue=orderVolumetricWeight,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   orderVolumetricWeight=value;
		   }
			
		 }
	   }
	  private string chargeableWeightUnitCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargeableWeightUnitCode  
	   {
	    
	     get
		{
		   return chargeableWeightUnitCode;
		 }
		 set
		 {
		   if(chargeableWeightUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeightUnitCode",OldValue=chargeableWeightUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargeableWeightUnitCode=value;
		   }
			
		 }
	   }
	  private string volumetricWeight ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VolumetricWeight  
	   {
	    
	     get
		{
		   return volumetricWeight;
		 }
		 set
		 {
		   if(volumetricWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumetricWeight",OldValue=volumetricWeight,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   volumetricWeight=value;
		   }
			
		 }
	   }
	  private string grossWeightInKG ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string GrossWeightInKG  
	   {
	    
	     get
		{
		   return grossWeightInKG;
		 }
		 set
		 {
		   if(grossWeightInKG != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeightInKG",OldValue=grossWeightInKG,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   grossWeightInKG=value;
		   }
			
		 }
	   }
	  private string chargeableWeightInKG ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargeableWeightInKG  
	   {
	    
	     get
		{
		   return chargeableWeightInKG;
		 }
		 set
		 {
		   if(chargeableWeightInKG != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeightInKG",OldValue=chargeableWeightInKG,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargeableWeightInKG=value;
		   }
			
		 }
	   }
	  private string cutoffDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CutoffDate  
	   {
	    
	     get
		{
		   return cutoffDate;
		 }
		 set
		 {
		   if(cutoffDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CutoffDate",OldValue=cutoffDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cutoffDate=value;
		   }
			
		 }
	   }
	  private string openReceivablesInProfitCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpenReceivablesInProfitCurrency  
	   {
	    
	     get
		{
		   return openReceivablesInProfitCurrency;
		 }
		 set
		 {
		   if(openReceivablesInProfitCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenReceivablesInProfitCurrency",OldValue=openReceivablesInProfitCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   openReceivablesInProfitCurrency=value;
		   }
			
		 }
	   }
	  private string accountedReceivablesInProfitCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountedReceivablesInProfitCurrency  
	   {
	    
	     get
		{
		   return accountedReceivablesInProfitCurrency;
		 }
		 set
		 {
		   if(accountedReceivablesInProfitCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountedReceivablesInProfitCurrency",OldValue=accountedReceivablesInProfitCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountedReceivablesInProfitCurrency=value;
		   }
			
		 }
	   }
	  private string issuingCarrierAgentId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IssuingCarrierAgentId  
	   {
	    
	     get
		{
		   return issuingCarrierAgentId;
		 }
		 set
		 {
		   if(issuingCarrierAgentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssuingCarrierAgentId",OldValue=issuingCarrierAgentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   issuingCarrierAgentId=value;
		   }
			
		 }
	   }
	  private string aWBChargeAmount ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBChargeAmount  
	   {
	    
	     get
		{
		   return aWBChargeAmount;
		 }
		 set
		 {
		   if(aWBChargeAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBChargeAmount",OldValue=aWBChargeAmount,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBChargeAmount=value;
		   }
			
		 }
	   }
	  private string aWBCommodityItemNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBCommodityItemNumber  
	   {
	    
	     get
		{
		   return aWBCommodityItemNumber;
		 }
		 set
		 {
		   if(aWBCommodityItemNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBCommodityItemNumber",OldValue=aWBCommodityItemNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBCommodityItemNumber=value;
		   }
			
		 }
	   }
	  private string mainCarriageFromPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFromPortName  
	   {
	    
	     get
		{
		   return mainCarriageFromPortName;
		 }
		 set
		 {
		   if(mainCarriageFromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFromPortName",OldValue=mainCarriageFromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFromPortName=value;
		   }
			
		 }
	   }
	  private string estimateProfitInProfitCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EstimateProfitInProfitCurrency  
	   {
	    
	     get
		{
		   return estimateProfitInProfitCurrency;
		 }
		 set
		 {
		   if(estimateProfitInProfitCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimateProfitInProfitCurrency",OldValue=estimateProfitInProfitCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   estimateProfitInProfitCurrency=value;
		   }
			
		 }
	   }
	  private string profitInProfitCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProfitInProfitCurrency  
	   {
	    
	     get
		{
		   return profitInProfitCurrency;
		 }
		 set
		 {
		   if(profitInProfitCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProfitInProfitCurrency",OldValue=profitInProfitCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   profitInProfitCurrency=value;
		   }
			
		 }
	   }
	  private string fWBStatusCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FWBStatusCode  
	   {
	    
	     get
		{
		   return fWBStatusCode;
		 }
		 set
		 {
		   if(fWBStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FWBStatusCode",OldValue=fWBStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fWBStatusCode=value;
		   }
			
		 }
	   }
	  private string fHLStatusCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FHLStatusCode  
	   {
	    
	     get
		{
		   return fHLStatusCode;
		 }
		 set
		 {
		   if(fHLStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FHLStatusCode",OldValue=fHLStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fHLStatusCode=value;
		   }
			
		 }
	   }
	  private string mainCarriageFromPartnerId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFromPartnerId  
	   {
	    
	     get
		{
		   return mainCarriageFromPartnerId;
		 }
		 set
		 {
		   if(mainCarriageFromPartnerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFromPartnerId",OldValue=mainCarriageFromPartnerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFromPartnerId=value;
		   }
			
		 }
	   }
	  private string mainCarriageToPartnerId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageToPartnerId  
	   {
	    
	     get
		{
		   return mainCarriageToPartnerId;
		 }
		 set
		 {
		   if(mainCarriageToPartnerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageToPartnerId",OldValue=mainCarriageToPartnerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageToPartnerId=value;
		   }
			
		 }
	   }
	  private string mainCarriageFromAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFromAddressId  
	   {
	    
	     get
		{
		   return mainCarriageFromAddressId;
		 }
		 set
		 {
		   if(mainCarriageFromAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFromAddressId",OldValue=mainCarriageFromAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFromAddressId=value;
		   }
			
		 }
	   }
	  private string mainCarriageToAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageToAddressId  
	   {
	    
	     get
		{
		   return mainCarriageToAddressId;
		 }
		 set
		 {
		   if(mainCarriageToAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageToAddressId",OldValue=mainCarriageToAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageToAddressId=value;
		   }
			
		 }
	   }
	  private string truckNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TruckNumber  
	   {
	    
	     get
		{
		   return truckNumber;
		 }
		 set
		 {
		   if(truckNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TruckNumber",OldValue=truckNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   truckNumber=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierPrefix ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierPrefix  
	   {
	    
	     get
		{
		   return mainCarriageCarrierPrefix;
		 }
		 set
		 {
		   if(mainCarriageCarrierPrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierPrefix",OldValue=mainCarriageCarrierPrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierPrefix=value;
		   }
			
		 }
	   }
	  private string transshipment1CarrierPrefix ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1CarrierPrefix  
	   {
	    
	     get
		{
		   return transshipment1CarrierPrefix;
		 }
		 set
		 {
		   if(transshipment1CarrierPrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1CarrierPrefix",OldValue=transshipment1CarrierPrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1CarrierPrefix=value;
		   }
			
		 }
	   }
	  private string transshipment2CarrierPrefix ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2CarrierPrefix  
	   {
	    
	     get
		{
		   return transshipment2CarrierPrefix;
		 }
		 set
		 {
		   if(transshipment2CarrierPrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2CarrierPrefix",OldValue=transshipment2CarrierPrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2CarrierPrefix=value;
		   }
			
		 }
	   }
	  private string transshipment3CarrierPrefix ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3CarrierPrefix  
	   {
	    
	     get
		{
		   return transshipment3CarrierPrefix;
		 }
		 set
		 {
		   if(transshipment3CarrierPrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3CarrierPrefix",OldValue=transshipment3CarrierPrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3CarrierPrefix=value;
		   }
			
		 }
	   }
	  private string fWBStatusName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FWBStatusName  
	   {
	    
	     get
		{
		   return fWBStatusName;
		 }
		 set
		 {
		   if(fWBStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FWBStatusName",OldValue=fWBStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fWBStatusName=value;
		   }
			
		 }
	   }
	  private string fHLStatusName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FHLStatusName  
	   {
	    
	     get
		{
		   return fHLStatusName;
		 }
		 set
		 {
		   if(fHLStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FHLStatusName",OldValue=fHLStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fHLStatusName=value;
		   }
			
		 }
	   }
	  private string aWBPrint ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBPrint  
	   {
	    
	     get
		{
		   return aWBPrint;
		 }
		 set
		 {
		   if(aWBPrint != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBPrint",OldValue=aWBPrint,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBPrint=value;
		   }
			
		 }
	   }
	  private string mainCarriageFullCarrierNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFullCarrierNumber  
	   {
	    
	     get
		{
		   return mainCarriageFullCarrierNumber;
		 }
		 set
		 {
		   if(mainCarriageFullCarrierNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFullCarrierNumber",OldValue=mainCarriageFullCarrierNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFullCarrierNumber=value;
		   }
			
		 }
	   }
	  private string fNAReason ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FNAReason  
	   {
	    
	     get
		{
		   return fNAReason;
		 }
		 set
		 {
		   if(fNAReason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FNAReason",OldValue=fNAReason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fNAReason=value;
		   }
			
		 }
	   }
	  private string carrierLastStatusCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierLastStatusCode  
	   {
	    
	     get
		{
		   return carrierLastStatusCode;
		 }
		 set
		 {
		   if(carrierLastStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierLastStatusCode",OldValue=carrierLastStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierLastStatusCode=value;
		   }
			
		 }
	   }
	  private string carrierLastStatusName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierLastStatusName  
	   {
	    
	     get
		{
		   return carrierLastStatusName;
		 }
		 set
		 {
		   if(carrierLastStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierLastStatusName",OldValue=carrierLastStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierLastStatusName=value;
		   }
			
		 }
	   }
	  private string carrierLastStatusDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierLastStatusDate  
	   {
	    
	     get
		{
		   return carrierLastStatusDate;
		 }
		 set
		 {
		   if(carrierLastStatusDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierLastStatusDate",OldValue=carrierLastStatusDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierLastStatusDate=value;
		   }
			
		 }
	   }
	  private string transshipment1FullCarrierNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1FullCarrierNumber  
	   {
	    
	     get
		{
		   return transshipment1FullCarrierNumber;
		 }
		 set
		 {
		   if(transshipment1FullCarrierNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1FullCarrierNumber",OldValue=transshipment1FullCarrierNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1FullCarrierNumber=value;
		   }
			
		 }
	   }
	  private string transshipment2FullCarrierNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2FullCarrierNumber  
	   {
	    
	     get
		{
		   return transshipment2FullCarrierNumber;
		 }
		 set
		 {
		   if(transshipment2FullCarrierNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2FullCarrierNumber",OldValue=transshipment2FullCarrierNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2FullCarrierNumber=value;
		   }
			
		 }
	   }
	  private string transshipment3FullCarrierNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3FullCarrierNumber  
	   {
	    
	     get
		{
		   return transshipment3FullCarrierNumber;
		 }
		 set
		 {
		   if(transshipment3FullCarrierNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3FullCarrierNumber",OldValue=transshipment3FullCarrierNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3FullCarrierNumber=value;
		   }
			
		 }
	   }
	  private string mainCarriageFinalDestinationETA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFinalDestinationETA  
	   {
	    
	     get
		{
		   return mainCarriageFinalDestinationETA;
		 }
		 set
		 {
		   if(mainCarriageFinalDestinationETA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFinalDestinationETA",OldValue=mainCarriageFinalDestinationETA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFinalDestinationETA=value;
		   }
			
		 }
	   }
	  private string mainCarriageFinalDestinationATA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFinalDestinationATA  
	   {
	    
	     get
		{
		   return mainCarriageFinalDestinationATA;
		 }
		 set
		 {
		   if(mainCarriageFinalDestinationATA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFinalDestinationATA",OldValue=mainCarriageFinalDestinationATA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFinalDestinationATA=value;
		   }
			
		 }
	   }
	  private string fHLStatusDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FHLStatusDate  
	   {
	    
	     get
		{
		   return fHLStatusDate;
		 }
		 set
		 {
		   if(fHLStatusDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FHLStatusDate",OldValue=fHLStatusDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fHLStatusDate=value;
		   }
			
		 }
	   }
	  private string fWBStatusDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FWBStatusDate  
	   {
	    
	     get
		{
		   return fWBStatusDate;
		 }
		 set
		 {
		   if(fWBStatusDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FWBStatusDate",OldValue=fWBStatusDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fWBStatusDate=value;
		   }
			
		 }
	   }
	  private string agentName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentName  
	   {
	    
	     get
		{
		   return agentName;
		 }
		 set
		 {
		   if(agentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentName",OldValue=agentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentName=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierName  
	   {
	    
	     get
		{
		   return mainCarriageCarrierName;
		 }
		 set
		 {
		   if(mainCarriageCarrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierName",OldValue=mainCarriageCarrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierName=value;
		   }
			
		 }
	   }
	  private string profitExchangeRate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProfitExchangeRate  
	   {
	    
	     get
		{
		   return profitExchangeRate;
		 }
		 set
		 {
		   if(profitExchangeRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProfitExchangeRate",OldValue=profitExchangeRate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   profitExchangeRate=value;
		   }
			
		 }
	   }
	  private string aMSBL ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AMSBL  
	   {
	    
	     get
		{
		   return aMSBL;
		 }
		 set
		 {
		   if(aMSBL != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AMSBL",OldValue=aMSBL,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aMSBL=value;
		   }
			
		 }
	   }
	  private string statusDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusDate  
	   {
	    
	     get
		{
		   return statusDate;
		 }
		 set
		 {
		   if(statusDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusDate",OldValue=statusDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusDate=value;
		   }
			
		 }
	   }
	  private string grossWeight ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string GrossWeight  
	   {
	    
	     get
		{
		   return grossWeight;
		 }
		 set
		 {
		   if(grossWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeight",OldValue=grossWeight,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   grossWeight=value;
		   }
			
		 }
	   }
	  private string numberOfPackages ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NumberOfPackages  
	   {
	    
	     get
		{
		   return numberOfPackages;
		 }
		 set
		 {
		   if(numberOfPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfPackages",OldValue=numberOfPackages,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   numberOfPackages=value;
		   }
			
		 }
	   }
	  private string numberOfContainers ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NumberOfContainers  
	   {
	    
	     get
		{
		   return numberOfContainers;
		 }
		 set
		 {
		   if(numberOfContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfContainers",OldValue=numberOfContainers,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   numberOfContainers=value;
		   }
			
		 }
	   }
	  private string volumeInCBM ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VolumeInCBM  
	   {
	    
	     get
		{
		   return volumeInCBM;
		 }
		 set
		 {
		   if(volumeInCBM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumeInCBM",OldValue=volumeInCBM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   volumeInCBM=value;
		   }
			
		 }
	   }
	  private string chargeableWeight ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargeableWeight  
	   {
	    
	     get
		{
		   return chargeableWeight;
		 }
		 set
		 {
		   if(chargeableWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeight",OldValue=chargeableWeight,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargeableWeight=value;
		   }
			
		 }
	   }
	  private string bookingNumberOfPackages ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingNumberOfPackages  
	   {
	    
	     get
		{
		   return bookingNumberOfPackages;
		 }
		 set
		 {
		   if(bookingNumberOfPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingNumberOfPackages",OldValue=bookingNumberOfPackages,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingNumberOfPackages=value;
		   }
			
		 }
	   }
	  private string orderChargeableWeight ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OrderChargeableWeight  
	   {
	    
	     get
		{
		   return orderChargeableWeight;
		 }
		 set
		 {
		   if(orderChargeableWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OrderChargeableWeight",OldValue=orderChargeableWeight,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   orderChargeableWeight=value;
		   }
			
		 }
	   }
	  private string aRInvoiceIssued ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ARInvoiceIssued  
	   {
	    
	     get
		{
		   return aRInvoiceIssued;
		 }
		 set
		 {
		   if(aRInvoiceIssued != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ARInvoiceIssued",OldValue=aRInvoiceIssued,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aRInvoiceIssued=value;
		   }
			
		 }
	   }
	  private string creditNoteIssued ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreditNoteIssued  
	   {
	    
	     get
		{
		   return creditNoteIssued;
		 }
		 set
		 {
		   if(creditNoteIssued != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditNoteIssued",OldValue=creditNoteIssued,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   creditNoteIssued=value;
		   }
			
		 }
	   }
	  private string cASSCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CASSCode  
	   {
	    
	     get
		{
		   return cASSCode;
		 }
		 set
		 {
		   if(cASSCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CASSCode",OldValue=cASSCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cASSCode=value;
		   }
			
		 }
	   }
	  private string tEU ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TEU  
	   {
	    
	     get
		{
		   return tEU;
		 }
		 set
		 {
		   if(tEU != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TEU",OldValue=tEU,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tEU=value;
		   }
			
		 }
	   }
	  private string finalArrivalDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalArrivalDate  
	   {
	    
	     get
		{
		   return finalArrivalDate;
		 }
		 set
		 {
		   if(finalArrivalDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalArrivalDate",OldValue=finalArrivalDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalArrivalDate=value;
		   }
			
		 }
	   }
	  private string accountNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountNumber  
	   {
	    
	     get
		{
		   return accountNumber;
		 }
		 set
		 {
		   if(accountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountNumber",OldValue=accountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountNumber=value;
		   }
			
		 }
	   }
	  private string asAgreedFreight ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AsAgreedFreight  
	   {
	    
	     get
		{
		   return asAgreedFreight;
		 }
		 set
		 {
		   if(asAgreedFreight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AsAgreedFreight",OldValue=asAgreedFreight,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   asAgreedFreight=value;
		   }
			
		 }
	   }
	  private string asAgreedOtherCharges ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AsAgreedOtherCharges  
	   {
	    
	     get
		{
		   return asAgreedOtherCharges;
		 }
		 set
		 {
		   if(asAgreedOtherCharges != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AsAgreedOtherCharges",OldValue=asAgreedOtherCharges,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   asAgreedOtherCharges=value;
		   }
			
		 }
	   }
	  private string deliveryOrder ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliveryOrder  
	   {
	    
	     get
		{
		   return deliveryOrder;
		 }
		 set
		 {
		   if(deliveryOrder != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveryOrder",OldValue=deliveryOrder,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliveryOrder=value;
		   }
			
		 }
	   }
	  private string importManifest ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImportManifest  
	   {
	    
	     get
		{
		   return importManifest;
		 }
		 set
		 {
		   if(importManifest != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImportManifest",OldValue=importManifest,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importManifest=value;
		   }
			
		 }
	   }
	  private string transportDocumentNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportDocumentNumber  
	   {
	    
	     get
		{
		   return transportDocumentNumber;
		 }
		 set
		 {
		   if(transportDocumentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportDocumentNumber",OldValue=transportDocumentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportDocumentNumber=value;
		   }
			
		 }
	   }
	  private string carrierTransportDocumentNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierTransportDocumentNumber  
	   {
	    
	     get
		{
		   return carrierTransportDocumentNumber;
		 }
		 set
		 {
		   if(carrierTransportDocumentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierTransportDocumentNumber",OldValue=carrierTransportDocumentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierTransportDocumentNumber=value;
		   }
			
		 }
	   }
	  private string freightLocationId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreightLocationId  
	   {
	    
	     get
		{
		   return freightLocationId;
		 }
		 set
		 {
		   if(freightLocationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreightLocationId",OldValue=freightLocationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freightLocationId=value;
		   }
			
		 }
	   }
	  private string lastFSRStatusRequestDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastFSRStatusRequestDate  
	   {
	    
	     get
		{
		   return lastFSRStatusRequestDate;
		 }
		 set
		 {
		   if(lastFSRStatusRequestDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastFSRStatusRequestDate",OldValue=lastFSRStatusRequestDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastFSRStatusRequestDate=value;
		   }
			
		 }
	   }
	  private string isMultipleCommodities ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsMultipleCommodities  
	   {
	    
	     get
		{
		   return isMultipleCommodities;
		 }
		 set
		 {
		   if(isMultipleCommodities != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMultipleCommodities",OldValue=isMultipleCommodities,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isMultipleCommodities=value;
		   }
			
		 }
	   }
	  private string specialServicesTypeId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecialServicesTypeId  
	   {
	    
	     get
		{
		   return specialServicesTypeId;
		 }
		 set
		 {
		   if(specialServicesTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecialServicesTypeId",OldValue=specialServicesTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specialServicesTypeId=value;
		   }
			
		 }
	   }
	  private string specialServicesTypeName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecialServicesTypeName  
	   {
	    
	     get
		{
		   return specialServicesTypeName;
		 }
		 set
		 {
		   if(specialServicesTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecialServicesTypeName",OldValue=specialServicesTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specialServicesTypeName=value;
		   }
			
		 }
	   }
	  private string fromPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortCountryCode  
	   {
	    
	     get
		{
		   return fromPortCountryCode;
		 }
		 set
		 {
		   if(fromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortCountryCode",OldValue=fromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string fromPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortCountryName  
	   {
	    
	     get
		{
		   return fromPortCountryName;
		 }
		 set
		 {
		   if(fromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortCountryName",OldValue=fromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortCountryName=value;
		   }
			
		 }
	   }
	  private string toPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortCountryCode  
	   {
	    
	     get
		{
		   return toPortCountryCode;
		 }
		 set
		 {
		   if(toPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortCountryCode",OldValue=toPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortCountryCode=value;
		   }
			
		 }
	   }
	  private string toPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortCountryName  
	   {
	    
	     get
		{
		   return toPortCountryName;
		 }
		 set
		 {
		   if(toPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortCountryName",OldValue=toPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortCountryName=value;
		   }
			
		 }
	   }
	  private string carrierNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierNumber  
	   {
	    
	     get
		{
		   return carrierNumber;
		 }
		 set
		 {
		   if(carrierNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierNumber",OldValue=carrierNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierNumber=value;
		   }
			
		 }
	   }
	  private string customerReference1 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerReference1  
	   {
	    
	     get
		{
		   return customerReference1;
		 }
		 set
		 {
		   if(customerReference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerReference1",OldValue=customerReference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerReference1=value;
		   }
			
		 }
	   }
	  private string customerReference2 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerReference2  
	   {
	    
	     get
		{
		   return customerReference2;
		 }
		 set
		 {
		   if(customerReference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerReference2",OldValue=customerReference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerReference2=value;
		   }
			
		 }
	   }
	  private string freightForwarderName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreightForwarderName  
	   {
	    
	     get
		{
		   return freightForwarderName;
		 }
		 set
		 {
		   if(freightForwarderName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreightForwarderName",OldValue=freightForwarderName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freightForwarderName=value;
		   }
			
		 }
	   }
	  private string customFileNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomFileNumber  
	   {
	    
	     get
		{
		   return customFileNumber;
		 }
		 set
		 {
		   if(customFileNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomFileNumber",OldValue=customFileNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customFileNumber=value;
		   }
			
		 }
	   }
	  private string productCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProductCode  
	   {
	    
	     get
		{
		   return productCode;
		 }
		 set
		 {
		   if(productCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProductCode",OldValue=productCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   productCode=value;
		   }
			
		 }
	   }
	  private string packagesQuantity ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackagesQuantity  
	   {
	    
	     get
		{
		   return packagesQuantity;
		 }
		 set
		 {
		   if(packagesQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackagesQuantity",OldValue=packagesQuantity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packagesQuantity=value;
		   }
			
		 }
	   }
	  private string cargonautFHLStatusCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargonautFHLStatusCode  
	   {
	    
	     get
		{
		   return cargonautFHLStatusCode;
		 }
		 set
		 {
		   if(cargonautFHLStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargonautFHLStatusCode",OldValue=cargonautFHLStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargonautFHLStatusCode=value;
		   }
			
		 }
	   }
	  private string cargonautFHLStatusName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargonautFHLStatusName  
	   {
	    
	     get
		{
		   return cargonautFHLStatusName;
		 }
		 set
		 {
		   if(cargonautFHLStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargonautFHLStatusName",OldValue=cargonautFHLStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargonautFHLStatusName=value;
		   }
			
		 }
	   }
	  private string cargonautFHLStatusDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargonautFHLStatusDate  
	   {
	    
	     get
		{
		   return cargonautFHLStatusDate;
		 }
		 set
		 {
		   if(cargonautFHLStatusDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargonautFHLStatusDate",OldValue=cargonautFHLStatusDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargonautFHLStatusDate=value;
		   }
			
		 }
	   }
	  private string cargonautFWBStatusCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargonautFWBStatusCode  
	   {
	    
	     get
		{
		   return cargonautFWBStatusCode;
		 }
		 set
		 {
		   if(cargonautFWBStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargonautFWBStatusCode",OldValue=cargonautFWBStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargonautFWBStatusCode=value;
		   }
			
		 }
	   }
	  private string cargonautFWBStatusName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargonautFWBStatusName  
	   {
	    
	     get
		{
		   return cargonautFWBStatusName;
		 }
		 set
		 {
		   if(cargonautFWBStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargonautFWBStatusName",OldValue=cargonautFWBStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargonautFWBStatusName=value;
		   }
			
		 }
	   }
	  private string cargonautFWBStatusDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargonautFWBStatusDate  
	   {
	    
	     get
		{
		   return cargonautFWBStatusDate;
		 }
		 set
		 {
		   if(cargonautFWBStatusDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargonautFWBStatusDate",OldValue=cargonautFWBStatusDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargonautFWBStatusDate=value;
		   }
			
		 }
	   }
	  private string numberOfInsidePackages ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NumberOfInsidePackages  
	   {
	    
	     get
		{
		   return numberOfInsidePackages;
		 }
		 set
		 {
		   if(numberOfInsidePackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfInsidePackages",OldValue=numberOfInsidePackages,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   numberOfInsidePackages=value;
		   }
			
		 }
	   }
	  private string numberOfInsidePackagesDetails ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NumberOfInsidePackagesDetails  
	   {
	    
	     get
		{
		   return numberOfInsidePackagesDetails;
		 }
		 set
		 {
		   if(numberOfInsidePackagesDetails != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfInsidePackagesDetails",OldValue=numberOfInsidePackagesDetails,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   numberOfInsidePackagesDetails=value;
		   }
			
		 }
	   }
	  private string consolidatorId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsolidatorId  
	   {
	    
	     get
		{
		   return consolidatorId;
		 }
		 set
		 {
		   if(consolidatorId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsolidatorId",OldValue=consolidatorId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consolidatorId=value;
		   }
			
		 }
	   }
	  private string consolidatorReference ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsolidatorReference  
	   {
	    
	     get
		{
		   return consolidatorReference;
		 }
		 set
		 {
		   if(consolidatorReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsolidatorReference",OldValue=consolidatorReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consolidatorReference=value;
		   }
			
		 }
	   }
	  private string consolidatorName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsolidatorName  
	   {
	    
	     get
		{
		   return consolidatorName;
		 }
		 set
		 {
		   if(consolidatorName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsolidatorName",OldValue=consolidatorName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consolidatorName=value;
		   }
			
		 }
	   }
	  private string consolidatorNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsolidatorNote  
	   {
	    
	     get
		{
		   return consolidatorNote;
		 }
		 set
		 {
		   if(consolidatorNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsolidatorNote",OldValue=consolidatorNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consolidatorNote=value;
		   }
			
		 }
	   }
	  private string consolidatorAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsolidatorAddressId  
	   {
	    
	     get
		{
		   return consolidatorAddressId;
		 }
		 set
		 {
		   if(consolidatorAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsolidatorAddressId",OldValue=consolidatorAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consolidatorAddressId=value;
		   }
			
		 }
	   }
	  private string consolidatorContactId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsolidatorContactId  
	   {
	    
	     get
		{
		   return consolidatorContactId;
		 }
		 set
		 {
		   if(consolidatorContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsolidatorContactId",OldValue=consolidatorContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consolidatorContactId=value;
		   }
			
		 }
	   }
	  private string accountManagerUserId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountManagerUserId  
	   {
	    
	     get
		{
		   return accountManagerUserId;
		 }
		 set
		 {
		   if(accountManagerUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountManagerUserId",OldValue=accountManagerUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountManagerUserId=value;
		   }
			
		 }
	   }
	  private string accountManagerUserName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountManagerUserName  
	   {
	    
	     get
		{
		   return accountManagerUserName;
		 }
		 set
		 {
		   if(accountManagerUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountManagerUserName",OldValue=accountManagerUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountManagerUserName=value;
		   }
			
		 }
	   }
	  private string createdByUserName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserName  
	   {
	    
	     get
		{
		   return createdByUserName;
		 }
		 set
		 {
		   if(createdByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserName",OldValue=createdByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserName=value;
		   }
			
		 }
	   }
	  private string salesmanUserName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SalesmanUserName  
	   {
	    
	     get
		{
		   return salesmanUserName;
		 }
		 set
		 {
		   if(salesmanUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SalesmanUserName",OldValue=salesmanUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   salesmanUserName=value;
		   }
			
		 }
	   }
	  private string manifestReason ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManifestReason  
	   {
	    
	     get
		{
		   return manifestReason;
		 }
		 set
		 {
		   if(manifestReason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManifestReason",OldValue=manifestReason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manifestReason=value;
		   }
			
		 }
	   }
	  private string manifestStatusCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManifestStatusCode  
	   {
	    
	     get
		{
		   return manifestStatusCode;
		 }
		 set
		 {
		   if(manifestStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManifestStatusCode",OldValue=manifestStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manifestStatusCode=value;
		   }
			
		 }
	   }
	  private string statusLocation ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusLocation  
	   {
	    
	     get
		{
		   return statusLocation;
		 }
		 set
		 {
		   if(statusLocation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusLocation",OldValue=statusLocation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusLocation=value;
		   }
			
		 }
	   }
	  private string airlinePrefix ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirlinePrefix  
	   {
	    
	     get
		{
		   return airlinePrefix;
		 }
		 set
		 {
		   if(airlinePrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirlinePrefix",OldValue=airlinePrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airlinePrefix=value;
		   }
			
		 }
	   }
	  private string operationalCloseDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OperationalCloseDate  
	   {
	    
	     get
		{
		   return operationalCloseDate;
		 }
		 set
		 {
		   if(operationalCloseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OperationalCloseDate",OldValue=operationalCloseDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   operationalCloseDate=value;
		   }
			
		 }
	   }
	  private string accountingCloseDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountingCloseDate  
	   {
	    
	     get
		{
		   return accountingCloseDate;
		 }
		 set
		 {
		   if(accountingCloseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingCloseDate",OldValue=accountingCloseDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountingCloseDate=value;
		   }
			
		 }
	   }
	  private string exceptionDescription ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExceptionDescription  
	   {
	    
	     get
		{
		   return exceptionDescription;
		 }
		 set
		 {
		   if(exceptionDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExceptionDescription",OldValue=exceptionDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exceptionDescription=value;
		   }
			
		 }
	   }
	  private string hasException ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string HasException  
	   {
	    
	     get
		{
		   return hasException;
		 }
		 set
		 {
		   if(hasException != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasException",OldValue=hasException,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   hasException=value;
		   }
			
		 }
	   }
	  private string customsDeclarationNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsDeclarationNumber  
	   {
	    
	     get
		{
		   return customsDeclarationNumber;
		 }
		 set
		 {
		   if(customsDeclarationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsDeclarationNumber",OldValue=customsDeclarationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsDeclarationNumber=value;
		   }
			
		 }
	   }
	  private string lastDocumentDateTime ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastDocumentDateTime  
	   {
	    
	     get
		{
		   return lastDocumentDateTime;
		 }
		 set
		 {
		   if(lastDocumentDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastDocumentDateTime",OldValue=lastDocumentDateTime,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastDocumentDateTime=value;
		   }
			
		 }
	   }
	  private string mainCarriageExpectedOrActual ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageExpectedOrActual  
	   {
	    
	     get
		{
		   return mainCarriageExpectedOrActual;
		 }
		 set
		 {
		   if(mainCarriageExpectedOrActual != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageExpectedOrActual",OldValue=mainCarriageExpectedOrActual,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageExpectedOrActual=value;
		   }
			
		 }
	   }
	  private string nextLegName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NextLegName  
	   {
	    
	     get
		{
		   return nextLegName;
		 }
		 set
		 {
		   if(nextLegName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextLegName",OldValue=nextLegName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nextLegName=value;
		   }
			
		 }
	   }
	  private string nextETD ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NextETD  
	   {
	    
	     get
		{
		   return nextETD;
		 }
		 set
		 {
		   if(nextETD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextETD",OldValue=nextETD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nextETD=value;
		   }
			
		 }
	   }
	  private string nextETA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NextETA  
	   {
	    
	     get
		{
		   return nextETA;
		 }
		 set
		 {
		   if(nextETA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextETA",OldValue=nextETA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nextETA=value;
		   }
			
		 }
	   }
	  private string estimateProfitInLocalCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EstimateProfitInLocalCurrency  
	   {
	    
	     get
		{
		   return estimateProfitInLocalCurrency;
		 }
		 set
		 {
		   if(estimateProfitInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimateProfitInLocalCurrency",OldValue=estimateProfitInLocalCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   estimateProfitInLocalCurrency=value;
		   }
			
		 }
	   }
	  private string isCancelled ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsCancelled  
	   {
	    
	     get
		{
		   return isCancelled;
		 }
		 set
		 {
		   if(isCancelled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancelled",OldValue=isCancelled,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isCancelled=value;
		   }
			
		 }
	   }
	  private string isOperationalClosed ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsOperationalClosed  
	   {
	    
	     get
		{
		   return isOperationalClosed;
		 }
		 set
		 {
		   if(isOperationalClosed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsOperationalClosed",OldValue=isOperationalClosed,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isOperationalClosed=value;
		   }
			
		 }
	   }
	  private string isAccountingClosed ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsAccountingClosed  
	   {
	    
	     get
		{
		   return isAccountingClosed;
		 }
		 set
		 {
		   if(isAccountingClosed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAccountingClosed",OldValue=isAccountingClosed,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isAccountingClosed=value;
		   }
			
		 }
	   }
	  private string openReceivablesInLocalCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpenReceivablesInLocalCurrency  
	   {
	    
	     get
		{
		   return openReceivablesInLocalCurrency;
		 }
		 set
		 {
		   if(openReceivablesInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenReceivablesInLocalCurrency",OldValue=openReceivablesInLocalCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   openReceivablesInLocalCurrency=value;
		   }
			
		 }
	   }
	  private string shipmentLevelCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentLevelCode  
	   {
	    
	     get
		{
		   return shipmentLevelCode;
		 }
		 set
		 {
		   if(shipmentLevelCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentLevelCode",OldValue=shipmentLevelCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentLevelCode=value;
		   }
			
		 }
	   }
	  private string shipmentLevelName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentLevelName  
	   {
	    
	     get
		{
		   return shipmentLevelName;
		 }
		 set
		 {
		   if(shipmentLevelName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentLevelName",OldValue=shipmentLevelName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentLevelName=value;
		   }
			
		 }
	   }
	  private string masterShipmentNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MasterShipmentNumber  
	   {
	    
	     get
		{
		   return masterShipmentNumber;
		 }
		 set
		 {
		   if(masterShipmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MasterShipmentNumber",OldValue=masterShipmentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   masterShipmentNumber=value;
		   }
			
		 }
	   }
	  private string accountedReceivablesInLocalCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountedReceivablesInLocalCurrency  
	   {
	    
	     get
		{
		   return accountedReceivablesInLocalCurrency;
		 }
		 set
		 {
		   if(accountedReceivablesInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountedReceivablesInLocalCurrency",OldValue=accountedReceivablesInLocalCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountedReceivablesInLocalCurrency=value;
		   }
			
		 }
	   }
	  private string profitInLocalCurrency ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProfitInLocalCurrency  
	   {
	    
	     get
		{
		   return profitInLocalCurrency;
		 }
		 set
		 {
		   if(profitInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProfitInLocalCurrency",OldValue=profitInLocalCurrency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   profitInLocalCurrency=value;
		   }
			
		 }
	   }
	  private string shipmentPayableStatusCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentPayableStatusCode  
	   {
	    
	     get
		{
		   return shipmentPayableStatusCode;
		 }
		 set
		 {
		   if(shipmentPayableStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentPayableStatusCode",OldValue=shipmentPayableStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentPayableStatusCode=value;
		   }
			
		 }
	   }
	  private string shipmentReceivableStatusCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentReceivableStatusCode  
	   {
	    
	     get
		{
		   return shipmentReceivableStatusCode;
		 }
		 set
		 {
		   if(shipmentReceivableStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentReceivableStatusCode",OldValue=shipmentReceivableStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentReceivableStatusCode=value;
		   }
			
		 }
	   }
	  private string shipmentPayableStatusName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentPayableStatusName  
	   {
	    
	     get
		{
		   return shipmentPayableStatusName;
		 }
		 set
		 {
		   if(shipmentPayableStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentPayableStatusName",OldValue=shipmentPayableStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentPayableStatusName=value;
		   }
			
		 }
	   }
	  private string shipmentReceivableStatusName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentReceivableStatusName  
	   {
	    
	     get
		{
		   return shipmentReceivableStatusName;
		 }
		 set
		 {
		   if(shipmentReceivableStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentReceivableStatusName",OldValue=shipmentReceivableStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentReceivableStatusName=value;
		   }
			
		 }
	   }
	  private string isHybrid ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsHybrid  
	   {
	    
	     get
		{
		   return isHybrid;
		 }
		 set
		 {
		   if(isHybrid != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsHybrid",OldValue=isHybrid,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isHybrid=value;
		   }
			
		 }
	   }
	  private string baseShipmentNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BaseShipmentNumber  
	   {
	    
	     get
		{
		   return baseShipmentNumber;
		 }
		 set
		 {
		   if(baseShipmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BaseShipmentNumber",OldValue=baseShipmentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   baseShipmentNumber=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierCode  
	   {
	    
	     get
		{
		   return mainCarriageCarrierCode;
		 }
		 set
		 {
		   if(mainCarriageCarrierCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierCode",OldValue=mainCarriageCarrierCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierCode=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierAddressId  
	   {
	    
	     get
		{
		   return mainCarriageCarrierAddressId;
		 }
		 set
		 {
		   if(mainCarriageCarrierAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierAddressId",OldValue=mainCarriageCarrierAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierAddressId=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierWebSite ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierWebSite  
	   {
	    
	     get
		{
		   return mainCarriageCarrierWebSite;
		 }
		 set
		 {
		   if(mainCarriageCarrierWebSite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierWebSite",OldValue=mainCarriageCarrierWebSite,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierWebSite=value;
		   }
			
		 }
	   }
	  private string isFlightDateActual ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsFlightDateActual  
	   {
	    
	     get
		{
		   return isFlightDateActual;
		 }
		 set
		 {
		   if(isFlightDateActual != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFlightDateActual",OldValue=isFlightDateActual,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isFlightDateActual=value;
		   }
			
		 }
	   }
	  private string directionName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DirectionName  
	   {
	    
	     get
		{
		   return directionName;
		 }
		 set
		 {
		   if(directionName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DirectionName",OldValue=directionName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   directionName=value;
		   }
			
		 }
	   }
	  private string transportModeName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeName  
	   {
	    
	     get
		{
		   return transportModeName;
		 }
		 set
		 {
		   if(transportModeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeName",OldValue=transportModeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeName=value;
		   }
			
		 }
	   }
	  private string shipmentTypeName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentTypeName  
	   {
	    
	     get
		{
		   return shipmentTypeName;
		 }
		 set
		 {
		   if(shipmentTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentTypeName",OldValue=shipmentTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentTypeName=value;
		   }
			
		 }
	   }
	  private string mAWBTakenFromStack ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWBTakenFromStack  
	   {
	    
	     get
		{
		   return mAWBTakenFromStack;
		 }
		 set
		 {
		   if(mAWBTakenFromStack != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBTakenFromStack",OldValue=mAWBTakenFromStack,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWBTakenFromStack=value;
		   }
			
		 }
	   }
	  private string mAWBReturnedToStack ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWBReturnedToStack  
	   {
	    
	     get
		{
		   return mAWBReturnedToStack;
		 }
		 set
		 {
		   if(mAWBReturnedToStack != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBReturnedToStack",OldValue=mAWBReturnedToStack,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWBReturnedToStack=value;
		   }
			
		 }
	   }
	  private string incotermName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IncotermName  
	   {
	    
	     get
		{
		   return incotermName;
		 }
		 set
		 {
		   if(incotermName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncotermName",OldValue=incotermName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   incotermName=value;
		   }
			
		 }
	   }
	  private string lastModified ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastModified  
	   {
	    
	     get
		{
		   return lastModified;
		 }
		 set
		 {
		   if(lastModified != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastModified",OldValue=lastModified,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastModified=value;
		   }
			
		 }
	   }
	  private string mAWBStackNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWBStackNumber  
	   {
	    
	     get
		{
		   return mAWBStackNumber;
		 }
		 set
		 {
		   if(mAWBStackNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBStackNumber",OldValue=mAWBStackNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWBStackNumber=value;
		   }
			
		 }
	   }
	  private string isSecured ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsSecured  
	   {
	    
	     get
		{
		   return isSecured;
		 }
		 set
		 {
		   if(isSecured != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSecured",OldValue=isSecured,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isSecured=value;
		   }
			
		 }
	   }
	  private string bookingNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingNumber  
	   {
	    
	     get
		{
		   return bookingNumber;
		 }
		 set
		 {
		   if(bookingNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingNumber",OldValue=bookingNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingNumber=value;
		   }
			
		 }
	   }
	  private string followUpId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FollowUpId  
	   {
	    
	     get
		{
		   return followUpId;
		 }
		 set
		 {
		   if(followUpId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowUpId",OldValue=followUpId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   followUpId=value;
		   }
			
		 }
	   }
	  private string shipmentPMId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentPMId  
	   {
	    
	     get
		{
		   return shipmentPMId;
		 }
		 set
		 {
		   if(shipmentPMId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentPMId",OldValue=shipmentPMId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentPMId=value;
		   }
			
		 }
	   }
	  private DateTime lastUpdate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime LastUpdate  
	   {
	    
	     get
		{
		   return lastUpdate;
		 }
		 set
		 {
		   if(lastUpdate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastUpdate",OldValue=lastUpdate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   lastUpdate=value;
		   }
			
		 }
	   }
	  private string newMessage ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NewMessage  
	   {
	    
	     get
		{
		   return newMessage;
		 }
		 set
		 {
		   if(newMessage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NewMessage",OldValue=newMessage,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   newMessage=value;
		   }
			
		 }
	   }
	  private string isAnyConversation ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsAnyConversation  
	   {
	    
	     get
		{
		   return isAnyConversation;
		 }
		 set
		 {
		   if(isAnyConversation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAnyConversation",OldValue=isAnyConversation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isAnyConversation=value;
		   }
			
		 }
	   }
	  private int numberOfShipments ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public int NumberOfShipments  
	   {
	    
	     get
		{
		   return numberOfShipments;
		 }
		 set
		 {
		   if(numberOfShipments != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfShipments",OldValue=numberOfShipments,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   numberOfShipments=value;
		   }
			
		 }
	   }
	  private string mainCarriageFinalDestinationPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFinalDestinationPortCode  
	   {
	    
	     get
		{
		   return mainCarriageFinalDestinationPortCode;
		 }
		 set
		 {
		   if(mainCarriageFinalDestinationPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFinalDestinationPortCode",OldValue=mainCarriageFinalDestinationPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFinalDestinationPortCode=value;
		   }
			
		 }
	   }
	  private string mainCarriageFinalDestinationPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFinalDestinationPortName  
	   {
	    
	     get
		{
		   return mainCarriageFinalDestinationPortName;
		 }
		 set
		 {
		   if(mainCarriageFinalDestinationPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFinalDestinationPortName",OldValue=mainCarriageFinalDestinationPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFinalDestinationPortName=value;
		   }
			
		 }
	   }
	  private string mainCarriageFinalDestinationPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFinalDestinationPortCountryCode  
	   {
	    
	     get
		{
		   return mainCarriageFinalDestinationPortCountryCode;
		 }
		 set
		 {
		   if(mainCarriageFinalDestinationPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFinalDestinationPortCountryCode",OldValue=mainCarriageFinalDestinationPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFinalDestinationPortCountryCode=value;
		   }
			
		 }
	   }
	  private string mainCarriageFinalDestinationPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFinalDestinationPortCountryName  
	   {
	    
	     get
		{
		   return mainCarriageFinalDestinationPortCountryName;
		 }
		 set
		 {
		   if(mainCarriageFinalDestinationPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFinalDestinationPortCountryName",OldValue=mainCarriageFinalDestinationPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFinalDestinationPortCountryName=value;
		   }
			
		 }
	   }
	  private DateTime accessDate ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime AccessDate  
	   {
	    
	     get
		{
		   return accessDate;
		 }
		 set
		 {
		   if(accessDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccessDate",OldValue=accessDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   accessDate=value;
		   }
			
		 }
	   }
	  private string updatedByUserName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserName  
	   {
	    
	     get
		{
		   return updatedByUserName;
		 }
		 set
		 {
		   if(updatedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserName",OldValue=updatedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserName=value;
		   }
			
		 }
	   }
	  private string eventNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EventNote  
	   {
	    
	     get
		{
		   return eventNote;
		 }
		 set
		 {
		   if(eventNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EventNote",OldValue=eventNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   eventNote=value;
		   }
			
		 }
	   }
	  private string quoteNumber ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteNumber  
	   {
	    
	     get
		{
		   return quoteNumber;
		 }
		 set
		 {
		   if(quoteNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteNumber",OldValue=quoteNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteNumber=value;
		   }
			
		 }
	   }
	  private string freelancerName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreelancerName  
	   {
	    
	     get
		{
		   return freelancerName;
		 }
		 set
		 {
		   if(freelancerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreelancerName",OldValue=freelancerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freelancerName=value;
		   }
			
		 }
	   }
	  private string issuingCarrierAgentName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IssuingCarrierAgentName  
	   {
	    
	     get
		{
		   return issuingCarrierAgentName;
		 }
		 set
		 {
		   if(issuingCarrierAgentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssuingCarrierAgentName",OldValue=issuingCarrierAgentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   issuingCarrierAgentName=value;
		   }
			
		 }
	   }
	  private string issuingCarrierAgentNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IssuingCarrierAgentNote  
	   {
	    
	     get
		{
		   return issuingCarrierAgentNote;
		 }
		 set
		 {
		   if(issuingCarrierAgentNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssuingCarrierAgentNote",OldValue=issuingCarrierAgentNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   issuingCarrierAgentNote=value;
		   }
			
		 }
	   }
	  private string freightForwarderNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreightForwarderNote  
	   {
	    
	     get
		{
		   return freightForwarderNote;
		 }
		 set
		 {
		   if(freightForwarderNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreightForwarderNote",OldValue=freightForwarderNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freightForwarderNote=value;
		   }
			
		 }
	   }
	  private string shipperNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperNote  
	   {
	    
	     get
		{
		   return shipperNote;
		 }
		 set
		 {
		   if(shipperNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperNote",OldValue=shipperNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperNote=value;
		   }
			
		 }
	   }
	  private string shipperAddressText ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperAddressText  
	   {
	    
	     get
		{
		   return shipperAddressText;
		 }
		 set
		 {
		   if(shipperAddressText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperAddressText",OldValue=shipperAddressText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperAddressText=value;
		   }
			
		 }
	   }
	  private string consigneeNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeNote  
	   {
	    
	     get
		{
		   return consigneeNote;
		 }
		 set
		 {
		   if(consigneeNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeNote",OldValue=consigneeNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeNote=value;
		   }
			
		 }
	   }
	  private string consigneeAddressText ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeAddressText  
	   {
	    
	     get
		{
		   return consigneeAddressText;
		 }
		 set
		 {
		   if(consigneeAddressText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeAddressText",OldValue=consigneeAddressText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeAddressText=value;
		   }
			
		 }
	   }
	  private string agentNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentNote  
	   {
	    
	     get
		{
		   return agentNote;
		 }
		 set
		 {
		   if(agentNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentNote",OldValue=agentNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentNote=value;
		   }
			
		 }
	   }
	  private string agentAddressText ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentAddressText  
	   {
	    
	     get
		{
		   return agentAddressText;
		 }
		 set
		 {
		   if(agentAddressText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentAddressText",OldValue=agentAddressText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentAddressText=value;
		   }
			
		 }
	   }
	  private string customAgentExportName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomAgentExportName  
	   {
	    
	     get
		{
		   return customAgentExportName;
		 }
		 set
		 {
		   if(customAgentExportName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomAgentExportName",OldValue=customAgentExportName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customAgentExportName=value;
		   }
			
		 }
	   }
	  private string customAgentExportNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomAgentExportNote  
	   {
	    
	     get
		{
		   return customAgentExportNote;
		 }
		 set
		 {
		   if(customAgentExportNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomAgentExportNote",OldValue=customAgentExportNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customAgentExportNote=value;
		   }
			
		 }
	   }
	  private string customAgentImportName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomAgentImportName  
	   {
	    
	     get
		{
		   return customAgentImportName;
		 }
		 set
		 {
		   if(customAgentImportName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomAgentImportName",OldValue=customAgentImportName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customAgentImportName=value;
		   }
			
		 }
	   }
	  private string customAgentImportNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomAgentImportNote  
	   {
	    
	     get
		{
		   return customAgentImportNote;
		 }
		 set
		 {
		   if(customAgentImportNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomAgentImportNote",OldValue=customAgentImportNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customAgentImportNote=value;
		   }
			
		 }
	   }
	  private string notify1Name ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notify1Name  
	   {
	    
	     get
		{
		   return notify1Name;
		 }
		 set
		 {
		   if(notify1Name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notify1Name",OldValue=notify1Name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notify1Name=value;
		   }
			
		 }
	   }
	  private string notify1Note ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notify1Note  
	   {
	    
	     get
		{
		   return notify1Note;
		 }
		 set
		 {
		   if(notify1Note != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notify1Note",OldValue=notify1Note,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notify1Note=value;
		   }
			
		 }
	   }
	  private string notify2Name ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notify2Name  
	   {
	    
	     get
		{
		   return notify2Name;
		 }
		 set
		 {
		   if(notify2Name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notify2Name",OldValue=notify2Name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notify2Name=value;
		   }
			
		 }
	   }
	  private string notify2Note ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notify2Note  
	   {
	    
	     get
		{
		   return notify2Note;
		 }
		 set
		 {
		   if(notify2Note != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notify2Note",OldValue=notify2Note,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notify2Note=value;
		   }
			
		 }
	   }
	  private string shipperNotExporterName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperNotExporterName  
	   {
	    
	     get
		{
		   return shipperNotExporterName;
		 }
		 set
		 {
		   if(shipperNotExporterName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperNotExporterName",OldValue=shipperNotExporterName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperNotExporterName=value;
		   }
			
		 }
	   }
	  private string shipperNotExporterNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperNotExporterNote  
	   {
	    
	     get
		{
		   return shipperNotExporterNote;
		 }
		 set
		 {
		   if(shipperNotExporterNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperNotExporterNote",OldValue=shipperNotExporterNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperNotExporterNote=value;
		   }
			
		 }
	   }
	  private string consigneeNotImporterName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeNotImporterName  
	   {
	    
	     get
		{
		   return consigneeNotImporterName;
		 }
		 set
		 {
		   if(consigneeNotImporterName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeNotImporterName",OldValue=consigneeNotImporterName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeNotImporterName=value;
		   }
			
		 }
	   }
	  private string consigneeNotImporterNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeNotImporterNote  
	   {
	    
	     get
		{
		   return consigneeNotImporterNote;
		 }
		 set
		 {
		   if(consigneeNotImporterNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeNotImporterNote",OldValue=consigneeNotImporterNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeNotImporterNote=value;
		   }
			
		 }
	   }
	  private string customClearancePointName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomClearancePointName  
	   {
	    
	     get
		{
		   return customClearancePointName;
		 }
		 set
		 {
		   if(customClearancePointName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomClearancePointName",OldValue=customClearancePointName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customClearancePointName=value;
		   }
			
		 }
	   }
	  private string customClearancePointNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomClearancePointNote  
	   {
	    
	     get
		{
		   return customClearancePointNote;
		 }
		 set
		 {
		   if(customClearancePointNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomClearancePointNote",OldValue=customClearancePointNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customClearancePointNote=value;
		   }
			
		 }
	   }
	  private string coloaderName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ColoaderName  
	   {
	    
	     get
		{
		   return coloaderName;
		 }
		 set
		 {
		   if(coloaderName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ColoaderName",OldValue=coloaderName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   coloaderName=value;
		   }
			
		 }
	   }
	  private string coloaderNote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ColoaderNote  
	   {
	    
	     get
		{
		   return coloaderNote;
		 }
		 set
		 {
		   if(coloaderNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ColoaderNote",OldValue=coloaderNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   coloaderNote=value;
		   }
			
		 }
	   }
	  private string isExceptionResolved ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsExceptionResolved  
	   {
	    
	     get
		{
		   return isExceptionResolved;
		 }
		 set
		 {
		   if(isExceptionResolved != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsExceptionResolved",OldValue=isExceptionResolved,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isExceptionResolved=value;
		   }
			
		 }
	   }
	  private string preCarriageCarrierName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageCarrierName  
	   {
	    
	     get
		{
		   return preCarriageCarrierName;
		 }
		 set
		 {
		   if(preCarriageCarrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageCarrierName",OldValue=preCarriageCarrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageCarrierName=value;
		   }
			
		 }
	   }
	  private string preCarriageCarrierCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageCarrierCode  
	   {
	    
	     get
		{
		   return preCarriageCarrierCode;
		 }
		 set
		 {
		   if(preCarriageCarrierCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageCarrierCode",OldValue=preCarriageCarrierCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageCarrierCode=value;
		   }
			
		 }
	   }
	  private string preCarriageFromPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageFromPortCode  
	   {
	    
	     get
		{
		   return preCarriageFromPortCode;
		 }
		 set
		 {
		   if(preCarriageFromPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageFromPortCode",OldValue=preCarriageFromPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageFromPortCode=value;
		   }
			
		 }
	   }
	  private string preCarriageFromPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageFromPortName  
	   {
	    
	     get
		{
		   return preCarriageFromPortName;
		 }
		 set
		 {
		   if(preCarriageFromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageFromPortName",OldValue=preCarriageFromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageFromPortName=value;
		   }
			
		 }
	   }
	  private string preCarriageFromPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageFromPortCountryCode  
	   {
	    
	     get
		{
		   return preCarriageFromPortCountryCode;
		 }
		 set
		 {
		   if(preCarriageFromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageFromPortCountryCode",OldValue=preCarriageFromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageFromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string preCarriageFromPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageFromPortCountryName  
	   {
	    
	     get
		{
		   return preCarriageFromPortCountryName;
		 }
		 set
		 {
		   if(preCarriageFromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageFromPortCountryName",OldValue=preCarriageFromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageFromPortCountryName=value;
		   }
			
		 }
	   }
	  private string preCarriageToPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageToPortCode  
	   {
	    
	     get
		{
		   return preCarriageToPortCode;
		 }
		 set
		 {
		   if(preCarriageToPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageToPortCode",OldValue=preCarriageToPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageToPortCode=value;
		   }
			
		 }
	   }
	  private string preCarriageToPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageToPortName  
	   {
	    
	     get
		{
		   return preCarriageToPortName;
		 }
		 set
		 {
		   if(preCarriageToPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageToPortName",OldValue=preCarriageToPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageToPortName=value;
		   }
			
		 }
	   }
	  private string preCarriageToPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageToPortCountryCode  
	   {
	    
	     get
		{
		   return preCarriageToPortCountryCode;
		 }
		 set
		 {
		   if(preCarriageToPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageToPortCountryCode",OldValue=preCarriageToPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageToPortCountryCode=value;
		   }
			
		 }
	   }
	  private string preCarriageToPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageToPortCountryName  
	   {
	    
	     get
		{
		   return preCarriageToPortCountryName;
		 }
		 set
		 {
		   if(preCarriageToPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageToPortCountryName",OldValue=preCarriageToPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageToPortCountryName=value;
		   }
			
		 }
	   }
	  private string preCarriageCarrierWebSite ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageCarrierWebSite  
	   {
	    
	     get
		{
		   return preCarriageCarrierWebSite;
		 }
		 set
		 {
		   if(preCarriageCarrierWebSite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageCarrierWebSite",OldValue=preCarriageCarrierWebSite,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageCarrierWebSite=value;
		   }
			
		 }
	   }
	  private string onCarriageCarrierName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageCarrierName  
	   {
	    
	     get
		{
		   return onCarriageCarrierName;
		 }
		 set
		 {
		   if(onCarriageCarrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageCarrierName",OldValue=onCarriageCarrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageCarrierName=value;
		   }
			
		 }
	   }
	  private string onCarriageCarrierCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageCarrierCode  
	   {
	    
	     get
		{
		   return onCarriageCarrierCode;
		 }
		 set
		 {
		   if(onCarriageCarrierCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageCarrierCode",OldValue=onCarriageCarrierCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageCarrierCode=value;
		   }
			
		 }
	   }
	  private string onCarriageFromPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageFromPortCode  
	   {
	    
	     get
		{
		   return onCarriageFromPortCode;
		 }
		 set
		 {
		   if(onCarriageFromPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageFromPortCode",OldValue=onCarriageFromPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageFromPortCode=value;
		   }
			
		 }
	   }
	  private string onCarriageFromPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageFromPortName  
	   {
	    
	     get
		{
		   return onCarriageFromPortName;
		 }
		 set
		 {
		   if(onCarriageFromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageFromPortName",OldValue=onCarriageFromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageFromPortName=value;
		   }
			
		 }
	   }
	  private string onCarriageFromPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageFromPortCountryCode  
	   {
	    
	     get
		{
		   return onCarriageFromPortCountryCode;
		 }
		 set
		 {
		   if(onCarriageFromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageFromPortCountryCode",OldValue=onCarriageFromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageFromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string onCarriageFromPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageFromPortCountryName  
	   {
	    
	     get
		{
		   return onCarriageFromPortCountryName;
		 }
		 set
		 {
		   if(onCarriageFromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageFromPortCountryName",OldValue=onCarriageFromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageFromPortCountryName=value;
		   }
			
		 }
	   }
	  private string onCarriageToPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageToPortCode  
	   {
	    
	     get
		{
		   return onCarriageToPortCode;
		 }
		 set
		 {
		   if(onCarriageToPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageToPortCode",OldValue=onCarriageToPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageToPortCode=value;
		   }
			
		 }
	   }
	  private string onCarriageToPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageToPortName  
	   {
	    
	     get
		{
		   return onCarriageToPortName;
		 }
		 set
		 {
		   if(onCarriageToPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageToPortName",OldValue=onCarriageToPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageToPortName=value;
		   }
			
		 }
	   }
	  private string onCarriageToPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageToPortCountryCode  
	   {
	    
	     get
		{
		   return onCarriageToPortCountryCode;
		 }
		 set
		 {
		   if(onCarriageToPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageToPortCountryCode",OldValue=onCarriageToPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageToPortCountryCode=value;
		   }
			
		 }
	   }
	  private string onCarriageToPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageToPortCountryName  
	   {
	    
	     get
		{
		   return onCarriageToPortCountryName;
		 }
		 set
		 {
		   if(onCarriageToPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageToPortCountryName",OldValue=onCarriageToPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageToPortCountryName=value;
		   }
			
		 }
	   }
	  private string onCarriageCarrierWebSite ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageCarrierWebSite  
	   {
	    
	     get
		{
		   return onCarriageCarrierWebSite;
		 }
		 set
		 {
		   if(onCarriageCarrierWebSite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageCarrierWebSite",OldValue=onCarriageCarrierWebSite,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageCarrierWebSite=value;
		   }
			
		 }
	   }
	  private string mainCarriageTransportModeId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageTransportModeId  
	   {
	    
	     get
		{
		   return mainCarriageTransportModeId;
		 }
		 set
		 {
		   if(mainCarriageTransportModeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageTransportModeId",OldValue=mainCarriageTransportModeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageTransportModeId=value;
		   }
			
		 }
	   }
	  private string mainCarriageFromPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFromPortCode  
	   {
	    
	     get
		{
		   return mainCarriageFromPortCode;
		 }
		 set
		 {
		   if(mainCarriageFromPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFromPortCode",OldValue=mainCarriageFromPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFromPortCode=value;
		   }
			
		 }
	   }
	  private string mainCarriageFromPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFromPortCountryName  
	   {
	    
	     get
		{
		   return mainCarriageFromPortCountryName;
		 }
		 set
		 {
		   if(mainCarriageFromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFromPortCountryName",OldValue=mainCarriageFromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFromPortCountryName=value;
		   }
			
		 }
	   }
	  private string mainCarriageFromPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFromPortCountryCode  
	   {
	    
	     get
		{
		   return mainCarriageFromPortCountryCode;
		 }
		 set
		 {
		   if(mainCarriageFromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFromPortCountryCode",OldValue=mainCarriageFromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string mainCarriageToPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageToPortCode  
	   {
	    
	     get
		{
		   return mainCarriageToPortCode;
		 }
		 set
		 {
		   if(mainCarriageToPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageToPortCode",OldValue=mainCarriageToPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageToPortCode=value;
		   }
			
		 }
	   }
	  private string mainCarriageToPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageToPortName  
	   {
	    
	     get
		{
		   return mainCarriageToPortName;
		 }
		 set
		 {
		   if(mainCarriageToPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageToPortName",OldValue=mainCarriageToPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageToPortName=value;
		   }
			
		 }
	   }
	  private string mainCarriageToPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageToPortCountryCode  
	   {
	    
	     get
		{
		   return mainCarriageToPortCountryCode;
		 }
		 set
		 {
		   if(mainCarriageToPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageToPortCountryCode",OldValue=mainCarriageToPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageToPortCountryCode=value;
		   }
			
		 }
	   }
	  private string mainCarriageToPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageToPortCountryName  
	   {
	    
	     get
		{
		   return mainCarriageToPortCountryName;
		 }
		 set
		 {
		   if(mainCarriageToPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageToPortCountryName",OldValue=mainCarriageToPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageToPortCountryName=value;
		   }
			
		 }
	   }
	  private string mainCarriageFromPortCountryEC ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFromPortCountryEC  
	   {
	    
	     get
		{
		   return mainCarriageFromPortCountryEC;
		 }
		 set
		 {
		   if(mainCarriageFromPortCountryEC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFromPortCountryEC",OldValue=mainCarriageFromPortCountryEC,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFromPortCountryEC=value;
		   }
			
		 }
	   }
	  private string mainCarriageToPortCountryEC ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageToPortCountryEC  
	   {
	    
	     get
		{
		   return mainCarriageToPortCountryEC;
		 }
		 set
		 {
		   if(mainCarriageToPortCountryEC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageToPortCountryEC",OldValue=mainCarriageToPortCountryEC,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageToPortCountryEC=value;
		   }
			
		 }
	   }
	  private string mainCarriageVesselName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageVesselName  
	   {
	    
	     get
		{
		   return mainCarriageVesselName;
		 }
		 set
		 {
		   if(mainCarriageVesselName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageVesselName",OldValue=mainCarriageVesselName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageVesselName=value;
		   }
			
		 }
	   }
	  private string preCarriageVesselName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreCarriageVesselName  
	   {
	    
	     get
		{
		   return preCarriageVesselName;
		 }
		 set
		 {
		   if(preCarriageVesselName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreCarriageVesselName",OldValue=preCarriageVesselName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preCarriageVesselName=value;
		   }
			
		 }
	   }
	  private string onCarriageVesselName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnCarriageVesselName  
	   {
	    
	     get
		{
		   return onCarriageVesselName;
		 }
		 set
		 {
		   if(onCarriageVesselName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnCarriageVesselName",OldValue=onCarriageVesselName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onCarriageVesselName=value;
		   }
			
		 }
	   }
	  private string transshipment1VesselName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1VesselName  
	   {
	    
	     get
		{
		   return transshipment1VesselName;
		 }
		 set
		 {
		   if(transshipment1VesselName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1VesselName",OldValue=transshipment1VesselName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1VesselName=value;
		   }
			
		 }
	   }
	  private string transshipment2VesselName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2VesselName  
	   {
	    
	     get
		{
		   return transshipment2VesselName;
		 }
		 set
		 {
		   if(transshipment2VesselName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2VesselName",OldValue=transshipment2VesselName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2VesselName=value;
		   }
			
		 }
	   }
	  private string transshipment3VesselName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3VesselName  
	   {
	    
	     get
		{
		   return transshipment3VesselName;
		 }
		 set
		 {
		   if(transshipment3VesselName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3VesselName",OldValue=transshipment3VesselName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3VesselName=value;
		   }
			
		 }
	   }
	  private string mainCarriageIsFromStack ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageIsFromStack  
	   {
	    
	     get
		{
		   return mainCarriageIsFromStack;
		 }
		 set
		 {
		   if(mainCarriageIsFromStack != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageIsFromStack",OldValue=mainCarriageIsFromStack,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageIsFromStack=value;
		   }
			
		 }
	   }
	  private string transshipment1CarrierName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1CarrierName  
	   {
	    
	     get
		{
		   return transshipment1CarrierName;
		 }
		 set
		 {
		   if(transshipment1CarrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1CarrierName",OldValue=transshipment1CarrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1CarrierName=value;
		   }
			
		 }
	   }
	  private string transshipment1CarrierCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1CarrierCode  
	   {
	    
	     get
		{
		   return transshipment1CarrierCode;
		 }
		 set
		 {
		   if(transshipment1CarrierCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1CarrierCode",OldValue=transshipment1CarrierCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1CarrierCode=value;
		   }
			
		 }
	   }
	  private string transshipment1FromPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1FromPortCode  
	   {
	    
	     get
		{
		   return transshipment1FromPortCode;
		 }
		 set
		 {
		   if(transshipment1FromPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1FromPortCode",OldValue=transshipment1FromPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1FromPortCode=value;
		   }
			
		 }
	   }
	  private string transshipment1FromPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1FromPortName  
	   {
	    
	     get
		{
		   return transshipment1FromPortName;
		 }
		 set
		 {
		   if(transshipment1FromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1FromPortName",OldValue=transshipment1FromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1FromPortName=value;
		   }
			
		 }
	   }
	  private string transshipment1FromPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1FromPortCountryCode  
	   {
	    
	     get
		{
		   return transshipment1FromPortCountryCode;
		 }
		 set
		 {
		   if(transshipment1FromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1FromPortCountryCode",OldValue=transshipment1FromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1FromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string transshipment1FromPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1FromPortCountryName  
	   {
	    
	     get
		{
		   return transshipment1FromPortCountryName;
		 }
		 set
		 {
		   if(transshipment1FromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1FromPortCountryName",OldValue=transshipment1FromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1FromPortCountryName=value;
		   }
			
		 }
	   }
	  private string transshipment1ToPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1ToPortCode  
	   {
	    
	     get
		{
		   return transshipment1ToPortCode;
		 }
		 set
		 {
		   if(transshipment1ToPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1ToPortCode",OldValue=transshipment1ToPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1ToPortCode=value;
		   }
			
		 }
	   }
	  private string transshipment1ToPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1ToPortName  
	   {
	    
	     get
		{
		   return transshipment1ToPortName;
		 }
		 set
		 {
		   if(transshipment1ToPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1ToPortName",OldValue=transshipment1ToPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1ToPortName=value;
		   }
			
		 }
	   }
	  private string transshipment1ToPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1ToPortCountryCode  
	   {
	    
	     get
		{
		   return transshipment1ToPortCountryCode;
		 }
		 set
		 {
		   if(transshipment1ToPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1ToPortCountryCode",OldValue=transshipment1ToPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1ToPortCountryCode=value;
		   }
			
		 }
	   }
	  private string transshipment1ToPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1ToPortCountryName  
	   {
	    
	     get
		{
		   return transshipment1ToPortCountryName;
		 }
		 set
		 {
		   if(transshipment1ToPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1ToPortCountryName",OldValue=transshipment1ToPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1ToPortCountryName=value;
		   }
			
		 }
	   }
	  private string transshipment1CarrierWebSite ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1CarrierWebSite  
	   {
	    
	     get
		{
		   return transshipment1CarrierWebSite;
		 }
		 set
		 {
		   if(transshipment1CarrierWebSite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1CarrierWebSite",OldValue=transshipment1CarrierWebSite,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1CarrierWebSite=value;
		   }
			
		 }
	   }
	  private string transshipment2CarrierWebSite ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2CarrierWebSite  
	   {
	    
	     get
		{
		   return transshipment2CarrierWebSite;
		 }
		 set
		 {
		   if(transshipment2CarrierWebSite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2CarrierWebSite",OldValue=transshipment2CarrierWebSite,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2CarrierWebSite=value;
		   }
			
		 }
	   }
	  private string transshipment3CarrierWebSite ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3CarrierWebSite  
	   {
	    
	     get
		{
		   return transshipment3CarrierWebSite;
		 }
		 set
		 {
		   if(transshipment3CarrierWebSite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3CarrierWebSite",OldValue=transshipment3CarrierWebSite,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3CarrierWebSite=value;
		   }
			
		 }
	   }
	  private string transshipment2CarrierName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2CarrierName  
	   {
	    
	     get
		{
		   return transshipment2CarrierName;
		 }
		 set
		 {
		   if(transshipment2CarrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2CarrierName",OldValue=transshipment2CarrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2CarrierName=value;
		   }
			
		 }
	   }
	  private string transshipment2CarrierCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2CarrierCode  
	   {
	    
	     get
		{
		   return transshipment2CarrierCode;
		 }
		 set
		 {
		   if(transshipment2CarrierCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2CarrierCode",OldValue=transshipment2CarrierCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2CarrierCode=value;
		   }
			
		 }
	   }
	  private string transshipment2FromPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2FromPortCode  
	   {
	    
	     get
		{
		   return transshipment2FromPortCode;
		 }
		 set
		 {
		   if(transshipment2FromPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2FromPortCode",OldValue=transshipment2FromPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2FromPortCode=value;
		   }
			
		 }
	   }
	  private string transshipment2FromPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2FromPortName  
	   {
	    
	     get
		{
		   return transshipment2FromPortName;
		 }
		 set
		 {
		   if(transshipment2FromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2FromPortName",OldValue=transshipment2FromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2FromPortName=value;
		   }
			
		 }
	   }
	  private string transshipment2FromPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2FromPortCountryCode  
	   {
	    
	     get
		{
		   return transshipment2FromPortCountryCode;
		 }
		 set
		 {
		   if(transshipment2FromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2FromPortCountryCode",OldValue=transshipment2FromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2FromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string transshipment2FromPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2FromPortCountryName  
	   {
	    
	     get
		{
		   return transshipment2FromPortCountryName;
		 }
		 set
		 {
		   if(transshipment2FromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2FromPortCountryName",OldValue=transshipment2FromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2FromPortCountryName=value;
		   }
			
		 }
	   }
	  private string transshipment2ToPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2ToPortCode  
	   {
	    
	     get
		{
		   return transshipment2ToPortCode;
		 }
		 set
		 {
		   if(transshipment2ToPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2ToPortCode",OldValue=transshipment2ToPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2ToPortCode=value;
		   }
			
		 }
	   }
	  private string transshipment2ToPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2ToPortName  
	   {
	    
	     get
		{
		   return transshipment2ToPortName;
		 }
		 set
		 {
		   if(transshipment2ToPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2ToPortName",OldValue=transshipment2ToPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2ToPortName=value;
		   }
			
		 }
	   }
	  private string transshipment2ToPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2ToPortCountryCode  
	   {
	    
	     get
		{
		   return transshipment2ToPortCountryCode;
		 }
		 set
		 {
		   if(transshipment2ToPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2ToPortCountryCode",OldValue=transshipment2ToPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2ToPortCountryCode=value;
		   }
			
		 }
	   }
	  private string transshipment2ToPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2ToPortCountryName  
	   {
	    
	     get
		{
		   return transshipment2ToPortCountryName;
		 }
		 set
		 {
		   if(transshipment2ToPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2ToPortCountryName",OldValue=transshipment2ToPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2ToPortCountryName=value;
		   }
			
		 }
	   }
	  private string transshipment3CarrierName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3CarrierName  
	   {
	    
	     get
		{
		   return transshipment3CarrierName;
		 }
		 set
		 {
		   if(transshipment3CarrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3CarrierName",OldValue=transshipment3CarrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3CarrierName=value;
		   }
			
		 }
	   }
	  private string transshipment3CarrierCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3CarrierCode  
	   {
	    
	     get
		{
		   return transshipment3CarrierCode;
		 }
		 set
		 {
		   if(transshipment3CarrierCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3CarrierCode",OldValue=transshipment3CarrierCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3CarrierCode=value;
		   }
			
		 }
	   }
	  private string transshipment3FromPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3FromPortCode  
	   {
	    
	     get
		{
		   return transshipment3FromPortCode;
		 }
		 set
		 {
		   if(transshipment3FromPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3FromPortCode",OldValue=transshipment3FromPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3FromPortCode=value;
		   }
			
		 }
	   }
	  private string transshipment3FromPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3FromPortName  
	   {
	    
	     get
		{
		   return transshipment3FromPortName;
		 }
		 set
		 {
		   if(transshipment3FromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3FromPortName",OldValue=transshipment3FromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3FromPortName=value;
		   }
			
		 }
	   }
	  private string transshipment3FromPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3FromPortCountryCode  
	   {
	    
	     get
		{
		   return transshipment3FromPortCountryCode;
		 }
		 set
		 {
		   if(transshipment3FromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3FromPortCountryCode",OldValue=transshipment3FromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3FromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string transshipment3FromPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3FromPortCountryName  
	   {
	    
	     get
		{
		   return transshipment3FromPortCountryName;
		 }
		 set
		 {
		   if(transshipment3FromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3FromPortCountryName",OldValue=transshipment3FromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3FromPortCountryName=value;
		   }
			
		 }
	   }
	  private string transshipment3ToPortCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3ToPortCode  
	   {
	    
	     get
		{
		   return transshipment3ToPortCode;
		 }
		 set
		 {
		   if(transshipment3ToPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3ToPortCode",OldValue=transshipment3ToPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3ToPortCode=value;
		   }
			
		 }
	   }
	  private string transshipment3ToPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3ToPortName  
	   {
	    
	     get
		{
		   return transshipment3ToPortName;
		 }
		 set
		 {
		   if(transshipment3ToPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3ToPortName",OldValue=transshipment3ToPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3ToPortName=value;
		   }
			
		 }
	   }
	  private string transshipment3ToPortCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3ToPortCountryCode  
	   {
	    
	     get
		{
		   return transshipment3ToPortCountryCode;
		 }
		 set
		 {
		   if(transshipment3ToPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3ToPortCountryCode",OldValue=transshipment3ToPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3ToPortCountryCode=value;
		   }
			
		 }
	   }
	  private string transshipment3ToPortCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3ToPortCountryName  
	   {
	    
	     get
		{
		   return transshipment3ToPortCountryName;
		 }
		 set
		 {
		   if(transshipment3ToPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3ToPortCountryName",OldValue=transshipment3ToPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3ToPortCountryName=value;
		   }
			
		 }
	   }
	  private string transshipment1ToPortCountryEC ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1ToPortCountryEC  
	   {
	    
	     get
		{
		   return transshipment1ToPortCountryEC;
		 }
		 set
		 {
		   if(transshipment1ToPortCountryEC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1ToPortCountryEC",OldValue=transshipment1ToPortCountryEC,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1ToPortCountryEC=value;
		   }
			
		 }
	   }
	  private string transshipment2ToPortCountryEC ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2ToPortCountryEC  
	   {
	    
	     get
		{
		   return transshipment2ToPortCountryEC;
		 }
		 set
		 {
		   if(transshipment2ToPortCountryEC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2ToPortCountryEC",OldValue=transshipment2ToPortCountryEC,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2ToPortCountryEC=value;
		   }
			
		 }
	   }
	  private string transshipment3ToPortCountryEC ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3ToPortCountryEC  
	   {
	    
	     get
		{
		   return transshipment3ToPortCountryEC;
		 }
		 set
		 {
		   if(transshipment3ToPortCountryEC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3ToPortCountryEC",OldValue=transshipment3ToPortCountryEC,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3ToPortCountryEC=value;
		   }
			
		 }
	   }
	  private string fromPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortName  
	   {
	    
	     get
		{
		   return fromPortName;
		 }
		 set
		 {
		   if(fromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortName",OldValue=fromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortName=value;
		   }
			
		 }
	   }
	  private string fromPortCountry ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortCountry  
	   {
	    
	     get
		{
		   return fromPortCountry;
		 }
		 set
		 {
		   if(fromPortCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortCountry",OldValue=fromPortCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortCountry=value;
		   }
			
		 }
	   }
	  private string toPortName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortName  
	   {
	    
	     get
		{
		   return toPortName;
		 }
		 set
		 {
		   if(toPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortName",OldValue=toPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortName=value;
		   }
			
		 }
	   }
	  private string toPortCountry ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortCountry  
	   {
	    
	     get
		{
		   return toPortCountry;
		 }
		 set
		 {
		   if(toPortCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortCountry",OldValue=toPortCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortCountry=value;
		   }
			
		 }
	   }
	  private string aWBCurrencyCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBCurrencyCode  
	   {
	    
	     get
		{
		   return aWBCurrencyCode;
		 }
		 set
		 {
		   if(aWBCurrencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBCurrencyCode",OldValue=aWBCurrencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBCurrencyCode=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineId  
	   {
	    
	     get
		{
		   return tenantZeroAirlineId;
		 }
		 set
		 {
		   if(tenantZeroAirlineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineId",OldValue=tenantZeroAirlineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineId=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineTTY ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineTTY  
	   {
	    
	     get
		{
		   return tenantZeroAirlineTTY;
		 }
		 set
		 {
		   if(tenantZeroAirlineTTY != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineTTY",OldValue=tenantZeroAirlineTTY,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineTTY=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlinePIMA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlinePIMA  
	   {
	    
	     get
		{
		   return tenantZeroAirlinePIMA;
		 }
		 set
		 {
		   if(tenantZeroAirlinePIMA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlinePIMA",OldValue=tenantZeroAirlinePIMA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlinePIMA=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineChampFWB ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineChampFWB  
	   {
	    
	     get
		{
		   return tenantZeroAirlineChampFWB;
		 }
		 set
		 {
		   if(tenantZeroAirlineChampFWB != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineChampFWB",OldValue=tenantZeroAirlineChampFWB,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineChampFWB=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineChampFHL ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineChampFHL  
	   {
	    
	     get
		{
		   return tenantZeroAirlineChampFHL;
		 }
		 set
		 {
		   if(tenantZeroAirlineChampFHL != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineChampFHL",OldValue=tenantZeroAirlineChampFHL,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineChampFHL=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineChampFSU ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineChampFSU  
	   {
	    
	     get
		{
		   return tenantZeroAirlineChampFSU;
		 }
		 set
		 {
		   if(tenantZeroAirlineChampFSU != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineChampFSU",OldValue=tenantZeroAirlineChampFSU,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineChampFSU=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineChampFSRFSA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineChampFSRFSA  
	   {
	    
	     get
		{
		   return tenantZeroAirlineChampFSRFSA;
		 }
		 set
		 {
		   if(tenantZeroAirlineChampFSRFSA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineChampFSRFSA",OldValue=tenantZeroAirlineChampFSRFSA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineChampFSRFSA=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineChampFVRFVA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineChampFVRFVA  
	   {
	    
	     get
		{
		   return tenantZeroAirlineChampFVRFVA;
		 }
		 set
		 {
		   if(tenantZeroAirlineChampFVRFVA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineChampFVRFVA",OldValue=tenantZeroAirlineChampFVRFVA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineChampFVRFVA=value;
		   }
			
		 }
	   }
	  private string carrierIsChampRegistered ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierIsChampRegistered  
	   {
	    
	     get
		{
		   return carrierIsChampRegistered;
		 }
		 set
		 {
		   if(carrierIsChampRegistered != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierIsChampRegistered",OldValue=carrierIsChampRegistered,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierIsChampRegistered=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineChampNeedsRegistration ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineChampNeedsRegistration  
	   {
	    
	     get
		{
		   return tenantZeroAirlineChampNeedsRegistration;
		 }
		 set
		 {
		   if(tenantZeroAirlineChampNeedsRegistration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineChampNeedsRegistration",OldValue=tenantZeroAirlineChampNeedsRegistration,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineChampNeedsRegistration=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineGLSHKFWB ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineGLSHKFWB  
	   {
	    
	     get
		{
		   return tenantZeroAirlineGLSHKFWB;
		 }
		 set
		 {
		   if(tenantZeroAirlineGLSHKFWB != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineGLSHKFWB",OldValue=tenantZeroAirlineGLSHKFWB,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineGLSHKFWB=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineGLSHKFHL ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineGLSHKFHL  
	   {
	    
	     get
		{
		   return tenantZeroAirlineGLSHKFHL;
		 }
		 set
		 {
		   if(tenantZeroAirlineGLSHKFHL != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineGLSHKFHL",OldValue=tenantZeroAirlineGLSHKFHL,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineGLSHKFHL=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineGLSHKFSU ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineGLSHKFSU  
	   {
	    
	     get
		{
		   return tenantZeroAirlineGLSHKFSU;
		 }
		 set
		 {
		   if(tenantZeroAirlineGLSHKFSU != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineGLSHKFSU",OldValue=tenantZeroAirlineGLSHKFSU,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineGLSHKFSU=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineGLSHKFSRFSA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineGLSHKFSRFSA  
	   {
	    
	     get
		{
		   return tenantZeroAirlineGLSHKFSRFSA;
		 }
		 set
		 {
		   if(tenantZeroAirlineGLSHKFSRFSA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineGLSHKFSRFSA",OldValue=tenantZeroAirlineGLSHKFSRFSA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineGLSHKFSRFSA=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineGLSHKFVRFVA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineGLSHKFVRFVA  
	   {
	    
	     get
		{
		   return tenantZeroAirlineGLSHKFVRFVA;
		 }
		 set
		 {
		   if(tenantZeroAirlineGLSHKFVRFVA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineGLSHKFVRFVA",OldValue=tenantZeroAirlineGLSHKFVRFVA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineGLSHKFVRFVA=value;
		   }
			
		 }
	   }
	  private string carrierIsGLSHKRegistered ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierIsGLSHKRegistered  
	   {
	    
	     get
		{
		   return carrierIsGLSHKRegistered;
		 }
		 set
		 {
		   if(carrierIsGLSHKRegistered != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierIsGLSHKRegistered",OldValue=carrierIsGLSHKRegistered,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierIsGLSHKRegistered=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineGLSHKNeedsRegistration ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineGLSHKNeedsRegistration  
	   {
	    
	     get
		{
		   return tenantZeroAirlineGLSHKNeedsRegistration;
		 }
		 set
		 {
		   if(tenantZeroAirlineGLSHKNeedsRegistration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineGLSHKNeedsRegistration",OldValue=tenantZeroAirlineGLSHKNeedsRegistration,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineGLSHKNeedsRegistration=value;
		   }
			
		 }
	   }
	  private string carrierIsCheckDigit ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierIsCheckDigit  
	   {
	    
	     get
		{
		   return carrierIsCheckDigit;
		 }
		 set
		 {
		   if(carrierIsCheckDigit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierIsCheckDigit",OldValue=carrierIsCheckDigit,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierIsCheckDigit=value;
		   }
			
		 }
	   }
	  private string carrierIsLimitedLength ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierIsLimitedLength  
	   {
	    
	     get
		{
		   return carrierIsLimitedLength;
		 }
		 set
		 {
		   if(carrierIsLimitedLength != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierIsLimitedLength",OldValue=carrierIsLimitedLength,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierIsLimitedLength=value;
		   }
			
		 }
	   }
	  private string convertFromHouseToDirect ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConvertFromHouseToDirect  
	   {
	    
	     get
		{
		   return convertFromHouseToDirect;
		 }
		 set
		 {
		   if(convertFromHouseToDirect != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConvertFromHouseToDirect",OldValue=convertFromHouseToDirect,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   convertFromHouseToDirect=value;
		   }
			
		 }
	   }
	  private string convertFromDirectToHouse ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConvertFromDirectToHouse  
	   {
	    
	     get
		{
		   return convertFromDirectToHouse;
		 }
		 set
		 {
		   if(convertFromDirectToHouse != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConvertFromDirectToHouse",OldValue=convertFromDirectToHouse,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   convertFromDirectToHouse=value;
		   }
			
		 }
	   }
	  private string customerRankName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerRankName  
	   {
	    
	     get
		{
		   return customerRankName;
		 }
		 set
		 {
		   if(customerRankName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerRankName",OldValue=customerRankName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerRankName=value;
		   }
			
		 }
	   }
	  private string moveTypeCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MoveTypeCode  
	   {
	    
	     get
		{
		   return moveTypeCode;
		 }
		 set
		 {
		   if(moveTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MoveTypeCode",OldValue=moveTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   moveTypeCode=value;
		   }
			
		 }
	   }
	  private string moveTypeName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MoveTypeName  
	   {
	    
	     get
		{
		   return moveTypeName;
		 }
		 set
		 {
		   if(moveTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MoveTypeName",OldValue=moveTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   moveTypeName=value;
		   }
			
		 }
	   }
	  private string mainCarriageSTD ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageSTD  
	   {
	    
	     get
		{
		   return mainCarriageSTD;
		 }
		 set
		 {
		   if(mainCarriageSTD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageSTD",OldValue=mainCarriageSTD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageSTD=value;
		   }
			
		 }
	   }
	  private string mainCarriageSTA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageSTA  
	   {
	    
	     get
		{
		   return mainCarriageSTA;
		 }
		 set
		 {
		   if(mainCarriageSTA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageSTA",OldValue=mainCarriageSTA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageSTA=value;
		   }
			
		 }
	   }
	  private string transshipment1STD ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1STD  
	   {
	    
	     get
		{
		   return transshipment1STD;
		 }
		 set
		 {
		   if(transshipment1STD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1STD",OldValue=transshipment1STD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1STD=value;
		   }
			
		 }
	   }
	  private string transshipment1STA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1STA  
	   {
	    
	     get
		{
		   return transshipment1STA;
		 }
		 set
		 {
		   if(transshipment1STA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1STA",OldValue=transshipment1STA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1STA=value;
		   }
			
		 }
	   }
	  private string transshipment2STD ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2STD  
	   {
	    
	     get
		{
		   return transshipment2STD;
		 }
		 set
		 {
		   if(transshipment2STD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2STD",OldValue=transshipment2STD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2STD=value;
		   }
			
		 }
	   }
	  private string transshipment2STA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2STA  
	   {
	    
	     get
		{
		   return transshipment2STA;
		 }
		 set
		 {
		   if(transshipment2STA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2STA",OldValue=transshipment2STA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2STA=value;
		   }
			
		 }
	   }
	  private string transshipment3STD ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3STD  
	   {
	    
	     get
		{
		   return transshipment3STD;
		 }
		 set
		 {
		   if(transshipment3STD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3STD",OldValue=transshipment3STD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3STD=value;
		   }
			
		 }
	   }
	  private string transshipment3STA ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment3STA  
	   {
	    
	     get
		{
		   return transshipment3STA;
		 }
		 set
		 {
		   if(transshipment3STA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment3STA",OldValue=transshipment3STA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment3STA=value;
		   }
			
		 }
	   }

	   private List<ShipmentFollowUpPM> followUps;
	    
       [Composition]
       [Include]
	   [Association(FollowUpShipment, Id,ShipmentId)]
	   [DataMember]
	   public virtual List<ShipmentFollowUpPM> FollowUps  
	   {
	        get
             {
                 if (followUps == null)
                 {
                     followUps = new List<ShipmentFollowUpPM>();
                 }
                 return followUps;
              }
             set { followUps = value; }
	    }
	   
	   private List<ShipmentFollowUpPM>  deletedFollowUps;
	   public virtual List<ShipmentFollowUpPM> DeletedFollowUps  
	   {
	        get
             {
                 if ( deletedFollowUps == null)
                 {
                      deletedFollowUps = new List<ShipmentFollowUpPM>();
                 }
                 return  deletedFollowUps;
              }
             set {  deletedFollowUps = value; }
	    }
	  
	   private List<ShipmentOrderPackagePM> shipmentOrderPackages;
	    
       [Composition]
       [Include]
	   [Association(ShipmentOrderPackagePMShipment, Id,ShipmentId)]
	   [DataMember]
	   public virtual List<ShipmentOrderPackagePM> ShipmentOrderPackages  
	   {
	        get
             {
                 if (shipmentOrderPackages == null)
                 {
                     shipmentOrderPackages = new List<ShipmentOrderPackagePM>();
                 }
                 return shipmentOrderPackages;
              }
             set { shipmentOrderPackages = value; }
	    }
	   
	   private List<ShipmentOrderPackagePM>  deletedShipmentOrderPackages;
	   public virtual List<ShipmentOrderPackagePM> DeletedShipmentOrderPackages  
	   {
	        get
             {
                 if ( deletedShipmentOrderPackages == null)
                 {
                      deletedShipmentOrderPackages = new List<ShipmentOrderPackagePM>();
                 }
                 return  deletedShipmentOrderPackages;
              }
             set {  deletedShipmentOrderPackages = value; }
	    }
	  
	   private List<ShipmentARInvoicePM> shipmentARInvoices;
	    
       [Composition]
       [Include]
	   [Association(ShipmentARInvoicePMShipment, Id,ShipmentId)]
	   [DataMember]
	   public virtual List<ShipmentARInvoicePM> ShipmentARInvoices  
	   {
	        get
             {
                 if (shipmentARInvoices == null)
                 {
                     shipmentARInvoices = new List<ShipmentARInvoicePM>();
                 }
                 return shipmentARInvoices;
              }
             set { shipmentARInvoices = value; }
	    }
	   
	   private List<ShipmentARInvoicePM>  deletedShipmentARInvoices;
	   public virtual List<ShipmentARInvoicePM> DeletedShipmentARInvoices  
	   {
	        get
             {
                 if ( deletedShipmentARInvoices == null)
                 {
                      deletedShipmentARInvoices = new List<ShipmentARInvoicePM>();
                 }
                 return  deletedShipmentARInvoices;
              }
             set {  deletedShipmentARInvoices = value; }
	    }
	  
	   private List<ShipmentAPInvoicePM> shipmentAPInvoices;
	    
       [Composition]
       [Include]
	   [Association(ShipmentAPInvoicePMShipment, Id,ShipmentId)]
	   [DataMember]
	   public virtual List<ShipmentAPInvoicePM> ShipmentAPInvoices  
	   {
	        get
             {
                 if (shipmentAPInvoices == null)
                 {
                     shipmentAPInvoices = new List<ShipmentAPInvoicePM>();
                 }
                 return shipmentAPInvoices;
              }
             set { shipmentAPInvoices = value; }
	    }
	   
	   private List<ShipmentAPInvoicePM>  deletedShipmentAPInvoices;
	   public virtual List<ShipmentAPInvoicePM> DeletedShipmentAPInvoices  
	   {
	        get
             {
                 if ( deletedShipmentAPInvoices == null)
                 {
                      deletedShipmentAPInvoices = new List<ShipmentAPInvoicePM>();
                 }
                 return  deletedShipmentAPInvoices;
              }
             set {  deletedShipmentAPInvoices = value; }
	    }
	  
	   private List<ConsoleShipmentPM> shipmentConsoleShipments;
	    
       [Composition]
       [Include]
	   [Association(ConsoleShipmentPMShipment, Id,MasterShipmentDataId)]
	   [DataMember]
	   public virtual List<ConsoleShipmentPM> ShipmentConsoleShipments  
	   {
	        get
             {
                 if (shipmentConsoleShipments == null)
                 {
                     shipmentConsoleShipments = new List<ConsoleShipmentPM>();
                 }
                 return shipmentConsoleShipments;
              }
             set { shipmentConsoleShipments = value; }
	    }
	   
	   private List<ConsoleShipmentPM>  deletedShipmentConsoleShipments;
	   public virtual List<ConsoleShipmentPM> DeletedShipmentConsoleShipments  
	   {
	        get
             {
                 if ( deletedShipmentConsoleShipments == null)
                 {
                      deletedShipmentConsoleShipments = new List<ConsoleShipmentPM>();
                 }
                 return  deletedShipmentConsoleShipments;
              }
             set {  deletedShipmentConsoleShipments = value; }
	    }
	  
	   private List<ShipmentAWBPrintOnlyPM> shipmentAWBPrintOnlies;
	    
       [Composition]
       [Include]
	   [Association(ShipmentAWBPrintOnlyPMShipment, Id,ShipmentId)]
	   [DataMember]
	   public virtual List<ShipmentAWBPrintOnlyPM> ShipmentAWBPrintOnlies  
	   {
	        get
             {
                 if (shipmentAWBPrintOnlies == null)
                 {
                     shipmentAWBPrintOnlies = new List<ShipmentAWBPrintOnlyPM>();
                 }
                 return shipmentAWBPrintOnlies;
              }
             set { shipmentAWBPrintOnlies = value; }
	    }
	   
	   private List<ShipmentAWBPrintOnlyPM>  deletedShipmentAWBPrintOnlies;
	   public virtual List<ShipmentAWBPrintOnlyPM> DeletedShipmentAWBPrintOnlies  
	   {
	        get
             {
                 if ( deletedShipmentAWBPrintOnlies == null)
                 {
                      deletedShipmentAWBPrintOnlies = new List<ShipmentAWBPrintOnlyPM>();
                 }
                 return  deletedShipmentAWBPrintOnlies;
              }
             set {  deletedShipmentAWBPrintOnlies = value; }
	    }
	  
	   private List<ShipmentCarrierStatusPM> shipmentCarrierStatuses;
	    
       [Composition]
       [Include]
	   [Association(ShipmentShipmentCarrierStatuses, Id,ShipmentId)]
	   [DataMember]
	   public virtual List<ShipmentCarrierStatusPM> ShipmentCarrierStatuses  
	   {
	        get
             {
                 if (shipmentCarrierStatuses == null)
                 {
                     shipmentCarrierStatuses = new List<ShipmentCarrierStatusPM>();
                 }
                 return shipmentCarrierStatuses;
              }
             set { shipmentCarrierStatuses = value; }
	    }
	   
	   private List<ShipmentCarrierStatusPM>  deletedShipmentCarrierStatuses;
	   public virtual List<ShipmentCarrierStatusPM> DeletedShipmentCarrierStatuses  
	   {
	        get
             {
                 if ( deletedShipmentCarrierStatuses == null)
                 {
                      deletedShipmentCarrierStatuses = new List<ShipmentCarrierStatusPM>();
                 }
                 return  deletedShipmentCarrierStatuses;
              }
             set {  deletedShipmentCarrierStatuses = value; }
	    }
	  
	   private List<AWBOCIPM> aWBOCIPMs;
	    
       [Composition]
       [Include]
	   [Association(ShipmentAWBOCI, Id,ShipmentId)]
	   [DataMember]
	   public virtual List<AWBOCIPM> AWBOCIPMs  
	   {
	        get
             {
                 if (aWBOCIPMs == null)
                 {
                     aWBOCIPMs = new List<AWBOCIPM>();
                 }
                 return aWBOCIPMs;
              }
             set { aWBOCIPMs = value; }
	    }
	   
	   private List<AWBOCIPM>  deletedAWBOCIPMs;
	   public virtual List<AWBOCIPM> DeletedAWBOCIPMs  
	   {
	        get
             {
                 if ( deletedAWBOCIPMs == null)
                 {
                      deletedAWBOCIPMs = new List<AWBOCIPM>();
                 }
                 return  deletedAWBOCIPMs;
              }
             set {  deletedAWBOCIPMs = value; }
	    }
	  
	   private List<ShipmentCommodityPM> shipmentCommodities;
	    
       [Composition]
       [Include]
	   [Association(ShipmentCommodityPMShipment, Id,ShipmentId)]
	   [DataMember]
	   public virtual List<ShipmentCommodityPM> ShipmentCommodities  
	   {
	        get
             {
                 if (shipmentCommodities == null)
                 {
                     shipmentCommodities = new List<ShipmentCommodityPM>();
                 }
                 return shipmentCommodities;
              }
             set { shipmentCommodities = value; }
	    }
	   
	   private List<ShipmentCommodityPM>  deletedShipmentCommodities;
	   public virtual List<ShipmentCommodityPM> DeletedShipmentCommodities  
	   {
	        get
             {
                 if ( deletedShipmentCommodities == null)
                 {
                      deletedShipmentCommodities = new List<ShipmentCommodityPM>();
                 }
                 return  deletedShipmentCommodities;
              }
             set {  deletedShipmentCommodities = value; }
	    }
	  	  private string markFollowUpsAsDone ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MarkFollowUpsAsDone  
	   {
	    
	     get
		{
		   return markFollowUpsAsDone;
		 }
		 set
		 {
		   if(markFollowUpsAsDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarkFollowUpsAsDone",OldValue=markFollowUpsAsDone,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   markFollowUpsAsDone=value;
		   }
			
		 }
	   }
	  private string calculateProfit ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CalculateProfit  
	   {
	    
	     get
		{
		   return calculateProfit;
		 }
		 set
		 {
		   if(calculateProfit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculateProfit",OldValue=calculateProfit,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   calculateProfit=value;
		   }
			
		 }
	   }
	  private string calculateStatus ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CalculateStatus  
	   {
	    
	     get
		{
		   return calculateStatus;
		 }
		 set
		 {
		   if(calculateStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculateStatus",OldValue=calculateStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   calculateStatus=value;
		   }
			
		 }
	   }
	  private string computedStatusName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ComputedStatusName  
	   {
	    
	     get
		{
		   return computedStatusName;
		 }
		 set
		 {
		   if(computedStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ComputedStatusName",OldValue=computedStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   computedStatusName=value;
		   }
			
		 }
	   }
	  private string customFilePocoId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomFilePocoId  
	   {
	    
	     get
		{
		   return customFilePocoId;
		 }
		 set
		 {
		   if(customFilePocoId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomFilePocoId",OldValue=customFilePocoId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customFilePocoId=value;
		   }
			
		 }
	   }
	  private string calculatePayables ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CalculatePayables  
	   {
	    
	     get
		{
		   return calculatePayables;
		 }
		 set
		 {
		   if(calculatePayables != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculatePayables",OldValue=calculatePayables,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   calculatePayables=value;
		   }
			
		 }
	   }
	  private string isAddingStackEvents ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsAddingStackEvents  
	   {
	    
	     get
		{
		   return isAddingStackEvents;
		 }
		 set
		 {
		   if(isAddingStackEvents != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAddingStackEvents",OldValue=isAddingStackEvents,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isAddingStackEvents=value;
		   }
			
		 }
	   }
	  private string isRemovingStackEvents ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsRemovingStackEvents  
	   {
	    
	     get
		{
		   return isRemovingStackEvents;
		 }
		 set
		 {
		   if(isRemovingStackEvents != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsRemovingStackEvents",OldValue=isRemovingStackEvents,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isRemovingStackEvents=value;
		   }
			
		 }
	   }
	  private string stackAirlineId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StackAirlineId  
	   {
	    
	     get
		{
		   return stackAirlineId;
		 }
		 set
		 {
		   if(stackAirlineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StackAirlineId",OldValue=stackAirlineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   stackAirlineId=value;
		   }
			
		 }
	   }
	  private string fromPartnerCity ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPartnerCity  
	   {
	    
	     get
		{
		   return fromPartnerCity;
		 }
		 set
		 {
		   if(fromPartnerCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPartnerCity",OldValue=fromPartnerCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPartnerCity=value;
		   }
			
		 }
	   }
	  private string fromPartnerCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPartnerCountryCode  
	   {
	    
	     get
		{
		   return fromPartnerCountryCode;
		 }
		 set
		 {
		   if(fromPartnerCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPartnerCountryCode",OldValue=fromPartnerCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPartnerCountryCode=value;
		   }
			
		 }
	   }
	  private string fromPartnerCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPartnerCountryName  
	   {
	    
	     get
		{
		   return fromPartnerCountryName;
		 }
		 set
		 {
		   if(fromPartnerCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPartnerCountryName",OldValue=fromPartnerCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPartnerCountryName=value;
		   }
			
		 }
	   }
	  private string toPartnerCity ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPartnerCity  
	   {
	    
	     get
		{
		   return toPartnerCity;
		 }
		 set
		 {
		   if(toPartnerCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPartnerCity",OldValue=toPartnerCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPartnerCity=value;
		   }
			
		 }
	   }
	  private string toPartnerCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPartnerCountryCode  
	   {
	    
	     get
		{
		   return toPartnerCountryCode;
		 }
		 set
		 {
		   if(toPartnerCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPartnerCountryCode",OldValue=toPartnerCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPartnerCountryCode=value;
		   }
			
		 }
	   }
	  private string toPartnerCountryName ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPartnerCountryName  
	   {
	    
	     get
		{
		   return toPartnerCountryName;
		 }
		 set
		 {
		   if(toPartnerCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPartnerCountryName",OldValue=toPartnerCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPartnerCountryName=value;
		   }
			
		 }
	   }
	  private string isSendFSRCreatingShipment ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsSendFSRCreatingShipment  
	   {
	    
	     get
		{
		   return isSendFSRCreatingShipment;
		 }
		 set
		 {
		   if(isSendFSRCreatingShipment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSendFSRCreatingShipment",OldValue=isSendFSRCreatingShipment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isSendFSRCreatingShipment=value;
		   }
			
		 }
	   }
	  private string toCountryId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToCountryId  
	   {
	    
	     get
		{
		   return toCountryId;
		 }
		 set
		 {
		   if(toCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToCountryId",OldValue=toCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toCountryId=value;
		   }
			
		 }
	   }
	  private string fromCountryId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromCountryId  
	   {
	    
	     get
		{
		   return fromCountryId;
		 }
		 set
		 {
		   if(fromCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromCountryId",OldValue=fromCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromCountryId=value;
		   }
			
		 }
	   }
	  private string toCountryIsEC ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToCountryIsEC  
	   {
	    
	     get
		{
		   return toCountryIsEC;
		 }
		 set
		 {
		   if(toCountryIsEC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToCountryIsEC",OldValue=toCountryIsEC,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toCountryIsEC=value;
		   }
			
		 }
	   }
	  private string fromCountryIsEC ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromCountryIsEC  
	   {
	    
	     get
		{
		   return fromCountryIsEC;
		 }
		 set
		 {
		   if(fromCountryIsEC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromCountryIsEC",OldValue=fromCountryIsEC,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromCountryIsEC=value;
		   }
			
		 }
	   }
	  private string copyFromShipmentId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CopyFromShipmentId  
	   {
	    
	     get
		{
		   return copyFromShipmentId;
		 }
		 set
		 {
		   if(copyFromShipmentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyFromShipmentId",OldValue=copyFromShipmentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   copyFromShipmentId=value;
		   }
			
		 }
	   }
	  private string isCopyFromShipment ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsCopyFromShipment  
	   {
	    
	     get
		{
		   return isCopyFromShipment;
		 }
		 set
		 {
		   if(isCopyFromShipment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCopyFromShipment",OldValue=isCopyFromShipment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isCopyFromShipment=value;
		   }
			
		 }
	   }
	  private string isBuildFromQuote ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsBuildFromQuote  
	   {
	    
	     get
		{
		   return isBuildFromQuote;
		 }
		 set
		 {
		   if(isBuildFromQuote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsBuildFromQuote",OldValue=isBuildFromQuote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isBuildFromQuote=value;
		   }
			
		 }
	   }
	  private string isBuildFromBooking ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsBuildFromBooking  
	   {
	    
	     get
		{
		   return isBuildFromBooking;
		 }
		 set
		 {
		   if(isBuildFromBooking != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsBuildFromBooking",OldValue=isBuildFromBooking,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isBuildFromBooking=value;
		   }
			
		 }
	   }
	  private string shipperMainAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperMainAddressId  
	   {
	    
	     get
		{
		   return shipperMainAddressId;
		 }
		 set
		 {
		   if(shipperMainAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperMainAddressId",OldValue=shipperMainAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperMainAddressId=value;
		   }
			
		 }
	   }
	  private string shipperPickAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperPickAddressId  
	   {
	    
	     get
		{
		   return shipperPickAddressId;
		 }
		 set
		 {
		   if(shipperPickAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperPickAddressId",OldValue=shipperPickAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperPickAddressId=value;
		   }
			
		 }
	   }
	  private string consigneeMainAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeMainAddressId  
	   {
	    
	     get
		{
		   return consigneeMainAddressId;
		 }
		 set
		 {
		   if(consigneeMainAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeMainAddressId",OldValue=consigneeMainAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeMainAddressId=value;
		   }
			
		 }
	   }
	  private string consigneePickAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneePickAddressId  
	   {
	    
	     get
		{
		   return consigneePickAddressId;
		 }
		 set
		 {
		   if(consigneePickAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneePickAddressId",OldValue=consigneePickAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneePickAddressId=value;
		   }
			
		 }
	   }
	  private string hasPreCarriage ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string HasPreCarriage  
	   {
	    
	     get
		{
		   return hasPreCarriage;
		 }
		 set
		 {
		   if(hasPreCarriage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasPreCarriage",OldValue=hasPreCarriage,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   hasPreCarriage=value;
		   }
			
		 }
	   }
	  private string hasOnCarriage ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string HasOnCarriage  
	   {
	    
	     get
		{
		   return hasOnCarriage;
		 }
		 set
		 {
		   if(hasOnCarriage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasOnCarriage",OldValue=hasOnCarriage,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   hasOnCarriage=value;
		   }
			
		 }
	   }
	  private string includePickUp ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IncludePickUp  
	   {
	    
	     get
		{
		   return includePickUp;
		 }
		 set
		 {
		   if(includePickUp != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncludePickUp",OldValue=includePickUp,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   includePickUp=value;
		   }
			
		 }
	   }
	  private string fromAddressCity ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromAddressCity  
	   {
	    
	     get
		{
		   return fromAddressCity;
		 }
		 set
		 {
		   if(fromAddressCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromAddressCity",OldValue=fromAddressCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromAddressCity=value;
		   }
			
		 }
	   }
	  private string fromAddressZipCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromAddressZipCode  
	   {
	    
	     get
		{
		   return fromAddressZipCode;
		 }
		 set
		 {
		   if(fromAddressZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromAddressZipCode",OldValue=fromAddressZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromAddressZipCode=value;
		   }
			
		 }
	   }
	  private string fromAddressCountryId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromAddressCountryId  
	   {
	    
	     get
		{
		   return fromAddressCountryId;
		 }
		 set
		 {
		   if(fromAddressCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromAddressCountryId",OldValue=fromAddressCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromAddressCountryId=value;
		   }
			
		 }
	   }
	  private string pickUpAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PickUpAddressId  
	   {
	    
	     get
		{
		   return pickUpAddressId;
		 }
		 set
		 {
		   if(pickUpAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickUpAddressId",OldValue=pickUpAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pickUpAddressId=value;
		   }
			
		 }
	   }
	  private string includeDelivery ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IncludeDelivery  
	   {
	    
	     get
		{
		   return includeDelivery;
		 }
		 set
		 {
		   if(includeDelivery != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncludeDelivery",OldValue=includeDelivery,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   includeDelivery=value;
		   }
			
		 }
	   }
	  private string toAddressCity ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressCity  
	   {
	    
	     get
		{
		   return toAddressCity;
		 }
		 set
		 {
		   if(toAddressCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressCity",OldValue=toAddressCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressCity=value;
		   }
			
		 }
	   }
	  private string toAddressZipCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressZipCode  
	   {
	    
	     get
		{
		   return toAddressZipCode;
		 }
		 set
		 {
		   if(toAddressZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressZipCode",OldValue=toAddressZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressZipCode=value;
		   }
			
		 }
	   }
	  private string toAddressCountryId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressCountryId  
	   {
	    
	     get
		{
		   return toAddressCountryId;
		 }
		 set
		 {
		   if(toAddressCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressCountryId",OldValue=toAddressCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressCountryId=value;
		   }
			
		 }
	   }
	  private string deliveryAddressId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliveryAddressId  
	   {
	    
	     get
		{
		   return deliveryAddressId;
		 }
		 set
		 {
		   if(deliveryAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveryAddressId",OldValue=deliveryAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliveryAddressId=value;
		   }
			
		 }
	   }
	  private string quantity1 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Quantity1  
	   {
	    
	     get
		{
		   return quantity1;
		 }
		 set
		 {
		   if(quantity1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity1",OldValue=quantity1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quantity1=value;
		   }
			
		 }
	   }
	  private string quantity2 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Quantity2  
	   {
	    
	     get
		{
		   return quantity2;
		 }
		 set
		 {
		   if(quantity2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity2",OldValue=quantity2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quantity2=value;
		   }
			
		 }
	   }
	  private string quantity3 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Quantity3  
	   {
	    
	     get
		{
		   return quantity3;
		 }
		 set
		 {
		   if(quantity3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity3",OldValue=quantity3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quantity3=value;
		   }
			
		 }
	   }
	  private string quantity4 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Quantity4  
	   {
	    
	     get
		{
		   return quantity4;
		 }
		 set
		 {
		   if(quantity4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity4",OldValue=quantity4,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quantity4=value;
		   }
			
		 }
	   }
	  private string quantity5 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Quantity5  
	   {
	    
	     get
		{
		   return quantity5;
		 }
		 set
		 {
		   if(quantity5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity5",OldValue=quantity5,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quantity5=value;
		   }
			
		 }
	   }
	  private string packageTypeId1 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeId1  
	   {
	    
	     get
		{
		   return packageTypeId1;
		 }
		 set
		 {
		   if(packageTypeId1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeId1",OldValue=packageTypeId1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeId1=value;
		   }
			
		 }
	   }
	  private string packageTypeId2 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeId2  
	   {
	    
	     get
		{
		   return packageTypeId2;
		 }
		 set
		 {
		   if(packageTypeId2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeId2",OldValue=packageTypeId2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeId2=value;
		   }
			
		 }
	   }
	  private string packageTypeId3 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeId3  
	   {
	    
	     get
		{
		   return packageTypeId3;
		 }
		 set
		 {
		   if(packageTypeId3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeId3",OldValue=packageTypeId3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeId3=value;
		   }
			
		 }
	   }
	  private string packageTypeId4 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeId4  
	   {
	    
	     get
		{
		   return packageTypeId4;
		 }
		 set
		 {
		   if(packageTypeId4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeId4",OldValue=packageTypeId4,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeId4=value;
		   }
			
		 }
	   }
	  private string packageTypeId5 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeId5  
	   {
	    
	     get
		{
		   return packageTypeId5;
		 }
		 set
		 {
		   if(packageTypeId5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeId5",OldValue=packageTypeId5,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeId5=value;
		   }
			
		 }
	   }
	  private string newConcurrencyGUID ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NewConcurrencyGUID  
	   {
	    
	     get
		{
		   return newConcurrencyGUID;
		 }
		 set
		 {
		   if(newConcurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NewConcurrencyGUID",OldValue=newConcurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   newConcurrencyGUID=value;
		   }
			
		 }
	   }
	  private int connectedShipmentsPayablesCount ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ConnectedShipmentsPayablesCount  
	   {
	    
	     get
		{
		   return connectedShipmentsPayablesCount;
		 }
		 set
		 {
		   if(connectedShipmentsPayablesCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedShipmentsPayablesCount",OldValue=connectedShipmentsPayablesCount,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   connectedShipmentsPayablesCount=value;
		   }
			
		 }
	   }
	  private int connectedShipmentsReceivablesCount ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ConnectedShipmentsReceivablesCount  
	   {
	    
	     get
		{
		   return connectedShipmentsReceivablesCount;
		 }
		 set
		 {
		   if(connectedShipmentsReceivablesCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedShipmentsReceivablesCount",OldValue=connectedShipmentsReceivablesCount,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   connectedShipmentsReceivablesCount=value;
		   }
			
		 }
	   }
	  private string isMissingDocument ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsMissingDocument  
	   {
	    
	     get
		{
		   return isMissingDocument;
		 }
		 set
		 {
		   if(isMissingDocument != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMissingDocument",OldValue=isMissingDocument,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isMissingDocument=value;
		   }
			
		 }
	   }
	  private string documentsSearchFields ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DocumentsSearchFields  
	   {
	    
	     get
		{
		   return documentsSearchFields;
		 }
		 set
		 {
		   if(documentsSearchFields != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentsSearchFields",OldValue=documentsSearchFields,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   documentsSearchFields=value;
		   }
			
		 }
	   }
	  private string shipperAddress1 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperAddress1  
	   {
	    
	     get
		{
		   return shipperAddress1;
		 }
		 set
		 {
		   if(shipperAddress1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperAddress1",OldValue=shipperAddress1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperAddress1=value;
		   }
			
		 }
	   }
	  private string shipperAddress2 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperAddress2  
	   {
	    
	     get
		{
		   return shipperAddress2;
		 }
		 set
		 {
		   if(shipperAddress2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperAddress2",OldValue=shipperAddress2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperAddress2=value;
		   }
			
		 }
	   }
	  private string shipperZipCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperZipCode  
	   {
	    
	     get
		{
		   return shipperZipCode;
		 }
		 set
		 {
		   if(shipperZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperZipCode",OldValue=shipperZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperZipCode=value;
		   }
			
		 }
	   }
	  private string shipperStateId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperStateId  
	   {
	    
	     get
		{
		   return shipperStateId;
		 }
		 set
		 {
		   if(shipperStateId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperStateId",OldValue=shipperStateId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperStateId=value;
		   }
			
		 }
	   }
	  private string shipperCountryId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperCountryId  
	   {
	    
	     get
		{
		   return shipperCountryId;
		 }
		 set
		 {
		   if(shipperCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperCountryId",OldValue=shipperCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperCountryId=value;
		   }
			
		 }
	   }
	  private string shipperCity ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperCity  
	   {
	    
	     get
		{
		   return shipperCity;
		 }
		 set
		 {
		   if(shipperCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperCity",OldValue=shipperCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperCity=value;
		   }
			
		 }
	   }
	  private string consigneeAddress1 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeAddress1  
	   {
	    
	     get
		{
		   return consigneeAddress1;
		 }
		 set
		 {
		   if(consigneeAddress1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeAddress1",OldValue=consigneeAddress1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeAddress1=value;
		   }
			
		 }
	   }
	  private string consigneeAddress2 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeAddress2  
	   {
	    
	     get
		{
		   return consigneeAddress2;
		 }
		 set
		 {
		   if(consigneeAddress2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeAddress2",OldValue=consigneeAddress2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeAddress2=value;
		   }
			
		 }
	   }
	  private string consigneeZipCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeZipCode  
	   {
	    
	     get
		{
		   return consigneeZipCode;
		 }
		 set
		 {
		   if(consigneeZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeZipCode",OldValue=consigneeZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeZipCode=value;
		   }
			
		 }
	   }
	  private string consigneeStateId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeStateId  
	   {
	    
	     get
		{
		   return consigneeStateId;
		 }
		 set
		 {
		   if(consigneeStateId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeStateId",OldValue=consigneeStateId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeStateId=value;
		   }
			
		 }
	   }
	  private string consigneeCountryId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeCountryId  
	   {
	    
	     get
		{
		   return consigneeCountryId;
		 }
		 set
		 {
		   if(consigneeCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeCountryId",OldValue=consigneeCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeCountryId=value;
		   }
			
		 }
	   }
	  private string consigneeCity ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeCity  
	   {
	    
	     get
		{
		   return consigneeCity;
		 }
		 set
		 {
		   if(consigneeCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeCity",OldValue=consigneeCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeCity=value;
		   }
			
		 }
	   }
	  private string notify1Address1 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notify1Address1  
	   {
	    
	     get
		{
		   return notify1Address1;
		 }
		 set
		 {
		   if(notify1Address1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notify1Address1",OldValue=notify1Address1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notify1Address1=value;
		   }
			
		 }
	   }
	  private string notify1Address2 ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notify1Address2  
	   {
	    
	     get
		{
		   return notify1Address2;
		 }
		 set
		 {
		   if(notify1Address2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notify1Address2",OldValue=notify1Address2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notify1Address2=value;
		   }
			
		 }
	   }
	  private string notify1ZipCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notify1ZipCode  
	   {
	    
	     get
		{
		   return notify1ZipCode;
		 }
		 set
		 {
		   if(notify1ZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notify1ZipCode",OldValue=notify1ZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notify1ZipCode=value;
		   }
			
		 }
	   }
	  private string notify1StateId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notify1StateId  
	   {
	    
	     get
		{
		   return notify1StateId;
		 }
		 set
		 {
		   if(notify1StateId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notify1StateId",OldValue=notify1StateId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notify1StateId=value;
		   }
			
		 }
	   }
	  private string notify1CountryId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notify1CountryId  
	   {
	    
	     get
		{
		   return notify1CountryId;
		 }
		 set
		 {
		   if(notify1CountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notify1CountryId",OldValue=notify1CountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notify1CountryId=value;
		   }
			
		 }
	   }
	  private string notify1City ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notify1City  
	   {
	    
	     get
		{
		   return notify1City;
		 }
		 set
		 {
		   if(notify1City != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notify1City",OldValue=notify1City,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notify1City=value;
		   }
			
		 }
	   }
	  private string mAWBReturnedToStackWithCancel ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWBReturnedToStackWithCancel  
	   {
	    
	     get
		{
		   return mAWBReturnedToStackWithCancel;
		 }
		 set
		 {
		   if(mAWBReturnedToStackWithCancel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBReturnedToStackWithCancel",OldValue=mAWBReturnedToStackWithCancel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWBReturnedToStackWithCancel=value;
		   }
			
		 }
	   }
	  private string mAWBStackAirlineId ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWBStackAirlineId  
	   {
	    
	     get
		{
		   return mAWBStackAirlineId;
		 }
		 set
		 {
		   if(mAWBStackAirlineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBStackAirlineId",OldValue=mAWBStackAirlineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWBStackAirlineId=value;
		   }
			
		 }
	   }
	  private string dontAddToImportersQueue ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DontAddToImportersQueue  
	   {
	    
	     get
		{
		   return dontAddToImportersQueue;
		 }
		 set
		 {
		   if(dontAddToImportersQueue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DontAddToImportersQueue",OldValue=dontAddToImportersQueue,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dontAddToImportersQueue=value;
		   }
			
		 }
	   }
	  private string fromCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromCountryCode  
	   {
	    
	     get
		{
		   return fromCountryCode;
		 }
		 set
		 {
		   if(fromCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromCountryCode",OldValue=fromCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromCountryCode=value;
		   }
			
		 }
	   }
	  private string toCountryCode ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToCountryCode  
	   {
	    
	     get
		{
		   return toCountryCode;
		 }
		 set
		 {
		   if(toCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToCountryCode",OldValue=toCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toCountryCode=value;
		   }
			
		 }
	   }
	  private string convertToCustomFile ;
	     
	  
       
	   [CustomValidation(typeof(EntityPMsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConvertToCustomFile  
	   {
	    
	     get
		{
		   return convertToCustomFile;
		 }
		 set
		 {
		   if(convertToCustomFile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConvertToCustomFile",OldValue=convertToCustomFile,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   convertToCustomFile=value;
		   }
			
		 }
	   }
   }
   
}
	 