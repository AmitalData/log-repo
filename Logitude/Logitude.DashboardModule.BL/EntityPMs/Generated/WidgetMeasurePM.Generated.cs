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
   public partial class WidgetMeasurePM : EntityPM
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
	  private string widgetId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string WidgetId  
	   {
	    
	     get
		{
		   return widgetId;
		 }
		 set
		 {
		   if(widgetId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WidgetId",OldValue=widgetId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   widgetId=value;
		   }
			
		 }
	   }
	  private string measureCode ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string MeasureCode  
	   {
	    
	     get
		{
		   return measureCode;
		 }
		 set
		 {
		   if(measureCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeasureCode",OldValue=measureCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   measureCode=value;
		   }
			
		 }
	   }
	  private string measureFieldId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string MeasureFieldId  
	   {
	    
	     get
		{
		   return measureFieldId;
		 }
		 set
		 {
		   if(measureFieldId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeasureFieldId",OldValue=measureFieldId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   measureFieldId=value;
		   }
			
		 }
	   }
	  private string renderAs ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string RenderAs  
	   {
	    
	     get
		{
		   return renderAs;
		 }
		 set
		 {
		   if(renderAs != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RenderAs",OldValue=renderAs,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   renderAs=value;
		   }
			
		 }
	   }
	  private string yAxisType ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string YAxisType  
	   {
	    
	     get
		{
		   return yAxisType;
		 }
		 set
		 {
		   if(yAxisType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="YAxisType",OldValue=yAxisType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   yAxisType=value;
		   }
			
		 }
	   }
	    }
   
}
	 