import { ServiceHelper } from './../../../CargoTracking/Utilities/ServiceHelper';
import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing';

@Injectable()
export class LoginService {
    private _apiUrl: string;
    constructor(private _http: HttpClient , @Inject('BASE_URL') baseUrl: string) {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl)  + 'api/Authentication';
    }


	PostUserValidation(loginParameters: LoginParameters) {
        var authHeaders = ServiceHelper.GetHeaders();

		return defer(() => {
            return this._http.post(this._apiUrl, loginParameters, {headers: authHeaders})
				.pipe(
					map((response: HttpResponse<any>) => {
						let userData = response;
						
						return userData;
					},catchError(error=>{
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

export class UserData {
	Id: string ;
    UserName: string;
	CardId: string;
	CardType: string;
	CurrentTenant: number;
	IsUser: boolean;
	Technology: string;
	IsLocked: boolean;
	IpRestricted: boolean;
	InValidMailOrPassword: boolean;
	HasError: boolean;
	ExceptionMessage: string;
	MustChangePassword: boolean;
	CompanyLogins: CompanyLogin[];
	ContactsCount: number;
	HtmlVersion: string;
	Token: string;
	UserId: string;
	Tenant: number;
	InvalidToken: boolean;
	InvalidMobileAccessPermission: boolean;
	SelectedCompanyLogin: CompanyLogin;
	InActive: boolean;
	PrivateLablehasZeroTenant: boolean;
	IsBrandingEnabled: boolean;
	Unlicensed: boolean;
	SilverlightEndDate: string;
	IsTwoFactorAuthenticationRequired: boolean;
	CodeExpirationDate: Date;
	UserMobileNumber: string;
	TwoFactorkey: string;
	KeepUserLoggedIn: boolean;
	PasswordExpirationDateMessage: string;
	IsPasswordExpirationDate: boolean;
	DocumentDownloadToken: string;
	NumberOfRetries: number;
	SessionTimeout: number;
	WebTokenExpirationWarningInMinutes: number;
	WebTokenLifeTimeInMinutes: number;
	Param1: boolean;
	CaptchaImage: string;
	CaptchaKey: string;
	InValidCaptcha: boolean;
	LastLoginDateTime: Date;
	InvalidDocumentToken: string;
}

export class CompanyLogin
{
	Tenant: number;
	CompanyName: string;
	CustomerName: string;
	IsUser: boolean;
	CardId: string;
	CardType: string;
	Email: string;
	ContactId: string;
	LicensedUser: boolean;
	InternetAccess: boolean;
	URL: string;
	Extension: string;
	Id: string;
	PrivateLabelId: string;
}