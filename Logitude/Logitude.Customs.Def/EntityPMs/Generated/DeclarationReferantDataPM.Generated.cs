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
   public partial class DeclarationReferantDataPM : EntityPM
   {
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
	  private string orderNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OrderNumber  
	   {
	    
	     get
		{
		   return orderNumber;
		 }
		 set
		 {
		   if(orderNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OrderNumber",OldValue=orderNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   orderNumber=value;
		   }
			
		 }
	   }
	  private string vendorId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorId  
	   {
	    
	     get
		{
		   return vendorId;
		 }
		 set
		 {
		   if(vendorId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorId",OldValue=vendorId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorId=value;
		   }
			
		 }
	   }
	  private DateTime arrivalDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ArrivalDate  
	   {
	    
	     get
		{
		   return arrivalDate;
		 }
		 set
		 {
		   if(arrivalDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ArrivalDate",OldValue=arrivalDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   arrivalDate=value;
		   }
			
		 }
	   }
	  private DateTime estimatedArrivalDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime EstimatedArrivalDate  
	   {
	    
	     get
		{
		   return estimatedArrivalDate;
		 }
		 set
		 {
		   if(estimatedArrivalDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedArrivalDate",OldValue=estimatedArrivalDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   estimatedArrivalDate=value;
		   }
			
		 }
	   }
	  private decimal? weight ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Weight  
	   {
	    
	     get
		{
		   return weight;
		 }
		 set
		 {
		   if(weight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Weight",OldValue=weight,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   weight=value;
		   }
			
		 }
	   }
	  private string classificationStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClassificationStatus  
	   {
	    
	     get
		{
		   return classificationStatus;
		 }
		 set
		 {
		   if(classificationStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClassificationStatus",OldValue=classificationStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   classificationStatus=value;
		   }
			
		 }
	   }
	  private string controllerStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ControllerStatus  
	   {
	    
	     get
		{
		   return controllerStatus;
		 }
		 set
		 {
		   if(controllerStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ControllerStatus",OldValue=controllerStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   controllerStatus=value;
		   }
			
		 }
	   }
	  private string collectionOfMoneyStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CollectionOfMoneyStatus  
	   {
	    
	     get
		{
		   return collectionOfMoneyStatus;
		 }
		 set
		 {
		   if(collectionOfMoneyStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CollectionOfMoneyStatus",OldValue=collectionOfMoneyStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   collectionOfMoneyStatus=value;
		   }
			
		 }
	   }
	  private DateTime? followUpDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FollowUpDate  
	   {
	    
	     get
		{
		   return followUpDate;
		 }
		 set
		 {
		   if(followUpDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowUpDate",OldValue=followUpDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   followUpDate=value;
		   }
			
		 }
	   }
	  private bool isExceptional ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsExceptional  
	   {
	    
	     get
		{
		   return isExceptional;
		 }
		 set
		 {
		   if(isExceptional != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsExceptional",OldValue=isExceptional,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isExceptional=value;
		   }
			
		 }
	   }
	  private bool withPaper ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool WithPaper  
	   {
	    
	     get
		{
		   return withPaper;
		 }
		 set
		 {
		   if(withPaper != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WithPaper",OldValue=withPaper,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   withPaper=value;
		   }
			
		 }
	   }
	  private string isClosedForFollowUp ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsClosedForFollowUp  
	   {
	    
	     get
		{
		   return isClosedForFollowUp;
		 }
		 set
		 {
		   if(isClosedForFollowUp != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosedForFollowUp",OldValue=isClosedForFollowUp,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isClosedForFollowUp=value;
		   }
			
		 }
	   }
	  private bool isClassificationRemarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClassificationRemarks  
	   {
	    
	     get
		{
		   return isClassificationRemarks;
		 }
		 set
		 {
		   if(isClassificationRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClassificationRemarks",OldValue=isClassificationRemarks,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClassificationRemarks=value;
		   }
			
		 }
	   }
	  private bool isControllerRemarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsControllerRemarks  
	   {
	    
	     get
		{
		   return isControllerRemarks;
		 }
		 set
		 {
		   if(isControllerRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsControllerRemarks",OldValue=isControllerRemarks,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isControllerRemarks=value;
		   }
			
		 }
	   }
	  private string preClassification ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreClassification  
	   {
	    
	     get
		{
		   return preClassification;
		 }
		 set
		 {
		   if(preClassification != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreClassification",OldValue=preClassification,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preClassification=value;
		   }
			
		 }
	   }
   }
   
}
	 