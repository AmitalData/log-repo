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
using Logitude.Accounting.Def.Validators;
  
namespace Logitude.Accounting.Def.EntityPMs
{
   [CustomValidation(typeof(AccountingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class AccountingIntegrityCheckPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private DateTime createDateTimeUTC ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime CreateDateTimeUTC  
	   {
	    
	     get
		{
		   return createDateTimeUTC;
		 }
		 set
		 {
		   if(createDateTimeUTC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDateTimeUTC",OldValue=createDateTimeUTC,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   createDateTimeUTC=value;
		   }
			
		 }
	   }
	  private string statusCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusCode  
	   {
	    
	     get
		{
		   return statusCode;
		 }
		 set
		 {
		   if(statusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusCode",OldValue=statusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusCode=value;
		   }
			
		 }
	   }
	  private string parametersXML ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ParametersXML  
	   {
	    
	     get
		{
		   return parametersXML;
		 }
		 set
		 {
		   if(parametersXML != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ParametersXML",OldValue=parametersXML,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   parametersXML=value;
		   }
			
		 }
	   }
	  private string resultXML ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ResultXML  
	   {
	    
	     get
		{
		   return resultXML;
		 }
		 set
		 {
		   if(resultXML != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResultXML",OldValue=resultXML,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   resultXML=value;
		   }
			
		 }
	   }
	  private bool hasException ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasException  
	   {
	    
	     get
		{
		   return hasException;
		 }
		 set
		 {
		   if(hasException != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasException",OldValue=hasException,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasException=value;
		   }
			
		 }
	   }
	  private DateTime? doneDateTimeUTC ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DoneDateTimeUTC  
	   {
	    
	     get
		{
		   return doneDateTimeUTC;
		 }
		 set
		 {
		   if(doneDateTimeUTC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DoneDateTimeUTC",OldValue=doneDateTimeUTC,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   doneDateTimeUTC=value;
		   }
			
		 }
	   }
	  private string statusName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusName  
	   {
	    
	     get
		{
		   return statusName;
		 }
		 set
		 {
		   if(statusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusName",OldValue=statusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusName=value;
		   }
			
		 }
	   }
	  private DateTime fromMonthInclusive ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime FromMonthInclusive  
	   {
	    
	     get
		{
		   return fromMonthInclusive;
		 }
		 set
		 {
		   if(fromMonthInclusive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromMonthInclusive",OldValue=fromMonthInclusive,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   fromMonthInclusive=value;
		   }
			
		 }
	   }
	  private DateTime toMonthInclusive ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ToMonthInclusive  
	   {
	    
	     get
		{
		   return toMonthInclusive;
		 }
		 set
		 {
		   if(toMonthInclusive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToMonthInclusive",OldValue=toMonthInclusive,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   toMonthInclusive=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
   }
   
}
	 