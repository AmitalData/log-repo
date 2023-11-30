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
   public partial class WarehouseEntryPM : EntityPM
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
	  private string entryNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntryNumber  
	   {
	    
	     get
		{
		   return entryNumber;
		 }
		 set
		 {
		   if(entryNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntryNumber",OldValue=entryNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entryNumber=value;
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
	  private DateTime? expectedEntryDate ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ExpectedEntryDate  
	   {
	    
	     get
		{
		   return expectedEntryDate;
		 }
		 set
		 {
		   if(expectedEntryDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExpectedEntryDate",OldValue=expectedEntryDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   expectedEntryDate=value;
		   }
			
		 }
	   }
	  private DateTime? actualEntryDate ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ActualEntryDate  
	   {
	    
	     get
		{
		   return actualEntryDate;
		 }
		 set
		 {
		   if(actualEntryDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActualEntryDate",OldValue=actualEntryDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   actualEntryDate=value;
		   }
			
		 }
	   }
	  private string receivedBy ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReceivedBy  
	   {
	    
	     get
		{
		   return receivedBy;
		 }
		 set
		 {
		   if(receivedBy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReceivedBy",OldValue=receivedBy,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   receivedBy=value;
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

	   private List<WarehouseEntryPackagePM> warehouseEntryPackages;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("WarehouseEntryPackageWarehouseEntry", "Id","WarehouseEntryId")]
	   [DataMember]
	   public virtual List<WarehouseEntryPackagePM> WarehouseEntryPackages  
	   {
	        get
             {
                 if (warehouseEntryPackages == null)
                 {
                     warehouseEntryPackages = new List<WarehouseEntryPackagePM>();
                 }
                 return warehouseEntryPackages;
              }
             set { warehouseEntryPackages = value; }
	    }
		   
	   private List<WarehouseEntryPackagePM>  deletedWarehouseEntryPackages;
	   public virtual List<WarehouseEntryPackagePM> DeletedWarehouseEntryPackages  
	   {
	        get
             {
                 if ( deletedWarehouseEntryPackages == null)
                 {
                      deletedWarehouseEntryPackages = new List<WarehouseEntryPackagePM>();
                 }
                 return  deletedWarehouseEntryPackages;
              }
             set {  deletedWarehouseEntryPackages = value; }
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
	  private string shipperId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperId  
	   {
	    
	     get
		{
		   return shipperId;
		 }
		 set
		 {
		   if(shipperId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperId",OldValue=shipperId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperId=value;
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
	  private string entryReference ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntryReference  
	   {
	    
	     get
		{
		   return entryReference;
		 }
		 set
		 {
		   if(entryReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntryReference",OldValue=entryReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entryReference=value;
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
	  private string origin ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Origin  
	   {
	    
	     get
		{
		   return origin;
		 }
		 set
		 {
		   if(origin != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Origin",OldValue=origin,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   origin=value;
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
	  private string mainCarriageCarrierName ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierName  
	   {
	    
	     get
		{
		   return mainCarriageCarrierName;
		 }
		 set
		 {
		   if(mainCarriageCarrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierName",OldValue=mainCarriageCarrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierName=value;
		   }
			
		 }
	   }
	  private string routing ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Routing  
	   {
	    
	     get
		{
		   return routing;
		 }
		 set
		 {
		   if(routing != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Routing",OldValue=routing,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   routing=value;
		   }
			
		 }
	   }
	  private bool connectedToShipment ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ConnectedToShipment  
	   {
	    
	     get
		{
		   return connectedToShipment;
		 }
		 set
		 {
		   if(connectedToShipment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedToShipment",OldValue=connectedToShipment,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   connectedToShipment=value;
		   }
			
		 }
	   }
	  private string fromAddressId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromAddressId  
	   {
	    
	     get
		{
		   return fromAddressId;
		 }
		 set
		 {
		   if(fromAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromAddressId",OldValue=fromAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromAddressId=value;
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
	  private string consigneeId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeId  
	   {
	    
	     get
		{
		   return consigneeId;
		 }
		 set
		 {
		   if(consigneeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeId",OldValue=consigneeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeId=value;
		   }
			
		 }
	   }
	  private string shipperReference1 ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperReference1  
	   {
	    
	     get
		{
		   return shipperReference1;
		 }
		 set
		 {
		   if(shipperReference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperReference1",OldValue=shipperReference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperReference1=value;
		   }
			
		 }
	   }
	  private string consigneeReference1 ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeReference1  
	   {
	    
	     get
		{
		   return consigneeReference1;
		 }
		 set
		 {
		   if(consigneeReference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeReference1",OldValue=consigneeReference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeReference1=value;
		   }
			
		 }
	   }
	  private string consigneeReference2 ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeReference2  
	   {
	    
	     get
		{
		   return consigneeReference2;
		 }
		 set
		 {
		   if(consigneeReference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeReference2",OldValue=consigneeReference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeReference2=value;
		   }
			
		 }
	   }
	  private string shipperReference2 ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperReference2  
	   {
	    
	     get
		{
		   return shipperReference2;
		 }
		 set
		 {
		   if(shipperReference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperReference2",OldValue=shipperReference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperReference2=value;
		   }
			
		 }
	   }
	  private string shipperName ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperName  
	   {
	    
	     get
		{
		   return shipperName;
		 }
		 set
		 {
		   if(shipperName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperName",OldValue=shipperName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperName=value;
		   }
			
		 }
	   }
	  private string consigneeName ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  private string manufacturer ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Manufacturer  
	   {
	    
	     get
		{
		   return manufacturer;
		 }
		 set
		 {
		   if(manufacturer != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Manufacturer",OldValue=manufacturer,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manufacturer=value;
		   }
			
		 }
	   }
	  private string fromPartnerId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPartnerId  
	   {
	    
	     get
		{
		   return fromPartnerId;
		 }
		 set
		 {
		   if(fromPartnerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPartnerId",OldValue=fromPartnerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPartnerId=value;
		   }
			
		 }
	   }
	  private string toPartnerId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPartnerId  
	   {
	    
	     get
		{
		   return toPartnerId;
		 }
		 set
		 {
		   if(toPartnerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPartnerId",OldValue=toPartnerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPartnerId=value;
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
	  private DateTime? lastStatusUpdateDate ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastStatusUpdateDate  
	   {
	    
	     get
		{
		   return lastStatusUpdateDate;
		 }
		 set
		 {
		   if(lastStatusUpdateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastStatusUpdateDate",OldValue=lastStatusUpdateDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastStatusUpdateDate=value;
		   }
			
		 }
	   }
	  private string masterHouse ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string MasterHouse  
	   {
	    
	     get
		{
		   return masterHouse;
		 }
		 set
		 {
		   if(masterHouse != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MasterHouse",OldValue=masterHouse,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   masterHouse=value;
		   }
			
		 }
	   }
	  private string entryReferencesAndDate ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntryReferencesAndDate  
	   {
	    
	     get
		{
		   return entryReferencesAndDate;
		 }
		 set
		 {
		   if(entryReferencesAndDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntryReferencesAndDate",OldValue=entryReferencesAndDate,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entryReferencesAndDate=value;
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
	  private string fromTypeCode ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromTypeCode  
	   {
	    
	     get
		{
		   return fromTypeCode;
		 }
		 set
		 {
		   if(fromTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromTypeCode",OldValue=fromTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromTypeCode=value;
		   }
			
		 }
	   }
	  private string fromCountryId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromCountryId  
	   {
	    
	     get
		{
		   return fromCountryId;
		 }
		 set
		 {
		   if(fromCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromCountryId",OldValue=fromCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromCountryId=value;
		   }
			
		 }
	   }
	  private string toCountryId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToCountryId  
	   {
	    
	     get
		{
		   return toCountryId;
		 }
		 set
		 {
		   if(toCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToCountryId",OldValue=toCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toCountryId=value;
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
	  private string connectedToReferenceNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConnectedToReferenceNumber  
	   {
	    
	     get
		{
		   return connectedToReferenceNumber;
		 }
		 set
		 {
		   if(connectedToReferenceNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedToReferenceNumber",OldValue=connectedToReferenceNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   connectedToReferenceNumber=value;
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
	  private string shipperPrimaryContactId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperPrimaryContactId  
	   {
	    
	     get
		{
		   return shipperPrimaryContactId;
		 }
		 set
		 {
		   if(shipperPrimaryContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperPrimaryContactId",OldValue=shipperPrimaryContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperPrimaryContactId=value;
		   }
			
		 }
	   }
	  private string consigneePrimaryContactId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneePrimaryContactId  
	   {
	    
	     get
		{
		   return consigneePrimaryContactId;
		 }
		 set
		 {
		   if(consigneePrimaryContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneePrimaryContactId",OldValue=consigneePrimaryContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneePrimaryContactId=value;
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
	 	   private CustomFieldClass field1;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field1  
	   {
	    
	     get
		{
		   return field1;
		 }
		 set
		 {
		   if(field1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field1",OldValue=field1,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field1=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field2;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field2  
	   {
	    
	     get
		{
		   return field2;
		 }
		 set
		 {
		   if(field2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field2",OldValue=field2,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field2=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field3;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field3  
	   {
	    
	     get
		{
		   return field3;
		 }
		 set
		 {
		   if(field3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field3",OldValue=field3,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field3=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field4;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field4  
	   {
	    
	     get
		{
		   return field4;
		 }
		 set
		 {
		   if(field4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field4",OldValue=field4,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field4=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field5;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field5  
	   {
	    
	     get
		{
		   return field5;
		 }
		 set
		 {
		   if(field5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field5",OldValue=field5,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field5=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field6;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field6  
	   {
	    
	     get
		{
		   return field6;
		 }
		 set
		 {
		   if(field6 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field6",OldValue=field6,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field6=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field7;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field7  
	   {
	    
	     get
		{
		   return field7;
		 }
		 set
		 {
		   if(field7 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field7",OldValue=field7,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field7=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field8;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field8  
	   {
	    
	     get
		{
		   return field8;
		 }
		 set
		 {
		   if(field8 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field8",OldValue=field8,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field8=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field9;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field9  
	   {
	    
	     get
		{
		   return field9;
		 }
		 set
		 {
		   if(field9 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field9",OldValue=field9,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field9=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field10;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field10  
	   {
	    
	     get
		{
		   return field10;
		 }
		 set
		 {
		   if(field10 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field10",OldValue=field10,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field10=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field11;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field11  
	   {
	    
	     get
		{
		   return field11;
		 }
		 set
		 {
		   if(field11 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field11",OldValue=field11,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field11=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field12;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field12  
	   {
	    
	     get
		{
		   return field12;
		 }
		 set
		 {
		   if(field12 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field12",OldValue=field12,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field12=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field13;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field13  
	   {
	    
	     get
		{
		   return field13;
		 }
		 set
		 {
		   if(field13 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field13",OldValue=field13,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field13=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field14;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field14  
	   {
	    
	     get
		{
		   return field14;
		 }
		 set
		 {
		   if(field14 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field14",OldValue=field14,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field14=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field15;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field15  
	   {
	    
	     get
		{
		   return field15;
		 }
		 set
		 {
		   if(field15 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field15",OldValue=field15,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field15=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field16;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field16  
	   {
	    
	     get
		{
		   return field16;
		 }
		 set
		 {
		   if(field16 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field16",OldValue=field16,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field16=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field17;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field17  
	   {
	    
	     get
		{
		   return field17;
		 }
		 set
		 {
		   if(field17 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field17",OldValue=field17,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field17=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field18;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field18  
	   {
	    
	     get
		{
		   return field18;
		 }
		 set
		 {
		   if(field18 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field18",OldValue=field18,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field18=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field19;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field19  
	   {
	    
	     get
		{
		   return field19;
		 }
		 set
		 {
		   if(field19 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field19",OldValue=field19,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field19=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field20;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field20  
	   {
	    
	     get
		{
		   return field20;
		 }
		 set
		 {
		   if(field20 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field20",OldValue=field20,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field20=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field21;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field21  
	   {
	    
	     get
		{
		   return field21;
		 }
		 set
		 {
		   if(field21 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field21",OldValue=field21,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field21=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field22;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field22  
	   {
	    
	     get
		{
		   return field22;
		 }
		 set
		 {
		   if(field22 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field22",OldValue=field22,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field22=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field23;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field23  
	   {
	    
	     get
		{
		   return field23;
		 }
		 set
		 {
		   if(field23 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field23",OldValue=field23,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field23=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field24;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field24  
	   {
	    
	     get
		{
		   return field24;
		 }
		 set
		 {
		   if(field24 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field24",OldValue=field24,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field24=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field25;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field25  
	   {
	    
	     get
		{
		   return field25;
		 }
		 set
		 {
		   if(field25 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field25",OldValue=field25,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field25=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field26;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field26  
	   {
	    
	     get
		{
		   return field26;
		 }
		 set
		 {
		   if(field26 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field26",OldValue=field26,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field26=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field27;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field27  
	   {
	    
	     get
		{
		   return field27;
		 }
		 set
		 {
		   if(field27 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field27",OldValue=field27,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field27=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field28;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field28  
	   {
	    
	     get
		{
		   return field28;
		 }
		 set
		 {
		   if(field28 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field28",OldValue=field28,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field28=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field29;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field29  
	   {
	    
	     get
		{
		   return field29;
		 }
		 set
		 {
		   if(field29 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field29",OldValue=field29,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field29=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field30;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field30  
	   {
	    
	     get
		{
		   return field30;
		 }
		 set
		 {
		   if(field30 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field30",OldValue=field30,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field30=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field31;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field31  
	   {
	    
	     get
		{
		   return field31;
		 }
		 set
		 {
		   if(field31 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field31",OldValue=field31,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field31=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field32;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field32  
	   {
	    
	     get
		{
		   return field32;
		 }
		 set
		 {
		   if(field32 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field32",OldValue=field32,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field32=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field33;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field33  
	   {
	    
	     get
		{
		   return field33;
		 }
		 set
		 {
		   if(field33 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field33",OldValue=field33,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field33=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field34;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field34  
	   {
	    
	     get
		{
		   return field34;
		 }
		 set
		 {
		   if(field34 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field34",OldValue=field34,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field34=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field35;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field35  
	   {
	    
	     get
		{
		   return field35;
		 }
		 set
		 {
		   if(field35 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field35",OldValue=field35,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field35=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field36;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field36  
	   {
	    
	     get
		{
		   return field36;
		 }
		 set
		 {
		   if(field36 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field36",OldValue=field36,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field36=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field37;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field37  
	   {
	    
	     get
		{
		   return field37;
		 }
		 set
		 {
		   if(field37 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field37",OldValue=field37,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field37=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field38;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field38  
	   {
	    
	     get
		{
		   return field38;
		 }
		 set
		 {
		   if(field38 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field38",OldValue=field38,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field38=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field39;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field39  
	   {
	    
	     get
		{
		   return field39;
		 }
		 set
		 {
		   if(field39 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field39",OldValue=field39,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field39=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field40;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field40  
	   {
	    
	     get
		{
		   return field40;
		 }
		 set
		 {
		   if(field40 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field40",OldValue=field40,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field40=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field41;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field41  
	   {
	    
	     get
		{
		   return field41;
		 }
		 set
		 {
		   if(field41 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field41",OldValue=field41,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field41=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field42;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field42  
	   {
	    
	     get
		{
		   return field42;
		 }
		 set
		 {
		   if(field42 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field42",OldValue=field42,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field42=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field43;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field43  
	   {
	    
	     get
		{
		   return field43;
		 }
		 set
		 {
		   if(field43 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field43",OldValue=field43,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field43=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field44;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field44  
	   {
	    
	     get
		{
		   return field44;
		 }
		 set
		 {
		   if(field44 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field44",OldValue=field44,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field44=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field45;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field45  
	   {
	    
	     get
		{
		   return field45;
		 }
		 set
		 {
		   if(field45 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field45",OldValue=field45,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field45=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field46;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field46  
	   {
	    
	     get
		{
		   return field46;
		 }
		 set
		 {
		   if(field46 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field46",OldValue=field46,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field46=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field47;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field47  
	   {
	    
	     get
		{
		   return field47;
		 }
		 set
		 {
		   if(field47 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field47",OldValue=field47,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field47=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field48;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field48  
	   {
	    
	     get
		{
		   return field48;
		 }
		 set
		 {
		   if(field48 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field48",OldValue=field48,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field48=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field49;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field49  
	   {
	    
	     get
		{
		   return field49;
		 }
		 set
		 {
		   if(field49 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field49",OldValue=field49,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field49=value;
		   }
			
		 }
	   }
	   private CustomFieldClass field50;
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public CustomFieldClass Field50  
	   {
	    
	     get
		{
		   return field50;
		 }
		 set
		 {
		   if(field50 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field50",OldValue=field50,NewValue=value,PropertyType="CustomFieldClass"};
		    NotifyPropertyChanged(values);
		   field50=value;
		   }
			
		 }
	   }
   }
   
}
	 