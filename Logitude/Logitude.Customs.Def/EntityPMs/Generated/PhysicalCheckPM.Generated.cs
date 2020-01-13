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
   public partial class PhysicalCheckPM : EntityPM
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
	  private string storageSiteCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageSiteCode  
	   {
	    
	     get
		{
		   return storageSiteCode;
		 }
		 set
		 {
		   if(storageSiteCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageSiteCode",OldValue=storageSiteCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageSiteCode=value;
		   }
			
		 }
	   }
	  private string storageSiteName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageSiteName  
	   {
	    
	     get
		{
		   return storageSiteName;
		 }
		 set
		 {
		   if(storageSiteName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageSiteName",OldValue=storageSiteName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageSiteName=value;
		   }
			
		 }
	   }
	  private string checkSiteCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CheckSiteCode  
	   {
	    
	     get
		{
		   return checkSiteCode;
		 }
		 set
		 {
		   if(checkSiteCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CheckSiteCode",OldValue=checkSiteCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   checkSiteCode=value;
		   }
			
		 }
	   }
	  private string checkSiteName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CheckSiteName  
	   {
	    
	     get
		{
		   return checkSiteName;
		 }
		 set
		 {
		   if(checkSiteName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CheckSiteName",OldValue=checkSiteName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   checkSiteName=value;
		   }
			
		 }
	   }
	  private string queueTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string QueueTypeCode  
	   {
	    
	     get
		{
		   return queueTypeCode;
		 }
		 set
		 {
		   if(queueTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QueueTypeCode",OldValue=queueTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   queueTypeCode=value;
		   }
			
		 }
	   }
	  private string queueTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string QueueTypeName  
	   {
	    
	     get
		{
		   return queueTypeName;
		 }
		 set
		 {
		   if(queueTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QueueTypeName",OldValue=queueTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   queueTypeName=value;
		   }
			
		 }
	   }
	  private string operationCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OperationCode  
	   {
	    
	     get
		{
		   return operationCode;
		 }
		 set
		 {
		   if(operationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OperationCode",OldValue=operationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   operationCode=value;
		   }
			
		 }
	   }
	  private string checkId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CheckId  
	   {
	    
	     get
		{
		   return checkId;
		 }
		 set
		 {
		   if(checkId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CheckId",OldValue=checkId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   checkId=value;
		   }
			
		 }
	   }
	  private string entityTypeId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityTypeId  
	   {
	    
	     get
		{
		   return entityTypeId;
		 }
		 set
		 {
		   if(entityTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityTypeId",OldValue=entityTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityTypeId=value;
		   }
			
		 }
	   }
	  private string containerNubmer ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerNubmer  
	   {
	    
	     get
		{
		   return containerNubmer;
		 }
		 set
		 {
		   if(containerNubmer != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerNubmer",OldValue=containerNubmer,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerNubmer=value;
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
	  private DateTime? limitDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LimitDate  
	   {
	    
	     get
		{
		   return limitDate;
		 }
		 set
		 {
		   if(limitDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LimitDate",OldValue=limitDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   limitDate=value;
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
	  private string rowNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RowNumber  
	   {
	    
	     get
		{
		   return rowNumber;
		 }
		 set
		 {
		   if(rowNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RowNumber",OldValue=rowNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   rowNumber=value;
		   }
			
		 }
	   }
	  private string checkEssence ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CheckEssence  
	   {
	    
	     get
		{
		   return checkEssence;
		 }
		 set
		 {
		   if(checkEssence != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CheckEssence",OldValue=checkEssence,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   checkEssence=value;
		   }
			
		 }
	   }
	  private bool isClosed ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClosed  
	   {
	    
	     get
		{
		   return isClosed;
		 }
		 set
		 {
		   if(isClosed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosed",OldValue=isClosed,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClosed=value;
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
	  private string statusMessageCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusMessageCode  
	   {
	    
	     get
		{
		   return statusMessageCode;
		 }
		 set
		 {
		   if(statusMessageCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusMessageCode",OldValue=statusMessageCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusMessageCode=value;
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
	  private string initiatorTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InitiatorTypeCode  
	   {
	    
	     get
		{
		   return initiatorTypeCode;
		 }
		 set
		 {
		   if(initiatorTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InitiatorTypeCode",OldValue=initiatorTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   initiatorTypeCode=value;
		   }
			
		 }
	   }
	  private string importerNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterNumber  
	   {
	    
	     get
		{
		   return importerNumber;
		 }
		 set
		 {
		   if(importerNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterNumber",OldValue=importerNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerNumber=value;
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
	  private string operationName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OperationName  
	   {
	    
	     get
		{
		   return operationName;
		 }
		 set
		 {
		   if(operationName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OperationName",OldValue=operationName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   operationName=value;
		   }
			
		 }
	   }
	  private string declarationNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationNo  
	   {
	    
	     get
		{
		   return declarationNo;
		 }
		 set
		 {
		   if(declarationNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationNo",OldValue=declarationNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationNo=value;
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
	  private string checkSiteId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CheckSiteId  
	   {
	    
	     get
		{
		   return checkSiteId;
		 }
		 set
		 {
		   if(checkSiteId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CheckSiteId",OldValue=checkSiteId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   checkSiteId=value;
		   }
			
		 }
	   }
	  private string customerCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerCode  
	   {
	    
	     get
		{
		   return customerCode;
		 }
		 set
		 {
		   if(customerCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerCode",OldValue=customerCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerCode=value;
		   }
			
		 }
	   }
	  private string customerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerName  
	   {
	    
	     get
		{
		   return customerName;
		 }
		 set
		 {
		   if(customerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerName",OldValue=customerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerName=value;
		   }
			
		 }
	   }
	  private string customFileNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomFileNo  
	   {
	    
	     get
		{
		   return customFileNo;
		 }
		 set
		 {
		   if(customFileNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomFileNo",OldValue=customFileNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customFileNo=value;
		   }
			
		 }
	   }
	  private string statusMessageName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusMessageName  
	   {
	    
	     get
		{
		   return statusMessageName;
		 }
		 set
		 {
		   if(statusMessageName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusMessageName",OldValue=statusMessageName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusMessageName=value;
		   }
			
		 }
	   }
	  private string concurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConcurrencyGUID  
	   {
	    
	     get
		{
		   return concurrencyGUID;
		 }
		 set
		 {
		   if(concurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConcurrencyGUID",OldValue=concurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   concurrencyGUID=value;
		   }
			
		 }
	   }
	  private string newConcurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NewConcurrencyGUID  
	   {
	    
	     get
		{
		   return newConcurrencyGUID;
		 }
		 set
		 {
		   if(newConcurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NewConcurrencyGUID",OldValue=newConcurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   newConcurrencyGUID=value;
		   }
			
		 }
	   }
	  private bool isComprehensiveCheck ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsComprehensiveCheck  
	   {
	    
	     get
		{
		   return isComprehensiveCheck;
		 }
		 set
		 {
		   if(isComprehensiveCheck != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsComprehensiveCheck",OldValue=isComprehensiveCheck,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isComprehensiveCheck=value;
		   }
			
		 }
	   }
	  private string checkTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CheckTypeCode  
	   {
	    
	     get
		{
		   return checkTypeCode;
		 }
		 set
		 {
		   if(checkTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CheckTypeCode",OldValue=checkTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   checkTypeCode=value;
		   }
			
		 }
	   }
	  private string checkTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CheckTypeName  
	   {
	    
	     get
		{
		   return checkTypeName;
		 }
		 set
		 {
		   if(checkTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CheckTypeName",OldValue=checkTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   checkTypeName=value;
		   }
			
		 }
	   }
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerId  
	   {
	    
	     get
		{
		   return customerId;
		 }
		 set
		 {
		   if(customerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerId",OldValue=customerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerId=value;
		   }
			
		 }
	   }
	  private bool noEscortRequired ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool NoEscortRequired  
	   {
	    
	     get
		{
		   return noEscortRequired;
		 }
		 set
		 {
		   if(noEscortRequired != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NoEscortRequired",OldValue=noEscortRequired,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   noEscortRequired=value;
		   }
			
		 }
	   }
	  private string vehicleChassisNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VehicleChassisNumber  
	   {
	    
	     get
		{
		   return vehicleChassisNumber;
		 }
		 set
		 {
		   if(vehicleChassisNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleChassisNumber",OldValue=vehicleChassisNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vehicleChassisNumber=value;
		   }
			
		 }
	   }
   }
   
}
	 