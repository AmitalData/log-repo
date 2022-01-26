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
   public partial class ExportDeclarationClosingDataPM : EntityPM
   {
   	  private string finalThirdCargoId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalThirdCargoId  
	   {
	    
	     get
		{
		   return finalThirdCargoId;
		 }
		 set
		 {
		   if(finalThirdCargoId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalThirdCargoId",OldValue=finalThirdCargoId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalThirdCargoId=value;
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
	  
       [Key]
	  
       
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
	  private string finalCargoTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalCargoTypeCode  
	   {
	    
	     get
		{
		   return finalCargoTypeCode;
		 }
		 set
		 {
		   if(finalCargoTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalCargoTypeCode",OldValue=finalCargoTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalCargoTypeCode=value;
		   }
			
		 }
	   }
	  private string finalManifestNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalManifestNumber  
	   {
	    
	     get
		{
		   return finalManifestNumber;
		 }
		 set
		 {
		   if(finalManifestNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalManifestNumber",OldValue=finalManifestNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalManifestNumber=value;
		   }
			
		 }
	   }
	  private string finalSecondCargoId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalSecondCargoId  
	   {
	    
	     get
		{
		   return finalSecondCargoId;
		 }
		 set
		 {
		   if(finalSecondCargoId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalSecondCargoId",OldValue=finalSecondCargoId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalSecondCargoId=value;
		   }
			
		 }
	   }
	  private DateTime? loadingDateTime ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LoadingDateTime  
	   {
	    
	     get
		{
		   return loadingDateTime;
		 }
		 set
		 {
		   if(loadingDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LoadingDateTime",OldValue=loadingDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   loadingDateTime=value;
		   }
			
		 }
	   }
	  private string finalShipCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalShipCode  
	   {
	    
	     get
		{
		   return finalShipCode;
		 }
		 set
		 {
		   if(finalShipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalShipCode",OldValue=finalShipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalShipCode=value;
		   }
			
		 }
	   }
	  private string finalLoadingSite ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalLoadingSite  
	   {
	    
	     get
		{
		   return finalLoadingSite;
		 }
		 set
		 {
		   if(finalLoadingSite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalLoadingSite",OldValue=finalLoadingSite,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalLoadingSite=value;
		   }
			
		 }
	   }
	  private string finalLoadingSiteName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalLoadingSiteName  
	   {
	    
	     get
		{
		   return finalLoadingSiteName;
		 }
		 set
		 {
		   if(finalLoadingSiteName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalLoadingSiteName",OldValue=finalLoadingSiteName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalLoadingSiteName=value;
		   }
			
		 }
	   }
	  private string finalCargoTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalCargoTypeName  
	   {
	    
	     get
		{
		   return finalCargoTypeName;
		 }
		 set
		 {
		   if(finalCargoTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalCargoTypeName",OldValue=finalCargoTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalCargoTypeName=value;
		   }
			
		 }
	   }
	  private string finalShipCodeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalShipCodeName  
	   {
	    
	     get
		{
		   return finalShipCodeName;
		 }
		 set
		 {
		   if(finalShipCodeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalShipCodeName",OldValue=finalShipCodeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalShipCodeName=value;
		   }
			
		 }
	   }
	  private string mAIN_AWB ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAIN_AWB  
	   {
	    
	     get
		{
		   return mAIN_AWB;
		 }
		 set
		 {
		   if(mAIN_AWB != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAIN_AWB",OldValue=mAIN_AWB,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAIN_AWB=value;
		   }
			
		 }
	   }
	  private string sMP ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SMP  
	   {
	    
	     get
		{
		   return sMP;
		 }
		 set
		 {
		   if(sMP != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SMP",OldValue=sMP,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sMP=value;
		   }
			
		 }
	   }
	  private DateTime? fLIGHT_DATE ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FLIGHT_DATE  
	   {
	    
	     get
		{
		   return fLIGHT_DATE;
		 }
		 set
		 {
		   if(fLIGHT_DATE != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FLIGHT_DATE",OldValue=fLIGHT_DATE,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fLIGHT_DATE=value;
		   }
			
		 }
	   }
   }
   
}
	 