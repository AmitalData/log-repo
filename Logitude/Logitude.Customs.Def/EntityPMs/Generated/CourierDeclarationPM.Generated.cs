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
   public partial class CourierDeclarationPM : EntityPM
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
	  private string declarationId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationId  
	   {
	    
	     get
		{
		   return declarationId;
		 }
		 set
		 {
		   if(declarationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationId",OldValue=declarationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationId=value;
		   }
			
		 }
	   }
	  private string courierMasterId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierMasterId  
	   {
	    
	     get
		{
		   return courierMasterId;
		 }
		 set
		 {
		   if(courierMasterId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierMasterId",OldValue=courierMasterId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierMasterId=value;
		   }
			
		 }
	   }
	  private int? sequenceNumeric ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? SequenceNumeric  
	   {
	    
	     get
		{
		   return sequenceNumeric;
		 }
		 set
		 {
		   if(sequenceNumeric != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SequenceNumeric",OldValue=sequenceNumeric,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   sequenceNumeric=value;
		   }
			
		 }
	   }
	  private string declarationNumberandVersionId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationNumberandVersionId  
	   {
	    
	     get
		{
		   return declarationNumberandVersionId;
		 }
		 set
		 {
		   if(declarationNumberandVersionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationNumberandVersionId",OldValue=declarationNumberandVersionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationNumberandVersionId=value;
		   }
			
		 }
	   }
	  private string externalDeclarationNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalDeclarationNumber  
	   {
	    
	     get
		{
		   return externalDeclarationNumber;
		 }
		 set
		 {
		   if(externalDeclarationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalDeclarationNumber",OldValue=externalDeclarationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalDeclarationNumber=value;
		   }
			
		 }
	   }
	  private string procedureCurrentName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProcedureCurrentName  
	   {
	    
	     get
		{
		   return procedureCurrentName;
		 }
		 set
		 {
		   if(procedureCurrentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProcedureCurrentName",OldValue=procedureCurrentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   procedureCurrentName=value;
		   }
			
		 }
	   }
	  private string declarationStatusTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationStatusTypeName  
	   {
	    
	     get
		{
		   return declarationStatusTypeName;
		 }
		 set
		 {
		   if(declarationStatusTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationStatusTypeName",OldValue=declarationStatusTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationStatusTypeName=value;
		   }
			
		 }
	   }
	  private string manifestCargoStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManifestCargoStatusName  
	   {
	    
	     get
		{
		   return manifestCargoStatusName;
		 }
		 set
		 {
		   if(manifestCargoStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManifestCargoStatusName",OldValue=manifestCargoStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manifestCargoStatusName=value;
		   }
			
		 }
	   }
	  private string courierCustomStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierCustomStatusName  
	   {
	    
	     get
		{
		   return courierCustomStatusName;
		 }
		 set
		 {
		   if(courierCustomStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierCustomStatusName",OldValue=courierCustomStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierCustomStatusName=value;
		   }
			
		 }
	   }
	  private string courierData ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CourierData  
	   {
	    
	     get
		{
		   return courierData;
		 }
		 set
		 {
		   if(courierData != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CourierData",OldValue=courierData,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   courierData=value;
		   }
			
		 }
	   }
	  private DateTime? hatraDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? HatraDate  
	   {
	    
	     get
		{
		   return hatraDate;
		 }
		 set
		 {
		   if(hatraDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HatraDate",OldValue=hatraDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   hatraDate=value;
		   }
			
		 }
	   }
   }
   
}
	 