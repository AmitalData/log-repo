"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var AWBUtilities = /** @class */ (function () {
    function AWBUtilities() {
    }
    AWBUtilities.IsText = function (input) {
        var myResult = true;
        if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
            input = input.trim().toUpperCase();
            if (!input.match(/^[A-Z0-9\-\. ]*$/)) {
                myResult = false;
            }
        }
        return myResult;
    };
    AWBUtilities.FormateValidate_IATACode = function (input) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
            if (input.length <= 7) {
                var pattern = /^\d+$/;
                if (pattern.test(input)) {
                    myResult = true;
                }
            }
        }
        return myResult;
    };
    AWBUtilities.FormateValidate_CASSCode = function (input) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
            if (input.length <= 4) {
                var pattern = /^\d+$/;
                if (pattern.test(input)) {
                    myResult = true;
                }
            }
        }
        return myResult;
    };
    AWBUtilities.FormateValidate_FlightNumber = function (input) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
            input = input.trim().toUpperCase();
            if (input.match(/^[0-9]{3,4}$/)) {
                myResult = true;
            }
            else if (input.match(/^[0-9]{4}[A-Z]{1}$/)) {
                myResult = true;
            }
        }
        return myResult;
    };
    AWBUtilities.IsAirlineRuleFieldValid = function (myRule, myFieldValue) {
        var myResult = true;
        if (myRule != null) {
            if (myFieldValue == null || isNaN(myFieldValue)) {
                if (myRule.IsMandatoryForSending) {
                    myResult = false;
                }
            }
            else if (typeof (myFieldValue) == "string") {
                if (Tools_1.AppTool.IsNullOrEmpty(myFieldValue)) {
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
                if (Tools_1.AppTool.IsNullOrZero(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        myResult = false;
                    }
                }
            }
        }
        return myResult;
    };
    AWBUtilities.GetWrongTextFormatMessage = function (fieldName) {
        var myResult = "Invalid format. Can contain [a-z/0-9/./-]";
        if (!Tools_1.AppTool.IsNullOrEmpty(fieldName)) {
            myResult = fieldName + " " + myResult;
        }
        return myResult;
    };
    AWBUtilities.ValidateAWBFFR = function (bookingPM) {
        var myResult = new AWBFFRValidator();
        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var mainETD = new Date(bookingPM.MainCarriageETD.valueOf());
        if (mainETD.valueOf() < todayDate.valueOf()) {
            myResult.ETDFieldHasError = true;
            myResult.ETDFieldErrorMessage = "Please notice that you can't send a booking with a past ETD";
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(bookingPM.Transshipment1FromPortId)) {
            var t1_ETD = new Date(bookingPM.Transshipment1ETD.valueOf());
            if (t1_ETD.valueOf() < todayDate.valueOf()) {
                myResult.ETDFieldHasError = true;
                myResult.ETDFieldErrorMessage = "Please notice that you can't send a booking with a past ETD";
            }
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(bookingPM.Transshipment2FromPortId)) {
            var t2_ETD = new Date(bookingPM.Transshipment1ETD.valueOf());
            if (t2_ETD.valueOf() < todayDate.valueOf()) {
                myResult.ETDFieldHasError = true;
                myResult.ETDFieldErrorMessage = "Please notice that you can't send a booking with a past ETD";
            }
        }
        if (SessionLocator_1.SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
            myResult.FFR = bookingPM.TenantZeroAirlineGLSHKFFR;
            if (Tools_1.AppTool.IsNullOrEmpty(bookingPM.TenantZeroAirlinePIMA)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "Airline communication parameter (PIMA) is missing";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.PIMA)) {
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
            if (Tools_1.AppTool.IsNullOrEmpty(bookingPM.TenantZeroAirlineTTY)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "This Airline doesn't support transmitting messages";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.TTY)) {
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
    };
    AWBUtilities.ValidateAWBFSR = function (bookingPM) {
        var myResult = new AWBFFRValidator();
        if (SessionLocator_1.SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
            myResult.FSR = bookingPM.TenantZeroAirlineGLSHKFSRFSA;
            if (Tools_1.AppTool.IsNullOrEmpty(bookingPM.TenantZeroAirlinePIMA)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "Airline communication parameter (PIMA) is missing";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.PIMA)) {
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
            if (Tools_1.AppTool.IsNullOrEmpty(bookingPM.TenantZeroAirlineTTY)) {
                myResult.IsValid = false;
                myResult.AirlineFieldHasError = true;
                myResult.AirlineFieldErrorMessage = "This Airline doesn't support transmitting messages";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.TTY)) {
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
    };
    return AWBUtilities;
}());
exports.AWBUtilities = AWBUtilities;
var AWBFFRValidator = /** @class */ (function () {
    function AWBFFRValidator() {
        this.IsValid = true;
        this.AirlineFieldHasError = false;
        this.TenantManagementFieldHasError = false;
        this.AirlineRegistrationHasError = false;
        this.AirlineFieldErrorMessage = null;
        this.AirlineRegistrationErrorMessage = null;
        this.TenantManagementFieldErrorMessage = null;
    }
    return AWBFFRValidator;
}());
exports.AWBFFRValidator = AWBFFRValidator;
//# sourceMappingURL=AWBUtilities.js.map