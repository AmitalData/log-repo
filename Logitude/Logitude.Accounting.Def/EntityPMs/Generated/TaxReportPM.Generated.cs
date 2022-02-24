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
   public partial class TaxReportPM : EntityPM
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
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
		   }
			
		 }
	   }
	  private DateTime lastUpdateDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime LastUpdateDate  
	   {
	    
	     get
		{
		   return lastUpdateDate;
		 }
		 set
		 {
		   if(lastUpdateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastUpdateDate",OldValue=lastUpdateDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   lastUpdateDate=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId  
	   {
	    
	     get
		{
		   return updatedByUserId;
		 }
		 set
		 {
		   if(updatedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserId",OldValue=updatedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserId=value;
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
	  private DateTime taxReportMonth ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime TaxReportMonth  
	   {
	    
	     get
		{
		   return taxReportMonth;
		 }
		 set
		 {
		   if(taxReportMonth != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxReportMonth",OldValue=taxReportMonth,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   taxReportMonth=value;
		   }
			
		 }
	   }
	  private string taxReportNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaxReportNumber  
	   {
	    
	     get
		{
		   return taxReportNumber;
		 }
		 set
		 {
		   if(taxReportNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxReportNumber",OldValue=taxReportNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taxReportNumber=value;
		   }
			
		 }
	   }
	  private string vatNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VatNumber  
	   {
	    
	     get
		{
		   return vatNumber;
		 }
		 set
		 {
		   if(vatNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VatNumber",OldValue=vatNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vatNumber=value;
		   }
			
		 }
	   }
	  private string taxReportTypeCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaxReportTypeCode  
	   {
	    
	     get
		{
		   return taxReportTypeCode;
		 }
		 set
		 {
		   if(taxReportTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxReportTypeCode",OldValue=taxReportTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taxReportTypeCode=value;
		   }
			
		 }
	   }
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCancelled  
	   {
	    
	     get
		{
		   return isCancelled;
		 }
		 set
		 {
		   if(isCancelled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancelled",OldValue=isCancelled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCancelled=value;
		   }
			
		 }
	   }
	  private decimal? taxableOutputAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TaxableOutputAmount  
	   {
	    
	     get
		{
		   return taxableOutputAmount;
		 }
		 set
		 {
		   if(taxableOutputAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxableOutputAmount",OldValue=taxableOutputAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   taxableOutputAmount=value;
		   }
			
		 }
	   }
	  private decimal? outputTaxAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? OutputTaxAmount  
	   {
	    
	     get
		{
		   return outputTaxAmount;
		 }
		 set
		 {
		   if(outputTaxAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OutputTaxAmount",OldValue=outputTaxAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   outputTaxAmount=value;
		   }
			
		 }
	   }
	  private decimal? taxableOutputsWithDiffPercent ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TaxableOutputsWithDiffPercent  
	   {
	    
	     get
		{
		   return taxableOutputsWithDiffPercent;
		 }
		 set
		 {
		   if(taxableOutputsWithDiffPercent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxableOutputsWithDiffPercent",OldValue=taxableOutputsWithDiffPercent,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   taxableOutputsWithDiffPercent=value;
		   }
			
		 }
	   }
	  private decimal? outputTaxAmountWithDiffPercent ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? OutputTaxAmountWithDiffPercent  
	   {
	    
	     get
		{
		   return outputTaxAmountWithDiffPercent;
		 }
		 set
		 {
		   if(outputTaxAmountWithDiffPercent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OutputTaxAmountWithDiffPercent",OldValue=outputTaxAmountWithDiffPercent,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   outputTaxAmountWithDiffPercent=value;
		   }
			
		 }
	   }
	  private decimal? exemptTaxableOutput ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ExemptTaxableOutput  
	   {
	    
	     get
		{
		   return exemptTaxableOutput;
		 }
		 set
		 {
		   if(exemptTaxableOutput != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExemptTaxableOutput",OldValue=exemptTaxableOutput,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   exemptTaxableOutput=value;
		   }
			
		 }
	   }
	  private int? outputLinesCount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? OutputLinesCount  
	   {
	    
	     get
		{
		   return outputLinesCount;
		 }
		 set
		 {
		   if(outputLinesCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OutputLinesCount",OldValue=outputLinesCount,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   outputLinesCount=value;
		   }
			
		 }
	   }
	  private decimal? otherInputsTaxAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? OtherInputsTaxAmount  
	   {
	    
	     get
		{
		   return otherInputsTaxAmount;
		 }
		 set
		 {
		   if(otherInputsTaxAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OtherInputsTaxAmount",OldValue=otherInputsTaxAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   otherInputsTaxAmount=value;
		   }
			
		 }
	   }
	  private decimal? equipmentInputsTaxAmount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? EquipmentInputsTaxAmount  
	   {
	    
	     get
		{
		   return equipmentInputsTaxAmount;
		 }
		 set
		 {
		   if(equipmentInputsTaxAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EquipmentInputsTaxAmount",OldValue=equipmentInputsTaxAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   equipmentInputsTaxAmount=value;
		   }
			
		 }
	   }
	  private int? inputLinesCount ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? InputLinesCount  
	   {
	    
	     get
		{
		   return inputLinesCount;
		 }
		 set
		 {
		   if(inputLinesCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InputLinesCount",OldValue=inputLinesCount,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   inputLinesCount=value;
		   }
			
		 }
	   }
	  private decimal? amountForPayRefund ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? AmountForPayRefund  
	   {
	    
	     get
		{
		   return amountForPayRefund;
		 }
		 set
		 {
		   if(amountForPayRefund != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AmountForPayRefund",OldValue=amountForPayRefund,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   amountForPayRefund=value;
		   }
			
		 }
	   }
	  private string statusCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusCode  
	   {
	    
	     get
		{
		   return statusCode;
		 }
		 set
		 {
		   if(statusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusCode",OldValue=statusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusCode=value;
		   }
			
		 }
	   }
	  private DateTime? processStartDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ProcessStartDate  
	   {
	    
	     get
		{
		   return processStartDate;
		 }
		 set
		 {
		   if(processStartDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProcessStartDate",OldValue=processStartDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   processStartDate=value;
		   }
			
		 }
	   }
	  private DateTime? processEndDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ProcessEndDate  
	   {
	    
	     get
		{
		   return processEndDate;
		 }
		 set
		 {
		   if(processEndDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProcessEndDate",OldValue=processEndDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   processEndDate=value;
		   }
			
		 }
	   }
	  private int? processProgress ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ProcessProgress  
	   {
	    
	     get
		{
		   return processProgress;
		 }
		 set
		 {
		   if(processProgress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProcessProgress",OldValue=processProgress,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   processProgress=value;
		   }
			
		 }
	   }
	  private string statusLocalName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusLocalName  
	   {
	    
	     get
		{
		   return statusLocalName;
		 }
		 set
		 {
		   if(statusLocalName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusLocalName",OldValue=statusLocalName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusLocalName=value;
		   }
			
		 }
	   }
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserName  
	   {
	    
	     get
		{
		   return createdByUserName;
		 }
		 set
		 {
		   if(createdByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserName",OldValue=createdByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserName=value;
		   }
			
		 }
	   }
	  private int year ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Year  
	   {
	    
	     get
		{
		   return year;
		 }
		 set
		 {
		   if(year != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Year",OldValue=year,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   year=value;
		   }
			
		 }
	   }
	  private string statusEnglishName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusEnglishName  
	   {
	    
	     get
		{
		   return statusEnglishName;
		 }
		 set
		 {
		   if(statusEnglishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusEnglishName",OldValue=statusEnglishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusEnglishName=value;
		   }
			
		 }
	   }
	  private int taxReportLineLastLine ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int TaxReportLineLastLine  
	   {
	    
	     get
		{
		   return taxReportLineLastLine;
		 }
		 set
		 {
		   if(taxReportLineLastLine != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxReportLineLastLine",OldValue=taxReportLineLastLine,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   taxReportLineLastLine=value;
		   }
			
		 }
	   }
	  private bool needsRebulid ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool NeedsRebulid  
	   {
	    
	     get
		{
		   return needsRebulid;
		 }
		 set
		 {
		   if(needsRebulid != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NeedsRebulid",OldValue=needsRebulid,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   needsRebulid=value;
		   }
			
		 }
	   }
	  private bool isNew ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsNew  
	   {
	    
	     get
		{
		   return isNew;
		 }
		 set
		 {
		   if(isNew != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsNew",OldValue=isNew,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isNew=value;
		   }
			
		 }
	   }
	  private string updatedByUserName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserName  
	   {
	    
	     get
		{
		   return updatedByUserName;
		 }
		 set
		 {
		   if(updatedByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserName",OldValue=updatedByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserName=value;
		   }
			
		 }
	   }
	  private bool createdInTwoMonthsLogic ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CreatedInTwoMonthsLogic  
	   {
	    
	     get
		{
		   return createdInTwoMonthsLogic;
		 }
		 set
		 {
		   if(createdInTwoMonthsLogic != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedInTwoMonthsLogic",OldValue=createdInTwoMonthsLogic,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   createdInTwoMonthsLogic=value;
		   }
			
		 }
	   }
	  private decimal? outputTaxAmountRound ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? OutputTaxAmountRound  
	   {
	    
	     get
		{
		   return outputTaxAmountRound;
		 }
		 set
		 {
		   if(outputTaxAmountRound != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OutputTaxAmountRound",OldValue=outputTaxAmountRound,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   outputTaxAmountRound=value;
		   }
			
		 }
	   }
	  private decimal? inputsTaxAmountRound ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? InputsTaxAmountRound  
	   {
	    
	     get
		{
		   return inputsTaxAmountRound;
		 }
		 set
		 {
		   if(inputsTaxAmountRound != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InputsTaxAmountRound",OldValue=inputsTaxAmountRound,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   inputsTaxAmountRound=value;
		   }
			
		 }
	   }
   }
   
}
	 