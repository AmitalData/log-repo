 import { Headers } from '@angular/http';
import { HybridLabelsBrandingData } from '../DataContracts/HybridLabelsBrandingData';
 
import { HybridLabelsBrandingDataRequest } from '../DataContracts/HybridLabelsBrandingDataRequest';
import { HybridLabelsImage } from '../DataContracts/HybridLabelsImage';

export class BrandingDataService {
     

    constructor() {

    }

    public static GetAppURL(baseUrl: string) {

        if (window.location.origin.indexOf('localhost') > -1)
            return 'http://localhost:9996/';
        else  
            return baseUrl
        
    }

    public static getId() {
        return HybridLabelsBrandingData.Id;
    }

    public static GetHybridLabelsDataRequest(baseUrl: string) {
        let BackgroundImageId: string = this.GetImageIdFromStorage("BackgroundImageId");
        let MainImageId: string = this.GetImageIdFromStorage("MainImageId");
        let LoginProgressImageId: string = this.GetImageIdFromStorage("LoginProgressImageId");
        let ForgetPasswordImageId: string = this.GetImageIdFromStorage("ForgetPasswordImageId"); 

        let BrandingDataRequest: HybridLabelsBrandingDataRequest = new HybridLabelsBrandingDataRequest();
        BrandingDataRequest.BackgroundImageId = BackgroundImageId;
        BrandingDataRequest.MainImageId = MainImageId;
        BrandingDataRequest.LoginProgressImageId = LoginProgressImageId;
        BrandingDataRequest.ForgetPasswordImageId = ForgetPasswordImageId;
        BrandingDataRequest.PrivateLabelUrl = baseUrl;
        return BrandingDataRequest;
    }


    public static SetHybridLabelsDataRequest(brandingData: any, baseUrl: string) {

        HybridLabelsBrandingData.Tenant = brandingData.Tenant;
        HybridLabelsBrandingData.MainColor = brandingData.MainColor || "#000000"; 
         
        document.documentElement.style.setProperty('--MainColor', HybridLabelsBrandingData.MainColor);  

        BrandingDataService.SetHybridLabelsImages(brandingData, baseUrl);
    }

    public static ConvertHexaToRGBA(color: string) {
        if (color) {
            var alpha = parseInt(color.slice(1, 3), 16) / 255;
            return 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + alpha + ')';
        }
    }
    private static SetHybridLabelsImages(BrandingData: any, baseUrl: string) {
        this.SetBackgroundImage(BrandingData, baseUrl);
        this.SetMainImage(BrandingData, baseUrl);
        this.SetLoginProgressImage(BrandingData);
        this.SetForgetPasswordImage(BrandingData); 
        this.SetMainLogo(BrandingData); 
    }

    private static SetMainLogo(BrandingData: any) {

        console.log("SetMainLogo" + BrandingData.MainLogo);
        if (BrandingData.MainLogo) {
            HybridLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(BrandingData.MainLogo);
            this.StoreImageInStorage("MainLogo", BrandingData.MainLogoId, BrandingData.MainLogoBytes);
        }
        else {
            var StorageMainImage: HybridLabelsImage = BrandingDataService.GetImageFromStorage("MainLogo");
            if (StorageMainImage && StorageMainImage.Id != null && StorageMainImage.Id == BrandingData.MainLogoId) {
                HybridLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(StorageMainImage.Data);
            }
        }

    }


    private static SetBackgroundImage(BrandingData: any, baseUrl: string) {
        if (BrandingData.BackgroundImageBytes) {
            HybridLabelsBrandingData.BackgroundImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.BackgroundImageBytes) + ")";
            this.StoreImageInStorage("BackgroundImage", BrandingData.BackgroundImageId, BrandingData.BackgroundImageBytes);
        }
        else {
            var StorageBackgroundImage: HybridLabelsImage = BrandingDataService.GetImageFromStorage("BackgroundImage");
            if (StorageBackgroundImage && StorageBackgroundImage.Id != null && StorageBackgroundImage.Id == BrandingData.BackgroundImageId) {
                HybridLabelsBrandingData.BackgroundImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageBackgroundImage.Data) + ")";
            }
            else {
                // Default image? 
                HybridLabelsBrandingData.BackgroundImageURL = "url('" + baseUrl + "../../../Images/LoginScreen/shadow2.png')"

            }
        }
    }

    private static SetMainImage(BrandingData: any, baseUrl: string) {
        if (BrandingData.MainImageBytes) {
            HybridLabelsBrandingData.MainImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.MainImageBytes) + ")";
            this.StoreImageInStorage("MainImage", BrandingData.MainImageId, BrandingData.MainImageBytes);
        }
        else {
            var StorageMainImage: HybridLabelsImage = BrandingDataService.GetImageFromStorage("MainImage");
            if (StorageMainImage && StorageMainImage.Id != null && StorageMainImage.Id == BrandingData.MainImageId) {
                HybridLabelsBrandingData.MainImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageMainImage.Data) + ")";
            }
            else {
                // Default image? 
                HybridLabelsBrandingData.MainImageURL = "url('" + baseUrl + "../../../Images/LoginScreen/shadow2.png')"

            }
        }
    }

    private static SetForgetPasswordImage(BrandingData: any) {
        if (BrandingData.ForgetPasswordImageBytes) {
            HybridLabelsBrandingData.ForgetPasswordImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.ForgetPasswordImageBytes) + ")";
            this.StoreImageInStorage("ForgetPasswordImage", BrandingData.ForgetPasswordImageId, BrandingData.ForgetPasswordImageBytes);
        }
        else {
            var StorageForgetPasswordImage: HybridLabelsImage = BrandingDataService.GetImageFromStorage("ForgetPasswordImage");
            if (StorageForgetPasswordImage && StorageForgetPasswordImage.Id != null && StorageForgetPasswordImage.Id == BrandingData.ForgetPasswordImageId) {
                HybridLabelsBrandingData.ForgetPasswordImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageForgetPasswordImage.Data) + ")";
            }
            else {
                // Default
                HybridLabelsBrandingData.ForgetPasswordImageURL = "url('../../../Images/LoginScreen/shadow2.png')"

            }
        }
    }


    private static SetFSorgetPasswordImage(BrandingData: any) {
        if (BrandingData.ForgetPasswordImageBytes) {
            HybridLabelsBrandingData.ForgetPasswordImageURL = BrandingDataService.GetImageFromBytes(BrandingData.ForgetPasswordImageBytes);
            this.StoreImageInStorage("ForgetPasswordImage", BrandingData.InvertedLogoId, BrandingData.ForgetPasswordImageBytes);
        }
        else {
            var StorageForgetPasswordImage: HybridLabelsImage = BrandingDataService.GetImageFromStorage("ForgetPasswordImage");
            if (StorageForgetPasswordImage && StorageForgetPasswordImage.Id != null && StorageForgetPasswordImage.Id == BrandingData.ForgetPasswordImage) {
                HybridLabelsBrandingData.ForgetPasswordImageURL = BrandingDataService.GetImageFromBytes(StorageForgetPasswordImage.Data);
            }
        }
    }

    private static SetLoginProgressImage(BrandingData: any) {
        if (BrandingData.LoginProgressImageBytes) {
            HybridLabelsBrandingData.LoginProgressImageURL = BrandingDataService.GetImageFromBytes(BrandingData.ShipmentHeaderBytes);
            this.StoreImageInStorage("ShipmentHeaderImage", BrandingData.ShipmentHeaderImageId, BrandingData.ShipmentHeaderBytes);
        }
        else {
            var StorageLoginProgressImage: HybridLabelsImage = BrandingDataService.GetImageFromStorage("LoginProgressImage");
            if (StorageLoginProgressImage && StorageLoginProgressImage.Id != null && StorageLoginProgressImage.Id == BrandingData.LoginProgressImageId) {
                HybridLabelsBrandingData.LoginProgressImageURL = BrandingDataService.GetImageFromBytes(StorageLoginProgressImage.Data);
            }
        }
    }
 
 
    private static GetImageFromBytes(ImageByte: any) {
        return "data:image/png;base64," + ImageByte;
    }

    private static StoreImageInStorage(ImgStorageKey: string, ImgId: string, ImgData: any) {
        localStorage.setItem(ImgStorageKey, JSON.stringify(new HybridLabelsImage(ImgId, ImgData)));
    }

    private static GetImageFromStorage(ImgStorageKey: string) {
        return JSON.parse(localStorage.getItem(ImgStorageKey));
    }


    private static GetImageIdFromStorage(ImgStorageKey: string) {
        var ImgId: string = null;
        var StorageImage: HybridLabelsImage = JSON.parse(localStorage.getItem(ImgStorageKey));
        if (StorageImage && StorageImage.Id) {
            ImgId = StorageImage.Id;
        }
        return ImgId;
    }

     

    public static GetForgetPasswordImage() { 
        if (HybridLabelsBrandingData.ForgetPasswordImageURL != null)
            return HybridLabelsBrandingData.ForgetPasswordImageURL;
        else
            HybridLabelsBrandingData.ForgetPasswordImageURL = "url('../../../Images/LoginScreen/shadow2.png')"
    }



    public static GetBackgroundImage() { 
        if (HybridLabelsBrandingData.BackgroundImageURL != null) 
            return HybridLabelsBrandingData.BackgroundImageURL;
        else
            HybridLabelsBrandingData.BackgroundImageURL = "url('../../../Images/LoginScreen/shadow2.png')"
    }



    public static GetABackgroundImage() {  

        if (HybridLabelsBrandingData.Tenant && HybridLabelsBrandingData.BackgroundImageURL != null) { 

            return HybridLabelsBrandingData.BackgroundImageURL; 
        }
        else 
            HybridLabelsBrandingData.BackgroundImageURL = "url('../../../Images/LoginScreen/shadow2.png')"
    }

    public static GetMainLogo() { 
        if (HybridLabelsBrandingData.MainImageURL != null)
            return HybridLabelsBrandingData.MainLogoURL;
        else 
            HybridLabelsBrandingData.MainImageURL = "url('../../../Images/LoginScreen/shadow2.png')"
    }


    public static GetMainImage() {
        console.log("main Image  : " + HybridLabelsBrandingData.MainImageURL);
        if (HybridLabelsBrandingData.MainImageURL != null)
            return HybridLabelsBrandingData.MainImageURL;
        else 
            HybridLabelsBrandingData.MainImageURL = "url('../../../Images/LoginScreen/shadow2.png')"
    }
     

        public static GetHeaders() { 
        var authHeader = new Headers();
        authHeader.append('Access-Control-Allow-Origin', '*'); 
       return authHeader;
    }
}
