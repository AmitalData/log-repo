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
   public partial class BookingAnswerPM : EntityPM
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
	  private string statusCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string StatusCode  
	   {
	    
	     get
		{
		   return statusCode;
		 }
		 set
		 {
		   if(statusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StatusCode",OldValue=statusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   statusCode=value;
		   }
			
		 }
	   }
	  private DateTime? eTD ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ETD  
	   {
	    
	     get
		{
		   return eTD;
		 }
		 set
		 {
		   if(eTD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ETD",OldValue=eTD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   eTD=value;
		   }
			
		 }
	   }
	  private DateTime? createDate ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private string master ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
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
	  private string origin ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Origin  
	   {
	    
	     get
		{
		   return origin;
		 }
		 set
		 {
		   if(origin != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Origin",OldValue=origin,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   origin=value;
		   }
			
		 }
	   }
	  private string destination ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Destination  
	   {
	    
	     get
		{
		   return destination;
		 }
		 set
		 {
		   if(destination != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Destination",OldValue=destination,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destination=value;
		   }
			
		 }
	   }
	  private string communicationLogId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CommunicationLogId  
	   {
	    
	     get
		{
		   return communicationLogId;
		 }
		 set
		 {
		   if(communicationLogId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CommunicationLogId",OldValue=communicationLogId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   communicationLogId=value;
		   }
			
		 }
	   }
	  private string flightNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FlightNumber  
	   {
	    
	     get
		{
		   return flightNumber;
		 }
		 set
		 {
		   if(flightNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FlightNumber",OldValue=flightNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   flightNumber=value;
		   }
			
		 }
	   }
	  private string bookingSpaceAllocationCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingSpaceAllocationCode  
	   {
	    
	     get
		{
		   return bookingSpaceAllocationCode;
		 }
		 set
		 {
		   if(bookingSpaceAllocationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingSpaceAllocationCode",OldValue=bookingSpaceAllocationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingSpaceAllocationCode=value;
		   }
			
		 }
	   }
	  private string carrierId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierId  
	   {
	    
	     get
		{
		   return carrierId;
		 }
		 set
		 {
		   if(carrierId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierId",OldValue=carrierId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierId=value;
		   }
			
		 }
	   }
	  private string otherServicesInformation ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OtherServicesInformation  
	   {
	    
	     get
		{
		   return otherServicesInformation;
		 }
		 set
		 {
		   if(otherServicesInformation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OtherServicesInformation",OldValue=otherServicesInformation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   otherServicesInformation=value;
		   }
			
		 }
	   }
	  private string descriptionOfGoods ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DescriptionOfGoods  
	   {
	    
	     get
		{
		   return descriptionOfGoods;
		 }
		 set
		 {
		   if(descriptionOfGoods != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DescriptionOfGoods",OldValue=descriptionOfGoods,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   descriptionOfGoods=value;
		   }
			
		 }
	   }
	  private int? numberOfPieces ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? NumberOfPieces  
	   {
	    
	     get
		{
		   return numberOfPieces;
		 }
		 set
		 {
		   if(numberOfPieces != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfPieces",OldValue=numberOfPieces,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   numberOfPieces=value;
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
	  private string weightUnitCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string WeightUnitCode  
	   {
	    
	     get
		{
		   return weightUnitCode;
		 }
		 set
		 {
		   if(weightUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WeightUnitCode",OldValue=weightUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   weightUnitCode=value;
		   }
			
		 }
	   }
	  private string carrierName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CarrierName  
	   {
	    
	     get
		{
		   return carrierName;
		 }
		 set
		 {
		   if(carrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierName",OldValue=carrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   carrierName=value;
		   }
			
		 }
	   }
	  private string originCountryName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginCountryName  
	   {
	    
	     get
		{
		   return originCountryName;
		 }
		 set
		 {
		   if(originCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginCountryName",OldValue=originCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originCountryName=value;
		   }
			
		 }
	   }
	  private string originCountryCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string OriginCountryCode  
	   {
	    
	     get
		{
		   return originCountryCode;
		 }
		 set
		 {
		   if(originCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OriginCountryCode",OldValue=originCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   originCountryCode=value;
		   }
			
		 }
	   }
	  private string destinationCountryCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationCountryCode  
	   {
	    
	     get
		{
		   return destinationCountryCode;
		 }
		 set
		 {
		   if(destinationCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationCountryCode",OldValue=destinationCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationCountryCode=value;
		   }
			
		 }
	   }
	  private string destinationCountryName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DestinationCountryName  
	   {
	    
	     get
		{
		   return destinationCountryName;
		 }
		 set
		 {
		   if(destinationCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DestinationCountryName",OldValue=destinationCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   destinationCountryName=value;
		   }
			
		 }
	   }
   }
   
}
	 