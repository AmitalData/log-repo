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
   public partial class PostPM : EntityPM
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
	  private string createdById ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedById  
	   {
	    
	     get
		{
		   return createdById;
		 }
		 set
		 {
		   if(createdById != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedById",OldValue=createdById,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdById=value;
		   }
			
		 }
	   }
	  private string groupId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupId  
	   {
	    
	     get
		{
		   return groupId;
		 }
		 set
		 {
		   if(groupId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupId",OldValue=groupId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupId=value;
		   }
			
		 }
	   }
	  private string bodyText ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string BodyText  
	   {
	    
	     get
		{
		   return bodyText;
		 }
		 set
		 {
		   if(bodyText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BodyText",OldValue=bodyText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bodyText=value;
		   }
			
		 }
	   }
	  private DateTime? createDate ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
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
	  private string parentPostId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParentPostId  
	   {
	    
	     get
		{
		   return parentPostId;
		 }
		 set
		 {
		   if(parentPostId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentPostId",OldValue=parentPostId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parentPostId=value;
		   }
			
		 }
	   }
	  private int numberOfLikes ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public int NumberOfLikes  
	   {
	    
	     get
		{
		   return numberOfLikes;
		 }
		 set
		 {
		   if(numberOfLikes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfLikes",OldValue=numberOfLikes,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   numberOfLikes=value;
		   }
			
		 }
	   }
	  private bool isPrivate ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPrivate  
	   {
	    
	     get
		{
		   return isPrivate;
		 }
		 set
		 {
		   if(isPrivate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPrivate",OldValue=isPrivate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPrivate=value;
		   }
			
		 }
	   }
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCancelled  
	   {
	    
	     get
		{
		   return isCancelled;
		 }
		 set
		 {
		   if(isCancelled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancelled",OldValue=isCancelled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCancelled=value;
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
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
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

	   private List<PostPM> postComments;
	 
		     
	   [Include]
	   [Association("PostComments", "Id","ParentPostId")]
	   [DataMember]
	   public virtual List<PostPM> PostComments  
	   {
	        get
             {
                 if (postComments == null)
                 {
                     postComments = new List<PostPM>();
                 }
                 return postComments;
              }
             set { postComments = value; }
	    }
		   
	   private List<PostPM>  deletedPostComments;
	   public virtual List<PostPM> DeletedPostComments  
	   {
	        get
             {
                 if ( deletedPostComments == null)
                 {
                      deletedPostComments = new List<PostPM>();
                 }
                 return  deletedPostComments;
              }
             set {  deletedPostComments = value; }
	    }
	  
	   private List<PostLikePM> postLikes;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("PostPostLike", "Id","PostId")]
	   [DataMember]
	   public virtual List<PostLikePM> PostLikes  
	   {
	        get
             {
                 if (postLikes == null)
                 {
                     postLikes = new List<PostLikePM>();
                 }
                 return postLikes;
              }
             set { postLikes = value; }
	    }
		   
	   private List<PostLikePM>  deletedPostLikes;
	   public virtual List<PostLikePM> DeletedPostLikes  
	   {
	        get
             {
                 if ( deletedPostLikes == null)
                 {
                      deletedPostLikes = new List<PostLikePM>();
                 }
                 return  deletedPostLikes;
              }
             set {  deletedPostLikes = value; }
	    }
	  	  private bool isAutomatic ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAutomatic  
	   {
	    
	     get
		{
		   return isAutomatic;
		 }
		 set
		 {
		   if(isAutomatic != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAutomatic",OldValue=isAutomatic,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAutomatic=value;
		   }
			
		 }
	   }
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
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
	  private int numberOfComments ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public int NumberOfComments  
	   {
	    
	     get
		{
		   return numberOfComments;
		 }
		 set
		 {
		   if(numberOfComments != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfComments",OldValue=numberOfComments,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   numberOfComments=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
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
	  private int? indexColor ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public int? IndexColor  
	   {
	    
	     get
		{
		   return indexColor;
		 }
		 set
		 {
		   if(indexColor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IndexColor",OldValue=indexColor,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   indexColor=value;
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
   }
   
}
	 