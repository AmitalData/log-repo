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
   public partial class AvailableStatusFieldPM : EntityPM
   {
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
	  private string fieldCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldCode  
	   {
	    
	     get
		{
		   return fieldCode;
		 }
		 set
		 {
		   if(fieldCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldCode",OldValue=fieldCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldCode=value;
		   }
			
		 }
	   }
	  private bool isAvailable ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAvailable  
	   {
	    
	     get
		{
		   return isAvailable;
		 }
		 set
		 {
		   if(isAvailable != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAvailable",OldValue=isAvailable,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAvailable=value;
		   }
			
		 }
	   }
	  private string statusFieldType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusFieldType  
	   {
	    
	     get
		{
		   return statusFieldType;
		 }
		 set
		 {
		   if(statusFieldType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusFieldType",OldValue=statusFieldType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusFieldType=value;
		   }
			
		 }
	   }
   }
   
}
	 