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
using Logitude.TariffModule.BL.Validators;
  
namespace Logitude.TariffModule.BL.EntityPMs
{
   [CustomValidation(typeof(TariffModuleClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TariffTypePM : EntityPM
   {
   	  private string code ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private string transportModeCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeCode  
	   {
	    
	     get
		{
		   return transportModeCode;
		 }
		 set
		 {
		   if(transportModeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeCode",OldValue=transportModeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeCode=value;
		   }
			
		 }
	   }
	  private string directionCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DirectionCode  
	   {
	    
	     get
		{
		   return directionCode;
		 }
		 set
		 {
		   if(directionCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DirectionCode",OldValue=directionCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   directionCode=value;
		   }
			
		 }
	   }
   }
   
}
	 