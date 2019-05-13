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
	  private decimal? minPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? MinPrice  
	   {
	    
	     get
		{
		   return minPrice;
		 }
		 set
		 {
		   if(minPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MinPrice",OldValue=minPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   minPrice=value;
		   }
			
		 }
	   }
	  private decimal? step1Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Step1Price  
	   {
	    
	     get
		{
		   return step1Price;
		 }
		 set
		 {
		   if(step1Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step1Price",OldValue=step1Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   step1Price=value;
		   }
			
		 }
	   }
	  private decimal? step2Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Step2Price  
	   {
	    
	     get
		{
		   return step2Price;
		 }
		 set
		 {
		   if(step2Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step2Price",OldValue=step2Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   step2Price=value;
		   }
			
		 }
	   }
	  private decimal? step3Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Step3Price  
	   {
	    
	     get
		{
		   return step3Price;
		 }
		 set
		 {
		   if(step3Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step3Price",OldValue=step3Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   step3Price=value;
		   }
			
		 }
	   }
	  private decimal? step4Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Step4Price  
	   {
	    
	     get
		{
		   return step4Price;
		 }
		 set
		 {
		   if(step4Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step4Price",OldValue=step4Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   step4Price=value;
		   }
			
		 }
	   }
	  private decimal? step5Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Step5Price  
	   {
	    
	     get
		{
		   return step5Price;
		 }
		 set
		 {
		   if(step5Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step5Price",OldValue=step5Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   step5Price=value;
		   }
			
		 }
	   }
	  private decimal? step6Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Step6Price  
	   {
	    
	     get
		{
		   return step6Price;
		 }
		 set
		 {
		   if(step6Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step6Price",OldValue=step6Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   step6Price=value;
		   }
			
		 }
	   }
	  private decimal? step7Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Step7Price  
	   {
	    
	     get
		{
		   return step7Price;
		 }
		 set
		 {
		   if(step7Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step7Price",OldValue=step7Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   step7Price=value;
		   }
			
		 }
	   }
	  private decimal? step8Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Step8Price  
	   {
	    
	     get
		{
		   return step8Price;
		 }
		 set
		 {
		   if(step8Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step8Price",OldValue=step8Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   step8Price=value;
		   }
			
		 }
	   }
	  private decimal? surcharge1Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge1Price  
	   {
	    
	     get
		{
		   return surcharge1Price;
		 }
		 set
		 {
		   if(surcharge1Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge1Price",OldValue=surcharge1Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge1Price=value;
		   }
			
		 }
	   }
	  private decimal? surcharge2Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge2Price  
	   {
	    
	     get
		{
		   return surcharge2Price;
		 }
		 set
		 {
		   if(surcharge2Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge2Price",OldValue=surcharge2Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge2Price=value;
		   }
			
		 }
	   }
	  private decimal? surcharge3Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge3Price  
	   {
	    
	     get
		{
		   return surcharge3Price;
		 }
		 set
		 {
		   if(surcharge3Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge3Price",OldValue=surcharge3Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge3Price=value;
		   }
			
		 }
	   }
	  private decimal? surcharge4Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge4Price  
	   {
	    
	     get
		{
		   return surcharge4Price;
		 }
		 set
		 {
		   if(surcharge4Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge4Price",OldValue=surcharge4Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge4Price=value;
		   }
			
		 }
	   }
	  private decimal? surcharge5Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge5Price  
	   {
	    
	     get
		{
		   return surcharge5Price;
		 }
		 set
		 {
		   if(surcharge5Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge5Price",OldValue=surcharge5Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge5Price=value;
		   }
			
		 }
	   }
	  private decimal? surcharge6Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge6Price  
	   {
	    
	     get
		{
		   return surcharge6Price;
		 }
		 set
		 {
		   if(surcharge6Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge6Price",OldValue=surcharge6Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge6Price=value;
		   }
			
		 }
	   }
	  private decimal? surcharge7Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge7Price  
	   {
	    
	     get
		{
		   return surcharge7Price;
		 }
		 set
		 {
		   if(surcharge7Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge7Price",OldValue=surcharge7Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge7Price=value;
		   }
			
		 }
	   }
	  private decimal? surcharge8Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge8Price  
	   {
	    
	     get
		{
		   return surcharge8Price;
		 }
		 set
		 {
		   if(surcharge8Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge8Price",OldValue=surcharge8Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge8Price=value;
		   }
			
		 }
	   }
	  private decimal? surcharge9Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge9Price  
	   {
	    
	     get
		{
		   return surcharge9Price;
		 }
		 set
		 {
		   if(surcharge9Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge9Price",OldValue=surcharge9Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge9Price=value;
		   }
			
		 }
	   }
	  private decimal? surcharge10Price ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge10Price  
	   {
	    
	     get
		{
		   return surcharge10Price;
		 }
		 set
		 {
		   if(surcharge10Price != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge10Price",OldValue=surcharge10Price,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge10Price=value;
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
	  private string destinationPortId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationPortId  
	   {
	    
	     get
		{
		   return destinationPortId;
		 }
		 set
		 {
		   if(destinationPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationPortId",OldValue=destinationPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationPortId=value;
		   }
			
		 }
	   }
	  private string originPortName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginPortName  
	   {
	    
	     get
		{
		   return originPortName;
		 }
		 set
		 {
		   if(originPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginPortName",OldValue=originPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originPortName=value;
		   }
			
		 }
	   }
	  private string originPortCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginPortCode  
	   {
	    
	     get
		{
		   return originPortCode;
		 }
		 set
		 {
		   if(originPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginPortCode",OldValue=originPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originPortCode=value;
		   }
			
		 }
	   }
	  private string destinationPortName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationPortName  
	   {
	    
	     get
		{
		   return destinationPortName;
		 }
		 set
		 {
		   if(destinationPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationPortName",OldValue=destinationPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationPortName=value;
		   }
			
		 }
	   }
	  private string destinationPortCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationPortCode  
	   {
	    
	     get
		{
		   return destinationPortCode;
		 }
		 set
		 {
		   if(destinationPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationPortCode",OldValue=destinationPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationPortCode=value;
		   }
			
		 }
	   }
	  private string originPortText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginPortText  
	   {
	    
	     get
		{
		   return originPortText;
		 }
		 set
		 {
		   if(originPortText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginPortText",OldValue=originPortText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originPortText=value;
		   }
			
		 }
	   }
	  private string destinationPortText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationPortText  
	   {
	    
	     get
		{
		   return destinationPortText;
		 }
		 set
		 {
		   if(destinationPortText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationPortText",OldValue=destinationPortText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationPortText=value;
		   }
			
		 }
	   }
	  private string minPriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string MinPriceText  
	   {
	    
	     get
		{
		   return minPriceText;
		 }
		 set
		 {
		   if(minPriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MinPriceText",OldValue=minPriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   minPriceText=value;
		   }
			
		 }
	   }
	  private string step1PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Step1PriceText  
	   {
	    
	     get
		{
		   return step1PriceText;
		 }
		 set
		 {
		   if(step1PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step1PriceText",OldValue=step1PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   step1PriceText=value;
		   }
			
		 }
	   }
	  private string step2PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Step2PriceText  
	   {
	    
	     get
		{
		   return step2PriceText;
		 }
		 set
		 {
		   if(step2PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step2PriceText",OldValue=step2PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   step2PriceText=value;
		   }
			
		 }
	   }
	  private string step3PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Step3PriceText  
	   {
	    
	     get
		{
		   return step3PriceText;
		 }
		 set
		 {
		   if(step3PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step3PriceText",OldValue=step3PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   step3PriceText=value;
		   }
			
		 }
	   }
	  private string step4PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Step4PriceText  
	   {
	    
	     get
		{
		   return step4PriceText;
		 }
		 set
		 {
		   if(step4PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step4PriceText",OldValue=step4PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   step4PriceText=value;
		   }
			
		 }
	   }
	  private string step5PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Step5PriceText  
	   {
	    
	     get
		{
		   return step5PriceText;
		 }
		 set
		 {
		   if(step5PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step5PriceText",OldValue=step5PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   step5PriceText=value;
		   }
			
		 }
	   }
	  private string step6PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Step6PriceText  
	   {
	    
	     get
		{
		   return step6PriceText;
		 }
		 set
		 {
		   if(step6PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step6PriceText",OldValue=step6PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   step6PriceText=value;
		   }
			
		 }
	   }
	  private string step7PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Step7PriceText  
	   {
	    
	     get
		{
		   return step7PriceText;
		 }
		 set
		 {
		   if(step7PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step7PriceText",OldValue=step7PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   step7PriceText=value;
		   }
			
		 }
	   }
	  private string step8PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Step8PriceText  
	   {
	    
	     get
		{
		   return step8PriceText;
		 }
		 set
		 {
		   if(step8PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Step8PriceText",OldValue=step8PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   step8PriceText=value;
		   }
			
		 }
	   }
	  private string surcharge1PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge1PriceText  
	   {
	    
	     get
		{
		   return surcharge1PriceText;
		 }
		 set
		 {
		   if(surcharge1PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge1PriceText",OldValue=surcharge1PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge1PriceText=value;
		   }
			
		 }
	   }
	  private string surcharge2PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge2PriceText  
	   {
	    
	     get
		{
		   return surcharge2PriceText;
		 }
		 set
		 {
		   if(surcharge2PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge2PriceText",OldValue=surcharge2PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge2PriceText=value;
		   }
			
		 }
	   }
	  private string surcharge3PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge3PriceText  
	   {
	    
	     get
		{
		   return surcharge3PriceText;
		 }
		 set
		 {
		   if(surcharge3PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge3PriceText",OldValue=surcharge3PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge3PriceText=value;
		   }
			
		 }
	   }
	  private string surcharge4PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge4PriceText  
	   {
	    
	     get
		{
		   return surcharge4PriceText;
		 }
		 set
		 {
		   if(surcharge4PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge4PriceText",OldValue=surcharge4PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge4PriceText=value;
		   }
			
		 }
	   }
	  private string surcharge5PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge5PriceText  
	   {
	    
	     get
		{
		   return surcharge5PriceText;
		 }
		 set
		 {
		   if(surcharge5PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge5PriceText",OldValue=surcharge5PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge5PriceText=value;
		   }
			
		 }
	   }
	  private string surcharge6PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge6PriceText  
	   {
	    
	     get
		{
		   return surcharge6PriceText;
		 }
		 set
		 {
		   if(surcharge6PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge6PriceText",OldValue=surcharge6PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge6PriceText=value;
		   }
			
		 }
	   }
	  private string surcharge7PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge7PriceText  
	   {
	    
	     get
		{
		   return surcharge7PriceText;
		 }
		 set
		 {
		   if(surcharge7PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge7PriceText",OldValue=surcharge7PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge7PriceText=value;
		   }
			
		 }
	   }
	  private string surcharge8PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge8PriceText  
	   {
	    
	     get
		{
		   return surcharge8PriceText;
		 }
		 set
		 {
		   if(surcharge8PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge8PriceText",OldValue=surcharge8PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge8PriceText=value;
		   }
			
		 }
	   }
	  private string surcharge9PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge9PriceText  
	   {
	    
	     get
		{
		   return surcharge9PriceText;
		 }
		 set
		 {
		   if(surcharge9PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge9PriceText",OldValue=surcharge9PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge9PriceText=value;
		   }
			
		 }
	   }
	  private string surcharge10PriceText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge10PriceText  
	   {
	    
	     get
		{
		   return surcharge10PriceText;
		 }
		 set
		 {
		   if(surcharge10PriceText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge10PriceText",OldValue=surcharge10PriceText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge10PriceText=value;
		   }
			
		 }
	   }
   }
   
}
	 