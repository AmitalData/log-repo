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
   public partial class CalculatedChartsOfAccountsLinePM : EntityPM
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
	  private DateTime updatedDateTime ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime UpdatedDateTime  
	   {
	    
	     get
		{
		   return updatedDateTime;
		 }
		 set
		 {
		   if(updatedDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedDateTime",OldValue=updatedDateTime,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   updatedDateTime=value;
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
	  private bool isDetailedGLAccount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDetailedGLAccount  
	   {
	    
	     get
		{
		   return isDetailedGLAccount;
		 }
		 set
		 {
		   if(isDetailedGLAccount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDetailedGLAccount",OldValue=isDetailedGLAccount,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDetailedGLAccount=value;
		   }
			
		 }
	   }
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCancelled  
	   {
	    
	     get
		{
		   return isCancelled;
		 }
		 set
		 {
		   if(isCancelled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancelled",OldValue=isCancelled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCancelled=value;
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
	  private string chartOfAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartOfAccountId  
	   {
	    
	     get
		{
		   return chartOfAccountId;
		 }
		 set
		 {
		   if(chartOfAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartOfAccountId",OldValue=chartOfAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartOfAccountId=value;
		   }
			
		 }
	   }
	  private string lineTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LineTypeCode  
	   {
	    
	     get
		{
		   return lineTypeCode;
		 }
		 set
		 {
		   if(lineTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineTypeCode",OldValue=lineTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lineTypeCode=value;
		   }
			
		 }
	   }
	  private string calculatedChartsOfAccountsId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CalculatedChartsOfAccountsId  
	   {
	    
	     get
		{
		   return calculatedChartsOfAccountsId;
		 }
		 set
		 {
		   if(calculatedChartsOfAccountsId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculatedChartsOfAccountsId",OldValue=calculatedChartsOfAccountsId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   calculatedChartsOfAccountsId=value;
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
	  private string createdByEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByEnglishName  
	   {
	    
	     get
		{
		   return createdByEnglishName;
		 }
		 set
		 {
		   if(createdByEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByEnglishName",OldValue=createdByEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByEnglishName=value;
		   }
			
		 }
	   }
	  private string updatedByEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByEnglishName  
	   {
	    
	     get
		{
		   return updatedByEnglishName;
		 }
		 set
		 {
		   if(updatedByEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByEnglishName",OldValue=updatedByEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByEnglishName=value;
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
	  private string gLAccountEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string GLAccountEnglishName  
	   {
	    
	     get
		{
		   return gLAccountEnglishName;
		 }
		 set
		 {
		   if(gLAccountEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GLAccountEnglishName",OldValue=gLAccountEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gLAccountEnglishName=value;
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
	  private string chartOfAccountEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartOfAccountEnglishName  
	   {
	    
	     get
		{
		   return chartOfAccountEnglishName;
		 }
		 set
		 {
		   if(chartOfAccountEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartOfAccountEnglishName",OldValue=chartOfAccountEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartOfAccountEnglishName=value;
		   }
			
		 }
	   }
	  private string chartOfAccountLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartOfAccountLocalName  
	   {
	    
	     get
		{
		   return chartOfAccountLocalName;
		 }
		 set
		 {
		   if(chartOfAccountLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartOfAccountLocalName",OldValue=chartOfAccountLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartOfAccountLocalName=value;
		   }
			
		 }
	   }
	  private string lineTypeEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LineTypeEnglishName  
	   {
	    
	     get
		{
		   return lineTypeEnglishName;
		 }
		 set
		 {
		   if(lineTypeEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineTypeEnglishName",OldValue=lineTypeEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lineTypeEnglishName=value;
		   }
			
		 }
	   }
	  private string lineTypeLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LineTypeLocalName  
	   {
	    
	     get
		{
		   return lineTypeLocalName;
		 }
		 set
		 {
		   if(lineTypeLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineTypeLocalName",OldValue=lineTypeLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lineTypeLocalName=value;
		   }
			
		 }
	   }
	  private int line ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string errorLog ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ErrorLog  
	   {
	    
	     get
		{
		   return errorLog;
		 }
		 set
		 {
		   if(errorLog != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ErrorLog",OldValue=errorLog,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   errorLog=value;
		   }
			
		 }
	   }
	  private string chartOfAccountIdForValidate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartOfAccountIdForValidate  
	   {
	    
	     get
		{
		   return chartOfAccountIdForValidate;
		 }
		 set
		 {
		   if(chartOfAccountIdForValidate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartOfAccountIdForValidate",OldValue=chartOfAccountIdForValidate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartOfAccountIdForValidate=value;
		   }
			
		 }
	   }
	  private string chartOfAccountTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartOfAccountTypeCode  
	   {
	    
	     get
		{
		   return chartOfAccountTypeCode;
		 }
		 set
		 {
		   if(chartOfAccountTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartOfAccountTypeCode",OldValue=chartOfAccountTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartOfAccountTypeCode=value;
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
	  private string chartsofAccountCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChartsofAccountCode  
	   {
	    
	     get
		{
		   return chartsofAccountCode;
		 }
		 set
		 {
		   if(chartsofAccountCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChartsofAccountCode",OldValue=chartsofAccountCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chartsofAccountCode=value;
		   }
			
		 }
	   }
	    }
   
}
	 