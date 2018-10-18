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
   public partial class PostLikePM : EntityPM
   {
   	  private string postId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string PostId  
	   {
	    
	     get
		{
		   return postId;
		 }
		 set
		 {
		   if(postId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PostId",OldValue=postId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   postId=value;
		   }
			
		 }
	   }
	  private string userId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string UserId  
	   {
	    
	     get
		{
		   return userId;
		 }
		 set
		 {
		   if(userId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UserId",OldValue=userId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   userId=value;
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
   }
   
}
	 