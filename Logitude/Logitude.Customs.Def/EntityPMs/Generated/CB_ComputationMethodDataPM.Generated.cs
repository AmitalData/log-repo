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
   public partial class CB_ComputationMethodDataPM : EntityPM
   {
   	  private int iD ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private decimal? alternateDefinedPerUnitMeasure ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? AlternateDefinedPerUnitMeasure  
	   {
	    
	     get
		{
		   return alternateDefinedPerUnitMeasure;
		 }
		 set
		 {
		   if(alternateDefinedPerUnitMeasure != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AlternateDefinedPerUnitMeasure",OldValue=alternateDefinedPerUnitMeasure,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   alternateDefinedPerUnitMeasure=value;
		   }
			
		 }
	   }
	  private decimal? alternateRate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? AlternateRate  
	   {
	    
	     get
		{
		   return alternateRate;
		 }
		 set
		 {
		   if(alternateRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AlternateRate",OldValue=alternateRate,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   alternateRate=value;
		   }
			
		 }
	   }
	  private string calculationReference ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CalculationReference  
	   {
	    
	     get
		{
		   return calculationReference;
		 }
		 set
		 {
		   if(calculationReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculationReference",OldValue=calculationReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   calculationReference=value;
		   }
			
		 }
	   }
	  private string computationMethodID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ComputationMethodID  
	   {
	    
	     get
		{
		   return computationMethodID;
		 }
		 set
		 {
		   if(computationMethodID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ComputationMethodID",OldValue=computationMethodID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   computationMethodID=value;
		   }
			
		 }
	   }
	  private string currencyTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyTypeID  
	   {
	    
	     get
		{
		   return currencyTypeID;
		 }
		 set
		 {
		   if(currencyTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyTypeID",OldValue=currencyTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyTypeID=value;
		   }
			
		 }
	   }
	  private decimal? definedPerUnitMethod ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DefinedPerUnitMethod  
	   {
	    
	     get
		{
		   return definedPerUnitMethod;
		 }
		 set
		 {
		   if(definedPerUnitMethod != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DefinedPerUnitMethod",OldValue=definedPerUnitMethod,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   definedPerUnitMethod=value;
		   }
			
		 }
	   }
	  private string englishCalculationReference ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EnglishCalculationReference  
	   {
	    
	     get
		{
		   return englishCalculationReference;
		 }
		 set
		 {
		   if(englishCalculationReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EnglishCalculationReference",OldValue=englishCalculationReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   englishCalculationReference=value;
		   }
			
		 }
	   }
	  private string measurementUnitID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MeasurementUnitID  
	   {
	    
	     get
		{
		   return measurementUnitID;
		 }
		 set
		 {
		   if(measurementUnitID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MeasurementUnitID",OldValue=measurementUnitID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   measurementUnitID=value;
		   }
			
		 }
	   }
	  private string alternate_MeasurementUnitID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Alternate_MeasurementUnitID  
	   {
	    
	     get
		{
		   return alternate_MeasurementUnitID;
		 }
		 set
		 {
		   if(alternate_MeasurementUnitID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Alternate_MeasurementUnitID",OldValue=alternate_MeasurementUnitID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   alternate_MeasurementUnitID=value;
		   }
			
		 }
	   }
	  private decimal? optionalTaxAddition ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? OptionalTaxAddition  
	   {
	    
	     get
		{
		   return optionalTaxAddition;
		 }
		 set
		 {
		   if(optionalTaxAddition != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OptionalTaxAddition",OldValue=optionalTaxAddition,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   optionalTaxAddition=value;
		   }
			
		 }
	   }
	  private decimal? rate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Rate  
	   {
	    
	     get
		{
		   return rate;
		 }
		 set
		 {
		   if(rate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Rate",OldValue=rate,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   rate=value;
		   }
			
		 }
	   }
	  private decimal? reductionRate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ReductionRate  
	   {
	    
	     get
		{
		   return reductionRate;
		 }
		 set
		 {
		   if(reductionRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReductionRate",OldValue=reductionRate,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   reductionRate=value;
		   }
			
		 }
	   }
	  private string tariffRelatedToQuotaID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffRelatedToQuotaID  
	   {
	    
	     get
		{
		   return tariffRelatedToQuotaID;
		 }
		 set
		 {
		   if(tariffRelatedToQuotaID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffRelatedToQuotaID",OldValue=tariffRelatedToQuotaID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffRelatedToQuotaID=value;
		   }
			
		 }
	   }
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string englishNotes ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EnglishNotes  
	   {
	    
	     get
		{
		   return englishNotes;
		 }
		 set
		 {
		   if(englishNotes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EnglishNotes",OldValue=englishNotes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   englishNotes=value;
		   }
			
		 }
	   }
	  private string cB_ID ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CB_ID  
	   {
	    
	     get
		{
		   return cB_ID;
		 }
		 set
		 {
		   if(cB_ID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CB_ID",OldValue=cB_ID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cB_ID=value;
		   }
			
		 }
	   }
	    }
   
}
	 