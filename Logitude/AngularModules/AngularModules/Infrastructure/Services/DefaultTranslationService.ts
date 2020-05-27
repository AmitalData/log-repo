import { TextCodePMService } from './StandardPMs/TextCodePMService';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { TextCodePM } from '../EntityPMs/TextCodePM';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class DefaultTranslationService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DefaultTranslation';
    }

    Post(args: DefaultTranslationAPIHelper) {
        return defer(() => {
            var mappedEntity: DefaultTranslationAPIHelper = this.MapJsonToDefaultTranslationAPIHelper(args, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                var myJsonResult = response;

                var mappedResult: DefaultTranslationAPIHelper = this.MapJsonToDefaultTranslationAPIHelper(myJsonResult, true, args);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    private MapJsonToDefaultTranslationAPIHelper(jsonPM: any, getCallMap: boolean = true, entity: DefaultTranslationAPIHelper = null) {
        if (!entity) {
            entity = new DefaultTranslationAPIHelper();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        var myPMService = new TextCodePMService();

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "TextCodes") {

                entity.TextCodes = new Array<TextCodePM>();

                for (var item in jsonPM.TextCodes) {
                    var jItem = jsonPM.TextCodes[item];

                    var newItemPM: TextCodePM = myPMService.MapJsonToEntityPM(jItem, getCallMap);

                    entity.TextCodes.push(newItemPM);
                }
            }

            else if (property === "UpdatedTextCodes") {

                entity.UpdatedTextCodes = new Array<TextCodePM>();

                for (var item in jsonPM.UpdatedTextCodes) {
                    var jItem = jsonPM.UpdatedTextCodes[item];

                    var newItemPM: TextCodePM = myPMService.MapJsonToEntityPM(jItem, getCallMap);

                    entity.UpdatedTextCodes.push(newItemPM);
                }
            }

            else {
                entity[property] = jsonPM[property];
            }
        }

        return entity;
    }
}

export class DefaultTranslationAPIHelper {
    Id: number;
    ObjectTableId: string;
    SpellCheckedFilterCode: string;
    CheckDateFilerCode: string;
    TextCodeTypeCode: string;
    SearchText: string;
    SkipDigit: number;
    TakeDigit: number;
    SelectedCheckDate: Date;
    Count: number;
    TextCodes: TextCodePM[] = [];
    UpdatedTextCodes: TextCodePM[] = [];
    IsUpdatingOnly: boolean;
    IsNewSearching: boolean;
}
