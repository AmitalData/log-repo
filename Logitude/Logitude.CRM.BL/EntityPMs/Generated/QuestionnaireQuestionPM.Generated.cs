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
using Logitude.CRM.BL.Validators;
  
namespace Logitude.CRM.BL.EntityPMs
{
   [CustomValidation(typeof(CRMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class QuestionnaireQuestionPM : EntityPM
   {
   	  private string questioneerId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuestioneerId  
	   {
	    
	     get
		{
		   return questioneerId;
		 }
		 set
		 {
		   if(questioneerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuestioneerId",OldValue=questioneerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   questioneerId=value;
		   }
			
		 }
	   }
	  private int versionNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int VersionNumber  
	   {
	    
	     get
		{
		   return versionNumber;
		 }
		 set
		 {
		   if(versionNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VersionNumber",OldValue=versionNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   versionNumber=value;
		   }
			
		 }
	   }
	  private int questionNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int QuestionNumber  
	   {
	    
	     get
		{
		   return questionNumber;
		 }
		 set
		 {
		   if(questionNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuestionNumber",OldValue=questionNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   questionNumber=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string question ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Question  
	   {
	    
	     get
		{
		   return question;
		 }
		 set
		 {
		   if(question != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Question",OldValue=question,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   question=value;
		   }
			
		 }
	   }
	  private string questionTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuestionTypeCode  
	   {
	    
	     get
		{
		   return questionTypeCode;
		 }
		 set
		 {
		   if(questionTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuestionTypeCode",OldValue=questionTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   questionTypeCode=value;
		   }
			
		 }
	   }
	  private bool isMandatory ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMandatory  
	   {
	    
	     get
		{
		   return isMandatory;
		 }
		 set
		 {
		   if(isMandatory != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMandatory",OldValue=isMandatory,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMandatory=value;
		   }
			
		 }
	   }
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId  
	   {
	    
	     get
		{
		   return updatedByUserId;
		 }
		 set
		 {
		   if(updatedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserId",OldValue=updatedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserId=value;
		   }
			
		 }
	   }
	  private string pickListCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PickListCode  
	   {
	    
	     get
		{
		   return pickListCode;
		 }
		 set
		 {
		   if(pickListCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickListCode",OldValue=pickListCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pickListCode=value;
		   }
			
		 }
	   }
	  private bool isAddOther ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAddOther  
	   {
	    
	     get
		{
		   return isAddOther;
		 }
		 set
		 {
		   if(isAddOther != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAddOther",OldValue=isAddOther,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAddOther=value;
		   }
			
		 }
	   }
   }
   
}
	 