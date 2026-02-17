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
using Logitude.CRM.BL.Validators;
  
namespace Logitude.CRM.BL.EntityPMs
{
   [CustomValidation(typeof(CRMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class SLAEscalationPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string sLAHeaderId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SLAHeaderId  
	   {
	    
	     get
		{
		   return sLAHeaderId;
		 }
		 set
		 {
		   if(sLAHeaderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SLAHeaderId",OldValue=sLAHeaderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sLAHeaderId=value;
		   }
			
		 }
	   }
	  private int lineNumber ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string escalationFor ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EscalationFor  
	   {
	    
	     get
		{
		   return escalationFor;
		 }
		 set
		 {
		   if(escalationFor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EscalationFor",OldValue=escalationFor,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   escalationFor=value;
		   }
			
		 }
	   }
	  private string escalationActionTimeIndicator ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EscalationActionTimeIndicator  
	   {
	    
	     get
		{
		   return escalationActionTimeIndicator;
		 }
		 set
		 {
		   if(escalationActionTimeIndicator != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EscalationActionTimeIndicator",OldValue=escalationActionTimeIndicator,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   escalationActionTimeIndicator=value;
		   }
			
		 }
	   }
	  private int? escalationTime ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? EscalationTime  
	   {
	    
	     get
		{
		   return escalationTime;
		 }
		 set
		 {
		   if(escalationTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EscalationTime",OldValue=escalationTime,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   escalationTime=value;
		   }
			
		 }
	   }
	  private string escalationTimeUnit ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EscalationTimeUnit  
	   {
	    
	     get
		{
		   return escalationTimeUnit;
		 }
		 set
		 {
		   if(escalationTimeUnit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EscalationTimeUnit",OldValue=escalationTimeUnit,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   escalationTimeUnit=value;
		   }
			
		 }
	   }
	  private int? escalaitonTimeInMinutes ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? EscalaitonTimeInMinutes  
	   {
	    
	     get
		{
		   return escalaitonTimeInMinutes;
		 }
		 set
		 {
		   if(escalaitonTimeInMinutes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EscalaitonTimeInMinutes",OldValue=escalaitonTimeInMinutes,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   escalaitonTimeInMinutes=value;
		   }
			
		 }
	   }

	   private List<SLAEscalationRecepientPM> sLAEscalationRecepients;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SLAEscalationRecepientSLAEscalation", "Id","SLAEscalationId")]
	   [DataMember]
	   public virtual List<SLAEscalationRecepientPM> SLAEscalationRecepients  
	   {
	        get
             {
                 if (sLAEscalationRecepients == null)
                 {
                     sLAEscalationRecepients = new List<SLAEscalationRecepientPM>();
                 }
                 return sLAEscalationRecepients;
              }
             set { sLAEscalationRecepients = value; }
	    }
		   
	   private List<SLAEscalationRecepientPM>  deletedSLAEscalationRecepients;
	   public virtual List<SLAEscalationRecepientPM> DeletedSLAEscalationRecepients  
	   {
	        get
             {
                 if ( deletedSLAEscalationRecepients == null)
                 {
                      deletedSLAEscalationRecepients = new List<SLAEscalationRecepientPM>();
                 }
                 return  deletedSLAEscalationRecepients;
              }
             set {  deletedSLAEscalationRecepients = value; }
	    }
	  	  private string timeIndicator ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TimeIndicator  
	   {
	    
	     get
		{
		   return timeIndicator;
		 }
		 set
		 {
		   if(timeIndicator != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TimeIndicator",OldValue=timeIndicator,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   timeIndicator=value;
		   }
			
		 }
	   }
	  private string timeUnitName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TimeUnitName  
	   {
	    
	     get
		{
		   return timeUnitName;
		 }
		 set
		 {
		   if(timeUnitName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TimeUnitName",OldValue=timeUnitName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   timeUnitName=value;
		   }
			
		 }
	   }
   }
   
}
	 