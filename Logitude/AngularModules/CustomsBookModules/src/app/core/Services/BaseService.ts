import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceHelper } from '../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../DataContracts/ServiceResponse';

@Injectable({
	providedIn: 'root',
})
export class BaseService {
	public _HttpClient: HttpClient;
	// public BaseURL: string;
	// public ApiURL!: string;
	// public HttpHeaders: any;
	constructor(public HttpClient: HttpClient) {
		// this.HttpClient = ServiceHelper.HttpClient;
		// this.BaseURL = ServiceHelper.GetLogitudeURL();
		// this.HttpHeaders = ServiceHelper.GetHttpHeaders();
	}

	GetServiceResponse(response: any) {
		var output = new ServiceResponse();
		output.Result = response;
		return output;
	}

	Get(url: string) {
		return this.HttpClient.get(url).pipe(
			map((response) => {
				return response;
			}),
		);
	}
}
