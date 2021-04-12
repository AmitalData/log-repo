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
   public partial class ExportReferencePM : EntityPM
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
	  private int storageNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int StorageNo  
	   {
	    
	     get
		{
		   return storageNo;
		 }
		 set
		 {
		   if(storageNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageNo",OldValue=storageNo,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   storageNo=value;
		   }
			
		 }
	   }
	  private string refType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RefType  
	   {
	    
	     get
		{
		   return refType;
		 }
		 set
		 {
		   if(refType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RefType",OldValue=refType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   refType=value;
		   }
			
		 }
	   }
	  private string refValue ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RefValue  
	   {
	    
	     get
		{
		   return refValue;
		 }
		 set
		 {
		   if(refValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RefValue",OldValue=refValue,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   refValue=value;
		   }
			
		 }
	   }
   }
   
}
	 