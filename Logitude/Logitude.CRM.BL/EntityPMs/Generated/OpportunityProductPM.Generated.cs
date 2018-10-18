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
using Logitude.CRM.BL.Validators;
  
namespace Logitude.CRM.BL.EntityPMs
{
   [CustomValidation(typeof(CRMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class OpportunityProductPM : EntityPM
   {
   	  private string opportunityId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpportunityId  
	   {
	    
	     get
		{
		   return opportunityId;
		 }
		 set
		 {
		   if(opportunityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpportunityId",OldValue=opportunityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   opportunityId=value;
		   }
			
		 }
	   }
	  private string opportunityProductTypeCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpportunityProductTypeCode  
	   {
	    
	     get
		{
		   return opportunityProductTypeCode;
		 }
		 set
		 {
		   if(opportunityProductTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpportunityProductTypeCode",OldValue=opportunityProductTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   opportunityProductTypeCode=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private decimal? chargeableWeight ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ChargeableWeight  
	   {
	    
	     get
		{
		   return chargeableWeight;
		 }
		 set
		 {
		   if(chargeableWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeight",OldValue=chargeableWeight,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   chargeableWeight=value;
		   }
			
		 }
	   }
	  private decimal? tEU ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? TEU  
	   {
	    
	     get
		{
		   return tEU;
		 }
		 set
		 {
		   if(tEU != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TEU",OldValue=tEU,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   tEU=value;
		   }
			
		 }
	   }
	  private int? numberOfShipments ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int? NumberOfShipments  
	   {
	    
	     get
		{
		   return numberOfShipments;
		 }
		 set
		 {
		   if(numberOfShipments != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfShipments",OldValue=numberOfShipments,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   numberOfShipments=value;
		   }
			
		 }
	   }
	  private string opportunityProductTypeName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string OpportunityProductTypeName  
	   {
	    
	     get
		{
		   return opportunityProductTypeName;
		 }
		 set
		 {
		   if(opportunityProductTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OpportunityProductTypeName",OldValue=opportunityProductTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   opportunityProductTypeName=value;
		   }
			
		 }
	   }

	   private List<OpportunityProductLocationPM> opportunityProductLocations;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("OpportunityProductOpportunityProductLocations", "OpportunityId,OpportunityProductTypeCode","OpportunityId,OpportunityProductTypeCode")]
	   [DataMember]
	   public virtual List<OpportunityProductLocationPM> OpportunityProductLocations  
	   {
	        get
             {
                 if (opportunityProductLocations == null)
                 {
                     opportunityProductLocations = new List<OpportunityProductLocationPM>();
                 }
                 return opportunityProductLocations;
              }
             set { opportunityProductLocations = value; }
	    }
		   
	   private List<OpportunityProductLocationPM>  deletedOpportunityProductLocations;
	   public virtual List<OpportunityProductLocationPM> DeletedOpportunityProductLocations  
	   {
	        get
             {
                 if ( deletedOpportunityProductLocations == null)
                 {
                      deletedOpportunityProductLocations = new List<OpportunityProductLocationPM>();
                 }
                 return  deletedOpportunityProductLocations;
              }
             set {  deletedOpportunityProductLocations = value; }
	    }
	  	  private decimal? revenue ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Revenue  
	   {
	    
	     get
		{
		   return revenue;
		 }
		 set
		 {
		   if(revenue != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Revenue",OldValue=revenue,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   revenue=value;
		   }
			
		 }
	   }
	  private string prepaidCollectId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PrepaidCollectId  
	   {
	    
	     get
		{
		   return prepaidCollectId;
		 }
		 set
		 {
		   if(prepaidCollectId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PrepaidCollectId",OldValue=prepaidCollectId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   prepaidCollectId=value;
		   }
			
		 }
	   }
	  private string prepaidCollectName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PrepaidCollectName  
	   {
	    
	     get
		{
		   return prepaidCollectName;
		 }
		 set
		 {
		   if(prepaidCollectName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PrepaidCollectName",OldValue=prepaidCollectName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   prepaidCollectName=value;
		   }
			
		 }
	   }
	  private bool notesRightToLeft ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool NotesRightToLeft  
	   {
	    
	     get
		{
		   return notesRightToLeft;
		 }
		 set
		 {
		   if(notesRightToLeft != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotesRightToLeft",OldValue=notesRightToLeft,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   notesRightToLeft=value;
		   }
			
		 }
	   }
   }
   
}
	 