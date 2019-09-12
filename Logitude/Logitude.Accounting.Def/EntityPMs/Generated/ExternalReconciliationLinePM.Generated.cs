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
   public partial class ExternalReconciliationLinePM : EntityPM
   {
   	  private string reconciliationId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReconciliationId  
	   {
	    
	     get
		{
		   return reconciliationId;
		 }
		 set
		 {
		   if(reconciliationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconciliationId",OldValue=reconciliationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reconciliationId=value;
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
	  private int groupNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int GroupNumber  
	   {
	    
	     get
		{
		   return groupNumber;
		 }
		 set
		 {
		   if(groupNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupNumber",OldValue=groupNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   groupNumber=value;
		   }
			
		 }
	   }
	  private string externalPageLineId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalPageLineId  
	   {
	    
	     get
		{
		   return externalPageLineId;
		 }
		 set
		 {
		   if(externalPageLineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalPageLineId",OldValue=externalPageLineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalPageLineId=value;
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
	  private string ledgerGLAccountId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LedgerGLAccountId  
	   {
	    
	     get
		{
		   return ledgerGLAccountId;
		 }
		 set
		 {
		   if(ledgerGLAccountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LedgerGLAccountId",OldValue=ledgerGLAccountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ledgerGLAccountId=value;
		   }
			
		 }
	   }
   }
   
}
	 