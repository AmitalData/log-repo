import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';





export class InterfaceTenantDefinitionsWebService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
           this._http = ServiceHelper.HttpClient;
           this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InterfaceTenantDefinitionWebService';      
       }
   
       get(tenant: number,code: string) {       
   
   
           return defer(() => {
               return this._http.get(this._apiUrl + '/GetSingleByTenantCode?' + 'tenant=' + tenant + '&code=' + code, ServiceHelper.GetHttpFullHeaders())
                   .pipe(
                       map((response: HttpResponse<any>) => {

                        var res = response;
                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = res;
                   
                        return serviceResponse; 
   
                       }),
                       
                       catchError(ServiceHelper.HandleServiceError)); 
           });                    
       }


     
  
    }