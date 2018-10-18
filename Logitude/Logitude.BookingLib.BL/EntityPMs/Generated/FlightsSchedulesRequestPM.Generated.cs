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
   public partial class FlightsSchedulesRequestPM : EntityPM
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
	  private string shipmentId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentId  
	   {
	    
	     get
		{
		   return shipmentId;
		 }
		 set
		 {
		   if(shipmentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentId",OldValue=shipmentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentId=value;
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
	  private decimal? grossWeight ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? GrossWeight  
	   {
	    
	     get
		{
		   return grossWeight;
		 }
		 set
		 {
		   if(grossWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeight",OldValue=grossWeight,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   grossWeight=value;
		   }
			
		 }
	   }
	  private string volumeUnitCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string VolumeUnitCode  
	   {
	    
	     get
		{
		   return volumeUnitCode;
		 }
		 set
		 {
		   if(volumeUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="VolumeUnitCode",OldValue=volumeUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   volumeUnitCode=value;
		   }
			
		 }
	   }
	  private string grossWeightUnitCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string GrossWeightUnitCode  
	   {
	    
	     get
		{
		   return grossWeightUnitCode;
		 }
		 set
		 {
		   if(grossWeightUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeightUnitCode",OldValue=grossWeightUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   grossWeightUnitCode=value;
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
	  private DateTime? responseDate ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ResponseDate  
	   {
	    
	     get
		{
		   return responseDate;
		 }
		 set
		 {
		   if(responseDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ResponseDate",OldValue=responseDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   responseDate=value;
		   }
			
		 }
	   }
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
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
	  private string answerOSI ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AnswerOSI  
	   {
	    
	     get
		{
		   return answerOSI;
		 }
		 set
		 {
		   if(answerOSI != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnswerOSI",OldValue=answerOSI,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   answerOSI=value;
		   }
			
		 }
	   }
	  private string answerReasonForNoReply ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AnswerReasonForNoReply  
	   {
	    
	     get
		{
		   return answerReasonForNoReply;
		 }
		 set
		 {
		   if(answerReasonForNoReply != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnswerReasonForNoReply",OldValue=answerReasonForNoReply,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   answerReasonForNoReply=value;
		   }
			
		 }
	   }
	  private string requestDetails ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string RequestDetails  
	   {
	    
	     get
		{
		   return requestDetails;
		 }
		 set
		 {
		   if(requestDetails != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RequestDetails",OldValue=requestDetails,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   requestDetails=value;
		   }
			
		 }
	   }

	   private List<FlightsSchedulesResponsePM> responses;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("FlightsSchedulesRequestResponses", "Id","RequestId")]
	   [DataMember]
	   public virtual List<FlightsSchedulesResponsePM> Responses  
	   {
	        get
             {
                 if (responses == null)
                 {
                     responses = new List<FlightsSchedulesResponsePM>();
                 }
                 return responses;
              }
             set { responses = value; }
	    }
		   
	   private List<FlightsSchedulesResponsePM>  deletedResponses;
	   public virtual List<FlightsSchedulesResponsePM> DeletedResponses  
	   {
	        get
             {
                 if ( deletedResponses == null)
                 {
                      deletedResponses = new List<FlightsSchedulesResponsePM>();
                 }
                 return  deletedResponses;
              }
             set {  deletedResponses = value; }
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
   }
   
}
	 