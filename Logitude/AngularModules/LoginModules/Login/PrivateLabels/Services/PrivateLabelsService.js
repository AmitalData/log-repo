import { Inject, Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/Observable/throw';
import { BrandingDataService } from './BrandingDataService';
import { Observable } from 'rxjs/Observable';
export var PrivateLabelsService = (function () {
    function PrivateLabelsService(_http, baseUrl) {
        this._http = _http;
        this.AuthHeader = new Headers();
        this.AuthHeader.append('Content-Type', 'application/json');
        this.AuthHeader.append('Accept', 'application/json');
        this.apiUrl = BrandingDataService.GetAppURL(baseUrl) + 'api/PrivateLable';
    }
    PrivateLabelsService.prototype.GetIsPrivateLableUrl = function (privatelableurl) {
        return this._http.get(this.apiUrl + "/GetIsPrivateLableByLoggedDomain", { headers: this.AuthHeader }).map(function (response) {
            return response.json();
        }).catch(this.handleError);
    };
    PrivateLabelsService.prototype.handleError = function (error) {
        console.error(error);
        return Observable.throw(error);
    };
    PrivateLabelsService.decorators = [
        { type: Injectable },
    ];
    /** @nocollapse */
    PrivateLabelsService.ctorParameters = [
        { type: Http, },
        { type: undefined, decorators: [{ type: Inject, args: ['BASE_URL',] },] },
    ];
    return PrivateLabelsService;
}());
//# sourceMappingURL=PrivateLabelsService.js.map