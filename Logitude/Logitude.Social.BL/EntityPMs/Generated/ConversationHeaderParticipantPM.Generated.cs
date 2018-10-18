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
using Logitude.Social.BL.Validators;
  
namespace Logitude.Social.BL.EntityPMs
{
   [CustomValidation(typeof(SocialClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class ConversationHeaderParticipantPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
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
	  private string conversationHeaderId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConversationHeaderId  
	   {
	    
	     get
		{
		   return conversationHeaderId;
		 }
		 set
		 {
		   if(conversationHeaderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConversationHeaderId",OldValue=conversationHeaderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   conversationHeaderId=value;
		   }
			
		 }
	   }
	  private string participantUserId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParticipantUserId  
	   {
	    
	     get
		{
		   return participantUserId;
		 }
		 set
		 {
		   if(participantUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParticipantUserId",OldValue=participantUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   participantUserId=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
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
	  private DateTime? leaveDate ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LeaveDate  
	   {
	    
	     get
		{
		   return leaveDate;
		 }
		 set
		 {
		   if(leaveDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeaveDate",OldValue=leaveDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   leaveDate=value;
		   }
			
		 }
	   }
	  private bool isLeft ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsLeft  
	   {
	    
	     get
		{
		   return isLeft;
		 }
		 set
		 {
		   if(isLeft != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsLeft",OldValue=isLeft,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isLeft=value;
		   }
			
		 }
	   }
	  private bool replied ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Replied  
	   {
	    
	     get
		{
		   return replied;
		 }
		 set
		 {
		   if(replied != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Replied",OldValue=replied,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   replied=value;
		   }
			
		 }
	   }
	  private bool isRead ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsRead  
	   {
	    
	     get
		{
		   return isRead;
		 }
		 set
		 {
		   if(isRead != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsRead",OldValue=isRead,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isRead=value;
		   }
			
		 }
	   }
	  private DateTime? lastReadDate ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastReadDate  
	   {
	    
	     get
		{
		   return lastReadDate;
		 }
		 set
		 {
		   if(lastReadDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastReadDate",OldValue=lastReadDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastReadDate=value;
		   }
			
		 }
	   }
	  private bool isDelete ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDelete  
	   {
	    
	     get
		{
		   return isDelete;
		 }
		 set
		 {
		   if(isDelete != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDelete",OldValue=isDelete,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDelete=value;
		   }
			
		 }
	   }
	  private DateTime? deleteDate ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DeleteDate  
	   {
	    
	     get
		{
		   return deleteDate;
		 }
		 set
		 {
		   if(deleteDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeleteDate",OldValue=deleteDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   deleteDate=value;
		   }
			
		 }
	   }
	  private string participantName ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParticipantName  
	   {
	    
	     get
		{
		   return participantName;
		 }
		 set
		 {
		   if(participantName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParticipantName",OldValue=participantName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   participantName=value;
		   }
			
		 }
	   }
   }
   
}
	 