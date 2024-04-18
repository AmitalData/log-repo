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
   public partial class ClientItemPM : EntityPM
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
	  private string itemDescription ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ItemDescription  
	   {
	    
	     get
		{
		   return itemDescription;
		 }
		 set
		 {
		   if(itemDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemDescription",OldValue=itemDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   itemDescription=value;
		   }
			
		 }
	   }
	  private string classificationCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClassificationCode  
	   {
	    
	     get
		{
		   return classificationCode;
		 }
		 set
		 {
		   if(classificationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClassificationCode",OldValue=classificationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   classificationCode=value;
		   }
			
		 }
	   }
	  private string itemCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ItemCode  
	   {
	    
	     get
		{
		   return itemCode;
		 }
		 set
		 {
		   if(itemCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemCode",OldValue=itemCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   itemCode=value;
		   }
			
		 }
	   }
	  private string originCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginCountryCode  
	   {
	    
	     get
		{
		   return originCountryCode;
		 }
		 set
		 {
		   if(originCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginCountryCode",OldValue=originCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originCountryCode=value;
		   }
			
		 }
	   }
	  private string originCountryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginCountryName  
	   {
	    
	     get
		{
		   return originCountryName;
		 }
		 set
		 {
		   if(originCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginCountryName",OldValue=originCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originCountryName=value;
		   }
			
		 }
	   }
	  private string clientCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClientCode  
	   {
	    
	     get
		{
		   return clientCode;
		 }
		 set
		 {
		   if(clientCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClientCode",OldValue=clientCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   clientCode=value;
		   }
			
		 }
	   }
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
	  private string itemKey ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ItemKey  
	   {
	    
	     get
		{
		   return itemKey;
		 }
		 set
		 {
		   if(itemKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemKey",OldValue=itemKey,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   itemKey=value;
		   }
			
		 }
	   }
	    }
   
}
	 