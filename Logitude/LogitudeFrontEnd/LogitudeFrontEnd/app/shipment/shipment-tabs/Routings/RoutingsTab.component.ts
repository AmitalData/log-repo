import {Component, OnInit}  from 'angular2/core';
import {RouteParams, Router} from 'angular2/router';
import {ShipmentsService} from '../../services/shipment-service/shipments.service';
import {TextCodeTranslator} from '../../../infrastructure/utilities/TextCodeTranslator'
import {TextcodeTranslationPipe} from '../../../infrastructure/pipes/textcode-translation/textcode-translation.pipe';
import {IconButton} from '../../../ApplicationControls/IconButton'
import {CountryFlagPipe} from '../../../infrastructure/pipes/CountryFlagPipe';
import {DateTimeToDatePipe} from '../../../infrastructure/pipes/DateTimeToDatePipe';
import {DateTimeToTimePipe} from '../../../infrastructure/pipes/DateTimeToTimePipe';
import {NgStyle} from 'angular2/common';

//class ShipmentPickUpPM {
//    public Id: string;
//}

//class ShipmentDeliveryPM {
//    public Id: string;
//}

class RoutingItem {
    public EntityPM: any;
    public Pickup: any;
    public Delivery: any;
    public LegType: string;
    
    constructor(entity: any, type: string, myPickup: any, myDelivery: any) {

        this.EntityPM = entity;
        this.Pickup = myPickup;
        this.Delivery = myDelivery;
        this.LegType = type;

        this.SetLegAppearance();
        this.GetLegnameTranslated();
        this.GetImageSource();
        this.GetFromCountryData();
        this.GetToCountryData();
        this.GetCarrierData();
        this.GetDates();
    }

    public LegHeight: number;
    public IsLegExists: boolean;
    public NoLegTextCode: string;
    public IsAddButtonVisible: boolean;
    public IsEditButtonVisible: boolean;
    public IsDeleteButtonVisible: boolean;
    SetLegAppearance() {
        
        if (this.LegType == "Pick Up" && this.Pickup == null) {
            this.LegHeight = 50;
            this.IsLegExists = false;
            this.NoLegTextCode = "Shipment.O.Routings.NoPickup";
        }

        else if (this.LegType == "Delivery" && this.Delivery == null) {
            this.LegHeight = 50;
            this.IsLegExists = false;
            this.NoLegTextCode = "Shipment.O.Routings.NoDelivery";
        }

        else {
            this.LegHeight = 100;
            this.IsLegExists = true;
        }

        if (this.IsLegExists) {
            this.IsAddButtonVisible = false;
            this.IsEditButtonVisible = true;

            switch (this.LegType) {
                case "Main Carriage":
                case "Transshipment1":
                case "Transshipment2":
                case "Transshipment3":
                    {

                        this.IsDeleteButtonVisible = false;
                        break;
                    }

                default: {

                    this.IsDeleteButtonVisible = true;
                    break;
                }
            }
        }

        else {
            this.IsAddButtonVisible = true;
            this.IsEditButtonVisible = false;
            this.IsDeleteButtonVisible = false;
        }
    }

    public LegNameTranslated: string;
    public LegTransportModeId: string;
    GetLegnameTranslated() {

        var myTextCode: string;
        var myLegTransportModeId: string;

        switch (this.LegType) {

            case "Pick Up": {
                myTextCode = "Shipment.O.Routings.Pickup";
                myLegTransportModeId = "I";
                break;
            }

            case "Pre Carriage": {
                myTextCode = "Shipment.O.Routings.PreCarriage";   
                myLegTransportModeId = this.EntityPM.PreCarriageTransportModeId;            
                break;
            }

            case "Main Carriage": {
                myTextCode = "Shipment.O.Routings.MainCarriageLeg1";   
                myLegTransportModeId = this.EntityPM.TransportModeId;              
                break;
            }

            case "Transshipment1": {
                myTextCode = (this.EntityPM.TransportModeId.toUpperCase() == "A") ? "Shipment.O.Routings.MainCarriageLeg2" : "Shipment.O.Routings.Transshipment1";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }

            case "Transshipment2": {
                myTextCode = (this.EntityPM.TransportModeId.toUpperCase() == "A") ? "Shipment.O.Routings.MainCarriageLeg3" : "Shipment.O.Routings.Transshipment2";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }

            case "Transshipment3": {
                myTextCode = (this.EntityPM.TransportModeId.toUpperCase() == "A") ? "Shipment.O.Routings.MainCarriageLeg4" : "Shipment.O.Routings.Transshipment3";
                myLegTransportModeId = this.EntityPM.TransportModeId;
                break;
            }

            case "On Carriage": {
                myTextCode = "Shipment.O.Routings.OnCarriage";
                myLegTransportModeId = this.EntityPM.OnCarriageTransportModeId;  
                break;
            }

            case "Delivery": {
                myTextCode = "Shipment.O.Routings.Delivery";
                myLegTransportModeId = "I";
                break;
            }
        }

        this.LegNameTranslated = TextCodeTranslator.transform(myTextCode);
        this.LegTransportModeId = myLegTransportModeId;
    }

    public ImageSource: string;
    GetImageSource() {

        switch (this.LegTransportModeId) {
            case "A": {
                this.ImageSource = 'images/Airline.png';
                break;
            }

            case "O": {
                this.ImageSource = 'images/Vessel.png';
                break;
            }

            case "I": {
                this.ImageSource = 'images/Trucker.png';
                break;
            }
        }
    }

    public FromCountryCode: string;
    public FromCountryName: string;
    public FromCountryText: string;
    GetFromCountryData() {

        var myCountryCode: string = "";
        var myCountryName: string = "";

        var myTextPart1: string = "";
        var myTextPart2: string = "";
        var myTextParts: string = "";

        switch (this.LegType) {

            case "Pick Up": {
                if (this.Pickup != null) {

                    if (this.Pickup.FromPortId != null) {
                        myCountryCode = this.Pickup.FromPortCountryCode;
                        myCountryName = this.Pickup.FromPortCountryName;
                    }

                    else {
                        myCountryCode = this.Pickup.FromAddressCountryCode;
                        myCountryName = this.Pickup.FromAddressCountryName;
                    }

                    switch (this.Pickup.PickUpFromTypeCode) {

                        case "PORT": {
                            myTextPart1 = this.Pickup.FromPortCode;
                            myTextPart2 = this.Pickup.FromPortName;
                            break;
                        }

                        case "PART": {

                            if (this.Pickup.FromAddressId != null) {
                                myTextPart1 = this.Pickup.FromAddressCity_Dummy;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Pickup.FromAddressCountryCode;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }

                            break;
                        }

                        default: {

                            if (this.Pickup.FromAddressCity != null) {
                                myTextPart1 = this.Pickup.FromAddressCity_Dummy;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Pickup.FromAddressCountryCode;
                                myTextPart2 = this.Pickup.FromAddressCountryName;
                            }

                            break;
                        }
                    }
                }

                break;
            }

            case "Pre Carriage": {
                myCountryCode = this.EntityPM.PreCarriageFromPortCountryCode;
                myCountryName = this.EntityPM.PreCarriageFromPortCountryName;
                myTextPart1 = this.EntityPM.PreCarriageFromPortCode;
                myTextPart2 = this.EntityPM.PreCarriageFromPortName;
                break;
            }

            case "Main Carriage": {
                myCountryCode = this.EntityPM.MainCarriageFromPortCountryCode;
                myCountryName = this.EntityPM.MainCarriageFromPortCountryName;
                myTextPart1 = this.EntityPM.MainCarriageFromPortCode;
                myTextPart2 = this.EntityPM.MainCarriageFromPortName;
                break;
            }

            case "Transshipment1": {
                myCountryCode = this.EntityPM.Transshipment1FromPortCountryCode;
                myCountryName = this.EntityPM.Transshipment1FromPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment1FromPortCode;
                myTextPart2 = this.EntityPM.Transshipment1FromPortName;
                break;
            }

            case "Transshipment2": {
                myCountryCode = this.EntityPM.Transshipment2FromPortCountryCode;
                myCountryName = this.EntityPM.Transshipment2FromPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment2FromPortCode;
                myTextPart2 = this.EntityPM.Transshipment2FromPortName;
                break;
            }

            case "Transshipment3": {
                myCountryCode = this.EntityPM.Transshipment3FromPortCountryCode;
                myCountryName = this.EntityPM.Transshipment3FromPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment3FromPortCode;
                myTextPart2 = this.EntityPM.Transshipment3FromPortName;
                break;
            }

            case "On Carriage": {
                myCountryCode = this.EntityPM.OnCarriageFromPortCountryCode;
                myCountryName = this.EntityPM.OnCarriageFromPortCountryName;
                myTextPart1 = this.EntityPM.OnCarriageFromPortCode;
                myTextPart2 = this.EntityPM.OnCarriageFromPortName;
                break;
            }

            case "Delivery": {
                if (this.Delivery != null) {

                    if (this.Delivery.FromPortId != null) {
                        myCountryCode = this.Delivery.FromPortCountryCode;
                        myCountryName = this.Delivery.FromPortCountryName;
                    }

                    else {
                        myCountryCode = this.Delivery.FromAddressCountryCode;
                        myCountryName = this.Delivery.FromAddressCountryName;
                    }

                    switch (this.Delivery.PickUpFromTypeCode) {

                        case "PORT": {
                            myTextPart1 = this.Delivery.FromPortCode;
                            myTextPart2 = this.Delivery.FromPortName;
                            break;
                        }

                        case "PART": {

                            if (this.Delivery.FromAddressId != null) {
                                myTextPart1 = this.Delivery.FromAddressCity_Dummy;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Delivery.FromAddressCountryCode;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }

                            break;
                        }

                        default: {

                            if (this.Delivery.FromAddressCity != null) {
                                myTextPart1 = this.Delivery.FromAddressCity_Dummy;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Delivery.FromAddressCountryCode;
                                myTextPart2 = this.Delivery.FromAddressCountryName;
                            }

                            break;
                        }
                    }
                }

                break;
            }
        }

        if (myTextPart1 != null && myTextPart2 != null) {
            myTextParts = myTextPart1 + " " + myTextPart2;
        }

        this.FromCountryCode = myCountryCode == null ? "" : myCountryCode;
        this.FromCountryName = myCountryName == null ? "" : myCountryName;
        this.FromCountryText = myTextParts;
    }
        
    public ToCountryCode: string;
    public ToCountryName: string;
    public ToCountryText: string;
    GetToCountryData() {

        var myCountryCode: string = "";
        var myCountryName: string = "";

        var myTextPart1: string = "";
        var myTextPart2: string = "";
        var myTextParts: string = "";

        switch (this.LegType) {

            case "Pick Up": {
                if (this.Pickup != null) {

                    if (this.Pickup.ToPortId != null) {
                        myCountryCode = this.Pickup.ToPortCountryCode;
                        myCountryName = this.Pickup.ToPortCountryName;
                    }

                    else {
                        myCountryCode = this.Pickup.ToAddressCountryCode;
                        myCountryName = this.Pickup.ToAddressCountryName;
                    }

                    switch (this.Pickup.PickUpToTypeCode) {

                        case "PORT": {
                            myTextPart1 = this.Pickup.ToPortCode;
                            myTextPart2 = this.Pickup.ToPortName;
                            break;
                        }

                        case "PART": {

                            if (this.Pickup.ToAddressId != null) {
                                myTextPart1 = this.Pickup.ToAddressCity_Dummy;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Pickup.ToAddressCountryCode;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }

                            break;
                        }

                        default: {

                            if (this.Pickup.ToAddressCity != null) {
                                myTextPart1 = this.Pickup.ToAddressCity_Dummy;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Pickup.ToAddressCountryCode;
                                myTextPart2 = this.Pickup.ToAddressCountryName;
                            }

                            break;
                        }
                    }
                }

                break;
            }

            case "Pre Carriage": {
                myCountryCode = this.EntityPM.PreCarriageToPortCountryCode;
                myCountryName = this.EntityPM.PreCarriageToPortCountryName;
                myTextPart1 = this.EntityPM.PreCarriageToPortCode;
                myTextPart2 = this.EntityPM.PreCarriageToPortName;
                break;
            }

            case "Main Carriage": {
                myCountryCode = this.EntityPM.MainCarriageToPortCountryCode;
                myCountryName = this.EntityPM.MainCarriageToPortCountryName;
                myTextPart1 = this.EntityPM.MainCarriageToPortCode;
                myTextPart2 = this.EntityPM.MainCarriageToPortName;
                break;
            }

            case "Transshipment1": {
                myCountryCode = this.EntityPM.Transshipment1ToPortCountryCode;
                myCountryName = this.EntityPM.Transshipment1ToPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment1ToPortCode;
                myTextPart2 = this.EntityPM.Transshipment1ToPortName;
                break;
            }

            case "Transshipment2": {
                myCountryCode = this.EntityPM.Transshipment2ToPortCountryCode;
                myCountryName = this.EntityPM.Transshipment2ToPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment2ToPortCode;
                myTextPart2 = this.EntityPM.Transshipment2ToPortName;
                break;
            }

            case "Transshipment3": {
                myCountryCode = this.EntityPM.Transshipment3ToPortCountryCode;
                myCountryName = this.EntityPM.Transshipment3ToPortCountryName;
                myTextPart1 = this.EntityPM.Transshipment3ToPortCode;
                myTextPart2 = this.EntityPM.Transshipment3ToPortName;
                break;
            }

            case "On Carriage": {
                myCountryCode = this.EntityPM.OnCarriageToPortCountryCode;
                myCountryName = this.EntityPM.OnCarriageToPortCountryName;
                myTextPart1 = this.EntityPM.OnCarriageToPortCode;
                myTextPart2 = this.EntityPM.OnCarriageToPortName;
                break;
            }

            case "Delivery": {
                if (this.Delivery != null) {

                    if (this.Delivery.ToPortId != null) {
                        myCountryCode = this.Delivery.ToPortCountryCode;
                        myCountryName = this.Delivery.ToPortCountryName;
                    }

                    else {
                        myCountryCode = this.Delivery.ToAddressCountryCode;
                        myCountryName = this.Delivery.ToAddressCountryName;
                    }

                    switch (this.Delivery.PickUpToTypeCode) {

                        case "PORT": {
                            myTextPart1 = this.Delivery.ToPortCode;
                            myTextPart2 = this.Delivery.ToPortName;
                            break;
                        }

                        case "PART": {

                            if (this.Delivery.ToAddressId != null) {
                                myTextPart1 = this.Delivery.ToAddressCity_Dummy;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Delivery.ToAddressCountryCode;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }

                            break;
                        }

                        default: {

                            if (this.Delivery.ToAddressCity != null) {
                                myTextPart1 = this.Delivery.ToAddressCity_Dummy;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }

                            else {
                                myTextPart1 = this.Delivery.ToAddressCountryCode;
                                myTextPart2 = this.Delivery.ToAddressCountryName;
                            }

                            break;
                        }
                    }
                }

                break;
            }
        }

        if (myTextPart1 != null && myTextPart2 != null) {
            myTextParts = myTextPart1 + " " + myTextPart2;
        }

        this.ToCountryCode = myCountryCode == null ? "" : myCountryCode;
        this.ToCountryName = myCountryName == null ? "" : myCountryName;
        this.ToCountryText = myTextParts;
    }

    public CarrierText: string;
    public CarrierSite: string;
    public IsCarrierTextVisible: boolean = false;
    public IsCarrierSiteVisible: boolean = false;
    public CarrierNumber: string;
    public VesselName: string;
    public IsVesselVisible: boolean = false;
    public IsBookingConfirmationVisible: boolean = false;
    GetCarrierData() {

        var myCarrierCode: string = "";
        var myCarrierName: string = "";
        var myCarrierSite: string = "";
        var myCarrierNumber: string = "";
        var isVesselVisible: boolean = false;
        var isBookingnVisible: boolean = false;

        switch (this.LegType) {

            case "Pick Up": {
                if (this.Pickup != null) {
                    myCarrierCode = this.Pickup.CarrierCode;
                    myCarrierName = this.Pickup.CarrierName;
                    myCarrierSite = this.Pickup.CarrierWebSite;
                    myCarrierNumber = this.Pickup.CarrierNumber;
                }
                break;
            }

            case "Pre Carriage": {
                myCarrierCode = this.EntityPM.PreCarriageCarrierCode;
                myCarrierName = this.EntityPM.PreCarriageCarrierName;
                myCarrierSite = this.EntityPM.PreCarriageCarrierWebSite;
                myCarrierNumber = this.EntityPM.PreCarriageCarrierNumber;

                if (this.EntityPM.PreCarriageTransportModeId == "O") {
                    isVesselVisible = true;
                }

                break;
            }

            case "Main Carriage": {
                myCarrierCode = this.EntityPM.MainCarriageCarrierCode;
                myCarrierName = this.EntityPM.MainCarriageCarrierName;
                myCarrierSite = this.EntityPM.MainCarriageCarrierWebSite;
                myCarrierNumber = this.EntityPM.MainCarriageCarrierNumber;

                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }

                if (this.EntityPM.BookingConfirmationNumber != null) {
                    isBookingnVisible = true;
                }

                break;
            }

            case "Transshipment1": {
                myCarrierCode = this.EntityPM.Transshipment1CarrierCode;
                myCarrierName = this.EntityPM.Transshipment1CarrierName;
                myCarrierSite = this.EntityPM.Transshipment1CarrierWebSite;
                myCarrierNumber = this.EntityPM.Transshipment1CarrierNumber;

                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }

                break;
            }

            case "Transshipment2": {
                myCarrierCode = this.EntityPM.Transshipment2CarrierCode;
                myCarrierName = this.EntityPM.Transshipment2CarrierName;
                myCarrierSite = this.EntityPM.Transshipment2CarrierWebSite;
                myCarrierNumber = this.EntityPM.Transshipment2CarrierNumber;

                if (this.EntityPM.TransportModeId == "O") {
                    isVesselVisible = true;
                }

                break;
            }

            case "Transshipment3": {
                myCarrierCode = this.EntityPM.Transshipment3CarrierCode;
                myCarrierName = this.EntityPM.Transshipment3CarrierName;
                myCarrierSite = this.EntityPM.Transshipment3CarrierWebSite;
                myCarrierNumber = this.EntityPM.Transshipment3CarrierNumber;
                break;
            }

            case "On Carriage": {
                myCarrierCode = this.EntityPM.OnCarriageCarrierCode;
                myCarrierName = this.EntityPM.OnCarriageCarrierName;
                myCarrierSite = this.EntityPM.OnCarriageCarrierWebSite;
                myCarrierNumber = this.EntityPM.OnCarriageCarrierNumber;

                if (this.EntityPM.OnCarriageTransportModeId == "O") {
                    isVesselVisible = true;
                }

                break;
            }

            case "Delivery": {
                if (this.Delivery != null) {
                    myCarrierCode = this.Delivery.CarrierCode;
                    myCarrierName = this.Delivery.CarrierName;
                    myCarrierSite = this.Delivery.CarrierWebSite;
                    myCarrierNumber = this.Delivery.CarrierNumber;
                }
                break;
            }
        }


        if (myCarrierSite != null) {
            if (myCarrierSite.indexOf('http://') == -1) {
                myCarrierSite = "http://" + myCarrierSite;
            }
        }

        this.CarrierSite = myCarrierSite;
        this.CarrierText = (myCarrierCode == null || myCarrierName == null) ? "" : myCarrierCode + " " + myCarrierName;
        this.IsCarrierSiteVisible = myCarrierSite != null ? true : false;
        this.IsCarrierTextVisible = !this.IsCarrierSiteVisible;
        this.VesselName = "";
        this.IsVesselVisible = isVesselVisible;
        this.CarrierNumber = myCarrierNumber == null ? "" : myCarrierNumber;
        this.IsBookingConfirmationVisible = isBookingnVisible;
    }

    public DepartureDate: Date;
    public DepartureColor: string;
    public ArrivalDate: Date;
    public ArrivalColor: string;
    public IsSetActualDepartureVisible: boolean = false;
    public IsSetActualArrivalVisible: boolean = false;
    GetDates() {

        var myETD: Date;
        var myATD: Date;
        var myETA: Date;
        var myATA: Date;      
        var myDepartureDate: Date; 
        var myArrivalDate: Date;   
        var myDepartureColor: string = "#F37021";
        var myArrivalColor: string = "#F37021";

        switch (this.LegType) {

            case "Pick Up": {

                if (this.Pickup != null) {
                    myETD = this.Pickup.ETD;
                    myATD = this.Pickup.ATD;
                    myETA = this.Pickup.ETA;
                    myATA = this.Pickup.ATA;
                }

                break;
            }

            case "Pre Carriage": {
                myETD = this.EntityPM.PreCarriageETD;
                myATD = this.EntityPM.PreCarriageATD;
                myETA = this.EntityPM.PreCarriageETA;
                myATA = this.EntityPM.PreCarriageATA;
                break;
            }

            case "Main Carriage": {
                myETD = this.EntityPM.MainCarriageETD;
                myATD = this.EntityPM.MainCarriageATD;
                myETA = this.EntityPM.MainCarriageETA;
                myATA = this.EntityPM.MainCarriageATA;
                break;
            }

            case "Transshipment1": {
                myETD = this.EntityPM.Transshipment1ETD;
                myATD = this.EntityPM.Transshipment1ATD;
                myETA = this.EntityPM.Transshipment1ETA;
                myATA = this.EntityPM.Transshipment1ATA;
                break;
            }

            case "Transshipment2": {
                myETD = this.EntityPM.Transshipment2ETD;
                myATD = this.EntityPM.Transshipment2ATD;
                myETA = this.EntityPM.Transshipment2ETA;
                myATA = this.EntityPM.Transshipment2ATA;
                break;
            }

            case "Transshipment3": {
                myETD = this.EntityPM.Transshipment3ETD;
                myATD = this.EntityPM.Transshipment3ATD;
                myETA = this.EntityPM.Transshipment3ETA;
                myATA = this.EntityPM.Transshipment3ATA;
                break;
            }

            case "On Carriage": {
                myETD = this.EntityPM.OnCarriageATD;
                myATD = this.EntityPM.OnCarriageATD;
                myETA = this.EntityPM.OnCarriageETA;
                myATA = this.EntityPM.OnCarriageATA;
                break;
            }

            case "Delivery": {
                if (this.Delivery != null) {
                    myETD = this.Delivery.ETD;
                    myATD = this.Delivery.ATD;
                    myETA = this.Delivery.ETA;
                    myATA = this.Delivery.ATA;
                }
                break;
            }
        }               

        var todayDate: Date = new Date();

        if (myATD != null) {
            myDepartureDate = myATD;
            myDepartureColor = "#45494A";
        }

        else if (myETD != null) {
            myDepartureDate = myETD;
            
            if (myDepartureDate < todayDate) {
                myDepartureColor = "blue";
            }

            this.IsSetActualDepartureVisible = true;
        }

        if (myATA != null) {
            myArrivalDate = myATA;
            myArrivalColor = "#45494A";
        }

        else if (myETA != null) {
            myArrivalDate = myETA;

            if (myArrivalDate < todayDate) {
                myArrivalColor = "blue";
            }

            this.IsSetActualArrivalVisible = true;
        }        

        this.DepartureDate = myDepartureDate;
        this.DepartureColor = myDepartureColor;
        this.ArrivalDate = myArrivalDate;
        this.ArrivalColor = myArrivalColor;
    }

    GetSwitchCase() {
        switch (this.LegType) {

            case "Pick Up": {

                break;
            }

            case "Pre Carriage": {

                break;
            }

            case "Main Carriage": {

                break;
            }

            case "Transshipment1": {

                break;
            }

            case "Transshipment2": {

                break;
            }

            case "Transshipment3": {

                break;
            }

            case "On Carriage": {

                break;
            }

            case "Delivery": {

                break;
            }
        }
    }

}

@Component({
    templateUrl: 'Views/Shipment/Tabs/RoutingsTab.html',
    pipes: [TextcodeTranslationPipe, CountryFlagPipe, DateTimeToDatePipe, DateTimeToTimePipe],
    directives: [IconButton, NgStyle],
})

export class RoutingsComponent implements OnInit {

    private _entityId: string;
    public EntityPM: any;
    public IsDataLoaded: boolean = false;
    public IsInlandDomestic: boolean = false;
    public ItemsCollection: RoutingItem[];

    constructor(private _router: Router, routeParams: RouteParams, private _shipmentsService: ShipmentsService) {
        this._entityId = routeParams.get('id');
    }

    BuildItemsCollection() {

        if (this.ItemsCollection == null) {
            this.ItemsCollection = new Array<RoutingItem>();
        }

        else {
            this.ItemsCollection = [];

            //A.length = 0
            //A.splice(0,A.length)
        }

        if (this.EntityPM.ShipmentPickUps.length == 0) {
            //var newPickup: ShipmentPickUpPM = new ShipmentPickUpPM();
            this.ItemsCollection.push(new RoutingItem(this.EntityPM, "Pick Up", null, null));
        }

        else {
            this.EntityPM.ShipmentPickUps.forEach((item) => {
                this.ItemsCollection.push(new RoutingItem(this.EntityPM, "Pick Up", item, null));
            }) 
        }

        if (this.EntityPM.PreCarriageFromPortId != null && this.EntityPM.PreCarriageToPortId != null) {
            this.ItemsCollection.push(new RoutingItem(this.EntityPM, "Pre Carriage", null, null));
        }

        this.ItemsCollection.push(new RoutingItem(this.EntityPM, "Main Carriage", null, null));

        if (this.EntityPM.Transshipment1FromPortId != null && this.EntityPM.Transshipment1ToPortId != null) {
            this.ItemsCollection.push(new RoutingItem(this.EntityPM, "Transshipment1", null, null));
        }

        if (this.EntityPM.Transshipment2FromPortId != null && this.EntityPM.Transshipment2ToPortId != null) {
            this.ItemsCollection.push(new RoutingItem(this.EntityPM, "Transshipment2", null, null));
        }

        if (this.EntityPM.Transshipment3FromPortId != null && this.EntityPM.Transshipment3ToPortId != null) {
            this.ItemsCollection.push(new RoutingItem(this.EntityPM, "Transshipment3", null, null));
        }

        if (this.EntityPM.OnCarriageFromPortId != null && this.EntityPM.OnCarriageToPortId != null) {
            this.ItemsCollection.push(new RoutingItem(this.EntityPM, "On Carriage", null, null));
        }

        if (this.EntityPM.ShipmentDeliveries.length == 0) {
            //var newDelivery: ShipmentDeliveryPM = new ShipmentDeliveryPM();
            this.ItemsCollection.push(new RoutingItem(this.EntityPM, "Delivery", null, null));
        }

        else {
            this.EntityPM.ShipmentDeliveries.forEach((item) => {
                this.ItemsCollection.push(new RoutingItem(this.EntityPM, "Delivery", null, item));
            })  
        }

        //if (shipmentPM.ShipmentPickUps.Count() == 0) {
        //    ShipmentPickUpPM pickup = new ShipmentPickUpPM();
        //    ObsList.Add(new RoutingsViewModelData(shipmentPM, this, "Pick Up", pickup, null));
        //}

        //else if (shipmentPM.ShipmentPickUps.Count() > 0) {
        //    foreach(ShipmentPickUpPM pickup in shipmentPM.ShipmentPickUps.OrderBy(o => o.PickUpDeliveryNumber))
        //    {
        //        ObsList.Add(new RoutingsViewModelData(shipmentPM, this, "Pick Up:" + pickup.PickUpDeliveryNumber, pickup, null));
        //    }
        //}

        //if (shipmentPM.PreCarriageFromPortId != null && shipmentPM.PreCarriageToPortId != null) {
        //    ObsList.Add(new RoutingsViewModelData(shipmentPM, this, "Pre Carriage"));
        //}

        //ObsList.Add(new RoutingsViewModelData(shipmentPM, this, "Main Carriage"));

        //if (shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null) {
        //    ObsList.Add(new RoutingsViewModelData(shipmentPM, this, "Transshipment1"));
        //}

        //if (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null) {
        //    ObsList.Add(new RoutingsViewModelData(shipmentPM, this, "Transshipment2"));
        //}

        //if (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null) {
        //    ObsList.Add(new RoutingsViewModelData(shipmentPM, this, "Transshipment3"));
        //}

        //if (shipmentPM.OnCarriageFromPortId != null && shipmentPM.OnCarriageToPortId != null) {
        //    ObsList.Add(new RoutingsViewModelData(shipmentPM, this, "On Carriage"));
        //}


        //if (shipmentPM.ShipmentDeliveries.Count() == 0) {
        //    ShipmentDeliveryPM delivery = new ShipmentDeliveryPM();
        //    ObsList.Add(new RoutingsViewModelData(shipmentPM, this, "Delivery", null, delivery));
        //}

        //else if (shipmentPM.ShipmentDeliveries.Count() > 0) {
        //    foreach(ShipmentDeliveryPM delivery in shipmentPM.ShipmentDeliveries.OrderBy(o => o.PickUpDeliveryNumber))
        //    {
        //        ObsList.Add(new RoutingsViewModelData(shipmentPM, this, "Delivery:" + delivery.PickUpDeliveryNumber, null, delivery));
        //    }
        //}
    }

    ngOnInit() {

        this.EntityPM = this._shipmentsService.getSingleShipmentByIdFromArray(this._entityId, 1);

        if (this.EntityPM != null) {            

            if (this.EntityPM.DirectionId == "D" && this.EntityPM.TransportModeId == "I") {
                this.IsInlandDomestic = true;
            }

            else {
                this.BuildItemsCollection();
            }

            this.IsDataLoaded = true;
        }
    }
}
