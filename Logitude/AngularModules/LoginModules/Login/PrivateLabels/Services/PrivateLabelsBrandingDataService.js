import { Inject, Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/Observable/throw';
import { BrandingDataService } from './BrandingDataService';
import { Observable } from 'rxjs/Observable';
export var PrivateLabelsBrandingDataService = (function () {
    function PrivateLabelsBrandingDataService(_http, baseUrl) {
        this._http = _http;
        this.httpHeaders = BrandingDataService.GetHeaders();
        this._apiUrl = BrandingDataService.GetAppURL(baseUrl) + 'api/TenantManagmentPrivateLabels';
    }
    PrivateLabelsBrandingDataService.prototype.GetUserDashboardBrandingData = function (BrandingDataRequest) {
        var url = '/PutGetPrivateLabelsBrandingData';
        var callUrl = this._apiUrl.concat(url);
        return this._http.put(callUrl, BrandingDataRequest, { headers: this.httpHeaders }).map(function (response) {
            var result = response.json();
            return result;
        }).catch(this.handleError);
    };
    PrivateLabelsBrandingDataService.prototype.handleError = function (error) {
        console.error(error);
        return Observable.throw(error);
    };
    PrivateLabelsBrandingDataService.decorators = [
        { type: Injectable },
    ];
    /** @nocollapse */
    PrivateLabelsBrandingDataService.ctorParameters = [
        { type: Http, },
        { type: undefined, decorators: [{ type: Inject, args: ['BASE_URL',] },] },
    ];
    return PrivateLabelsBrandingDataService;
}());
//# sourceMappingURL=PrivateLabelsBrandingDataService.js.map