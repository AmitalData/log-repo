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
using Logitude.Accounting.Def.Validators;
  
namespace Logitude.Accounting.Def.EntityPMs
{
   [CustomValidation(typeof(AccountingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class InvoiceApiCommunicationLogPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private DateTime? createDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string documentId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DocumentId  
	   {
	    
	     get
		{
		   return documentId;
		 }
		 set
		 {
		   if(documentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentId",OldValue=documentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   documentId=value;
		   }
			
		 }
	   }
	  private string step ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Step  
	   {
	    
	     get
		{
		   return step;
		 }
		 set
		 {
		   if(step != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step",OldValue=step,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   step=value;
		   }
			
		 }
	   }
	  private string statusCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusCode  
	   {
	    
	     get
		{
		   return statusCode;
		 }
		 set
		 {
		   if(statusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusCode",OldValue=statusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusCode=value;
		   }
			
		 }
	   }
	  private string exception ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Exception  
	   {
	    
	     get
		{
		   return exception;
		 }
		 set
		 {
		   if(exception != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Exception",OldValue=exception,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exception=value;
		   }
			
		 }
	   }
	  private string statusName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string stepName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StepName  
	   {
	    
	     get
		{
		   return stepName;
		 }
		 set
		 {
		   if(stepName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StepName",OldValue=stepName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   stepName=value;
		   }
			
		 }
	   }
	  private string externalID ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalID  
	   {
	    
	     get
		{
		   return externalID;
		 }
		 set
		 {
		   if(externalID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalID",OldValue=externalID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalID=value;
		   }
			
		 }
	   }
	  private string aRInvoiceId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ARInvoiceId  
	   {
	    
	     get
		{
		   return aRInvoiceId;
		 }
		 set
		 {
		   if(aRInvoiceId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ARInvoiceId",OldValue=aRInvoiceId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aRInvoiceId=value;
		   }
			
		 }
	   }
	  private string invoiceNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string externalInvoiceNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalInvoiceNumber  
	   {
	    
	     get
		{
		   return externalInvoiceNumber;
		 }
		 set
		 {
		   if(externalInvoiceNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalInvoiceNumber",OldValue=externalInvoiceNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalInvoiceNumber=value;
		   }
			
		 }
	   }
	    }
   
}
	 