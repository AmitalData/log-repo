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
   public partial class ConversationHeaderMessagePM : EntityPM
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
	  private string messageBody ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string MessageBody  
	   {
	    
	     get
		{
		   return messageBody;
		 }
		 set
		 {
		   if(messageBody != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MessageBody",OldValue=messageBody,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   messageBody=value;
		   }
			
		 }
	   }
	  private string userName ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string UserName  
	   {
	    
	     get
		{
		   return userName;
		 }
		 set
		 {
		   if(userName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UserName",OldValue=userName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   userName=value;
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
	  private string imageDetailId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImageDetailId  
	   {
	    
	     get
		{
		   return imageDetailId;
		 }
		 set
		 {
		   if(imageDetailId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImageDetailId",OldValue=imageDetailId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   imageDetailId=value;
		   }
			
		 }
	   }
	  private string defaultColor ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string DefaultColor  
	   {
	    
	     get
		{
		   return defaultColor;
		 }
		 set
		 {
		   if(defaultColor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefaultColor",OldValue=defaultColor,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   defaultColor=value;
		   }
			
		 }
	   }
	  private string regardingEntity ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string RegardingEntity  
	   {
	    
	     get
		{
		   return regardingEntity;
		 }
		 set
		 {
		   if(regardingEntity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RegardingEntity",OldValue=regardingEntity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   regardingEntity=value;
		   }
			
		 }
	   }
   }
   
}
	 