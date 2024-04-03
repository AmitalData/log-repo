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
   public partial class GLAccountMoreDataPM : EntityPM
   {
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
	  private decimal balanceInLocalCurrency ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal BalanceInLocalCurrency  
	   {
	    
	     get
		{
		   return balanceInLocalCurrency;
		 }
		 set
		 {
		   if(balanceInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BalanceInLocalCurrency",OldValue=balanceInLocalCurrency,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   balanceInLocalCurrency=value;
		   }
			
		 }
	   }
	  private decimal localBalanceInDue ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal LocalBalanceInDue  
	   {
	    
	     get
		{
		   return localBalanceInDue;
		 }
		 set
		 {
		   if(localBalanceInDue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalBalanceInDue",OldValue=localBalanceInDue,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   localBalanceInDue=value;
		   }
			
		 }
	   }
	  private DateTime? nextDueDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? NextDueDate  
	   {
	    
	     get
		{
		   return nextDueDate;
		 }
		 set
		 {
		   if(nextDueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextDueDate",OldValue=nextDueDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   nextDueDate=value;
		   }
			
		 }
	   }
	  private decimal? totalOpenChequesInLocalCur ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotalOpenChequesInLocalCur  
	   {
	    
	     get
		{
		   return totalOpenChequesInLocalCur;
		 }
		 set
		 {
		   if(totalOpenChequesInLocalCur != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalOpenChequesInLocalCur",OldValue=totalOpenChequesInLocalCur,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totalOpenChequesInLocalCur=value;
		   }
			
		 }
	   }
	  private decimal? totFutureOpenChequesInLocalCur ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotFutureOpenChequesInLocalCur  
	   {
	    
	     get
		{
		   return totFutureOpenChequesInLocalCur;
		 }
		 set
		 {
		   if(totFutureOpenChequesInLocalCur != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotFutureOpenChequesInLocalCur",OldValue=totFutureOpenChequesInLocalCur,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totFutureOpenChequesInLocalCur=value;
		   }
			
		 }
	   }
	  private decimal? balanceInForeignCurrency ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? BalanceInForeignCurrency  
	   {
	    
	     get
		{
		   return balanceInForeignCurrency;
		 }
		 set
		 {
		   if(balanceInForeignCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BalanceInForeignCurrency",OldValue=balanceInForeignCurrency,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   balanceInForeignCurrency=value;
		   }
			
		 }
	   }
	  private decimal? foreignBalanceInDue ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ForeignBalanceInDue  
	   {
	    
	     get
		{
		   return foreignBalanceInDue;
		 }
		 set
		 {
		   if(foreignBalanceInDue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignBalanceInDue",OldValue=foreignBalanceInDue,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   foreignBalanceInDue=value;
		   }
			
		 }
	   }
	    }
   
}
	 