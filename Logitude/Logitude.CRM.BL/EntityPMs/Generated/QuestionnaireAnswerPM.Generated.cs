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
   public partial class QuestionnaireAnswerPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string questioneerId ;
	  	  
       
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
	  private string objectTableId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ObjectTableId  
	   {
	    
	     get
		{
		   return objectTableId;
		 }
		 set
		 {
		   if(objectTableId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ObjectTableId",OldValue=objectTableId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   objectTableId=value;
		   }
			
		 }
	   }
	  private string entityId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityId  
	   {
	    
	     get
		{
		   return entityId;
		 }
		 set
		 {
		   if(entityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityId",OldValue=entityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityId=value;
		   }
			
		 }
	   }

	   private List<QuestionnaireAnswerLinePM> questionnaireAnswerLines;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("QuestionnaireAnswerLineQuestionnaireAnswer", "Id","QuestionnaireAnswerId")]
	   [DataMember]
	   public virtual List<QuestionnaireAnswerLinePM> QuestionnaireAnswerLines  
	   {
	        get
             {
                 if (questionnaireAnswerLines == null)
                 {
                     questionnaireAnswerLines = new List<QuestionnaireAnswerLinePM>();
                 }
                 return questionnaireAnswerLines;
              }
             set { questionnaireAnswerLines = value; }
	    }
		   
	   private List<QuestionnaireAnswerLinePM>  deletedQuestionnaireAnswerLines;
	   public virtual List<QuestionnaireAnswerLinePM> DeletedQuestionnaireAnswerLines  
	   {
	        get
             {
                 if ( deletedQuestionnaireAnswerLines == null)
                 {
                      deletedQuestionnaireAnswerLines = new List<QuestionnaireAnswerLinePM>();
                 }
                 return  deletedQuestionnaireAnswerLines;
              }
             set {  deletedQuestionnaireAnswerLines = value; }
	    }
	  	  private bool hasTwoColumn ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasTwoColumn  
	   {
	    
	     get
		{
		   return hasTwoColumn;
		 }
		 set
		 {
		   if(hasTwoColumn != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasTwoColumn",OldValue=hasTwoColumn,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasTwoColumn=value;
		   }
			
		 }
	   }
   }
   
}
	 