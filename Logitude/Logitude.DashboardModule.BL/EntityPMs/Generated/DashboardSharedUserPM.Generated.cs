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
using Logitude.DashboardModule.BL.Validators;
  
namespace Logitude.DashboardModule.BL.EntityPMs
{
   [CustomValidation(typeof(DashboardModuleClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class DashboardSharedUserPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  private string dashboardId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DashboardId  
	   {
	    
	     get
		{
		   return dashboardId;
		 }
		 set
		 {
		   if(dashboardId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DashboardId",OldValue=dashboardId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dashboardId=value;
		   }
			
		 }
	   }
	  private string userId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  private string userName ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	 