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
   public partial class InterestReportLinesByDatePM : EntityPM
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
	  private string interestReportId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterestReportId  
	   {
	    
	     get
		{
		   return interestReportId;
		 }
		 set
		 {
		   if(interestReportId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterestReportId",OldValue=interestReportId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interestReportId=value;
		   }
			
		 }
	   }
	  private DateTime fromDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime FromDate  
	   {
	    
	     get
		{
		   return fromDate;
		 }
		 set
		 {
		   if(fromDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromDate",OldValue=fromDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   fromDate=value;
		   }
			
		 }
	   }
	  private DateTime toDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ToDate  
	   {
	    
	     get
		{
		   return toDate;
		 }
		 set
		 {
		   if(toDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToDate",OldValue=toDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   toDate=value;
		   }
			
		 }
	   }
	  private int totalInterestDays ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int TotalInterestDays  
	   {
	    
	     get
		{
		   return totalInterestDays;
		 }
		 set
		 {
		   if(totalInterestDays != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalInterestDays",OldValue=totalInterestDays,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   totalInterestDays=value;
		   }
			
		 }
	   }
	  private decimal totalAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal TotalAmount  
	   {
	    
	     get
		{
		   return totalAmount;
		 }
		 set
		 {
		   if(totalAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalAmount",OldValue=totalAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   totalAmount=value;
		   }
			
		 }
	   }
	  private decimal accumulatedAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal AccumulatedAmount  
	   {
	    
	     get
		{
		   return accumulatedAmount;
		 }
		 set
		 {
		   if(accumulatedAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccumulatedAmount",OldValue=accumulatedAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   accumulatedAmount=value;
		   }
			
		 }
	   }
	  private decimal standardInterestPercentage ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal StandardInterestPercentage  
	   {
	    
	     get
		{
		   return standardInterestPercentage;
		 }
		 set
		 {
		   if(standardInterestPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StandardInterestPercentage",OldValue=standardInterestPercentage,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   standardInterestPercentage=value;
		   }
			
		 }
	   }
	  private decimal exceptionalInterestPercentage ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ExceptionalInterestPercentage  
	   {
	    
	     get
		{
		   return exceptionalInterestPercentage;
		 }
		 set
		 {
		   if(exceptionalInterestPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExceptionalInterestPercentage",OldValue=exceptionalInterestPercentage,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   exceptionalInterestPercentage=value;
		   }
			
		 }
	   }
	  private decimal creditInterestPercentage ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CreditInterestPercentage  
	   {
	    
	     get
		{
		   return creditInterestPercentage;
		 }
		 set
		 {
		   if(creditInterestPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditInterestPercentage",OldValue=creditInterestPercentage,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   creditInterestPercentage=value;
		   }
			
		 }
	   }
	  private decimal standardInterestAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal StandardInterestAmount  
	   {
	    
	     get
		{
		   return standardInterestAmount;
		 }
		 set
		 {
		   if(standardInterestAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StandardInterestAmount",OldValue=standardInterestAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   standardInterestAmount=value;
		   }
			
		 }
	   }
	  private decimal exceptionalInterestAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal ExceptionalInterestAmount  
	   {
	    
	     get
		{
		   return exceptionalInterestAmount;
		 }
		 set
		 {
		   if(exceptionalInterestAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExceptionalInterestAmount",OldValue=exceptionalInterestAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   exceptionalInterestAmount=value;
		   }
			
		 }
	   }
	  private decimal creditInterestAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CreditInterestAmount  
	   {
	    
	     get
		{
		   return creditInterestAmount;
		 }
		 set
		 {
		   if(creditInterestAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreditInterestAmount",OldValue=creditInterestAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   creditInterestAmount=value;
		   }
			
		 }
	   }
	  private decimal calculatedStandInterestAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CalculatedStandInterestAmount  
	   {
	    
	     get
		{
		   return calculatedStandInterestAmount;
		 }
		 set
		 {
		   if(calculatedStandInterestAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculatedStandInterestAmount",OldValue=calculatedStandInterestAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   calculatedStandInterestAmount=value;
		   }
			
		 }
	   }
	  private decimal calculatedExcepInterestAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CalculatedExcepInterestAmount  
	   {
	    
	     get
		{
		   return calculatedExcepInterestAmount;
		 }
		 set
		 {
		   if(calculatedExcepInterestAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculatedExcepInterestAmount",OldValue=calculatedExcepInterestAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   calculatedExcepInterestAmount=value;
		   }
			
		 }
	   }
	  private decimal calculatedCreditInterestAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal CalculatedCreditInterestAmount  
	   {
	    
	     get
		{
		   return calculatedCreditInterestAmount;
		 }
		 set
		 {
		   if(calculatedCreditInterestAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculatedCreditInterestAmount",OldValue=calculatedCreditInterestAmount,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   calculatedCreditInterestAmount=value;
		   }
			
		 }
	   }
	  private string calculationDetails ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CalculationDetails  
	   {
	    
	     get
		{
		   return calculationDetails;
		 }
		 set
		 {
		   if(calculationDetails != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CalculationDetails",OldValue=calculationDetails,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   calculationDetails=value;
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
	  private decimal totalInterest ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal TotalInterest  
	   {
	    
	     get
		{
		   return totalInterest;
		 }
		 set
		 {
		   if(totalInterest != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalInterest",OldValue=totalInterest,NewValue=value,PropertyType="decimal"};
		    NotifyPropertyChanged(values);
		   totalInterest=value;
		   }
			
		 }
	   }
	  private bool? isOpenBalanceLine ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? IsOpenBalanceLine  
	   {
	    
	     get
		{
		   return isOpenBalanceLine;
		 }
		 set
		 {
		   if(isOpenBalanceLine != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsOpenBalanceLine",OldValue=isOpenBalanceLine,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   isOpenBalanceLine=value;
		   }
			
		 }
	   }
   }
   
}
	 