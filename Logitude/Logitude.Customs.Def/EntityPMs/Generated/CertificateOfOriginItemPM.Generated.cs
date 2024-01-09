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
   public partial class CertificateOfOriginItemPM : EntityPM
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
	  private string certificateOfOriginId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CertificateOfOriginId  
	   {
	    
	     get
		{
		   return certificateOfOriginId;
		 }
		 set
		 {
		   if(certificateOfOriginId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CertificateOfOriginId",OldValue=certificateOfOriginId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   certificateOfOriginId=value;
		   }
			
		 }
	   }
	  private string itemSerial ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ItemSerial  
	   {
	    
	     get
		{
		   return itemSerial;
		 }
		 set
		 {
		   if(itemSerial != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemSerial",OldValue=itemSerial,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   itemSerial=value;
		   }
			
		 }
	   }
	  private string itemId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ItemId  
	   {
	    
	     get
		{
		   return itemId;
		 }
		 set
		 {
		   if(itemId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemId",OldValue=itemId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   itemId=value;
		   }
			
		 }
	   }
	  private string originCriterionCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginCriterionCode  
	   {
	    
	     get
		{
		   return originCriterionCode;
		 }
		 set
		 {
		   if(originCriterionCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginCriterionCode",OldValue=originCriterionCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originCriterionCode=value;
		   }
			
		 }
	   }
	  private string marksAndNumbers ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MarksAndNumbers  
	   {
	    
	     get
		{
		   return marksAndNumbers;
		 }
		 set
		 {
		   if(marksAndNumbers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarksAndNumbers",OldValue=marksAndNumbers,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   marksAndNumbers=value;
		   }
			
		 }
	   }
	  private string packageQuantity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageQuantity  
	   {
	    
	     get
		{
		   return packageQuantity;
		 }
		 set
		 {
		   if(packageQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageQuantity",OldValue=packageQuantity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageQuantity=value;
		   }
			
		 }
	   }
	  private string packageType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageType  
	   {
	    
	     get
		{
		   return packageType;
		 }
		 set
		 {
		   if(packageType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType",OldValue=packageType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageType=value;
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
	  private string weight ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Weight  
	   {
	    
	     get
		{
		   return weight;
		 }
		 set
		 {
		   if(weight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Weight",OldValue=weight,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   weight=value;
		   }
			
		 }
	   }
	  private string measureType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MeasureType  
	   {
	    
	     get
		{
		   return measureType;
		 }
		 set
		 {
		   if(measureType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeasureType",OldValue=measureType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   measureType=value;
		   }
			
		 }
	   }
	  private string invoiceConnect ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InvoiceConnect  
	   {
	    
	     get
		{
		   return invoiceConnect;
		 }
		 set
		 {
		   if(invoiceConnect != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceConnect",OldValue=invoiceConnect,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   invoiceConnect=value;
		   }
			
		 }
	   }
	  private string containerIsoCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerIsoCode  
	   {
	    
	     get
		{
		   return containerIsoCode;
		 }
		 set
		 {
		   if(containerIsoCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerIsoCode",OldValue=containerIsoCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerIsoCode=value;
		   }
			
		 }
	   }
   }
   
}
	 