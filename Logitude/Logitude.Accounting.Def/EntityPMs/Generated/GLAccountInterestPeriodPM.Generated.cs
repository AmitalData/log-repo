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
   public partial class GLAccountInterestPeriodPM : EntityPM
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
	  private int lineNumber ;
	  
       [Key]
	  
       
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
	  private string gLAccountId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string GLAccountId  
	   {
	    
	     get
		{
		   return gLAccountId;
		 }
		 set
		 {
		   if(gLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GLAccountId",OldValue=gLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gLAccountId=value;
		   }
			
		 }
	   }
	  private DateTime periodStartDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime PeriodStartDate  
	   {
	    
	     get
		{
		   return periodStartDate;
		 }
		 set
		 {
		   if(periodStartDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PeriodStartDate",OldValue=periodStartDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   periodStartDate=value;
		   }
			
		 }
	   }
	  private string standardInterestRateBaseId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StandardInterestRateBaseId  
	   {
	    
	     get
		{
		   return standardInterestRateBaseId;
		 }
		 set
		 {
		   if(standardInterestRateBaseId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StandardInterestRateBaseId",OldValue=standardInterestRateBaseId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   standardInterestRateBaseId=value;
		   }
			
		 }
	   }
	  private decimal? standardAddInterestPercent ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? StandardAddInterestPercent  
	   {
	    
	     get
		{
		   return standardAddInterestPercent;
		 }
		 set
		 {
		   if(standardAddInterestPercent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StandardAddInterestPercent",OldValue=standardAddInterestPercent,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   standardAddInterestPercent=value;
		   }
			
		 }
	   }
	  private string exceptionalInterestRateBaseId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExceptionalInterestRateBaseId  
	   {
	    
	     get
		{
		   return exceptionalInterestRateBaseId;
		 }
		 set
		 {
		   if(exceptionalInterestRateBaseId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExceptionalInterestRateBaseId",OldValue=exceptionalInterestRateBaseId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exceptionalInterestRateBaseId=value;
		   }
			
		 }
	   }
	  private decimal? exceptionalAddInterestPercent ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ExceptionalAddInterestPercent  
	   {
	    
	     get
		{
		   return exceptionalAddInterestPercent;
		 }
		 set
		 {
		   if(exceptionalAddInterestPercent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExceptionalAddInterestPercent",OldValue=exceptionalAddInterestPercent,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   exceptionalAddInterestPercent=value;
		   }
			
		 }
	   }
	  private string creditInterestRateBaseId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreditInterestRateBaseId  
	   {
	    
	     get
		{
		   return creditInterestRateBaseId;
		 }
		 set
		 {
		   if(creditInterestRateBaseId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditInterestRateBaseId",OldValue=creditInterestRateBaseId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   creditInterestRateBaseId=value;
		   }
			
		 }
	   }
	  private decimal? creditAddInterestPercent ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? CreditAddInterestPercent  
	   {
	    
	     get
		{
		   return creditAddInterestPercent;
		 }
		 set
		 {
		   if(creditAddInterestPercent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditAddInterestPercent",OldValue=creditAddInterestPercent,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   creditAddInterestPercent=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId  
	   {
	    
	     get
		{
		   return updatedByUserId;
		 }
		 set
		 {
		   if(updatedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserId",OldValue=updatedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserId=value;
		   }
			
		 }
	   }
	  private DateTime updateDateTime ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime UpdateDateTime  
	   {
	    
	     get
		{
		   return updateDateTime;
		 }
		 set
		 {
		   if(updateDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDateTime",OldValue=updateDateTime,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   updateDateTime=value;
		   }
			
		 }
	   }
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
		   }
			
		 }
	   }
	  private DateTime createDateTime ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime CreateDateTime  
	   {
	    
	     get
		{
		   return createDateTime;
		 }
		 set
		 {
		   if(createDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDateTime",OldValue=createDateTime,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   createDateTime=value;
		   }
			
		 }
	   }
	  private string updatedByUserName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserName  
	   {
	    
	     get
		{
		   return updatedByUserName;
		 }
		 set
		 {
		   if(updatedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserName",OldValue=updatedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserName=value;
		   }
			
		 }
	   }
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserName  
	   {
	    
	     get
		{
		   return createdByUserName;
		 }
		 set
		 {
		   if(createdByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserName",OldValue=createdByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserName=value;
		   }
			
		 }
	   }
	  private string exceptionalInterestRateName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExceptionalInterestRateName  
	   {
	    
	     get
		{
		   return exceptionalInterestRateName;
		 }
		 set
		 {
		   if(exceptionalInterestRateName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExceptionalInterestRateName",OldValue=exceptionalInterestRateName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exceptionalInterestRateName=value;
		   }
			
		 }
	   }
	  private string creditInterestRateBaseName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreditInterestRateBaseName  
	   {
	    
	     get
		{
		   return creditInterestRateBaseName;
		 }
		 set
		 {
		   if(creditInterestRateBaseName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditInterestRateBaseName",OldValue=creditInterestRateBaseName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   creditInterestRateBaseName=value;
		   }
			
		 }
	   }
	  private string standardInterestRateBaseName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StandardInterestRateBaseName  
	   {
	    
	     get
		{
		   return standardInterestRateBaseName;
		 }
		 set
		 {
		   if(standardInterestRateBaseName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StandardInterestRateBaseName",OldValue=standardInterestRateBaseName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   standardInterestRateBaseName=value;
		   }
			
		 }
	   }
   }
   
}
	 