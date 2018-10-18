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
   public partial class BookingPM : EntityPM
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
	  private string bookingNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingNumber  
	   {
	    
	     get
		{
		   return bookingNumber;
		 }
		 set
		 {
		   if(bookingNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingNumber",OldValue=bookingNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingNumber=value;
		   }
			
		 }
	   }
	  private string directionCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DirectionCode  
	   {
	    
	     get
		{
		   return directionCode;
		 }
		 set
		 {
		   if(directionCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DirectionCode",OldValue=directionCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   directionCode=value;
		   }
			
		 }
	   }
	  private string transportModeCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeCode  
	   {
	    
	     get
		{
		   return transportModeCode;
		 }
		 set
		 {
		   if(transportModeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeCode",OldValue=transportModeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeCode=value;
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
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
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
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId  
	   {
	    
	     get
		{
		   return updatedByUserId;
		 }
		 set
		 {
		   if(updatedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserId",OldValue=updatedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserId=value;
		   }
			
		 }
	   }
	  private string bookingStatusCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingStatusCode  
	   {
	    
	     get
		{
		   return bookingStatusCode;
		 }
		 set
		 {
		   if(bookingStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingStatusCode",OldValue=bookingStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingStatusCode=value;
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
	  private string spaceAllocationCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpaceAllocationCode  
	   {
	    
	     get
		{
		   return spaceAllocationCode;
		 }
		 set
		 {
		   if(spaceAllocationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpaceAllocationCode",OldValue=spaceAllocationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   spaceAllocationCode=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierId  
	   {
	    
	     get
		{
		   return mainCarriageCarrierId;
		 }
		 set
		 {
		   if(mainCarriageCarrierId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierId",OldValue=mainCarriageCarrierId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierId=value;
		   }
			
		 }
	   }
	  private bool mainCarriageIsFromStack ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool MainCarriageIsFromStack  
	   {
	    
	     get
		{
		   return mainCarriageIsFromStack;
		 }
		 set
		 {
		   if(mainCarriageIsFromStack != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageIsFromStack",OldValue=mainCarriageIsFromStack,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   mainCarriageIsFromStack=value;
		   }
			
		 }
	   }
	  private string mainCarriageSpaceAllocationCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageSpaceAllocationCode  
	   {
	    
	     get
		{
		   return mainCarriageSpaceAllocationCode;
		 }
		 set
		 {
		   if(mainCarriageSpaceAllocationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageSpaceAllocationCode",OldValue=mainCarriageSpaceAllocationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageSpaceAllocationCode=value;
		   }
			
		 }
	   }
	  private string mainCarriageAllotmentIdentification ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageAllotmentIdentification  
	   {
	    
	     get
		{
		   return mainCarriageAllotmentIdentification;
		 }
		 set
		 {
		   if(mainCarriageAllotmentIdentification != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageAllotmentIdentification",OldValue=mainCarriageAllotmentIdentification,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageAllotmentIdentification=value;
		   }
			
		 }
	   }
	  private string mainCarriageFromPortId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFromPortId  
	   {
	    
	     get
		{
		   return mainCarriageFromPortId;
		 }
		 set
		 {
		   if(mainCarriageFromPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFromPortId",OldValue=mainCarriageFromPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFromPortId=value;
		   }
			
		 }
	   }
	  private string mainCarriageToPortId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageToPortId  
	   {
	    
	     get
		{
		   return mainCarriageToPortId;
		 }
		 set
		 {
		   if(mainCarriageToPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageToPortId",OldValue=mainCarriageToPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageToPortId=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierPrefix ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierPrefix  
	   {
	    
	     get
		{
		   return mainCarriageCarrierPrefix;
		 }
		 set
		 {
		   if(mainCarriageCarrierPrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierPrefix",OldValue=mainCarriageCarrierPrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierPrefix=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierNumber  
	   {
	    
	     get
		{
		   return mainCarriageCarrierNumber;
		 }
		 set
		 {
		   if(mainCarriageCarrierNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierNumber",OldValue=mainCarriageCarrierNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierNumber=value;
		   }
			
		 }
	   }
	  private DateTime? mainCarriageETD ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? MainCarriageETD  
	   {
	    
	     get
		{
		   return mainCarriageETD;
		 }
		 set
		 {
		   if(mainCarriageETD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageETD",OldValue=mainCarriageETD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   mainCarriageETD=value;
		   }
			
		 }
	   }
	  private string transshipment1FromPortId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1FromPortId  
	   {
	    
	     get
		{
		   return transshipment1FromPortId;
		 }
		 set
		 {
		   if(transshipment1FromPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1FromPortId",OldValue=transshipment1FromPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1FromPortId=value;
		   }
			
		 }
	   }
	  private string transshipment1ToPortId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1ToPortId  
	   {
	    
	     get
		{
		   return transshipment1ToPortId;
		 }
		 set
		 {
		   if(transshipment1ToPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1ToPortId",OldValue=transshipment1ToPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1ToPortId=value;
		   }
			
		 }
	   }
	  private string transshipment1CarrierId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1CarrierId  
	   {
	    
	     get
		{
		   return transshipment1CarrierId;
		 }
		 set
		 {
		   if(transshipment1CarrierId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1CarrierId",OldValue=transshipment1CarrierId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1CarrierId=value;
		   }
			
		 }
	   }
	  private DateTime? transshipment1ETD ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? Transshipment1ETD  
	   {
	    
	     get
		{
		   return transshipment1ETD;
		 }
		 set
		 {
		   if(transshipment1ETD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1ETD",OldValue=transshipment1ETD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   transshipment1ETD=value;
		   }
			
		 }
	   }
	  private string transshipment1AllotmentIdentification ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1AllotmentIdentification  
	   {
	    
	     get
		{
		   return transshipment1AllotmentIdentification;
		 }
		 set
		 {
		   if(transshipment1AllotmentIdentification != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1AllotmentIdentification",OldValue=transshipment1AllotmentIdentification,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1AllotmentIdentification=value;
		   }
			
		 }
	   }
	  private string transshipment1CarrierPrefix ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1CarrierPrefix  
	   {
	    
	     get
		{
		   return transshipment1CarrierPrefix;
		 }
		 set
		 {
		   if(transshipment1CarrierPrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1CarrierPrefix",OldValue=transshipment1CarrierPrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1CarrierPrefix=value;
		   }
			
		 }
	   }
	  private string transshipment1CarrierNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1CarrierNumber  
	   {
	    
	     get
		{
		   return transshipment1CarrierNumber;
		 }
		 set
		 {
		   if(transshipment1CarrierNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1CarrierNumber",OldValue=transshipment1CarrierNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1CarrierNumber=value;
		   }
			
		 }
	   }
	  private string transshipment1SpaceAllocationCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1SpaceAllocationCode  
	   {
	    
	     get
		{
		   return transshipment1SpaceAllocationCode;
		 }
		 set
		 {
		   if(transshipment1SpaceAllocationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1SpaceAllocationCode",OldValue=transshipment1SpaceAllocationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1SpaceAllocationCode=value;
		   }
			
		 }
	   }
	  private string transshipment2FromPortId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2FromPortId  
	   {
	    
	     get
		{
		   return transshipment2FromPortId;
		 }
		 set
		 {
		   if(transshipment2FromPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2FromPortId",OldValue=transshipment2FromPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2FromPortId=value;
		   }
			
		 }
	   }
	  private string transshipment2ToPortId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2ToPortId  
	   {
	    
	     get
		{
		   return transshipment2ToPortId;
		 }
		 set
		 {
		   if(transshipment2ToPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2ToPortId",OldValue=transshipment2ToPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2ToPortId=value;
		   }
			
		 }
	   }
	  private string transshipment2CarrierId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2CarrierId  
	   {
	    
	     get
		{
		   return transshipment2CarrierId;
		 }
		 set
		 {
		   if(transshipment2CarrierId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2CarrierId",OldValue=transshipment2CarrierId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2CarrierId=value;
		   }
			
		 }
	   }
	  private DateTime? transshipment2ETD ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? Transshipment2ETD  
	   {
	    
	     get
		{
		   return transshipment2ETD;
		 }
		 set
		 {
		   if(transshipment2ETD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2ETD",OldValue=transshipment2ETD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   transshipment2ETD=value;
		   }
			
		 }
	   }
	  private string transshipment2AllotmentIdentification ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2AllotmentIdentification  
	   {
	    
	     get
		{
		   return transshipment2AllotmentIdentification;
		 }
		 set
		 {
		   if(transshipment2AllotmentIdentification != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2AllotmentIdentification",OldValue=transshipment2AllotmentIdentification,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2AllotmentIdentification=value;
		   }
			
		 }
	   }
	  private string transshipment2CarrierPrefix ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2CarrierPrefix  
	   {
	    
	     get
		{
		   return transshipment2CarrierPrefix;
		 }
		 set
		 {
		   if(transshipment2CarrierPrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2CarrierPrefix",OldValue=transshipment2CarrierPrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2CarrierPrefix=value;
		   }
			
		 }
	   }
	  private string transshipment2CarrierNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2CarrierNumber  
	   {
	    
	     get
		{
		   return transshipment2CarrierNumber;
		 }
		 set
		 {
		   if(transshipment2CarrierNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2CarrierNumber",OldValue=transshipment2CarrierNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2CarrierNumber=value;
		   }
			
		 }
	   }
	  private string transshipment2SpaceAllocationCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2SpaceAllocationCode  
	   {
	    
	     get
		{
		   return transshipment2SpaceAllocationCode;
		 }
		 set
		 {
		   if(transshipment2SpaceAllocationCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2SpaceAllocationCode",OldValue=transshipment2SpaceAllocationCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2SpaceAllocationCode=value;
		   }
			
		 }
	   }
	  private string fFRStatusCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FFRStatusCode  
	   {
	    
	     get
		{
		   return fFRStatusCode;
		 }
		 set
		 {
		   if(fFRStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FFRStatusCode",OldValue=fFRStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fFRStatusCode=value;
		   }
			
		 }
	   }
	  private DateTime? fFRStatusDate ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FFRStatusDate  
	   {
	    
	     get
		{
		   return fFRStatusDate;
		 }
		 set
		 {
		   if(fFRStatusDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FFRStatusDate",OldValue=fFRStatusDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fFRStatusDate=value;
		   }
			
		 }
	   }
	  private string fNAReason ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FNAReason  
	   {
	    
	     get
		{
		   return fNAReason;
		 }
		 set
		 {
		   if(fNAReason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FNAReason",OldValue=fNAReason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fNAReason=value;
		   }
			
		 }
	   }
	  private string shipperId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperId  
	   {
	    
	     get
		{
		   return shipperId;
		 }
		 set
		 {
		   if(shipperId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperId",OldValue=shipperId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperId=value;
		   }
			
		 }
	   }
	  private string shipperAddressId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperAddressId  
	   {
	    
	     get
		{
		   return shipperAddressId;
		 }
		 set
		 {
		   if(shipperAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperAddressId",OldValue=shipperAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperAddressId=value;
		   }
			
		 }
	   }
	  private string shipperReference ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperReference  
	   {
	    
	     get
		{
		   return shipperReference;
		 }
		 set
		 {
		   if(shipperReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperReference",OldValue=shipperReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperReference=value;
		   }
			
		 }
	   }
	  private string consigneeId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeId  
	   {
	    
	     get
		{
		   return consigneeId;
		 }
		 set
		 {
		   if(consigneeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeId",OldValue=consigneeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeId=value;
		   }
			
		 }
	   }
	  private string consigneeAddressId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeAddressId  
	   {
	    
	     get
		{
		   return consigneeAddressId;
		 }
		 set
		 {
		   if(consigneeAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeAddressId",OldValue=consigneeAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeAddressId=value;
		   }
			
		 }
	   }
	  private string consigneeReference ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeReference  
	   {
	    
	     get
		{
		   return consigneeReference;
		 }
		 set
		 {
		   if(consigneeReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeReference",OldValue=consigneeReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeReference=value;
		   }
			
		 }
	   }
	  private string issuingCarrierAgentId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string IssuingCarrierAgentId  
	   {
	    
	     get
		{
		   return issuingCarrierAgentId;
		 }
		 set
		 {
		   if(issuingCarrierAgentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssuingCarrierAgentId",OldValue=issuingCarrierAgentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   issuingCarrierAgentId=value;
		   }
			
		 }
	   }
	  private string issuingCarrierAddressId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string IssuingCarrierAddressId  
	   {
	    
	     get
		{
		   return issuingCarrierAddressId;
		 }
		 set
		 {
		   if(issuingCarrierAddressId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssuingCarrierAddressId",OldValue=issuingCarrierAddressId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   issuingCarrierAddressId=value;
		   }
			
		 }
	   }
	  private string iATACodeId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string IATACodeId  
	   {
	    
	     get
		{
		   return iATACodeId;
		 }
		 set
		 {
		   if(iATACodeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IATACodeId",OldValue=iATACodeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   iATACodeId=value;
		   }
			
		 }
	   }
	  private string cASSCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CASSCode  
	   {
	    
	     get
		{
		   return cASSCode;
		 }
		 set
		 {
		   if(cASSCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CASSCode",OldValue=cASSCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cASSCode=value;
		   }
			
		 }
	   }
	  private string aWBSpecialHandlingCodeId1 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBSpecialHandlingCodeId1  
	   {
	    
	     get
		{
		   return aWBSpecialHandlingCodeId1;
		 }
		 set
		 {
		   if(aWBSpecialHandlingCodeId1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBSpecialHandlingCodeId1",OldValue=aWBSpecialHandlingCodeId1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBSpecialHandlingCodeId1=value;
		   }
			
		 }
	   }
	  private string aWBSpecialHandlingCodeId2 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBSpecialHandlingCodeId2  
	   {
	    
	     get
		{
		   return aWBSpecialHandlingCodeId2;
		 }
		 set
		 {
		   if(aWBSpecialHandlingCodeId2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBSpecialHandlingCodeId2",OldValue=aWBSpecialHandlingCodeId2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBSpecialHandlingCodeId2=value;
		   }
			
		 }
	   }
	  private string aWBSpecialHandlingCodeId3 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBSpecialHandlingCodeId3  
	   {
	    
	     get
		{
		   return aWBSpecialHandlingCodeId3;
		 }
		 set
		 {
		   if(aWBSpecialHandlingCodeId3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBSpecialHandlingCodeId3",OldValue=aWBSpecialHandlingCodeId3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBSpecialHandlingCodeId3=value;
		   }
			
		 }
	   }
	  private string aWBSpecialHandlingCodeId4 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBSpecialHandlingCodeId4  
	   {
	    
	     get
		{
		   return aWBSpecialHandlingCodeId4;
		 }
		 set
		 {
		   if(aWBSpecialHandlingCodeId4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBSpecialHandlingCodeId4",OldValue=aWBSpecialHandlingCodeId4,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBSpecialHandlingCodeId4=value;
		   }
			
		 }
	   }
	  private string aWBSpecialHandlingCodeId5 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBSpecialHandlingCodeId5  
	   {
	    
	     get
		{
		   return aWBSpecialHandlingCodeId5;
		 }
		 set
		 {
		   if(aWBSpecialHandlingCodeId5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBSpecialHandlingCodeId5",OldValue=aWBSpecialHandlingCodeId5,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBSpecialHandlingCodeId5=value;
		   }
			
		 }
	   }
	  private string aWBSpecialHandlingCodeId6 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBSpecialHandlingCodeId6  
	   {
	    
	     get
		{
		   return aWBSpecialHandlingCodeId6;
		 }
		 set
		 {
		   if(aWBSpecialHandlingCodeId6 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBSpecialHandlingCodeId6",OldValue=aWBSpecialHandlingCodeId6,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBSpecialHandlingCodeId6=value;
		   }
			
		 }
	   }
	  private string aWBSpecialHandlingCodeId7 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBSpecialHandlingCodeId7  
	   {
	    
	     get
		{
		   return aWBSpecialHandlingCodeId7;
		 }
		 set
		 {
		   if(aWBSpecialHandlingCodeId7 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBSpecialHandlingCodeId7",OldValue=aWBSpecialHandlingCodeId7,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBSpecialHandlingCodeId7=value;
		   }
			
		 }
	   }
	  private string aWBSpecialHandlingCodeId8 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBSpecialHandlingCodeId8  
	   {
	    
	     get
		{
		   return aWBSpecialHandlingCodeId8;
		 }
		 set
		 {
		   if(aWBSpecialHandlingCodeId8 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBSpecialHandlingCodeId8",OldValue=aWBSpecialHandlingCodeId8,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBSpecialHandlingCodeId8=value;
		   }
			
		 }
	   }
	  private string aWBSpecialHandlingCodeId9 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBSpecialHandlingCodeId9  
	   {
	    
	     get
		{
		   return aWBSpecialHandlingCodeId9;
		 }
		 set
		 {
		   if(aWBSpecialHandlingCodeId9 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBSpecialHandlingCodeId9",OldValue=aWBSpecialHandlingCodeId9,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBSpecialHandlingCodeId9=value;
		   }
			
		 }
	   }
	  private string aWBCarrierTarrifReference ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBCarrierTarrifReference  
	   {
	    
	     get
		{
		   return aWBCarrierTarrifReference;
		 }
		 set
		 {
		   if(aWBCarrierTarrifReference != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBCarrierTarrifReference",OldValue=aWBCarrierTarrifReference,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBCarrierTarrifReference=value;
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
	  private string notes ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Notes  
	   {
	    
	     get
		{
		   return notes;
		 }
		 set
		 {
		   if(notes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Notes",OldValue=notes,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notes=value;
		   }
			
		 }
	   }
	  private string specialServicesRequest ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpecialServicesRequest  
	   {
	    
	     get
		{
		   return specialServicesRequest;
		 }
		 set
		 {
		   if(specialServicesRequest != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpecialServicesRequest",OldValue=specialServicesRequest,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   specialServicesRequest=value;
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
	  private string bookingStatusName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingStatusName  
	   {
	    
	     get
		{
		   return bookingStatusName;
		 }
		 set
		 {
		   if(bookingStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingStatusName",OldValue=bookingStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingStatusName=value;
		   }
			
		 }
	   }
	  private string spaceAllocationName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SpaceAllocationName  
	   {
	    
	     get
		{
		   return spaceAllocationName;
		 }
		 set
		 {
		   if(spaceAllocationName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpaceAllocationName",OldValue=spaceAllocationName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   spaceAllocationName=value;
		   }
			
		 }
	   }
	  private string fFRStatusName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FFRStatusName  
	   {
	    
	     get
		{
		   return fFRStatusName;
		 }
		 set
		 {
		   if(fFRStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FFRStatusName",OldValue=fFRStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fFRStatusName=value;
		   }
			
		 }
	   }
	  private string routing ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Routing  
	   {
	    
	     get
		{
		   return routing;
		 }
		 set
		 {
		   if(routing != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Routing",OldValue=routing,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   routing=value;
		   }
			
		 }
	   }
	  private string firstFlight ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FirstFlight  
	   {
	    
	     get
		{
		   return firstFlight;
		 }
		 set
		 {
		   if(firstFlight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstFlight",OldValue=firstFlight,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   firstFlight=value;
		   }
			
		 }
	   }
	  private string airline ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Airline  
	   {
	    
	     get
		{
		   return airline;
		 }
		 set
		 {
		   if(airline != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Airline",OldValue=airline,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airline=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
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

	   private List<BookingPackagePM> bookingPackages;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("BookingBookingPackages", "Id","BookingId")]
	   [DataMember]
	   public virtual List<BookingPackagePM> BookingPackages  
	   {
	        get
             {
                 if (bookingPackages == null)
                 {
                     bookingPackages = new List<BookingPackagePM>();
                 }
                 return bookingPackages;
              }
             set { bookingPackages = value; }
	    }
		   
	   private List<BookingPackagePM>  deletedBookingPackages;
	   public virtual List<BookingPackagePM> DeletedBookingPackages  
	   {
	        get
             {
                 if ( deletedBookingPackages == null)
                 {
                      deletedBookingPackages = new List<BookingPackagePM>();
                 }
                 return  deletedBookingPackages;
              }
             set {  deletedBookingPackages = value; }
	    }
	  	  private string directionName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DirectionName  
	   {
	    
	     get
		{
		   return directionName;
		 }
		 set
		 {
		   if(directionName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DirectionName",OldValue=directionName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   directionName=value;
		   }
			
		 }
	   }
	  private string transportModeName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TransportModeName  
	   {
	    
	     get
		{
		   return transportModeName;
		 }
		 set
		 {
		   if(transportModeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModeName",OldValue=transportModeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transportModeName=value;
		   }
			
		 }
	   }
	  private string issuingCarrierIATACode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string IssuingCarrierIATACode  
	   {
	    
	     get
		{
		   return issuingCarrierIATACode;
		 }
		 set
		 {
		   if(issuingCarrierIATACode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssuingCarrierIATACode",OldValue=issuingCarrierIATACode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   issuingCarrierIATACode=value;
		   }
			
		 }
	   }
	  private string shipperName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperName  
	   {
	    
	     get
		{
		   return shipperName;
		 }
		 set
		 {
		   if(shipperName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperName",OldValue=shipperName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperName=value;
		   }
			
		 }
	   }
	  private string consigneeName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeName  
	   {
	    
	     get
		{
		   return consigneeName;
		 }
		 set
		 {
		   if(consigneeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeName",OldValue=consigneeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeName=value;
		   }
			
		 }
	   }
	  private string issuingCarrierAgentName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string IssuingCarrierAgentName  
	   {
	    
	     get
		{
		   return issuingCarrierAgentName;
		 }
		 set
		 {
		   if(issuingCarrierAgentName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IssuingCarrierAgentName",OldValue=issuingCarrierAgentName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   issuingCarrierAgentName=value;
		   }
			
		 }
	   }
	  private bool grossWeightEdited ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool GrossWeightEdited  
	   {
	    
	     get
		{
		   return grossWeightEdited;
		 }
		 set
		 {
		   if(grossWeightEdited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeightEdited",OldValue=grossWeightEdited,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   grossWeightEdited=value;
		   }
			
		 }
	   }
	  private bool chargeableWeightEdited ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ChargeableWeightEdited  
	   {
	    
	     get
		{
		   return chargeableWeightEdited;
		 }
		 set
		 {
		   if(chargeableWeightEdited != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeightEdited",OldValue=chargeableWeightEdited,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   chargeableWeightEdited=value;
		   }
			
		 }
	   }
	  private int? numberOfPackages ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public int? NumberOfPackages  
	   {
	    
	     get
		{
		   return numberOfPackages;
		 }
		 set
		 {
		   if(numberOfPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NumberOfPackages",OldValue=numberOfPackages,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   numberOfPackages=value;
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
	  private decimal? chargeableWeight ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ChargeableWeight  
	   {
	    
	     get
		{
		   return chargeableWeight;
		 }
		 set
		 {
		   if(chargeableWeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeight",OldValue=chargeableWeight,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   chargeableWeight=value;
		   }
			
		 }
	   }
	  private string aWBCommodityItemNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBCommodityItemNumber  
	   {
	    
	     get
		{
		   return aWBCommodityItemNumber;
		 }
		 set
		 {
		   if(aWBCommodityItemNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBCommodityItemNumber",OldValue=aWBCommodityItemNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBCommodityItemNumber=value;
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
	  private string chargeableWeightUnitCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargeableWeightUnitCode  
	   {
	    
	     get
		{
		   return chargeableWeightUnitCode;
		 }
		 set
		 {
		   if(chargeableWeightUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeightUnitCode",OldValue=chargeableWeightUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargeableWeightUnitCode=value;
		   }
			
		 }
	   }
	  private string dimensionsUnitCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DimensionsUnitCode  
	   {
	    
	     get
		{
		   return dimensionsUnitCode;
		 }
		 set
		 {
		   if(dimensionsUnitCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DimensionsUnitCode",OldValue=dimensionsUnitCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dimensionsUnitCode=value;
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
	  private decimal? grossWeightInKG ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? GrossWeightInKG  
	   {
	    
	     get
		{
		   return grossWeightInKG;
		 }
		 set
		 {
		   if(grossWeightInKG != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GrossWeightInKG",OldValue=grossWeightInKG,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   grossWeightInKG=value;
		   }
			
		 }
	   }
	  private decimal? chargeableWeightInKG ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? ChargeableWeightInKG  
	   {
	    
	     get
		{
		   return chargeableWeightInKG;
		 }
		 set
		 {
		   if(chargeableWeightInKG != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargeableWeightInKG",OldValue=chargeableWeightInKG,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   chargeableWeightInKG=value;
		   }
			
		 }
	   }
	  private decimal? ratio ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Ratio  
	   {
	    
	     get
		{
		   return ratio;
		 }
		 set
		 {
		   if(ratio != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Ratio",OldValue=ratio,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   ratio=value;
		   }
			
		 }
	   }
	  private decimal? dimFactor ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? DimFactor  
	   {
	    
	     get
		{
		   return dimFactor;
		 }
		 set
		 {
		   if(dimFactor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DimFactor",OldValue=dimFactor,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   dimFactor=value;
		   }
			
		 }
	   }
	  private string mainCarriageFinalDestinationPortId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageFinalDestinationPortId  
	   {
	    
	     get
		{
		   return mainCarriageFinalDestinationPortId;
		 }
		 set
		 {
		   if(mainCarriageFinalDestinationPortId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageFinalDestinationPortId",OldValue=mainCarriageFinalDestinationPortId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageFinalDestinationPortId=value;
		   }
			
		 }
	   }
	  private string longMaster ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LongMaster  
	   {
	    
	     get
		{
		   return longMaster;
		 }
		 set
		 {
		   if(longMaster != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LongMaster",OldValue=longMaster,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   longMaster=value;
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
	  private string dangerousClassNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DangerousClassNumber  
	   {
	    
	     get
		{
		   return dangerousClassNumber;
		 }
		 set
		 {
		   if(dangerousClassNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DangerousClassNumber",OldValue=dangerousClassNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dangerousClassNumber=value;
		   }
			
		 }
	   }
	  private string dangerousUnNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DangerousUnNumber  
	   {
	    
	     get
		{
		   return dangerousUnNumber;
		 }
		 set
		 {
		   if(dangerousUnNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DangerousUnNumber",OldValue=dangerousUnNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dangerousUnNumber=value;
		   }
			
		 }
	   }
	  private string dangerousPackagingGroup ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DangerousPackagingGroup  
	   {
	    
	     get
		{
		   return dangerousPackagingGroup;
		 }
		 set
		 {
		   if(dangerousPackagingGroup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DangerousPackagingGroup",OldValue=dangerousPackagingGroup,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dangerousPackagingGroup=value;
		   }
			
		 }
	   }
	  private string dangerousIMDGCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DangerousIMDGCode  
	   {
	    
	     get
		{
		   return dangerousIMDGCode;
		 }
		 set
		 {
		   if(dangerousIMDGCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DangerousIMDGCode",OldValue=dangerousIMDGCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dangerousIMDGCode=value;
		   }
			
		 }
	   }
	  private string dangerousFlashPoint ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DangerousFlashPoint  
	   {
	    
	     get
		{
		   return dangerousFlashPoint;
		 }
		 set
		 {
		   if(dangerousFlashPoint != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DangerousFlashPoint",OldValue=dangerousFlashPoint,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dangerousFlashPoint=value;
		   }
			
		 }
	   }
	  private string dangerousMaterialDescription ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DangerousMaterialDescription  
	   {
	    
	     get
		{
		   return dangerousMaterialDescription;
		 }
		 set
		 {
		   if(dangerousMaterialDescription != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DangerousMaterialDescription",OldValue=dangerousMaterialDescription,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dangerousMaterialDescription=value;
		   }
			
		 }
	   }
	  private string mainHarmonize ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainHarmonize  
	   {
	    
	     get
		{
		   return mainHarmonize;
		 }
		 set
		 {
		   if(mainHarmonize != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainHarmonize",OldValue=mainHarmonize,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainHarmonize=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierName  
	   {
	    
	     get
		{
		   return mainCarriageCarrierName;
		 }
		 set
		 {
		   if(mainCarriageCarrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierName",OldValue=mainCarriageCarrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierName=value;
		   }
			
		 }
	   }
	  private string aWBHandlingInformation ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBHandlingInformation  
	   {
	    
	     get
		{
		   return aWBHandlingInformation;
		 }
		 set
		 {
		   if(aWBHandlingInformation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBHandlingInformation",OldValue=aWBHandlingInformation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBHandlingInformation=value;
		   }
			
		 }
	   }
	  private string answerOtherServicesInformation ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AnswerOtherServicesInformation  
	   {
	    
	     get
		{
		   return answerOtherServicesInformation;
		 }
		 set
		 {
		   if(answerOtherServicesInformation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnswerOtherServicesInformation",OldValue=answerOtherServicesInformation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   answerOtherServicesInformation=value;
		   }
			
		 }
	   }
	  private bool hasResponse ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasResponse  
	   {
	    
	     get
		{
		   return hasResponse;
		 }
		 set
		 {
		   if(hasResponse != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasResponse",OldValue=hasResponse,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasResponse=value;
		   }
			
		 }
	   }
	  private string fMAAcknowledgementReason ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FMAAcknowledgementReason  
	   {
	    
	     get
		{
		   return fMAAcknowledgementReason;
		 }
		 set
		 {
		   if(fMAAcknowledgementReason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FMAAcknowledgementReason",OldValue=fMAAcknowledgementReason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fMAAcknowledgementReason=value;
		   }
			
		 }
	   }
	  private bool isCancelled ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCancelled  
	   {
	    
	     get
		{
		   return isCancelled;
		 }
		 set
		 {
		   if(isCancelled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCancelled",OldValue=isCancelled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCancelled=value;
		   }
			
		 }
	   }
	  private bool hasErrors ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasErrors  
	   {
	    
	     get
		{
		   return hasErrors;
		 }
		 set
		 {
		   if(hasErrors != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasErrors",OldValue=hasErrors,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasErrors=value;
		   }
			
		 }
	   }
	  private bool waitingForResponse ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool WaitingForResponse  
	   {
	    
	     get
		{
		   return waitingForResponse;
		 }
		 set
		 {
		   if(waitingForResponse != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="WaitingForResponse",OldValue=waitingForResponse,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   waitingForResponse=value;
		   }
			
		 }
	   }
	  private string interlineId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string InterlineId  
	   {
	    
	     get
		{
		   return interlineId;
		 }
		 set
		 {
		   if(interlineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InterlineId",OldValue=interlineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   interlineId=value;
		   }
			
		 }
	   }

	   private List<BookingAnswerPM> bookingAnswers;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("BookingBookingAnswers", "Id","BookingId")]
	   [DataMember]
	   public virtual List<BookingAnswerPM> BookingAnswers  
	   {
	        get
             {
                 if (bookingAnswers == null)
                 {
                     bookingAnswers = new List<BookingAnswerPM>();
                 }
                 return bookingAnswers;
              }
             set { bookingAnswers = value; }
	    }
		   
	   private List<BookingAnswerPM>  deletedBookingAnswers;
	   public virtual List<BookingAnswerPM> DeletedBookingAnswers  
	   {
	        get
             {
                 if ( deletedBookingAnswers == null)
                 {
                      deletedBookingAnswers = new List<BookingAnswerPM>();
                 }
                 return  deletedBookingAnswers;
              }
             set {  deletedBookingAnswers = value; }
	    }
	  	  private string bookingLevelCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingLevelCode  
	   {
	    
	     get
		{
		   return bookingLevelCode;
		 }
		 set
		 {
		   if(bookingLevelCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingLevelCode",OldValue=bookingLevelCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingLevelCode=value;
		   }
			
		 }
	   }
	  private string bookingLevelName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingLevelName  
	   {
	    
	     get
		{
		   return bookingLevelName;
		 }
		 set
		 {
		   if(bookingLevelName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingLevelName",OldValue=bookingLevelName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingLevelName=value;
		   }
			
		 }
	   }
	  private string airlinePrefix ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AirlinePrefix  
	   {
	    
	     get
		{
		   return airlinePrefix;
		 }
		 set
		 {
		   if(airlinePrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AirlinePrefix",OldValue=airlinePrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   airlinePrefix=value;
		   }
			
		 }
	   }
	  private string createdByUserName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserName  
	   {
	    
	     get
		{
		   return createdByUserName;
		 }
		 set
		 {
		   if(createdByUserName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserName",OldValue=createdByUserName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserName=value;
		   }
			
		 }
	   }
	  private string bookingProductId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingProductId  
	   {
	    
	     get
		{
		   return bookingProductId;
		 }
		 set
		 {
		   if(bookingProductId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingProductId",OldValue=bookingProductId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingProductId=value;
		   }
			
		 }
	   }
	  private string bookingProductName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BookingProductName  
	   {
	    
	     get
		{
		   return bookingProductName;
		 }
		 set
		 {
		   if(bookingProductName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BookingProductName",OldValue=bookingProductName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bookingProductName=value;
		   }
			
		 }
	   }
	  private string lastSentByUserId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string LastSentByUserId  
	   {
	    
	     get
		{
		   return lastSentByUserId;
		 }
		 set
		 {
		   if(lastSentByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastSentByUserId",OldValue=lastSentByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   lastSentByUserId=value;
		   }
			
		 }
	   }
	  private string shipmentNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentNumber  
	   {
	    
	     get
		{
		   return shipmentNumber;
		 }
		 set
		 {
		   if(shipmentNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentNumber",OldValue=shipmentNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentNumber=value;
		   }
			
		 }
	   }
	  private string descriptionOfGoodsId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DescriptionOfGoodsId  
	   {
	    
	     get
		{
		   return descriptionOfGoodsId;
		 }
		 set
		 {
		   if(descriptionOfGoodsId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DescriptionOfGoodsId",OldValue=descriptionOfGoodsId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   descriptionOfGoodsId=value;
		   }
			
		 }
	   }
	  private string mainCarriageCarrierCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainCarriageCarrierCode  
	   {
	    
	     get
		{
		   return mainCarriageCarrierCode;
		 }
		 set
		 {
		   if(mainCarriageCarrierCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainCarriageCarrierCode",OldValue=mainCarriageCarrierCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainCarriageCarrierCode=value;
		   }
			
		 }
	   }
	  private string descriptionOfGoodsService ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string DescriptionOfGoodsService  
	   {
	    
	     get
		{
		   return descriptionOfGoodsService;
		 }
		 set
		 {
		   if(descriptionOfGoodsService != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DescriptionOfGoodsService",OldValue=descriptionOfGoodsService,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   descriptionOfGoodsService=value;
		   }
			
		 }
	   }
	  private string concurrencyGUID ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConcurrencyGUID  
	   {
	    
	     get
		{
		   return concurrencyGUID;
		 }
		 set
		 {
		   if(concurrencyGUID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConcurrencyGUID",OldValue=concurrencyGUID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   concurrencyGUID=value;
		   }
			
		 }
	   }
	  private string accountNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AccountNumber  
	   {
	    
	     get
		{
		   return accountNumber;
		 }
		 set
		 {
		   if(accountNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AccountNumber",OldValue=accountNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   accountNumber=value;
		   }
			
		 }
	   }
	  private string aWBSpecialHandlingCodeId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string AWBSpecialHandlingCodeId  
	   {
	    
	     get
		{
		   return aWBSpecialHandlingCodeId;
		 }
		 set
		 {
		   if(aWBSpecialHandlingCodeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AWBSpecialHandlingCodeId",OldValue=aWBSpecialHandlingCodeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   aWBSpecialHandlingCodeId=value;
		   }
			
		 }
	   }
	  private bool isTemperatureSensitive ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsTemperatureSensitive  
	   {
	    
	     get
		{
		   return isTemperatureSensitive;
		 }
		 set
		 {
		   if(isTemperatureSensitive != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsTemperatureSensitive",OldValue=isTemperatureSensitive,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isTemperatureSensitive=value;
		   }
			
		 }
	   }
	  private string shipperAddress1 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperAddress1  
	   {
	    
	     get
		{
		   return shipperAddress1;
		 }
		 set
		 {
		   if(shipperAddress1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperAddress1",OldValue=shipperAddress1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperAddress1=value;
		   }
			
		 }
	   }
	  private string shipperAddress2 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperAddress2  
	   {
	    
	     get
		{
		   return shipperAddress2;
		 }
		 set
		 {
		   if(shipperAddress2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperAddress2",OldValue=shipperAddress2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperAddress2=value;
		   }
			
		 }
	   }
	  private string shipperCity ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperCity  
	   {
	    
	     get
		{
		   return shipperCity;
		 }
		 set
		 {
		   if(shipperCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperCity",OldValue=shipperCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperCity=value;
		   }
			
		 }
	   }
	  private string shipperCountryId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperCountryId  
	   {
	    
	     get
		{
		   return shipperCountryId;
		 }
		 set
		 {
		   if(shipperCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperCountryId",OldValue=shipperCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperCountryId=value;
		   }
			
		 }
	   }
	  private string shipperStateId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperStateId  
	   {
	    
	     get
		{
		   return shipperStateId;
		 }
		 set
		 {
		   if(shipperStateId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperStateId",OldValue=shipperStateId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperStateId=value;
		   }
			
		 }
	   }
	  private string shipperZipCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipperZipCode  
	   {
	    
	     get
		{
		   return shipperZipCode;
		 }
		 set
		 {
		   if(shipperZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipperZipCode",OldValue=shipperZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipperZipCode=value;
		   }
			
		 }
	   }
	  private string consigneeAddress1 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeAddress1  
	   {
	    
	     get
		{
		   return consigneeAddress1;
		 }
		 set
		 {
		   if(consigneeAddress1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeAddress1",OldValue=consigneeAddress1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeAddress1=value;
		   }
			
		 }
	   }
	  private string consigneeAddress2 ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeAddress2  
	   {
	    
	     get
		{
		   return consigneeAddress2;
		 }
		 set
		 {
		   if(consigneeAddress2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeAddress2",OldValue=consigneeAddress2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeAddress2=value;
		   }
			
		 }
	   }
	  private string consigneeCity ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeCity  
	   {
	    
	     get
		{
		   return consigneeCity;
		 }
		 set
		 {
		   if(consigneeCity != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeCity",OldValue=consigneeCity,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeCity=value;
		   }
			
		 }
	   }
	  private string consigneeCountryId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeCountryId  
	   {
	    
	     get
		{
		   return consigneeCountryId;
		 }
		 set
		 {
		   if(consigneeCountryId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeCountryId",OldValue=consigneeCountryId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeCountryId=value;
		   }
			
		 }
	   }
	  private string consigneeStateId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeStateId  
	   {
	    
	     get
		{
		   return consigneeStateId;
		 }
		 set
		 {
		   if(consigneeStateId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeStateId",OldValue=consigneeStateId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeStateId=value;
		   }
			
		 }
	   }
	  private string consigneeZipCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConsigneeZipCode  
	   {
	    
	     get
		{
		   return consigneeZipCode;
		 }
		 set
		 {
		   if(consigneeZipCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConsigneeZipCode",OldValue=consigneeZipCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   consigneeZipCode=value;
		   }
			
		 }
	   }
	  private string mainFromPortCountryCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainFromPortCountryCode  
	   {
	    
	     get
		{
		   return mainFromPortCountryCode;
		 }
		 set
		 {
		   if(mainFromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainFromPortCountryCode",OldValue=mainFromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainFromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string mainFromPortCountryName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainFromPortCountryName  
	   {
	    
	     get
		{
		   return mainFromPortCountryName;
		 }
		 set
		 {
		   if(mainFromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainFromPortCountryName",OldValue=mainFromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainFromPortCountryName=value;
		   }
			
		 }
	   }
	  private string mainFromPortCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainFromPortCode  
	   {
	    
	     get
		{
		   return mainFromPortCode;
		 }
		 set
		 {
		   if(mainFromPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainFromPortCode",OldValue=mainFromPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainFromPortCode=value;
		   }
			
		 }
	   }
	  private string mainFromPortName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainFromPortName  
	   {
	    
	     get
		{
		   return mainFromPortName;
		 }
		 set
		 {
		   if(mainFromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainFromPortName",OldValue=mainFromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainFromPortName=value;
		   }
			
		 }
	   }
	  private string mainToPortCountryCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainToPortCountryCode  
	   {
	    
	     get
		{
		   return mainToPortCountryCode;
		 }
		 set
		 {
		   if(mainToPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainToPortCountryCode",OldValue=mainToPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainToPortCountryCode=value;
		   }
			
		 }
	   }
	  private string mainToPortCountryName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainToPortCountryName  
	   {
	    
	     get
		{
		   return mainToPortCountryName;
		 }
		 set
		 {
		   if(mainToPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainToPortCountryName",OldValue=mainToPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainToPortCountryName=value;
		   }
			
		 }
	   }
	  private string mainToPortCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainToPortCode  
	   {
	    
	     get
		{
		   return mainToPortCode;
		 }
		 set
		 {
		   if(mainToPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainToPortCode",OldValue=mainToPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainToPortCode=value;
		   }
			
		 }
	   }
	  private string mainToPortName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MainToPortName  
	   {
	    
	     get
		{
		   return mainToPortName;
		 }
		 set
		 {
		   if(mainToPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MainToPortName",OldValue=mainToPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mainToPortName=value;
		   }
			
		 }
	   }
	  private DateTime? lastFSRStatusRequestDate ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastFSRStatusRequestDate  
	   {
	    
	     get
		{
		   return lastFSRStatusRequestDate;
		 }
		 set
		 {
		   if(lastFSRStatusRequestDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastFSRStatusRequestDate",OldValue=lastFSRStatusRequestDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastFSRStatusRequestDate=value;
		   }
			
		 }
	   }
	  private bool zeroIsDescOfGoodsFromList ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ZeroIsDescOfGoodsFromList  
	   {
	    
	     get
		{
		   return zeroIsDescOfGoodsFromList;
		 }
		 set
		 {
		   if(zeroIsDescOfGoodsFromList != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ZeroIsDescOfGoodsFromList",OldValue=zeroIsDescOfGoodsFromList,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   zeroIsDescOfGoodsFromList=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineId  
	   {
	    
	     get
		{
		   return tenantZeroAirlineId;
		 }
		 set
		 {
		   if(tenantZeroAirlineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineId",OldValue=tenantZeroAirlineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineId=value;
		   }
			
		 }
	   }
	  private bool tenantZeroIsProductMandatory ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TenantZeroIsProductMandatory  
	   {
	    
	     get
		{
		   return tenantZeroIsProductMandatory;
		 }
		 set
		 {
		   if(tenantZeroIsProductMandatory != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroIsProductMandatory",OldValue=tenantZeroIsProductMandatory,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tenantZeroIsProductMandatory=value;
		   }
			
		 }
	   }
	  private bool tenantZeroIsManagingProduct ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TenantZeroIsManagingProduct  
	   {
	    
	     get
		{
		   return tenantZeroIsManagingProduct;
		 }
		 set
		 {
		   if(tenantZeroIsManagingProduct != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroIsManagingProduct",OldValue=tenantZeroIsManagingProduct,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tenantZeroIsManagingProduct=value;
		   }
			
		 }
	   }
	  private bool tenantZeroAirlineGLSHKFFR ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TenantZeroAirlineGLSHKFFR  
	   {
	    
	     get
		{
		   return tenantZeroAirlineGLSHKFFR;
		 }
		 set
		 {
		   if(tenantZeroAirlineGLSHKFFR != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineGLSHKFFR",OldValue=tenantZeroAirlineGLSHKFFR,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineGLSHKFFR=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlinePIMA ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlinePIMA  
	   {
	    
	     get
		{
		   return tenantZeroAirlinePIMA;
		 }
		 set
		 {
		   if(tenantZeroAirlinePIMA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlinePIMA",OldValue=tenantZeroAirlinePIMA,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlinePIMA=value;
		   }
			
		 }
	   }
	  private bool zeroGLSHKNeedsRegistration ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ZeroGLSHKNeedsRegistration  
	   {
	    
	     get
		{
		   return zeroGLSHKNeedsRegistration;
		 }
		 set
		 {
		   if(zeroGLSHKNeedsRegistration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ZeroGLSHKNeedsRegistration",OldValue=zeroGLSHKNeedsRegistration,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   zeroGLSHKNeedsRegistration=value;
		   }
			
		 }
	   }
	  private bool carrierIsGLSHKRegistered ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CarrierIsGLSHKRegistered  
	   {
	    
	     get
		{
		   return carrierIsGLSHKRegistered;
		 }
		 set
		 {
		   if(carrierIsGLSHKRegistered != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierIsGLSHKRegistered",OldValue=carrierIsGLSHKRegistered,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   carrierIsGLSHKRegistered=value;
		   }
			
		 }
	   }
	  private bool tenantZeroAirlineChampFFR ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TenantZeroAirlineChampFFR  
	   {
	    
	     get
		{
		   return tenantZeroAirlineChampFFR;
		 }
		 set
		 {
		   if(tenantZeroAirlineChampFFR != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineChampFFR",OldValue=tenantZeroAirlineChampFFR,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineChampFFR=value;
		   }
			
		 }
	   }
	  private string tenantZeroAirlineTTY ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantZeroAirlineTTY  
	   {
	    
	     get
		{
		   return tenantZeroAirlineTTY;
		 }
		 set
		 {
		   if(tenantZeroAirlineTTY != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineTTY",OldValue=tenantZeroAirlineTTY,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineTTY=value;
		   }
			
		 }
	   }
	  private bool zeroChampNeedsRegistration ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ZeroChampNeedsRegistration  
	   {
	    
	     get
		{
		   return zeroChampNeedsRegistration;
		 }
		 set
		 {
		   if(zeroChampNeedsRegistration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ZeroChampNeedsRegistration",OldValue=zeroChampNeedsRegistration,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   zeroChampNeedsRegistration=value;
		   }
			
		 }
	   }
	  private bool carrierIsChampRegistered ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CarrierIsChampRegistered  
	   {
	    
	     get
		{
		   return carrierIsChampRegistered;
		 }
		 set
		 {
		   if(carrierIsChampRegistered != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierIsChampRegistered",OldValue=carrierIsChampRegistered,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   carrierIsChampRegistered=value;
		   }
			
		 }
	   }
	  private bool mAWBTakenFromStack ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool MAWBTakenFromStack  
	   {
	    
	     get
		{
		   return mAWBTakenFromStack;
		 }
		 set
		 {
		   if(mAWBTakenFromStack != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBTakenFromStack",OldValue=mAWBTakenFromStack,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   mAWBTakenFromStack=value;
		   }
			
		 }
	   }
	  private string trans1FromPortCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans1FromPortCode  
	   {
	    
	     get
		{
		   return trans1FromPortCode;
		 }
		 set
		 {
		   if(trans1FromPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans1FromPortCode",OldValue=trans1FromPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans1FromPortCode=value;
		   }
			
		 }
	   }
	  private string trans1FromPortName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans1FromPortName  
	   {
	    
	     get
		{
		   return trans1FromPortName;
		 }
		 set
		 {
		   if(trans1FromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans1FromPortName",OldValue=trans1FromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans1FromPortName=value;
		   }
			
		 }
	   }
	  private string trans1FromPortCountryCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans1FromPortCountryCode  
	   {
	    
	     get
		{
		   return trans1FromPortCountryCode;
		 }
		 set
		 {
		   if(trans1FromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans1FromPortCountryCode",OldValue=trans1FromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans1FromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string trans1FromPortCountryName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans1FromPortCountryName  
	   {
	    
	     get
		{
		   return trans1FromPortCountryName;
		 }
		 set
		 {
		   if(trans1FromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans1FromPortCountryName",OldValue=trans1FromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans1FromPortCountryName=value;
		   }
			
		 }
	   }
	  private string trans1ToPortCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans1ToPortCode  
	   {
	    
	     get
		{
		   return trans1ToPortCode;
		 }
		 set
		 {
		   if(trans1ToPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans1ToPortCode",OldValue=trans1ToPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans1ToPortCode=value;
		   }
			
		 }
	   }
	  private string trans1ToPortName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans1ToPortName  
	   {
	    
	     get
		{
		   return trans1ToPortName;
		 }
		 set
		 {
		   if(trans1ToPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans1ToPortName",OldValue=trans1ToPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans1ToPortName=value;
		   }
			
		 }
	   }
	  private string trans1ToPortCountryCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans1ToPortCountryCode  
	   {
	    
	     get
		{
		   return trans1ToPortCountryCode;
		 }
		 set
		 {
		   if(trans1ToPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans1ToPortCountryCode",OldValue=trans1ToPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans1ToPortCountryCode=value;
		   }
			
		 }
	   }
	  private string trans1ToPortCountryName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans1ToPortCountryName  
	   {
	    
	     get
		{
		   return trans1ToPortCountryName;
		 }
		 set
		 {
		   if(trans1ToPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans1ToPortCountryName",OldValue=trans1ToPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans1ToPortCountryName=value;
		   }
			
		 }
	   }
	  private string trans2FromPortCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans2FromPortCode  
	   {
	    
	     get
		{
		   return trans2FromPortCode;
		 }
		 set
		 {
		   if(trans2FromPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans2FromPortCode",OldValue=trans2FromPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans2FromPortCode=value;
		   }
			
		 }
	   }
	  private string trans2FromPortName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans2FromPortName  
	   {
	    
	     get
		{
		   return trans2FromPortName;
		 }
		 set
		 {
		   if(trans2FromPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans2FromPortName",OldValue=trans2FromPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans2FromPortName=value;
		   }
			
		 }
	   }
	  private string trans2FromPortCountryCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans2FromPortCountryCode  
	   {
	    
	     get
		{
		   return trans2FromPortCountryCode;
		 }
		 set
		 {
		   if(trans2FromPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans2FromPortCountryCode",OldValue=trans2FromPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans2FromPortCountryCode=value;
		   }
			
		 }
	   }
	  private string trans2FromPortCountryName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans2FromPortCountryName  
	   {
	    
	     get
		{
		   return trans2FromPortCountryName;
		 }
		 set
		 {
		   if(trans2FromPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans2FromPortCountryName",OldValue=trans2FromPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans2FromPortCountryName=value;
		   }
			
		 }
	   }
	  private string trans2ToPortCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans2ToPortCode  
	   {
	    
	     get
		{
		   return trans2ToPortCode;
		 }
		 set
		 {
		   if(trans2ToPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans2ToPortCode",OldValue=trans2ToPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans2ToPortCode=value;
		   }
			
		 }
	   }
	  private string trans2ToPortName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans2ToPortName  
	   {
	    
	     get
		{
		   return trans2ToPortName;
		 }
		 set
		 {
		   if(trans2ToPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans2ToPortName",OldValue=trans2ToPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans2ToPortName=value;
		   }
			
		 }
	   }
	  private string trans2ToPortCountryCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans2ToPortCountryCode  
	   {
	    
	     get
		{
		   return trans2ToPortCountryCode;
		 }
		 set
		 {
		   if(trans2ToPortCountryCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans2ToPortCountryCode",OldValue=trans2ToPortCountryCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans2ToPortCountryCode=value;
		   }
			
		 }
	   }
	  private string trans2ToPortCountryName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Trans2ToPortCountryName  
	   {
	    
	     get
		{
		   return trans2ToPortCountryName;
		 }
		 set
		 {
		   if(trans2ToPortCountryName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Trans2ToPortCountryName",OldValue=trans2ToPortCountryName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   trans2ToPortCountryName=value;
		   }
			
		 }
	   }
	  private bool carrierIsCheckDigit ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CarrierIsCheckDigit  
	   {
	    
	     get
		{
		   return carrierIsCheckDigit;
		 }
		 set
		 {
		   if(carrierIsCheckDigit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierIsCheckDigit",OldValue=carrierIsCheckDigit,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   carrierIsCheckDigit=value;
		   }
			
		 }
	   }
	  private bool carrierIsLimitedLength ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CarrierIsLimitedLength  
	   {
	    
	     get
		{
		   return carrierIsLimitedLength;
		 }
		 set
		 {
		   if(carrierIsLimitedLength != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CarrierIsLimitedLength",OldValue=carrierIsLimitedLength,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   carrierIsLimitedLength=value;
		   }
			
		 }
	   }
	  private string transshipment1CarrierName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment1CarrierName  
	   {
	    
	     get
		{
		   return transshipment1CarrierName;
		 }
		 set
		 {
		   if(transshipment1CarrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment1CarrierName",OldValue=transshipment1CarrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment1CarrierName=value;
		   }
			
		 }
	   }
	  private string transshipment2CarrierName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string Transshipment2CarrierName  
	   {
	    
	     get
		{
		   return transshipment2CarrierName;
		 }
		 set
		 {
		   if(transshipment2CarrierName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Transshipment2CarrierName",OldValue=transshipment2CarrierName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   transshipment2CarrierName=value;
		   }
			
		 }
	   }
	  private string mAWBStackAirlineId ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWBStackAirlineId  
	   {
	    
	     get
		{
		   return mAWBStackAirlineId;
		 }
		 set
		 {
		   if(mAWBStackAirlineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBStackAirlineId",OldValue=mAWBStackAirlineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWBStackAirlineId=value;
		   }
			
		 }
	   }
	  private string mAWBStackNumber ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string MAWBStackNumber  
	   {
	    
	     get
		{
		   return mAWBStackNumber;
		 }
		 set
		 {
		   if(mAWBStackNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBStackNumber",OldValue=mAWBStackNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mAWBStackNumber=value;
		   }
			
		 }
	   }
	  private bool mAWBReturnedToStack ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool MAWBReturnedToStack  
	   {
	    
	     get
		{
		   return mAWBReturnedToStack;
		 }
		 set
		 {
		   if(mAWBReturnedToStack != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBReturnedToStack",OldValue=mAWBReturnedToStack,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   mAWBReturnedToStack=value;
		   }
			
		 }
	   }
	  private bool mAWBReturnedToStackWithCancel ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool MAWBReturnedToStackWithCancel  
	   {
	    
	     get
		{
		   return mAWBReturnedToStackWithCancel;
		 }
		 set
		 {
		   if(mAWBReturnedToStackWithCancel != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MAWBReturnedToStackWithCancel",OldValue=mAWBReturnedToStackWithCancel,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   mAWBReturnedToStackWithCancel=value;
		   }
			
		 }
	   }
	  private bool isCopyMode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCopyMode  
	   {
	    
	     get
		{
		   return isCopyMode;
		 }
		 set
		 {
		   if(isCopyMode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCopyMode",OldValue=isCopyMode,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCopyMode=value;
		   }
			
		 }
	   }
	  private string finalDestinationPortCode ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalDestinationPortCode  
	   {
	    
	     get
		{
		   return finalDestinationPortCode;
		 }
		 set
		 {
		   if(finalDestinationPortCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalDestinationPortCode",OldValue=finalDestinationPortCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalDestinationPortCode=value;
		   }
			
		 }
	   }
	  private bool tenantZeroAirlineChampFVR ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TenantZeroAirlineChampFVR  
	   {
	    
	     get
		{
		   return tenantZeroAirlineChampFVR;
		 }
		 set
		 {
		   if(tenantZeroAirlineChampFVR != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineChampFVR",OldValue=tenantZeroAirlineChampFVR,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineChampFVR=value;
		   }
			
		 }
	   }
	  private bool tenantZeroAirlineGLSHKFVR ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TenantZeroAirlineGLSHKFVR  
	   {
	    
	     get
		{
		   return tenantZeroAirlineGLSHKFVR;
		 }
		 set
		 {
		   if(tenantZeroAirlineGLSHKFVR != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineGLSHKFVR",OldValue=tenantZeroAirlineGLSHKFVR,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineGLSHKFVR=value;
		   }
			
		 }
	   }
	  private string finalDestinationPortName ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string FinalDestinationPortName  
	   {
	    
	     get
		{
		   return finalDestinationPortName;
		 }
		 set
		 {
		   if(finalDestinationPortName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalDestinationPortName",OldValue=finalDestinationPortName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   finalDestinationPortName=value;
		   }
			
		 }
	   }
	  private bool tenantZeroAirlineGLSHKFSRFSA ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TenantZeroAirlineGLSHKFSRFSA  
	   {
	    
	     get
		{
		   return tenantZeroAirlineGLSHKFSRFSA;
		 }
		 set
		 {
		   if(tenantZeroAirlineGLSHKFSRFSA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineGLSHKFSRFSA",OldValue=tenantZeroAirlineGLSHKFSRFSA,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineGLSHKFSRFSA=value;
		   }
			
		 }
	   }
	  private bool tenantZeroAirlineChampFSRFSA ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TenantZeroAirlineChampFSRFSA  
	   {
	    
	     get
		{
		   return tenantZeroAirlineChampFSRFSA;
		 }
		 set
		 {
		   if(tenantZeroAirlineChampFSRFSA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantZeroAirlineChampFSRFSA",OldValue=tenantZeroAirlineChampFSRFSA,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   tenantZeroAirlineChampFSRFSA=value;
		   }
			
		 }
	   }
	  private string updatedByPartner ;
	  	  
       
	   [CustomValidation(typeof(BookingValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByPartner  
	   {
	    
	     get
		{
		   return updatedByPartner;
		 }
		 set
		 {
		   if(updatedByPartner != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByPartner",OldValue=updatedByPartner,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByPartner=value;
		   }
			
		 }
	   }
   }
   
}
	 