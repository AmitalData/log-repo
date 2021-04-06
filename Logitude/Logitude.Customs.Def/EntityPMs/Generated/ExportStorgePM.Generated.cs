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
	  private bool isConnectedToDeclaration ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConnectedToDeclaration  
	   {
	    
	     get
		{
		   return isConnectedToDeclaration;
		 }
		 set
		 {
		   if(isConnectedToDeclaration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConnectedToDeclaration",OldValue=isConnectedToDeclaration,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConnectedToDeclaration=value;
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
   }
   
}
	 