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
   public partial class CustomsItemPM : EntityPM
   {
   	  private int customsBookTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CustomsBookTypeID  
	   {
	    
	     get
		{
		   return customsBookTypeID;
		 }
		 set
		 {
		   if(customsBookTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBookTypeID",OldValue=customsBookTypeID,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   customsBookTypeID=value;
		   }
			
		 }
	   }
	  private string fullClassification ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FullClassification  
	   {
	    
	     get
		{
		   return fullClassification;
		 }
		 set
		 {
		   if(fullClassification != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FullClassification",OldValue=fullClassification,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fullClassification=value;
		   }
			
		 }
	   }
	  private int customsItemCategoryID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CustomsItemCategoryID  
	   {
	    
	     get
		{
		   return customsItemCategoryID;
		 }
		 set
		 {
		   if(customsItemCategoryID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsItemCategoryID",OldValue=customsItemCategoryID,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   customsItemCategoryID=value;
		   }
			
		 }
	   }
	  private int? customsItemHierarchicLocatioID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? CustomsItemHierarchicLocatioID  
	   {
	    
	     get
		{
		   return customsItemHierarchicLocatioID;
		 }
		 set
		 {
		   if(customsItemHierarchicLocatioID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsItemHierarchicLocatioID",OldValue=customsItemHierarchicLocatioID,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   customsItemHierarchicLocatioID=value;
		   }
			
		 }
	   }
	  private string computedCheckDigit ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ComputedCheckDigit  
	   {
	    
	     get
		{
		   return computedCheckDigit;
		 }
		 set
		 {
		   if(computedCheckDigit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ComputedCheckDigit",OldValue=computedCheckDigit,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   computedCheckDigit=value;
		   }
			
		 }
	   }
	  private string classificationWithCheckDigit ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClassificationWithCheckDigit  
	   {
	    
	     get
		{
		   return classificationWithCheckDigit;
		 }
		 set
		 {
		   if(classificationWithCheckDigit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClassificationWithCheckDigit",OldValue=classificationWithCheckDigit,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   classificationWithCheckDigit=value;
		   }
			
		 }
	   }
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

	   private List<CustomsItemDetailsHistoryPM> customsItemDetailsHistory;
	 
		     
	   [Include]
	   [Association("CustomsItemCustomsItemDetailsHistory", "ID","CustomsItemID")]
	   [DataMember]
	   public virtual List<CustomsItemDetailsHistoryPM> CustomsItemDetailsHistory  
	   {
	        get
             {
                 if (customsItemDetailsHistory == null)
                 {
                     customsItemDetailsHistory = new List<CustomsItemDetailsHistoryPM>();
                 }
                 return customsItemDetailsHistory;
              }
             set { customsItemDetailsHistory = value; }
	    }
		   
	   private List<CustomsItemDetailsHistoryPM>  deletedCustomsItemDetailsHistory;
	   public virtual List<CustomsItemDetailsHistoryPM> DeletedCustomsItemDetailsHistory  
	   {
	        get
             {
                 if ( deletedCustomsItemDetailsHistory == null)
                 {
                      deletedCustomsItemDetailsHistory = new List<CustomsItemDetailsHistoryPM>();
                 }
                 return  deletedCustomsItemDetailsHistory;
              }
             set {  deletedCustomsItemDetailsHistory = value; }
	    }
	  
	   private List<PropertiesDetailsHistoryPM> propertiesDetailsHistory;
	 
		     
	   [Include]
	   [Association("CustomsItemPropertiesDetailsHistory", "ID","CustomsItemID")]
	   [DataMember]
	   public virtual List<PropertiesDetailsHistoryPM> PropertiesDetailsHistory  
	   {
	        get
             {
                 if (propertiesDetailsHistory == null)
                 {
                     propertiesDetailsHistory = new List<PropertiesDetailsHistoryPM>();
                 }
                 return propertiesDetailsHistory;
              }
             set { propertiesDetailsHistory = value; }
	    }
		   
	   private List<PropertiesDetailsHistoryPM>  deletedPropertiesDetailsHistory;
	   public virtual List<PropertiesDetailsHistoryPM> DeletedPropertiesDetailsHistory  
	   {
	        get
             {
                 if ( deletedPropertiesDetailsHistory == null)
                 {
                      deletedPropertiesDetailsHistory = new List<PropertiesDetailsHistoryPM>();
                 }
                 return  deletedPropertiesDetailsHistory;
              }
             set {  deletedPropertiesDetailsHistory = value; }
	    }
	  	    }
   
}
	 