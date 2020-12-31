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
using Logitude.TariffModule.BL.Validators;
  
namespace Logitude.TariffModule.BL.EntityPMs
{
   [CustomValidation(typeof(TariffModuleClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TariffVersionUploadedExcelPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private DateTime uploadDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime UploadDate  
	   {
	    
	     get
		{
		   return uploadDate;
		 }
		 set
		 {
		   if(uploadDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UploadDate",OldValue=uploadDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   uploadDate=value;
		   }
			
		 }
	   }
	  private string uploadedByUserId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string UploadedByUserId  
	   {
	    
	     get
		{
		   return uploadedByUserId;
		 }
		 set
		 {
		   if(uploadedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UploadedByUserId",OldValue=uploadedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   uploadedByUserId=value;
		   }
			
		 }
	   }
	  private string tariffId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffId  
	   {
	    
	     get
		{
		   return tariffId;
		 }
		 set
		 {
		   if(tariffId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffId",OldValue=tariffId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffId=value;
		   }
			
		 }
	   }
	  private int version ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int Version  
	   {
	    
	     get
		{
		   return version;
		 }
		 set
		 {
		   if(version != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Version",OldValue=version,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   version=value;
		   }
			
		 }
	   }
	  private string documentId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DocumentId  
	   {
	    
	     get
		{
		   return documentId;
		 }
		 set
		 {
		   if(documentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentId",OldValue=documentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   documentId=value;
		   }
			
		 }
	   }
	  private int numberOfLines ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int NumberOfLines  
	   {
	    
	     get
		{
		   return numberOfLines;
		 }
		 set
		 {
		   if(numberOfLines != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfLines",OldValue=numberOfLines,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   numberOfLines=value;
		   }
			
		 }
	   }
	  private int index ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int Index  
	   {
	    
	     get
		{
		   return index;
		 }
		 set
		 {
		   if(index != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Index",OldValue=index,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   index=value;
		   }
			
		 }
	   }
	  private string uploadedByUserName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string UploadedByUserName  
	   {
	    
	     get
		{
		   return uploadedByUserName;
		 }
		 set
		 {
		   if(uploadedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UploadedByUserName",OldValue=uploadedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   uploadedByUserName=value;
		   }
			
		 }
	   }
	  private string fileName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string FileName  
	   {
	    
	     get
		{
		   return fileName;
		 }
		 set
		 {
		   if(fileName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileName",OldValue=fileName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fileName=value;
		   }
			
		 }
	   }
   }
   
}
	 