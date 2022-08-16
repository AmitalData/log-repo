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
   public partial class WidgetPM : EntityPM
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
	  private string title ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string Title  
	   {
	    
	     get
		{
		   return title;
		 }
		 set
		 {
		   if(title != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Title",OldValue=title,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   title=value;
		   }
			
		 }
	   }
	  private string groupBy ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupBy  
	   {
	    
	     get
		{
		   return groupBy;
		 }
		 set
		 {
		   if(groupBy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupBy",OldValue=groupBy,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupBy=value;
		   }
			
		 }
	   }
	  private string dashboardId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private string startPotistion ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string StartPotistion  
	   {
	    
	     get
		{
		   return startPotistion;
		 }
		 set
		 {
		   if(startPotistion != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartPotistion",OldValue=startPotistion,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   startPotistion=value;
		   }
			
		 }
	   }
	  private string endPosition ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string EndPosition  
	   {
	    
	     get
		{
		   return endPosition;
		 }
		 set
		 {
		   if(endPosition != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndPosition",OldValue=endPosition,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   endPosition=value;
		   }
			
		 }
	   }
	  private string typeCode ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeCode  
	   {
	    
	     get
		{
		   return typeCode;
		 }
		 set
		 {
		   if(typeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeCode",OldValue=typeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeCode=value;
		   }
			
		 }
	   }
   }
   
}
	 