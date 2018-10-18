import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DeclarationList} from '../../EntityLists/DeclarationList';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {VendorInsertUpdateDeleteMessageRequestParams} from '../../DataContract/RequestParams/VendorInsertUpdateDeleteMessageRequestParams';
import {VendorAddCommunicationDeviceRequestParams} from '../../DataContract/RequestParams/VendorAddCommunicationDeviceRequestParams';
import {VendorSearchByCustomsAgentRequestParams} from '../../DataContract/RequestParams/VendorSearchByCustomsAgentRequestParams';


@Injectable()

export class VendorMessagesService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Vendor';

    }

    PostAddNewVendorRequest(params: VendorInsertUpdateDeleteMessageRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostAddNewVendorRequest/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    PostAddNewVendorCommunicationRequest(params: VendorAddCommunicationDeviceRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostAddNewVendorCommunicationRequest/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    PostSearchVendorRequest(params: VendorSearchByCustomsAgentRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSearchVendorRequest/',
                JSON.stringify(params),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    GetVendorByNumber(vendorNumber: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetVendorByNumber/?vendorNumber=" + vendorNumber, {
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

    PutRecallSuppliersFromFileRequest(fileUploadParamerter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/PutRecallSuppliersFromFileRequest', JSON.stringify(fileUploadParamerter), {
                headers: authHeader,

            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }
}