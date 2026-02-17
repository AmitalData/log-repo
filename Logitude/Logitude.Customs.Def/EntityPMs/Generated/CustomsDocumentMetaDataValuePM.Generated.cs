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
   public partial class CustomsDocumentMetaDataValuePM : EntityPM
   {
   	  private string customsDocumentId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsDocumentId  
	   {
	    
	     get
		{
		   return customsDocumentId;
		 }
		 set
		 {
		   if(customsDocumentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsDocumentId",OldValue=customsDocumentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsDocumentId=value;
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
	  private string metaDataTypeCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MetaDataTypeCode  
	   {
	    
	     get
		{
		   return metaDataTypeCode;
		 }
		 set
		 {
		   if(metaDataTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MetaDataTypeCode",OldValue=metaDataTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   metaDataTypeCode=value;
		   }
			
		 }
	   }
	  private string metaDataValue ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MetaDataValue  
	   {
	    
	     get
		{
		   return metaDataValue;
		 }
		 set
		 {
		   if(metaDataValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MetaDataValue",OldValue=metaDataValue,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   metaDataValue=value;
		   }
			
		 }
	   }
	  private string metaDataTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MetaDataTypeName  
	   {
	    
	     get
		{
		   return metaDataTypeName;
		 }
		 set
		 {
		   if(metaDataTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MetaDataTypeName",OldValue=metaDataTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   metaDataTypeName=value;
		   }
			
		 }
	   }
   }
   
}
	 