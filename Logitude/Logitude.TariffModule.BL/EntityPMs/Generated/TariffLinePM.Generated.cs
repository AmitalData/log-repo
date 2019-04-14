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
using Logitude.TariffModule.BL.Validators;
  
namespace Logitude.TariffModule.BL.EntityPMs
{
   [CustomValidation(typeof(TariffModuleClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TariffLinePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private DateTime? startDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private DateTime? expirationDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ExpirationDate  
	   {
	    
	     get
		{
		   return expirationDate;
		 }
		 set
		 {
		   if(expirationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExpirationDate",OldValue=expirationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   expirationDate=value;
		   }
			
		 }
	   }
	  private string tariffId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffId  
	   {
	    
	     get
		{
		   return tariffId;
		 }
		 set
		 {
		   if(tariffId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffId",OldValue=tariffId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffId=value;
		   }
			
		 }
	   }
	  private int version ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int Version  
	   {
	    
	     get
		{
		   return version;
		 }
		 set
		 {
		   if(version != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Version",OldValue=version,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   version=value;
		   }
			
		 }
	   }
	  private int? minPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? MinPrice  
	   {
	    
	     get
		{
		   return minPrice;
		 }
		 set
		 {
		   if(minPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MinPrice",OldValue=minPrice,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   minPrice=value;
		   }
			
		 }
	   }
	  private int? step1Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Step1Price  
	   {
	    
	     get
		{
		   return step1Price;
		 }
		 set
		 {
		   if(step1Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step1Price",OldValue=step1Price,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   step1Price=value;
		   }
			
		 }
	   }
	  private int? step2Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Step2Price  
	   {
	    
	     get
		{
		   return step2Price;
		 }
		 set
		 {
		   if(step2Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step2Price",OldValue=step2Price,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   step2Price=value;
		   }
			
		 }
	   }
	  private int? step3Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Step3Price  
	   {
	    
	     get
		{
		   return step3Price;
		 }
		 set
		 {
		   if(step3Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step3Price",OldValue=step3Price,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   step3Price=value;
		   }
			
		 }
	   }
	  private int? step4Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Step4Price  
	   {
	    
	     get
		{
		   return step4Price;
		 }
		 set
		 {
		   if(step4Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step4Price",OldValue=step4Price,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   step4Price=value;
		   }
			
		 }
	   }
	  private int? step5Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Step5Price  
	   {
	    
	     get
		{
		   return step5Price;
		 }
		 set
		 {
		   if(step5Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step5Price",OldValue=step5Price,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   step5Price=value;
		   }
			
		 }
	   }
	  private int? step6Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Step6Price  
	   {
	    
	     get
		{
		   return step6Price;
		 }
		 set
		 {
		   if(step6Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step6Price",OldValue=step6Price,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   step6Price=value;
		   }
			
		 }
	   }
	  private int? step7Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Step7Price  
	   {
	    
	     get
		{
		   return step7Price;
		 }
		 set
		 {
		   if(step7Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step7Price",OldValue=step7Price,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   step7Price=value;
		   }
			
		 }
	   }
	  private int? step8Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Step8Price  
	   {
	    
	     get
		{
		   return step8Price;
		 }
		 set
		 {
		   if(step8Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step8Price",OldValue=step8Price,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   step8Price=value;
		   }
			
		 }
	   }
	  private string originPortId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginPortId  
	   {
	    
	     get
		{
		   return originPortId;
		 }
		 set
		 {
		   if(originPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginPortId",OldValue=originPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originPortId=value;
		   }
			
		 }
	   }
   }
   
}
	 