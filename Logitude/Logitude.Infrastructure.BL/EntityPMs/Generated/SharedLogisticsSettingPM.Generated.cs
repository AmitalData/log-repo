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
   }
   
}
	 