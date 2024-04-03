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
   public partial class LogisticActionRequestPM : EntityPM
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
	  private DateTime requestDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime RequestDate  
	   {
	    
	     get
		{
		   return requestDate;
		 }
		 set
		 {
		   if(requestDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestDate",OldValue=requestDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   requestDate=value;
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
	  private string exporterIdentifierType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterIdentifierType  
	   {
	    
	     get
		{
		   return exporterIdentifierType;
		 }
		 set
		 {
		   if(exporterIdentifierType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterIdentifierType",OldValue=exporterIdentifierType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterIdentifierType=value;
		   }
			
		 }
	   }
	  private string exporterNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterNumber  
	   {
	    
	     get
		{
		   return exporterNumber;
		 }
		 set
		 {
		   if(exporterNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterNumber",OldValue=exporterNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterNumber=value;
		   }
			
		 }
	   }
	  private string passportCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PassportCountry  
	   {
	    
	     get
		{
		   return passportCountry;
		 }
		 set
		 {
		   if(passportCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PassportCountry",OldValue=passportCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   passportCountry=value;
		   }
			
		 }
	   }
	  private string passportNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PassportNumber  
	   {
	    
	     get
		{
		   return passportNumber;
		 }
		 set
		 {
		   if(passportNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PassportNumber",OldValue=passportNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   passportNumber=value;
		   }
			
		 }
	   }
	  private string requestType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestType  
	   {
	    
	     get
		{
		   return requestType;
		 }
		 set
		 {
		   if(requestType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestType",OldValue=requestType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestType=value;
		   }
			
		 }
	   }
	  private string requestReason ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestReason  
	   {
	    
	     get
		{
		   return requestReason;
		 }
		 set
		 {
		   if(requestReason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestReason",OldValue=requestReason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestReason=value;
		   }
			
		 }
	   }
	  private string deliverySiteID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliverySiteID  
	   {
	    
	     get
		{
		   return deliverySiteID;
		 }
		 set
		 {
		   if(deliverySiteID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliverySiteID",OldValue=deliverySiteID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliverySiteID=value;
		   }
			
		 }
	   }
	  private string cargoIdentifierType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoIdentifierType  
	   {
	    
	     get
		{
		   return cargoIdentifierType;
		 }
		 set
		 {
		   if(cargoIdentifierType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoIdentifierType",OldValue=cargoIdentifierType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoIdentifierType=value;
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
	  private string packagingTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackagingTypeCode  
	   {
	    
	     get
		{
		   return packagingTypeCode;
		 }
		 set
		 {
		   if(packagingTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackagingTypeCode",OldValue=packagingTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packagingTypeCode=value;
		   }
			
		 }
	   }
	  private decimal quantity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal Quantity  
	   {
	    
	     get
		{
		   return quantity;
		 }
		 set
		 {
		   if(quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity",OldValue=quantity,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   quantity=value;
		   }
			
		 }
	   }
	  private string requestNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestNumber  
	   {
	    
	     get
		{
		   return requestNumber;
		 }
		 set
		 {
		   if(requestNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestNumber",OldValue=requestNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestNumber=value;
		   }
			
		 }
	   }
	  private string responseStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ResponseStatusCode  
	   {
	    
	     get
		{
		   return responseStatusCode;
		 }
		 set
		 {
		   if(responseStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResponseStatusCode",OldValue=responseStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   responseStatusCode=value;
		   }
			
		 }
	   }
	  private string operationalStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OperationalStatus  
	   {
	    
	     get
		{
		   return operationalStatus;
		 }
		 set
		 {
		   if(operationalStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OperationalStatus",OldValue=operationalStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   operationalStatus=value;
		   }
			
		 }
	   }
	  private string direction ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Direction  
	   {
	    
	     get
		{
		   return direction;
		 }
		 set
		 {
		   if(direction != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Direction",OldValue=direction,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   direction=value;
		   }
			
		 }
	   }
	  private string transportmodeId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportmodeId  
	   {
	    
	     get
		{
		   return transportmodeId;
		 }
		 set
		 {
		   if(transportmodeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportmodeId",OldValue=transportmodeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportmodeId=value;
		   }
			
		 }
	   }
	  private string decisionRmarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DecisionRmarks  
	   {
	    
	     get
		{
		   return decisionRmarks;
		 }
		 set
		 {
		   if(decisionRmarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DecisionRmarks",OldValue=decisionRmarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   decisionRmarks=value;
		   }
			
		 }
	   }
	  private string customsUserName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsUserName  
	   {
	    
	     get
		{
		   return customsUserName;
		 }
		 set
		 {
		   if(customsUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsUserName",OldValue=customsUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsUserName=value;
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
	  private string requestCancelStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestCancelStatus  
	   {
	    
	     get
		{
		   return requestCancelStatus;
		 }
		 set
		 {
		   if(requestCancelStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestCancelStatus",OldValue=requestCancelStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestCancelStatus=value;
		   }
			
		 }
	   }
	  private bool? haveDeclarationId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? HaveDeclarationId  
	   {
	    
	     get
		{
		   return haveDeclarationId;
		 }
		 set
		 {
		   if(haveDeclarationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HaveDeclarationId",OldValue=haveDeclarationId,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   haveDeclarationId=value;
		   }
			
		 }
	   }
	  private string calculatedExporterName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CalculatedExporterName  
	   {
	    
	     get
		{
		   return calculatedExporterName;
		 }
		 set
		 {
		   if(calculatedExporterName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculatedExporterName",OldValue=calculatedExporterName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   calculatedExporterName=value;
		   }
			
		 }
	   }
	  private string operationalStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OperationalStatusName  
	   {
	    
	     get
		{
		   return operationalStatusName;
		 }
		 set
		 {
		   if(operationalStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OperationalStatusName",OldValue=operationalStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   operationalStatusName=value;
		   }
			
		 }
	   }
	  private string responseStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ResponseStatus  
	   {
	    
	     get
		{
		   return responseStatus;
		 }
		 set
		 {
		   if(responseStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResponseStatus",OldValue=responseStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   responseStatus=value;
		   }
			
		 }
	   }
	    }
   
}
	 