import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ServiceHelper } from "../../../Infrastructure/Utilities/ServiceHelper";
import { defer, Observable } from 'rxjs';
import { ServiceResponse } from "../../../Infrastructure/DataContracts/ServiceResponse";
import { SessionInfo } from "../../../Infrastructure/Utilities/SessionInfo";
import { catchError, map } from "rxjs/operators";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";

@Injectable()

export class NewQuoteOPWebService {
    private _http: HttpClient
    private _apiUrl: string;
    
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/NewQuoteOPWebService';
    }

    /*example :-  var service = new DeclarationWebService();  PTERMID=code
    service.GetETBPAYTRtemList(this.PTERMID , this.costSearchText, 30, true).subscribe((res: ServiceResponse) => {
    });*/

    GetIncoterm(filters: ApiQueryFilters) {
        let urlparameters: string = `/Incoterms/?`;
        urlparameters = this.addFilterToUri(filters, urlparameters);
        let callUrl: string = this._apiUrl.concat(urlparameters);
        
        return this.sendGetHttp(callUrl);      
    }

    GetCarriers(DIRECTIONID: string, TRANSPORTMODEID: string, filters: ApiQueryFilters) {
        let urlparameters: string = `/Carriers/?DIRECTIONID=${DIRECTIONID}&TRANSPORTMODEID=${TRANSPORTMODEID}`;
        urlparameters = this.addFilterToUri(filters, urlparameters);
        let callUrl: string = this._apiUrl.concat(urlparameters);
        
        return this.sendGetHttp(callUrl);      
    }

    GetSpecialService(DIRECTIONID: string, TRANSPORTMODEID: string, filters: ApiQueryFilters) {
        let urlparameters: string = `/SpecialServices/?DIRECTIONID=${DIRECTIONID}&TRANSPORTMODEID=${TRANSPORTMODEID}`;
        urlparameters = this.addFilterToUri(filters, urlparameters);
        let callUrl: string = this._apiUrl.concat(urlparameters);
        
        return this.sendGetHttp(callUrl);      
    }
    
    GetPorts(DIRECTIONID: string, TRANSPORTMODEID: string, filters: ApiQueryFilters) {
        let urlparameters: string = `/ports/?DIRECTIONID=${DIRECTIONID}&TRANSPORTMODEID=${TRANSPORTMODEID}`;
        urlparameters = this.addFilterToUri(filters, urlparameters);
        let callUrl: string = this._apiUrl.concat(urlparameters);

        return this.sendGetHttp(callUrl);
    }

    private sendGetHttp(callUrl: string) {



        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return this._http.get(callUrl, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map(response => {

                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = response;
                        return serviceResponse;
                    }),

                    catchError(ServiceHelper.HandleServiceError));
        });
    }

    private addFilterToUri(filters: ApiQueryFilters, urlparameters: string): string {
        const mykeys: string[] = Object.keys(filters);
        let addtionalFiltersValues: string = '';

        for (var i in mykeys) {
            const propName: string = mykeys[i];
            let propValue = filters[propName];
            const ignoreFilter: boolean = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") 
                urlparameters += '&';            

            if (!ignoreFilter)
                urlparameters += propName + '=' +  encodeURIComponent(propValue);

            if (propName == "AdditionalFilters" && propValue.length > 0) 
                addtionalFiltersValues = JSON.stringify(propValue);            
        }

        if (addtionalFiltersValues) 
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        
        return urlparameters;
    }
}
