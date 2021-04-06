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
   public partial class ExportStorgeCargoPM : EntityPM
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
	  private string storageID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageID  
	   {
	    
	     get
		{
		   return storageID;
		 }
		 set
		 {
		   if(storageID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageID",OldValue=storageID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageID=value;
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
	 