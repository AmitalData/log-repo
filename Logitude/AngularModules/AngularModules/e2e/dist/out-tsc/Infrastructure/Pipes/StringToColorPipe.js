"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../Tools");
var StringToColorPipe = /** @class */ (function () {
    function StringToColorPipe() {
    }
    StringToColorPipe.prototype.transform = function (input, Parameter) {
        if (Parameter === void 0) { Parameter = null; }
        var myResult = "#282E30";
        if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
            var value = input + "";
            if (!Tools_1.AppTool.IsNullOrEmpty(Parameter)) {
                myResult = this.ApplyParameterPipe(value, Parameter);
            }
            else {
                myResult = this.ApplyPipe(value);
            }
        }
        return myResult;
    };
    StringToColorPipe.prototype.ApplyPipe = function (value) {
        var color = "#282E30";
        switch (value) {
            case "Draft":
                {
                    color = "Orange";
                    break;
                }
            case "Created":
            case "Waiting For Approval":
            case "Approval Canceled":
            case "Not Sent":
            case "Partially Sent":
            case "Not Transferred":
                {
                    // Black
                    color = "#282E30";
                    break;
                }
            case "Void":
                {
                    color = "#6E7172";
                    break;
                }
            case "Booking Request Rejected":
            case "Cancellation Request Rejected":
            case "Declined":
            case "Error":
                {
                    // Red
                    color = "#E53030";
                    break;
                }
            case "Air waybill":
            case "Received by Airline":
            case "Waiting for Confirmation":
            case "Accepted":
            case "Hybriding":
            case "Booking Request":
            case "Used":
                {
                    // Green
                    color = "#009161";
                    break;
                }
            case "Confirmed":
            case "Booking Confirmed":
            case "Departed":
            case "Arrived":
            case "Sent":
            case "Transferred":
            case "In Progress":
                {
                    color = "#27AAE1";
                    break;
                }
            case "Printed":
            case "Pick Up":
            case "On Hand":
                {
                    color = "#F37021";
                    break;
                }
            case "Paid":
            case "Cleared":
            case "Delivery":
            case "Delivered":
                {
                    color = "#2BB673";
                    break;
                }
            case "Approved":
            case "Unpaid":
            case "Approved By Customer":
                {
                    color = "#8DC63F";
                    break;
                }
            case "Auto Credit":
            case "Auto Credited":
            case "Sent To Customer":
                {
                    color = "Orange";
                    break;
                }
            case "Viewed":
                {
                    color = "#00D377";
                    break;
                }
            case "Not Connected":
                {
                    color = "Gray";
                    break;
                }
            case "Connected":
                {
                    color = "Blue";
                    break;
                }
            case "InActive":
            case "Customs Delay":
            case "Transferred With Errors":
                {
                    color = "Red";
                    break;
                }
            case "Waiting":
                {
                    color = "Orange";
                    break;
                }
            case "Past": {
                color = "red";
                break;
            }
            case "Present": {
                color = "Green";
                break;
            }
            case "Future": {
                color = "Magenta";
                break;
            }
            case "Potential": {
                color = "orange";
                break;
            }
            case "Active": {
                color = "green";
                break;
            }
            case "Waiting for Activation": {
                color = "red";
                break;
            }
            case "Inactive": {
                color = "pink";
                break;
            }
        }
        return color;
    };
    StringToColorPipe.prototype.ApplyParameterPipe = function (value, Parameter) {
        var myResult = "#282E30";
        if (Parameter == "CustomerStatusCode") {
            myResult = this.ApplyCustomerStatusCodePipe(value);
        }
        else if (Parameter == "TicketSeverityCode") {
            myResult = this.ApplyTicketSeverityCodePipe(value);
        }
        else if (Parameter == "QuoteRatingCode") {
            myResult = this.ApplyQuoteRatingCodePipe(value);
        }
        else if (Parameter == "ActivityPriorityCode") {
            myResult = this.ActivityPriorityCodePipe(value);
        }
        else if (Parameter == "OpportunityRatingCode") {
            myResult = this.OpportunityRatingCodePipe(value);
        }
        else if (Parameter == "OpportunityStage") {
            myResult = this.OpportunityStagePipe(value);
        }
        else if (Parameter == "RatingCodeToImage") {
            myResult = this.RatingCodeToImage(value);
        }
        else if (Parameter == "UpdateDateOpportunity") {
            myResult = this.UpdateDateOpportunity(value);
        }
        else if (Parameter == "Number" || Parameter == "number") {
            myResult = this.ApplyNumberPipe(value);
        }
        else if (Parameter == "ActivityDueDate") {
            myResult = this.ActivityDueDatePipe(value);
        }
        else if (Parameter == "DeclarationStatusTypeCode") {
            myResult = this.ApplyDeclarationStatusCodePipe(value);
        }
        else if (Parameter == "CustomsTransmissionsStatus") {
            myResult = this.ApplyCustomsTransmissionsStatusPipe(value);
        }
        else if (Parameter == "CourierCustomStatusCode") {
            myResult = this.ApplyCourierCustomStatusPipe(value);
        }
        else {
            switch (Parameter + ":" + value) {
                case "MessagingStock:New":
                case "MessagingStock:Active":
                    {
                        myResult = "#009161";
                        break;
                    }
            }
        }
        return myResult;
    };
    StringToColorPipe.prototype.ApplyCustomerStatusCodePipe = function (value) {
        var myResult = "#282E30";
        switch (value) {
            case "POT":
                {
                    myResult = "#E36C0A";
                    break;
                }
            case "ACT":
                {
                    myResult = "#00B076";
                    break;
                }
            case "WAC":
                {
                    myResult = "#FF0000";
                    break;
                }
            case "INA":
                {
                    myResult = "#F40CB2";
                    break;
                }
        }
        return myResult;
    };
    StringToColorPipe.prototype.ApplyDeclarationStatusCodePipe = function (value) {
        var myResult = "";
        switch (value) {
            case "3":
            case "7":
            case "8":
            case "13":
                {
                    myResult = "#009161"; // green
                    break;
                }
            case "4":
            case "5":
            case "6":
            case "10":
            case "11":
            case "15":
            case "21":
                {
                    myResult = "#F37021"; // orange
                    break;
                }
            case "0":
            case "1":
            case "2":
            case "9":
            case "12":
            case "14":
            case "16":
            case "17":
            case "18":
            case "19":
            case "20":
                {
                    myResult = "#E53030"; // red
                    break;
                }
        }
        return myResult;
    };
    StringToColorPipe.prototype.ApplyTicketSeverityCodePipe = function (value) {
        var myResult = "#282E30";
        switch (value) {
            case "UR":
                {
                    myResult = "Red";
                    break;
                }
            case "HI":
                {
                    myResult = "Orange";
                    break;
                }
            case "MD":
                {
                    myResult = "Gray";
                    break;
                }
            case "LW":
                {
                    myResult = "Blue";
                    break;
                }
        }
        return myResult;
    };
    StringToColorPipe.prototype.ApplyQuoteRatingCodePipe = function (value) {
        var myResult = "#282E30";
        switch (value) {
            case "C":
                {
                    myResult = "#2772D0";
                    break;
                }
            case "H":
                {
                    myResult = "#CE1111";
                    break;
                }
            case "N":
                {
                    myResult = "Gray";
                    break;
                }
            case "W":
                {
                    myResult = "#FF893B";
                    break;
                }
        }
        return myResult;
    };
    StringToColorPipe.prototype.ApplyNumberPipe = function (value) {
        var myResult = "#282E30";
        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
            var myValue = 0;
            if (typeof (value) == "string") {
                value = value.replace(',', '');
                if (Tools_1.FormatTool.IsDecimal(value)) {
                    myValue = +value;
                }
            }
            if (myValue < 0) {
                myResult = Tools_1.FontTool.Red;
            }
            else if (myValue > 0) {
                myResult = Tools_1.FontTool.Green;
            }
        }
        return myResult;
    };
    StringToColorPipe.prototype.ActivityPriorityCodePipe = function (value) {
        var myResult = "#282E30";
        switch (value) {
            case "01":
                {
                    //Low
                    myResult = "#2772D0";
                    break;
                }
            case "02":
                {
                    //Normal
                    myResult = "#D1D1D1";
                    break;
                }
            case "03":
                {
                    //High
                    myResult = "#CE1111";
                    break;
                }
        }
        return myResult;
    };
    StringToColorPipe.prototype.OpportunityRatingCodePipe = function (value) {
        var myResult = "#282E30";
        switch (value) {
            case "H":
                {
                    myResult = "#CE1111";
                    break;
                }
            case "C":
                {
                    myResult = "#2772D0";
                    break;
                }
            case "W":
                {
                    myResult = "#FF893B";
                    break;
                }
            case "N":
                {
                    myResult = "#D1D1D1";
                    break;
                }
        }
        return myResult;
    };
    StringToColorPipe.prototype.OpportunityStagePipe = function (value) {
        var myResult = "#E483FB";
        if (value == "Qualification") {
            myResult = "#5F3255";
        }
        else if (value == "Approved") {
            myResult = "#574600";
        }
        else if (value == "Development") {
            myResult = "#505AD2";
        }
        else if (value == "Quote") {
            myResult = "#E400FF";
        }
        else if (value == "Closed Won") {
            myResult = "#49AE2C";
        }
        else if (value == "Closed Lost") {
            myResult = "#FF0000";
        }
        else if (value == "Draft") {
            myResult = "Orange";
        }
        return myResult;
    };
    StringToColorPipe.prototype.UpdateDateOpportunity = function (value) {
        var myResult = "Red";
        if (value == "Today") {
            myResult = "Green";
        }
        return myResult;
    };
    StringToColorPipe.prototype.RatingCodeToImage = function (value) {
        var myResult = "#E483FB";
        if (value == "Hot") {
            myResult = "red";
        }
        else if (value == "Cold") {
            myResult = "blue";
        }
        else if (value == "Warm") {
            myResult = "Orange";
        }
        else if (value == "Neutral") {
            myResult = "lightgray";
        }
        return myResult;
    };
    StringToColorPipe.prototype.ActivityDueDatePipe = function (value) {
        var myresult = "rgb(110,113,114)";
        if (value != null) {
            if (Tools_1.DateTool.GetDateParts(value).DateObject < Tools_1.DateTool.GetCurrentDateAsUtc()) {
                myresult = "red";
            }
        }
        return myresult;
    };
    StringToColorPipe.prototype.ApplyCustomsTransmissionsStatusPipe = function (value) {
        var myResult = "black";
        switch (value) {
            case "SENT":
            case "ACPT":
                {
                    myResult = "Green";
                    break;
                }
            case "EROR":
                {
                    myResult = "Red";
                    break;
                }
            case "NSEN":
                {
                    myResult = "Orange";
                    break;
                }
        }
        return myResult;
    };
    StringToColorPipe.prototype.ApplyCourierCustomStatusPipe = function (value) {
        var myResult = "#282E30";
        switch (value) {
            case "1": //Hatara
                {
                    myResult = "#00B076";
                    break;
                }
            case "2": //Suspended
                {
                    myResult = "#FF0000";
                    break;
                }
        }
        return myResult;
    };
    StringToColorPipe = __decorate([
        core_1.Pipe({ name: 'StringToColorPipe' })
    ], StringToColorPipe);
    return StringToColorPipe;
}());
exports.StringToColorPipe = StringToColorPipe;
//# sourceMappingURL=StringToColorPipe.js.map