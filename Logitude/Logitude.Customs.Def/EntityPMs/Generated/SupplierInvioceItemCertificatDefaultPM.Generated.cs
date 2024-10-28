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
   public partial class SupplierInvioceItemCertificatDefaultPM : EntityPM
   {
   	  private string supplierInvioceExportDefaultId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SupplierInvioceExportDefaultId  
	   {
	    
	     get
		{
		   return supplierInvioceExportDefaultId;
		 }
		 set
		 {
		   if(supplierInvioceExportDefaultId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SupplierInvioceExportDefaultId",OldValue=supplierInvioceExportDefaultId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   supplierInvioceExportDefaultId=value;
		   }
			
		 }
	   }
	  private string certificateNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CertificateNumber  
	   {
	    
	     get
		{
		   return certificateNumber;
		 }
		 set
		 {
		   if(certificateNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CertificateNumber",OldValue=certificateNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   certificateNumber=value;
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
	  private string reqConfirmationTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReqConfirmationTypeCode  
	   {
	    
	     get
		{
		   return reqConfirmationTypeCode;
		 }
		 set
		 {
		   if(reqConfirmationTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReqConfirmationTypeCode",OldValue=reqConfirmationTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reqConfirmationTypeCode=value;
		   }
			
		 }
	   }
	  private string certificateExemptionTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CertificateExemptionTypeCode  
	   {
	    
	     get
		{
		   return certificateExemptionTypeCode;
		 }
		 set
		 {
		   if(certificateExemptionTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CertificateExemptionTypeCode",OldValue=certificateExemptionTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   certificateExemptionTypeCode=value;
		   }
			
		 }
	   }
	  private string attachmentTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AttachmentTypeCode  
	   {
	    
	     get
		{
		   return attachmentTypeCode;
		 }
		 set
		 {
		   if(attachmentTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AttachmentTypeCode",OldValue=attachmentTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   attachmentTypeCode=value;
		   }
			
		 }
	   }
	  private string resConfirmationTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ResConfirmationTypeCode  
	   {
	    
	     get
		{
		   return resConfirmationTypeCode;
		 }
		 set
		 {
		   if(resConfirmationTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResConfirmationTypeCode",OldValue=resConfirmationTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   resConfirmationTypeCode=value;
		   }
			
		 }
	   }
	  private string customsAttachmentID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsAttachmentID  
	   {
	    
	     get
		{
		   return customsAttachmentID;
		 }
		 set
		 {
		   if(customsAttachmentID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsAttachmentID",OldValue=customsAttachmentID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsAttachmentID=value;
		   }
			
		 }
	   }
	  private int sequenceNumeric ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int SequenceNumeric  
	   {
	    
	     get
		{
		   return sequenceNumeric;
		 }
		 set
		 {
		   if(sequenceNumeric != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SequenceNumeric",OldValue=sequenceNumeric,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   sequenceNumeric=value;
		   }
			
		 }
	   }
	    }
   
}
	 