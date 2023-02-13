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
using Logitude.Infrastructure.BL.Validators;
  
namespace Logitude.Infrastructure.BL.EntityPMs
{
   [CustomValidation(typeof(InfrastructureClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class SharedLogisticsSettingPM : EntityPM
   {
   	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private bool isAgentShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAgentShared  
	   {
	    
	     get
		{
		   return isAgentShared;
		 }
		 set
		 {
		   if(isAgentShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAgentShared",OldValue=isAgentShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAgentShared=value;
		   }
			
		 }
	   }
	  private bool isShipperNotExporterShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsShipperNotExporterShared  
	   {
	    
	     get
		{
		   return isShipperNotExporterShared;
		 }
		 set
		 {
		   if(isShipperNotExporterShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsShipperNotExporterShared",OldValue=isShipperNotExporterShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isShipperNotExporterShared=value;
		   }
			
		 }
	   }
	  private bool isNotify1Shared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsNotify1Shared  
	   {
	    
	     get
		{
		   return isNotify1Shared;
		 }
		 set
		 {
		   if(isNotify1Shared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsNotify1Shared",OldValue=isNotify1Shared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isNotify1Shared=value;
		   }
			
		 }
	   }
	  private bool isNotify2Shared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsNotify2Shared  
	   {
	    
	     get
		{
		   return isNotify2Shared;
		 }
		 set
		 {
		   if(isNotify2Shared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsNotify2Shared",OldValue=isNotify2Shared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isNotify2Shared=value;
		   }
			
		 }
	   }
	  private bool isFreightForwarderShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsFreightForwarderShared  
	   {
	    
	     get
		{
		   return isFreightForwarderShared;
		 }
		 set
		 {
		   if(isFreightForwarderShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFreightForwarderShared",OldValue=isFreightForwarderShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isFreightForwarderShared=value;
		   }
			
		 }
	   }
	  private bool isColoaderShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsColoaderShared  
	   {
	    
	     get
		{
		   return isColoaderShared;
		 }
		 set
		 {
		   if(isColoaderShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsColoaderShared",OldValue=isColoaderShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isColoaderShared=value;
		   }
			
		 }
	   }
	  private bool isConsigneeNotImporterShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConsigneeNotImporterShared  
	   {
	    
	     get
		{
		   return isConsigneeNotImporterShared;
		 }
		 set
		 {
		   if(isConsigneeNotImporterShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConsigneeNotImporterShared",OldValue=isConsigneeNotImporterShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConsigneeNotImporterShared=value;
		   }
			
		 }
	   }
	  private bool isMainCarrierShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMainCarrierShared  
	   {
	    
	     get
		{
		   return isMainCarrierShared;
		 }
		 set
		 {
		   if(isMainCarrierShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMainCarrierShared",OldValue=isMainCarrierShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMainCarrierShared=value;
		   }
			
		 }
	   }
	  private bool isPickDelivCarriesShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPickDelivCarriesShared  
	   {
	    
	     get
		{
		   return isPickDelivCarriesShared;
		 }
		 set
		 {
		   if(isPickDelivCarriesShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPickDelivCarriesShared",OldValue=isPickDelivCarriesShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPickDelivCarriesShared=value;
		   }
			
		 }
	   }
	  private bool isInvoicesMenuEnabled ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsInvoicesMenuEnabled  
	   {
	    
	     get
		{
		   return isInvoicesMenuEnabled;
		 }
		 set
		 {
		   if(isInvoicesMenuEnabled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsInvoicesMenuEnabled",OldValue=isInvoicesMenuEnabled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isInvoicesMenuEnabled=value;
		   }
			
		 }
	   }
	  private bool isMoneyTabEnabled ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMoneyTabEnabled  
	   {
	    
	     get
		{
		   return isMoneyTabEnabled;
		 }
		 set
		 {
		   if(isMoneyTabEnabled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMoneyTabEnabled",OldValue=isMoneyTabEnabled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMoneyTabEnabled=value;
		   }
			
		 }
	   }
	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private bool isIssuingCarrierAgentShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsIssuingCarrierAgentShared  
	   {
	    
	     get
		{
		   return isIssuingCarrierAgentShared;
		 }
		 set
		 {
		   if(isIssuingCarrierAgentShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsIssuingCarrierAgentShared",OldValue=isIssuingCarrierAgentShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isIssuingCarrierAgentShared=value;
		   }
			
		 }
	   }
	  private bool isCustomsAgentExportShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomsAgentExportShared  
	   {
	    
	     get
		{
		   return isCustomsAgentExportShared;
		 }
		 set
		 {
		   if(isCustomsAgentExportShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomsAgentExportShared",OldValue=isCustomsAgentExportShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomsAgentExportShared=value;
		   }
			
		 }
	   }
	  private bool isCustomsAgentImportShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomsAgentImportShared  
	   {
	    
	     get
		{
		   return isCustomsAgentImportShared;
		 }
		 set
		 {
		   if(isCustomsAgentImportShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomsAgentImportShared",OldValue=isCustomsAgentImportShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomsAgentImportShared=value;
		   }
			
		 }
	   }
	  private bool isCustomClearancePoinShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomClearancePoinShared  
	   {
	    
	     get
		{
		   return isCustomClearancePoinShared;
		 }
		 set
		 {
		   if(isCustomClearancePoinShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomClearancePoinShared",OldValue=isCustomClearancePoinShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomClearancePoinShared=value;
		   }
			
		 }
	   }
	  private bool isConsolidatorShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConsolidatorShared  
	   {
	    
	     get
		{
		   return isConsolidatorShared;
		 }
		 set
		 {
		   if(isConsolidatorShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConsolidatorShared",OldValue=isConsolidatorShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConsolidatorShared=value;
		   }
			
		 }
	   }
	  private bool isReleasingAgentShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsReleasingAgentShared  
	   {
	    
	     get
		{
		   return isReleasingAgentShared;
		 }
		 set
		 {
		   if(isReleasingAgentShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsReleasingAgentShared",OldValue=isReleasingAgentShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isReleasingAgentShared=value;
		   }
			
		 }
	   }
	  private bool isShipperShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsShipperShared  
	   {
	    
	     get
		{
		   return isShipperShared;
		 }
		 set
		 {
		   if(isShipperShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsShipperShared",OldValue=isShipperShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isShipperShared=value;
		   }
			
		 }
	   }
	  private bool isConsigneeShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConsigneeShared  
	   {
	    
	     get
		{
		   return isConsigneeShared;
		 }
		 set
		 {
		   if(isConsigneeShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConsigneeShared",OldValue=isConsigneeShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConsigneeShared=value;
		   }
			
		 }
	   }
	  private bool isShowAmountLocalCurrency ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsShowAmountLocalCurrency  
	   {
	    
	     get
		{
		   return isShowAmountLocalCurrency;
		 }
		 set
		 {
		   if(isShowAmountLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsShowAmountLocalCurrency",OldValue=isShowAmountLocalCurrency,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isShowAmountLocalCurrency=value;
		   }
			
		 }
	   }
	  private bool isShipperShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsShipperShowContactTS  
	   {
	    
	     get
		{
		   return isShipperShowContactTS;
		 }
		 set
		 {
		   if(isShipperShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsShipperShowContactTS",OldValue=isShipperShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isShipperShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isConsigneeShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConsigneeShowContactTS  
	   {
	    
	     get
		{
		   return isConsigneeShowContactTS;
		 }
		 set
		 {
		   if(isConsigneeShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConsigneeShowContactTS",OldValue=isConsigneeShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConsigneeShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isAgentShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAgentShowContactTS  
	   {
	    
	     get
		{
		   return isAgentShowContactTS;
		 }
		 set
		 {
		   if(isAgentShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAgentShowContactTS",OldValue=isAgentShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAgentShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isShipperNotExShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsShipperNotExShowContactTS  
	   {
	    
	     get
		{
		   return isShipperNotExShowContactTS;
		 }
		 set
		 {
		   if(isShipperNotExShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsShipperNotExShowContactTS",OldValue=isShipperNotExShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isShipperNotExShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isConsigneeNotImShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConsigneeNotImShowContactTS  
	   {
	    
	     get
		{
		   return isConsigneeNotImShowContactTS;
		 }
		 set
		 {
		   if(isConsigneeNotImShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConsigneeNotImShowContactTS",OldValue=isConsigneeNotImShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConsigneeNotImShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isNotify1ShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsNotify1ShowContactTS  
	   {
	    
	     get
		{
		   return isNotify1ShowContactTS;
		 }
		 set
		 {
		   if(isNotify1ShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsNotify1ShowContactTS",OldValue=isNotify1ShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isNotify1ShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isNotify2ShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsNotify2ShowContactTS  
	   {
	    
	     get
		{
		   return isNotify2ShowContactTS;
		 }
		 set
		 {
		   if(isNotify2ShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsNotify2ShowContactTS",OldValue=isNotify2ShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isNotify2ShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isFreightForwardShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsFreightForwardShowContactTS  
	   {
	    
	     get
		{
		   return isFreightForwardShowContactTS;
		 }
		 set
		 {
		   if(isFreightForwardShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFreightForwardShowContactTS",OldValue=isFreightForwardShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isFreightForwardShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isColoaderShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsColoaderShowContactTS  
	   {
	    
	     get
		{
		   return isColoaderShowContactTS;
		 }
		 set
		 {
		   if(isColoaderShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsColoaderShowContactTS",OldValue=isColoaderShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isColoaderShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isCustomAgentExShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomAgentExShowContactTS  
	   {
	    
	     get
		{
		   return isCustomAgentExShowContactTS;
		 }
		 set
		 {
		   if(isCustomAgentExShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomAgentExShowContactTS",OldValue=isCustomAgentExShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomAgentExShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isCustomAgentImShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomAgentImShowContactTS  
	   {
	    
	     get
		{
		   return isCustomAgentImShowContactTS;
		 }
		 set
		 {
		   if(isCustomAgentImShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomAgentImShowContactTS",OldValue=isCustomAgentImShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomAgentImShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isCustomCleaPointShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomCleaPointShowContactTS  
	   {
	    
	     get
		{
		   return isCustomCleaPointShowContactTS;
		 }
		 set
		 {
		   if(isCustomCleaPointShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomCleaPointShowContactTS",OldValue=isCustomCleaPointShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomCleaPointShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isConsolidatorShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConsolidatorShowContactTS  
	   {
	    
	     get
		{
		   return isConsolidatorShowContactTS;
		 }
		 set
		 {
		   if(isConsolidatorShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConsolidatorShowContactTS",OldValue=isConsolidatorShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConsolidatorShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isReleasingAgentShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsReleasingAgentShowContactTS  
	   {
	    
	     get
		{
		   return isReleasingAgentShowContactTS;
		 }
		 set
		 {
		   if(isReleasingAgentShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsReleasingAgentShowContactTS",OldValue=isReleasingAgentShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isReleasingAgentShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isIssuingCarAgentShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsIssuingCarAgentShowContactTS  
	   {
	    
	     get
		{
		   return isIssuingCarAgentShowContactTS;
		 }
		 set
		 {
		   if(isIssuingCarAgentShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsIssuingCarAgentShowContactTS",OldValue=isIssuingCarAgentShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isIssuingCarAgentShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isCustomerShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomerShared  
	   {
	    
	     get
		{
		   return isCustomerShared;
		 }
		 set
		 {
		   if(isCustomerShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomerShared",OldValue=isCustomerShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomerShared=value;
		   }
			
		 }
	   }
	  private bool isCustomerShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomerShowContactTS  
	   {
	    
	     get
		{
		   return isCustomerShowContactTS;
		 }
		 set
		 {
		   if(isCustomerShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomerShowContactTS",OldValue=isCustomerShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomerShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isAccountManagerShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAccountManagerShared  
	   {
	    
	     get
		{
		   return isAccountManagerShared;
		 }
		 set
		 {
		   if(isAccountManagerShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAccountManagerShared",OldValue=isAccountManagerShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAccountManagerShared=value;
		   }
			
		 }
	   }
	  private bool isAccountManagerShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAccountManagerShowContactTS  
	   {
	    
	     get
		{
		   return isAccountManagerShowContactTS;
		 }
		 set
		 {
		   if(isAccountManagerShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAccountManagerShowContactTS",OldValue=isAccountManagerShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAccountManagerShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isSalesmanShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSalesmanShared  
	   {
	    
	     get
		{
		   return isSalesmanShared;
		 }
		 set
		 {
		   if(isSalesmanShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSalesmanShared",OldValue=isSalesmanShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSalesmanShared=value;
		   }
			
		 }
	   }
	  private bool isSalesmanShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSalesmanShowContactTS  
	   {
	    
	     get
		{
		   return isSalesmanShowContactTS;
		 }
		 set
		 {
		   if(isSalesmanShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSalesmanShowContactTS",OldValue=isSalesmanShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSalesmanShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isCollectorShared ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCollectorShared  
	   {
	    
	     get
		{
		   return isCollectorShared;
		 }
		 set
		 {
		   if(isCollectorShared != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCollectorShared",OldValue=isCollectorShared,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCollectorShared=value;
		   }
			
		 }
	   }
	  private bool isCollectorShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCollectorShowContactTS  
	   {
	    
	     get
		{
		   return isCollectorShowContactTS;
		 }
		 set
		 {
		   if(isCollectorShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCollectorShowContactTS",OldValue=isCollectorShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCollectorShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isPickDelivCarShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPickDelivCarShowContactTS  
	   {
	    
	     get
		{
		   return isPickDelivCarShowContactTS;
		 }
		 set
		 {
		   if(isPickDelivCarShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPickDelivCarShowContactTS",OldValue=isPickDelivCarShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPickDelivCarShowContactTS=value;
		   }
			
		 }
	   }
	  private bool isMainCarShowContactTS ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMainCarShowContactTS  
	   {
	    
	     get
		{
		   return isMainCarShowContactTS;
		 }
		 set
		 {
		   if(isMainCarShowContactTS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMainCarShowContactTS",OldValue=isMainCarShowContactTS,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMainCarShowContactTS=value;
		   }
			
		 }
	   }
	    }
   
}
	 