
import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ConversationHeaderParticipantPM} from '../../EntityPMs/ConversationHeaderParticipantPM';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
@Injectable()

export class ConversationHeaderParticipantExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ConversationHeaderParticipantExtended';
    }
    

    MakeMeReadMessage(conversationHeaderId: string, userid:string) {


        return this._http.get(this._apiUrl + '/GetMakeMeReadMessage/?' + 'conversationHeaderId=' + conversationHeaderId + "&userid=" + userid , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
          
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }),catchError(ServiceHelper.HandleServiceError));
    }

    MakeConversationHeaderParticipantReadMessage(conversationHeaderId: string, userid: string) {


        return this._http.get(this._apiUrl + '/GetMakeConversationHeaderParticipantReadMessage/?' + 'conversationHeaderId=' + conversationHeaderId + "&userid=" + userid, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }),catchError(ServiceHelper.HandleServiceError));
    }

    MakeConversationHeaderParticipantRepliedOrRead(conversationHeaderId: string, userid: string, type: string) {

        return this._http.get(this._apiUrl + '/GetMakeConversationHeaderParticipantRepliedOrRead/?' + 'conversationHeaderId=' + conversationHeaderId + "&userid=" + userid + "&type=" + type, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }),catchError(ServiceHelper.HandleServiceError));
    }


    MakeConversationHeaderParticipantReadAndUnRead(conversationHeaderId: string, userid: string, typequery: string) {

        return this._http.get(this._apiUrl + '/GetMakeConversationHeaderParticipantReadAndUnRead/?' + 'conversationHeaderId=' + conversationHeaderId + "&userid=" + userid + "&typequery=" + typequery, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }),catchError(ServiceHelper.HandleServiceError));
    }


    





    MakeDeleteParticipantUnDelete(conversationHeaderId: string) {


        return this._http.get(this._apiUrl + '/GetMakeDeleteParticipantUnDelete/?' + 'conversationHeaderId=' + conversationHeaderId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }),catchError(ServiceHelper.HandleServiceError));
    }

    DeleteConversationHeaderParticipant(conversationHeaderId: string, userid: string) {


        return this._http.get(this._apiUrl + '/GetDeleteConversationHeaderParticipant/?' + 'conversationHeaderId=' + conversationHeaderId + "&userid=" + userid, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;

        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetAllConversationHeaderParticipantPMByConversationHeaderId(conversationHeaderId: string) {

        return this._http.get(this._apiUrl + '/GetAllConversationHeaderParticipantPMByConversationHeaderId/?' + 'conversationHeaderId=' + conversationHeaderId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result:any = response;
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

        }),catchError(ServiceHelper.HandleServiceError));
    }




    GetAllParticipantsConversationHeaderMessageId(conversationHeaderId: string){


        return this._http.get(this._apiUrl + '/GetAllParticipantsConversationHeaderMessageId/?' + 'conversationHeaderId=' + conversationHeaderId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;

            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }


    SaveConversationHeaderParticipantPMLists(conversationHeaderParticipantPMLists: any) {
        return defer(() => {


            return this._http.post(this._apiUrl + '/PostSaveConversationHeaderParticipantPMLists', JSON.stringify(conversationHeaderParticipantPMLists), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {

                    var result = response;
                  
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();

                    pmresponse.Result = result
                    return pmresponse;

                }),catchError(ServiceHelper.HandleServiceError));
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
