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
   public partial class SupplierInvoicePM : EntityPM
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
	  private DateTime? issueDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? IssueDate  
	   {
	    
	     get
		{
		   return issueDate;
		 }
		 set
		 {
		   if(issueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssueDate",OldValue=issueDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   issueDate=value;
		   }
			
		 }
	   }
	  private string accountTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountTypeCode  
	   {
	    
	     get
		{
		   return accountTypeCode;
		 }
		 set
		 {
		   if(accountTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountTypeCode",OldValue=accountTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountTypeCode=value;
		   }
			
		 }
	   }
	  private bool isPreference ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPreference  
	   {
	    
	     get
		{
		   return isPreference;
		 }
		 set
		 {
		   if(isPreference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPreference",OldValue=isPreference,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPreference=value;
		   }
			
		 }
	   }
	  private string preferenceDocumentTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreferenceDocumentTypeCode  
	   {
	    
	     get
		{
		   return preferenceDocumentTypeCode;
		 }
		 set
		 {
		   if(preferenceDocumentTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreferenceDocumentTypeCode",OldValue=preferenceDocumentTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preferenceDocumentTypeCode=value;
		   }
			
		 }
	   }
	  private string paymentTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentTypeCode  
	   {
	    
	     get
		{
		   return paymentTypeCode;
		 }
		 set
		 {
		   if(paymentTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentTypeCode",OldValue=paymentTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentTypeCode=value;
		   }
			
		 }
	   }
	  private string invoiceCurrencyTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InvoiceCurrencyTypeCode  
	   {
	    
	     get
		{
		   return invoiceCurrencyTypeCode;
		 }
		 set
		 {
		   if(invoiceCurrencyTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceCurrencyTypeCode",OldValue=invoiceCurrencyTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   invoiceCurrencyTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? invoiceAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? InvoiceAmount  
	   {
	    
	     get
		{
		   return invoiceAmount;
		 }
		 set
		 {
		   if(invoiceAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceAmount",OldValue=invoiceAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   invoiceAmount=value;
		   }
			
		 }
	   }
	  private string actualPayedCurrencyTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActualPayedCurrencyTypeCode  
	   {
	    
	     get
		{
		   return actualPayedCurrencyTypeCode;
		 }
		 set
		 {
		   if(actualPayedCurrencyTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActualPayedCurrencyTypeCode",OldValue=actualPayedCurrencyTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   actualPayedCurrencyTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? actualPayedAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ActualPayedAmount  
	   {
	    
	     get
		{
		   return actualPayedAmount;
		 }
		 set
		 {
		   if(actualPayedAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActualPayedAmount",OldValue=actualPayedAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   actualPayedAmount=value;
		   }
			
		 }
	   }
	  private string vendorId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string incotermCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IncotermCode  
	   {
	    
	     get
		{
		   return incotermCode;
		 }
		 set
		 {
		   if(incotermCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncotermCode",OldValue=incotermCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   incotermCode=value;
		   }
			
		 }
	   }
	  private string issueCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IssueCountryCode  
	   {
	    
	     get
		{
		   return issueCountryCode;
		 }
		 set
		 {
		   if(issueCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssueCountryCode",OldValue=issueCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   issueCountryCode=value;
		   }
			
		 }
	   }
	  private string paymentTermsCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentTermsCode  
	   {
	    
	     get
		{
		   return paymentTermsCode;
		 }
		 set
		 {
		   if(paymentTermsCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentTermsCode",OldValue=paymentTermsCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentTermsCode=value;
		   }
			
		 }
	   }
	  private decimal? totalFreightInFreightCurrency ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotalFreightInFreightCurrency  
	   {
	    
	     get
		{
		   return totalFreightInFreightCurrency;
		 }
		 set
		 {
		   if(totalFreightInFreightCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalFreightInFreightCurrency",OldValue=totalFreightInFreightCurrency,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totalFreightInFreightCurrency=value;
		   }
			
		 }
	   }
	  private decimal? totalFreightInNIS ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotalFreightInNIS  
	   {
	    
	     get
		{
		   return totalFreightInNIS;
		 }
		 set
		 {
		   if(totalFreightInNIS != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalFreightInNIS",OldValue=totalFreightInNIS,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totalFreightInNIS=value;
		   }
			
		 }
	   }
	  private decimal? exchangeRate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ExchangeRate  
	   {
	    
	     get
		{
		   return exchangeRate;
		 }
		 set
		 {
		   if(exchangeRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExchangeRate",OldValue=exchangeRate,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   exchangeRate=value;
		   }
			
		 }
	   }

	   private List<SupplierInvoiceItemPM> supplierInvoiceItems;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceSupplierInvoiceItems", "DeclarationId,InvoiceCounterKey","DeclarationId,CounterKey")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemPM> SupplierInvoiceItems  
	   {
	        get
             {
                 if (supplierInvoiceItems == null)
                 {
                     supplierInvoiceItems = new List<SupplierInvoiceItemPM>();
                 }
                 return supplierInvoiceItems;
              }
             set { supplierInvoiceItems = value; }
	    }
		   
	   private List<SupplierInvoiceItemPM>  deletedSupplierInvoiceItems;
	   public virtual List<SupplierInvoiceItemPM> DeletedSupplierInvoiceItems  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItems == null)
                 {
                      deletedSupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
                 }
                 return  deletedSupplierInvoiceItems;
              }
             set {  deletedSupplierInvoiceItems = value; }
	    }
	  
	   private List<SupplierInvoiceModificationPM> supplierInvoiceModifications;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceSupplierInvoiceModification", "DeclarationId,InvoiceCounterKey","DeclarationId,InvoiceCounterKey")]
	   [DataMember]
	   public virtual List<SupplierInvoiceModificationPM> SupplierInvoiceModifications  
	   {
	        get
             {
                 if (supplierInvoiceModifications == null)
                 {
                     supplierInvoiceModifications = new List<SupplierInvoiceModificationPM>();
                 }
                 return supplierInvoiceModifications;
              }
             set { supplierInvoiceModifications = value; }
	    }
		   
	   private List<SupplierInvoiceModificationPM>  deletedSupplierInvoiceModifications;
	   public virtual List<SupplierInvoiceModificationPM> DeletedSupplierInvoiceModifications  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceModifications == null)
                 {
                      deletedSupplierInvoiceModifications = new List<SupplierInvoiceModificationPM>();
                 }
                 return  deletedSupplierInvoiceModifications;
              }
             set {  deletedSupplierInvoiceModifications = value; }
	    }
	  	  private string issueCountryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IssueCountryName  
	   {
	    
	     get
		{
		   return issueCountryName;
		 }
		 set
		 {
		   if(issueCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssueCountryName",OldValue=issueCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   issueCountryName=value;
		   }
			
		 }
	   }
	  private string preferenceDocumentTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreferenceDocumentTypeName  
	   {
	    
	     get
		{
		   return preferenceDocumentTypeName;
		 }
		 set
		 {
		   if(preferenceDocumentTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreferenceDocumentTypeName",OldValue=preferenceDocumentTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   preferenceDocumentTypeName=value;
		   }
			
		 }
	   }

	   private List<SupplierInvoiceFreightAmountPM> supplierInvoiceFreightAmounts;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceFreightAmounts", "DeclarationId, InvoiceCounterKey","DeclarationId, InvoiceCounterKey")]
	   [DataMember]
	   public virtual List<SupplierInvoiceFreightAmountPM> SupplierInvoiceFreightAmounts  
	   {
	        get
             {
                 if (supplierInvoiceFreightAmounts == null)
                 {
                     supplierInvoiceFreightAmounts = new List<SupplierInvoiceFreightAmountPM>();
                 }
                 return supplierInvoiceFreightAmounts;
              }
             set { supplierInvoiceFreightAmounts = value; }
	    }
		   
	   private List<SupplierInvoiceFreightAmountPM>  deletedSupplierInvoiceFreightAmounts;
	   public virtual List<SupplierInvoiceFreightAmountPM> DeletedSupplierInvoiceFreightAmounts  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceFreightAmounts == null)
                 {
                      deletedSupplierInvoiceFreightAmounts = new List<SupplierInvoiceFreightAmountPM>();
                 }
                 return  deletedSupplierInvoiceFreightAmounts;
              }
             set {  deletedSupplierInvoiceFreightAmounts = value; }
	    }
	  	  private string insruanceCurrencyTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InsruanceCurrencyTypeCode  
	   {
	    
	     get
		{
		   return insruanceCurrencyTypeCode;
		 }
		 set
		 {
		   if(insruanceCurrencyTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InsruanceCurrencyTypeCode",OldValue=insruanceCurrencyTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   insruanceCurrencyTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? insuranceAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? InsuranceAmount  
	   {
	    
	     get
		{
		   return insuranceAmount;
		 }
		 set
		 {
		   if(insuranceAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InsuranceAmount",OldValue=insuranceAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   insuranceAmount=value;
		   }
			
		 }
	   }
	  private string vendorName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private decimal? insruancePercentage ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? InsruancePercentage  
	   {
	    
	     get
		{
		   return insruancePercentage;
		 }
		 set
		 {
		   if(insruancePercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InsruancePercentage",OldValue=insruancePercentage,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   insruancePercentage=value;
		   }
			
		 }
	   }
	  private string freightCurrencyTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreightCurrencyTypeCode  
	   {
	    
	     get
		{
		   return freightCurrencyTypeCode;
		 }
		 set
		 {
		   if(freightCurrencyTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreightCurrencyTypeCode",OldValue=freightCurrencyTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freightCurrencyTypeCode=value;
		   }
			
		 }
	   }
	  private bool isPrimarySupplierInvoice ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPrimarySupplierInvoice  
	   {
	    
	     get
		{
		   return isPrimarySupplierInvoice;
		 }
		 set
		 {
		   if(isPrimarySupplierInvoice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPrimarySupplierInvoice",OldValue=isPrimarySupplierInvoice,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPrimarySupplierInvoice=value;
		   }
			
		 }
	   }
	  private string actualPayedCurrencyTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActualPayedCurrencyTypeName  
	   {
	    
	     get
		{
		   return actualPayedCurrencyTypeName;
		 }
		 set
		 {
		   if(actualPayedCurrencyTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActualPayedCurrencyTypeName",OldValue=actualPayedCurrencyTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   actualPayedCurrencyTypeName=value;
		   }
			
		 }
	   }
	  private int invoiceItemLastLineNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int InvoiceItemLastLineNumber  
	   {
	    
	     get
		{
		   return invoiceItemLastLineNumber;
		 }
		 set
		 {
		   if(invoiceItemLastLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceItemLastLineNumber",OldValue=invoiceItemLastLineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   invoiceItemLastLineNumber=value;
		   }
			
		 }
	   }
	  private int fullItemsCount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int FullItemsCount  
	   {
	    
	     get
		{
		   return fullItemsCount;
		 }
		 set
		 {
		   if(fullItemsCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FullItemsCount",OldValue=fullItemsCount,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   fullItemsCount=value;
		   }
			
		 }
	   }
	  private string incotermName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IncotermName  
	   {
	    
	     get
		{
		   return incotermName;
		 }
		 set
		 {
		   if(incotermName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncotermName",OldValue=incotermName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   incotermName=value;
		   }
			
		 }
	   }
	  private int? maxSequence ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? MaxSequence  
	   {
	    
	     get
		{
		   return maxSequence;
		 }
		 set
		 {
		   if(maxSequence != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MaxSequence",OldValue=maxSequence,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   maxSequence=value;
		   }
			
		 }
	   }
	  private bool isAccumalated ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAccumalated  
	   {
	    
	     get
		{
		   return isAccumalated;
		 }
		 set
		 {
		   if(isAccumalated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAccumalated",OldValue=isAccumalated,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAccumalated=value;
		   }
			
		 }
	   }
	  private string accumalationStateCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccumalationStateCode  
	   {
	    
	     get
		{
		   return accumalationStateCode;
		 }
		 set
		 {
		   if(accumalationStateCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccumalationStateCode",OldValue=accumalationStateCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accumalationStateCode=value;
		   }
			
		 }
	   }
	  private int? fullParentsCount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? FullParentsCount  
	   {
	    
	     get
		{
		   return fullParentsCount;
		 }
		 set
		 {
		   if(fullParentsCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FullParentsCount",OldValue=fullParentsCount,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   fullParentsCount=value;
		   }
			
		 }
	   }
	  private int? fullChildrenCount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? FullChildrenCount  
	   {
	    
	     get
		{
		   return fullChildrenCount;
		 }
		 set
		 {
		   if(fullChildrenCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FullChildrenCount",OldValue=fullChildrenCount,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   fullChildrenCount=value;
		   }
			
		 }
	   }
	  private string unfInvoiceCounterKey ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string UnfInvoiceCounterKey  
	   {
	    
	     get
		{
		   return unfInvoiceCounterKey;
		 }
		 set
		 {
		   if(unfInvoiceCounterKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UnfInvoiceCounterKey",OldValue=unfInvoiceCounterKey,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   unfInvoiceCounterKey=value;
		   }
			
		 }
	   }
	  private decimal? vendorComissionPercentage ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? VendorComissionPercentage  
	   {
	    
	     get
		{
		   return vendorComissionPercentage;
		 }
		 set
		 {
		   if(vendorComissionPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorComissionPercentage",OldValue=vendorComissionPercentage,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   vendorComissionPercentage=value;
		   }
			
		 }
	   }
	  private bool isValueForCustomsOnly ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsValueForCustomsOnly  
	   {
	    
	     get
		{
		   return isValueForCustomsOnly;
		 }
		 set
		 {
		   if(isValueForCustomsOnly != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsValueForCustomsOnly",OldValue=isValueForCustomsOnly,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isValueForCustomsOnly=value;
		   }
			
		 }
	   }
	  private decimal? invoiceAmountInUSD ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? InvoiceAmountInUSD  
	   {
	    
	     get
		{
		   return invoiceAmountInUSD;
		 }
		 set
		 {
		   if(invoiceAmountInUSD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceAmountInUSD",OldValue=invoiceAmountInUSD,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   invoiceAmountInUSD=value;
		   }
			
		 }
	   }
	  private string changeInSupplierInvoice ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChangeInSupplierInvoice  
	   {
	    
	     get
		{
		   return changeInSupplierInvoice;
		 }
		 set
		 {
		   if(changeInSupplierInvoice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChangeInSupplierInvoice",OldValue=changeInSupplierInvoice,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   changeInSupplierInvoice=value;
		   }
			
		 }
	   }
	  private string invoiceCurrencyTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InvoiceCurrencyTypeName  
	   {
	    
	     get
		{
		   return invoiceCurrencyTypeName;
		 }
		 set
		 {
		   if(invoiceCurrencyTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceCurrencyTypeName",OldValue=invoiceCurrencyTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   invoiceCurrencyTypeName=value;
		   }
			
		 }
	   }
	  private string buyerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BuyerName  
	   {
	    
	     get
		{
		   return buyerName;
		 }
		 set
		 {
		   if(buyerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BuyerName",OldValue=buyerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   buyerName=value;
		   }
			
		 }
	   }
	  private string buyerAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BuyerAddress  
	   {
	    
	     get
		{
		   return buyerAddress;
		 }
		 set
		 {
		   if(buyerAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BuyerAddress",OldValue=buyerAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   buyerAddress=value;
		   }
			
		 }
	   }
	  private string buyerCountryCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BuyerCountryCode  
	   {
	    
	     get
		{
		   return buyerCountryCode;
		 }
		 set
		 {
		   if(buyerCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BuyerCountryCode",OldValue=buyerCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   buyerCountryCode=value;
		   }
			
		 }
	   }
	  private string buyerRoleCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BuyerRoleCode  
	   {
	    
	     get
		{
		   return buyerRoleCode;
		 }
		 set
		 {
		   if(buyerRoleCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BuyerRoleCode",OldValue=buyerRoleCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   buyerRoleCode=value;
		   }
			
		 }
	   }
	  private string partyRelationshipCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PartyRelationshipCode  
	   {
	    
	     get
		{
		   return partyRelationshipCode;
		 }
		 set
		 {
		   if(partyRelationshipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PartyRelationshipCode",OldValue=partyRelationshipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   partyRelationshipCode=value;
		   }
			
		 }
	   }
	  private string partyRelationshipName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PartyRelationshipName  
	   {
	    
	     get
		{
		   return partyRelationshipName;
		 }
		 set
		 {
		   if(partyRelationshipName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PartyRelationshipName",OldValue=partyRelationshipName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   partyRelationshipName=value;
		   }
			
		 }
	   }
	  private string buyerRoleName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BuyerRoleName  
	   {
	    
	     get
		{
		   return buyerRoleName;
		 }
		 set
		 {
		   if(buyerRoleName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BuyerRoleName",OldValue=buyerRoleName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   buyerRoleName=value;
		   }
			
		 }
	   }
	  private string buyerCountryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string BuyerCountryName  
	   {
	    
	     get
		{
		   return buyerCountryName;
		 }
		 set
		 {
		   if(buyerCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BuyerCountryName",OldValue=buyerCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   buyerCountryName=value;
		   }
			
		 }
	   }

	   private List<SupplierInvoicePaymentPM> supplierInvoicePayments;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoicePayment", "DeclarationId, InvoiceCounterKey","DeclarationId, InvoiceCounterKey")]
	   [DataMember]
	   public virtual List<SupplierInvoicePaymentPM> SupplierInvoicePayments  
	   {
	        get
             {
                 if (supplierInvoicePayments == null)
                 {
                     supplierInvoicePayments = new List<SupplierInvoicePaymentPM>();
                 }
                 return supplierInvoicePayments;
              }
             set { supplierInvoicePayments = value; }
	    }
		   
	   private List<SupplierInvoicePaymentPM>  deletedSupplierInvoicePayments;
	   public virtual List<SupplierInvoicePaymentPM> DeletedSupplierInvoicePayments  
	   {
	        get
             {
                 if ( deletedSupplierInvoicePayments == null)
                 {
                      deletedSupplierInvoicePayments = new List<SupplierInvoicePaymentPM>();
                 }
                 return  deletedSupplierInvoicePayments;
              }
             set {  deletedSupplierInvoicePayments = value; }
	    }
	  
	   private List<SupplierInvoiceUCRPM> supplierInvoiceUCRs;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceUCR", "DeclarationId, InvoiceCounterKey","DeclarationId, InvoiceCounterKey")]
	   [DataMember]
	   public virtual List<SupplierInvoiceUCRPM> SupplierInvoiceUCRs  
	   {
	        get
             {
                 if (supplierInvoiceUCRs == null)
                 {
                     supplierInvoiceUCRs = new List<SupplierInvoiceUCRPM>();
                 }
                 return supplierInvoiceUCRs;
              }
             set { supplierInvoiceUCRs = value; }
	    }
		   
	   private List<SupplierInvoiceUCRPM>  deletedSupplierInvoiceUCRs;
	   public virtual List<SupplierInvoiceUCRPM> DeletedSupplierInvoiceUCRs  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceUCRs == null)
                 {
                      deletedSupplierInvoiceUCRs = new List<SupplierInvoiceUCRPM>();
                 }
                 return  deletedSupplierInvoiceUCRs;
              }
             set {  deletedSupplierInvoiceUCRs = value; }
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
	  private string insruanceCurrencyTypeCodeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InsruanceCurrencyTypeCodeName  
	   {
	    
	     get
		{
		   return insruanceCurrencyTypeCodeName;
		 }
		 set
		 {
		   if(insruanceCurrencyTypeCodeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InsruanceCurrencyTypeCodeName",OldValue=insruanceCurrencyTypeCodeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   insruanceCurrencyTypeCodeName=value;
		   }
			
		 }
	   }
	  private string exportFreightAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportFreightAmount  
	   {
	    
	     get
		{
		   return exportFreightAmount;
		 }
		 set
		 {
		   if(exportFreightAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportFreightAmount",OldValue=exportFreightAmount,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportFreightAmount=value;
		   }
			
		 }
	   }
	  private string exportInsuranceAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportInsuranceAmount  
	   {
	    
	     get
		{
		   return exportInsuranceAmount;
		 }
		 set
		 {
		   if(exportInsuranceAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportInsuranceAmount",OldValue=exportInsuranceAmount,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportInsuranceAmount=value;
		   }
			
		 }
	   }
	  private string dutyRegimeProtocolCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DutyRegimeProtocolCode  
	   {
	    
	     get
		{
		   return dutyRegimeProtocolCode;
		 }
		 set
		 {
		   if(dutyRegimeProtocolCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DutyRegimeProtocolCode",OldValue=dutyRegimeProtocolCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dutyRegimeProtocolCode=value;
		   }
			
		 }
	   }
	  private string vendorNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VendorNumber  
	   {
	    
	     get
		{
		   return vendorNumber;
		 }
		 set
		 {
		   if(vendorNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VendorNumber",OldValue=vendorNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vendorNumber=value;
		   }
			
		 }
	   }
	    }
   
}
	 