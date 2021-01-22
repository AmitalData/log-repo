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
   public partial class PaymentChequePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string internalNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InternalNumber  
	   {
	    
	     get
		{
		   return internalNumber;
		 }
		 set
		 {
		   if(internalNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InternalNumber",OldValue=internalNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   internalNumber=value;
		   }
			
		 }
	   }
	  private string chequeNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChequeNumber  
	   {
	    
	     get
		{
		   return chequeNumber;
		 }
		 set
		 {
		   if(chequeNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChequeNumber",OldValue=chequeNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chequeNumber=value;
		   }
			
		 }
	   }
	  private string payToGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PayToGLAccountId  
	   {
	    
	     get
		{
		   return payToGLAccountId;
		 }
		 set
		 {
		   if(payToGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PayToGLAccountId",OldValue=payToGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   payToGLAccountId=value;
		   }
			
		 }
	   }
	  private string payToName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PayToName  
	   {
	    
	     get
		{
		   return payToName;
		 }
		 set
		 {
		   if(payToName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PayToName",OldValue=payToName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   payToName=value;
		   }
			
		 }
	   }
	  private string bankAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankAccountId  
	   {
	    
	     get
		{
		   return bankAccountId;
		 }
		 set
		 {
		   if(bankAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankAccountId",OldValue=bankAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankAccountId=value;
		   }
			
		 }
	   }
	  private string bankAccountGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankAccountGLAccountId  
	   {
	    
	     get
		{
		   return bankAccountGLAccountId;
		 }
		 set
		 {
		   if(bankAccountGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankAccountGLAccountId",OldValue=bankAccountGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankAccountGLAccountId=value;
		   }
			
		 }
	   }
	  private decimal? localAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? LocalAmount  
	   {
	    
	     get
		{
		   return localAmount;
		 }
		 set
		 {
		   if(localAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalAmount",OldValue=localAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   localAmount=value;
		   }
			
		 }
	   }
	  private string currencyId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private decimal? foreignAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ForeignAmount  
	   {
	    
	     get
		{
		   return foreignAmount;
		 }
		 set
		 {
		   if(foreignAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ForeignAmount",OldValue=foreignAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   foreignAmount=value;
		   }
			
		 }
	   }
	  private decimal? exchangeRate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private DateTime? valueDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ValueDate  
	   {
	    
	     get
		{
		   return valueDate;
		 }
		 set
		 {
		   if(valueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ValueDate",OldValue=valueDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   valueDate=value;
		   }
			
		 }
	   }
	  private DateTime? printDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? PrintDate  
	   {
	    
	     get
		{
		   return printDate;
		 }
		 set
		 {
		   if(printDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PrintDate",OldValue=printDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   printDate=value;
		   }
			
		 }
	   }
	  private DateTime? approveDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ApproveDate  
	   {
	    
	     get
		{
		   return approveDate;
		 }
		 set
		 {
		   if(approveDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ApproveDate",OldValue=approveDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   approveDate=value;
		   }
			
		 }
	   }
	  private string approvedByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ApprovedByUserId  
	   {
	    
	     get
		{
		   return approvedByUserId;
		 }
		 set
		 {
		   if(approvedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ApprovedByUserId",OldValue=approvedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   approvedByUserId=value;
		   }
			
		 }
	   }
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCancelled  
	   {
	    
	     get
		{
		   return isCancelled;
		 }
		 set
		 {
		   if(isCancelled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancelled",OldValue=isCancelled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCancelled=value;
		   }
			
		 }
	   }
	  private string cancelledByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CancelledByUserId  
	   {
	    
	     get
		{
		   return cancelledByUserId;
		 }
		 set
		 {
		   if(cancelledByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CancelledByUserId",OldValue=cancelledByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cancelledByUserId=value;
		   }
			
		 }
	   }
	  private DateTime? cancelledDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CancelledDate  
	   {
	    
	     get
		{
		   return cancelledDate;
		 }
		 set
		 {
		   if(cancelledDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CancelledDate",OldValue=cancelledDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   cancelledDate=value;
		   }
			
		 }
	   }
	  private string cancellationRemarks ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CancellationRemarks  
	   {
	    
	     get
		{
		   return cancellationRemarks;
		 }
		 set
		 {
		   if(cancellationRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CancellationRemarks",OldValue=cancellationRemarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cancellationRemarks=value;
		   }
			
		 }
	   }
	  private string paymentChequeStatusCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentChequeStatusCode  
	   {
	    
	     get
		{
		   return paymentChequeStatusCode;
		 }
		 set
		 {
		   if(paymentChequeStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentChequeStatusCode",OldValue=paymentChequeStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentChequeStatusCode=value;
		   }
			
		 }
	   }
	  private string entityId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityId  
	   {
	    
	     get
		{
		   return entityId;
		 }
		 set
		 {
		   if(entityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityId",OldValue=entityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityId=value;
		   }
			
		 }
	   }
	  private string objectTableId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ObjectTableId  
	   {
	    
	     get
		{
		   return objectTableId;
		 }
		 set
		 {
		   if(objectTableId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ObjectTableId",OldValue=objectTableId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   objectTableId=value;
		   }
			
		 }
	   }
	  private string gLAccountNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string GLAccountNumber  
	   {
	    
	     get
		{
		   return gLAccountNumber;
		 }
		 set
		 {
		   if(gLAccountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GLAccountNumber",OldValue=gLAccountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gLAccountNumber=value;
		   }
			
		 }
	   }
	  private string bankAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankAccountName  
	   {
	    
	     get
		{
		   return bankAccountName;
		 }
		 set
		 {
		   if(bankAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankAccountName",OldValue=bankAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankAccountName=value;
		   }
			
		 }
	   }
	  private string paymentChequeStatusName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentChequeStatusName  
	   {
	    
	     get
		{
		   return paymentChequeStatusName;
		 }
		 set
		 {
		   if(paymentChequeStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentChequeStatusName",OldValue=paymentChequeStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentChequeStatusName=value;
		   }
			
		 }
	   }
	  private string bankAccountCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankAccountCode  
	   {
	    
	     get
		{
		   return bankAccountCode;
		 }
		 set
		 {
		   if(bankAccountCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankAccountCode",OldValue=bankAccountCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankAccountCode=value;
		   }
			
		 }
	   }
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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

	   private List<PaymentChequeLinePM> paymentChequeLines;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("PaymentChequePaymentChequeLines", "Id","PaymentChequeId")]
	   [DataMember]
	   public virtual List<PaymentChequeLinePM> PaymentChequeLines  
	   {
	        get
             {
                 if (paymentChequeLines == null)
                 {
                     paymentChequeLines = new List<PaymentChequeLinePM>();
                 }
                 return paymentChequeLines;
              }
             set { paymentChequeLines = value; }
	    }
		   
	   private List<PaymentChequeLinePM>  deletedPaymentChequeLines;
	   public virtual List<PaymentChequeLinePM> DeletedPaymentChequeLines  
	   {
	        get
             {
                 if ( deletedPaymentChequeLines == null)
                 {
                      deletedPaymentChequeLines = new List<PaymentChequeLinePM>();
                 }
                 return  deletedPaymentChequeLines;
              }
             set {  deletedPaymentChequeLines = value; }
	    }
	  	  private int paymentChequeLineLastLine ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int PaymentChequeLineLastLine  
	   {
	    
	     get
		{
		   return paymentChequeLineLastLine;
		 }
		 set
		 {
		   if(paymentChequeLineLastLine != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentChequeLineLastLine",OldValue=paymentChequeLineLastLine,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   paymentChequeLineLastLine=value;
		   }
			
		 }
	   }
	  private string gLAccountCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string GLAccountCurrencyId  
	   {
	    
	     get
		{
		   return gLAccountCurrencyId;
		 }
		 set
		 {
		   if(gLAccountCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GLAccountCurrencyId",OldValue=gLAccountCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gLAccountCurrencyId=value;
		   }
			
		 }
	   }
	  private string bankGLAccountCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankGLAccountCurrencyId  
	   {
	    
	     get
		{
		   return bankGLAccountCurrencyId;
		 }
		 set
		 {
		   if(bankGLAccountCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankGLAccountCurrencyId",OldValue=bankGLAccountCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankGLAccountCurrencyId=value;
		   }
			
		 }
	   }
	  private bool isGLAccountMultiCurrency ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsGLAccountMultiCurrency  
	   {
	    
	     get
		{
		   return isGLAccountMultiCurrency;
		 }
		 set
		 {
		   if(isGLAccountMultiCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsGLAccountMultiCurrency",OldValue=isGLAccountMultiCurrency,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isGLAccountMultiCurrency=value;
		   }
			
		 }
	   }
	  private bool isBankGlAccountMultiCur ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsBankGlAccountMultiCur  
	   {
	    
	     get
		{
		   return isBankGlAccountMultiCur;
		 }
		 set
		 {
		   if(isBankGlAccountMultiCur != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsBankGlAccountMultiCur",OldValue=isBankGlAccountMultiCur,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isBankGlAccountMultiCur=value;
		   }
			
		 }
	   }
	  private string uniqueField ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UniqueField  
	   {
	    
	     get
		{
		   return uniqueField;
		 }
		 set
		 {
		   if(uniqueField != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UniqueField",OldValue=uniqueField,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   uniqueField=value;
		   }
			
		 }
	   }
	  private string gLAccountName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string GLAccountName  
	   {
	    
	     get
		{
		   return gLAccountName;
		 }
		 set
		 {
		   if(gLAccountName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GLAccountName",OldValue=gLAccountName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   gLAccountName=value;
		   }
			
		 }
	   }
	  private string currencyCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string bankLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankLocalName  
	   {
	    
	     get
		{
		   return bankLocalName;
		 }
		 set
		 {
		   if(bankLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankLocalName",OldValue=bankLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankLocalName=value;
		   }
			
		 }
	   }
	  private string bankEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankEnglishName  
	   {
	    
	     get
		{
		   return bankEnglishName;
		 }
		 set
		 {
		   if(bankEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankEnglishName",OldValue=bankEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankEnglishName=value;
		   }
			
		 }
	   }
	  private string aPPaymentId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string APPaymentId  
	   {
	    
	     get
		{
		   return aPPaymentId;
		 }
		 set
		 {
		   if(aPPaymentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="APPaymentId",OldValue=aPPaymentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aPPaymentId=value;
		   }
			
		 }
	   }
	  private bool cancelledByAPPayment ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CancelledByAPPayment  
	   {
	    
	     get
		{
		   return cancelledByAPPayment;
		 }
		 set
		 {
		   if(cancelledByAPPayment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CancelledByAPPayment",OldValue=cancelledByAPPayment,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   cancelledByAPPayment=value;
		   }
			
		 }
	   }
	  private string aPPaymentNo ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string APPaymentNo  
	   {
	    
	     get
		{
		   return aPPaymentNo;
		 }
		 set
		 {
		   if(aPPaymentNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="APPaymentNo",OldValue=aPPaymentNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aPPaymentNo=value;
		   }
			
		 }
	   }
   }
   
}
	 