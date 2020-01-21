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
   public partial class InterestReportPM : EntityPM
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
	  private DateTime? createDateTime ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CreateDateTime  
	   {
	    
	     get
		{
		   return createDateTime;
		 }
		 set
		 {
		   if(createDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDateTime",OldValue=createDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   createDateTime=value;
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
	  private DateTime? updateDateTime ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UpdateDateTime  
	   {
	    
	     get
		{
		   return updateDateTime;
		 }
		 set
		 {
		   if(updateDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDateTime",OldValue=updateDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   updateDateTime=value;
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
	  private string reportNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReportNumber  
	   {
	    
	     get
		{
		   return reportNumber;
		 }
		 set
		 {
		   if(reportNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReportNumber",OldValue=reportNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reportNumber=value;
		   }
			
		 }
	   }
	  private DateTime interestCalculationDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime InterestCalculationDate  
	   {
	    
	     get
		{
		   return interestCalculationDate;
		 }
		 set
		 {
		   if(interestCalculationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestCalculationDate",OldValue=interestCalculationDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   interestCalculationDate=value;
		   }
			
		 }
	   }
	  private decimal? totalAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotalAmount  
	   {
	    
	     get
		{
		   return totalAmount;
		 }
		 set
		 {
		   if(totalAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalAmount",OldValue=totalAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totalAmount=value;
		   }
			
		 }
	   }
	  private decimal? openBalance ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? OpenBalance  
	   {
	    
	     get
		{
		   return openBalance;
		 }
		 set
		 {
		   if(openBalance != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenBalance",OldValue=openBalance,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   openBalance=value;
		   }
			
		 }
	   }
	  private decimal? closeBalance ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? CloseBalance  
	   {
	    
	     get
		{
		   return closeBalance;
		 }
		 set
		 {
		   if(closeBalance != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CloseBalance",OldValue=closeBalance,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   closeBalance=value;
		   }
			
		 }
	   }
	  private string aRinvoiceId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ARinvoiceId  
	   {
	    
	     get
		{
		   return aRinvoiceId;
		 }
		 set
		 {
		   if(aRinvoiceId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ARinvoiceId",OldValue=aRinvoiceId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aRinvoiceId=value;
		   }
			
		 }
	   }
	  private decimal? invoiceAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? InvoiceAmount  
	   {
	    
	     get
		{
		   return invoiceAmount;
		 }
		 set
		 {
		   if(invoiceAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceAmount",OldValue=invoiceAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   invoiceAmount=value;
		   }
			
		 }
	   }
	  private decimal? gLAccountInterestCreditLimit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? GLAccountInterestCreditLimit  
	   {
	    
	     get
		{
		   return gLAccountInterestCreditLimit;
		 }
		 set
		 {
		   if(gLAccountInterestCreditLimit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GLAccountInterestCreditLimit",OldValue=gLAccountInterestCreditLimit,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   gLAccountInterestCreditLimit=value;
		   }
			
		 }
	   }
	  private string interestReportStatusCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterestReportStatusCode  
	   {
	    
	     get
		{
		   return interestReportStatusCode;
		 }
		 set
		 {
		   if(interestReportStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestReportStatusCode",OldValue=interestReportStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interestReportStatusCode=value;
		   }
			
		 }
	   }
	  private string createdByLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByLocalName  
	   {
	    
	     get
		{
		   return createdByLocalName;
		 }
		 set
		 {
		   if(createdByLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByLocalName",OldValue=createdByLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByLocalName=value;
		   }
			
		 }
	   }
	  private string gLAccountDisplayNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string GLAccountDisplayNumber  
	   {
	    
	     get
		{
		   return gLAccountDisplayNumber;
		 }
		 set
		 {
		   if(gLAccountDisplayNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GLAccountDisplayNumber",OldValue=gLAccountDisplayNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gLAccountDisplayNumber=value;
		   }
			
		 }
	   }
	  private string gLAccountLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string GLAccountLocalName  
	   {
	    
	     get
		{
		   return gLAccountLocalName;
		 }
		 set
		 {
		   if(gLAccountLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GLAccountLocalName",OldValue=gLAccountLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gLAccountLocalName=value;
		   }
			
		 }
	   }
	  private string aRInvoiceNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ARInvoiceNumber  
	   {
	    
	     get
		{
		   return aRInvoiceNumber;
		 }
		 set
		 {
		   if(aRInvoiceNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ARInvoiceNumber",OldValue=aRInvoiceNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aRInvoiceNumber=value;
		   }
			
		 }
	   }
	  private string updatedByLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByLocalName  
	   {
	    
	     get
		{
		   return updatedByLocalName;
		 }
		 set
		 {
		   if(updatedByLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByLocalName",OldValue=updatedByLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByLocalName=value;
		   }
			
		 }
	   }
	  private string interestReportStatusName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterestReportStatusName  
	   {
	    
	     get
		{
		   return interestReportStatusName;
		 }
		 set
		 {
		   if(interestReportStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestReportStatusName",OldValue=interestReportStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interestReportStatusName=value;
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
	  private string interestReportStatusLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterestReportStatusLocalName  
	   {
	    
	     get
		{
		   return interestReportStatusLocalName;
		 }
		 set
		 {
		   if(interestReportStatusLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestReportStatusLocalName",OldValue=interestReportStatusLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interestReportStatusLocalName=value;
		   }
			
		 }
	   }
   }
   
}
	 