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
   public partial class DecCargoSplitCargoIdentifierPM : EntityPM
   {
   	  private string declarationCargoSplitId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationCargoSplitId  
	   {
	    
	     get
		{
		   return declarationCargoSplitId;
		 }
		 set
		 {
		   if(declarationCargoSplitId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationCargoSplitId",OldValue=declarationCargoSplitId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationCargoSplitId=value;
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
	  private int lineNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int LineNumber  
	   {
	    
	     get
		{
		   return lineNumber;
		 }
		 set
		 {
		   if(lineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineNumber",OldValue=lineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   lineNumber=value;
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
   }
   
}
	 