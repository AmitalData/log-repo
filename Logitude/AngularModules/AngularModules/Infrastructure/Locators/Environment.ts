import {ObjectsLocator} from './ObjectsLocator';
declare var changeFavicon: any;
declare var changeTitle: any;

export class Environment {
    //public static LogoCode: string;
    //public static PrivateLabelUrl: string;

    public static GetEnvironmentUrl() {
        var myResult: string = null;

        if (ObjectsLocator.GlobalSetting != null) {
            switch (ObjectsLocator.GlobalSetting.LogoCode) {
                case "A.N.G":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = 'http://' + ObjectsLocator.PrivateLableSettings.PrivateLabelUrl;
                        }
                        else {
                            myResult = "https://angularjs.org/";
                        }
                        break;
                    }

                case "C.R.M":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = 'http://' + ObjectsLocator.PrivateLableSettings.PrivateLabelUrl;
                        }
                        else {
                            myResult = "http://www.logitudeworld.com";
                        }
                        break;
                    }

                case "U.N.I":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = 'http://' + ObjectsLocator.PrivateLableSettings.PrivateLabelUrl;
                        }
                        else {
                            myResult = "http://www.amital.co.il";
                        }
                        break;
                    }

                case "L.O.B":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = 'http://' + ObjectsLocator.PrivateLableSettings.PrivateLabelUrl;
                        }
                        else {
                            myResult = "http://www.logbox.co.il";
                        }
                        break;
                    }

                default:
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = 'http://' + ObjectsLocator.PrivateLableSettings.PrivateLabelUrl;
                        }
                        else {
                            myResult = "http://www.logitudeworld.com";
                        }
                        break;
                    }
            }
        }

        return myResult;
    }
    public static GetContactUsEmail() {
        var myResult: string = null;

        if (ObjectsLocator.GlobalSetting != null) {
            switch (ObjectsLocator.GlobalSetting.LogoCode) {

                case "U.N.I":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator.PrivateLableSettings.ContactUsEmail;
                        }
                        else {
                            myResult = "info@amital.co.il";
                        }
                        break;
                    }

                case "L.O.B":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator.PrivateLableSettings.ContactUsEmail;
                        }
                        else {
                            myResult = "sales@logbox.co.il";
                        }
                        break;
                    }

                default:
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator.PrivateLableSettings.ContactUsEmail;
                        }
                        else {
                            myResult = "info@logitudeworld.com";
                        }
                        break;
                    }
            }
        }

        return myResult;
    }
    public static GetEnvironmentIcon() {
        var myResult: string = null;

        if (ObjectsLocator.GlobalSetting != null) {
            switch (ObjectsLocator.GlobalSetting.LogoCode) {
                case "A.N.G":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = "data:image/JPEG;base64," + ObjectsLocator.PrivateLableSettings.SmallLogo;
                        }
                        else {
                            myResult = "./Images/ApplicationLogo/Angular/AngularSmallLogo.png";
                        }

                        break;
                    }

                case "C.R.M":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = "data:image/JPEG;base64," + ObjectsLocator.PrivateLableSettings.SmallLogo;
                        }
                        else {
                            myResult = "./Images/ApplicationLogo/CRMSmallLogo.png";
                        }

                        break;
                    }

                case "U.N.I":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = "data:image/JPEG;base64," + ObjectsLocator.PrivateLableSettings.SmallLogo;
                        }
                        else {
                            myResult = "./Images/ApplicationLogo/UnifreightSmallLogo.png";
                        }
                        break;
                    }

                case "L.O.B":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = "data:image/JPEG;base64," + ObjectsLocator.PrivateLableSettings.SmallLogo;
                        }
                        else {
                            myResult = "./Images/ApplicationLogo/LogBox.png";
                        }
                        break;
                    }

                default:
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = "data:image/JPEG;base64," + ObjectsLocator.PrivateLableSettings.SmallLogo;
                        }
                        else {
                            myResult = "./Images/ApplicationLogo/LogitudeSmallLogo.png";
                        }
                        break;
                    }
            }
        }

        return myResult;
    }
    public static GetEnvironmentName() {
        var myResult: string = null;

        if (ObjectsLocator.GlobalSetting != null) {
            switch (ObjectsLocator.GlobalSetting.LogoCode) {
                case "A.N.G":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            myResult = "Angular";
                        }
                        break;
                    }

                case "C.R.M":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            myResult = "CRM";
                        }
                        break;
                    }

                case "U.N.I":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            myResult = "Unifreight";
                        }

                        break;
                    }

                case "L.O.B":
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            myResult = "LogBox";
                        }
                        break;
                    }

                default:
                    {
                        if (ObjectsLocator.PrivateLableSettings) {
                            myResult = ObjectsLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            myResult = "Logitude";
                        }
                        break;
                    }
            }
        }

        return myResult;
    }
    public static SetFavIconAndTitle() {

        if (ObjectsLocator.GlobalSetting != null) {
            switch (ObjectsLocator.GlobalSetting.LogoCode) {
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
                        if (ObjectsLocator.PrivateLableSettings) {
                            changeFavicon("data:image/JPEG;base64," + ObjectsLocator.PrivateLableSettings.SmallLogo);
                            changeTitle(ObjectsLocator.PrivateLableSettings.PrivateLabelShortName);
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
    }
}