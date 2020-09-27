import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomsRequiredFieldListService} from '../StandardLists/CustomsRequiredFieldListService'
import {CustomsRequiredFieldList} from '../../../Customs/EntityLists/CustomsRequiredFieldList';
import {RequierdFieldObject} from '../../../CustomsModules/CustomsMaintenance/Components/RequiredFields/AddEditRequiredFieldsComponent';

@Injectable()

export class CustomsRequierdFieldsWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequierdFields';

    }

    GetSomeObjectTables() {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSomeObjectTables/", ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = response;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));

        }

        );
    }

    GetCustomsRequiredFieldListsByObjectTable(objectTableId: string) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCustomsRequiredFieldListsByObjectTable/?objectTableId=" + objectTableId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var srv = new CustomsRequiredFieldListService();

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;
                var _mappedListsArray: Array<CustomsRequiredFieldList> = [];

                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CustomsRequiredFieldList;
                        entity = srv.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));

        }

        );
    }

    PostRequiredFields(fields: RequierdFieldObject[]) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(fields);

            return this._http.post(
                this._apiUrl + '/PostRequiredFields/',
                JSON.stringify(fields),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

}