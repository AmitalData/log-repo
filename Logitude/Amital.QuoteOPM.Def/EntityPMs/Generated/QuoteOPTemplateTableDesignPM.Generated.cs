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
   public partial class QuoteOPTemplateTableDesignPM : EntityPM
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
	  private string borderTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BorderTypeCode  
	   {
	    
	     get
		{
		   return borderTypeCode;
		 }
		 set
		 {
		   if(borderTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BorderTypeCode",OldValue=borderTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   borderTypeCode=value;
		   }
			
		 }
	   }
	  private string borderColor ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BorderColor  
	   {
	    
	     get
		{
		   return borderColor;
		 }
		 set
		 {
		   if(borderColor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BorderColor",OldValue=borderColor,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   borderColor=value;
		   }
			
		 }
	   }
	  private int borderThickness ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int BorderThickness  
	   {
	    
	     get
		{
		   return borderThickness;
		 }
		 set
		 {
		   if(borderThickness != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BorderThickness",OldValue=borderThickness,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   borderThickness=value;
		   }
			
		 }
	   }
	  private string headerDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string HeaderDesignId  
	   {
	    
	     get
		{
		   return headerDesignId;
		 }
		 set
		 {
		   if(headerDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HeaderDesignId",OldValue=headerDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   headerDesignId=value;
		   }
			
		 }
	   }
	  private string linesDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string LinesDesignId  
	   {
	    
	     get
		{
		   return linesDesignId;
		 }
		 set
		 {
		   if(linesDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LinesDesignId",OldValue=linesDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   linesDesignId=value;
		   }
			
		 }
	   }
	  private string groupByDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupByDesignId  
	   {
	    
	     get
		{
		   return groupByDesignId;
		 }
		 set
		 {
		   if(groupByDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupByDesignId",OldValue=groupByDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupByDesignId=value;
		   }
			
		 }
	   }
   }
   
}
	 