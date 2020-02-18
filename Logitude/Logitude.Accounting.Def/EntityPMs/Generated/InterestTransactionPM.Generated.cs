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
   public partial class InterestTransactionPM : EntityPM
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
	  private string gLAccountId ;
	  	  
       
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
	  private string interestEntityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterestEntityTypeCode  
	   {
	    
	     get
		{
		   return interestEntityTypeCode;
		 }
		 set
		 {
		   if(interestEntityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestEntityTypeCode",OldValue=interestEntityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interestEntityTypeCode=value;
		   }
			
		 }
	   }
	  private string entityId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityId  
	   {
	    
	     get
		{
		   return entityId;
		 }
		 set
		 {
		   if(entityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityId",OldValue=entityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityId=value;
		   }
			
		 }
	   }
	  private int originalEntityLineNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int OriginalEntityLineNumber  
	   {
	    
	     get
		{
		   return originalEntityLineNumber;
		 }
		 set
		 {
		   if(originalEntityLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalEntityLineNumber",OldValue=originalEntityLineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   originalEntityLineNumber=value;
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
	  private decimal? foreignAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ForeignAmount  
	   {
	    
	     get
		{
		   return foreignAmount;
		 }
		 set
		 {
		   if(foreignAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignAmount",OldValue=foreignAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   foreignAmount=value;
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
	  private DateTime interestValueDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime InterestValueDate  
	   {
	    
	     get
		{
		   return interestValueDate;
		 }
		 set
		 {
		   if(interestValueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestValueDate",OldValue=interestValueDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   interestValueDate=value;
		   }
			
		 }
	   }
	  private string interestReportId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterestReportId  
	   {
	    
	     get
		{
		   return interestReportId;
		 }
		 set
		 {
		   if(interestReportId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestReportId",OldValue=interestReportId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interestReportId=value;
		   }
			
		 }
	   }
	  private bool isClosed ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClosed  
	   {
	    
	     get
		{
		   return isClosed;
		 }
		 set
		 {
		   if(isClosed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosed",OldValue=isClosed,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClosed=value;
		   }
			
		 }
	   }
	  private string currencyCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyCode  
	   {
	    
	     get
		{
		   return currencyCode;
		 }
		 set
		 {
		   if(currencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyCode",OldValue=currencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyCode=value;
		   }
			
		 }
	   }
	  private string interestEntityNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterestEntityNumber  
	   {
	    
	     get
		{
		   return interestEntityNumber;
		 }
		 set
		 {
		   if(interestEntityNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestEntityNumber",OldValue=interestEntityNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interestEntityNumber=value;
		   }
			
		 }
	   }
	  private string journalNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string JournalNumber  
	   {
	    
	     get
		{
		   return journalNumber;
		 }
		 set
		 {
		   if(journalNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JournalNumber",OldValue=journalNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   journalNumber=value;
		   }
			
		 }
	   }
	  private string interestEntityType ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterestEntityType  
	   {
	    
	     get
		{
		   return interestEntityType;
		 }
		 set
		 {
		   if(interestEntityType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestEntityType",OldValue=interestEntityType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interestEntityType=value;
		   }
			
		 }
	   }
	  private string interestEntityIconCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterestEntityIconCode  
	   {
	    
	     get
		{
		   return interestEntityIconCode;
		 }
		 set
		 {
		   if(interestEntityIconCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestEntityIconCode",OldValue=interestEntityIconCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interestEntityIconCode=value;
		   }
			
		 }
	   }
	  private string journalId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string JournalId  
	   {
	    
	     get
		{
		   return journalId;
		 }
		 set
		 {
		   if(journalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JournalId",OldValue=journalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   journalId=value;
		   }
			
		 }
	   }
	  private string accountEntityCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountEntityCode  
	   {
	    
	     get
		{
		   return accountEntityCode;
		 }
		 set
		 {
		   if(accountEntityCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountEntityCode",OldValue=accountEntityCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountEntityCode=value;
		   }
			
		 }
	   }
   }
   
}
	 