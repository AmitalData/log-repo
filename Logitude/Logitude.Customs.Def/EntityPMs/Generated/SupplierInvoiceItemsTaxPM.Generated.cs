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
   public partial class SupplierInvoiceItemsTaxPM : EntityPM
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
	  private int invoiceCounterKey ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int InvoiceCounterKey  
	   {
	    
	     get
		{
		   return invoiceCounterKey;
		 }
		 set
		 {
		   if(invoiceCounterKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceCounterKey",OldValue=invoiceCounterKey,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   invoiceCounterKey=value;
		   }
			
		 }
	   }
	  private int lineNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int LineNumber  
	   {
	    
	     get
		{
		   return lineNumber;
		 }
		 set
		 {
		   if(lineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineNumber",OldValue=lineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   lineNumber=value;
		   }
			
		 }
	   }
	  private string taxTypeCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaxTypeCode  
	   {
	    
	     get
		{
		   return taxTypeCode;
		 }
		 set
		 {
		   if(taxTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxTypeCode",OldValue=taxTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taxTypeCode=value;
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
	  private string tradeAgreementTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeAgreementTypeCode  
	   {
	    
	     get
		{
		   return tradeAgreementTypeCode;
		 }
		 set
		 {
		   if(tradeAgreementTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeAgreementTypeCode",OldValue=tradeAgreementTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeAgreementTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? taxRate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TaxRate  
	   {
	    
	     get
		{
		   return taxRate;
		 }
		 set
		 {
		   if(taxRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxRate",OldValue=taxRate,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   taxRate=value;
		   }
			
		 }
	   }
	  private decimal? taxBaseAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TaxBaseAmount  
	   {
	    
	     get
		{
		   return taxBaseAmount;
		 }
		 set
		 {
		   if(taxBaseAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxBaseAmount",OldValue=taxBaseAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   taxBaseAmount=value;
		   }
			
		 }
	   }
	  private decimal? taxAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TaxAmount  
	   {
	    
	     get
		{
		   return taxAmount;
		 }
		 set
		 {
		   if(taxAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxAmount",OldValue=taxAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   taxAmount=value;
		   }
			
		 }
	   }
	  private decimal? deferedTaxAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DeferedTaxAmount  
	   {
	    
	     get
		{
		   return deferedTaxAmount;
		 }
		 set
		 {
		   if(deferedTaxAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeferedTaxAmount",OldValue=deferedTaxAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   deferedTaxAmount=value;
		   }
			
		 }
	   }
	  private decimal? definedPerUnitMeasure ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DefinedPerUnitMeasure  
	   {
	    
	     get
		{
		   return definedPerUnitMeasure;
		 }
		 set
		 {
		   if(definedPerUnitMeasure != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefinedPerUnitMeasure",OldValue=definedPerUnitMeasure,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   definedPerUnitMeasure=value;
		   }
			
		 }
	   }
	  private decimal? alternateDefinedPerUnitMeasure ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? AlternateDefinedPerUnitMeasure  
	   {
	    
	     get
		{
		   return alternateDefinedPerUnitMeasure;
		 }
		 set
		 {
		   if(alternateDefinedPerUnitMeasure != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AlternateDefinedPerUnitMeasure",OldValue=alternateDefinedPerUnitMeasure,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   alternateDefinedPerUnitMeasure=value;
		   }
			
		 }
	   }
	  private decimal? definedPerUnitQuantity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DefinedPerUnitQuantity  
	   {
	    
	     get
		{
		   return definedPerUnitQuantity;
		 }
		 set
		 {
		   if(definedPerUnitQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefinedPerUnitQuantity",OldValue=definedPerUnitQuantity,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   definedPerUnitQuantity=value;
		   }
			
		 }
	   }
	  private decimal? alternateDefinedPerUnitQuant ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? AlternateDefinedPerUnitQuant  
	   {
	    
	     get
		{
		   return alternateDefinedPerUnitQuant;
		 }
		 set
		 {
		   if(alternateDefinedPerUnitQuant != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AlternateDefinedPerUnitQuant",OldValue=alternateDefinedPerUnitQuant,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   alternateDefinedPerUnitQuant=value;
		   }
			
		 }
	   }
	  private string measurementUnitCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MeasurementUnitCode  
	   {
	    
	     get
		{
		   return measurementUnitCode;
		 }
		 set
		 {
		   if(measurementUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeasurementUnitCode",OldValue=measurementUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   measurementUnitCode=value;
		   }
			
		 }
	   }
	  private string alternateMeasurementUnitCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AlternateMeasurementUnitCode  
	   {
	    
	     get
		{
		   return alternateMeasurementUnitCode;
		 }
		 set
		 {
		   if(alternateMeasurementUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AlternateMeasurementUnitCode",OldValue=alternateMeasurementUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   alternateMeasurementUnitCode=value;
		   }
			
		 }
	   }
	  private string tradeLevyNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeLevyNumber  
	   {
	    
	     get
		{
		   return tradeLevyNumber;
		 }
		 set
		 {
		   if(tradeLevyNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeLevyNumber",OldValue=tradeLevyNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeLevyNumber=value;
		   }
			
		 }
	   }
	  private decimal? totalBtlCoverageNIS ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotalBtlCoverageNIS  
	   {
	    
	     get
		{
		   return totalBtlCoverageNIS;
		 }
		 set
		 {
		   if(totalBtlCoverageNIS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalBtlCoverageNIS",OldValue=totalBtlCoverageNIS,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totalBtlCoverageNIS=value;
		   }
			
		 }
	   }
	  private decimal? alternateRate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? AlternateRate  
	   {
	    
	     get
		{
		   return alternateRate;
		 }
		 set
		 {
		   if(alternateRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AlternateRate",OldValue=alternateRate,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   alternateRate=value;
		   }
			
		 }
	   }
	  private string taxTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaxTypeName  
	   {
	    
	     get
		{
		   return taxTypeName;
		 }
		 set
		 {
		   if(taxTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxTypeName",OldValue=taxTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taxTypeName=value;
		   }
			
		 }
	   }
	  private string tradeAgreementTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeAgreementTypeName  
	   {
	    
	     get
		{
		   return tradeAgreementTypeName;
		 }
		 set
		 {
		   if(tradeAgreementTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeAgreementTypeName",OldValue=tradeAgreementTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeAgreementTypeName=value;
		   }
			
		 }
	   }
	  private string measurementUnitName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MeasurementUnitName  
	   {
	    
	     get
		{
		   return measurementUnitName;
		 }
		 set
		 {
		   if(measurementUnitName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeasurementUnitName",OldValue=measurementUnitName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   measurementUnitName=value;
		   }
			
		 }
	   }
	  private string alternateMeasurementUnitName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AlternateMeasurementUnitName  
	   {
	    
	     get
		{
		   return alternateMeasurementUnitName;
		 }
		 set
		 {
		   if(alternateMeasurementUnitName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AlternateMeasurementUnitName",OldValue=alternateMeasurementUnitName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   alternateMeasurementUnitName=value;
		   }
			
		 }
	   }
   }
   
}
	 