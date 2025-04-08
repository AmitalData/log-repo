import { Component } from '@angular/core';
import { DocumentsMetaDataTypePM } from 'Common/EntityPMs/DocumentsMetaDataTypePM';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { DocumentsMetaDataTypePMService } from 'Common/Services/StandardPMs/DocumentsMetaDataTypePMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'app-new-documents-metadata-type',
    templateUrl: './NewDocumentsMetadataTypeComponent.html',
    styles: [ `
        .view-container {
            display: block;
            margin-top: 10px;
        }

        .view-container .field-container {
            width: 600px;
            margin: 15px;
            display: flex;
        }

        .view-container .field-container LogLabel {
            flex: 0 0 175px;
        }

        ValidationSummary {
            height: 26px;
            overflow: auto;
            display: block;
            margin-top: 10px;
        }
    `]
})
export class NewDocumentsMetadataTypeComponent extends BaseComponent {
    ObjectTableName: string = 'DocumentsMetaDataType';
    documentsMetaDataTypePM: DocumentsMetaDataTypePM = new DocumentsMetaDataTypePM();
    dataReady: boolean = false;
    validationErrorsList: string[] = [];
    isUpdate: boolean = false;
    tableDataInit: boolean = false;

    constructor(public entityArgs: EntityArgs) {
        super();

        new EntityResourceService().getEntityResourceByTableName("DocumentsMetaDataType").subscribe(() => this.tableDataInit = true);

        if (entityArgs.EntityPM)
            this.documentsMetaDataTypePM = entityArgs.EntityPM;
        else
            this.documentsMetaDataTypePM.Tenant = SessionLocator.Tenant;
        
        this.isUpdate = !!entityArgs.EntityPM;
        this.dataReady = true;
    }

    SetNewWizardArgs(args: any): void {}

    close(save: boolean) {
        if (save)
            this.saveData();
        else
            SessionLocator.SelectedSession.CurrentWindow.Close(null);
    }

    async saveData() {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        const result: ServiceResponse = await new Promise<ServiceResponse>(res => 
            new DocumentsMetaDataTypePMService().insert(this.documentsMetaDataTypePM).subscribe((myResult: ServiceResponse) => res(myResult)));

        SessionLocator.SelectedSession.StopBusyIndicator();

        if (result.HasError)
            this.validationErrorsList = result.ErrorsArray;
        else
            SessionLocator.SelectedSession.CurrentWindow.Close('');
    }

}