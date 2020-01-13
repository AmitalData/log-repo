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
   public partial class CourierMasterPM : EntityPM
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
	  private DateTime? createDateTime ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CreateDateTime  
	   {
	    
	     get
		{
		   return createDateTime;
		 }
		 set
		 {
		   if(createDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDateTime",OldValue=createDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   createDateTime=value;
		   }
			
		 }
	   }
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
		   }
			
		 }
	   }
	  private DateTime? updateDateTime ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UpdateDateTime  
	   {
	    
	     get
		{
		   return updateDateTime;
		 }
		 set
		 {
		   if(updateDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDateTime",OldValue=updateDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   updateDateTime=value;
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
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserName  
	   {
	    
	     get
		{
		   return createdByUserName;
		 }
		 set
		 {
		   if(createdByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserName",OldValue=createdByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserName=value;
		   }
			
		 }
	   }
	  private string airlineId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirlineId  
	   {
	    
	     get
		{
		   return airlineId;
		 }
		 set
		 {
		   if(airlineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirlineId",OldValue=airlineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airlineId=value;
		   }
			
		 }
	   }
	  private string airlineName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirlineName  
	   {
	    
	     get
		{
		   return airlineName;
		 }
		 set
		 {
		   if(airlineName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirlineName",OldValue=airlineName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airlineName=value;
		   }
			
		 }
	   }
	  private string mAWB ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWB  
	   {
	    
	     get
		{
		   return mAWB;
		 }
		 set
		 {
		   if(mAWB != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWB",OldValue=mAWB,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWB=value;
		   }
			
		 }
	   }
	  private string mAWBTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWBTypeName  
	   {
	    
	     get
		{
		   return mAWBTypeName;
		 }
		 set
		 {
		   if(mAWBTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBTypeName",OldValue=mAWBTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWBTypeName=value;
		   }
			
		 }
	   }
	  private string hAWB ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string HAWB  
	   {
	    
	     get
		{
		   return hAWB;
		 }
		 set
		 {
		   if(hAWB != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HAWB",OldValue=hAWB,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   hAWB=value;
		   }
			
		 }
	   }
	  private DateTime? estimatedArrivalDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EstimatedArrivalDate  
	   {
	    
	     get
		{
		   return estimatedArrivalDate;
		 }
		 set
		 {
		   if(estimatedArrivalDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedArrivalDate",OldValue=estimatedArrivalDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   estimatedArrivalDate=value;
		   }
			
		 }
	   }
	  private string gatewayPortCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string GatewayPortCode  
	   {
	    
	     get
		{
		   return gatewayPortCode;
		 }
		 set
		 {
		   if(gatewayPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GatewayPortCode",OldValue=gatewayPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gatewayPortCode=value;
		   }
			
		 }
	   }
	  private string gatewayPortName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string GatewayPortName  
	   {
	    
	     get
		{
		   return gatewayPortName;
		 }
		 set
		 {
		   if(gatewayPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GatewayPortName",OldValue=gatewayPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gatewayPortName=value;
		   }
			
		 }
	   }
	  private string originPortCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginPortCode  
	   {
	    
	     get
		{
		   return originPortCode;
		 }
		 set
		 {
		   if(originPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginPortCode",OldValue=originPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originPortCode=value;
		   }
			
		 }
	   }
	  private string originPortName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginPortName  
	   {
	    
	     get
		{
		   return originPortName;
		 }
		 set
		 {
		   if(originPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginPortName",OldValue=originPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originPortName=value;
		   }
			
		 }
	   }
	  private bool isOpen ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsOpen  
	   {
	    
	     get
		{
		   return isOpen;
		 }
		 set
		 {
		   if(isOpen != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsOpen",OldValue=isOpen,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isOpen=value;
		   }
			
		 }
	   }
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCancelled  
	   {
	    
	     get
		{
		   return isCancelled;
		 }
		 set
		 {
		   if(isCancelled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancelled",OldValue=isCancelled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCancelled=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId  
	   {
	    
	     get
		{
		   return updatedByUserId;
		 }
		 set
		 {
		   if(updatedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserId",OldValue=updatedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserId=value;
		   }
			
		 }
	   }
	  private string updatedByUserName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserName  
	   {
	    
	     get
		{
		   return updatedByUserName;
		 }
		 set
		 {
		   if(updatedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserName",OldValue=updatedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserName=value;
		   }
			
		 }
	   }
	  private string airlinePrefix ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirlinePrefix  
	   {
	    
	     get
		{
		   return airlinePrefix;
		 }
		 set
		 {
		   if(airlinePrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirlinePrefix",OldValue=airlinePrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airlinePrefix=value;
		   }
			
		 }
	   }
	  private bool selectedDeclarationChanged ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool SelectedDeclarationChanged  
	   {
	    
	     get
		{
		   return selectedDeclarationChanged;
		 }
		 set
		 {
		   if(selectedDeclarationChanged != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SelectedDeclarationChanged",OldValue=selectedDeclarationChanged,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   selectedDeclarationChanged=value;
		   }
			
		 }
	   }
	  private string connectedDeclarations ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConnectedDeclarations  
	   {
	    
	     get
		{
		   return connectedDeclarations;
		 }
		 set
		 {
		   if(connectedDeclarations != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedDeclarations",OldValue=connectedDeclarations,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   connectedDeclarations=value;
		   }
			
		 }
	   }
	  private string notConnectedDeclarations ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotConnectedDeclarations  
	   {
	    
	     get
		{
		   return notConnectedDeclarations;
		 }
		 set
		 {
		   if(notConnectedDeclarations != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotConnectedDeclarations",OldValue=notConnectedDeclarations,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notConnectedDeclarations=value;
		   }
			
		 }
	   }
	  private string mAWBTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWBTypeCode  
	   {
	    
	     get
		{
		   return mAWBTypeCode;
		 }
		 set
		 {
		   if(mAWBTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBTypeCode",OldValue=mAWBTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWBTypeCode=value;
		   }
			
		 }
	   }
	  private string manifestNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManifestNumber  
	   {
	    
	     get
		{
		   return manifestNumber;
		 }
		 set
		 {
		   if(manifestNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManifestNumber",OldValue=manifestNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manifestNumber=value;
		   }
			
		 }
	   }
	  private int? packageQuantity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? PackageQuantity  
	   {
	    
	     get
		{
		   return packageQuantity;
		 }
		 set
		 {
		   if(packageQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageQuantity",OldValue=packageQuantity,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   packageQuantity=value;
		   }
			
		 }
	   }
	  private decimal? grossMassMeasure ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? GrossMassMeasure  
	   {
	    
	     get
		{
		   return grossMassMeasure;
		 }
		 set
		 {
		   if(grossMassMeasure != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossMassMeasure",OldValue=grossMassMeasure,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   grossMassMeasure=value;
		   }
			
		 }
	   }
	  private string shortHAWB ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShortHAWB  
	   {
	    
	     get
		{
		   return shortHAWB;
		 }
		 set
		 {
		   if(shortHAWB != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShortHAWB",OldValue=shortHAWB,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shortHAWB=value;
		   }
			
		 }
	   }
	  private string flightNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FlightNumber  
	   {
	    
	     get
		{
		   return flightNumber;
		 }
		 set
		 {
		   if(flightNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FlightNumber",OldValue=flightNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   flightNumber=value;
		   }
			
		 }
	   }
	  private DateTime? departureDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DepartureDate  
	   {
	    
	     get
		{
		   return departureDate;
		 }
		 set
		 {
		   if(departureDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartureDate",OldValue=departureDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   departureDate=value;
		   }
			
		 }
	   }
	  private DateTime? estimatedArrivalDateOnly ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EstimatedArrivalDateOnly  
	   {
	    
	     get
		{
		   return estimatedArrivalDateOnly;
		 }
		 set
		 {
		   if(estimatedArrivalDateOnly != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedArrivalDateOnly",OldValue=estimatedArrivalDateOnly,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   estimatedArrivalDateOnly=value;
		   }
			
		 }
	   }
	  private DateTime? estimatedArrivalTimeOnly ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EstimatedArrivalTimeOnly  
	   {
	    
	     get
		{
		   return estimatedArrivalTimeOnly;
		 }
		 set
		 {
		   if(estimatedArrivalTimeOnly != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedArrivalTimeOnly",OldValue=estimatedArrivalTimeOnly,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   estimatedArrivalTimeOnly=value;
		   }
			
		 }
	   }
	  private string weightValueCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string WeightValueCode  
	   {
	    
	     get
		{
		   return weightValueCode;
		 }
		 set
		 {
		   if(weightValueCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WeightValueCode",OldValue=weightValueCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   weightValueCode=value;
		   }
			
		 }
	   }
	  private string weightValueName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string WeightValueName  
	   {
	    
	     get
		{
		   return weightValueName;
		 }
		 set
		 {
		   if(weightValueName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WeightValueName",OldValue=weightValueName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   weightValueName=value;
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
	  private string truckerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TruckerId  
	   {
	    
	     get
		{
		   return truckerId;
		 }
		 set
		 {
		   if(truckerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TruckerId",OldValue=truckerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   truckerId=value;
		   }
			
		 }
	   }
	  private string integratorCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IntegratorCode  
	   {
	    
	     get
		{
		   return integratorCode;
		 }
		 set
		 {
		   if(integratorCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IntegratorCode",OldValue=integratorCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   integratorCode=value;
		   }
			
		 }
	   }
	  private string integratorName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IntegratorName  
	   {
	    
	     get
		{
		   return integratorName;
		 }
		 set
		 {
		   if(integratorName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IntegratorName",OldValue=integratorName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   integratorName=value;
		   }
			
		 }
	   }
	  private string integratorNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IntegratorNumber  
	   {
	    
	     get
		{
		   return integratorNumber;
		 }
		 set
		 {
		   if(integratorNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IntegratorNumber",OldValue=integratorNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   integratorNumber=value;
		   }
			
		 }
	   }
	  private bool isReadyForInvoice ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsReadyForInvoice  
	   {
	    
	     get
		{
		   return isReadyForInvoice;
		 }
		 set
		 {
		   if(isReadyForInvoice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsReadyForInvoice",OldValue=isReadyForInvoice,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isReadyForInvoice=value;
		   }
			
		 }
	   }
	  private bool isAllDecClosedForFollowUp ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAllDecClosedForFollowUp  
	   {
	    
	     get
		{
		   return isAllDecClosedForFollowUp;
		 }
		 set
		 {
		   if(isAllDecClosedForFollowUp != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAllDecClosedForFollowUp",OldValue=isAllDecClosedForFollowUp,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAllDecClosedForFollowUp=value;
		   }
			
		 }
	   }
	  private int calcClosedForFollowUp ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CalcClosedForFollowUp  
	   {
	    
	     get
		{
		   return calcClosedForFollowUp;
		 }
		 set
		 {
		   if(calcClosedForFollowUp != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalcClosedForFollowUp",OldValue=calcClosedForFollowUp,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   calcClosedForFollowUp=value;
		   }
			
		 }
	   }
	  private int calcMissingClassification ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CalcMissingClassification  
	   {
	    
	     get
		{
		   return calcMissingClassification;
		 }
		 set
		 {
		   if(calcMissingClassification != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalcMissingClassification",OldValue=calcMissingClassification,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   calcMissingClassification=value;
		   }
			
		 }
	   }
	  private int calcMissingImporterId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CalcMissingImporterId  
	   {
	    
	     get
		{
		   return calcMissingImporterId;
		 }
		 set
		 {
		   if(calcMissingImporterId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalcMissingImporterId",OldValue=calcMissingImporterId,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   calcMissingImporterId=value;
		   }
			
		 }
	   }
	  private int calcPendingCustoms ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CalcPendingCustoms  
	   {
	    
	     get
		{
		   return calcPendingCustoms;
		 }
		 set
		 {
		   if(calcPendingCustoms != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalcPendingCustoms",OldValue=calcPendingCustoms,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   calcPendingCustoms=value;
		   }
			
		 }
	   }
	  private int calcPending900 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CalcPending900  
	   {
	    
	     get
		{
		   return calcPending900;
		 }
		 set
		 {
		   if(calcPending900 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalcPending900",OldValue=calcPending900,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   calcPending900=value;
		   }
			
		 }
	   }
	  private int calcSuspendedDeclarations ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CalcSuspendedDeclarations  
	   {
	    
	     get
		{
		   return calcSuspendedDeclarations;
		 }
		 set
		 {
		   if(calcSuspendedDeclarations != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalcSuspendedDeclarations",OldValue=calcSuspendedDeclarations,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   calcSuspendedDeclarations=value;
		   }
			
		 }
	   }
   }
   
}
	 