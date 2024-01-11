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
   public partial class TapagPM : EntityPM
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
	  private string tapagNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TapagNumber  
	   {
	    
	     get
		{
		   return tapagNumber;
		 }
		 set
		 {
		   if(tapagNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TapagNumber",OldValue=tapagNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tapagNumber=value;
		   }
			
		 }
	   }
	  private string leadingFileNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LeadingFileNumber  
	   {
	    
	     get
		{
		   return leadingFileNumber;
		 }
		 set
		 {
		   if(leadingFileNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeadingFileNumber",OldValue=leadingFileNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   leadingFileNumber=value;
		   }
			
		 }
	   }
	  private string tapagTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TapagTypeCode  
	   {
	    
	     get
		{
		   return tapagTypeCode;
		 }
		 set
		 {
		   if(tapagTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TapagTypeCode",OldValue=tapagTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tapagTypeCode=value;
		   }
			
		 }
	   }
	  private string tapagTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TapagTypeName  
	   {
	    
	     get
		{
		   return tapagTypeName;
		 }
		 set
		 {
		   if(tapagTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TapagTypeName",OldValue=tapagTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tapagTypeName=value;
		   }
			
		 }
	   }
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerId  
	   {
	    
	     get
		{
		   return customerId;
		 }
		 set
		 {
		   if(customerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerId",OldValue=customerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerId=value;
		   }
			
		 }
	   }
	  private string customerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerName  
	   {
	    
	     get
		{
		   return customerName;
		 }
		 set
		 {
		   if(customerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerName",OldValue=customerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerName=value;
		   }
			
		 }
	   }
	  private string importerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterId  
	   {
	    
	     get
		{
		   return importerId;
		 }
		 set
		 {
		   if(importerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterId",OldValue=importerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerId=value;
		   }
			
		 }
	   }
	  private string importerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterName  
	   {
	    
	     get
		{
		   return importerName;
		 }
		 set
		 {
		   if(importerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterName",OldValue=importerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerName=value;
		   }
			
		 }
	   }
	  private string customsBranchCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBranchCode  
	   {
	    
	     get
		{
		   return customsBranchCode;
		 }
		 set
		 {
		   if(customsBranchCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBranchCode",OldValue=customsBranchCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBranchCode=value;
		   }
			
		 }
	   }
	  private string customsBranchName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBranchName  
	   {
	    
	     get
		{
		   return customsBranchName;
		 }
		 set
		 {
		   if(customsBranchName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBranchName",OldValue=customsBranchName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBranchName=value;
		   }
			
		 }
	   }
	  private string professionUnitTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProfessionUnitTypeCode  
	   {
	    
	     get
		{
		   return professionUnitTypeCode;
		 }
		 set
		 {
		   if(professionUnitTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProfessionUnitTypeCode",OldValue=professionUnitTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   professionUnitTypeCode=value;
		   }
			
		 }
	   }
	  private string professionUnitTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProfessionUnitTypeName  
	   {
	    
	     get
		{
		   return professionUnitTypeName;
		 }
		 set
		 {
		   if(professionUnitTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProfessionUnitTypeName",OldValue=professionUnitTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   professionUnitTypeName=value;
		   }
			
		 }
	   }
	  private string specializationTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecializationTypeCode  
	   {
	    
	     get
		{
		   return specializationTypeCode;
		 }
		 set
		 {
		   if(specializationTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecializationTypeCode",OldValue=specializationTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specializationTypeCode=value;
		   }
			
		 }
	   }
	  private string specializationTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecializationTypeName  
	   {
	    
	     get
		{
		   return specializationTypeName;
		 }
		 set
		 {
		   if(specializationTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecializationTypeName",OldValue=specializationTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specializationTypeName=value;
		   }
			
		 }
	   }
	  private DateTime? createDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private DateTime? followDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FollowDate  
	   {
	    
	     get
		{
		   return followDate;
		 }
		 set
		 {
		   if(followDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FollowDate",OldValue=followDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   followDate=value;
		   }
			
		 }
	   }
	  private DateTime? validityDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ValidityDate  
	   {
	    
	     get
		{
		   return validityDate;
		 }
		 set
		 {
		   if(validityDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ValidityDate",OldValue=validityDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   validityDate=value;
		   }
			
		 }
	   }
	  private bool isClosed ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClosed  
	   {
	    
	     get
		{
		   return isClosed;
		 }
		 set
		 {
		   if(isClosed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosed",OldValue=isClosed,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClosed=value;
		   }
			
		 }
	   }
	  private string customsTapagFile ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsTapagFile  
	   {
	    
	     get
		{
		   return customsTapagFile;
		 }
		 set
		 {
		   if(customsTapagFile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsTapagFile",OldValue=customsTapagFile,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsTapagFile=value;
		   }
			
		 }
	   }
	  private int? customsNumeral ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? CustomsNumeral  
	   {
	    
	     get
		{
		   return customsNumeral;
		 }
		 set
		 {
		   if(customsNumeral != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsNumeral",OldValue=customsNumeral,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   customsNumeral=value;
		   }
			
		 }
	   }
	  private string requestFileNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestFileNumber  
	   {
	    
	     get
		{
		   return requestFileNumber;
		 }
		 set
		 {
		   if(requestFileNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestFileNumber",OldValue=requestFileNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestFileNumber=value;
		   }
			
		 }
	   }
	  private string referantId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReferantId  
	   {
	    
	     get
		{
		   return referantId;
		 }
		 set
		 {
		   if(referantId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReferantId",OldValue=referantId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   referantId=value;
		   }
			
		 }
	   }
	  private string referantName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReferantName  
	   {
	    
	     get
		{
		   return referantName;
		 }
		 set
		 {
		   if(referantName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReferantName",OldValue=referantName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   referantName=value;
		   }
			
		 }
	   }
	    }
   
}
	 