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
   public partial class Aur_ItemPM : EntityPM
   {
   	  private string paymentId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentId  
	   {
	    
	     get
		{
		   return paymentId;
		 }
		 set
		 {
		   if(paymentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentId",OldValue=paymentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentId=value;
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
	  private string salesOrderid ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SalesOrderid  
	   {
	    
	     get
		{
		   return salesOrderid;
		 }
		 set
		 {
		   if(salesOrderid != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SalesOrderid",OldValue=salesOrderid,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   salesOrderid=value;
		   }
			
		 }
	   }
	  private string relatedContract ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string RelatedContract  
	   {
	    
	     get
		{
		   return relatedContract;
		 }
		 set
		 {
		   if(relatedContract != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RelatedContract",OldValue=relatedContract,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   relatedContract=value;
		   }
			
		 }
	   }
	  private string productNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProductNumber  
	   {
	    
	     get
		{
		   return productNumber;
		 }
		 set
		 {
		   if(productNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProductNumber",OldValue=productNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   productNumber=value;
		   }
			
		 }
	   }
	  private string productName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProductName  
	   {
	    
	     get
		{
		   return productName;
		 }
		 set
		 {
		   if(productName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProductName",OldValue=productName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   productName=value;
		   }
			
		 }
	   }
	  private decimal? pricePerUnit ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? PricePerUnit  
	   {
	    
	     get
		{
		   return pricePerUnit;
		 }
		 set
		 {
		   if(pricePerUnit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PricePerUnit",OldValue=pricePerUnit,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   pricePerUnit=value;
		   }
			
		 }
	   }
	  private decimal quantity ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal Quantity  
	   {
	    
	     get
		{
		   return quantity;
		 }
		 set
		 {
		   if(quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity",OldValue=quantity,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   quantity=value;
		   }
			
		 }
	   }
	  private decimal? discount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Discount  
	   {
	    
	     get
		{
		   return discount;
		 }
		 set
		 {
		   if(discount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Discount",OldValue=discount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   discount=value;
		   }
			
		 }
	   }
	  private decimal baseAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal BaseAmount  
	   {
	    
	     get
		{
		   return baseAmount;
		 }
		 set
		 {
		   if(baseAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BaseAmount",OldValue=baseAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   baseAmount=value;
		   }
			
		 }
	   }
	  private decimal tax ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal Tax  
	   {
	    
	     get
		{
		   return tax;
		 }
		 set
		 {
		   if(tax != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Tax",OldValue=tax,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   tax=value;
		   }
			
		 }
	   }
	  private decimal extendedAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ExtendedAmount  
	   {
	    
	     get
		{
		   return extendedAmount;
		 }
		 set
		 {
		   if(extendedAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExtendedAmount",OldValue=extendedAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   extendedAmount=value;
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
	    }
   
}
	 