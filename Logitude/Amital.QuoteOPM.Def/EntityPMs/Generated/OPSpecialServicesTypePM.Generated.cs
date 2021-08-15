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
   public partial class OPSpecialServicesTypePM : EntityPM
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
	  private string code ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Code  
	   {
	    
	     get
		{
		   return code;
		 }
		 set
		 {
		   if(code != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Code",OldValue=code,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   code=value;
		   }
			
		 }
	   }
	  private string englishName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EnglishName  
	   {
	    
	     get
		{
		   return englishName;
		 }
		 set
		 {
		   if(englishName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EnglishName",OldValue=englishName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   englishName=value;
		   }
			
		 }
	   }
	  private string localName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LocalName  
	   {
	    
	     get
		{
		   return localName;
		 }
		 set
		 {
		   if(localName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalName",OldValue=localName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   localName=value;
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
	  private bool inActive ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string partnerCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PartnerCode  
	   {
	    
	     get
		{
		   return partnerCode;
		 }
		 set
		 {
		   if(partnerCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PartnerCode",OldValue=partnerCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   partnerCode=value;
		   }
			
		 }
	   }
   }
   
}
	 