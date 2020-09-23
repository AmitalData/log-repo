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
   public partial class SupplierInvoiceItemPM : EntityPM
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
	  private int counterKey ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CounterKey  
	   {
	    
	     get
		{
		   return counterKey;
		 }
		 set
		 {
		   if(counterKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CounterKey",OldValue=counterKey,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   counterKey=value;
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
	  private string itemCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ItemCode  
	   {
	    
	     get
		{
		   return itemCode;
		 }
		 set
		 {
		   if(itemCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemCode",OldValue=itemCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   itemCode=value;
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
	  private string classificationCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClassificationCode  
	   {
	    
	     get
		{
		   return classificationCode;
		 }
		 set
		 {
		   if(classificationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClassificationCode",OldValue=classificationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   classificationCode=value;
		   }
			
		 }
	   }
	  private string dangerousClassificationCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DangerousClassificationCode  
	   {
	    
	     get
		{
		   return dangerousClassificationCode;
		 }
		 set
		 {
		   if(dangerousClassificationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DangerousClassificationCode",OldValue=dangerousClassificationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dangerousClassificationCode=value;
		   }
			
		 }
	   }
	  private string dangerousPackingGroupTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DangerousPackingGroupTypeCode  
	   {
	    
	     get
		{
		   return dangerousPackingGroupTypeCode;
		 }
		 set
		 {
		   if(dangerousPackingGroupTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DangerousPackingGroupTypeCode",OldValue=dangerousPackingGroupTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dangerousPackingGroupTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? itemPrice ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ItemPrice  
	   {
	    
	     get
		{
		   return itemPrice;
		 }
		 set
		 {
		   if(itemPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemPrice",OldValue=itemPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   itemPrice=value;
		   }
			
		 }
	   }
	  private decimal? nonCustomsItemPrice ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? NonCustomsItemPrice  
	   {
	    
	     get
		{
		   return nonCustomsItemPrice;
		 }
		 set
		 {
		   if(nonCustomsItemPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonCustomsItemPrice",OldValue=nonCustomsItemPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   nonCustomsItemPrice=value;
		   }
			
		 }
	   }
	  private decimal? wholeSaleItemPrice ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? WholeSaleItemPrice  
	   {
	    
	     get
		{
		   return wholeSaleItemPrice;
		 }
		 set
		 {
		   if(wholeSaleItemPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WholeSaleItemPrice",OldValue=wholeSaleItemPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   wholeSaleItemPrice=value;
		   }
			
		 }
	   }
	  private string manufactureIdentifier ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ManufactureIdentifier  
	   {
	    
	     get
		{
		   return manufactureIdentifier;
		 }
		 set
		 {
		   if(manufactureIdentifier != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ManufactureIdentifier",OldValue=manufactureIdentifier,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   manufactureIdentifier=value;
		   }
			
		 }
	   }
	  private string customsBookTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBookTypeCode  
	   {
	    
	     get
		{
		   return customsBookTypeCode;
		 }
		 set
		 {
		   if(customsBookTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBookTypeCode",OldValue=customsBookTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBookTypeCode=value;
		   }
			
		 }
	   }
	  private string taxExemptCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaxExemptCode  
	   {
	    
	     get
		{
		   return taxExemptCode;
		 }
		 set
		 {
		   if(taxExemptCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxExemptCode",OldValue=taxExemptCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taxExemptCode=value;
		   }
			
		 }
	   }
	  private decimal? optionalTamaPercentage ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? OptionalTamaPercentage  
	   {
	    
	     get
		{
		   return optionalTamaPercentage;
		 }
		 set
		 {
		   if(optionalTamaPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OptionalTamaPercentage",OldValue=optionalTamaPercentage,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   optionalTamaPercentage=value;
		   }
			
		 }
	   }
	  private string salesTaxExemptionTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SalesTaxExemptionTypeCode  
	   {
	    
	     get
		{
		   return salesTaxExemptionTypeCode;
		 }
		 set
		 {
		   if(salesTaxExemptionTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SalesTaxExemptionTypeCode",OldValue=salesTaxExemptionTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   salesTaxExemptionTypeCode=value;
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

	   private List<SupplierInvoiceItemsTaxPM> supplierInvoiceItemTaxes;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemsSupplierInvoiceItemsTax", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,LineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemsTaxPM> SupplierInvoiceItemTaxes  
	   {
	        get
             {
                 if (supplierInvoiceItemTaxes == null)
                 {
                     supplierInvoiceItemTaxes = new List<SupplierInvoiceItemsTaxPM>();
                 }
                 return supplierInvoiceItemTaxes;
              }
             set { supplierInvoiceItemTaxes = value; }
	    }
		   
	   private List<SupplierInvoiceItemsTaxPM>  deletedSupplierInvoiceItemTaxes;
	   public virtual List<SupplierInvoiceItemsTaxPM> DeletedSupplierInvoiceItemTaxes  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemTaxes == null)
                 {
                      deletedSupplierInvoiceItemTaxes = new List<SupplierInvoiceItemsTaxPM>();
                 }
                 return  deletedSupplierInvoiceItemTaxes;
              }
             set {  deletedSupplierInvoiceItemTaxes = value; }
	    }
	  
	   private List<SupplierInvoiceItemsConDeclarPM> supplierInvoiceItemsConDeclars;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemSupplierInvoiceItemsConDeclars", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemsConDeclarPM> SupplierInvoiceItemsConDeclars  
	   {
	        get
             {
                 if (supplierInvoiceItemsConDeclars == null)
                 {
                     supplierInvoiceItemsConDeclars = new List<SupplierInvoiceItemsConDeclarPM>();
                 }
                 return supplierInvoiceItemsConDeclars;
              }
             set { supplierInvoiceItemsConDeclars = value; }
	    }
		   
	   private List<SupplierInvoiceItemsConDeclarPM>  deletedSupplierInvoiceItemsConDeclars;
	   public virtual List<SupplierInvoiceItemsConDeclarPM> DeletedSupplierInvoiceItemsConDeclars  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemsConDeclars == null)
                 {
                      deletedSupplierInvoiceItemsConDeclars = new List<SupplierInvoiceItemsConDeclarPM>();
                 }
                 return  deletedSupplierInvoiceItemsConDeclars;
              }
             set {  deletedSupplierInvoiceItemsConDeclars = value; }
	    }
	  
	   private List<SupplierInvioceItemCertificatPM> supplierInvioceItemCertificats;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvioceItemSupplierInvioceItemCertificats", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,LineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvioceItemCertificatPM> SupplierInvioceItemCertificats  
	   {
	        get
             {
                 if (supplierInvioceItemCertificats == null)
                 {
                     supplierInvioceItemCertificats = new List<SupplierInvioceItemCertificatPM>();
                 }
                 return supplierInvioceItemCertificats;
              }
             set { supplierInvioceItemCertificats = value; }
	    }
		   
	   private List<SupplierInvioceItemCertificatPM>  deletedSupplierInvioceItemCertificats;
	   public virtual List<SupplierInvioceItemCertificatPM> DeletedSupplierInvioceItemCertificats  
	   {
	        get
             {
                 if ( deletedSupplierInvioceItemCertificats == null)
                 {
                      deletedSupplierInvioceItemCertificats = new List<SupplierInvioceItemCertificatPM>();
                 }
                 return  deletedSupplierInvioceItemCertificats;
              }
             set {  deletedSupplierInvioceItemCertificats = value; }
	    }
	  
	   private List<SupplierInvoiceItemsModPM> supplierInvoiceItemsMods;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemsSupplierInvoiceItemsMods", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,LineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemsModPM> SupplierInvoiceItemsMods  
	   {
	        get
             {
                 if (supplierInvoiceItemsMods == null)
                 {
                     supplierInvoiceItemsMods = new List<SupplierInvoiceItemsModPM>();
                 }
                 return supplierInvoiceItemsMods;
              }
             set { supplierInvoiceItemsMods = value; }
	    }
		   
	   private List<SupplierInvoiceItemsModPM>  deletedSupplierInvoiceItemsMods;
	   public virtual List<SupplierInvoiceItemsModPM> DeletedSupplierInvoiceItemsMods  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemsMods == null)
                 {
                      deletedSupplierInvoiceItemsMods = new List<SupplierInvoiceItemsModPM>();
                 }
                 return  deletedSupplierInvoiceItemsMods;
              }
             set {  deletedSupplierInvoiceItemsMods = value; }
	    }
	  	  private string itemPriceCurrencyCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ItemPriceCurrencyCode  
	   {
	    
	     get
		{
		   return itemPriceCurrencyCode;
		 }
		 set
		 {
		   if(itemPriceCurrencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemPriceCurrencyCode",OldValue=itemPriceCurrencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   itemPriceCurrencyCode=value;
		   }
			
		 }
	   }
	  private string nonCustomsItemPriceCurCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NonCustomsItemPriceCurCode  
	   {
	    
	     get
		{
		   return nonCustomsItemPriceCurCode;
		 }
		 set
		 {
		   if(nonCustomsItemPriceCurCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NonCustomsItemPriceCurCode",OldValue=nonCustomsItemPriceCurCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nonCustomsItemPriceCurCode=value;
		   }
			
		 }
	   }
	  private string wholeSaleItemPriceCurrencyCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string WholeSaleItemPriceCurrencyCode  
	   {
	    
	     get
		{
		   return wholeSaleItemPriceCurrencyCode;
		 }
		 set
		 {
		   if(wholeSaleItemPriceCurrencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WholeSaleItemPriceCurrencyCode",OldValue=wholeSaleItemPriceCurrencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   wholeSaleItemPriceCurrencyCode=value;
		   }
			
		 }
	   }
	  private string actualInvoiceLines ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActualInvoiceLines  
	   {
	    
	     get
		{
		   return actualInvoiceLines;
		 }
		 set
		 {
		   if(actualInvoiceLines != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActualInvoiceLines",OldValue=actualInvoiceLines,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   actualInvoiceLines=value;
		   }
			
		 }
	   }

	   private List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNums;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemSupplierInvoiceItemsSerialNum", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemsSerialNumPM> SupplierInvoiceItemsSerialNums  
	   {
	        get
             {
                 if (supplierInvoiceItemsSerialNums == null)
                 {
                     supplierInvoiceItemsSerialNums = new List<SupplierInvoiceItemsSerialNumPM>();
                 }
                 return supplierInvoiceItemsSerialNums;
              }
             set { supplierInvoiceItemsSerialNums = value; }
	    }
		   
	   private List<SupplierInvoiceItemsSerialNumPM>  deletedSupplierInvoiceItemsSerialNums;
	   public virtual List<SupplierInvoiceItemsSerialNumPM> DeletedSupplierInvoiceItemsSerialNums  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemsSerialNums == null)
                 {
                      deletedSupplierInvoiceItemsSerialNums = new List<SupplierInvoiceItemsSerialNumPM>();
                 }
                 return  deletedSupplierInvoiceItemsSerialNums;
              }
             set {  deletedSupplierInvoiceItemsSerialNums = value; }
	    }
	  
	   private List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescripts;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemSupplierInvoiceItemsDescript", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemsDescriptPM> SupplierInvoiceItemsDescripts  
	   {
	        get
             {
                 if (supplierInvoiceItemsDescripts == null)
                 {
                     supplierInvoiceItemsDescripts = new List<SupplierInvoiceItemsDescriptPM>();
                 }
                 return supplierInvoiceItemsDescripts;
              }
             set { supplierInvoiceItemsDescripts = value; }
	    }
		   
	   private List<SupplierInvoiceItemsDescriptPM>  deletedSupplierInvoiceItemsDescripts;
	   public virtual List<SupplierInvoiceItemsDescriptPM> DeletedSupplierInvoiceItemsDescripts  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemsDescripts == null)
                 {
                      deletedSupplierInvoiceItemsDescripts = new List<SupplierInvoiceItemsDescriptPM>();
                 }
                 return  deletedSupplierInvoiceItemsDescripts;
              }
             set {  deletedSupplierInvoiceItemsDescripts = value; }
	    }
	  
	   private List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProdIdents;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemSupplierInvoiceItemsProdIdents", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemsProdIdentPM> SupplierInvoiceItemsProdIdents  
	   {
	        get
             {
                 if (supplierInvoiceItemsProdIdents == null)
                 {
                     supplierInvoiceItemsProdIdents = new List<SupplierInvoiceItemsProdIdentPM>();
                 }
                 return supplierInvoiceItemsProdIdents;
              }
             set { supplierInvoiceItemsProdIdents = value; }
	    }
		   
	   private List<SupplierInvoiceItemsProdIdentPM>  deletedSupplierInvoiceItemsProdIdents;
	   public virtual List<SupplierInvoiceItemsProdIdentPM> DeletedSupplierInvoiceItemsProdIdents  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemsProdIdents == null)
                 {
                      deletedSupplierInvoiceItemsProdIdents = new List<SupplierInvoiceItemsProdIdentPM>();
                 }
                 return  deletedSupplierInvoiceItemsProdIdents;
              }
             set {  deletedSupplierInvoiceItemsProdIdents = value; }
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
	  private string tradeAgreementCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeAgreementCode  
	   {
	    
	     get
		{
		   return tradeAgreementCode;
		 }
		 set
		 {
		   if(tradeAgreementCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeAgreementCode",OldValue=tradeAgreementCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeAgreementCode=value;
		   }
			
		 }
	   }
	  private string tradeAgreementName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeAgreementName  
	   {
	    
	     get
		{
		   return tradeAgreementName;
		 }
		 set
		 {
		   if(tradeAgreementName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeAgreementName",OldValue=tradeAgreementName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeAgreementName=value;
		   }
			
		 }
	   }

	   private List<SupplierInvoiceItemProcesTypePM> supplierInvoiceItemProcesTypes;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemSupplierInvoiceItemProcesType", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemProcesTypePM> SupplierInvoiceItemProcesTypes  
	   {
	        get
             {
                 if (supplierInvoiceItemProcesTypes == null)
                 {
                     supplierInvoiceItemProcesTypes = new List<SupplierInvoiceItemProcesTypePM>();
                 }
                 return supplierInvoiceItemProcesTypes;
              }
             set { supplierInvoiceItemProcesTypes = value; }
	    }
		   
	   private List<SupplierInvoiceItemProcesTypePM>  deletedSupplierInvoiceItemProcesTypes;
	   public virtual List<SupplierInvoiceItemProcesTypePM> DeletedSupplierInvoiceItemProcesTypes  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemProcesTypes == null)
                 {
                      deletedSupplierInvoiceItemProcesTypes = new List<SupplierInvoiceItemProcesTypePM>();
                 }
                 return  deletedSupplierInvoiceItemProcesTypes;
              }
             set {  deletedSupplierInvoiceItemProcesTypes = value; }
	    }
	  	  private decimal? statisticQuantity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? StatisticQuantity  
	   {
	    
	     get
		{
		   return statisticQuantity;
		 }
		 set
		 {
		   if(statisticQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatisticQuantity",OldValue=statisticQuantity,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   statisticQuantity=value;
		   }
			
		 }
	   }
	  private decimal? invoiceQuantity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? InvoiceQuantity  
	   {
	    
	     get
		{
		   return invoiceQuantity;
		 }
		 set
		 {
		   if(invoiceQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceQuantity",OldValue=invoiceQuantity,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   invoiceQuantity=value;
		   }
			
		 }
	   }
	  private decimal? additionalQuantity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? AdditionalQuantity  
	   {
	    
	     get
		{
		   return additionalQuantity;
		 }
		 set
		 {
		   if(additionalQuantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AdditionalQuantity",OldValue=additionalQuantity,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   additionalQuantity=value;
		   }
			
		 }
	   }
	  private string invoiceQuantityType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InvoiceQuantityType  
	   {
	    
	     get
		{
		   return invoiceQuantityType;
		 }
		 set
		 {
		   if(invoiceQuantityType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceQuantityType",OldValue=invoiceQuantityType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   invoiceQuantityType=value;
		   }
			
		 }
	   }
	  private string statisticQuantityType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatisticQuantityType  
	   {
	    
	     get
		{
		   return statisticQuantityType;
		 }
		 set
		 {
		   if(statisticQuantityType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatisticQuantityType",OldValue=statisticQuantityType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statisticQuantityType=value;
		   }
			
		 }
	   }
	  private string additionalQuantityType ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AdditionalQuantityType  
	   {
	    
	     get
		{
		   return additionalQuantityType;
		 }
		 set
		 {
		   if(additionalQuantityType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AdditionalQuantityType",OldValue=additionalQuantityType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   additionalQuantityType=value;
		   }
			
		 }
	   }
	  private string invoiceQuantityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InvoiceQuantityTypeName  
	   {
	    
	     get
		{
		   return invoiceQuantityTypeName;
		 }
		 set
		 {
		   if(invoiceQuantityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceQuantityTypeName",OldValue=invoiceQuantityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   invoiceQuantityTypeName=value;
		   }
			
		 }
	   }
	  private string statisticQuantityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatisticQuantityTypeName  
	   {
	    
	     get
		{
		   return statisticQuantityTypeName;
		 }
		 set
		 {
		   if(statisticQuantityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatisticQuantityTypeName",OldValue=statisticQuantityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statisticQuantityTypeName=value;
		   }
			
		 }
	   }
	  private string additionalQuantityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AdditionalQuantityTypeName  
	   {
	    
	     get
		{
		   return additionalQuantityTypeName;
		 }
		 set
		 {
		   if(additionalQuantityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AdditionalQuantityTypeName",OldValue=additionalQuantityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   additionalQuantityTypeName=value;
		   }
			
		 }
	   }
	  private string taxExemptName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaxExemptName  
	   {
	    
	     get
		{
		   return taxExemptName;
		 }
		 set
		 {
		   if(taxExemptName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxExemptName",OldValue=taxExemptName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taxExemptName=value;
		   }
			
		 }
	   }

	   private List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemLevies;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemSupplierInvoiceItemLevy", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemsLevyPM> SupplierInvoiceItemLevies  
	   {
	        get
             {
                 if (supplierInvoiceItemLevies == null)
                 {
                     supplierInvoiceItemLevies = new List<SupplierInvoiceItemsLevyPM>();
                 }
                 return supplierInvoiceItemLevies;
              }
             set { supplierInvoiceItemLevies = value; }
	    }
		   
	   private List<SupplierInvoiceItemsLevyPM>  deletedSupplierInvoiceItemLevies;
	   public virtual List<SupplierInvoiceItemsLevyPM> DeletedSupplierInvoiceItemLevies  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemLevies == null)
                 {
                      deletedSupplierInvoiceItemLevies = new List<SupplierInvoiceItemsLevyPM>();
                 }
                 return  deletedSupplierInvoiceItemLevies;
              }
             set {  deletedSupplierInvoiceItemLevies = value; }
	    }
	  	  private string preferenceDocumentNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreferenceDocumentNumber  
	   {
	    
	     get
		{
		   return preferenceDocumentNumber;
		 }
		 set
		 {
		   if(preferenceDocumentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreferenceDocumentNumber",OldValue=preferenceDocumentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preferenceDocumentNumber=value;
		   }
			
		 }
	   }

	   private List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehicles;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("Customs.SupplierInvoiceItemCustoms.SupplierInvoiceItemVehicles", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemVehiclePM> SupplierInvoiceItemVehicles  
	   {
	        get
             {
                 if (supplierInvoiceItemVehicles == null)
                 {
                     supplierInvoiceItemVehicles = new List<SupplierInvoiceItemVehiclePM>();
                 }
                 return supplierInvoiceItemVehicles;
              }
             set { supplierInvoiceItemVehicles = value; }
	    }
		   
	   private List<SupplierInvoiceItemVehiclePM>  deletedSupplierInvoiceItemVehicles;
	   public virtual List<SupplierInvoiceItemVehiclePM> DeletedSupplierInvoiceItemVehicles  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemVehicles == null)
                 {
                      deletedSupplierInvoiceItemVehicles = new List<SupplierInvoiceItemVehiclePM>();
                 }
                 return  deletedSupplierInvoiceItemVehicles;
              }
             set {  deletedSupplierInvoiceItemVehicles = value; }
	    }
	  	  private string itemDescription ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ItemDescription  
	   {
	    
	     get
		{
		   return itemDescription;
		 }
		 set
		 {
		   if(itemDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemDescription",OldValue=itemDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   itemDescription=value;
		   }
			
		 }
	   }
	  private bool isItemChanged ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsItemChanged  
	   {
	    
	     get
		{
		   return isItemChanged;
		 }
		 set
		 {
		   if(isItemChanged != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsItemChanged",OldValue=isItemChanged,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isItemChanged=value;
		   }
			
		 }
	   }
	  private string catalogNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CatalogNumber  
	   {
	    
	     get
		{
		   return catalogNumber;
		 }
		 set
		 {
		   if(catalogNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CatalogNumber",OldValue=catalogNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   catalogNumber=value;
		   }
			
		 }
	   }

	   private List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehicles;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemSupplierInvoiceItemModVehicle", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemModVehiclePM> SupplierInvoiceItemModVehicles  
	   {
	        get
             {
                 if (supplierInvoiceItemModVehicles == null)
                 {
                     supplierInvoiceItemModVehicles = new List<SupplierInvoiceItemModVehiclePM>();
                 }
                 return supplierInvoiceItemModVehicles;
              }
             set { supplierInvoiceItemModVehicles = value; }
	    }
		   
	   private List<SupplierInvoiceItemModVehiclePM>  deletedSupplierInvoiceItemModVehicles;
	   public virtual List<SupplierInvoiceItemModVehiclePM> DeletedSupplierInvoiceItemModVehicles  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemModVehicles == null)
                 {
                      deletedSupplierInvoiceItemModVehicles = new List<SupplierInvoiceItemModVehiclePM>();
                 }
                 return  deletedSupplierInvoiceItemModVehicles;
              }
             set {  deletedSupplierInvoiceItemModVehicles = value; }
	    }
	  	  private string certificatesStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CertificatesStatusCode  
	   {
	    
	     get
		{
		   return certificatesStatusCode;
		 }
		 set
		 {
		   if(certificatesStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CertificatesStatusCode",OldValue=certificatesStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   certificatesStatusCode=value;
		   }
			
		 }
	   }
	  private bool isUsed ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string invoiceNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InvoiceNumber  
	   {
	    
	     get
		{
		   return invoiceNumber;
		 }
		 set
		 {
		   if(invoiceNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceNumber",OldValue=invoiceNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   invoiceNumber=value;
		   }
			
		 }
	   }
	  private decimal? deferredCustomsTax ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DeferredCustomsTax  
	   {
	    
	     get
		{
		   return deferredCustomsTax;
		 }
		 set
		 {
		   if(deferredCustomsTax != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeferredCustomsTax",OldValue=deferredCustomsTax,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   deferredCustomsTax=value;
		   }
			
		 }
	   }
	  private decimal? deferredPurchaseTax ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DeferredPurchaseTax  
	   {
	    
	     get
		{
		   return deferredPurchaseTax;
		 }
		 set
		 {
		   if(deferredPurchaseTax != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeferredPurchaseTax",OldValue=deferredPurchaseTax,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   deferredPurchaseTax=value;
		   }
			
		 }
	   }
	  private bool vehicleStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool VehicleStatus  
	   {
	    
	     get
		{
		   return vehicleStatus;
		 }
		 set
		 {
		   if(vehicleStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleStatus",OldValue=vehicleStatus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   vehicleStatus=value;
		   }
			
		 }
	   }
	  private bool itemAdditionalStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ItemAdditionalStatus  
	   {
	    
	     get
		{
		   return itemAdditionalStatus;
		 }
		 set
		 {
		   if(itemAdditionalStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemAdditionalStatus",OldValue=itemAdditionalStatus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   itemAdditionalStatus=value;
		   }
			
		 }
	   }
	  private string itemHash ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ItemHash  
	   {
	    
	     get
		{
		   return itemHash;
		 }
		 set
		 {
		   if(itemHash != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemHash",OldValue=itemHash,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   itemHash=value;
		   }
			
		 }
	   }
	  private int? parentLineNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ParentLineNumber  
	   {
	    
	     get
		{
		   return parentLineNumber;
		 }
		 set
		 {
		   if(parentLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParentLineNumber",OldValue=parentLineNumber,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   parentLineNumber=value;
		   }
			
		 }
	   }
	  private bool notForAccumaltion ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool NotForAccumaltion  
	   {
	    
	     get
		{
		   return notForAccumaltion;
		 }
		 set
		 {
		   if(notForAccumaltion != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotForAccumaltion",OldValue=notForAccumaltion,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   notForAccumaltion=value;
		   }
			
		 }
	   }
	  private bool isParent ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsParent  
	   {
	    
	     get
		{
		   return isParent;
		 }
		 set
		 {
		   if(isParent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsParent",OldValue=isParent,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isParent=value;
		   }
			
		 }
	   }
	  private int? unfInvoiceLine ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? UnfInvoiceLine  
	   {
	    
	     get
		{
		   return unfInvoiceLine;
		 }
		 set
		 {
		   if(unfInvoiceLine != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UnfInvoiceLine",OldValue=unfInvoiceLine,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   unfInvoiceLine=value;
		   }
			
		 }
	   }
	  private string orderByLineNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OrderByLineNo  
	   {
	    
	     get
		{
		   return orderByLineNo;
		 }
		 set
		 {
		   if(orderByLineNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OrderByLineNo",OldValue=orderByLineNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   orderByLineNo=value;
		   }
			
		 }
	   }
	  private bool isCopy ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCopy  
	   {
	    
	     get
		{
		   return isCopy;
		 }
		 set
		 {
		   if(isCopy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCopy",OldValue=isCopy,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCopy=value;
		   }
			
		 }
	   }
	  private string lastCopyFromOrderNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastCopyFromOrderNo  
	   {
	    
	     get
		{
		   return lastCopyFromOrderNo;
		 }
		 set
		 {
		   if(lastCopyFromOrderNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastCopyFromOrderNo",OldValue=lastCopyFromOrderNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastCopyFromOrderNo=value;
		   }
			
		 }
	   }
	  private string clasifiedRemarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClasifiedRemarks  
	   {
	    
	     get
		{
		   return clasifiedRemarks;
		 }
		 set
		 {
		   if(clasifiedRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClasifiedRemarks",OldValue=clasifiedRemarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   clasifiedRemarks=value;
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
	  private string marksAndNumbers ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MarksAndNumbers  
	   {
	    
	     get
		{
		   return marksAndNumbers;
		 }
		 set
		 {
		   if(marksAndNumbers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarksAndNumbers",OldValue=marksAndNumbers,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   marksAndNumbers=value;
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
	  private decimal? weight ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private decimal ocrHeight ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal OcrHeight  
	   {
	    
	     get
		{
		   return ocrHeight;
		 }
		 set
		 {
		   if(ocrHeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OcrHeight",OldValue=ocrHeight,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   ocrHeight=value;
		   }
			
		 }
	   }
	  private decimal ocrTop ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal OcrTop  
	   {
	    
	     get
		{
		   return ocrTop;
		 }
		 set
		 {
		   if(ocrTop != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OcrTop",OldValue=ocrTop,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   ocrTop=value;
		   }
			
		 }
	   }
	  private decimal ocrPageNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal OcrPageNumber  
	   {
	    
	     get
		{
		   return ocrPageNumber;
		 }
		 set
		 {
		   if(ocrPageNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OcrPageNumber",OldValue=ocrPageNumber,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   ocrPageNumber=value;
		   }
			
		 }
	   }
	  private string classificationTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClassificationTypeCode  
	   {
	    
	     get
		{
		   return classificationTypeCode;
		 }
		 set
		 {
		   if(classificationTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClassificationTypeCode",OldValue=classificationTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   classificationTypeCode=value;
		   }
			
		 }
	   }
	  private string transactionNatureCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransactionNatureCode  
	   {
	    
	     get
		{
		   return transactionNatureCode;
		 }
		 set
		 {
		   if(transactionNatureCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransactionNatureCode",OldValue=transactionNatureCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transactionNatureCode=value;
		   }
			
		 }
	   }
	  private string claimReasonCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClaimReasonCode  
	   {
	    
	     get
		{
		   return claimReasonCode;
		 }
		 set
		 {
		   if(claimReasonCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClaimReasonCode",OldValue=claimReasonCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   claimReasonCode=value;
		   }
			
		 }
	   }
	  private string classificationTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClassificationTypeName  
	   {
	    
	     get
		{
		   return classificationTypeName;
		 }
		 set
		 {
		   if(classificationTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClassificationTypeName",OldValue=classificationTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   classificationTypeName=value;
		   }
			
		 }
	   }
	  private string transactionNatureName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransactionNatureName  
	   {
	    
	     get
		{
		   return transactionNatureName;
		 }
		 set
		 {
		   if(transactionNatureName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransactionNatureName",OldValue=transactionNatureName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transactionNatureName=value;
		   }
			
		 }
	   }
	  private string claimReasonName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClaimReasonName  
	   {
	    
	     get
		{
		   return claimReasonName;
		 }
		 set
		 {
		   if(claimReasonName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClaimReasonName",OldValue=claimReasonName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   claimReasonName=value;
		   }
			
		 }
	   }

	   private List<SupplierInvoiceItemsPricePM> supplierInvoiceItemsPrices;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemsPrices", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemsPricePM> SupplierInvoiceItemsPrices  
	   {
	        get
             {
                 if (supplierInvoiceItemsPrices == null)
                 {
                     supplierInvoiceItemsPrices = new List<SupplierInvoiceItemsPricePM>();
                 }
                 return supplierInvoiceItemsPrices;
              }
             set { supplierInvoiceItemsPrices = value; }
	    }
		   
	   private List<SupplierInvoiceItemsPricePM>  deletedSupplierInvoiceItemsPrices;
	   public virtual List<SupplierInvoiceItemsPricePM> DeletedSupplierInvoiceItemsPrices  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemsPrices == null)
                 {
                      deletedSupplierInvoiceItemsPrices = new List<SupplierInvoiceItemsPricePM>();
                 }
                 return  deletedSupplierInvoiceItemsPrices;
              }
             set {  deletedSupplierInvoiceItemsPrices = value; }
	    }
	  
	   private List<SuppInvoiceItemsAbachStatementPM> suppInvoiceItemsAbachStatements;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SuppInvoiceItemsAbachStatements", "DeclarationId,CounterKey,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber")]
	   [DataMember]
	   public virtual List<SuppInvoiceItemsAbachStatementPM> SuppInvoiceItemsAbachStatements  
	   {
	        get
             {
                 if (suppInvoiceItemsAbachStatements == null)
                 {
                     suppInvoiceItemsAbachStatements = new List<SuppInvoiceItemsAbachStatementPM>();
                 }
                 return suppInvoiceItemsAbachStatements;
              }
             set { suppInvoiceItemsAbachStatements = value; }
	    }
		   
	   private List<SuppInvoiceItemsAbachStatementPM>  deletedSuppInvoiceItemsAbachStatements;
	   public virtual List<SuppInvoiceItemsAbachStatementPM> DeletedSuppInvoiceItemsAbachStatements  
	   {
	        get
             {
                 if ( deletedSuppInvoiceItemsAbachStatements == null)
                 {
                      deletedSuppInvoiceItemsAbachStatements = new List<SuppInvoiceItemsAbachStatementPM>();
                 }
                 return  deletedSuppInvoiceItemsAbachStatements;
              }
             set {  deletedSuppInvoiceItemsAbachStatements = value; }
	    }
	  	  private decimal? itemFOBAmountForeign ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ItemFOBAmountForeign  
	   {
	    
	     get
		{
		   return itemFOBAmountForeign;
		 }
		 set
		 {
		   if(itemFOBAmountForeign != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemFOBAmountForeign",OldValue=itemFOBAmountForeign,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   itemFOBAmountForeign=value;
		   }
			
		 }
	   }
	  private decimal? itemFOBAmountNIS ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ItemFOBAmountNIS  
	   {
	    
	     get
		{
		   return itemFOBAmountNIS;
		 }
		 set
		 {
		   if(itemFOBAmountNIS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ItemFOBAmountNIS",OldValue=itemFOBAmountNIS,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   itemFOBAmountNIS=value;
		   }
			
		 }
	   }
   }
   
}
	 