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
   public partial class GLAccountRecocileDataPM : EntityPM
   {
   	  private string accountId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountId  
	   {
	    
	     get
		{
		   return accountId;
		 }
		 set
		 {
		   if(accountId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountId",OldValue=accountId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountId=value;
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
	  private string lastReconciledByUserId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastReconciledByUserId  
	   {
	    
	     get
		{
		   return lastReconciledByUserId;
		 }
		 set
		 {
		   if(lastReconciledByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastReconciledByUserId",OldValue=lastReconciledByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastReconciledByUserId=value;
		   }
			
		 }
	   }
	  private DateTime? lastReconcileDateTime ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastReconcileDateTime  
	   {
	    
	     get
		{
		   return lastReconcileDateTime;
		 }
		 set
		 {
		   if(lastReconcileDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastReconcileDateTime",OldValue=lastReconcileDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastReconcileDateTime=value;
		   }
			
		 }
	   }
   }
   
}
	 