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
   public partial class CB_QuotaDetailsHistoryPM : EntityPM
   {
   	  private string iD ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private DateTime? startDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartDate  
	   {
	    
	     get
		{
		   return startDate;
		 }
		 set
		 {
		   if(startDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDate",OldValue=startDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startDate=value;
		   }
			
		 }
	   }
	  private DateTime? endDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndDate  
	   {
	    
	     get
		{
		   return endDate;
		 }
		 set
		 {
		   if(endDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndDate",OldValue=endDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endDate=value;
		   }
			
		 }
	   }
	  private string entityStatusID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityStatusID  
	   {
	    
	     get
		{
		   return entityStatusID;
		 }
		 set
		 {
		   if(entityStatusID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityStatusID",OldValue=entityStatusID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityStatusID=value;
		   }
			
		 }
	   }
	  private bool isImportLicenseRequired ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsImportLicenseRequired  
	   {
	    
	     get
		{
		   return isImportLicenseRequired;
		 }
		 set
		 {
		   if(isImportLicenseRequired != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsImportLicenseRequired",OldValue=isImportLicenseRequired,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isImportLicenseRequired=value;
		   }
			
		 }
	   }
	  private string measurementUnitID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MeasurementUnitID  
	   {
	    
	     get
		{
		   return measurementUnitID;
		 }
		 set
		 {
		   if(measurementUnitID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeasurementUnitID",OldValue=measurementUnitID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   measurementUnitID=value;
		   }
			
		 }
	   }
	  private int? quantity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Quantity  
	   {
	    
	     get
		{
		   return quantity;
		 }
		 set
		 {
		   if(quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity",OldValue=quantity,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   quantity=value;
		   }
			
		 }
	   }
	  private string quotaComputationBasisID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuotaComputationBasisID  
	   {
	    
	     get
		{
		   return quotaComputationBasisID;
		 }
		 set
		 {
		   if(quotaComputationBasisID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuotaComputationBasisID",OldValue=quotaComputationBasisID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quotaComputationBasisID=value;
		   }
			
		 }
	   }
	  private string quotaIncrementID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuotaIncrementID  
	   {
	    
	     get
		{
		   return quotaIncrementID;
		 }
		 set
		 {
		   if(quotaIncrementID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuotaIncrementID",OldValue=quotaIncrementID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quotaIncrementID=value;
		   }
			
		 }
	   }
	  private string renewalMethodID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RenewalMethodID  
	   {
	    
	     get
		{
		   return renewalMethodID;
		 }
		 set
		 {
		   if(renewalMethodID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RenewalMethodID",OldValue=renewalMethodID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   renewalMethodID=value;
		   }
			
		 }
	   }
	  private DateTime? renewalUntilDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? RenewalUntilDate  
	   {
	    
	     get
		{
		   return renewalUntilDate;
		 }
		 set
		 {
		   if(renewalUntilDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RenewalUntilDate",OldValue=renewalUntilDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   renewalUntilDate=value;
		   }
			
		 }
	   }
	  private string quotaID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuotaID  
	   {
	    
	     get
		{
		   return quotaID;
		 }
		 set
		 {
		   if(quotaID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuotaID",OldValue=quotaID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quotaID=value;
		   }
			
		 }
	   }
	  private int changeRequestTypePriority ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ChangeRequestTypePriority  
	   {
	    
	     get
		{
		   return changeRequestTypePriority;
		 }
		 set
		 {
		   if(changeRequestTypePriority != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChangeRequestTypePriority",OldValue=changeRequestTypePriority,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   changeRequestTypePriority=value;
		   }
			
		 }
	   }
	  private decimal? quotaValueIncrement ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? QuotaValueIncrement  
	   {
	    
	     get
		{
		   return quotaValueIncrement;
		 }
		 set
		 {
		   if(quotaValueIncrement != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuotaValueIncrement",OldValue=quotaValueIncrement,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   quotaValueIncrement=value;
		   }
			
		 }
	   }
	  private string perYearFrequency ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PerYearFrequency  
	   {
	    
	     get
		{
		   return perYearFrequency;
		 }
		 set
		 {
		   if(perYearFrequency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PerYearFrequency",OldValue=perYearFrequency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   perYearFrequency=value;
		   }
			
		 }
	   }
	  private string currencyTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyTypeID  
	   {
	    
	     get
		{
		   return currencyTypeID;
		 }
		 set
		 {
		   if(currencyTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyTypeID",OldValue=currencyTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyTypeID=value;
		   }
			
		 }
	   }
   }
   
}
	 