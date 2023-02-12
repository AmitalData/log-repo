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
using Logitude.Infrastructure.BL.Validators;
  
namespace Logitude.Infrastructure.BL.EntityPMs
{
   [CustomValidation(typeof(InfrastructureClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class LastRunDetailPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private DateTime lastRunDate ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime LastRunDate  
	   {
	    
	     get
		{
		   return lastRunDate;
		 }
		 set
		 {
		   if(lastRunDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastRunDate",OldValue=lastRunDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   lastRunDate=value;
		   }
			
		 }
	   }
	  private string lastRunByUserId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastRunByUserId  
	   {
	    
	     get
		{
		   return lastRunByUserId;
		 }
		 set
		 {
		   if(lastRunByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastRunByUserId",OldValue=lastRunByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastRunByUserId=value;
		   }
			
		 }
	   }
	  private string lastRunByUserName ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastRunByUserName  
	   {
	    
	     get
		{
		   return lastRunByUserName;
		 }
		 set
		 {
		   if(lastRunByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastRunByUserName",OldValue=lastRunByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastRunByUserName=value;
		   }
			
		 }
	   }
	    }
   
}
	 