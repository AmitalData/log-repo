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
   public partial class ConsignmentPM : EntityPM
   {
   	  private string declarationId ;
	  
       [Key]
	  
       
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
	  private int? consignmentNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ConsignmentNumber  
	   {
	    
	     get
		{
		   return consignmentNumber;
		 }
		 set
		 {
		   if(consignmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsignmentNumber",OldValue=consignmentNumber,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   consignmentNumber=value;
		   }
			
		 }
	   }
	  private int? sequenceNumeric ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? SequenceNumeric  
	   {
	    
	     get
		{
		   return sequenceNumeric;
		 }
		 set
		 {
		   if(sequenceNumeric != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SequenceNumeric",OldValue=sequenceNumeric,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   sequenceNumeric=value;
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
	  private string cargoTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoTypeName  
	   {
	    
	     get
		{
		   return cargoTypeName;
		 }
		 set
		 {
		   if(cargoTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoTypeName",OldValue=cargoTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoTypeName=value;
		   }
			
		 }
	   }
	  private DateTime? manifestDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ManifestDate  
	   {
	    
	     get
		{
		   return manifestDate;
		 }
		 set
		 {
		   if(manifestDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManifestDate",OldValue=manifestDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   manifestDate=value;
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
	  private DateTime? unloadDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UnloadDate  
	   {
	    
	     get
		{
		   return unloadDate;
		 }
		 set
		 {
		   if(unloadDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UnloadDate",OldValue=unloadDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   unloadDate=value;
		   }
			
		 }
	   }
	  private string unloadPortCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UnloadPortCode  
	   {
	    
	     get
		{
		   return unloadPortCode;
		 }
		 set
		 {
		   if(unloadPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UnloadPortCode",OldValue=unloadPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   unloadPortCode=value;
		   }
			
		 }
	   }
	  private string unloadPortName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UnloadPortName  
	   {
	    
	     get
		{
		   return unloadPortName;
		 }
		 set
		 {
		   if(unloadPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UnloadPortName",OldValue=unloadPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   unloadPortName=value;
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
	  private string isLastReleaseFromWarehous ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsLastReleaseFromWarehous  
	   {
	    
	     get
		{
		   return isLastReleaseFromWarehous;
		 }
		 set
		 {
		   if(isLastReleaseFromWarehous != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsLastReleaseFromWarehous",OldValue=isLastReleaseFromWarehous,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isLastReleaseFromWarehous=value;
		   }
			
		 }
	   }
	  private string loadingPortCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LoadingPortCode  
	   {
	    
	     get
		{
		   return loadingPortCode;
		 }
		 set
		 {
		   if(loadingPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LoadingPortCode",OldValue=loadingPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   loadingPortCode=value;
		   }
			
		 }
	   }
	  private string originCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginCountryCode  
	   {
	    
	     get
		{
		   return originCountryCode;
		 }
		 set
		 {
		   if(originCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginCountryCode",OldValue=originCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originCountryCode=value;
		   }
			
		 }
	   }
	  private string originCountryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginCountryName  
	   {
	    
	     get
		{
		   return originCountryName;
		 }
		 set
		 {
		   if(originCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginCountryName",OldValue=originCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originCountryName=value;
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
	  private string receiverWarehouseCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReceiverWarehouseCode  
	   {
	    
	     get
		{
		   return receiverWarehouseCode;
		 }
		 set
		 {
		   if(receiverWarehouseCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReceiverWarehouseCode",OldValue=receiverWarehouseCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   receiverWarehouseCode=value;
		   }
			
		 }
	   }
	  private string receiverWarehouseName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReceiverWarehouseName  
	   {
	    
	     get
		{
		   return receiverWarehouseName;
		 }
		 set
		 {
		   if(receiverWarehouseName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReceiverWarehouseName",OldValue=receiverWarehouseName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   receiverWarehouseName=value;
		   }
			
		 }
	   }

	   private List<ConsignmentPackagePM> consignmentPackages;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ConsignmentConsignmentPackage", "DeclarationId,ConsignmentNumber","DeclarationId,ConsignmentNumber")]
	   [DataMember]
	   public virtual List<ConsignmentPackagePM> ConsignmentPackages  
	   {
	        get
             {
                 if (consignmentPackages == null)
                 {
                     consignmentPackages = new List<ConsignmentPackagePM>();
                 }
                 return consignmentPackages;
              }
             set { consignmentPackages = value; }
	    }
		   
	   private List<ConsignmentPackagePM>  deletedConsignmentPackages;
	   public virtual List<ConsignmentPackagePM> DeletedConsignmentPackages  
	   {
	        get
             {
                 if ( deletedConsignmentPackages == null)
                 {
                      deletedConsignmentPackages = new List<ConsignmentPackagePM>();
                 }
                 return  deletedConsignmentPackages;
              }
             set {  deletedConsignmentPackages = value; }
	    }
	  
	   private List<ConsignmentInternalTransitionPM> consignmentInternalTransitions;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ConsignmentConsignmentInternalTransition", "DeclarationId,ConsignmentNumber","DeclarationId,ConsignmentNumber")]
	   [DataMember]
	   public virtual List<ConsignmentInternalTransitionPM> ConsignmentInternalTransitions  
	   {
	        get
             {
                 if (consignmentInternalTransitions == null)
                 {
                     consignmentInternalTransitions = new List<ConsignmentInternalTransitionPM>();
                 }
                 return consignmentInternalTransitions;
              }
             set { consignmentInternalTransitions = value; }
	    }
		   
	   private List<ConsignmentInternalTransitionPM>  deletedConsignmentInternalTransitions;
	   public virtual List<ConsignmentInternalTransitionPM> DeletedConsignmentInternalTransitions  
	   {
	        get
             {
                 if ( deletedConsignmentInternalTransitions == null)
                 {
                      deletedConsignmentInternalTransitions = new List<ConsignmentInternalTransitionPM>();
                 }
                 return  deletedConsignmentInternalTransitions;
              }
             set {  deletedConsignmentInternalTransitions = value; }
	    }
	  	  private DateTime? cargoDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CargoDate  
	   {
	    
	     get
		{
		   return cargoDate;
		 }
		 set
		 {
		   if(cargoDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoDate",OldValue=cargoDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   cargoDate=value;
		   }
			
		 }
	   }
	  private string deliveryPlaceName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliveryPlaceName  
	   {
	    
	     get
		{
		   return deliveryPlaceName;
		 }
		 set
		 {
		   if(deliveryPlaceName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveryPlaceName",OldValue=deliveryPlaceName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliveryPlaceName=value;
		   }
			
		 }
	   }
	  private bool isDangerousGoods ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDangerousGoods  
	   {
	    
	     get
		{
		   return isDangerousGoods;
		 }
		 set
		 {
		   if(isDangerousGoods != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDangerousGoods",OldValue=isDangerousGoods,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDangerousGoods=value;
		   }
			
		 }
	   }
	  private string finalDestinationPortCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalDestinationPortCode  
	   {
	    
	     get
		{
		   return finalDestinationPortCode;
		 }
		 set
		 {
		   if(finalDestinationPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalDestinationPortCode",OldValue=finalDestinationPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalDestinationPortCode=value;
		   }
			
		 }
	   }
	  private string finalDestinationPortName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalDestinationPortName  
	   {
	    
	     get
		{
		   return finalDestinationPortName;
		 }
		 set
		 {
		   if(finalDestinationPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalDestinationPortName",OldValue=finalDestinationPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalDestinationPortName=value;
		   }
			
		 }
	   }
	  private string exportRecieverWareHouseCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportRecieverWareHouseCode  
	   {
	    
	     get
		{
		   return exportRecieverWareHouseCode;
		 }
		 set
		 {
		   if(exportRecieverWareHouseCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportRecieverWareHouseCode",OldValue=exportRecieverWareHouseCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportRecieverWareHouseCode=value;
		   }
			
		 }
	   }
	  private string exportRecieverWareHouseName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportRecieverWareHouseName  
	   {
	    
	     get
		{
		   return exportRecieverWareHouseName;
		 }
		 set
		 {
		   if(exportRecieverWareHouseName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportRecieverWareHouseName",OldValue=exportRecieverWareHouseName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportRecieverWareHouseName=value;
		   }
			
		 }
	   }
	  private string exportUnloadingPortCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportUnloadingPortCode  
	   {
	    
	     get
		{
		   return exportUnloadingPortCode;
		 }
		 set
		 {
		   if(exportUnloadingPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportUnloadingPortCode",OldValue=exportUnloadingPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportUnloadingPortCode=value;
		   }
			
		 }
	   }
	  private string exportLoadingPortCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportLoadingPortCode  
	   {
	    
	     get
		{
		   return exportLoadingPortCode;
		 }
		 set
		 {
		   if(exportLoadingPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportLoadingPortCode",OldValue=exportLoadingPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportLoadingPortCode=value;
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
	  private string consignmentType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsignmentType  
	   {
	    
	     get
		{
		   return consignmentType;
		 }
		 set
		 {
		   if(consignmentType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsignmentType",OldValue=consignmentType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consignmentType=value;
		   }
			
		 }
	   }
	  private string deliverySiteCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliverySiteCode  
	   {
	    
	     get
		{
		   return deliverySiteCode;
		 }
		 set
		 {
		   if(deliverySiteCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliverySiteCode",OldValue=deliverySiteCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliverySiteCode=value;
		   }
			
		 }
	   }
	  private string exportStoragesId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportStoragesId  
	   {
	    
	     get
		{
		   return exportStoragesId;
		 }
		 set
		 {
		   if(exportStoragesId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportStoragesId",OldValue=exportStoragesId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportStoragesId=value;
		   }
			
		 }
	   }
	  private string cargoTypeCodeForExport ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CargoTypeCodeForExport  
	   {
	    
	     get
		{
		   return cargoTypeCodeForExport;
		 }
		 set
		 {
		   if(cargoTypeCodeForExport != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CargoTypeCodeForExport",OldValue=cargoTypeCodeForExport,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cargoTypeCodeForExport=value;
		   }
			
		 }
	   }
	  private string storageSiteCodeExport ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StorageSiteCodeExport  
	   {
	    
	     get
		{
		   return storageSiteCodeExport;
		 }
		 set
		 {
		   if(storageSiteCodeExport != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StorageSiteCodeExport",OldValue=storageSiteCodeExport,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   storageSiteCodeExport=value;
		   }
			
		 }
	   }
	  private string exportContainerizationID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportContainerizationID  
	   {
	    
	     get
		{
		   return exportContainerizationID;
		 }
		 set
		 {
		   if(exportContainerizationID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportContainerizationID",OldValue=exportContainerizationID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportContainerizationID=value;
		   }
			
		 }
	   }
	    }
   
}
	 