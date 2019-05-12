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
using Logitude.TariffModule.BL.Validators;
  
namespace Logitude.TariffModule.BL.EntityPMs
{
   [CustomValidation(typeof(TariffModuleClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TariffVersionPM : EntityPM
   {
   	  private string tariffId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffId  
	   {
	    
	     get
		{
		   return tariffId;
		 }
		 set
		 {
		   if(tariffId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffId",OldValue=tariffId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffId=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
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
	  private DateTime? startDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartDate  
	   {
	    
	     get
		{
		   return startDate;
		 }
		 set
		 {
		   if(startDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDate",OldValue=startDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startDate=value;
		   }
			
		 }
	   }
	  private DateTime? expirationDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ExpirationDate  
	   {
	    
	     get
		{
		   return expirationDate;
		 }
		 set
		 {
		   if(expirationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExpirationDate",OldValue=expirationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   expirationDate=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private int version ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int Version  
	   {
	    
	     get
		{
		   return version;
		 }
		 set
		 {
		   if(version != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Version",OldValue=version,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   version=value;
		   }
			
		 }
	   }
	  private bool isDraft ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDraft  
	   {
	    
	     get
		{
		   return isDraft;
		 }
		 set
		 {
		   if(isDraft != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDraft",OldValue=isDraft,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDraft=value;
		   }
			
		 }
	   }
	  private DateTime? approveDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ApproveDate  
	   {
	    
	     get
		{
		   return approveDate;
		 }
		 set
		 {
		   if(approveDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ApproveDate",OldValue=approveDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   approveDate=value;
		   }
			
		 }
	   }
	  private string approvedByUserId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ApprovedByUserId  
	   {
	    
	     get
		{
		   return approvedByUserId;
		 }
		 set
		 {
		   if(approvedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ApprovedByUserId",OldValue=approvedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   approvedByUserId=value;
		   }
			
		 }
	   }

	   private List<TariffLinePM> tariffLines;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("TariffVersionTariffLines", "Version","Version")]
	   [DataMember]
	   public virtual List<TariffLinePM> TariffLines  
	   {
	        get
             {
                 if (tariffLines == null)
                 {
                     tariffLines = new List<TariffLinePM>();
                 }
                 return tariffLines;
              }
             set { tariffLines = value; }
	    }
		   
	   private List<TariffLinePM>  deletedTariffLines;
	   public virtual List<TariffLinePM> DeletedTariffLines  
	   {
	        get
             {
                 if ( deletedTariffLines == null)
                 {
                      deletedTariffLines = new List<TariffLinePM>();
                 }
                 return  deletedTariffLines;
              }
             set {  deletedTariffLines = value; }
	    }
	     }
   
}
	 