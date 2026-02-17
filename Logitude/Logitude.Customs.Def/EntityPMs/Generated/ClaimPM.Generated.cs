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
   public partial class ClaimPM : EntityPM
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
	  private string importerClaimTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterClaimTypeCode  
	   {
	    
	     get
		{
		   return importerClaimTypeCode;
		 }
		 set
		 {
		   if(importerClaimTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterClaimTypeCode",OldValue=importerClaimTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerClaimTypeCode=value;
		   }
			
		 }
	   }
	  private string importerClaimTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterClaimTypeName  
	   {
	    
	     get
		{
		   return importerClaimTypeName;
		 }
		 set
		 {
		   if(importerClaimTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterClaimTypeName",OldValue=importerClaimTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerClaimTypeName=value;
		   }
			
		 }
	   }
	  private string soldierPersonalNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SoldierPersonalNumber  
	   {
	    
	     get
		{
		   return soldierPersonalNumber;
		 }
		 set
		 {
		   if(soldierPersonalNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SoldierPersonalNumber",OldValue=soldierPersonalNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   soldierPersonalNumber=value;
		   }
			
		 }
	   }
	  private DateTime? submitDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? SubmitDate  
	   {
	    
	     get
		{
		   return submitDate;
		 }
		 set
		 {
		   if(submitDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SubmitDate",OldValue=submitDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   submitDate=value;
		   }
			
		 }
	   }
	  private string clientId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClientId  
	   {
	    
	     get
		{
		   return clientId;
		 }
		 set
		 {
		   if(clientId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClientId",OldValue=clientId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   clientId=value;
		   }
			
		 }
	   }
	  private string passportCountryTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PassportCountryTypeCode  
	   {
	    
	     get
		{
		   return passportCountryTypeCode;
		 }
		 set
		 {
		   if(passportCountryTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PassportCountryTypeCode",OldValue=passportCountryTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   passportCountryTypeCode=value;
		   }
			
		 }
	   }
	  private string passportCountryTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PassportCountryTypeName  
	   {
	    
	     get
		{
		   return passportCountryTypeName;
		 }
		 set
		 {
		   if(passportCountryTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PassportCountryTypeName",OldValue=passportCountryTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   passportCountryTypeName=value;
		   }
			
		 }
	   }
	  private string passportNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PassportNumber  
	   {
	    
	     get
		{
		   return passportNumber;
		 }
		 set
		 {
		   if(passportNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PassportNumber",OldValue=passportNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   passportNumber=value;
		   }
			
		 }
	   }
	  private string passportTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PassportTypeCode  
	   {
	    
	     get
		{
		   return passportTypeCode;
		 }
		 set
		 {
		   if(passportTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PassportTypeCode",OldValue=passportTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   passportTypeCode=value;
		   }
			
		 }
	   }
	  private string passportTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PassportTypeName  
	   {
	    
	     get
		{
		   return passportTypeName;
		 }
		 set
		 {
		   if(passportTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PassportTypeName",OldValue=passportTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   passportTypeName=value;
		   }
			
		 }
	   }
	  private string customsAddressCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsAddressCode  
	   {
	    
	     get
		{
		   return customsAddressCode;
		 }
		 set
		 {
		   if(customsAddressCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsAddressCode",OldValue=customsAddressCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsAddressCode=value;
		   }
			
		 }
	   }
	  private string contactPhoneAddressCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactPhoneAddressCode  
	   {
	    
	     get
		{
		   return contactPhoneAddressCode;
		 }
		 set
		 {
		   if(contactPhoneAddressCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactPhoneAddressCode",OldValue=contactPhoneAddressCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactPhoneAddressCode=value;
		   }
			
		 }
	   }
	  private string claimSubmiterNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClaimSubmiterNumber  
	   {
	    
	     get
		{
		   return claimSubmiterNumber;
		 }
		 set
		 {
		   if(claimSubmiterNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClaimSubmiterNumber",OldValue=claimSubmiterNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   claimSubmiterNumber=value;
		   }
			
		 }
	   }
	  private string claimSubmiterTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClaimSubmiterTypeCode  
	   {
	    
	     get
		{
		   return claimSubmiterTypeCode;
		 }
		 set
		 {
		   if(claimSubmiterTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClaimSubmiterTypeCode",OldValue=claimSubmiterTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   claimSubmiterTypeCode=value;
		   }
			
		 }
	   }
	  private string claimSubmiterTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClaimSubmiterTypeName  
	   {
	    
	     get
		{
		   return claimSubmiterTypeName;
		 }
		 set
		 {
		   if(claimSubmiterTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClaimSubmiterTypeName",OldValue=claimSubmiterTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   claimSubmiterTypeName=value;
		   }
			
		 }
	   }
	  private string hebrewCorporationName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string HebrewCorporationName  
	   {
	    
	     get
		{
		   return hebrewCorporationName;
		 }
		 set
		 {
		   if(hebrewCorporationName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HebrewCorporationName",OldValue=hebrewCorporationName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   hebrewCorporationName=value;
		   }
			
		 }
	   }
	  private string addressCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AddressCode  
	   {
	    
	     get
		{
		   return addressCode;
		 }
		 set
		 {
		   if(addressCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AddressCode",OldValue=addressCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   addressCode=value;
		   }
			
		 }
	   }
	  private string beneficiaryExternalID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BeneficiaryExternalID  
	   {
	    
	     get
		{
		   return beneficiaryExternalID;
		 }
		 set
		 {
		   if(beneficiaryExternalID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BeneficiaryExternalID",OldValue=beneficiaryExternalID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   beneficiaryExternalID=value;
		   }
			
		 }
	   }
	  private string beneficiaryActivityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BeneficiaryActivityTypeCode  
	   {
	    
	     get
		{
		   return beneficiaryActivityTypeCode;
		 }
		 set
		 {
		   if(beneficiaryActivityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BeneficiaryActivityTypeCode",OldValue=beneficiaryActivityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   beneficiaryActivityTypeCode=value;
		   }
			
		 }
	   }
	  private string beneficiaryActivityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BeneficiaryActivityTypeName  
	   {
	    
	     get
		{
		   return beneficiaryActivityTypeName;
		 }
		 set
		 {
		   if(beneficiaryActivityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BeneficiaryActivityTypeName",OldValue=beneficiaryActivityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   beneficiaryActivityTypeName=value;
		   }
			
		 }
	   }
	  private string accountCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountCountryCode  
	   {
	    
	     get
		{
		   return accountCountryCode;
		 }
		 set
		 {
		   if(accountCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountCountryCode",OldValue=accountCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountCountryCode=value;
		   }
			
		 }
	   }
	  private string accountCountryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountCountryName  
	   {
	    
	     get
		{
		   return accountCountryName;
		 }
		 set
		 {
		   if(accountCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountCountryName",OldValue=accountCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountCountryName=value;
		   }
			
		 }
	   }
	  private string bankTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankTypeCode  
	   {
	    
	     get
		{
		   return bankTypeCode;
		 }
		 set
		 {
		   if(bankTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankTypeCode",OldValue=bankTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankTypeCode=value;
		   }
			
		 }
	   }
	  private string accountBranchCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountBranchCode  
	   {
	    
	     get
		{
		   return accountBranchCode;
		 }
		 set
		 {
		   if(accountBranchCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountBranchCode",OldValue=accountBranchCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountBranchCode=value;
		   }
			
		 }
	   }
	  private string accountBranchName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountBranchName  
	   {
	    
	     get
		{
		   return accountBranchName;
		 }
		 set
		 {
		   if(accountBranchName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountBranchName",OldValue=accountBranchName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountBranchName=value;
		   }
			
		 }
	   }
	  private string accountNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountNumber  
	   {
	    
	     get
		{
		   return accountNumber;
		 }
		 set
		 {
		   if(accountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountNumber",OldValue=accountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountNumber=value;
		   }
			
		 }
	   }
	  private string accountCurrencyTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountCurrencyTypeCode  
	   {
	    
	     get
		{
		   return accountCurrencyTypeCode;
		 }
		 set
		 {
		   if(accountCurrencyTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountCurrencyTypeCode",OldValue=accountCurrencyTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountCurrencyTypeCode=value;
		   }
			
		 }
	   }
	  private string accountCurrencyTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountCurrencyTypeName  
	   {
	    
	     get
		{
		   return accountCurrencyTypeName;
		 }
		 set
		 {
		   if(accountCurrencyTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountCurrencyTypeName",OldValue=accountCurrencyTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountCurrencyTypeName=value;
		   }
			
		 }
	   }
	  private string foreignBank ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForeignBank  
	   {
	    
	     get
		{
		   return foreignBank;
		 }
		 set
		 {
		   if(foreignBank != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignBank",OldValue=foreignBank,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   foreignBank=value;
		   }
			
		 }
	   }
	  private string foreignBranch ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForeignBranch  
	   {
	    
	     get
		{
		   return foreignBranch;
		 }
		 set
		 {
		   if(foreignBranch != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignBranch",OldValue=foreignBranch,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   foreignBranch=value;
		   }
			
		 }
	   }
	  private string foreignAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForeignAccountNumber  
	   {
	    
	     get
		{
		   return foreignAccountNumber;
		 }
		 set
		 {
		   if(foreignAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignAccountNumber",OldValue=foreignAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   foreignAccountNumber=value;
		   }
			
		 }
	   }
	  private string importerAffidavit ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterAffidavit  
	   {
	    
	     get
		{
		   return importerAffidavit;
		 }
		 set
		 {
		   if(importerAffidavit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterAffidavit",OldValue=importerAffidavit,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerAffidavit=value;
		   }
			
		 }
	   }
	  private string rawMaterialsDescription ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RawMaterialsDescription  
	   {
	    
	     get
		{
		   return rawMaterialsDescription;
		 }
		 set
		 {
		   if(rawMaterialsDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RawMaterialsDescription",OldValue=rawMaterialsDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   rawMaterialsDescription=value;
		   }
			
		 }
	   }
	  private string customsFiles ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsFiles  
	   {
	    
	     get
		{
		   return customsFiles;
		 }
		 set
		 {
		   if(customsFiles != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsFiles",OldValue=customsFiles,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsFiles=value;
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

	   private List<ClaimImporterDeclarsPage3PM> claimImporterDeclarsPage3;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ClaimImporterDeclarsPage3s", "Id","ClaimId")]
	   [DataMember]
	   public virtual List<ClaimImporterDeclarsPage3PM> ClaimImporterDeclarsPage3  
	   {
	        get
             {
                 if (claimImporterDeclarsPage3 == null)
                 {
                     claimImporterDeclarsPage3 = new List<ClaimImporterDeclarsPage3PM>();
                 }
                 return claimImporterDeclarsPage3;
              }
             set { claimImporterDeclarsPage3 = value; }
	    }
		   
	   private List<ClaimImporterDeclarsPage3PM>  deletedClaimImporterDeclarsPage3;
	   public virtual List<ClaimImporterDeclarsPage3PM> DeletedClaimImporterDeclarsPage3  
	   {
	        get
             {
                 if ( deletedClaimImporterDeclarsPage3 == null)
                 {
                      deletedClaimImporterDeclarsPage3 = new List<ClaimImporterDeclarsPage3PM>();
                 }
                 return  deletedClaimImporterDeclarsPage3;
              }
             set {  deletedClaimImporterDeclarsPage3 = value; }
	    }
	  
	   private List<ClaimsRelatedEntityPM> claimsRelatedEntities;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ClaimEntities", "Id","ClaimId")]
	   [DataMember]
	   public virtual List<ClaimsRelatedEntityPM> ClaimsRelatedEntities  
	   {
	        get
             {
                 if (claimsRelatedEntities == null)
                 {
                     claimsRelatedEntities = new List<ClaimsRelatedEntityPM>();
                 }
                 return claimsRelatedEntities;
              }
             set { claimsRelatedEntities = value; }
	    }
		   
	   private List<ClaimsRelatedEntityPM>  deletedClaimsRelatedEntities;
	   public virtual List<ClaimsRelatedEntityPM> DeletedClaimsRelatedEntities  
	   {
	        get
             {
                 if ( deletedClaimsRelatedEntities == null)
                 {
                      deletedClaimsRelatedEntities = new List<ClaimsRelatedEntityPM>();
                 }
                 return  deletedClaimsRelatedEntities;
              }
             set {  deletedClaimsRelatedEntities = value; }
	    }
	  
	   private List<ClaimImporterDeclarsPage3APM> claimImporterDeclarsPage3A;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ClaimImporterDeclarsPage3A", "Id","ClaimId")]
	   [DataMember]
	   public virtual List<ClaimImporterDeclarsPage3APM> ClaimImporterDeclarsPage3A  
	   {
	        get
             {
                 if (claimImporterDeclarsPage3A == null)
                 {
                     claimImporterDeclarsPage3A = new List<ClaimImporterDeclarsPage3APM>();
                 }
                 return claimImporterDeclarsPage3A;
              }
             set { claimImporterDeclarsPage3A = value; }
	    }
		   
	   private List<ClaimImporterDeclarsPage3APM>  deletedClaimImporterDeclarsPage3A;
	   public virtual List<ClaimImporterDeclarsPage3APM> DeletedClaimImporterDeclarsPage3A  
	   {
	        get
             {
                 if ( deletedClaimImporterDeclarsPage3A == null)
                 {
                      deletedClaimImporterDeclarsPage3A = new List<ClaimImporterDeclarsPage3APM>();
                 }
                 return  deletedClaimImporterDeclarsPage3A;
              }
             set {  deletedClaimImporterDeclarsPage3A = value; }
	    }
	  
	   private List<ClaimImporterDeclarsPage3BPM> claimImporterDeclarsPage3B;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ClaimImporterDeclarsPage3B", "Id","ClaimId")]
	   [DataMember]
	   public virtual List<ClaimImporterDeclarsPage3BPM> ClaimImporterDeclarsPage3B  
	   {
	        get
             {
                 if (claimImporterDeclarsPage3B == null)
                 {
                     claimImporterDeclarsPage3B = new List<ClaimImporterDeclarsPage3BPM>();
                 }
                 return claimImporterDeclarsPage3B;
              }
             set { claimImporterDeclarsPage3B = value; }
	    }
		   
	   private List<ClaimImporterDeclarsPage3BPM>  deletedClaimImporterDeclarsPage3B;
	   public virtual List<ClaimImporterDeclarsPage3BPM> DeletedClaimImporterDeclarsPage3B  
	   {
	        get
             {
                 if ( deletedClaimImporterDeclarsPage3B == null)
                 {
                      deletedClaimImporterDeclarsPage3B = new List<ClaimImporterDeclarsPage3BPM>();
                 }
                 return  deletedClaimImporterDeclarsPage3B;
              }
             set {  deletedClaimImporterDeclarsPage3B = value; }
	    }
	  	  private string tapagNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TapagNumber  
	   {
	    
	     get
		{
		   return tapagNumber;
		 }
		 set
		 {
		   if(tapagNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TapagNumber",OldValue=tapagNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tapagNumber=value;
		   }
			
		 }
	   }
	  private string leadingFileNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LeadingFileNumber  
	   {
	    
	     get
		{
		   return leadingFileNumber;
		 }
		 set
		 {
		   if(leadingFileNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeadingFileNumber",OldValue=leadingFileNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   leadingFileNumber=value;
		   }
			
		 }
	   }
	  private string tapagTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TapagTypeCode  
	   {
	    
	     get
		{
		   return tapagTypeCode;
		 }
		 set
		 {
		   if(tapagTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TapagTypeCode",OldValue=tapagTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tapagTypeCode=value;
		   }
			
		 }
	   }
	  private string tapagTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TapagTypeName  
	   {
	    
	     get
		{
		   return tapagTypeName;
		 }
		 set
		 {
		   if(tapagTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TapagTypeName",OldValue=tapagTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tapagTypeName=value;
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
	  private DateTime? createDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private DateTime? followDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FollowDate  
	   {
	    
	     get
		{
		   return followDate;
		 }
		 set
		 {
		   if(followDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowDate",OldValue=followDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   followDate=value;
		   }
			
		 }
	   }
	  private DateTime? validityDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ValidityDate  
	   {
	    
	     get
		{
		   return validityDate;
		 }
		 set
		 {
		   if(validityDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ValidityDate",OldValue=validityDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   validityDate=value;
		   }
			
		 }
	   }
	  private bool isClosed ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string tapagId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TapagId  
	   {
	    
	     get
		{
		   return tapagId;
		 }
		 set
		 {
		   if(tapagId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TapagId",OldValue=tapagId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tapagId=value;
		   }
			
		 }
	   }
	  private string customsBranchCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBranchCode  
	   {
	    
	     get
		{
		   return customsBranchCode;
		 }
		 set
		 {
		   if(customsBranchCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBranchCode",OldValue=customsBranchCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBranchCode=value;
		   }
			
		 }
	   }
	  private string referantId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReferantId  
	   {
	    
	     get
		{
		   return referantId;
		 }
		 set
		 {
		   if(referantId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReferantId",OldValue=referantId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   referantId=value;
		   }
			
		 }
	   }
	  private string referantName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReferantName  
	   {
	    
	     get
		{
		   return referantName;
		 }
		 set
		 {
		   if(referantName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReferantName",OldValue=referantName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   referantName=value;
		   }
			
		 }
	   }
	  private bool isSendClaimsRelatedEntity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSendClaimsRelatedEntity  
	   {
	    
	     get
		{
		   return isSendClaimsRelatedEntity;
		 }
		 set
		 {
		   if(isSendClaimsRelatedEntity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSendClaimsRelatedEntity",OldValue=isSendClaimsRelatedEntity,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSendClaimsRelatedEntity=value;
		   }
			
		 }
	   }
	  private string customsBranchName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBranchName  
	   {
	    
	     get
		{
		   return customsBranchName;
		 }
		 set
		 {
		   if(customsBranchName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBranchName",OldValue=customsBranchName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBranchName=value;
		   }
			
		 }
	   }
   }
   
}
	 