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
   public partial class CB_RuleClassificationPM : EntityPM
   {
   	  private string cB_ID ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CB_ID  
	   {
	    
	     get
		{
		   return cB_ID;
		 }
		 set
		 {
		   if(cB_ID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CB_ID",OldValue=cB_ID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cB_ID=value;
		   }
			
		 }
	   }
	  private int customsItemID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CustomsItemID  
	   {
	    
	     get
		{
		   return customsItemID;
		 }
		 set
		 {
		   if(customsItemID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsItemID",OldValue=customsItemID,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   customsItemID=value;
		   }
			
		 }
	   }
	  private int iD ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private string customsBookType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBookType  
	   {
	    
	     get
		{
		   return customsBookType;
		 }
		 set
		 {
		   if(customsBookType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBookType",OldValue=customsBookType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBookType=value;
		   }
			
		 }
	   }
	  private string rules ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Rules  
	   {
	    
	     get
		{
		   return rules;
		 }
		 set
		 {
		   if(rules != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Rules",OldValue=rules,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   rules=value;
		   }
			
		 }
	   }
	  private int? parentID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ParentID  
	   {
	    
	     get
		{
		   return parentID;
		 }
		 set
		 {
		   if(parentID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentID",OldValue=parentID,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   parentID=value;
		   }
			
		 }
	   }
	  private string index ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Index  
	   {
	    
	     get
		{
		   return index;
		 }
		 set
		 {
		   if(index != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Index",OldValue=index,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   index=value;
		   }
			
		 }
	   }
	    }
   
}
	 