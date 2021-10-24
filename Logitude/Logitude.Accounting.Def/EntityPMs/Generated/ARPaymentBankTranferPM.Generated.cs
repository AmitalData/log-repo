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
   public partial class ARPaymentBankTranferPM : EntityPM
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
	  private string paymentId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentId  
	   {
	    
	     get
		{
		   return paymentId;
		 }
		 set
		 {
		   if(paymentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentId",OldValue=paymentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentId=value;
		   }
			
		 }
	   }
	  private int lineNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int LineNumber  
	   {
	    
	     get
		{
		   return lineNumber;
		 }
		 set
		 {
		   if(lineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineNumber",OldValue=lineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   lineNumber=value;
		   }
			
		 }
	   }
	  private string paymentRef ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentRef  
	   {
	    
	     get
		{
		   return paymentRef;
		 }
		 set
		 {
		   if(paymentRef != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentRef",OldValue=paymentRef,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentRef=value;
		   }
			
		 }
	   }
	  private DateTime valueDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ValueDate  
	   {
	    
	     get
		{
		   return valueDate;
		 }
		 set
		 {
		   if(valueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ValueDate",OldValue=valueDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   valueDate=value;
		   }
			
		 }
	   }
	  private string bankAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankAccountId  
	   {
	    
	     get
		{
		   return bankAccountId;
		 }
		 set
		 {
		   if(bankAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankAccountId",OldValue=bankAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankAccountId=value;
		   }
			
		 }
	   }
	  private string currencyId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyId  
	   {
	    
	     get
		{
		   return currencyId;
		 }
		 set
		 {
		   if(currencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyId",OldValue=currencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyId=value;
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
	  private decimal exchageRate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ExchageRate  
	   {
	    
	     get
		{
		   return exchageRate;
		 }
		 set
		 {
		   if(exchageRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExchageRate",OldValue=exchageRate,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   exchageRate=value;
		   }
			
		 }
	   }
   }
   
}
	 