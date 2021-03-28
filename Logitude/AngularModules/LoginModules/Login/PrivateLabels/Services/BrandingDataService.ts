 import { Headers } from '@angular/http';
import { PrivateLabelsBrandingData } from '../DataContracts/PrivateLabelsBrandingData'; 
import { PrivateLabelsBrandingDataRequest } from '../DataContracts/PrivateLabelsBrandingDataRequest';
import { PrivateLabelsImage } from '../DataContracts/PrivateLabelsImage';

export class BrandingDataService {
     
 
     
    
    public static DefaultLoginImage: string = "url('./Images/LoginScreen/screen_trucks.jpg')"; 
    // need static variables for SecondaryColor and MainColor 
    public static DefaultImages = [
        { id: "BackgroundImage", image: "url('./Images/LoginScreen/map.png')" },
        { id: "LoginImage", image: "url('./Images/LoginScreen/header.jpg')"},
        { id: "LoginProgressImage", image: "url('./Images/LoginScreen/header.jpg')" },
        { id: "ForgetPasswordImage", image: "url('./Images/LoginScreen/header.jpg')" },
        { id: "MainLogo", image: "./Images/LoginScreen/header.jpg" },
        { id: "SmallLogo", image: "./Images/LoginScreen/sheader.jpg" },
    ];
 
    public static DefaultBackground: string = "url('./Images/PrivateLabel/Background.png')";
    public static DefaultMainImage: string = "url('./Images/PrivateLabel/MainImage.png')";
    public static DefaultLoginProgress: string = "url('./Images/PrivateLabel/MainImage.png')";
    public static DefaultForgetPassword: string = "url('./Images/PrivateLabel/MainImage.png')";
    public static DefaultMainLogo: string = "./Images/PrivateLabel/LogBoxLogo.png";
    public static DefaultSmallLogo: string = "./Images/PrivateLabel/LogBoxLogo.png";  

    constructor() {

    }
     
    public static GetAppURL(baseUrl: string) {

        if (window.location.origin.indexOf('localhost') > -1)
            return 'http://localhost:9996/';
        else  
            return baseUrl
        
    }

    public static getId() {
        return PrivateLabelsBrandingData.Id;
    }

    public static GetPrivateLabelsDataRequest(baseUrl: string) {
        let BackgroundImageId: string = this.GetImageIdFromStorage("BackgroundImage");
        let LoginImageId: string = this.GetImageIdFromStorage("LoginImage");
        let LoginProgressImageId: string = this.GetImageIdFromStorage("LoginProgressImage");
        let ForgetPasswordImageId: string = this.GetImageIdFromStorage("ForgetPasswordImage"); 
        let MainLogoId: string = this.GetImageIdFromStorage("MainLog"); 
        let SmallLogoId: string = this.GetImageIdFromStorage("SmallLogo"); 


        let BrandingDataRequest: PrivateLabelsBrandingDataRequest = new PrivateLabelsBrandingDataRequest();
        BrandingDataRequest.BackgroundImageId = BackgroundImageId;
        BrandingDataRequest.LoginImageId = LoginImageId;
        BrandingDataRequest.LoginProgressImageId = LoginProgressImageId;
        BrandingDataRequest.ForgetPasswordImageId = ForgetPasswordImageId;
        BrandingDataRequest.MainLogoId = MainLogoId;
        BrandingDataRequest.SmallLogoId = SmallLogoId;
        BrandingDataRequest.PrivateLabelUrl = baseUrl;
        return BrandingDataRequest;
    }


    public static SetPrivateLabelsDataRequest(brandingData: any, baseUrl: string) {

        PrivateLabelsBrandingData.Tenant = brandingData.Tenant;
        PrivateLabelsBrandingData.MainColor = brandingData.MainColor || "#000000"; 
         
        document.documentElement.style.setProperty('--MainColor', PrivateLabelsBrandingData.MainColor);  

        BrandingDataService.SetPrivateLabelsImages(brandingData, baseUrl);
    }

    public static ConvertHexaToRGBA(color: string) {
        if (color) {
            var alpha = parseInt(color.slice(1, 3), 16) / 255;
            return 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + alpha + ')';
        }
    }
    private static SetPrivateLabelsImages(BrandingData: any, baseUrl: string) {
        this.SetBackgroundImage(BrandingData);
        this.SetLoginImage(BrandingData);
        this.SetLoginProgressImage(BrandingData);
        this.SetForgetPasswordImage(BrandingData); 
        this.SetMainLogo(BrandingData); 
        this.SetSmallLogo(BrandingData);  
    }

    // Bingind as [src] img 
    private static SetMainLogo(BrandingData: any) {
        if (BrandingData.MainLogo) {
            PrivateLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(BrandingData.MainLogo);
            this.StoreImageInStorage("MainLogo", "MainLogo", BrandingData.MainLogo);
        }
        else {
            var StorageMainImage: PrivateLabelsImage = BrandingDataService.GetImageFromStorage("MainLogo");
            if (StorageMainImage && StorageMainImage.Id != null && StorageMainImage.Id == BrandingData.MainLogoId) {
                PrivateLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(StorageMainImage.Data);
            }
            else {
                PrivateLabelsBrandingData.MainLogoURL = this.DefaultMainLogo;
            }
        }

    }

    private static SetSmallLogo(BrandingData: any) {
        if (BrandingData.SmallLogo) {
            PrivateLabelsBrandingData.SmallLogoURL = BrandingDataService.GetImageFromBytes(BrandingData.SmallLogo);
            this.StoreImageInStorage("SmallLogo", "SmallLogo", BrandingData.SmallLogo);
        }
        else {
            var StorageSmallLogo: PrivateLabelsImage = BrandingDataService.GetImageFromStorage("SmallLogo");
            if (StorageSmallLogo && StorageSmallLogo.Id != null && StorageSmallLogo.Id == BrandingData.SmallLogoId) {
                PrivateLabelsBrandingData.SmallLogoURL = BrandingDataService.GetImageFromBytes(StorageSmallLogo.Data);
            }
            else {
                PrivateLabelsBrandingData.SmallLogoURL = this.DefaultSmallLogo;
            }
        }

    }

    // Binding as [style.background-image] url
    private static SetBackgroundImage(BrandingData: any) {
        if (BrandingData.BackgroundImageBytes) {
            PrivateLabelsBrandingData.BackgroundImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.BackgroundImageBytes) + ")";
            this.StoreImageInStorage("BackgroundImage", BrandingData.BackgroundImageId, BrandingData.BackgroundImageBytes);
        }
        else {
            var StorageBackgroundImage: PrivateLabelsImage = BrandingDataService.GetImageFromStorage("BackgroundImage");
            if (StorageBackgroundImage && StorageBackgroundImage.Id != null && StorageBackgroundImage.Id == BrandingData.BackgroundImageId) {
                PrivateLabelsBrandingData.BackgroundImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageBackgroundImage.Data) + ")";
            }
            else { 
                PrivateLabelsBrandingData.BackgroundImageURL = this.DefaultBackground;
            }
        }
    }

    private static SetLoginImage(BrandingData) {
        if (BrandingData.LoginImageBytes) {
            PrivateLabelsBrandingData.LoginImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.LoginImageBytes) + ")";
            this.StoreImageInStorage("LoginImage", BrandingData.LoginImageId, BrandingData.LoginImageBytes);
        }
        else {
            var StorageLoginImage: PrivateLabelsImage = BrandingDataService.GetImageFromStorage("LoginImage");
            if (StorageLoginImage && StorageLoginImage.Id != null && StorageLoginImage.Id == BrandingData.LoginImageId) {
                PrivateLabelsBrandingData.LoginImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageLoginImage.Data) + ")";
            }
            else { 
                PrivateLabelsBrandingData.LoginImageURL = this.DefaultLoginImage;
            }
        }
    }

    private static SetForgetPasswordImage(BrandingData: any) {
        if (BrandingData.ForgetPasswordImageBytes) {
            PrivateLabelsBrandingData.ForgetPasswordImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.ForgetPasswordImageBytes) + ")";
            this.StoreImageInStorage("ForgetPasswordImage", BrandingData.ForgetPasswordImageId, BrandingData.ForgetPasswordImageBytes);
        }
        else {
            var StorageForgetPasswordImage: PrivateLabelsImage = BrandingDataService.GetImageFromStorage("ForgetPasswordImage");
            if (StorageForgetPasswordImage && StorageForgetPasswordImage.Id != null && StorageForgetPasswordImage.Id == BrandingData.ForgetPasswordImageId) {
                PrivateLabelsBrandingData.ForgetPasswordImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageForgetPasswordImage.Data) + ")";
            }
            else { 
                PrivateLabelsBrandingData.ForgetPasswordImageURL = this.DefaultForgetPassword;

            }
        }
    }

    private static SetLoginProgressImage(BrandingData: any) {
        if (BrandingData.LoginProgressImageBytes) {
            PrivateLabelsBrandingData.LoginProgressImageURL = "url(" + BrandingDataService.GetImageFromBytes(BrandingData.LoginProgressImageBytes) + ")";
            this.StoreImageInStorage("LoginProgressImage", BrandingData.LoginProgressImageId, BrandingData.LoginProgressImageBytes);
        }
        else {
            var StorageLoginProgressImage: PrivateLabelsImage = BrandingDataService.GetImageFromStorage("LoginProgressImage");
            if (StorageLoginProgressImage && StorageLoginProgressImage.Id != null && StorageLoginProgressImage.Id == BrandingData.LoginProgressImageId) {
                PrivateLabelsBrandingData.LoginProgressImageURL = "url(" + BrandingDataService.GetImageFromBytes(StorageLoginProgressImage.Data) + ")";
            }
            else { 
                PrivateLabelsBrandingData.LoginProgressImageURL = this.DefaultLoginProgress;

            }
        }
    }
     
 
    private static GetImageFromBytes(ImageByte: any) {
        return "data:image/png;base64," + ImageByte;
    }

    private static StoreImageInStorage(ImgStorageKey: string, ImgId: string, ImgData: any) {
        localStorage.setItem(ImgStorageKey, JSON.stringify(new PrivateLabelsImage(ImgId, ImgData)));
    }

    private static GetImageFromStorage(ImgStorageKey: string) {
        return JSON.parse(localStorage.getItem(ImgStorageKey));
    }


    private static GetImageIdFromStorage(ImgStorageKey: string) {
        var ImgId: string = null;
        var StorageImage: PrivateLabelsImage = JSON.parse(localStorage.getItem(ImgStorageKey));
        if (StorageImage && StorageImage.Id) {
            ImgId = StorageImage.Id;
        }
        return ImgId;
    } 
      

    public static GetLoginProgressImage() {
        if (PrivateLabelsBrandingData.LoginProgressImageURL != null)
            return PrivateLabelsBrandingData.LoginProgressImageURL;
        else
            PrivateLabelsBrandingData.LoginProgressImageURL = this.DefaultLoginProgress;
    }

    public static GetForgetPasswordImage() { 
        if (PrivateLabelsBrandingData.ForgetPasswordImageURL != null)
            return PrivateLabelsBrandingData.ForgetPasswordImageURL;
        else
            PrivateLabelsBrandingData.ForgetPasswordImageURL = this.DefaultForgetPassword;
    }

    public static GetBackgroundImageFromStorage() {

        let background: PrivateLabelsImage = BrandingDataService.GetImageFromStorage("BackgroundImage");
        if (background && background.Id != null) {
            PrivateLabelsBrandingData.BackgroundImageURL = "url(" + BrandingDataService.GetImageFromBytes(background.Data) + ")";
        }
        else {
            PrivateLabelsBrandingData.BackgroundImageURL = this.DefaultBackground;

        } 
        return PrivateLabelsBrandingData.BackgroundImageURL;
    }


    public static GetImage(id: string) {
        let imageURL = null;
        let image = BrandingDataService.GetImageFromStorage(id);

        if (image && image.Id != null) { 
            if (id == "MainLogo")
            {
                imageURL = BrandingDataService.GetImageFromBytes(image.Data);
            }
            else {

              imageURL = "url(" + BrandingDataService.GetImageFromBytes(image.Data) + ")"
            }
        } else {
            imageURL = BrandingDataService.DefaultImages.find(x => x.id === id).image;
            console.log("print defualt " + imageURL);
        }
        return imageURL;
    }

    public static GetMainLogoFromStorage() {

       let StorageMainImage: PrivateLabelsImage = BrandingDataService.GetImageFromStorage("MainLogo");
        if (StorageMainImage && StorageMainImage.Id != null) {
            PrivateLabelsBrandingData.MainLogoURL = BrandingDataService.GetImageFromBytes(StorageMainImage.Data);
        }
        else {
            PrivateLabelsBrandingData.MainLogoURL = this.DefaultMainLogo;
        }
        return PrivateLabelsBrandingData.MainLogoURL;
    }


    public static GetLoginProgressFromStorage() {

        let loginProcess: PrivateLabelsImage = BrandingDataService.GetImageFromStorage("LoginProgressImage");
        if (loginProcess && loginProcess.Id != null) {
            PrivateLabelsBrandingData.LoginProgressImage = "url(" + BrandingDataService.GetImageFromBytes(loginProcess.Data) + ")";
        }
        else {
            PrivateLabelsBrandingData.LoginProgressImageURL = this.DefaultLoginProgress;

        }
        return PrivateLabelsBrandingData.LoginProgressImageURL;
    }


    public static GetLoginImageFromStorage() {

        let loginImage: PrivateLabelsImage = BrandingDataService.GetImageFromStorage("LoginImage");
        if (loginImage && loginImage.Id != null) {
            PrivateLabelsBrandingData.LoginImageURL = "url(" + BrandingDataService.GetImageFromBytes(loginImage.Data) + ")";
        }
        else {
            PrivateLabelsBrandingData.LoginImageURL = this.DefaultLoginImage;

        }
        return PrivateLabelsBrandingData.LoginImageURL;
    }



    public static GetBackgroundImage() { 
        if (PrivateLabelsBrandingData.BackgroundImageURL != null)
            return PrivateLabelsBrandingData.BackgroundImageURL;
        else
            PrivateLabelsBrandingData.BackgroundImageURL = this.DefaultBackground;
    } 

    public static GetMainLogo() { 
        if (PrivateLabelsBrandingData.MainLogoURL != null)
            return PrivateLabelsBrandingData.MainLogoURL;
        else
            PrivateLabelsBrandingData.MainLogoURL = this.DefaultMainLogo;
    }

    public static GetSmallLogo() {
        if (PrivateLabelsBrandingData.SmallLogoURL != null)
            return PrivateLabelsBrandingData.SmallLogoURL;
        else
            PrivateLabelsBrandingData.SmallLogoURL = this.DefaultSmallLogo;
    }

    public static GetLoginImage() { 
        if (PrivateLabelsBrandingData.LoginImageURL != null)
            return PrivateLabelsBrandingData.LoginImageURL;
        else
            PrivateLabelsBrandingData.LoginImageURL = this.DefaultLoginImage;
    } 

        public static GetHeaders() { 
        var authHeader = new Headers();
        authHeader.append('Access-Control-Allow-Origin', '*'); 
       return authHeader;
    }
}
