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
   public partial class CustomsCollateralsAnswerPM : EntityPM
   {
   	  private string customsCollateralId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsCollateralId  
	   {
	    
	     get
		{
		   return customsCollateralId;
		 }
		 set
		 {
		   if(customsCollateralId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsCollateralId",OldValue=customsCollateralId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsCollateralId=value;
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
	  private string answerEntityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AnswerEntityTypeCode  
	   {
	    
	     get
		{
		   return answerEntityTypeCode;
		 }
		 set
		 {
		   if(answerEntityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnswerEntityTypeCode",OldValue=answerEntityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   answerEntityTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? allocatedAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? AllocatedAmount  
	   {
	    
	     get
		{
		   return allocatedAmount;
		 }
		 set
		 {
		   if(allocatedAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AllocatedAmount",OldValue=allocatedAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   allocatedAmount=value;
		   }
			
		 }
	   }
	  private string remarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Remarks  
	   {
	    
	     get
		{
		   return remarks;
		 }
		 set
		 {
		   if(remarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Remarks",OldValue=remarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   remarks=value;
		   }
			
		 }
	   }
	  private string customsTapgFile ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsTapgFile  
	   {
	    
	     get
		{
		   return customsTapgFile;
		 }
		 set
		 {
		   if(customsTapgFile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsTapgFile",OldValue=customsTapgFile,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsTapgFile=value;
		   }
			
		 }
	   }
	  private string customsNumeral ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsNumeral  
	   {
	    
	     get
		{
		   return customsNumeral;
		 }
		 set
		 {
		   if(customsNumeral != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsNumeral",OldValue=customsNumeral,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsNumeral=value;
		   }
			
		 }
	   }
	  private string answerForCollateralStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AnswerForCollateralStatusCode  
	   {
	    
	     get
		{
		   return answerForCollateralStatusCode;
		 }
		 set
		 {
		   if(answerForCollateralStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnswerForCollateralStatusCode",OldValue=answerForCollateralStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   answerForCollateralStatusCode=value;
		   }
			
		 }
	   }
	  private string errors ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Errors  
	   {
	    
	     get
		{
		   return errors;
		 }
		 set
		 {
		   if(errors != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Errors",OldValue=errors,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   errors=value;
		   }
			
		 }
	   }
	  private string answerEntityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AnswerEntityTypeName  
	   {
	    
	     get
		{
		   return answerEntityTypeName;
		 }
		 set
		 {
		   if(answerEntityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnswerEntityTypeName",OldValue=answerEntityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   answerEntityTypeName=value;
		   }
			
		 }
	   }
	  private string answerForCollateralStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AnswerForCollateralStatusName  
	   {
	    
	     get
		{
		   return answerForCollateralStatusName;
		 }
		 set
		 {
		   if(answerForCollateralStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnswerForCollateralStatusName",OldValue=answerForCollateralStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   answerForCollateralStatusName=value;
		   }
			
		 }
	   }
	  private string tapagId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TapagId  
	   {
	    
	     get
		{
		   return tapagId;
		 }
		 set
		 {
		   if(tapagId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TapagId",OldValue=tapagId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tapagId=value;
		   }
			
		 }
	   }
	  private bool newFileRequest ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool NewFileRequest  
	   {
	    
	     get
		{
		   return newFileRequest;
		 }
		 set
		 {
		   if(newFileRequest != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NewFileRequest",OldValue=newFileRequest,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   newFileRequest=value;
		   }
			
		 }
	   }
	  private string requestFileTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestFileTypeCode  
	   {
	    
	     get
		{
		   return requestFileTypeCode;
		 }
		 set
		 {
		   if(requestFileTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestFileTypeCode",OldValue=requestFileTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestFileTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? requestFileAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? RequestFileAmount  
	   {
	    
	     get
		{
		   return requestFileAmount;
		 }
		 set
		 {
		   if(requestFileAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestFileAmount",OldValue=requestFileAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   requestFileAmount=value;
		   }
			
		 }
	   }

	   private List<CollateralsRequestFileCondPM> collateralsRequestFileConds;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("CollateralsRequestFileCondAnswers", "CustomsCollateralId,LineNumber","CustomsCollateralId,LineNumber")]
	   [DataMember]
	   public virtual List<CollateralsRequestFileCondPM> CollateralsRequestFileConds  
	   {
	        get
             {
                 if (collateralsRequestFileConds == null)
                 {
                     collateralsRequestFileConds = new List<CollateralsRequestFileCondPM>();
                 }
                 return collateralsRequestFileConds;
              }
             set { collateralsRequestFileConds = value; }
	    }
		   
	   private List<CollateralsRequestFileCondPM>  deletedCollateralsRequestFileConds;
	   public virtual List<CollateralsRequestFileCondPM> DeletedCollateralsRequestFileConds  
	   {
	        get
             {
                 if ( deletedCollateralsRequestFileConds == null)
                 {
                      deletedCollateralsRequestFileConds = new List<CollateralsRequestFileCondPM>();
                 }
                 return  deletedCollateralsRequestFileConds;
              }
             set {  deletedCollateralsRequestFileConds = value; }
	    }
	  	  private int answerRequestFileConditionLastLineNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int AnswerRequestFileConditionLastLineNumber  
	   {
	    
	     get
		{
		   return answerRequestFileConditionLastLineNumber;
		 }
		 set
		 {
		   if(answerRequestFileConditionLastLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnswerRequestFileConditionLastLineNumber",OldValue=answerRequestFileConditionLastLineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   answerRequestFileConditionLastLineNumber=value;
		   }
			
		 }
	   }
	  private string requestFileTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestFileTypeName  
	   {
	    
	     get
		{
		   return requestFileTypeName;
		 }
		 set
		 {
		   if(requestFileTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestFileTypeName",OldValue=requestFileTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestFileTypeName=value;
		   }
			
		 }
	   }
	  private bool isClosed ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClosed  
	   {
	    
	     get
		{
		   return isClosed;
		 }
		 set
		 {
		   if(isClosed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosed",OldValue=isClosed,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClosed=value;
		   }
			
		 }
	   }
	  private string requestedTapagNumeral ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestedTapagNumeral  
	   {
	    
	     get
		{
		   return requestedTapagNumeral;
		 }
		 set
		 {
		   if(requestedTapagNumeral != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestedTapagNumeral",OldValue=requestedTapagNumeral,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestedTapagNumeral=value;
		   }
			
		 }
	   }
	  private string requestedTapagFile ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestedTapagFile  
	   {
	    
	     get
		{
		   return requestedTapagFile;
		 }
		 set
		 {
		   if(requestedTapagFile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestedTapagFile",OldValue=requestedTapagFile,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestedTapagFile=value;
		   }
			
		 }
	   }
	  private string paymentOrderId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentOrderId  
	   {
	    
	     get
		{
		   return paymentOrderId;
		 }
		 set
		 {
		   if(paymentOrderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderId",OldValue=paymentOrderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentOrderId=value;
		   }
			
		 }
	   }
	  private string paymentOrderNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentOrderNumber  
	   {
	    
	     get
		{
		   return paymentOrderNumber;
		 }
		 set
		 {
		   if(paymentOrderNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderNumber",OldValue=paymentOrderNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentOrderNumber=value;
		   }
			
		 }
	   }
	  private string paymentOrderStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentOrderStatus  
	   {
	    
	     get
		{
		   return paymentOrderStatus;
		 }
		 set
		 {
		   if(paymentOrderStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderStatus",OldValue=paymentOrderStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentOrderStatus=value;
		   }
			
		 }
	   }
	    }
   
}
	 