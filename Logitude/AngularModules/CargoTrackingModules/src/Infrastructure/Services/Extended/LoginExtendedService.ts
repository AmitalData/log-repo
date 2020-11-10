import { ServiceHelper } from '../../../CargoTracking/Utilities/ServiceHelper';
import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';

@Injectable()
export class LoginExtendedService {
	private _apiUrl: string;
	constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
		this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/Authentication';
	}

	PostUserValidation(loginParameters: LoginParameters) {
		var authHeaders = ServiceHelper.GetHeaders();

		return defer(() => {
			return this._http.post(this._apiUrl, loginParameters, { headers: authHeaders })
				.pipe(
					map((response: HttpResponse<any>) => {
						let userData = response;

						return userData;
					}, catchError(error => {
						return error;
					})));
		});
	}

	PostLoginData(loginParameters: LoginParameters, tenant: number) {
		var authHeaders = ServiceHelper.GetHeaders();

		return defer(() => {
			return this._http.post(this._apiUrl + "?tenant=" + tenant, loginParameters, { headers: authHeaders })
				.pipe(
					map((response: HttpResponse<any>) => {
						let userData = response;

						return userData;
					}, catchError(error => {
						return error;
					})));
		});
	}
}


export class LoginParameters {
	Email: string;
	Password: string;
	IsUser: boolean;
	CardId: string;
	CardType: string;
	ByToken: boolean;
	IsMobileLogin: boolean;
	GetToken: boolean;
	IsAngularLogin: boolean;
	MobileVersion: string;
	ClientType: string;
	CaptchaKey: string;
	CaptchaCode: string;
}
