import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../Utilities/ServiceHelper';
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {GeneralDomainService, FieldsTranslations} from './GeneralDomainService';

@Injectable()

export class TranslateLablesService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TranslateLables';
    }

    Post(args: TranslateLabelsAPIHelper) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: TranslateLabelsAPIHelper = this.MapJsonToTranslateLabelsAPIHelper(args, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();

                var mappedResult: TranslateLabelsAPIHelper = this.MapJsonToTranslateLabelsAPIHelper(myJsonResult, true, args);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    private MapJsonToTranslateLabelsAPIHelper(jsonPM: any, getCallMap: boolean = true, entity: TranslateLabelsAPIHelper = null) {
        if (!entity) {
            entity = new TranslateLabelsAPIHelper();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        var myService = new GeneralDomainService();

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "Translations") {

                entity.Translations = new Array<FieldsTranslations>();

                for (var item in jsonPM.Translations) {
                    var jItem = jsonPM.Translations[item];

                    var newItemPM: FieldsTranslations = myService.MapFieldsTranslations(jItem, getCallMap);

                    entity.Translations.push(newItemPM);
                }
            }

            //else if (property === "UpdatedTextCodes") {

            //    entity.UpdatedTextCodes = new Array<TextCodePM>();

            //    for (var item in jsonPM.UpdatedTextCodes) {
            //        var jItem = jsonPM.UpdatedTextCodes[item];

            //        var newItemPM: TextCodePM = myPMService.MapJsonToEntityPM(jItem, getCallMap);

            //        entity.UpdatedTextCodes.push(newItemPM);
            //    }
            //}

            else {
                entity[property] = jsonPM[property];
            }
        }

        return entity;
    }
}

export class TranslateLabelsAPIHelper {
    Id: number;
    Language: string;
    ObjectTableId: string;
    SpellCheckedFilterCode: string;
    CheckDateFilerCode: string;
    TextCodeTypeCode: string;
    SearchText: string;
    SkipDigit: number;
    TakeDigit: number;
    SelectedCheckDate: Date;
    CountAll: number;
    Translations: FieldsTranslations[] = [];
    UpdatedTranslations: FieldsTranslations[] = [];
}