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
using Logitude.BookingLib.BL.Validators;
  
namespace Logitude.BookingLib.BL.EntityPMs
{
   [CustomValidation(typeof(BookingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class BookingPackagePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
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
	  private string bookingId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingId  
	   {
	    
	     get
		{
		   return bookingId;
		 }
		 set
		 {
		   if(bookingId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingId",OldValue=bookingId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingId=value;
		   }
			
		 }
	   }
	  private string description ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Description  
	   {
	    
	     get
		{
		   return description;
		 }
		 set
		 {
		   if(description != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Description",OldValue=description,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   description=value;
		   }
			
		 }
	   }
	  private string packageTypeId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeId  
	   {
	    
	     get
		{
		   return packageTypeId;
		 }
		 set
		 {
		   if(packageTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeId",OldValue=packageTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeId=value;
		   }
			
		 }
	   }
	  private string containerNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerNumber  
	   {
	    
	     get
		{
		   return containerNumber;
		 }
		 set
		 {
		   if(containerNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerNumber",OldValue=containerNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerNumber=value;
		   }
			
		 }
	   }
	  private string seal ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Seal  
	   {
	    
	     get
		{
		   return seal;
		 }
		 set
		 {
		   if(seal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Seal",OldValue=seal,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   seal=value;
		   }
			
		 }
	   }
	  private int? quantity ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Quantity  
	   {
	    
	     get
		{
		   return quantity;
		 }
		 set
		 {
		   if(quantity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Quantity",OldValue=quantity,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   quantity=value;
		   }
			
		 }
	   }
	  private decimal? weight ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Weight  
	   {
	    
	     get
		{
		   return weight;
		 }
		 set
		 {
		   if(weight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Weight",OldValue=weight,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   weight=value;
		   }
			
		 }
	   }
	  private decimal? volume ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Volume  
	   {
	    
	     get
		{
		   return volume;
		 }
		 set
		 {
		   if(volume != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Volume",OldValue=volume,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   volume=value;
		   }
			
		 }
	   }
	  private decimal? tare ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Tare  
	   {
	    
	     get
		{
		   return tare;
		 }
		 set
		 {
		   if(tare != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Tare",OldValue=tare,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   tare=value;
		   }
			
		 }
	   }
	  private decimal? height ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Height  
	   {
	    
	     get
		{
		   return height;
		 }
		 set
		 {
		   if(height != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Height",OldValue=height,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   height=value;
		   }
			
		 }
	   }
	  private decimal? width ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Width  
	   {
	    
	     get
		{
		   return width;
		 }
		 set
		 {
		   if(width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Width",OldValue=width,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   width=value;
		   }
			
		 }
	   }
	  private decimal? length ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Length  
	   {
	    
	     get
		{
		   return length;
		 }
		 set
		 {
		   if(length != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Length",OldValue=length,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   length=value;
		   }
			
		 }
	   }
	  private string unNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UnNumber  
	   {
	    
	     get
		{
		   return unNumber;
		 }
		 set
		 {
		   if(unNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UnNumber",OldValue=unNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   unNumber=value;
		   }
			
		 }
	   }
	  private string classNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClassNumber  
	   {
	    
	     get
		{
		   return classNumber;
		 }
		 set
		 {
		   if(classNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClassNumber",OldValue=classNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   classNumber=value;
		   }
			
		 }
	   }
	  private decimal? temperature ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Temperature  
	   {
	    
	     get
		{
		   return temperature;
		 }
		 set
		 {
		   if(temperature != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Temperature",OldValue=temperature,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   temperature=value;
		   }
			
		 }
	   }
	  private decimal? ventilation ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Ventilation  
	   {
	    
	     get
		{
		   return ventilation;
		 }
		 set
		 {
		   if(ventilation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Ventilation",OldValue=ventilation,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   ventilation=value;
		   }
			
		 }
	   }
	  private string seal2 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Seal2  
	   {
	    
	     get
		{
		   return seal2;
		 }
		 set
		 {
		   if(seal2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Seal2",OldValue=seal2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   seal2=value;
		   }
			
		 }
	   }
	  private int? sOC ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? SOC  
	   {
	    
	     get
		{
		   return sOC;
		 }
		 set
		 {
		   if(sOC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SOC",OldValue=sOC,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   sOC=value;
		   }
			
		 }
	   }
	  private string marksAndNumbers ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MarksAndNumbers  
	   {
	    
	     get
		{
		   return marksAndNumbers;
		 }
		 set
		 {
		   if(marksAndNumbers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MarksAndNumbers",OldValue=marksAndNumbers,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   marksAndNumbers=value;
		   }
			
		 }
	   }
	  private string packagingGroup ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackagingGroup  
	   {
	    
	     get
		{
		   return packagingGroup;
		 }
		 set
		 {
		   if(packagingGroup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackagingGroup",OldValue=packagingGroup,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packagingGroup=value;
		   }
			
		 }
	   }
	  private string iMDGCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string IMDGCode  
	   {
	    
	     get
		{
		   return iMDGCode;
		 }
		 set
		 {
		   if(iMDGCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IMDGCode",OldValue=iMDGCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   iMDGCode=value;
		   }
			
		 }
	   }
	  private string flashPoint ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FlashPoint  
	   {
	    
	     get
		{
		   return flashPoint;
		 }
		 set
		 {
		   if(flashPoint != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FlashPoint",OldValue=flashPoint,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   flashPoint=value;
		   }
			
		 }
	   }
	  private string harmonize ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Harmonize  
	   {
	    
	     get
		{
		   return harmonize;
		 }
		 set
		 {
		   if(harmonize != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Harmonize",OldValue=harmonize,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   harmonize=value;
		   }
			
		 }
	   }
	  private string materialDescription ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MaterialDescription  
	   {
	    
	     get
		{
		   return materialDescription;
		 }
		 set
		 {
		   if(materialDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MaterialDescription",OldValue=materialDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   materialDescription=value;
		   }
			
		 }
	   }
	  private bool isDangerous ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDangerous  
	   {
	    
	     get
		{
		   return isDangerous;
		 }
		 set
		 {
		   if(isDangerous != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDangerous",OldValue=isDangerous,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDangerous=value;
		   }
			
		 }
	   }
	  private string originalBookingPackageId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginalBookingPackageId  
	   {
	    
	     get
		{
		   return originalBookingPackageId;
		 }
		 set
		 {
		   if(originalBookingPackageId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginalBookingPackageId",OldValue=originalBookingPackageId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originalBookingPackageId=value;
		   }
			
		 }
	   }
	  private decimal? volumetricWeight ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? VolumetricWeight  
	   {
	    
	     get
		{
		   return volumetricWeight;
		 }
		 set
		 {
		   if(volumetricWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumetricWeight",OldValue=volumetricWeight,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   volumetricWeight=value;
		   }
			
		 }
	   }
	  private string commodityId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CommodityId  
	   {
	    
	     get
		{
		   return commodityId;
		 }
		 set
		 {
		   if(commodityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CommodityId",OldValue=commodityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   commodityId=value;
		   }
			
		 }
	   }
	  private string packageTypeName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackageTypeName  
	   {
	    
	     get
		{
		   return packageTypeName;
		 }
		 set
		 {
		   if(packageTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackageTypeName",OldValue=packageTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packageTypeName=value;
		   }
			
		 }
	   }
   }
   
}
	 