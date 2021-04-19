import { HttpHeaders } from '@angular/common/http';
import { CargoTrackingBrandingData } from '../DataContracts/CargoTrackingBrandingData';
import { CargoTrackingBrandingDataRequest } from '../DataContracts/CargoTrackingBrandingDataRequest';
import { CargoTrackingImage } from '../DataContracts/CargoTrackingImage';

export  class ServiceHelper{


   public static favIcon: HTMLLinkElement = document.querySelector('#appIcon');

    constructor(){

    }
    public static GetAppURL(baseUrl:string){

        if (window.location.origin.indexOf('localhost') > -1)
            return 'http://localhost:9996/';
        else{
            if(baseUrl.includes('/CargoTracking')){
                baseUrl = baseUrl.replace("/CargoTracking","");
            }
            return baseUrl
        }

    }

    public static GetcargoTrackingDataRequest(baseUrl:string)
    {   var BackgroundId:string = this.GetImageIdFromStorage("BackgroundImg");
        var CompanyLogoId:string = this.GetImageIdFromStorage("CompanyLogoImg");
        var InvertedLogoId:string = this.GetImageIdFromStorage("InvertedLogoImg");
        var BrowserIconId:string = this.GetImageIdFromStorage("BrowserIconImg");
        var ShipmentHeaderImageId:string = this.GetImageIdFromStorage("ShipmentHeaderImage");

        var BrandingDataRequest:CargoTrackingBrandingDataRequest = new CargoTrackingBrandingDataRequest();
        BrandingDataRequest.BackgroundId = BackgroundId;
        BrandingDataRequest.ComapnylogoId = CompanyLogoId;
        BrandingDataRequest.InvertedLogoId = InvertedLogoId;
        BrandingDataRequest.BrowserIconId = BrowserIconId;
        BrandingDataRequest.ShipmentHeaderImageId = ShipmentHeaderImageId;

        BrandingDataRequest.Domain = baseUrl;
        return BrandingDataRequest;
    }


    public static SetCargoTrackingDate(brandingData:any,baseUrl:string){

        CargoTrackingBrandingData.Tenant = brandingData.Tenant;
        CargoTrackingBrandingData.MainColor = brandingData.MainColor || "#000000";
        CargoTrackingBrandingData.SecondaryColor = brandingData.SecondaryColor || "#002664";

        // document.documentElement.style.setProperty('--BGColor', CargoTrackingBrandingData.MainColor || 'RGB(250,251,252)');
        document.documentElement.style.setProperty('--MainColor', CargoTrackingBrandingData.MainColor);
        document.documentElement.style.setProperty('--busyIndicatorColor', CargoTrackingBrandingData.MainColor);
        document.documentElement.style.setProperty('--secondaryColor', CargoTrackingBrandingData.SecondaryColor);

        ServiceHelper.SetCarogTrackingImages(brandingData,baseUrl);
    }

    public static ConvertHexaToRGBA(color: string)
    {
        if (color) {
            var alpha = parseInt(color.slice(1, 3), 16) / 255;
            return 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + alpha + ')';
        }
    }
    private static SetCarogTrackingImages(BrandingData:any,baseUrl:string)
    {
        this.SetBackgroundImg(BrandingData,baseUrl);
        this.SetCompanyLogo(BrandingData,baseUrl);
        this.SetBrowserIcon(BrandingData);
        this.SetShipmentHeaderImage(BrandingData);
        this.SetInvertedLogo(BrandingData);
    }

    private static SetBackgroundImg(BrandingData:any,baseUrl:string)
    {
        if(BrandingData.BackgroundBytes){
            CargoTrackingBrandingData.BackgroundURL = "url("+ServiceHelper.GetImageFromBytes(BrandingData.BackgroundBytes)+")";
            this.StoreImageInStorage("BackgroundImg",BrandingData.BackgroundId,BrandingData.BackgroundBytes);
        }
        else{
            var StorageBackground:CargoTrackingImage = ServiceHelper.GetImageFromStorage("BackgroundImg");
                if(StorageBackground && StorageBackground.Id!=null && StorageBackground.Id == BrandingData.BackgroundId){
                    CargoTrackingBrandingData.BackgroundURL ="url("+ServiceHelper.GetImageFromBytes(StorageBackground.Data)+")";
                }
                else{
                    CargoTrackingBrandingData.BackgroundURL ="url('"+baseUrl+"assets/images/misc/map-bg.svg')"
                }
        }
    }

    private static SetCompanyLogo(BrandingData:any,baseUrl:string)
    {
        if(BrandingData.ComapnylogoBytes){
            CargoTrackingBrandingData.ComapnylogoURL = ServiceHelper.GetImageFromBytes(BrandingData.ComapnylogoBytes);
            this.StoreImageInStorage("CompanyLogoImg",BrandingData.ComapnylogoId,BrandingData.ComapnylogoBytes);
        }
        else{
            var StorageCompanyLogo:CargoTrackingImage = ServiceHelper.GetImageFromStorage("CompanyLogoImg");
                if(StorageCompanyLogo && StorageCompanyLogo.Id!=null && StorageCompanyLogo.Id == BrandingData.ComapnylogoId){
                    CargoTrackingBrandingData.ComapnylogoURL =ServiceHelper.GetImageFromBytes(StorageCompanyLogo.Data);
                }
        }
    }

    private static SetShipmentHeaderImage(BrandingData:any)
    {
        if(BrandingData.ShipmentHeaderBytes){
            CargoTrackingBrandingData.ShipmentHeaderURL = ServiceHelper.GetImageFromBytes(BrandingData.ShipmentHeaderBytes);
            this.StoreImageInStorage("ShipmentHeaderImage",BrandingData.ShipmentHeaderImageId,BrandingData.ShipmentHeaderBytes);
        }
        else{
            var StorageShipmentHeaderImage:CargoTrackingImage = ServiceHelper.GetImageFromStorage("ShipmentHeaderImage");
                if(StorageShipmentHeaderImage && StorageShipmentHeaderImage.Id!=null && StorageShipmentHeaderImage.Id == BrandingData.ShipmentHeaderImageId){
                    CargoTrackingBrandingData.ShipmentHeaderURL =ServiceHelper.GetImageFromBytes(StorageShipmentHeaderImage.Data);
                }
        }
    }

    private static SetInvertedLogo(BrandingData:any)
    {
        if(BrandingData.InvertedLogoBytes){
            CargoTrackingBrandingData.InvertedLogoURL = ServiceHelper.GetImageFromBytes(BrandingData.InvertedLogoBytes);
            this.StoreImageInStorage("InvertedLogoImg",BrandingData.InvertedLogoId,BrandingData.InvertedLogoBytes);
        }
        else{
            var StorageInvertedLogo:CargoTrackingImage = ServiceHelper.GetImageFromStorage("InvertedLogoImg");
                if(StorageInvertedLogo && StorageInvertedLogo.Id!=null && StorageInvertedLogo.Id == BrandingData.InvertedLogoId){
                    CargoTrackingBrandingData.InvertedLogoURL =ServiceHelper.GetImageFromBytes(StorageInvertedLogo.Data);
                }
        }
    }

    private static SetBrowserIcon(BrandingData:any)
    {
        if(BrandingData.BrowserIconBytes){
            CargoTrackingBrandingData.BrowserIconURL =ServiceHelper.GetImageFromBytes(BrandingData.BrowserIconBytes);
            ServiceHelper.favIcon.href =CargoTrackingBrandingData.BrowserIconURL;
            this.StoreImageInStorage("BrowserIconImg",BrandingData.BrowserIconId,BrandingData.BrowserIconBytes);
        }
        else{
            var StorageBrowserIcon:CargoTrackingImage = ServiceHelper.GetImageFromStorage("BrowserIconImg");
                if(StorageBrowserIcon && StorageBrowserIcon.Id!=null && StorageBrowserIcon.Id == BrandingData.BrowserIconId){
                    CargoTrackingBrandingData.BrowserIconURL =ServiceHelper.GetImageFromBytes(StorageBrowserIcon.Data);
                    ServiceHelper.favIcon.href =CargoTrackingBrandingData.BrowserIconURL;
                }
        }
    }

    private static GetImageFromBytes(ImageByte:any){
        return "data:image/png;base64,"+ImageByte;
    }

    private static StoreImageInStorage(ImgStorageKey:string,ImgId:string,ImgData:any){
        localStorage.setItem(ImgStorageKey,JSON.stringify(new CargoTrackingImage(ImgId,ImgData)));
    }

    private static GetImageFromStorage(ImgStorageKey:string){
        return JSON.parse(localStorage.getItem(ImgStorageKey));
    }


    private static GetImageIdFromStorage(ImgStorageKey:string){
        var ImgId:string=null;
        var StorageImage:CargoTrackingImage = JSON.parse(localStorage.getItem(ImgStorageKey));
        if(StorageImage && StorageImage.Id){
            ImgId=StorageImage.Id;
        }
        return ImgId;
    }

    public static GetHeaders(){

        var authHeader = new HttpHeaders();
        authHeader.append('Access-Control-Allow-Origin', '*');

        return authHeader;
    }
}
