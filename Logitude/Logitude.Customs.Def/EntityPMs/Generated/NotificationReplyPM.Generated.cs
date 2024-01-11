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
using Logitude.Customs.Def.Validators;
  
namespace Logitude.Customs.Def.EntityPMs
{
   [CustomValidation(typeof(CustomsClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class NotificationReplyPM : EntityPM
   {
   	  private string notificationId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotificationId  
	   {
	    
	     get
		{
		   return notificationId;
		 }
		 set
		 {
		   if(notificationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotificationId",OldValue=notificationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notificationId=value;
		   }
			
		 }
	   }
	  private int line ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int Line  
	   {
	    
	     get
		{
		   return line;
		 }
		 set
		 {
		   if(line != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Line",OldValue=line,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   line=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string responseToCustoms ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ResponseToCustoms  
	   {
	    
	     get
		{
		   return responseToCustoms;
		 }
		 set
		 {
		   if(responseToCustoms != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResponseToCustoms",OldValue=responseToCustoms,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   responseToCustoms=value;
		   }
			
		 }
	   }
	  private string repliedByUserId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RepliedByUserId  
	   {
	    
	     get
		{
		   return repliedByUserId;
		 }
		 set
		 {
		   if(repliedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RepliedByUserId",OldValue=repliedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   repliedByUserId=value;
		   }
			
		 }
	   }
	  private DateTime? replyDateTime ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ReplyDateTime  
	   {
	    
	     get
		{
		   return replyDateTime;
		 }
		 set
		 {
		   if(replyDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReplyDateTime",OldValue=replyDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   replyDateTime=value;
		   }
			
		 }
	   }
	  private string repliedByUserName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RepliedByUserName  
	   {
	    
	     get
		{
		   return repliedByUserName;
		 }
		 set
		 {
		   if(repliedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RepliedByUserName",OldValue=repliedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   repliedByUserName=value;
		   }
			
		 }
	   }
	    }
   
}
	 