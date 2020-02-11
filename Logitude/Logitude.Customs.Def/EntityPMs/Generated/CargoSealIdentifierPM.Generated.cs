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
using Logitude.Customs.Def.Validators;
  
namespace Logitude.Customs.Def.EntityPMs
{
   [CustomValidation(typeof(CustomsClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class CargoSealIdentifierPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string declarationId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationId  
	   {
	    
	     get
		{
		   return declarationId;
		 }
		 set
		 {
		   if(declarationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationId",OldValue=declarationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationId=value;
		   }
			
		 }
	   }
	  private string cargoRowNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoRowNumber  
	   {
	    
	     get
		{
		   return cargoRowNumber;
		 }
		 set
		 {
		   if(cargoRowNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoRowNumber",OldValue=cargoRowNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoRowNumber=value;
		   }
			
		 }
	   }
	  private string containerNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerNumber  
	   {
	    
	     get
		{
		   return containerNumber;
		 }
		 set
		 {
		   if(containerNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerNumber",OldValue=containerNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerNumber=value;
		   }
			
		 }
	   }
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string importerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterId  
	   {
	    
	     get
		{
		   return importerId;
		 }
		 set
		 {
		   if(importerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterId",OldValue=importerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerId=value;
		   }
			
		 }
	   }
	  private string cargoIdentifierTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoIdentifierTypeCode  
	   {
	    
	     get
		{
		   return cargoIdentifierTypeCode;
		 }
		 set
		 {
		   if(cargoIdentifierTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoIdentifierTypeCode",OldValue=cargoIdentifierTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoIdentifierTypeCode=value;
		   }
			
		 }
	   }
	  private string cargoIdentifierTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoIdentifierTypeName  
	   {
	    
	     get
		{
		   return cargoIdentifierTypeName;
		 }
		 set
		 {
		   if(cargoIdentifierTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoIdentifierTypeName",OldValue=cargoIdentifierTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoIdentifierTypeName=value;
		   }
			
		 }
	   }
	  private string cargoIdentifierKey1 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoIdentifierKey1  
	   {
	    
	     get
		{
		   return cargoIdentifierKey1;
		 }
		 set
		 {
		   if(cargoIdentifierKey1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoIdentifierKey1",OldValue=cargoIdentifierKey1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoIdentifierKey1=value;
		   }
			
		 }
	   }
	  private string cargoIdentifierKey2 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoIdentifierKey2  
	   {
	    
	     get
		{
		   return cargoIdentifierKey2;
		 }
		 set
		 {
		   if(cargoIdentifierKey2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoIdentifierKey2",OldValue=cargoIdentifierKey2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoIdentifierKey2=value;
		   }
			
		 }
	   }
	  private string cargoIdentifierKey3 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoIdentifierKey3  
	   {
	    
	     get
		{
		   return cargoIdentifierKey3;
		 }
		 set
		 {
		   if(cargoIdentifierKey3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoIdentifierKey3",OldValue=cargoIdentifierKey3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoIdentifierKey3=value;
		   }
			
		 }
	   }
	  private string status ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Status  
	   {
	    
	     get
		{
		   return status;
		 }
		 set
		 {
		   if(status != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Status",OldValue=status,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   status=value;
		   }
			
		 }
	   }

	   private List<CargoSealPM> cargoSeals;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("CargoSeals", "Id","CargoSealIdentifierId")]
	   [DataMember]
	   public virtual List<CargoSealPM> CargoSeals  
	   {
	        get
             {
                 if (cargoSeals == null)
                 {
                     cargoSeals = new List<CargoSealPM>();
                 }
                 return cargoSeals;
              }
             set { cargoSeals = value; }
	    }
		   
	   private List<CargoSealPM>  deletedCargoSeals;
	   public virtual List<CargoSealPM> DeletedCargoSeals  
	   {
	        get
             {
                 if ( deletedCargoSeals == null)
                 {
                      deletedCargoSeals = new List<CargoSealPM>();
                 }
                 return  deletedCargoSeals;
              }
             set {  deletedCargoSeals = value; }
	    }
	     }
   
}
	 