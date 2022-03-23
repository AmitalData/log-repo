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
   public partial class SupplierInvoiceFreightAmountPM : EntityPM
   {
   	  private string declarationId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationId  
	   {
	    
	     get
		{
		   return declarationId;
		 }
		 set
		 {
		   if(declarationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationId",OldValue=declarationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationId=value;
		   }
			
		 }
	   }
	  private int invoiceCounterKey ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int InvoiceCounterKey  
	   {
	    
	     get
		{
		   return invoiceCounterKey;
		 }
		 set
		 {
		   if(invoiceCounterKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceCounterKey",OldValue=invoiceCounterKey,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   invoiceCounterKey=value;
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
	  private string currencyTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyTypeCode  
	   {
	    
	     get
		{
		   return currencyTypeCode;
		 }
		 set
		 {
		   if(currencyTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyTypeCode",OldValue=currencyTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyTypeCode=value;
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
	  private string currencyTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyTypeName  
	   {
	    
	     get
		{
		   return currencyTypeName;
		 }
		 set
		 {
		   if(currencyTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyTypeName",OldValue=currencyTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyTypeName=value;
		   }
			
		 }
	   }
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
   }
   
}
	 