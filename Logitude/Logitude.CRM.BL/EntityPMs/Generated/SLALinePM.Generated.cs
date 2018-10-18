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
using Logitude.CRM.BL.Validators;
  
namespace Logitude.CRM.BL.EntityPMs
{
   [CustomValidation(typeof(CRMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class SLALinePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string sLAHeaderId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SLAHeaderId  
	   {
	    
	     get
		{
		   return sLAHeaderId;
		 }
		 set
		 {
		   if(sLAHeaderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SLAHeaderId",OldValue=sLAHeaderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sLAHeaderId=value;
		   }
			
		 }
	   }
	  private string severityId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SeverityId  
	   {
	    
	     get
		{
		   return severityId;
		 }
		 set
		 {
		   if(severityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SeverityId",OldValue=severityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   severityId=value;
		   }
			
		 }
	   }
	  private string businessHoursId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BusinessHoursId  
	   {
	    
	     get
		{
		   return businessHoursId;
		 }
		 set
		 {
		   if(businessHoursId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BusinessHoursId",OldValue=businessHoursId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   businessHoursId=value;
		   }
			
		 }
	   }
	  private int? firstResponseTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? FirstResponseTime  
	   {
	    
	     get
		{
		   return firstResponseTime;
		 }
		 set
		 {
		   if(firstResponseTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstResponseTime",OldValue=firstResponseTime,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   firstResponseTime=value;
		   }
			
		 }
	   }
	  private string firstResponseTimeUnit ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FirstResponseTimeUnit  
	   {
	    
	     get
		{
		   return firstResponseTimeUnit;
		 }
		 set
		 {
		   if(firstResponseTimeUnit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstResponseTimeUnit",OldValue=firstResponseTimeUnit,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   firstResponseTimeUnit=value;
		   }
			
		 }
	   }
	  private int? firstResponseTimeInMinute ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? FirstResponseTimeInMinute  
	   {
	    
	     get
		{
		   return firstResponseTimeInMinute;
		 }
		 set
		 {
		   if(firstResponseTimeInMinute != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstResponseTimeInMinute",OldValue=firstResponseTimeInMinute,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   firstResponseTimeInMinute=value;
		   }
			
		 }
	   }
	  private int? resolveWithinTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ResolveWithinTime  
	   {
	    
	     get
		{
		   return resolveWithinTime;
		 }
		 set
		 {
		   if(resolveWithinTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResolveWithinTime",OldValue=resolveWithinTime,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   resolveWithinTime=value;
		   }
			
		 }
	   }
	  private string resolveWithinTimeUnit ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ResolveWithinTimeUnit  
	   {
	    
	     get
		{
		   return resolveWithinTimeUnit;
		 }
		 set
		 {
		   if(resolveWithinTimeUnit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResolveWithinTimeUnit",OldValue=resolveWithinTimeUnit,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   resolveWithinTimeUnit=value;
		   }
			
		 }
	   }
	  private int? resolveWithinTimeInMinute ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ResolveWithinTimeInMinute  
	   {
	    
	     get
		{
		   return resolveWithinTimeInMinute;
		 }
		 set
		 {
		   if(resolveWithinTimeInMinute != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResolveWithinTimeInMinute",OldValue=resolveWithinTimeInMinute,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   resolveWithinTimeInMinute=value;
		   }
			
		 }
	   }
	  private bool firstResponseEscalate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FirstResponseEscalate  
	   {
	    
	     get
		{
		   return firstResponseEscalate;
		 }
		 set
		 {
		   if(firstResponseEscalate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstResponseEscalate",OldValue=firstResponseEscalate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   firstResponseEscalate=value;
		   }
			
		 }
	   }
	  private bool resolveWithinEscalate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ResolveWithinEscalate  
	   {
	    
	     get
		{
		   return resolveWithinEscalate;
		 }
		 set
		 {
		   if(resolveWithinEscalate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResolveWithinEscalate",OldValue=resolveWithinEscalate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   resolveWithinEscalate=value;
		   }
			
		 }
	   }
	  private string severityName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SeverityName  
	   {
	    
	     get
		{
		   return severityName;
		 }
		 set
		 {
		   if(severityName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SeverityName",OldValue=severityName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   severityName=value;
		   }
			
		 }
	   }
   }
   
}
	 