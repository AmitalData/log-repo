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
   public partial class PaymentOrderConnectionTablePM : EntityPM
   {
   	  private string paymentOrderId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentOrderId  
	   {
	    
	     get
		{
		   return paymentOrderId;
		 }
		 set
		 {
		   if(paymentOrderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderId",OldValue=paymentOrderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentOrderId=value;
		   }
			
		 }
	   }
	  private string connectedEntityCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConnectedEntityCode  
	   {
	    
	     get
		{
		   return connectedEntityCode;
		 }
		 set
		 {
		   if(connectedEntityCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedEntityCode",OldValue=connectedEntityCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   connectedEntityCode=value;
		   }
			
		 }
	   }
	  private string connectedEntityId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConnectedEntityId  
	   {
	    
	     get
		{
		   return connectedEntityId;
		 }
		 set
		 {
		   if(connectedEntityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedEntityId",OldValue=connectedEntityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   connectedEntityId=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
   }
   
}
	 