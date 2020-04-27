
import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass';
import {ConversationHeaderPM} from '../../EntityPMs/ConversationHeaderPM';
@Injectable()

export class ConversationHeaderExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ConversationHeadersExtended';
    }

   GetMessageByFiltered(messageFilter: any) {
        return defer(() => {
 
            return this._http.post(this._apiUrl + '/PostGetMessageByFilter', JSON.stringify(messageFilter), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {

                    var result:any = response;
                    var entity: ConversationHeaderPM;
                    var conversationHeaderPMLists: ConversationHeaderPM[];
                    conversationHeaderPMLists = new Array<ConversationHeaderPM>();
                    result.forEach((item) => {
                        entity = this.MapJsonToEntityPM(item);
                        conversationHeaderPMLists.push(entity);
                    });
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();

                    pmresponse.Result = conversationHeaderPMLists;
                    return pmresponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }
        );

    }

   GetLoggedContactMessageInfo(userId: string, tenant: number) {


       return this._http.get(this._apiUrl + '/GetLoggedContactMessageInfo/?' + 'userId=' + userId + '&tenant='+ tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

           var result = response;

           var pmresponse: ServiceResponse;
           pmresponse = new ServiceResponse();

           pmresponse.Result = result;
           return pmresponse;

       }),catchError(ServiceHelper.HandleServiceError));
   }




   GetCountUnReadConversationHeaderPMs(userid: string, entityId: string, objectTableId: string, areaMessage: string) {

       return this._http.get(this._apiUrl + '/GetCountUnReadConversationHeaderPMs/?' + 'userid=' + userid + '&entityId=' + entityId + '&objectTableId=' + objectTableId + '&areaMessage=' + areaMessage, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

           var result = response;
           var pmresponse: ServiceResponse;
           pmresponse = new ServiceResponse();
           pmresponse.Result = result;

           return pmresponse;
       }),catchError(ServiceHelper.HandleServiceError));
   }
   




    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ConversationHeaderPM = null) {


        if (!entityPM) {

            entityPM = new ConversationHeaderPM();
        }

        var customFields: Array<string> = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

                continue;
            }
            var property = jsonPMKeys[key];

            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }

        }


        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
   }

    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }


}
