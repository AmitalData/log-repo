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
   public partial class ExportStoragePM : EntityPM
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string exportFileNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportFileNo  
	   {
	    
	     get
		{
		   return exportFileNo;
		 }
		 set
		 {
		   if(exportFileNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportFileNo",OldValue=exportFileNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportFileNo=value;
		   }
			
		 }
	   }
	  private string storageStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageStatus  
	   {
	    
	     get
		{
		   return storageStatus;
		 }
		 set
		 {
		   if(storageStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageStatus",OldValue=storageStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageStatus=value;
		   }
			
		 }
	   }
	  private string cargoTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoTypeCode  
	   {
	    
	     get
		{
		   return cargoTypeCode;
		 }
		 set
		 {
		   if(cargoTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoTypeCode",OldValue=cargoTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoTypeCode=value;
		   }
			
		 }
	   }
	  private DateTime openDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime OpenDate  
	   {
	    
	     get
		{
		   return openDate;
		 }
		 set
		 {
		   if(openDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenDate",OldValue=openDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   openDate=value;
		   }
			
		 }
	   }
	  private string cargoType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoType  
	   {
	    
	     get
		{
		   return cargoType;
		 }
		 set
		 {
		   if(cargoType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoType",OldValue=cargoType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoType=value;
		   }
			
		 }
	   }
	  private string customsStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsStatus  
	   {
	    
	     get
		{
		   return customsStatus;
		 }
		 set
		 {
		   if(customsStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsStatus",OldValue=customsStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsStatus=value;
		   }
			
		 }
	   }
	  private string exporterID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterID  
	   {
	    
	     get
		{
		   return exporterID;
		 }
		 set
		 {
		   if(exporterID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterID",OldValue=exporterID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterID=value;
		   }
			
		 }
	   }
	  private string shipCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipCode  
	   {
	    
	     get
		{
		   return shipCode;
		 }
		 set
		 {
		   if(shipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipCode",OldValue=shipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipCode=value;
		   }
			
		 }
	   }
	  private string firstCargoID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FirstCargoID  
	   {
	    
	     get
		{
		   return firstCargoID;
		 }
		 set
		 {
		   if(firstCargoID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstCargoID",OldValue=firstCargoID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   firstCargoID=value;
		   }
			
		 }
	   }
	  private string secondCargoID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SecondCargoID  
	   {
	    
	     get
		{
		   return secondCargoID;
		 }
		 set
		 {
		   if(secondCargoID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SecondCargoID",OldValue=secondCargoID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   secondCargoID=value;
		   }
			
		 }
	   }
	  private string thirdCargoID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ThirdCargoID  
	   {
	    
	     get
		{
		   return thirdCargoID;
		 }
		 set
		 {
		   if(thirdCargoID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ThirdCargoID",OldValue=thirdCargoID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   thirdCargoID=value;
		   }
			
		 }
	   }
	  private string declarationStatusTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationStatusTypeName  
	   {
	    
	     get
		{
		   return declarationStatusTypeName;
		 }
		 set
		 {
		   if(declarationStatusTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationStatusTypeName",OldValue=declarationStatusTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationStatusTypeName=value;
		   }
			
		 }
	   }
	  private string cargoTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoTypeName  
	   {
	    
	     get
		{
		   return cargoTypeName;
		 }
		 set
		 {
		   if(cargoTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoTypeName",OldValue=cargoTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoTypeName=value;
		   }
			
		 }
	   }
	  private string customStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomStatusName  
	   {
	    
	     get
		{
		   return customStatusName;
		 }
		 set
		 {
		   if(customStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomStatusName",OldValue=customStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customStatusName=value;
		   }
			
		 }
	   }
	  private string exporterName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterName  
	   {
	    
	     get
		{
		   return exporterName;
		 }
		 set
		 {
		   if(exporterName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterName",OldValue=exporterName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterName=value;
		   }
			
		 }
	   }
	  private string shipName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipName  
	   {
	    
	     get
		{
		   return shipName;
		 }
		 set
		 {
		   if(shipName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipName",OldValue=shipName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipName=value;
		   }
			
		 }
	   }
	  private string storErrorXML ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorErrorXML  
	   {
	    
	     get
		{
		   return storErrorXML;
		 }
		 set
		 {
		   if(storErrorXML != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorErrorXML",OldValue=storErrorXML,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storErrorXML=value;
		   }
			
		 }
	   }
	  private string storageNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageNo  
	   {
	    
	     get
		{
		   return storageNo;
		 }
		 set
		 {
		   if(storageNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageNo",OldValue=storageNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageNo=value;
		   }
			
		 }
	   }
	  private string exportDealIdentification ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportDealIdentification  
	   {
	    
	     get
		{
		   return exportDealIdentification;
		 }
		 set
		 {
		   if(exportDealIdentification != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportDealIdentification",OldValue=exportDealIdentification,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportDealIdentification=value;
		   }
			
		 }
	   }
	  private string cargoTypeCodeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoTypeCodeName  
	   {
	    
	     get
		{
		   return cargoTypeCodeName;
		 }
		 set
		 {
		   if(cargoTypeCodeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoTypeCodeName",OldValue=cargoTypeCodeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoTypeCodeName=value;
		   }
			
		 }
	   }
	  private string declarationStatusTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationStatusTypeCode  
	   {
	    
	     get
		{
		   return declarationStatusTypeCode;
		 }
		 set
		 {
		   if(declarationStatusTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationStatusTypeCode",OldValue=declarationStatusTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationStatusTypeCode=value;
		   }
			
		 }
	   }
	  private string declaration_ID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Declaration_ID  
	   {
	    
	     get
		{
		   return declaration_ID;
		 }
		 set
		 {
		   if(declaration_ID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Declaration_ID",OldValue=declaration_ID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declaration_ID=value;
		   }
			
		 }
	   }
	  private string declarationCustomFileNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationCustomFileNo  
	   {
	    
	     get
		{
		   return declarationCustomFileNo;
		 }
		 set
		 {
		   if(declarationCustomFileNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationCustomFileNo",OldValue=declarationCustomFileNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationCustomFileNo=value;
		   }
			
		 }
	   }
	  private string declarationNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationNumber  
	   {
	    
	     get
		{
		   return declarationNumber;
		 }
		 set
		 {
		   if(declarationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationNumber",OldValue=declarationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationNumber=value;
		   }
			
		 }
	   }
	  private string exporterCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterCode  
	   {
	    
	     get
		{
		   return exporterCode;
		 }
		 set
		 {
		   if(exporterCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterCode",OldValue=exporterCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterCode=value;
		   }
			
		 }
	   }
   }
   
}
	 