import { ServiceHelper } from '../../../CargoTracking/Utilities/ServiceHelper';
import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';

@Injectable()
export class LoginExtendedService {
	private _apiUrl: string;
	constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
		this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/';
	}

	PostUserValidation(loginParameters: LoginParameters) {
		var authHeaders = ServiceHelper.GetHeaders();

		return defer(() => {
			return this._http.post(this._apiUrl + "Authentication", loginParameters, { headers: authHeaders })
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
			return this._http.post(this._apiUrl + "Authentication?tenant=" + tenant, loginParameters, { headers: authHeaders })
				.pipe(
					map((response: HttpResponse<any>) => {
						let userData = response;

						return userData;
					}, catchError(error => {
						return error;
					})));
		});
	}

	PostRequestResetUserPassword(resetPasswordParameters: ResetPasswordParameters){
		var authHeaders = ServiceHelper.GetHeaders();

		return defer(() => {
			return this._http.post(this._apiUrl + "ResetPassword?PostResetPassword", resetPasswordParameters, { headers: authHeaders })
				.pipe(
					map((response: HttpResponse<any>) => {
						let userData = response;

						return userData;
					}, catchError(error => {
						return error;
					})));
		});
	}

	PostChangePassword(email: string, resetPasswordParameters: any){
		var authHeaders = ServiceHelper.GetHeaders();

		return defer(() => {
			return this._http.post(this._apiUrl + "Authentication?email=" + email, resetPasswordParameters, { headers: authHeaders })
				.pipe(
					map((response: HttpResponse<any>) => {
						let serviceResponse = response;

						return serviceResponse;
					}, catchError(error => {
						return error;
					})));
		});
	}

	CheckUserPassword(changePasswordParameter: ChangePasswordParameter){
		var authHeaders = ServiceHelper.GetHeaders();

		return defer(() => {
			return this._http.post(this._apiUrl + "PasswordChange/PostCheckPasswordUser", changePasswordParameter, { headers: authHeaders })
				.pipe(
					map((response: HttpResponse<any>) => {
						let serviceResponse = response;

						return serviceResponse;
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

export class ResetPasswordParameters {
	Email: string;
	IsChampLogin: boolean;
	CaptchaKey: string;
	CaptchaCode: string;
	PageName: string;
	Domain: string;
	BrandingTenant: string;
}

export class ChangePasswordParameter {
    public Email: string;
    public CurrentPassword: string;
    public ContactId: string;
    public NewPassword: string;
}