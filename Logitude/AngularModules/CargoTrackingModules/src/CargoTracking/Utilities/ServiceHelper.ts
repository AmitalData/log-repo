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
    public static GetHeaders(){

        var authHeader = new HttpHeaders();
        // authHeader.append('token', AppLocator.token);
        authHeader.append('Access-Control-Allow-Origin', '*');

        return authHeader;
    }
}
