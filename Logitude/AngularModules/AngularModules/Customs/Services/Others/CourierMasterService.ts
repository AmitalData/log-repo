import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DeclarationPM} from '../../EntityPMs/DeclarationPM';
import {CourierMasterPM} from '../../EntityPMs/CourierMasterPM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { SendPayReadyLowRequestParams } from '../../DataContract/RequestParams/SendPayReadyLowRequestParams';
import { SendALLCorrectRequestParams } from '../../DataContract/RequestParams/SendALLCorrectRequestParams';
import { GatepassRequestMessageRequestParams } from '../../DataContract/RequestParams/GatepassRequestMessageRequestParams';
import { SendALLStorageSiteRequestParams } from '../../DataContract/RequestParams/SendALLStorageSiteRequestParams';
import { SendUnCorrectDocumentsRequestParams } from '../../DataContract/RequestParams/SendUnCorrectDocumentsRequestParams';
import { AppTool } from '../../../Infrastructure/Tools';


@Injectable()


export class CourierMasterService {

    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CourierMaster';
    }

    GetIfCourierMasterExists(Id: string,airlineId: string, HAWB: string, MAWB:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetIfCourierMasterExists?' + 'Id=' + Id + '&airlineId=' + airlineId + '&HAWB=' + HAWB + '&MAWB=' + MAWB, { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        });


    }

    getPromiseByFilters(filters: ApiQueryFilters) {

        return new Promise((resolve, reject) => {

            resolve(this.getByFilters(filters));

        });
    }

    getByFilters(filters: ApiQueryFilters) {

        var urlparameters = '/GetCourierConnectedDeclarations?';
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

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters);//


        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response.json();
                var _mappedListsArray: Array<DeclarationPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: DeclarationPM;
                        entity = this.MapJsonToDeclarationPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getPromiseByFilters1(filters: ApiQueryFilters) {

        return new Promise((resolve, reject) => {

            resolve(this.getByFilters1(filters));

        });
    }

    getByFilters1(filters: ApiQueryFilters) {

        var urlparameters = '/GetNotConnectedDeclarations?';
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

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters);//


        return Observable.defer(() => {
            return this._http.get(callUrl, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response.json();
                var _mappedListsArray: Array<DeclarationPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: DeclarationPM;
                        entity = this.MapJsonToDeclarationPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetRequiredFieldsForCourierMaster(courierMasterId: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetRequiredFieldsForCourierMaster/?courierMasterId=" + courierMasterId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetRequiredFieldsForCourierMasterIncludeManifest(courierMasterId: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetRequiredFieldsForCourierMasterIncludeManifest/?courierMasterId=" + courierMasterId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToDeclarationPM(json: any, mapParent: boolean = true, entity: DeclarationPM = null) {


        if (!entity) {

            entity = new DeclarationPM();
        }

        var jsonPMKeys = Object.keys(json);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entity[property] = json[property];
        }


        //  entity.IsDirty = false;



        return entity;
    }


    getCourierMasterByDeclarationId(declarationId: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getCourierMasterByDeclarationId?' + 'declarationId=' + declarationId, {
                headers: authHeader
            }).map(response => {
                var pm = response.json();



                var entity: CourierMasterPM;
                if (pm) {
                    entity = this.MapJsonToCourierMasterPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

         

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetStatistic(CourierMasterId) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetStatistic?' + 'CourierMasterId=' + CourierMasterId, {
                headers: authHeader
            }).map(response => {
                var KeyValuePairList = response.json();



                

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = KeyValuePairList;



                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetSendPayReadyLow2755(CourierMasterId, HAWB, InternalBankId:string ) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSendPayReadyLow2755?' + 'CourierMasterId=' + CourierMasterId + '&HAWB=' + HAWB + '&InternalBankId=' + InternalBankId, {
                headers: authHeader
            }).map(response => {
                var messString = response.json();
                

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;

                
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    PostSendPayReadyLow2755(requestParams: SendPayReadyLowRequestParams) {

        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendPayReadyLow2755/', JSON.stringify(requestParams), { headers: authHeader })
                .map((res) => {
                    var messString = res.json();


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;


                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
            ;

        });
    }
    PostSendALLTerminal(requestParams: SendALLCorrectRequestParams) {

        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendALLTerminal/', JSON.stringify(requestParams), { headers: authHeader })
                .map((res) => {
                    var messString = res.json();


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;


                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
            ;

        });
    }
    PostSendALLCorrectDec(requestParams: SendALLCorrectRequestParams) {

        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendALLCorrectDec/', JSON.stringify(requestParams), { headers: authHeader })
                .map((res) => {
                    var messString = res.json();


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;


                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
            ;

        });
    }

    PostSendALLCorrectManifest(requestParams: SendALLCorrectRequestParams) {

        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendALLCorrectManifest/', JSON.stringify(requestParams), { headers: authHeader })
                .map((res) => {
                    var messString = res.json();


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;


                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
            ;

        });
    }

    PostSendALLChangeStorageSiteCode(requestParams: SendALLStorageSiteRequestParams) {

        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendALLChangeStorageSiteCode/', JSON.stringify(requestParams), { headers: authHeader })
                .map((res) => {
                    var messString = res.json();
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;

                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
            ;

        });
    }

    PostSendUnCorrectDocuments(requestParams: SendUnCorrectDocumentsRequestParams) {

        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendUnCorrectDocuments/', JSON.stringify(requestParams), { headers: authHeader })
                .map((res) => {
                    var messString = res.json();
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
            ;

        });
    }

    GetSendALLCorrectManifest(CourierMasterId, HAWB, CourierDeclarationStatusCode) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSendALLCorrectManifest?' + 'CourierMasterId=' + CourierMasterId + '&HAWB=' + HAWB + '&CourierDeclarationStatusCode=' + CourierDeclarationStatusCode, {
                headers: authHeader
            }).map(response => {
                var messString = response.json();





                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;



                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    GetSendALLCorrectDec(CourierMasterId, HAWB, CourierDeclarationStatusCode) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSendALLCorrectDec?' + 'CourierMasterId=' + CourierMasterId + '&HAWB=' + HAWB + '&CourierDeclarationStatusCode=' + CourierDeclarationStatusCode, {
                headers: authHeader
            }).map(response => {
                var messString = response.json();





                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;



                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    GetSendALLDeclarationsStatusRequest(CourierMasterId, testerSendOption: string = null) {
        let sTesterSendOption = '';
        if (!AppTool.IsNullOrEmpty(testerSendOption)){
            sTesterSendOption =   testerSendOption
        }
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSendALLDeclarationsStatusRequest?' + 'CourierMasterId=' + CourierMasterId +
                '&testerSendOption=' +  sTesterSendOption, {
                headers: authHeader
            }).map(response => {
                var messString = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    GetSendECTHRDataMaman(declarationId) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSendECTHRDataMaman?' + 'declarationId=' + declarationId, {
                headers: authHeader
            }).map(response => {
                var messString = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetSendFTPMamanRequest(CourierMasterId) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSendFTPMamanRequest?' + 'CourierMasterId=' + CourierMasterId, {
                headers: authHeader
            }).map(response => {
                var messString = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToCourierMasterPM(json: any, mapParent: boolean = true, entity: CourierMasterPM = null) {


        if (!entity) {

            entity = new CourierMasterPM();
        }

        var jsonPMKeys = Object.keys(json);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entity[property] = json[property];
        }


        //  entity.IsDirty = false;



        return entity;
    }

    PostGatepassRequestMessage(requestParams: GatepassRequestMessageRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostGatepassRequestMessage/',
                JSON.stringify(requestParams),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        });
    }

    GetPending(CourierMasterId) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetPending?' + 'CourierMasterId=' + CourierMasterId, {
                headers: authHeader
            }).map(response => {
                var KeyValuePairList = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = KeyValuePairList;

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    GetIfAllowToCancelCourierMaster(CourierMasterId) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetIfAllowToCancelCourierMaster?' + 'CourierMasterId=' + CourierMasterId, {
                headers: authHeader
            }).map(response => {
 
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }
}
