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
   public partial class ConfirmationTypePM : EntityPM
   {
   	  private string code ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Code  
	   {
	    
	     get
		{
		   return code;
		 }
		 set
		 {
		   if(code != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Code",OldValue=code,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   code=value;
		   }
			
		 }
	   }
	  private string englishName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EnglishName  
	   {
	    
	     get
		{
		   return englishName;
		 }
		 set
		 {
		   if(englishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EnglishName",OldValue=englishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   englishName=value;
		   }
			
		 }
	   }
	  private string localName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LocalName  
	   {
	    
	     get
		{
		   return localName;
		 }
		 set
		 {
		   if(localName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalName",OldValue=localName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   localName=value;
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
	  private bool inactive ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Inactive  
	   {
	    
	     get
		{
		   return inactive;
		 }
		 set
		 {
		   if(inactive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Inactive",OldValue=inactive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   inactive=value;
		   }
			
		 }
	   }
	  private int? malamID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? MalamID  
	   {
	    
	     get
		{
		   return malamID;
		 }
		 set
		 {
		   if(malamID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MalamID",OldValue=malamID,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   malamID=value;
		   }
			
		 }
	   }
	  private int? state ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? State  
	   {
	    
	     get
		{
		   return state;
		 }
		 set
		 {
		   if(state != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="State",OldValue=state,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   state=value;
		   }
			
		 }
	   }
	  private int? exempt_CertificateDocument ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Exempt_CertificateDocument  
	   {
	    
	     get
		{
		   return exempt_CertificateDocument;
		 }
		 set
		 {
		   if(exempt_CertificateDocument != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Exempt_CertificateDocument",OldValue=exempt_CertificateDocument,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   exempt_CertificateDocument=value;
		   }
			
		 }
	   }
	  private bool? isImport ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsImport  
	   {
	    
	     get
		{
		   return isImport;
		 }
		 set
		 {
		   if(isImport != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsImport",OldValue=isImport,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isImport=value;
		   }
			
		 }
	   }
	  private bool? isExemptOtherAuthority ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsExemptOtherAuthority  
	   {
	    
	     get
		{
		   return isExemptOtherAuthority;
		 }
		 set
		 {
		   if(isExemptOtherAuthority != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsExemptOtherAuthority",OldValue=isExemptOtherAuthority,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isExemptOtherAuthority=value;
		   }
			
		 }
	   }
	  private int? confirmationComputerization ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ConfirmationComputerization  
	   {
	    
	     get
		{
		   return confirmationComputerization;
		 }
		 set
		 {
		   if(confirmationComputerization != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConfirmationComputerization",OldValue=confirmationComputerization,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   confirmationComputerization=value;
		   }
			
		 }
	   }
	  private bool? isCEO ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsCEO  
	   {
	    
	     get
		{
		   return isCEO;
		 }
		 set
		 {
		   if(isCEO != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCEO",OldValue=isCEO,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isCEO=value;
		   }
			
		 }
	   }
	  private bool? isNeedDeclaration ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsNeedDeclaration  
	   {
	    
	     get
		{
		   return isNeedDeclaration;
		 }
		 set
		 {
		   if(isNeedDeclaration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsNeedDeclaration",OldValue=isNeedDeclaration,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isNeedDeclaration=value;
		   }
			
		 }
	   }
	  private int? certificateDocumentCategory ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? CertificateDocumentCategory  
	   {
	    
	     get
		{
		   return certificateDocumentCategory;
		 }
		 set
		 {
		   if(certificateDocumentCategory != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CertificateDocumentCategory",OldValue=certificateDocumentCategory,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   certificateDocumentCategory=value;
		   }
			
		 }
	   }
	  private int? authorityID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? AuthorityID  
	   {
	    
	     get
		{
		   return authorityID;
		 }
		 set
		 {
		   if(authorityID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AuthorityID",OldValue=authorityID,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   authorityID=value;
		   }
			
		 }
	   }
	  private bool? isQuotaCheckNeeded ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsQuotaCheckNeeded  
	   {
	    
	     get
		{
		   return isQuotaCheckNeeded;
		 }
		 set
		 {
		   if(isQuotaCheckNeeded != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsQuotaCheckNeeded",OldValue=isQuotaCheckNeeded,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isQuotaCheckNeeded=value;
		   }
			
		 }
	   }
	  private int? externalIDNumPerAuthority ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ExternalIDNumPerAuthority  
	   {
	    
	     get
		{
		   return externalIDNumPerAuthority;
		 }
		 set
		 {
		   if(externalIDNumPerAuthority != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalIDNumPerAuthority",OldValue=externalIDNumPerAuthority,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   externalIDNumPerAuthority=value;
		   }
			
		 }
	   }
	  private bool? isForCustomsItem ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsForCustomsItem  
	   {
	    
	     get
		{
		   return isForCustomsItem;
		 }
		 set
		 {
		   if(isForCustomsItem != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsForCustomsItem",OldValue=isForCustomsItem,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isForCustomsItem=value;
		   }
			
		 }
	   }
	  private bool? isPharmacy ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsPharmacy  
	   {
	    
	     get
		{
		   return isPharmacy;
		 }
		 set
		 {
		   if(isPharmacy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPharmacy",OldValue=isPharmacy,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isPharmacy=value;
		   }
			
		 }
	   }
	  private bool? isVeterinarian ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsVeterinarian  
	   {
	    
	     get
		{
		   return isVeterinarian;
		 }
		 set
		 {
		   if(isVeterinarian != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsVeterinarian",OldValue=isVeterinarian,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isVeterinarian=value;
		   }
			
		 }
	   }
	  private bool? isVehicleStandardization ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsVehicleStandardization  
	   {
	    
	     get
		{
		   return isVehicleStandardization;
		 }
		 set
		 {
		   if(isVehicleStandardization != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsVehicleStandardization",OldValue=isVehicleStandardization,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isVehicleStandardization=value;
		   }
			
		 }
	   }
	  private bool? isQuantityMandatory ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsQuantityMandatory  
	   {
	    
	     get
		{
		   return isQuantityMandatory;
		 }
		 set
		 {
		   if(isQuantityMandatory != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsQuantityMandatory",OldValue=isQuantityMandatory,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isQuantityMandatory=value;
		   }
			
		 }
	   }
	  private bool? isForCE ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsForCE  
	   {
	    
	     get
		{
		   return isForCE;
		 }
		 set
		 {
		   if(isForCE != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsForCE",OldValue=isForCE,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isForCE=value;
		   }
			
		 }
	   }
   }
   
}
	 