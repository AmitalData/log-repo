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
   public partial class AccountingEntitiesJournalPM : EntityPM
   {
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
	  private string accountingEntityId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountingEntityId  
	   {
	    
	     get
		{
		   return accountingEntityId;
		 }
		 set
		 {
		   if(accountingEntityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingEntityId",OldValue=accountingEntityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountingEntityId=value;
		   }
			
		 }
	   }
	  private string accountingEntityCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountingEntityCode  
	   {
	    
	     get
		{
		   return accountingEntityCode;
		 }
		 set
		 {
		   if(accountingEntityCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingEntityCode",OldValue=accountingEntityCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountingEntityCode=value;
		   }
			
		 }
	   }
	  private string action ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Action  
	   {
	    
	     get
		{
		   return action;
		 }
		 set
		 {
		   if(action != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Action",OldValue=action,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   action=value;
		   }
			
		 }
	   }
	  private int id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Id  
	   {
	    
	     get
		{
		   return id;
		 }
		 set
		 {
		   if(id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Id",OldValue=id,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   id=value;
		   }
			
		 }
	   }
	  private string childEntityId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChildEntityId  
	   {
	    
	     get
		{
		   return childEntityId;
		 }
		 set
		 {
		   if(childEntityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChildEntityId",OldValue=childEntityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   childEntityId=value;
		   }
			
		 }
	   }
   }
   
}
	 