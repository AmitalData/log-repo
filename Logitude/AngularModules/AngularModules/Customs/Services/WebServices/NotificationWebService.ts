import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { NotificationPM } from '../../EntityPMs/NotificationPM';
import { MessageToAgentRequestParams } from '../../DataContract/RequestParams/MessageToAgentRequestParams';
import { NotificationPMService } from '../StandardPMs/NotificationPMService';

@Injectable()

export class NotificationWebService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/NotificationWebService';
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/NotificationWebService';

    }

    //NotificationReply
    GetNotificationsByDefinitionCode(objectTableId: string, entityId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var notificationPMService: NotificationPMService = new NotificationPMService();

            return this._http.get(this._apiUrl + "/GetNotificationsByDefinitionCode/?objectTableId=" + objectTableId + "&entityId=" + entityId + "&tenant=" + tenant
                , {
                    headers: authHeader
                }).map(response => {

                    var allLists = response.json();
                    var _mappedListsArray: Array<NotificationPM> = [];
                    if (allLists) {
                        for (var key in allLists) {

                            var entity: NotificationPM;
                            entity = notificationPMService.MapJsonToEntityPM(allLists[key]);
                            _mappedListsArray.push(entity);
                        }
                    }

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = _mappedListsArray;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    PostSendNotificationReplyRequest(entity: MessageToAgentRequestParams) {

        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendNotificationReplyRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    SetNotificationsStatus(Ids: string[], status: string) {

   

        // Send request
        return Observable.defer(() => {

            // Prepare parameters
            var IdsParameterString = "";
            if (Ids && Ids.length > 0) {
                Ids.forEach(el => {
                    IdsParameterString += 'Ids=' + el + '&';
                });

            } else {
                console.log("[ERROR] cannot set notification status without Ids!", Ids, status);
                return;
            }

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSetNotificationsStatus/?" + IdsParameterString + "status=" + status
                , { headers: authHeader }).map(response => {

                    //var res = response.json();

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    
                    //serviceResponse.Result = res;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }

}

