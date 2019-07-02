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
   public partial class FullAccountingSettingPM : EntityPM
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
	  private string deductionFileNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeductionFileNumber  
	   {
	    
	     get
		{
		   return deductionFileNumber;
		 }
		 set
		 {
		   if(deductionFileNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeductionFileNumber",OldValue=deductionFileNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deductionFileNumber=value;
		   }
			
		 }
	   }
	  private string consolidationVAT ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsolidationVAT  
	   {
	    
	     get
		{
		   return consolidationVAT;
		 }
		 set
		 {
		   if(consolidationVAT != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsolidationVAT",OldValue=consolidationVAT,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consolidationVAT=value;
		   }
			
		 }
	   }
	  private string defaultVATTypeId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DefaultVATTypeId  
	   {
	    
	     get
		{
		   return defaultVATTypeId;
		 }
		 set
		 {
		   if(defaultVATTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefaultVATTypeId",OldValue=defaultVATTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   defaultVATTypeId=value;
		   }
			
		 }
	   }
	  private string vATInputsGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VATInputsGLAccountId  
	   {
	    
	     get
		{
		   return vATInputsGLAccountId;
		 }
		 set
		 {
		   if(vATInputsGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VATInputsGLAccountId",OldValue=vATInputsGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vATInputsGLAccountId=value;
		   }
			
		 }
	   }
	  private string automaticReconcileMethodId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutomaticReconcileMethodId  
	   {
	    
	     get
		{
		   return automaticReconcileMethodId;
		 }
		 set
		 {
		   if(automaticReconcileMethodId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticReconcileMethodId",OldValue=automaticReconcileMethodId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   automaticReconcileMethodId=value;
		   }
			
		 }
	   }
	  private string exchangeRateDiffGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExchangeRateDiffGLAccountId  
	   {
	    
	     get
		{
		   return exchangeRateDiffGLAccountId;
		 }
		 set
		 {
		   if(exchangeRateDiffGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExchangeRateDiffGLAccountId",OldValue=exchangeRateDiffGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exchangeRateDiffGLAccountId=value;
		   }
			
		 }
	   }
	  private string revenueExpenseGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string RevenueExpenseGLAccountId  
	   {
	    
	     get
		{
		   return revenueExpenseGLAccountId;
		 }
		 set
		 {
		   if(revenueExpenseGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RevenueExpenseGLAccountId",OldValue=revenueExpenseGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   revenueExpenseGLAccountId=value;
		   }
			
		 }
	   }
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
	  private string vATOutputGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VATOutputGLAccountId  
	   {
	    
	     get
		{
		   return vATOutputGLAccountId;
		 }
		 set
		 {
		   if(vATOutputGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VATOutputGLAccountId",OldValue=vATOutputGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vATOutputGLAccountId=value;
		   }
			
		 }
	   }
	  private string customerControlAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerControlAccountId  
	   {
	    
	     get
		{
		   return customerControlAccountId;
		 }
		 set
		 {
		   if(customerControlAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerControlAccountId",OldValue=customerControlAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerControlAccountId=value;
		   }
			
		 }
	   }
	  private string customerControlAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerControlAccountName  
	   {
	    
	     get
		{
		   return customerControlAccountName;
		 }
		 set
		 {
		   if(customerControlAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerControlAccountName",OldValue=customerControlAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerControlAccountName=value;
		   }
			
		 }
	   }
	  private string customerControlAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerControlAccountNumber  
	   {
	    
	     get
		{
		   return customerControlAccountNumber;
		 }
		 set
		 {
		   if(customerControlAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerControlAccountNumber",OldValue=customerControlAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerControlAccountNumber=value;
		   }
			
		 }
	   }
	  private string vendorControlAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorControlAccountId  
	   {
	    
	     get
		{
		   return vendorControlAccountId;
		 }
		 set
		 {
		   if(vendorControlAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorControlAccountId",OldValue=vendorControlAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorControlAccountId=value;
		   }
			
		 }
	   }
	  private string vendorControlAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorControlAccountName  
	   {
	    
	     get
		{
		   return vendorControlAccountName;
		 }
		 set
		 {
		   if(vendorControlAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorControlAccountName",OldValue=vendorControlAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorControlAccountName=value;
		   }
			
		 }
	   }
	  private string vendorControlAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorControlAccountNumber  
	   {
	    
	     get
		{
		   return vendorControlAccountNumber;
		 }
		 set
		 {
		   if(vendorControlAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorControlAccountNumber",OldValue=vendorControlAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorControlAccountNumber=value;
		   }
			
		 }
	   }
	  private string fileControlAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FileControlAccountId  
	   {
	    
	     get
		{
		   return fileControlAccountId;
		 }
		 set
		 {
		   if(fileControlAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileControlAccountId",OldValue=fileControlAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fileControlAccountId=value;
		   }
			
		 }
	   }
	  private string fileControlAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FileControlAccountName  
	   {
	    
	     get
		{
		   return fileControlAccountName;
		 }
		 set
		 {
		   if(fileControlAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileControlAccountName",OldValue=fileControlAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fileControlAccountName=value;
		   }
			
		 }
	   }
	  private string fileControlAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FileControlAccountNumber  
	   {
	    
	     get
		{
		   return fileControlAccountNumber;
		 }
		 set
		 {
		   if(fileControlAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileControlAccountNumber",OldValue=fileControlAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fileControlAccountNumber=value;
		   }
			
		 }
	   }
	  private string oceanExportJobControlAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OceanExportJobControlAccountId  
	   {
	    
	     get
		{
		   return oceanExportJobControlAccountId;
		 }
		 set
		 {
		   if(oceanExportJobControlAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OceanExportJobControlAccountId",OldValue=oceanExportJobControlAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oceanExportJobControlAccountId=value;
		   }
			
		 }
	   }
	  private string oceanExportJobControlAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OceanExportJobControlAccountName  
	   {
	    
	     get
		{
		   return oceanExportJobControlAccountName;
		 }
		 set
		 {
		   if(oceanExportJobControlAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OceanExportJobControlAccountName",OldValue=oceanExportJobControlAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oceanExportJobControlAccountName=value;
		   }
			
		 }
	   }
	  private string oceanExportJobControlAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OceanExportJobControlAccountNumber  
	   {
	    
	     get
		{
		   return oceanExportJobControlAccountNumber;
		 }
		 set
		 {
		   if(oceanExportJobControlAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OceanExportJobControlAccountNumber",OldValue=oceanExportJobControlAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oceanExportJobControlAccountNumber=value;
		   }
			
		 }
	   }
	  private string airExportJobControlAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirExportJobControlAccountId  
	   {
	    
	     get
		{
		   return airExportJobControlAccountId;
		 }
		 set
		 {
		   if(airExportJobControlAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirExportJobControlAccountId",OldValue=airExportJobControlAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airExportJobControlAccountId=value;
		   }
			
		 }
	   }
	  private string airExportJobControlAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirExportJobControlAccountName  
	   {
	    
	     get
		{
		   return airExportJobControlAccountName;
		 }
		 set
		 {
		   if(airExportJobControlAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirExportJobControlAccountName",OldValue=airExportJobControlAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airExportJobControlAccountName=value;
		   }
			
		 }
	   }
	  private string airExportJobControlAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirExportJobControlAccountNumber  
	   {
	    
	     get
		{
		   return airExportJobControlAccountNumber;
		 }
		 set
		 {
		   if(airExportJobControlAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirExportJobControlAccountNumber",OldValue=airExportJobControlAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airExportJobControlAccountNumber=value;
		   }
			
		 }
	   }
	  private string oceanImportJobControlAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OceanImportJobControlAccountId  
	   {
	    
	     get
		{
		   return oceanImportJobControlAccountId;
		 }
		 set
		 {
		   if(oceanImportJobControlAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OceanImportJobControlAccountId",OldValue=oceanImportJobControlAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oceanImportJobControlAccountId=value;
		   }
			
		 }
	   }
	  private string oceanImportJobControlAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OceanImportJobControlAccountName  
	   {
	    
	     get
		{
		   return oceanImportJobControlAccountName;
		 }
		 set
		 {
		   if(oceanImportJobControlAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OceanImportJobControlAccountName",OldValue=oceanImportJobControlAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oceanImportJobControlAccountName=value;
		   }
			
		 }
	   }
	  private string oceanImportJobControlAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OceanImportJobControlAccountNumber  
	   {
	    
	     get
		{
		   return oceanImportJobControlAccountNumber;
		 }
		 set
		 {
		   if(oceanImportJobControlAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OceanImportJobControlAccountNumber",OldValue=oceanImportJobControlAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   oceanImportJobControlAccountNumber=value;
		   }
			
		 }
	   }
	  private string airImportJobControlAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirImportJobControlAccountId  
	   {
	    
	     get
		{
		   return airImportJobControlAccountId;
		 }
		 set
		 {
		   if(airImportJobControlAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirImportJobControlAccountId",OldValue=airImportJobControlAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airImportJobControlAccountId=value;
		   }
			
		 }
	   }
	  private string airImportJobControlAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirImportJobControlAccountName  
	   {
	    
	     get
		{
		   return airImportJobControlAccountName;
		 }
		 set
		 {
		   if(airImportJobControlAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirImportJobControlAccountName",OldValue=airImportJobControlAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airImportJobControlAccountName=value;
		   }
			
		 }
	   }
	  private string airImportJobControlAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirImportJobControlAccountNumber  
	   {
	    
	     get
		{
		   return airImportJobControlAccountNumber;
		 }
		 set
		 {
		   if(airImportJobControlAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirImportJobControlAccountNumber",OldValue=airImportJobControlAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airImportJobControlAccountNumber=value;
		   }
			
		 }
	   }
	  private string tenantPaymentTermId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantPaymentTermId  
	   {
	    
	     get
		{
		   return tenantPaymentTermId;
		 }
		 set
		 {
		   if(tenantPaymentTermId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantPaymentTermId",OldValue=tenantPaymentTermId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantPaymentTermId=value;
		   }
			
		 }
	   }
	  private DateTime? accountingActivationDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? AccountingActivationDate  
	   {
	    
	     get
		{
		   return accountingActivationDate;
		 }
		 set
		 {
		   if(accountingActivationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingActivationDate",OldValue=accountingActivationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   accountingActivationDate=value;
		   }
			
		 }
	   }
	  private bool accountingActivated ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AccountingActivated  
	   {
	    
	     get
		{
		   return accountingActivated;
		 }
		 set
		 {
		   if(accountingActivated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingActivated",OldValue=accountingActivated,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   accountingActivated=value;
		   }
			
		 }
	   }
	  private string externalReconciliationDefault ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalReconciliationDefault  
	   {
	    
	     get
		{
		   return externalReconciliationDefault;
		 }
		 set
		 {
		   if(externalReconciliationDefault != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalReconciliationDefault",OldValue=externalReconciliationDefault,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalReconciliationDefault=value;
		   }
			
		 }
	   }
	  private string taxWithholdingGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaxWithholdingGLAccountId  
	   {
	    
	     get
		{
		   return taxWithholdingGLAccountId;
		 }
		 set
		 {
		   if(taxWithholdingGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxWithholdingGLAccountId",OldValue=taxWithholdingGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taxWithholdingGLAccountId=value;
		   }
			
		 }
	   }
	  private decimal? defaultTaxWithholdPercentage ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DefaultTaxWithholdPercentage  
	   {
	    
	     get
		{
		   return defaultTaxWithholdPercentage;
		 }
		 set
		 {
		   if(defaultTaxWithholdPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefaultTaxWithholdPercentage",OldValue=defaultTaxWithholdPercentage,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   defaultTaxWithholdPercentage=value;
		   }
			
		 }
	   }
	  private string customsGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsGLAccountId  
	   {
	    
	     get
		{
		   return customsGLAccountId;
		 }
		 set
		 {
		   if(customsGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsGLAccountId",OldValue=customsGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsGLAccountId=value;
		   }
			
		 }
	   }
	  private string defaultDifferencesGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DefaultDifferencesGLAccountId  
	   {
	    
	     get
		{
		   return defaultDifferencesGLAccountId;
		 }
		 set
		 {
		   if(defaultDifferencesGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefaultDifferencesGLAccountId",OldValue=defaultDifferencesGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   defaultDifferencesGLAccountId=value;
		   }
			
		 }
	   }
	  private string defaultExternalDiffGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DefaultExternalDiffGLAccountId  
	   {
	    
	     get
		{
		   return defaultExternalDiffGLAccountId;
		 }
		 set
		 {
		   if(defaultExternalDiffGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefaultExternalDiffGLAccountId",OldValue=defaultExternalDiffGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   defaultExternalDiffGLAccountId=value;
		   }
			
		 }
	   }
	  private string softwareVersion ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SoftwareVersion  
	   {
	    
	     get
		{
		   return softwareVersion;
		 }
		 set
		 {
		   if(softwareVersion != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SoftwareVersion",OldValue=softwareVersion,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   softwareVersion=value;
		   }
			
		 }
	   }
	  private bool isPaymentChequesActivated ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPaymentChequesActivated  
	   {
	    
	     get
		{
		   return isPaymentChequesActivated;
		 }
		 set
		 {
		   if(isPaymentChequesActivated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPaymentChequesActivated",OldValue=isPaymentChequesActivated,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPaymentChequesActivated=value;
		   }
			
		 }
	   }
	  private int? gLAccounterCounterLength ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? GLAccounterCounterLength  
	   {
	    
	     get
		{
		   return gLAccounterCounterLength;
		 }
		 set
		 {
		   if(gLAccounterCounterLength != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GLAccounterCounterLength",OldValue=gLAccounterCounterLength,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   gLAccounterCounterLength=value;
		   }
			
		 }
	   }
   }
   
}
	 