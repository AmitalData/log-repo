import {Injectable} from '@angular/core';
import { HttpClient,HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';

import { defer, of } from 'rxjs';
import { ClassLevelValidator } from 'Infrastructure/Validators/ClassLevelValidator';
import { RatesTablePMService } from 'Infrastructure/Services/StandardPMs/RatesTablePMService';
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { RatesTablePM } from 'Infrastructure/EntityPMs/RatesTablePM';
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';
import { StringIterator } from 'cypress/types/lodash';
@Injectable()

export class RatesTableExtendedService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ratestablescustom';
    }


    UpdateRate(entityPM: RatesTablePM) {
 
		var callTime = new Date();  
		var ratesTablePMService;
		return defer(() => {

			var serviceResponse: ServiceResponse = new ServiceResponse();
			var validator: ClassLevelValidator = new ClassLevelValidator();                
			var errorsArray = validator.Validate("RatesTable", entityPM);


			if (errorsArray.length == 0) {
                ratesTablePMService = new RatesTablePMService();
				var mappedEntity: RatesTablePM = ratesTablePMService.MapJsonToEntityPM(entityPM, false);
				
				return this._http.post(this._apiUrl+"/UpdateRate", JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
					.pipe(
						map((response: HttpResponse<any>) => {

							var pm = response.body;
							if (pm) {
								var mappedResult: RatesTablePM = ratesTablePMService.MapJsonToEntityPM(pm, true, entityPM);
								serviceResponse.Result = mappedResult;
							}						

							var servertime = response.headers.get('ServerExecutionTime');
							PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "RatesTable", "SaveChanges", "");                    
												                             
							return serviceResponse;
						}),

						catchError(ServiceHelper.HandleServiceError));
			}

			else {
				serviceResponse.HasError = true;
				serviceResponse.ErrorsArray = errorsArray;
				return of(serviceResponse);
			}
		});
	}

	GetLastUpdateByCurrencyCode(foreignCurrency:string,tenantCurrencyId:string){
		var url = this._apiUrl + '/GetLastUpdateByCurrencyCode?' + 'foreignCurrency=' + foreignCurrency + '&tenantCurrencyId=' + tenantCurrencyId;
	
		return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
			var serviceResponse: ServiceResponse;
			serviceResponse = new ServiceResponse();
			serviceResponse.Result = response;

			return serviceResponse;
		}), catchError(ServiceHelper.HandleServiceError));
	}

}

