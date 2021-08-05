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
   public partial class QuoteOPSaleChargePM : EntityPM
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
	  private string fixedAmountCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FixedAmountCode  
	   {
	    
	     get
		{
		   return fixedAmountCode;
		 }
		 set
		 {
		   if(fixedAmountCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FixedAmountCode",OldValue=fixedAmountCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fixedAmountCode=value;
		   }
			
		 }
	   }
	  private string foreignAmountFixed ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ForeignAmountFixed  
	   {
	    
	     get
		{
		   return foreignAmountFixed;
		 }
		 set
		 {
		   if(foreignAmountFixed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignAmountFixed",OldValue=foreignAmountFixed,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   foreignAmountFixed=value;
		   }
			
		 }
	   }
	  private string localAmountFixed ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LocalAmountFixed  
	   {
	    
	     get
		{
		   return localAmountFixed;
		 }
		 set
		 {
		   if(localAmountFixed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalAmountFixed",OldValue=localAmountFixed,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   localAmountFixed=value;
		   }
			
		 }
	   }
	  private string isAllIN ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsAllIN  
	   {
	    
	     get
		{
		   return isAllIN;
		 }
		 set
		 {
		   if(isAllIN != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAllIN",OldValue=isAllIN,NewValue=value,PropertyType="string"};
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
	  private double saleQuantity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleQuantity  
	   {
	    
	     get
		{
		   return saleQuantity;
		 }
		 set
		 {
		   if(saleQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleQuantity",OldValue=saleQuantity,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleQuantity=value;
		   }
			
		 }
	   }
	  private double saleUnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleUnitPrice  
	   {
	    
	     get
		{
		   return saleUnitPrice;
		 }
		 set
		 {
		   if(saleUnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice",OldValue=saleUnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice=value;
		   }
			
		 }
	   }
	  private double saleTotalAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleTotalAmount  
	   {
	    
	     get
		{
		   return saleTotalAmount;
		 }
		 set
		 {
		   if(saleTotalAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleTotalAmount",OldValue=saleTotalAmount,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleTotalAmount=value;
		   }
			
		 }
	   }
	  private double saleTotalAmountLocal ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleTotalAmountLocal  
	   {
	    
	     get
		{
		   return saleTotalAmountLocal;
		 }
		 set
		 {
		   if(saleTotalAmountLocal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleTotalAmountLocal",OldValue=saleTotalAmountLocal,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleTotalAmountLocal=value;
		   }
			
		 }
	   }
	  private double saleContainerType1UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleContainerType1UnitPrice  
	   {
	    
	     get
		{
		   return saleContainerType1UnitPrice;
		 }
		 set
		 {
		   if(saleContainerType1UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleContainerType1UnitPrice",OldValue=saleContainerType1UnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleContainerType1UnitPrice=value;
		   }
			
		 }
	   }
	  private double saleContainerType2UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleContainerType2UnitPrice  
	   {
	    
	     get
		{
		   return saleContainerType2UnitPrice;
		 }
		 set
		 {
		   if(saleContainerType2UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleContainerType2UnitPrice",OldValue=saleContainerType2UnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleContainerType2UnitPrice=value;
		   }
			
		 }
	   }
	  private double saleContainerType3UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleContainerType3UnitPrice  
	   {
	    
	     get
		{
		   return saleContainerType3UnitPrice;
		 }
		 set
		 {
		   if(saleContainerType3UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleContainerType3UnitPrice",OldValue=saleContainerType3UnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleContainerType3UnitPrice=value;
		   }
			
		 }
	   }
	  private double saleContainerType4UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleContainerType4UnitPrice  
	   {
	    
	     get
		{
		   return saleContainerType4UnitPrice;
		 }
		 set
		 {
		   if(saleContainerType4UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleContainerType4UnitPrice",OldValue=saleContainerType4UnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleContainerType4UnitPrice=value;
		   }
			
		 }
	   }
	  private double saleContainerType5UnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleContainerType5UnitPrice  
	   {
	    
	     get
		{
		   return saleContainerType5UnitPrice;
		 }
		 set
		 {
		   if(saleContainerType5UnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleContainerType5UnitPrice",OldValue=saleContainerType5UnitPrice,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleContainerType5UnitPrice=value;
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
	  private string priceBreaks ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PriceBreaks  
	   {
	    
	     get
		{
		   return priceBreaks;
		 }
		 set
		 {
		   if(priceBreaks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PriceBreaks",OldValue=priceBreaks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   priceBreaks=value;
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
	  private string saleMeasurementLocalName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SaleMeasurementLocalName  
	   {
	    
	     get
		{
		   return saleMeasurementLocalName;
		 }
		 set
		 {
		   if(saleMeasurementLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleMeasurementLocalName",OldValue=saleMeasurementLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   saleMeasurementLocalName=value;
		   }
			
		 }
	   }
	  private double saleUnitPriceInSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleUnitPriceInSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPriceInSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPriceInSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPriceInSaleCurrency",OldValue=saleUnitPriceInSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleUnitPriceInSaleCurrency=value;
		   }
			
		 }
	   }
	  private double saleUnitPrice1InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleUnitPrice1InSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPrice1InSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPrice1InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice1InSaleCurrency",OldValue=saleUnitPrice1InSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice1InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double saleUnitPrice2InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleUnitPrice2InSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPrice2InSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPrice2InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice2InSaleCurrency",OldValue=saleUnitPrice2InSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice2InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double saleUnitPrice3InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleUnitPrice3InSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPrice3InSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPrice3InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice3InSaleCurrency",OldValue=saleUnitPrice3InSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice3InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double saleUnitPrice4InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleUnitPrice4InSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPrice4InSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPrice4InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice4InSaleCurrency",OldValue=saleUnitPrice4InSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice4InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double saleUnitPrice5InSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleUnitPrice5InSaleCurrency  
	   {
	    
	     get
		{
		   return saleUnitPrice5InSaleCurrency;
		 }
		 set
		 {
		   if(saleUnitPrice5InSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice5InSaleCurrency",OldValue=saleUnitPrice5InSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice5InSaleCurrency=value;
		   }
			
		 }
	   }
	  private double saleAmountInSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleAmountInSaleCurrency  
	   {
	    
	     get
		{
		   return saleAmountInSaleCurrency;
		 }
		 set
		 {
		   if(saleAmountInSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleAmountInSaleCurrency",OldValue=saleAmountInSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleAmountInSaleCurrency=value;
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
	  private double salesWithVATAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SalesWithVATAmount  
	   {
	    
	     get
		{
		   return salesWithVATAmount;
		 }
		 set
		 {
		   if(salesWithVATAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SalesWithVATAmount",OldValue=salesWithVATAmount,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   salesWithVATAmount=value;
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
	 