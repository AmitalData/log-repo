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
   public partial class SupplierInvoiceItemVehiclePM : EntityPM
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
	  private int invoiceCounterKey ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int InvoiceCounterKey  
	   {
	    
	     get
		{
		   return invoiceCounterKey;
		 }
		 set
		 {
		   if(invoiceCounterKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceCounterKey",OldValue=invoiceCounterKey,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   invoiceCounterKey=value;
		   }
			
		 }
	   }
	  private int invoiceItemLineNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int InvoiceItemLineNumber  
	   {
	    
	     get
		{
		   return invoiceItemLineNumber;
		 }
		 set
		 {
		   if(invoiceItemLineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InvoiceItemLineNumber",OldValue=invoiceItemLineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   invoiceItemLineNumber=value;
		   }
			
		 }
	   }
	  private int lineNumber ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int LineNumber  
	   {
	    
	     get
		{
		   return lineNumber;
		 }
		 set
		 {
		   if(lineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineNumber",OldValue=lineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   lineNumber=value;
		   }
			
		 }
	   }
	  private int? sequenceNumeric ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? SequenceNumeric  
	   {
	    
	     get
		{
		   return sequenceNumeric;
		 }
		 set
		 {
		   if(sequenceNumeric != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SequenceNumeric",OldValue=sequenceNumeric,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   sequenceNumeric=value;
		   }
			
		 }
	   }
	  private string vehicleTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VehicleTypeCode  
	   {
	    
	     get
		{
		   return vehicleTypeCode;
		 }
		 set
		 {
		   if(vehicleTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleTypeCode",OldValue=vehicleTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vehicleTypeCode=value;
		   }
			
		 }
	   }
	  private string vehicleChassisNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VehicleChassisNumber  
	   {
	    
	     get
		{
		   return vehicleChassisNumber;
		 }
		 set
		 {
		   if(vehicleChassisNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleChassisNumber",OldValue=vehicleChassisNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vehicleChassisNumber=value;
		   }
			
		 }
	   }
	  private string richbitFileNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RichbitFileNumber  
	   {
	    
	     get
		{
		   return richbitFileNumber;
		 }
		 set
		 {
		   if(richbitFileNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RichbitFileNumber",OldValue=richbitFileNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   richbitFileNumber=value;
		   }
			
		 }
	   }
	  private string vehicleId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VehicleId  
	   {
	    
	     get
		{
		   return vehicleId;
		 }
		 set
		 {
		   if(vehicleId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleId",OldValue=vehicleId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vehicleId=value;
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
	  private string richbitFileStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string RichbitFileStatus  
	   {
	    
	     get
		{
		   return richbitFileStatus;
		 }
		 set
		 {
		   if(richbitFileStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RichbitFileStatus",OldValue=richbitFileStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   richbitFileStatus=value;
		   }
			
		 }
	   }

	   private List<SupplierInvoiceItemVehicleModPM> supplierInvoiceItemVehicleMods;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemVehicleSupplierInvoiceItemVehicleMods", "DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber,VehicleLineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemVehicleModPM> SupplierInvoiceItemVehicleMods  
	   {
	        get
             {
                 if (supplierInvoiceItemVehicleMods == null)
                 {
                     supplierInvoiceItemVehicleMods = new List<SupplierInvoiceItemVehicleModPM>();
                 }
                 return supplierInvoiceItemVehicleMods;
              }
             set { supplierInvoiceItemVehicleMods = value; }
	    }
		   
	   private List<SupplierInvoiceItemVehicleModPM>  deletedSupplierInvoiceItemVehicleMods;
	   public virtual List<SupplierInvoiceItemVehicleModPM> DeletedSupplierInvoiceItemVehicleMods  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemVehicleMods == null)
                 {
                      deletedSupplierInvoiceItemVehicleMods = new List<SupplierInvoiceItemVehicleModPM>();
                 }
                 return  deletedSupplierInvoiceItemVehicleMods;
              }
             set {  deletedSupplierInvoiceItemVehicleMods = value; }
	    }
	  
	   private List<SupplierInvoiceItemVehicleAddPM> supplierInvoiceItemVehicleAdds;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("SupplierInvoiceItemVehicleSupplierInvoiceItemVehicleAdds", "DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber,LineNumber","DeclarationId,InvoiceCounterKey,InvoiceItemLineNumber,LineNumber")]
	   [DataMember]
	   public virtual List<SupplierInvoiceItemVehicleAddPM> SupplierInvoiceItemVehicleAdds  
	   {
	        get
             {
                 if (supplierInvoiceItemVehicleAdds == null)
                 {
                     supplierInvoiceItemVehicleAdds = new List<SupplierInvoiceItemVehicleAddPM>();
                 }
                 return supplierInvoiceItemVehicleAdds;
              }
             set { supplierInvoiceItemVehicleAdds = value; }
	    }
		   
	   private List<SupplierInvoiceItemVehicleAddPM>  deletedSupplierInvoiceItemVehicleAdds;
	   public virtual List<SupplierInvoiceItemVehicleAddPM> DeletedSupplierInvoiceItemVehicleAdds  
	   {
	        get
             {
                 if ( deletedSupplierInvoiceItemVehicleAdds == null)
                 {
                      deletedSupplierInvoiceItemVehicleAdds = new List<SupplierInvoiceItemVehicleAddPM>();
                 }
                 return  deletedSupplierInvoiceItemVehicleAdds;
              }
             set {  deletedSupplierInvoiceItemVehicleAdds = value; }
	    }
	  	  private bool excludeFromInterface ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ExcludeFromInterface  
	   {
	    
	     get
		{
		   return excludeFromInterface;
		 }
		 set
		 {
		   if(excludeFromInterface != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExcludeFromInterface",OldValue=excludeFromInterface,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   excludeFromInterface=value;
		   }
			
		 }
	   }
	  private string identifierID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IdentifierID  
	   {
	    
	     get
		{
		   return identifierID;
		 }
		 set
		 {
		   if(identifierID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IdentifierID",OldValue=identifierID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   identifierID=value;
		   }
			
		 }
	   }
	  private string vehicleTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string VehicleTypeName  
	   {
	    
	     get
		{
		   return vehicleTypeName;
		 }
		 set
		 {
		   if(vehicleTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VehicleTypeName",OldValue=vehicleTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   vehicleTypeName=value;
		   }
			
		 }
	   }
   }
   
}
	 