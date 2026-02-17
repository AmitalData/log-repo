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
   public partial class CustomsSettingPM : EntityPM
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
	  private bool isConnectedToUniFreight ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConnectedToUniFreight  
	   {
	    
	     get
		{
		   return isConnectedToUniFreight;
		 }
		 set
		 {
		   if(isConnectedToUniFreight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConnectedToUniFreight",OldValue=isConnectedToUniFreight,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConnectedToUniFreight=value;
		   }
			
		 }
	   }
	  private string customsAgentId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsAgentId  
	   {
	    
	     get
		{
		   return customsAgentId;
		 }
		 set
		 {
		   if(customsAgentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsAgentId",OldValue=customsAgentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsAgentId=value;
		   }
			
		 }
	   }
	  private string signServiceAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SignServiceAddress  
	   {
	    
	     get
		{
		   return signServiceAddress;
		 }
		 set
		 {
		   if(signServiceAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SignServiceAddress",OldValue=signServiceAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   signServiceAddress=value;
		   }
			
		 }
	   }
	  private string iIGServiceAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IIGServiceAddress  
	   {
	    
	     get
		{
		   return iIGServiceAddress;
		 }
		 set
		 {
		   if(iIGServiceAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IIGServiceAddress",OldValue=iIGServiceAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   iIGServiceAddress=value;
		   }
			
		 }
	   }
	  private string dCAServiceAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DCAServiceAddress  
	   {
	    
	     get
		{
		   return dCAServiceAddress;
		 }
		 set
		 {
		   if(dCAServiceAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DCAServiceAddress",OldValue=dCAServiceAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dCAServiceAddress=value;
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
	  private string dCAPartnerVault ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DCAPartnerVault  
	   {
	    
	     get
		{
		   return dCAPartnerVault;
		 }
		 set
		 {
		   if(dCAPartnerVault != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DCAPartnerVault",OldValue=dCAPartnerVault,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dCAPartnerVault=value;
		   }
			
		 }
	   }
	  private string uServerServiceAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UServerServiceAddress  
	   {
	    
	     get
		{
		   return uServerServiceAddress;
		 }
		 set
		 {
		   if(uServerServiceAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UServerServiceAddress",OldValue=uServerServiceAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   uServerServiceAddress=value;
		   }
			
		 }
	   }
	  private string defaultNotificationAssignee ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DefaultNotificationAssignee  
	   {
	    
	     get
		{
		   return defaultNotificationAssignee;
		 }
		 set
		 {
		   if(defaultNotificationAssignee != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefaultNotificationAssignee",OldValue=defaultNotificationAssignee,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   defaultNotificationAssignee=value;
		   }
			
		 }
	   }
	  private string defaultNotificationAssigneeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DefaultNotificationAssigneeName  
	   {
	    
	     get
		{
		   return defaultNotificationAssigneeName;
		 }
		 set
		 {
		   if(defaultNotificationAssigneeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefaultNotificationAssigneeName",OldValue=defaultNotificationAssigneeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   defaultNotificationAssigneeName=value;
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
	  private string customsEnvoirmentTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsEnvoirmentTypeCode  
	   {
	    
	     get
		{
		   return customsEnvoirmentTypeCode;
		 }
		 set
		 {
		   if(customsEnvoirmentTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsEnvoirmentTypeCode",OldValue=customsEnvoirmentTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsEnvoirmentTypeCode=value;
		   }
			
		 }
	   }
	  private string customsEnvoirmentTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsEnvoirmentTypeName  
	   {
	    
	     get
		{
		   return customsEnvoirmentTypeName;
		 }
		 set
		 {
		   if(customsEnvoirmentTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsEnvoirmentTypeName",OldValue=customsEnvoirmentTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsEnvoirmentTypeName=value;
		   }
			
		 }
	   }
	  private string onPremiseFillingService ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OnPremiseFillingService  
	   {
	    
	     get
		{
		   return onPremiseFillingService;
		 }
		 set
		 {
		   if(onPremiseFillingService != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OnPremiseFillingService",OldValue=onPremiseFillingService,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   onPremiseFillingService=value;
		   }
			
		 }
	   }
	  private string unfConnectionString ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UnfConnectionString  
	   {
	    
	     get
		{
		   return unfConnectionString;
		 }
		 set
		 {
		   if(unfConnectionString != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UnfConnectionString",OldValue=unfConnectionString,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   unfConnectionString=value;
		   }
			
		 }
	   }
	  private bool tehilaDca ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TehilaDca  
	   {
	    
	     get
		{
		   return tehilaDca;
		 }
		 set
		 {
		   if(tehilaDca != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TehilaDca",OldValue=tehilaDca,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tehilaDca=value;
		   }
			
		 }
	   }
	  private bool blockAgentBankForMasab ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool BlockAgentBankForMasab  
	   {
	    
	     get
		{
		   return blockAgentBankForMasab;
		 }
		 set
		 {
		   if(blockAgentBankForMasab != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BlockAgentBankForMasab",OldValue=blockAgentBankForMasab,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   blockAgentBankForMasab=value;
		   }
			
		 }
	   }
	  private string paymentOrderAccCard ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentOrderAccCard  
	   {
	    
	     get
		{
		   return paymentOrderAccCard;
		 }
		 set
		 {
		   if(paymentOrderAccCard != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderAccCard",OldValue=paymentOrderAccCard,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentOrderAccCard=value;
		   }
			
		 }
	   }
	  private bool unifreightCertificateActivated ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool UnifreightCertificateActivated  
	   {
	    
	     get
		{
		   return unifreightCertificateActivated;
		 }
		 set
		 {
		   if(unifreightCertificateActivated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UnifreightCertificateActivated",OldValue=unifreightCertificateActivated,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   unifreightCertificateActivated=value;
		   }
			
		 }
	   }
	  private bool autoFillPaymentScreen ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AutoFillPaymentScreen  
	   {
	    
	     get
		{
		   return autoFillPaymentScreen;
		 }
		 set
		 {
		   if(autoFillPaymentScreen != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutoFillPaymentScreen",OldValue=autoFillPaymentScreen,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   autoFillPaymentScreen=value;
		   }
			
		 }
	   }
	  private bool autoFillAccountType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AutoFillAccountType  
	   {
	    
	     get
		{
		   return autoFillAccountType;
		 }
		 set
		 {
		   if(autoFillAccountType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutoFillAccountType",OldValue=autoFillAccountType,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   autoFillAccountType=value;
		   }
			
		 }
	   }
	  private bool autoUnitMeasurement ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AutoUnitMeasurement  
	   {
	    
	     get
		{
		   return autoUnitMeasurement;
		 }
		 set
		 {
		   if(autoUnitMeasurement != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutoUnitMeasurement",OldValue=autoUnitMeasurement,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   autoUnitMeasurement=value;
		   }
			
		 }
	   }
   }
   
}
	 