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
import { CurrencyRatePM } from 'Infrastructure/EntityPMs/CurrencyRatePM';
@Injectable()

export class DefaultAndConfigurationExtendedService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/defaultandconfigurationextended';
    }

	GetBySetKey(setKey: string) {
		var url = this._apiUrl + '/GetBySetKey?setKey=' + setKey;
	
		return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
			var serviceResponse: ServiceResponse;
			serviceResponse = new ServiceResponse();
			serviceResponse.Result = response;

			return serviceResponse;
		}), catchError(ServiceHelper.HandleServiceError));
	}
}
