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
   public partial class PendingByKeywordPM : EntityPM
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
	  private string courierPendingReasonCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierPendingReasonCode  
	   {
	    
	     get
		{
		   return courierPendingReasonCode;
		 }
		 set
		 {
		   if(courierPendingReasonCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierPendingReasonCode",OldValue=courierPendingReasonCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierPendingReasonCode=value;
		   }
			
		 }
	   }
	  private string courierPendingReasonName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierPendingReasonName  
	   {
	    
	     get
		{
		   return courierPendingReasonName;
		 }
		 set
		 {
		   if(courierPendingReasonName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierPendingReasonName",OldValue=courierPendingReasonName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierPendingReasonName=value;
		   }
			
		 }
	   }
	  private string keywordsList ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string KeywordsList  
	   {
	    
	     get
		{
		   return keywordsList;
		 }
		 set
		 {
		   if(keywordsList != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="KeywordsList",OldValue=keywordsList,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   keywordsList=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string searchByFieldCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SearchByFieldCode  
	   {
	    
	     get
		{
		   return searchByFieldCode;
		 }
		 set
		 {
		   if(searchByFieldCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SearchByFieldCode",OldValue=searchByFieldCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   searchByFieldCode=value;
		   }
			
		 }
	   }
	  private string searchByFieldName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SearchByFieldName  
	   {
	    
	     get
		{
		   return searchByFieldName;
		 }
		 set
		 {
		   if(searchByFieldName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SearchByFieldName",OldValue=searchByFieldName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   searchByFieldName=value;
		   }
			
		 }
	   }
   }
   
}
	 