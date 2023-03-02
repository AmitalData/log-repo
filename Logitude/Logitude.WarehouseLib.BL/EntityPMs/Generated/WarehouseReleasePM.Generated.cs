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
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.WarehouseLib.BL.Validators;
  
namespace Logitude.WarehouseLib.BL.EntityPMs
{
   [CustomValidation(typeof(WarehouseClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class WarehouseReleasePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  private string releaseNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReleaseNumber  
	   {
	    
	     get
		{
		   return releaseNumber;
		 }
		 set
		 {
		   if(releaseNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReleaseNumber",OldValue=releaseNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   releaseNumber=value;
		   }
			
		 }
	   }
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerId  
	   {
	    
	     get
		{
		   return customerId;
		 }
		 set
		 {
		   if(customerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerId",OldValue=customerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerId=value;
		   }
			
		 }
	   }
	  private string shipmentId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentId  
	   {
	    
	     get
		{
		   return shipmentId;
		 }
		 set
		 {
		   if(shipmentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentId",OldValue=shipmentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentId=value;
		   }
			
		 }
	   }
	  private string shipmentNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentNumber  
	   {
	    
	     get
		{
		   return shipmentNumber;
		 }
		 set
		 {
		   if(shipmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentNumber",OldValue=shipmentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentNumber=value;
		   }
			
		 }
	   }
	  private string warehouseId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string WarehouseId  
	   {
	    
	     get
		{
		   return warehouseId;
		 }
		 set
		 {
		   if(warehouseId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WarehouseId",OldValue=warehouseId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   warehouseId=value;
		   }
			
		 }
	   }
	  private DateTime? expectedReleaseDate ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ExpectedReleaseDate  
	   {
	    
	     get
		{
		   return expectedReleaseDate;
		 }
		 set
		 {
		   if(expectedReleaseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExpectedReleaseDate",OldValue=expectedReleaseDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   expectedReleaseDate=value;
		   }
			
		 }
	   }
	  private DateTime? actualReleaseDate ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ActualReleaseDate  
	   {
	    
	     get
		{
		   return actualReleaseDate;
		 }
		 set
		 {
		   if(actualReleaseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActualReleaseDate",OldValue=actualReleaseDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   actualReleaseDate=value;
		   }
			
		 }
	   }
	  private string releaseBy ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReleaseBy  
	   {
	    
	     get
		{
		   return releaseBy;
		 }
		 set
		 {
		   if(releaseBy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReleaseBy",OldValue=releaseBy,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   releaseBy=value;
		   }
			
		 }
	   }
	  private string specialInstruction ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecialInstruction  
	   {
	    
	     get
		{
		   return specialInstruction;
		 }
		 set
		 {
		   if(specialInstruction != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecialInstruction",OldValue=specialInstruction,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specialInstruction=value;
		   }
			
		 }
	   }
	  private string statusCode ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusCode  
	   {
	    
	     get
		{
		   return statusCode;
		 }
		 set
		 {
		   if(statusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusCode",OldValue=statusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusCode=value;
		   }
			
		 }
	   }
	  private int totalPieces ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public int TotalPieces  
	   {
	    
	     get
		{
		   return totalPieces;
		 }
		 set
		 {
		   if(totalPieces != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalPieces",OldValue=totalPieces,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   totalPieces=value;
		   }
			
		 }
	   }
	  private decimal totalGrossWeight ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal TotalGrossWeight  
	   {
	    
	     get
		{
		   return totalGrossWeight;
		 }
		 set
		 {
		   if(totalGrossWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalGrossWeight",OldValue=totalGrossWeight,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   totalGrossWeight=value;
		   }
			
		 }
	   }
	  private string grossWeightUnitCode ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string GrossWeightUnitCode  
	   {
	    
	     get
		{
		   return grossWeightUnitCode;
		 }
		 set
		 {
		   if(grossWeightUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeightUnitCode",OldValue=grossWeightUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   grossWeightUnitCode=value;
		   }
			
		 }
	   }
	  private decimal totalVolume ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal TotalVolume  
	   {
	    
	     get
		{
		   return totalVolume;
		 }
		 set
		 {
		   if(totalVolume != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalVolume",OldValue=totalVolume,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   totalVolume=value;
		   }
			
		 }
	   }
	  private string volumeUnitCode ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string VolumeUnitCode  
	   {
	    
	     get
		{
		   return volumeUnitCode;
		 }
		 set
		 {
		   if(volumeUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumeUnitCode",OldValue=volumeUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   volumeUnitCode=value;
		   }
			
		 }
	   }
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notes  
	   {
	    
	     get
		{
		   return notes;
		 }
		 set
		 {
		   if(notes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notes",OldValue=notes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notes=value;
		   }
			
		 }
	   }
	  private string customerRef1 ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerRef1  
	   {
	    
	     get
		{
		   return customerRef1;
		 }
		 set
		 {
		   if(customerRef1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerRef1",OldValue=customerRef1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerRef1=value;
		   }
			
		 }
	   }
	  private string customerRef2 ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerRef2  
	   {
	    
	     get
		{
		   return customerRef2;
		 }
		 set
		 {
		   if(customerRef2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerRef2",OldValue=customerRef2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerRef2=value;
		   }
			
		 }
	   }
	  private string houseNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string HouseNumber  
	   {
	    
	     get
		{
		   return houseNumber;
		 }
		 set
		 {
		   if(houseNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HouseNumber",OldValue=houseNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   houseNumber=value;
		   }
			
		 }
	   }
	  private string masterNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string MasterNumber  
	   {
	    
	     get
		{
		   return masterNumber;
		 }
		 set
		 {
		   if(masterNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MasterNumber",OldValue=masterNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   masterNumber=value;
		   }
			
		 }
	   }

	   private List<WarehouseReleasePackagePM> warehouseReleasePackages;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("WarehouseReleasePackageWarehouseRelease", "Id","WarehouseReleaseId")]
	   [DataMember]
	   public virtual List<WarehouseReleasePackagePM> WarehouseReleasePackages  
	   {
	        get
             {
                 if (warehouseReleasePackages == null)
                 {
                     warehouseReleasePackages = new List<WarehouseReleasePackagePM>();
                 }
                 return warehouseReleasePackages;
              }
             set { warehouseReleasePackages = value; }
	    }
		   
	   private List<WarehouseReleasePackagePM>  deletedWarehouseReleasePackages;
	   public virtual List<WarehouseReleasePackagePM> DeletedWarehouseReleasePackages  
	   {
	        get
             {
                 if ( deletedWarehouseReleasePackages == null)
                 {
                      deletedWarehouseReleasePackages = new List<WarehouseReleasePackagePM>();
                 }
                 return  deletedWarehouseReleasePackages;
              }
             set {  deletedWarehouseReleasePackages = value; }
	    }
	  	  private string warehouseName ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string WarehouseName  
	   {
	    
	     get
		{
		   return warehouseName;
		 }
		 set
		 {
		   if(warehouseName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WarehouseName",OldValue=warehouseName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   warehouseName=value;
		   }
			
		 }
	   }
	  private string customerName ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerName  
	   {
	    
	     get
		{
		   return customerName;
		 }
		 set
		 {
		   if(customerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerName",OldValue=customerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerName=value;
		   }
			
		 }
	   }
	  private string references ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string References  
	   {
	    
	     get
		{
		   return references;
		 }
		 set
		 {
		   if(references != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="References",OldValue=references,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   references=value;
		   }
			
		 }
	   }
	  private string statusName ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusName  
	   {
	    
	     get
		{
		   return statusName;
		 }
		 set
		 {
		   if(statusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusName",OldValue=statusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusName=value;
		   }
			
		 }
	   }
	  private string dimensionsUnitCode ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string DimensionsUnitCode  
	   {
	    
	     get
		{
		   return dimensionsUnitCode;
		 }
		 set
		 {
		   if(dimensionsUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DimensionsUnitCode",OldValue=dimensionsUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dimensionsUnitCode=value;
		   }
			
		 }
	   }
	  private string shipmentTypeId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentTypeId  
	   {
	    
	     get
		{
		   return shipmentTypeId;
		 }
		 set
		 {
		   if(shipmentTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentTypeId",OldValue=shipmentTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentTypeId=value;
		   }
			
		 }
	   }
	  private string shipmentNumberWithType ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentNumberWithType  
	   {
	    
	     get
		{
		   return shipmentNumberWithType;
		 }
		 set
		 {
		   if(shipmentNumberWithType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentNumberWithType",OldValue=shipmentNumberWithType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentNumberWithType=value;
		   }
			
		 }
	   }
	  private string transportModeId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  private string shipmentLevelCode ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentLevelCode  
	   {
	    
	     get
		{
		   return shipmentLevelCode;
		 }
		 set
		 {
		   if(shipmentLevelCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentLevelCode",OldValue=shipmentLevelCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentLevelCode=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  private string directionId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string DirectionId  
	   {
	    
	     get
		{
		   return directionId;
		 }
		 set
		 {
		   if(directionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DirectionId",OldValue=directionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   directionId=value;
		   }
			
		 }
	   }
	  private DateTime? releaseDate ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ReleaseDate  
	   {
	    
	     get
		{
		   return releaseDate;
		 }
		 set
		 {
		   if(releaseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReleaseDate",OldValue=releaseDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   releaseDate=value;
		   }
			
		 }
	   }
	  private int totalQuantity ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public int TotalQuantity  
	   {
	    
	     get
		{
		   return totalQuantity;
		 }
		 set
		 {
		   if(totalQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalQuantity",OldValue=totalQuantity,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   totalQuantity=value;
		   }
			
		 }
	   }
	  private string chargeableWeightUnitCode ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargeableWeightUnitCode  
	   {
	    
	     get
		{
		   return chargeableWeightUnitCode;
		 }
		 set
		 {
		   if(chargeableWeightUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeightUnitCode",OldValue=chargeableWeightUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargeableWeightUnitCode=value;
		   }
			
		 }
	   }
	  private string connectedTo ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConnectedTo  
	   {
	    
	     get
		{
		   return connectedTo;
		 }
		 set
		 {
		   if(connectedTo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedTo",OldValue=connectedTo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   connectedTo=value;
		   }
			
		 }
	   }
	  private string fromPortId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortId  
	   {
	    
	     get
		{
		   return fromPortId;
		 }
		 set
		 {
		   if(fromPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortId",OldValue=fromPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortId=value;
		   }
			
		 }
	   }
	  private string toPortId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortId  
	   {
	    
	     get
		{
		   return toPortId;
		 }
		 set
		 {
		   if(toPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortId",OldValue=toPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortId=value;
		   }
			
		 }
	   }
	  private string customerAddressId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerAddressId  
	   {
	    
	     get
		{
		   return customerAddressId;
		 }
		 set
		 {
		   if(customerAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerAddressId",OldValue=customerAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerAddressId=value;
		   }
			
		 }
	   }
	  private decimal totalVolumetricWeight ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal TotalVolumetricWeight  
	   {
	    
	     get
		{
		   return totalVolumetricWeight;
		 }
		 set
		 {
		   if(totalVolumetricWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalVolumetricWeight",OldValue=totalVolumetricWeight,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   totalVolumetricWeight=value;
		   }
			
		 }
	   }
	  private double? ratio ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Ratio  
	   {
	    
	     get
		{
		   return ratio;
		 }
		 set
		 {
		   if(ratio != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Ratio",OldValue=ratio,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   ratio=value;
		   }
			
		 }
	   }
	  private string toTypeCode ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToTypeCode  
	   {
	    
	     get
		{
		   return toTypeCode;
		 }
		 set
		 {
		   if(toTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToTypeCode",OldValue=toTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toTypeCode=value;
		   }
			
		 }
	   }
	  private string toPartnerCardId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPartnerCardId  
	   {
	    
	     get
		{
		   return toPartnerCardId;
		 }
		 set
		 {
		   if(toPartnerCardId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPartnerCardId",OldValue=toPartnerCardId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPartnerCardId=value;
		   }
			
		 }
	   }
	  private string toAddressId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressId  
	   {
	    
	     get
		{
		   return toAddressId;
		 }
		 set
		 {
		   if(toAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressId",OldValue=toAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressId=value;
		   }
			
		 }
	   }
	  private string toAddressZipCode ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressZipCode  
	   {
	    
	     get
		{
		   return toAddressZipCode;
		 }
		 set
		 {
		   if(toAddressZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressZipCode",OldValue=toAddressZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressZipCode=value;
		   }
			
		 }
	   }
	  private string toAddressCity ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressCity  
	   {
	    
	     get
		{
		   return toAddressCity;
		 }
		 set
		 {
		   if(toAddressCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressCity",OldValue=toAddressCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressCity=value;
		   }
			
		 }
	   }
	  private string toAddressCountryId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressCountryId  
	   {
	    
	     get
		{
		   return toAddressCountryId;
		 }
		 set
		 {
		   if(toAddressCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressCountryId",OldValue=toAddressCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressCountryId=value;
		   }
			
		 }
	   }
	  private bool isUsed ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsUsed  
	   {
	    
	     get
		{
		   return isUsed;
		 }
		 set
		 {
		   if(isUsed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsUsed",OldValue=isUsed,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isUsed=value;
		   }
			
		 }
	   }
	  private string destination ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Destination  
	   {
	    
	     get
		{
		   return destination;
		 }
		 set
		 {
		   if(destination != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Destination",OldValue=destination,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destination=value;
		   }
			
		 }
	   }
	  private string truckerId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  private string truckerReference ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string TruckerReference  
	   {
	    
	     get
		{
		   return truckerReference;
		 }
		 set
		 {
		   if(truckerReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TruckerReference",OldValue=truckerReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   truckerReference=value;
		   }
			
		 }
	   }
	  private string childEntityReference ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChildEntityReference  
	   {
	    
	     get
		{
		   return childEntityReference;
		 }
		 set
		 {
		   if(childEntityReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChildEntityReference",OldValue=childEntityReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   childEntityReference=value;
		   }
			
		 }
	   }
	  private string masterShipmentNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string MasterShipmentNumber  
	   {
	    
	     get
		{
		   return masterShipmentNumber;
		 }
		 set
		 {
		   if(masterShipmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MasterShipmentNumber",OldValue=masterShipmentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   masterShipmentNumber=value;
		   }
			
		 }
	   }
	  private bool isUpdateByAutomation ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsUpdateByAutomation  
	   {
	    
	     get
		{
		   return isUpdateByAutomation;
		 }
		 set
		 {
		   if(isUpdateByAutomation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsUpdateByAutomation",OldValue=isUpdateByAutomation,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isUpdateByAutomation=value;
		   }
			
		 }
	   }
	  private string customerPrimaryContactId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerPrimaryContactId  
	   {
	    
	     get
		{
		   return customerPrimaryContactId;
		 }
		 set
		 {
		   if(customerPrimaryContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerPrimaryContactId",OldValue=customerPrimaryContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerPrimaryContactId=value;
		   }
			
		 }
	   }

	   private List<CustomChildEntity> customChildEntities;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public List<CustomChildEntity> CustomChildEntities
	   {
	     get { return customChildEntities; }
		 set {
		   if(customChildEntities == value) return;
		   NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomChildEntities",OldValue=customChildEntities,NewValue=value,PropertyType="List<CustomChildEntity>"};
		   NotifyPropertyChanged(values);
		   customChildEntities=value;
		 }
	   }
	    }
   
}
	 