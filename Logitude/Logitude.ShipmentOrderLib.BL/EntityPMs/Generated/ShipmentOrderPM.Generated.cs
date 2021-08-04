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
using Logitude.ShipmentOrderLib.BL.Validators;
  
namespace Logitude.ShipmentOrderLib.BL.EntityPMs
{
   [CustomValidation(typeof(ShipmentOrderClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class ShipmentOrderPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  private string descriptionofGoods ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
	   [DataMember]
       public string DescriptionofGoods  
	   {
	    
	     get
		{
		   return descriptionofGoods;
		 }
		 set
		 {
		   if(descriptionofGoods != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DescriptionofGoods",OldValue=descriptionofGoods,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   descriptionofGoods=value;
		   }
			
		 }
	   }
	  private string shipmentTypeId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentTypeId  
	   {
	    
	     get
		{
		   return shipmentTypeId;
		 }
		 set
		 {
		   if(shipmentTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentTypeId",OldValue=shipmentTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentTypeId=value;
		   }
			
		 }
	   }
	  private string master ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  private string vesselId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  private DateTime eTD ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ETD  
	   {
	    
	     get
		{
		   return eTD;
		 }
		 set
		 {
		   if(eTD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ETD",OldValue=eTD,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   eTD=value;
		   }
			
		 }
	   }
	  private DateTime eTA ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ETA  
	   {
	    
	     get
		{
		   return eTA;
		 }
		 set
		 {
		   if(eTA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ETA",OldValue=eTA,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   eTA=value;
		   }
			
		 }
	   }
	  private DateTime aTD ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ATD  
	   {
	    
	     get
		{
		   return aTD;
		 }
		 set
		 {
		   if(aTD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ATD",OldValue=aTD,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   aTD=value;
		   }
			
		 }
	   }
	  private DateTime aTA ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ATA  
	   {
	    
	     get
		{
		   return aTA;
		 }
		 set
		 {
		   if(aTA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ATA",OldValue=aTA,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   aTA=value;
		   }
			
		 }
	   }
	  private string customsAgentId ;
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(ShipmentOrderValidationClass), "ValidateClass")]
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
   }
   
}
	 