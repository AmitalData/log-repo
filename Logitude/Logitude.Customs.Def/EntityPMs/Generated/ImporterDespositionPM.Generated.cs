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
   public partial class ImporterDespositionPM : EntityPM
   {
   	  private string depositionNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepositionNumber  
	   {
	    
	     get
		{
		   return depositionNumber;
		 }
		 set
		 {
		   if(depositionNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepositionNumber",OldValue=depositionNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   depositionNumber=value;
		   }
			
		 }
	   }
	  private string importerDepositionStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterDepositionStatusCode  
	   {
	    
	     get
		{
		   return importerDepositionStatusCode;
		 }
		 set
		 {
		   if(importerDepositionStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterDepositionStatusCode",OldValue=importerDepositionStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerDepositionStatusCode=value;
		   }
			
		 }
	   }
	  private string importerlId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterlId  
	   {
	    
	     get
		{
		   return importerlId;
		 }
		 set
		 {
		   if(importerlId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterlId",OldValue=importerlId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerlId=value;
		   }
			
		 }
	   }
	  private string vendorID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorID  
	   {
	    
	     get
		{
		   return vendorID;
		 }
		 set
		 {
		   if(vendorID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorID",OldValue=vendorID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorID=value;
		   }
			
		 }
	   }
	  private DateTime? startDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartDate  
	   {
	    
	     get
		{
		   return startDate;
		 }
		 set
		 {
		   if(startDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDate",OldValue=startDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startDate=value;
		   }
			
		 }
	   }
	  private DateTime? endDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndDate  
	   {
	    
	     get
		{
		   return endDate;
		 }
		 set
		 {
		   if(endDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndDate",OldValue=endDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endDate=value;
		   }
			
		 }
	   }
	  private string notesToAgent ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotesToAgent  
	   {
	    
	     get
		{
		   return notesToAgent;
		 }
		 set
		 {
		   if(notesToAgent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotesToAgent",OldValue=notesToAgent,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notesToAgent=value;
		   }
			
		 }
	   }
	  private string errorMessage ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ErrorMessage  
	   {
	    
	     get
		{
		   return errorMessage;
		 }
		 set
		 {
		   if(errorMessage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ErrorMessage",OldValue=errorMessage,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   errorMessage=value;
		   }
			
		 }
	   }
	  private string importerDepositionStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterDepositionStatusName  
	   {
	    
	     get
		{
		   return importerDepositionStatusName;
		 }
		 set
		 {
		   if(importerDepositionStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterDepositionStatusName",OldValue=importerDepositionStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerDepositionStatusName=value;
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
   }
   
}
	 