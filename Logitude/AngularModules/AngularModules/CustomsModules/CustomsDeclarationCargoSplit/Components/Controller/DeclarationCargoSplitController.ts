declare var window;
import { DeclarationCargoSplitPM } from "../../../../Customs/EntityPMs/DeclarationCargoSplitPM";
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {EventEmitter} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { SessionLocator } from "../../../../Infrastructure/Utilities/SessionLocator";

export class DeclarationCargoSplitController {
  private IsDisplayOnly: boolean;
  private http: HttpClient;
    private apiUrl: string;
    private tenant: number;

  constructor(private DeclarationCargoSplitPM: DeclarationCargoSplitPM) {
    this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestSheetExtended';
      this.http = ServiceHelper.HttpClient;
      this.tenant = SessionLocator.Tenant;
  }



  DisplayOnlyCheck() {
    var rresponse: ServiceResponse = new ServiceResponse();
    var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
    return defer(() => {
      var rresponse: ServiceResponse = new ServiceResponse();
      rresponse.Result = { IsDisplayOnly: false, DisplayOnlyMessage: "" };
      return of(rresponse);
    });
  }

  CheckRequestsInProgress(declarationId: string) {
    // Request sheets in progress check

    var table = window.ObjectTables.filter(d => d.Name === "Customs.DeclarationCargoSplit")[0];
    var table2 = window.ObjectTables.filter(d => d.Name === "Customs.Declaration")[0];

    return defer(() => {
        return this.http.get(this.apiUrl + '/GetRequestInProgress/?' + 'tenant=' + this.tenant + '&interfaceTypeCode=8370' + '&objectTableId1=' + table.Id + '&entityId1=' + this.DeclarationCargoSplitPM.Id + '&objectTableId2=' + table2.Id + '&entityId2=' + encodeURIComponent(declarationId) + '&customFileNo=' + "" + '&displayOnlyMode= false', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
        var serviceResponse: ServiceResponse = new ServiceResponse();
        var requestSheets:any = response;
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
      }), catchError(ServiceHelper.HandleServiceError));
    }

    );
  }

  public SelectionCompleted: EventEmitter<any> = new EventEmitter();

}

