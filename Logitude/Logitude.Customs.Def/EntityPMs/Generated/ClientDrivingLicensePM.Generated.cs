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
   public partial class ClientDrivingLicensePM : EntityPM
   {
   	  private string clientId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClientId  
	   {
	    
	     get
		{
		   return clientId;
		 }
		 set
		 {
		   if(clientId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClientId",OldValue=clientId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   clientId=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private int line ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int Line  
	   {
	    
	     get
		{
		   return line;
		 }
		 set
		 {
		   if(line != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Line",OldValue=line,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   line=value;
		   }
			
		 }
	   }
	  private string drivingLicenseNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DrivingLicenseNumber  
	   {
	    
	     get
		{
		   return drivingLicenseNumber;
		 }
		 set
		 {
		   if(drivingLicenseNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DrivingLicenseNumber",OldValue=drivingLicenseNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   drivingLicenseNumber=value;
		   }
			
		 }
	   }
	  private DateTime? driverLicenseValidityDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DriverLicenseValidityDate  
	   {
	    
	     get
		{
		   return driverLicenseValidityDate;
		 }
		 set
		 {
		   if(driverLicenseValidityDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DriverLicenseValidityDate",OldValue=driverLicenseValidityDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   driverLicenseValidityDate=value;
		   }
			
		 }
	   }
	  private string drivingLicenseCountryID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DrivingLicenseCountryID  
	   {
	    
	     get
		{
		   return drivingLicenseCountryID;
		 }
		 set
		 {
		   if(drivingLicenseCountryID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DrivingLicenseCountryID",OldValue=drivingLicenseCountryID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   drivingLicenseCountryID=value;
		   }
			
		 }
	   }

	   private List<ClientDrivingLicenseTypePM> clientDrivingLicenseTypes;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("ClientDrivingLicenseClientDrivingLicenseType", "ClientId,Line","ClientId,ClientDrivingLicenseLine")]
	   [DataMember]
	   public virtual List<ClientDrivingLicenseTypePM> ClientDrivingLicenseTypes  
	   {
	        get
             {
                 if (clientDrivingLicenseTypes == null)
                 {
                     clientDrivingLicenseTypes = new List<ClientDrivingLicenseTypePM>();
                 }
                 return clientDrivingLicenseTypes;
              }
             set { clientDrivingLicenseTypes = value; }
	    }
		   
	   private List<ClientDrivingLicenseTypePM>  deletedClientDrivingLicenseTypes;
	   public virtual List<ClientDrivingLicenseTypePM> DeletedClientDrivingLicenseTypes  
	   {
	        get
             {
                 if ( deletedClientDrivingLicenseTypes == null)
                 {
                      deletedClientDrivingLicenseTypes = new List<ClientDrivingLicenseTypePM>();
                 }
                 return  deletedClientDrivingLicenseTypes;
              }
             set {  deletedClientDrivingLicenseTypes = value; }
	    }
	  	  private string drivingLicenseCountryName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DrivingLicenseCountryName  
	   {
	    
	     get
		{
		   return drivingLicenseCountryName;
		 }
		 set
		 {
		   if(drivingLicenseCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DrivingLicenseCountryName",OldValue=drivingLicenseCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   drivingLicenseCountryName=value;
		   }
			
		 }
	   }
   }
   
}
	 