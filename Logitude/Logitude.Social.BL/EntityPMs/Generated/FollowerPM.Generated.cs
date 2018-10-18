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
   public partial class FollowerPM : EntityPM
   {
   	  private string followeeUserId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string FolloweeUserId  
	   {
	    
	     get
		{
		   return followeeUserId;
		 }
		 set
		 {
		   if(followeeUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FolloweeUserId",OldValue=followeeUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   followeeUserId=value;
		   }
			
		 }
	   }
	  private string followerUserId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string FollowerUserId  
	   {
	    
	     get
		{
		   return followerUserId;
		 }
		 set
		 {
		   if(followerUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowerUserId",OldValue=followerUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   followerUserId=value;
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
	  private bool? cancelledDate ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? CancelledDate  
	   {
	    
	     get
		{
		   return cancelledDate;
		 }
		 set
		 {
		   if(cancelledDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CancelledDate",OldValue=cancelledDate,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   cancelledDate=value;
		   }
			
		 }
	   }
   }
   
}
	 