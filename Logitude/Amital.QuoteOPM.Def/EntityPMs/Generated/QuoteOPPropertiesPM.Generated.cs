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
	  private int order ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int Order  
	   {
	    
	     get
		{
		   return order;
		 }
		 set
		 {
		   if(order != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Order",OldValue=order,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   order=value;
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
   }
   
}
	 