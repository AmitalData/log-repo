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
   public partial class OpportunityPM : EntityPM
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
	  private string ownerId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OwnerId  
	   {
	    
	     get
		{
		   return ownerId;
		 }
		 set
		 {
		   if(ownerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OwnerId",OldValue=ownerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ownerId=value;
		   }
			
		 }
	   }
	  private string subject ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Subject  
	   {
	    
	     get
		{
		   return subject;
		 }
		 set
		 {
		   if(subject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Subject",OldValue=subject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   subject=value;
		   }
			
		 }
	   }
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string leadSourceId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LeadSourceId  
	   {
	    
	     get
		{
		   return leadSourceId;
		 }
		 set
		 {
		   if(leadSourceId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeadSourceId",OldValue=leadSourceId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   leadSourceId=value;
		   }
			
		 }
	   }
	  private string contactId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactId  
	   {
	    
	     get
		{
		   return contactId;
		 }
		 set
		 {
		   if(contactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactId",OldValue=contactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactId=value;
		   }
			
		 }
	   }
	  private DateTime? estimatedClosingDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EstimatedClosingDate  
	   {
	    
	     get
		{
		   return estimatedClosingDate;
		 }
		 set
		 {
		   if(estimatedClosingDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedClosingDate",OldValue=estimatedClosingDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   estimatedClosingDate=value;
		   }
			
		 }
	   }
	  private string stageId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string StageId  
	   {
	    
	     get
		{
		   return stageId;
		 }
		 set
		 {
		   if(stageId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StageId",OldValue=stageId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   stageId=value;
		   }
			
		 }
	   }
	  private int? probability ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Probability  
	   {
	    
	     get
		{
		   return probability;
		 }
		 set
		 {
		   if(probability != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Probability",OldValue=probability,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   probability=value;
		   }
			
		 }
	   }
	  private DateTime? createDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string ratingCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string RatingCode  
	   {
	    
	     get
		{
		   return ratingCode;
		 }
		 set
		 {
		   if(ratingCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RatingCode",OldValue=ratingCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ratingCode=value;
		   }
			
		 }
	   }
	  private bool isClosed ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private DateTime? actualClosingDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ActualClosingDate  
	   {
	    
	     get
		{
		   return actualClosingDate;
		 }
		 set
		 {
		   if(actualClosingDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActualClosingDate",OldValue=actualClosingDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   actualClosingDate=value;
		   }
			
		 }
	   }
	  private string closingDescription ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosingDescription  
	   {
	    
	     get
		{
		   return closingDescription;
		 }
		 set
		 {
		   if(closingDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosingDescription",OldValue=closingDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closingDescription=value;
		   }
			
		 }
	   }
	  private string customerName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SearchFields  
	   {
	    
	     get
		{
		   return searchFields;
		 }
		 set
		 {
		   if(searchFields != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SearchFields",OldValue=searchFields,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   searchFields=value;
		   }
			
		 }
	   }
	  private string ownerName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OwnerName  
	   {
	    
	     get
		{
		   return ownerName;
		 }
		 set
		 {
		   if(ownerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OwnerName",OldValue=ownerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ownerName=value;
		   }
			
		 }
	   }
	  private string stageName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string StageName  
	   {
	    
	     get
		{
		   return stageName;
		 }
		 set
		 {
		   if(stageName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StageName",OldValue=stageName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   stageName=value;
		   }
			
		 }
	   }
	  private string ratingName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string RatingName  
	   {
	    
	     get
		{
		   return ratingName;
		 }
		 set
		 {
		   if(ratingName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RatingName",OldValue=ratingName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ratingName=value;
		   }
			
		 }
	   }
	  private string closedToCompetitorId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosedToCompetitorId  
	   {
	    
	     get
		{
		   return closedToCompetitorId;
		 }
		 set
		 {
		   if(closedToCompetitorId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosedToCompetitorId",OldValue=closedToCompetitorId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closedToCompetitorId=value;
		   }
			
		 }
	   }
	  private int? numberOfShipments ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? NumberOfShipments  
	   {
	    
	     get
		{
		   return numberOfShipments;
		 }
		 set
		 {
		   if(numberOfShipments != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfShipments",OldValue=numberOfShipments,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   numberOfShipments=value;
		   }
			
		 }
	   }

	   private List<OpportunityProductPM> opportunityProducts;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("OpportunityOpportunityProducts", "Id","OpportunityId")]
	   [DataMember]
	   public virtual List<OpportunityProductPM> OpportunityProducts  
	   {
	        get
             {
                 if (opportunityProducts == null)
                 {
                     opportunityProducts = new List<OpportunityProductPM>();
                 }
                 return opportunityProducts;
              }
             set { opportunityProducts = value; }
	    }
		   
	   private List<OpportunityProductPM>  deletedOpportunityProducts;
	   public virtual List<OpportunityProductPM> DeletedOpportunityProducts  
	   {
	        get
             {
                 if ( deletedOpportunityProducts == null)
                 {
                      deletedOpportunityProducts = new List<OpportunityProductPM>();
                 }
                 return  deletedOpportunityProducts;
              }
             set {  deletedOpportunityProducts = value; }
	    }
	  	  private decimal? valueField ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ValueField  
	   {
	    
	     get
		{
		   return valueField;
		 }
		 set
		 {
		   if(valueField != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ValueField",OldValue=valueField,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   valueField=value;
		   }
			
		 }
	   }

	   private List<OpportunityCompetitorPM> opportunityCompetitors;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("OpportunityCompetitorOpportunity", "Id","OpportunityId")]
	   [DataMember]
	   public virtual List<OpportunityCompetitorPM> OpportunityCompetitors  
	   {
	        get
             {
                 if (opportunityCompetitors == null)
                 {
                     opportunityCompetitors = new List<OpportunityCompetitorPM>();
                 }
                 return opportunityCompetitors;
              }
             set { opportunityCompetitors = value; }
	    }
		   
	   private List<OpportunityCompetitorPM>  deletedOpportunityCompetitors;
	   public virtual List<OpportunityCompetitorPM> DeletedOpportunityCompetitors  
	   {
	        get
             {
                 if ( deletedOpportunityCompetitors == null)
                 {
                      deletedOpportunityCompetitors = new List<OpportunityCompetitorPM>();
                 }
                 return  deletedOpportunityCompetitors;
              }
             set {  deletedOpportunityCompetitors = value; }
	    }
	  	  private DateTime? lastStageDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastStageDate  
	   {
	    
	     get
		{
		   return lastStageDate;
		 }
		 set
		 {
		   if(lastStageDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastStageDate",OldValue=lastStageDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastStageDate=value;
		   }
			
		 }
	   }
	  private string lastStageIdBeforeClosure ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastStageIdBeforeClosure  
	   {
	    
	     get
		{
		   return lastStageIdBeforeClosure;
		 }
		 set
		 {
		   if(lastStageIdBeforeClosure != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastStageIdBeforeClosure",OldValue=lastStageIdBeforeClosure,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastStageIdBeforeClosure=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field1 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field1  
	   {
	    
	     get
		{
		   return field1;
		 }
		 set
		 {
		   if(field1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field1",OldValue=field1,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field1=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field2 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field2  
	   {
	    
	     get
		{
		   return field2;
		 }
		 set
		 {
		   if(field2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field2",OldValue=field2,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field2=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field3 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field3  
	   {
	    
	     get
		{
		   return field3;
		 }
		 set
		 {
		   if(field3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field3",OldValue=field3,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field3=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field4 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field4  
	   {
	    
	     get
		{
		   return field4;
		 }
		 set
		 {
		   if(field4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field4",OldValue=field4,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field4=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field5 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field5  
	   {
	    
	     get
		{
		   return field5;
		 }
		 set
		 {
		   if(field5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field5",OldValue=field5,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field5=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field6 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field6  
	   {
	    
	     get
		{
		   return field6;
		 }
		 set
		 {
		   if(field6 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field6",OldValue=field6,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field6=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field7 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field7  
	   {
	    
	     get
		{
		   return field7;
		 }
		 set
		 {
		   if(field7 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field7",OldValue=field7,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field7=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field8 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field8  
	   {
	    
	     get
		{
		   return field8;
		 }
		 set
		 {
		   if(field8 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field8",OldValue=field8,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field8=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field9 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field9  
	   {
	    
	     get
		{
		   return field9;
		 }
		 set
		 {
		   if(field9 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field9",OldValue=field9,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field9=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field10 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field10  
	   {
	    
	     get
		{
		   return field10;
		 }
		 set
		 {
		   if(field10 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field10",OldValue=field10,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field10=value;
		   }
			
		 }
	   }
	  private DateTime? lastCompletedActivityDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastCompletedActivityDate  
	   {
	    
	     get
		{
		   return lastCompletedActivityDate;
		 }
		 set
		 {
		   if(lastCompletedActivityDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastCompletedActivityDate",OldValue=lastCompletedActivityDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastCompletedActivityDate=value;
		   }
			
		 }
	   }
	  private string leadDescription ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LeadDescription  
	   {
	    
	     get
		{
		   return leadDescription;
		 }
		 set
		 {
		   if(leadDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeadDescription",OldValue=leadDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   leadDescription=value;
		   }
			
		 }
	   }
	  private string lastCompletedActivityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastCompletedActivityTypeCode  
	   {
	    
	     get
		{
		   return lastCompletedActivityTypeCode;
		 }
		 set
		 {
		   if(lastCompletedActivityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastCompletedActivityTypeCode",OldValue=lastCompletedActivityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastCompletedActivityTypeCode=value;
		   }
			
		 }
	   }
	  private string lastActivitySubject ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastActivitySubject  
	   {
	    
	     get
		{
		   return lastActivitySubject;
		 }
		 set
		 {
		   if(lastActivitySubject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastActivitySubject",OldValue=lastActivitySubject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastActivitySubject=value;
		   }
			
		 }
	   }
	  private DateTime? nextActivityDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? NextActivityDate  
	   {
	    
	     get
		{
		   return nextActivityDate;
		 }
		 set
		 {
		   if(nextActivityDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextActivityDate",OldValue=nextActivityDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   nextActivityDate=value;
		   }
			
		 }
	   }
	  private string nextActivityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NextActivityTypeCode  
	   {
	    
	     get
		{
		   return nextActivityTypeCode;
		 }
		 set
		 {
		   if(nextActivityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextActivityTypeCode",OldValue=nextActivityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nextActivityTypeCode=value;
		   }
			
		 }
	   }
	  private string nextActivitySubject ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NextActivitySubject  
	   {
	    
	     get
		{
		   return nextActivitySubject;
		 }
		 set
		 {
		   if(nextActivitySubject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextActivitySubject",OldValue=nextActivitySubject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nextActivitySubject=value;
		   }
			
		 }
	   }
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notes  
	   {
	    
	     get
		{
		   return notes;
		 }
		 set
		 {
		   if(notes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notes",OldValue=notes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notes=value;
		   }
			
		 }
	   }
	  private DateTime? stageDueDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StageDueDate  
	   {
	    
	     get
		{
		   return stageDueDate;
		 }
		 set
		 {
		   if(stageDueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StageDueDate",OldValue=stageDueDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   stageDueDate=value;
		   }
			
		 }
	   }
	  private string businessUnitId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BusinessUnitId  
	   {
	    
	     get
		{
		   return businessUnitId;
		 }
		 set
		 {
		   if(businessUnitId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BusinessUnitId",OldValue=businessUnitId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   businessUnitId=value;
		   }
			
		 }
	   }
	  private int? stageProbability ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? StageProbability  
	   {
	    
	     get
		{
		   return stageProbability;
		 }
		 set
		 {
		   if(stageProbability != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StageProbability",OldValue=stageProbability,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   stageProbability=value;
		   }
			
		 }
	   }
	  private int? ratingIndexOrder ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? RatingIndexOrder  
	   {
	    
	     get
		{
		   return ratingIndexOrder;
		 }
		 set
		 {
		   if(ratingIndexOrder != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RatingIndexOrder",OldValue=ratingIndexOrder,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   ratingIndexOrder=value;
		   }
			
		 }
	   }
	  private string leadUserId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LeadUserId  
	   {
	    
	     get
		{
		   return leadUserId;
		 }
		 set
		 {
		   if(leadUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeadUserId",OldValue=leadUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   leadUserId=value;
		   }
			
		 }
	   }
	  private string leadPartnerId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LeadPartnerId  
	   {
	    
	     get
		{
		   return leadPartnerId;
		 }
		 set
		 {
		   if(leadPartnerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeadPartnerId",OldValue=leadPartnerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   leadPartnerId=value;
		   }
			
		 }
	   }
	  private string agentId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentId  
	   {
	    
	     get
		{
		   return agentId;
		 }
		 set
		 {
		   if(agentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentId",OldValue=agentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentId=value;
		   }
			
		 }
	   }
	  private string foreignClientId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForeignClientId  
	   {
	    
	     get
		{
		   return foreignClientId;
		 }
		 set
		 {
		   if(foreignClientId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignClientId",OldValue=foreignClientId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   foreignClientId=value;
		   }
			
		 }
	   }

	   private List<OpportunityAdditionalServicePM> opportunityAdditionalServices;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("OpportunityAdditionalServiceOpportunity", "Id","OpportunityId")]
	   [DataMember]
	   public virtual List<OpportunityAdditionalServicePM> OpportunityAdditionalServices  
	   {
	        get
             {
                 if (opportunityAdditionalServices == null)
                 {
                     opportunityAdditionalServices = new List<OpportunityAdditionalServicePM>();
                 }
                 return opportunityAdditionalServices;
              }
             set { opportunityAdditionalServices = value; }
	    }
		   
	   private List<OpportunityAdditionalServicePM>  deletedOpportunityAdditionalServices;
	   public virtual List<OpportunityAdditionalServicePM> DeletedOpportunityAdditionalServices  
	   {
	        get
             {
                 if ( deletedOpportunityAdditionalServices == null)
                 {
                      deletedOpportunityAdditionalServices = new List<OpportunityAdditionalServicePM>();
                 }
                 return  deletedOpportunityAdditionalServices;
              }
             set {  deletedOpportunityAdditionalServices = value; }
	    }
	  	  private string contactName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactName  
	   {
	    
	     get
		{
		   return contactName;
		 }
		 set
		 {
		   if(contactName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactName",OldValue=contactName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactName=value;
		   }
			
		 }
	   }
	  private string contactPhone ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactPhone  
	   {
	    
	     get
		{
		   return contactPhone;
		 }
		 set
		 {
		   if(contactPhone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactPhone",OldValue=contactPhone,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactPhone=value;
		   }
			
		 }
	   }
	  private string concurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConcurrencyGUID  
	   {
	    
	     get
		{
		   return concurrencyGUID;
		 }
		 set
		 {
		   if(concurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConcurrencyGUID",OldValue=concurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   concurrencyGUID=value;
		   }
			
		 }
	   }
	  private string closingReasonId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosingReasonId  
	   {
	    
	     get
		{
		   return closingReasonId;
		 }
		 set
		 {
		   if(closingReasonId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosingReasonId",OldValue=closingReasonId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closingReasonId=value;
		   }
			
		 }
	   }
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCancelled  
	   {
	    
	     get
		{
		   return isCancelled;
		 }
		 set
		 {
		   if(isCancelled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancelled",OldValue=isCancelled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCancelled=value;
		   }
			
		 }
	   }
	  private string closingReasonName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosingReasonName  
	   {
	    
	     get
		{
		   return closingReasonName;
		 }
		 set
		 {
		   if(closingReasonName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosingReasonName",OldValue=closingReasonName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closingReasonName=value;
		   }
			
		 }
	   }
	  private string leadSourceName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LeadSourceName  
	   {
	    
	     get
		{
		   return leadSourceName;
		 }
		 set
		 {
		   if(leadSourceName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeadSourceName",OldValue=leadSourceName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   leadSourceName=value;
		   }
			
		 }
	   }
	  private string opportunityTypeId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpportunityTypeId  
	   {
	    
	     get
		{
		   return opportunityTypeId;
		 }
		 set
		 {
		   if(opportunityTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpportunityTypeId",OldValue=opportunityTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   opportunityTypeId=value;
		   }
			
		 }
	   }
	  private string opportunityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpportunityTypeName  
	   {
	    
	     get
		{
		   return opportunityTypeName;
		 }
		 set
		 {
		   if(opportunityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpportunityTypeName",OldValue=opportunityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   opportunityTypeName=value;
		   }
			
		 }
	   }
	  private string leadPartnerName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LeadPartnerName  
	   {
	    
	     get
		{
		   return leadPartnerName;
		 }
		 set
		 {
		   if(leadPartnerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeadPartnerName",OldValue=leadPartnerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   leadPartnerName=value;
		   }
			
		 }
	   }
	  private bool isClosedLost ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClosedLost  
	   {
	    
	     get
		{
		   return isClosedLost;
		 }
		 set
		 {
		   if(isClosedLost != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosedLost",OldValue=isClosedLost,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClosedLost=value;
		   }
			
		 }
	   }
	  private string customerRankCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerRankCode  
	   {
	    
	     get
		{
		   return customerRankCode;
		 }
		 set
		 {
		   if(customerRankCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerRankCode",OldValue=customerRankCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerRankCode=value;
		   }
			
		 }
	   }
	  private string customerRankName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerRankName  
	   {
	    
	     get
		{
		   return customerRankName;
		 }
		 set
		 {
		   if(customerRankName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerRankName",OldValue=customerRankName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerRankName=value;
		   }
			
		 }
	   }
	  private bool postToFollowersAsWon ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool PostToFollowersAsWon  
	   {
	    
	     get
		{
		   return postToFollowersAsWon;
		 }
		 set
		 {
		   if(postToFollowersAsWon != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PostToFollowersAsWon",OldValue=postToFollowersAsWon,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   postToFollowersAsWon=value;
		   }
			
		 }
	   }
	  private string closingReasonCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosingReasonCode  
	   {
	    
	     get
		{
		   return closingReasonCode;
		 }
		 set
		 {
		   if(closingReasonCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosingReasonCode",OldValue=closingReasonCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closingReasonCode=value;
		   }
			
		 }
	   }
	  private string customerExternalId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerExternalId  
	   {
	    
	     get
		{
		   return customerExternalId;
		 }
		 set
		 {
		   if(customerExternalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerExternalId",OldValue=customerExternalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerExternalId=value;
		   }
			
		 }
	   }
	  private bool isCopy ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCopy  
	   {
	    
	     get
		{
		   return isCopy;
		 }
		 set
		 {
		   if(isCopy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCopy",OldValue=isCopy,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCopy=value;
		   }
			
		 }
	   }
	  private string copyFromEntityId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CopyFromEntityId  
	   {
	    
	     get
		{
		   return copyFromEntityId;
		 }
		 set
		 {
		   if(copyFromEntityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyFromEntityId",OldValue=copyFromEntityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   copyFromEntityId=value;
		   }
			
		 }
	   }
	  private bool isCustomerBlockedBusinessUnit ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomerBlockedBusinessUnit  
	   {
	    
	     get
		{
		   return isCustomerBlockedBusinessUnit;
		 }
		 set
		 {
		   if(isCustomerBlockedBusinessUnit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomerBlockedBusinessUnit",OldValue=isCustomerBlockedBusinessUnit,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomerBlockedBusinessUnit=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field11 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field11  
	   {
	    
	     get
		{
		   return field11;
		 }
		 set
		 {
		   if(field11 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field11",OldValue=field11,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field11=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field12 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field12  
	   {
	    
	     get
		{
		   return field12;
		 }
		 set
		 {
		   if(field12 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field12",OldValue=field12,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field12=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field13 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field13  
	   {
	    
	     get
		{
		   return field13;
		 }
		 set
		 {
		   if(field13 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field13",OldValue=field13,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field13=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field14 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field14  
	   {
	    
	     get
		{
		   return field14;
		 }
		 set
		 {
		   if(field14 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field14",OldValue=field14,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field14=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field15 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field15  
	   {
	    
	     get
		{
		   return field15;
		 }
		 set
		 {
		   if(field15 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field15",OldValue=field15,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field15=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field16 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field16  
	   {
	    
	     get
		{
		   return field16;
		 }
		 set
		 {
		   if(field16 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field16",OldValue=field16,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field16=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field17 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field17  
	   {
	    
	     get
		{
		   return field17;
		 }
		 set
		 {
		   if(field17 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field17",OldValue=field17,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field17=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field18 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field18  
	   {
	    
	     get
		{
		   return field18;
		 }
		 set
		 {
		   if(field18 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field18",OldValue=field18,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field18=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field19 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field19  
	   {
	    
	     get
		{
		   return field19;
		 }
		 set
		 {
		   if(field19 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field19",OldValue=field19,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field19=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field20 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field20  
	   {
	    
	     get
		{
		   return field20;
		 }
		 set
		 {
		   if(field20 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field20",OldValue=field20,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field20=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field21 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field21  
	   {
	    
	     get
		{
		   return field21;
		 }
		 set
		 {
		   if(field21 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field21",OldValue=field21,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field21=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field22 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field22  
	   {
	    
	     get
		{
		   return field22;
		 }
		 set
		 {
		   if(field22 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field22",OldValue=field22,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field22=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field23 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field23  
	   {
	    
	     get
		{
		   return field23;
		 }
		 set
		 {
		   if(field23 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field23",OldValue=field23,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field23=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field24 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field24  
	   {
	    
	     get
		{
		   return field24;
		 }
		 set
		 {
		   if(field24 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field24",OldValue=field24,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field24=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field25 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field25  
	   {
	    
	     get
		{
		   return field25;
		 }
		 set
		 {
		   if(field25 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field25",OldValue=field25,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field25=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field26 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field26  
	   {
	    
	     get
		{
		   return field26;
		 }
		 set
		 {
		   if(field26 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field26",OldValue=field26,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field26=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field27 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field27  
	   {
	    
	     get
		{
		   return field27;
		 }
		 set
		 {
		   if(field27 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field27",OldValue=field27,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field27=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field28 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field28  
	   {
	    
	     get
		{
		   return field28;
		 }
		 set
		 {
		   if(field28 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field28",OldValue=field28,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field28=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field29 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field29  
	   {
	    
	     get
		{
		   return field29;
		 }
		 set
		 {
		   if(field29 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field29",OldValue=field29,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field29=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field30 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field30  
	   {
	    
	     get
		{
		   return field30;
		 }
		 set
		 {
		   if(field30 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field30",OldValue=field30,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field30=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field31 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field31  
	   {
	    
	     get
		{
		   return field31;
		 }
		 set
		 {
		   if(field31 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field31",OldValue=field31,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field31=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field32 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field32  
	   {
	    
	     get
		{
		   return field32;
		 }
		 set
		 {
		   if(field32 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field32",OldValue=field32,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field32=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field33 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field33  
	   {
	    
	     get
		{
		   return field33;
		 }
		 set
		 {
		   if(field33 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field33",OldValue=field33,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field33=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field34 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field34  
	   {
	    
	     get
		{
		   return field34;
		 }
		 set
		 {
		   if(field34 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field34",OldValue=field34,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field34=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field35 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field35  
	   {
	    
	     get
		{
		   return field35;
		 }
		 set
		 {
		   if(field35 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field35",OldValue=field35,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field35=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field36 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field36  
	   {
	    
	     get
		{
		   return field36;
		 }
		 set
		 {
		   if(field36 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field36",OldValue=field36,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field36=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field37 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field37  
	   {
	    
	     get
		{
		   return field37;
		 }
		 set
		 {
		   if(field37 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field37",OldValue=field37,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field37=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field38 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field38  
	   {
	    
	     get
		{
		   return field38;
		 }
		 set
		 {
		   if(field38 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field38",OldValue=field38,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field38=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field39 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field39  
	   {
	    
	     get
		{
		   return field39;
		 }
		 set
		 {
		   if(field39 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field39",OldValue=field39,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field39=value;
		   }
			
		 }
	   }
	  private CustomFieldClass field40 ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field40  
	   {
	    
	     get
		{
		   return field40;
		 }
		 set
		 {
		   if(field40 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field40",OldValue=field40,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field40=value;
		   }
			
		 }
	   }
	  private int? numberOfConnectedQuotes ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? NumberOfConnectedQuotes  
	   {
	    
	     get
		{
		   return numberOfConnectedQuotes;
		 }
		 set
		 {
		   if(numberOfConnectedQuotes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfConnectedQuotes",OldValue=numberOfConnectedQuotes,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   numberOfConnectedQuotes=value;
		   }
			
		 }
	   }
	  private string userName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string UserName  
	   {
	    
	     get
		{
		   return userName;
		 }
		 set
		 {
		   if(userName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UserName",OldValue=userName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   userName=value;
		   }
			
		 }
	   }
	  private string clientId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClientId  
	   {
	    
	     get
		{
		   return clientId;
		 }
		 set
		 {
		   if(clientId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClientId",OldValue=clientId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   clientId=value;
		   }
			
		 }
	   }
	  private string leadOrigin ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LeadOrigin  
	   {
	    
	     get
		{
		   return leadOrigin;
		 }
		 set
		 {
		   if(leadOrigin != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeadOrigin",OldValue=leadOrigin,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   leadOrigin=value;
		   }
			
		 }
	   }
	  private string campaign ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Campaign  
	   {
	    
	     get
		{
		   return campaign;
		 }
		 set
		 {
		   if(campaign != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Campaign",OldValue=campaign,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   campaign=value;
		   }
			
		 }
	   }
	    }
   
}
	 