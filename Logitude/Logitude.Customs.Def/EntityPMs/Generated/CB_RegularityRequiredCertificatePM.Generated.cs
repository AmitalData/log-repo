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
   public partial class CB_RegularityRequiredCertificatePM : EntityPM
   {
   	  private int iD ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private int regularityInceptionID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int RegularityInceptionID  
	   {
	    
	     get
		{
		   return regularityInceptionID;
		 }
		 set
		 {
		   if(regularityInceptionID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RegularityInceptionID",OldValue=regularityInceptionID,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   regularityInceptionID=value;
		   }
			
		 }
	   }
	  private string confirmationTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConfirmationTypeID  
	   {
	    
	     get
		{
		   return confirmationTypeID;
		 }
		 set
		 {
		   if(confirmationTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConfirmationTypeID",OldValue=confirmationTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   confirmationTypeID=value;
		   }
			
		 }
	   }
	  private int? number ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Number  
	   {
	    
	     get
		{
		   return number;
		 }
		 set
		 {
		   if(number != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Number",OldValue=number,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   number=value;
		   }
			
		 }
	   }
	  private string textualCondition ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TextualCondition  
	   {
	    
	     get
		{
		   return textualCondition;
		 }
		 set
		 {
		   if(textualCondition != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TextualCondition",OldValue=textualCondition,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   textualCondition=value;
		   }
			
		 }
	   }
	  private int? trNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? TrNumber  
	   {
	    
	     get
		{
		   return trNumber;
		 }
		 set
		 {
		   if(trNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TrNumber",OldValue=trNumber,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   trNumber=value;
		   }
			
		 }
	   }
	  private string authorityID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string AuthorityID  
	   {
	    
	     get
		{
		   return authorityID;
		 }
		 set
		 {
		   if(authorityID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AuthorityID",OldValue=authorityID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   authorityID=value;
		   }
			
		 }
	   }
	  private string cB_ID ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CB_ID  
	   {
	    
	     get
		{
		   return cB_ID;
		 }
		 set
		 {
		   if(cB_ID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CB_ID",OldValue=cB_ID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cB_ID=value;
		   }
			
		 }
	   }
   }
   
}
	 