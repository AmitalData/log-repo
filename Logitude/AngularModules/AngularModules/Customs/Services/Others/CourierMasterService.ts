import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, Observable, of } from 'rxjs';
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
import { PendingRequestParams } from 'Customs/DataContract/RequestParams/PendingRequestParams';
import { SendALLDelayFormParams } from '../../DataContract/RequestParams/SendALLDelayFormParams';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';


@Injectable()


export class CourierMasterService {

    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CourierMaster';
    }

    GetIfCourierMasterExists(Id: string,airlineId: string, HAWB: string, MAWB:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        return defer(() => {
            return this._http.get(this._apiUrl + '/GetIfCourierMasterExists?' + 'Id=' + Id + '&airlineId=' + airlineId + '&HAWB=' + HAWB + '&MAWB=' + MAWB, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }

    public connectedSelectAll: boolean;
    public disconnectedSelectAll: boolean;
    public connectedItems: ObservableCollection = new ObservableCollection([]);
    public disconnectedItems: ObservableCollection = new ObservableCollection([]);

    public isNotDirty: boolean;

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


        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response;
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
            }),catchError(ServiceHelper.HandleServiceError));
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


        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response;
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
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetRequiredFieldsForCourierMaster(courierMasterId: string) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetRequiredFieldsForCourierMaster/?courierMasterId=" + courierMasterId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetRequiredFieldsForCourierMasterIncludeManifest(courierMasterId: string) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetRequiredFieldsForCourierMasterIncludeManifest/?courierMasterId=" + courierMasterId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
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
        return defer(() => {
            return this._http.get(this._apiUrl + '/getCourierMasterByDeclarationId?' + 'declarationId=' + declarationId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pm = response;



                var entity: CourierMasterPM;
                if (pm) {
                    entity = this.MapJsonToCourierMasterPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

         

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetStatistic(CourierMasterId,IsWorkSheetFromExcel:boolean,userId:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetStatistic?' + 'CourierMasterId=' + CourierMasterId + '&IsWorkSheetFromExcel=' + IsWorkSheetFromExcel+ '&userId=' + userId , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var KeyValuePairList = response;



                

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = KeyValuePairList;



                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetSendPayReadyLow2755(CourierMasterId, HAWB, InternalBankId:string,IsWorkSheetFromExcel:boolean=false) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSendPayReadyLow2755?' + 'CourierMasterId=' + CourierMasterId + '&HAWB=' + HAWB + '&InternalBankId=' + InternalBankId+ '&IsWorkSheetFromExcel=' + IsWorkSheetFromExcel, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var messString = response;
                

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;

                
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    PostSendPayReadyLow2755(requestParams: SendPayReadyLowRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendPayReadyLow2755/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;


                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }
    PostSendALLTerminal(requestParams: SendALLCorrectRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendALLTerminal/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;


                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }
    PostSendALLCorrectDec(requestParams: SendALLCorrectRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendALLCorrectDec/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;


                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }

    PostSendDelayFormForDeclarations(requestParams: SendALLDelayFormParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendDelayFormForDeclarations/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;


                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }

    PostSendALLCorrectManifest(requestParams: SendALLCorrectRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendALLCorrectManifest/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;


                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }

    PostSendALLChangeStorageSiteCode(requestParams: SendALLStorageSiteRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendALLChangeStorageSiteCode/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }

    PostSendUnCorrectDocuments(requestParams: SendUnCorrectDocumentsRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendUnCorrectDocuments/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }
    
    GetSendDocumentsFromQueue(courierMasterId, MAWB) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSendDocumentsFromQueue?' + 'courierMasterId=' + courierMasterId + '&MAWB=' + MAWB, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var messString = response;





                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;



                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });

    }
    GetSendALLCorrectManifest(CourierMasterId, HAWB, CourierDeclarationStatusCode) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSendALLCorrectManifest?' + 'CourierMasterId=' + CourierMasterId + '&HAWB=' + HAWB + '&CourierDeclarationStatusCode=' + CourierDeclarationStatusCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var messString = response;





                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;



                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    GetSendALLCorrectDec(CourierMasterId, HAWB, CourierDeclarationStatusCode) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSendALLCorrectDec?' + 'CourierMasterId=' + CourierMasterId + '&HAWB=' + HAWB + '&CourierDeclarationStatusCode=' + CourierDeclarationStatusCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var messString = response;





                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;



                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    GetSendALLDeclarationsStatusRequest(CourierMasterId, testerSendOption: string = null,IsWorkSheetFromExcel:boolean) {
        let sTesterSendOption = '';
        if (!AppTool.IsNullOrEmpty(testerSendOption)){
            sTesterSendOption =   testerSendOption
        }
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSendALLDeclarationsStatusRequest?' + 'CourierMasterId=' + CourierMasterId +
                '&testerSendOption=' +  sTesterSendOption+ '&IsWorkSheetFromExcel=' + IsWorkSheetFromExcel, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var messString = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    GetSendECTHRDataMaman(declarationId) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSendECTHRDataMaman?' + 'declarationId=' + declarationId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var messString = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSendFTPMamanRequest(CourierMasterId) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSendFTPMamanRequest?' + 'CourierMasterId=' + CourierMasterId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var messString = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = messString;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
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

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostGatepassRequestMessage/',
                JSON.stringify(requestParams),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));

        });
    }

    GetPending(CourierMasterId,IsWorkSheetFromExcel : boolean=false) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetPending?' + 'CourierMasterId=' + CourierMasterId+ '&IsWorkSheetFromExcel=' + IsWorkSheetFromExcel, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var KeyValuePairList = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = KeyValuePairList;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    GetIfAllowToCancelCourierMaster(CourierMasterId) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetIfAllowToCancelCourierMaster?' + 'CourierMasterId=' + CourierMasterId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
 
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    PostSendClosePending(requestParams: PendingRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendClosePending/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }


    
 
    sendConnectDeclaration(courierMasterId: string, tenant: number, MAWB: string, connectedAll: boolean, disconnectedAll: boolean, connectedItems: string[], disconnectedItems: string[]) {
        const ajax: Observable<any> = this._http.post(
            this._apiUrl + "/sendConnectDeclaration",
            { courierMasterId: courierMasterId, tenant: tenant, MAWB: MAWB || '', connectedAll: !!connectedAll, disconnectedAll: !!disconnectedAll, connectedItems: connectedItems, disconnectedItems: disconnectedItems },
            { headers: ServiceHelper.GetHttpHeaders().headers }
        );

        return LogtuideTableDataService.createInstance().sendAjaxAndGetDataStandart(ajax);
    }
    PostApproveAllPending(requestParams: PendingRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostApproveAllPending/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }
}
