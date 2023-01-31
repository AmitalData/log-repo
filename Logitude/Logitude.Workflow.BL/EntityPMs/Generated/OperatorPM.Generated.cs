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
using Logitude.Workflow.BL.Validators;
  
namespace Logitude.Workflow.BL.EntityPMs
{
   [CustomValidation(typeof(WorkflowClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class OperatorPM : EntityPM
   {
   	  private string code ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string Code  
	   {
	    
	     get
		{
		   return code;
		 }
		 set
		 {
		   if(code != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Code",OldValue=code,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   code=value;
		   }
			
		 }
	   }
	  private string name ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string Name  
	   {
	    
	     get
		{
		   return name;
		 }
		 set
		 {
		   if(name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Name",OldValue=name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   name=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string SearchFields  
	   {
	    
	     get
		{
		   return searchFields;
		 }
		 set
		 {
		   if(searchFields != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SearchFields",OldValue=searchFields,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   searchFields=value;
		   }
			
		 }
	   }
	  private string sign ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string Sign  
	   {
	    
	     get
		{
		   return sign;
		 }
		 set
		 {
		   if(sign != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Sign",OldValue=sign,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sign=value;
		   }
			
		 }
	   }
	  private string categoryCode ;
	  	  
       
	   [CustomValidation(typeof(WorkflowValidationClass), "ValidateClass")]
	   [DataMember]
       public string CategoryCode  
	   {
	    
	     get
		{
		   return categoryCode;
		 }
		 set
		 {
		   if(categoryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CategoryCode",OldValue=categoryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   categoryCode=value;
		   }
			
		 }
	   }
	    }
   
}
	 