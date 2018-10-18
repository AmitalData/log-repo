
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ConversationHeaderMessagePM} from '../../EntityPMs/ConversationHeaderMessagePM';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
@Injectable()

export class ConversationHeaderMessageExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ConversationHeaderMessageExtended';
    }


    GetAllConversationMessageForHeaderQuery(conversationHeaderId: string, userId: string){

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetAllConversationMessageForHeaderQuery/?' + 'conversationHeaderId=' + conversationHeaderId + '&userId='  + userId, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: ConversationHeaderMessagePM;
            var conversationHeaderMessagePMLists: ConversationHeaderMessagePM[];
            conversationHeaderMessagePMLists = new Array<ConversationHeaderMessagePM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                conversationHeaderMessagePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = conversationHeaderMessagePMLists;
            return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }





    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ConversationHeaderMessagePM = null) {


        if (!entityPM) {

            entityPM = new ConversationHeaderMessagePM();
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

    public GetNewEntityPM() {
        var entityPM: ConversationHeaderMessagePM;
        entityPM = new ConversationHeaderMessagePM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }

 


 
}
