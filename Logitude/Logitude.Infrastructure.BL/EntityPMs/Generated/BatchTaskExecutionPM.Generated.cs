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
using Logitude.Infrastructure.BL.Validators;
  
namespace Logitude.Infrastructure.BL.EntityPMs
{
   [CustomValidation(typeof(InfrastructureClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class BatchTaskExecutionPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private string className ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClassName  
	   {
	    
	     get
		{
		   return className;
		 }
		 set
		 {
		   if(className != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClassName",OldValue=className,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   className=value;
		   }
			
		 }
	   }
	  private string prametersXml ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string PrametersXml  
	   {
	    
	     get
		{
		   return prametersXml;
		 }
		 set
		 {
		   if(prametersXml != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PrametersXml",OldValue=prametersXml,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   prametersXml=value;
		   }
			
		 }
	   }
	  private string statusCode ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusCode  
	   {
	    
	     get
		{
		   return statusCode;
		 }
		 set
		 {
		   if(statusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusCode",OldValue=statusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusCode=value;
		   }
			
		 }
	   }
	  private string errorLog ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string ErrorLog  
	   {
	    
	     get
		{
		   return errorLog;
		 }
		 set
		 {
		   if(errorLog != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ErrorLog",OldValue=errorLog,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   errorLog=value;
		   }
			
		 }
	   }
	  private DateTime? startDateTime ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private DateTime? doneDateTime ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DoneDateTime  
	   {
	    
	     get
		{
		   return doneDateTime;
		 }
		 set
		 {
		   if(doneDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DoneDateTime",OldValue=doneDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   doneDateTime=value;
		   }
			
		 }
	   }
	  private string progressMessage ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProgressMessage  
	   {
	    
	     get
		{
		   return progressMessage;
		 }
		 set
		 {
		   if(progressMessage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProgressMessage",OldValue=progressMessage,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   progressMessage=value;
		   }
			
		 }
	   }
	  private int progressPercentage ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public int ProgressPercentage  
	   {
	    
	     get
		{
		   return progressPercentage;
		 }
		 set
		 {
		   if(progressPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProgressPercentage",OldValue=progressPercentage,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   progressPercentage=value;
		   }
			
		 }
	   }
	  private string statusName ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusName  
	   {
	    
	     get
		{
		   return statusName;
		 }
		 set
		 {
		   if(statusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusName",OldValue=statusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusName=value;
		   }
			
		 }
	   }
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserName  
	   {
	    
	     get
		{
		   return createdByUserName;
		 }
		 set
		 {
		   if(createdByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserName",OldValue=createdByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserName=value;
		   }
			
		 }
	   }
	  private string subject ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private string callStack ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string CallStack  
	   {
	    
	     get
		{
		   return callStack;
		 }
		 set
		 {
		   if(callStack != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CallStack",OldValue=callStack,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   callStack=value;
		   }
			
		 }
	   }
   }
   
}
	 