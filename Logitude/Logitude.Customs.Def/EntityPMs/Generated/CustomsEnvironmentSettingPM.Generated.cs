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
   public partial class CustomsEnvironmentSettingPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string environmentCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EnvironmentCode  
	   {
	    
	     get
		{
		   return environmentCode;
		 }
		 set
		 {
		   if(environmentCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EnvironmentCode",OldValue=environmentCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   environmentCode=value;
		   }
			
		 }
	   }
	  private bool useRabbitMQ ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool UseRabbitMQ  
	   {
	    
	     get
		{
		   return useRabbitMQ;
		 }
		 set
		 {
		   if(useRabbitMQ != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UseRabbitMQ",OldValue=useRabbitMQ,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   useRabbitMQ=value;
		   }
			
		 }
	   }
	  private string rabbitHost ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RabbitHost  
	   {
	    
	     get
		{
		   return rabbitHost;
		 }
		 set
		 {
		   if(rabbitHost != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RabbitHost",OldValue=rabbitHost,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   rabbitHost=value;
		   }
			
		 }
	   }
	  private string rabbitUserName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RabbitUserName  
	   {
	    
	     get
		{
		   return rabbitUserName;
		 }
		 set
		 {
		   if(rabbitUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RabbitUserName",OldValue=rabbitUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   rabbitUserName=value;
		   }
			
		 }
	   }
	  private string rabbitPassword ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RabbitPassword  
	   {
	    
	     get
		{
		   return rabbitPassword;
		 }
		 set
		 {
		   if(rabbitPassword != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RabbitPassword",OldValue=rabbitPassword,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   rabbitPassword=value;
		   }
			
		 }
	   }
   }
   
}
	 