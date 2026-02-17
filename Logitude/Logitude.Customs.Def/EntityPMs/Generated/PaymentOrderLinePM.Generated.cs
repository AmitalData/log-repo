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
   public partial class PaymentOrderLinePM : EntityPM
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
	  private string paragraphTypeCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParagraphTypeCode  
	   {
	    
	     get
		{
		   return paragraphTypeCode;
		 }
		 set
		 {
		   if(paragraphTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParagraphTypeCode",OldValue=paragraphTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paragraphTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? amount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Amount  
	   {
	    
	     get
		{
		   return amount;
		 }
		 set
		 {
		   if(amount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Amount",OldValue=amount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   amount=value;
		   }
			
		 }
	   }
	  private string paragraphTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParagraphTypeName  
	   {
	    
	     get
		{
		   return paragraphTypeName;
		 }
		 set
		 {
		   if(paragraphTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParagraphTypeName",OldValue=paragraphTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paragraphTypeName=value;
		   }
			
		 }
	   }
   }
   
}
	 