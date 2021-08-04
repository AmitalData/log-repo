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
   public partial class DeclarationCourierStatusPM : EntityPM
   {
   	  private string declarationId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationId  
	   {
	    
	     get
		{
		   return declarationId;
		 }
		 set
		 {
		   if(declarationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationId",OldValue=declarationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationId=value;
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
	  private string courierManifestStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierManifestStatusName  
	   {
	    
	     get
		{
		   return courierManifestStatusName;
		 }
		 set
		 {
		   if(courierManifestStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierManifestStatusName",OldValue=courierManifestStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierManifestStatusName=value;
		   }
			
		 }
	   }
	  private string courierDeclarationStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierDeclarationStatusCode  
	   {
	    
	     get
		{
		   return courierDeclarationStatusCode;
		 }
		 set
		 {
		   if(courierDeclarationStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierDeclarationStatusCode",OldValue=courierDeclarationStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierDeclarationStatusCode=value;
		   }
			
		 }
	   }
	  private string courierDeclarationStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierDeclarationStatusName  
	   {
	    
	     get
		{
		   return courierDeclarationStatusName;
		 }
		 set
		 {
		   if(courierDeclarationStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierDeclarationStatusName",OldValue=courierDeclarationStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierDeclarationStatusName=value;
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
	  private string courierPaymentStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierPaymentStatusName  
	   {
	    
	     get
		{
		   return courierPaymentStatusName;
		 }
		 set
		 {
		   if(courierPaymentStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierPaymentStatusName",OldValue=courierPaymentStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierPaymentStatusName=value;
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
	  private string highLowValue ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string HighLowValue  
	   {
	    
	     get
		{
		   return highLowValue;
		 }
		 set
		 {
		   if(highLowValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HighLowValue",OldValue=highLowValue,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   highLowValue=value;
		   }
			
		 }
	   }
	  private string documentStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DocumentStatusCode  
	   {
	    
	     get
		{
		   return documentStatusCode;
		 }
		 set
		 {
		   if(documentStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentStatusCode",OldValue=documentStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   documentStatusCode=value;
		   }
			
		 }
	   }
	  private string courierHawb ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierHawb  
	   {
	    
	     get
		{
		   return courierHawb;
		 }
		 set
		 {
		   if(courierHawb != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierHawb",OldValue=courierHawb,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierHawb=value;
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
	  private bool isMNFTab ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMNFTab  
	   {
	    
	     get
		{
		   return isMNFTab;
		 }
		 set
		 {
		   if(isMNFTab != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMNFTab",OldValue=isMNFTab,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMNFTab=value;
		   }
			
		 }
	   }
	  private bool isPAYTab ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPAYTab  
	   {
	    
	     get
		{
		   return isPAYTab;
		 }
		 set
		 {
		   if(isPAYTab != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPAYTab",OldValue=isPAYTab,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPAYTab=value;
		   }
			
		 }
	   }
	  private bool isDECTab ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDECTab  
	   {
	    
	     get
		{
		   return isDECTab;
		 }
		 set
		 {
		   if(isDECTab != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDECTab",OldValue=isDECTab,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDECTab=value;
		   }
			
		 }
	   }
	  private bool isDOCTab ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDOCTab  
	   {
	    
	     get
		{
		   return isDOCTab;
		 }
		 set
		 {
		   if(isDOCTab != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDOCTab",OldValue=isDOCTab,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDOCTab=value;
		   }
			
		 }
	   }
	  private bool isSVGTab ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSVGTab  
	   {
	    
	     get
		{
		   return isSVGTab;
		 }
		 set
		 {
		   if(isSVGTab != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSVGTab",OldValue=isSVGTab,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSVGTab=value;
		   }
			
		 }
	   }
	  private bool isMNFRTab ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMNFRTab  
	   {
	    
	     get
		{
		   return isMNFRTab;
		 }
		 set
		 {
		   if(isMNFRTab != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMNFRTab",OldValue=isMNFRTab,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMNFRTab=value;
		   }
			
		 }
	   }
	  private bool isDECRTab ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDECRTab  
	   {
	    
	     get
		{
		   return isDECRTab;
		 }
		 set
		 {
		   if(isDECRTab != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDECRTab",OldValue=isDECRTab,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDECRTab=value;
		   }
			
		 }
	   }
	  private bool isHOLDTab ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsHOLDTab  
	   {
	    
	     get
		{
		   return isHOLDTab;
		 }
		 set
		 {
		   if(isHOLDTab != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsHOLDTab",OldValue=isHOLDTab,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isHOLDTab=value;
		   }
			
		 }
	   }
	  private bool isACCTab ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsACCTab  
	   {
	    
	     get
		{
		   return isACCTab;
		 }
		 set
		 {
		   if(isACCTab != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsACCTab",OldValue=isACCTab,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isACCTab=value;
		   }
			
		 }
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
	  private string courierPendingReasonCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierPendingReasonCode  
	   {
	    
	     get
		{
		   return courierPendingReasonCode;
		 }
		 set
		 {
		   if(courierPendingReasonCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierPendingReasonCode",OldValue=courierPendingReasonCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierPendingReasonCode=value;
		   }
			
		 }
	   }
	  private string courierPendingReasonName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierPendingReasonName  
	   {
	    
	     get
		{
		   return courierPendingReasonName;
		 }
		 set
		 {
		   if(courierPendingReasonName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierPendingReasonName",OldValue=courierPendingReasonName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierPendingReasonName=value;
		   }
			
		 }
	   }
	  private string pendingRemarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PendingRemarks  
	   {
	    
	     get
		{
		   return pendingRemarks;
		 }
		 set
		 {
		   if(pendingRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PendingRemarks",OldValue=pendingRemarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pendingRemarks=value;
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
	  private string specialActionStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecialActionStatus  
	   {
	    
	     get
		{
		   return specialActionStatus;
		 }
		 set
		 {
		   if(specialActionStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecialActionStatus",OldValue=specialActionStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specialActionStatus=value;
		   }
			
		 }
	   }
	  private string specialActionsErrorXml ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecialActionsErrorXml  
	   {
	    
	     get
		{
		   return specialActionsErrorXml;
		 }
		 set
		 {
		   if(specialActionsErrorXml != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecialActionsErrorXml",OldValue=specialActionsErrorXml,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specialActionsErrorXml=value;
		   }
			
		 }
	   }
	  private string courierPendingReasonErrorPlace ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierPendingReasonErrorPlace  
	   {
	    
	     get
		{
		   return courierPendingReasonErrorPlace;
		 }
		 set
		 {
		   if(courierPendingReasonErrorPlace != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierPendingReasonErrorPlace",OldValue=courierPendingReasonErrorPlace,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierPendingReasonErrorPlace=value;
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
	  private string manualProcessCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManualProcessCode  
	   {
	    
	     get
		{
		   return manualProcessCode;
		 }
		 set
		 {
		   if(manualProcessCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManualProcessCode",OldValue=manualProcessCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manualProcessCode=value;
		   }
			
		 }
	   }
	  private string terminalSuspentionNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TerminalSuspentionNumber  
	   {
	    
	     get
		{
		   return terminalSuspentionNumber;
		 }
		 set
		 {
		   if(terminalSuspentionNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TerminalSuspentionNumber",OldValue=terminalSuspentionNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   terminalSuspentionNumber=value;
		   }
			
		 }
	   }
	  private string lastMileStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastMileStatusCode  
	   {
	    
	     get
		{
		   return lastMileStatusCode;
		 }
		 set
		 {
		   if(lastMileStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastMileStatusCode",OldValue=lastMileStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastMileStatusCode=value;
		   }
			
		 }
	   }
	  private DateTime? lastMileStatusDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastMileStatusDate  
	   {
	    
	     get
		{
		   return lastMileStatusDate;
		 }
		 set
		 {
		   if(lastMileStatusDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastMileStatusDate",OldValue=lastMileStatusDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastMileStatusDate=value;
		   }
			
		 }
	   }
	  private string lastMileStatusRemarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastMileStatusRemarks  
	   {
	    
	     get
		{
		   return lastMileStatusRemarks;
		 }
		 set
		 {
		   if(lastMileStatusRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastMileStatusRemarks",OldValue=lastMileStatusRemarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastMileStatusRemarks=value;
		   }
			
		 }
	   }
	  private string storageSiteStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageSiteStatusCode  
	   {
	    
	     get
		{
		   return storageSiteStatusCode;
		 }
		 set
		 {
		   if(storageSiteStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageSiteStatusCode",OldValue=storageSiteStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageSiteStatusCode=value;
		   }
			
		 }
	   }
	  private string storageSiteErrorText ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageSiteErrorText  
	   {
	    
	     get
		{
		   return storageSiteErrorText;
		 }
		 set
		 {
		   if(storageSiteErrorText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageSiteErrorText",OldValue=storageSiteErrorText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageSiteErrorText=value;
		   }
			
		 }
	   }
	  private string storageSiteStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageSiteStatusName  
	   {
	    
	     get
		{
		   return storageSiteStatusName;
		 }
		 set
		 {
		   if(storageSiteStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageSiteStatusName",OldValue=storageSiteStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageSiteStatusName=value;
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

	   private List<DeclarationPendingPM> declarationPendings;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("DeclarationCourierStatusDeclarationPending", "DeclarationId","DeclarationID")]
	   [DataMember]
	   public virtual List<DeclarationPendingPM> DeclarationPendings  
	   {
	        get
             {
                 if (declarationPendings == null)
                 {
                     declarationPendings = new List<DeclarationPendingPM>();
                 }
                 return declarationPendings;
              }
             set { declarationPendings = value; }
	    }
		   
	   private List<DeclarationPendingPM>  deletedDeclarationPendings;
	   public virtual List<DeclarationPendingPM> DeletedDeclarationPendings  
	   {
	        get
             {
                 if ( deletedDeclarationPendings == null)
                 {
                      deletedDeclarationPendings = new List<DeclarationPendingPM>();
                 }
                 return  deletedDeclarationPendings;
              }
             set {  deletedDeclarationPendings = value; }
	    }
	  	  private string lastMileStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastMileStatusName  
	   {
	    
	     get
		{
		   return lastMileStatusName;
		 }
		 set
		 {
		   if(lastMileStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastMileStatusName",OldValue=lastMileStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastMileStatusName=value;
		   }
			
		 }
	   }
	  private string sortedImporterCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SortedImporterCode  
	   {
	    
	     get
		{
		   return sortedImporterCode;
		 }
		 set
		 {
		   if(sortedImporterCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SortedImporterCode",OldValue=sortedImporterCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sortedImporterCode=value;
		   }
			
		 }
	   }
	  private string sortedDocumentStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SortedDocumentStatusCode  
	   {
	    
	     get
		{
		   return sortedDocumentStatusCode;
		 }
		 set
		 {
		   if(sortedDocumentStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SortedDocumentStatusCode",OldValue=sortedDocumentStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sortedDocumentStatusCode=value;
		   }
			
		 }
	   }
	  private string sortedCourierManifestStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SortedCourierManifestStatus  
	   {
	    
	     get
		{
		   return sortedCourierManifestStatus;
		 }
		 set
		 {
		   if(sortedCourierManifestStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SortedCourierManifestStatus",OldValue=sortedCourierManifestStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sortedCourierManifestStatus=value;
		   }
			
		 }
	   }
	  private string sortedCourierDeclarationStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SortedCourierDeclarationStatus  
	   {
	    
	     get
		{
		   return sortedCourierDeclarationStatus;
		 }
		 set
		 {
		   if(sortedCourierDeclarationStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SortedCourierDeclarationStatus",OldValue=sortedCourierDeclarationStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sortedCourierDeclarationStatus=value;
		   }
			
		 }
	   }
	  private bool delivered ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Delivered  
	   {
	    
	     get
		{
		   return delivered;
		 }
		 set
		 {
		   if(delivered != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Delivered",OldValue=delivered,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   delivered=value;
		   }
			
		 }
	   }
	  private string truckerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TruckerId  
	   {
	    
	     get
		{
		   return truckerId;
		 }
		 set
		 {
		   if(truckerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TruckerId",OldValue=truckerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   truckerId=value;
		   }
			
		 }
	   }
	  private string distributionArea ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DistributionArea  
	   {
	    
	     get
		{
		   return distributionArea;
		 }
		 set
		 {
		   if(distributionArea != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DistributionArea",OldValue=distributionArea,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   distributionArea=value;
		   }
			
		 }
	   }
	  private string crateNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CrateNumber  
	   {
	    
	     get
		{
		   return crateNumber;
		 }
		 set
		 {
		   if(crateNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CrateNumber",OldValue=crateNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   crateNumber=value;
		   }
			
		 }
	   }
	  private string truckerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TruckerName  
	   {
	    
	     get
		{
		   return truckerName;
		 }
		 set
		 {
		   if(truckerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TruckerName",OldValue=truckerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   truckerName=value;
		   }
			
		 }
	   }
   }
   
}
	 