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
   public partial class FlightsSchedulesResponsePM : EntityPM
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
	  private string fromPortId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortId  
	   {
	    
	     get
		{
		   return fromPortId;
		 }
		 set
		 {
		   if(fromPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortId",OldValue=fromPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortId=value;
		   }
			
		 }
	   }
	  private string toPortId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortId  
	   {
	    
	     get
		{
		   return toPortId;
		 }
		 set
		 {
		   if(toPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortId",OldValue=toPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortId=value;
		   }
			
		 }
	   }
	  private string airlineId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirlineId  
	   {
	    
	     get
		{
		   return airlineId;
		 }
		 set
		 {
		   if(airlineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirlineId",OldValue=airlineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airlineId=value;
		   }
			
		 }
	   }
	  private string requestId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestId  
	   {
	    
	     get
		{
		   return requestId;
		 }
		 set
		 {
		   if(requestId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestId",OldValue=requestId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestId=value;
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
	  private DateTime? eTA ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ETA  
	   {
	    
	     get
		{
		   return eTA;
		 }
		 set
		 {
		   if(eTA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ETA",OldValue=eTA,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   eTA=value;
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
	  private string airplaneType ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirplaneType  
	   {
	    
	     get
		{
		   return airplaneType;
		 }
		 set
		 {
		   if(airplaneType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirplaneType",OldValue=airplaneType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airplaneType=value;
		   }
			
		 }
	   }
	  private int numberOfStops ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public int NumberOfStops  
	   {
	    
	     get
		{
		   return numberOfStops;
		 }
		 set
		 {
		   if(numberOfStops != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfStops",OldValue=numberOfStops,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   numberOfStops=value;
		   }
			
		 }
	   }
	  private int resultNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public int ResultNumber  
	   {
	    
	     get
		{
		   return resultNumber;
		 }
		 set
		 {
		   if(resultNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResultNumber",OldValue=resultNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   resultNumber=value;
		   }
			
		 }
	   }
	  private int lineNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
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
	  private string fromPortCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortCode  
	   {
	    
	     get
		{
		   return fromPortCode;
		 }
		 set
		 {
		   if(fromPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortCode",OldValue=fromPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortCode=value;
		   }
			
		 }
	   }
	  private string fromPortName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortName  
	   {
	    
	     get
		{
		   return fromPortName;
		 }
		 set
		 {
		   if(fromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortName",OldValue=fromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortName=value;
		   }
			
		 }
	   }
	  private string toPortCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortCode  
	   {
	    
	     get
		{
		   return toPortCode;
		 }
		 set
		 {
		   if(toPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortCode",OldValue=toPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortCode=value;
		   }
			
		 }
	   }
	  private string toPortName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortName  
	   {
	    
	     get
		{
		   return toPortName;
		 }
		 set
		 {
		   if(toPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortName",OldValue=toPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortName=value;
		   }
			
		 }
	   }
	  private bool missingPort ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool MissingPort  
	   {
	    
	     get
		{
		   return missingPort;
		 }
		 set
		 {
		   if(missingPort != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MissingPort",OldValue=missingPort,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   missingPort=value;
		   }
			
		 }
	   }
	  private string airlineCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirlineCode  
	   {
	    
	     get
		{
		   return airlineCode;
		 }
		 set
		 {
		   if(airlineCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirlineCode",OldValue=airlineCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airlineCode=value;
		   }
			
		 }
	   }
	  private string airlineName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirlineName  
	   {
	    
	     get
		{
		   return airlineName;
		 }
		 set
		 {
		   if(airlineName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirlineName",OldValue=airlineName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airlineName=value;
		   }
			
		 }
	   }
	  private string fromPortCountryCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortCountryCode  
	   {
	    
	     get
		{
		   return fromPortCountryCode;
		 }
		 set
		 {
		   if(fromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortCountryCode",OldValue=fromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string fromPortCountryName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromPortCountryName  
	   {
	    
	     get
		{
		   return fromPortCountryName;
		 }
		 set
		 {
		   if(fromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromPortCountryName",OldValue=fromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromPortCountryName=value;
		   }
			
		 }
	   }
	  private string toPortCountryCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortCountryCode  
	   {
	    
	     get
		{
		   return toPortCountryCode;
		 }
		 set
		 {
		   if(toPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortCountryCode",OldValue=toPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortCountryCode=value;
		   }
			
		 }
	   }
	  private string toPortCountryName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToPortCountryName  
	   {
	    
	     get
		{
		   return toPortCountryName;
		 }
		 set
		 {
		   if(toPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToPortCountryName",OldValue=toPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toPortCountryName=value;
		   }
			
		 }
	   }
   }
   
}
	 