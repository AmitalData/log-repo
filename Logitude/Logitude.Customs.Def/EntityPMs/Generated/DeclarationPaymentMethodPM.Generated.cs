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
   public partial class DeclarationPaymentMethodPM : EntityPM
   {
   	  private string declarationId ;
	  
       [Key]
	  
       
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
	  private int sequenceNumeric ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int SequenceNumeric  
	   {
	    
	     get
		{
		   return sequenceNumeric;
		 }
		 set
		 {
		   if(sequenceNumeric != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SequenceNumeric",OldValue=sequenceNumeric,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   sequenceNumeric=value;
		   }
			
		 }
	   }
	  private string payerActivityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PayerActivityTypeCode  
	   {
	    
	     get
		{
		   return payerActivityTypeCode;
		 }
		 set
		 {
		   if(payerActivityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PayerActivityTypeCode",OldValue=payerActivityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   payerActivityTypeCode=value;
		   }
			
		 }
	   }
	  private string methodTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MethodTypeCode  
	   {
	    
	     get
		{
		   return methodTypeCode;
		 }
		 set
		 {
		   if(methodTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MethodTypeCode",OldValue=methodTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   methodTypeCode=value;
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
	  private string payerActivityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PayerActivityTypeName  
	   {
	    
	     get
		{
		   return payerActivityTypeName;
		 }
		 set
		 {
		   if(payerActivityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PayerActivityTypeName",OldValue=payerActivityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   payerActivityTypeName=value;
		   }
			
		 }
	   }
	  private string methodTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MethodTypeName  
	   {
	    
	     get
		{
		   return methodTypeName;
		 }
		 set
		 {
		   if(methodTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MethodTypeName",OldValue=methodTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   methodTypeName=value;
		   }
			
		 }
	   }
	  private string internalBankName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InternalBankName  
	   {
	    
	     get
		{
		   return internalBankName;
		 }
		 set
		 {
		   if(internalBankName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InternalBankName",OldValue=internalBankName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   internalBankName=value;
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
	 