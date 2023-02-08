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
   public partial class GLAccountTotalByMonthPM : EntityPM
   {
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
	  private string accountId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountId  
	   {
	    
	     get
		{
		   return accountId;
		 }
		 set
		 {
		   if(accountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountId",OldValue=accountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountId=value;
		   }
			
		 }
	   }
	  private string dateTypeCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DateTypeCode  
	   {
	    
	     get
		{
		   return dateTypeCode;
		 }
		 set
		 {
		   if(dateTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DateTypeCode",OldValue=dateTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dateTypeCode=value;
		   }
			
		 }
	   }
	  private int year ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Year  
	   {
	    
	     get
		{
		   return year;
		 }
		 set
		 {
		   if(year != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Year",OldValue=year,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   year=value;
		   }
			
		 }
	   }
	  private int month ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Month  
	   {
	    
	     get
		{
		   return month;
		 }
		 set
		 {
		   if(month != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Month",OldValue=month,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   month=value;
		   }
			
		 }
	   }
	  private string currencyId ;
	  
       [Key]
	  
       
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
	  private decimal localAmountDebit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal LocalAmountDebit  
	   {
	    
	     get
		{
		   return localAmountDebit;
		 }
		 set
		 {
		   if(localAmountDebit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalAmountDebit",OldValue=localAmountDebit,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   localAmountDebit=value;
		   }
			
		 }
	   }
	  private decimal localAmountCredit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal LocalAmountCredit  
	   {
	    
	     get
		{
		   return localAmountCredit;
		 }
		 set
		 {
		   if(localAmountCredit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalAmountCredit",OldValue=localAmountCredit,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   localAmountCredit=value;
		   }
			
		 }
	   }
	  private decimal foreignAmountDebit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ForeignAmountDebit  
	   {
	    
	     get
		{
		   return foreignAmountDebit;
		 }
		 set
		 {
		   if(foreignAmountDebit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignAmountDebit",OldValue=foreignAmountDebit,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   foreignAmountDebit=value;
		   }
			
		 }
	   }
	  private decimal foreignAmountCredit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ForeignAmountCredit  
	   {
	    
	     get
		{
		   return foreignAmountCredit;
		 }
		 set
		 {
		   if(foreignAmountCredit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignAmountCredit",OldValue=foreignAmountCredit,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   foreignAmountCredit=value;
		   }
			
		 }
	   }
	  private string currencyName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyName  
	   {
	    
	     get
		{
		   return currencyName;
		 }
		 set
		 {
		   if(currencyName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyName",OldValue=currencyName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyName=value;
		   }
			
		 }
	   }
	  private string accountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountName  
	   {
	    
	     get
		{
		   return accountName;
		 }
		 set
		 {
		   if(accountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountName",OldValue=accountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountName=value;
		   }
			
		 }
	   }
	  private decimal? localBalance ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? LocalBalance  
	   {
	    
	     get
		{
		   return localBalance;
		 }
		 set
		 {
		   if(localBalance != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalBalance",OldValue=localBalance,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   localBalance=value;
		   }
			
		 }
	   }
	  private string currencySign ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencySign  
	   {
	    
	     get
		{
		   return currencySign;
		 }
		 set
		 {
		   if(currencySign != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencySign",OldValue=currencySign,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencySign=value;
		   }
			
		 }
	   }
	  private string cardId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CardId  
	   {
	    
	     get
		{
		   return cardId;
		 }
		 set
		 {
		   if(cardId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CardId",OldValue=cardId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cardId=value;
		   }
			
		 }
	   }
	  private string accountTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountTypeCode  
	   {
	    
	     get
		{
		   return accountTypeCode;
		 }
		 set
		 {
		   if(accountTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountTypeCode",OldValue=accountTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountTypeCode=value;
		   }
			
		 }
	   }
	    }
   
}
	 