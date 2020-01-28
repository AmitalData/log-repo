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
   public partial class DeclarationPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string customFileNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomFileNo  
	   {
	    
	     get
		{
		   return customFileNo;
		 }
		 set
		 {
		   if(customFileNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomFileNo",OldValue=customFileNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customFileNo=value;
		   }
			
		 }
	   }
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerId  
	   {
	    
	     get
		{
		   return customerId;
		 }
		 set
		 {
		   if(customerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerId",OldValue=customerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerId=value;
		   }
			
		 }
	   }
	  private string importerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterId  
	   {
	    
	     get
		{
		   return importerId;
		 }
		 set
		 {
		   if(importerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterId",OldValue=importerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerId=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string declarationNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationNumber  
	   {
	    
	     get
		{
		   return declarationNumber;
		 }
		 set
		 {
		   if(declarationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationNumber",OldValue=declarationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationNumber=value;
		   }
			
		 }
	   }
	  private string customerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerName  
	   {
	    
	     get
		{
		   return customerName;
		 }
		 set
		 {
		   if(customerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerName",OldValue=customerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerName=value;
		   }
			
		 }
	   }
	  private string versionId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VersionId  
	   {
	    
	     get
		{
		   return versionId;
		 }
		 set
		 {
		   if(versionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VersionId",OldValue=versionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   versionId=value;
		   }
			
		 }
	   }
	  private string externalDeclarationNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalDeclarationNumber  
	   {
	    
	     get
		{
		   return externalDeclarationNumber;
		 }
		 set
		 {
		   if(externalDeclarationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalDeclarationNumber",OldValue=externalDeclarationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalDeclarationNumber=value;
		   }
			
		 }
	   }
	  private string declarationOfficeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationOfficeCode  
	   {
	    
	     get
		{
		   return declarationOfficeCode;
		 }
		 set
		 {
		   if(declarationOfficeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationOfficeCode",OldValue=declarationOfficeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationOfficeCode=value;
		   }
			
		 }
	   }
	  private DateTime? taxationDateTime ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? TaxationDateTime  
	   {
	    
	     get
		{
		   return taxationDateTime;
		 }
		 set
		 {
		   if(taxationDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxationDateTime",OldValue=taxationDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   taxationDateTime=value;
		   }
			
		 }
	   }
	  private string agentId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentId  
	   {
	    
	     get
		{
		   return agentId;
		 }
		 set
		 {
		   if(agentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentId",OldValue=agentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentId=value;
		   }
			
		 }
	   }
	  private string procedureCurrentCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProcedureCurrentCode  
	   {
	    
	     get
		{
		   return procedureCurrentCode;
		 }
		 set
		 {
		   if(procedureCurrentCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProcedureCurrentCode",OldValue=procedureCurrentCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   procedureCurrentCode=value;
		   }
			
		 }
	   }
	  private string procedureCurrentName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProcedureCurrentName  
	   {
	    
	     get
		{
		   return procedureCurrentName;
		 }
		 set
		 {
		   if(procedureCurrentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProcedureCurrentName",OldValue=procedureCurrentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   procedureCurrentName=value;
		   }
			
		 }
	   }
	  private string autonomyRegionTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutonomyRegionTypeCode  
	   {
	    
	     get
		{
		   return autonomyRegionTypeCode;
		 }
		 set
		 {
		   if(autonomyRegionTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutonomyRegionTypeCode",OldValue=autonomyRegionTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   autonomyRegionTypeCode=value;
		   }
			
		 }
	   }
	  private string autonomyRegionTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutonomyRegionTypeName  
	   {
	    
	     get
		{
		   return autonomyRegionTypeName;
		 }
		 set
		 {
		   if(autonomyRegionTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutonomyRegionTypeName",OldValue=autonomyRegionTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   autonomyRegionTypeName=value;
		   }
			
		 }
	   }
	  private string importerPassCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterPassCountryCode  
	   {
	    
	     get
		{
		   return importerPassCountryCode;
		 }
		 set
		 {
		   if(importerPassCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterPassCountryCode",OldValue=importerPassCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerPassCountryCode=value;
		   }
			
		 }
	   }
	  private string importerPassCountryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterPassCountryName  
	   {
	    
	     get
		{
		   return importerPassCountryName;
		 }
		 set
		 {
		   if(importerPassCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterPassCountryName",OldValue=importerPassCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerPassCountryName=value;
		   }
			
		 }
	   }
	  private string transferImporterId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransferImporterId  
	   {
	    
	     get
		{
		   return transferImporterId;
		 }
		 set
		 {
		   if(transferImporterId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransferImporterId",OldValue=transferImporterId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transferImporterId=value;
		   }
			
		 }
	   }
	  private string transferImporterCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransferImporterCountryCode  
	   {
	    
	     get
		{
		   return transferImporterCountryCode;
		 }
		 set
		 {
		   if(transferImporterCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransferImporterCountryCode",OldValue=transferImporterCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transferImporterCountryCode=value;
		   }
			
		 }
	   }
	  private string transferImporterCountryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransferImporterCountryName  
	   {
	    
	     get
		{
		   return transferImporterCountryName;
		 }
		 set
		 {
		   if(transferImporterCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransferImporterCountryName",OldValue=transferImporterCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transferImporterCountryName=value;
		   }
			
		 }
	   }
	  private string entitleImporterId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntitleImporterId  
	   {
	    
	     get
		{
		   return entitleImporterId;
		 }
		 set
		 {
		   if(entitleImporterId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntitleImporterId",OldValue=entitleImporterId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entitleImporterId=value;
		   }
			
		 }
	   }
	  private string importerEntitlementTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterEntitlementTypeCode  
	   {
	    
	     get
		{
		   return importerEntitlementTypeCode;
		 }
		 set
		 {
		   if(importerEntitlementTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterEntitlementTypeCode",OldValue=importerEntitlementTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerEntitlementTypeCode=value;
		   }
			
		 }
	   }
	  private string importerEntitlementTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterEntitlementTypeName  
	   {
	    
	     get
		{
		   return importerEntitlementTypeName;
		 }
		 set
		 {
		   if(importerEntitlementTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterEntitlementTypeName",OldValue=importerEntitlementTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerEntitlementTypeName=value;
		   }
			
		 }
	   }
	  private string entitleImporterCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntitleImporterCountryCode  
	   {
	    
	     get
		{
		   return entitleImporterCountryCode;
		 }
		 set
		 {
		   if(entitleImporterCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntitleImporterCountryCode",OldValue=entitleImporterCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entitleImporterCountryCode=value;
		   }
			
		 }
	   }
	  private string entitleImporterCountryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntitleImporterCountryName  
	   {
	    
	     get
		{
		   return entitleImporterCountryName;
		 }
		 set
		 {
		   if(entitleImporterCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntitleImporterCountryName",OldValue=entitleImporterCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entitleImporterCountryName=value;
		   }
			
		 }
	   }
	  private string declarationDocumentId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationDocumentId  
	   {
	    
	     get
		{
		   return declarationDocumentId;
		 }
		 set
		 {
		   if(declarationDocumentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationDocumentId",OldValue=declarationDocumentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationDocumentId=value;
		   }
			
		 }
	   }
	  private string declarationDocumentTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationDocumentTypeCode  
	   {
	    
	     get
		{
		   return declarationDocumentTypeCode;
		 }
		 set
		 {
		   if(declarationDocumentTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationDocumentTypeCode",OldValue=declarationDocumentTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationDocumentTypeCode=value;
		   }
			
		 }
	   }
	  private string declarationDocumentTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationDocumentTypeName  
	   {
	    
	     get
		{
		   return declarationDocumentTypeName;
		 }
		 set
		 {
		   if(declarationDocumentTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationDocumentTypeName",OldValue=declarationDocumentTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationDocumentTypeName=value;
		   }
			
		 }
	   }
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private bool isChanged ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsChanged  
	   {
	    
	     get
		{
		   return isChanged;
		 }
		 set
		 {
		   if(isChanged != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsChanged",OldValue=isChanged,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isChanged=value;
		   }
			
		 }
	   }
	  private DateTime? paymentDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PaymentDate  
	   {
	    
	     get
		{
		   return paymentDate;
		 }
		 set
		 {
		   if(paymentDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentDate",OldValue=paymentDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   paymentDate=value;
		   }
			
		 }
	   }
	  private DateTime? hatraDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? HatraDate  
	   {
	    
	     get
		{
		   return hatraDate;
		 }
		 set
		 {
		   if(hatraDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HatraDate",OldValue=hatraDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   hatraDate=value;
		   }
			
		 }
	   }
	  private string declarationStatusTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationStatusTypeCode  
	   {
	    
	     get
		{
		   return declarationStatusTypeCode;
		 }
		 set
		 {
		   if(declarationStatusTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationStatusTypeCode",OldValue=declarationStatusTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationStatusTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? loadingFactor ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? LoadingFactor  
	   {
	    
	     get
		{
		   return loadingFactor;
		 }
		 set
		 {
		   if(loadingFactor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LoadingFactor",OldValue=loadingFactor,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   loadingFactor=value;
		   }
			
		 }
	   }
	  private decimal? dealValue ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DealValue  
	   {
	    
	     get
		{
		   return dealValue;
		 }
		 set
		 {
		   if(dealValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DealValue",OldValue=dealValue,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   dealValue=value;
		   }
			
		 }
	   }
	  private decimal? cIFValue ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? CIFValue  
	   {
	    
	     get
		{
		   return cIFValue;
		 }
		 set
		 {
		   if(cIFValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CIFValue",OldValue=cIFValue,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   cIFValue=value;
		   }
			
		 }
	   }
	  private decimal? totalTax ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotalTax  
	   {
	    
	     get
		{
		   return totalTax;
		 }
		 set
		 {
		   if(totalTax != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalTax",OldValue=totalTax,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totalTax=value;
		   }
			
		 }
	   }
	  private string declarationNumberandVersionId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationNumberandVersionId  
	   {
	    
	     get
		{
		   return declarationNumberandVersionId;
		 }
		 set
		 {
		   if(declarationNumberandVersionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationNumberandVersionId",OldValue=declarationNumberandVersionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationNumberandVersionId=value;
		   }
			
		 }
	   }

	   private List<ConsignmentPM> consignments;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("DeclarationDeclarationConsignments", "Id","DeclarationId")]
	   [DataMember]
	   public virtual List<ConsignmentPM> Consignments  
	   {
	        get
             {
                 if (consignments == null)
                 {
                     consignments = new List<ConsignmentPM>();
                 }
                 return consignments;
              }
             set { consignments = value; }
	    }
		   
	   private List<ConsignmentPM>  deletedConsignments;
	   public virtual List<ConsignmentPM> DeletedConsignments  
	   {
	        get
             {
                 if ( deletedConsignments == null)
                 {
                      deletedConsignments = new List<ConsignmentPM>();
                 }
                 return  deletedConsignments;
              }
             set {  deletedConsignments = value; }
	    }
	  
	   private List<SupplierInvoicePM> supplierInvoices;
	 
		     
	   [Include]
	   [Association("DeclarationSupplierInvoices", "Id","DeclarationId")]
	   [DataMember]
	   public virtual List<SupplierInvoicePM> SupplierInvoices  
	   {
	        get
             {
                 if (supplierInvoices == null)
                 {
                     supplierInvoices = new List<SupplierInvoicePM>();
                 }
                 return supplierInvoices;
              }
             set { supplierInvoices = value; }
	    }
		   
	   private List<SupplierInvoicePM>  deletedSupplierInvoices;
	   public virtual List<SupplierInvoicePM> DeletedSupplierInvoices  
	   {
	        get
             {
                 if ( deletedSupplierInvoices == null)
                 {
                      deletedSupplierInvoices = new List<SupplierInvoicePM>();
                 }
                 return  deletedSupplierInvoices;
              }
             set {  deletedSupplierInvoices = value; }
	    }
	  
	   private List<DeclarationTaxPM> declarationTaxes;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("DeclarationdeclarationTax", "Id","DeclarationId")]
	   [DataMember]
	   public virtual List<DeclarationTaxPM> DeclarationTaxes  
	   {
	        get
             {
                 if (declarationTaxes == null)
                 {
                     declarationTaxes = new List<DeclarationTaxPM>();
                 }
                 return declarationTaxes;
              }
             set { declarationTaxes = value; }
	    }
		   
	   private List<DeclarationTaxPM>  deletedDeclarationTaxes;
	   public virtual List<DeclarationTaxPM> DeletedDeclarationTaxes  
	   {
	        get
             {
                 if ( deletedDeclarationTaxes == null)
                 {
                      deletedDeclarationTaxes = new List<DeclarationTaxPM>();
                 }
                 return  deletedDeclarationTaxes;
              }
             set {  deletedDeclarationTaxes = value; }
	    }
	  	  private string fileState ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FileState  
	   {
	    
	     get
		{
		   return fileState;
		 }
		 set
		 {
		   if(fileState != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileState",OldValue=fileState,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fileState=value;
		   }
			
		 }
	   }
	  private string transportModeId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeId  
	   {
	    
	     get
		{
		   return transportModeId;
		 }
		 set
		 {
		   if(transportModeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeId",OldValue=transportModeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeId=value;
		   }
			
		 }
	   }
	  private string errosXml ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ErrosXml  
	   {
	    
	     get
		{
		   return errosXml;
		 }
		 set
		 {
		   if(errosXml != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ErrosXml",OldValue=errosXml,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   errosXml=value;
		   }
			
		 }
	   }
	  private string declarationOfficeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationOfficeName  
	   {
	    
	     get
		{
		   return declarationOfficeName;
		 }
		 set
		 {
		   if(declarationOfficeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationOfficeName",OldValue=declarationOfficeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationOfficeName=value;
		   }
			
		 }
	   }
	  private string importerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterName  
	   {
	    
	     get
		{
		   return importerName;
		 }
		 set
		 {
		   if(importerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterName",OldValue=importerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerName=value;
		   }
			
		 }
	   }
	  private string departmentId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepartmentId  
	   {
	    
	     get
		{
		   return departmentId;
		 }
		 set
		 {
		   if(departmentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartmentId",OldValue=departmentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   departmentId=value;
		   }
			
		 }
	   }
	  private string departmentName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepartmentName  
	   {
	    
	     get
		{
		   return departmentName;
		 }
		 set
		 {
		   if(departmentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartmentName",OldValue=departmentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   departmentName=value;
		   }
			
		 }
	   }
	  private string referentUserId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReferentUserId  
	   {
	    
	     get
		{
		   return referentUserId;
		 }
		 set
		 {
		   if(referentUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReferentUserId",OldValue=referentUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   referentUserId=value;
		   }
			
		 }
	   }
	  private string declarationStatusTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationStatusTypeName  
	   {
	    
	     get
		{
		   return declarationStatusTypeName;
		 }
		 set
		 {
		   if(declarationStatusTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationStatusTypeName",OldValue=declarationStatusTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationStatusTypeName=value;
		   }
			
		 }
	   }
	  private string storageSiteCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageSiteCode  
	   {
	    
	     get
		{
		   return storageSiteCode;
		 }
		 set
		 {
		   if(storageSiteCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageSiteCode",OldValue=storageSiteCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageSiteCode=value;
		   }
			
		 }
	   }
	  private decimal? platformFee ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? PlatformFee  
	   {
	    
	     get
		{
		   return platformFee;
		 }
		 set
		 {
		   if(platformFee != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PlatformFee",OldValue=platformFee,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   platformFee=value;
		   }
			
		 }
	   }

	   private List<DeclarationConstraintPM> declarationConstraints;
	 
		     
	   [Include]
	   [Association("DeclarationDeclarationConstraint", "Id","DeclarationID")]
	   [DataMember]
	   public virtual List<DeclarationConstraintPM> DeclarationConstraints  
	   {
	        get
             {
                 if (declarationConstraints == null)
                 {
                     declarationConstraints = new List<DeclarationConstraintPM>();
                 }
                 return declarationConstraints;
              }
             set { declarationConstraints = value; }
	    }
		   
	   private List<DeclarationConstraintPM>  deletedDeclarationConstraints;
	   public virtual List<DeclarationConstraintPM> DeletedDeclarationConstraints  
	   {
	        get
             {
                 if ( deletedDeclarationConstraints == null)
                 {
                      deletedDeclarationConstraints = new List<DeclarationConstraintPM>();
                 }
                 return  deletedDeclarationConstraints;
              }
             set {  deletedDeclarationConstraints = value; }
	    }
	  	  private string customerCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerCode  
	   {
	    
	     get
		{
		   return customerCode;
		 }
		 set
		 {
		   if(customerCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerCode",OldValue=customerCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerCode=value;
		   }
			
		 }
	   }
	  private DateTime? createDateTime ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private DateTime? updateDateTime ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string entitleImporterName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntitleImporterName  
	   {
	    
	     get
		{
		   return entitleImporterName;
		 }
		 set
		 {
		   if(entitleImporterName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntitleImporterName",OldValue=entitleImporterName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entitleImporterName=value;
		   }
			
		 }
	   }
	  private string transportModeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeName  
	   {
	    
	     get
		{
		   return transportModeName;
		 }
		 set
		 {
		   if(transportModeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeName",OldValue=transportModeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeName=value;
		   }
			
		 }
	   }
	  private string transferImporterName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransferImporterName  
	   {
	    
	     get
		{
		   return transferImporterName;
		 }
		 set
		 {
		   if(transferImporterName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransferImporterName",OldValue=transferImporterName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transferImporterName=value;
		   }
			
		 }
	   }
	  private decimal? dealValueWithoutFactor ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DealValueWithoutFactor  
	   {
	    
	     get
		{
		   return dealValueWithoutFactor;
		 }
		 set
		 {
		   if(dealValueWithoutFactor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DealValueWithoutFactor",OldValue=dealValueWithoutFactor,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   dealValueWithoutFactor=value;
		   }
			
		 }
	   }
	  private string importerCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterCode  
	   {
	    
	     get
		{
		   return importerCode;
		 }
		 set
		 {
		   if(importerCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterCode",OldValue=importerCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerCode=value;
		   }
			
		 }
	   }
	  private string transferImporterCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransferImporterCode  
	   {
	    
	     get
		{
		   return transferImporterCode;
		 }
		 set
		 {
		   if(transferImporterCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransferImporterCode",OldValue=transferImporterCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transferImporterCode=value;
		   }
			
		 }
	   }
	  private string entitleImporterCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntitleImporterCode  
	   {
	    
	     get
		{
		   return entitleImporterCode;
		 }
		 set
		 {
		   if(entitleImporterCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntitleImporterCode",OldValue=entitleImporterCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entitleImporterCode=value;
		   }
			
		 }
	   }
	  private bool markAsChanged ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool MarkAsChanged  
	   {
	    
	     get
		{
		   return markAsChanged;
		 }
		 set
		 {
		   if(markAsChanged != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarkAsChanged",OldValue=markAsChanged,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   markAsChanged=value;
		   }
			
		 }
	   }
	  private string concurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConcurrencyGUID  
	   {
	    
	     get
		{
		   return concurrencyGUID;
		 }
		 set
		 {
		   if(concurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConcurrencyGUID",OldValue=concurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   concurrencyGUID=value;
		   }
			
		 }
	   }
	  private string newConcurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NewConcurrencyGUID  
	   {
	    
	     get
		{
		   return newConcurrencyGUID;
		 }
		 set
		 {
		   if(newConcurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NewConcurrencyGUID",OldValue=newConcurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   newConcurrencyGUID=value;
		   }
			
		 }
	   }
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string importerTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterTypeCode  
	   {
	    
	     get
		{
		   return importerTypeCode;
		 }
		 set
		 {
		   if(importerTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterTypeCode",OldValue=importerTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerTypeCode=value;
		   }
			
		 }
	   }
	  private string transferImporterTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransferImporterTypeCode  
	   {
	    
	     get
		{
		   return transferImporterTypeCode;
		 }
		 set
		 {
		   if(transferImporterTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransferImporterTypeCode",OldValue=transferImporterTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transferImporterTypeCode=value;
		   }
			
		 }
	   }
	  private string entitleImporterTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntitleImporterTypeCode  
	   {
	    
	     get
		{
		   return entitleImporterTypeCode;
		 }
		 set
		 {
		   if(entitleImporterTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntitleImporterTypeCode",OldValue=entitleImporterTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entitleImporterTypeCode=value;
		   }
			
		 }
	   }
	  private string importerTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterTypeName  
	   {
	    
	     get
		{
		   return importerTypeName;
		 }
		 set
		 {
		   if(importerTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterTypeName",OldValue=importerTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerTypeName=value;
		   }
			
		 }
	   }
	  private string transferImporterTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransferImporterTypeName  
	   {
	    
	     get
		{
		   return transferImporterTypeName;
		 }
		 set
		 {
		   if(transferImporterTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransferImporterTypeName",OldValue=transferImporterTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transferImporterTypeName=value;
		   }
			
		 }
	   }
	  private string entitleImporterTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntitleImporterTypeName  
	   {
	    
	     get
		{
		   return entitleImporterTypeName;
		 }
		 set
		 {
		   if(entitleImporterTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntitleImporterTypeName",OldValue=entitleImporterTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entitleImporterTypeName=value;
		   }
			
		 }
	   }
	  private string userNotes ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UserNotes  
	   {
	    
	     get
		{
		   return userNotes;
		 }
		 set
		 {
		   if(userNotes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UserNotes",OldValue=userNotes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   userNotes=value;
		   }
			
		 }
	   }
	  private bool freightValuesFilled ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FreightValuesFilled  
	   {
	    
	     get
		{
		   return freightValuesFilled;
		 }
		 set
		 {
		   if(freightValuesFilled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreightValuesFilled",OldValue=freightValuesFilled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   freightValuesFilled=value;
		   }
			
		 }
	   }
	  private string paidDeclarationWithoutRelease ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaidDeclarationWithoutRelease  
	   {
	    
	     get
		{
		   return paidDeclarationWithoutRelease;
		 }
		 set
		 {
		   if(paidDeclarationWithoutRelease != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaidDeclarationWithoutRelease",OldValue=paidDeclarationWithoutRelease,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paidDeclarationWithoutRelease=value;
		   }
			
		 }
	   }
	  private string declarationWithoutRelease ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationWithoutRelease  
	   {
	    
	     get
		{
		   return declarationWithoutRelease;
		 }
		 set
		 {
		   if(declarationWithoutRelease != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationWithoutRelease",OldValue=declarationWithoutRelease,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationWithoutRelease=value;
		   }
			
		 }
	   }
	  private bool hasConstraint ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasConstraint  
	   {
	    
	     get
		{
		   return hasConstraint;
		 }
		 set
		 {
		   if(hasConstraint != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasConstraint",OldValue=hasConstraint,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasConstraint=value;
		   }
			
		 }
	   }
	  private string primaryInvoiceCounterKey ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PrimaryInvoiceCounterKey  
	   {
	    
	     get
		{
		   return primaryInvoiceCounterKey;
		 }
		 set
		 {
		   if(primaryInvoiceCounterKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PrimaryInvoiceCounterKey",OldValue=primaryInvoiceCounterKey,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   primaryInvoiceCounterKey=value;
		   }
			
		 }
	   }
	  private string paymentOrderNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentOrderNumber  
	   {
	    
	     get
		{
		   return paymentOrderNumber;
		 }
		 set
		 {
		   if(paymentOrderNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderNumber",OldValue=paymentOrderNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentOrderNumber=value;
		   }
			
		 }
	   }
	  private string paymentStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentStatusCode  
	   {
	    
	     get
		{
		   return paymentStatusCode;
		 }
		 set
		 {
		   if(paymentStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentStatusCode",OldValue=paymentStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentStatusCode=value;
		   }
			
		 }
	   }
	  private bool isSignedVersion ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSignedVersion  
	   {
	    
	     get
		{
		   return isSignedVersion;
		 }
		 set
		 {
		   if(isSignedVersion != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSignedVersion",OldValue=isSignedVersion,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSignedVersion=value;
		   }
			
		 }
	   }
	  private string signedByUserId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SignedByUserId  
	   {
	    
	     get
		{
		   return signedByUserId;
		 }
		 set
		 {
		   if(signedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SignedByUserId",OldValue=signedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   signedByUserId=value;
		   }
			
		 }
	   }
	  private string storageSiteName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageSiteName  
	   {
	    
	     get
		{
		   return storageSiteName;
		 }
		 set
		 {
		   if(storageSiteName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageSiteName",OldValue=storageSiteName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageSiteName=value;
		   }
			
		 }
	   }
	  private bool hasDocument ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasDocument  
	   {
	    
	     get
		{
		   return hasDocument;
		 }
		 set
		 {
		   if(hasDocument != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasDocument",OldValue=hasDocument,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasDocument=value;
		   }
			
		 }
	   }
	  private string signerPersonalId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SignerPersonalId  
	   {
	    
	     get
		{
		   return signerPersonalId;
		 }
		 set
		 {
		   if(signerPersonalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SignerPersonalId",OldValue=signerPersonalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   signerPersonalId=value;
		   }
			
		 }
	   }
	  private int? customsNumeral ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? CustomsNumeral  
	   {
	    
	     get
		{
		   return customsNumeral;
		 }
		 set
		 {
		   if(customsNumeral != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsNumeral",OldValue=customsNumeral,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   customsNumeral=value;
		   }
			
		 }
	   }
	  private bool isConvertedDeclaration ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConvertedDeclaration  
	   {
	    
	     get
		{
		   return isConvertedDeclaration;
		 }
		 set
		 {
		   if(isConvertedDeclaration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConvertedDeclaration",OldValue=isConvertedDeclaration,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConvertedDeclaration=value;
		   }
			
		 }
	   }
	  private string correctionsXml ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CorrectionsXml  
	   {
	    
	     get
		{
		   return correctionsXml;
		 }
		 set
		 {
		   if(correctionsXml != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CorrectionsXml",OldValue=correctionsXml,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   correctionsXml=value;
		   }
			
		 }
	   }
	  private bool resetDeclarationNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ResetDeclarationNumber  
	   {
	    
	     get
		{
		   return resetDeclarationNumber;
		 }
		 set
		 {
		   if(resetDeclarationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResetDeclarationNumber",OldValue=resetDeclarationNumber,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   resetDeclarationNumber=value;
		   }
			
		 }
	   }
	  private bool isCopiedFromOtherDeclaration ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCopiedFromOtherDeclaration  
	   {
	    
	     get
		{
		   return isCopiedFromOtherDeclaration;
		 }
		 set
		 {
		   if(isCopiedFromOtherDeclaration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCopiedFromOtherDeclaration",OldValue=isCopiedFromOtherDeclaration,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCopiedFromOtherDeclaration=value;
		   }
			
		 }
	   }
	  private string requestFileNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestFileNumber  
	   {
	    
	     get
		{
		   return requestFileNumber;
		 }
		 set
		 {
		   if(requestFileNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestFileNumber",OldValue=requestFileNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestFileNumber=value;
		   }
			
		 }
	   }
	  private bool isReleaseFile ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsReleaseFile  
	   {
	    
	     get
		{
		   return isReleaseFile;
		 }
		 set
		 {
		   if(isReleaseFile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsReleaseFile",OldValue=isReleaseFile,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isReleaseFile=value;
		   }
			
		 }
	   }
	  private bool vatChanged ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool VatChanged  
	   {
	    
	     get
		{
		   return vatChanged;
		 }
		 set
		 {
		   if(vatChanged != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatChanged",OldValue=vatChanged,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   vatChanged=value;
		   }
			
		 }
	   }
	  private bool isConnectedToUnifreight ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConnectedToUnifreight  
	   {
	    
	     get
		{
		   return isConnectedToUnifreight;
		 }
		 set
		 {
		   if(isConnectedToUnifreight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConnectedToUnifreight",OldValue=isConnectedToUnifreight,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConnectedToUnifreight=value;
		   }
			
		 }
	   }
	  private string mainImporterEntitlemntTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainImporterEntitlemntTypeCode  
	   {
	    
	     get
		{
		   return mainImporterEntitlemntTypeCode;
		 }
		 set
		 {
		   if(mainImporterEntitlemntTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainImporterEntitlemntTypeCode",OldValue=mainImporterEntitlemntTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainImporterEntitlemntTypeCode=value;
		   }
			
		 }
	   }
	  private string transImporterEntitleTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransImporterEntitleTypeCode  
	   {
	    
	     get
		{
		   return transImporterEntitleTypeCode;
		 }
		 set
		 {
		   if(transImporterEntitleTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransImporterEntitleTypeCode",OldValue=transImporterEntitleTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transImporterEntitleTypeCode=value;
		   }
			
		 }
	   }
	  private string importerAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterAddress  
	   {
	    
	     get
		{
		   return importerAddress;
		 }
		 set
		 {
		   if(importerAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterAddress",OldValue=importerAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerAddress=value;
		   }
			
		 }
	   }
	  private string transferImporterAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransferImporterAddress  
	   {
	    
	     get
		{
		   return transferImporterAddress;
		 }
		 set
		 {
		   if(transferImporterAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransferImporterAddress",OldValue=transferImporterAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transferImporterAddress=value;
		   }
			
		 }
	   }
	  private string entitleImporterAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntitleImporterAddress  
	   {
	    
	     get
		{
		   return entitleImporterAddress;
		 }
		 set
		 {
		   if(entitleImporterAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntitleImporterAddress",OldValue=entitleImporterAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entitleImporterAddress=value;
		   }
			
		 }
	   }
	  private string importerPassportNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterPassportNumber  
	   {
	    
	     get
		{
		   return importerPassportNumber;
		 }
		 set
		 {
		   if(importerPassportNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterPassportNumber",OldValue=importerPassportNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerPassportNumber=value;
		   }
			
		 }
	   }
	  private string transferPassportNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransferPassportNumber  
	   {
	    
	     get
		{
		   return transferPassportNumber;
		 }
		 set
		 {
		   if(transferPassportNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransferPassportNumber",OldValue=transferPassportNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transferPassportNumber=value;
		   }
			
		 }
	   }
	  private string entitlePassportNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntitlePassportNumber  
	   {
	    
	     get
		{
		   return entitlePassportNumber;
		 }
		 set
		 {
		   if(entitlePassportNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntitlePassportNumber",OldValue=entitlePassportNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entitlePassportNumber=value;
		   }
			
		 }
	   }
	  private string facilityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FacilityTypeName  
	   {
	    
	     get
		{
		   return facilityTypeName;
		 }
		 set
		 {
		   if(facilityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FacilityTypeName",OldValue=facilityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   facilityTypeName=value;
		   }
			
		 }
	   }
	  private string calculatedImporterName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CalculatedImporterName  
	   {
	    
	     get
		{
		   return calculatedImporterName;
		 }
		 set
		 {
		   if(calculatedImporterName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculatedImporterName",OldValue=calculatedImporterName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   calculatedImporterName=value;
		   }
			
		 }
	   }
	  private string calculatedTransferImporterName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CalculatedTransferImporterName  
	   {
	    
	     get
		{
		   return calculatedTransferImporterName;
		 }
		 set
		 {
		   if(calculatedTransferImporterName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculatedTransferImporterName",OldValue=calculatedTransferImporterName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   calculatedTransferImporterName=value;
		   }
			
		 }
	   }
	  private string calculatedEntitleImporterName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CalculatedEntitleImporterName  
	   {
	    
	     get
		{
		   return calculatedEntitleImporterName;
		 }
		 set
		 {
		   if(calculatedEntitleImporterName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculatedEntitleImporterName",OldValue=calculatedEntitleImporterName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   calculatedEntitleImporterName=value;
		   }
			
		 }
	   }
	  private string customerVatNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerVatNo  
	   {
	    
	     get
		{
		   return customerVatNo;
		 }
		 set
		 {
		   if(customerVatNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerVatNo",OldValue=customerVatNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerVatNo=value;
		   }
			
		 }
	   }

	   private List<DeclarationErrorViewPM> declarationErrorViews2;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("DeclarationErrorViewsDeclaration", "Id","DeclarationId")]
	   [DataMember]
	   public virtual List<DeclarationErrorViewPM> DeclarationErrorViews2  
	   {
	        get
             {
                 if (declarationErrorViews2 == null)
                 {
                     declarationErrorViews2 = new List<DeclarationErrorViewPM>();
                 }
                 return declarationErrorViews2;
              }
             set { declarationErrorViews2 = value; }
	    }
		   
	   private List<DeclarationErrorViewPM>  deletedDeclarationErrorViews2;
	   public virtual List<DeclarationErrorViewPM> DeletedDeclarationErrorViews2  
	   {
	        get
             {
                 if ( deletedDeclarationErrorViews2 == null)
                 {
                      deletedDeclarationErrorViews2 = new List<DeclarationErrorViewPM>();
                 }
                 return  deletedDeclarationErrorViews2;
              }
             set {  deletedDeclarationErrorViews2 = value; }
	    }
	  	  private bool declarationPaymentChanged ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool DeclarationPaymentChanged  
	   {
	    
	     get
		{
		   return declarationPaymentChanged;
		 }
		 set
		 {
		   if(declarationPaymentChanged != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationPaymentChanged",OldValue=declarationPaymentChanged,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   declarationPaymentChanged=value;
		   }
			
		 }
	   }
	  private string customsRequestsSheetId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsRequestsSheetId  
	   {
	    
	     get
		{
		   return customsRequestsSheetId;
		 }
		 set
		 {
		   if(customsRequestsSheetId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsRequestsSheetId",OldValue=customsRequestsSheetId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsRequestsSheetId=value;
		   }
			
		 }
	   }
	  private string documentDeclarationId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DocumentDeclarationId  
	   {
	    
	     get
		{
		   return documentDeclarationId;
		 }
		 set
		 {
		   if(documentDeclarationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentDeclarationId",OldValue=documentDeclarationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   documentDeclarationId=value;
		   }
			
		 }
	   }
	  private string storageStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageStatusCode  
	   {
	    
	     get
		{
		   return storageStatusCode;
		 }
		 set
		 {
		   if(storageStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageStatusCode",OldValue=storageStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageStatusCode=value;
		   }
			
		 }
	   }
	  private string casualSupplierName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualSupplierName  
	   {
	    
	     get
		{
		   return casualSupplierName;
		 }
		 set
		 {
		   if(casualSupplierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualSupplierName",OldValue=casualSupplierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualSupplierName=value;
		   }
			
		 }
	   }
	  private string casualSupplierAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualSupplierAddress  
	   {
	    
	     get
		{
		   return casualSupplierAddress;
		 }
		 set
		 {
		   if(casualSupplierAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualSupplierAddress",OldValue=casualSupplierAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualSupplierAddress=value;
		   }
			
		 }
	   }
	  private bool isCourierDeclaration ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCourierDeclaration  
	   {
	    
	     get
		{
		   return isCourierDeclaration;
		 }
		 set
		 {
		   if(isCourierDeclaration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCourierDeclaration",OldValue=isCourierDeclaration,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCourierDeclaration=value;
		   }
			
		 }
	   }
	  private string manifestCargoStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManifestCargoStatusCode  
	   {
	    
	     get
		{
		   return manifestCargoStatusCode;
		 }
		 set
		 {
		   if(manifestCargoStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManifestCargoStatusCode",OldValue=manifestCargoStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manifestCargoStatusCode=value;
		   }
			
		 }
	   }
	  private string manifestErrorXml ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManifestErrorXml  
	   {
	    
	     get
		{
		   return manifestErrorXml;
		 }
		 set
		 {
		   if(manifestErrorXml != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManifestErrorXml",OldValue=manifestErrorXml,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manifestErrorXml=value;
		   }
			
		 }
	   }
	  private string courierHAWB ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierHAWB  
	   {
	    
	     get
		{
		   return courierHAWB;
		 }
		 set
		 {
		   if(courierHAWB != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierHAWB",OldValue=courierHAWB,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierHAWB=value;
		   }
			
		 }
	   }
	  private string manifestNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManifestNumber  
	   {
	    
	     get
		{
		   return manifestNumber;
		 }
		 set
		 {
		   if(manifestNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManifestNumber",OldValue=manifestNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manifestNumber=value;
		   }
			
		 }
	   }
	  private string storageStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageStatusName  
	   {
	    
	     get
		{
		   return storageStatusName;
		 }
		 set
		 {
		   if(storageStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageStatusName",OldValue=storageStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageStatusName=value;
		   }
			
		 }
	   }
	  private bool isAccumulated ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAccumulated  
	   {
	    
	     get
		{
		   return isAccumulated;
		 }
		 set
		 {
		   if(isAccumulated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAccumulated",OldValue=isAccumulated,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAccumulated=value;
		   }
			
		 }
	   }
	  private bool excludeConsignment ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ExcludeConsignment  
	   {
	    
	     get
		{
		   return excludeConsignment;
		 }
		 set
		 {
		   if(excludeConsignment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExcludeConsignment",OldValue=excludeConsignment,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   excludeConsignment=value;
		   }
			
		 }
	   }
	  private string courierCustomStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierCustomStatusCode  
	   {
	    
	     get
		{
		   return courierCustomStatusCode;
		 }
		 set
		 {
		   if(courierCustomStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierCustomStatusCode",OldValue=courierCustomStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierCustomStatusCode=value;
		   }
			
		 }
	   }
	  private string courierSuspentionReasonCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierSuspentionReasonCode  
	   {
	    
	     get
		{
		   return courierSuspentionReasonCode;
		 }
		 set
		 {
		   if(courierSuspentionReasonCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierSuspentionReasonCode",OldValue=courierSuspentionReasonCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierSuspentionReasonCode=value;
		   }
			
		 }
	   }
	  private string courierReleaseStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierReleaseStatusCode  
	   {
	    
	     get
		{
		   return courierReleaseStatusCode;
		 }
		 set
		 {
		   if(courierReleaseStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierReleaseStatusCode",OldValue=courierReleaseStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierReleaseStatusCode=value;
		   }
			
		 }
	   }
	  private string courierHataraStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierHataraStatusCode  
	   {
	    
	     get
		{
		   return courierHataraStatusCode;
		 }
		 set
		 {
		   if(courierHataraStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierHataraStatusCode",OldValue=courierHataraStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierHataraStatusCode=value;
		   }
			
		 }
	   }
	  private string courierData ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierData  
	   {
	    
	     get
		{
		   return courierData;
		 }
		 set
		 {
		   if(courierData != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierData",OldValue=courierData,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierData=value;
		   }
			
		 }
	   }
	  private bool invoiceHasFreight ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool InvoiceHasFreight  
	   {
	    
	     get
		{
		   return invoiceHasFreight;
		 }
		 set
		 {
		   if(invoiceHasFreight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceHasFreight",OldValue=invoiceHasFreight,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   invoiceHasFreight=value;
		   }
			
		 }
	   }
	  private decimal? dealValueWithFactor ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DealValueWithFactor  
	   {
	    
	     get
		{
		   return dealValueWithFactor;
		 }
		 set
		 {
		   if(dealValueWithFactor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DealValueWithFactor",OldValue=dealValueWithFactor,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   dealValueWithFactor=value;
		   }
			
		 }
	   }
	  private bool isValueForCustomsOnly ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsValueForCustomsOnly  
	   {
	    
	     get
		{
		   return isValueForCustomsOnly;
		 }
		 set
		 {
		   if(isValueForCustomsOnly != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsValueForCustomsOnly",OldValue=isValueForCustomsOnly,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isValueForCustomsOnly=value;
		   }
			
		 }
	   }
	  private string weightValue ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string WeightValue  
	   {
	    
	     get
		{
		   return weightValue;
		 }
		 set
		 {
		   if(weightValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WeightValue",OldValue=weightValue,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   weightValue=value;
		   }
			
		 }
	   }
	  private string weightValueName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string WeightValueName  
	   {
	    
	     get
		{
		   return weightValueName;
		 }
		 set
		 {
		   if(weightValueName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WeightValueName",OldValue=weightValueName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   weightValueName=value;
		   }
			
		 }
	   }

	   private List<DeclarationConsAcceptancePM> declarationConsAcceptances;
	 
		     
	   [Include]
	   [Association("DeclarationDeclarationConsAcceptances", "Id","DeclarationId")]
	   [DataMember]
	   public virtual List<DeclarationConsAcceptancePM> DeclarationConsAcceptances  
	   {
	        get
             {
                 if (declarationConsAcceptances == null)
                 {
                     declarationConsAcceptances = new List<DeclarationConsAcceptancePM>();
                 }
                 return declarationConsAcceptances;
              }
             set { declarationConsAcceptances = value; }
	    }
		   
	   private List<DeclarationConsAcceptancePM>  deletedDeclarationConsAcceptances;
	   public virtual List<DeclarationConsAcceptancePM> DeletedDeclarationConsAcceptances  
	   {
	        get
             {
                 if ( deletedDeclarationConsAcceptances == null)
                 {
                      deletedDeclarationConsAcceptances = new List<DeclarationConsAcceptancePM>();
                 }
                 return  deletedDeclarationConsAcceptances;
              }
             set {  deletedDeclarationConsAcceptances = value; }
	    }
	  	  private string courierSearchFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierSearchFields  
	   {
	    
	     get
		{
		   return courierSearchFields;
		 }
		 set
		 {
		   if(courierSearchFields != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierSearchFields",OldValue=courierSearchFields,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierSearchFields=value;
		   }
			
		 }
	   }
	  private string courierCustomStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierCustomStatusName  
	   {
	    
	     get
		{
		   return courierCustomStatusName;
		 }
		 set
		 {
		   if(courierCustomStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierCustomStatusName",OldValue=courierCustomStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierCustomStatusName=value;
		   }
			
		 }
	   }
	  private string manifestCargoStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManifestCargoStatusName  
	   {
	    
	     get
		{
		   return manifestCargoStatusName;
		 }
		 set
		 {
		   if(manifestCargoStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManifestCargoStatusName",OldValue=manifestCargoStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manifestCargoStatusName=value;
		   }
			
		 }
	   }
	  private string mAWBCourierMaster ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWBCourierMaster  
	   {
	    
	     get
		{
		   return mAWBCourierMaster;
		 }
		 set
		 {
		   if(mAWBCourierMaster != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBCourierMaster",OldValue=mAWBCourierMaster,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWBCourierMaster=value;
		   }
			
		 }
	   }
	  private string courierSuspentionReasonName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierSuspentionReasonName  
	   {
	    
	     get
		{
		   return courierSuspentionReasonName;
		 }
		 set
		 {
		   if(courierSuspentionReasonName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierSuspentionReasonName",OldValue=courierSuspentionReasonName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierSuspentionReasonName=value;
		   }
			
		 }
	   }
	  private string acceptanceStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AcceptanceStatusCode  
	   {
	    
	     get
		{
		   return acceptanceStatusCode;
		 }
		 set
		 {
		   if(acceptanceStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AcceptanceStatusCode",OldValue=acceptanceStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   acceptanceStatusCode=value;
		   }
			
		 }
	   }
	  private string casualImporterAddress1 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterAddress1  
	   {
	    
	     get
		{
		   return casualImporterAddress1;
		 }
		 set
		 {
		   if(casualImporterAddress1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterAddress1",OldValue=casualImporterAddress1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterAddress1=value;
		   }
			
		 }
	   }
	  private string casualImporterAddress2 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterAddress2  
	   {
	    
	     get
		{
		   return casualImporterAddress2;
		 }
		 set
		 {
		   if(casualImporterAddress2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterAddress2",OldValue=casualImporterAddress2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterAddress2=value;
		   }
			
		 }
	   }
	  private string casualImporterCity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterCity  
	   {
	    
	     get
		{
		   return casualImporterCity;
		 }
		 set
		 {
		   if(casualImporterCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterCity",OldValue=casualImporterCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterCity=value;
		   }
			
		 }
	   }
	  private string casualImporterZipCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterZipCode  
	   {
	    
	     get
		{
		   return casualImporterZipCode;
		 }
		 set
		 {
		   if(casualImporterZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterZipCode",OldValue=casualImporterZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterZipCode=value;
		   }
			
		 }
	   }
	  private string casualImporterFax ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterFax  
	   {
	    
	     get
		{
		   return casualImporterFax;
		 }
		 set
		 {
		   if(casualImporterFax != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterFax",OldValue=casualImporterFax,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterFax=value;
		   }
			
		 }
	   }
	  private string casualImporterEmail ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterEmail  
	   {
	    
	     get
		{
		   return casualImporterEmail;
		 }
		 set
		 {
		   if(casualImporterEmail != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterEmail",OldValue=casualImporterEmail,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterEmail=value;
		   }
			
		 }
	   }
	  private string casualImporterTel ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterTel  
	   {
	    
	     get
		{
		   return casualImporterTel;
		 }
		 set
		 {
		   if(casualImporterTel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterTel",OldValue=casualImporterTel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterTel=value;
		   }
			
		 }
	   }
	  private string casualImporterContact ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterContact  
	   {
	    
	     get
		{
		   return casualImporterContact;
		 }
		 set
		 {
		   if(casualImporterContact != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterContact",OldValue=casualImporterContact,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterContact=value;
		   }
			
		 }
	   }
	  private string itemsProcessTypesList ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ItemsProcessTypesList  
	   {
	    
	     get
		{
		   return itemsProcessTypesList;
		 }
		 set
		 {
		   if(itemsProcessTypesList != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemsProcessTypesList",OldValue=itemsProcessTypesList,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   itemsProcessTypesList=value;
		   }
			
		 }
	   }
	  private bool isClose ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClose  
	   {
	    
	     get
		{
		   return isClose;
		 }
		 set
		 {
		   if(isClose != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClose",OldValue=isClose,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClose=value;
		   }
			
		 }
	   }
	  private string acceptanceStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AcceptanceStatusName  
	   {
	    
	     get
		{
		   return acceptanceStatusName;
		 }
		 set
		 {
		   if(acceptanceStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AcceptanceStatusName",OldValue=acceptanceStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   acceptanceStatusName=value;
		   }
			
		 }
	   }
	  private string courierSuspentionCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierSuspentionCode  
	   {
	    
	     get
		{
		   return courierSuspentionCode;
		 }
		 set
		 {
		   if(courierSuspentionCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierSuspentionCode",OldValue=courierSuspentionCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierSuspentionCode=value;
		   }
			
		 }
	   }
	  private string courierSuspentionName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierSuspentionName  
	   {
	    
	     get
		{
		   return courierSuspentionName;
		 }
		 set
		 {
		   if(courierSuspentionName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierSuspentionName",OldValue=courierSuspentionName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierSuspentionName=value;
		   }
			
		 }
	   }
	  private string depositionStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepositionStatusCode  
	   {
	    
	     get
		{
		   return depositionStatusCode;
		 }
		 set
		 {
		   if(depositionStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepositionStatusCode",OldValue=depositionStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   depositionStatusCode=value;
		   }
			
		 }
	   }
	  private string courierMasterId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierMasterId  
	   {
	    
	     get
		{
		   return courierMasterId;
		 }
		 set
		 {
		   if(courierMasterId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierMasterId",OldValue=courierMasterId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierMasterId=value;
		   }
			
		 }
	   }
	  private bool isClosedForFollowUp ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClosedForFollowUp  
	   {
	    
	     get
		{
		   return isClosedForFollowUp;
		 }
		 set
		 {
		   if(isClosedForFollowUp != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosedForFollowUp",OldValue=isClosedForFollowUp,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClosedForFollowUp=value;
		   }
			
		 }
	   }
	  private string fastIndividualProcessCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FastIndividualProcessCode  
	   {
	    
	     get
		{
		   return fastIndividualProcessCode;
		 }
		 set
		 {
		   if(fastIndividualProcessCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FastIndividualProcessCode",OldValue=fastIndividualProcessCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fastIndividualProcessCode=value;
		   }
			
		 }
	   }
	  private decimal? totalInvoiceAmountInUSD ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotalInvoiceAmountInUSD  
	   {
	    
	     get
		{
		   return totalInvoiceAmountInUSD;
		 }
		 set
		 {
		   if(totalInvoiceAmountInUSD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalInvoiceAmountInUSD",OldValue=totalInvoiceAmountInUSD,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totalInvoiceAmountInUSD=value;
		   }
			
		 }
	   }
	  private bool isPending902 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPending902  
	   {
	    
	     get
		{
		   return isPending902;
		 }
		 set
		 {
		   if(isPending902 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPending902",OldValue=isPending902,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPending902=value;
		   }
			
		 }
	   }
	  private bool isCourierMissingClassification ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCourierMissingClassification  
	   {
	    
	     get
		{
		   return isCourierMissingClassification;
		 }
		 set
		 {
		   if(isCourierMissingClassification != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCourierMissingClassification",OldValue=isCourierMissingClassification,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCourierMissingClassification=value;
		   }
			
		 }
	   }
	  private string mAWB ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWB  
	   {
	    
	     get
		{
		   return mAWB;
		 }
		 set
		 {
		   if(mAWB != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWB",OldValue=mAWB,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWB=value;
		   }
			
		 }
	   }
	  private bool isPending900 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPending900  
	   {
	    
	     get
		{
		   return isPending900;
		 }
		 set
		 {
		   if(isPending900 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPending900",OldValue=isPending900,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPending900=value;
		   }
			
		 }
	   }
	  private string courierPendingReasonList ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierPendingReasonList  
	   {
	    
	     get
		{
		   return courierPendingReasonList;
		 }
		 set
		 {
		   if(courierPendingReasonList != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierPendingReasonList",OldValue=courierPendingReasonList,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierPendingReasonList=value;
		   }
			
		 }
	   }
	  private string cargoDescription ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoDescription  
	   {
	    
	     get
		{
		   return cargoDescription;
		 }
		 set
		 {
		   if(cargoDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoDescription",OldValue=cargoDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoDescription=value;
		   }
			
		 }
	   }
	  private bool isPaymentProtested ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPaymentProtested  
	   {
	    
	     get
		{
		   return isPaymentProtested;
		 }
		 set
		 {
		   if(isPaymentProtested != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPaymentProtested",OldValue=isPaymentProtested,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPaymentProtested=value;
		   }
			
		 }
	   }
	  private string fastIndividualProcessName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FastIndividualProcessName  
	   {
	    
	     get
		{
		   return fastIndividualProcessName;
		 }
		 set
		 {
		   if(fastIndividualProcessName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FastIndividualProcessName",OldValue=fastIndividualProcessName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fastIndividualProcessName=value;
		   }
			
		 }
	   }

	   private List<DecDangersContactPM> decDangersContacts;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("DeclarationDecDangersContacts", "DeclarationId","DeclarationId")]
	   [DataMember]
	   public virtual List<DecDangersContactPM> DecDangersContacts  
	   {
	        get
             {
                 if (decDangersContacts == null)
                 {
                     decDangersContacts = new List<DecDangersContactPM>();
                 }
                 return decDangersContacts;
              }
             set { decDangersContacts = value; }
	    }
		   
	   private List<DecDangersContactPM>  deletedDecDangersContacts;
	   public virtual List<DecDangersContactPM> DeletedDecDangersContacts  
	   {
	        get
             {
                 if ( deletedDecDangersContacts == null)
                 {
                      deletedDecDangersContacts = new List<DecDangersContactPM>();
                 }
                 return  deletedDecDangersContacts;
              }
             set {  deletedDecDangersContacts = value; }
	    }
	  	  private string amendmentRequestNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AmendmentRequestNumber  
	   {
	    
	     get
		{
		   return amendmentRequestNumber;
		 }
		 set
		 {
		   if(amendmentRequestNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentRequestNumber",OldValue=amendmentRequestNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   amendmentRequestNumber=value;
		   }
			
		 }
	   }
	  private string amendmentStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AmendmentStatus  
	   {
	    
	     get
		{
		   return amendmentStatus;
		 }
		 set
		 {
		   if(amendmentStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentStatus",OldValue=amendmentStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   amendmentStatus=value;
		   }
			
		 }
	   }
	  private DateTime? amendmentissueDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? AmendmentissueDate  
	   {
	    
	     get
		{
		   return amendmentissueDate;
		 }
		 set
		 {
		   if(amendmentissueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentissueDate",OldValue=amendmentissueDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   amendmentissueDate=value;
		   }
			
		 }
	   }
	  private string amendmentRemarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AmendmentRemarks  
	   {
	    
	     get
		{
		   return amendmentRemarks;
		 }
		 set
		 {
		   if(amendmentRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentRemarks",OldValue=amendmentRemarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   amendmentRemarks=value;
		   }
			
		 }
	   }
	  private bool? amendmentDeficitInitiated ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? AmendmentDeficitInitiated  
	   {
	    
	     get
		{
		   return amendmentDeficitInitiated;
		 }
		 set
		 {
		   if(amendmentDeficitInitiated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentDeficitInitiated",OldValue=amendmentDeficitInitiated,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   amendmentDeficitInitiated=value;
		   }
			
		 }
	   }
	  private string amendDeficitInitiatedReasTo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AmendDeficitInitiatedReasTo  
	   {
	    
	     get
		{
		   return amendDeficitInitiatedReasTo;
		 }
		 set
		 {
		   if(amendDeficitInitiatedReasTo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendDeficitInitiatedReasTo",OldValue=amendDeficitInitiatedReasTo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   amendDeficitInitiatedReasTo=value;
		   }
			
		 }
	   }
	  private string amendmentCorrectedByUserId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AmendmentCorrectedByUserId  
	   {
	    
	     get
		{
		   return amendmentCorrectedByUserId;
		 }
		 set
		 {
		   if(amendmentCorrectedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentCorrectedByUserId",OldValue=amendmentCorrectedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   amendmentCorrectedByUserId=value;
		   }
			
		 }
	   }
	  private string amendmentRejectionReason ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AmendmentRejectionReason  
	   {
	    
	     get
		{
		   return amendmentRejectionReason;
		 }
		 set
		 {
		   if(amendmentRejectionReason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentRejectionReason",OldValue=amendmentRejectionReason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   amendmentRejectionReason=value;
		   }
			
		 }
	   }
	  private bool? isAmendment ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsAmendment  
	   {
	    
	     get
		{
		   return isAmendment;
		 }
		 set
		 {
		   if(isAmendment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAmendment",OldValue=isAmendment,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isAmendment=value;
		   }
			
		 }
	   }
	  private string amendmentOriginalDeclartation ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AmendmentOriginalDeclartation  
	   {
	    
	     get
		{
		   return amendmentOriginalDeclartation;
		 }
		 set
		 {
		   if(amendmentOriginalDeclartation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentOriginalDeclartation",OldValue=amendmentOriginalDeclartation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   amendmentOriginalDeclartation=value;
		   }
			
		 }
	   }
	  private string amendmentCorrectedByUserName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AmendmentCorrectedByUserName  
	   {
	    
	     get
		{
		   return amendmentCorrectedByUserName;
		 }
		 set
		 {
		   if(amendmentCorrectedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentCorrectedByUserName",OldValue=amendmentCorrectedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   amendmentCorrectedByUserName=value;
		   }
			
		 }
	   }
	  private string amendmentStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AmendmentStatusName  
	   {
	    
	     get
		{
		   return amendmentStatusName;
		 }
		 set
		 {
		   if(amendmentStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentStatusName",OldValue=amendmentStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   amendmentStatusName=value;
		   }
			
		 }
	   }
	  private string courierManifestStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierManifestStatusCode  
	   {
	    
	     get
		{
		   return courierManifestStatusCode;
		 }
		 set
		 {
		   if(courierManifestStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierManifestStatusCode",OldValue=courierManifestStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierManifestStatusCode=value;
		   }
			
		 }
	   }
	  private string courierPaymentStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierPaymentStatusCode  
	   {
	    
	     get
		{
		   return courierPaymentStatusCode;
		 }
		 set
		 {
		   if(courierPaymentStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierPaymentStatusCode",OldValue=courierPaymentStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierPaymentStatusCode=value;
		   }
			
		 }
	   }
	  private bool isPendingNotNull ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPendingNotNull  
	   {
	    
	     get
		{
		   return isPendingNotNull;
		 }
		 set
		 {
		   if(isPendingNotNull != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPendingNotNull",OldValue=isPendingNotNull,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPendingNotNull=value;
		   }
			
		 }
	   }
	  private bool amendmentDontDisplayInList ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AmendmentDontDisplayInList  
	   {
	    
	     get
		{
		   return amendmentDontDisplayInList;
		 }
		 set
		 {
		   if(amendmentDontDisplayInList != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentDontDisplayInList",OldValue=amendmentDontDisplayInList,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   amendmentDontDisplayInList=value;
		   }
			
		 }
	   }
	  private string declarationType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationType  
	   {
	    
	     get
		{
		   return declarationType;
		 }
		 set
		 {
		   if(declarationType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationType",OldValue=declarationType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationType=value;
		   }
			
		 }
	   }

	  private string amendmentMessage ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AmendmentMessage  
	   {
	    
	     get
		{
		   return amendmentMessage;
		 }
		 set
		 {
		   if(amendmentMessage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmendmentMessage",OldValue=amendmentMessage,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   amendmentMessage=value;
		   }
			
		 }
	   }
	  private bool isAmendmentDisplayOnly ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAmendmentDisplayOnly  
	   {
	    
	     get
		{
		   return isAmendmentDisplayOnly;
		 }
		 set
		 {
		   if(isAmendmentDisplayOnly != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAmendmentDisplayOnly",OldValue=isAmendmentDisplayOnly,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAmendmentDisplayOnly=value;
		   }
			
		 }
	   }

   }
   
}
	 