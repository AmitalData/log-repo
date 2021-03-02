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
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string securityKey ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SecurityKey  
	   {
	    
	     get
		{
		   return securityKey;
		 }
		 set
		 {
		   if(securityKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SecurityKey",OldValue=securityKey,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   securityKey=value;
		   }
			
		 }
	   }
	  private string consigneeName ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string shipperName ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string customerReference ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerReference  
	   {
	    
	     get
		{
		   return customerReference;
		 }
		 set
		 {
		   if(customerReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerReference",OldValue=customerReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerReference=value;
		   }
			
		 }
	   }
	  private bool isMainRecord ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMainRecord  
	   {
	    
	     get
		{
		   return isMainRecord;
		 }
		 set
		 {
		   if(isMainRecord != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMainRecord",OldValue=isMainRecord,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMainRecord=value;
		   }
			
		 }
	   }
	  private DateTime? pickupEstimationDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PickupEstimationDate  
	   {
	    
	     get
		{
		   return pickupEstimationDate;
		 }
		 set
		 {
		   if(pickupEstimationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupEstimationDate",OldValue=pickupEstimationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   pickupEstimationDate=value;
		   }
			
		 }
	   }
	  private DateTime? fromWarehouseDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FromWarehouseDate  
	   {
	    
	     get
		{
		   return fromWarehouseDate;
		 }
		 set
		 {
		   if(fromWarehouseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromWarehouseDate",OldValue=fromWarehouseDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fromWarehouseDate=value;
		   }
			
		 }
	   }
	  private DateTime? fromWarehouseEstimationDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FromWarehouseEstimationDate  
	   {
	    
	     get
		{
		   return fromWarehouseEstimationDate;
		 }
		 set
		 {
		   if(fromWarehouseEstimationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromWarehouseEstimationDate",OldValue=fromWarehouseEstimationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fromWarehouseEstimationDate=value;
		   }
			
		 }
	   }
	  private string fromWarehouseNotes ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromWarehouseNotes  
	   {
	    
	     get
		{
		   return fromWarehouseNotes;
		 }
		 set
		 {
		   if(fromWarehouseNotes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromWarehouseNotes",OldValue=fromWarehouseNotes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromWarehouseNotes=value;
		   }
			
		 }
	   }
	  private bool? departureDone ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? DepartureDone  
	   {
	    
	     get
		{
		   return departureDone;
		 }
		 set
		 {
		   if(departureDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartureDone",OldValue=departureDone,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   departureDone=value;
		   }
			
		 }
	   }
	  private DateTime? departureDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DepartureDate  
	   {
	    
	     get
		{
		   return departureDate;
		 }
		 set
		 {
		   if(departureDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartureDate",OldValue=departureDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   departureDate=value;
		   }
			
		 }
	   }
	  private DateTime? departureEstimationDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DepartureEstimationDate  
	   {
	    
	     get
		{
		   return departureEstimationDate;
		 }
		 set
		 {
		   if(departureEstimationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartureEstimationDate",OldValue=departureEstimationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   departureEstimationDate=value;
		   }
			
		 }
	   }
	  private bool? arrivalDone ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? ArrivalDone  
	   {
	    
	     get
		{
		   return arrivalDone;
		 }
		 set
		 {
		   if(arrivalDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ArrivalDone",OldValue=arrivalDone,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   arrivalDone=value;
		   }
			
		 }
	   }
	  private DateTime? arrivalDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ArrivalDate  
	   {
	    
	     get
		{
		   return arrivalDate;
		 }
		 set
		 {
		   if(arrivalDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ArrivalDate",OldValue=arrivalDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   arrivalDate=value;
		   }
			
		 }
	   }
	  private DateTime? arrivalEstimationDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ArrivalEstimationDate  
	   {
	    
	     get
		{
		   return arrivalEstimationDate;
		 }
		 set
		 {
		   if(arrivalEstimationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ArrivalEstimationDate",OldValue=arrivalEstimationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   arrivalEstimationDate=value;
		   }
			
		 }
	   }
	  private bool? toWarehouseDone ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? ToWarehouseDone  
	   {
	    
	     get
		{
		   return toWarehouseDone;
		 }
		 set
		 {
		   if(toWarehouseDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToWarehouseDone",OldValue=toWarehouseDone,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   toWarehouseDone=value;
		   }
			
		 }
	   }
	  private DateTime? toWarehouseDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ToWarehouseDate  
	   {
	    
	     get
		{
		   return toWarehouseDate;
		 }
		 set
		 {
		   if(toWarehouseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToWarehouseDate",OldValue=toWarehouseDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   toWarehouseDate=value;
		   }
			
		 }
	   }
	  private DateTime? toWarehouseEstimationDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ToWarehouseEstimationDate  
	   {
	    
	     get
		{
		   return toWarehouseEstimationDate;
		 }
		 set
		 {
		   if(toWarehouseEstimationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToWarehouseEstimationDate",OldValue=toWarehouseEstimationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   toWarehouseEstimationDate=value;
		   }
			
		 }
	   }
	  private string toWarehouseNotes ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToWarehouseNotes  
	   {
	    
	     get
		{
		   return toWarehouseNotes;
		 }
		 set
		 {
		   if(toWarehouseNotes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToWarehouseNotes",OldValue=toWarehouseNotes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toWarehouseNotes=value;
		   }
			
		 }
	   }
	  private bool? customsPaymentDone ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? CustomsPaymentDone  
	   {
	    
	     get
		{
		   return customsPaymentDone;
		 }
		 set
		 {
		   if(customsPaymentDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsPaymentDone",OldValue=customsPaymentDone,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   customsPaymentDone=value;
		   }
			
		 }
	   }
	  private DateTime? customsPaymentDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CustomsPaymentDate  
	   {
	    
	     get
		{
		   return customsPaymentDate;
		 }
		 set
		 {
		   if(customsPaymentDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsPaymentDate",OldValue=customsPaymentDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   customsPaymentDate=value;
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
	  private bool? deliveredDone ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? DeliveredDone  
	   {
	    
	     get
		{
		   return deliveredDone;
		 }
		 set
		 {
		   if(deliveredDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveredDone",OldValue=deliveredDone,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   deliveredDone=value;
		   }
			
		 }
	   }
	  private DateTime? deliveredDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DeliveredDate  
	   {
	    
	     get
		{
		   return deliveredDate;
		 }
		 set
		 {
		   if(deliveredDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveredDate",OldValue=deliveredDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   deliveredDate=value;
		   }
			
		 }
	   }
	  private DateTime? deliveredEstimationDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DeliveredEstimationDate  
	   {
	    
	     get
		{
		   return deliveredEstimationDate;
		 }
		 set
		 {
		   if(deliveredEstimationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveredEstimationDate",OldValue=deliveredEstimationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   deliveredEstimationDate=value;
		   }
			
		 }
	   }
	  private bool? fromWarehouseDone ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? FromWarehouseDone  
	   {
	    
	     get
		{
		   return fromWarehouseDone;
		 }
		 set
		 {
		   if(fromWarehouseDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromWarehouseDone",OldValue=fromWarehouseDone,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   fromWarehouseDone=value;
		   }
			
		 }
	   }
	  private DateTime? firstPickupETD ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FirstPickupETD  
	   {
	    
	     get
		{
		   return firstPickupETD;
		 }
		 set
		 {
		   if(firstPickupETD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstPickupETD",OldValue=firstPickupETD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   firstPickupETD=value;
		   }
			
		 }
	   }
	  private DateTime? warehouseLegActualEntryDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? WarehouseLegActualEntryDate  
	   {
	    
	     get
		{
		   return warehouseLegActualEntryDate;
		 }
		 set
		 {
		   if(warehouseLegActualEntryDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WarehouseLegActualEntryDate",OldValue=warehouseLegActualEntryDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   warehouseLegActualEntryDate=value;
		   }
			
		 }
	   }
	  private DateTime? warehouseLegExpectedEntryDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? WarehouseLegExpectedEntryDate  
	   {
	    
	     get
		{
		   return warehouseLegExpectedEntryDate;
		 }
		 set
		 {
		   if(warehouseLegExpectedEntryDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WarehouseLegExpectedEntryDate",OldValue=warehouseLegExpectedEntryDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   warehouseLegExpectedEntryDate=value;
		   }
			
		 }
	   }
	  private string warehouseLegRemarks ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string WarehouseLegRemarks  
	   {
	    
	     get
		{
		   return warehouseLegRemarks;
		 }
		 set
		 {
		   if(warehouseLegRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WarehouseLegRemarks",OldValue=warehouseLegRemarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   warehouseLegRemarks=value;
		   }
			
		 }
	   }
	  private DateTime? declarationDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DeclarationDate  
	   {
	    
	     get
		{
		   return declarationDate;
		 }
		 set
		 {
		   if(declarationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationDate",OldValue=declarationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   declarationDate=value;
		   }
			
		 }
	   }
	  private DateTime? customsClearanceDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CustomsClearanceDate  
	   {
	    
	     get
		{
		   return customsClearanceDate;
		 }
		 set
		 {
		   if(customsClearanceDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsClearanceDate",OldValue=customsClearanceDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   customsClearanceDate=value;
		   }
			
		 }
	   }
	  private int id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Id  
	   {
	    
	     get
		{
		   return id;
		 }
		 set
		 {
		   if(id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Id",OldValue=id,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   id=value;
		   }
			
		 }
	   }
	  private string searchReferences ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SearchReferences  
	   {
	    
	     get
		{
		   return searchReferences;
		 }
		 set
		 {
		   if(searchReferences != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SearchReferences",OldValue=searchReferences,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   searchReferences=value;
		   }
			
		 }
	   }
	  private string fromPortName ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string toPortName ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string currentMilestoneName ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrentMilestoneName  
	   {
	    
	     get
		{
		   return currentMilestoneName;
		 }
		 set
		 {
		   if(currentMilestoneName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrentMilestoneName",OldValue=currentMilestoneName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currentMilestoneName=value;
		   }
			
		 }
	   }
	  private string transportModeName ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string fromPortCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string toPortCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private bool isFavorite ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsFavorite  
	   {
	    
	     get
		{
		   return isFavorite;
		 }
		 set
		 {
		   if(isFavorite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFavorite",OldValue=isFavorite,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isFavorite=value;
		   }
			
		 }
	   }
	  private string containersNumbers ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainersNumbers  
	   {
	    
	     get
		{
		   return containersNumbers;
		 }
		 set
		 {
		   if(containersNumbers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainersNumbers",OldValue=containersNumbers,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containersNumbers=value;
		   }
			
		 }
	   }
	  private int? packagesQuantity ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? PackagesQuantity  
	   {
	    
	     get
		{
		   return packagesQuantity;
		 }
		 set
		 {
		   if(packagesQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackagesQuantity",OldValue=packagesQuantity,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   packagesQuantity=value;
		   }
			
		 }
	   }
	  private string directionId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private bool assignedTruckerDone ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AssignedTruckerDone  
	   {
	    
	     get
		{
		   return assignedTruckerDone;
		 }
		 set
		 {
		   if(assignedTruckerDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AssignedTruckerDone",OldValue=assignedTruckerDone,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   assignedTruckerDone=value;
		   }
			
		 }
	   }
	  private DateTime? assignedTruckerDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? AssignedTruckerDate  
	   {
	    
	     get
		{
		   return assignedTruckerDate;
		 }
		 set
		 {
		   if(assignedTruckerDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AssignedTruckerDate",OldValue=assignedTruckerDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   assignedTruckerDate=value;
		   }
			
		 }
	   }
	  private DateTime? assignedTruckerEstimationDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? AssignedTruckerEstimationDate  
	   {
	    
	     get
		{
		   return assignedTruckerEstimationDate;
		 }
		 set
		 {
		   if(assignedTruckerEstimationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AssignedTruckerEstimationDate",OldValue=assignedTruckerEstimationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   assignedTruckerEstimationDate=value;
		   }
			
		 }
	   }
	  private string assignedTruckerNotes ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AssignedTruckerNotes  
	   {
	    
	     get
		{
		   return assignedTruckerNotes;
		 }
		 set
		 {
		   if(assignedTruckerNotes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AssignedTruckerNotes",OldValue=assignedTruckerNotes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   assignedTruckerNotes=value;
		   }
			
		 }
	   }
	  private string grossWeightUnitCode ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string GrossWeightUnitCode  
	   {
	    
	     get
		{
		   return grossWeightUnitCode;
		 }
		 set
		 {
		   if(grossWeightUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeightUnitCode",OldValue=grossWeightUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   grossWeightUnitCode=value;
		   }
			
		 }
	   }
   }
   
}
	 