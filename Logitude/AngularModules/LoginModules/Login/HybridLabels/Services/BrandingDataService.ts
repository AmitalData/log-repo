 import { Headers } from '@angular/http';
import { HybridLabelsBrandingData } from '../DataContracts/HybridLabelsBrandingData';
 
import { HybridLabelsBrandingDataRequest } from '../DataContracts/HybridLabelsBrandingDataRequest';
import { HybridLabelsImage } from '../DataContracts/HybridLabelsImage';

export class BrandingDataService {
     
    public static DefaultBackground: string = "url('../../../Images/LoginScreen/map.png')";
    public static DefaultMainImage: string = "url('../../../Images/LoginScreen/screen_trucks.jpg')";
    public static DefaultLoginProgress: string = "url('../../../Images/LoginScreen/screen_kids.jpg')";
    public static DefaultForgetPassword: string = "url('./Images/LoginScreen/screen_kids.jpg')";
    public static DefaultMainLogo: string = "'../../../Images/LoginScreen/header.jpg'";
    public static DefaultSmallLogo: string = "'../../../Images/LoginScreen/sheader.jpg'"; 

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
        let BackgroundImageId: string = this.GetImageIdFromStorage("BackgroundImage");
        let MainImageId: string = this.GetImageIdFromStorage("MainImage");
        let LoginProgressImageId: string = this.GetImageIdFromStorage("LoginProgressImage");
        let ForgetPasswordImageId: string = this.GetImageIdFromStorage("ForgetPasswordImage"); 
        let MainLogoId: string = this.GetImageIdFromStorage("MainLog"); 
        let SmallLogoId: string = this.GetImageIdFromStorage("SmallLogo"); 


        let BrandingDataRequest: HybridLabelsBrandingDataRequest = new HybridLabelsBrandingDataRequest();
        BrandingDataRequest.BackgroundImageId = BackgroundImageId;
        BrandingDataRequest.MainImageId = MainImageId;
        BrandingDataRequest.LoginProgressImageId = LoginProgressImageId;
        BrandingDataRequest.ForgetPasswordImageId = ForgetPasswordImageId;
        BrandingDataRequest.MainLogoId = MainLogoId;
        BrandingDataRequest.SmallLogoId = SmallLogoId;
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
        this.SetSmallLogo(BrandingData); 
    }

    // Bingind as [src] img 
    private static SetMainLogo(BrandingData: any) {
        if (BrandingData.MainLogo) {
            HybridLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(BrandingData.MainLogo);
            this.StoreImageInStorage("MainLogo", "MainLogo", BrandingData.MainLogo);
        }
        else {
            var StorageMainImage: HybridLabelsImage = BrandingDataService.GetImageFromStorage("MainLogo");
            if (StorageMainImage && StorageMainImage.Id != null && StorageMainImage.Id == BrandingData.MainLogoId) {
                HybridLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(StorageMainImage.Data);
            }
            else {
                HybridLabelsBrandingData.MainLogoURL = this.DefaultMainLogo;
            }
        }

    }

    private static SetSmallLogo(BrandingData: any) {
        if (BrandingData.SmallLogo) {
            HybridLabelsBrandingData.SmallLogoURL = BrandingDataService.GetImageFromBytes(BrandingData.SmallLogo);
            this.StoreImageInStorage("SmallLogo", "SmallLogo", BrandingData.SmallLogo);
        }
        else {
            var StorageSmallLogo: HybridLabelsImage = BrandingDataService.GetImageFromStorage("SmallLogo");
            if (StorageSmallLogo && StorageSmallLogo.Id != null && StorageSmallLogo.Id == BrandingData.SmallLogoId) {
                HybridLabelsBrandingData.SmallLogoURL = BrandingDataService.GetImageFromBytes(StorageSmallLogo.Data);
            }
            else {
                HybridLabelsBrandingData.SmallLogoURL = this.DefaultSmallLogo;
            }
        }

    }

    // Binding as [style.background-image] url
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
                HybridLabelsBrandingData.BackgroundImageURL = this.DefaultBackground;
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
                HybridLabelsBrandingData.MainImageURL = this.DefaultMainImage;
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
                HybridLabelsBrandingData.ForgetPasswordImageURL = this.DefaultForgetPassword;

            }
        }
    }

    private static SetLoginProgressImage(BrandingData: any) {
        if (BrandingData.LoginProgressImageBytes) {
            HybridLabelsBrandingData.LoginProgressImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.LoginProgressImageBytes) + ")";
            this.StoreImageInStorage("LoginProgressImage", BrandingData.LoginProgressImageId, BrandingData.LoginProgressImageBytes);
        }
        else {
            var StorageLoginProgressImage: HybridLabelsImage = BrandingDataService.GetImageFromStorage("LoginProgressImage");
            if (StorageLoginProgressImage && StorageLoginProgressImage.Id != null && StorageLoginProgressImage.Id == BrandingData.LoginProgressImageId) {
                HybridLabelsBrandingData.LoginProgressImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageLoginProgressImage.Data) + ")";
            }
            else { 
                HybridLabelsBrandingData.LoginProgressImageURL = this.DefaultLoginProgress;

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
      

    public static GetLoginProgressImage() {
        if (HybridLabelsBrandingData.LoginProgressImageURL != null)
            return HybridLabelsBrandingData.LoginProgressImageURL;
        else
            HybridLabelsBrandingData.LoginProgressImageURL = this.DefaultLoginProgress;
    }

    public static GetForgetPasswordImage() { 
        if (HybridLabelsBrandingData.ForgetPasswordImageURL != null)
            return HybridLabelsBrandingData.ForgetPasswordImageURL;
        else
            HybridLabelsBrandingData.ForgetPasswordImageURL = this.DefaultForgetPassword;
    }



    public static GetBackgroundImage() { 
        if (HybridLabelsBrandingData.BackgroundImageURL != null)
            return HybridLabelsBrandingData.BackgroundImageURL;
        else
            HybridLabelsBrandingData.BackgroundImageURL = this.DefaultBackground;
    } 

    public static GetMainLogo() { 
        if (HybridLabelsBrandingData.MainLogoURL != null)
            return HybridLabelsBrandingData.MainLogoURL;
        else
            HybridLabelsBrandingData.MainLogoURL = this.DefaultMainLogo;
    }

    public static GetSmallLogo() {
        if (HybridLabelsBrandingData.SmallLogoURL != null)
            return HybridLabelsBrandingData.SmallLogoURL;
        else
            HybridLabelsBrandingData.SmallLogoURL = this.DefaultSmallLogo;
    }

    public static GetMainImage() { 
        if (HybridLabelsBrandingData.MainImageURL != null)
            return HybridLabelsBrandingData.MainImageURL;
        else
            HybridLabelsBrandingData.MainImageURL = this.DefaultMainImage;
    } 

        public static GetHeaders() { 
        var authHeader = new Headers();
        authHeader.append('Access-Control-Allow-Origin', '*'); 
       return authHeader;
    }
}
