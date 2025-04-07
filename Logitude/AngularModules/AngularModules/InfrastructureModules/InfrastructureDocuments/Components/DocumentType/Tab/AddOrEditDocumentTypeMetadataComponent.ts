import { Component } from '@angular/core';
import { DocumentTypeMetaDataPM } from 'Common/EntityPMs/DocumentTypeMetaDataPM';
import { DocumentTypeMetaDataPMService } from 'Common/Services/StandardPMs/DocumentTypeMetaDataPMService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'app-add-or-edit-document-type-metadata',
    templateUrl: './AddOrEditDocumentTypeMetadataComponent.html',
    styleUrls: ['./AddOrEditDocumentTypeMetadataComponent.scss']
})
export class AddOrEditDocumentTypeMetadataComponent {
    tableName: string = 'DocumentTypeMetaData';
    documentTypeMetaData: DocumentTypeMetaDataPM;
    documentTypeID: string;
    dataReady: boolean = false;
    validationErrorsList: string[] = [];
    isUpdate: boolean = false;
    documentsMetaDataTypeFilter: ApiQueryFilters = new ApiQueryFilters();

    SetWindowArgs({ documentTypeID, existsMetadataIds = null, documentTypeMetaData = null }: { documentTypeID: string, existsMetadataIds: string[], documentTypeMetaData: DocumentTypeMetaDataPM }) {
        if (documentTypeMetaData)
            this.documentTypeMetaData = documentTypeMetaData;
        else {
            this.documentTypeMetaData = new DocumentTypeMetaDataPM();
            this.documentTypeMetaData.DocumentTypeId = documentTypeID;
            this.documentTypeMetaData.Tenant = SessionLocator.Tenant;
        }
        this.isUpdate = !!documentTypeMetaData;
        this.documentTypeMetaData.Mandatory = !!this.documentTypeMetaData.Mandatory;
        this.documentTypeID = documentTypeID;
        this.documentsMetaDataTypeFilter.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number", false);
        if(existsMetadataIds?.length > 0)
            this.documentsMetaDataTypeFilter.addAdditionalFilter("Id", existsMetadataIds.join(','), null, null, "Exclude", false, false, false, "string", false, true);
        this.dataReady = true;
    }

    close(save: boolean) {
        if (save)
            this.saveData();
        else
            SessionLocator.SelectedSession.CurrentWindow.Close(null);
    }

    async saveData() {
        if (this.documentTypeMetaData.DocumentTypeId == null) {
            this.validationErrorsList.push(TextCodeTranslator.Translate('General.M.FieldIsRequired').replace(
                '%FieldName',
                TextCodeTranslator.Translate(this.tableName + '.F.DocumentTypeId')
            ));

            return;
        }

        delete this.documentTypeMetaData.UIProperties;
        const serviceApiClient = new DocumentTypeMetaDataPMService();

        SessionLocator.SelectedSession.StartBusyIndicator('');
        const observable = this.isUpdate ? serviceApiClient.update(this.documentTypeMetaData) : serviceApiClient.insert(this.documentTypeMetaData);
        const res: boolean = await new Promise(resolve => observable.subscribe((response: ServiceResponse) => resolve(!response.HasError)));
        
        SessionLocator.SelectedSession.StopBusyIndicator();

        SessionLocator.SelectedSession.CurrentWindow.Close(res as any);
    }
}