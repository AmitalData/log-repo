import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class BatchPrintService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BatchPrint';
    }


    Print(entity: BatchPrintManagerArgs) {
        return defer(() => {

            var mappedEntity: BatchPrintManagerArgs = this.MapJsonToBatchPrintManagerArgs(entity, false);

            return this._httpClient.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                var result = response;
                var pmresponse: ServiceResponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToBatchPrintManagerArgs(jsonPM: any, getCallMap: boolean = true, entity: BatchPrintManagerArgs = null) {
        if (!entity) {
            entity = new BatchPrintManagerArgs();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else if (property === "EntityIds") {

                entity.EntityIds = new Array<PrintEntityKeys>();
                for (var item in jsonPM.EntityIds) {
                    var jItem = jsonPM.EntityIds[item];

                    var newItemPM: PrintEntityKeys;
                    newItemPM = this.MapPrintEntityKeys(jItem);
                    entity.EntityIds.push(newItemPM);
                }
            }

            else {
                entity[property] = jsonPM[property];
            }
        }

        return entity;
    }
    MapPrintEntityKeys(jsonPM: any, mapParent: boolean = true, entityPM: PrintEntityKeys = null) {
        if (!entityPM) {
            entityPM = new PrintEntityKeys();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
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

export class BatchPrintManagerArgs {
    public DocumentTypeId: string;
    public TemplateId: string;
    public CopyId: string;
    public ObjectTableId: string;
    public Tenant: number;
    public EntityIds: PrintEntityKeys[];
    public ChildObjectTableId: string;
}

export class PrintEntityKeys {
    public EntityId: string;
    public EntityNumber: string;
    public ChildEntityId: string;
    public ObjectTableId: string;
}

export class PrintingResult {
    public DocumentId: string;
    public FileName: string;
    public SecurityId: string;
    public NotValidRows: PrintingRow[];
}

export class PrintingRow {
    public Error: string;
    public EntityId: string;
    public EntityNumber: string;
}
