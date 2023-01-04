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
   public partial class UserPinnedDashboardPM : EntityPM
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
	  private string dashboards ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Dashboards  
	   {
	    
	     get
		{
		   return dashboards;
		 }
		 set
		 {
		   if(dashboards != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Dashboards",OldValue=dashboards,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dashboards=value;
		   }
			
		 }
	   }
   }
   
}
	 