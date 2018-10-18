
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ConversationHeaderParticipantPM} from '../../EntityPMs/ConversationHeaderParticipantPM';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
@Injectable()

export class ConversationHeaderParticipantExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ConversationHeaderParticipantExtended';
    }
    

    MakeMeReadMessage(conversationHeaderId: string, userid:string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetMakeMeReadMessage/?' + 'conversationHeaderId=' + conversationHeaderId + "&userid=" + userid , { headers: authHeader }).map(response => {

            var result = response.json();
          
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }

    MakeConversationHeaderParticipantReadMessage(conversationHeaderId: string, userid: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetMakeConversationHeaderParticipantReadMessage/?' + 'conversationHeaderId=' + conversationHeaderId + "&userid=" + userid, { headers: authHeader }).map(response => {

            var result = response.json();
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }

    MakeConversationHeaderParticipantRepliedOrRead(conversationHeaderId: string, userid: string, type: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetMakeConversationHeaderParticipantRepliedOrRead/?' + 'conversationHeaderId=' + conversationHeaderId + "&userid=" + userid + "&type=" + type, { headers: authHeader }).map(response => {

            var result = response.json();
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }


    MakeConversationHeaderParticipantReadAndUnRead(conversationHeaderId: string, userid: string, typequery: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetMakeConversationHeaderParticipantReadAndUnRead/?' + 'conversationHeaderId=' + conversationHeaderId + "&userid=" + userid + "&typequery=" + typequery, { headers: authHeader }).map(response => {

            var result = response.json();

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }


    





    MakeDeleteParticipantUnDelete(conversationHeaderId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetMakeDeleteParticipantUnDelete/?' + 'conversationHeaderId=' + conversationHeaderId, { headers: authHeader }).map(response => {

            var result = response.json();
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }

    DeleteConversationHeaderParticipant(conversationHeaderId: string, userid: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetDeleteConversationHeaderParticipant/?' + 'conversationHeaderId=' + conversationHeaderId + "&userid=" + userid, { headers: authHeader }).map(response => {

            var result = response.json();
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }

    GetAllConversationHeaderParticipantPMByConversationHeaderId(conversationHeaderId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetAllConversationHeaderParticipantPMByConversationHeaderId/?' + 'conversationHeaderId=' + conversationHeaderId, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: ConversationHeaderParticipantPM;
            var  conversationHeaderParticipantPMLists: ConversationHeaderParticipantPM[];
            conversationHeaderParticipantPMLists = new Array<ConversationHeaderParticipantPM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                conversationHeaderParticipantPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = conversationHeaderParticipantPMLists;
            return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }




    GetAllParticipantsConversationHeaderMessageId(conversationHeaderId: string){

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetAllParticipantsConversationHeaderMessageId/?' + 'conversationHeaderId=' + conversationHeaderId, { headers: authHeader }).map(response => {

            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }


    SaveConversationHeaderParticipantPMLists(conversationHeaderParticipantPMLists: any) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            return this._http.post(this._apiUrl + '/PostSaveConversationHeaderParticipantPMLists', JSON.stringify(conversationHeaderParticipantPMLists),
                { headers: authHeader }).map((response) => {

                    var result = response.json();
                  
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();

                    pmresponse.Result = result
                    return pmresponse;

                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ConversationHeaderParticipantPM = null) {


        if (!entityPM) {

            entityPM = new ConversationHeaderParticipantPM();
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
