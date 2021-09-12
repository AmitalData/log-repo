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
   public partial class QuoteOPPM : EntityPM
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
	  private string quoteTemplateId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteTemplateId  
	   {
	    
	     get
		{
		   return quoteTemplateId;
		 }
		 set
		 {
		   if(quoteTemplateId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteTemplateId",OldValue=quoteTemplateId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteTemplateId=value;
		   }
			
		 }
	   }
	  private string concurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConcurrencyGUID  
	   {
	    
	     get
		{
		   return concurrencyGUID;
		 }
		 set
		 {
		   if(concurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConcurrencyGUID",OldValue=concurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   concurrencyGUID=value;
		   }
			
		 }
	   }
	  private int lastVersionNumber ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int LastVersionNumber  
	   {
	    
	     get
		{
		   return lastVersionNumber;
		 }
		 set
		 {
		   if(lastVersionNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastVersionNumber",OldValue=lastVersionNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   lastVersionNumber=value;
		   }
			
		 }
	   }
	  private string freelancerId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreelancerId  
	   {
	    
	     get
		{
		   return freelancerId;
		 }
		 set
		 {
		   if(freelancerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreelancerId",OldValue=freelancerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freelancerId=value;
		   }
			
		 }
	   }
	  private string freelancerAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreelancerAddressId  
	   {
	    
	     get
		{
		   return freelancerAddressId;
		 }
		 set
		 {
		   if(freelancerAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreelancerAddressId",OldValue=freelancerAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freelancerAddressId=value;
		   }
			
		 }
	   }
	  private string freelancerContactId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreelancerContactId  
	   {
	    
	     get
		{
		   return freelancerContactId;
		 }
		 set
		 {
		   if(freelancerContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreelancerContactId",OldValue=freelancerContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freelancerContactId=value;
		   }
			
		 }
	   }
	  private string lastModified ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastModified  
	   {
	    
	     get
		{
		   return lastModified;
		 }
		 set
		 {
		   if(lastModified != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastModified",OldValue=lastModified,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastModified=value;
		   }
			
		 }
	   }
	  private string field1 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field1  
	   {
	    
	     get
		{
		   return field1;
		 }
		 set
		 {
		   if(field1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field1",OldValue=field1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field1=value;
		   }
			
		 }
	   }
	  private string field2 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field2  
	   {
	    
	     get
		{
		   return field2;
		 }
		 set
		 {
		   if(field2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field2",OldValue=field2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field2=value;
		   }
			
		 }
	   }
	  private string field3 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field3  
	   {
	    
	     get
		{
		   return field3;
		 }
		 set
		 {
		   if(field3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field3",OldValue=field3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field3=value;
		   }
			
		 }
	   }
	  private string field4 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field4  
	   {
	    
	     get
		{
		   return field4;
		 }
		 set
		 {
		   if(field4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field4",OldValue=field4,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field4=value;
		   }
			
		 }
	   }
	  private string field5 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field5  
	   {
	    
	     get
		{
		   return field5;
		 }
		 set
		 {
		   if(field5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field5",OldValue=field5,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field5=value;
		   }
			
		 }
	   }
	  private string field6 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field6  
	   {
	    
	     get
		{
		   return field6;
		 }
		 set
		 {
		   if(field6 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field6",OldValue=field6,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field6=value;
		   }
			
		 }
	   }
	  private string field7 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field7  
	   {
	    
	     get
		{
		   return field7;
		 }
		 set
		 {
		   if(field7 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field7",OldValue=field7,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field7=value;
		   }
			
		 }
	   }
	  private string field8 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field8  
	   {
	    
	     get
		{
		   return field8;
		 }
		 set
		 {
		   if(field8 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field8",OldValue=field8,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field8=value;
		   }
			
		 }
	   }
	  private string field9 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field9  
	   {
	    
	     get
		{
		   return field9;
		 }
		 set
		 {
		   if(field9 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field9",OldValue=field9,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field9=value;
		   }
			
		 }
	   }
	  private string field10 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field10  
	   {
	    
	     get
		{
		   return field10;
		 }
		 set
		 {
		   if(field10 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field10",OldValue=field10,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field10=value;
		   }
			
		 }
	   }
	  private bool isByKG ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsByKG  
	   {
	    
	     get
		{
		   return isByKG;
		 }
		 set
		 {
		   if(isByKG != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsByKG",OldValue=isByKG,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isByKG=value;
		   }
			
		 }
	   }
	  private bool isByContainer ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsByContainer  
	   {
	    
	     get
		{
		   return isByContainer;
		 }
		 set
		 {
		   if(isByContainer != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsByContainer",OldValue=isByContainer,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isByContainer=value;
		   }
			
		 }
	   }
	  private bool estimateProfitEdited ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool EstimateProfitEdited  
	   {
	    
	     get
		{
		   return estimateProfitEdited;
		 }
		 set
		 {
		   if(estimateProfitEdited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimateProfitEdited",OldValue=estimateProfitEdited,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   estimateProfitEdited=value;
		   }
			
		 }
	   }
	  private string opportunityId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpportunityId  
	   {
	    
	     get
		{
		   return opportunityId;
		 }
		 set
		 {
		   if(opportunityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpportunityId",OldValue=opportunityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   opportunityId=value;
		   }
			
		 }
	   }
	  private DateTime? lastStageDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastStageDate  
	   {
	    
	     get
		{
		   return lastStageDate;
		 }
		 set
		 {
		   if(lastStageDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastStageDate",OldValue=lastStageDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastStageDate=value;
		   }
			
		 }
	   }
	  private string agentReference1 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentReference1  
	   {
	    
	     get
		{
		   return agentReference1;
		 }
		 set
		 {
		   if(agentReference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentReference1",OldValue=agentReference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentReference1=value;
		   }
			
		 }
	   }
	  private string agentReference2 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentReference2  
	   {
	    
	     get
		{
		   return agentReference2;
		 }
		 set
		 {
		   if(agentReference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentReference2",OldValue=agentReference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentReference2=value;
		   }
			
		 }
	   }
	  private bool isSaleCurrencySameAsCost ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSaleCurrencySameAsCost  
	   {
	    
	     get
		{
		   return isSaleCurrencySameAsCost;
		 }
		 set
		 {
		   if(isSaleCurrencySameAsCost != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSaleCurrencySameAsCost",OldValue=isSaleCurrencySameAsCost,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSaleCurrencySameAsCost=value;
		   }
			
		 }
	   }
	  private double? estimateProfit ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? EstimateProfit  
	   {
	    
	     get
		{
		   return estimateProfit;
		 }
		 set
		 {
		   if(estimateProfit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimateProfit",OldValue=estimateProfit,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   estimateProfit=value;
		   }
			
		 }
	   }
	  private bool isFixedPrice ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsFixedPrice  
	   {
	    
	     get
		{
		   return isFixedPrice;
		 }
		 set
		 {
		   if(isFixedPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFixedPrice",OldValue=isFixedPrice,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isFixedPrice=value;
		   }
			
		 }
	   }
	  private string customerContactId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerContactId  
	   {
	    
	     get
		{
		   return customerContactId;
		 }
		 set
		 {
		   if(customerContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerContactId",OldValue=customerContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerContactId=value;
		   }
			
		 }
	   }
	  private double costTotalAmountInLocalCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostTotalAmountInLocalCurrency  
	   {
	    
	     get
		{
		   return costTotalAmountInLocalCurrency;
		 }
		 set
		 {
		   if(costTotalAmountInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostTotalAmountInLocalCurrency",OldValue=costTotalAmountInLocalCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costTotalAmountInLocalCurrency=value;
		   }
			
		 }
	   }
	  private double saleTotalAmountInLocalCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double SaleTotalAmountInLocalCurrency  
	   {
	    
	     get
		{
		   return saleTotalAmountInLocalCurrency;
		 }
		 set
		 {
		   if(saleTotalAmountInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleTotalAmountInLocalCurrency",OldValue=saleTotalAmountInLocalCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   saleTotalAmountInLocalCurrency=value;
		   }
			
		 }
	   }
	  private double costTotalAmountInSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double CostTotalAmountInSaleCurrency  
	   {
	    
	     get
		{
		   return costTotalAmountInSaleCurrency;
		 }
		 set
		 {
		   if(costTotalAmountInSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostTotalAmountInSaleCurrency",OldValue=costTotalAmountInSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   costTotalAmountInSaleCurrency=value;
		   }
			
		 }
	   }
	  private double? saleTotalAmountInSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? SaleTotalAmountInSaleCurrency  
	   {
	    
	     get
		{
		   return saleTotalAmountInSaleCurrency;
		 }
		 set
		 {
		   if(saleTotalAmountInSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleTotalAmountInSaleCurrency",OldValue=saleTotalAmountInSaleCurrency,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   saleTotalAmountInSaleCurrency=value;
		   }
			
		 }
	   }
	  private double estimateProfitInSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double EstimateProfitInSaleCurrency  
	   {
	    
	     get
		{
		   return estimateProfitInSaleCurrency;
		 }
		 set
		 {
		   if(estimateProfitInSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimateProfitInSaleCurrency",OldValue=estimateProfitInSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   estimateProfitInSaleCurrency=value;
		   }
			
		 }
	   }
	  private string shipperName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperName  
	   {
	    
	     get
		{
		   return shipperName;
		 }
		 set
		 {
		   if(shipperName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperName",OldValue=shipperName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperName=value;
		   }
			
		 }
	   }
	  private string consigneeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeName  
	   {
	    
	     get
		{
		   return consigneeName;
		 }
		 set
		 {
		   if(consigneeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeName",OldValue=consigneeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeName=value;
		   }
			
		 }
	   }
	  private string deliveryAddress ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliveryAddress  
	   {
	    
	     get
		{
		   return deliveryAddress;
		 }
		 set
		 {
		   if(deliveryAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveryAddress",OldValue=deliveryAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliveryAddress=value;
		   }
			
		 }
	   }
	  private string pickUpAddress ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PickUpAddress  
	   {
	    
	     get
		{
		   return pickUpAddress;
		 }
		 set
		 {
		   if(pickUpAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickUpAddress",OldValue=pickUpAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pickUpAddress=value;
		   }
			
		 }
	   }
	  private string saleCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SaleCurrencyId  
	   {
	    
	     get
		{
		   return saleCurrencyId;
		 }
		 set
		 {
		   if(saleCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleCurrencyId",OldValue=saleCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   saleCurrencyId=value;
		   }
			
		 }
	   }
	  private double? exchangeRate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ExchangeRate  
	   {
	    
	     get
		{
		   return exchangeRate;
		 }
		 set
		 {
		   if(exchangeRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExchangeRate",OldValue=exchangeRate,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   exchangeRate=value;
		   }
			
		 }
	   }
	  private string customerName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerName  
	   {
	    
	     get
		{
		   return customerName;
		 }
		 set
		 {
		   if(customerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerName",OldValue=customerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerName=value;
		   }
			
		 }
	   }
	  private string customerNote ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerNote  
	   {
	    
	     get
		{
		   return customerNote;
		 }
		 set
		 {
		   if(customerNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerNote",OldValue=customerNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerNote=value;
		   }
			
		 }
	   }
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCancelled  
	   {
	    
	     get
		{
		   return isCancelled;
		 }
		 set
		 {
		   if(isCancelled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancelled",OldValue=isCancelled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCancelled=value;
		   }
			
		 }
	   }

	   private List<QuoteOPCostChargePM> quoteCostCharges;
	 
		     
	   [Include]
	   [Association("QuoteOPCostChargePMs", "Id","QuoteCostCharges")]
	   [DataMember]
	   public virtual List<QuoteOPCostChargePM> QuoteCostCharges  
	   {
	        get
             {
                 if (quoteCostCharges == null)
                 {
                     quoteCostCharges = new List<QuoteOPCostChargePM>();
                 }
                 return quoteCostCharges;
              }
             set { quoteCostCharges = value; }
	    }
		   
	   private List<QuoteOPCostChargePM>  deletedQuoteCostCharges;
	   public virtual List<QuoteOPCostChargePM> DeletedQuoteCostCharges  
	   {
	        get
             {
                 if ( deletedQuoteCostCharges == null)
                 {
                      deletedQuoteCostCharges = new List<QuoteOPCostChargePM>();
                 }
                 return  deletedQuoteCostCharges;
              }
             set {  deletedQuoteCostCharges = value; }
	    }
	  
	   private List<QuoteOPSaleChargePM> quoteSaleCharges;
	 
		     
	   [Include]
	   [Association("QuoteOPSaleChargePMs", "Id","QuoteOPId")]
	   [DataMember]
	   public virtual List<QuoteOPSaleChargePM> QuoteSaleCharges  
	   {
	        get
             {
                 if (quoteSaleCharges == null)
                 {
                     quoteSaleCharges = new List<QuoteOPSaleChargePM>();
                 }
                 return quoteSaleCharges;
              }
             set { quoteSaleCharges = value; }
	    }
		   
	   private List<QuoteOPSaleChargePM>  deletedQuoteSaleCharges;
	   public virtual List<QuoteOPSaleChargePM> DeletedQuoteSaleCharges  
	   {
	        get
             {
                 if ( deletedQuoteSaleCharges == null)
                 {
                      deletedQuoteSaleCharges = new List<QuoteOPSaleChargePM>();
                 }
                 return  deletedQuoteSaleCharges;
              }
             set {  deletedQuoteSaleCharges = value; }
	    }
	  	  private double totalReceivablesAmount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double TotalReceivablesAmount  
	   {
	    
	     get
		{
		   return totalReceivablesAmount;
		 }
		 set
		 {
		   if(totalReceivablesAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalReceivablesAmount",OldValue=totalReceivablesAmount,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   totalReceivablesAmount=value;
		   }
			
		 }
	   }
	  private string quoteNumber ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteNumber  
	   {
	    
	     get
		{
		   return quoteNumber;
		 }
		 set
		 {
		   if(quoteNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteNumber",OldValue=quoteNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteNumber=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierId  
	   {
	    
	     get
		{
		   return mainCarriageCarrierId;
		 }
		 set
		 {
		   if(mainCarriageCarrierId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierId",OldValue=mainCarriageCarrierId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierId=value;
		   }
			
		 }
	   }
	  private string directionId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DirectionId  
	   {
	    
	     get
		{
		   return directionId;
		 }
		 set
		 {
		   if(directionId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DirectionId",OldValue=directionId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   directionId=value;
		   }
			
		 }
	   }
	  private string transportModeId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeId  
	   {
	    
	     get
		{
		   return transportModeId;
		 }
		 set
		 {
		   if(transportModeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeId",OldValue=transportModeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeId=value;
		   }
			
		 }
	   }
	  private string departmentId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepartmentId  
	   {
	    
	     get
		{
		   return departmentId;
		 }
		 set
		 {
		   if(departmentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartmentId",OldValue=departmentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   departmentId=value;
		   }
			
		 }
	   }
	  private string branchId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BranchId  
	   {
	    
	     get
		{
		   return branchId;
		 }
		 set
		 {
		   if(branchId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BranchId",OldValue=branchId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   branchId=value;
		   }
			
		 }
	   }
	  private string shipmentTypeId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentTypeId  
	   {
	    
	     get
		{
		   return shipmentTypeId;
		 }
		 set
		 {
		   if(shipmentTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentTypeId",OldValue=shipmentTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentTypeId=value;
		   }
			
		 }
	   }
	  private string quoteCustomerTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteCustomerTypeCode  
	   {
	    
	     get
		{
		   return quoteCustomerTypeCode;
		 }
		 set
		 {
		   if(quoteCustomerTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteCustomerTypeCode",OldValue=quoteCustomerTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteCustomerTypeCode=value;
		   }
			
		 }
	   }
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerId  
	   {
	    
	     get
		{
		   return customerId;
		 }
		 set
		 {
		   if(customerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerId",OldValue=customerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerId=value;
		   }
			
		 }
	   }
	  private string shipperId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperId  
	   {
	    
	     get
		{
		   return shipperId;
		 }
		 set
		 {
		   if(shipperId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperId",OldValue=shipperId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperId=value;
		   }
			
		 }
	   }
	  private string shipperContactId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperContactId  
	   {
	    
	     get
		{
		   return shipperContactId;
		 }
		 set
		 {
		   if(shipperContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperContactId",OldValue=shipperContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperContactId=value;
		   }
			
		 }
	   }
	  private string shipperReference1 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperReference1  
	   {
	    
	     get
		{
		   return shipperReference1;
		 }
		 set
		 {
		   if(shipperReference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperReference1",OldValue=shipperReference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperReference1=value;
		   }
			
		 }
	   }
	  private string shipperReference2 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperReference2  
	   {
	    
	     get
		{
		   return shipperReference2;
		 }
		 set
		 {
		   if(shipperReference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperReference2",OldValue=shipperReference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperReference2=value;
		   }
			
		 }
	   }
	  private string consigneeId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeId  
	   {
	    
	     get
		{
		   return consigneeId;
		 }
		 set
		 {
		   if(consigneeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeId",OldValue=consigneeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeId=value;
		   }
			
		 }
	   }
	  private string consigneeContactId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeContactId  
	   {
	    
	     get
		{
		   return consigneeContactId;
		 }
		 set
		 {
		   if(consigneeContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeContactId",OldValue=consigneeContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeContactId=value;
		   }
			
		 }
	   }
	  private string consigneeReference1 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeReference1  
	   {
	    
	     get
		{
		   return consigneeReference1;
		 }
		 set
		 {
		   if(consigneeReference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeReference1",OldValue=consigneeReference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeReference1=value;
		   }
			
		 }
	   }
	  private string consigneeReference2 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeReference2  
	   {
	    
	     get
		{
		   return consigneeReference2;
		 }
		 set
		 {
		   if(consigneeReference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeReference2",OldValue=consigneeReference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeReference2=value;
		   }
			
		 }
	   }
	  private string fromPortId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortId  
	   {
	    
	     get
		{
		   return fromPortId;
		 }
		 set
		 {
		   if(fromPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortId",OldValue=fromPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortId=value;
		   }
			
		 }
	   }
	  private string fromPort ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPort  
	   {
	    
	     get
		{
		   return fromPort;
		 }
		 set
		 {
		   if(fromPort != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPort",OldValue=fromPort,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPort=value;
		   }
			
		 }
	   }
	  private string toPortId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortId  
	   {
	    
	     get
		{
		   return toPortId;
		 }
		 set
		 {
		   if(toPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortId",OldValue=toPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortId=value;
		   }
			
		 }
	   }
	  private string toPort ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPort  
	   {
	    
	     get
		{
		   return toPort;
		 }
		 set
		 {
		   if(toPort != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPort",OldValue=toPort,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPort=value;
		   }
			
		 }
	   }
	  private string incotermId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string IncotermId  
	   {
	    
	     get
		{
		   return incotermId;
		 }
		 set
		 {
		   if(incotermId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncotermId",OldValue=incotermId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   incotermId=value;
		   }
			
		 }
	   }
	  private string salesmanUserId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SalesmanUserId  
	   {
	    
	     get
		{
		   return salesmanUserId;
		 }
		 set
		 {
		   if(salesmanUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SalesmanUserId",OldValue=salesmanUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   salesmanUserId=value;
		   }
			
		 }
	   }
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
		   }
			
		 }
	   }
	  private DateTime openDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime OpenDate  
	   {
	    
	     get
		{
		   return openDate;
		 }
		 set
		 {
		   if(openDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpenDate",OldValue=openDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   openDate=value;
		   }
			
		 }
	   }
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string descriptionOfGoods ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DescriptionOfGoods  
	   {
	    
	     get
		{
		   return descriptionOfGoods;
		 }
		 set
		 {
		   if(descriptionOfGoods != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DescriptionOfGoods",OldValue=descriptionOfGoods,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   descriptionOfGoods=value;
		   }
			
		 }
	   }
	  private double? chargeableWeight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ChargeableWeight  
	   {
	    
	     get
		{
		   return chargeableWeight;
		 }
		 set
		 {
		   if(chargeableWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeight",OldValue=chargeableWeight,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   chargeableWeight=value;
		   }
			
		 }
	   }
	  private double? grossWeight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? GrossWeight  
	   {
	    
	     get
		{
		   return grossWeight;
		 }
		 set
		 {
		   if(grossWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeight",OldValue=grossWeight,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   grossWeight=value;
		   }
			
		 }
	   }
	  private bool isClosed ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClosed  
	   {
	    
	     get
		{
		   return isClosed;
		 }
		 set
		 {
		   if(isClosed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClosed",OldValue=isClosed,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClosed=value;
		   }
			
		 }
	   }
	  private string dimensionsUnitCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DimensionsUnitCode  
	   {
	    
	     get
		{
		   return dimensionsUnitCode;
		 }
		 set
		 {
		   if(dimensionsUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DimensionsUnitCode",OldValue=dimensionsUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dimensionsUnitCode=value;
		   }
			
		 }
	   }
	  private double? volume ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Volume  
	   {
	    
	     get
		{
		   return volume;
		 }
		 set
		 {
		   if(volume != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Volume",OldValue=volume,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   volume=value;
		   }
			
		 }
	   }
	  private double? ratio ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Ratio  
	   {
	    
	     get
		{
		   return ratio;
		 }
		 set
		 {
		   if(ratio != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Ratio",OldValue=ratio,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   ratio=value;
		   }
			
		 }
	   }
	  private int? numberOfPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? NumberOfPackages  
	   {
	    
	     get
		{
		   return numberOfPackages;
		 }
		 set
		 {
		   if(numberOfPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfPackages",OldValue=numberOfPackages,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   numberOfPackages=value;
		   }
			
		 }
	   }
	  private int? numberOfContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? NumberOfContainers  
	   {
	    
	     get
		{
		   return numberOfContainers;
		 }
		 set
		 {
		   if(numberOfContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfContainers",OldValue=numberOfContainers,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   numberOfContainers=value;
		   }
			
		 }
	   }
	  private string volumeUnitCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string VolumeUnitCode  
	   {
	    
	     get
		{
		   return volumeUnitCode;
		 }
		 set
		 {
		   if(volumeUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumeUnitCode",OldValue=volumeUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   volumeUnitCode=value;
		   }
			
		 }
	   }
	  private bool isDangerous ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDangerous  
	   {
	    
	     get
		{
		   return isDangerous;
		 }
		 set
		 {
		   if(isDangerous != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDangerous",OldValue=isDangerous,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDangerous=value;
		   }
			
		 }
	   }
	  private int? expirationDays ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ExpirationDays  
	   {
	    
	     get
		{
		   return expirationDays;
		 }
		 set
		 {
		   if(expirationDays != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExpirationDays",OldValue=expirationDays,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   expirationDays=value;
		   }
			
		 }
	   }
	  private DateTime? expirationDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private bool isFreightBySteps ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsFreightBySteps  
	   {
	    
	     get
		{
		   return isFreightBySteps;
		 }
		 set
		 {
		   if(isFreightBySteps != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFreightBySteps",OldValue=isFreightBySteps,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isFreightBySteps=value;
		   }
			
		 }
	   }
	  private string totalContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TotalContainers  
	   {
	    
	     get
		{
		   return totalContainers;
		 }
		 set
		 {
		   if(totalContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalContainers",OldValue=totalContainers,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   totalContainers=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string packageType1Id ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageType1Id  
	   {
	    
	     get
		{
		   return packageType1Id;
		 }
		 set
		 {
		   if(packageType1Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType1Id",OldValue=packageType1Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageType1Id=value;
		   }
			
		 }
	   }
	  private string packageType2Id ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageType2Id  
	   {
	    
	     get
		{
		   return packageType2Id;
		 }
		 set
		 {
		   if(packageType2Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType2Id",OldValue=packageType2Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageType2Id=value;
		   }
			
		 }
	   }
	  private string packageType3Id ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageType3Id  
	   {
	    
	     get
		{
		   return packageType3Id;
		 }
		 set
		 {
		   if(packageType3Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType3Id",OldValue=packageType3Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageType3Id=value;
		   }
			
		 }
	   }
	  private string packageType4Id ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageType4Id  
	   {
	    
	     get
		{
		   return packageType4Id;
		 }
		 set
		 {
		   if(packageType4Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType4Id",OldValue=packageType4Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageType4Id=value;
		   }
			
		 }
	   }
	  private string packageType5Id ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageType5Id  
	   {
	    
	     get
		{
		   return packageType5Id;
		 }
		 set
		 {
		   if(packageType5Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType5Id",OldValue=packageType5Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageType5Id=value;
		   }
			
		 }
	   }
	  private int? packageType1Quantity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? PackageType1Quantity  
	   {
	    
	     get
		{
		   return packageType1Quantity;
		 }
		 set
		 {
		   if(packageType1Quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType1Quantity",OldValue=packageType1Quantity,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   packageType1Quantity=value;
		   }
			
		 }
	   }
	  private int? packageType2Quantity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? PackageType2Quantity  
	   {
	    
	     get
		{
		   return packageType2Quantity;
		 }
		 set
		 {
		   if(packageType2Quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType2Quantity",OldValue=packageType2Quantity,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   packageType2Quantity=value;
		   }
			
		 }
	   }
	  private int? packageType3Quantity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? PackageType3Quantity  
	   {
	    
	     get
		{
		   return packageType3Quantity;
		 }
		 set
		 {
		   if(packageType3Quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType3Quantity",OldValue=packageType3Quantity,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   packageType3Quantity=value;
		   }
			
		 }
	   }
	  private int? packageType4Quantity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? PackageType4Quantity  
	   {
	    
	     get
		{
		   return packageType4Quantity;
		 }
		 set
		 {
		   if(packageType4Quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType4Quantity",OldValue=packageType4Quantity,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   packageType4Quantity=value;
		   }
			
		 }
	   }
	  private int? packageType5Quantity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? PackageType5Quantity  
	   {
	    
	     get
		{
		   return packageType5Quantity;
		 }
		 set
		 {
		   if(packageType5Quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageType5Quantity",OldValue=packageType5Quantity,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   packageType5Quantity=value;
		   }
			
		 }
	   }

	   private List<QuoteOPChargePM> quoteCharges;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("QuoteOPCharge", "Id","QuoteOPId")]
	   [DataMember]
	   public virtual List<QuoteOPChargePM> QuoteCharges  
	   {
	        get
             {
                 if (quoteCharges == null)
                 {
                     quoteCharges = new List<QuoteOPChargePM>();
                 }
                 return quoteCharges;
              }
             set { quoteCharges = value; }
	    }
		   
	   private List<QuoteOPChargePM>  deletedQuoteCharges;
	   public virtual List<QuoteOPChargePM> DeletedQuoteCharges  
	   {
	        get
             {
                 if ( deletedQuoteCharges == null)
                 {
                      deletedQuoteCharges = new List<QuoteOPChargePM>();
                 }
                 return  deletedQuoteCharges;
              }
             set {  deletedQuoteCharges = value; }
	    }
	  	  private string quoteTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteTypeCode  
	   {
	    
	     get
		{
		   return quoteTypeCode;
		 }
		 set
		 {
		   if(quoteTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteTypeCode",OldValue=quoteTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteTypeCode=value;
		   }
			
		 }
	   }
	  private string quoteTypeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteTypeName  
	   {
	    
	     get
		{
		   return quoteTypeName;
		 }
		 set
		 {
		   if(quoteTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteTypeName",OldValue=quoteTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteTypeName=value;
		   }
			
		 }
	   }
	  private string grossWeightUnitCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string GrossWeightUnitCode  
	   {
	    
	     get
		{
		   return grossWeightUnitCode;
		 }
		 set
		 {
		   if(grossWeightUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeightUnitCode",OldValue=grossWeightUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   grossWeightUnitCode=value;
		   }
			
		 }
	   }
	  private string chargeableWeightUnitCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargeableWeightUnitCode  
	   {
	    
	     get
		{
		   return chargeableWeightUnitCode;
		 }
		 set
		 {
		   if(chargeableWeightUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeightUnitCode",OldValue=chargeableWeightUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargeableWeightUnitCode=value;
		   }
			
		 }
	   }
	  private double? volumetricWeight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? VolumetricWeight  
	   {
	    
	     get
		{
		   return volumetricWeight;
		 }
		 set
		 {
		   if(volumetricWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumetricWeight",OldValue=volumetricWeight,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   volumetricWeight=value;
		   }
			
		 }
	   }
	  private string pickupLocation ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PickupLocation  
	   {
	    
	     get
		{
		   return pickupLocation;
		 }
		 set
		 {
		   if(pickupLocation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupLocation",OldValue=pickupLocation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pickupLocation=value;
		   }
			
		 }
	   }
	  private string deliveryLocation ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliveryLocation  
	   {
	    
	     get
		{
		   return deliveryLocation;
		 }
		 set
		 {
		   if(deliveryLocation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveryLocation",OldValue=deliveryLocation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliveryLocation=value;
		   }
			
		 }
	   }
	  private string departmentName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepartmentName  
	   {
	    
	     get
		{
		   return departmentName;
		 }
		 set
		 {
		   if(departmentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartmentName",OldValue=departmentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   departmentName=value;
		   }
			
		 }
	   }
	  private string branchName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BranchName  
	   {
	    
	     get
		{
		   return branchName;
		 }
		 set
		 {
		   if(branchName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BranchName",OldValue=branchName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   branchName=value;
		   }
			
		 }
	   }
	  private string fromPartnerId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPartnerId  
	   {
	    
	     get
		{
		   return fromPartnerId;
		 }
		 set
		 {
		   if(fromPartnerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPartnerId",OldValue=fromPartnerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPartnerId=value;
		   }
			
		 }
	   }
	  private string toPartnerId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPartnerId  
	   {
	    
	     get
		{
		   return toPartnerId;
		 }
		 set
		 {
		   if(toPartnerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPartnerId",OldValue=toPartnerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPartnerId=value;
		   }
			
		 }
	   }
	  private string fromPartnerAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPartnerAddressId  
	   {
	    
	     get
		{
		   return fromPartnerAddressId;
		 }
		 set
		 {
		   if(fromPartnerAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPartnerAddressId",OldValue=fromPartnerAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPartnerAddressId=value;
		   }
			
		 }
	   }
	  private string toPartnerAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPartnerAddressId  
	   {
	    
	     get
		{
		   return toPartnerAddressId;
		 }
		 set
		 {
		   if(toPartnerAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPartnerAddressId",OldValue=toPartnerAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPartnerAddressId=value;
		   }
			
		 }
	   }
	  private string fromLocation ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromLocation  
	   {
	    
	     get
		{
		   return fromLocation;
		 }
		 set
		 {
		   if(fromLocation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromLocation",OldValue=fromLocation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromLocation=value;
		   }
			
		 }
	   }
	  private string toLocation ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToLocation  
	   {
	    
	     get
		{
		   return toLocation;
		 }
		 set
		 {
		   if(toLocation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToLocation",OldValue=toLocation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toLocation=value;
		   }
			
		 }
	   }
	  private string fromAddressCity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromAddressCity  
	   {
	    
	     get
		{
		   return fromAddressCity;
		 }
		 set
		 {
		   if(fromAddressCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromAddressCity",OldValue=fromAddressCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromAddressCity=value;
		   }
			
		 }
	   }
	  private string fromAddressCountryId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromAddressCountryId  
	   {
	    
	     get
		{
		   return fromAddressCountryId;
		 }
		 set
		 {
		   if(fromAddressCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromAddressCountryId",OldValue=fromAddressCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromAddressCountryId=value;
		   }
			
		 }
	   }
	  private string fromAddressZipCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromAddressZipCode  
	   {
	    
	     get
		{
		   return fromAddressZipCode;
		 }
		 set
		 {
		   if(fromAddressZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromAddressZipCode",OldValue=fromAddressZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromAddressZipCode=value;
		   }
			
		 }
	   }
	  private string toAddressCity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressCity  
	   {
	    
	     get
		{
		   return toAddressCity;
		 }
		 set
		 {
		   if(toAddressCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressCity",OldValue=toAddressCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressCity=value;
		   }
			
		 }
	   }
	  private string toAddressCountryId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressCountryId  
	   {
	    
	     get
		{
		   return toAddressCountryId;
		 }
		 set
		 {
		   if(toAddressCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressCountryId",OldValue=toAddressCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressCountryId=value;
		   }
			
		 }
	   }
	  private string toAddressZipCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressZipCode  
	   {
	    
	     get
		{
		   return toAddressZipCode;
		 }
		 set
		 {
		   if(toAddressZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressZipCode",OldValue=toAddressZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressZipCode=value;
		   }
			
		 }
	   }
	  private double? dimFactor ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? DimFactor  
	   {
	    
	     get
		{
		   return dimFactor;
		 }
		 set
		 {
		   if(dimFactor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DimFactor",OldValue=dimFactor,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   dimFactor=value;
		   }
			
		 }
	   }
	  private bool includePickUp ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IncludePickUp  
	   {
	    
	     get
		{
		   return includePickUp;
		 }
		 set
		 {
		   if(includePickUp != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncludePickUp",OldValue=includePickUp,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   includePickUp=value;
		   }
			
		 }
	   }
	  private bool includeDelivery ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IncludeDelivery  
	   {
	    
	     get
		{
		   return includeDelivery;
		 }
		 set
		 {
		   if(includeDelivery != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncludeDelivery",OldValue=includeDelivery,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   includeDelivery=value;
		   }
			
		 }
	   }
	  private string pickUpAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PickUpAddressId  
	   {
	    
	     get
		{
		   return pickUpAddressId;
		 }
		 set
		 {
		   if(pickUpAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickUpAddressId",OldValue=pickUpAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pickUpAddressId=value;
		   }
			
		 }
	   }
	  private string deliveryAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliveryAddressId  
	   {
	    
	     get
		{
		   return deliveryAddressId;
		 }
		 set
		 {
		   if(deliveryAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveryAddressId",OldValue=deliveryAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliveryAddressId=value;
		   }
			
		 }
	   }
	  private string quoteClosingReasonCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteClosingReasonCode  
	   {
	    
	     get
		{
		   return quoteClosingReasonCode;
		 }
		 set
		 {
		   if(quoteClosingReasonCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteClosingReasonCode",OldValue=quoteClosingReasonCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteClosingReasonCode=value;
		   }
			
		 }
	   }
	  private DateTime? sentDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? SentDate  
	   {
	    
	     get
		{
		   return sentDate;
		 }
		 set
		 {
		   if(sentDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SentDate",OldValue=sentDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   sentDate=value;
		   }
			
		 }
	   }
	  private DateTime? acceptedDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? AcceptedDate  
	   {
	    
	     get
		{
		   return acceptedDate;
		 }
		 set
		 {
		   if(acceptedDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AcceptedDate",OldValue=acceptedDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   acceptedDate=value;
		   }
			
		 }
	   }
	  private DateTime? declinedDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DeclinedDate  
	   {
	    
	     get
		{
		   return declinedDate;
		 }
		 set
		 {
		   if(declinedDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclinedDate",OldValue=declinedDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   declinedDate=value;
		   }
			
		 }
	   }
	  private int? usageCount ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? UsageCount  
	   {
	    
	     get
		{
		   return usageCount;
		 }
		 set
		 {
		   if(usageCount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UsageCount",OldValue=usageCount,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   usageCount=value;
		   }
			
		 }
	   }
	  private DateTime? lastUsageDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastUsageDate  
	   {
	    
	     get
		{
		   return lastUsageDate;
		 }
		 set
		 {
		   if(lastUsageDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastUsageDate",OldValue=lastUsageDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastUsageDate=value;
		   }
			
		 }
	   }
	  private string businessUnitId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BusinessUnitId  
	   {
	    
	     get
		{
		   return businessUnitId;
		 }
		 set
		 {
		   if(businessUnitId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BusinessUnitId",OldValue=businessUnitId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   businessUnitId=value;
		   }
			
		 }
	   }

	   private List<QuoteOPPackagePM> quotePackages;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("QuoteOPPackage", "Id","QuoteOPId")]
	   [DataMember]
	   public virtual List<QuoteOPPackagePM> QuotePackages  
	   {
	        get
             {
                 if (quotePackages == null)
                 {
                     quotePackages = new List<QuoteOPPackagePM>();
                 }
                 return quotePackages;
              }
             set { quotePackages = value; }
	    }
		   
	   private List<QuoteOPPackagePM>  deletedQuotePackages;
	   public virtual List<QuoteOPPackagePM> DeletedQuotePackages  
	   {
	        get
             {
                 if ( deletedQuotePackages == null)
                 {
                      deletedQuotePackages = new List<QuoteOPPackagePM>();
                 }
                 return  deletedQuotePackages;
              }
             set {  deletedQuotePackages = value; }
	    }
	  	  private string customerReference1 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerReference1  
	   {
	    
	     get
		{
		   return customerReference1;
		 }
		 set
		 {
		   if(customerReference1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerReference1",OldValue=customerReference1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerReference1=value;
		   }
			
		 }
	   }
	  private string customerReference2 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerReference2  
	   {
	    
	     get
		{
		   return customerReference2;
		 }
		 set
		 {
		   if(customerReference2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerReference2",OldValue=customerReference2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerReference2=value;
		   }
			
		 }
	   }
	  private string subject ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Subject  
	   {
	    
	     get
		{
		   return subject;
		 }
		 set
		 {
		   if(subject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Subject",OldValue=subject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   subject=value;
		   }
			
		 }
	   }
	  private bool isSubjectEdited ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSubjectEdited  
	   {
	    
	     get
		{
		   return isSubjectEdited;
		 }
		 set
		 {
		   if(isSubjectEdited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSubjectEdited",OldValue=isSubjectEdited,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSubjectEdited=value;
		   }
			
		 }
	   }
	  private string salesmanName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SalesmanName  
	   {
	    
	     get
		{
		   return salesmanName;
		 }
		 set
		 {
		   if(salesmanName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SalesmanName",OldValue=salesmanName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   salesmanName=value;
		   }
			
		 }
	   }
	  private string incotermCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string IncotermCode  
	   {
	    
	     get
		{
		   return incotermCode;
		 }
		 set
		 {
		   if(incotermCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncotermCode",OldValue=incotermCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   incotermCode=value;
		   }
			
		 }
	   }
	  private string stageId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string StageId  
	   {
	    
	     get
		{
		   return stageId;
		 }
		 set
		 {
		   if(stageId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StageId",OldValue=stageId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   stageId=value;
		   }
			
		 }
	   }
	  private string stageName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string StageName  
	   {
	    
	     get
		{
		   return stageName;
		 }
		 set
		 {
		   if(stageName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StageName",OldValue=stageName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   stageName=value;
		   }
			
		 }
	   }
	  private DateTime? stageDueDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StageDueDate  
	   {
	    
	     get
		{
		   return stageDueDate;
		 }
		 set
		 {
		   if(stageDueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StageDueDate",OldValue=stageDueDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   stageDueDate=value;
		   }
			
		 }
	   }
	  private string ratingCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string RatingCode  
	   {
	    
	     get
		{
		   return ratingCode;
		 }
		 set
		 {
		   if(ratingCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RatingCode",OldValue=ratingCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ratingCode=value;
		   }
			
		 }
	   }
	  private DateTime? lastActivityDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastActivityDate  
	   {
	    
	     get
		{
		   return lastActivityDate;
		 }
		 set
		 {
		   if(lastActivityDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastActivityDate",OldValue=lastActivityDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastActivityDate=value;
		   }
			
		 }
	   }
	  private string lastActivitySubject ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastActivitySubject  
	   {
	    
	     get
		{
		   return lastActivitySubject;
		 }
		 set
		 {
		   if(lastActivitySubject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastActivitySubject",OldValue=lastActivitySubject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastActivitySubject=value;
		   }
			
		 }
	   }
	  private string lastActivityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastActivityTypeCode  
	   {
	    
	     get
		{
		   return lastActivityTypeCode;
		 }
		 set
		 {
		   if(lastActivityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastActivityTypeCode",OldValue=lastActivityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastActivityTypeCode=value;
		   }
			
		 }
	   }
	  private DateTime? nextActivityDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? NextActivityDate  
	   {
	    
	     get
		{
		   return nextActivityDate;
		 }
		 set
		 {
		   if(nextActivityDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextActivityDate",OldValue=nextActivityDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   nextActivityDate=value;
		   }
			
		 }
	   }
	  private string nextActivitySubject ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NextActivitySubject  
	   {
	    
	     get
		{
		   return nextActivitySubject;
		 }
		 set
		 {
		   if(nextActivitySubject != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextActivitySubject",OldValue=nextActivitySubject,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nextActivitySubject=value;
		   }
			
		 }
	   }
	  private string nextActivityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NextActivityTypeCode  
	   {
	    
	     get
		{
		   return nextActivityTypeCode;
		 }
		 set
		 {
		   if(nextActivityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NextActivityTypeCode",OldValue=nextActivityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   nextActivityTypeCode=value;
		   }
			
		 }
	   }
	  private string ratingName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string RatingName  
	   {
	    
	     get
		{
		   return ratingName;
		 }
		 set
		 {
		   if(ratingName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RatingName",OldValue=ratingName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ratingName=value;
		   }
			
		 }
	   }
	  private int stageMaxDays ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int StageMaxDays  
	   {
	    
	     get
		{
		   return stageMaxDays;
		 }
		 set
		 {
		   if(stageMaxDays != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StageMaxDays",OldValue=stageMaxDays,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   stageMaxDays=value;
		   }
			
		 }
	   }
	  private bool isAutomaticallyClosed ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAutomaticallyClosed  
	   {
	    
	     get
		{
		   return isAutomaticallyClosed;
		 }
		 set
		 {
		   if(isAutomaticallyClosed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAutomaticallyClosed",OldValue=isAutomaticallyClosed,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAutomaticallyClosed=value;
		   }
			
		 }
	   }
	  private DateTime? automaticallyCloseDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? AutomaticallyCloseDate  
	   {
	    
	     get
		{
		   return automaticallyCloseDate;
		 }
		 set
		 {
		   if(automaticallyCloseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticallyCloseDate",OldValue=automaticallyCloseDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   automaticallyCloseDate=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId  
	   {
	    
	     get
		{
		   return updatedByUserId;
		 }
		 set
		 {
		   if(updatedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserId",OldValue=updatedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserId=value;
		   }
			
		 }
	   }
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private int? automaticallyCloseDays ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? AutomaticallyCloseDays  
	   {
	    
	     get
		{
		   return automaticallyCloseDays;
		 }
		 set
		 {
		   if(automaticallyCloseDays != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticallyCloseDays",OldValue=automaticallyCloseDays,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   automaticallyCloseDays=value;
		   }
			
		 }
	   }
	  private string productCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProductCode  
	   {
	    
	     get
		{
		   return productCode;
		 }
		 set
		 {
		   if(productCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProductCode",OldValue=productCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   productCode=value;
		   }
			
		 }
	   }
	  private string eTDLabel ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ETDLabel  
	   {
	    
	     get
		{
		   return eTDLabel;
		 }
		 set
		 {
		   if(eTDLabel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ETDLabel",OldValue=eTDLabel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   eTDLabel=value;
		   }
			
		 }
	   }
	  private string eTALabel ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ETALabel  
	   {
	    
	     get
		{
		   return eTALabel;
		 }
		 set
		 {
		   if(eTALabel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ETALabel",OldValue=eTALabel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   eTALabel=value;
		   }
			
		 }
	   }
	  private string transitTime ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string departureFrequency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DepartureFrequency  
	   {
	    
	     get
		{
		   return departureFrequency;
		 }
		 set
		 {
		   if(departureFrequency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DepartureFrequency",OldValue=departureFrequency,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   departureFrequency=value;
		   }
			
		 }
	   }
	  private DateTime? eTD ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ETD  
	   {
	    
	     get
		{
		   return eTD;
		 }
		 set
		 {
		   if(eTD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ETD",OldValue=eTD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   eTD=value;
		   }
			
		 }
	   }
	  private DateTime? eTA ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ETA  
	   {
	    
	     get
		{
		   return eTA;
		 }
		 set
		 {
		   if(eTA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ETA",OldValue=eTA,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   eTA=value;
		   }
			
		 }
	   }
	  private string agentId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentId  
	   {
	    
	     get
		{
		   return agentId;
		 }
		 set
		 {
		   if(agentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentId",OldValue=agentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentId=value;
		   }
			
		 }
	   }
	  private string agentName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentName  
	   {
	    
	     get
		{
		   return agentName;
		 }
		 set
		 {
		   if(agentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentName",OldValue=agentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentName=value;
		   }
			
		 }
	   }
	  private string agentAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentAddressId  
	   {
	    
	     get
		{
		   return agentAddressId;
		 }
		 set
		 {
		   if(agentAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentAddressId",OldValue=agentAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentAddressId=value;
		   }
			
		 }
	   }
	  private string agentContactId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string AgentContactId  
	   {
	    
	     get
		{
		   return agentContactId;
		 }
		 set
		 {
		   if(agentContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentContactId",OldValue=agentContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   agentContactId=value;
		   }
			
		 }
	   }
	  private string moveTypeId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string MoveTypeId  
	   {
	    
	     get
		{
		   return moveTypeId;
		 }
		 set
		 {
		   if(moveTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MoveTypeId",OldValue=moveTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   moveTypeId=value;
		   }
			
		 }
	   }
	  private double? tEU ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? TEU  
	   {
	    
	     get
		{
		   return tEU;
		 }
		 set
		 {
		   if(tEU != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TEU",OldValue=tEU,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   tEU=value;
		   }
			
		 }
	   }
	  private string salesTotalAmounts ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SalesTotalAmounts  
	   {
	    
	     get
		{
		   return salesTotalAmounts;
		 }
		 set
		 {
		   if(salesTotalAmounts != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SalesTotalAmounts",OldValue=salesTotalAmounts,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   salesTotalAmounts=value;
		   }
			
		 }
	   }

	   private List<QuoteOPSalesTotalPM> quoteSalesTotals;
	 
		     
	   [Include]
	   [Association("QuoteOPSalesTotals", "","")]
	   [DataMember]
	   public virtual List<QuoteOPSalesTotalPM> QuoteSalesTotals  
	   {
	        get
             {
                 if (quoteSalesTotals == null)
                 {
                     quoteSalesTotals = new List<QuoteOPSalesTotalPM>();
                 }
                 return quoteSalesTotals;
              }
             set { quoteSalesTotals = value; }
	    }
		   
	   private List<QuoteOPSalesTotalPM>  deletedQuoteSalesTotals;
	   public virtual List<QuoteOPSalesTotalPM> DeletedQuoteSalesTotals  
	   {
	        get
             {
                 if ( deletedQuoteSalesTotals == null)
                 {
                      deletedQuoteSalesTotals = new List<QuoteOPSalesTotalPM>();
                 }
                 return  deletedQuoteSalesTotals;
              }
             set {  deletedQuoteSalesTotals = value; }
	    }
	  	  private double? valueOfGoods ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ValueOfGoods  
	   {
	    
	     get
		{
		   return valueOfGoods;
		 }
		 set
		 {
		   if(valueOfGoods != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ValueOfGoods",OldValue=valueOfGoods,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   valueOfGoods=value;
		   }
			
		 }
	   }
	  private string valueOfGoodsCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ValueOfGoodsCurrencyId  
	   {
	    
	     get
		{
		   return valueOfGoodsCurrencyId;
		 }
		 set
		 {
		   if(valueOfGoodsCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ValueOfGoodsCurrencyId",OldValue=valueOfGoodsCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   valueOfGoodsCurrencyId=value;
		   }
			
		 }
	   }
	  private bool isChargesByVAT ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsChargesByVAT  
	   {
	    
	     get
		{
		   return isChargesByVAT;
		 }
		 set
		 {
		   if(isChargesByVAT != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsChargesByVAT",OldValue=isChargesByVAT,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isChargesByVAT=value;
		   }
			
		 }
	   }

	   private List<QuoteOPVATsTotalPM> totalVATPerQuote;
	 
		     
	   [Include]
	   [Association("QuoteOPVATsTotalPMs", "Id","QuoteOPId")]
	   [DataMember]
	   public virtual List<QuoteOPVATsTotalPM> TotalVATPerQuote  
	   {
	        get
             {
                 if (totalVATPerQuote == null)
                 {
                     totalVATPerQuote = new List<QuoteOPVATsTotalPM>();
                 }
                 return totalVATPerQuote;
              }
             set { totalVATPerQuote = value; }
	    }
		   
	   private List<QuoteOPVATsTotalPM>  deletedTotalVATPerQuote;
	   public virtual List<QuoteOPVATsTotalPM> DeletedTotalVATPerQuote  
	   {
	        get
             {
                 if ( deletedTotalVATPerQuote == null)
                 {
                      deletedTotalVATPerQuote = new List<QuoteOPVATsTotalPM>();
                 }
                 return  deletedTotalVATPerQuote;
              }
             set {  deletedTotalVATPerQuote = value; }
	    }
	  	  private bool isSecured ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSecured  
	   {
	    
	     get
		{
		   return isSecured;
		 }
		 set
		 {
		   if(isSecured != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSecured",OldValue=isSecured,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSecured=value;
		   }
			
		 }
	   }
	  private string directionName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DirectionName  
	   {
	    
	     get
		{
		   return directionName;
		 }
		 set
		 {
		   if(directionName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DirectionName",OldValue=directionName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   directionName=value;
		   }
			
		 }
	   }
	  private string transportModeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeName  
	   {
	    
	     get
		{
		   return transportModeName;
		 }
		 set
		 {
		   if(transportModeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeName",OldValue=transportModeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeName=value;
		   }
			
		 }
	   }
	  private bool isCustomerSet ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCustomerSet  
	   {
	    
	     get
		{
		   return isCustomerSet;
		 }
		 set
		 {
		   if(isCustomerSet != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCustomerSet",OldValue=isCustomerSet,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCustomerSet=value;
		   }
			
		 }
	   }
	  private string baseShipmentNumber ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BaseShipmentNumber  
	   {
	    
	     get
		{
		   return baseShipmentNumber;
		 }
		 set
		 {
		   if(baseShipmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BaseShipmentNumber",OldValue=baseShipmentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   baseShipmentNumber=value;
		   }
			
		 }
	   }
	  private string saleCurrencyCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SaleCurrencyCode  
	   {
	    
	     get
		{
		   return saleCurrencyCode;
		 }
		 set
		 {
		   if(saleCurrencyCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleCurrencyCode",OldValue=saleCurrencyCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   saleCurrencyCode=value;
		   }
			
		 }
	   }
	  private string freelancerName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreelancerName  
	   {
	    
	     get
		{
		   return freelancerName;
		 }
		 set
		 {
		   if(freelancerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreelancerName",OldValue=freelancerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freelancerName=value;
		   }
			
		 }
	   }
	  private string shipperNote ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperNote  
	   {
	    
	     get
		{
		   return shipperNote;
		 }
		 set
		 {
		   if(shipperNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperNote",OldValue=shipperNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperNote=value;
		   }
			
		 }
	   }
	  private string consigneeNote ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeNote  
	   {
	    
	     get
		{
		   return consigneeNote;
		 }
		 set
		 {
		   if(consigneeNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeNote",OldValue=consigneeNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeNote=value;
		   }
			
		 }
	   }
	  private string shipperMainAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperMainAddressId  
	   {
	    
	     get
		{
		   return shipperMainAddressId;
		 }
		 set
		 {
		   if(shipperMainAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperMainAddressId",OldValue=shipperMainAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperMainAddressId=value;
		   }
			
		 }
	   }
	  private string shipperPickAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperPickAddressId  
	   {
	    
	     get
		{
		   return shipperPickAddressId;
		 }
		 set
		 {
		   if(shipperPickAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperPickAddressId",OldValue=shipperPickAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperPickAddressId=value;
		   }
			
		 }
	   }
	  private string consigneeMainAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeMainAddressId  
	   {
	    
	     get
		{
		   return consigneeMainAddressId;
		 }
		 set
		 {
		   if(consigneeMainAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeMainAddressId",OldValue=consigneeMainAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeMainAddressId=value;
		   }
			
		 }
	   }
	  private string consigneePickAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneePickAddressId  
	   {
	    
	     get
		{
		   return consigneePickAddressId;
		 }
		 set
		 {
		   if(consigneePickAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneePickAddressId",OldValue=consigneePickAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneePickAddressId=value;
		   }
			
		 }
	   }
	  private string customerRankName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerRankName  
	   {
	    
	     get
		{
		   return customerRankName;
		 }
		 set
		 {
		   if(customerRankName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerRankName",OldValue=customerRankName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerRankName=value;
		   }
			
		 }
	   }
	  private string fromPortName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortName  
	   {
	    
	     get
		{
		   return fromPortName;
		 }
		 set
		 {
		   if(fromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortName",OldValue=fromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortName=value;
		   }
			
		 }
	   }
	  private string fromPortCountry ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortCountry  
	   {
	    
	     get
		{
		   return fromPortCountry;
		 }
		 set
		 {
		   if(fromPortCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortCountry",OldValue=fromPortCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortCountry=value;
		   }
			
		 }
	   }
	  private string toPortName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortName  
	   {
	    
	     get
		{
		   return toPortName;
		 }
		 set
		 {
		   if(toPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortName",OldValue=toPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortName=value;
		   }
			
		 }
	   }
	  private string toPortCountry ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortCountry  
	   {
	    
	     get
		{
		   return toPortCountry;
		 }
		 set
		 {
		   if(toPortCountry != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortCountry",OldValue=toPortCountry,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortCountry=value;
		   }
			
		 }
	   }
	  private string actionType ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ActionType  
	   {
	    
	     get
		{
		   return actionType;
		 }
		 set
		 {
		   if(actionType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActionType",OldValue=actionType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   actionType=value;
		   }
			
		 }
	   }
	  private string eventNote ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EventNote  
	   {
	    
	     get
		{
		   return eventNote;
		 }
		 set
		 {
		   if(eventNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EventNote",OldValue=eventNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   eventNote=value;
		   }
			
		 }
	   }
	  private bool isCopy ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCopy  
	   {
	    
	     get
		{
		   return isCopy;
		 }
		 set
		 {
		   if(isCopy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCopy",OldValue=isCopy,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCopy=value;
		   }
			
		 }
	   }
	  private string toCountryId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string fromCountryId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private bool fromCountryIsEC ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FromCountryIsEC  
	   {
	    
	     get
		{
		   return fromCountryIsEC;
		 }
		 set
		 {
		   if(fromCountryIsEC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromCountryIsEC",OldValue=fromCountryIsEC,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fromCountryIsEC=value;
		   }
			
		 }
	   }
	  private bool toCountryIsEC ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ToCountryIsEC  
	   {
	    
	     get
		{
		   return toCountryIsEC;
		 }
		 set
		 {
		   if(toCountryIsEC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToCountryIsEC",OldValue=toCountryIsEC,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   toCountryIsEC=value;
		   }
			
		 }
	   }
	  private string fromPartnerName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPartnerName  
	   {
	    
	     get
		{
		   return fromPartnerName;
		 }
		 set
		 {
		   if(fromPartnerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPartnerName",OldValue=fromPartnerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPartnerName=value;
		   }
			
		 }
	   }
	  private string toPartnerName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPartnerName  
	   {
	    
	     get
		{
		   return toPartnerName;
		 }
		 set
		 {
		   if(toPartnerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPartnerName",OldValue=toPartnerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPartnerName=value;
		   }
			
		 }
	   }
	  private bool isPotentialShipper ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPotentialShipper  
	   {
	    
	     get
		{
		   return isPotentialShipper;
		 }
		 set
		 {
		   if(isPotentialShipper != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPotentialShipper",OldValue=isPotentialShipper,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPotentialShipper=value;
		   }
			
		 }
	   }
	  private bool isPotentialConsignee ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsPotentialConsignee  
	   {
	    
	     get
		{
		   return isPotentialConsignee;
		 }
		 set
		 {
		   if(isPotentialConsignee != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsPotentialConsignee",OldValue=isPotentialConsignee,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isPotentialConsignee=value;
		   }
			
		 }
	   }
	  private string incotermName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string IncotermName  
	   {
	    
	     get
		{
		   return incotermName;
		 }
		 set
		 {
		   if(incotermName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncotermName",OldValue=incotermName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   incotermName=value;
		   }
			
		 }
	   }
	  private string fromCountryCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string fromCountryName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string toCountryCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string toCountryName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private bool isHybrid ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsHybrid  
	   {
	    
	     get
		{
		   return isHybrid;
		 }
		 set
		 {
		   if(isHybrid != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsHybrid",OldValue=isHybrid,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isHybrid=value;
		   }
			
		 }
	   }

	   private List<QuoteOPFollowUpPM> followUps;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("FollowOPUpQuote", "Id","QuoteId")]
	   [DataMember]
	   public virtual List<QuoteOPFollowUpPM> FollowUps  
	   {
	        get
             {
                 if (followUps == null)
                 {
                     followUps = new List<QuoteOPFollowUpPM>();
                 }
                 return followUps;
              }
             set { followUps = value; }
	    }
		   
	   private List<QuoteOPFollowUpPM>  deletedFollowUps;
	   public virtual List<QuoteOPFollowUpPM> DeletedFollowUps  
	   {
	        get
             {
                 if ( deletedFollowUps == null)
                 {
                      deletedFollowUps = new List<QuoteOPFollowUpPM>();
                 }
                 return  deletedFollowUps;
              }
             set {  deletedFollowUps = value; }
	    }
	  
	   private List<QuoteOPDocumentVersionPM> quoteDocumentVersions;
	 
	   [DataMember]
	   public virtual List<QuoteOPDocumentVersionPM> QuoteDocumentVersions  
	   {
	        get
             {
                 if (quoteDocumentVersions == null)
                 {
                     quoteDocumentVersions = new List<QuoteOPDocumentVersionPM>();
                 }
                 return quoteDocumentVersions;
              }
             set { quoteDocumentVersions = value; }
	    }
		   
	   private List<QuoteOPDocumentVersionPM>  deletedQuoteDocumentVersions;
	   public virtual List<QuoteOPDocumentVersionPM> DeletedQuoteDocumentVersions  
	   {
	        get
             {
                 if ( deletedQuoteDocumentVersions == null)
                 {
                      deletedQuoteDocumentVersions = new List<QuoteOPDocumentVersionPM>();
                 }
                 return  deletedQuoteDocumentVersions;
              }
             set {  deletedQuoteDocumentVersions = value; }
	    }
	  	  private bool markFollowUpsAsDone ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool MarkFollowUpsAsDone  
	   {
	    
	     get
		{
		   return markFollowUpsAsDone;
		 }
		 set
		 {
		   if(markFollowUpsAsDone != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarkFollowUpsAsDone",OldValue=markFollowUpsAsDone,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   markFollowUpsAsDone=value;
		   }
			
		 }
	   }

	   private List<QuoteOPTotalVATPM> totalVATs;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("QuoteOPTotalVATPMQuote", "Id","QuoteId")]
	   [DataMember]
	   public virtual List<QuoteOPTotalVATPM> TotalVATs  
	   {
	        get
             {
                 if (totalVATs == null)
                 {
                     totalVATs = new List<QuoteOPTotalVATPM>();
                 }
                 return totalVATs;
              }
             set { totalVATs = value; }
	    }
		   
	   private List<QuoteOPTotalVATPM>  deletedTotalVATs;
	   public virtual List<QuoteOPTotalVATPM> DeletedTotalVATs  
	   {
	        get
             {
                 if ( deletedTotalVATs == null)
                 {
                      deletedTotalVATs = new List<QuoteOPTotalVATPM>();
                 }
                 return  deletedTotalVATs;
              }
             set {  deletedTotalVATs = value; }
	    }
	  	  private bool isQuoteDataExternal ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsQuoteDataExternal  
	   {
	    
	     get
		{
		   return isQuoteDataExternal;
		 }
		 set
		 {
		   if(isQuoteDataExternal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsQuoteDataExternal",OldValue=isQuoteDataExternal,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isQuoteDataExternal=value;
		   }
			
		 }
	   }
	  private bool isQuoteDocumentExternal ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsQuoteDocumentExternal  
	   {
	    
	     get
		{
		   return isQuoteDocumentExternal;
		 }
		 set
		 {
		   if(isQuoteDocumentExternal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsQuoteDocumentExternal",OldValue=isQuoteDocumentExternal,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isQuoteDocumentExternal=value;
		   }
			
		 }
	   }
	  private bool totalPerContainer ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TotalPerContainer  
	   {
	    
	     get
		{
		   return totalPerContainer;
		 }
		 set
		 {
		   if(totalPerContainer != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalPerContainer",OldValue=totalPerContainer,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   totalPerContainer=value;
		   }
			
		 }
	   }
	  private string quotationSections ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuotationSections  
	   {
	    
	     get
		{
		   return quotationSections;
		 }
		 set
		 {
		   if(quotationSections != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuotationSections",OldValue=quotationSections,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quotationSections=value;
		   }
			
		 }
	   }
	  private string sameOrFixed ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SameOrFixed  
	   {
	    
	     get
		{
		   return sameOrFixed;
		 }
		 set
		 {
		   if(sameOrFixed != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SameOrFixed",OldValue=sameOrFixed,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sameOrFixed=value;
		   }
			
		 }
	   }
	  private double? grossWeightInKG ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? GrossWeightInKG  
	   {
	    
	     get
		{
		   return grossWeightInKG;
		 }
		 set
		 {
		   if(grossWeightInKG != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeightInKG",OldValue=grossWeightInKG,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   grossWeightInKG=value;
		   }
			
		 }
	   }
	  private double? grossWeightPerTon ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? GrossWeightPerTon  
	   {
	    
	     get
		{
		   return grossWeightPerTon;
		 }
		 set
		 {
		   if(grossWeightPerTon != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeightPerTon",OldValue=grossWeightPerTon,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   grossWeightPerTon=value;
		   }
			
		 }
	   }
	  private string notifyId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyId  
	   {
	    
	     get
		{
		   return notifyId;
		 }
		 set
		 {
		   if(notifyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyId",OldValue=notifyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyId=value;
		   }
			
		 }
	   }
	  private string notifyAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyAddressId  
	   {
	    
	     get
		{
		   return notifyAddressId;
		 }
		 set
		 {
		   if(notifyAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyAddressId",OldValue=notifyAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyAddressId=value;
		   }
			
		 }
	   }
	  private string notifyContactId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyContactId  
	   {
	    
	     get
		{
		   return notifyContactId;
		 }
		 set
		 {
		   if(notifyContactId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyContactId",OldValue=notifyContactId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyContactId=value;
		   }
			
		 }
	   }
	  private string notifyName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyName  
	   {
	    
	     get
		{
		   return notifyName;
		 }
		 set
		 {
		   if(notifyName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyName",OldValue=notifyName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyName=value;
		   }
			
		 }
	   }
	  private double totalSaleIncludingVATAmountInSaleCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double TotalSaleIncludingVATAmountInSaleCurrency  
	   {
	    
	     get
		{
		   return totalSaleIncludingVATAmountInSaleCurrency;
		 }
		 set
		 {
		   if(totalSaleIncludingVATAmountInSaleCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalSaleIncludingVATAmountInSaleCurrency",OldValue=totalSaleIncludingVATAmountInSaleCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   totalSaleIncludingVATAmountInSaleCurrency=value;
		   }
			
		 }
	   }
	  private double totalSaleIncludingVATAmountInLocalCurrency ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double TotalSaleIncludingVATAmountInLocalCurrency  
	   {
	    
	     get
		{
		   return totalSaleIncludingVATAmountInLocalCurrency;
		 }
		 set
		 {
		   if(totalSaleIncludingVATAmountInLocalCurrency != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalSaleIncludingVATAmountInLocalCurrency",OldValue=totalSaleIncludingVATAmountInLocalCurrency,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   totalSaleIncludingVATAmountInLocalCurrency=value;
		   }
			
		 }
	   }
	  private int? numberOfFollowUps ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? NumberOfFollowUps  
	   {
	    
	     get
		{
		   return numberOfFollowUps;
		 }
		 set
		 {
		   if(numberOfFollowUps != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfFollowUps",OldValue=numberOfFollowUps,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   numberOfFollowUps=value;
		   }
			
		 }
	   }
	  private string notifyNote ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyNote  
	   {
	    
	     get
		{
		   return notifyNote;
		 }
		 set
		 {
		   if(notifyNote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyNote",OldValue=notifyNote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyNote=value;
		   }
			
		 }
	   }
	  private string notifyAddress1 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyAddress1  
	   {
	    
	     get
		{
		   return notifyAddress1;
		 }
		 set
		 {
		   if(notifyAddress1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyAddress1",OldValue=notifyAddress1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyAddress1=value;
		   }
			
		 }
	   }
	  private string notifyAddress2 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyAddress2  
	   {
	    
	     get
		{
		   return notifyAddress2;
		 }
		 set
		 {
		   if(notifyAddress2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyAddress2",OldValue=notifyAddress2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyAddress2=value;
		   }
			
		 }
	   }
	  private string notifyZipCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyZipCode  
	   {
	    
	     get
		{
		   return notifyZipCode;
		 }
		 set
		 {
		   if(notifyZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyZipCode",OldValue=notifyZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyZipCode=value;
		   }
			
		 }
	   }
	  private string notifyStateId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyStateId  
	   {
	    
	     get
		{
		   return notifyStateId;
		 }
		 set
		 {
		   if(notifyStateId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyStateId",OldValue=notifyStateId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyStateId=value;
		   }
			
		 }
	   }
	  private string notifyCountryId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyCountryId  
	   {
	    
	     get
		{
		   return notifyCountryId;
		 }
		 set
		 {
		   if(notifyCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyCountryId",OldValue=notifyCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyCountryId=value;
		   }
			
		 }
	   }
	  private string notifyCity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotifyCity  
	   {
	    
	     get
		{
		   return notifyCity;
		 }
		 set
		 {
		   if(notifyCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotifyCity",OldValue=notifyCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notifyCity=value;
		   }
			
		 }
	   }
	  private bool dontExportQuotationsToIntegratedSystem ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool DontExportQuotationsToIntegratedSystem  
	   {
	    
	     get
		{
		   return dontExportQuotationsToIntegratedSystem;
		 }
		 set
		 {
		   if(dontExportQuotationsToIntegratedSystem != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DontExportQuotationsToIntegratedSystem",OldValue=dontExportQuotationsToIntegratedSystem,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   dontExportQuotationsToIntegratedSystem=value;
		   }
			
		 }
	   }
	  private string quoteLevel ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteLevel  
	   {
	    
	     get
		{
		   return quoteLevel;
		 }
		 set
		 {
		   if(quoteLevel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteLevel",OldValue=quoteLevel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteLevel=value;
		   }
			
		 }
	   }
	  private bool convertToLCL ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ConvertToLCL  
	   {
	    
	     get
		{
		   return convertToLCL;
		 }
		 set
		 {
		   if(convertToLCL != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConvertToLCL",OldValue=convertToLCL,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   convertToLCL=value;
		   }
			
		 }
	   }
	  private bool convertToFCL ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ConvertToFCL  
	   {
	    
	     get
		{
		   return convertToFCL;
		 }
		 set
		 {
		   if(convertToFCL != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConvertToFCL",OldValue=convertToFCL,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   convertToFCL=value;
		   }
			
		 }
	   }
	  private bool grossWeightEdited ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool GrossWeightEdited  
	   {
	    
	     get
		{
		   return grossWeightEdited;
		 }
		 set
		 {
		   if(grossWeightEdited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeightEdited",OldValue=grossWeightEdited,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   grossWeightEdited=value;
		   }
			
		 }
	   }
	  private bool chargeableWeightEdited ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ChargeableWeightEdited  
	   {
	    
	     get
		{
		   return chargeableWeightEdited;
		 }
		 set
		 {
		   if(chargeableWeightEdited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeightEdited",OldValue=chargeableWeightEdited,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   chargeableWeightEdited=value;
		   }
			
		 }
	   }
	  private double? chargeableWeightInKG ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ChargeableWeightInKG  
	   {
	    
	     get
		{
		   return chargeableWeightInKG;
		 }
		 set
		 {
		   if(chargeableWeightInKG != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeightInKG",OldValue=chargeableWeightInKG,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   chargeableWeightInKG=value;
		   }
			
		 }
	   }
	  private double? volumeInCBM ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? VolumeInCBM  
	   {
	    
	     get
		{
		   return volumeInCBM;
		 }
		 set
		 {
		   if(volumeInCBM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumeInCBM",OldValue=volumeInCBM,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   volumeInCBM=value;
		   }
			
		 }
	   }
	  private DateTime? startDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private bool isCopyExchangeRates ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCopyExchangeRates  
	   {
	    
	     get
		{
		   return isCopyExchangeRates;
		 }
		 set
		 {
		   if(isCopyExchangeRates != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCopyExchangeRates",OldValue=isCopyExchangeRates,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCopyExchangeRates=value;
		   }
			
		 }
	   }
	  private string quoteVersion ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteVersion  
	   {
	    
	     get
		{
		   return quoteVersion;
		 }
		 set
		 {
		   if(quoteVersion != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteVersion",OldValue=quoteVersion,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteVersion=value;
		   }
			
		 }
	   }
	  private string field11 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field11  
	   {
	    
	     get
		{
		   return field11;
		 }
		 set
		 {
		   if(field11 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field11",OldValue=field11,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field11=value;
		   }
			
		 }
	   }
	  private string field12 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field12  
	   {
	    
	     get
		{
		   return field12;
		 }
		 set
		 {
		   if(field12 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field12",OldValue=field12,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field12=value;
		   }
			
		 }
	   }
	  private string field13 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field13  
	   {
	    
	     get
		{
		   return field13;
		 }
		 set
		 {
		   if(field13 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field13",OldValue=field13,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field13=value;
		   }
			
		 }
	   }
	  private string field14 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field14  
	   {
	    
	     get
		{
		   return field14;
		 }
		 set
		 {
		   if(field14 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field14",OldValue=field14,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field14=value;
		   }
			
		 }
	   }
	  private string field15 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field15  
	   {
	    
	     get
		{
		   return field15;
		 }
		 set
		 {
		   if(field15 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field15",OldValue=field15,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field15=value;
		   }
			
		 }
	   }
	  private string field16 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field16  
	   {
	    
	     get
		{
		   return field16;
		 }
		 set
		 {
		   if(field16 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field16",OldValue=field16,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field16=value;
		   }
			
		 }
	   }
	  private string field17 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field17  
	   {
	    
	     get
		{
		   return field17;
		 }
		 set
		 {
		   if(field17 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field17",OldValue=field17,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field17=value;
		   }
			
		 }
	   }
	  private string field18 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field18  
	   {
	    
	     get
		{
		   return field18;
		 }
		 set
		 {
		   if(field18 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field18",OldValue=field18,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field18=value;
		   }
			
		 }
	   }
	  private string field19 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field19  
	   {
	    
	     get
		{
		   return field19;
		 }
		 set
		 {
		   if(field19 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field19",OldValue=field19,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field19=value;
		   }
			
		 }
	   }
	  private string field20 ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Field20  
	   {
	    
	     get
		{
		   return field20;
		 }
		 set
		 {
		   if(field20 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Field20",OldValue=field20,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   field20=value;
		   }
			
		 }
	   }
	  private DateTime? requestDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? RequestDate  
	   {
	    
	     get
		{
		   return requestDate;
		 }
		 set
		 {
		   if(requestDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestDate",OldValue=requestDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   requestDate=value;
		   }
			
		 }
	   }
	  private bool isCreatedFromTicket ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCreatedFromTicket  
	   {
	    
	     get
		{
		   return isCreatedFromTicket;
		 }
		 set
		 {
		   if(isCreatedFromTicket != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCreatedFromTicket",OldValue=isCreatedFromTicket,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCreatedFromTicket=value;
		   }
			
		 }
	   }
	  private DateTime? ticketCreateDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? TicketCreateDate  
	   {
	    
	     get
		{
		   return ticketCreateDate;
		 }
		 set
		 {
		   if(ticketCreateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketCreateDate",OldValue=ticketCreateDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   ticketCreateDate=value;
		   }
			
		 }
	   }
	  private double? estimatedProfitInLocal ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? EstimatedProfitInLocal  
	   {
	    
	     get
		{
		   return estimatedProfitInLocal;
		 }
		 set
		 {
		   if(estimatedProfitInLocal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedProfitInLocal",OldValue=estimatedProfitInLocal,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   estimatedProfitInLocal=value;
		   }
			
		 }
	   }
	  private double? estimatedProfitInProfit ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? EstimatedProfitInProfit  
	   {
	    
	     get
		{
		   return estimatedProfitInProfit;
		 }
		 set
		 {
		   if(estimatedProfitInProfit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedProfitInProfit",OldValue=estimatedProfitInProfit,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   estimatedProfitInProfit=value;
		   }
			
		 }
	   }
	  private string profitCurrencyId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ProfitCurrencyId  
	   {
	    
	     get
		{
		   return profitCurrencyId;
		 }
		 set
		 {
		   if(profitCurrencyId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProfitCurrencyId",OldValue=profitCurrencyId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   profitCurrencyId=value;
		   }
			
		 }
	   }
	  private double? profitExchangeRate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? ProfitExchangeRate  
	   {
	    
	     get
		{
		   return profitExchangeRate;
		 }
		 set
		 {
		   if(profitExchangeRate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ProfitExchangeRate",OldValue=profitExchangeRate,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   profitExchangeRate=value;
		   }
			
		 }
	   }
	  private string countryForStatisticsId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CountryForStatisticsId  
	   {
	    
	     get
		{
		   return countryForStatisticsId;
		 }
		 set
		 {
		   if(countryForStatisticsId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryForStatisticsId",OldValue=countryForStatisticsId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   countryForStatisticsId=value;
		   }
			
		 }
	   }
	  private string externalEntityNumber ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExternalEntityNumber  
	   {
	    
	     get
		{
		   return externalEntityNumber;
		 }
		 set
		 {
		   if(externalEntityNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExternalEntityNumber",OldValue=externalEntityNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   externalEntityNumber=value;
		   }
			
		 }
	   }
	  private string quoteHTMLDocumentId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteHTMLDocumentId  
	   {
	    
	     get
		{
		   return quoteHTMLDocumentId;
		 }
		 set
		 {
		   if(quoteHTMLDocumentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteHTMLDocumentId",OldValue=quoteHTMLDocumentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteHTMLDocumentId=value;
		   }
			
		 }
	   }
	  private string quoteClosingReasonId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteClosingReasonId  
	   {
	    
	     get
		{
		   return quoteClosingReasonId;
		 }
		 set
		 {
		   if(quoteClosingReasonId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteClosingReasonId",OldValue=quoteClosingReasonId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteClosingReasonId=value;
		   }
			
		 }
	   }
	  private string shipmentSubTypeId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentSubTypeId  
	   {
	    
	     get
		{
		   return shipmentSubTypeId;
		 }
		 set
		 {
		   if(shipmentSubTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentSubTypeId",OldValue=shipmentSubTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentSubTypeId=value;
		   }
			
		 }
	   }
	  private string shipmentSubTypeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentSubTypeName  
	   {
	    
	     get
		{
		   return shipmentSubTypeName;
		 }
		 set
		 {
		   if(shipmentSubTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentSubTypeName",OldValue=shipmentSubTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentSubTypeName=value;
		   }
			
		 }
	   }
	  private string deliveryCity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliveryCity  
	   {
	    
	     get
		{
		   return deliveryCity;
		 }
		 set
		 {
		   if(deliveryCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveryCity",OldValue=deliveryCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliveryCity=value;
		   }
			
		 }
	   }
	  private string deliveryCountryId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliveryCountryId  
	   {
	    
	     get
		{
		   return deliveryCountryId;
		 }
		 set
		 {
		   if(deliveryCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveryCountryId",OldValue=deliveryCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliveryCountryId=value;
		   }
			
		 }
	   }
	  private string deliveryZipCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliveryZipCode  
	   {
	    
	     get
		{
		   return deliveryZipCode;
		 }
		 set
		 {
		   if(deliveryZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveryZipCode",OldValue=deliveryZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliveryZipCode=value;
		   }
			
		 }
	   }
	  private string pickupCity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PickupCity  
	   {
	    
	     get
		{
		   return pickupCity;
		 }
		 set
		 {
		   if(pickupCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupCity",OldValue=pickupCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pickupCity=value;
		   }
			
		 }
	   }
	  private string pickupCountryId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PickupCountryId  
	   {
	    
	     get
		{
		   return pickupCountryId;
		 }
		 set
		 {
		   if(pickupCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupCountryId",OldValue=pickupCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pickupCountryId=value;
		   }
			
		 }
	   }
	  private string pickupZipCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PickupZipCode  
	   {
	    
	     get
		{
		   return pickupZipCode;
		 }
		 set
		 {
		   if(pickupZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupZipCode",OldValue=pickupZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pickupZipCode=value;
		   }
			
		 }
	   }
	  private double? pickupDeliveryRatio ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? PickupDeliveryRatio  
	   {
	    
	     get
		{
		   return pickupDeliveryRatio;
		 }
		 set
		 {
		   if(pickupDeliveryRatio != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupDeliveryRatio",OldValue=pickupDeliveryRatio,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   pickupDeliveryRatio=value;
		   }
			
		 }
	   }
	  private double? pickupDeliveryChargeableWeight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? PickupDeliveryChargeableWeight  
	   {
	    
	     get
		{
		   return pickupDeliveryChargeableWeight;
		 }
		 set
		 {
		   if(pickupDeliveryChargeableWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupDeliveryChargeableWeight",OldValue=pickupDeliveryChargeableWeight,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   pickupDeliveryChargeableWeight=value;
		   }
			
		 }
	   }
	  private double? pickupDeliveryVolumetricWeight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? PickupDeliveryVolumetricWeight  
	   {
	    
	     get
		{
		   return pickupDeliveryVolumetricWeight;
		 }
		 set
		 {
		   if(pickupDeliveryVolumetricWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupDeliveryVolumetricWeight",OldValue=pickupDeliveryVolumetricWeight,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   pickupDeliveryVolumetricWeight=value;
		   }
			
		 }
	   }
	  private string pickupDeliveryCWeightUnitCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PickupDeliveryCWeightUnitCode  
	   {
	    
	     get
		{
		   return pickupDeliveryCWeightUnitCode;
		 }
		 set
		 {
		   if(pickupDeliveryCWeightUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupDeliveryCWeightUnitCode",OldValue=pickupDeliveryCWeightUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pickupDeliveryCWeightUnitCode=value;
		   }
			
		 }
	   }
	  private string regionalTaxId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string RegionalTaxId  
	   {
	    
	     get
		{
		   return regionalTaxId;
		 }
		 set
		 {
		   if(regionalTaxId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RegionalTaxId",OldValue=regionalTaxId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   regionalTaxId=value;
		   }
			
		 }
	   }
	  private double? regionalTaxPercentage ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? RegionalTaxPercentage  
	   {
	    
	     get
		{
		   return regionalTaxPercentage;
		 }
		 set
		 {
		   if(regionalTaxPercentage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RegionalTaxPercentage",OldValue=regionalTaxPercentage,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   regionalTaxPercentage=value;
		   }
			
		 }
	   }
	  private bool descriptionRightToLeft ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool DescriptionRightToLeft  
	   {
	    
	     get
		{
		   return descriptionRightToLeft;
		 }
		 set
		 {
		   if(descriptionRightToLeft != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DescriptionRightToLeft",OldValue=descriptionRightToLeft,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   descriptionRightToLeft=value;
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
	  private bool isRefreshFollowUp ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsRefreshFollowUp  
	   {
	    
	     get
		{
		   return isRefreshFollowUp;
		 }
		 set
		 {
		   if(isRefreshFollowUp != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsRefreshFollowUp",OldValue=isRefreshFollowUp,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isRefreshFollowUp=value;
		   }
			
		 }
	   }
	  private bool isRefreshQuoteFollowUps ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsRefreshQuoteFollowUps  
	   {
	    
	     get
		{
		   return isRefreshQuoteFollowUps;
		 }
		 set
		 {
		   if(isRefreshQuoteFollowUps != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsRefreshQuoteFollowUps",OldValue=isRefreshQuoteFollowUps,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isRefreshQuoteFollowUps=value;
		   }
			
		 }
	   }
	  private bool convertTransportMode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ConvertTransportMode  
	   {
	    
	     get
		{
		   return convertTransportMode;
		 }
		 set
		 {
		   if(convertTransportMode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConvertTransportMode",OldValue=convertTransportMode,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   convertTransportMode=value;
		   }
			
		 }
	   }
	  private string specialServiceId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecialServiceId  
	   {
	    
	     get
		{
		   return specialServiceId;
		 }
		 set
		 {
		   if(specialServiceId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecialServiceId",OldValue=specialServiceId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specialServiceId=value;
		   }
			
		 }
	   }
	  private string specialServiceName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecialServiceName  
	   {
	    
	     get
		{
		   return specialServiceName;
		 }
		 set
		 {
		   if(specialServiceName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecialServiceName",OldValue=specialServiceName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specialServiceName=value;
		   }
			
		 }
	   }
   }
   
}
	 