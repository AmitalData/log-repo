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
   public partial class OpportunityCompetitorPM : EntityPM
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
	  private string competitorId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string CompetitorId  
	   {
	    
	     get
		{
		   return competitorId;
		 }
		 set
		 {
		   if(competitorId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CompetitorId",OldValue=competitorId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   competitorId=value;
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

	   private List<OpportunityCompetitorProductPM> opportunityCompetitorProducts;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("OpportunityCompetitorProductOpportunityCompetitor", "OpportunityId,CompetitorId","OpportunityId,CompetitorId")]
	   [DataMember]
	   public virtual List<OpportunityCompetitorProductPM> OpportunityCompetitorProducts  
	   {
	        get
             {
                 if (opportunityCompetitorProducts == null)
                 {
                     opportunityCompetitorProducts = new List<OpportunityCompetitorProductPM>();
                 }
                 return opportunityCompetitorProducts;
              }
             set { opportunityCompetitorProducts = value; }
	    }
		   
	   private List<OpportunityCompetitorProductPM>  deletedOpportunityCompetitorProducts;
	   public virtual List<OpportunityCompetitorProductPM> DeletedOpportunityCompetitorProducts  
	   {
	        get
             {
                 if ( deletedOpportunityCompetitorProducts == null)
                 {
                      deletedOpportunityCompetitorProducts = new List<OpportunityCompetitorProductPM>();
                 }
                 return  deletedOpportunityCompetitorProducts;
              }
             set {  deletedOpportunityCompetitorProducts = value; }
	    }
	  	  private string englishName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
   }
   
}
	 