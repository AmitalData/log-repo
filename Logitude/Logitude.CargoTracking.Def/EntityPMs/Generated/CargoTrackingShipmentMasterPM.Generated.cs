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
using Logitude.CargoTracking.Def.Validators;
  
namespace Logitude.CargoTracking.Def.EntityPMs
{
   [CustomValidation(typeof(CargoTrackingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class CargoTrackingShipmentMasterPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string master ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Master  
	   {
	    
	     get
		{
		   return master;
		 }
		 set
		 {
		   if(master != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Master",OldValue=master,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   master=value;
		   }
			
		 }
	   }
	  private DateTime? mainCarriageATD ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? MainCarriageATD  
	   {
	    
	     get
		{
		   return mainCarriageATD;
		 }
		 set
		 {
		   if(mainCarriageATD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageATD",OldValue=mainCarriageATD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   mainCarriageATD=value;
		   }
			
		 }
	   }
	  private DateTime? mainCarriageETD ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? MainCarriageETD  
	   {
	    
	     get
		{
		   return mainCarriageETD;
		 }
		 set
		 {
		   if(mainCarriageETD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageETD",OldValue=mainCarriageETD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   mainCarriageETD=value;
		   }
			
		 }
	   }
	  private DateTime? mainCarriageATA ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? MainCarriageATA  
	   {
	    
	     get
		{
		   return mainCarriageATA;
		 }
		 set
		 {
		   if(mainCarriageATA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageATA",OldValue=mainCarriageATA,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   mainCarriageATA=value;
		   }
			
		 }
	   }
	  private DateTime? mainCarriageETA ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? MainCarriageETA  
	   {
	    
	     get
		{
		   return mainCarriageETA;
		 }
		 set
		 {
		   if(mainCarriageETA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageETA",OldValue=mainCarriageETA,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   mainCarriageETA=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
   }
   
}
	 