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
   }
   
}
	 