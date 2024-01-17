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
   public partial class CB_TradeAgreementPM : EntityPM
   {
   	  private string iD ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="string"};
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
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string title ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Title  
	   {
	    
	     get
		{
		   return title;
		 }
		 set
		 {
		   if(title != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Title",OldValue=title,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   title=value;
		   }
			
		 }
	   }
	  private string additionName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AdditionName  
	   {
	    
	     get
		{
		   return additionName;
		 }
		 set
		 {
		   if(additionName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AdditionName",OldValue=additionName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   additionName=value;
		   }
			
		 }
	   }
	  private string countryGroupID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CountryGroupID  
	   {
	    
	     get
		{
		   return countryGroupID;
		 }
		 set
		 {
		   if(countryGroupID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CountryGroupID",OldValue=countryGroupID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   countryGroupID=value;
		   }
			
		 }
	   }
	  private string customsBookTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsBookTypeID  
	   {
	    
	     get
		{
		   return customsBookTypeID;
		 }
		 set
		 {
		   if(customsBookTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBookTypeID",OldValue=customsBookTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsBookTypeID=value;
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
	  private string tradeAgreementAbbreviation ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TradeAgreementAbbreviation  
	   {
	    
	     get
		{
		   return tradeAgreementAbbreviation;
		 }
		 set
		 {
		   if(tradeAgreementAbbreviation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TradeAgreementAbbreviation",OldValue=tradeAgreementAbbreviation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tradeAgreementAbbreviation=value;
		   }
			
		 }
	   }
   }
   
}
	 