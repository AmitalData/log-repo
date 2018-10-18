import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../../Infrastructure/Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {NotificationList} from '../../EntityLists/NotificationList';
import {NotificationFiltersDataCount} from '../../DataContract/NotificationFiltersDataCount';
import {NotificationPM} from  '../../EntityPMs/NotificationPM';
import {SelectedNotifications} from '../../DataContract/SelectedNotifications';

export class NotificationExtendedListService{

    private _http: Http;
    private _apiUrl: string;
    public static CachedData: Array<NotificationList> = [];
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/NotificationListExtended';
    }

    getByFilters(filters: ApiQueryFilters) {

        var urlparameters = '/getbyfilters?';
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
                var _mappedListsArray: Array<NotificationList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: NotificationList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    getCountByFilters(filters: ApiQueryFilters) {

        var urlparameters = '/getCountByFilters?';
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

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                var entity = this.MapJsonToEntity(serviceResponse.Result);
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

 

    PutNotificationsStatus(notification: NotificationList) {



        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');




            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

          

            return this._http.put(this._apiUrl + '/PutNotificationsStatus/', JSON.stringify(notification),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    if (pm) {
                       
                        serviceResponse.Result = pm;
                    }


                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        }

        );


    }

    PutNotificationBadjCount(notification: NotificationPM) {


        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');




            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();



            return this._http.put(this._apiUrl + '/PutNotificationBadjCount/', JSON.stringify(notification),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    if (pm) {

                        serviceResponse.Result = pm;
                    }


                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    GetGetTopTenNotifications(userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetTopTenNotifications';

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTopTenNotifications/?' + 'userId=' + userId, { headers: authHeader }).map(response => {


                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray: Array<NotificationPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: NotificationPM;
                        entity = this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }  

    GetOpenNotificationsCount(userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return Observable.defer(() => {
            var callURL = this._apiUrl + '/GetOpenNotificationsCount?' + 'userId=' + userId;

            return this._http.get(callURL, { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        });


    }
    
    GetNotificationsBadjCount(userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return Observable.defer(() => {
            var callURL = this._apiUrl + '/GetNotificationsBadjCount?' + 'userId=' + userId;

            return this._http.get(callURL, { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        });


    }

    PutNotificationStatus(selectedNotifications: SelectedNotifications) {
    return Observable.defer(() => {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');




        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();



        return this._http.put(this._apiUrl + '/PutNotificationStatus/', JSON.stringify(selectedNotifications),
            { headers: authHeader }).map((res) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = res.json();
                var _mappedListsArray: Array<NotificationPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: NotificationPM;
                        entity = this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
    });

    }


    MapJsonToEntityList(jsonList: any) {

        var entityList: NotificationList;
        entityList = new NotificationList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: NotificationPM;
        entityPM = new NotificationPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }

    MapJsonToEntity(json: any) {

        var entity: NotificationFiltersDataCount;
        entity = new NotificationFiltersDataCount();
        var jsonListKeys = Object.keys(json);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entity[property] = json[property];
        }


        return entity;
    }

}