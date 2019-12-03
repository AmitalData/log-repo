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
   public partial class BIReportPM : EntityPM
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
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private string name ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private string description ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string Description  
	   {
	    
	     get
		{
		   return description;
		 }
		 set
		 {
		   if(description != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Description",OldValue=description,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   description=value;
		   }
			
		 }
	   }
	  private string dWQueryId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string DWQueryId  
	   {
	    
	     get
		{
		   return dWQueryId;
		 }
		 set
		 {
		   if(dWQueryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DWQueryId",OldValue=dWQueryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dWQueryId=value;
		   }
			
		 }
	   }
	  private bool inactive ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Inactive  
	   {
	    
	     get
		{
		   return inactive;
		 }
		 set
		 {
		   if(inactive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Inactive",OldValue=inactive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   inactive=value;
		   }
			
		 }
	   }
	  private string typeCode ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeCode  
	   {
	    
	     get
		{
		   return typeCode;
		 }
		 set
		 {
		   if(typeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeCode",OldValue=typeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeCode=value;
		   }
			
		 }
	   }
	  private string aGGridOptionsXML ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string AGGridOptionsXML  
	   {
	    
	     get
		{
		   return aGGridOptionsXML;
		 }
		 set
		 {
		   if(aGGridOptionsXML != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AGGridOptionsXML",OldValue=aGGridOptionsXML,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aGGridOptionsXML=value;
		   }
			
		 }
	   }
	  private string bIReportFolderId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string BIReportFolderId  
	   {
	    
	     get
		{
		   return bIReportFolderId;
		 }
		 set
		 {
		   if(bIReportFolderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BIReportFolderId",OldValue=bIReportFolderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bIReportFolderId=value;
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
	  private string updatedByUserName ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private DateTime lastRunDate ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime LastRunDate  
	   {
	    
	     get
		{
		   return lastRunDate;
		 }
		 set
		 {
		   if(lastRunDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastRunDate",OldValue=lastRunDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   lastRunDate=value;
		   }
			
		 }
	   }
	  private string lastRunByUserId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastRunByUserId  
	   {
	    
	     get
		{
		   return lastRunByUserId;
		 }
		 set
		 {
		   if(lastRunByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastRunByUserId",OldValue=lastRunByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastRunByUserId=value;
		   }
			
		 }
	   }
	  private string lastRunByUserName ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastRunByUserName  
	   {
	    
	     get
		{
		   return lastRunByUserName;
		 }
		 set
		 {
		   if(lastRunByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastRunByUserName",OldValue=lastRunByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastRunByUserName=value;
		   }
			
		 }
	   }
   }
   
}
	 