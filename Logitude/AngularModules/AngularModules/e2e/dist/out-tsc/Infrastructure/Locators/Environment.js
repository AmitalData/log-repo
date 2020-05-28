"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ObjectsLocator_1 = require("./ObjectsLocator");
var Environment = /** @class */ (function () {
    function Environment() {
    }
    //public static LogoCode: string;
    //public static PrivateLabelUrl: string;
    Environment.GetEnvironmentUrl = function () {
        var myResult = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting != null) {
            switch (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LogoCode) {
                case "A.N.G":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = 'http://' + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelUrl;
                        }
                        else {
                            myResult = "https://angularjs.org/";
                        }
                        break;
                    }
                case "C.R.M":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = 'http://' + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelUrl;
                        }
                        else {
                            myResult = "http://www.logitudeworld.com";
                        }
                        break;
                    }
                case "U.N.I":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = 'http://' + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelUrl;
                        }
                        else {
                            myResult = "http://www.amital.co.il";
                        }
                        break;
                    }
                case "L.O.B":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = 'http://' + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelUrl;
                        }
                        else {
                            myResult = "http://www.logbox.co.il";
                        }
                        break;
                    }
                default:
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = 'http://' + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelUrl;
                        }
                        else {
                            myResult = "http://www.logitudeworld.com";
                        }
                        break;
                    }
            }
        }
        return myResult;
    };
    Environment.GetContactUsEmail = function () {
        var myResult = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting != null) {
            switch (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LogoCode) {
                case "U.N.I":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.ContactUsEmail;
                        }
                        else {
                            myResult = "info@amital.co.il";
                        }
                        break;
                    }
                case "L.O.B":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.ContactUsEmail;
                        }
                        else {
                            myResult = "sales@logbox.co.il";
                        }
                        break;
                    }
                default:
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.ContactUsEmail;
                        }
                        else {
                            myResult = "info@logitudeworld.com";
                        }
                        break;
                    }
            }
        }
        return myResult;
    };
    Environment.GetEnvironmentIcon = function () {
        var myResult = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting != null) {
            switch (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LogoCode) {
                case "A.N.G":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = "data:image/JPEG;base64," + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.SmallLogo;
                        }
                        else {
                            myResult = "./Images/ApplicationLogo/Angular/AngularSmallLogo.png";
                        }
                        break;
                    }
                case "C.R.M":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = "data:image/JPEG;base64," + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.SmallLogo;
                        }
                        else {
                            myResult = "./Images/ApplicationLogo/CRMSmallLogo.png";
                        }
                        break;
                    }
                case "U.N.I":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = "data:image/JPEG;base64," + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.SmallLogo;
                        }
                        else {
                            myResult = "./Images/ApplicationLogo/UnifreightSmallLogo.png";
                        }
                        break;
                    }
                case "L.O.B":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = "data:image/JPEG;base64," + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.SmallLogo;
                        }
                        else {
                            myResult = "./Images/ApplicationLogo/LogBox.png";
                        }
                        break;
                    }
                default:
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = "data:image/JPEG;base64," + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.SmallLogo;
                        }
                        else {
                            myResult = "./Images/ApplicationLogo/LogitudeSmallLogo.png";
                        }
                        break;
                    }
            }
        }
        return myResult;
    };
    Environment.GetEnvironmentName = function () {
        var myResult = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting != null) {
            switch (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LogoCode) {
                case "A.N.G":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            myResult = "Angular";
                        }
                        break;
                    }
                case "C.R.M":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            myResult = "CRM";
                        }
                        break;
                    }
                case "U.N.I":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            myResult = "Unifreight";
                        }
                        break;
                    }
                case "L.O.B":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            myResult = "LogBox";
                        }
                        break;
                    }
                default:
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            myResult = "Logitude";
                        }
                        break;
                    }
            }
        }
        return myResult;
    };
    Environment.SetFavIconAndTitle = function () {
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting != null) {
            switch (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LogoCode) {
                case "A.N.G":
                    {
                        break;
                    }
                case "C.R.M":
                    {
                        changeFavicon('./Images/ApplicationLogo/CRMIcon.png');
                        break;
                    }
                case "U.N.I":
                    {
                        changeFavicon('./Images/ApplicationLogo/UnifreightSmallLogo.png');
                        break;
                    }
                case "L.O.B":
                    {
                        if (ObjectsLocator_1.ObjectsLocator.PrivateLableSettings) {
                            changeFavicon("data:image/JPEG;base64," + ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.SmallLogo);
                            changeTitle(ObjectsLocator_1.ObjectsLocator.PrivateLableSettings.PrivateLabelShortName);
                        }
                        else {
                            changeFavicon('./Images/ApplicationLogo/LogBoxIcon.png');
                            changeTitle("LogBox");
                        }
                        break;
                    }
                default:
                    {
                        changeFavicon('./Images/ApplicationLogo/LogitudeSmallLogo.png');
                        changeTitle('Logitude');
                        break;
                    }
            }
        }
    };
    return Environment;
}());
exports.Environment = Environment;
//# sourceMappingURL=Environment.js.map