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
   public partial class CB_RegularityInceptionPM : EntityPM
   {
   	  private string iD ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private string regularityRequirementID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RegularityRequirementID  
	   {
	    
	     get
		{
		   return regularityRequirementID;
		 }
		 set
		 {
		   if(regularityRequirementID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RegularityRequirementID",OldValue=regularityRequirementID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   regularityRequirementID=value;
		   }
			
		 }
	   }
	  private string interConditionsRelationshipID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterConditionsRelationshipID  
	   {
	    
	     get
		{
		   return interConditionsRelationshipID;
		 }
		 set
		 {
		   if(interConditionsRelationshipID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterConditionsRelationshipID",OldValue=interConditionsRelationshipID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interConditionsRelationshipID=value;
		   }
			
		 }
	   }
	  private bool isPersonalImportIncluded ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPersonalImportIncluded  
	   {
	    
	     get
		{
		   return isPersonalImportIncluded;
		 }
		 set
		 {
		   if(isPersonalImportIncluded != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPersonalImportIncluded",OldValue=isPersonalImportIncluded,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPersonalImportIncluded=value;
		   }
			
		 }
	   }
	  private string requirementGoodsDescription ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequirementGoodsDescription  
	   {
	    
	     get
		{
		   return requirementGoodsDescription;
		 }
		 set
		 {
		   if(requirementGoodsDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequirementGoodsDescription",OldValue=requirementGoodsDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requirementGoodsDescription=value;
		   }
			
		 }
	   }
	  private string regularityRequirementWarnID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RegularityRequirementWarnID  
	   {
	    
	     get
		{
		   return regularityRequirementWarnID;
		 }
		 set
		 {
		   if(regularityRequirementWarnID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RegularityRequirementWarnID",OldValue=regularityRequirementWarnID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   regularityRequirementWarnID=value;
		   }
			
		 }
	   }
	  private bool isCarnetIncluded ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCarnetIncluded  
	   {
	    
	     get
		{
		   return isCarnetIncluded;
		 }
		 set
		 {
		   if(isCarnetIncluded != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCarnetIncluded",OldValue=isCarnetIncluded,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCarnetIncluded=value;
		   }
			
		 }
	   }
   }
   
}
	 