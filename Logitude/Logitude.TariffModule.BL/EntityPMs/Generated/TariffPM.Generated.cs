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
   public partial class TariffPM : EntityPM
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
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private string name ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Name  
	   {
	    
	     get
		{
		   return name;
		 }
		 set
		 {
		   if(name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Name",OldValue=name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   name=value;
		   }
			
		 }
	   }
	  private bool inActive ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool InActive  
	   {
	    
	     get
		{
		   return inActive;
		 }
		 set
		 {
		   if(inActive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InActive",OldValue=inActive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   inActive=value;
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
	  private string sellerId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string SellerId  
	   {
	    
	     get
		{
		   return sellerId;
		 }
		 set
		 {
		   if(sellerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SellerId",OldValue=sellerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sellerId=value;
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
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private DateTime? lastExpirationDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastExpirationDate  
	   {
	    
	     get
		{
		   return lastExpirationDate;
		 }
		 set
		 {
		   if(lastExpirationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastExpirationDate",OldValue=lastExpirationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastExpirationDate=value;
		   }
			
		 }
	   }
	  private string priceSteps ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string PriceSteps  
	   {
	    
	     get
		{
		   return priceSteps;
		 }
		 set
		 {
		   if(priceSteps != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PriceSteps",OldValue=priceSteps,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   priceSteps=value;
		   }
			
		 }
	   }
	  private string typeCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeCode  
	   {
	    
	     get
		{
		   return typeCode;
		 }
		 set
		 {
		   if(typeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeCode",OldValue=typeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeCode=value;
		   }
			
		 }
	   }
	  private string typeName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeName  
	   {
	    
	     get
		{
		   return typeName;
		 }
		 set
		 {
		   if(typeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeName",OldValue=typeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeName=value;
		   }
			
		 }
	   }
	  private DateTime? lastStartDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastStartDate  
	   {
	    
	     get
		{
		   return lastStartDate;
		 }
		 set
		 {
		   if(lastStartDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastStartDate",OldValue=lastStartDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastStartDate=value;
		   }
			
		 }
	   }
	  private int lastVersion ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int LastVersion  
	   {
	    
	     get
		{
		   return lastVersion;
		 }
		 set
		 {
		   if(lastVersion != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastVersion",OldValue=lastVersion,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   lastVersion=value;
		   }
			
		 }
	   }
	  private string contractNumber ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContractNumber  
	   {
	    
	     get
		{
		   return contractNumber;
		 }
		 set
		 {
		   if(contractNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContractNumber",OldValue=contractNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   contractNumber=value;
		   }
			
		 }
	   }
	  private string sellerName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string SellerName  
	   {
	    
	     get
		{
		   return sellerName;
		 }
		 set
		 {
		   if(sellerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SellerName",OldValue=sellerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sellerName=value;
		   }
			
		 }
	   }
	  private bool setAsInActive ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool SetAsInActive  
	   {
	    
	     get
		{
		   return setAsInActive;
		 }
		 set
		 {
		   if(setAsInActive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SetAsInActive",OldValue=setAsInActive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   setAsInActive=value;
		   }
			
		 }
	   }
	  private bool setAsReActive ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool SetAsReActive  
	   {
	    
	     get
		{
		   return setAsReActive;
		 }
		 set
		 {
		   if(setAsReActive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SetAsReActive",OldValue=setAsReActive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   setAsReActive=value;
		   }
			
		 }
	   }
	  private string tariffNumber ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffNumber  
	   {
	    
	     get
		{
		   return tariffNumber;
		 }
		 set
		 {
		   if(tariffNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffNumber",OldValue=tariffNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffNumber=value;
		   }
			
		 }
	   }

	   private List<TariffVersionPM> tariffVersions;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("TariffTariffVersions", "Id","TariffId")]
	   [DataMember]
	   public virtual List<TariffVersionPM> TariffVersions  
	   {
	        get
             {
                 if (tariffVersions == null)
                 {
                     tariffVersions = new List<TariffVersionPM>();
                 }
                 return tariffVersions;
              }
             set { tariffVersions = value; }
	    }
		   
	   private List<TariffVersionPM>  deletedTariffVersions;
	   public virtual List<TariffVersionPM> DeletedTariffVersions  
	   {
	        get
             {
                 if ( deletedTariffVersions == null)
                 {
                      deletedTariffVersions = new List<TariffVersionPM>();
                 }
                 return  deletedTariffVersions;
              }
             set {  deletedTariffVersions = value; }
	    }
	  	  private bool tariffLinesAdded ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TariffLinesAdded  
	   {
	    
	     get
		{
		   return tariffLinesAdded;
		 }
		 set
		 {
		   if(tariffLinesAdded != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffLinesAdded",OldValue=tariffLinesAdded,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tariffLinesAdded=value;
		   }
			
		 }
	   }
	  private string surcharge1Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge1Id  
	   {
	    
	     get
		{
		   return surcharge1Id;
		 }
		 set
		 {
		   if(surcharge1Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge1Id",OldValue=surcharge1Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge1Id=value;
		   }
			
		 }
	   }
	  private string surcharge2Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge2Id  
	   {
	    
	     get
		{
		   return surcharge2Id;
		 }
		 set
		 {
		   if(surcharge2Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge2Id",OldValue=surcharge2Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge2Id=value;
		   }
			
		 }
	   }
	  private string surcharge3Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge3Id  
	   {
	    
	     get
		{
		   return surcharge3Id;
		 }
		 set
		 {
		   if(surcharge3Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge3Id",OldValue=surcharge3Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge3Id=value;
		   }
			
		 }
	   }
	  private string surcharge4Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge4Id  
	   {
	    
	     get
		{
		   return surcharge4Id;
		 }
		 set
		 {
		   if(surcharge4Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge4Id",OldValue=surcharge4Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge4Id=value;
		   }
			
		 }
	   }
	  private string surcharge5Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge5Id  
	   {
	    
	     get
		{
		   return surcharge5Id;
		 }
		 set
		 {
		   if(surcharge5Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge5Id",OldValue=surcharge5Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge5Id=value;
		   }
			
		 }
	   }
	  private string surcharge6Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge6Id  
	   {
	    
	     get
		{
		   return surcharge6Id;
		 }
		 set
		 {
		   if(surcharge6Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge6Id",OldValue=surcharge6Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge6Id=value;
		   }
			
		 }
	   }
	  private string surcharge7Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge7Id  
	   {
	    
	     get
		{
		   return surcharge7Id;
		 }
		 set
		 {
		   if(surcharge7Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge7Id",OldValue=surcharge7Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge7Id=value;
		   }
			
		 }
	   }
	  private string surcharge8Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge8Id  
	   {
	    
	     get
		{
		   return surcharge8Id;
		 }
		 set
		 {
		   if(surcharge8Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge8Id",OldValue=surcharge8Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge8Id=value;
		   }
			
		 }
	   }
	  private string surcharge9Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge9Id  
	   {
	    
	     get
		{
		   return surcharge9Id;
		 }
		 set
		 {
		   if(surcharge9Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge9Id",OldValue=surcharge9Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge9Id=value;
		   }
			
		 }
	   }
	  private string surcharge10Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge10Id  
	   {
	    
	     get
		{
		   return surcharge10Id;
		 }
		 set
		 {
		   if(surcharge10Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge10Id",OldValue=surcharge10Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge10Id=value;
		   }
			
		 }
	   }
	  private string surcharge1UOM ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge1UOM  
	   {
	    
	     get
		{
		   return surcharge1UOM;
		 }
		 set
		 {
		   if(surcharge1UOM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge1UOM",OldValue=surcharge1UOM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge1UOM=value;
		   }
			
		 }
	   }
	  private string surcharge2UOM ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge2UOM  
	   {
	    
	     get
		{
		   return surcharge2UOM;
		 }
		 set
		 {
		   if(surcharge2UOM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge2UOM",OldValue=surcharge2UOM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge2UOM=value;
		   }
			
		 }
	   }
	  private string surcharge3UOM ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge3UOM  
	   {
	    
	     get
		{
		   return surcharge3UOM;
		 }
		 set
		 {
		   if(surcharge3UOM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge3UOM",OldValue=surcharge3UOM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge3UOM=value;
		   }
			
		 }
	   }
	  private string surcharge4UOM ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge4UOM  
	   {
	    
	     get
		{
		   return surcharge4UOM;
		 }
		 set
		 {
		   if(surcharge4UOM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge4UOM",OldValue=surcharge4UOM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge4UOM=value;
		   }
			
		 }
	   }
	  private string surcharge5UOM ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge5UOM  
	   {
	    
	     get
		{
		   return surcharge5UOM;
		 }
		 set
		 {
		   if(surcharge5UOM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge5UOM",OldValue=surcharge5UOM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge5UOM=value;
		   }
			
		 }
	   }
	  private string surcharge6UOM ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge6UOM  
	   {
	    
	     get
		{
		   return surcharge6UOM;
		 }
		 set
		 {
		   if(surcharge6UOM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge6UOM",OldValue=surcharge6UOM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge6UOM=value;
		   }
			
		 }
	   }
	  private string surcharge7UOM ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge7UOM  
	   {
	    
	     get
		{
		   return surcharge7UOM;
		 }
		 set
		 {
		   if(surcharge7UOM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge7UOM",OldValue=surcharge7UOM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge7UOM=value;
		   }
			
		 }
	   }
	  private string surcharge8UOM ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge8UOM  
	   {
	    
	     get
		{
		   return surcharge8UOM;
		 }
		 set
		 {
		   if(surcharge8UOM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge8UOM",OldValue=surcharge8UOM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge8UOM=value;
		   }
			
		 }
	   }
	  private string surcharge9UOM ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge9UOM  
	   {
	    
	     get
		{
		   return surcharge9UOM;
		 }
		 set
		 {
		   if(surcharge9UOM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge9UOM",OldValue=surcharge9UOM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge9UOM=value;
		   }
			
		 }
	   }
	  private string surcharge10UOM ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Surcharge10UOM  
	   {
	    
	     get
		{
		   return surcharge10UOM;
		 }
		 set
		 {
		   if(surcharge10UOM != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Surcharge10UOM",OldValue=surcharge10UOM,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surcharge10UOM=value;
		   }
			
		 }
	   }

	   private List<TariffVersionPM> activeVersions;
	 
		     
	   [Include]
	   [Association("TariffActiveVersions", "Id","TariffId")]
	   [DataMember]
	   public virtual List<TariffVersionPM> ActiveVersions  
	   {
	        get
             {
                 if (activeVersions == null)
                 {
                     activeVersions = new List<TariffVersionPM>();
                 }
                 return activeVersions;
              }
             set { activeVersions = value; }
	    }
		   
	   private List<TariffVersionPM>  deletedActiveVersions;
	   public virtual List<TariffVersionPM> DeletedActiveVersions  
	   {
	        get
             {
                 if ( deletedActiveVersions == null)
                 {
                      deletedActiveVersions = new List<TariffVersionPM>();
                 }
                 return  deletedActiveVersions;
              }
             set {  deletedActiveVersions = value; }
	    }
	  	  private int tariffLinesAddedNumbers ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int TariffLinesAddedNumbers  
	   {
	    
	     get
		{
		   return tariffLinesAddedNumbers;
		 }
		 set
		 {
		   if(tariffLinesAddedNumbers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffLinesAddedNumbers",OldValue=tariffLinesAddedNumbers,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   tariffLinesAddedNumbers=value;
		   }
			
		 }
	   }
	  private bool tariffLinesAddedFromExcel ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TariffLinesAddedFromExcel  
	   {
	    
	     get
		{
		   return tariffLinesAddedFromExcel;
		 }
		 set
		 {
		   if(tariffLinesAddedFromExcel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffLinesAddedFromExcel",OldValue=tariffLinesAddedFromExcel,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tariffLinesAddedFromExcel=value;
		   }
			
		 }
	   }
	  private string fileUploadedName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string FileUploadedName  
	   {
	    
	     get
		{
		   return fileUploadedName;
		 }
		 set
		 {
		   if(fileUploadedName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileUploadedName",OldValue=fileUploadedName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fileUploadedName=value;
		   }
			
		 }
	   }
	  private string concurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private bool isApprovingDraftVersion ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsApprovingDraftVersion  
	   {
	    
	     get
		{
		   return isApprovingDraftVersion;
		 }
		 set
		 {
		   if(isApprovingDraftVersion != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsApprovingDraftVersion",OldValue=isApprovingDraftVersion,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isApprovingDraftVersion=value;
		   }
			
		 }
	   }
	  private bool isSurchargeUpdate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSurchargeUpdate  
	   {
	    
	     get
		{
		   return isSurchargeUpdate;
		 }
		 set
		 {
		   if(isSurchargeUpdate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSurchargeUpdate",OldValue=isSurchargeUpdate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSurchargeUpdate=value;
		   }
			
		 }
	   }

	   private List<TariffLineExpirationDatePM> deletedLinesExpirationDates;
	 
	   [DataMember]
	   public virtual List<TariffLineExpirationDatePM> DeletedLinesExpirationDates  
	   {
	        get
             {
                 if (deletedLinesExpirationDates == null)
                 {
                     deletedLinesExpirationDates = new List<TariffLineExpirationDatePM>();
                 }
                 return deletedLinesExpirationDates;
              }
             set { deletedLinesExpirationDates = value; }
	    }
		   
	   private List<TariffLineExpirationDatePM>  deletedDeletedLinesExpirationDates;
	   public virtual List<TariffLineExpirationDatePM> DeletedDeletedLinesExpirationDates  
	   {
	        get
             {
                 if ( deletedDeletedLinesExpirationDates == null)
                 {
                      deletedDeletedLinesExpirationDates = new List<TariffLineExpirationDatePM>();
                 }
                 return  deletedDeletedLinesExpirationDates;
              }
             set {  deletedDeletedLinesExpirationDates = value; }
	    }
	  	  private bool isFromUpdateScreen ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsFromUpdateScreen  
	   {
	    
	     get
		{
		   return isFromUpdateScreen;
		 }
		 set
		 {
		   if(isFromUpdateScreen != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFromUpdateScreen",OldValue=isFromUpdateScreen,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isFromUpdateScreen=value;
		   }
			
		 }
	   }
	  private bool isFromCopy ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsFromCopy  
	   {
	    
	     get
		{
		   return isFromCopy;
		 }
		 set
		 {
		   if(isFromCopy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsFromCopy",OldValue=isFromCopy,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isFromCopy=value;
		   }
			
		 }
	   }
	  private bool isUpdatingMissingPorts ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsUpdatingMissingPorts  
	   {
	    
	     get
		{
		   return isUpdatingMissingPorts;
		 }
		 set
		 {
		   if(isUpdatingMissingPorts != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsUpdatingMissingPorts",OldValue=isUpdatingMissingPorts,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isUpdatingMissingPorts=value;
		   }
			
		 }
	   }
	  private string containerType1Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType1Id  
	   {
	    
	     get
		{
		   return containerType1Id;
		 }
		 set
		 {
		   if(containerType1Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType1Id",OldValue=containerType1Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType1Id=value;
		   }
			
		 }
	   }
	  private string containerType2Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType2Id  
	   {
	    
	     get
		{
		   return containerType2Id;
		 }
		 set
		 {
		   if(containerType2Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType2Id",OldValue=containerType2Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType2Id=value;
		   }
			
		 }
	   }
	  private string containerType3Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType3Id  
	   {
	    
	     get
		{
		   return containerType3Id;
		 }
		 set
		 {
		   if(containerType3Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType3Id",OldValue=containerType3Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType3Id=value;
		   }
			
		 }
	   }
	  private string containerType4Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType4Id  
	   {
	    
	     get
		{
		   return containerType4Id;
		 }
		 set
		 {
		   if(containerType4Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType4Id",OldValue=containerType4Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType4Id=value;
		   }
			
		 }
	   }
	  private string containerType5Id ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerType5Id  
	   {
	    
	     get
		{
		   return containerType5Id;
		 }
		 set
		 {
		   if(containerType5Id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerType5Id",OldValue=containerType5Id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerType5Id=value;
		   }
			
		 }
	   }
	  private string transportModeCode ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeCode  
	   {
	    
	     get
		{
		   return transportModeCode;
		 }
		 set
		 {
		   if(transportModeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeCode",OldValue=transportModeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeCode=value;
		   }
			
		 }
	   }
	  private string transportModeName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private string tariffProductId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffProductId  
	   {
	    
	     get
		{
		   return tariffProductId;
		 }
		 set
		 {
		   if(tariffProductId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffProductId",OldValue=tariffProductId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffProductId=value;
		   }
			
		 }
	   }
	  private string sellerPartnerTypeId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string SellerPartnerTypeId  
	   {
	    
	     get
		{
		   return sellerPartnerTypeId;
		 }
		 set
		 {
		   if(sellerPartnerTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SellerPartnerTypeId",OldValue=sellerPartnerTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sellerPartnerTypeId=value;
		   }
			
		 }
	   }
	  private bool isRefreshTranslations ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsRefreshTranslations  
	   {
	    
	     get
		{
		   return isRefreshTranslations;
		 }
		 set
		 {
		   if(isRefreshTranslations != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsRefreshTranslations",OldValue=isRefreshTranslations,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isRefreshTranslations=value;
		   }
			
		 }
	   }
	  private DateTime? lastUsedDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastUsedDate  
	   {
	    
	     get
		{
		   return lastUsedDate;
		 }
		 set
		 {
		   if(lastUsedDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastUsedDate",OldValue=lastUsedDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastUsedDate=value;
		   }
			
		 }
	   }
	  private string freightChargeId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string FreightChargeId  
	   {
	    
	     get
		{
		   return freightChargeId;
		 }
		 set
		 {
		   if(freightChargeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FreightChargeId",OldValue=freightChargeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   freightChargeId=value;
		   }
			
		 }
	   }
	  private string customsBrokerId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBrokerId  
	   {
	    
	     get
		{
		   return customsBrokerId;
		 }
		 set
		 {
		   if(customsBrokerId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBrokerId",OldValue=customsBrokerId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBrokerId=value;
		   }
			
		 }
	   }
	  private string customsBrokerName ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBrokerName  
	   {
	    
	     get
		{
		   return customsBrokerName;
		 }
		 set
		 {
		   if(customsBrokerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBrokerName",OldValue=customsBrokerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBrokerName=value;
		   }
			
		 }
	   }
	  private string customsBrokerPartnerTypeId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBrokerPartnerTypeId  
	   {
	    
	     get
		{
		   return customsBrokerPartnerTypeId;
		 }
		 set
		 {
		   if(customsBrokerPartnerTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBrokerPartnerTypeId",OldValue=customsBrokerPartnerTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBrokerPartnerTypeId=value;
		   }
			
		 }
	   }
   }
   
}
	 