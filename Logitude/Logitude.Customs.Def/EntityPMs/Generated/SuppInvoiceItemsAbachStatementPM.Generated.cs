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
   public partial class SuppInvoiceItemsAbachStatementPM : EntityPM
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
	  private int invoiceItemLineNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int InvoiceItemLineNumber  
	   {
	    
	     get
		{
		   return invoiceItemLineNumber;
		 }
		 set
		 {
		   if(invoiceItemLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceItemLineNumber",OldValue=invoiceItemLineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   invoiceItemLineNumber=value;
		   }
			
		 }
	   }
	  private int? sequenceNumeric ;
	  
       [Key]
	  
       
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
	  private string statementTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatementTypeCode  
	   {
	    
	     get
		{
		   return statementTypeCode;
		 }
		 set
		 {
		   if(statementTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatementTypeCode",OldValue=statementTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statementTypeCode=value;
		   }
			
		 }
	   }
	  private bool isStatementInd ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsStatementInd  
	   {
	    
	     get
		{
		   return isStatementInd;
		 }
		 set
		 {
		   if(isStatementInd != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsStatementInd",OldValue=isStatementInd,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isStatementInd=value;
		   }
			
		 }
	   }
	  private string statementTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatementTypeName  
	   {
	    
	     get
		{
		   return statementTypeName;
		 }
		 set
		 {
		   if(statementTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatementTypeName",OldValue=statementTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statementTypeName=value;
		   }
			
		 }
	   }
   }
   
}
	 