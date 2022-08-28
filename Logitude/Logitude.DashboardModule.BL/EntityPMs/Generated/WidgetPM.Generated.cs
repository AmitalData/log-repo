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
   public partial class WidgetPM : EntityPM
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
	  private string title ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  private string groupById ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupById  
	   {
	    
	     get
		{
		   return groupById;
		 }
		 set
		 {
		   if(groupById != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupById",OldValue=groupById,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupById=value;
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
	  private string startPotistion ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  private string entityId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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

	   private List<WidgetMeasurePM> widgetMeasures;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("WidgetWidgetMeasure", "Id","WidgetId")]
	   [DataMember]
	   public virtual List<WidgetMeasurePM> WidgetMeasures  
	   {
	        get
             {
                 if (widgetMeasures == null)
                 {
                     widgetMeasures = new List<WidgetMeasurePM>();
                 }
                 return widgetMeasures;
              }
             set { widgetMeasures = value; }
	    }
		   
	   private List<WidgetMeasurePM>  deletedWidgetMeasures;
	   public virtual List<WidgetMeasurePM> DeletedWidgetMeasures  
	   {
	        get
             {
                 if ( deletedWidgetMeasures == null)
                 {
                      deletedWidgetMeasures = new List<WidgetMeasurePM>();
                 }
                 return  deletedWidgetMeasures;
              }
             set {  deletedWidgetMeasures = value; }
	    }
	     }
   
}
	 