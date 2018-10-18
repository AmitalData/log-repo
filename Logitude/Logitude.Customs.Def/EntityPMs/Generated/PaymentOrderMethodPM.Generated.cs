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
   public partial class PaymentOrderMethodPM : EntityPM
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
	  private int line ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int Line  
	   {
	    
	     get
		{
		   return line;
		 }
		 set
		 {
		   if(line != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Line",OldValue=line,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   line=value;
		   }
			
		 }
	   }
	  private string typeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeCode  
	   {
	    
	     get
		{
		   return typeCode;
		 }
		 set
		 {
		   if(typeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeCode",OldValue=typeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeCode=value;
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
	  private string bankCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankCode  
	   {
	    
	     get
		{
		   return bankCode;
		 }
		 set
		 {
		   if(bankCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankCode",OldValue=bankCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankCode=value;
		   }
			
		 }
	   }
	  private string branchCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BranchCode  
	   {
	    
	     get
		{
		   return branchCode;
		 }
		 set
		 {
		   if(branchCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BranchCode",OldValue=branchCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   branchCode=value;
		   }
			
		 }
	   }
	  private string accountNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string paymentMethodStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentMethodStatusCode  
	   {
	    
	     get
		{
		   return paymentMethodStatusCode;
		 }
		 set
		 {
		   if(paymentMethodStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentMethodStatusCode",OldValue=paymentMethodStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentMethodStatusCode=value;
		   }
			
		 }
	   }
	  private string internalBankId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InternalBankId  
	   {
	    
	     get
		{
		   return internalBankId;
		 }
		 set
		 {
		   if(internalBankId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InternalBankId",OldValue=internalBankId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   internalBankId=value;
		   }
			
		 }
	   }
	  private string typeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeName  
	   {
	    
	     get
		{
		   return typeName;
		 }
		 set
		 {
		   if(typeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeName",OldValue=typeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeName=value;
		   }
			
		 }
	   }
	  private string bankName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankName  
	   {
	    
	     get
		{
		   return bankName;
		 }
		 set
		 {
		   if(bankName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankName",OldValue=bankName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankName=value;
		   }
			
		 }
	   }
	  private string paymentMethodStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentMethodStatusName  
	   {
	    
	     get
		{
		   return paymentMethodStatusName;
		 }
		 set
		 {
		   if(paymentMethodStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentMethodStatusName",OldValue=paymentMethodStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentMethodStatusName=value;
		   }
			
		 }
	   }
	  private string customerActivityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerActivityTypeCode  
	   {
	    
	     get
		{
		   return customerActivityTypeCode;
		 }
		 set
		 {
		   if(customerActivityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerActivityTypeCode",OldValue=customerActivityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerActivityTypeCode=value;
		   }
			
		 }
	   }
	  private string customerActivityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerActivityTypeName  
	   {
	    
	     get
		{
		   return customerActivityTypeName;
		 }
		 set
		 {
		   if(customerActivityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerActivityTypeName",OldValue=customerActivityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerActivityTypeName=value;
		   }
			
		 }
	   }
	  private string customsBranchId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBranchId  
	   {
	    
	     get
		{
		   return customsBranchId;
		 }
		 set
		 {
		   if(customsBranchId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBranchId",OldValue=customsBranchId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBranchId=value;
		   }
			
		 }
	   }
   }
   
}
	 