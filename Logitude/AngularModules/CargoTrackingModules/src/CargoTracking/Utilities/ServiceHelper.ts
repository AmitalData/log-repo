import { HttpHeaders } from '@angular/common/http';

export  class ServiceHelper{
    public static GetAppURL(){

        if (window.location.origin.indexOf('localhost') > -1)
            return 'http://localhost:9996/';
        else
            return window.location.origin + "/"+ window.location.pathname.split('/')[1] +"/";
    }
    public static GetHeaders(){

        var authHeader = new HttpHeaders();
        // authHeader.append('token', AppLocator.token);
        authHeader.append('Access-Control-Allow-Origin', '*');

        return authHeader;
    }
}
