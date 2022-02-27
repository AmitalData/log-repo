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
   public partial class DeclarationCasualDetailsPM : EntityPM
   {
   	  private string declarationId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationId  
	   {
	    
	     get
		{
		   return declarationId;
		 }
		 set
		 {
		   if(declarationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationId",OldValue=declarationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationId=value;
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
	  private string casualSupplierName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualSupplierName  
	   {
	    
	     get
		{
		   return casualSupplierName;
		 }
		 set
		 {
		   if(casualSupplierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualSupplierName",OldValue=casualSupplierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualSupplierName=value;
		   }
			
		 }
	   }
	  private string casualSupplierAddress ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualSupplierAddress  
	   {
	    
	     get
		{
		   return casualSupplierAddress;
		 }
		 set
		 {
		   if(casualSupplierAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualSupplierAddress",OldValue=casualSupplierAddress,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualSupplierAddress=value;
		   }
			
		 }
	   }
	  private string casualImporterAddress1 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterAddress1  
	   {
	    
	     get
		{
		   return casualImporterAddress1;
		 }
		 set
		 {
		   if(casualImporterAddress1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterAddress1",OldValue=casualImporterAddress1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterAddress1=value;
		   }
			
		 }
	   }
	  private string casualImporterAddress2 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterAddress2  
	   {
	    
	     get
		{
		   return casualImporterAddress2;
		 }
		 set
		 {
		   if(casualImporterAddress2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterAddress2",OldValue=casualImporterAddress2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterAddress2=value;
		   }
			
		 }
	   }
	  private string casualImporterCity ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterCity  
	   {
	    
	     get
		{
		   return casualImporterCity;
		 }
		 set
		 {
		   if(casualImporterCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterCity",OldValue=casualImporterCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterCity=value;
		   }
			
		 }
	   }
	  private string casualImporterZipCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterZipCode  
	   {
	    
	     get
		{
		   return casualImporterZipCode;
		 }
		 set
		 {
		   if(casualImporterZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterZipCode",OldValue=casualImporterZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterZipCode=value;
		   }
			
		 }
	   }
	  private string casualImporterFax ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterFax  
	   {
	    
	     get
		{
		   return casualImporterFax;
		 }
		 set
		 {
		   if(casualImporterFax != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterFax",OldValue=casualImporterFax,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterFax=value;
		   }
			
		 }
	   }
	  private string casualImporterEmail ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterEmail  
	   {
	    
	     get
		{
		   return casualImporterEmail;
		 }
		 set
		 {
		   if(casualImporterEmail != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterEmail",OldValue=casualImporterEmail,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterEmail=value;
		   }
			
		 }
	   }
	  private string casualImporterTel ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterTel  
	   {
	    
	     get
		{
		   return casualImporterTel;
		 }
		 set
		 {
		   if(casualImporterTel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterTel",OldValue=casualImporterTel,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterTel=value;
		   }
			
		 }
	   }
	  private string casualImporterContact ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CasualImporterContact  
	   {
	    
	     get
		{
		   return casualImporterContact;
		 }
		 set
		 {
		   if(casualImporterContact != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CasualImporterContact",OldValue=casualImporterContact,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   casualImporterContact=value;
		   }
			
		 }
	   }
   }
   
}
	 