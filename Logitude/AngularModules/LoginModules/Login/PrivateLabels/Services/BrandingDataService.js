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
        var LoginImageId = this.GetImageIdFromStorage("LoginImage");
        var LoginProgressImageId = this.GetImageIdFromStorage("LoginProgressImage");
        var ForgetPasswordImageId = this.GetImageIdFromStorage("ForgetPasswordImage");
        var MainLogoId = this.GetImageIdFromStorage("MainLog");
        var SmallLogoId = this.GetImageIdFromStorage("SmallLogo");
        var BrandingDataRequest = new PrivateLabelsBrandingDataRequest();
        BrandingDataRequest.BackgroundImageId = BackgroundImageId;
        BrandingDataRequest.LoginImageId = LoginImageId;
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
        PrivateLabelsBrandingData.SecondaryColor = brandingData.SecondaryColor || "#000000";
        document.documentElement.style.setProperty('--MainColor', PrivateLabelsBrandingData.MainColor);
        document.documentElement.style.setProperty('--SecondaryColor', PrivateLabelsBrandingData.SecondaryColor);
        this.MainColor = PrivateLabelsBrandingData.MainColor;
        this.SecondaryColor = PrivateLabelsBrandingData.SecondaryColor;
        BrandingDataService.SetPrivateLabelsImages(brandingData, baseUrl);
        BrandingDataService.SetPrivateLabelsColors(brandingData);
    };
    BrandingDataService.ConvertHexaToRGBA = function (color) {
        if (color) {
            var alpha = parseInt(color.slice(1, 3), 16) / 255;
            return 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + alpha + ')';
        }
    };
    BrandingDataService.SetPrivateLabelsColors = function (BrandingData) {
        this.SetMainColor(BrandingData);
        this.SetSecondaryColor(BrandingData);
    };
    BrandingDataService.SetMainColor = function (BrandingData) {
        if (BrandingData.MainColor) {
            this.StoreImageInStorage("MainColor", "MainColor", BrandingData.MainColor);
        }
    };
    BrandingDataService.SetSecondaryColor = function (BrandingData) {
        if (BrandingData.SecondaryColor) {
            this.StoreImageInStorage("SecondaryColor", "SecondaryColor", BrandingData.SecondaryColor);
        }
    };
    BrandingDataService.GetColor = function (ColorStorageKey) {
        var color = JSON.parse(localStorage.getItem(ColorStorageKey));
        if (color && color.Id != null) {
            return color.Data;
        }
        else {
            return color;
        }
    };
    BrandingDataService.SetPrivateLabelsImages = function (BrandingData, baseUrl) {
        this.SetBackgroundImage(BrandingData);
        this.SetLoginImage(BrandingData);
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
        }
    };
    BrandingDataService.SetLoginImage = function (BrandingData) {
        if (BrandingData.LoginImageBytes) {
            PrivateLabelsBrandingData.LoginImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.LoginImageBytes) + ")";
            this.StoreImageInStorage("LoginImage", BrandingData.LoginImageId, BrandingData.LoginImageBytes);
        }
        else {
            var StorageLoginImage = BrandingDataService.GetImageFromStorage("LoginImage");
            if (StorageLoginImage && StorageLoginImage.Id != null && StorageLoginImage.Id == BrandingData.LoginImageId) {
                PrivateLabelsBrandingData.LoginImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageLoginImage.Data) + ")";
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
    BrandingDataService.GetImage = function (id) {
        var imageURL = null;
        var image = BrandingDataService.GetImageFromStorage(id);
        if (image && image.Id != null) {
            if (id == "MainLogo" || id == "SmallLogo") {
                imageURL = BrandingDataService.GetImageFromBytes(image.Data);
            }
            else {
                imageURL = "url(" + BrandingDataService.GetImageFromBytes(image.Data) + ")";
            }
        }
        //else {
        //    imageURL = BrandingDataService.DefaultImages.find(x => x.id === id).image; 
        // }
        return imageURL;
    };
    BrandingDataService.GetDefaultImage = function (imageId) {
        return BrandingDataService.DefaultImages.find(function (x) { return x.id === imageId; }).image;
    };
    BrandingDataService.GetHeaders = function () {
        var authHeader = new Headers();
        authHeader.append('Access-Control-Allow-Origin', '*');
        return authHeader;
    };
    BrandingDataService.DefaultImages = [
        { id: "BackgroundImage", image: "url('./Images/PrivateLabel/Background.png')" },
        { id: "LoginImage", image: "url('./Images/PrivateLabel/MainImage.png')" },
        { id: "LoginProgressImage", image: "url('./Images/PrivateLabel/MainImage.png')" },
        { id: "ForgetPasswordImage", image: "url('./Images/PrivateLabel/MainImage.png')" },
        { id: "MainLogo", image: "./Images/PrivateLabel/LogBoxLogo.png" },
        { id: "SmallLogo", image: "./Images/PrivateLabel/LogBoxLogo.png" },
    ];
    BrandingDataService.MainColor = null;
    BrandingDataService.SecondaryColor = null;
    return BrandingDataService;
}());
//# sourceMappingURL=BrandingDataService.js.map