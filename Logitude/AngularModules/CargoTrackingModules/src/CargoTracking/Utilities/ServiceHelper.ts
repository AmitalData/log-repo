import { HttpHeaders } from '@angular/common/http';

export  class ServiceHelper{

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
     
    public static ConvertHexaToRGBA(color: string)
    {
        if (color) {
            var alpha = parseInt(color.slice(1, 3), 16) / 255;
            return 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + alpha + ')';
        }
    }    

    public static GetHeaders(){

        var authHeader = new HttpHeaders();
        // authHeader.append('token', AppLocator.token);
        authHeader.append('Access-Control-Allow-Origin', '*');

        return authHeader;
    }
}
