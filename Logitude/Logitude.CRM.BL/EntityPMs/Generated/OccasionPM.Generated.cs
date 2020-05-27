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
   public partial class OccasionPM : EntityPM
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
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string name ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Name  
	   {
	    
	     get
		{
		   return name;
		 }
		 set
		 {
		   if(name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Name",OldValue=name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   name=value;
		   }
			
		 }
	   }
	  private DateTime? startDateTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartDateTime  
	   {
	    
	     get
		{
		   return startDateTime;
		 }
		 set
		 {
		   if(startDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDateTime",OldValue=startDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startDateTime=value;
		   }
			
		 }
	   }
	  private DateTime? endDateTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndDateTime  
	   {
	    
	     get
		{
		   return endDateTime;
		 }
		 set
		 {
		   if(endDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndDateTime",OldValue=endDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endDateTime=value;
		   }
			
		 }
	   }
	  private string goal ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Goal  
	   {
	    
	     get
		{
		   return goal;
		 }
		 set
		 {
		   if(goal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Goal",OldValue=goal,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   goal=value;
		   }
			
		 }
	   }
	  private string location ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Location  
	   {
	    
	     get
		{
		   return location;
		 }
		 set
		 {
		   if(location != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Location",OldValue=location,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   location=value;
		   }
			
		 }
	   }
	  private string ownerId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OwnerId  
	   {
	    
	     get
		{
		   return ownerId;
		 }
		 set
		 {
		   if(ownerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OwnerId",OldValue=ownerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ownerId=value;
		   }
			
		 }
	   }
	  private string industryId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string IndustryId  
	   {
	    
	     get
		{
		   return industryId;
		 }
		 set
		 {
		   if(industryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IndustryId",OldValue=industryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   industryId=value;
		   }
			
		 }
	   }
	  private string occasionTypeId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OccasionTypeId  
	   {
	    
	     get
		{
		   return occasionTypeId;
		 }
		 set
		 {
		   if(occasionTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OccasionTypeId",OldValue=occasionTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   occasionTypeId=value;
		   }
			
		 }
	   }
	  private string occasionStatusId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OccasionStatusId  
	   {
	    
	     get
		{
		   return occasionStatusId;
		 }
		 set
		 {
		   if(occasionStatusId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OccasionStatusId",OldValue=occasionStatusId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   occasionStatusId=value;
		   }
			
		 }
	   }
	  private string createdByContactName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByContactName  
	   {
	    
	     get
		{
		   return createdByContactName;
		 }
		 set
		 {
		   if(createdByContactName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByContactName",OldValue=createdByContactName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByContactName=value;
		   }
			
		 }
	   }
	  private string updatedByUserName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserName  
	   {
	    
	     get
		{
		   return updatedByUserName;
		 }
		 set
		 {
		   if(updatedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserName",OldValue=updatedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserName=value;
		   }
			
		 }
	   }
	  private string typeName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeName  
	   {
	    
	     get
		{
		   return typeName;
		 }
		 set
		 {
		   if(typeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeName",OldValue=typeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeName=value;
		   }
			
		 }
	   }
	  private string ownerName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OwnerName  
	   {
	    
	     get
		{
		   return ownerName;
		 }
		 set
		 {
		   if(ownerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OwnerName",OldValue=ownerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ownerName=value;
		   }
			
		 }
	   }
	  private string occasionStatusName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OccasionStatusName  
	   {
	    
	     get
		{
		   return occasionStatusName;
		 }
		 set
		 {
		   if(occasionStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OccasionStatusName",OldValue=occasionStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   occasionStatusName=value;
		   }
			
		 }
	   }
	  private string industryName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string IndustryName  
	   {
	    
	     get
		{
		   return industryName;
		 }
		 set
		 {
		   if(industryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IndustryName",OldValue=industryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   industryName=value;
		   }
			
		 }
	   }

	   private List<OccasionInviteePM> occasionInvitees;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("OccasionInviteeOccasion", "Id","OccasionId")]
	   [DataMember]
	   public virtual List<OccasionInviteePM> OccasionInvitees  
	   {
	        get
             {
                 if (occasionInvitees == null)
                 {
                     occasionInvitees = new List<OccasionInviteePM>();
                 }
                 return occasionInvitees;
              }
             set { occasionInvitees = value; }
	    }
		   
	   private List<OccasionInviteePM>  deletedOccasionInvitees;
	   public virtual List<OccasionInviteePM> DeletedOccasionInvitees  
	   {
	        get
             {
                 if ( deletedOccasionInvitees == null)
                 {
                      deletedOccasionInvitees = new List<OccasionInviteePM>();
                 }
                 return  deletedOccasionInvitees;
              }
             set {  deletedOccasionInvitees = value; }
	    }
	  	  private int participatedCustomers ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int ParticipatedCustomers  
	   {
	    
	     get
		{
		   return participatedCustomers;
		 }
		 set
		 {
		   if(participatedCustomers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParticipatedCustomers",OldValue=participatedCustomers,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   participatedCustomers=value;
		   }
			
		 }
	   }
	  private int participatedContacts ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int ParticipatedContacts  
	   {
	    
	     get
		{
		   return participatedContacts;
		 }
		 set
		 {
		   if(participatedContacts != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParticipatedContacts",OldValue=participatedContacts,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   participatedContacts=value;
		   }
			
		 }
	   }
	  private int invitedCustomers ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int InvitedCustomers  
	   {
	    
	     get
		{
		   return invitedCustomers;
		 }
		 set
		 {
		   if(invitedCustomers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvitedCustomers",OldValue=invitedCustomers,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   invitedCustomers=value;
		   }
			
		 }
	   }
	  private int invitedContacts ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int InvitedContacts  
	   {
	    
	     get
		{
		   return invitedContacts;
		 }
		 set
		 {
		   if(invitedContacts != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvitedContacts",OldValue=invitedContacts,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   invitedContacts=value;
		   }
			
		 }
	   }
	  private bool isAllAdded ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAllAdded  
	   {
	    
	     get
		{
		   return isAllAdded;
		 }
		 set
		 {
		   if(isAllAdded != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAllAdded",OldValue=isAllAdded,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAllAdded=value;
		   }
			
		 }
	   }

	   private List<OccasionInviteePM> removedOccasionInvitees;
	 
	   [DataMember]
	   public virtual List<OccasionInviteePM> RemovedOccasionInvitees  
	   {
	        get
             {
                 if (removedOccasionInvitees == null)
                 {
                     removedOccasionInvitees = new List<OccasionInviteePM>();
                 }
                 return removedOccasionInvitees;
              }
             set { removedOccasionInvitees = value; }
	    }
		   
	   private List<OccasionInviteePM>  deletedRemovedOccasionInvitees;
	   public virtual List<OccasionInviteePM> DeletedRemovedOccasionInvitees  
	   {
	        get
             {
                 if ( deletedRemovedOccasionInvitees == null)
                 {
                      deletedRemovedOccasionInvitees = new List<OccasionInviteePM>();
                 }
                 return  deletedRemovedOccasionInvitees;
              }
             set {  deletedRemovedOccasionInvitees = value; }
	    }
	     }
   
}
	 