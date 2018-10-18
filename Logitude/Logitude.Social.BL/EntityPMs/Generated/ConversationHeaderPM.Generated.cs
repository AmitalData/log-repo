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
   public partial class ConversationHeaderPM : EntityPM
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
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
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
	  private string objectTableId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
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
	  private bool isWaitingForResponse ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsWaitingForResponse  
	   {
	    
	     get
		{
		   return isWaitingForResponse;
		 }
		 set
		 {
		   if(isWaitingForResponse != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsWaitingForResponse",OldValue=isWaitingForResponse,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isWaitingForResponse=value;
		   }
			
		 }
	   }
	  private string entityDescription ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityDescription  
	   {
	    
	     get
		{
		   return entityDescription;
		 }
		 set
		 {
		   if(entityDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityDescription",OldValue=entityDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityDescription=value;
		   }
			
		 }
	   }
	  private string lasMessageUserId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string LasMessageUserId  
	   {
	    
	     get
		{
		   return lasMessageUserId;
		 }
		 set
		 {
		   if(lasMessageUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LasMessageUserId",OldValue=lasMessageUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lasMessageUserId=value;
		   }
			
		 }
	   }
	  private string lasMessageUserName ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string LasMessageUserName  
	   {
	    
	     get
		{
		   return lasMessageUserName;
		 }
		 set
		 {
		   if(lasMessageUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LasMessageUserName",OldValue=lasMessageUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lasMessageUserName=value;
		   }
			
		 }
	   }
	  private string lasMessageBody ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string LasMessageBody  
	   {
	    
	     get
		{
		   return lasMessageBody;
		 }
		 set
		 {
		   if(lasMessageBody != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LasMessageBody",OldValue=lasMessageBody,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lasMessageBody=value;
		   }
			
		 }
	   }
	  private string messageParticipants ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string MessageParticipants  
	   {
	    
	     get
		{
		   return messageParticipants;
		 }
		 set
		 {
		   if(messageParticipants != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MessageParticipants",OldValue=messageParticipants,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   messageParticipants=value;
		   }
			
		 }
	   }
	  private DateTime? lastMessageDate ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastMessageDate  
	   {
	    
	     get
		{
		   return lastMessageDate;
		 }
		 set
		 {
		   if(lastMessageDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastMessageDate",OldValue=lastMessageDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastMessageDate=value;
		   }
			
		 }
	   }
	  private int messageParticipantsCount ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public int MessageParticipantsCount  
	   {
	    
	     get
		{
		   return messageParticipantsCount;
		 }
		 set
		 {
		   if(messageParticipantsCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MessageParticipantsCount",OldValue=messageParticipantsCount,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   messageParticipantsCount=value;
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
	  private bool isReplied ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsReplied  
	   {
	    
	     get
		{
		   return isReplied;
		 }
		 set
		 {
		   if(isReplied != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsReplied",OldValue=isReplied,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isReplied=value;
		   }
			
		 }
	   }
	  private string userImageDetailId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string UserImageDetailId  
	   {
	    
	     get
		{
		   return userImageDetailId;
		 }
		 set
		 {
		   if(userImageDetailId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UserImageDetailId",OldValue=userImageDetailId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   userImageDetailId=value;
		   }
			
		 }
	   }
	  private string firstImageDetailId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string FirstImageDetailId  
	   {
	    
	     get
		{
		   return firstImageDetailId;
		 }
		 set
		 {
		   if(firstImageDetailId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstImageDetailId",OldValue=firstImageDetailId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   firstImageDetailId=value;
		   }
			
		 }
	   }
	  private string firstMessageUserId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string FirstMessageUserId  
	   {
	    
	     get
		{
		   return firstMessageUserId;
		 }
		 set
		 {
		   if(firstMessageUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstMessageUserId",OldValue=firstMessageUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   firstMessageUserId=value;
		   }
			
		 }
	   }
	  private int numberUnreadComment ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public int NumberUnreadComment  
	   {
	    
	     get
		{
		   return numberUnreadComment;
		 }
		 set
		 {
		   if(numberUnreadComment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberUnreadComment",OldValue=numberUnreadComment,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   numberUnreadComment=value;
		   }
			
		 }
	   }
	  private string firstMessageBody ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string FirstMessageBody  
	   {
	    
	     get
		{
		   return firstMessageBody;
		 }
		 set
		 {
		   if(firstMessageBody != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstMessageBody",OldValue=firstMessageBody,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   firstMessageBody=value;
		   }
			
		 }
	   }
   }
   
}
	 