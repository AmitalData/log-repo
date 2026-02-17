import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomsRequiredFieldListService} from '../StandardLists/CustomsRequiredFieldListService'
import {CustomsRequiredFieldList} from '../../../Customs/EntityLists/CustomsRequiredFieldList';
import {RequierdFieldObject} from '../../../CustomsModules/CustomsMaintenance/Components/RequiredFields/AddEditRequiredFieldsComponent';

@Injectable()

export class CustomsRequierdFieldsWebService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequierdFields';

    }

    GetSomeObjectTables() {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSomeObjectTables/", {
                    headers: authHeader
                }).map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = response.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    GetCustomsRequiredFieldListsByObjectTable(objectTableId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCustomsRequiredFieldListsByObjectTable/?objectTableId=" + objectTableId, {
                headers: authHeader
            }).map(response => {

                var srv = new CustomsRequiredFieldListService();

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response.json();
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
            }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    PostRequiredFields(fields: RequierdFieldObject[]) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(fields);

            return this._http.post(
                this._apiUrl + '/PostRequiredFields/',
                JSON.stringify(fields),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

}