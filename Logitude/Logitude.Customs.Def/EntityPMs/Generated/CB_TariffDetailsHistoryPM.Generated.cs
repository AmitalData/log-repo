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
   public partial class CB_TariffDetailsHistoryPM : EntityPM
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
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private int tariffID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int TariffID  
	   {
	    
	     get
		{
		   return tariffID;
		 }
		 set
		 {
		   if(tariffID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffID",OldValue=tariffID,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   tariffID=value;
		   }
			
		 }
	   }
	  private int? quotaID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? QuotaID  
	   {
	    
	     get
		{
		   return quotaID;
		 }
		 set
		 {
		   if(quotaID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuotaID",OldValue=quotaID,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   quotaID=value;
		   }
			
		 }
	   }
	  private DateTime? startDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartDate  
	   {
	    
	     get
		{
		   return startDate;
		 }
		 set
		 {
		   if(startDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDate",OldValue=startDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startDate=value;
		   }
			
		 }
	   }
	  private DateTime? endDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndDate  
	   {
	    
	     get
		{
		   return endDate;
		 }
		 set
		 {
		   if(endDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndDate",OldValue=endDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endDate=value;
		   }
			
		 }
	   }
	  private string entityStatusID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityStatusID  
	   {
	    
	     get
		{
		   return entityStatusID;
		 }
		 set
		 {
		   if(entityStatusID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityStatusID",OldValue=entityStatusID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityStatusID=value;
		   }
			
		 }
	   }
	  private int? withinQuota_ComputMethDataID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? WithinQuota_ComputMethDataID  
	   {
	    
	     get
		{
		   return withinQuota_ComputMethDataID;
		 }
		 set
		 {
		   if(withinQuota_ComputMethDataID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WithinQuota_ComputMethDataID",OldValue=withinQuota_ComputMethDataID,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   withinQuota_ComputMethDataID=value;
		   }
			
		 }
	   }
	  private int? withoutQuota_ComputMethDataID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? WithoutQuota_ComputMethDataID  
	   {
	    
	     get
		{
		   return withoutQuota_ComputMethDataID;
		 }
		 set
		 {
		   if(withoutQuota_ComputMethDataID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WithoutQuota_ComputMethDataID",OldValue=withoutQuota_ComputMethDataID,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   withoutQuota_ComputMethDataID=value;
		   }
			
		 }
	   }
	  private int changeRequestTypePriority ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ChangeRequestTypePriority  
	   {
	    
	     get
		{
		   return changeRequestTypePriority;
		 }
		 set
		 {
		   if(changeRequestTypePriority != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChangeRequestTypePriority",OldValue=changeRequestTypePriority,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   changeRequestTypePriority=value;
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
	 