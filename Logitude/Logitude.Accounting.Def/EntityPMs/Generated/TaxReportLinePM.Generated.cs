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
using Logitude.Accounting.Def.Validators;
  
namespace Logitude.Accounting.Def.EntityPMs
{
   [CustomValidation(typeof(AccountingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TaxReportLinePM : EntityPM
   {
   	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private DateTime lastUpdateDateTime ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime LastUpdateDateTime  
	   {
	    
	     get
		{
		   return lastUpdateDateTime;
		 }
		 set
		 {
		   if(lastUpdateDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastUpdateDateTime",OldValue=lastUpdateDateTime,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   lastUpdateDateTime=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string taxReportId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaxReportId  
	   {
	    
	     get
		{
		   return taxReportId;
		 }
		 set
		 {
		   if(taxReportId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxReportId",OldValue=taxReportId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taxReportId=value;
		   }
			
		 }
	   }
	  private int line ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Line  
	   {
	    
	     get
		{
		   return line;
		 }
		 set
		 {
		   if(line != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Line",OldValue=line,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   line=value;
		   }
			
		 }
	   }
	  private string outputOrInput ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OutputOrInput  
	   {
	    
	     get
		{
		   return outputOrInput;
		 }
		 set
		 {
		   if(outputOrInput != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OutputOrInput",OldValue=outputOrInput,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   outputOrInput=value;
		   }
			
		 }
	   }
	  private string lineTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LineTypeCode  
	   {
	    
	     get
		{
		   return lineTypeCode;
		 }
		 set
		 {
		   if(lineTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineTypeCode",OldValue=lineTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lineTypeCode=value;
		   }
			
		 }
	   }
	  private string vatNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VatNumber  
	   {
	    
	     get
		{
		   return vatNumber;
		 }
		 set
		 {
		   if(vatNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatNumber",OldValue=vatNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vatNumber=value;
		   }
			
		 }
	   }
	  private string reference ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Reference  
	   {
	    
	     get
		{
		   return reference;
		 }
		 set
		 {
		   if(reference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Reference",OldValue=reference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reference=value;
		   }
			
		 }
	   }
	  private string referecneGroup ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReferecneGroup  
	   {
	    
	     get
		{
		   return referecneGroup;
		 }
		 set
		 {
		   if(referecneGroup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReferecneGroup",OldValue=referecneGroup,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   referecneGroup=value;
		   }
			
		 }
	   }
	  private DateTime? referenceDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ReferenceDate  
	   {
	    
	     get
		{
		   return referenceDate;
		 }
		 set
		 {
		   if(referenceDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReferenceDate",OldValue=referenceDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   referenceDate=value;
		   }
			
		 }
	   }
	  private decimal? vatAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? VatAmount  
	   {
	    
	     get
		{
		   return vatAmount;
		 }
		 set
		 {
		   if(vatAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatAmount",OldValue=vatAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   vatAmount=value;
		   }
			
		 }
	   }
	  private decimal? vatableInvoiceAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? VatableInvoiceAmount  
	   {
	    
	     get
		{
		   return vatableInvoiceAmount;
		 }
		 set
		 {
		   if(vatableInvoiceAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatableInvoiceAmount",OldValue=vatableInvoiceAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   vatableInvoiceAmount=value;
		   }
			
		 }
	   }
	  private string statusCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string transmitStatusCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransmitStatusCode  
	   {
	    
	     get
		{
		   return transmitStatusCode;
		 }
		 set
		 {
		   if(transmitStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransmitStatusCode",OldValue=transmitStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transmitStatusCode=value;
		   }
			
		 }
	   }
	  private string journalId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string JournalId  
	   {
	    
	     get
		{
		   return journalId;
		 }
		 set
		 {
		   if(journalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JournalId",OldValue=journalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   journalId=value;
		   }
			
		 }
	   }
	  private bool? isManuallyChanged ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsManuallyChanged  
	   {
	    
	     get
		{
		   return isManuallyChanged;
		 }
		 set
		 {
		   if(isManuallyChanged != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsManuallyChanged",OldValue=isManuallyChanged,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isManuallyChanged=value;
		   }
			
		 }
	   }
	  private bool isEquipment ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsEquipment  
	   {
	    
	     get
		{
		   return isEquipment;
		 }
		 set
		 {
		   if(isEquipment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsEquipment",OldValue=isEquipment,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isEquipment=value;
		   }
			
		 }
	   }
	  private string statusLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusLocalName  
	   {
	    
	     get
		{
		   return statusLocalName;
		 }
		 set
		 {
		   if(statusLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusLocalName",OldValue=statusLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusLocalName=value;
		   }
			
		 }
	   }
	  private string statusEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusEnglishName  
	   {
	    
	     get
		{
		   return statusEnglishName;
		 }
		 set
		 {
		   if(statusEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusEnglishName",OldValue=statusEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusEnglishName=value;
		   }
			
		 }
	   }
	  private string journalNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string JournalNumber  
	   {
	    
	     get
		{
		   return journalNumber;
		 }
		 set
		 {
		   if(journalNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JournalNumber",OldValue=journalNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   journalNumber=value;
		   }
			
		 }
	   }
	  private DateTime? taxReportDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? TaxReportDate  
	   {
	    
	     get
		{
		   return taxReportDate;
		 }
		 set
		 {
		   if(taxReportDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxReportDate",OldValue=taxReportDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   taxReportDate=value;
		   }
			
		 }
	   }
	  private bool isExternalLine ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsExternalLine  
	   {
	    
	     get
		{
		   return isExternalLine;
		 }
		 set
		 {
		   if(isExternalLine != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsExternalLine",OldValue=isExternalLine,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isExternalLine=value;
		   }
			
		 }
	   }
	  private decimal? totalInvoiceAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotalInvoiceAmount  
	   {
	    
	     get
		{
		   return totalInvoiceAmount;
		 }
		 set
		 {
		   if(totalInvoiceAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalInvoiceAmount",OldValue=totalInvoiceAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totalInvoiceAmount=value;
		   }
			
		 }
	   }
	  private string originalReference ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginalReference  
	   {
	    
	     get
		{
		   return originalReference;
		 }
		 set
		 {
		   if(originalReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalReference",OldValue=originalReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originalReference=value;
		   }
			
		 }
	   }
	  private string updatedBUserName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedBUserName  
	   {
	    
	     get
		{
		   return updatedBUserName;
		 }
		 set
		 {
		   if(updatedBUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedBUserName",OldValue=updatedBUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedBUserName=value;
		   }
			
		 }
	   }
	  private int journalLineNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int JournalLineNumber  
	   {
	    
	     get
		{
		   return journalLineNumber;
		 }
		 set
		 {
		   if(journalLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JournalLineNumber",OldValue=journalLineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   journalLineNumber=value;
		   }
			
		 }
	   }
	  private string previousReference ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PreviousReference  
	   {
	    
	     get
		{
		   return previousReference;
		 }
		 set
		 {
		   if(previousReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PreviousReference",OldValue=previousReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   previousReference=value;
		   }
			
		 }
	   }
	  private decimal? vatAmountRound ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? VatAmountRound  
	   {
	    
	     get
		{
		   return vatAmountRound;
		 }
		 set
		 {
		   if(vatAmountRound != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatAmountRound",OldValue=vatAmountRound,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   vatAmountRound=value;
		   }
			
		 }
	   }
	  private string ledgerTransactionId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LedgerTransactionId  
	   {
	    
	     get
		{
		   return ledgerTransactionId;
		 }
		 set
		 {
		   if(ledgerTransactionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LedgerTransactionId",OldValue=ledgerTransactionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ledgerTransactionId=value;
		   }
			
		 }
	   }
	  private double? subTotalInLocalCurrency ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SubTotalInLocalCurrency  
	   {
	    
	     get
		{
		   return subTotalInLocalCurrency;
		 }
		 set
		 {
		   if(subTotalInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SubTotalInLocalCurrency",OldValue=subTotalInLocalCurrency,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   subTotalInLocalCurrency=value;
		   }
			
		 }
	   }
	  private string confirmationNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConfirmationNumber  
	   {
	    
	     get
		{
		   return confirmationNumber;
		 }
		 set
		 {
		   if(confirmationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConfirmationNumber",OldValue=confirmationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   confirmationNumber=value;
		   }
			
		 }
	   }
	    }
   
}
	 