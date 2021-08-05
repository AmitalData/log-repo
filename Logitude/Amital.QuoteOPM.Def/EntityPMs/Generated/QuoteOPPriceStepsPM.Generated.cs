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
   public partial class QuoteOPPriceStepsPM : EntityPM
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
	  private string quoteOPChargeId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteOPChargeId  
	   {
	    
	     get
		{
		   return quoteOPChargeId;
		 }
		 set
		 {
		   if(quoteOPChargeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteOPChargeId",OldValue=quoteOPChargeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteOPChargeId=value;
		   }
			
		 }
	   }
	  private double? step ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Step  
	   {
	    
	     get
		{
		   return step;
		 }
		 set
		 {
		   if(step != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step",OldValue=step,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   step=value;
		   }
			
		 }
	   }
	  private double? costUnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? CostUnitPrice  
	   {
	    
	     get
		{
		   return costUnitPrice;
		 }
		 set
		 {
		   if(costUnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostUnitPrice",OldValue=costUnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   costUnitPrice=value;
		   }
			
		 }
	   }
	  private double? saleUnitPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleUnitPrice  
	   {
	    
	     get
		{
		   return saleUnitPrice;
		 }
		 set
		 {
		   if(saleUnitPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleUnitPrice",OldValue=saleUnitPrice,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleUnitPrice=value;
		   }
			
		 }
	   }
	  private double? markupValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? MarkupValue  
	   {
	    
	     get
		{
		   return markupValue;
		 }
		 set
		 {
		   if(markupValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarkupValue",OldValue=markupValue,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   markupValue=value;
		   }
			
		 }
	   }
	  private string measurementUnit ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string MeasurementUnit  
	   {
	    
	     get
		{
		   return measurementUnit;
		 }
		 set
		 {
		   if(measurementUnit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeasurementUnit",OldValue=measurementUnit,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   measurementUnit=value;
		   }
			
		 }
	   }
   }
   
}
	 