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
using Amital.QuoteOPM.Def.Validators;
  
namespace Amital.QuoteOPM.Def.EntityPMs
{
   [CustomValidation(typeof(QuoteOPMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class QuoteOPSettingPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private bool copyExchangeRates ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyExchangeRates  
	   {
	    
	     get
		{
		   return copyExchangeRates;
		 }
		 set
		 {
		   if(copyExchangeRates != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyExchangeRates",OldValue=copyExchangeRates,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyExchangeRates=value;
		   }
			
		 }
	   }
	  private int automaticallyCloseDays ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int AutomaticallyCloseDays  
	   {
	    
	     get
		{
		   return automaticallyCloseDays;
		 }
		 set
		 {
		   if(automaticallyCloseDays != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticallyCloseDays",OldValue=automaticallyCloseDays,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   automaticallyCloseDays=value;
		   }
			
		 }
	   }
	  private bool copyShipper ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyShipper  
	   {
	    
	     get
		{
		   return copyShipper;
		 }
		 set
		 {
		   if(copyShipper != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyShipper",OldValue=copyShipper,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyShipper=value;
		   }
			
		 }
	   }
	  private bool copyConsignee ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyConsignee  
	   {
	    
	     get
		{
		   return copyConsignee;
		 }
		 set
		 {
		   if(copyConsignee != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyConsignee",OldValue=copyConsignee,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyConsignee=value;
		   }
			
		 }
	   }
	  private bool copyMainCarriage ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyMainCarriage  
	   {
	    
	     get
		{
		   return copyMainCarriage;
		 }
		 set
		 {
		   if(copyMainCarriage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyMainCarriage",OldValue=copyMainCarriage,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyMainCarriage=value;
		   }
			
		 }
	   }
	  private bool copyPickup ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyPickup  
	   {
	    
	     get
		{
		   return copyPickup;
		 }
		 set
		 {
		   if(copyPickup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyPickup",OldValue=copyPickup,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyPickup=value;
		   }
			
		 }
	   }
	  private bool copyDelivery ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyDelivery  
	   {
	    
	     get
		{
		   return copyDelivery;
		 }
		 set
		 {
		   if(copyDelivery != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyDelivery",OldValue=copyDelivery,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyDelivery=value;
		   }
			
		 }
	   }
	  private bool copyChargesTypes ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyChargesTypes  
	   {
	    
	     get
		{
		   return copyChargesTypes;
		 }
		 set
		 {
		   if(copyChargesTypes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyChargesTypes",OldValue=copyChargesTypes,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyChargesTypes=value;
		   }
			
		 }
	   }
	  private bool copyChargesCost ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyChargesCost  
	   {
	    
	     get
		{
		   return copyChargesCost;
		 }
		 set
		 {
		   if(copyChargesCost != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyChargesCost",OldValue=copyChargesCost,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyChargesCost=value;
		   }
			
		 }
	   }
	  private bool copyChargesSale ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyChargesSale  
	   {
	    
	     get
		{
		   return copyChargesSale;
		 }
		 set
		 {
		   if(copyChargesSale != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyChargesSale",OldValue=copyChargesSale,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyChargesSale=value;
		   }
			
		 }
	   }
	  private bool editMainCarriage ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool EditMainCarriage  
	   {
	    
	     get
		{
		   return editMainCarriage;
		 }
		 set
		 {
		   if(editMainCarriage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EditMainCarriage",OldValue=editMainCarriage,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   editMainCarriage=value;
		   }
			
		 }
	   }
	  private bool copyAgent ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyAgent  
	   {
	    
	     get
		{
		   return copyAgent;
		 }
		 set
		 {
		   if(copyAgent != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyAgent",OldValue=copyAgent,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyAgent=value;
		   }
			
		 }
	   }
	  private bool copyNotify ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CopyNotify  
	   {
	    
	     get
		{
		   return copyNotify;
		 }
		 set
		 {
		   if(copyNotify != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CopyNotify",OldValue=copyNotify,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   copyNotify=value;
		   }
			
		 }
	   }
	  private bool isSaleAsCostCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSaleAsCostCurrency  
	   {
	    
	     get
		{
		   return isSaleAsCostCurrency;
		 }
		 set
		 {
		   if(isSaleAsCostCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSaleAsCostCurrency",OldValue=isSaleAsCostCurrency,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSaleAsCostCurrency=value;
		   }
			
		 }
	   }
	  private bool isMultiCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMultiCurrency  
	   {
	    
	     get
		{
		   return isMultiCurrency;
		 }
		 set
		 {
		   if(isMultiCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMultiCurrency",OldValue=isMultiCurrency,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMultiCurrency=value;
		   }
			
		 }
	   }
	  private int? quoteExpirationDays ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? QuoteExpirationDays  
	   {
	    
	     get
		{
		   return quoteExpirationDays;
		 }
		 set
		 {
		   if(quoteExpirationDays != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteExpirationDays",OldValue=quoteExpirationDays,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   quoteExpirationDays=value;
		   }
			
		 }
	   }
   }
   
}
	 