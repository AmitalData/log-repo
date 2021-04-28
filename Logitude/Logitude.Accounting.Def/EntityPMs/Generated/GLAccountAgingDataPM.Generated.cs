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
   public partial class GLAccountAgingDataPM : EntityPM
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
	  private decimal? periodPast ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? PeriodPast  
	   {
	    
	     get
		{
		   return periodPast;
		 }
		 set
		 {
		   if(periodPast != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PeriodPast",OldValue=periodPast,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   periodPast=value;
		   }
			
		 }
	   }
	  private decimal? period0 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Period0  
	   {
	    
	     get
		{
		   return period0;
		 }
		 set
		 {
		   if(period0 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Period0",OldValue=period0,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   period0=value;
		   }
			
		 }
	   }
	  private decimal? period1 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Period1  
	   {
	    
	     get
		{
		   return period1;
		 }
		 set
		 {
		   if(period1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Period1",OldValue=period1,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   period1=value;
		   }
			
		 }
	   }
	  private decimal? period2 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Period2  
	   {
	    
	     get
		{
		   return period2;
		 }
		 set
		 {
		   if(period2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Period2",OldValue=period2,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   period2=value;
		   }
			
		 }
	   }
	  private decimal? period3 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Period3  
	   {
	    
	     get
		{
		   return period3;
		 }
		 set
		 {
		   if(period3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Period3",OldValue=period3,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   period3=value;
		   }
			
		 }
	   }
	  private decimal? period4 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Period4  
	   {
	    
	     get
		{
		   return period4;
		 }
		 set
		 {
		   if(period4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Period4",OldValue=period4,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   period4=value;
		   }
			
		 }
	   }
	  private decimal? period5 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Period5  
	   {
	    
	     get
		{
		   return period5;
		 }
		 set
		 {
		   if(period5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Period5",OldValue=period5,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   period5=value;
		   }
			
		 }
	   }
	  private decimal? periodFuture ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? PeriodFuture  
	   {
	    
	     get
		{
		   return periodFuture;
		 }
		 set
		 {
		   if(periodFuture != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PeriodFuture",OldValue=periodFuture,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   periodFuture=value;
		   }
			
		 }
	   }
	  private int? totalOpenTransactions ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? TotalOpenTransactions  
	   {
	    
	     get
		{
		   return totalOpenTransactions;
		 }
		 set
		 {
		   if(totalOpenTransactions != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalOpenTransactions",OldValue=totalOpenTransactions,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   totalOpenTransactions=value;
		   }
			
		 }
	   }
   }
   
}
	 