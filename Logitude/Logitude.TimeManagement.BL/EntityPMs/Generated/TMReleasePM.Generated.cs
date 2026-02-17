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
using Logitude.TimeManagement.BL.Validators;
  
namespace Logitude.TimeManagement.BL.EntityPMs
{
   [CustomValidation(typeof(TimeManagementClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TMReleasePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
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
	  private string releaseName ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReleaseName  
	   {
	    
	     get
		{
		   return releaseName;
		 }
		 set
		 {
		   if(releaseName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReleaseName",OldValue=releaseName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   releaseName=value;
		   }
			
		 }
	   }
	  private DateTime fromDate ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime FromDate  
	   {
	    
	     get
		{
		   return fromDate;
		 }
		 set
		 {
		   if(fromDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromDate",OldValue=fromDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   fromDate=value;
		   }
			
		 }
	   }
	  private DateTime toDate ;
	  	  
       
	   [CustomValidation(typeof(TimeManagementValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ToDate  
	   {
	    
	     get
		{
		   return toDate;
		 }
		 set
		 {
		   if(toDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToDate",OldValue=toDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   toDate=value;
		   }
			
		 }
	   }
   }
   
}
	 