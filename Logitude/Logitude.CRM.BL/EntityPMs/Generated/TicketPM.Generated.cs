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
   public partial class TicketPM : EntityPM
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
	  private string ticketNumber ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TicketNumber  
	   {
	    
	     get
		{
		   return ticketNumber;
		 }
		 set
		 {
		   if(ticketNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketNumber",OldValue=ticketNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ticketNumber=value;
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
	  private string createdByContactId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByContactId  
	   {
	    
	     get
		{
		   return createdByContactId;
		 }
		 set
		 {
		   if(createdByContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByContactId",OldValue=createdByContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByContactId=value;
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
	  private string companyId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CompanyId  
	   {
	    
	     get
		{
		   return companyId;
		 }
		 set
		 {
		   if(companyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CompanyId",OldValue=companyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   companyId=value;
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
	  private string mainClassificationId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainClassificationId  
	   {
	    
	     get
		{
		   return mainClassificationId;
		 }
		 set
		 {
		   if(mainClassificationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainClassificationId",OldValue=mainClassificationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainClassificationId=value;
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
	  private string severityId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SeverityId  
	   {
	    
	     get
		{
		   return severityId;
		 }
		 set
		 {
		   if(severityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SeverityId",OldValue=severityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   severityId=value;
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
	  private string ticketTypeId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TicketTypeId  
	   {
	    
	     get
		{
		   return ticketTypeId;
		 }
		 set
		 {
		   if(ticketTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketTypeId",OldValue=ticketTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ticketTypeId=value;
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
	  private string ticketDescription ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TicketDescription  
	   {
	    
	     get
		{
		   return ticketDescription;
		 }
		 set
		 {
		   if(ticketDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketDescription",OldValue=ticketDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ticketDescription=value;
		   }
			
		 }
	   }
	  private string companyName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CompanyName  
	   {
	    
	     get
		{
		   return companyName;
		 }
		 set
		 {
		   if(companyName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CompanyName",OldValue=companyName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   companyName=value;
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
	  private string mainClassificationName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainClassificationName  
	   {
	    
	     get
		{
		   return mainClassificationName;
		 }
		 set
		 {
		   if(mainClassificationName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainClassificationName",OldValue=mainClassificationName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainClassificationName=value;
		   }
			
		 }
	   }
	  private string typeName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeName  
	   {
	    
	     get
		{
		   return typeName;
		 }
		 set
		 {
		   if(typeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeName",OldValue=typeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeName=value;
		   }
			
		 }
	   }
	  private string severityName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SeverityName  
	   {
	    
	     get
		{
		   return severityName;
		 }
		 set
		 {
		   if(severityName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SeverityName",OldValue=severityName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   severityName=value;
		   }
			
		 }
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
	  private string lastCompletedActivitySubject ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastCompletedActivitySubject  
	   {
	    
	     get
		{
		   return lastCompletedActivitySubject;
		 }
		 set
		 {
		   if(lastCompletedActivitySubject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastCompletedActivitySubject",OldValue=lastCompletedActivitySubject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastCompletedActivitySubject=value;
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
	  private string cCs ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CCs  
	   {
	    
	     get
		{
		   return cCs;
		 }
		 set
		 {
		   if(cCs != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CCs",OldValue=cCs,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cCs=value;
		   }
			
		 }
	   }
	  private string bcc ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Bcc  
	   {
	    
	     get
		{
		   return bcc;
		 }
		 set
		 {
		   if(bcc != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Bcc",OldValue=bcc,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bcc=value;
		   }
			
		 }
	   }
	  private string createdByContactName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByContactName  
	   {
	    
	     get
		{
		   return createdByContactName;
		 }
		 set
		 {
		   if(createdByContactName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByContactName",OldValue=createdByContactName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByContactName=value;
		   }
			
		 }
	   }
	  private string rankCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string RankCode  
	   {
	    
	     get
		{
		   return rankCode;
		 }
		 set
		 {
		   if(rankCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RankCode",OldValue=rankCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   rankCode=value;
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
	  private string employeeGroupId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EmployeeGroupId  
	   {
	    
	     get
		{
		   return employeeGroupId;
		 }
		 set
		 {
		   if(employeeGroupId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EmployeeGroupId",OldValue=employeeGroupId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   employeeGroupId=value;
		   }
			
		 }
	   }
	  private DateTime? firstResponseTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FirstResponseTime  
	   {
	    
	     get
		{
		   return firstResponseTime;
		 }
		 set
		 {
		   if(firstResponseTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstResponseTime",OldValue=firstResponseTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   firstResponseTime=value;
		   }
			
		 }
	   }
	  private DateTime? firstResponseDue ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FirstResponseDue  
	   {
	    
	     get
		{
		   return firstResponseDue;
		 }
		 set
		 {
		   if(firstResponseDue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstResponseDue",OldValue=firstResponseDue,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   firstResponseDue=value;
		   }
			
		 }
	   }
	  private DateTime? fullResolvedTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FullResolvedTime  
	   {
	    
	     get
		{
		   return fullResolvedTime;
		 }
		 set
		 {
		   if(fullResolvedTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FullResolvedTime",OldValue=fullResolvedTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fullResolvedTime=value;
		   }
			
		 }
	   }
	  private DateTime? resolveWithinDue ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ResolveWithinDue  
	   {
	    
	     get
		{
		   return resolveWithinDue;
		 }
		 set
		 {
		   if(resolveWithinDue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResolveWithinDue",OldValue=resolveWithinDue,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   resolveWithinDue=value;
		   }
			
		 }
	   }
	  private string contactEmail ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactEmail  
	   {
	    
	     get
		{
		   return contactEmail;
		 }
		 set
		 {
		   if(contactEmail != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactEmail",OldValue=contactEmail,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactEmail=value;
		   }
			
		 }
	   }
	  private string guidId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string GuidId  
	   {
	    
	     get
		{
		   return guidId;
		 }
		 set
		 {
		   if(guidId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GuidId",OldValue=guidId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   guidId=value;
		   }
			
		 }
	   }
	  private int openEscalation ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int OpenEscalation  
	   {
	    
	     get
		{
		   return openEscalation;
		 }
		 set
		 {
		   if(openEscalation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenEscalation",OldValue=openEscalation,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   openEscalation=value;
		   }
			
		 }
	   }
	  private string internalUsers ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string InternalUsers  
	   {
	    
	     get
		{
		   return internalUsers;
		 }
		 set
		 {
		   if(internalUsers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InternalUsers",OldValue=internalUsers,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   internalUsers=value;
		   }
			
		 }
	   }
	  private string closureDescription ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClosureDescription  
	   {
	    
	     get
		{
		   return closureDescription;
		 }
		 set
		 {
		   if(closureDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClosureDescription",OldValue=closureDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   closureDescription=value;
		   }
			
		 }
	   }
	  private bool closewithoutnotifying ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Closewithoutnotifying  
	   {
	    
	     get
		{
		   return closewithoutnotifying;
		 }
		 set
		 {
		   if(closewithoutnotifying != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Closewithoutnotifying",OldValue=closewithoutnotifying,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   closewithoutnotifying=value;
		   }
			
		 }
	   }
	  private string stageCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string StageCode  
	   {
	    
	     get
		{
		   return stageCode;
		 }
		 set
		 {
		   if(stageCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StageCode",OldValue=stageCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   stageCode=value;
		   }
			
		 }
	   }
	  private string lastCompletedActivityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastCompletedActivityTypeName  
	   {
	    
	     get
		{
		   return lastCompletedActivityTypeName;
		 }
		 set
		 {
		   if(lastCompletedActivityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastCompletedActivityTypeName",OldValue=lastCompletedActivityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastCompletedActivityTypeName=value;
		   }
			
		 }
	   }
	  private string nextActivityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NextActivityTypeName  
	   {
	    
	     get
		{
		   return nextActivityTypeName;
		 }
		 set
		 {
		   if(nextActivityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextActivityTypeName",OldValue=nextActivityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nextActivityTypeName=value;
		   }
			
		 }
	   }
	  private string secondaryClassificationId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SecondaryClassificationId  
	   {
	    
	     get
		{
		   return secondaryClassificationId;
		 }
		 set
		 {
		   if(secondaryClassificationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SecondaryClassificationId",OldValue=secondaryClassificationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   secondaryClassificationId=value;
		   }
			
		 }
	   }
	  private string secondaryClassificationName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SecondaryClassificationName  
	   {
	    
	     get
		{
		   return secondaryClassificationName;
		 }
		 set
		 {
		   if(secondaryClassificationName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SecondaryClassificationName",OldValue=secondaryClassificationName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   secondaryClassificationName=value;
		   }
			
		 }
	   }
	  private string shipmentId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentId  
	   {
	    
	     get
		{
		   return shipmentId;
		 }
		 set
		 {
		   if(shipmentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentId",OldValue=shipmentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentId=value;
		   }
			
		 }
	   }
	  private string shipmentNumber ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentNumber  
	   {
	    
	     get
		{
		   return shipmentNumber;
		 }
		 set
		 {
		   if(shipmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentNumber",OldValue=shipmentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentNumber=value;
		   }
			
		 }
	   }
	  private string updatedByUserName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserName  
	   {
	    
	     get
		{
		   return updatedByUserName;
		 }
		 set
		 {
		   if(updatedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserName",OldValue=updatedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserName=value;
		   }
			
		 }
	   }
	  private DateTime? firstResolveDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FirstResolveDate  
	   {
	    
	     get
		{
		   return firstResolveDate;
		 }
		 set
		 {
		   if(firstResolveDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstResolveDate",OldValue=firstResolveDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   firstResolveDate=value;
		   }
			
		 }
	   }
	  private string ticketHeader ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TicketHeader  
	   {
	    
	     get
		{
		   return ticketHeader;
		 }
		 set
		 {
		   if(ticketHeader != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketHeader",OldValue=ticketHeader,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ticketHeader=value;
		   }
			
		 }
	   }
	  private string ticketFooter ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TicketFooter  
	   {
	    
	     get
		{
		   return ticketFooter;
		 }
		 set
		 {
		   if(ticketFooter != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketFooter",OldValue=ticketFooter,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ticketFooter=value;
		   }
			
		 }
	   }
	  private string ticketReplyto ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TicketReplyto  
	   {
	    
	     get
		{
		   return ticketReplyto;
		 }
		 set
		 {
		   if(ticketReplyto != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketReplyto",OldValue=ticketReplyto,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ticketReplyto=value;
		   }
			
		 }
	   }
	  private string severityCode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SeverityCode  
	   {
	    
	     get
		{
		   return severityCode;
		 }
		 set
		 {
		   if(severityCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SeverityCode",OldValue=severityCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   severityCode=value;
		   }
			
		 }
	   }
	  private DateTime? ticketFirstResponseTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? TicketFirstResponseTime  
	   {
	    
	     get
		{
		   return ticketFirstResponseTime;
		 }
		 set
		 {
		   if(ticketFirstResponseTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketFirstResponseTime",OldValue=ticketFirstResponseTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   ticketFirstResponseTime=value;
		   }
			
		 }
	   }
	  private DateTime? ticketFirstResolveTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? TicketFirstResolveTime  
	   {
	    
	     get
		{
		   return ticketFirstResolveTime;
		 }
		 set
		 {
		   if(ticketFirstResolveTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketFirstResolveTime",OldValue=ticketFirstResolveTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   ticketFirstResolveTime=value;
		   }
			
		 }
	   }
	  private string source ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Source  
	   {
	    
	     get
		{
		   return source;
		 }
		 set
		 {
		   if(source != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Source",OldValue=source,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   source=value;
		   }
			
		 }
	   }
	  private string createdbyType ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedbyType  
	   {
	    
	     get
		{
		   return createdbyType;
		 }
		 set
		 {
		   if(createdbyType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedbyType",OldValue=createdbyType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdbyType=value;
		   }
			
		 }
	   }
	  private string sourceName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SourceName  
	   {
	    
	     get
		{
		   return sourceName;
		 }
		 set
		 {
		   if(sourceName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SourceName",OldValue=sourceName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sourceName=value;
		   }
			
		 }
	   }
	  private string createdbyTypeName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedbyTypeName  
	   {
	    
	     get
		{
		   return createdbyTypeName;
		 }
		 set
		 {
		   if(createdbyTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedbyTypeName",OldValue=createdbyTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdbyTypeName=value;
		   }
			
		 }
	   }
	  private DateTime? firstCloseDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FirstCloseDate  
	   {
	    
	     get
		{
		   return firstCloseDate;
		 }
		 set
		 {
		   if(firstCloseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstCloseDate",OldValue=firstCloseDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   firstCloseDate=value;
		   }
			
		 }
	   }
	  private DateTime? lastCloseDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastCloseDate  
	   {
	    
	     get
		{
		   return lastCloseDate;
		 }
		 set
		 {
		   if(lastCloseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastCloseDate",OldValue=lastCloseDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastCloseDate=value;
		   }
			
		 }
	   }
	  private DateTime? openDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? OpenDate  
	   {
	    
	     get
		{
		   return openDate;
		 }
		 set
		 {
		   if(openDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenDate",OldValue=openDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   openDate=value;
		   }
			
		 }
	   }
	  private int? openPeriodMinutes ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? OpenPeriodMinutes  
	   {
	    
	     get
		{
		   return openPeriodMinutes;
		 }
		 set
		 {
		   if(openPeriodMinutes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenPeriodMinutes",OldValue=openPeriodMinutes,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   openPeriodMinutes=value;
		   }
			
		 }
	   }
	  private bool isCreatedFromOutSide ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCreatedFromOutSide  
	   {
	    
	     get
		{
		   return isCreatedFromOutSide;
		 }
		 set
		 {
		   if(isCreatedFromOutSide != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCreatedFromOutSide",OldValue=isCreatedFromOutSide,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCreatedFromOutSide=value;
		   }
			
		 }
	   }
	  private bool isResolveDue ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsResolveDue  
	   {
	    
	     get
		{
		   return isResolveDue;
		 }
		 set
		 {
		   if(isResolveDue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsResolveDue",OldValue=isResolveDue,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isResolveDue=value;
		   }
			
		 }
	   }
	  private string resolveColor ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ResolveColor  
	   {
	    
	     get
		{
		   return resolveColor;
		 }
		 set
		 {
		   if(resolveColor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResolveColor",OldValue=resolveColor,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   resolveColor=value;
		   }
			
		 }
	   }
	  private bool isResolveExamination ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsResolveExamination  
	   {
	    
	     get
		{
		   return isResolveExamination;
		 }
		 set
		 {
		   if(isResolveExamination != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsResolveExamination",OldValue=isResolveExamination,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isResolveExamination=value;
		   }
			
		 }
	   }
	  private bool isResponseDue ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsResponseDue  
	   {
	    
	     get
		{
		   return isResponseDue;
		 }
		 set
		 {
		   if(isResponseDue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsResponseDue",OldValue=isResponseDue,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isResponseDue=value;
		   }
			
		 }
	   }
	  private string responseColor ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ResponseColor  
	   {
	    
	     get
		{
		   return responseColor;
		 }
		 set
		 {
		   if(responseColor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResponseColor",OldValue=responseColor,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   responseColor=value;
		   }
			
		 }
	   }
	  private bool isResponseExamination ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsResponseExamination  
	   {
	    
	     get
		{
		   return isResponseExamination;
		 }
		 set
		 {
		   if(isResponseExamination != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsResponseExamination",OldValue=isResponseExamination,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isResponseExamination=value;
		   }
			
		 }
	   }
	  private string companyTableName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CompanyTableName  
	   {
	    
	     get
		{
		   return companyTableName;
		 }
		 set
		 {
		   if(companyTableName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CompanyTableName",OldValue=companyTableName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   companyTableName=value;
		   }
			
		 }
	   }
	  private string rankName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string RankName  
	   {
	    
	     get
		{
		   return rankName;
		 }
		 set
		 {
		   if(rankName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RankName",OldValue=rankName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   rankName=value;
		   }
			
		 }
	   }
	  private string ownerEmail ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OwnerEmail  
	   {
	    
	     get
		{
		   return ownerEmail;
		 }
		 set
		 {
		   if(ownerEmail != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OwnerEmail",OldValue=ownerEmail,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ownerEmail=value;
		   }
			
		 }
	   }

	   private List<DocumentDataPM> ticketDocumentData;
	 
	   [DataMember]
	   public virtual List<DocumentDataPM> TicketDocumentData  
	   {
	        get
             {
                 if (ticketDocumentData == null)
                 {
                     ticketDocumentData = new List<DocumentDataPM>();
                 }
                 return ticketDocumentData;
              }
             set { ticketDocumentData = value; }
	    }
		   
	   private List<DocumentDataPM>  deletedTicketDocumentData;
	   public virtual List<DocumentDataPM> DeletedTicketDocumentData  
	   {
	        get
             {
                 if ( deletedTicketDocumentData == null)
                 {
                      deletedTicketDocumentData = new List<DocumentDataPM>();
                 }
                 return  deletedTicketDocumentData;
              }
             set {  deletedTicketDocumentData = value; }
	    }
	  
	   private List<CorrespondencePM> ticketCorrespondence;
	 
		     
	   [Include]
	   [Association("TicketCorrespondence", "Id","EntityId")]
	   [DataMember]
	   public virtual List<CorrespondencePM> TicketCorrespondence  
	   {
	        get
             {
                 if (ticketCorrespondence == null)
                 {
                     ticketCorrespondence = new List<CorrespondencePM>();
                 }
                 return ticketCorrespondence;
              }
             set { ticketCorrespondence = value; }
	    }
		   
	   private List<CorrespondencePM>  deletedTicketCorrespondence;
	   public virtual List<CorrespondencePM> DeletedTicketCorrespondence  
	   {
	        get
             {
                 if ( deletedTicketCorrespondence == null)
                 {
                      deletedTicketCorrespondence = new List<CorrespondencePM>();
                 }
                 return  deletedTicketCorrespondence;
              }
             set {  deletedTicketCorrespondence = value; }
	    }
	  	  private string employeeGroupName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EmployeeGroupName  
	   {
	    
	     get
		{
		   return employeeGroupName;
		 }
		 set
		 {
		   if(employeeGroupName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EmployeeGroupName",OldValue=employeeGroupName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   employeeGroupName=value;
		   }
			
		 }
	   }
	  private bool internalMode ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool InternalMode  
	   {
	    
	     get
		{
		   return internalMode;
		 }
		 set
		 {
		   if(internalMode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InternalMode",OldValue=internalMode,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   internalMode=value;
		   }
			
		 }
	   }
	  private string customerContactId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerContactId  
	   {
	    
	     get
		{
		   return customerContactId;
		 }
		 set
		 {
		   if(customerContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerContactId",OldValue=customerContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerContactId=value;
		   }
			
		 }
	   }
	  private string classificationManager ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClassificationManager  
	   {
	    
	     get
		{
		   return classificationManager;
		 }
		 set
		 {
		   if(classificationManager != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClassificationManager",OldValue=classificationManager,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   classificationManager=value;
		   }
			
		 }
	   }
	  private string classificationNotify ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClassificationNotify  
	   {
	    
	     get
		{
		   return classificationNotify;
		 }
		 set
		 {
		   if(classificationNotify != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClassificationNotify",OldValue=classificationNotify,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   classificationNotify=value;
		   }
			
		 }
	   }
	  private string groupNotify ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupNotify  
	   {
	    
	     get
		{
		   return groupNotify;
		 }
		 set
		 {
		   if(groupNotify != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupNotify",OldValue=groupNotify,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupNotify=value;
		   }
			
		 }
	   }
	  private string groupManager ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupManager  
	   {
	    
	     get
		{
		   return groupManager;
		 }
		 set
		 {
		   if(groupManager != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupManager",OldValue=groupManager,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupManager=value;
		   }
			
		 }
	   }
	  private string quoteId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteId  
	   {
	    
	     get
		{
		   return quoteId;
		 }
		 set
		 {
		   if(quoteId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteId",OldValue=quoteId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteId=value;
		   }
			
		 }
	   }
	  private string quoteNumber ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteNumber  
	   {
	    
	     get
		{
		   return quoteNumber;
		 }
		 set
		 {
		   if(quoteNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteNumber",OldValue=quoteNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteNumber=value;
		   }
			
		 }
	   }
	  private string contactTel ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactTel  
	   {
	    
	     get
		{
		   return contactTel;
		 }
		 set
		 {
		   if(contactTel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactTel",OldValue=contactTel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactTel=value;
		   }
			
		 }
	   }
	  private string sLAName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SLAName  
	   {
	    
	     get
		{
		   return sLAName;
		 }
		 set
		 {
		   if(sLAName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SLAName",OldValue=sLAName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sLAName=value;
		   }
			
		 }
	   }
	  private string sLAId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SLAId  
	   {
	    
	     get
		{
		   return sLAId;
		 }
		 set
		 {
		   if(sLAId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SLAId",OldValue=sLAId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sLAId=value;
		   }
			
		 }
	   }
	  private string entityType ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityType  
	   {
	    
	     get
		{
		   return entityType;
		 }
		 set
		 {
		   if(entityType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityType",OldValue=entityType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityType=value;
		   }
			
		 }
	   }
	  private string entityNumber ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityNumber  
	   {
	    
	     get
		{
		   return entityNumber;
		 }
		 set
		 {
		   if(entityNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityNumber",OldValue=entityNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityNumber=value;
		   }
			
		 }
	   }
   }
   
}
	 