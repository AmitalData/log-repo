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
   public partial class CashBookLinePM : EntityPM
   {
   	  private string cashBookId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CashBookId  
	   {
	    
	     get
		{
		   return cashBookId;
		 }
		 set
		 {
		   if(cashBookId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CashBookId",OldValue=cashBookId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cashBookId=value;
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
	  private string aRPChequeId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ARPChequeId  
	   {
	    
	     get
		{
		   return aRPChequeId;
		 }
		 set
		 {
		   if(aRPChequeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ARPChequeId",OldValue=aRPChequeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aRPChequeId=value;
		   }
			
		 }
	   }
	  private string chequeNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChequeNumber  
	   {
	    
	     get
		{
		   return chequeNumber;
		 }
		 set
		 {
		   if(chequeNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChequeNumber",OldValue=chequeNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chequeNumber=value;
		   }
			
		 }
	   }
	  private bool isDeposited ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDeposited  
	   {
	    
	     get
		{
		   return isDeposited;
		 }
		 set
		 {
		   if(isDeposited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDeposited",OldValue=isDeposited,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDeposited=value;
		   }
			
		 }
	   }
	  private DateTime dueDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime DueDate  
	   {
	    
	     get
		{
		   return dueDate;
		 }
		 set
		 {
		   if(dueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DueDate",OldValue=dueDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   dueDate=value;
		   }
			
		 }
	   }
	  private decimal localAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal LocalAmount  
	   {
	    
	     get
		{
		   return localAmount;
		 }
		 set
		 {
		   if(localAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalAmount",OldValue=localAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   localAmount=value;
		   }
			
		 }
	   }
	  private string currency ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Currency  
	   {
	    
	     get
		{
		   return currency;
		 }
		 set
		 {
		   if(currency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Currency",OldValue=currency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currency=value;
		   }
			
		 }
	   }
	  private decimal foreignAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ForeignAmount  
	   {
	    
	     get
		{
		   return foreignAmount;
		 }
		 set
		 {
		   if(foreignAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignAmount",OldValue=foreignAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   foreignAmount=value;
		   }
			
		 }
	   }
	  private string accountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string bank ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Bank  
	   {
	    
	     get
		{
		   return bank;
		 }
		 set
		 {
		   if(bank != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Bank",OldValue=bank,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bank=value;
		   }
			
		 }
	   }
	  private string branch ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Branch  
	   {
	    
	     get
		{
		   return branch;
		 }
		 set
		 {
		   if(branch != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Branch",OldValue=branch,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   branch=value;
		   }
			
		 }
	   }
	  private string aRPaymentNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ARPaymentNumber  
	   {
	    
	     get
		{
		   return aRPaymentNumber;
		 }
		 set
		 {
		   if(aRPaymentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ARPaymentNumber",OldValue=aRPaymentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aRPaymentNumber=value;
		   }
			
		 }
	   }
	  private string aRPaymentId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ARPaymentId  
	   {
	    
	     get
		{
		   return aRPaymentId;
		 }
		 set
		 {
		   if(aRPaymentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ARPaymentId",OldValue=aRPaymentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aRPaymentId=value;
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
	  private string aRPChequeStatusName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ARPChequeStatusName  
	   {
	    
	     get
		{
		   return aRPChequeStatusName;
		 }
		 set
		 {
		   if(aRPChequeStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ARPChequeStatusName",OldValue=aRPChequeStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aRPChequeStatusName=value;
		   }
			
		 }
	   }
	  private string aRPChequeStatusCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ARPChequeStatusCode  
	   {
	    
	     get
		{
		   return aRPChequeStatusCode;
		 }
		 set
		 {
		   if(aRPChequeStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ARPChequeStatusCode",OldValue=aRPChequeStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aRPChequeStatusCode=value;
		   }
			
		 }
	   }
	    }
   
}
	 