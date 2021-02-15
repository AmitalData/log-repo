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
    BrandingDataService.GetHybridLabelsDataRequest = function (baseUrl) {
        var BackgroundImageId = this.GetImageIdFromStorage("BackgroundImageId");
        var MainImageId = this.GetImageIdFromStorage("MainImageId");
        var LoginProgressImageId = this.GetImageIdFromStorage("LoginProgressImageId");
        var ForgetPasswordImageId = this.GetImageIdFromStorage("ForgetPasswordImageId");
        var BrandingDataRequest = new HybridLabelsBrandingDataRequest();
        BrandingDataRequest.BackgroundImageId = BackgroundImageId;
        BrandingDataRequest.MainImageId = MainImageId;
        BrandingDataRequest.LoginProgressImageId = LoginProgressImageId;
        BrandingDataRequest.ForgetPasswordImageId = ForgetPasswordImageId;
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
    };
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
                // Default image? 
                HybridLabelsBrandingData.BackgroundImageURL = "url('" + baseUrl + "../../../Images/LoginScreen/shadow2.png')";
            }
        }
    };
    BrandingDataService.SetMainImage = function (BrandingData, baseUrl) {
        if (BrandingData.MainImageBytes) {
            HybridLabelsBrandingData.MainImageURL = BrandingDataService.GetImageFromBytes(BrandingData.MainImageBytes);
            this.StoreImageInStorage("MainImage", BrandingData.MainImageId, BrandingData.MainImageBytes);
        }
        else {
            var StorageMainImage = BrandingDataService.GetImageFromStorage("MainImage");
            if (StorageMainImage && StorageMainImage.Id != null && StorageMainImage.Id == BrandingData.MainImageId) {
                HybridLabelsBrandingData.MainImageURL = BrandingDataService.GetImageFromBytes(StorageMainImage.Data);
            }
        }
    };
    BrandingDataService.SetLoginProgressImage = function (BrandingData) {
        if (BrandingData.LoginProgressImageBytes) {
            HybridLabelsBrandingData.LoginProgressImageURL = BrandingDataService.GetImageFromBytes(BrandingData.ShipmentHeaderBytes);
            this.StoreImageInStorage("ShipmentHeaderImage", BrandingData.ShipmentHeaderImageId, BrandingData.ShipmentHeaderBytes);
        }
        else {
            var StorageLoginProgressImage = BrandingDataService.GetImageFromStorage("LoginProgressImage");
            if (StorageLoginProgressImage && StorageLoginProgressImage.Id != null && StorageLoginProgressImage.Id == BrandingData.LoginProgressImageId) {
                HybridLabelsBrandingData.LoginProgressImageURL = BrandingDataService.GetImageFromBytes(StorageLoginProgressImage.Data);
            }
        }
    };
    BrandingDataService.SetForgetPasswordImage = function (BrandingData) {
        if (BrandingData.ForgetPasswordImageBytes) {
            HybridLabelsBrandingData.ForgetPasswordImageURL = BrandingDataService.GetImageFromBytes(BrandingData.ForgetPasswordImageBytes);
            this.StoreImageInStorage("ForgetPasswordImage", BrandingData.InvertedLogoId, BrandingData.ForgetPasswordImageBytes);
        }
        else {
            var StorageForgetPasswordImage = BrandingDataService.GetImageFromStorage("ForgetPasswordImage");
            if (StorageForgetPasswordImage && StorageForgetPasswordImage.Id != null && StorageForgetPasswordImage.Id == BrandingData.InvertedLogoId) {
                HybridLabelsBrandingData.ForgetPasswordImageURL = BrandingDataService.GetImageFromBytes(StorageForgetPasswordImage.Data);
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
    BrandingDataService.GetBackgroundImage = function () {
        if (HybridLabelsBrandingData.Tenant && HybridLabelsBrandingData.BackgroundImageURL != null)
            return HybridLabelsBrandingData.BackgroundImageURL;
        else
            // Default image? 
            HybridLabelsBrandingData.BackgroundImageURL = "url('../../../Images/LoginScreen/shadow2.png')";
    };
    BrandingDataService.GetMainImage = function () {
        if (HybridLabelsBrandingData.Tenant && HybridLabelsBrandingData.MainImageURL != null)
            return HybridLabelsBrandingData.MainImageURL;
        else
            // Default image? 
            HybridLabelsBrandingData.MainImageURL = "url('../../../Images/LoginScreen/shadow2.png')";
    };
    BrandingDataService.GetHeaders = function () {
        var authHeader = new Headers();
        authHeader.append('Access-Control-Allow-Origin', '*');
        return authHeader;
    };
    return BrandingDataService;
}());
//# sourceMappingURL=BrandingDataService.js.map