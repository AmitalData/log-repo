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
   public partial class RemarksClassificationPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private int customsItemsID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CustomsItemsID  
	   {
	    
	     get
		{
		   return customsItemsID;
		 }
		 set
		 {
		   if(customsItemsID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsItemsID",OldValue=customsItemsID,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   customsItemsID=value;
		   }
			
		 }
	   }
	  private string remarkDescription ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RemarkDescription  
	   {
	    
	     get
		{
		   return remarkDescription;
		 }
		 set
		 {
		   if(remarkDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RemarkDescription",OldValue=remarkDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   remarkDescription=value;
		   }
			
		 }
	   }
   }
   
}
	 