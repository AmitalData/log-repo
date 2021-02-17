import { Headers } from '@angular/http';
import { HybridLabelsBrandingData } from '../DataContracts/HybridLabelsBrandingData';
import { HybridLabelsBrandingDataRequest } from '../DataContracts/HybridLabelsBrandingDataRequest';
import { HybridLabelsImage } from '../DataContracts/HybridLabelsImage';
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
        return HybridLabelsBrandingData.Id;
    };
    BrandingDataService.GetHybridLabelsDataRequest = function (baseUrl) {
        var BackgroundImageId = this.GetImageIdFromStorage("BackgroundImage");
        var MainImageId = this.GetImageIdFromStorage("MainImage");
        var LoginProgressImageId = this.GetImageIdFromStorage("LoginProgressImage");
        var ForgetPasswordImageId = this.GetImageIdFromStorage("ForgetPasswordImage");
        var MainLogoId = this.GetImageIdFromStorage("MainLog");
        var SmallLogoId = this.GetImageIdFromStorage("SmallLogo");
        var BrandingDataRequest = new HybridLabelsBrandingDataRequest();
        BrandingDataRequest.BackgroundImageId = BackgroundImageId;
        BrandingDataRequest.MainImageId = MainImageId;
        BrandingDataRequest.LoginProgressImageId = LoginProgressImageId;
        BrandingDataRequest.ForgetPasswordImageId = ForgetPasswordImageId;
        BrandingDataRequest.MainLogoId = MainLogoId;
        BrandingDataRequest.SmallLogoId = SmallLogoId;
        BrandingDataRequest.PrivateLabelUrl = baseUrl;
        return BrandingDataRequest;
    };
    BrandingDataService.SetHybridLabelsDataRequest = function (brandingData, baseUrl) {
        HybridLabelsBrandingData.Tenant = brandingData.Tenant;
        HybridLabelsBrandingData.MainColor = brandingData.MainColor || "#000000";
        document.documentElement.style.setProperty('--MainColor', HybridLabelsBrandingData.MainColor);
        BrandingDataService.SetHybridLabelsImages(brandingData, baseUrl);
    };
    BrandingDataService.ConvertHexaToRGBA = function (color) {
        if (color) {
            var alpha = parseInt(color.slice(1, 3), 16) / 255;
            return 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + alpha + ')';
        }
    };
    BrandingDataService.SetHybridLabelsImages = function (BrandingData, baseUrl) {
        this.SetBackgroundImage(BrandingData, baseUrl);
        this.SetMainImage(BrandingData, baseUrl);
        this.SetLoginProgressImage(BrandingData);
        this.SetForgetPasswordImage(BrandingData);
        this.SetMainLogo(BrandingData);
        this.SetSmallLogo(BrandingData);
    };
    // Bingind as [src] img 
    BrandingDataService.SetMainLogo = function (BrandingData) {
        if (BrandingData.MainLogo) {
            HybridLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(BrandingData.MainLogo);
            this.StoreImageInStorage("MainLogo", "MainLogo", BrandingData.MainLogo);
        }
        else {
            var StorageMainImage = BrandingDataService.GetImageFromStorage("MainLogo");
            if (StorageMainImage && StorageMainImage.Id != null && StorageMainImage.Id == BrandingData.MainLogoId) {
                HybridLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(StorageMainImage.Data);
            }
            else {
                HybridLabelsBrandingData.MainLogoURL = this.DefaultMainLogo;
            }
        }
    };
    BrandingDataService.SetSmallLogo = function (BrandingData) {
        if (BrandingData.SmallLogo) {
            HybridLabelsBrandingData.SmallLogoURL = BrandingDataService.GetImageFromBytes(BrandingData.SmallLogo);
            this.StoreImageInStorage("SmallLogo", "SmallLogo", BrandingData.SmallLogo);
        }
        else {
            var StorageSmallLogo = BrandingDataService.GetImageFromStorage("SmallLogo");
            if (StorageSmallLogo && StorageSmallLogo.Id != null && StorageSmallLogo.Id == BrandingData.SmallLogoId) {
                HybridLabelsBrandingData.SmallLogoURL = BrandingDataService.GetImageFromBytes(StorageSmallLogo.Data);
            }
            else {
                HybridLabelsBrandingData.SmallLogoURL = this.DefaultSmallLogo;
            }
        }
    };
    // Binding as [style.background-image] url
    BrandingDataService.SetBackgroundImage = function (BrandingData, baseUrl) {
        if (BrandingData.BackgroundImageBytes) {
            HybridLabelsBrandingData.BackgroundImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.BackgroundImageBytes) + ")";
            this.StoreImageInStorage("BackgroundImage", BrandingData.BackgroundImageId, BrandingData.BackgroundImageBytes);
        }
        else {
            var StorageBackgroundImage = BrandingDataService.GetImageFromStorage("BackgroundImage");
            if (StorageBackgroundImage && StorageBackgroundImage.Id != null && StorageBackgroundImage.Id == BrandingData.BackgroundImageId) {
                HybridLabelsBrandingData.BackgroundImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageBackgroundImage.Data) + ")";
            }
            else {
                HybridLabelsBrandingData.BackgroundImageURL = this.DefaultBackground;
            }
        }
    };
    BrandingDataService.SetMainImage = function (BrandingData, baseUrl) {
        if (BrandingData.MainImageBytes) {
            HybridLabelsBrandingData.MainImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.MainImageBytes) + ")";
            this.StoreImageInStorage("MainImage", BrandingData.MainImageId, BrandingData.MainImageBytes);
        }
        else {
            var StorageMainImage = BrandingDataService.GetImageFromStorage("MainImage");
            if (StorageMainImage && StorageMainImage.Id != null && StorageMainImage.Id == BrandingData.MainImageId) {
                HybridLabelsBrandingData.MainImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageMainImage.Data) + ")";
            }
            else {
                HybridLabelsBrandingData.MainImageURL = this.DefaultMainImage;
            }
        }
    };
    BrandingDataService.SetForgetPasswordImage = function (BrandingData) {
        if (BrandingData.ForgetPasswordImageBytes) {
            HybridLabelsBrandingData.ForgetPasswordImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.ForgetPasswordImageBytes) + ")";
            this.StoreImageInStorage("ForgetPasswordImage", BrandingData.ForgetPasswordImageId, BrandingData.ForgetPasswordImageBytes);
        }
        else {
            var StorageForgetPasswordImage = BrandingDataService.GetImageFromStorage("ForgetPasswordImage");
            if (StorageForgetPasswordImage && StorageForgetPasswordImage.Id != null && StorageForgetPasswordImage.Id == BrandingData.ForgetPasswordImageId) {
                HybridLabelsBrandingData.ForgetPasswordImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageForgetPasswordImage.Data) + ")";
            }
            else {
                HybridLabelsBrandingData.ForgetPasswordImageURL = this.DefaultForgetPassword;
            }
        }
    };
    BrandingDataService.SetLoginProgressImage = function (BrandingData) {
        if (BrandingData.LoginProgressImageBytes) {
            HybridLabelsBrandingData.LoginProgressImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.LoginProgressImageBytes) + ")";
            this.StoreImageInStorage("LoginProgressImage", BrandingData.LoginProgressImageId, BrandingData.LoginProgressImageBytes);
        }
        else {
            var StorageLoginProgressImage = BrandingDataService.GetImageFromStorage("LoginProgressImage");
            if (StorageLoginProgressImage && StorageLoginProgressImage.Id != null && StorageLoginProgressImage.Id == BrandingData.LoginProgressImageId) {
                HybridLabelsBrandingData.LoginProgressImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageLoginProgressImage.Data) + ")";
            }
            else {
                HybridLabelsBrandingData.LoginProgressImageURL = this.DefaultLoginProgress;
            }
        }
    };
    BrandingDataService.GetImageFromBytes = function (ImageByte) {
        return "data:image/png;base64," + ImageByte;
    };
    BrandingDataService.StoreImageInStorage = function (ImgStorageKey, ImgId, ImgData) {
        localStorage.setItem(ImgStorageKey, JSON.stringify(new HybridLabelsImage(ImgId, ImgData)));
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
        if (HybridLabelsBrandingData.LoginProgressImageURL != null)
            return HybridLabelsBrandingData.LoginProgressImageURL;
        else
            HybridLabelsBrandingData.LoginProgressImageURL = this.DefaultLoginProgress;
    };
    BrandingDataService.GetForgetPasswordImage = function () {
        if (HybridLabelsBrandingData.ForgetPasswordImageURL != null)
            return HybridLabelsBrandingData.ForgetPasswordImageURL;
        else
            HybridLabelsBrandingData.ForgetPasswordImageURL = this.DefaultForgetPassword;
    };
    BrandingDataService.GetBackgroundImage = function () {
        if (HybridLabelsBrandingData.BackgroundImageURL != null)
            return HybridLabelsBrandingData.BackgroundImageURL;
        else
            HybridLabelsBrandingData.BackgroundImageURL = this.DefaultBackground;
    };
    BrandingDataService.GetMainLogo = function () {
        if (HybridLabelsBrandingData.MainLogoURL != null)
            return HybridLabelsBrandingData.MainLogoURL;
        else
            HybridLabelsBrandingData.MainLogoURL = this.DefaultMainLogo;
    };
    BrandingDataService.GetSmallLogo = function () {
        if (HybridLabelsBrandingData.SmallLogoURL != null)
            return HybridLabelsBrandingData.SmallLogoURL;
        else
            HybridLabelsBrandingData.SmallLogoURL = this.DefaultSmallLogo;
    };
    BrandingDataService.GetMainImage = function () {
        if (HybridLabelsBrandingData.MainImageURL != null)
            return HybridLabelsBrandingData.MainImageURL;
        else
            HybridLabelsBrandingData.MainImageURL = this.DefaultMainImage;
    };
    BrandingDataService.GetHeaders = function () {
        var authHeader = new Headers();
        authHeader.append('Access-Control-Allow-Origin', '*');
        return authHeader;
    };
    BrandingDataService.DefaultBackground = "url('../../../Images/LoginScreen/map.png')";
    BrandingDataService.DefaultMainImage = "url('../../../Images/LoginScreen/screen_trucks.jpg')";
    BrandingDataService.DefaultLoginProgress = "url('../../../Images/LoginScreen/screen_kids.jpg')";
    BrandingDataService.DefaultForgetPassword = "url('../../../Images/LoginScreen/screen_kids.jpg')";
    BrandingDataService.DefaultMainLogo = "'../../../Images/LoginScreen/header.jpg'";
    BrandingDataService.DefaultSmallLogo = "'../../../Images/LoginScreen/sheader.jpg'";
    return BrandingDataService;
}());
//# sourceMappingURL=BrandingDataService.js.map