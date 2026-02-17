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
   public partial class WarehouseEntryPackagePM : EntityPM
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
	  private string warehouseEntryId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string WarehouseEntryId  
	   {
	    
	     get
		{
		   return warehouseEntryId;
		 }
		 set
		 {
		   if(warehouseEntryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WarehouseEntryId",OldValue=warehouseEntryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   warehouseEntryId=value;
		   }
			
		 }
	   }
	  private string containerNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
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
	  private int quantity ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public int Quantity  
	   {
	    
	     get
		{
		   return quantity;
		 }
		 set
		 {
		   if(quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity",OldValue=quantity,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   quantity=value;
		   }
			
		 }
	   }
	  private decimal? weight ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Weight  
	   {
	    
	     get
		{
		   return weight;
		 }
		 set
		 {
		   if(weight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Weight",OldValue=weight,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   weight=value;
		   }
			
		 }
	   }
	  private decimal? volume ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Volume  
	   {
	    
	     get
		{
		   return volume;
		 }
		 set
		 {
		   if(volume != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Volume",OldValue=volume,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   volume=value;
		   }
			
		 }
	   }
	  private string description ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Description  
	   {
	    
	     get
		{
		   return description;
		 }
		 set
		 {
		   if(description != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Description",OldValue=description,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   description=value;
		   }
			
		 }
	   }
	  private string packageTypeId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeId  
	   {
	    
	     get
		{
		   return packageTypeId;
		 }
		 set
		 {
		   if(packageTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeId",OldValue=packageTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeId=value;
		   }
			
		 }
	   }
	  private string seal ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Seal  
	   {
	    
	     get
		{
		   return seal;
		 }
		 set
		 {
		   if(seal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Seal",OldValue=seal,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   seal=value;
		   }
			
		 }
	   }
	  private string harmonize ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Harmonize  
	   {
	    
	     get
		{
		   return harmonize;
		 }
		 set
		 {
		   if(harmonize != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Harmonize",OldValue=harmonize,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   harmonize=value;
		   }
			
		 }
	   }
	  private double? width ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Width  
	   {
	    
	     get
		{
		   return width;
		 }
		 set
		 {
		   if(width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Width",OldValue=width,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   width=value;
		   }
			
		 }
	   }
	  private double? length ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Length  
	   {
	    
	     get
		{
		   return length;
		 }
		 set
		 {
		   if(length != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Length",OldValue=length,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   length=value;
		   }
			
		 }
	   }
	  private double? height ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Height  
	   {
	    
	     get
		{
		   return height;
		 }
		 set
		 {
		   if(height != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Height",OldValue=height,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   height=value;
		   }
			
		 }
	   }
	  private string packageTypeName ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeName  
	   {
	    
	     get
		{
		   return packageTypeName;
		 }
		 set
		 {
		   if(packageTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeName",OldValue=packageTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeName=value;
		   }
			
		 }
	   }
	  private string dimensions ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Dimensions  
	   {
	    
	     get
		{
		   return dimensions;
		 }
		 set
		 {
		   if(dimensions != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Dimensions",OldValue=dimensions,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dimensions=value;
		   }
			
		 }
	   }
	  private bool isContainer ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsContainer  
	   {
	    
	     get
		{
		   return isContainer;
		 }
		 set
		 {
		   if(isContainer != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsContainer",OldValue=isContainer,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isContainer=value;
		   }
			
		 }
	   }
	  private int instock ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public int Instock  
	   {
	    
	     get
		{
		   return instock;
		 }
		 set
		 {
		   if(instock != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Instock",OldValue=instock,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   instock=value;
		   }
			
		 }
	   }
	  private bool isSelected ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSelected  
	   {
	    
	     get
		{
		   return isSelected;
		 }
		 set
		 {
		   if(isSelected != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSelected",OldValue=isSelected,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSelected=value;
		   }
			
		 }
	   }
	  private int releaseQTY ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public int ReleaseQTY  
	   {
	    
	     get
		{
		   return releaseQTY;
		 }
		 set
		 {
		   if(releaseQTY != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReleaseQTY",OldValue=releaseQTY,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   releaseQTY=value;
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
	  private int instockTemp ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public int InstockTemp  
	   {
	    
	     get
		{
		   return instockTemp;
		 }
		 set
		 {
		   if(instockTemp != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InstockTemp",OldValue=instockTemp,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   instockTemp=value;
		   }
			
		 }
	   }
	  private string containerNumberWarning ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerNumberWarning  
	   {
	    
	     get
		{
		   return containerNumberWarning;
		 }
		 set
		 {
		   if(containerNumberWarning != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerNumberWarning",OldValue=containerNumberWarning,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerNumberWarning=value;
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
	  private string location ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Location  
	   {
	    
	     get
		{
		   return location;
		 }
		 set
		 {
		   if(location != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Location",OldValue=location,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   location=value;
		   }
			
		 }
	   }
	  private string dimensionUnitCode ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string DimensionUnitCode  
	   {
	    
	     get
		{
		   return dimensionUnitCode;
		 }
		 set
		 {
		   if(dimensionUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DimensionUnitCode",OldValue=dimensionUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dimensionUnitCode=value;
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
	  private bool isConnectedToShipment ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsConnectedToShipment  
	   {
	    
	     get
		{
		   return isConnectedToShipment;
		 }
		 set
		 {
		   if(isConnectedToShipment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsConnectedToShipment",OldValue=isConnectedToShipment,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isConnectedToShipment=value;
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
	  private double? volumetricWeight ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public double? VolumetricWeight  
	   {
	    
	     get
		{
		   return volumetricWeight;
		 }
		 set
		 {
		   if(volumetricWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumetricWeight",OldValue=volumetricWeight,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   volumetricWeight=value;
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
	  private string make ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Make  
	   {
	    
	     get
		{
		   return make;
		 }
		 set
		 {
		   if(make != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Make",OldValue=make,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   make=value;
		   }
			
		 }
	   }
	  private string model ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Model  
	   {
	    
	     get
		{
		   return model;
		 }
		 set
		 {
		   if(model != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Model",OldValue=model,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   model=value;
		   }
			
		 }
	   }
	  private string year ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Year  
	   {
	    
	     get
		{
		   return year;
		 }
		 set
		 {
		   if(year != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Year",OldValue=year,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   year=value;
		   }
			
		 }
	   }
	  private string color ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string Color  
	   {
	    
	     get
		{
		   return color;
		 }
		 set
		 {
		   if(color != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Color",OldValue=color,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   color=value;
		   }
			
		 }
	   }
	  private string chassisNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChassisNumber  
	   {
	    
	     get
		{
		   return chassisNumber;
		 }
		 set
		 {
		   if(chassisNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChassisNumber",OldValue=chassisNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chassisNumber=value;
		   }
			
		 }
	   }
	  private string registrationNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string RegistrationNumber  
	   {
	    
	     get
		{
		   return registrationNumber;
		 }
		 set
		 {
		   if(registrationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RegistrationNumber",OldValue=registrationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   registrationNumber=value;
		   }
			
		 }
	   }
	  private string countryId ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string CountryId  
	   {
	    
	     get
		{
		   return countryId;
		 }
		 set
		 {
		   if(countryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryId",OldValue=countryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   countryId=value;
		   }
			
		 }
	   }
	  private string commodityNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string CommodityNumber  
	   {
	    
	     get
		{
		   return commodityNumber;
		 }
		 set
		 {
		   if(commodityNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CommodityNumber",OldValue=commodityNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   commodityNumber=value;
		   }
			
		 }
	   }
	  private string releasesNumber ;
	  	  
       
	   [CustomValidation(typeof(WarehouseValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReleasesNumber  
	   {
	    
	     get
		{
		   return releasesNumber;
		 }
		 set
		 {
		   if(releasesNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReleasesNumber",OldValue=releasesNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   releasesNumber=value;
		   }
			
		 }
	   }
   }
   
}
	 