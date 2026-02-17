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
   public partial class ReconcileExternalPageLinePM : EntityPM
   {
   	  private string reconcileExternalPageId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReconcileExternalPageId  
	   {
	    
	     get
		{
		   return reconcileExternalPageId;
		 }
		 set
		 {
		   if(reconcileExternalPageId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconcileExternalPageId",OldValue=reconcileExternalPageId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reconcileExternalPageId=value;
		   }
			
		 }
	   }
	  private int lineNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private decimal amount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal Amount  
	   {
	    
	     get
		{
		   return amount;
		 }
		 set
		 {
		   if(amount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Amount",OldValue=amount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   amount=value;
		   }
			
		 }
	   }
	  private DateTime referenceDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ReferenceDate  
	   {
	    
	     get
		{
		   return referenceDate;
		 }
		 set
		 {
		   if(referenceDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReferenceDate",OldValue=referenceDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   referenceDate=value;
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
	  private bool isReconciled ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsReconciled  
	   {
	    
	     get
		{
		   return isReconciled;
		 }
		 set
		 {
		   if(isReconciled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsReconciled",OldValue=isReconciled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isReconciled=value;
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
	  private string reconcileRemarks ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReconcileRemarks  
	   {
	    
	     get
		{
		   return reconcileRemarks;
		 }
		 set
		 {
		   if(reconcileRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconcileRemarks",OldValue=reconcileRemarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reconcileRemarks=value;
		   }
			
		 }
	   }
	  private int? groupHash ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? GroupHash  
	   {
	    
	     get
		{
		   return groupHash;
		 }
		 set
		 {
		   if(groupHash != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupHash",OldValue=groupHash,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   groupHash=value;
		   }
			
		 }
	   }
	  private string reconciliationNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ReconciliationNumber  
	   {
	    
	     get
		{
		   return reconciliationNumber;
		 }
		 set
		 {
		   if(reconciliationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReconciliationNumber",OldValue=reconciliationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reconciliationNumber=value;
		   }
			
		 }
	   }
   }
   
}
	 