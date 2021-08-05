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
using Amital.QuoteOPM.Def.Validators;
  
namespace Amital.QuoteOPM.Def.EntityPMs
{
   [CustomValidation(typeof(QuoteOPMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class QuoteOPTotalVATPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string quoteOPId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteOPId  
	   {
	    
	     get
		{
		   return quoteOPId;
		 }
		 set
		 {
		   if(quoteOPId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteOPId",OldValue=quoteOPId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteOPId=value;
		   }
			
		 }
	   }
	  private double? quoteCurrencyVATAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? QuoteCurrencyVATAmount  
	   {
	    
	     get
		{
		   return quoteCurrencyVATAmount;
		 }
		 set
		 {
		   if(quoteCurrencyVATAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteCurrencyVATAmount",OldValue=quoteCurrencyVATAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   quoteCurrencyVATAmount=value;
		   }
			
		 }
	   }
	  private double? quoteCurrencyVatableAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? QuoteCurrencyVatableAmount  
	   {
	    
	     get
		{
		   return quoteCurrencyVatableAmount;
		 }
		 set
		 {
		   if(quoteCurrencyVatableAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteCurrencyVatableAmount",OldValue=quoteCurrencyVatableAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   quoteCurrencyVatableAmount=value;
		   }
			
		 }
	   }
	  private double? localCurrencyVATAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? LocalCurrencyVATAmount  
	   {
	    
	     get
		{
		   return localCurrencyVATAmount;
		 }
		 set
		 {
		   if(localCurrencyVATAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalCurrencyVATAmount",OldValue=localCurrencyVATAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   localCurrencyVATAmount=value;
		   }
			
		 }
	   }
	  private double? localCurrencyVatableAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? LocalCurrencyVatableAmount  
	   {
	    
	     get
		{
		   return localCurrencyVatableAmount;
		 }
		 set
		 {
		   if(localCurrencyVatableAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalCurrencyVatableAmount",OldValue=localCurrencyVatableAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   localCurrencyVatableAmount=value;
		   }
			
		 }
	   }
	  private double? profitCurrencyVATAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ProfitCurrencyVATAmount  
	   {
	    
	     get
		{
		   return profitCurrencyVATAmount;
		 }
		 set
		 {
		   if(profitCurrencyVATAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProfitCurrencyVATAmount",OldValue=profitCurrencyVATAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   profitCurrencyVATAmount=value;
		   }
			
		 }
	   }
	  private double? profitCurrencyVatableAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ProfitCurrencyVatableAmount  
	   {
	    
	     get
		{
		   return profitCurrencyVatableAmount;
		 }
		 set
		 {
		   if(profitCurrencyVatableAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProfitCurrencyVatableAmount",OldValue=profitCurrencyVatableAmount,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   profitCurrencyVatableAmount=value;
		   }
			
		 }
	   }
	  private double? vatPercent ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? VatPercent  
	   {
	    
	     get
		{
		   return vatPercent;
		 }
		 set
		 {
		   if(vatPercent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatPercent",OldValue=vatPercent,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   vatPercent=value;
		   }
			
		 }
	   }
	  private string externalVATCard ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalVATCard  
	   {
	    
	     get
		{
		   return externalVATCard;
		 }
		 set
		 {
		   if(externalVATCard != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalVATCard",OldValue=externalVATCard,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalVATCard=value;
		   }
			
		 }
	   }
	  private string externalTAXItemId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalTAXItemId  
	   {
	    
	     get
		{
		   return externalTAXItemId;
		 }
		 set
		 {
		   if(externalTAXItemId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalTAXItemId",OldValue=externalTAXItemId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalTAXItemId=value;
		   }
			
		 }
	   }
	  private string vatOPTypeId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VatOPTypeId  
	   {
	    
	     get
		{
		   return vatOPTypeId;
		 }
		 set
		 {
		   if(vatOPTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatOPTypeId",OldValue=vatOPTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vatOPTypeId=value;
		   }
			
		 }
	   }
	  private string vatTypeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VatTypeName  
	   {
	    
	     get
		{
		   return vatTypeName;
		 }
		 set
		 {
		   if(vatTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatTypeName",OldValue=vatTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vatTypeName=value;
		   }
			
		 }
	   }
	  private string vatTypeCell ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VatTypeCell  
	   {
	    
	     get
		{
		   return vatTypeCell;
		 }
		 set
		 {
		   if(vatTypeCell != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatTypeCell",OldValue=vatTypeCell,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vatTypeCell=value;
		   }
			
		 }
	   }
   }
   
}
	 