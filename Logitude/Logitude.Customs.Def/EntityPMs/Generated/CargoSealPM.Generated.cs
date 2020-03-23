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
   public partial class CargoSealPM : EntityPM
   {
   	  private string cargoSealIdentifierId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoSealIdentifierId  
	   {
	    
	     get
		{
		   return cargoSealIdentifierId;
		 }
		 set
		 {
		   if(cargoSealIdentifierId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoSealIdentifierId",OldValue=cargoSealIdentifierId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoSealIdentifierId=value;
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
	  private string sealNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SealNumber  
	   {
	    
	     get
		{
		   return sealNumber;
		 }
		 set
		 {
		   if(sealNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SealNumber",OldValue=sealNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sealNumber=value;
		   }
			
		 }
	   }
	  private string remarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Remarks  
	   {
	    
	     get
		{
		   return remarks;
		 }
		 set
		 {
		   if(remarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Remarks",OldValue=remarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   remarks=value;
		   }
			
		 }
	   }
	  private string sealCompletenessStateCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SealCompletenessStateCode  
	   {
	    
	     get
		{
		   return sealCompletenessStateCode;
		 }
		 set
		 {
		   if(sealCompletenessStateCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SealCompletenessStateCode",OldValue=sealCompletenessStateCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sealCompletenessStateCode=value;
		   }
			
		 }
	   }
	  private string sealCompletenessStateName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SealCompletenessStateName  
	   {
	    
	     get
		{
		   return sealCompletenessStateName;
		 }
		 set
		 {
		   if(sealCompletenessStateName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SealCompletenessStateName",OldValue=sealCompletenessStateName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sealCompletenessStateName=value;
		   }
			
		 }
	   }
	  private string sealTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SealTypeCode  
	   {
	    
	     get
		{
		   return sealTypeCode;
		 }
		 set
		 {
		   if(sealTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SealTypeCode",OldValue=sealTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sealTypeCode=value;
		   }
			
		 }
	   }
	  private string sealTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SealTypeName  
	   {
	    
	     get
		{
		   return sealTypeName;
		 }
		 set
		 {
		   if(sealTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SealTypeName",OldValue=sealTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sealTypeName=value;
		   }
			
		 }
	   }
	  private string updateReasonCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdateReasonCode  
	   {
	    
	     get
		{
		   return updateReasonCode;
		 }
		 set
		 {
		   if(updateReasonCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateReasonCode",OldValue=updateReasonCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updateReasonCode=value;
		   }
			
		 }
	   }
	  private string updateReasonName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdateReasonName  
	   {
	    
	     get
		{
		   return updateReasonName;
		 }
		 set
		 {
		   if(updateReasonName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateReasonName",OldValue=updateReasonName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updateReasonName=value;
		   }
			
		 }
	   }
	  private string updateTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdateTypeCode  
	   {
	    
	     get
		{
		   return updateTypeCode;
		 }
		 set
		 {
		   if(updateTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateTypeCode",OldValue=updateTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updateTypeCode=value;
		   }
			
		 }
	   }
	  private string updateTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdateTypeName  
	   {
	    
	     get
		{
		   return updateTypeName;
		 }
		 set
		 {
		   if(updateTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateTypeName",OldValue=updateTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updateTypeName=value;
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
	  private bool canToAdd ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CanToAdd  
	   {
	    
	     get
		{
		   return canToAdd;
		 }
		 set
		 {
		   if(canToAdd != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CanToAdd",OldValue=canToAdd,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   canToAdd=value;
		   }
			
		 }
	   }
   }
   
}
	 