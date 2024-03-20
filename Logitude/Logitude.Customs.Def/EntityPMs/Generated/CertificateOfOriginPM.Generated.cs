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
   public partial class CertificateOfOriginPM : EntityPM
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
	  private string counter ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Counter  
	   {
	    
	     get
		{
		   return counter;
		 }
		 set
		 {
		   if(counter != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Counter",OldValue=counter,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   counter=value;
		   }
			
		 }
	   }
	  private string cooTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CooTypeCode  
	   {
	    
	     get
		{
		   return cooTypeCode;
		 }
		 set
		 {
		   if(cooTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CooTypeCode",OldValue=cooTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cooTypeCode=value;
		   }
			
		 }
	   }
	  private string requestReasonCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestReasonCode  
	   {
	    
	     get
		{
		   return requestReasonCode;
		 }
		 set
		 {
		   if(requestReasonCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestReasonCode",OldValue=requestReasonCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestReasonCode=value;
		   }
			
		 }
	   }
	  private string cOONumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string COONumber  
	   {
	    
	     get
		{
		   return cOONumber;
		 }
		 set
		 {
		   if(cOONumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="COONumber",OldValue=cOONumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cOONumber=value;
		   }
			
		 }
	   }
	  private string cOONumberToCancel ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string COONumberToCancel  
	   {
	    
	     get
		{
		   return cOONumberToCancel;
		 }
		 set
		 {
		   if(cOONumberToCancel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="COONumberToCancel",OldValue=cOONumberToCancel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cOONumberToCancel=value;
		   }
			
		 }
	   }
	  private string replacementReason ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReplacementReason  
	   {
	    
	     get
		{
		   return replacementReason;
		 }
		 set
		 {
		   if(replacementReason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReplacementReason",OldValue=replacementReason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   replacementReason=value;
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
	  private string exporterVat ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterVat  
	   {
	    
	     get
		{
		   return exporterVat;
		 }
		 set
		 {
		   if(exporterVat != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterVat",OldValue=exporterVat,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterVat=value;
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
	  private string exporterAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterAddress  
	   {
	    
	     get
		{
		   return exporterAddress;
		 }
		 set
		 {
		   if(exporterAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterAddress",OldValue=exporterAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterAddress=value;
		   }
			
		 }
	   }
	  private string exporterCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExporterCountry  
	   {
	    
	     get
		{
		   return exporterCountry;
		 }
		 set
		 {
		   if(exporterCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExporterCountry",OldValue=exporterCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exporterCountry=value;
		   }
			
		 }
	   }
	  private string tradeAgreementCountry1 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeAgreementCountry1  
	   {
	    
	     get
		{
		   return tradeAgreementCountry1;
		 }
		 set
		 {
		   if(tradeAgreementCountry1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeAgreementCountry1",OldValue=tradeAgreementCountry1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeAgreementCountry1=value;
		   }
			
		 }
	   }
	  private string tradeAgreementCountry2 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeAgreementCountry2  
	   {
	    
	     get
		{
		   return tradeAgreementCountry2;
		 }
		 set
		 {
		   if(tradeAgreementCountry2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeAgreementCountry2",OldValue=tradeAgreementCountry2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeAgreementCountry2=value;
		   }
			
		 }
	   }
	  private string tradeAgreementGroupOfCountries ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeAgreementGroupOfCountries  
	   {
	    
	     get
		{
		   return tradeAgreementGroupOfCountries;
		 }
		 set
		 {
		   if(tradeAgreementGroupOfCountries != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeAgreementGroupOfCountries",OldValue=tradeAgreementGroupOfCountries,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeAgreementGroupOfCountries=value;
		   }
			
		 }
	   }
	  private string consigneeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeName  
	   {
	    
	     get
		{
		   return consigneeName;
		 }
		 set
		 {
		   if(consigneeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeName",OldValue=consigneeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeName=value;
		   }
			
		 }
	   }
	  private string consigneeAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeAddress  
	   {
	    
	     get
		{
		   return consigneeAddress;
		 }
		 set
		 {
		   if(consigneeAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeAddress",OldValue=consigneeAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeAddress=value;
		   }
			
		 }
	   }
	  private string consigneeCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeCountry  
	   {
	    
	     get
		{
		   return consigneeCountry;
		 }
		 set
		 {
		   if(consigneeCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeCountry",OldValue=consigneeCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeCountry=value;
		   }
			
		 }
	   }
	  private string consigneeRemarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeRemarks  
	   {
	    
	     get
		{
		   return consigneeRemarks;
		 }
		 set
		 {
		   if(consigneeRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeRemarks",OldValue=consigneeRemarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeRemarks=value;
		   }
			
		 }
	   }
	  private bool isConsigneeForPrint ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConsigneeForPrint  
	   {
	    
	     get
		{
		   return isConsigneeForPrint;
		 }
		 set
		 {
		   if(isConsigneeForPrint != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConsigneeForPrint",OldValue=isConsigneeForPrint,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConsigneeForPrint=value;
		   }
			
		 }
	   }
	  private string originCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginCountry  
	   {
	    
	     get
		{
		   return originCountry;
		 }
		 set
		 {
		   if(originCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginCountry",OldValue=originCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originCountry=value;
		   }
			
		 }
	   }
	  private string originGroupOfCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginGroupOfCountry  
	   {
	    
	     get
		{
		   return originGroupOfCountry;
		 }
		 set
		 {
		   if(originGroupOfCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginGroupOfCountry",OldValue=originGroupOfCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originGroupOfCountry=value;
		   }
			
		 }
	   }
	  private string destinationCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationCountry  
	   {
	    
	     get
		{
		   return destinationCountry;
		 }
		 set
		 {
		   if(destinationCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationCountry",OldValue=destinationCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationCountry=value;
		   }
			
		 }
	   }
	  private string destinationGroupOfCountries ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationGroupOfCountries  
	   {
	    
	     get
		{
		   return destinationGroupOfCountries;
		 }
		 set
		 {
		   if(destinationGroupOfCountries != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationGroupOfCountries",OldValue=destinationGroupOfCountries,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationGroupOfCountries=value;
		   }
			
		 }
	   }
	  private string transport ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transport  
	   {
	    
	     get
		{
		   return transport;
		 }
		 set
		 {
		   if(transport != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transport",OldValue=transport,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transport=value;
		   }
			
		 }
	   }
	  private string portOfShipment ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PortOfShipment  
	   {
	    
	     get
		{
		   return portOfShipment;
		 }
		 set
		 {
		   if(portOfShipment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PortOfShipment",OldValue=portOfShipment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   portOfShipment=value;
		   }
			
		 }
	   }
	  private bool isCumulation ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCumulation  
	   {
	    
	     get
		{
		   return isCumulation;
		 }
		 set
		 {
		   if(isCumulation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCumulation",OldValue=isCumulation,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCumulation=value;
		   }
			
		 }
	   }
	  private string cumulationCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CumulationCountry  
	   {
	    
	     get
		{
		   return cumulationCountry;
		 }
		 set
		 {
		   if(cumulationCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CumulationCountry",OldValue=cumulationCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cumulationCountry=value;
		   }
			
		 }
	   }
	  private string cumulationGroupOfCountries ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CumulationGroupOfCountries  
	   {
	    
	     get
		{
		   return cumulationGroupOfCountries;
		 }
		 set
		 {
		   if(cumulationGroupOfCountries != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CumulationGroupOfCountries",OldValue=cumulationGroupOfCountries,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cumulationGroupOfCountries=value;
		   }
			
		 }
	   }
	  private string placeOfManufacture ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PlaceOfManufacture  
	   {
	    
	     get
		{
		   return placeOfManufacture;
		 }
		 set
		 {
		   if(placeOfManufacture != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PlaceOfManufacture",OldValue=placeOfManufacture,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   placeOfManufacture=value;
		   }
			
		 }
	   }
	  private string zipCodeOfManufacture ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ZipCodeOfManufacture  
	   {
	    
	     get
		{
		   return zipCodeOfManufacture;
		 }
		 set
		 {
		   if(zipCodeOfManufacture != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ZipCodeOfManufacture",OldValue=zipCodeOfManufacture,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   zipCodeOfManufacture=value;
		   }
			
		 }
	   }
	  private string observations ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Observations  
	   {
	    
	     get
		{
		   return observations;
		 }
		 set
		 {
		   if(observations != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Observations",OldValue=observations,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   observations=value;
		   }
			
		 }
	   }
	  private bool isExportDecForPrint ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsExportDecForPrint  
	   {
	    
	     get
		{
		   return isExportDecForPrint;
		 }
		 set
		 {
		   if(isExportDecForPrint != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsExportDecForPrint",OldValue=isExportDecForPrint,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isExportDecForPrint=value;
		   }
			
		 }
	   }
	  private bool isUnitedInvoices ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsUnitedInvoices  
	   {
	    
	     get
		{
		   return isUnitedInvoices;
		 }
		 set
		 {
		   if(isUnitedInvoices != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsUnitedInvoices",OldValue=isUnitedInvoices,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isUnitedInvoices=value;
		   }
			
		 }
	   }
	  private string customsHouse ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsHouse  
	   {
	    
	     get
		{
		   return customsHouse;
		 }
		 set
		 {
		   if(customsHouse != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsHouse",OldValue=customsHouse,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsHouse=value;
		   }
			
		 }
	   }
	  private string issuingCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IssuingCountry  
	   {
	    
	     get
		{
		   return issuingCountry;
		 }
		 set
		 {
		   if(issuingCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssuingCountry",OldValue=issuingCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   issuingCountry=value;
		   }
			
		 }
	   }
	  private string cityOfDeclaration ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CityOfDeclaration  
	   {
	    
	     get
		{
		   return cityOfDeclaration;
		 }
		 set
		 {
		   if(cityOfDeclaration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CityOfDeclaration",OldValue=cityOfDeclaration,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cityOfDeclaration=value;
		   }
			
		 }
	   }
	  private string countryOfDeclaration ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CountryOfDeclaration  
	   {
	    
	     get
		{
		   return countryOfDeclaration;
		 }
		 set
		 {
		   if(countryOfDeclaration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryOfDeclaration",OldValue=countryOfDeclaration,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   countryOfDeclaration=value;
		   }
			
		 }
	   }
	  private DateTime? dateOfDeclaration ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DateOfDeclaration  
	   {
	    
	     get
		{
		   return dateOfDeclaration;
		 }
		 set
		 {
		   if(dateOfDeclaration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DateOfDeclaration",OldValue=dateOfDeclaration,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   dateOfDeclaration=value;
		   }
			
		 }
	   }
	  private bool isDeclaredByManufacture ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDeclaredByManufacture  
	   {
	    
	     get
		{
		   return isDeclaredByManufacture;
		 }
		 set
		 {
		   if(isDeclaredByManufacture != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDeclaredByManufacture",OldValue=isDeclaredByManufacture,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDeclaredByManufacture=value;
		   }
			
		 }
	   }
	  private bool isDeclaredByExporter ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDeclaredByExporter  
	   {
	    
	     get
		{
		   return isDeclaredByExporter;
		 }
		 set
		 {
		   if(isDeclaredByExporter != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDeclaredByExporter",OldValue=isDeclaredByExporter,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDeclaredByExporter=value;
		   }
			
		 }
	   }
	  private bool isAttachedList ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAttachedList  
	   {
	    
	     get
		{
		   return isAttachedList;
		 }
		 set
		 {
		   if(isAttachedList != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAttachedList",OldValue=isAttachedList,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAttachedList=value;
		   }
			
		 }
	   }
	  private bool insufficentWorkingInd ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool InsufficentWorkingInd  
	   {
	    
	     get
		{
		   return insufficentWorkingInd;
		 }
		 set
		 {
		   if(insufficentWorkingInd != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InsufficentWorkingInd",OldValue=insufficentWorkingInd,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   insufficentWorkingInd=value;
		   }
			
		 }
	   }
	  private string insufficentWorkingText ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InsufficentWorkingText  
	   {
	    
	     get
		{
		   return insufficentWorkingText;
		 }
		 set
		 {
		   if(insufficentWorkingText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InsufficentWorkingText",OldValue=insufficentWorkingText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   insufficentWorkingText=value;
		   }
			
		 }
	   }
	  private DateTime? nonExportDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? NonExportDate  
	   {
	    
	     get
		{
		   return nonExportDate;
		 }
		 set
		 {
		   if(nonExportDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonExportDate",OldValue=nonExportDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   nonExportDate=value;
		   }
			
		 }
	   }
	  private string nonExportCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonExportCountry  
	   {
	    
	     get
		{
		   return nonExportCountry;
		 }
		 set
		 {
		   if(nonExportCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonExportCountry",OldValue=nonExportCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonExportCountry=value;
		   }
			
		 }
	   }
	  private string nonImportBillOfLadingNum ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonImportBillOfLadingNum  
	   {
	    
	     get
		{
		   return nonImportBillOfLadingNum;
		 }
		 set
		 {
		   if(nonImportBillOfLadingNum != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonImportBillOfLadingNum",OldValue=nonImportBillOfLadingNum,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonImportBillOfLadingNum=value;
		   }
			
		 }
	   }
	  private string nonExportPort ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonExportPort  
	   {
	    
	     get
		{
		   return nonExportPort;
		 }
		 set
		 {
		   if(nonExportPort != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonExportPort",OldValue=nonExportPort,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonExportPort=value;
		   }
			
		 }
	   }
	  private DateTime? nonImportDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? NonImportDate  
	   {
	    
	     get
		{
		   return nonImportDate;
		 }
		 set
		 {
		   if(nonImportDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonImportDate",OldValue=nonImportDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   nonImportDate=value;
		   }
			
		 }
	   }
	  private string nonExportBillOfLadingNum ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonExportBillOfLadingNum  
	   {
	    
	     get
		{
		   return nonExportBillOfLadingNum;
		 }
		 set
		 {
		   if(nonExportBillOfLadingNum != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonExportBillOfLadingNum",OldValue=nonExportBillOfLadingNum,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonExportBillOfLadingNum=value;
		   }
			
		 }
	   }
	  private string nonTransirCountry ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonTransirCountry  
	   {
	    
	     get
		{
		   return nonTransirCountry;
		 }
		 set
		 {
		   if(nonTransirCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonTransirCountry",OldValue=nonTransirCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonTransirCountry=value;
		   }
			
		 }
	   }
	  private string nonPortOfEntrance ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonPortOfEntrance  
	   {
	    
	     get
		{
		   return nonPortOfEntrance;
		 }
		 set
		 {
		   if(nonPortOfEntrance != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonPortOfEntrance",OldValue=nonPortOfEntrance,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonPortOfEntrance=value;
		   }
			
		 }
	   }
	  private DateTime? nonExpectedExitDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? NonExpectedExitDate  
	   {
	    
	     get
		{
		   return nonExpectedExitDate;
		 }
		 set
		 {
		   if(nonExpectedExitDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonExpectedExitDate",OldValue=nonExpectedExitDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   nonExpectedExitDate=value;
		   }
			
		 }
	   }
	  private string nonExitPort ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonExitPort  
	   {
	    
	     get
		{
		   return nonExitPort;
		 }
		 set
		 {
		   if(nonExitPort != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonExitPort",OldValue=nonExitPort,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonExitPort=value;
		   }
			
		 }
	   }
	  private string nonGoodsDescription ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonGoodsDescription  
	   {
	    
	     get
		{
		   return nonGoodsDescription;
		 }
		 set
		 {
		   if(nonGoodsDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonGoodsDescription",OldValue=nonGoodsDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonGoodsDescription=value;
		   }
			
		 }
	   }
	  private string nonDeclaringCompany ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonDeclaringCompany  
	   {
	    
	     get
		{
		   return nonDeclaringCompany;
		 }
		 set
		 {
		   if(nonDeclaringCompany != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonDeclaringCompany",OldValue=nonDeclaringCompany,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonDeclaringCompany=value;
		   }
			
		 }
	   }
	  private string nonDeclaringPerson ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonDeclaringPerson  
	   {
	    
	     get
		{
		   return nonDeclaringPerson;
		 }
		 set
		 {
		   if(nonDeclaringPerson != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonDeclaringPerson",OldValue=nonDeclaringPerson,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonDeclaringPerson=value;
		   }
			
		 }
	   }
	  private string nonDeclaringPosition ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonDeclaringPosition  
	   {
	    
	     get
		{
		   return nonDeclaringPosition;
		 }
		 set
		 {
		   if(nonDeclaringPosition != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonDeclaringPosition",OldValue=nonDeclaringPosition,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonDeclaringPosition=value;
		   }
			
		 }
	   }
	  private string nonManifestNum ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonManifestNum  
	   {
	    
	     get
		{
		   return nonManifestNum;
		 }
		 set
		 {
		   if(nonManifestNum != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonManifestNum",OldValue=nonManifestNum,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonManifestNum=value;
		   }
			
		 }
	   }
	  private string errXml ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ErrXml  
	   {
	    
	     get
		{
		   return errXml;
		 }
		 set
		 {
		   if(errXml != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ErrXml",OldValue=errXml,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   errXml=value;
		   }
			
		 }
	   }
	  private string cooStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CooStatusCode  
	   {
	    
	     get
		{
		   return cooStatusCode;
		 }
		 set
		 {
		   if(cooStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CooStatusCode",OldValue=cooStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cooStatusCode=value;
		   }
			
		 }
	   }
	  private string feedbackRemark ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FeedbackRemark  
	   {
	    
	     get
		{
		   return feedbackRemark;
		 }
		 set
		 {
		   if(feedbackRemark != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FeedbackRemark",OldValue=feedbackRemark,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   feedbackRemark=value;
		   }
			
		 }
	   }
	  private string rejectCancelReason ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RejectCancelReason  
	   {
	    
	     get
		{
		   return rejectCancelReason;
		 }
		 set
		 {
		   if(rejectCancelReason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RejectCancelReason",OldValue=rejectCancelReason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   rejectCancelReason=value;
		   }
			
		 }
	   }
	  private DateTime? issueDateIfReleased ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? IssueDateIfReleased  
	   {
	    
	     get
		{
		   return issueDateIfReleased;
		 }
		 set
		 {
		   if(issueDateIfReleased != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssueDateIfReleased",OldValue=issueDateIfReleased,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   issueDateIfReleased=value;
		   }
			
		 }
	   }
	  private string queryUrl ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string QueryUrl  
	   {
	    
	     get
		{
		   return queryUrl;
		 }
		 set
		 {
		   if(queryUrl != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QueryUrl",OldValue=queryUrl,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   queryUrl=value;
		   }
			
		 }
	   }
	  private string cooPdf ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CooPdf  
	   {
	    
	     get
		{
		   return cooPdf;
		 }
		 set
		 {
		   if(cooPdf != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CooPdf",OldValue=cooPdf,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cooPdf=value;
		   }
			
		 }
	   }
	  private string coodPdf1 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CoodPdf1  
	   {
	    
	     get
		{
		   return coodPdf1;
		 }
		 set
		 {
		   if(coodPdf1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CoodPdf1",OldValue=coodPdf1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   coodPdf1=value;
		   }
			
		 }
	   }
	  private string openByUser ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpenByUser  
	   {
	    
	     get
		{
		   return openByUser;
		 }
		 set
		 {
		   if(openByUser != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenByUser",OldValue=openByUser,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   openByUser=value;
		   }
			
		 }
	   }
	  private bool isSubmitted ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSubmitted  
	   {
	    
	     get
		{
		   return isSubmitted;
		 }
		 set
		 {
		   if(isSubmitted != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSubmitted",OldValue=isSubmitted,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSubmitted=value;
		   }
			
		 }
	   }

	   private List<CertificateOfOriginInvoicePM> certificateOriginInvoiceItems;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("CertificateOfOriginInvoiceItems", "Id","CertificateOfOriginId")]
	   [DataMember]
	   public virtual List<CertificateOfOriginInvoicePM> CertificateOriginInvoiceItems  
	   {
	        get
             {
                 if (certificateOriginInvoiceItems == null)
                 {
                     certificateOriginInvoiceItems = new List<CertificateOfOriginInvoicePM>();
                 }
                 return certificateOriginInvoiceItems;
              }
             set { certificateOriginInvoiceItems = value; }
	    }
		   
	   private List<CertificateOfOriginInvoicePM>  deletedCertificateOriginInvoiceItems;
	   public virtual List<CertificateOfOriginInvoicePM> DeletedCertificateOriginInvoiceItems  
	   {
	        get
             {
                 if ( deletedCertificateOriginInvoiceItems == null)
                 {
                      deletedCertificateOriginInvoiceItems = new List<CertificateOfOriginInvoicePM>();
                 }
                 return  deletedCertificateOriginInvoiceItems;
              }
             set {  deletedCertificateOriginInvoiceItems = value; }
	    }
	  
	   private List<CertificateOfOriginItemPM> certificateOriginItemItems;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("CertificateOriginItemItems", "Id","CertificateOfOriginId")]
	   [DataMember]
	   public virtual List<CertificateOfOriginItemPM> CertificateOriginItemItems  
	   {
	        get
             {
                 if (certificateOriginItemItems == null)
                 {
                     certificateOriginItemItems = new List<CertificateOfOriginItemPM>();
                 }
                 return certificateOriginItemItems;
              }
             set { certificateOriginItemItems = value; }
	    }
		   
	   private List<CertificateOfOriginItemPM>  deletedCertificateOriginItemItems;
	   public virtual List<CertificateOfOriginItemPM> DeletedCertificateOriginItemItems  
	   {
	        get
             {
                 if ( deletedCertificateOriginItemItems == null)
                 {
                      deletedCertificateOriginItemItems = new List<CertificateOfOriginItemPM>();
                 }
                 return  deletedCertificateOriginItemItems;
              }
             set {  deletedCertificateOriginItemItems = value; }
	    }
	  	  private string openByUserName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpenByUserName  
	   {
	    
	     get
		{
		   return openByUserName;
		 }
		 set
		 {
		   if(openByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenByUserName",OldValue=openByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   openByUserName=value;
		   }
			
		 }
	   }
	  private string cooTypeCodeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CooTypeCodeName  
	   {
	    
	     get
		{
		   return cooTypeCodeName;
		 }
		 set
		 {
		   if(cooTypeCodeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CooTypeCodeName",OldValue=cooTypeCodeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cooTypeCodeName=value;
		   }
			
		 }
	   }
	  private string requestReasonCodeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestReasonCodeName  
	   {
	    
	     get
		{
		   return requestReasonCodeName;
		 }
		 set
		 {
		   if(requestReasonCodeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestReasonCodeName",OldValue=requestReasonCodeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestReasonCodeName=value;
		   }
			
		 }
	   }
	  private string cooStatusCodeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CooStatusCodeName  
	   {
	    
	     get
		{
		   return cooStatusCodeName;
		 }
		 set
		 {
		   if(cooStatusCodeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CooStatusCodeName",OldValue=cooStatusCodeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cooStatusCodeName=value;
		   }
			
		 }
	   }
	  private int listCounter ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ListCounter  
	   {
	    
	     get
		{
		   return listCounter;
		 }
		 set
		 {
		   if(listCounter != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ListCounter",OldValue=listCounter,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   listCounter=value;
		   }
			
		 }
	   }

	   private List<string> certificateOriginDocuments;
	 
		     
	   [Include]
	   [Association("", "","")]
	   [DataMember]
	   public virtual List<string> CertificateOriginDocuments  
	   {
	        get
             {
                 if (certificateOriginDocuments == null)
                 {
                     certificateOriginDocuments = new List<string>();
                 }
                 return certificateOriginDocuments;
              }
             set { certificateOriginDocuments = value; }
	    }
		   
	   private List<string>  deletedCertificateOriginDocuments;
	   public virtual List<string> DeletedCertificateOriginDocuments  
	   {
	        get
             {
                 if ( deletedCertificateOriginDocuments == null)
                 {
                      deletedCertificateOriginDocuments = new List<string>();
                 }
                 return  deletedCertificateOriginDocuments;
              }
             set {  deletedCertificateOriginDocuments = value; }
	    }
	  	  private string updateDeclaration ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdateDeclaration  
	   {
	    
	     get
		{
		   return updateDeclaration;
		 }
		 set
		 {
		   if(updateDeclaration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDeclaration",OldValue=updateDeclaration,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updateDeclaration=value;
		   }
			
		 }
	   }
	  private bool isChange ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsChange  
	   {
	    
	     get
		{
		   return isChange;
		 }
		 set
		 {
		   if(isChange != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsChange",OldValue=isChange,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isChange=value;
		   }
			
		 }
	   }
   }
   
}
	 