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
   public partial class CustomsDocumentsDefinitionPM : EntityPM
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
	  private string documentTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DocumentTypeCode  
	   {
	    
	     get
		{
		   return documentTypeCode;
		 }
		 set
		 {
		   if(documentTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentTypeCode",OldValue=documentTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   documentTypeCode=value;
		   }
			
		 }
	   }
	  private string documentTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DocumentTypeName  
	   {
	    
	     get
		{
		   return documentTypeName;
		 }
		 set
		 {
		   if(documentTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentTypeName",OldValue=documentTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   documentTypeName=value;
		   }
			
		 }
	   }
	  private string transportationTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportationTypeCode  
	   {
	    
	     get
		{
		   return transportationTypeCode;
		 }
		 set
		 {
		   if(transportationTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportationTypeCode",OldValue=transportationTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportationTypeCode=value;
		   }
			
		 }
	   }
	  private string transportationTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportationTypeName  
	   {
	    
	     get
		{
		   return transportationTypeName;
		 }
		 set
		 {
		   if(transportationTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportationTypeName",OldValue=transportationTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportationTypeName=value;
		   }
			
		 }
	   }
	  private string processTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProcessTypeCode  
	   {
	    
	     get
		{
		   return processTypeCode;
		 }
		 set
		 {
		   if(processTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProcessTypeCode",OldValue=processTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   processTypeCode=value;
		   }
			
		 }
	   }
	  private string processTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProcessTypeName  
	   {
	    
	     get
		{
		   return processTypeName;
		 }
		 set
		 {
		   if(processTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProcessTypeName",OldValue=processTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   processTypeName=value;
		   }
			
		 }
	   }
	  private string cargoTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoTypeCode  
	   {
	    
	     get
		{
		   return cargoTypeCode;
		 }
		 set
		 {
		   if(cargoTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoTypeCode",OldValue=cargoTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoTypeCode=value;
		   }
			
		 }
	   }
	  private string cargoTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoTypeName  
	   {
	    
	     get
		{
		   return cargoTypeName;
		 }
		 set
		 {
		   if(cargoTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoTypeName",OldValue=cargoTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoTypeName=value;
		   }
			
		 }
	   }
	  private bool mandatory ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Mandatory  
	   {
	    
	     get
		{
		   return mandatory;
		 }
		 set
		 {
		   if(mandatory != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Mandatory",OldValue=mandatory,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   mandatory=value;
		   }
			
		 }
	   }
	  private bool inactive ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Inactive  
	   {
	    
	     get
		{
		   return inactive;
		 }
		 set
		 {
		   if(inactive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Inactive",OldValue=inactive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   inactive=value;
		   }
			
		 }
	   }
   }
   
}
	 