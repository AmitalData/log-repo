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
   public partial class JournalExternalReconcilePM : EntityPM
   {
   	  private string journalId ;
	  
       [Key]
	  
       
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
	  private string reconcileExternalPageLineId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReconcileExternalPageLineId  
	   {
	    
	     get
		{
		   return reconcileExternalPageLineId;
		 }
		 set
		 {
		   if(reconcileExternalPageLineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconcileExternalPageLineId",OldValue=reconcileExternalPageLineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reconcileExternalPageLineId=value;
		   }
			
		 }
	   }
	  private bool skipAccountsValidation ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool SkipAccountsValidation  
	   {
	    
	     get
		{
		   return skipAccountsValidation;
		 }
		 set
		 {
		   if(skipAccountsValidation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SkipAccountsValidation",OldValue=skipAccountsValidation,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   skipAccountsValidation=value;
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
	  private string originalJournalId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginalJournalId  
	   {
	    
	     get
		{
		   return originalJournalId;
		 }
		 set
		 {
		   if(originalJournalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalJournalId",OldValue=originalJournalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originalJournalId=value;
		   }
			
		 }
	   }
	    }
   
}
	 