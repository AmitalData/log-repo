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
using Amital.QuoteOPM.Def.Validators;
  
namespace Amital.QuoteOPM.Def.EntityPMs
{
   [CustomValidation(typeof(QuoteOPMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class QuoteOPChargePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string quoteOPId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteOPId  
	   {
	    
	     get
		{
		   return quoteOPId;
		 }
		 set
		 {
		   if(quoteOPId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteOPId",OldValue=quoteOPId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteOPId=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private DateTime valueDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ValueDate  
	   {
	    
	     get
		{
		   return valueDate;
		 }
		 set
		 {
		   if(valueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ValueDate",OldValue=valueDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   valueDate=value;
		   }
			
		 }
	   }
	  private double? containerType1MarkUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ContainerType1MarkUpValue  
	   {
	    
	     get
		{
		   return containerType1MarkUpValue;
		 }
		 set
		 {
		   if(containerType1MarkUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType1MarkUpValue",OldValue=containerType1MarkUpValue,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   containerType1MarkUpValue=value;
		   }
			
		 }
	   }
	  private double? containerType2MarkUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ContainerType2MarkUpValue  
	   {
	    
	     get
		{
		   return containerType2MarkUpValue;
		 }
		 set
		 {
		   if(containerType2MarkUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType2MarkUpValue",OldValue=containerType2MarkUpValue,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   containerType2MarkUpValue=value;
		   }
			
		 }
	   }
	  private double? containerType3MarkUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ContainerType3MarkUpValue  
	   {
	    
	     get
		{
		   return containerType3MarkUpValue;
		 }
		 set
		 {
		   if(containerType3MarkUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType3MarkUpValue",OldValue=containerType3MarkUpValue,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   containerType3MarkUpValue=value;
		   }
			
		 }
	   }
	  private double? containerType4MarkUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ContainerType4MarkUpValue  
	   {
	    
	     get
		{
		   return containerType4MarkUpValue;
		 }
		 set
		 {
		   if(containerType4MarkUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType4MarkUpValue",OldValue=containerType4MarkUpValue,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   containerType4MarkUpValue=value;
		   }
			
		 }
	   }
	  private double? containerType5MarkUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ContainerType5MarkUpValue  
	   {
	    
	     get
		{
		   return containerType5MarkUpValue;
		 }
		 set
		 {
		   if(containerType5MarkUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType5MarkUpValue",OldValue=containerType5MarkUpValue,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   containerType5MarkUpValue=value;
		   }
			
		 }
	   }
	  private string containerType1MarkUpTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType1MarkUpTypeCode  
	   {
	    
	     get
		{
		   return containerType1MarkUpTypeCode;
		 }
		 set
		 {
		   if(containerType1MarkUpTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType1MarkUpTypeCode",OldValue=containerType1MarkUpTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType1MarkUpTypeCode=value;
		   }
			
		 }
	   }
	  private string containerType2MarkUpTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType2MarkUpTypeCode  
	   {
	    
	     get
		{
		   return containerType2MarkUpTypeCode;
		 }
		 set
		 {
		   if(containerType2MarkUpTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType2MarkUpTypeCode",OldValue=containerType2MarkUpTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType2MarkUpTypeCode=value;
		   }
			
		 }
	   }
	  private string containerType3MarkUpTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType3MarkUpTypeCode  
	   {
	    
	     get
		{
		   return containerType3MarkUpTypeCode;
		 }
		 set
		 {
		   if(containerType3MarkUpTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType3MarkUpTypeCode",OldValue=containerType3MarkUpTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType3MarkUpTypeCode=value;
		   }
			
		 }
	   }
	  private string containerType4MarkUpTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType4MarkUpTypeCode  
	   {
	    
	     get
		{
		   return containerType4MarkUpTypeCode;
		 }
		 set
		 {
		   if(containerType4MarkUpTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType4MarkUpTypeCode",OldValue=containerType4MarkUpTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType4MarkUpTypeCode=value;
		   }
			
		 }
	   }
	  private string containerType5MarkUpTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType5MarkUpTypeCode  
	   {
	    
	     get
		{
		   return containerType5MarkUpTypeCode;
		 }
		 set
		 {
		   if(containerType5MarkUpTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType5MarkUpTypeCode",OldValue=containerType5MarkUpTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType5MarkUpTypeCode=value;
		   }
			
		 }
	   }
	  private double? costExchangeRate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostExchangeRate  
	   {
	    
	     get
		{
		   return costExchangeRate;
		 }
		 set
		 {
		   if(costExchangeRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostExchangeRate",OldValue=costExchangeRate,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costExchangeRate=value;
		   }
			
		 }
	   }
	  private string costCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CostCurrencyId  
	   {
	    
	     get
		{
		   return costCurrencyId;
		 }
		 set
		 {
		   if(costCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostCurrencyId",OldValue=costCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   costCurrencyId=value;
		   }
			
		 }
	   }
	  private bool costIsFixedRate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CostIsFixedRate  
	   {
	    
	     get
		{
		   return costIsFixedRate;
		 }
		 set
		 {
		   if(costIsFixedRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostIsFixedRate",OldValue=costIsFixedRate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   costIsFixedRate=value;
		   }
			
		 }
	   }
	  private string chargesTypeId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargesTypeId  
	   {
	    
	     get
		{
		   return chargesTypeId;
		 }
		 set
		 {
		   if(chargesTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargesTypeId",OldValue=chargesTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargesTypeId=value;
		   }
			
		 }
	   }
	  private string vendorId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorId  
	   {
	    
	     get
		{
		   return vendorId;
		 }
		 set
		 {
		   if(vendorId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorId",OldValue=vendorId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorId=value;
		   }
			
		 }
	   }
	  private string saleCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SaleCurrencyId  
	   {
	    
	     get
		{
		   return saleCurrencyId;
		 }
		 set
		 {
		   if(saleCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleCurrencyId",OldValue=saleCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   saleCurrencyId=value;
		   }
			
		 }
	   }
	  private double? saleExchangeRate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleExchangeRate  
	   {
	    
	     get
		{
		   return saleExchangeRate;
		 }
		 set
		 {
		   if(saleExchangeRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleExchangeRate",OldValue=saleExchangeRate,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleExchangeRate=value;
		   }
			
		 }
	   }
	  private string markUpTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string MarkUpTypeCode  
	   {
	    
	     get
		{
		   return markUpTypeCode;
		 }
		 set
		 {
		   if(markUpTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarkUpTypeCode",OldValue=markUpTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   markUpTypeCode=value;
		   }
			
		 }
	   }
	  private double? markUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? MarkUpValue  
	   {
	    
	     get
		{
		   return markUpValue;
		 }
		 set
		 {
		   if(markUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarkUpValue",OldValue=markUpValue,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   markUpValue=value;
		   }
			
		 }
	   }
	  private bool isAllIN ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAllIN  
	   {
	    
	     get
		{
		   return isAllIN;
		 }
		 set
		 {
		   if(isAllIN != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAllIN",OldValue=isAllIN,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAllIN=value;
		   }
			
		 }
	   }
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string costMeasurementId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CostMeasurementId  
	   {
	    
	     get
		{
		   return costMeasurementId;
		 }
		 set
		 {
		   if(costMeasurementId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostMeasurementId",OldValue=costMeasurementId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   costMeasurementId=value;
		   }
			
		 }
	   }
	  private double? costQuantity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostQuantity  
	   {
	    
	     get
		{
		   return costQuantity;
		 }
		 set
		 {
		   if(costQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostQuantity",OldValue=costQuantity,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costQuantity=value;
		   }
			
		 }
	   }
	  private double? costUnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostUnitPrice  
	   {
	    
	     get
		{
		   return costUnitPrice;
		 }
		 set
		 {
		   if(costUnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostUnitPrice",OldValue=costUnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costUnitPrice=value;
		   }
			
		 }
	   }
	  private double? costTotalAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostTotalAmount  
	   {
	    
	     get
		{
		   return costTotalAmount;
		 }
		 set
		 {
		   if(costTotalAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostTotalAmount",OldValue=costTotalAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costTotalAmount=value;
		   }
			
		 }
	   }
	  private double? costTotalAmountLocal ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostTotalAmountLocal  
	   {
	    
	     get
		{
		   return costTotalAmountLocal;
		 }
		 set
		 {
		   if(costTotalAmountLocal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostTotalAmountLocal",OldValue=costTotalAmountLocal,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costTotalAmountLocal=value;
		   }
			
		 }
	   }
	  private double? costContainerType1UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostContainerType1UnitPrice  
	   {
	    
	     get
		{
		   return costContainerType1UnitPrice;
		 }
		 set
		 {
		   if(costContainerType1UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostContainerType1UnitPrice",OldValue=costContainerType1UnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costContainerType1UnitPrice=value;
		   }
			
		 }
	   }
	  private double? costContainerType2UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostContainerType2UnitPrice  
	   {
	    
	     get
		{
		   return costContainerType2UnitPrice;
		 }
		 set
		 {
		   if(costContainerType2UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostContainerType2UnitPrice",OldValue=costContainerType2UnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costContainerType2UnitPrice=value;
		   }
			
		 }
	   }
	  private double? costContainerType3UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostContainerType3UnitPrice  
	   {
	    
	     get
		{
		   return costContainerType3UnitPrice;
		 }
		 set
		 {
		   if(costContainerType3UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostContainerType3UnitPrice",OldValue=costContainerType3UnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costContainerType3UnitPrice=value;
		   }
			
		 }
	   }
	  private double? costContainerType4UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostContainerType4UnitPrice  
	   {
	    
	     get
		{
		   return costContainerType4UnitPrice;
		 }
		 set
		 {
		   if(costContainerType4UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostContainerType4UnitPrice",OldValue=costContainerType4UnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costContainerType4UnitPrice=value;
		   }
			
		 }
	   }
	  private double? costContainerType5UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostContainerType5UnitPrice  
	   {
	    
	     get
		{
		   return costContainerType5UnitPrice;
		 }
		 set
		 {
		   if(costContainerType5UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostContainerType5UnitPrice",OldValue=costContainerType5UnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costContainerType5UnitPrice=value;
		   }
			
		 }
	   }
	  private string saleMeasurementId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SaleMeasurementId  
	   {
	    
	     get
		{
		   return saleMeasurementId;
		 }
		 set
		 {
		   if(saleMeasurementId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleMeasurementId",OldValue=saleMeasurementId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   saleMeasurementId=value;
		   }
			
		 }
	   }
	  private double? saleQuantity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleQuantity  
	   {
	    
	     get
		{
		   return saleQuantity;
		 }
		 set
		 {
		   if(saleQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleQuantity",OldValue=saleQuantity,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleQuantity=value;
		   }
			
		 }
	   }
	  private double? saleUnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleUnitPrice  
	   {
	    
	     get
		{
		   return saleUnitPrice;
		 }
		 set
		 {
		   if(saleUnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice",OldValue=saleUnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice=value;
		   }
			
		 }
	   }
	  private double? saleTotalAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleTotalAmount  
	   {
	    
	     get
		{
		   return saleTotalAmount;
		 }
		 set
		 {
		   if(saleTotalAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleTotalAmount",OldValue=saleTotalAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleTotalAmount=value;
		   }
			
		 }
	   }
	  private double? saleTotalAmountLocal ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleTotalAmountLocal  
	   {
	    
	     get
		{
		   return saleTotalAmountLocal;
		 }
		 set
		 {
		   if(saleTotalAmountLocal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleTotalAmountLocal",OldValue=saleTotalAmountLocal,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleTotalAmountLocal=value;
		   }
			
		 }
	   }
	  private double? saleContainerType1UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleContainerType1UnitPrice  
	   {
	    
	     get
		{
		   return saleContainerType1UnitPrice;
		 }
		 set
		 {
		   if(saleContainerType1UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleContainerType1UnitPrice",OldValue=saleContainerType1UnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleContainerType1UnitPrice=value;
		   }
			
		 }
	   }
	  private double? saleContainerType2UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleContainerType2UnitPrice  
	   {
	    
	     get
		{
		   return saleContainerType2UnitPrice;
		 }
		 set
		 {
		   if(saleContainerType2UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleContainerType2UnitPrice",OldValue=saleContainerType2UnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleContainerType2UnitPrice=value;
		   }
			
		 }
	   }
	  private double? saleContainerType3UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleContainerType3UnitPrice  
	   {
	    
	     get
		{
		   return saleContainerType3UnitPrice;
		 }
		 set
		 {
		   if(saleContainerType3UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleContainerType3UnitPrice",OldValue=saleContainerType3UnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleContainerType3UnitPrice=value;
		   }
			
		 }
	   }
	  private double? saleContainerType4UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleContainerType4UnitPrice  
	   {
	    
	     get
		{
		   return saleContainerType4UnitPrice;
		 }
		 set
		 {
		   if(saleContainerType4UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleContainerType4UnitPrice",OldValue=saleContainerType4UnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleContainerType4UnitPrice=value;
		   }
			
		 }
	   }
	  private double? saleContainerType5UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleContainerType5UnitPrice  
	   {
	    
	     get
		{
		   return saleContainerType5UnitPrice;
		 }
		 set
		 {
		   if(saleContainerType5UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleContainerType5UnitPrice",OldValue=saleContainerType5UnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleContainerType5UnitPrice=value;
		   }
			
		 }
	   }
	  private double? costMaxAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostMaxAmount  
	   {
	    
	     get
		{
		   return costMaxAmount;
		 }
		 set
		 {
		   if(costMaxAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostMaxAmount",OldValue=costMaxAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costMaxAmount=value;
		   }
			
		 }
	   }
	  private double? costMinAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostMinAmount  
	   {
	    
	     get
		{
		   return costMinAmount;
		 }
		 set
		 {
		   if(costMinAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostMinAmount",OldValue=costMinAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costMinAmount=value;
		   }
			
		 }
	   }
	  private double? saleMaxAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleMaxAmount  
	   {
	    
	     get
		{
		   return saleMaxAmount;
		 }
		 set
		 {
		   if(saleMaxAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleMaxAmount",OldValue=saleMaxAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleMaxAmount=value;
		   }
			
		 }
	   }
	  private double? saleMinAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleMinAmount  
	   {
	    
	     get
		{
		   return saleMinAmount;
		 }
		 set
		 {
		   if(saleMinAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleMinAmount",OldValue=saleMinAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleMinAmount=value;
		   }
			
		 }
	   }
	  private bool isChargeBySteps ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsChargeBySteps  
	   {
	    
	     get
		{
		   return isChargeBySteps;
		 }
		 set
		 {
		   if(isChargeBySteps != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsChargeBySteps",OldValue=isChargeBySteps,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isChargeBySteps=value;
		   }
			
		 }
	   }
	  private bool saleIsFixedRate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool SaleIsFixedRate  
	   {
	    
	     get
		{
		   return saleIsFixedRate;
		 }
		 set
		 {
		   if(saleIsFixedRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleIsFixedRate",OldValue=saleIsFixedRate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   saleIsFixedRate=value;
		   }
			
		 }
	   }
	  private int viewOrder ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int ViewOrder  
	   {
	    
	     get
		{
		   return viewOrder;
		 }
		 set
		 {
		   if(viewOrder != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ViewOrder",OldValue=viewOrder,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   viewOrder=value;
		   }
			
		 }
	   }
	  private string quoteTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteTypeCode  
	   {
	    
	     get
		{
		   return quoteTypeCode;
		 }
		 set
		 {
		   if(quoteTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteTypeCode",OldValue=quoteTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteTypeCode=value;
		   }
			
		 }
	   }
	  private string chargesTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargesTypeCode  
	   {
	    
	     get
		{
		   return chargesTypeCode;
		 }
		 set
		 {
		   if(chargesTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargesTypeCode",OldValue=chargesTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargesTypeCode=value;
		   }
			
		 }
	   }
	  private string chargesTypeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargesTypeName  
	   {
	    
	     get
		{
		   return chargesTypeName;
		 }
		 set
		 {
		   if(chargesTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargesTypeName",OldValue=chargesTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargesTypeName=value;
		   }
			
		 }
	   }
	  private string chargesTypeLocalName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargesTypeLocalName  
	   {
	    
	     get
		{
		   return chargesTypeLocalName;
		 }
		 set
		 {
		   if(chargesTypeLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargesTypeLocalName",OldValue=chargesTypeLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargesTypeLocalName=value;
		   }
			
		 }
	   }
	  private string chargesGroupCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargesGroupCode  
	   {
	    
	     get
		{
		   return chargesGroupCode;
		 }
		 set
		 {
		   if(chargesGroupCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargesGroupCode",OldValue=chargesGroupCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargesGroupCode=value;
		   }
			
		 }
	   }
	  private string vendorName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorName  
	   {
	    
	     get
		{
		   return vendorName;
		 }
		 set
		 {
		   if(vendorName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorName",OldValue=vendorName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorName=value;
		   }
			
		 }
	   }
	  private string costMeasurementCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CostMeasurementCode  
	   {
	    
	     get
		{
		   return costMeasurementCode;
		 }
		 set
		 {
		   if(costMeasurementCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostMeasurementCode",OldValue=costMeasurementCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   costMeasurementCode=value;
		   }
			
		 }
	   }
	  private string costMeasurementShortName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CostMeasurementShortName  
	   {
	    
	     get
		{
		   return costMeasurementShortName;
		 }
		 set
		 {
		   if(costMeasurementShortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostMeasurementShortName",OldValue=costMeasurementShortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   costMeasurementShortName=value;
		   }
			
		 }
	   }
	  private string saleMeasurementCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SaleMeasurementCode  
	   {
	    
	     get
		{
		   return saleMeasurementCode;
		 }
		 set
		 {
		   if(saleMeasurementCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleMeasurementCode",OldValue=saleMeasurementCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   saleMeasurementCode=value;
		   }
			
		 }
	   }
	  private string saleMeasurementShortName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SaleMeasurementShortName  
	   {
	    
	     get
		{
		   return saleMeasurementShortName;
		 }
		 set
		 {
		   if(saleMeasurementShortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleMeasurementShortName",OldValue=saleMeasurementShortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   saleMeasurementShortName=value;
		   }
			
		 }
	   }
	  private string costCurrencyCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CostCurrencyCode  
	   {
	    
	     get
		{
		   return costCurrencyCode;
		 }
		 set
		 {
		   if(costCurrencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostCurrencyCode",OldValue=costCurrencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   costCurrencyCode=value;
		   }
			
		 }
	   }
	  private string saleCurrencyCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SaleCurrencyCode  
	   {
	    
	     get
		{
		   return saleCurrencyCode;
		 }
		 set
		 {
		   if(saleCurrencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleCurrencyCode",OldValue=saleCurrencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   saleCurrencyCode=value;
		   }
			
		 }
	   }
	  private double costUnitPriceInSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostUnitPriceInSaleCurrency  
	   {
	    
	     get
		{
		   return costUnitPriceInSaleCurrency;
		 }
		 set
		 {
		   if(costUnitPriceInSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostUnitPriceInSaleCurrency",OldValue=costUnitPriceInSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costUnitPriceInSaleCurrency=value;
		   }
			
		 }
	   }
	  private double costUnitPrice1InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostUnitPrice1InSaleCurrency  
	   {
	    
	     get
		{
		   return costUnitPrice1InSaleCurrency;
		 }
		 set
		 {
		   if(costUnitPrice1InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostUnitPrice1InSaleCurrency",OldValue=costUnitPrice1InSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costUnitPrice1InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double costUnitPrice2InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostUnitPrice2InSaleCurrency  
	   {
	    
	     get
		{
		   return costUnitPrice2InSaleCurrency;
		 }
		 set
		 {
		   if(costUnitPrice2InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostUnitPrice2InSaleCurrency",OldValue=costUnitPrice2InSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costUnitPrice2InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double costUnitPrice3InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostUnitPrice3InSaleCurrency  
	   {
	    
	     get
		{
		   return costUnitPrice3InSaleCurrency;
		 }
		 set
		 {
		   if(costUnitPrice3InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostUnitPrice3InSaleCurrency",OldValue=costUnitPrice3InSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costUnitPrice3InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double costUnitPrice4InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostUnitPrice4InSaleCurrency  
	   {
	    
	     get
		{
		   return costUnitPrice4InSaleCurrency;
		 }
		 set
		 {
		   if(costUnitPrice4InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostUnitPrice4InSaleCurrency",OldValue=costUnitPrice4InSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costUnitPrice4InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double costUnitPrice5InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostUnitPrice5InSaleCurrency  
	   {
	    
	     get
		{
		   return costUnitPrice5InSaleCurrency;
		 }
		 set
		 {
		   if(costUnitPrice5InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostUnitPrice5InSaleCurrency",OldValue=costUnitPrice5InSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costUnitPrice5InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double? costAmountInSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostAmountInSaleCurrency  
	   {
	    
	     get
		{
		   return costAmountInSaleCurrency;
		 }
		 set
		 {
		   if(costAmountInSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostAmountInSaleCurrency",OldValue=costAmountInSaleCurrency,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costAmountInSaleCurrency=value;
		   }
			
		 }
	   }

	   private List<QuoteOPPriceStepsPM> quoteOPChargePriceSteps;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("QuoteOPChargeQuotePriceSteps", "Id","QuoteOPChargeId")]
	   [DataMember]
	   public virtual List<QuoteOPPriceStepsPM> QuoteOPChargePriceSteps  
	   {
	        get
             {
                 if (quoteOPChargePriceSteps == null)
                 {
                     quoteOPChargePriceSteps = new List<QuoteOPPriceStepsPM>();
                 }
                 return quoteOPChargePriceSteps;
              }
             set { quoteOPChargePriceSteps = value; }
	    }
		   
	   private List<QuoteOPPriceStepsPM>  deletedQuoteOPChargePriceSteps;
	   public virtual List<QuoteOPPriceStepsPM> DeletedQuoteOPChargePriceSteps  
	   {
	        get
             {
                 if ( deletedQuoteOPChargePriceSteps == null)
                 {
                      deletedQuoteOPChargePriceSteps = new List<QuoteOPPriceStepsPM>();
                 }
                 return  deletedQuoteOPChargePriceSteps;
              }
             set {  deletedQuoteOPChargePriceSteps = value; }
	    }
	  	  private string vatTypeId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VatTypeId  
	   {
	    
	     get
		{
		   return vatTypeId;
		 }
		 set
		 {
		   if(vatTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatTypeId",OldValue=vatTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vatTypeId=value;
		   }
			
		 }
	   }
	  private double? vatPercentage ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? VatPercentage  
	   {
	    
	     get
		{
		   return vatPercentage;
		 }
		 set
		 {
		   if(vatPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatPercentage",OldValue=vatPercentage,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   vatPercentage=value;
		   }
			
		 }
	   }
	  private string vatTypeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VatTypeName  
	   {
	    
	     get
		{
		   return vatTypeName;
		 }
		 set
		 {
		   if(vatTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatTypeName",OldValue=vatTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vatTypeName=value;
		   }
			
		 }
	   }
	  private double vatAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double VatAmount  
	   {
	    
	     get
		{
		   return vatAmount;
		 }
		 set
		 {
		   if(vatAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatAmount",OldValue=vatAmount,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   vatAmount=value;
		   }
			
		 }
	   }
	  private double? saleUnitPriceInSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleUnitPriceInSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPriceInSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPriceInSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPriceInSaleCurrency",OldValue=saleUnitPriceInSaleCurrency,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleUnitPriceInSaleCurrency=value;
		   }
			
		 }
	   }
	  private double? saleUnitPrice1InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleUnitPrice1InSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPrice1InSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPrice1InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice1InSaleCurrency",OldValue=saleUnitPrice1InSaleCurrency,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice1InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double? saleUnitPrice2InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleUnitPrice2InSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPrice2InSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPrice2InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice2InSaleCurrency",OldValue=saleUnitPrice2InSaleCurrency,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice2InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double? saleUnitPrice3InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleUnitPrice3InSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPrice3InSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPrice3InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice3InSaleCurrency",OldValue=saleUnitPrice3InSaleCurrency,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice3InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double? saleUnitPrice4InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleUnitPrice4InSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPrice4InSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPrice4InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice4InSaleCurrency",OldValue=saleUnitPrice4InSaleCurrency,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice4InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double? saleUnitPrice5InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleUnitPrice5InSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPrice5InSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPrice5InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice5InSaleCurrency",OldValue=saleUnitPrice5InSaleCurrency,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice5InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double? saleAmountInSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleAmountInSaleCurrency  
	   {
	    
	     get
		{
		   return saleAmountInSaleCurrency;
		 }
		 set
		 {
		   if(saleAmountInSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleAmountInSaleCurrency",OldValue=saleAmountInSaleCurrency,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleAmountInSaleCurrency=value;
		   }
			
		 }
	   }
	  private string chargesTypeDescription ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargesTypeDescription  
	   {
	    
	     get
		{
		   return chargesTypeDescription;
		 }
		 set
		 {
		   if(chargesTypeDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargesTypeDescription",OldValue=chargesTypeDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargesTypeDescription=value;
		   }
			
		 }
	   }
	  private string externalVATCard ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalVATCard  
	   {
	    
	     get
		{
		   return externalVATCard;
		 }
		 set
		 {
		   if(externalVATCard != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalVATCard",OldValue=externalVATCard,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalVATCard=value;
		   }
			
		 }
	   }
	  private string externalTAXItemId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalTAXItemId  
	   {
	    
	     get
		{
		   return externalTAXItemId;
		 }
		 set
		 {
		   if(externalTAXItemId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalTAXItemId",OldValue=externalTAXItemId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalTAXItemId=value;
		   }
			
		 }
	   }
	  private bool vatIsMultiPercentage ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool VatIsMultiPercentage  
	   {
	    
	     get
		{
		   return vatIsMultiPercentage;
		 }
		 set
		 {
		   if(vatIsMultiPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatIsMultiPercentage",OldValue=vatIsMultiPercentage,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   vatIsMultiPercentage=value;
		   }
			
		 }
	   }
	  private bool isBackToBack ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsBackToBack  
	   {
	    
	     get
		{
		   return isBackToBack;
		 }
		 set
		 {
		   if(isBackToBack != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsBackToBack",OldValue=isBackToBack,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isBackToBack=value;
		   }
			
		 }
	   }
	  private string markUpText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string MarkUpText  
	   {
	    
	     get
		{
		   return markUpText;
		 }
		 set
		 {
		   if(markUpText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarkUpText",OldValue=markUpText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   markUpText=value;
		   }
			
		 }
	   }
	  private string containerType1MarkUpText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType1MarkUpText  
	   {
	    
	     get
		{
		   return containerType1MarkUpText;
		 }
		 set
		 {
		   if(containerType1MarkUpText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType1MarkUpText",OldValue=containerType1MarkUpText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType1MarkUpText=value;
		   }
			
		 }
	   }
	  private string containerType2MarkUpText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType2MarkUpText  
	   {
	    
	     get
		{
		   return containerType2MarkUpText;
		 }
		 set
		 {
		   if(containerType2MarkUpText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType2MarkUpText",OldValue=containerType2MarkUpText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType2MarkUpText=value;
		   }
			
		 }
	   }
	  private string containerType3MarkUpText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType3MarkUpText  
	   {
	    
	     get
		{
		   return containerType3MarkUpText;
		 }
		 set
		 {
		   if(containerType3MarkUpText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType3MarkUpText",OldValue=containerType3MarkUpText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType3MarkUpText=value;
		   }
			
		 }
	   }
	  private string containerType4MarkUpText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType4MarkUpText  
	   {
	    
	     get
		{
		   return containerType4MarkUpText;
		 }
		 set
		 {
		   if(containerType4MarkUpText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType4MarkUpText",OldValue=containerType4MarkUpText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType4MarkUpText=value;
		   }
			
		 }
	   }
	  private string containerType5MarkUpText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType5MarkUpText  
	   {
	    
	     get
		{
		   return containerType5MarkUpText;
		 }
		 set
		 {
		   if(containerType5MarkUpText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType5MarkUpText",OldValue=containerType5MarkUpText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType5MarkUpText=value;
		   }
			
		 }
	   }
	  private bool isCostAllIn ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCostAllIn  
	   {
	    
	     get
		{
		   return isCostAllIn;
		 }
		 set
		 {
		   if(isCostAllIn != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCostAllIn",OldValue=isCostAllIn,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCostAllIn=value;
		   }
			
		 }
	   }
	  private string tariffId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffId  
	   {
	    
	     get
		{
		   return tariffId;
		 }
		 set
		 {
		   if(tariffId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffId",OldValue=tariffId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffId=value;
		   }
			
		 }
	   }
	  private string tariffNumber ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffNumber  
	   {
	    
	     get
		{
		   return tariffNumber;
		 }
		 set
		 {
		   if(tariffNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffNumber",OldValue=tariffNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffNumber=value;
		   }
			
		 }
	   }
	  private int tariffVersion ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int TariffVersion  
	   {
	    
	     get
		{
		   return tariffVersion;
		 }
		 set
		 {
		   if(tariffVersion != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffVersion",OldValue=tariffVersion,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   tariffVersion=value;
		   }
			
		 }
	   }
	  private string vendorCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorCode  
	   {
	    
	     get
		{
		   return vendorCode;
		 }
		 set
		 {
		   if(vendorCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorCode",OldValue=vendorCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorCode=value;
		   }
			
		 }
	   }
	  private bool hasPickup ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasPickup  
	   {
	    
	     get
		{
		   return hasPickup;
		 }
		 set
		 {
		   if(hasPickup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasPickup",OldValue=hasPickup,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasPickup=value;
		   }
			
		 }
	   }
	  private bool hasDelivery ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasDelivery  
	   {
	    
	     get
		{
		   return hasDelivery;
		 }
		 set
		 {
		   if(hasDelivery != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasDelivery",OldValue=hasDelivery,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasDelivery=value;
		   }
			
		 }
	   }
	  private bool isRegionalTax ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsRegionalTax  
	   {
	    
	     get
		{
		   return isRegionalTax;
		 }
		 set
		 {
		   if(isRegionalTax != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsRegionalTax",OldValue=isRegionalTax,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isRegionalTax=value;
		   }
			
		 }
	   }
	  private double? costRatio ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostRatio  
	   {
	    
	     get
		{
		   return costRatio;
		 }
		 set
		 {
		   if(costRatio != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostRatio",OldValue=costRatio,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costRatio=value;
		   }
			
		 }
	   }
	  private double? saleRatio ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleRatio  
	   {
	    
	     get
		{
		   return saleRatio;
		 }
		 set
		 {
		   if(saleRatio != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleRatio",OldValue=saleRatio,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleRatio=value;
		   }
			
		 }
	   }
	  private string tariffLineId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffLineId  
	   {
	    
	     get
		{
		   return tariffLineId;
		 }
		 set
		 {
		   if(tariffLineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffLineId",OldValue=tariffLineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffLineId=value;
		   }
			
		 }
	   }
   }
   
}
	 