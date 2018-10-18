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
   public partial class QuestionnaireAnswerLinePM : EntityPM
   {
   	  private string questionnaireAnswerId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuestionnaireAnswerId  
	   {
	    
	     get
		{
		   return questionnaireAnswerId;
		 }
		 set
		 {
		   if(questionnaireAnswerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuestionnaireAnswerId",OldValue=questionnaireAnswerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   questionnaireAnswerId=value;
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
	  private string answerValue ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string AnswerValue  
	   {
	    
	     get
		{
		   return answerValue;
		 }
		 set
		 {
		   if(answerValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnswerValue",OldValue=answerValue,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   answerValue=value;
		   }
			
		 }
	   }
   }
   
}
	 