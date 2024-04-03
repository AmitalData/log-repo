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
   public partial class CustomsCollateralPM : EntityPM
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
	  private string collateralRequestNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CollateralRequestNumber  
	   {
	    
	     get
		{
		   return collateralRequestNumber;
		 }
		 set
		 {
		   if(collateralRequestNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CollateralRequestNumber",OldValue=collateralRequestNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   collateralRequestNumber=value;
		   }
			
		 }
	   }
	  private DateTime? requestValidityDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? RequestValidityDate  
	   {
	    
	     get
		{
		   return requestValidityDate;
		 }
		 set
		 {
		   if(requestValidityDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestValidityDate",OldValue=requestValidityDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   requestValidityDate=value;
		   }
			
		 }
	   }
	  private DateTime? collateralValidityDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CollateralValidityDate  
	   {
	    
	     get
		{
		   return collateralValidityDate;
		 }
		 set
		 {
		   if(collateralValidityDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CollateralValidityDate",OldValue=collateralValidityDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   collateralValidityDate=value;
		   }
			
		 }
	   }
	  private string collateralRequestStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CollateralRequestStatusCode  
	   {
	    
	     get
		{
		   return collateralRequestStatusCode;
		 }
		 set
		 {
		   if(collateralRequestStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CollateralRequestStatusCode",OldValue=collateralRequestStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   collateralRequestStatusCode=value;
		   }
			
		 }
	   }
	  private string requestedCollateralTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestedCollateralTypeCode  
	   {
	    
	     get
		{
		   return requestedCollateralTypeCode;
		 }
		 set
		 {
		   if(requestedCollateralTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestedCollateralTypeCode",OldValue=requestedCollateralTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestedCollateralTypeCode=value;
		   }
			
		 }
	   }
	  private string organizationUnitTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OrganizationUnitTypeCode  
	   {
	    
	     get
		{
		   return organizationUnitTypeCode;
		 }
		 set
		 {
		   if(organizationUnitTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OrganizationUnitTypeCode",OldValue=organizationUnitTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   organizationUnitTypeCode=value;
		   }
			
		 }
	   }
	  private string customsHouseTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsHouseTypeCode  
	   {
	    
	     get
		{
		   return customsHouseTypeCode;
		 }
		 set
		 {
		   if(customsHouseTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsHouseTypeCode",OldValue=customsHouseTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsHouseTypeCode=value;
		   }
			
		 }
	   }
	  private string workerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string WorkerName  
	   {
	    
	     get
		{
		   return workerName;
		 }
		 set
		 {
		   if(workerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WorkerName",OldValue=workerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   workerName=value;
		   }
			
		 }
	   }
	  private string remarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Remarks  
	   {
	    
	     get
		{
		   return remarks;
		 }
		 set
		 {
		   if(remarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Remarks",OldValue=remarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   remarks=value;
		   }
			
		 }
	   }
	  private string fileNo ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FileNo  
	   {
	    
	     get
		{
		   return fileNo;
		 }
		 set
		 {
		   if(fileNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileNo",OldValue=fileNo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fileNo=value;
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
	  private string entityIdKey1 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityIdKey1  
	   {
	    
	     get
		{
		   return entityIdKey1;
		 }
		 set
		 {
		   if(entityIdKey1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityIdKey1",OldValue=entityIdKey1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityIdKey1=value;
		   }
			
		 }
	   }
	  private string entityIdKey2 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityIdKey2  
	   {
	    
	     get
		{
		   return entityIdKey2;
		 }
		 set
		 {
		   if(entityIdKey2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityIdKey2",OldValue=entityIdKey2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityIdKey2=value;
		   }
			
		 }
	   }
	  private string entityIdKey3 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityIdKey3  
	   {
	    
	     get
		{
		   return entityIdKey3;
		 }
		 set
		 {
		   if(entityIdKey3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityIdKey3",OldValue=entityIdKey3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityIdKey3=value;
		   }
			
		 }
	   }

	   private List<CustomsCollateralsConditionPM> customsCollateralsConditions;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("CustomsCollateralsCustomsCollateralsConditions", "Id","CustomsCollateralId")]
	   [DataMember]
	   public virtual List<CustomsCollateralsConditionPM> CustomsCollateralsConditions  
	   {
	        get
             {
                 if (customsCollateralsConditions == null)
                 {
                     customsCollateralsConditions = new List<CustomsCollateralsConditionPM>();
                 }
                 return customsCollateralsConditions;
              }
             set { customsCollateralsConditions = value; }
	    }
		   
	   private List<CustomsCollateralsConditionPM>  deletedCustomsCollateralsConditions;
	   public virtual List<CustomsCollateralsConditionPM> DeletedCustomsCollateralsConditions  
	   {
	        get
             {
                 if ( deletedCustomsCollateralsConditions == null)
                 {
                      deletedCustomsCollateralsConditions = new List<CustomsCollateralsConditionPM>();
                 }
                 return  deletedCustomsCollateralsConditions;
              }
             set {  deletedCustomsCollateralsConditions = value; }
	    }
	  
	   private List<CustomsCollateralsAnswerPM> customsCollateralsAnswers;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("CustomsCollateralsCustomsCollateralsAnswers", "Id","CustomsCollateralId")]
	   [DataMember]
	   public virtual List<CustomsCollateralsAnswerPM> CustomsCollateralsAnswers  
	   {
	        get
             {
                 if (customsCollateralsAnswers == null)
                 {
                     customsCollateralsAnswers = new List<CustomsCollateralsAnswerPM>();
                 }
                 return customsCollateralsAnswers;
              }
             set { customsCollateralsAnswers = value; }
	    }
		   
	   private List<CustomsCollateralsAnswerPM>  deletedCustomsCollateralsAnswers;
	   public virtual List<CustomsCollateralsAnswerPM> DeletedCustomsCollateralsAnswers  
	   {
	        get
             {
                 if ( deletedCustomsCollateralsAnswers == null)
                 {
                      deletedCustomsCollateralsAnswers = new List<CustomsCollateralsAnswerPM>();
                 }
                 return  deletedCustomsCollateralsAnswers;
              }
             set {  deletedCustomsCollateralsAnswers = value; }
	    }
	  	  private string collateralRequestStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CollateralRequestStatusName  
	   {
	    
	     get
		{
		   return collateralRequestStatusName;
		 }
		 set
		 {
		   if(collateralRequestStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CollateralRequestStatusName",OldValue=collateralRequestStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   collateralRequestStatusName=value;
		   }
			
		 }
	   }
	  private string requestedCollateralTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestedCollateralTypeName  
	   {
	    
	     get
		{
		   return requestedCollateralTypeName;
		 }
		 set
		 {
		   if(requestedCollateralTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestedCollateralTypeName",OldValue=requestedCollateralTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestedCollateralTypeName=value;
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
	  private bool includingThirdPartyGuarantee ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IncludingThirdPartyGuarantee  
	   {
	    
	     get
		{
		   return includingThirdPartyGuarantee;
		 }
		 set
		 {
		   if(includingThirdPartyGuarantee != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IncludingThirdPartyGuarantee",OldValue=includingThirdPartyGuarantee,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   includingThirdPartyGuarantee=value;
		   }
			
		 }
	   }
	  private string declarationId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationId  
	   {
	    
	     get
		{
		   return declarationId;
		 }
		 set
		 {
		   if(declarationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationId",OldValue=declarationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationId=value;
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
	  private string customsHouseTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsHouseTypeName  
	   {
	    
	     get
		{
		   return customsHouseTypeName;
		 }
		 set
		 {
		   if(customsHouseTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsHouseTypeName",OldValue=customsHouseTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsHouseTypeName=value;
		   }
			
		 }
	   }
	  private string organizationUnitTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OrganizationUnitTypeName  
	   {
	    
	     get
		{
		   return organizationUnitTypeName;
		 }
		 set
		 {
		   if(organizationUnitTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OrganizationUnitTypeName",OldValue=organizationUnitTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   organizationUnitTypeName=value;
		   }
			
		 }
	   }
	  private DateTime? createDateTime ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CreateDateTime  
	   {
	    
	     get
		{
		   return createDateTime;
		 }
		 set
		 {
		   if(createDateTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDateTime",OldValue=createDateTime,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   createDateTime=value;
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
	  private string paymentOrderId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PaymentOrderId  
	   {
	    
	     get
		{
		   return paymentOrderId;
		 }
		 set
		 {
		   if(paymentOrderId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PaymentOrderId",OldValue=paymentOrderId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   paymentOrderId=value;
		   }
			
		 }
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
	  private bool isAnswer ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsAnswer  
	   {
	    
	     get
		{
		   return isAnswer;
		 }
		 set
		 {
		   if(isAnswer != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsAnswer",OldValue=isAnswer,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isAnswer=value;
		   }
			
		 }
	   }
	  private string direction ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Direction  
	   {
	    
	     get
		{
		   return direction;
		 }
		 set
		 {
		   if(direction != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Direction",OldValue=direction,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   direction=value;
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
	    }
   
}
	 