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
   public partial class FollowEntityPM : EntityPM
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
	  private string followerUserId ;
	  	  
       
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
	  private DateTime? cancelledDate ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CancelledDate  
	   {
	    
	     get
		{
		   return cancelledDate;
		 }
		 set
		 {
		   if(cancelledDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CancelledDate",OldValue=cancelledDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   cancelledDate=value;
		   }
			
		 }
	   }
	  private string followerName ;
	  	  
       
	   [CustomValidation(typeof(SocialValidationClass), "ValidateClass")]
	   [DataMember]
       public string FollowerName  
	   {
	    
	     get
		{
		   return followerName;
		 }
		 set
		 {
		   if(followerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowerName",OldValue=followerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   followerName=value;
		   }
			
		 }
	   }
   }
   
}
	 