
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass';
import {ConversationHeaderPM} from '../../EntityPMs/ConversationHeaderPM';
@Injectable()

export class ConversationHeaderExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ConversationHeadersExtended';
    }

   GetMessageByFiltered(messageFilter: any) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');



            return this._http.post(this._apiUrl + '/PostGetMessageByFilter', JSON.stringify(messageFilter),
                { headers: authHeader }).map((response) => {

                    var result = response.json();
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

                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }

   GetLoggedContactMessageInfo(userId: string, tenant: number) {

       var authHeader = new Headers();
       authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
       return this._http.get(this._apiUrl + '/GetLoggedContactMessageInfo/?' + 'userId=' + userId + '&tenant='+ tenant, { headers: authHeader }).map(response => {

           var result = response.json();

           var pmresponse: ServiceResponse;
           pmresponse = new ServiceResponse();

           pmresponse.Result = result;
           return pmresponse;

       }).catch(ServiceHelper.HandleServiceError);
   }




   GetCountUnReadConversationHeaderPMs(userid: string, entityId: string, objectTableId: string, areaMessage: string) {

       var authHeader = new Headers();
       authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
       return this._http.get(this._apiUrl + '/GetCountUnReadConversationHeaderPMs/?' + 'userid=' + userid + '&entityId=' + entityId + '&objectTableId=' + objectTableId + '&areaMessage=' + areaMessage, { headers: authHeader }).map(response => {

           var result = response.json();
           var pmresponse: ServiceResponse;
           pmresponse = new ServiceResponse();
           pmresponse.Result = result;

           return pmresponse;
       }).catch(ServiceHelper.HandleServiceError);
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
