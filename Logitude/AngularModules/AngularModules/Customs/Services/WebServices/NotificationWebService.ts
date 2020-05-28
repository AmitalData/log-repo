import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
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
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/NotificationWebService';
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/NotificationWebService';

    }

    //NotificationReply
    GetNotificationsByDefinitionCode(objectTableId: string, entityId: string, tenant: number) {
        return defer(() => {

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var notificationPMService: NotificationPMService = new NotificationPMService();

            return this._http.get(this._apiUrl + "/GetNotificationsByDefinitionCode/?objectTableId=" + objectTableId + "&entityId=" + entityId + "&tenant=" + tenant
                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var allLists = response;
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
                }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    PostSendNotificationReplyRequest(entity: MessageToAgentRequestParams) {

        return defer(() => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendNotificationReplyRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    SetNotificationsStatus(Ids: string[], status: string) {

        // Send request
        return defer(() => {

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

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSetNotificationsStatus/?" + IdsParameterString + "status=" + status
                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    //var res = response;

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    
                    //serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }
        );

    }

}

