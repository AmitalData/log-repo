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
   public partial class ConfirmationNumberTokenLogPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string companyIdInvoiceProducer ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CompanyIdInvoiceProducer  
	   {
	    
	     get
		{
		   return companyIdInvoiceProducer;
		 }
		 set
		 {
		   if(companyIdInvoiceProducer != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CompanyIdInvoiceProducer",OldValue=companyIdInvoiceProducer,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   companyIdInvoiceProducer=value;
		   }
			
		 }
	   }
	  private string companyIdInvoiceRecipient ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CompanyIdInvoiceRecipient  
	   {
	    
	     get
		{
		   return companyIdInvoiceRecipient;
		 }
		 set
		 {
		   if(companyIdInvoiceRecipient != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CompanyIdInvoiceRecipient",OldValue=companyIdInvoiceRecipient,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   companyIdInvoiceRecipient=value;
		   }
			
		 }
	   }
	  private string invoiceNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InvoiceNumber  
	   {
	    
	     get
		{
		   return invoiceNumber;
		 }
		 set
		 {
		   if(invoiceNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceNumber",OldValue=invoiceNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   invoiceNumber=value;
		   }
			
		 }
	   }
	  private string callType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CallType  
	   {
	    
	     get
		{
		   return callType;
		 }
		 set
		 {
		   if(callType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CallType",OldValue=callType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   callType=value;
		   }
			
		 }
	   }
	  private int? communicationType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? CommunicationType  
	   {
	    
	     get
		{
		   return communicationType;
		 }
		 set
		 {
		   if(communicationType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CommunicationType",OldValue=communicationType,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   communicationType=value;
		   }
			
		 }
	   }
	  private string communicationLogId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CommunicationLogId  
	   {
	    
	     get
		{
		   return communicationLogId;
		 }
		 set
		 {
		   if(communicationLogId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CommunicationLogId",OldValue=communicationLogId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   communicationLogId=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	    }
   
}
	 