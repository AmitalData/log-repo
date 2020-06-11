
import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { InterestReportArguments } from '../../DataContracts/InterestReportArgs';


@Injectable()

export class InterestReportExtendedListService {

  private _apiUrl: string;
  private httpClient: HttpClient;
  constructor() {

    this.httpClient = ServiceHelper.HttpClient;
    this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/interestreport';
  }

  getByFilters(filters: ApiQueryFilters) {

      var urlparameters = '/GetInterestReportsByFilters?';
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

    return this.httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
      map((response: ServiceResponse) => {
        var serviceResponse: ServiceResponse = new ServiceResponse();
        serviceResponse = response;
        return serviceResponse;
      }),
      catchError(ServiceHelper.HandleServiceError));


  }

 
    PutInterestReortStatus(interestReportArgs: InterestReportArguments) {
        return this.httpClient.put(this._apiUrl + "/PutInterestReportStatus", JSON.stringify(interestReportArgs), ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var result = res;
                serviceResponse.Result = result;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }


}
