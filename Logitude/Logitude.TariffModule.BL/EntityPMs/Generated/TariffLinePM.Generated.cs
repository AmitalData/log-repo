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
	  private bool hasErrors ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasErrors  
	   {
	    
	     get
		{
		   return hasErrors;
		 }
		 set
		 {
		   if(hasErrors != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasErrors",OldValue=hasErrors,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasErrors=value;
		   }
			
		 }
	   }
	  private string errorText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ErrorText  
	   {
	    
	     get
		{
		   return errorText;
		 }
		 set
		 {
		   if(errorText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ErrorText",OldValue=errorText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   errorText=value;
		   }
			
		 }
	   }
	  private string lineUniqueKey ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string LineUniqueKey  
	   {
	    
	     get
		{
		   return lineUniqueKey;
		 }
		 set
		 {
		   if(lineUniqueKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineUniqueKey",OldValue=lineUniqueKey,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lineUniqueKey=value;
		   }
			
		 }
	   }
	  private string lineUniqueKeyText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string LineUniqueKeyText  
	   {
	    
	     get
		{
		   return lineUniqueKeyText;
		 }
		 set
		 {
		   if(lineUniqueKeyText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineUniqueKeyText",OldValue=lineUniqueKeyText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lineUniqueKeyText=value;
		   }
			
		 }
	   }
	  private int index ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int Index  
	   {
	    
	     get
		{
		   return index;
		 }
		 set
		 {
		   if(index != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Index",OldValue=index,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   index=value;
		   }
			
		 }
	   }
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notes  
	   {
	    
	     get
		{
		   return notes;
		 }
		 set
		 {
		   if(notes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notes",OldValue=notes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notes=value;
		   }
			
		 }
	   }
	  private bool addedManually ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AddedManually  
	   {
	    
	     get
		{
		   return addedManually;
		 }
		 set
		 {
		   if(addedManually != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AddedManually",OldValue=addedManually,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   addedManually=value;
		   }
			
		 }
	   }
	  private bool isFromAllOtherPorts ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsFromAllOtherPorts  
	   {
	    
	     get
		{
		   return isFromAllOtherPorts;
		 }
		 set
		 {
		   if(isFromAllOtherPorts != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFromAllOtherPorts",OldValue=isFromAllOtherPorts,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isFromAllOtherPorts=value;
		   }
			
		 }
	   }
	  private bool isToAllOtherPorts ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsToAllOtherPorts  
	   {
	    
	     get
		{
		   return isToAllOtherPorts;
		 }
		 set
		 {
		   if(isToAllOtherPorts != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsToAllOtherPorts",OldValue=isToAllOtherPorts,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isToAllOtherPorts=value;
		   }
			
		 }
	   }
	  private decimal? surcharge1MinPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge1MinPrice  
	   {
	    
	     get
		{
		   return surcharge1MinPrice;
		 }
		 set
		 {
		   if(surcharge1MinPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge1MinPrice",OldValue=surcharge1MinPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge1MinPrice=value;
		   }
			
		 }
	   }
	  private decimal? surcharge2MinPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge2MinPrice  
	   {
	    
	     get
		{
		   return surcharge2MinPrice;
		 }
		 set
		 {
		   if(surcharge2MinPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge2MinPrice",OldValue=surcharge2MinPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge2MinPrice=value;
		   }
			
		 }
	   }
	  private decimal? surcharge3MinPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge3MinPrice  
	   {
	    
	     get
		{
		   return surcharge3MinPrice;
		 }
		 set
		 {
		   if(surcharge3MinPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge3MinPrice",OldValue=surcharge3MinPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge3MinPrice=value;
		   }
			
		 }
	   }
	  private decimal? surcharge4MinPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge4MinPrice  
	   {
	    
	     get
		{
		   return surcharge4MinPrice;
		 }
		 set
		 {
		   if(surcharge4MinPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge4MinPrice",OldValue=surcharge4MinPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge4MinPrice=value;
		   }
			
		 }
	   }
	  private decimal? surcharge5MinPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge5MinPrice  
	   {
	    
	     get
		{
		   return surcharge5MinPrice;
		 }
		 set
		 {
		   if(surcharge5MinPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge5MinPrice",OldValue=surcharge5MinPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge5MinPrice=value;
		   }
			
		 }
	   }
	  private decimal? surcharge6MinPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge6MinPrice  
	   {
	    
	     get
		{
		   return surcharge6MinPrice;
		 }
		 set
		 {
		   if(surcharge6MinPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge6MinPrice",OldValue=surcharge6MinPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge6MinPrice=value;
		   }
			
		 }
	   }
	  private decimal? surcharge7MinPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge7MinPrice  
	   {
	    
	     get
		{
		   return surcharge7MinPrice;
		 }
		 set
		 {
		   if(surcharge7MinPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge7MinPrice",OldValue=surcharge7MinPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge7MinPrice=value;
		   }
			
		 }
	   }
	  private decimal? surcharge8MinPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge8MinPrice  
	   {
	    
	     get
		{
		   return surcharge8MinPrice;
		 }
		 set
		 {
		   if(surcharge8MinPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge8MinPrice",OldValue=surcharge8MinPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge8MinPrice=value;
		   }
			
		 }
	   }
	  private decimal? surcharge9MinPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge9MinPrice  
	   {
	    
	     get
		{
		   return surcharge9MinPrice;
		 }
		 set
		 {
		   if(surcharge9MinPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge9MinPrice",OldValue=surcharge9MinPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge9MinPrice=value;
		   }
			
		 }
	   }
	  private decimal? surcharge10MinPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Surcharge10MinPrice  
	   {
	    
	     get
		{
		   return surcharge10MinPrice;
		 }
		 set
		 {
		   if(surcharge10MinPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge10MinPrice",OldValue=surcharge10MinPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   surcharge10MinPrice=value;
		   }
			
		 }
	   }
	  private string currencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyId  
	   {
	    
	     get
		{
		   return currencyId;
		 }
		 set
		 {
		   if(currencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyId",OldValue=currencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyId=value;
		   }
			
		 }
	   }
	  private string currencyCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CurrencyCode  
	   {
	    
	     get
		{
		   return currencyCode;
		 }
		 set
		 {
		   if(currencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CurrencyCode",OldValue=currencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   currencyCode=value;
		   }
			
		 }
	   }

	   private List<TariffLinesContainersPricePM> containersPrices;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("TariffLineTariffLinesContainersPrice", "Id","TariffLineId")]
	   [DataMember]
	   public virtual List<TariffLinesContainersPricePM> ContainersPrices  
	   {
	        get
             {
                 if (containersPrices == null)
                 {
                     containersPrices = new List<TariffLinesContainersPricePM>();
                 }
                 return containersPrices;
              }
             set { containersPrices = value; }
	    }
		   
	   private List<TariffLinesContainersPricePM>  deletedContainersPrices;
	   public virtual List<TariffLinesContainersPricePM> DeletedContainersPrices  
	   {
	        get
             {
                 if ( deletedContainersPrices == null)
                 {
                      deletedContainersPrices = new List<TariffLinesContainersPricePM>();
                 }
                 return  deletedContainersPrices;
              }
             set {  deletedContainersPrices = value; }
	    }
	  	  private string originPortCombinedCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginPortCombinedCode  
	   {
	    
	     get
		{
		   return originPortCombinedCode;
		 }
		 set
		 {
		   if(originPortCombinedCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginPortCombinedCode",OldValue=originPortCombinedCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originPortCombinedCode=value;
		   }
			
		 }
	   }
	  private string destinationPortCombinedCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationPortCombinedCode  
	   {
	    
	     get
		{
		   return destinationPortCombinedCode;
		 }
		 set
		 {
		   if(destinationPortCombinedCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationPortCombinedCode",OldValue=destinationPortCombinedCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationPortCombinedCode=value;
		   }
			
		 }
	   }
	  private string transitTime ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransitTime  
	   {
	    
	     get
		{
		   return transitTime;
		 }
		 set
		 {
		   if(transitTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransitTime",OldValue=transitTime,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transitTime=value;
		   }
			
		 }
	   }
	  private bool originPortHasWrongTransMode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool OriginPortHasWrongTransMode  
	   {
	    
	     get
		{
		   return originPortHasWrongTransMode;
		 }
		 set
		 {
		   if(originPortHasWrongTransMode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginPortHasWrongTransMode",OldValue=originPortHasWrongTransMode,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   originPortHasWrongTransMode=value;
		   }
			
		 }
	   }
	  private bool destinationPortHasWrongTransMode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool DestinationPortHasWrongTransMode  
	   {
	    
	     get
		{
		   return destinationPortHasWrongTransMode;
		 }
		 set
		 {
		   if(destinationPortHasWrongTransMode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationPortHasWrongTransMode",OldValue=destinationPortHasWrongTransMode,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   destinationPortHasWrongTransMode=value;
		   }
			
		 }
	   }
	  private bool isMinPriceMinus ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMinPriceMinus  
	   {
	    
	     get
		{
		   return isMinPriceMinus;
		 }
		 set
		 {
		   if(isMinPriceMinus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMinPriceMinus",OldValue=isMinPriceMinus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMinPriceMinus=value;
		   }
			
		 }
	   }
	  private bool isPrice1Minus ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPrice1Minus  
	   {
	    
	     get
		{
		   return isPrice1Minus;
		 }
		 set
		 {
		   if(isPrice1Minus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPrice1Minus",OldValue=isPrice1Minus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPrice1Minus=value;
		   }
			
		 }
	   }
	  private bool isPrice2Minus ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPrice2Minus  
	   {
	    
	     get
		{
		   return isPrice2Minus;
		 }
		 set
		 {
		   if(isPrice2Minus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPrice2Minus",OldValue=isPrice2Minus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPrice2Minus=value;
		   }
			
		 }
	   }
	  private bool isPrice3Minus ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPrice3Minus  
	   {
	    
	     get
		{
		   return isPrice3Minus;
		 }
		 set
		 {
		   if(isPrice3Minus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPrice3Minus",OldValue=isPrice3Minus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPrice3Minus=value;
		   }
			
		 }
	   }
	  private bool isPrice4Minus ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPrice4Minus  
	   {
	    
	     get
		{
		   return isPrice4Minus;
		 }
		 set
		 {
		   if(isPrice4Minus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPrice4Minus",OldValue=isPrice4Minus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPrice4Minus=value;
		   }
			
		 }
	   }
	  private bool isPrice5Minus ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPrice5Minus  
	   {
	    
	     get
		{
		   return isPrice5Minus;
		 }
		 set
		 {
		   if(isPrice5Minus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPrice5Minus",OldValue=isPrice5Minus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPrice5Minus=value;
		   }
			
		 }
	   }
	  private bool isPrice6Minus ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPrice6Minus  
	   {
	    
	     get
		{
		   return isPrice6Minus;
		 }
		 set
		 {
		   if(isPrice6Minus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPrice6Minus",OldValue=isPrice6Minus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPrice6Minus=value;
		   }
			
		 }
	   }
	  private bool isPrice7Minus ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPrice7Minus  
	   {
	    
	     get
		{
		   return isPrice7Minus;
		 }
		 set
		 {
		   if(isPrice7Minus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPrice7Minus",OldValue=isPrice7Minus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPrice7Minus=value;
		   }
			
		 }
	   }
	  private bool isPrice8Minus ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPrice8Minus  
	   {
	    
	     get
		{
		   return isPrice8Minus;
		 }
		 set
		 {
		   if(isPrice8Minus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPrice8Minus",OldValue=isPrice8Minus,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPrice8Minus=value;
		   }
			
		 }
	   }
	  private bool lineEdited ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool LineEdited  
	   {
	    
	     get
		{
		   return lineEdited;
		 }
		 set
		 {
		   if(lineEdited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineEdited",OldValue=lineEdited,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   lineEdited=value;
		   }
			
		 }
	   }
	  private bool isDifferentCurrenciesPerCharge ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDifferentCurrenciesPerCharge  
	   {
	    
	     get
		{
		   return isDifferentCurrenciesPerCharge;
		 }
		 set
		 {
		   if(isDifferentCurrenciesPerCharge != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDifferentCurrenciesPerCharge",OldValue=isDifferentCurrenciesPerCharge,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDifferentCurrenciesPerCharge=value;
		   }
			
		 }
	   }
	  private string surcharge1CurrencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge1CurrencyId  
	   {
	    
	     get
		{
		   return surcharge1CurrencyId;
		 }
		 set
		 {
		   if(surcharge1CurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge1CurrencyId",OldValue=surcharge1CurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge1CurrencyId=value;
		   }
			
		 }
	   }
	  private string surcharge2CurrencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge2CurrencyId  
	   {
	    
	     get
		{
		   return surcharge2CurrencyId;
		 }
		 set
		 {
		   if(surcharge2CurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge2CurrencyId",OldValue=surcharge2CurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge2CurrencyId=value;
		   }
			
		 }
	   }
	  private string surcharge3CurrencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge3CurrencyId  
	   {
	    
	     get
		{
		   return surcharge3CurrencyId;
		 }
		 set
		 {
		   if(surcharge3CurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge3CurrencyId",OldValue=surcharge3CurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge3CurrencyId=value;
		   }
			
		 }
	   }
	  private string surcharge4CurrencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge4CurrencyId  
	   {
	    
	     get
		{
		   return surcharge4CurrencyId;
		 }
		 set
		 {
		   if(surcharge4CurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge4CurrencyId",OldValue=surcharge4CurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge4CurrencyId=value;
		   }
			
		 }
	   }
	  private string surcharge5CurrencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge5CurrencyId  
	   {
	    
	     get
		{
		   return surcharge5CurrencyId;
		 }
		 set
		 {
		   if(surcharge5CurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge5CurrencyId",OldValue=surcharge5CurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge5CurrencyId=value;
		   }
			
		 }
	   }
	  private string surcharge6CurrencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge6CurrencyId  
	   {
	    
	     get
		{
		   return surcharge6CurrencyId;
		 }
		 set
		 {
		   if(surcharge6CurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge6CurrencyId",OldValue=surcharge6CurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge6CurrencyId=value;
		   }
			
		 }
	   }
	  private string surcharge7CurrencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge7CurrencyId  
	   {
	    
	     get
		{
		   return surcharge7CurrencyId;
		 }
		 set
		 {
		   if(surcharge7CurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge7CurrencyId",OldValue=surcharge7CurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge7CurrencyId=value;
		   }
			
		 }
	   }
	  private string surcharge8CurrencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge8CurrencyId  
	   {
	    
	     get
		{
		   return surcharge8CurrencyId;
		 }
		 set
		 {
		   if(surcharge8CurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge8CurrencyId",OldValue=surcharge8CurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge8CurrencyId=value;
		   }
			
		 }
	   }
	  private string surcharge9CurrencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge9CurrencyId  
	   {
	    
	     get
		{
		   return surcharge9CurrencyId;
		 }
		 set
		 {
		   if(surcharge9CurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge9CurrencyId",OldValue=surcharge9CurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge9CurrencyId=value;
		   }
			
		 }
	   }
	  private string surcharge10CurrencyId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge10CurrencyId  
	   {
	    
	     get
		{
		   return surcharge10CurrencyId;
		 }
		 set
		 {
		   if(surcharge10CurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge10CurrencyId",OldValue=surcharge10CurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge10CurrencyId=value;
		   }
			
		 }
	   }
	  private string viaPortId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ViaPortId  
	   {
	    
	     get
		{
		   return viaPortId;
		 }
		 set
		 {
		   if(viaPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ViaPortId",OldValue=viaPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   viaPortId=value;
		   }
			
		 }
	   }
	  private string viaPortText ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ViaPortText  
	   {
	    
	     get
		{
		   return viaPortText;
		 }
		 set
		 {
		   if(viaPortText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ViaPortText",OldValue=viaPortText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   viaPortText=value;
		   }
			
		 }
	   }
	  private string viaPortName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ViaPortName  
	   {
	    
	     get
		{
		   return viaPortName;
		 }
		 set
		 {
		   if(viaPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ViaPortName",OldValue=viaPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   viaPortName=value;
		   }
			
		 }
	   }
	  private string viaPortCombinedCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ViaPortCombinedCode  
	   {
	    
	     get
		{
		   return viaPortCombinedCode;
		 }
		 set
		 {
		   if(viaPortCombinedCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ViaPortCombinedCode",OldValue=viaPortCombinedCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   viaPortCombinedCode=value;
		   }
			
		 }
	   }
	  private string viaPortCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ViaPortCode  
	   {
	    
	     get
		{
		   return viaPortCode;
		 }
		 set
		 {
		   if(viaPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ViaPortCode",OldValue=viaPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   viaPortCode=value;
		   }
			
		 }
	   }
	  private bool viaPortHasWrongTransMode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ViaPortHasWrongTransMode  
	   {
	    
	     get
		{
		   return viaPortHasWrongTransMode;
		 }
		 set
		 {
		   if(viaPortHasWrongTransMode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ViaPortHasWrongTransMode",OldValue=viaPortHasWrongTransMode,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   viaPortHasWrongTransMode=value;
		   }
			
		 }
	   }
	  private string fromCountryId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromCountryId  
	   {
	    
	     get
		{
		   return fromCountryId;
		 }
		 set
		 {
		   if(fromCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromCountryId",OldValue=fromCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromCountryId=value;
		   }
			
		 }
	   }
	  private string toCountryId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToCountryId  
	   {
	    
	     get
		{
		   return toCountryId;
		 }
		 set
		 {
		   if(toCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToCountryId",OldValue=toCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toCountryId=value;
		   }
			
		 }
	   }
	  private string fromCountryCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromCountryCode  
	   {
	    
	     get
		{
		   return fromCountryCode;
		 }
		 set
		 {
		   if(fromCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromCountryCode",OldValue=fromCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromCountryCode=value;
		   }
			
		 }
	   }
	  private string toCountryCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToCountryCode  
	   {
	    
	     get
		{
		   return toCountryCode;
		 }
		 set
		 {
		   if(toCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToCountryCode",OldValue=toCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toCountryCode=value;
		   }
			
		 }
	   }
	  private string fromCountryName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromCountryName  
	   {
	    
	     get
		{
		   return fromCountryName;
		 }
		 set
		 {
		   if(fromCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromCountryName",OldValue=fromCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromCountryName=value;
		   }
			
		 }
	   }
	  private string toCountryName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToCountryName  
	   {
	    
	     get
		{
		   return toCountryName;
		 }
		 set
		 {
		   if(toCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToCountryName",OldValue=toCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toCountryName=value;
		   }
			
		 }
	   }
	  private bool isFromAllOtherCountries ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsFromAllOtherCountries  
	   {
	    
	     get
		{
		   return isFromAllOtherCountries;
		 }
		 set
		 {
		   if(isFromAllOtherCountries != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFromAllOtherCountries",OldValue=isFromAllOtherCountries,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isFromAllOtherCountries=value;
		   }
			
		 }
	   }
	  private bool isToAllOtherCountries ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsToAllOtherCountries  
	   {
	    
	     get
		{
		   return isToAllOtherCountries;
		 }
		 set
		 {
		   if(isToAllOtherCountries != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsToAllOtherCountries",OldValue=isToAllOtherCountries,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isToAllOtherCountries=value;
		   }
			
		 }
	   }
	  private string unitOfMeasurementCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string UnitOfMeasurementCode  
	   {
	    
	     get
		{
		   return unitOfMeasurementCode;
		 }
		 set
		 {
		   if(unitOfMeasurementCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UnitOfMeasurementCode",OldValue=unitOfMeasurementCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   unitOfMeasurementCode=value;
		   }
			
		 }
	   }
   }
   
}
	 