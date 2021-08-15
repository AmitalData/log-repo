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
using Logitude.ShipmentOrderModule.Def.Validators;
  
namespace Logitude.ShipmentOrderModule.Def.EntityPMs
{
   [CustomValidation(typeof(ShipmentOrderModuleClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class ShipmentOrderPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
		   }
			
		 }
	   }
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string orderNumber ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string OrderNumber  
	   {
	    
	     get
		{
		   return orderNumber;
		 }
		 set
		 {
		   if(orderNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OrderNumber",OldValue=orderNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   orderNumber=value;
		   }
			
		 }
	   }
	  private string transportModeId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string consigneeId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string shipperId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string agentId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string incotermId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string accountManagerId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountManagerId  
	   {
	    
	     get
		{
		   return accountManagerId;
		 }
		 set
		 {
		   if(accountManagerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountManagerId",OldValue=accountManagerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountManagerId=value;
		   }
			
		 }
	   }
	  private string pONumber ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string PONumber  
	   {
	    
	     get
		{
		   return pONumber;
		 }
		 set
		 {
		   if(pONumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PONumber",OldValue=pONumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pONumber=value;
		   }
			
		 }
	   }
	  private string descriptionOfGoods ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DescriptionOfGoods  
	   {
	    
	     get
		{
		   return descriptionOfGoods;
		 }
		 set
		 {
		   if(descriptionOfGoods != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DescriptionOfGoods",OldValue=descriptionOfGoods,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   descriptionOfGoods=value;
		   }
			
		 }
	   }
	  private string master ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string carrierNumber ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string vesselId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string VesselId  
	   {
	    
	     get
		{
		   return vesselId;
		 }
		 set
		 {
		   if(vesselId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VesselId",OldValue=vesselId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vesselId=value;
		   }
			
		 }
	   }
	  private DateTime? eTD ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ETD  
	   {
	    
	     get
		{
		   return eTD;
		 }
		 set
		 {
		   if(eTD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ETD",OldValue=eTD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   eTD=value;
		   }
			
		 }
	   }
	  private DateTime? eTA ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ETA  
	   {
	    
	     get
		{
		   return eTA;
		 }
		 set
		 {
		   if(eTA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ETA",OldValue=eTA,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   eTA=value;
		   }
			
		 }
	   }
	  private DateTime? aTD ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ATD  
	   {
	    
	     get
		{
		   return aTD;
		 }
		 set
		 {
		   if(aTD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ATD",OldValue=aTD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   aTD=value;
		   }
			
		 }
	   }
	  private DateTime? aTA ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ATA  
	   {
	    
	     get
		{
		   return aTA;
		 }
		 set
		 {
		   if(aTA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ATA",OldValue=aTA,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   aTA=value;
		   }
			
		 }
	   }
	  private string customsAgentId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsAgentId  
	   {
	    
	     get
		{
		   return customsAgentId;
		 }
		 set
		 {
		   if(customsAgentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsAgentId",OldValue=customsAgentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsAgentId=value;
		   }
			
		 }
	   }
	  private string specialServicesTypeId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string customerReferences ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerReferences  
	   {
	    
	     get
		{
		   return customerReferences;
		 }
		 set
		 {
		   if(customerReferences != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerReferences",OldValue=customerReferences,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerReferences=value;
		   }
			
		 }
	   }
	  private string transportModeName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private bool isReadyForPickup ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsReadyForPickup  
	   {
	    
	     get
		{
		   return isReadyForPickup;
		 }
		 set
		 {
		   if(isReadyForPickup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsReadyForPickup",OldValue=isReadyForPickup,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isReadyForPickup=value;
		   }
			
		 }
	   }
	  private DateTime? pickupEstimatedDateTime ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PickupEstimatedDateTime  
	   {
	    
	     get
		{
		   return pickupEstimatedDateTime;
		 }
		 set
		 {
		   if(pickupEstimatedDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupEstimatedDateTime",OldValue=pickupEstimatedDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   pickupEstimatedDateTime=value;
		   }
			
		 }
	   }
	  private DateTime? pickupActualDateTime ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PickupActualDateTime  
	   {
	    
	     get
		{
		   return pickupActualDateTime;
		 }
		 set
		 {
		   if(pickupActualDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupActualDateTime",OldValue=pickupActualDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   pickupActualDateTime=value;
		   }
			
		 }
	   }
	  private string forwarderId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForwarderId  
	   {
	    
	     get
		{
		   return forwarderId;
		 }
		 set
		 {
		   if(forwarderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForwarderId",OldValue=forwarderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   forwarderId=value;
		   }
			
		 }
	   }
	  private DateTime? bookingConfirmationDate ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? BookingConfirmationDate  
	   {
	    
	     get
		{
		   return bookingConfirmationDate;
		 }
		 set
		 {
		   if(bookingConfirmationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingConfirmationDate",OldValue=bookingConfirmationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   bookingConfirmationDate=value;
		   }
			
		 }
	   }
	  private string consigneeName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string agentName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string incotermCode ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string accountManagerName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountManagerName  
	   {
	    
	     get
		{
		   return accountManagerName;
		 }
		 set
		 {
		   if(accountManagerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountManagerName",OldValue=accountManagerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountManagerName=value;
		   }
			
		 }
	   }
	  private string vesselName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string VesselName  
	   {
	    
	     get
		{
		   return vesselName;
		 }
		 set
		 {
		   if(vesselName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VesselName",OldValue=vesselName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vesselName=value;
		   }
			
		 }
	   }
	  private string customsAgentName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsAgentName  
	   {
	    
	     get
		{
		   return customsAgentName;
		 }
		 set
		 {
		   if(customsAgentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsAgentName",OldValue=customsAgentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsAgentName=value;
		   }
			
		 }
	   }
	  private string specialServicesTypeName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string forwarderName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForwarderName  
	   {
	    
	     get
		{
		   return forwarderName;
		 }
		 set
		 {
		   if(forwarderName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForwarderName",OldValue=forwarderName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   forwarderName=value;
		   }
			
		 }
	   }
	  private string shipmentNumber ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private DateTime? supplyDateTime ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? SupplyDateTime  
	   {
	    
	     get
		{
		   return supplyDateTime;
		 }
		 set
		 {
		   if(supplyDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SupplyDateTime",OldValue=supplyDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   supplyDateTime=value;
		   }
			
		 }
	   }
	  private string originPortId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginPortId  
	   {
	    
	     get
		{
		   return originPortId;
		 }
		 set
		 {
		   if(originPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginPortId",OldValue=originPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originPortId=value;
		   }
			
		 }
	   }
	  private string destinationPortId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationPortId  
	   {
	    
	     get
		{
		   return destinationPortId;
		 }
		 set
		 {
		   if(destinationPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationPortId",OldValue=destinationPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationPortId=value;
		   }
			
		 }
	   }
	  private string gatewayId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string GatewayId  
	   {
	    
	     get
		{
		   return gatewayId;
		 }
		 set
		 {
		   if(gatewayId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GatewayId",OldValue=gatewayId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gatewayId=value;
		   }
			
		 }
	   }
	  private string casualImporterName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterName  
	   {
	    
	     get
		{
		   return casualImporterName;
		 }
		 set
		 {
		   if(casualImporterName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterName",OldValue=casualImporterName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterName=value;
		   }
			
		 }
	   }
	  private string casualSupplierName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualSupplierName  
	   {
	    
	     get
		{
		   return casualSupplierName;
		 }
		 set
		 {
		   if(casualSupplierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualSupplierName",OldValue=casualSupplierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualSupplierName=value;
		   }
			
		 }
	   }
	  private string shipmentLevelCode ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private DateTime? pODate ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PODate  
	   {
	    
	     get
		{
		   return pODate;
		 }
		 set
		 {
		   if(pODate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PODate",OldValue=pODate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   pODate=value;
		   }
			
		 }
	   }
	  private string bookingConfirmationNumber ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingConfirmationNumber  
	   {
	    
	     get
		{
		   return bookingConfirmationNumber;
		 }
		 set
		 {
		   if(bookingConfirmationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingConfirmationNumber",OldValue=bookingConfirmationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingConfirmationNumber=value;
		   }
			
		 }
	   }
	  private string originPortName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginPortName  
	   {
	    
	     get
		{
		   return originPortName;
		 }
		 set
		 {
		   if(originPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginPortName",OldValue=originPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originPortName=value;
		   }
			
		 }
	   }
	  private string destinationPortName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationPortName  
	   {
	    
	     get
		{
		   return destinationPortName;
		 }
		 set
		 {
		   if(destinationPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationPortName",OldValue=destinationPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationPortName=value;
		   }
			
		 }
	   }
	  private string gatewayName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string GatewayName  
	   {
	    
	     get
		{
		   return gatewayName;
		 }
		 set
		 {
		   if(gatewayName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GatewayName",OldValue=gatewayName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gatewayName=value;
		   }
			
		 }
	   }
	  private string directionName ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
	  private string directionId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderModuleValidationClass), "ValidateClass")]
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
   }
   
}
	 