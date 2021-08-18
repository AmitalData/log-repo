import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';


@Injectable()

export class TaxReportLineExtendedListService {
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TaxReportOp';
    }

    getByFilters(filters: ApiQueryFilters) {

        var urlparameters = '/GetLinesByFilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var callUrl = this._apiUrl.concat(urlparameters);

        return this.httpClient.get(callUrl,  ServiceHelper.GetHttpHeaders()).pipe(
            map((response:ServiceResponse) => {
                var serviceResponse: ServiceResponse;
                serviceResponse = response;

    
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 
      
    }

    PostCreateTaxReportLines(fileUploadParamerter: ImageParameter) {

        return defer(() => {
            return this.httpClient.post(this._apiUrl + '/PostCreateTaxReportLines', JSON.stringify(fileUploadParamerter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }), catchError(ServiceHelper.HandleServiceError));
        }
        );

    }
}
