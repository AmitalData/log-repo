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
using Logitude.WarehouseLib.BL.Validators;
  
namespace Logitude.WarehouseLib.BL.EntityPMs
{
   [CustomValidation(typeof(WarehouseClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class WarehouseEntryPackagesReleasePM : EntityPM
   {
   	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  private string entryPackageId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntryPackageId  
	   {
	    
	     get
		{
		   return entryPackageId;
		 }
		 set
		 {
		   if(entryPackageId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntryPackageId",OldValue=entryPackageId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entryPackageId=value;
		   }
			
		 }
	   }
	  private string releasePackageId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReleasePackageId  
	   {
	    
	     get
		{
		   return releasePackageId;
		 }
		 set
		 {
		   if(releasePackageId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReleasePackageId",OldValue=releasePackageId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   releasePackageId=value;
		   }
			
		 }
	   }
	  private int quantity ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public int Quantity  
	   {
	    
	     get
		{
		   return quantity;
		 }
		 set
		 {
		   if(quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity",OldValue=quantity,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   quantity=value;
		   }
			
		 }
	   }
	  private bool isCanceled ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCanceled  
	   {
	    
	     get
		{
		   return isCanceled;
		 }
		 set
		 {
		   if(isCanceled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCanceled",OldValue=isCanceled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCanceled=value;
		   }
			
		 }
	   }
   }
   
}
	 