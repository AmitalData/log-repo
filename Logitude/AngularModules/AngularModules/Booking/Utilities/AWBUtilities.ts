import {BookingPM} from '../EntityPMs/BookingPM';
import {InfraSettings} from '../../Infrastructure/Utilities/InfraSettings';
import {AppTool, DateTool} from '../../Infrastructure/Tools';

export class AWBUtilities {

    public static IsText(input: string): boolean {
        var myResult = true;

        if (!AppTool.IsNullOrEmpty(input)) {
            input = input.trim().toUpperCase();

            if (!input.match(/^[A-Z0-9\-\. ]*$/)) {
                myResult = false;
            }
        }

        return myResult;
    }

    public static FormateValidate_IATACode(input: string): boolean {
        var myResult: boolean = false;

        if (!AppTool.IsNullOrEmpty(input)) {
            if (input.length <= 7) {
                var pattern = /^\d+$/;
                if (pattern.test(input)) {
                    myResult = true;
                }
            }
        }

        return myResult;
    }

    public static FormateValidate_CASSCode(input: string): boolean {
        var myResult: boolean = false;

        if (!AppTool.IsNullOrEmpty(input)) {
            if (input.length <= 4) {
                var pattern = /^\d+$/;
                if (pattern.test(input)) {
                    myResult = true;
                }
            }
        }

        return myResult;
    }

    public static FormateValidate_FlightNumber(input: string): boolean {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(input)) {
            input = input.trim().toUpperCase();

            if (input.match(/^[0-9]{3,4}$/)) {
                myResult = true;
            }

            else if (input.match(/^[0-9]{4}[A-Z]{1}$/)) {
                myResult = true;
            }
        }

        return myResult;
    }

    public static IsAirlineRuleFieldValid(myRule: any, myFieldValue: any): boolean {
        var myResult = true;

        if (myRule != null) {

            if (myFieldValue == null || isNaN(myFieldValue)) {
                if (myRule.IsMandatoryForSending) {
                    myResult = false;
                }
            }

            else if (typeof (myFieldValue) == "string") {
                if (AppTool.IsNullOrEmpty(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        myResult = false;
                    }
                }

                else if (myRule.MaxSize > 0) {
                    if (myFieldValue.length > myRule.MaxSize) {
                        myResult = false;
                    }
                }
            }

            else if (typeof (myFieldValue) == "number") {
                if (AppTool.IsNullOrZero(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        myResult = false;
                    }
                }
            }
        }

        return myResult;
    }

    public static GetWrongTextFormatMessage(fieldName: string): string {
        var myResult: string = "Invalid format. Can contain [a-z/0-9/./-]";

        if (!AppTool.IsNullOrEmpty(fieldName)) {
            myResult = fieldName + " " + myResult;
        }

        return myResult;
    }

    public static ValidateAWBFFR(bookingPM: BookingPM) {
        var myResult = new AWBFFRValidator();

        var myTenantManagementPM = InfraSettings.TenantManagementPM;
        var todayDate = DateTool.GetCurrentDateTimeAsUtc();
        var mainETD = new Date(bookingPM.MainCarriageETD.valueOf());

        if (mainETD.valueOf() < todayDate.valueOf()) {
            myResult.ETDFieldHasError = true;
            myResult.ETDFieldErrorMessage = "Please notice that you can't send a booking with a past ETD";
        }

        else if (!AppTool.IsNullOrEmpty(bookingPM.Transshipment1FromPortId)) {
            var t1_ETD = new Date(bookingPM.Transshipment1ETD.valueOf());

            if (t1_ETD.valueOf() < todayDate.valueOf()) {
                myResult.ETDFieldHasError = true;
                myResult.ETDFieldErrorMessage = "Please notice that you can't send a booking with a past ETD";
            }
        }

        else if (!AppTool.IsNullOrEmpty(bookingPM.Transshipment2FromPortId)) {
            var t2_ETD = new Date(bookingPM.Transshipment1ETD.valueOf());
            if (t2_ETD.valueOf() < todayDate.valueOf()) {
                myResult.ETDFieldHasError = true;
                myResult.ETDFieldErrorMessage = "Please notice that you can't send a booking with a past ETD";
            }
        }
        
        if (myTenantManagementPM.AWBMessagesCCSTypeCode == "GLSHK") {
            myResult.FFR = bookingPM.TenantZeroAirlineGLSHKFFR;

            if (AppTool.IsNullOrEmpty(bookingPM.TenantZeroAirlinePIMA)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "Airline communication parameter (PIMA) is missing";
            }

            if (AppTool.IsNullOrEmpty(myTenantManagementPM.PIMA)) {
                myResult.IsValid = false;
                myResult.TenantManagementFieldHasError = true;
                myResult.TenantManagementFieldErrorMessage = "Tenant communication parameter (PIMA) is missing";
            }

            if (bookingPM.ZeroGLSHKNeedsRegistration && !bookingPM.CarrierIsGLSHKRegistered) {
                myResult.AirlineRegistrationHasError = true;
                myResult.AirlineRegistrationErrorMessage = "Can’t send this message, the airline needs GLSHK registration. Please contact your account manager";
            }
        }

        else {
            myResult.FFR = bookingPM.TenantZeroAirlineChampFFR;

            if (AppTool.IsNullOrEmpty(bookingPM.TenantZeroAirlineTTY)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "This Airline doesn't support transmitting messages";
            }

            if (AppTool.IsNullOrEmpty(myTenantManagementPM.TTY)) {
                myResult.IsValid = false;
                myResult.TenantManagementFieldHasError = true;
                myResult.TenantManagementFieldErrorMessage = "Tenant communication parameter (TTY) is missing";
            }

            if (bookingPM.ZeroChampNeedsRegistration && !bookingPM.CarrierIsChampRegistered) {
                myResult.AirlineRegistrationHasError = true;
                myResult.AirlineRegistrationErrorMessage = "Can’t send this message, the airline needs Champ registration. Please contact your account manager";
            }
        }

        return myResult;
    }

    public static ValidateAWBFSR(bookingPM: BookingPM) {
        var myResult = new AWBFFRValidator();
        var myTenantManagementPM = InfraSettings.TenantManagementPM;

        if (myTenantManagementPM.AWBMessagesCCSTypeCode == "GLSHK") {
            myResult.FSR = bookingPM.TenantZeroAirlineGLSHKFSRFSA;

            if (AppTool.IsNullOrEmpty(bookingPM.TenantZeroAirlinePIMA)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "Airline communication parameter (PIMA) is missing";
            }

            if (AppTool.IsNullOrEmpty(myTenantManagementPM.PIMA)) {
                myResult.IsValid = false;
                myResult.TenantManagementFieldHasError = true;
                myResult.TenantManagementFieldErrorMessage = "Tenant communication parameter (PIMA) is missing";
            }

            if (bookingPM.ZeroGLSHKNeedsRegistration && !bookingPM.CarrierIsGLSHKRegistered) {
                myResult.AirlineRegistrationHasError = true;
                myResult.AirlineRegistrationErrorMessage = "Can’t send this message, the airline needs GLSHK registration. Please contact your account manager";
            }
        }

        else {
            myResult.FSR = bookingPM.TenantZeroAirlineChampFSRFSA;

            if (AppTool.IsNullOrEmpty(bookingPM.TenantZeroAirlineTTY)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "This Airline doesn't support transmitting messages";
            }

            if (AppTool.IsNullOrEmpty(myTenantManagementPM.TTY)) {
                myResult.IsValid = false;
                myResult.TenantManagementFieldHasError = true;
                myResult.TenantManagementFieldErrorMessage = "Tenant communication parameter (TTY) is missing";
            }

            if (bookingPM.ZeroChampNeedsRegistration && !bookingPM.CarrierIsChampRegistered) {
                myResult.AirlineRegistrationHasError = true;
                myResult.AirlineRegistrationErrorMessage = "Can’t send this message, the airline needs Champ registration. Please contact your account manager";
            }
        }

        return myResult;
    }
}

export class AWBFFRValidator {
    public IsValid: boolean;    
    public FFR: boolean;
    public FSR: boolean;
    public AirlineFieldHasError: boolean;
    public AirlineFieldErrorMessage: string;
    public TenantManagementFieldHasError: boolean;
    public TenantManagementFieldErrorMessage: string;
    public AirlineRegistrationHasError: boolean;
    public AirlineRegistrationErrorMessage: string;
    public ETDFieldHasError: boolean;
    public ETDFieldErrorMessage: string;

    constructor() {
        this.IsValid = true;
        this.AirlineFieldHasError = false;
        this.TenantManagementFieldHasError = false;
        this.AirlineRegistrationHasError = false;
        this.AirlineFieldErrorMessage = null;
        this.AirlineRegistrationErrorMessage = null;
        this.TenantManagementFieldErrorMessage = null;
    }
}