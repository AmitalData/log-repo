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
   public partial class PaymentOrderPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string paymentNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentNumber  
	   {
	    
	     get
		{
		   return paymentNumber;
		 }
		 set
		 {
		   if(paymentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentNumber",OldValue=paymentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentNumber=value;
		   }
			
		 }
	   }
	  private decimal? totalSumToPay ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TotalSumToPay  
	   {
	    
	     get
		{
		   return totalSumToPay;
		 }
		 set
		 {
		   if(totalSumToPay != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalSumToPay",OldValue=totalSumToPay,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   totalSumToPay=value;
		   }
			
		 }
	   }
	  private DateTime? lastPayDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastPayDate  
	   {
	    
	     get
		{
		   return lastPayDate;
		 }
		 set
		 {
		   if(lastPayDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastPayDate",OldValue=lastPayDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastPayDate=value;
		   }
			
		 }
	   }
	  private string reason ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Reason  
	   {
	    
	     get
		{
		   return reason;
		 }
		 set
		 {
		   if(reason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Reason",OldValue=reason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   reason=value;
		   }
			
		 }
	   }
	  private string customerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string customerActivityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerActivityTypeCode  
	   {
	    
	     get
		{
		   return customerActivityTypeCode;
		 }
		 set
		 {
		   if(customerActivityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerActivityTypeCode",OldValue=customerActivityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerActivityTypeCode=value;
		   }
			
		 }
	   }
	  private string paymentOrderTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentOrderTypeCode  
	   {
	    
	     get
		{
		   return paymentOrderTypeCode;
		 }
		 set
		 {
		   if(paymentOrderTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderTypeCode",OldValue=paymentOrderTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentOrderTypeCode=value;
		   }
			
		 }
	   }
	  private string paymentProcessCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentProcessCode  
	   {
	    
	     get
		{
		   return paymentProcessCode;
		 }
		 set
		 {
		   if(paymentProcessCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentProcessCode",OldValue=paymentProcessCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentProcessCode=value;
		   }
			
		 }
	   }
	  private string paymentStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentStatusCode  
	   {
	    
	     get
		{
		   return paymentStatusCode;
		 }
		 set
		 {
		   if(paymentStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentStatusCode",OldValue=paymentStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentStatusCode=value;
		   }
			
		 }
	   }
	  private string customsHouseCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsHouseCode  
	   {
	    
	     get
		{
		   return customsHouseCode;
		 }
		 set
		 {
		   if(customsHouseCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsHouseCode",OldValue=customsHouseCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsHouseCode=value;
		   }
			
		 }
	   }
	  private DateTime? actualPayDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ActualPayDate  
	   {
	    
	     get
		{
		   return actualPayDate;
		 }
		 set
		 {
		   if(actualPayDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActualPayDate",OldValue=actualPayDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   actualPayDate=value;
		   }
			
		 }
	   }
	  private string internalNotes ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InternalNotes  
	   {
	    
	     get
		{
		   return internalNotes;
		 }
		 set
		 {
		   if(internalNotes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InternalNotes",OldValue=internalNotes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   internalNotes=value;
		   }
			
		 }
	   }
	  private DateTime? createDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime?"};
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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

	   private List<PaymentOrderLinePM> paymentOrderLines;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("PaymentOrderPaymentOrderLines", "Id","PaymentOrderId")]
	   [DataMember]
	   public virtual List<PaymentOrderLinePM> PaymentOrderLines  
	   {
	        get
             {
                 if (paymentOrderLines == null)
                 {
                     paymentOrderLines = new List<PaymentOrderLinePM>();
                 }
                 return paymentOrderLines;
              }
             set { paymentOrderLines = value; }
	    }
		   
	   private List<PaymentOrderLinePM>  deletedPaymentOrderLines;
	   public virtual List<PaymentOrderLinePM> DeletedPaymentOrderLines  
	   {
	        get
             {
                 if ( deletedPaymentOrderLines == null)
                 {
                      deletedPaymentOrderLines = new List<PaymentOrderLinePM>();
                 }
                 return  deletedPaymentOrderLines;
              }
             set {  deletedPaymentOrderLines = value; }
	    }
	  
	   private List<PaymentOrderMethodPM> paymentOrderMethods;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("PaymentOrderPaymentOrderMethod", "Id","PaymentOrderId")]
	   [DataMember]
	   public virtual List<PaymentOrderMethodPM> PaymentOrderMethods  
	   {
	        get
             {
                 if (paymentOrderMethods == null)
                 {
                     paymentOrderMethods = new List<PaymentOrderMethodPM>();
                 }
                 return paymentOrderMethods;
              }
             set { paymentOrderMethods = value; }
	    }
		   
	   private List<PaymentOrderMethodPM>  deletedPaymentOrderMethods;
	   public virtual List<PaymentOrderMethodPM> DeletedPaymentOrderMethods  
	   {
	        get
             {
                 if ( deletedPaymentOrderMethods == null)
                 {
                      deletedPaymentOrderMethods = new List<PaymentOrderMethodPM>();
                 }
                 return  deletedPaymentOrderMethods;
              }
             set {  deletedPaymentOrderMethods = value; }
	    }
	  
	   private List<PaymentOrderProtestReasonPM> paymentOrderProtestReasons;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("PaymentOrderPaymentOrderProtestReason", "Id","PaymentOrderId")]
	   [DataMember]
	   public virtual List<PaymentOrderProtestReasonPM> PaymentOrderProtestReasons  
	   {
	        get
             {
                 if (paymentOrderProtestReasons == null)
                 {
                     paymentOrderProtestReasons = new List<PaymentOrderProtestReasonPM>();
                 }
                 return paymentOrderProtestReasons;
              }
             set { paymentOrderProtestReasons = value; }
	    }
		   
	   private List<PaymentOrderProtestReasonPM>  deletedPaymentOrderProtestReasons;
	   public virtual List<PaymentOrderProtestReasonPM> DeletedPaymentOrderProtestReasons  
	   {
	        get
             {
                 if ( deletedPaymentOrderProtestReasons == null)
                 {
                      deletedPaymentOrderProtestReasons = new List<PaymentOrderProtestReasonPM>();
                 }
                 return  deletedPaymentOrderProtestReasons;
              }
             set {  deletedPaymentOrderProtestReasons = value; }
	    }
	  	  private string customerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string paymentStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentStatusName  
	   {
	    
	     get
		{
		   return paymentStatusName;
		 }
		 set
		 {
		   if(paymentStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentStatusName",OldValue=paymentStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentStatusName=value;
		   }
			
		 }
	   }
	  private string paymentOrderTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentOrderTypeName  
	   {
	    
	     get
		{
		   return paymentOrderTypeName;
		 }
		 set
		 {
		   if(paymentOrderTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderTypeName",OldValue=paymentOrderTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentOrderTypeName=value;
		   }
			
		 }
	   }
	  private string paymentProcessName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentProcessName  
	   {
	    
	     get
		{
		   return paymentProcessName;
		 }
		 set
		 {
		   if(paymentProcessName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentProcessName",OldValue=paymentProcessName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentProcessName=value;
		   }
			
		 }
	   }
	  private string customsEntityTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsEntityTypeCode  
	   {
	    
	     get
		{
		   return customsEntityTypeCode;
		 }
		 set
		 {
		   if(customsEntityTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsEntityTypeCode",OldValue=customsEntityTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsEntityTypeCode=value;
		   }
			
		 }
	   }
	  private string firstEntityID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FirstEntityID  
	   {
	    
	     get
		{
		   return firstEntityID;
		 }
		 set
		 {
		   if(firstEntityID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstEntityID",OldValue=firstEntityID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   firstEntityID=value;
		   }
			
		 }
	   }
	  private string secondEntityID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SecondEntityID  
	   {
	    
	     get
		{
		   return secondEntityID;
		 }
		 set
		 {
		   if(secondEntityID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SecondEntityID",OldValue=secondEntityID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   secondEntityID=value;
		   }
			
		 }
	   }
	  private string thirdEntityID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ThirdEntityID  
	   {
	    
	     get
		{
		   return thirdEntityID;
		 }
		 set
		 {
		   if(thirdEntityID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ThirdEntityID",OldValue=thirdEntityID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   thirdEntityID=value;
		   }
			
		 }
	   }
	  private string importerId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterId  
	   {
	    
	     get
		{
		   return importerId;
		 }
		 set
		 {
		   if(importerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterId",OldValue=importerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerId=value;
		   }
			
		 }
	   }
	  private string importerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterName  
	   {
	    
	     get
		{
		   return importerName;
		 }
		 set
		 {
		   if(importerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterName",OldValue=importerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerName=value;
		   }
			
		 }
	   }
	  private bool isClosed ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string customerActivityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomerActivityTypeName  
	   {
	    
	     get
		{
		   return customerActivityTypeName;
		 }
		 set
		 {
		   if(customerActivityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerActivityTypeName",OldValue=customerActivityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customerActivityTypeName=value;
		   }
			
		 }
	   }
	  private string customsEntityTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsEntityTypeName  
	   {
	    
	     get
		{
		   return customsEntityTypeName;
		 }
		 set
		 {
		   if(customsEntityTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsEntityTypeName",OldValue=customsEntityTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsEntityTypeName=value;
		   }
			
		 }
	   }
	  private string customsHouseName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsHouseName  
	   {
	    
	     get
		{
		   return customsHouseName;
		 }
		 set
		 {
		   if(customsHouseName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsHouseName",OldValue=customsHouseName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsHouseName=value;
		   }
			
		 }
	   }

	   private List<PaymentOrderConnectionTablePM> paymentOrderConnectionTables;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("PaymentOrderPaymentOrderConnection", "Id","PaymentOrderId")]
	   [DataMember]
	   public virtual List<PaymentOrderConnectionTablePM> PaymentOrderConnectionTables  
	   {
	        get
             {
                 if (paymentOrderConnectionTables == null)
                 {
                     paymentOrderConnectionTables = new List<PaymentOrderConnectionTablePM>();
                 }
                 return paymentOrderConnectionTables;
              }
             set { paymentOrderConnectionTables = value; }
	    }
		   
	   private List<PaymentOrderConnectionTablePM>  deletedPaymentOrderConnectionTables;
	   public virtual List<PaymentOrderConnectionTablePM> DeletedPaymentOrderConnectionTables  
	   {
	        get
             {
                 if ( deletedPaymentOrderConnectionTables == null)
                 {
                      deletedPaymentOrderConnectionTables = new List<PaymentOrderConnectionTablePM>();
                 }
                 return  deletedPaymentOrderConnectionTables;
              }
             set {  deletedPaymentOrderConnectionTables = value; }
	    }
	  	  private bool hasDeficit ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasDeficit  
	   {
	    
	     get
		{
		   return hasDeficit;
		 }
		 set
		 {
		   if(hasDeficit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasDeficit",OldValue=hasDeficit,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasDeficit=value;
		   }
			
		 }
	   }
	  private bool hasDeposit ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasDeposit  
	   {
	    
	     get
		{
		   return hasDeposit;
		 }
		 set
		 {
		   if(hasDeposit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasDeposit",OldValue=hasDeposit,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasDeposit=value;
		   }
			
		 }
	   }
	  private string concurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string newConcurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NewConcurrencyGUID  
	   {
	    
	     get
		{
		   return newConcurrencyGUID;
		 }
		 set
		 {
		   if(newConcurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NewConcurrencyGUID",OldValue=newConcurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   newConcurrencyGUID=value;
		   }
			
		 }
	   }
	  private string accountingCustomFile ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountingCustomFile  
	   {
	    
	     get
		{
		   return accountingCustomFile;
		 }
		 set
		 {
		   if(accountingCustomFile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountingCustomFile",OldValue=accountingCustomFile,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountingCustomFile=value;
		   }
			
		 }
	   }
	  private string customFiles ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomFiles  
	   {
	    
	     get
		{
		   return customFiles;
		 }
		 set
		 {
		   if(customFiles != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomFiles",OldValue=customFiles,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customFiles=value;
		   }
			
		 }
	   }
	  private string paymentOrderSelectedLabel ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentOrderSelectedLabel  
	   {
	    
	     get
		{
		   return paymentOrderSelectedLabel;
		 }
		 set
		 {
		   if(paymentOrderSelectedLabel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderSelectedLabel",OldValue=paymentOrderSelectedLabel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentOrderSelectedLabel=value;
		   }
			
		 }
	   }
	  private decimal? paymentOrderLeftAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? PaymentOrderLeftAmount  
	   {
	    
	     get
		{
		   return paymentOrderLeftAmount;
		 }
		 set
		 {
		   if(paymentOrderLeftAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderLeftAmount",OldValue=paymentOrderLeftAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   paymentOrderLeftAmount=value;
		   }
			
		 }
	   }
	  private string documentPaymentId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DocumentPaymentId  
	   {
	    
	     get
		{
		   return documentPaymentId;
		 }
		 set
		 {
		   if(documentPaymentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DocumentPaymentId",OldValue=documentPaymentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   documentPaymentId=value;
		   }
			
		 }
	   }
	  private bool customerChanged ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CustomerChanged  
	   {
	    
	     get
		{
		   return customerChanged;
		 }
		 set
		 {
		   if(customerChanged != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomerChanged",OldValue=customerChanged,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   customerChanged=value;
		   }
			
		 }
	   }
   }
   
}
	 