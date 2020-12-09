import { HttpHeaders } from '@angular/common/http';
import { CargoTrackingBrandingData } from '../DataContracts/CargoTrackingBrandingData';

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

    public static GetCurrentDomain(baseUrl:string){
        // if(baseUrl.includes('/CargoTracking')){
        //     baseUrl = baseUrl.replace("/CargoTracking","");
        // }
        return baseUrl 
    }
     
    public static SetCargoTrackingDate(brandingData:any,baseUrl:string){

        CargoTrackingBrandingData.Tenant = brandingData.Tenant;
        CargoTrackingBrandingData.MainColor = brandingData.MainColor != null ? this.ConvertHexaToRGBA(brandingData.MainColor) :"#000000";
        CargoTrackingBrandingData.SecondaryColor = brandingData.SecondaryColor ? this.ConvertHexaToRGBA(brandingData.SecondaryColor) : "#002664";
        document.documentElement.style.setProperty('--BGColor', CargoTrackingBrandingData.MainColor);
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
        this.SetBackGroundImg(BrandingData,baseUrl);
        this.SetComapnyLogo(BrandingData,baseUrl);
        this.SetBrowserIcon(BrandingData,baseUrl);
    }
    private static SetBackGroundImg(BrandingData:any,baseUrl:string)
    {
        if(BrandingData.BackgroundURL){
            CargoTrackingBrandingData.BackgroundURL = "url("+ ServiceHelper.GetAppURL(baseUrl)+BrandingData.BackgroundURL+")";
        }
        else{
            CargoTrackingBrandingData.BackgroundURL ="url('"+baseUrl+"assets/images/misc/map-bg.svg')"
        } 
    }

    private static SetComapnyLogo(BrandingData:any,baseUrl:string)
    {
        if(BrandingData.ComapnylogoURL){
            CargoTrackingBrandingData.ComapnylogoURL =   ServiceHelper.GetAppURL(baseUrl)+BrandingData.ComapnylogoURL;
        }
    }

    private static SetBrowserIcon(BrandingData:any,baseUrl:string)
    {
        if(BrandingData.BrowserIconURL){
            CargoTrackingBrandingData.BrowserIconURL =   ServiceHelper.GetAppURL(baseUrl)+BrandingData.BrowserIconURL ;
            ServiceHelper.favIcon.href =CargoTrackingBrandingData.BrowserIconURL;
        }
    }
    public static GetHeaders(){

        var authHeader = new HttpHeaders();
        // authHeader.append('token', AppLocator.token);
        authHeader.append('Access-Control-Allow-Origin', '*');

        return authHeader;
    }
}
