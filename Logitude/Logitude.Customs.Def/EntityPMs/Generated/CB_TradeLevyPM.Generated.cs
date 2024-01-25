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
   public partial class CB_TradeLevyPM : EntityPM
   {
   	  private string iD ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private string customsBookTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBookTypeID  
	   {
	    
	     get
		{
		   return customsBookTypeID;
		 }
		 set
		 {
		   if(customsBookTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBookTypeID",OldValue=customsBookTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBookTypeID=value;
		   }
			
		 }
	   }
	  private string levyNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LevyNumber  
	   {
	    
	     get
		{
		   return levyNumber;
		 }
		 set
		 {
		   if(levyNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LevyNumber",OldValue=levyNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   levyNumber=value;
		   }
			
		 }
	   }
	  private DateTime? endOfInquiryDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndOfInquiryDate  
	   {
	    
	     get
		{
		   return endOfInquiryDate;
		 }
		 set
		 {
		   if(endOfInquiryDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndOfInquiryDate",OldValue=endOfInquiryDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endOfInquiryDate=value;
		   }
			
		 }
	   }
	  private DateTime? endOfLevyDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndOfLevyDate  
	   {
	    
	     get
		{
		   return endOfLevyDate;
		 }
		 set
		 {
		   if(endOfLevyDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndOfLevyDate",OldValue=endOfLevyDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endOfLevyDate=value;
		   }
			
		 }
	   }
	  private string inceptionCodeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InceptionCodeID  
	   {
	    
	     get
		{
		   return inceptionCodeID;
		 }
		 set
		 {
		   if(inceptionCodeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InceptionCodeID",OldValue=inceptionCodeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   inceptionCodeID=value;
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
	  private string tradeLevyStatusID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeLevyStatusID  
	   {
	    
	     get
		{
		   return tradeLevyStatusID;
		 }
		 set
		 {
		   if(tradeLevyStatusID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeLevyStatusID",OldValue=tradeLevyStatusID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeLevyStatusID=value;
		   }
			
		 }
	   }
	  private string computationMethodDataID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ComputationMethodDataID  
	   {
	    
	     get
		{
		   return computationMethodDataID;
		 }
		 set
		 {
		   if(computationMethodDataID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ComputationMethodDataID",OldValue=computationMethodDataID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   computationMethodDataID=value;
		   }
			
		 }
	   }
	  private string levyTrustID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LevyTrustID  
	   {
	    
	     get
		{
		   return levyTrustID;
		 }
		 set
		 {
		   if(levyTrustID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LevyTrustID",OldValue=levyTrustID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   levyTrustID=value;
		   }
			
		 }
	   }
	  private string paragraphTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParagraphTypeID  
	   {
	    
	     get
		{
		   return paragraphTypeID;
		 }
		 set
		 {
		   if(paragraphTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParagraphTypeID",OldValue=paragraphTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paragraphTypeID=value;
		   }
			
		 }
	   }
   }
   
}
	 