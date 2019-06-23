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
   public partial class OccasionPM : EntityPM
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
	  private string name ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Name  
	   {
	    
	     get
		{
		   return name;
		 }
		 set
		 {
		   if(name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Name",OldValue=name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   name=value;
		   }
			
		 }
	   }
	  private DateTime? startDateTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartDateTime  
	   {
	    
	     get
		{
		   return startDateTime;
		 }
		 set
		 {
		   if(startDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDateTime",OldValue=startDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startDateTime=value;
		   }
			
		 }
	   }
	  private DateTime? endDateTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndDateTime  
	   {
	    
	     get
		{
		   return endDateTime;
		 }
		 set
		 {
		   if(endDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndDateTime",OldValue=endDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endDateTime=value;
		   }
			
		 }
	   }
	  private string goal ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Goal  
	   {
	    
	     get
		{
		   return goal;
		 }
		 set
		 {
		   if(goal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Goal",OldValue=goal,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   goal=value;
		   }
			
		 }
	   }
	  private string location ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Location  
	   {
	    
	     get
		{
		   return location;
		 }
		 set
		 {
		   if(location != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Location",OldValue=location,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   location=value;
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
	  private string industryId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string IndustryId  
	   {
	    
	     get
		{
		   return industryId;
		 }
		 set
		 {
		   if(industryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IndustryId",OldValue=industryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   industryId=value;
		   }
			
		 }
	   }
	  private string occasionTypeId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OccasionTypeId  
	   {
	    
	     get
		{
		   return occasionTypeId;
		 }
		 set
		 {
		   if(occasionTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OccasionTypeId",OldValue=occasionTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   occasionTypeId=value;
		   }
			
		 }
	   }
	  private string occasionStatusId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OccasionStatusId  
	   {
	    
	     get
		{
		   return occasionStatusId;
		 }
		 set
		 {
		   if(occasionStatusId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OccasionStatusId",OldValue=occasionStatusId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   occasionStatusId=value;
		   }
			
		 }
	   }
   }
   
}
	 