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
using Logitude.Accounting.Def.Validators;
  
namespace Logitude.Accounting.Def.EntityPMs
{
   [CustomValidation(typeof(AccountingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class AutomaticReconcileMethodPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string code ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Code  
	   {
	    
	     get
		{
		   return code;
		 }
		 set
		 {
		   if(code != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Code",OldValue=code,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   code=value;
		   }
			
		 }
	   }
	  private string automaticReconcile1 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutomaticReconcile1  
	   {
	    
	     get
		{
		   return automaticReconcile1;
		 }
		 set
		 {
		   if(automaticReconcile1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticReconcile1",OldValue=automaticReconcile1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   automaticReconcile1=value;
		   }
			
		 }
	   }
	  private string automaticReconcile2 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutomaticReconcile2  
	   {
	    
	     get
		{
		   return automaticReconcile2;
		 }
		 set
		 {
		   if(automaticReconcile2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticReconcile2",OldValue=automaticReconcile2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   automaticReconcile2=value;
		   }
			
		 }
	   }
	  private string automaticReconcile3 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutomaticReconcile3  
	   {
	    
	     get
		{
		   return automaticReconcile3;
		 }
		 set
		 {
		   if(automaticReconcile3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticReconcile3",OldValue=automaticReconcile3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   automaticReconcile3=value;
		   }
			
		 }
	   }
	  private string automaticReconcileName1 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutomaticReconcileName1  
	   {
	    
	     get
		{
		   return automaticReconcileName1;
		 }
		 set
		 {
		   if(automaticReconcileName1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticReconcileName1",OldValue=automaticReconcileName1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   automaticReconcileName1=value;
		   }
			
		 }
	   }
	  private string automaticReconcileName2 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutomaticReconcileName2  
	   {
	    
	     get
		{
		   return automaticReconcileName2;
		 }
		 set
		 {
		   if(automaticReconcileName2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticReconcileName2",OldValue=automaticReconcileName2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   automaticReconcileName2=value;
		   }
			
		 }
	   }
	  private string automaticReconcileName3 ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AutomaticReconcileName3  
	   {
	    
	     get
		{
		   return automaticReconcileName3;
		 }
		 set
		 {
		   if(automaticReconcileName3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AutomaticReconcileName3",OldValue=automaticReconcileName3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   automaticReconcileName3=value;
		   }
			
		 }
	   }
	  private string name ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Name  
	   {
	    
	     get
		{
		   return name;
		 }
		 set
		 {
		   if(name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Name",OldValue=name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   name=value;
		   }
			
		 }
	   }
	  private bool inactive ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Inactive  
	   {
	    
	     get
		{
		   return inactive;
		 }
		 set
		 {
		   if(inactive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Inactive",OldValue=inactive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   inactive=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string localName ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LocalName  
	   {
	    
	     get
		{
		   return localName;
		 }
		 set
		 {
		   if(localName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LocalName",OldValue=localName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   localName=value;
		   }
			
		 }
	   }
	    }
   
}
	 