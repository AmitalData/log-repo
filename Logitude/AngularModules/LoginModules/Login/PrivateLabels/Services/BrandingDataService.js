import { Headers } from '@angular/http';
import { PrivateLabelsBrandingData } from '../DataContracts/PrivateLabelsBrandingData';
import { PrivateLabelsBrandingDataRequest } from '../DataContracts/PrivateLabelsBrandingDataRequest';
import { PrivateLabelsImage } from '../DataContracts/PrivateLabelsImage';
export var BrandingDataService = (function () {
    function BrandingDataService() {
    }
    BrandingDataService.GetAppURL = function (baseUrl) {
        if (window.location.origin.indexOf('localhost') > -1)
            return 'http://localhost:9996/';
        else
            return baseUrl;
    };
    BrandingDataService.getId = function () {
        return PrivateLabelsBrandingData.Id;
    };
    BrandingDataService.GetPrivateLabelsDataRequest = function (baseUrl) {
        var BackgroundImageId = this.GetImageIdFromStorage("BackgroundImage");
        var MainImageId = this.GetImageIdFromStorage("MainImage");
        var LoginProgressImageId = this.GetImageIdFromStorage("LoginProgressImage");
        var ForgetPasswordImageId = this.GetImageIdFromStorage("ForgetPasswordImage");
        var MainLogoId = this.GetImageIdFromStorage("MainLog");
        var SmallLogoId = this.GetImageIdFromStorage("SmallLogo");
        var BrandingDataRequest = new PrivateLabelsBrandingDataRequest();
        BrandingDataRequest.BackgroundImageId = BackgroundImageId;
        BrandingDataRequest.MainImageId = MainImageId;
        BrandingDataRequest.LoginProgressImageId = LoginProgressImageId;
        BrandingDataRequest.ForgetPasswordImageId = ForgetPasswordImageId;
        BrandingDataRequest.MainLogoId = MainLogoId;
        BrandingDataRequest.SmallLogoId = SmallLogoId;
        BrandingDataRequest.PrivateLabelUrl = baseUrl;
        return BrandingDataRequest;
    };
    BrandingDataService.SetPrivateLabelsDataRequest = function (brandingData, baseUrl) {
        PrivateLabelsBrandingData.Tenant = brandingData.Tenant;
        PrivateLabelsBrandingData.MainColor = brandingData.MainColor || "#000000";
        document.documentElement.style.setProperty('--MainColor', PrivateLabelsBrandingData.MainColor);
        BrandingDataService.SetPrivateLabelsImages(brandingData, baseUrl);
    };
    BrandingDataService.ConvertHexaToRGBA = function (color) {
        if (color) {
            var alpha = parseInt(color.slice(1, 3), 16) / 255;
            return 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + alpha + ')';
        }
    };
    BrandingDataService.SetPrivateLabelsImages = function (BrandingData, baseUrl) {
        this.SetBackgroundImage(BrandingData);
        this.SetMainImage(BrandingData);
        this.SetLoginProgressImage(BrandingData);
        this.SetForgetPasswordImage(BrandingData);
        this.SetMainLogo(BrandingData);
        this.SetSmallLogo(BrandingData);
    };
    // Bingind as [src] img 
    BrandingDataService.SetMainLogo = function (BrandingData) {
        if (BrandingData.MainLogo) {
            PrivateLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(BrandingData.MainLogo);
            this.StoreImageInStorage("MainLogo", "MainLogo", BrandingData.MainLogo);
        }
        else {
            var StorageMainImage = BrandingDataService.GetImageFromStorage("MainLogo");
            if (StorageMainImage && StorageMainImage.Id != null && StorageMainImage.Id == BrandingData.MainLogoId) {
                PrivateLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(StorageMainImage.Data);
            }
            else {
                PrivateLabelsBrandingData.MainLogoURL = this.DefaultMainLogo;
            }
        }
    };
    BrandingDataService.SetSmallLogo = function (BrandingData) {
        if (BrandingData.SmallLogo) {
            PrivateLabelsBrandingData.SmallLogoURL = BrandingDataService.GetImageFromBytes(BrandingData.SmallLogo);
            this.StoreImageInStorage("SmallLogo", "SmallLogo", BrandingData.SmallLogo);
        }
        else {
            var StorageSmallLogo = BrandingDataService.GetImageFromStorage("SmallLogo");
            if (StorageSmallLogo && StorageSmallLogo.Id != null && StorageSmallLogo.Id == BrandingData.SmallLogoId) {
                PrivateLabelsBrandingData.SmallLogoURL = BrandingDataService.GetImageFromBytes(StorageSmallLogo.Data);
            }
            else {
                PrivateLabelsBrandingData.SmallLogoURL = this.DefaultSmallLogo;
            }
        }
    };
    // Binding as [style.background-image] url
    BrandingDataService.SetBackgroundImage = function (BrandingData) {
        if (BrandingData.BackgroundImageBytes) {
            PrivateLabelsBrandingData.BackgroundImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.BackgroundImageBytes) + ")";
            this.StoreImageInStorage("BackgroundImage", BrandingData.BackgroundImageId, BrandingData.BackgroundImageBytes);
        }
        else {
            var StorageBackgroundImage = BrandingDataService.GetImageFromStorage("BackgroundImage");
            if (StorageBackgroundImage && StorageBackgroundImage.Id != null && StorageBackgroundImage.Id == BrandingData.BackgroundImageId) {
                PrivateLabelsBrandingData.BackgroundImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageBackgroundImage.Data) + ")";
            }
            else {
                PrivateLabelsBrandingData.BackgroundImageURL = this.DefaultBackground;
            }
        }
    };
    BrandingDataService.SetMainImage = function (BrandingData) {
        if (BrandingData.MainImageBytes) {
            PrivateLabelsBrandingData.MainImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.MainImageBytes) + ")";
            this.StoreImageInStorage("MainImage", BrandingData.MainImageId, BrandingData.MainImageBytes);
        }
        else {
            var StorageMainImage = BrandingDataService.GetImageFromStorage("MainImage");
            if (StorageMainImage && StorageMainImage.Id != null && StorageMainImage.Id == BrandingData.MainImageId) {
                PrivateLabelsBrandingData.MainImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageMainImage.Data) + ")";
            }
            else {
                PrivateLabelsBrandingData.MainImageURL = this.DefaultMainImage;
            }
        }
    };
    BrandingDataService.SetForgetPasswordImage = function (BrandingData) {
        if (BrandingData.ForgetPasswordImageBytes) {
            PrivateLabelsBrandingData.ForgetPasswordImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.ForgetPasswordImageBytes) + ")";
            this.StoreImageInStorage("ForgetPasswordImage", BrandingData.ForgetPasswordImageId, BrandingData.ForgetPasswordImageBytes);
        }
        else {
            var StorageForgetPasswordImage = BrandingDataService.GetImageFromStorage("ForgetPasswordImage");
            if (StorageForgetPasswordImage && StorageForgetPasswordImage.Id != null && StorageForgetPasswordImage.Id == BrandingData.ForgetPasswordImageId) {
                PrivateLabelsBrandingData.ForgetPasswordImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageForgetPasswordImage.Data) + ")";
            }
            else {
                PrivateLabelsBrandingData.ForgetPasswordImageURL = this.DefaultForgetPassword;
            }
        }
    };
    BrandingDataService.SetLoginProgressImage = function (BrandingData) {
        if (BrandingData.LoginProgressImageBytes) {
            PrivateLabelsBrandingData.LoginProgressImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.LoginProgressImageBytes) + ")";
            this.StoreImageInStorage("LoginProgressImage", BrandingData.LoginProgressImageId, BrandingData.LoginProgressImageBytes);
        }
        else {
            var StorageLoginProgressImage = BrandingDataService.GetImageFromStorage("LoginProgressImage");
            if (StorageLoginProgressImage && StorageLoginProgressImage.Id != null && StorageLoginProgressImage.Id == BrandingData.LoginProgressImageId) {
                PrivateLabelsBrandingData.LoginProgressImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageLoginProgressImage.Data) + ")";
            }
            else {
                PrivateLabelsBrandingData.LoginProgressImageURL = this.DefaultLoginProgress;
            }
        }
    };
    BrandingDataService.GetImageFromBytes = function (ImageByte) {
        return "data:image/png;base64," + ImageByte;
    };
    BrandingDataService.StoreImageInStorage = function (ImgStorageKey, ImgId, ImgData) {
        localStorage.setItem(ImgStorageKey, JSON.stringify(new PrivateLabelsImage(ImgId, ImgData)));
    };
    BrandingDataService.GetImageFromStorage = function (ImgStorageKey) {
        return JSON.parse(localStorage.getItem(ImgStorageKey));
    };
    BrandingDataService.GetImageIdFromStorage = function (ImgStorageKey) {
        var ImgId = null;
        var StorageImage = JSON.parse(localStorage.getItem(ImgStorageKey));
        if (StorageImage && StorageImage.Id) {
            ImgId = StorageImage.Id;
        }
        return ImgId;
    };
    BrandingDataService.GetLoginProgressImage = function () {
        if (PrivateLabelsBrandingData.LoginProgressImageURL != null)
            return PrivateLabelsBrandingData.LoginProgressImageURL;
        else
            PrivateLabelsBrandingData.LoginProgressImageURL = this.DefaultLoginProgress;
    };
    BrandingDataService.GetForgetPasswordImage = function () {
        if (PrivateLabelsBrandingData.ForgetPasswordImageURL != null)
            return PrivateLabelsBrandingData.ForgetPasswordImageURL;
        else
            PrivateLabelsBrandingData.ForgetPasswordImageURL = this.DefaultForgetPassword;
    };
    BrandingDataService.GetBackgroundImageFromStorage = function () {
        var background = BrandingDataService.GetImageFromStorage("BackgroundImage");
        if (background && background.Id != null) {
            PrivateLabelsBrandingData.BackgroundImageURL = "url(" + BrandingDataService.GetImageFromBytes(background.Data) + ")";
        }
        else {
            PrivateLabelsBrandingData.BackgroundImageURL = this.DefaultBackground;
        }
        return PrivateLabelsBrandingData.BackgroundImageURL;
    };
    BrandingDataService.GetMainLogoFromStorage = function () {
        var StorageMainImage = BrandingDataService.GetImageFromStorage("MainLogo");
        if (StorageMainImage && StorageMainImage.Id != null) {
            PrivateLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(StorageMainImage.Data);
        }
        else {
            PrivateLabelsBrandingData.MainLogoURL = this.DefaultMainLogo;
        }
        return PrivateLabelsBrandingData.MainLogoURL;
    };
    BrandingDataService.GetLoginProgressFromStorage = function () {
        var loginProcess = BrandingDataService.GetImageFromStorage("LoginProgressImage");
        if (loginProcess && loginProcess.Id != null) {
            PrivateLabelsBrandingData.LoginProgressImage = "url(" + BrandingDataService.GetImageFromBytes(loginProcess.Data) + ")";
        }
        else {
            PrivateLabelsBrandingData.LoginProgressImageURL = this.DefaultLoginProgress;
        }
        return PrivateLabelsBrandingData.LoginProgressImageURL;
    };
    BrandingDataService.GetBackgroundImage = function () {
        if (PrivateLabelsBrandingData.BackgroundImageURL != null)
            return PrivateLabelsBrandingData.BackgroundImageURL;
        else
            PrivateLabelsBrandingData.BackgroundImageURL = this.DefaultBackground;
    };
    BrandingDataService.GetMainLogo = function () {
        if (PrivateLabelsBrandingData.MainLogoURL != null)
            return PrivateLabelsBrandingData.MainLogoURL;
        else
            PrivateLabelsBrandingData.MainLogoURL = this.DefaultMainLogo;
    };
    BrandingDataService.GetSmallLogo = function () {
        if (PrivateLabelsBrandingData.SmallLogoURL != null)
            return PrivateLabelsBrandingData.SmallLogoURL;
        else
            PrivateLabelsBrandingData.SmallLogoURL = this.DefaultSmallLogo;
    };
    BrandingDataService.GetMainImage = function () {
        if (PrivateLabelsBrandingData.MainImageURL != null)
            return PrivateLabelsBrandingData.MainImageURL;
        else
            PrivateLabelsBrandingData.MainImageURL = this.DefaultMainImage;
    };
    BrandingDataService.GetHeaders = function () {
        var authHeader = new Headers();
        authHeader.append('Access-Control-Allow-Origin', '*');
        return authHeader;
    };
    BrandingDataService.DefaultBackground = "url('./Images/LoginScreen/map.png')";
    BrandingDataService.DefaultMainImage = "url('./Images/LoginScreen/screen_trucks.jpg')";
    BrandingDataService.DefaultLoginProgress = "url('./Images/LoginScreen/screen_kids.jpg')";
    BrandingDataService.DefaultForgetPassword = "url('./Images/LoginScreen/screen_kids.jpg')";
    BrandingDataService.DefaultMainLogo = "./Images/LoginScreen/header.jpg";
    BrandingDataService.DefaultSmallLogo = "./Images/LoginScreen/sheader.jpg";
    return BrandingDataService;
}());
//# sourceMappingURL=BrandingDataService.js.map