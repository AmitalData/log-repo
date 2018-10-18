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
using Logitude.CRM.BL.Validators;
  
namespace Logitude.CRM.BL.EntityPMs
{
   [CustomValidation(typeof(CRMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class CorrespondencePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string createdByContactId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByContactId  
	   {
	    
	     get
		{
		   return createdByContactId;
		 }
		 set
		 {
		   if(createdByContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByContactId",OldValue=createdByContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByContactId=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string description ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Description  
	   {
	    
	     get
		{
		   return description;
		 }
		 set
		 {
		   if(description != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Description",OldValue=description,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   description=value;
		   }
			
		 }
	   }
	  private bool isInternal ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsInternal  
	   {
	    
	     get
		{
		   return isInternal;
		 }
		 set
		 {
		   if(isInternal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsInternal",OldValue=isInternal,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isInternal=value;
		   }
			
		 }
	   }
	  private string objectTableId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ObjectTableId  
	   {
	    
	     get
		{
		   return objectTableId;
		 }
		 set
		 {
		   if(objectTableId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ObjectTableId",OldValue=objectTableId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   objectTableId=value;
		   }
			
		 }
	   }
	  private string entityId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityId  
	   {
	    
	     get
		{
		   return entityId;
		 }
		 set
		 {
		   if(entityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityId",OldValue=entityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityId=value;
		   }
			
		 }
	   }
	  private string activityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivityTypeCode  
	   {
	    
	     get
		{
		   return activityTypeCode;
		 }
		 set
		 {
		   if(activityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivityTypeCode",OldValue=activityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activityTypeCode=value;
		   }
			
		 }
	   }
	  private string activityId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivityId  
	   {
	    
	     get
		{
		   return activityId;
		 }
		 set
		 {
		   if(activityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivityId",OldValue=activityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activityId=value;
		   }
			
		 }
	   }
	  private string activitySubject ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActivitySubject  
	   {
	    
	     get
		{
		   return activitySubject;
		 }
		 set
		 {
		   if(activitySubject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivitySubject",OldValue=activitySubject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   activitySubject=value;
		   }
			
		 }
	   }
	  private string contactName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactName  
	   {
	    
	     get
		{
		   return contactName;
		 }
		 set
		 {
		   if(contactName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactName",OldValue=contactName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactName=value;
		   }
			
		 }
	   }
	  private string cCs ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CCs  
	   {
	    
	     get
		{
		   return cCs;
		 }
		 set
		 {
		   if(cCs != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CCs",OldValue=cCs,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cCs=value;
		   }
			
		 }
	   }
	  private string bcc ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Bcc  
	   {
	    
	     get
		{
		   return bcc;
		 }
		 set
		 {
		   if(bcc != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Bcc",OldValue=bcc,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bcc=value;
		   }
			
		 }
	   }
	  private bool notifyMe ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool NotifyMe  
	   {
	    
	     get
		{
		   return notifyMe;
		 }
		 set
		 {
		   if(notifyMe != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyMe",OldValue=notifyMe,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   notifyMe=value;
		   }
			
		 }
	   }
	  private bool notifyOwner ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool NotifyOwner  
	   {
	    
	     get
		{
		   return notifyOwner;
		 }
		 set
		 {
		   if(notifyOwner != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyOwner",OldValue=notifyOwner,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   notifyOwner=value;
		   }
			
		 }
	   }
	  private string internalUsers ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string InternalUsers  
	   {
	    
	     get
		{
		   return internalUsers;
		 }
		 set
		 {
		   if(internalUsers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InternalUsers",OldValue=internalUsers,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   internalUsers=value;
		   }
			
		 }
	   }
	  private string contactEmail ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactEmail  
	   {
	    
	     get
		{
		   return contactEmail;
		 }
		 set
		 {
		   if(contactEmail != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactEmail",OldValue=contactEmail,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactEmail=value;
		   }
			
		 }
	   }
	  private string direction ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Direction  
	   {
	    
	     get
		{
		   return direction;
		 }
		 set
		 {
		   if(direction != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Direction",OldValue=direction,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   direction=value;
		   }
			
		 }
	   }
	  private string hTMLFullBody ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string HTMLFullBody  
	   {
	    
	     get
		{
		   return hTMLFullBody;
		 }
		 set
		 {
		   if(hTMLFullBody != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HTMLFullBody",OldValue=hTMLFullBody,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   hTMLFullBody=value;
		   }
			
		 }
	   }
	  private bool rightToLeft ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool RightToLeft  
	   {
	    
	     get
		{
		   return rightToLeft;
		 }
		 set
		 {
		   if(rightToLeft != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RightToLeft",OldValue=rightToLeft,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   rightToLeft=value;
		   }
			
		 }
	   }

	   private List<string> attachments;
	 
	   [DataMember]
	   public virtual List<string> Attachments  
	   {
	        get
             {
                 if (attachments == null)
                 {
                     attachments = new List<string>();
                 }
                 return attachments;
              }
             set { attachments = value; }
	    }
		   
	   private List<string>  deletedAttachments;
	   public virtual List<string> DeletedAttachments  
	   {
	        get
             {
                 if ( deletedAttachments == null)
                 {
                      deletedAttachments = new List<string>();
                 }
                 return  deletedAttachments;
              }
             set {  deletedAttachments = value; }
	    }
	     }
   
}
	 