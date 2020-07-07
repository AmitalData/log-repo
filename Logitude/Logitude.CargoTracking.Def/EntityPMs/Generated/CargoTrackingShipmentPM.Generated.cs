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
using Logitude.CargoTracking.Def.Validators;
  
namespace Logitude.CargoTracking.Def.EntityPMs
{
   [CustomValidation(typeof(CargoTrackingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class CargoTrackingShipmentPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string entityId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityId  
	   {
	    
	     get
		{
		   return entityId;
		 }
		 set
		 {
		   if(entityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityId",OldValue=entityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityId=value;
		   }
			
		 }
	   }
	  private string forwardingShipmentHeaderId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForwardingShipmentHeaderId  
	   {
	    
	     get
		{
		   return forwardingShipmentHeaderId;
		 }
		 set
		 {
		   if(forwardingShipmentHeaderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForwardingShipmentHeaderId",OldValue=forwardingShipmentHeaderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   forwardingShipmentHeaderId=value;
		   }
			
		 }
	   }
	  private string customsShipmentHeaderId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsShipmentHeaderId  
	   {
	    
	     get
		{
		   return customsShipmentHeaderId;
		 }
		 set
		 {
		   if(customsShipmentHeaderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsShipmentHeaderId",OldValue=customsShipmentHeaderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsShipmentHeaderId=value;
		   }
			
		 }
	   }
	  private string entityType ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityType  
	   {
	    
	     get
		{
		   return entityType;
		 }
		 set
		 {
		   if(entityType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityType",OldValue=entityType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityType=value;
		   }
			
		 }
	   }
	  private string currentMilestoneCode ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrentMilestoneCode  
	   {
	    
	     get
		{
		   return currentMilestoneCode;
		 }
		 set
		 {
		   if(currentMilestoneCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrentMilestoneCode",OldValue=currentMilestoneCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currentMilestoneCode=value;
		   }
			
		 }
	   }
	  private DateTime? currentMilestoneDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CurrentMilestoneDate  
	   {
	    
	     get
		{
		   return currentMilestoneDate;
		 }
		 set
		 {
		   if(currentMilestoneDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrentMilestoneDate",OldValue=currentMilestoneDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   currentMilestoneDate=value;
		   }
			
		 }
	   }
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string transportModeId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string master ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string house ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string shipmentNumber ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string fromPortId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string shipperId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string consigneeId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private double? grossWeight ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public double? GrossWeight  
	   {
	    
	     get
		{
		   return grossWeight;
		 }
		 set
		 {
		   if(grossWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeight",OldValue=grossWeight,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   grossWeight=value;
		   }
			
		 }
	   }
	  private double? volume ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Volume  
	   {
	    
	     get
		{
		   return volume;
		 }
		 set
		 {
		   if(volume != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Volume",OldValue=volume,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   volume=value;
		   }
			
		 }
	   }
	  private bool? pickupDone ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? PickupDone  
	   {
	    
	     get
		{
		   return pickupDone;
		 }
		 set
		 {
		   if(pickupDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupDone",OldValue=pickupDone,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   pickupDone=value;
		   }
			
		 }
	   }
	  private bool? clearanceDone ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? ClearanceDone  
	   {
	    
	     get
		{
		   return clearanceDone;
		 }
		 set
		 {
		   if(clearanceDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClearanceDone",OldValue=clearanceDone,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   clearanceDone=value;
		   }
			
		 }
	   }
	  private DateTime? pickupDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PickupDate  
	   {
	    
	     get
		{
		   return pickupDate;
		 }
		 set
		 {
		   if(pickupDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupDate",OldValue=pickupDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   pickupDate=value;
		   }
			
		 }
	   }
	  private DateTime? clearanceDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ClearanceDate  
	   {
	    
	     get
		{
		   return clearanceDate;
		 }
		 set
		 {
		   if(clearanceDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClearanceDate",OldValue=clearanceDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   clearanceDate=value;
		   }
			
		 }
	   }
   }
   
}
	 