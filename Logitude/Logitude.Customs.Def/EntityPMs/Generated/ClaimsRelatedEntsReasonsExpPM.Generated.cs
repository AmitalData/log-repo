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
   public partial class ClaimsRelatedEntsReasonsExpPM : EntityPM
   {
   	  private string claimId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClaimId  
	   {
	    
	     get
		{
		   return claimId;
		 }
		 set
		 {
		   if(claimId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClaimId",OldValue=claimId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   claimId=value;
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
	  private int counterKey ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CounterKey  
	   {
	    
	     get
		{
		   return counterKey;
		 }
		 set
		 {
		   if(counterKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CounterKey",OldValue=counterKey,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   counterKey=value;
		   }
			
		 }
	   }
	  private int reasonLineNo ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ReasonLineNo  
	   {
	    
	     get
		{
		   return reasonLineNo;
		 }
		 set
		 {
		   if(reasonLineNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ReasonLineNo",OldValue=reasonLineNo,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   reasonLineNo=value;
		   }
			
		 }
	   }
	  private int lineNo ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int LineNo  
	   {
	    
	     get
		{
		   return lineNo;
		 }
		 set
		 {
		   if(lineNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineNo",OldValue=lineNo,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   lineNo=value;
		   }
			
		 }
	   }
	  private string claimExplanationTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClaimExplanationTypeCode  
	   {
	    
	     get
		{
		   return claimExplanationTypeCode;
		 }
		 set
		 {
		   if(claimExplanationTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClaimExplanationTypeCode",OldValue=claimExplanationTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   claimExplanationTypeCode=value;
		   }
			
		 }
	   }
	  private string claimExplanationTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClaimExplanationTypeName  
	   {
	    
	     get
		{
		   return claimExplanationTypeName;
		 }
		 set
		 {
		   if(claimExplanationTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClaimExplanationTypeName",OldValue=claimExplanationTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   claimExplanationTypeName=value;
		   }
			
		 }
	   }
	  private string explanationNote ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExplanationNote  
	   {
	    
	     get
		{
		   return explanationNote;
		 }
		 set
		 {
		   if(explanationNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExplanationNote",OldValue=explanationNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   explanationNote=value;
		   }
			
		 }
	   }
   }
   
}
	 