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
   public partial class OccasionInviteePM : EntityPM
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
	  private DateTime addedDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime AddedDate  
	   {
	    
	     get
		{
		   return addedDate;
		 }
		 set
		 {
		   if(addedDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AddedDate",OldValue=addedDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   addedDate=value;
		   }
			
		 }
	   }
	  private string addedByUserId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string AddedByUserId  
	   {
	    
	     get
		{
		   return addedByUserId;
		 }
		 set
		 {
		   if(addedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AddedByUserId",OldValue=addedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   addedByUserId=value;
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
	  private string occasionId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OccasionId  
	   {
	    
	     get
		{
		   return occasionId;
		 }
		 set
		 {
		   if(occasionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OccasionId",OldValue=occasionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   occasionId=value;
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
	  private string addedByUserName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string AddedByUserName  
	   {
	    
	     get
		{
		   return addedByUserName;
		 }
		 set
		 {
		   if(addedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AddedByUserName",OldValue=addedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   addedByUserName=value;
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
	  private string occasionName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OccasionName  
	   {
	    
	     get
		{
		   return occasionName;
		 }
		 set
		 {
		   if(occasionName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OccasionName",OldValue=occasionName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   occasionName=value;
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
	  private bool invited ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Invited  
	   {
	    
	     get
		{
		   return invited;
		 }
		 set
		 {
		   if(invited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Invited",OldValue=invited,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   invited=value;
		   }
			
		 }
	   }
	  private bool participated ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Participated  
	   {
	    
	     get
		{
		   return participated;
		 }
		 set
		 {
		   if(participated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Participated",OldValue=participated,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   participated=value;
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
	  private string contactPosition ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactPosition  
	   {
	    
	     get
		{
		   return contactPosition;
		 }
		 set
		 {
		   if(contactPosition != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactPosition",OldValue=contactPosition,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactPosition=value;
		   }
			
		 }
	   }
	  private string contactMobile ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContactMobile  
	   {
	    
	     get
		{
		   return contactMobile;
		 }
		 set
		 {
		   if(contactMobile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContactMobile",OldValue=contactMobile,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contactMobile=value;
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
   }
   
}
	 