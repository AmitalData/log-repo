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
   public partial class CustomsPartnerFtpPM : EntityPM
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
	  private string typeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeCode  
	   {
	    
	     get
		{
		   return typeCode;
		 }
		 set
		 {
		   if(typeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeCode",OldValue=typeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeCode=value;
		   }
			
		 }
	   }
	  private string partnerCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string PartnerCode  
	   {
	    
	     get
		{
		   return partnerCode;
		 }
		 set
		 {
		   if(partnerCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PartnerCode",OldValue=partnerCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   partnerCode=value;
		   }
			
		 }
	   }
	  private string interfaceName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterfaceName  
	   {
	    
	     get
		{
		   return interfaceName;
		 }
		 set
		 {
		   if(interfaceName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterfaceName",OldValue=interfaceName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interfaceName=value;
		   }
			
		 }
	   }
	  private string ftpDetailsId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FtpDetailsId  
	   {
	    
	     get
		{
		   return ftpDetailsId;
		 }
		 set
		 {
		   if(ftpDetailsId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FtpDetailsId",OldValue=ftpDetailsId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ftpDetailsId=value;
		   }
			
		 }
	   }
	  private string fileName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FileName  
	   {
	    
	     get
		{
		   return fileName;
		 }
		 set
		 {
		   if(fileName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileName",OldValue=fileName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fileName=value;
		   }
			
		 }
	   }
	  private string fileExt ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FileExt  
	   {
	    
	     get
		{
		   return fileExt;
		 }
		 set
		 {
		   if(fileExt != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FileExt",OldValue=fileExt,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fileExt=value;
		   }
			
		 }
	   }
	  private string communicationDetails ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CommunicationDetails  
	   {
	    
	     get
		{
		   return communicationDetails;
		 }
		 set
		 {
		   if(communicationDetails != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CommunicationDetails",OldValue=communicationDetails,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   communicationDetails=value;
		   }
			
		 }
	   }
	    }
   
}
	 