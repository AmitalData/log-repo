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
   public partial class QuoteopPackagePM : EntityPM
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
	  private string quoteOPId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string QuoteOPId  
	   {
	    
	     get
		{
		   return quoteOPId;
		 }
		 set
		 {
		   if(quoteOPId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteOPId",OldValue=quoteOPId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quoteOPId=value;
		   }
			
		 }
	   }
	  private string packageTypeId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeId  
	   {
	    
	     get
		{
		   return packageTypeId;
		 }
		 set
		 {
		   if(packageTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeId",OldValue=packageTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeId=value;
		   }
			
		 }
	   }
	  private string packageTypeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeName  
	   {
	    
	     get
		{
		   return packageTypeName;
		 }
		 set
		 {
		   if(packageTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeName",OldValue=packageTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeName=value;
		   }
			
		 }
	   }
	  private int? quantity ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Quantity  
	   {
	    
	     get
		{
		   return quantity;
		 }
		 set
		 {
		   if(quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity",OldValue=quantity,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   quantity=value;
		   }
			
		 }
	   }
	  private double? grossWeight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? GrossWeight  
	   {
	    
	     get
		{
		   return grossWeight;
		 }
		 set
		 {
		   if(grossWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeight",OldValue=grossWeight,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   grossWeight=value;
		   }
			
		 }
	   }
	  private double? volume ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Volume  
	   {
	    
	     get
		{
		   return volume;
		 }
		 set
		 {
		   if(volume != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Volume",OldValue=volume,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   volume=value;
		   }
			
		 }
	   }
	  private double? height ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Height  
	   {
	    
	     get
		{
		   return height;
		 }
		 set
		 {
		   if(height != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Height",OldValue=height,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   height=value;
		   }
			
		 }
	   }
	  private double? width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Width  
	   {
	    
	     get
		{
		   return width;
		 }
		 set
		 {
		   if(width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Width",OldValue=width,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   width=value;
		   }
			
		 }
	   }
	  private double? length ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? Length  
	   {
	    
	     get
		{
		   return length;
		 }
		 set
		 {
		   if(length != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Length",OldValue=length,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   length=value;
		   }
			
		 }
	   }
	  private double? volumetricWeight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? VolumetricWeight  
	   {
	    
	     get
		{
		   return volumetricWeight;
		 }
		 set
		 {
		   if(volumetricWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumetricWeight",OldValue=volumetricWeight,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   volumetricWeight=value;
		   }
			
		 }
	   }
	  private string quote ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Quote  
	   {
	    
	     get
		{
		   return quote;
		 }
		 set
		 {
		   if(quote != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quote",OldValue=quote,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   quote=value;
		   }
			
		 }
	   }
	  private string dimensions ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Dimensions  
	   {
	    
	     get
		{
		   return dimensions;
		 }
		 set
		 {
		   if(dimensions != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Dimensions",OldValue=dimensions,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dimensions=value;
		   }
			
		 }
	   }
   }
   
}
	 