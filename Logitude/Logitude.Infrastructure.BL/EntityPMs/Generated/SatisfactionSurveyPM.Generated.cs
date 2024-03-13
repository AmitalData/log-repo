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
   public partial class SatisfactionSurveyPM : EntityPM
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
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private string rating ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string Rating  
	   {
	    
	     get
		{
		   return rating;
		 }
		 set
		 {
		   if(rating != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Rating",OldValue=rating,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   rating=value;
		   }
			
		 }
	   }
	  private string comments ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string Comments  
	   {
	    
	     get
		{
		   return comments;
		 }
		 set
		 {
		   if(comments != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Comments",OldValue=comments,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   comments=value;
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
	  private string hash ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string Hash  
	   {
	    
	     get
		{
		   return hash;
		 }
		 set
		 {
		   if(hash != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Hash",OldValue=hash,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   hash=value;
		   }
			
		 }
	   }
	  private bool isSync ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSync  
	   {
	    
	     get
		{
		   return isSync;
		 }
		 set
		 {
		   if(isSync != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSync",OldValue=isSync,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSync=value;
		   }
			
		 }
	   }
	  private string lockedBy ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string LockedBy  
	   {
	    
	     get
		{
		   return lockedBy;
		 }
		 set
		 {
		   if(lockedBy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LockedBy",OldValue=lockedBy,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lockedBy=value;
		   }
			
		 }
	   }
	  private DateTime? syncDate ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? SyncDate  
	   {
	    
	     get
		{
		   return syncDate;
		 }
		 set
		 {
		   if(syncDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SyncDate",OldValue=syncDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   syncDate=value;
		   }
			
		 }
	   }
	    }
   
}
	 