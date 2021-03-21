import { Inject, Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/Observable/throw';
import { BrandingDataService } from './BrandingDataService';
import { Observable } from 'rxjs/Observable';
export var HybridLabelsBrandingDataService = (function () {
    function HybridLabelsBrandingDataService(_http, baseUrl) {
        this._http = _http;
        this.httpHeaders = BrandingDataService.GetHeaders();
        this._apiUrl = BrandingDataService.GetAppURL(baseUrl) + 'api/TenantManagmentPrivateLabels';
    }
    HybridLabelsBrandingDataService.prototype.GetUserDashboardBrandingData = function (BrandingDataRequest) {
        var url = '/PutGetHybridLabelsBrandingData';
        var callUrl = this._apiUrl.concat(url);
        return this._http.put(callUrl, BrandingDataRequest, { headers: this.httpHeaders }).map(function (response) {
            var result = response.json();
            return result;
        }).catch(this.handleError);
    };
    HybridLabelsBrandingDataService.prototype.handleError = function (error) {
        console.error(error);
        return Observable.throw(error);
    };
    HybridLabelsBrandingDataService.decorators = [
        { type: Injectable },
    ];
    /** @nocollapse */
    HybridLabelsBrandingDataService.ctorParameters = [
        { type: Http, },
        { type: undefined, decorators: [{ type: Inject, args: ['BASE_URL',] },] },
    ];
    return HybridLabelsBrandingDataService;
}());
//# sourceMappingURL=HybridLabelsBrandingDataService.js.map