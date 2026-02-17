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
   }
   
}
	 