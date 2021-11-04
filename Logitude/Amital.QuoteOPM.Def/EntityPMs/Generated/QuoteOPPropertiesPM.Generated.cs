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
   public partial class QuoteOPPropertiesPM : EntityPM
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
	  private string quoteID ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteID  
	   {
	    
	     get
		{
		   return quoteID;
		 }
		 set
		 {
		   if(quoteID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteID",OldValue=quoteID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteID=value;
		   }
			
		 }
	   }
	  private int indexOrder ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int IndexOrder  
	   {
	    
	     get
		{
		   return indexOrder;
		 }
		 set
		 {
		   if(indexOrder != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IndexOrder",OldValue=indexOrder,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   indexOrder=value;
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
	  private string specialServiceID ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecialServiceID  
	   {
	    
	     get
		{
		   return specialServiceID;
		 }
		 set
		 {
		   if(specialServiceID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecialServiceID",OldValue=specialServiceID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specialServiceID=value;
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
	  private string fromAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromAddressId  
	   {
	    
	     get
		{
		   return fromAddressId;
		 }
		 set
		 {
		   if(fromAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromAddressId",OldValue=fromAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromAddressId=value;
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
	  private string toAddressId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToAddressId  
	   {
	    
	     get
		{
		   return toAddressId;
		 }
		 set
		 {
		   if(toAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToAddressId",OldValue=toAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toAddressId=value;
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
   }
   
}
	 