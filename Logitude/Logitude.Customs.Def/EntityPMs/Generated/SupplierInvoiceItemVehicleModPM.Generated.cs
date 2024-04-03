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
   public partial class SupplierInvoiceItemVehicleModPM : EntityPM
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
	  private int vehicleLineNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int VehicleLineNumber  
	   {
	    
	     get
		{
		   return vehicleLineNumber;
		 }
		 set
		 {
		   if(vehicleLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleLineNumber",OldValue=vehicleLineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   vehicleLineNumber=value;
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
	  private string adjustmentTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AdjustmentTypeCode  
	   {
	    
	     get
		{
		   return adjustmentTypeCode;
		 }
		 set
		 {
		   if(adjustmentTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AdjustmentTypeCode",OldValue=adjustmentTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   adjustmentTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? deductAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DeductAmount  
	   {
	    
	     get
		{
		   return deductAmount;
		 }
		 set
		 {
		   if(deductAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeductAmount",OldValue=deductAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   deductAmount=value;
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
	    }
   
}
	 