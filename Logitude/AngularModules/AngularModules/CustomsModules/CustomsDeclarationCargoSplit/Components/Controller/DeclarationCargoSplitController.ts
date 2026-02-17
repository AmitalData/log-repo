declare var window;
import { DeclarationPM } from "../../../../Customs/EntityPMs/DeclarationPM";
import { DeclarationCargoSplitPM } from "../../../../Customs/EntityPMs/DeclarationCargoSplitPM";
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Observable}     from 'rxjs/Rx';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, ArrayTool, DateTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Http, Headers} from '@angular/http';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {Output}  from '@angular/core';
import {EventEmitter} from '@angular/core';

export class DeclarationCargoSplitController {
    private IsDisplayOnly: boolean;
    private http: Http;
    private apiUrl: string;
    
    constructor(private DeclarationCargoSplitPM: DeclarationCargoSplitPM) {
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestSheetExtended';
        this.http = ServiceHelper.Http;

    }

   

    DisplayOnlyCheck() {
        var rresponse: ServiceResponse = new ServiceResponse();
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        return Observable.defer(() => {
            var rresponse: ServiceResponse = new ServiceResponse();
            rresponse.Result = { IsDisplayOnly: false, DisplayOnlyMessage: "" };
            return Observable.of(rresponse);
        });
    }

    CheckRequestsInProgress(declarationId: string) {
        // Request sheets in progress check
        var authHeader = new Headers();
        var table = window.ObjectTables.filter(d => d.Name === "Customs.DeclarationCargoSplit")[0];
        var table2 = window.ObjectTables.filter(d => d.Name === "Customs.Declaration")[0];

        authHeader.append('Token', SessionInfo.Token);
        return Observable.defer(() => {
            return this.http.get(this.apiUrl + '/GetRequestInProgress/?' + 'tenant=' + this.DeclarationCargoSplitPM.Tenant + '&interfaceTypeCode=8370' + '&objectTableId1=' + table.Id + '&entityId1=' + this.DeclarationCargoSplitPM.Id + '&objectTableId2=' + table2.Id + '&entityId2=' + encodeURIComponent(declarationId) + '&customFileNo=' + "" + '&displayOnlyMode= false', { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                var requestSheets = response.json();
                if (requestSheets == null || requestSheets.length == 0) {
                    serviceResponse.Result = new DisplayOnlyCheckResult(false, "");
                }
                else {
                    var RequestInProgressInterfaceTypeName = requestSheets[0].InterfaceTypeName;
                    var text = TextCodeTranslator.Translate("Customs.General.RequestInProgress");
                    text = text.replace('{0}', RequestInProgressInterfaceTypeName);
                    serviceResponse.Result = new DisplayOnlyCheckResult(true, text);
                }
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }
 
    public SelectionCompleted: EventEmitter<any> = new EventEmitter();
    
}

