"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var TextCodeTranslator_1 = require("./Utilities/TextCodeTranslator");
var SessionLocator_1 = require("./Utilities/SessionLocator");
var AppTool = /** @class */ (function () {
    function AppTool() {
    }
    AppTool.GetCounterPrefixLength = function (prefix) {
        //[B]_[YYYY]_SH
        var prefixLength = 0;
        var currentVar = "";
        var allVariables = [];
        if (!AppTool.IsNullOrEmpty(prefix)) {
            for (var i = 0; i < prefix.length; i++) {
                var c = prefix.charAt(i);
                if (c == '[') {
                    currentVar += c;
                }
                else if (c == ']') {
                    currentVar += c;
                    allVariables.push(currentVar);
                    currentVar = "";
                }
                else if (!AppTool.IsNullOrEmpty(currentVar))
                    currentVar += c;
            }
            var prefixWithoutVars_1 = prefix;
            allVariables.forEach(function (prefVar) {
                if (prefVar == "[B]")
                    prefixLength += 4;
                if (prefVar == "[YY]" || prefVar == "[MM]")
                    prefixLength += 2;
                if (prefVar == "[YYYY]")
                    prefixLength += 4;
            });
            allVariables.forEach(function (prefVar) {
                if (prefVar == "[B]" || prefVar == "[YY]" || prefVar == "[MM]" || "[YYYY]") {
                    prefixWithoutVars_1 = prefixWithoutVars_1.replace(prefVar, "");
                }
            });
            prefixLength += prefixWithoutVars_1.length;
        }
        return prefixLength;
    };
    AppTool.GetCounterResolvedNumber = function (prefix, startNumber, suffix, counterSize) {
        //[B]_[YYYY]_SH
        var calculatedNumber = (startNumber ? startNumber.toString() : '');
        var calculatedPrefix = (!AppTool.IsNullOrEmpty(prefix) ? prefix : '');
        var calculatedSuffix = (!AppTool.IsNullOrEmpty(suffix) ? suffix : '');
        var currentDate = new Date();
        var curr_date = currentDate.getDate();
        var curr_month = (currentDate.getMonth() + 1).toString(); //Months are zero based
        var mm = AppTool.PadLeft(curr_month, 2, '0');
        var yyyy = currentDate.getFullYear().toString();
        var yy = yyyy.substring(2, yyyy.length);
        if (!AppTool.IsNullOrEmpty(prefix)) {
            calculatedPrefix = calculatedPrefix.replace("[YYYY]", yyyy);
            calculatedPrefix = calculatedPrefix.replace("[YY]", yy);
            calculatedPrefix = calculatedPrefix.replace("[B]", 'BBBBB');
            calculatedPrefix = calculatedPrefix.replace("[MM]", mm);
        }
        if (!AppTool.IsNullOrEmpty(suffix)) {
            calculatedSuffix = calculatedSuffix.replace("[YYYY]", yyyy);
            calculatedSuffix = calculatedSuffix.replace("[YY]", yy);
            calculatedSuffix = calculatedSuffix.replace("[B]", 'BBBBB');
            calculatedSuffix = calculatedSuffix.replace("[MM]", mm);
        }
        var totalNumberLength = calculatedNumber.length;
        if (calculatedPrefix)
            totalNumberLength += calculatedPrefix.length;
        if (calculatedPrefix)
            totalNumberLength += calculatedSuffix.length;
        if (counterSize > totalNumberLength)
            calculatedNumber = AppTool.PadLeft(calculatedNumber, ((counterSize - totalNumberLength) + calculatedNumber.length), '0');
        calculatedNumber = calculatedPrefix + calculatedNumber + calculatedSuffix;
        return calculatedNumber;
    };
    AppTool.IsNullOrEmpty = function (myFieldValue) {
        var myResult = false;
        if (myFieldValue == null || myFieldValue === undefined) {
            myResult = true;
        }
        else if (typeof (myFieldValue) == "string") {
            myFieldValue = myFieldValue.split(" ").join("");
            if (myFieldValue.trim().length == 0) {
                myResult = true;
            }
        }
        else if (typeof (myFieldValue) == "number") {
            if (isNaN(myFieldValue)) {
                myResult = true;
            }
            else if (myFieldValue.toString().trim().length == 0) {
                myResult = true;
            }
        }
        return myResult;
    };
    AppTool.IsNullOrZero = function (myFieldValue) {
        var myResult = false;
        if (myFieldValue == null || myFieldValue === undefined) {
            myResult = true;
        }
        else if (myFieldValue.toString().trim().length == 0) {
            myResult = true;
        }
        else if (typeof (myFieldValue) == "number") {
            if (isNaN(myFieldValue)) {
                myResult = true;
            }
            else if (myFieldValue == 0) {
                myResult = true;
            }
        }
        return myResult;
    };
    AppTool.IsLCLEntity = function (myTransportModeId, myShipmentTypeId) {
        var myResult = false;
        if (myTransportModeId != null) {
            myTransportModeId = myTransportModeId.toUpperCase();
        }
        if (myShipmentTypeId != null) {
            myShipmentTypeId = myShipmentTypeId.toUpperCase();
        }
        if (myTransportModeId == "A") {
            myResult = true;
        }
        else if (myTransportModeId == "O" && myShipmentTypeId == "LCLD") {
            myResult = true;
        }
        else if (myTransportModeId == "I" && myShipmentTypeId == "LTL") {
            myResult = true;
        }
        return myResult;
    };
    AppTool.IsFCLEntity = function (myTransportModeId, myShipmentTypeId) {
        var myResult = false;
        if (myTransportModeId != null) {
            myTransportModeId = myTransportModeId.toUpperCase();
        }
        if (myShipmentTypeId != null) {
            myShipmentTypeId = myShipmentTypeId.toUpperCase();
        }
        if (myTransportModeId == "O" && (myShipmentTypeId == "FCLD" || myShipmentTypeId == "MYGO")) {
            myResult = true;
        }
        if (myTransportModeId == "I" && (myShipmentTypeId == "FTL" || myShipmentTypeId == "MYGI")) {
            myResult = true;
        }
        return myResult;
    };
    AppTool.PadLeft = function (myString, myCount, myChar) {
        if (this.IsNullOrZero(myCount)) {
            myCount = 0;
        }
        if (this.IsNullOrEmpty(myChar)) {
            myChar = "";
        }
        if (this.IsNullOrEmpty(myString)) {
            myString = "";
        }
        while (myString.length < myCount) {
            myString = myChar + myString;
        }
        return myString;
    };
    AppTool.PadRight = function (myString, myCount, myChar) {
        if (this.IsNullOrZero(myCount)) {
            myCount = 0;
        }
        if (this.IsNullOrEmpty(myChar)) {
            myChar = "";
        }
        if (this.IsNullOrEmpty(myString)) {
            myString = "";
        }
        while (myString.length < myCount) {
            myString = myString + myChar;
        }
        return myString;
    };
    AppTool.Round = function (num, decimals) {
        var myResult = null;
        if (!this.IsNullOrEmpty(num)) {
            var sign = num >= 0 ? 1 : -1;
            var myResultString = (Math.round((num * Math.pow(10, decimals)) + (sign * 0.001)) / Math.pow(10, decimals)).toFixed(decimals);
            myResult = +myResultString;
        }
        return myResult;
    };
    AppTool.GetNewGuid = function () {
        //http://stackoverflow.com/questions/26501688/a-typescript-guid-class
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };
    AppTool.GetLogitudeURL = function () {
        var logitude_url = location.href.replace('index.html', '');
        if (location.href.indexOf('localhost') > -1) {
            logitude_url = 'http://localhost:9996/';
        }
        else {
            var urlArr = location.href.split("/index.html");
            var url = urlArr[0];
            url = url.replace(url.substring(url.lastIndexOf('/'), url.length), "");
            logitude_url = url + "/";
        }
        return logitude_url;
    };
    AppTool.GetComponentName = function (myComponentPath) {
        var myComponentName = null;
        if (!this.IsNullOrEmpty(myComponentPath)) {
            var urlParts = myComponentPath.split("/");
            myComponentName = urlParts[urlParts.length - 1];
        }
        return myComponentName;
    };
    AppTool.GetLongMasterField = function (myTransportModeId, myAirlinePrefix, myMaster) {
        var myField = null;
        if (myTransportModeId == "A") {
            if (!this.IsNullOrEmpty(myAirlinePrefix) && !this.IsNullOrEmpty(myMaster)) {
                myField = myAirlinePrefix + "-" + myMaster;
            }
        }
        else {
            myField = myMaster;
        }
        return myField;
    };
    AppTool.IsAirlineRuleFieldValid = function (myRule, myFieldValue) {
        var myResult = true;
        if (myRule != null) {
            if (myFieldValue == null || isNaN(myFieldValue)) {
                if (myRule.IsMandatoryForSending) {
                    myResult = false;
                }
            }
            else if (typeof (myFieldValue) == "string") {
                if (this.IsNullOrEmpty(myFieldValue)) {
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
                if (this.IsNullOrZero(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        myResult = false;
                    }
                }
            }
        }
        return myResult;
    };
    AppTool.ValidateMasterField = function (myMasterField, myTransportModeId, isCheckDigit, isLimitedLength) {
        var myResult = null;
        if (!this.IsNullOrEmpty(myMasterField) && myTransportModeId == "A") {
            if (!FormatTool.IsNumeric(myMasterField)) {
                myResult = "Master Field must be all digits";
            }
            else {
                if (isLimitedLength && myMasterField.length != 8) {
                    myResult = "Master Field length must be 8 digits";
                }
                else {
                    if (isCheckDigit) {
                        var myPrefix = myMasterField.substr(0, 7);
                        var myCheckDegit = myMasterField.substr(7, 1);
                        var myPrefixInteger = +myPrefix;
                        var myMod = myPrefixInteger % 7;
                        if (myMod >= 7) {
                            myMod = myMod % 7;
                        }
                        if (myMod.toString() != myCheckDegit) {
                            myResult = "Master Field invalid check digit";
                        }
                    }
                }
            }
        }
        return myResult;
    };
    // Measurments
    AppTool.GetWeightFromWeight = function (fromWeightCode, toWeightCode, myWeight) {
        var myResult = null;
        var factorOfConvert = 1;
        if (myWeight != null) {
            if (this.IsNullOrEmpty(fromWeightCode)) {
                factorOfConvert = 1;
            }
            else {
                switch (fromWeightCode.toUpperCase()) {
                    case "KG": {
                        factorOfConvert = 1;
                        break;
                    }
                    case "LB": {
                        factorOfConvert = 0.45359237;
                        break;
                    } // 1 LB = 0.45359237 KG
                    case "MT": {
                        factorOfConvert = 1000;
                        break;
                    } // 1 mt = 1000 KG
                }
            }
            var weightInKilograms = myWeight * factorOfConvert;
            if (this.IsNullOrEmpty(toWeightCode)) {
                factorOfConvert = 1;
            }
            else {
                switch (toWeightCode.toUpperCase()) {
                    case "KG": {
                        factorOfConvert = 1;
                        break;
                    }
                    case "LB": {
                        factorOfConvert = 0.45359237;
                        break;
                    } // 1 LB = 0.45359237 KG
                    case "MT": {
                        factorOfConvert = 1000;
                        break;
                    } // 1 mt = 1000 KG
                }
            }
            myResult = weightInKilograms / factorOfConvert;
        }
        if (myResult != null) {
            myResult = this.Round(myResult, 3);
        }
        return myResult;
    };
    AppTool.GetWeightFromVolume = function (volumeCode, weightCode, myVolume, myRatio) {
        var myResult = null;
        var factorOfConvert = 1;
        if (myVolume != null) {
            var volumeInCBM = null;
            var weightInKilograms = null;
            if (this.IsNullOrEmpty(volumeCode)) {
                factorOfConvert = 1;
            }
            else {
                switch (volumeCode.toUpperCase()) {
                    case "CBM": {
                        factorOfConvert = 1;
                        break;
                    }
                    case "CBI": {
                        factorOfConvert = 61024;
                        break;
                    } // 1m³ = 61024in³
                    case "CBF": {
                        factorOfConvert = 35.315;
                        break;
                    } // 1m³ = 35.315ft³
                }
            }
            volumeInCBM = myVolume / factorOfConvert;
            weightInKilograms = (volumeInCBM * 1000) / myRatio;
            if (this.IsNullOrEmpty(weightCode)) {
                factorOfConvert = 1;
            }
            else {
                switch (weightCode.toUpperCase()) {
                    case "KG": {
                        factorOfConvert = 1;
                        break;
                    }
                    case "LB": {
                        factorOfConvert = 0.45359237;
                        break;
                    } // 1 LB = 0.45359237 KG
                    case "MT": {
                        factorOfConvert = 1000;
                        break;
                    } // 1 mt = 1000 KG
                }
            }
            myResult = weightInKilograms / factorOfConvert;
        }
        if (myResult != null) {
            myResult = this.Round(myResult, 3);
        }
        return myResult;
    };
    AppTool.GetWeightFromDimentions = function (myWidth, myHeight, myLength, myQuantity, dimentionCode, weightCode, myRatio) {
        var myResult = null;
        var factorOfConvert = 1;
        if (this.IsNullOrEmpty(dimentionCode)) {
            factorOfConvert = 1;
        }
        else {
            switch (dimentionCode.toUpperCase()) {
                case "CM": {
                    factorOfConvert = 1;
                    break;
                }
                case "INC": {
                    factorOfConvert = 2.54;
                    break;
                } // 1 inch = 2.54 centimeters
                case "FT": {
                    factorOfConvert = 30.48;
                    break;
                } // 1ft = 30.48 centimeters
            }
        }
        var volumeInCentimeters = myQuantity * (myWidth * myHeight * myLength) * Math.pow(factorOfConvert, 3);
        var volumeInCBM = volumeInCentimeters / Math.pow(100, 3);
        var weightInKilograms = (volumeInCBM * 1000) / myRatio;
        if (this.IsNullOrEmpty(weightCode)) {
            factorOfConvert = 1;
        }
        else {
            switch (weightCode.toUpperCase()) {
                case "KG": {
                    factorOfConvert = 1;
                    break;
                }
                case "LB": {
                    factorOfConvert = 0.45359237;
                    break;
                } // 1 LB = 0.45359237 KG
                case "MT": {
                    factorOfConvert = 1000;
                    break;
                } // 1 mt = 1000 KG
            }
        }
        myResult = weightInKilograms / factorOfConvert;
        if (myResult != null) {
            myResult = this.Round(myResult, 3);
        }
        return myResult;
    };
    AppTool.GetRatio = function (directionId, transportModeId, shipmentTypeId, countryCode) {
        var myResult = null;
        if (!this.IsNullOrEmpty(countryCode)) {
            if (countryCode.toUpperCase() == "US") {
                if (directionId == "D") {
                    if (transportModeId == "A") {
                        myResult = 7;
                    }
                    else if (transportModeId == "I") {
                        if (shipmentTypeId == "LTL") {
                            myResult = 9;
                        }
                    }
                }
            }
        }
        if (myResult == null) {
            {
                switch (transportModeId) {
                    case "A": {
                        myResult = 6;
                        break;
                    }
                    case "O": {
                        myResult = 1;
                        break;
                    }
                    case "I":
                        {
                            if (shipmentTypeId == "LTL") {
                                myResult = 3.3;
                            }
                            else {
                                myResult = 1;
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        return myResult;
    };
    AppTool.GetRatioFromDimFactor = function (myDimFactor, dimentionCode, weightCode) {
        var myResult = null;
        if (myDimFactor != null) {
            var WeightFactorOfConvert = 1;
            var DimensiosFactorOfConvert = 1;
            if (!this.IsNullOrEmpty(weightCode)) {
                switch (weightCode.toUpperCase()) {
                    case "KG": {
                        WeightFactorOfConvert = 1;
                        break;
                    }
                    case "LB": {
                        WeightFactorOfConvert = 0.45359237;
                        break;
                    }
                    case "MT": {
                        WeightFactorOfConvert = 1000;
                        break;
                    }
                }
            }
            if (!this.IsNullOrEmpty(dimentionCode)) {
                switch (dimentionCode.toUpperCase()) {
                    case "CM": {
                        DimensiosFactorOfConvert = 1;
                        break;
                    }
                    case "INC": {
                        DimensiosFactorOfConvert = 2.54;
                        break;
                    }
                    case "FT": {
                        DimensiosFactorOfConvert = 30.48;
                        break;
                    }
                }
            }
            myResult = myDimFactor * Math.pow(DimensiosFactorOfConvert, 3) / (WeightFactorOfConvert * 1000);
        }
        return myResult;
    };
    AppTool.GetDimFactorFromRatio = function (myRatio, dimentionCode, weightCode) {
        var myResult = null;
        if (myRatio != null) {
            var WeightFactorOfConvert = 1;
            var DimensiosFactorOfConvert = 1;
            if (!this.IsNullOrEmpty(weightCode)) {
                switch (weightCode.toUpperCase()) {
                    case "KG": {
                        WeightFactorOfConvert = 1;
                        break;
                    }
                    case "LB": {
                        WeightFactorOfConvert = 0.45359237;
                        break;
                    }
                    case "MT": {
                        WeightFactorOfConvert = 1000;
                        break;
                    }
                }
            }
            if (!this.IsNullOrEmpty(dimentionCode)) {
                switch (dimentionCode.toUpperCase()) {
                    case "CM": {
                        DimensiosFactorOfConvert = 1;
                        break;
                    }
                    case "INC": {
                        DimensiosFactorOfConvert = 2.54;
                        break;
                    }
                    case "FT": {
                        DimensiosFactorOfConvert = 30.48;
                        break;
                    }
                }
            }
            myResult = WeightFactorOfConvert * 1000 * myRatio / Math.pow(DimensiosFactorOfConvert, 3);
        }
        if (myResult != null) {
            var toString = myResult.toString();
            var myArray = toString.split('.');
            if (myArray.length > 1) {
                var strDigits = "0." + myArray[1];
                var digits = +strDigits;
                if (digits < 0.5) {
                    myResult = Math.floor(myResult);
                }
                else {
                    myResult = Math.ceil(myResult);
                }
            }
        }
        return myResult;
    };
    AppTool.GetVolumeFromWeight = function (weightCode, volumeCode, myWeight, myRatio) {
        var myResult = null;
        var factorOfConvert = 1;
        if (myWeight != null) {
            if (this.IsNullOrEmpty(weightCode)) {
                factorOfConvert = 1;
            }
            else {
                switch (weightCode.toUpperCase()) {
                    case "KG": {
                        factorOfConvert = 1;
                        break;
                    }
                    case "LB": {
                        factorOfConvert = 0.45359237;
                        break;
                    } // 1 LB = 0.45359237 KG
                    case "MT": {
                        factorOfConvert = 1000;
                        break;
                    } // 1 mt = 1000 KG
                }
            }
            var weightInKilograms = weightInKilograms = myWeight * factorOfConvert;
            var volumeInCBM = (weightInKilograms * myRatio) / 1000;
            if (this.IsNullOrEmpty(volumeCode)) {
                factorOfConvert = 1;
            }
            else {
                switch (volumeCode.toUpperCase()) {
                    case "CBM": {
                        factorOfConvert = 1;
                        break;
                    }
                    case "CBI": {
                        factorOfConvert = 61024;
                        break;
                    } // 1m³ = 61024in³
                    case "CBF": {
                        factorOfConvert = 35.315;
                        break;
                    } // 1m³ = 35.315ft³
                }
            }
            myResult = volumeInCBM * factorOfConvert;
        }
        if (myResult != null) {
            myResult = this.Round(myResult, 3);
        }
        return myResult;
    };
    AppTool.GetVolumeFromDimentions = function (dimentionCode, volumeCode, myWidth, myHeight, myLength, myQuantity) {
        var myResult = null;
        var factorOfConvert = 1;
        if (this.IsNullOrEmpty(dimentionCode)) {
            dimentionCode = "CM";
        }
        if (this.IsNullOrEmpty(volumeCode)) {
            volumeCode = "CBM";
        }
        if (dimentionCode.toUpperCase() == "INC" && volumeCode.toUpperCase() == "CBI") {
            myResult = myQuantity * (myWidth * myHeight * myLength);
        }
        else if (dimentionCode.toUpperCase() == "FT" && volumeCode.toUpperCase() == "CBF") {
            myResult = myQuantity * (myWidth * myHeight * myLength);
        }
        else {
            switch (dimentionCode.toUpperCase()) {
                case "CM": {
                    factorOfConvert = 1;
                    break;
                }
                case "INC": {
                    factorOfConvert = 2.54;
                    break;
                } // 1 inch = 2.54 centimeters
                case "FT": {
                    factorOfConvert = 30.48;
                    break;
                } // 1ft = 30.48 centimeters
            }
            var volumeInCentimeters = myQuantity * (myWidth * myHeight * myLength) * Math.pow(factorOfConvert, 3);
            var volumeInCBM = volumeInCentimeters / Math.pow(100, 3);
            switch (volumeCode.toUpperCase()) {
                case "CBM": {
                    factorOfConvert = 1;
                    break;
                }
                case "CBI": {
                    factorOfConvert = 61024;
                    break;
                } // 1m³ = 61024in³
                case "CBF": {
                    factorOfConvert = 35.315;
                    break;
                } // 1m³ = 35.315ft³
            }
            myResult = volumeInCBM * factorOfConvert;
        }
        if (myResult != null) {
            myResult = this.Round(myResult, 3);
        }
        return myResult;
    };
    AppTool.GetDimentionsCodeFromVolumeCode = function (volumeCode) {
        var myResult = null;
        if (volumeCode) {
            switch (volumeCode.toUpperCase()) {
                case "CBM": {
                    myResult = "Cm";
                    break;
                }
                case "CBF": {
                    myResult = "Ft";
                    break;
                }
                case "CBI": {
                    myResult = "Inc";
                    break;
                }
            }
        }
        return myResult;
    };
    AppTool.ComputePackageVolume = function (myQuantity, myWidth, myHeight, myLength, myWeight, myRatio, dimentionCode, volumeCode, fromWeightCode) {
        var myResult = null;
        if (myWidth == null || myHeight == null || myLength == null || myQuantity == null) {
            if (myWeight != null) {
                myResult = this.GetVolumeFromWeight(fromWeightCode, volumeCode, myWeight, myRatio);
            }
        }
        else {
            myResult = this.GetVolumeFromDimentions(dimentionCode, volumeCode, myWidth, myHeight, myLength, myQuantity);
        }
        if (myResult != null) {
            myResult = this.Round(myResult, 3);
        }
        return myResult;
    };
    AppTool.ComputePackageVolumetricWeight = function (myQuantity, myWidth, myHeight, myLength, myVolume, myWeight, myRatio, dimentionCode, volumeCode, grossWeightCode, chargeableWeightCode) {
        var myResult = null;
        if (myWidth == null || myHeight == null || myLength == null || myQuantity == null) {
            if (myVolume != null) {
                myResult = this.GetWeightFromVolume(volumeCode, chargeableWeightCode, myVolume, myRatio);
            }
            else if (myWeight != null) {
                myResult = this.GetWeightFromWeight(grossWeightCode, chargeableWeightCode, myWeight);
            }
        }
        else {
            myResult = this.GetWeightFromDimentions(myWidth, myHeight, myLength, myQuantity, dimentionCode, chargeableWeightCode, myRatio);
        }
        if (myResult != null) {
            myResult = this.Round(myResult, 3);
        }
        return myResult;
    };
    AppTool.CalculateChargeableWeight = function (myGrossWeight, myVolumetricWeight, grossWeightUnitCode, chargeableWeightUnitCode, directionId, transportModeId) {
        var myResult = null;
        if (myGrossWeight != null || myVolumetricWeight != null) {
            var grossWeightInVolumetricUnit = this.GetWeightFromWeight(grossWeightUnitCode, chargeableWeightUnitCode, myGrossWeight);
            if (grossWeightInVolumetricUnit != null && myVolumetricWeight == null) {
                myResult = grossWeightInVolumetricUnit;
            }
            if (grossWeightInVolumetricUnit == null && myVolumetricWeight != null) {
                myResult = myVolumetricWeight;
            }
            if (grossWeightInVolumetricUnit != null && myVolumetricWeight != null) {
                myResult = grossWeightInVolumetricUnit > myVolumetricWeight ? grossWeightInVolumetricUnit : myVolumetricWeight;
            }
        }
        if (myResult != null) {
            myResult = this.RoundChargeableWeight(myResult, chargeableWeightUnitCode, directionId, transportModeId);
        }
        return myResult;
    };
    AppTool.RoundChargeableWeight = function (myArgs, chargeableWeightUnitCode, directionId, transportModeId) {
        var result = myArgs;
        if (chargeableWeightUnitCode != "MT") {
            if (directionId == "E" && transportModeId == "A") {
                if (result != null) {
                    var toString = result.toString();
                    var r = toString.split('.');
                    if (r.length > 1) {
                        var strDigits = "0." + r[1];
                        var digits = +strDigits;
                        var integer = +r[0];
                        if (digits <= 0.5) {
                            result = integer + 0.5;
                        }
                        else {
                            result = integer + 1;
                        }
                    }
                }
            }
        }
        return result;
    };
    AppTool.GetChargeableWeightUnitCode = function (myTransportModeId, myShipmentTypeId) {
        var myResult = this.TenantPM.ChargeableWeightUnitCode;
        if ((myTransportModeId == "O" && myShipmentTypeId == "LCLD") || (myTransportModeId == "I" && myShipmentTypeId == "LTL")) {
            myResult = this.TenantPM.WeightMeasurementUnitCode;
        }
        return myResult;
    };
    AppTool.DeepCopy = function (oldObj) {
        var newObj = oldObj;
        if (oldObj && typeof oldObj === "object") {
            if (Object.prototype.toString.call(oldObj) === "[object Array]") {
                newObj = [];
            }
            else {
                newObj = Object.create(oldObj);
            }
            for (var i in oldObj) {
                newObj[i] = this.DeepCopy(oldObj[i]);
            }
        }
        return newObj;
    };
    AppTool.CreateCopy = function (dataContext, fields) {
        //var copy = Object.create(dataContext);
        var copy = {};
        if (dataContext != null && fields != null) {
            for (var i in fields) {
                var fieldName = fields[i];
                var fieldValue = dataContext[fieldName];
                copy[fieldName] = fieldValue;
            }
        }
        return copy;
    };
    AppTool.RejectChanges = function (dataContext, fields) {
        if (dataContext != null && fields != null) {
            for (var i in fields) {
                dataContext[i] = fields[i];
            }
        }
    };
    AppTool.GetEnvironmentLogo = function (myLogoCode) {
        var myResult = null;
        var LogoURL = window.sessionStorage.getItem("LogoURL");
        if (myLogoCode == "C.R.M") {
            myResult = "./Images/ApplicationLogo/CRMLogo.png";
        }
        else if (myLogoCode == "A.N.G") {
            myResult = "./Images/ApplicationLogo/Angular/AngularLogo.png";
        }
        else if (myLogoCode == "U.N.I") {
            myResult = "./Images/ApplicationLogo/UnifreightLogo.jpg";
        }
        else if (myLogoCode == "L.O.B") {
            if (LogoURL) {
                myResult = LogoURL;
            }
            else {
                myResult = "./Images/ApplicationLogo/LogBox.png";
            }
        }
        else {
            myResult = "./Images/ApplicationLogo/LogitudeLogo.jpg";
        }
        return myResult;
    };
    AppTool.GetTextWidth = function (myString, fontSize) {
        if (fontSize === void 0) { fontSize = 12; }
        var myResult = 0;
        if (!this.IsNullOrEmpty(myString)) {
            var canvas = document.createElement('canvas');
            var ctx = canvas.getContext("2d");
            ctx.font = fontSize + "px Lucida Sans Unicode";
            myResult = ctx.measureText(myString).width;
        }
        return myResult;
    };
    AppTool.AddAmounts = function (amount1, amount2) {
        var myResult = 0;
        if (!this.IsNullOrEmpty(amount1)) {
            myResult += amount1;
        }
        if (!this.IsNullOrEmpty(amount2)) {
            myResult += amount2;
        }
        return myResult;
    };
    AppTool.GetIdsArrayText = function (array) {
        var _this = this;
        var myResult = null;
        if (array) {
            if (array.length > 0) {
                array.forEach(function (item) {
                    if (_this.IsNullOrEmpty(myResult)) {
                        myResult = item;
                    }
                    else {
                        myResult += "." + item;
                    }
                });
            }
        }
        return myResult;
    };
    AppTool.GetMainMenuIconCode = function (Code) {
        var myResult = null;
        switch (Code) {
            case "General.MH.Operations": {
                myResult = "Box";
                break;
            }
            case "General.MH.ContainersFU": {
                myResult = "FollowUps";
                break;
            }
            case "General.MH.Quotes": {
                myResult = "Quote";
                break;
            }
            case "General.MH.SharedLogistics": {
                myResult = "LogBoxIcon";
                break;
            }
            case "General.MH.CRM":
            case "General.MH.Dashboard":
            case "General.MH.Importers":
                {
                    myResult = "Bars";
                    break;
                }
            case "General.MH.FullAccounting":
            case "General.MH.Accounting":
                {
                    myResult = "Dollar";
                    break;
                }
            case "General.MH.Contacts":
                {
                    myResult = "Contacts";
                    break;
                }
            case "General.MH.Social":
                {
                    myResult = "Social";
                    break;
                }
            case "General.MH.Maintenance": {
                myResult = "Maintenance";
                break;
            }
            case "General.MH.Reports": {
                myResult = "Table";
                break;
            }
            case "General.MH.GettingStarted": {
                myResult = "GettingStarted";
                break;
            }
            case "General.MH.ActivationWizard":
                {
                    myResult = "Box";
                    break;
                }
            case "General.MH.Ticket":
                {
                    myResult = "Tickets";
                    break;
                }
            case "General.MH.TimeManagement":
                {
                    myResult = "Time";
                    break;
                }
            case "General.MH.FilingInbox": {
                myResult = "Inbox";
                break;
            }
            case "General.MH.CrossDocks": {
                myResult = "CrossDocks";
                break;
            }
            case "General.MH.Documents":
            case "General.MH.DocumentsFiling":
                {
                    myResult = "Documents";
                    break;
                }
            case "General.MH.Depositions": {
                myResult = "Deposition";
                break;
            }
            case "General.MH.TariffModule": {
                myResult = "Tariff";
                break;
            }
            default: {
                myResult = "Person";
                break;
            }
        }
        return myResult;
    };
    AppTool.Replace = function (myString, replacedText, replaceWith) {
        if (!this.IsNullOrEmpty(myString)) {
            myString = myString.split(replacedText).join(replaceWith);
        }
        return myString;
    };
    AppTool.KillEventEmitter = function (event) {
        if (event) {
            var isStopped = event['isStopped'];
            if (isStopped == false) {
                event.unsubscribe();
            }
            event = null;
        }
    };
    AppTool.GetActivityImageSrc = function (code) {
        var ImageSrc = "";
        switch (code) {
            case "TS": {
                ImageSrc = "./Images/Buttons/TS.png";
                break;
            }
            case "VM": {
                ImageSrc = "./Images/Buttons/VM.png";
                break;
            }
            case "AP": {
                ImageSrc = "./Images/Buttons/AP.png";
                break;
            }
            case "CL": {
                ImageSrc = "./Images/Buttons/CL.png";
                break;
            }
            case "EI":
                {
                    ImageSrc = "./Images/Buttons/EI.png";
                    break;
                }
            case "EO":
                {
                    ImageSrc = "./Images/Buttons/EO.png";
                    break;
                }
        }
        return ImageSrc;
    };
    AppTool.IsMobileDetected = function () {
        if (navigator.userAgent.match(/Android/i)
            || navigator.userAgent.match(/webOS/i)
            || navigator.userAgent.match(/iPhone/i)
            || navigator.userAgent.match(/iPad/i)
            || navigator.userAgent.match(/iPod/i)
            || navigator.userAgent.match(/BlackBerry/i)
            || navigator.userAgent.match(/Windows Phone/i)) {
            return true;
        }
        else {
            return false;
        }
    };
    return AppTool;
}());
exports.AppTool = AppTool;
var DateTool = /** @class */ (function () {
    function DateTool() {
    }
    DateTool.GetCurrentDateAsUtc = function () {
        var myResult = new Date();
        myResult.setUTCFullYear(myResult.getFullYear());
        myResult.setUTCMonth(myResult.getMonth());
        myResult.setUTCDate(myResult.getDate());
        myResult.setUTCHours(0);
        myResult.setUTCMinutes(0);
        myResult.setUTCSeconds(0);
        myResult.setUTCMilliseconds(0);
        return myResult;
    };
    DateTool.GetCurrentDateTimeAsUtc = function () {
        var myResult = new Date();
        myResult.setUTCFullYear(myResult.getFullYear());
        myResult.setUTCMonth(myResult.getMonth());
        myResult.setUTCDate(myResult.getDate());
        myResult.setUTCHours(myResult.getHours());
        myResult.setUTCMinutes(myResult.getMinutes());
        myResult.setUTCSeconds(myResult.getSeconds());
        myResult.setUTCMilliseconds(myResult.getMilliseconds());
        return myResult;
    };
    DateTool.GetDateByDay = function (day) {
        var myResult = new Date();
        myResult.setUTCMilliseconds(0);
        myResult.setUTCSeconds(0);
        myResult.setUTCMinutes(0);
        myResult.setUTCHours(0);
        myResult.setUTCDate(myResult.getDate() + day);
        myResult.setUTCMonth(myResult.getMonth());
        myResult.setUTCFullYear(myResult.getFullYear());
        return myResult;
    };
    DateTool.GetDateByMonth = function (month) {
        var myResult = new Date();
        var myResultDays = myResult.getDate();
        var myResultMonth = myResult.getMonth() + month;
        var myResultFullYear = myResult.getFullYear();
        if (myResultDays > 28) {
            var requestedMonthMaxDays = new Date(myResultFullYear, myResultMonth + 1, 0).getDate();
            if (requestedMonthMaxDays < myResultDays) {
                myResultDays = requestedMonthMaxDays;
            }
        }
        myResult.setUTCMilliseconds(0);
        myResult.setUTCSeconds(0);
        myResult.setUTCMinutes(0);
        myResult.setUTCHours(0);
        myResult.setUTCDate(myResultDays);
        myResult.setUTCMonth(myResultMonth);
        myResult.setUTCFullYear(myResultFullYear);
        return myResult;
    };
    DateTool.GetStartOfTheWeek = function (date) {
        var myResult = null;
        if (date != null) {
            date = new Date(date.toString());
            var day = date.getDay() + 1;
            var diff = date.getDate() - day + (day == 0 ? -6 : 1);
            myResult = new Date(date.setDate(diff));
        }
        return myResult;
    };
    DateTool.GetDateFromDate = function (givenDate, isDateOnly) {
        if (isDateOnly === void 0) { isDateOnly = false; }
        var myResult = null;
        if (!AppTool.IsNullOrEmpty(givenDate)) {
            if (typeof (givenDate) == "string") {
                var str = givenDate + "";
                //console.log("toDateString:" + new Date(str).toDateString());
                //console.log("toISOString:" + new Date(str).toISOString());
                //console.log("toLocaleDateString:" + new Date(str).toLocaleDateString());
                //console.log("toUTCString:" + new Date(str).toUTCString());
                //givenDate = new Date(Date.parse(str));
                givenDate = new Date(str);
                //givenDate.setHours(givenDate.getHours() + 2);
            }
            myResult = new Date();
            myResult.setUTCFullYear(givenDate.getUTCFullYear());
            myResult.setUTCMonth(givenDate.getUTCMonth());
            myResult.setUTCDate(givenDate.getUTCDate());
            if (isDateOnly) {
                myResult.setUTCHours(0);
                myResult.setUTCMinutes(0);
                myResult.setUTCSeconds(0);
                myResult.setUTCMilliseconds(0);
            }
            else {
                myResult.setUTCHours(givenDate.getUTCHours());
                myResult.setUTCMinutes(givenDate.getUTCMinutes());
                myResult.setUTCSeconds(givenDate.getUTCSeconds());
                myResult.setUTCMilliseconds(givenDate.getUTCMilliseconds());
            }
        }
        return myResult;
    };
    DateTool.TruncateTime = function (date, setActual) {
        if (setActual === void 0) { setActual = false; }
        var myResult = null;
        if (date != null) {
            var myResult = this.GetDateParts(date).DateObject;
            if (setActual) {
                myResult.setHours(0);
                myResult.setMinutes(0);
                myResult.setSeconds(0);
                myResult.setMilliseconds(0);
            }
            else {
                myResult.setUTCHours(0);
                myResult.setUTCMinutes(0);
                myResult.setUTCSeconds(0);
                myResult.setUTCMilliseconds(0);
            }
        }
        return myResult;
    };
    DateTool.AddHours = function (myDate, hours) {
        var myResult = this.GetDateParts(myDate).DateObject;
        if (myResult != null) {
            myResult.setUTCHours(myResult.getHours() + hours);
        }
        return myResult;
    };
    DateTool.AddDays = function (myDate, days) {
        var myResult = this.GetDateParts(myDate).DateObject;
        if (myResult != null) {
            myResult.setUTCDate(myResult.getDate() + days);
        }
        return myResult;
    };
    DateTool.NextDay = function (myDate, days) {
        var myResult = this.GetDateParts(myDate).DateObject;
        if (myDate != null) {
            myResult.setUTCDate(myDate.getDate() + days);
        }
        return myResult;
    };
    DateTool.GetDaysBetweenDates = function (date1, date2, useFloor) {
        if (useFloor === void 0) { useFloor = false; }
        var myResult = 0;
        if (date1 != null && date2 != null) {
            if (date1 != undefined && date2 != undefined) {
                date1 = this.TruncateTime(date1, useFloor);
                date2 = this.TruncateTime(date2, useFloor);
                var d1 = new Date(date1.toString());
                var d2 = new Date(date2.toString());
                var timeDiff = Math.abs(d2.getTime() - d1.getTime());
                var daysDiff = timeDiff / (1000 * 3600 * 24);
                if (useFloor) {
                    myResult = Math.floor(daysDiff);
                }
                else {
                    myResult = Math.ceil(daysDiff);
                }
            }
        }
        return myResult;
    };
    DateTool.GetMinutesBetweenDates = function (date1, date2) {
        var myResult = 0;
        if (date1 != null && date2 != null) {
            if (date1 != undefined && date2 != undefined) {
                date1 = this.TruncateTime(date1);
                date2 = this.TruncateTime(date2);
                var d1 = new Date(date1.toString());
                var d2 = new Date(date2.toString());
                var timeDiff = Math.abs(d2.getTime() - d1.getTime());
                var Daysdiff = Math.ceil(timeDiff / (1000 * 60));
                myResult = Daysdiff;
            }
        }
        return myResult;
    };
    DateTool.GetHoursBetweenDates = function (date1, date2) {
        var myResult = 0;
        if (date1 != null && date2 != null) {
            if (date1 != undefined && date2 != undefined) {
                date1 = this.TruncateTime(date1);
                date2 = this.TruncateTime(date2);
                var d1 = new Date(date1.toString());
                var d2 = new Date(date2.toString());
                var timeDiff = Math.abs(d2.getTime() - d1.getTime());
                var Daysdiff = Math.ceil(timeDiff / (1000 * 3600));
                myResult = Daysdiff;
            }
        }
        return myResult;
    };
    DateTool.GetSecondsBetweenDates = function (date1, date2) {
        var myResult = 0;
        if (date1 != null && date2 != null) {
            if (date1 != undefined && date2 != undefined) {
                date1 = this.TruncateTime(date1);
                date2 = this.TruncateTime(date2);
                var d1 = new Date(date1.toString());
                var d2 = new Date(date2.toString());
                var timeDiff = Math.abs(d2.getTime() - d1.getTime());
                var Daysdiff = Math.ceil(timeDiff / (1000));
                myResult = Daysdiff;
            }
        }
        return myResult;
    };
    DateTool.GetDateParts = function (input) {
        var myDateParts = new DateParts();
        if (!AppTool.IsNullOrEmpty(input)) {
            // 2009-12-18 10:54:50.546
            // 2016-12-27T00:00:00
            // 2016-12-27T16:58:15.527
            // 2012-02-07CET00:00:00
            // 2009-04-19T16:11:05+02:00
            // 2009-04-19T16:11:05Z
            if (typeof (input) == "string") {
                var dateTimeString = (input + "").replace('Z', '');
                var splitBy = " ";
                if (dateTimeString.indexOf("CET") !== -1) {
                    splitBy = "CET";
                }
                else if (dateTimeString.indexOf("T") !== -1) {
                    splitBy = "T";
                }
                if (!AppTool.IsNullOrEmpty(dateTimeString)) {
                    var dateString = dateTimeString.split(splitBy)[0];
                    var timeString = dateTimeString.split(splitBy)[1];
                    if (!AppTool.IsNullOrEmpty(dateString)) {
                        var dateParts = dateString.split('-');
                        myDateParts.Year = +dateParts[0];
                        myDateParts.Month = +dateParts[1];
                        myDateParts.Day = +dateParts[2];
                        myDateParts.LocalYear = myDateParts.Year;
                        myDateParts.LocalMonth = myDateParts.Month;
                        myDateParts.LocalDay = myDateParts.Day;
                    }
                    if (!AppTool.IsNullOrEmpty(timeString)) {
                        var timeParts = timeString.split(':');
                        myDateParts.Hours = +timeParts[0];
                        myDateParts.Minutes = +timeParts[1];
                        myDateParts.Seconds = +timeParts[2].split('.')[0];
                        if (!AppTool.IsNullOrEmpty(timeParts[2].split('.')[1])) {
                            myDateParts.Milliseconds = +timeParts[2].split('.')[1];
                        }
                    }
                }
            }
            else {
                myDateParts.Year = input.getUTCFullYear();
                myDateParts.Month = input.getUTCMonth() + 1;
                myDateParts.Day = input.getUTCDate();
                myDateParts.Hours = input.getUTCHours();
                myDateParts.Minutes = input.getUTCMinutes();
                myDateParts.Seconds = input.getUTCSeconds();
                myDateParts.Milliseconds = input.getUTCMilliseconds();
                // "Thu Apr 05 2018 03:00:00 GMT+0300 (Eastern Europe Daylight Time)"
                var inputConverted = input + "";
                var inputConvertedParts = inputConverted.split(' ');
                var MonthName = inputConvertedParts[1];
                myDateParts.LocalYear = +inputConvertedParts[3];
                myDateParts.LocalMonth = this.GetMonthFromName(MonthName);
                myDateParts.LocalDay = +inputConvertedParts[2];
            }
            // we need it by this sort (Year > Month > Day)
            // if current month is 6 (max day is 30/6)
            // and we used this method to get date of (31/5)
            // then it will be a problem
            // the new Date(); will init date as today date (month 6)
            // but then when assign the day (31) it will 31/6 which will be converted to 1/7
            // then when assign the month (5) it will become (1/5) instead of (31/5)
            // example
            // input                                                26/06/2017
            // var dateObject = new Date();                         31/08/2017  Thu Aug 31 2017 08:16:39 GMT+0300
            // dateObject.setUTCFullYear(myDateParts.Year);         31/08/2017
            // dateObject.setUTCMonth((myDateParts.Month - 1));     01/07/2017  Sat Jul 01 2017 08:21:59 GMT+0300   (No Date 31/06 so it turned it to 01/07)
            // dateObject.setUTCDate(myDateParts.Day);              26/07/2017  Wed Jul 26 2017 08:21:59 GMT+0300
            var dateObject = new Date();
            dateObject.setUTCMonth(0);
            dateObject.setUTCDate(1);
            dateObject.setUTCFullYear(myDateParts.Year);
            dateObject.setUTCMonth((myDateParts.Month - 1));
            dateObject.setUTCDate(myDateParts.Day);
            dateObject.setUTCHours(myDateParts.Hours);
            dateObject.setUTCMinutes(myDateParts.Minutes);
            dateObject.setUTCSeconds(myDateParts.Seconds);
            dateObject.setUTCMilliseconds(myDateParts.Milliseconds);
            myDateParts.DateObject = dateObject;
            myDateParts.Hours12 = myDateParts.Hours > 12 ? (myDateParts.Hours - 12) : myDateParts.Hours;
            myDateParts.DateTicks = myDateParts.DateObject.valueOf();
            myDateParts.TotalMinutes = (myDateParts.Hours * 60) + myDateParts.Minutes;
        }
        return myDateParts;
    };
    DateTool.GetDateFormats = function (input) {
        var myDateFormats = new DateFormats();
        if (!AppTool.IsNullOrEmpty(input)) {
            var myDateParts = this.GetDateParts(input);
            var stringOfYear = AppTool.PadLeft("" + myDateParts.Year, 4, '0');
            var stringOfMonth = AppTool.PadLeft("" + myDateParts.Month, 2, '0');
            var stringOfDay = AppTool.PadLeft("" + myDateParts.Day, 2, '0');
            var stringOfHours = AppTool.PadLeft("" + myDateParts.Hours, 2, '0');
            var stringOfHours12 = AppTool.PadLeft("" + myDateParts.Hours12, 2, '0');
            var stringOfMinutes = AppTool.PadLeft("" + myDateParts.Minutes, 2, '0');
            var stringOfSeconds = AppTool.PadLeft("" + myDateParts.Seconds, 2, '0');
            var stringOfMilliseconds = AppTool.PadLeft("" + myDateParts.Milliseconds, 3, '0');
            // Build Formats
            myDateFormats.AMPM = myDateParts.Hours >= 12 ? 'PM' : 'AM';
            myDateFormats.LocalDateString = myDateParts.DateObject.toLocaleDateString();
            myDateFormats.LocalTimeString = myDateParts.DateObject.toLocaleTimeString();
            myDateFormats.LocalDateTimeString = myDateParts.DateObject.toLocaleString();
            myDateFormats.ShortTimeString = stringOfHours + ":" + stringOfMinutes;
            myDateFormats.LongTimeString = stringOfHours + ":" + stringOfMinutes + ":" + stringOfSeconds;
            myDateFormats.ShortTimeString12 = stringOfHours12 + ":" + stringOfMinutes + " " + myDateFormats.AMPM;
            myDateFormats.LongTimeString12 = stringOfHours12 + ":" + stringOfMinutes + ":" + stringOfSeconds + " " + myDateFormats.AMPM;
            // ShortDateString
            if (!AppTool.IsNullOrEmpty(this.TenantPM.DateTimeFormat)) {
                var myDateTimeFormatPrefix = this.TenantPM.DateTimeFormat.toLowerCase().substring(0, 2);
                if (myDateTimeFormatPrefix == "mm") {
                    myDateFormats.ShortDateString = stringOfMonth + "/" + stringOfDay + "/" + stringOfYear;
                }
                else {
                    myDateFormats.ShortDateString = stringOfDay + "/" + stringOfMonth + "/" + stringOfYear;
                }
            }
            else {
                myDateFormats.ShortDateString = myDateFormats.LocalDateString;
            }
            // DateString
            //var date_Today = this.GetCurrentDateAsUtc();
            //var date_Tommorow = this.AddDays(this.GetCurrentDateAsUtc(), 1);
            //var date_Yesterday = this.AddDays(this.GetCurrentDateAsUtc(), -1);
            var date_Today = new Date();
            var date_Tommorow = new Date();
            var date_Yesterday = new Date();
            date_Tommorow.setDate(date_Tommorow.getDate() + 1);
            date_Yesterday.setDate(date_Yesterday.getDate() + -1);
            var dateParts_Today = this.GetDateParts(date_Today);
            var dateParts_Tommorow = this.GetDateParts(date_Tommorow);
            var dateParts_Yesterday = this.GetDateParts(date_Yesterday);
            if (dateParts_Today.LocalYear == myDateParts.Year && dateParts_Today.LocalMonth == myDateParts.Month && dateParts_Today.LocalDay == myDateParts.Day) {
                myDateFormats.DateString = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Today");
            }
            else if (dateParts_Tommorow.LocalYear == myDateParts.Year && dateParts_Tommorow.LocalMonth == myDateParts.Month && dateParts_Tommorow.LocalDay == myDateParts.Day) {
                myDateFormats.DateString = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Tomorrow");
            }
            else if (dateParts_Yesterday.LocalYear == myDateParts.Year && dateParts_Yesterday.LocalMonth == myDateParts.Month && dateParts_Yesterday.LocalDay == myDateParts.Day) {
                myDateFormats.DateString = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Yesterday");
            }
            else {
                myDateFormats.DateString = myDateFormats.ShortDateString;
            }
            myDateFormats.DateParts = myDateParts;
            this.GetPartsNames(myDateFormats);
        }
        return myDateFormats;
    };
    DateTool.GetTodayDateParts = function () {
        var myResult = new Date();
        var Hours_NOR = myResult.getHours();
        var Hours_UTC = myResult.getUTCHours();
        var Hours = Hours_UTC - Hours_NOR;
        myResult.setUTCFullYear(myResult.getUTCFullYear());
        myResult.setUTCMonth(myResult.getUTCMonth());
        myResult.setUTCDate(myResult.getUTCDate());
        myResult.setUTCHours(Hours);
        myResult.setUTCMinutes(0);
        myResult.setUTCSeconds(0);
        myResult.setUTCMilliseconds(0);
        var myDateParts = this.GetDateParts(myResult);
        return myDateParts;
    };
    DateTool.GetTommorowDateParts = function () {
        var myResult = new Date();
        var Hours_NOR = myResult.getHours();
        var Hours_UTC = myResult.getUTCHours();
        var Hours = Hours_UTC - Hours_NOR;
        myResult.setUTCFullYear(myResult.getUTCFullYear());
        myResult.setUTCMonth(myResult.getUTCMonth());
        myResult.setUTCDate(myResult.getUTCDate() + 1);
        myResult.setUTCHours(Hours);
        myResult.setUTCMinutes(0);
        myResult.setUTCSeconds(0);
        myResult.setUTCMilliseconds(0);
        var myDateParts = this.GetDateParts(myResult);
        return myDateParts;
    };
    DateTool.GetYesterdayDateParts = function () {
        var myResult = new Date();
        var Hours_NOR = myResult.getHours();
        var Hours_UTC = myResult.getUTCHours();
        var Hours = Hours_UTC - Hours_NOR;
        myResult.setUTCFullYear(myResult.getUTCFullYear());
        myResult.setUTCMonth(myResult.getUTCMonth());
        myResult.setUTCDate(myResult.getUTCDate() - 1);
        myResult.setUTCHours(Hours);
        myResult.setUTCMinutes(0);
        myResult.setUTCSeconds(0);
        myResult.setUTCMilliseconds(0);
        var myDateParts = this.GetDateParts(myResult);
        return myDateParts;
    };
    DateTool.GetPartsNames = function (dateFormats) {
        if (dateFormats) {
            var dayIndex = dateFormats.DateParts.DateObject.getDay();
            var monthIndex = dateFormats.DateParts.DateObject.getMonth();
            switch (dayIndex) {
                case 0: {
                    dateFormats.DayName = "Sunday", dateFormats.DayNameShort = "Sun";
                    break;
                }
                case 1: {
                    dateFormats.DayName = "Monday", dateFormats.DayNameShort = "Mon";
                    break;
                }
                case 2: {
                    dateFormats.DayName = "Tuesday", dateFormats.DayNameShort = "Tue";
                    break;
                }
                case 3: {
                    dateFormats.DayName = "Wednesday", dateFormats.DayNameShort = "Wed";
                    break;
                }
                case 4: {
                    dateFormats.DayName = "Thursday", dateFormats.DayNameShort = "Thu";
                    break;
                }
                case 5: {
                    dateFormats.DayName = "Friday", dateFormats.DayNameShort = "Fri";
                    break;
                }
                case 6: {
                    dateFormats.DayName = "Saturday", dateFormats.DayNameShort = "Sat";
                    break;
                }
            }
            switch (monthIndex) {
                case 0: {
                    dateFormats.MonthName = "January", dateFormats.MonthNameShort = "Jan";
                    break;
                }
                case 1: {
                    dateFormats.MonthName = "February", dateFormats.MonthNameShort = "Feb";
                    break;
                }
                case 2: {
                    dateFormats.MonthName = "March", dateFormats.MonthNameShort = "Mar";
                    break;
                }
                case 3: {
                    dateFormats.MonthName = "April", dateFormats.MonthNameShort = "Apr";
                    break;
                }
                case 4: {
                    dateFormats.MonthName = "May", dateFormats.MonthNameShort = "May";
                    break;
                }
                case 5: {
                    dateFormats.MonthName = "June", dateFormats.MonthNameShort = "Jun";
                    break;
                }
                case 6: {
                    dateFormats.MonthName = "July", dateFormats.MonthNameShort = "Jul";
                    break;
                }
                case 7: {
                    dateFormats.MonthName = "August", dateFormats.MonthNameShort = "Aug";
                    break;
                }
                case 8: {
                    dateFormats.MonthName = "September", dateFormats.MonthNameShort = "Sep";
                    break;
                }
                case 9: {
                    dateFormats.MonthName = "October", dateFormats.MonthNameShort = "Oct";
                    break;
                }
                case 10: {
                    dateFormats.MonthName = "November", dateFormats.MonthNameShort = "Nov";
                    break;
                }
                case 11: {
                    dateFormats.MonthName = "December", dateFormats.MonthNameShort = "Dec";
                    break;
                }
            }
        }
    };
    DateTool.GetMonthFromName = function (name) {
        var myResult = 0;
        if (name) {
            switch (name.toLowerCase()) {
                case "jan":
                case "january": {
                    myResult = 1;
                    break;
                }
                case "feb":
                case "february": {
                    myResult = 2;
                    break;
                }
                case "mar":
                case "march": {
                    myResult = 3;
                    break;
                }
                case "apr":
                case "april": {
                    myResult = 4;
                    break;
                }
                case "may":
                case "may": {
                    myResult = 5;
                    break;
                }
                case "jun":
                case "june": {
                    myResult = 6;
                    break;
                }
                case "jul":
                case "july": {
                    myResult = 7;
                    break;
                }
                case "aug":
                case "august": {
                    myResult = 8;
                    break;
                }
                case "sep":
                case "september": {
                    myResult = 9;
                    break;
                }
                case "oct":
                case "october": {
                    myResult = 10;
                    break;
                }
                case "nov":
                case "november": {
                    myResult = 11;
                    break;
                }
                case "dec":
                case "december": {
                    myResult = 12;
                    break;
                }
            }
        }
        return myResult;
    };
    DateTool.GetCurrentFullDayOfWeek = function (dayIndex) {
        var dayName = "";
        switch (dayIndex) {
            case 0: {
                dayName = "Sunday";
                break;
            }
            case 1: {
                dayName = "Monday";
                break;
            }
            case 2: {
                dayName = "Tuesday";
                break;
            }
            case 3: {
                dayName = "Wednesday";
                break;
            }
            case 4: {
                dayName = "Thursday";
                break;
            }
            case 5: {
                dayName = "Friday";
                break;
            }
            case 6: {
                dayName = "Saturday";
                break;
            }
        }
        return dayName;
    };
    DateTool.GetRelativeRateDate = function (entityDate, rateDate, endText) {
        var dateString = "";
        if (entityDate != null && rateDate != null) {
            var count = 0;
            var dateParts1 = this.GetDateParts(entityDate);
            var dateParts2 = this.GetDateParts(rateDate);
            if (dateParts2.DateTicks > dateParts1.DateTicks) {
                var todayDate = this.GetCurrentDateAsUtc();
                var dateTime = dateParts2.DateObject;
                if (dateTime.valueOf() === todayDate.valueOf()) {
                    //dateString = TextCodeTranslator.Translate("General.O.Today");
                }
                else {
                    dateString = this.GetDateFormats(rateDate).ShortDateString;
                }
            }
            else {
                if (AppTool.IsNullOrEmpty(endText)) {
                    endText = "old";
                }
                var days = DateTool.GetDaysBetweenDates(dateParts1.DateObject, dateParts2.DateObject);
                if (days == 0) {
                    //dateString = TextCodeTranslator.Translate("General.O.Today");
                }
                else {
                    if (days <= 30) {
                        dateString = days == 1 ? days + " day" : days + " days";
                    }
                    else {
                        if (Math.abs(dateParts1.Year - dateParts2.Year) > 0) {
                            count = Math.abs(dateParts1.Year - dateParts2.Year);
                            dateString = count == 1 ? count + " year" : count + " years";
                        }
                        else if (Math.abs(dateParts1.Month - dateParts2.Month) > 0) {
                            count = Math.abs(dateParts1.Month - dateParts2.Month);
                            dateString = count == 1 ? count + " month" : count + " months";
                        }
                    }
                    dateString = dateString + " " + endText;
                }
            }
        }
        return dateString;
    };
    DateTool.AddMillisecond = function (myDate, millisecond) {
        if (myDate) {
            var myResult = new Date(myDate + "");
            if (myResult != null) {
                myResult.setTime(myResult.getTime() + millisecond);
            }
            return myResult;
        }
        return myDate;
    };
    DateTool.AddSecond = function (myDate, second) {
        return DateTool.AddMillisecond(myDate, second * 1000);
    };
    DateTool.AddMinute = function (myDate, minute) {
        return DateTool.AddSecond(myDate, minute * 60);
    };
    DateTool.AddHour = function (myDate, hour) {
        return DateTool.AddMinute(myDate, hour * 60);
    };
    DateTool.GetDate = function (year, month, day, hour, minute, second) {
        var date = new Date();
        date.setUTCHours(hour);
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);
        date.setUTCMilliseconds(0);
        var endDayNumber = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
        if (endDayNumber == 31) {
            date.setUTCDate(day);
            date.setUTCMonth(month);
        }
        else {
            date.setUTCMonth(month);
            date.setUTCDate(day);
        }
        date.setUTCFullYear(year);
        return date;
    };
    DateTool.GetMinDateTime = function () {
        var myResult;
        //myResult= new Date();
        //myResult.setUTCFullYear(0);
        //myResult.setUTCMonth(0);
        //myResult.setUTCDate(0);
        //myResult.setUTCHours(0);
        //myResult.setUTCMinutes(0);
        //myResult.setUTCSeconds(0);
        //myResult.setUTCMilliseconds(0);
        //https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date
        //The JavaScript date is based on a time value that is milliseconds since midnight 01 January, 1970 UTC.A day holds 86, 400, 000 milliseconds.The JavaScript Date object range is - 100, 000, 000 days to 100, 000, 000 days relative to 01 January, 1970 UTC.
        // itzik my birthday is 1971 so i was born 1 year After Century :) !!!
        //myResult = new Date(1970);
        myResult = new Date(null);
        return myResult;
    };
    DateTool.IsNullOrMinDateTime = function (myDate) {
        if (myDate == null || myDate === undefined) {
            return true;
        }
        if (myDate.valueOf() == DateTool.GetMinDateTime().valueOf()) {
            return true;
        }
        return false;
    };
    DateTool.IsDateBigger = function (date1, date2) {
        var myResult = false;
        if (!AppTool.IsNullOrEmpty(date1) && !AppTool.IsNullOrEmpty(date2)) {
            var Date1Ticks = this.GetDateParts(date1).DateTicks;
            var Date2Ticks = this.GetDateParts(date2).DateTicks;
            if (Date1Ticks > Date2Ticks) {
                myResult = true;
            }
        }
        return myResult;
    };
    DateTool.IsActualDateValid = function (date) {
        var myResult = true;
        if (!AppTool.IsNullOrEmpty(date)) {
            var Date1Ticks = this.GetDateParts(date).DateTicks;
            var Date2Ticks = this.GetCurrentDateTimeAsUtc().valueOf();
            if (Date1Ticks > Date2Ticks) {
                var ticks = Date1Ticks - Date2Ticks;
                var seconds = ticks / 1000;
                var minutes = seconds / 60;
                if (minutes > (24 * 60)) {
                    myResult = false;
                }
            }
        }
        return myResult;
    };
    DateTool.ActualDateMessage = "Can't set Field to future date";
    return DateTool;
}());
exports.DateTool = DateTool;
var FontTool = /** @class */ (function () {
    function FontTool() {
    }
    FontTool.Red = "#E53030";
    FontTool.Black = "#282E30";
    FontTool.Green = "#009161";
    FontTool.Gray = "#6E7172";
    FontTool.Orange = "#F37021";
    FontTool.Magenta = "#FF6BFF";
    FontTool.CellDisabledColor = "#A8AAAD";
    FontTool.CellDisabledBackground = "rgba(230, 231, 232, 0.5)";
    FontTool.CellIsCheckedBackground = "rgba(208, 224, 234, 0.4)";
    return FontTool;
}());
exports.FontTool = FontTool;
var FormatTool = /** @class */ (function () {
    function FormatTool() {
    }
    FormatTool.IsEmail = function (input) {
        var myResult = true;
        if (!AppTool.IsNullOrEmpty(input)) {
            var exp = /^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (!input.match(exp)) {
                myResult = false;
            }
        }
        return myResult;
    };
    FormatTool.IsTextFormatted = function (input) {
        var myResult = false;
        if (AppTool.IsNullOrEmpty(input)) {
            myResult = true;
        }
        else {
            input = input.trim().toUpperCase();
            input = input.replace(/[^A-Z0-9\-\. ]/g, "");
            // trdtrdt dd' sd < lo
            // input.replace(/[A-Z0-9\-\. ]/g, "");     Result='<
            // input.replace(/[^A-Z0-9\-\. ]/g, "");    Result= trdtrdt dd sd  lo
            if (!AppTool.IsNullOrEmpty(input)) {
                myResult = true;
            }
        }
        return myResult;
    };
    FormatTool.IsDecimal = function (input) {
        var isDecimal = false;
        if (input != null) {
            input = input.trim();
            if (input.length > 0) {
                isDecimal = true;
                var isFirstDot = true;
                var isFirstminus = true;
                for (var i = 0; i < input.length; i++) {
                    var letter = input[i];
                    if (!letter.match(/^[0-9]/ig)) {
                        if (letter == '.') {
                            if (i == 0) {
                                isDecimal = false;
                                break;
                            }
                            else if (isFirstDot) {
                                isFirstDot = false;
                                continue;
                            }
                            else {
                                isDecimal = false;
                                break;
                            }
                        }
                        else if (letter == '-') {
                            if (i != 0) {
                                isDecimal = false;
                                break;
                            }
                            else {
                                if (isFirstminus) {
                                    isFirstminus = false;
                                    continue;
                                }
                                else {
                                    isDecimal = false;
                                    break;
                                }
                            }
                        }
                        else {
                            isDecimal = false;
                            break;
                        }
                    }
                }
            }
        }
        return isDecimal;
    };
    FormatTool.IsText = function (input) {
        var myResult = true;
        if (!AppTool.IsNullOrEmpty(input)) {
            input = input.trim().toUpperCase();
            if (!input.match(/^[A-Z0-9\-\. ]*$/)) {
                myResult = false;
            }
        }
        return myResult;
    };
    FormatTool.IsAlpha = function (input) {
        var myResult = true;
        if (!AppTool.IsNullOrEmpty(input)) {
            input = input.trim().toUpperCase();
            if (!input.match(/^[A-Z]*$/)) {
                myResult = false;
            }
        }
        return myResult;
    };
    FormatTool.IsNumeric = function (input) {
        var myResult = true;
        if (!AppTool.IsNullOrEmpty(input)) {
            input = input.trim().toUpperCase();
            if (!input.match(/^[0-9]*$/)) {
                myResult = false;
            }
        }
        return myResult;
    };
    FormatTool.IsAlphaNumeric = function (input) {
        var myResult = true;
        if (!AppTool.IsNullOrEmpty(input)) {
            input = input.trim().toUpperCase();
            if (!input.match(/^[A-Z0-9]*$/)) {
                myResult = false;
            }
        }
        return myResult;
    };
    FormatTool.IsEnglishText = function (input) {
        var myResult = true;
        if (!AppTool.IsNullOrEmpty(input)) {
            input = input.trim().toUpperCase();
            // English Only
            //if (!input.match(/^[\x20-\x7E]*$/)) {
            //    myResult = false;
            //}
            // English + Latin
            if (!input.match(/^[\x20-\xFF]*$/)) {
                myResult = false;
            }
        }
        return myResult;
    };
    FormatTool.FormatNumber = function (myNumber, myFormat) {
        if (myFormat === void 0) { myFormat = 'N2'; }
        var first = ",";
        var second = ".";
        switch (SessionLocator_1.SessionLocator.TenantPM.NumberFormatCode) {
            case "CD": {
                first = ",";
                second = ".";
                break;
            }
            case "DC": {
                first = ".";
                second = ",";
                break;
            }
            case "AD": {
                first = "'";
                second = ".";
                break;
            }
            default:
                {
                    first = ",";
                    second = ".";
                    break;
                }
        }
        var myResult = "";
        if (!AppTool.IsNullOrEmpty(myNumber)) {
            var isValid = true;
            if (typeof (myNumber) == "string") {
                isValid = false;
                if (FormatTool.IsDecimal(myNumber + "")) {
                    myNumber = +myNumber;
                    isValid = true;
                }
            }
            if (isValid) {
                var myFractionDigits = 2;
                switch (myFormat.toString().toLowerCase()) {
                    case "n0": {
                        myFractionDigits = 0;
                        break;
                    }
                    case "n1": {
                        myFractionDigits = 1;
                        break;
                    }
                    case "n2": {
                        myFractionDigits = 2;
                        break;
                    }
                    case "n3": {
                        myFractionDigits = 3;
                        break;
                    }
                    case "n4": {
                        myFractionDigits = 4;
                        break;
                    }
                    case "n5": {
                        myFractionDigits = 5;
                        break;
                    }
                    default: {
                        myFractionDigits = 2;
                        break;
                    }
                }
                myNumber = AppTool.Round(myNumber, myFractionDigits);
                var myStringNumber = myNumber + "";
                var myStringNumber1 = myStringNumber.split('.')[0];
                var myStringNumber2 = myStringNumber.split('.')[1];
                myResult = myStringNumber1.replace(/\B(?=(\d{3})+(?!\d))/g, first);
                if (!AppTool.IsNullOrEmpty(myStringNumber2)) {
                    myResult += second + myStringNumber2;
                }
                if (myFormat.toString().toLowerCase() != "n0") {
                    var side1;
                    var side2;
                    if (first == "." && !myResult.toString().toLowerCase().includes(",")) {
                        var arr = myResult.split('.');
                        var firstRound = true;
                        var zero = /^0+$/;
                        if (arr.length == 2) {
                            if (arr[1].match(zero)) {
                                side1 = arr[0];
                            }
                            else if (arr[0].match(zero)) {
                                side1 = "0";
                            }
                        }
                        else if (arr.length == 1) {
                            side1 = arr[0];
                        }
                        if (AppTool.IsNullOrEmpty(side1)) {
                            arr.forEach(function (p) {
                                if (firstRound)
                                    side1 = p;
                                else
                                    side1 += "." + p;
                                firstRound = false;
                            });
                        }
                        if (arr.length > 1)
                            if (myResult.includes(second))
                                side2 = myResult.split(second)[myResult.split(second).length - 1];
                    }
                    else {
                        if (myResult.toString().toLowerCase().includes(",") && first == ".") {
                            side1 = myResult.split(',')[0];
                            side2 = myResult.split(',')[1];
                        }
                        else {
                            side1 = myResult.split('.')[0];
                            side2 = myResult.split('.')[1];
                        }
                    }
                    myResult = side1 + second + AppTool.PadRight(side2, myFractionDigits, '0');
                }
            }
        }
        return myResult;
    };
    FormatTool.CustomFormatNumber = function (myNumber, myFormat) {
        if (myFormat === void 0) { myFormat = 'N2'; }
        var first = ",";
        var second = ".";
        switch (SessionLocator_1.SessionLocator.TenantPM.NumberFormatCode) {
            case "CD": {
                first = ",";
                second = ".";
                break;
            }
            case "DC": {
                first = ".";
                second = ",";
                break;
            }
            case "AD": {
                first = "'";
                second = ".";
                break;
            }
            default:
                {
                    first = ",";
                    second = ".";
                    break;
                }
        }
        var myResult = "";
        if (!AppTool.IsNullOrEmpty(myNumber)) {
            var isValid = true;
            if (typeof (myNumber) == "string") {
                isValid = false;
                if (FormatTool.IsDecimal(myNumber + "")) {
                    myNumber = +myNumber;
                    isValid = true;
                }
            }
            if (isValid) {
                var myFractionDigits = 2;
                switch (myFormat.toString().toLowerCase()) {
                    case "n0": {
                        myFractionDigits = 0;
                        break;
                    }
                    case "n1": {
                        myFractionDigits = 1;
                        break;
                    }
                    case "n2": {
                        myFractionDigits = 2;
                        break;
                    }
                    case "n3": {
                        myFractionDigits = 3;
                        break;
                    }
                    case "n4": {
                        myFractionDigits = 4;
                        break;
                    }
                    case "n5": {
                        myFractionDigits = 5;
                        break;
                    }
                    default: {
                        myFractionDigits = 2;
                        break;
                    }
                }
                myNumber = AppTool.Round(myNumber, myFractionDigits);
                var myStringNumber = myNumber + "";
                var myStringNumber1 = myStringNumber.split('.')[0];
                var myStringNumber2 = myStringNumber.split('.')[1];
                myResult = myStringNumber1.replace(/\B(?=(\d{3})+(?!\d))/g, first);
                if (!AppTool.IsNullOrEmpty(myStringNumber2)) {
                    myResult += second + myStringNumber2;
                }
                if (myFormat.toString().toLowerCase() != "n0") {
                    var side1 = myResult.split('.')[0];
                    var side2 = myResult.split('.')[1];
                    myResult = side1 + second + AppTool.PadRight(side2, myFractionDigits, '0');
                }
            }
        }
        return myResult;
    };
    FormatTool.Validate_IATACode = function (input) {
        var myResult = false;
        if (!AppTool.IsNullOrEmpty(input)) {
            if (input.length <= 7) {
                var pattern = /^\d+$/;
                if (pattern.test(input)) {
                    myResult = true;
                }
            }
        }
        return myResult;
    };
    FormatTool.Validate_CASSCode = function (input) {
        var myResult = false;
        if (!AppTool.IsNullOrEmpty(input)) {
            if (!AppTool.IsNullOrEmpty(input)) {
                if (input.length <= 4) {
                    var pattern = /^\d+$/;
                    if (pattern.test(input)) {
                        myResult = true;
                    }
                }
            }
        }
        return myResult;
    };
    FormatTool.Validate_FlightNumber = function (input) {
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
    };
    FormatTool.Validate_CommodityNo = function (input) {
        var myResult = false;
        if (AppTool.IsNullOrEmpty(input)) {
            myResult = true;
        }
        else {
            if (input.match(/^[0-9]{4,7}$/)) {
                myResult = true;
            }
        }
        return myResult;
    };
    FormatTool.Validate_DeclaredCarriage = function (input) {
        var myResult = false;
        if (AppTool.IsNullOrEmpty(input)) {
            myResult = true;
        }
        else {
            input = input.replace(" ", "").toUpperCase();
            if (input == "NVD") {
                myResult = true;
            }
            else {
                input = (input.length <= 12) ? input : input.substring(0, 12);
                if (this.IsDecimal(input)) {
                    myResult = true;
                }
            }
        }
        return myResult;
    };
    FormatTool.Validate_DeclaredCustoms = function (input) {
        var myResult = false;
        if (AppTool.IsNullOrEmpty(input)) {
            myResult = true;
        }
        else {
            input = input.replace(" ", "").toUpperCase();
            if (input == "NCV") {
                myResult = true;
            }
            else {
                input = (input.length <= 12) ? input : input.substring(0, 12);
                if (this.IsDecimal(input)) {
                    myResult = true;
                }
            }
        }
        return myResult;
    };
    FormatTool.Validate_DeclaredInsurrence = function (input) {
        var myResult = false;
        if (AppTool.IsNullOrEmpty(input)) {
            myResult = true;
        }
        else {
            input = input.replace(" ", "").toUpperCase();
            if (input == "XXX") {
                myResult = true;
            }
            else {
                input = (input.length <= 12) ? input : input.substring(0, 12);
                if (this.IsDecimal(input)) {
                    myResult = true;
                }
            }
        }
        return myResult;
    };
    FormatTool.GetWrongTextFormatMessage = function (fieldName) {
        var myResult = "Invalid format. Can contain [a-z/0-9/./-]";
        if (!AppTool.IsNullOrEmpty(fieldName)) {
            myResult = fieldName + " " + myResult;
        }
        return myResult;
    };
    FormatTool.GetIntegerValue = function (input) {
        var myResult = 0;
        if (!AppTool.IsNullOrEmpty(input)) {
            var inputString = input + "";
            if (this.IsDecimal(inputString)) {
                myResult = +inputString.split(".")[0];
            }
        }
        return myResult;
    };
    FormatTool.ValidateContainerNumber = function (input) {
        var myResult = null;
        if (!AppTool.IsNullOrEmpty(input)) {
            input = input.trim().toUpperCase();
            if (!input.match(/^([a-zA-Z]{4})([0-9]{7})$/)) {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.ContainerNumberFormatisInvalid");
            }
            else {
                var sum = 0;
                for (var i = 0; i < input.length - 1; i++) {
                    if (this.IsAlpha(input[i])) {
                        sum = sum + (this.GetCharCode(input[i]) * Math.pow(2, i));
                    }
                    else {
                        sum = sum + (this.GetIntegerDigit(input[i]) * Math.pow(2, i));
                    }
                }
                var integrSum = this.GetIntegerValue(sum);
                var checkDigit = this.GetIntegerDigit(input[input.length - 1]);
                var divisionby11 = integrSum / 11;
                var erasedecimaldigits = this.GetIntegerValue(divisionby11);
                var multiplyby11 = erasedecimaldigits * 11;
                var validCheckDigit = (integrSum - multiplyby11);
                if (validCheckDigit == 10) {
                    validCheckDigit = 0;
                }
                if (validCheckDigit != checkDigit) {
                    var wStr = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.ContainerNumberCheckDigitiswrong");
                    wStr = wStr.replace("%CheckDigit", validCheckDigit.toString());
                    myResult = wStr;
                }
            }
        }
        return myResult;
    };
    FormatTool.GetCharCode = function (c) {
        switch (c) {
            case 'A': return 10;
            case 'B': return 12;
            case 'C': return 13;
            case 'D': return 14;
            case 'E': return 15;
            case 'F': return 16;
            case 'G': return 17;
            case 'H': return 18;
            case 'I': return 19;
            case 'J': return 20;
            case 'K': return 21;
            case 'L': return 23;
            case 'M': return 24;
            case 'N': return 25;
            case 'O': return 26;
            case 'P': return 27;
            case 'Q': return 28;
            case 'R': return 29;
            case 'S': return 30;
            case 'T': return 31;
            case 'U': return 32;
            case 'V': return 34;
            case 'W': return 35;
            case 'X': return 36;
            case 'Y': return 37;
            case 'Z': return 38;
            default: return 0;
        }
    };
    FormatTool.GetIntegerDigit = function (c) {
        switch (c) {
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
            default: return 0;
        }
    };
    FormatTool.FormatBigNumbersToExtension = function (value) {
        if (value >= 1000000000)
            return (Math.round(value / 1000000000 * 100) / 100) + "Bil";
        else if (value >= 1000000)
            return (Math.round(value / 1000000 * 100) / 100) + "Mil";
        else if (value >= 1000) {
            console.log(Math.round(value / 1000) / 1000);
            return (Math.round(value / 1000) / 1000) + "K";
        }
        else
            return value + "";
    };
    FormatTool.PatternText = "lol";
    return FormatTool;
}());
exports.FormatTool = FormatTool;
var ArrayTool = /** @class */ (function () {
    function ArrayTool() {
    }
    ArrayTool.Sum = function (array, fieldname) {
        var myResult = 0;
        if (array && fieldname) {
            array.forEach(function (item) {
                var itemValue = item[fieldname];
                if (!AppTool.IsNullOrEmpty(itemValue)) {
                    if (typeof (itemValue) == "string") {
                        if (FormatTool.IsDecimal(itemValue)) {
                            itemValue = +itemValue;
                        }
                    }
                    if (typeof (itemValue) == "number") {
                        myResult += itemValue;
                    }
                }
            });
        }
        if (AppTool.IsNullOrEmpty(myResult)) {
            myResult = 0;
        }
        return myResult;
    };
    ArrayTool.Contains = function (array, item) {
        var myResult = false;
        if (array && item) {
            if (array.indexOf(item) > -1) {
                myResult = true;
            }
        }
        return myResult;
    };
    ArrayTool.Max = function (array, fieldname) {
        var myResult = 0;
        if (array && fieldname) {
            array.forEach(function (item) {
                var itemValue = item[fieldname];
                if (!AppTool.IsNullOrEmpty(itemValue)) {
                    if (typeof (itemValue) == "string") {
                        if (FormatTool.IsDecimal(itemValue)) {
                            itemValue = +itemValue;
                        }
                    }
                    if (typeof (itemValue) == "number") {
                        if (myResult < itemValue) {
                            myResult = itemValue;
                        }
                    }
                }
            });
        }
        if (AppTool.IsNullOrEmpty(myResult)) {
            myResult = 0;
        }
        return myResult;
    };
    ArrayTool.Sort = function (array, fieldname) {
        var myResult = array;
        if (array) {
            if (array.length > 1) {
                var filledItem = array.filter(function (f) { return !AppTool.IsNullOrEmpty(f[fieldname]); })[0];
                if (filledItem) {
                    var fieldValue = filledItem[fieldname];
                    var fieldType = typeof (fieldValue);
                    switch (fieldType) {
                        case "string": {
                            break;
                        }
                        case "number": {
                            break;
                        }
                        case "date": {
                            break;
                        }
                    }
                }
            }
        }
        return myResult;
    };
    ArrayTool.SortByDate = function (array, fieldname) {
        var myResult = array;
        if (array) {
            if (array.length > 1) {
                var array_Sorted = [];
                var array_Fixed = [];
                array.forEach(function (item) {
                    array_Fixed.push(new SortingClass(item, fieldname));
                });
                array_Fixed.sort(function (a, b) { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1; }).forEach(function (item) {
                    array_Sorted.push(item.Item);
                });
                myResult = array_Sorted;
            }
        }
        return myResult;
    };
    ArrayTool.GroupIt = function (array, funcReturnKeyFromItemOrLiteral
    //(item: any) => string  //User-Defined Type Guards
    ///fieldname: string
    ) {
        var _this = this;
        //let grouped: {};//return object that his properties R the Keys {USD: Array(2)} or {USD: Array(2) , EUR : Array(1)}
        var toString = Object.prototype.toString;
        var isFunction = function (o) { return toString.call(o) == '[object Function]'; };
        var propName = funcReturnKeyFromItemOrLiteral.toString();
        return array.reduce(function (grouped, item) {
            //var key = funcReturnKeyFromItem(item) ? item.apply(this, [item]) : item[item];
            //var key = isFunction(fieldname) ? fieldname.apply(this, [item]) : item[fieldname];
            //: funcReturnKeyFromItem;
            var key = "";
            if (isFunction(funcReturnKeyFromItemOrLiteral)) {
                key = funcReturnKeyFromItemOrLiteral.apply(_this, [item]);
            }
            else {
                key = item[propName];
            }
            //if (AppTool.IsNullOrEmpty(grouped[key])) {
            //    grouped.push(new Array<any>());
            //} 
            //grouped[key].push(item);
            grouped[key] = grouped[key] || [];
            grouped[key].push(item);
            return grouped; //{USD: Array(2)} or {USD: Array(2) , EUR : Array(1)}
        }, {});
    };
    return ArrayTool;
}());
exports.ArrayTool = ArrayTool;
var FileLoader = /** @class */ (function () {
    function FileLoader() {
    }
    FileLoader.LoadFroalaResources = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            if (_this.IsFroalaFilesLoaded) {
                resolve(true);
            }
            else {
                _this.IsFroalaFilesLoaded = true;
                var i = 0;
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/froala_editor.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/align.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/char_counter.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/code_beautifier.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/code_view.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/colors.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/emoticons.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/entities.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/file.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/font_family.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/font_size.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/fullscreen.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/image.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/image_manager.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/inline_style.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/line_breaker.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/link.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/lists.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/paragraph_format.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/paragraph_style.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/quick_insert.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/quote.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/table.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/save.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/url.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/js/plugins/video.min.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/font-awesome.min.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/froala_editor.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/froala_style.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/code_view.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/colors.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/emoticons.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/image_manager.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/image.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/line_breaker.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/table.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/char_counter.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/video.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/fullscreen.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/quick_insert.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/plugins/file.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Froala/css/codemirror.min.css"));
                _this.ResourcesLoaded.subscribe(function (s) {
                    resolve(true);
                });
                _this.RunLoadingFroalaResources();
                //FileLoader.LoadJS("_Resources/Froala/js/froala_editor.min.js", () => {
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/align.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/char_counter.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/code_beautifier.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/code_view.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/colors.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/emoticons.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/entities.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/file.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/font_family.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/font_size.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/fullscreen.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/image.min.js", () => {
                //        FileLoader.LoadJS("_Resources/Froala/js/plugins/image_manager.min.js");
                //    });
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/inline_style.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/line_breaker.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/link.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/lists.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/paragraph_format.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/paragraph_style.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/quick_insert.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/quote.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/table.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/save.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/url.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/video.min.js");
                //    FileLoader.LoadCSS("_Resources/Froala/css/font-awesome.min.css", () => {
                //        FileLoader.LoadCSS("_Resources/Froala/css/froala_editor.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/froala_style.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/code_view.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/colors.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/emoticons.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/image_manager.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/image.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/line_breaker.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/table.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/char_counter.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/video.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/fullscreen.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/quick_insert.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/file.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/codemirror.min.css");
                //        resolve(true);
                //    });                    
                //});
                //this.LoadJS("Froala/js/plugins/image.min.js", this.OnImageMinLoaded);
                //this.LoadCSS("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.4.0/css/font-awesome.min.css");
            }
        });
    };
    FileLoader.LoadStimulSoftResources = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            if (_this.IsFroalaFilesLoaded) {
                resolve(true);
            }
            else {
                _this.IsFroalaFilesLoaded = true;
                var i = 0;
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/js/jquery.1.8.3.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/js/stimulsoft.reports.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/js/stimulsoft.reports.maps.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/js/stimulsoft.viewer.js"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/js/stimulsoft.designer.js"));
                //this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/js/stimulsoft.designer.runtime.js"));
                //this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.designer.office2013.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.designer.office2013.darkgrayblue.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.designer.runtime.office2013.darkgrayblue.css"));
                _this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.viewer.office2013.darkgrayblue.css"));
                //this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.designer.office2013.lightgrayblue.css"));
                //this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.designer.office2013.lightgrayblue.css"));
                //this.AllFroalaResources.push(new ResourceFile(++i, "_Resources/Stimulsoft/css/stimulsoft.viewer.office2013.darkgrayblue.css"));
                _this.ResourcesLoaded.subscribe(function (s) {
                    resolve(true);
                });
                _this.RunLoadingFroalaResources();
                //FileLoader.LoadJS("_Resources/Froala/js/froala_editor.min.js", () => {
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/align.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/char_counter.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/code_beautifier.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/code_view.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/colors.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/emoticons.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/entities.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/file.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/font_family.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/font_size.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/fullscreen.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/image.min.js", () => {
                //        FileLoader.LoadJS("_Resources/Froala/js/plugins/image_manager.min.js");
                //    });
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/inline_style.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/line_breaker.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/link.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/lists.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/paragraph_format.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/paragraph_style.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/quick_insert.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/quote.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/table.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/save.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/url.min.js");
                //    FileLoader.LoadJS("_Resources/Froala/js/plugins/video.min.js");
                //    FileLoader.LoadCSS("_Resources/Froala/css/font-awesome.min.css", () => {
                //        FileLoader.LoadCSS("_Resources/Froala/css/froala_editor.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/froala_style.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/code_view.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/colors.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/emoticons.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/image_manager.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/image.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/line_breaker.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/table.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/char_counter.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/video.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/fullscreen.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/quick_insert.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/plugins/file.css");
                //        FileLoader.LoadCSS("_Resources/Froala/css/codemirror.min.css");
                //        resolve(true);
                //    });                    
                //});
                //this.LoadJS("Froala/js/plugins/image.min.js", this.OnImageMinLoaded);
                //this.LoadCSS("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.4.0/css/font-awesome.min.css");
            }
        });
    };
    //public static OnImageMinLoaded() {
    //    FileLoader.LoadJS("Froala/js/plugins/image_manager.min.js");
    //}
    FileLoader.LoadJS = function (fileURL, callback) {
        if (callback === void 0) { callback = null; }
        if (fileURL) {
            var script = document.createElement('script');
            script.setAttribute("type", "text/javascript");
            script.setAttribute("src", fileURL);
            if (callback) {
                script.onload = callback;
            }
            document.getElementsByTagName("head")[0].appendChild(script);
        }
    };
    FileLoader.LoadCSS = function (fileURL, callback) {
        if (callback === void 0) { callback = null; }
        if (fileURL) {
            var link = document.createElement("link");
            link.setAttribute("rel", "stylesheet");
            link.setAttribute("type", "text/css");
            link.setAttribute("href", fileURL);
            if (callback) {
                link.onload = callback;
            }
            document.getElementsByTagName("head")[0].appendChild(link);
        }
    };
    FileLoader.RunLoadingFroalaResources = function () {
        var file = this.AllFroalaResources.sort(function (a, b) { return a.Index - b.Index; }).filter(function (f) { return f.IsLoaded == false; })[0];
        if (file) {
            if (file.URL.indexOf(".css") > -1) {
                FileLoader.LoadCSS(file.URL, function () {
                    file.IsLoaded = true;
                    FileLoader.RunLoadingFroalaResources();
                });
            }
            else {
                FileLoader.LoadJS(file.URL, function () {
                    file.IsLoaded = true;
                    FileLoader.RunLoadingFroalaResources();
                });
            }
        }
        else {
            this.ResourcesLoaded.emit(true);
        }
    };
    FileLoader.AllFroalaResources = [];
    FileLoader.IsFroalaFilesLoaded = false;
    FileLoader.ResourcesLoaded = new core_1.EventEmitter();
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], FileLoader, "ResourcesLoaded", void 0);
    return FileLoader;
}());
exports.FileLoader = FileLoader;
var ResourceFile = /** @class */ (function () {
    function ResourceFile(index, url) {
        this.URL = url;
        this.Index = index;
        this.IsLoaded = false;
    }
    return ResourceFile;
}());
////--- itzik : Why Array Tool ? - better extend !!!!
var AmitalList = /** @class */ (function (_super) {
    __extends(AmitalList, _super);
    function AmitalList() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    AmitalList.prototype.Max = function (select) {
        return this.reduce(function (a, b) { return select(a) > select(b) ? a : b; });
    };
    return AmitalList;
}(Array));
////--- itzik : Why Array Tool ? - better extend !!!!
var Pattern = /** @class */ (function () {
    function Pattern() {
    }
    Pattern.Text = 'lol';
    return Pattern;
}());
exports.Pattern = Pattern;
var DateParts = /** @class */ (function () {
    function DateParts() {
        this.Year = 0;
        this.Month = 0;
        this.Day = 0;
        this.Hours = 0;
        this.Hours12 = 0;
        this.Minutes = 0;
        this.Seconds = 0;
        this.Milliseconds = 0;
        this.DateObject = null;
        this.DateTicks = 0;
        this.LocalYear = 0;
        this.LocalMonth = 0;
        this.LocalDay = 0;
        this.TotalMinutes = 0;
    }
    return DateParts;
}());
exports.DateParts = DateParts;
var DateFormats = /** @class */ (function () {
    function DateFormats() {
    }
    return DateFormats;
}());
exports.DateFormats = DateFormats;
var ImageTool = /** @class */ (function () {
    function ImageTool() {
    }
    ImageTool.IsImageExists = function (image_url) {
        var isDisplay = false;
        if (!AppTool.IsNullOrEmpty(image_url)) {
            var imageInfo = this.AllImagesInfo.filter(function (f) { return f.Url.toLowerCase() == image_url.toLowerCase(); })[0];
            if (imageInfo) {
                isDisplay = imageInfo.Exists;
            }
            else {
                var http = new XMLHttpRequest();
                http.open('HEAD', image_url, false);
                http.send();
                isDisplay = http.status == 404 ? false : true;
                http.abort();
                imageInfo = new ImageInfo();
                imageInfo.Url = image_url.toLowerCase();
                imageInfo.Exists = isDisplay;
                this.AllImagesInfo.push(imageInfo);
            }
            if (this.AllImagesInfo.length >= 250) {
                this.AllImagesInfo = [];
            }
        }
        return isDisplay;
    };
    ImageTool.AllImagesInfo = [];
    return ImageTool;
}());
exports.ImageTool = ImageTool;
var ImageInfo = /** @class */ (function () {
    function ImageInfo() {
        this.Url = null;
        this.Exists = null;
    }
    return ImageInfo;
}());
var SortingClass = /** @class */ (function () {
    function SortingClass(item, fieldname) {
        this.SortingValue = 0;
        this.Item = item;
        this.SortingValue = DateTool.GetDateParts(item[fieldname]).DateTicks;
    }
    return SortingClass;
}());
//# sourceMappingURL=Tools.js.map