import { Component } from "@angular/core";
import { DocumentTypeMetaDataPM } from "Common/EntityPMs/DocumentTypeMetaDataPM";
import { DocumentTypePM } from "Common/EntityPMs/DocumentTypePM";
import { DocumentTypeMetaDataExtendedService } from "Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService";
import { DocumentsMetaDataTypePMService } from "Common/Services/StandardPMs/DocumentsMetaDataTypePMService";
import { DocumentTypeMetaDataPMService } from "Common/Services/StandardPMs/DocumentTypeMetaDataPMService";
import { ConfirmWindow } from "Controls/Windows/ConfirmWindow";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { LogtuideTableDataService } from "Infrastructure/Services/logtuide-table-data.service";
import { ObservableCollection } from "Infrastructure/Utilities/ObservableCollection";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { FieldData } from "InfrastructureModules/InfrastructureOthers/AmitalAPI/amitalApiTypes";
import { TextBoxField } from "InfrastructureModules/InfrastructureOthers/AmitalAPI/components/LogTexBoxFormComponent";
import { fieldsError } from "InfrastructureModules/InfrastructureOthers/AmitalAPI/WindowsComponent/AmitalAPIAddWindowService";

@Component({
    selector: 'document-type-metadata',
    templateUrl: './DocumentTypeMetadataComponent.html',
    styleUrls: ['./DocumentTypeMetadataComponent.scss']
})
export class DocumentTypeMetadataComponent {
    documentType: DocumentTypePM;
    documentTypeMetaDataExtendedService: DocumentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService();
    documentsMetaDataTypeService: DocumentsMetaDataTypePMService = new DocumentsMetaDataTypePMService();
    documentTypeMetaDataService: DocumentTypeMetaDataPMService = new DocumentTypeMetaDataPMService();
    schemaDataSource: ObservableCollection = new ObservableCollection([]);
    schemaTableReady: boolean = false;
    schemaColumns: (FieldData & { width: string })[] = [
        // { name: 'Id', label: 'Id', width: '37' },
        // { name: 'tenants', label: 'Tenants', width: '60' },
        { name: 'DocumentsMetaDataTypeCode', label: 'Code', width: '100' },
        { name: 'DocumentsMetaDataTypeEnglishName', label: 'English Name', width: '150' },
        { name: 'DocumentsMetaDataTypeLocalName', label: 'Local Name', width: '150' },
        { name: 'DocumentsMetaDataTypeFormat', label: 'Format', width: '100' },
        { name: 'Mandatory', label: 'Mandatory', width: '100' },
    ];

    constructor(private entityArgs: EntityArgs,) {
        if (this.entityArgs) {
            this.documentType = this.entityArgs.EntityPM
            this.Run();
        }
    }

    async Run() {
        console.log('************* Run');
        await this.initTable();
    }
    
    async initTable() {
        SessionLocator.SelectedSession.StartBusyIndicator('');

        console.log('************* initTable', this.entityArgs);
        this.schemaTableReady = false;
        
        const documentTypeMetaData: DocumentTypeMetaDataPM[] = await new Promise((resolve, reject) => {
            this.documentTypeMetaDataExtendedService.GetDocumentTypeMetaDataByDocumentTypeId(this.documentType.Id, SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
                resolve(response.Result);
            });
        });
        this.schemaDataSource = new ObservableCollection(documentTypeMetaData);
        this.schemaTableReady = true;

        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    async openRemovePopup(id: string) {
        const win = new ConfirmWindow();
        win.Show(TextCodeTranslator.Translate('Accounting.General.O.Areyousuredeleteline'));
        const deleteConfirm: boolean = await win.WindowClosedPromise() as boolean;

        if (!deleteConfirm) return;

        SessionLocator.SelectedSession.StartBusyIndicator('');
        try {
            await new Promise<void>(resolve => this.documentTypeMetaDataExtendedService.Delete(id, SessionLocator.Tenant).subscribe(() => resolve()));
        } catch (error) { }
        SessionLocator.SelectedSession.StopBusyIndicator();

        this.initTable();
    }

    async openAddEditPopup(orginalRow: any = null) {
        const window = new LogitudeWindow();
        window.WindowArgs = { 
            documentTypeID: this.documentType.Id, 
            existsMetadataIds: this.schemaDataSource.Collection.map((d: DocumentTypeMetaDataPM) => d.DocumentsMetaDataTypeId),
            documentTypeMetaData: orginalRow,
        };
        window.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/Tab/AddOrEditDocumentTypeMetadataComponent');
        const res = await new Promise(resolve => window.WindowClosed.subscribe((result: any) => resolve(result)));
        if(res)
            this.initTable();        
    }   
}