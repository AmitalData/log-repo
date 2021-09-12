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
using Amital.QuoteOPM.Def.Validators;
  
namespace Amital.QuoteOPM.Def.EntityPMs
{
   [CustomValidation(typeof(QuoteOPMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class QuoteOPDocumentVersionPM : EntityPM
   {
   	  private string quoteOPId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteOPId  
	   {
	    
	     get
		{
		   return quoteOPId;
		 }
		 set
		 {
		   if(quoteOPId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteOPId",OldValue=quoteOPId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteOPId=value;
		   }
			
		 }
	   }
	  private int versionNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int VersionNumber  
	   {
	    
	     get
		{
		   return versionNumber;
		 }
		 set
		 {
		   if(versionNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VersionNumber",OldValue=versionNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   versionNumber=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private DateTime? createDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private DateTime? sendDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? SendDate  
	   {
	    
	     get
		{
		   return sendDate;
		 }
		 set
		 {
		   if(sendDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SendDate",OldValue=sendDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   sendDate=value;
		   }
			
		 }
	   }
	  private string versionType ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VersionType  
	   {
	    
	     get
		{
		   return versionType;
		 }
		 set
		 {
		   if(versionType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VersionType",OldValue=versionType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   versionType=value;
		   }
			
		 }
	   }
	  private string documentId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DocumentId  
	   {
	    
	     get
		{
		   return documentId;
		 }
		 set
		 {
		   if(documentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentId",OldValue=documentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   documentId=value;
		   }
			
		 }
	   }
	  private bool isSent ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSent  
	   {
	    
	     get
		{
		   return isSent;
		 }
		 set
		 {
		   if(isSent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSent",OldValue=isSent,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSent=value;
		   }
			
		 }
	   }
	  private string quoteOPTemplateId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteOPTemplateId  
	   {
	    
	     get
		{
		   return quoteOPTemplateId;
		 }
		 set
		 {
		   if(quoteOPTemplateId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteOPTemplateId",OldValue=quoteOPTemplateId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteOPTemplateId=value;
		   }
			
		 }
	   }
	  private string versionTypeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VersionTypeName  
	   {
	    
	     get
		{
		   return versionTypeName;
		 }
		 set
		 {
		   if(versionTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VersionTypeName",OldValue=versionTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   versionTypeName=value;
		   }
			
		 }
	   }
	  private double fileSize ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double FileSize  
	   {
	    
	     get
		{
		   return fileSize;
		 }
		 set
		 {
		   if(fileSize != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileSize",OldValue=fileSize,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   fileSize=value;
		   }
			
		 }
	   }
	  private string fileName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FileName  
	   {
	    
	     get
		{
		   return fileName;
		 }
		 set
		 {
		   if(fileName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileName",OldValue=fileName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fileName=value;
		   }
			
		 }
	   }
	  private string extension ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Extension  
	   {
	    
	     get
		{
		   return extension;
		 }
		 set
		 {
		   if(extension != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Extension",OldValue=extension,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   extension=value;
		   }
			
		 }
	   }
	  private string displayVersionTypeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DisplayVersionTypeName  
	   {
	    
	     get
		{
		   return displayVersionTypeName;
		 }
		 set
		 {
		   if(displayVersionTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DisplayVersionTypeName",OldValue=displayVersionTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   displayVersionTypeName=value;
		   }
			
		 }
	   }
	  private string updateByUserName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdateByUserName  
	   {
	    
	     get
		{
		   return updateByUserName;
		 }
		 set
		 {
		   if(updateByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateByUserName",OldValue=updateByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updateByUserName=value;
		   }
			
		 }
	   }
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
   }
   
}
	 