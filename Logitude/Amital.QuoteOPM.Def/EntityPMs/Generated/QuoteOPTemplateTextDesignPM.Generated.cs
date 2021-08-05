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
   public partial class QuoteOPTemplateTextDesignPM : EntityPM
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
	  private double fontSize ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double FontSize  
	   {
	    
	     get
		{
		   return fontSize;
		 }
		 set
		 {
		   if(fontSize != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FontSize",OldValue=fontSize,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   fontSize=value;
		   }
			
		 }
	   }
	  private string textColor ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TextColor  
	   {
	    
	     get
		{
		   return textColor;
		 }
		 set
		 {
		   if(textColor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TextColor",OldValue=textColor,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   textColor=value;
		   }
			
		 }
	   }
	  private string fontFamily ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FontFamily  
	   {
	    
	     get
		{
		   return fontFamily;
		 }
		 set
		 {
		   if(fontFamily != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FontFamily",OldValue=fontFamily,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fontFamily=value;
		   }
			
		 }
	   }
	  private string backgroundColor ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string BackgroundColor  
	   {
	    
	     get
		{
		   return backgroundColor;
		 }
		 set
		 {
		   if(backgroundColor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BackgroundColor",OldValue=backgroundColor,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   backgroundColor=value;
		   }
			
		 }
	   }
	  private string fontWeight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FontWeight  
	   {
	    
	     get
		{
		   return fontWeight;
		 }
		 set
		 {
		   if(fontWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FontWeight",OldValue=fontWeight,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fontWeight=value;
		   }
			
		 }
	   }
	  private bool italic ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Italic  
	   {
	    
	     get
		{
		   return italic;
		 }
		 set
		 {
		   if(italic != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Italic",OldValue=italic,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   italic=value;
		   }
			
		 }
	   }
	  private bool unDerLine ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool UnDerLine  
	   {
	    
	     get
		{
		   return unDerLine;
		 }
		 set
		 {
		   if(unDerLine != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UnDerLine",OldValue=unDerLine,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   unDerLine=value;
		   }
			
		 }
	   }
	  private string alignment ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Alignment  
	   {
	    
	     get
		{
		   return alignment;
		 }
		 set
		 {
		   if(alignment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Alignment",OldValue=alignment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   alignment=value;
		   }
			
		 }
	   }
	  private string title ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private string textValue ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TextValue  
	   {
	    
	     get
		{
		   return textValue;
		 }
		 set
		 {
		   if(textValue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TextValue",OldValue=textValue,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   textValue=value;
		   }
			
		 }
	   }
	  private string hideAlignment ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string HideAlignment  
	   {
	    
	     get
		{
		   return hideAlignment;
		 }
		 set
		 {
		   if(hideAlignment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HideAlignment",OldValue=hideAlignment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   hideAlignment=value;
		   }
			
		 }
	   }
	  private string sampleText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string SampleText  
	   {
	    
	     get
		{
		   return sampleText;
		 }
		 set
		 {
		   if(sampleText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SampleText",OldValue=sampleText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sampleText=value;
		   }
			
		 }
	   }
   }
   
}
	 