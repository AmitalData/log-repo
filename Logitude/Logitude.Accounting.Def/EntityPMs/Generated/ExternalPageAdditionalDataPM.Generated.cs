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
   public partial class ExternalPageAdditionalDataPM : EntityPM
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
	  private string objectTableId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ObjectTableId  
	   {
	    
	     get
		{
		   return objectTableId;
		 }
		 set
		 {
		   if(objectTableId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ObjectTableId",OldValue=objectTableId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   objectTableId=value;
		   }
			
		 }
	   }
	  private string entityId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityId  
	   {
	    
	     get
		{
		   return entityId;
		 }
		 set
		 {
		   if(entityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityId",OldValue=entityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityId=value;
		   }
			
		 }
	   }
	  private string lastPageNumber ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastPageNumber  
	   {
	    
	     get
		{
		   return lastPageNumber;
		 }
		 set
		 {
		   if(lastPageNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastPageNumber",OldValue=lastPageNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastPageNumber=value;
		   }
			
		 }
	   }
	  private DateTime? lastPageEndDate ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastPageEndDate  
	   {
	    
	     get
		{
		   return lastPageEndDate;
		 }
		 set
		 {
		   if(lastPageEndDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastPageEndDate",OldValue=lastPageEndDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastPageEndDate=value;
		   }
			
		 }
	   }
	  private decimal? lastPageCloseBalance ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? LastPageCloseBalance  
	   {
	    
	     get
		{
		   return lastPageCloseBalance;
		 }
		 set
		 {
		   if(lastPageCloseBalance != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastPageCloseBalance",OldValue=lastPageCloseBalance,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   lastPageCloseBalance=value;
		   }
			
		 }
	   }
   }
   
}
	 