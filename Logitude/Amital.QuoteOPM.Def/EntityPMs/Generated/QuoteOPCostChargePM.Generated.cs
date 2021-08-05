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
   public partial class QuoteOPCostChargePM : EntityPM
   {
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
	  private string currencyId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyId  
	   {
	    
	     get
		{
		   return currencyId;
		 }
		 set
		 {
		   if(currencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyId",OldValue=currencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyId=value;
		   }
			
		 }
	   }
	  private double saleExchangeRate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleExchangeRate  
	   {
	    
	     get
		{
		   return saleExchangeRate;
		 }
		 set
		 {
		   if(saleExchangeRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleExchangeRate",OldValue=saleExchangeRate,NewValue=value,PropertyType="double"};
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
	  private double markUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double MarkUpValue  
	   {
	    
	     get
		{
		   return markUpValue;
		 }
		 set
		 {
		   if(markUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarkUpValue",OldValue=markUpValue,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   markUpValue=value;
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
	  private double costQuantity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostQuantity  
	   {
	    
	     get
		{
		   return costQuantity;
		 }
		 set
		 {
		   if(costQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostQuantity",OldValue=costQuantity,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costQuantity=value;
		   }
			
		 }
	   }
	  private double costUnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostUnitPrice  
	   {
	    
	     get
		{
		   return costUnitPrice;
		 }
		 set
		 {
		   if(costUnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostUnitPrice",OldValue=costUnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costUnitPrice=value;
		   }
			
		 }
	   }
	  private double costTotalAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostTotalAmount  
	   {
	    
	     get
		{
		   return costTotalAmount;
		 }
		 set
		 {
		   if(costTotalAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostTotalAmount",OldValue=costTotalAmount,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costTotalAmount=value;
		   }
			
		 }
	   }
	  private double costTotalAmountLocal ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostTotalAmountLocal  
	   {
	    
	     get
		{
		   return costTotalAmountLocal;
		 }
		 set
		 {
		   if(costTotalAmountLocal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostTotalAmountLocal",OldValue=costTotalAmountLocal,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costTotalAmountLocal=value;
		   }
			
		 }
	   }
	  private double costContainerType1UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostContainerType1UnitPrice  
	   {
	    
	     get
		{
		   return costContainerType1UnitPrice;
		 }
		 set
		 {
		   if(costContainerType1UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostContainerType1UnitPrice",OldValue=costContainerType1UnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costContainerType1UnitPrice=value;
		   }
			
		 }
	   }
	  private double costContainerType2UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostContainerType2UnitPrice  
	   {
	    
	     get
		{
		   return costContainerType2UnitPrice;
		 }
		 set
		 {
		   if(costContainerType2UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostContainerType2UnitPrice",OldValue=costContainerType2UnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costContainerType2UnitPrice=value;
		   }
			
		 }
	   }
	  private double costContainerType3UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostContainerType3UnitPrice  
	   {
	    
	     get
		{
		   return costContainerType3UnitPrice;
		 }
		 set
		 {
		   if(costContainerType3UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostContainerType3UnitPrice",OldValue=costContainerType3UnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costContainerType3UnitPrice=value;
		   }
			
		 }
	   }
	  private double costContainerType4UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostContainerType4UnitPrice  
	   {
	    
	     get
		{
		   return costContainerType4UnitPrice;
		 }
		 set
		 {
		   if(costContainerType4UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostContainerType4UnitPrice",OldValue=costContainerType4UnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costContainerType4UnitPrice=value;
		   }
			
		 }
	   }
	  private double costContainerType5UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostContainerType5UnitPrice  
	   {
	    
	     get
		{
		   return costContainerType5UnitPrice;
		 }
		 set
		 {
		   if(costContainerType5UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostContainerType5UnitPrice",OldValue=costContainerType5UnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costContainerType5UnitPrice=value;
		   }
			
		 }
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
	  private double vatPercentage ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double VatPercentage  
	   {
	    
	     get
		{
		   return vatPercentage;
		 }
		 set
		 {
		   if(vatPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatPercentage",OldValue=vatPercentage,NewValue=value,PropertyType="double"};
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
	  private string uOMPercentage ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string UOMPercentage  
	   {
	    
	     get
		{
		   return uOMPercentage;
		 }
		 set
		 {
		   if(uOMPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UOMPercentage",OldValue=uOMPercentage,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   uOMPercentage=value;
		   }
			
		 }
	   }
	  private double costMinAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostMinAmount  
	   {
	    
	     get
		{
		   return costMinAmount;
		 }
		 set
		 {
		   if(costMinAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostMinAmount",OldValue=costMinAmount,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costMinAmount=value;
		   }
			
		 }
	   }
	  private double costMaxAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostMaxAmount  
	   {
	    
	     get
		{
		   return costMaxAmount;
		 }
		 set
		 {
		   if(costMaxAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostMaxAmount",OldValue=costMaxAmount,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costMaxAmount=value;
		   }
			
		 }
	   }
	  private string id ;
	  	  
       
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
	  private string quoteId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteId  
	   {
	    
	     get
		{
		   return quoteId;
		 }
		 set
		 {
		   if(quoteId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteId",OldValue=quoteId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteId=value;
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
	  private string currencyCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyCode  
	   {
	    
	     get
		{
		   return currencyCode;
		 }
		 set
		 {
		   if(currencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyCode",OldValue=currencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyCode=value;
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
	  private double containerType1MarkUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double ContainerType1MarkUpValue  
	   {
	    
	     get
		{
		   return containerType1MarkUpValue;
		 }
		 set
		 {
		   if(containerType1MarkUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType1MarkUpValue",OldValue=containerType1MarkUpValue,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   containerType1MarkUpValue=value;
		   }
			
		 }
	   }
	  private double containerType2MarkUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double ContainerType2MarkUpValue  
	   {
	    
	     get
		{
		   return containerType2MarkUpValue;
		 }
		 set
		 {
		   if(containerType2MarkUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType2MarkUpValue",OldValue=containerType2MarkUpValue,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   containerType2MarkUpValue=value;
		   }
			
		 }
	   }
	  private double containerType3MarkUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double ContainerType3MarkUpValue  
	   {
	    
	     get
		{
		   return containerType3MarkUpValue;
		 }
		 set
		 {
		   if(containerType3MarkUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType3MarkUpValue",OldValue=containerType3MarkUpValue,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   containerType3MarkUpValue=value;
		   }
			
		 }
	   }
	  private double containerType4MarkUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double ContainerType4MarkUpValue  
	   {
	    
	     get
		{
		   return containerType4MarkUpValue;
		 }
		 set
		 {
		   if(containerType4MarkUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType4MarkUpValue",OldValue=containerType4MarkUpValue,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   containerType4MarkUpValue=value;
		   }
			
		 }
	   }
	  private double containerType5MarkUpValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double ContainerType5MarkUpValue  
	   {
	    
	     get
		{
		   return containerType5MarkUpValue;
		 }
		 set
		 {
		   if(containerType5MarkUpValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType5MarkUpValue",OldValue=containerType5MarkUpValue,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   containerType5MarkUpValue=value;
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
	  private double saleMinAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleMinAmount  
	   {
	    
	     get
		{
		   return saleMinAmount;
		 }
		 set
		 {
		   if(saleMinAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleMinAmount",OldValue=saleMinAmount,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleMinAmount=value;
		   }
			
		 }
	   }
	  private double saleMaxAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleMaxAmount  
	   {
	    
	     get
		{
		   return saleMaxAmount;
		 }
		 set
		 {
		   if(saleMaxAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleMaxAmount",OldValue=saleMaxAmount,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleMaxAmount=value;
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
   }
   
}
	 