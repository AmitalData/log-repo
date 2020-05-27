"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FeatureLocator_1 = require("../Infrastructure/Utilities/FeatureLocator");
var CRMTool = /** @class */ (function () {
    function CRMTool() {
    }
    CRMTool.IsTicketEditEnabled = function (entityPM) {
        var myResult = true;
        var closedButton = !FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "SaveAsClosed") && !FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "SaveAsResolved") && !FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "SaveAsOpen");
        if (entityPM != null) {
            if (entityPM.IsClosed || entityPM.IsCancelled || closedButton) {
                myResult = false;
            }
        }
        return myResult;
    };
    CRMTool.GetDurationsList = function () {
        var durations = [];
        durations.push(new ActivtyDuration(1, "1 minute", false));
        durations.push(new ActivtyDuration(5, "5 minutes", false));
        durations.push(new ActivtyDuration(15, "15 minutes", false));
        durations.push(new ActivtyDuration(30, "30 minutes", false));
        durations.push(new ActivtyDuration(45, "45 minutes", false));
        durations.push(new ActivtyDuration(60, "1 hour", false));
        durations.push(new ActivtyDuration(90, "1.5 hours", false));
        durations.push(new ActivtyDuration(120, "2 hours", false));
        durations.push(new ActivtyDuration(150, "2.5 hours", false));
        durations.push(new ActivtyDuration(180, "3 hours", false));
        durations.push(new ActivtyDuration(210, "3.5 hours", false));
        durations.push(new ActivtyDuration(240, "4 hours", false));
        durations.push(new ActivtyDuration(270, "4.5 hours", false));
        durations.push(new ActivtyDuration(300, "5 hours", false));
        durations.push(new ActivtyDuration(330, "5.5 hours", false));
        durations.push(new ActivtyDuration(360, "6 hours", false));
        durations.push(new ActivtyDuration(390, "6.5 hours", false));
        durations.push(new ActivtyDuration(420, "7 hours", false));
        durations.push(new ActivtyDuration(450, "7.5 hours", false));
        durations.push(new ActivtyDuration(480, "8 hours", false));
        durations.push(new ActivtyDuration(1440, "1 day", true));
        durations.push(new ActivtyDuration(2880, "2 days", true));
        durations.push(new ActivtyDuration(4320, "3 days", true));
        return durations;
    };
    CRMTool.RoundTimeForwardByMinutes = function (dateTime, minutes) {
        if (dateTime.getUTCMinutes() < 30) {
            dateTime.setUTCMinutes((minutes));
        }
        else if (dateTime.getUTCMinutes() > 30) {
            dateTime.setUTCMinutes(60);
        }
        return dateTime;
    };
    CRMTool.GetActivityImageSrc = function (code) {
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
            case "TX": {
                ImageSrc = "./Images/Buttons/TX.png";
                break;
            }
        }
        return ImageSrc;
    };
    return CRMTool;
}());
exports.CRMTool = CRMTool;
var ActivtyDuration = /** @class */ (function () {
    function ActivtyDuration(minuts, name, isday) {
        this.Minuts = minuts;
        this.Name = name;
        this.IsDay = isday;
    }
    return ActivtyDuration;
}());
exports.ActivtyDuration = ActivtyDuration;
//# sourceMappingURL=Tools.js.map