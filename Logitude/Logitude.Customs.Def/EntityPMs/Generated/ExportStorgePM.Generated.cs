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
   public partial class ExportStorgePM : EntityPM
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
	  private string orderNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OrderNo  
	   {
	    
	     get
		{
		   return orderNo;
		 }
		 set
		 {
		   if(orderNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OrderNo",OldValue=orderNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   orderNo=value;
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
	  private string fclLcl ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FclLcl  
	   {
	    
	     get
		{
		   return fclLcl;
		 }
		 set
		 {
		   if(fclLcl != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FclLcl",OldValue=fclLcl,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fclLcl=value;
		   }
			
		 }
	   }
	  private string fclLclName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FclLclName  
	   {
	    
	     get
		{
		   return fclLclName;
		 }
		 set
		 {
		   if(fclLclName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FclLclName",OldValue=fclLclName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fclLclName=value;
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
	  private string transportModeId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeId  
	   {
	    
	     get
		{
		   return transportModeId;
		 }
		 set
		 {
		   if(transportModeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeId",OldValue=transportModeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeId=value;
		   }
			
		 }
	   }
	  private string transportModeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeName  
	   {
	    
	     get
		{
		   return transportModeName;
		 }
		 set
		 {
		   if(transportModeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeName",OldValue=transportModeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeName=value;
		   }
			
		 }
	   }
	  private int storageNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int StorageNo  
	   {
	    
	     get
		{
		   return storageNo;
		 }
		 set
		 {
		   if(storageNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageNo",OldValue=storageNo,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   storageNo=value;
		   }
			
		 }
	   }
	  private double voyageNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public double VoyageNo  
	   {
	    
	     get
		{
		   return voyageNo;
		 }
		 set
		 {
		   if(voyageNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VoyageNo",OldValue=voyageNo,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   voyageNo=value;
		   }
			
		 }
	   }
	  private DateTime storageDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime StorageDate  
	   {
	    
	     get
		{
		   return storageDate;
		 }
		 set
		 {
		   if(storageDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageDate",OldValue=storageDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   storageDate=value;
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
	  private bool isOpenStoarge ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsOpenStoarge  
	   {
	    
	     get
		{
		   return isOpenStoarge;
		 }
		 set
		 {
		   if(isOpenStoarge != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsOpenStoarge",OldValue=isOpenStoarge,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isOpenStoarge=value;
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
	  private string senderCodeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SenderCodeID  
	   {
	    
	     get
		{
		   return senderCodeID;
		 }
		 set
		 {
		   if(senderCodeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SenderCodeID",OldValue=senderCodeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   senderCodeID=value;
		   }
			
		 }
	   }
	  private int messageFromForm ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int MessageFromForm  
	   {
	    
	     get
		{
		   return messageFromForm;
		 }
		 set
		 {
		   if(messageFromForm != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MessageFromForm",OldValue=messageFromForm,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   messageFromForm=value;
		   }
			
		 }
	   }
	  private double replyPhoneNumeric ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public double ReplyPhoneNumeric  
	   {
	    
	     get
		{
		   return replyPhoneNumeric;
		 }
		 set
		 {
		   if(replyPhoneNumeric != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReplyPhoneNumeric",OldValue=replyPhoneNumeric,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   replyPhoneNumeric=value;
		   }
			
		 }
	   }
	  private double operatorID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public double OperatorID  
	   {
	    
	     get
		{
		   return operatorID;
		 }
		 set
		 {
		   if(operatorID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OperatorID",OldValue=operatorID,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   operatorID=value;
		   }
			
		 }
	   }
	  private string informedParty ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InformedParty  
	   {
	    
	     get
		{
		   return informedParty;
		 }
		 set
		 {
		   if(informedParty != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InformedParty",OldValue=informedParty,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   informedParty=value;
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
	  private int declarationsInContainer ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int DeclarationsInContainer  
	   {
	    
	     get
		{
		   return declarationsInContainer;
		 }
		 set
		 {
		   if(declarationsInContainer != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationsInContainer",OldValue=declarationsInContainer,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   declarationsInContainer=value;
		   }
			
		 }
	   }
	  private int exportManifestNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ExportManifestNumber  
	   {
	    
	     get
		{
		   return exportManifestNumber;
		 }
		 set
		 {
		   if(exportManifestNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportManifestNumber",OldValue=exportManifestNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   exportManifestNumber=value;
		   }
			
		 }
	   }
	  private string receivingSite ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReceivingSite  
	   {
	    
	     get
		{
		   return receivingSite;
		 }
		 set
		 {
		   if(receivingSite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReceivingSite",OldValue=receivingSite,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   receivingSite=value;
		   }
			
		 }
	   }
	  private string stuffingSiteType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StuffingSiteType  
	   {
	    
	     get
		{
		   return stuffingSiteType;
		 }
		 set
		 {
		   if(stuffingSiteType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StuffingSiteType",OldValue=stuffingSiteType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   stuffingSiteType=value;
		   }
			
		 }
	   }
	  private string loadingSite ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LoadingSite  
	   {
	    
	     get
		{
		   return loadingSite;
		 }
		 set
		 {
		   if(loadingSite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LoadingSite",OldValue=loadingSite,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   loadingSite=value;
		   }
			
		 }
	   }
	  private string forwarderReference ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForwarderReference  
	   {
	    
	     get
		{
		   return forwarderReference;
		 }
		 set
		 {
		   if(forwarderReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForwarderReference",OldValue=forwarderReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   forwarderReference=value;
		   }
			
		 }
	   }
	  private decimal transactionQuantity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal TransactionQuantity  
	   {
	    
	     get
		{
		   return transactionQuantity;
		 }
		 set
		 {
		   if(transactionQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransactionQuantity",OldValue=transactionQuantity,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   transactionQuantity=value;
		   }
			
		 }
	   }
	  private string exportDocument ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportDocument  
	   {
	    
	     get
		{
		   return exportDocument;
		 }
		 set
		 {
		   if(exportDocument != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportDocument",OldValue=exportDocument,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportDocument=value;
		   }
			
		 }
	   }
	  private string messageContent ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MessageContent  
	   {
	    
	     get
		{
		   return messageContent;
		 }
		 set
		 {
		   if(messageContent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MessageContent",OldValue=messageContent,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   messageContent=value;
		   }
			
		 }
	   }
	  private string bookingNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingNumber  
	   {
	    
	     get
		{
		   return bookingNumber;
		 }
		 set
		 {
		   if(bookingNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingNumber",OldValue=bookingNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingNumber=value;
		   }
			
		 }
	   }
	  private string logisticDeliveryTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LogisticDeliveryTypeID  
	   {
	    
	     get
		{
		   return logisticDeliveryTypeID;
		 }
		 set
		 {
		   if(logisticDeliveryTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LogisticDeliveryTypeID",OldValue=logisticDeliveryTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   logisticDeliveryTypeID=value;
		   }
			
		 }
	   }
	  private decimal cargoRows ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CargoRows  
	   {
	    
	     get
		{
		   return cargoRows;
		 }
		 set
		 {
		   if(cargoRows != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoRows",OldValue=cargoRows,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   cargoRows=value;
		   }
			
		 }
	   }
	  private string exporterIdentificationType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterIdentificationType  
	   {
	    
	     get
		{
		   return exporterIdentificationType;
		 }
		 set
		 {
		   if(exporterIdentificationType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterIdentificationType",OldValue=exporterIdentificationType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterIdentificationType=value;
		   }
			
		 }
	   }
	  private string finalDestinationInternatID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalDestinationInternatID  
	   {
	    
	     get
		{
		   return finalDestinationInternatID;
		 }
		 set
		 {
		   if(finalDestinationInternatID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalDestinationInternatID",OldValue=finalDestinationInternatID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalDestinationInternatID=value;
		   }
			
		 }
	   }
	  private string firstDestinationInternatID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FirstDestinationInternatID  
	   {
	    
	     get
		{
		   return firstDestinationInternatID;
		 }
		 set
		 {
		   if(firstDestinationInternatID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstDestinationInternatID",OldValue=firstDestinationInternatID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   firstDestinationInternatID=value;
		   }
			
		 }
	   }
	  private string originAbroadSite ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginAbroadSite  
	   {
	    
	     get
		{
		   return originAbroadSite;
		 }
		 set
		 {
		   if(originAbroadSite != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginAbroadSite",OldValue=originAbroadSite,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originAbroadSite=value;
		   }
			
		 }
	   }
	  private string expectedPortArrivalDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExpectedPortArrivalDate  
	   {
	    
	     get
		{
		   return expectedPortArrivalDate;
		 }
		 set
		 {
		   if(expectedPortArrivalDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExpectedPortArrivalDate",OldValue=expectedPortArrivalDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   expectedPortArrivalDate=value;
		   }
			
		 }
	   }
	  private string draggedOrSupportedNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DraggedOrSupportedNumber  
	   {
	    
	     get
		{
		   return draggedOrSupportedNumber;
		 }
		 set
		 {
		   if(draggedOrSupportedNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DraggedOrSupportedNumber",OldValue=draggedOrSupportedNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   draggedOrSupportedNumber=value;
		   }
			
		 }
	   }
	  private string truckOrTrainNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TruckOrTrainNumber  
	   {
	    
	     get
		{
		   return truckOrTrainNumber;
		 }
		 set
		 {
		   if(truckOrTrainNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TruckOrTrainNumber",OldValue=truckOrTrainNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   truckOrTrainNumber=value;
		   }
			
		 }
	   }
	  private string driverId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DriverId  
	   {
	    
	     get
		{
		   return driverId;
		 }
		 set
		 {
		   if(driverId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DriverId",OldValue=driverId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   driverId=value;
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
	  private string transportCompany ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportCompany  
	   {
	    
	     get
		{
		   return transportCompany;
		 }
		 set
		 {
		   if(transportCompany != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportCompany",OldValue=transportCompany,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportCompany=value;
		   }
			
		 }
	   }
	  private string shipAgent ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipAgent  
	   {
	    
	     get
		{
		   return shipAgent;
		 }
		 set
		 {
		   if(shipAgent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipAgent",OldValue=shipAgent,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipAgent=value;
		   }
			
		 }
	   }
	  private string shippingCompanyCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShippingCompanyCode  
	   {
	    
	     get
		{
		   return shippingCompanyCode;
		 }
		 set
		 {
		   if(shippingCompanyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShippingCompanyCode",OldValue=shippingCompanyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shippingCompanyCode=value;
		   }
			
		 }
	   }
	  private string securityClearence ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SecurityClearence  
	   {
	    
	     get
		{
		   return securityClearence;
		 }
		 set
		 {
		   if(securityClearence != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SecurityClearence",OldValue=securityClearence,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   securityClearence=value;
		   }
			
		 }
	   }
	  private string transferCargoMethodType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransferCargoMethodType  
	   {
	    
	     get
		{
		   return transferCargoMethodType;
		 }
		 set
		 {
		   if(transferCargoMethodType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransferCargoMethodType",OldValue=transferCargoMethodType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transferCargoMethodType=value;
		   }
			
		 }
	   }
	  private string storageOrDockID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageOrDockID  
	   {
	    
	     get
		{
		   return storageOrDockID;
		 }
		 set
		 {
		   if(storageOrDockID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageOrDockID",OldValue=storageOrDockID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageOrDockID=value;
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
	  private string exporterFileNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterFileNumber  
	   {
	    
	     get
		{
		   return exporterFileNumber;
		 }
		 set
		 {
		   if(exporterFileNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterFileNumber",OldValue=exporterFileNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterFileNumber=value;
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
	  private string manifest ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Manifest  
	   {
	    
	     get
		{
		   return manifest;
		 }
		 set
		 {
		   if(manifest != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Manifest",OldValue=manifest,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manifest=value;
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
	  private string cargoDescription ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoDescription  
	   {
	    
	     get
		{
		   return cargoDescription;
		 }
		 set
		 {
		   if(cargoDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoDescription",OldValue=cargoDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoDescription=value;
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
	  private string handlingCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string HandlingCode  
	   {
	    
	     get
		{
		   return handlingCode;
		 }
		 set
		 {
		   if(handlingCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HandlingCode",OldValue=handlingCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   handlingCode=value;
		   }
			
		 }
	   }
	  private decimal dangerousGoodsIndication ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal DangerousGoodsIndication  
	   {
	    
	     get
		{
		   return dangerousGoodsIndication;
		 }
		 set
		 {
		   if(dangerousGoodsIndication != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DangerousGoodsIndication",OldValue=dangerousGoodsIndication,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   dangerousGoodsIndication=value;
		   }
			
		 }
	   }
	  private decimal codeBreaksIndication ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CodeBreaksIndication  
	   {
	    
	     get
		{
		   return codeBreaksIndication;
		 }
		 set
		 {
		   if(codeBreaksIndication != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CodeBreaksIndication",OldValue=codeBreaksIndication,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   codeBreaksIndication=value;
		   }
			
		 }
	   }
	  private decimal damageCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal DamageCode  
	   {
	    
	     get
		{
		   return damageCode;
		 }
		 set
		 {
		   if(damageCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DamageCode",OldValue=damageCode,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   damageCode=value;
		   }
			
		 }
	   }
	  private string foreignCurrencyType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForeignCurrencyType  
	   {
	    
	     get
		{
		   return foreignCurrencyType;
		 }
		 set
		 {
		   if(foreignCurrencyType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignCurrencyType",OldValue=foreignCurrencyType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   foreignCurrencyType=value;
		   }
			
		 }
	   }
	  private decimal foreignCurrencyAmoun ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ForeignCurrencyAmoun  
	   {
	    
	     get
		{
		   return foreignCurrencyAmoun;
		 }
		 set
		 {
		   if(foreignCurrencyAmoun != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignCurrencyAmoun",OldValue=foreignCurrencyAmoun,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   foreignCurrencyAmoun=value;
		   }
			
		 }
	   }
	  private decimal goodsValueNIS ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal GoodsValueNIS  
	   {
	    
	     get
		{
		   return goodsValueNIS;
		 }
		 set
		 {
		   if(goodsValueNIS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GoodsValueNIS",OldValue=goodsValueNIS,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   goodsValueNIS=value;
		   }
			
		 }
	   }
	  private string packageType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageType  
	   {
	    
	     get
		{
		   return packageType;
		 }
		 set
		 {
		   if(packageType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType",OldValue=packageType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageType=value;
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
	  private string marksNumbers ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MarksNumbers  
	   {
	    
	     get
		{
		   return marksNumbers;
		 }
		 set
		 {
		   if(marksNumbers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarksNumbers",OldValue=marksNumbers,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   marksNumbers=value;
		   }
			
		 }
	   }
	  private decimal weightInPortMandatory ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal WeightInPortMandatory  
	   {
	    
	     get
		{
		   return weightInPortMandatory;
		 }
		 set
		 {
		   if(weightInPortMandatory != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WeightInPortMandatory",OldValue=weightInPortMandatory,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   weightInPortMandatory=value;
		   }
			
		 }
	   }
	  private decimal weight ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal Weight  
	   {
	    
	     get
		{
		   return weight;
		 }
		 set
		 {
		   if(weight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Weight",OldValue=weight,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   weight=value;
		   }
			
		 }
	   }
	  private decimal volumeSize ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal VolumeSize  
	   {
	    
	     get
		{
		   return volumeSize;
		 }
		 set
		 {
		   if(volumeSize != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumeSize",OldValue=volumeSize,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   volumeSize=value;
		   }
			
		 }
	   }
	  private string licensePlateNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LicensePlateNumber  
	   {
	    
	     get
		{
		   return licensePlateNumber;
		 }
		 set
		 {
		   if(licensePlateNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LicensePlateNumber",OldValue=licensePlateNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   licensePlateNumber=value;
		   }
			
		 }
	   }
	  private string customsItem ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsItem  
	   {
	    
	     get
		{
		   return customsItem;
		 }
		 set
		 {
		   if(customsItem != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsItem",OldValue=customsItem,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsItem=value;
		   }
			
		 }
	   }
	  private string riskLevel ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RiskLevel  
	   {
	    
	     get
		{
		   return riskLevel;
		 }
		 set
		 {
		   if(riskLevel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RiskLevel",OldValue=riskLevel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   riskLevel=value;
		   }
			
		 }
	   }
	  private string dangerousSubstancename ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DangerousSubstancename  
	   {
	    
	     get
		{
		   return dangerousSubstancename;
		 }
		 set
		 {
		   if(dangerousSubstancename != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DangerousSubstancename",OldValue=dangerousSubstancename,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dangerousSubstancename=value;
		   }
			
		 }
	   }
	  private string weightVerificationNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string WeightVerificationNumber  
	   {
	    
	     get
		{
		   return weightVerificationNumber;
		 }
		 set
		 {
		   if(weightVerificationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WeightVerificationNumber",OldValue=weightVerificationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   weightVerificationNumber=value;
		   }
			
		 }
	   }
	  private decimal exporterReportedWeightID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ExporterReportedWeightID  
	   {
	    
	     get
		{
		   return exporterReportedWeightID;
		 }
		 set
		 {
		   if(exporterReportedWeightID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterReportedWeightID",OldValue=exporterReportedWeightID,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   exporterReportedWeightID=value;
		   }
			
		 }
	   }
	  private string exporterReportedWeightName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterReportedWeightName  
	   {
	    
	     get
		{
		   return exporterReportedWeightName;
		 }
		 set
		 {
		   if(exporterReportedWeightName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterReportedWeightName",OldValue=exporterReportedWeightName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterReportedWeightName=value;
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
	  private decimal coolingActivated ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CoolingActivated  
	   {
	    
	     get
		{
		   return coolingActivated;
		 }
		 set
		 {
		   if(coolingActivated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CoolingActivated",OldValue=coolingActivated,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   coolingActivated=value;
		   }
			
		 }
	   }
	  private decimal requiredTemperature ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal RequiredTemperature  
	   {
	    
	     get
		{
		   return requiredTemperature;
		 }
		 set
		 {
		   if(requiredTemperature != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequiredTemperature",OldValue=requiredTemperature,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   requiredTemperature=value;
		   }
			
		 }
	   }
	  private string pharmaGroceryIndication ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PharmaGroceryIndication  
	   {
	    
	     get
		{
		   return pharmaGroceryIndication;
		 }
		 set
		 {
		   if(pharmaGroceryIndication != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PharmaGroceryIndication",OldValue=pharmaGroceryIndication,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pharmaGroceryIndication=value;
		   }
			
		 }
	   }
	  private decimal leftException ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal LeftException  
	   {
	    
	     get
		{
		   return leftException;
		 }
		 set
		 {
		   if(leftException != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LeftException",OldValue=leftException,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   leftException=value;
		   }
			
		 }
	   }
	  private decimal rightException ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal RightException  
	   {
	    
	     get
		{
		   return rightException;
		 }
		 set
		 {
		   if(rightException != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RightException",OldValue=rightException,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   rightException=value;
		   }
			
		 }
	   }
	  private decimal frontException ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal FrontException  
	   {
	    
	     get
		{
		   return frontException;
		 }
		 set
		 {
		   if(frontException != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FrontException",OldValue=frontException,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   frontException=value;
		   }
			
		 }
	   }
	  private decimal backException ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal BackException  
	   {
	    
	     get
		{
		   return backException;
		 }
		 set
		 {
		   if(backException != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BackException",OldValue=backException,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   backException=value;
		   }
			
		 }
	   }
	  private decimal heightException ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal HeightException  
	   {
	    
	     get
		{
		   return heightException;
		 }
		 set
		 {
		   if(heightException != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HeightException",OldValue=heightException,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   heightException=value;
		   }
			
		 }
	   }
	  private string containerLineCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerLineCode  
	   {
	    
	     get
		{
		   return containerLineCode;
		 }
		 set
		 {
		   if(containerLineCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerLineCode",OldValue=containerLineCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerLineCode=value;
		   }
			
		 }
	   }
	  private decimal ventValue ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal VentValue  
	   {
	    
	     get
		{
		   return ventValue;
		 }
		 set
		 {
		   if(ventValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VentValue",OldValue=ventValue,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   ventValue=value;
		   }
			
		 }
	   }
	  private decimal humidityPercentage ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal HumidityPercentage  
	   {
	    
	     get
		{
		   return humidityPercentage;
		 }
		 set
		 {
		   if(humidityPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HumidityPercentage",OldValue=humidityPercentage,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   humidityPercentage=value;
		   }
			
		 }
	   }
	  private decimal co2Percentage ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal Co2Percentage  
	   {
	    
	     get
		{
		   return co2Percentage;
		 }
		 set
		 {
		   if(co2Percentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Co2Percentage",OldValue=co2Percentage,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   co2Percentage=value;
		   }
			
		 }
	   }
	  private decimal o2Percentage ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal O2Percentage  
	   {
	    
	     get
		{
		   return o2Percentage;
		 }
		 set
		 {
		   if(o2Percentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="O2Percentage",OldValue=o2Percentage,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   o2Percentage=value;
		   }
			
		 }
	   }
	  private string sealNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SealNumber  
	   {
	    
	     get
		{
		   return sealNumber;
		 }
		 set
		 {
		   if(sealNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SealNumber",OldValue=sealNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sealNumber=value;
		   }
			
		 }
	   }
	  private string sealType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SealType  
	   {
	    
	     get
		{
		   return sealType;
		 }
		 set
		 {
		   if(sealType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SealType",OldValue=sealType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sealType=value;
		   }
			
		 }
	   }
	  private string coolingReportingMethod ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CoolingReportingMethod  
	   {
	    
	     get
		{
		   return coolingReportingMethod;
		 }
		 set
		 {
		   if(coolingReportingMethod != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CoolingReportingMethod",OldValue=coolingReportingMethod,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   coolingReportingMethod=value;
		   }
			
		 }
	   }
	  private string fullnessCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FullnessCode  
	   {
	    
	     get
		{
		   return fullnessCode;
		 }
		 set
		 {
		   if(fullnessCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FullnessCode",OldValue=fullnessCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fullnessCode=value;
		   }
			
		 }
	   }
	  private string ownershipCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OwnershipCode  
	   {
	    
	     get
		{
		   return ownershipCode;
		 }
		 set
		 {
		   if(ownershipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OwnershipCode",OldValue=ownershipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ownershipCode=value;
		   }
			
		 }
	   }
	  private string containerTypeWCO ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerTypeWCO  
	   {
	    
	     get
		{
		   return containerTypeWCO;
		 }
		 set
		 {
		   if(containerTypeWCO != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerTypeWCO",OldValue=containerTypeWCO,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerTypeWCO=value;
		   }
			
		 }
	   }
	  private string uNNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UNNumber  
	   {
	    
	     get
		{
		   return uNNumber;
		 }
		 set
		 {
		   if(uNNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UNNumber",OldValue=uNNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   uNNumber=value;
		   }
			
		 }
	   }
	  private string riskGroup ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RiskGroup  
	   {
	    
	     get
		{
		   return riskGroup;
		 }
		 set
		 {
		   if(riskGroup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RiskGroup",OldValue=riskGroup,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   riskGroup=value;
		   }
			
		 }
	   }
   }
   
}
	 