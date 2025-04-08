import { ChangeDetectorRef, Component } from "@angular/core";
import { DocumentsFilingMetaDataValuePM } from "Common/EntityPMs/DocumentsFilingMetaDataValuePM";
import { DocumentsFilingPM } from "Common/EntityPMs/DocumentsFilingPM";
import { DocumentTypeMetaDataPM } from "Common/EntityPMs/DocumentTypeMetaDataPM";
import { CommonDomainService } from "Common/Services/CommonDomainService";
import { DocumentsFilingExtendedPMService } from "Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService";
import { DocumentTypeMetaDataExtendedService } from "Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService";
import { DocumentsFilingPMService } from "Common/Services/StandardPMs/DocumentsFilingPMService";
import { LogitudeWindowTemplateComponent } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { TraceEventTypeCodes, TraceEventExtendedPMService } from "Infrastructure/Services/ExtendedPMs/TraceEventExtendedPMService";
import { LogtuideTableDataService } from "Infrastructure/Services/logtuide-table-data.service";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { Observable } from "rxjs";

@Component({
    selector: "app-new-documents-filing",
    templateUrl: "./DocumentsFilingComponent.html",
    styleUrls: ["./DocumentsFilingComponent.scss"]
})
export class DocumentsFilingComponent {
    ObjectTableName: string = 'DocumentsFiling';
    documentsFilingPM: DocumentsFilingPM = new DocumentsFilingPM();
    orginalDocumentsFilingPM: DocumentsFilingPM & any = null;
    fileData: string = '';
    base64File: string = '';
    rotationAngle: number = 0;
    allowedExtensions: string[] = ['csv', 'jpg', 'jpeg', 'tif', 'tiff', 'txt', 'zip', 'png', 'gif', 'bmp', 'doc', 'xls', 'ppt', 'docx', 'xlsx', 'pptx', 'pdf', '7z', 'html', 'htm'];
    documentTypeMetaDataList: DocumentTypeMetaDataPM[];
    documentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService();
    isNew: boolean = false;
    isEditComponent: boolean = true;
    dataReady: boolean = false;
    tableDataInit: boolean = false;
    errors: string[] = [];
    inputFileWidth: number = 580;
    inputFileHeight: number = 700;
    inSavingProcess: boolean = false;
    directionCodes: { name: string, id: string }[] = [
        { name: 'In', id: 'I' },
        { name: 'Out', id: 'O' }
    ];
    entities: string[]
    selectedSession = SessionLocator.SelectedSession;
    dataInitProcess: boolean = false;
    finishInitialCdr: boolean = false;
    objectTablesFilter: ApiQueryFilters = this.getObjectTablesFilter();
    documentTypeFilter: ApiQueryFilters = this.getDocumentTypeFilter();

    getObjectTablesFilter() {
        const filter = new ApiQueryFilters();
        const tables: string[] = ['Customs.Declaration', 'Shipment'];
        filter.addAdditionalFilter("Name", tables.join(','), null, null, "InListExact", false, false, false, "string", false, true);
        return filter;
    }
    
    getDocumentTypeFilter() {
        const filter = new ApiQueryFilters();        
        filter.addAdditionalFilter("IsDocIn", true, null, null, "Equals", false, false, false, "boolean", false);
        return filter;
    }

    constructor(entityArgs: EntityArgs, private cdr: ChangeDetectorRef) {
        this.isEditComponent = !!SessionLocator.SelectedSession.CurrentEditComponent;
        this.listner();

        if (!this.isEditComponent) {
            (<LogitudeWindowTemplateComponent>SessionLocator.SelectedSession.CurrentWindow?.ComponentRef.instance).logWindow.Height = 840;
            (<LogitudeWindowTemplateComponent>SessionLocator.SelectedSession.CurrentWindow?.ComponentRef.instance).logWindow.Width = 1090;
            (<LogitudeWindowTemplateComponent>SessionLocator.SelectedSession.CurrentWindow?.ComponentRef.instance).SetWindowSize();
        }

        this.initObjectTable();

        if (entityArgs.EntityPM)
            this.initExistsData(entityArgs.EntityPM);
    }

    listner(): void {
        if (this.isEditComponent) {
            SessionLocator.SelectedSession.CurrentEditComponent.SaveStart.subscribe(() => this.inSavingProcess = true);

            SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe(async (isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.sendTraceEvent(this.documentsFilingPM.DocumentId);
                    await this.documentTypeChanged(true);
                    this.orginalDocumentsFilingPM = JSON.parse(JSON.stringify(new DocumentsFilingPMService().MapJsonToEntityPM(this.documentsFilingPM)));
                }

                this.inSavingProcess = false;
            });
        }
    }

    async initObjectTable() {
        await Promise.all([
            await new Promise<void>(res => new EntityResourceService().getEntityResourceByTableName("Customs.Declaration").subscribe((myResult: ServiceResponse) => res())),
            await new Promise<void>(res => new EntityResourceService().getEntityResourceByTableName("Customs.CustomsDocument").subscribe((myResult: ServiceResponse) => res())),
        ]);
        this.tableDataInit = true;
    }

    SetNewWizardArgs(args: any): void {
        this.initNewData();
    }

    initNewData(): void {
        const documentsFilingPM: DocumentsFilingPM = new DocumentsFilingPM();
        documentsFilingPM.Tenant = SessionLocator.Tenant;
        documentsFilingPM.DirectionCode = 'I';
        this.isNew = true;
        this.initData(documentsFilingPM);
    }

    async initExistsData(documentsFilingPM: DocumentsFilingPM): Promise<void> {
        this.initDocument(documentsFilingPM.DocumentId);
        await this.initData(documentsFilingPM);
        this.orginalDocumentsFilingPM = JSON.parse(JSON.stringify(new DocumentsFilingPMService().MapJsonToEntityPM(documentsFilingPM)));
    }

    async initData(documentsFilingPM: DocumentsFilingPM): Promise<void> {
        this.documentsFilingPM = documentsFilingPM;
        this.documentsFilingPM.UIProperties = new UIProperties();
        this.documentsFilingPM['_isDFComponent'] = true;
        this.dataReady = true;
        await this.documentTypeChanged(true);

        setTimeout(() => {
            this.cdr.detectChanges();
            this.finishInitialCdr = true;
        }, 0);
    }

    async initDocument(documentId: string): Promise<void> {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        const documentBase64: string =
            await LogtuideTableDataService.createInstance().getDataFromService(
                new CommonDomainService().GetFilingAttachPdfReport(documentId));

        this.fileData = documentBase64;
        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    onFileSelected(file: FileList | File): void {
        if (!file || (file instanceof FileList && file.length == 0)) return;
        if (file instanceof FileList) file = file[0];

        this.documentsFilingPM.FileName = file.name;
        this.documentsFilingPM.FileExtension = file.name.split('.').pop();

        const reader = new FileReader();
        reader.onload = (e: any) => {
            const blob = new Blob([e.target.result], { type: (<File>file)?.type });
            this.fileData = URL.createObjectURL(blob);
        }
        reader.readAsArrayBuffer(file);
    }

    onFileCleared() {
        this.fileData = '';
        this.documentsFilingPM.FileName = '';
        this.documentsFilingPM.FileExtension = '';
    }

    async close(save: boolean): Promise<void> {
        if (save) {
            const success: boolean = await this.sendToServer();
            if (!success) return;
        }

        SessionLocator.SelectedSession?.CloseCurrentWindow();
    }

    async documentTypeChanged(initialSelected: boolean, e: any = null): Promise<void> {
        this.dataInitProcess = true;
        SessionLocator.SelectedSession.StartBusyIndicator('');

        this.documentTypeMetaDataList = await new Promise<DocumentTypeMetaDataPM[]>(resolve =>
            this.documentTypeMetaDataExtendedService.GetDocumentTypeMetaDataByDocumentTypeId(this.documentsFilingPM.DocumentTypeId, SessionLocator.Tenant).subscribe((myResult: ServiceResponse) =>
                resolve(myResult.Result as DocumentTypeMetaDataPM[])));

        if (!initialSelected && this.finishInitialCdr)
            this.documentsFilingPM.DocumentsFilingMetaDataValues = [];        

        this.documentTypeMetaDataList.forEach((item) => {
            let value: DocumentsFilingMetaDataValuePM & any = this.documentsFilingPM.DocumentsFilingMetaDataValues?.find(a => a.DocumentsMetaDataTypeId == item.DocumentsMetaDataTypeId);
            if (!value) {
                value = new DocumentsFilingMetaDataValuePM(this.documentsFilingPM);
                value.DocumentsMetaDataTypeId = item.DocumentsMetaDataTypeId;
                value.Tenant = SessionLocator.Tenant;
                value.DocumentsFilingId = this.documentsFilingPM.Id;
                value.ChangeSetOp = "Insert";

                this.documentsFilingPM.DocumentsFilingMetaDataValues.push(value);
            } else {
                // this.documentsFilingPM.DocumentsFilingMetaDataValues.forEach((item) => {
                // item.UIProperties = new UIProperties;
                // value = item;
                value.ChangeSetOp = "Update";
                value.OldEntityPM = item;
            }
            value.Mandatory = item.Mandatory;
            value.DocumentsMetaDataTypeEnglishName = item.DocumentsMetaDataTypeEnglishName;
            value.DocumentsMetaDataTypeFormat = item.DocumentsMetaDataTypeFormat;
            (<any>this.documentTypeMetaDataList.find(a => a.DocumentsMetaDataTypeId == item.DocumentsMetaDataTypeId)).DocumentsFilingMetaDataValuePM = value;
        });

        this.cdr.detectChanges();
        this.dataInitProcess = false;

        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    async sendToServer(): Promise<boolean> {
        SessionLocator.SelectedSession.StartBusyIndicator('');

        const serviceRequest: Observable<ServiceResponse> = this.isNew ?
            new DocumentsFilingExtendedPMService().PostDocumentAndDocumentFiling(this.documentsFilingPM, this.base64File, this.rotationAngle) :
            new DocumentsFilingPMService().update(this.documentsFilingPM);

        const result = await new Promise<ServiceResponse>(res => serviceRequest.subscribe((myResult: ServiceResponse) => res(myResult)));
        if (result.HasError)
            this.errors = result.ErrorsArray;
        else 
            this.sendTraceEvent((<DocumentsFilingPM>result.Result).DocumentId);

        SessionLocator.SelectedSession.StopBusyIndicator();

        return !result.HasError;
    }

    metadataChanged(): void {
        if (!this.dataInitProcess)
            this.documentsFilingPM.IsDirty = true;
    }

    sendTraceEvent(documentsId: string): void {        
        const traceEventTypeCode = this.isNew ? TraceEventTypeCodes.CREATE : TraceEventTypeCodes.UPDATE;
        const notes: string = this.isNew ? '' : this.calculateChanges();
        new TraceEventExtendedPMService().CreateTraceEvent(SessionLocator.Tenant, documentsId, 'DocumentsFiling', traceEventTypeCode, '', notes).subscribe();
    }

    calculateChanges(): string {
        let changes: string = '';
        this.documentsFilingPM.DocumentsFilingMetaDataValues.forEach((newValue) => {
            const originalValue: DocumentsFilingMetaDataValuePM & any = this.orginalDocumentsFilingPM.documentsFilingMetaDataValues?.find(a => a.id == newValue.Id);
            if (originalValue.metaDataValue !== newValue.MetaDataValue)
                changes += `Field: ${originalValue.DocumentsMetaDataTypeEnglishName} - Old Value: ${originalValue.metaDataValue} - New Value: ${newValue.MetaDataValue}, `;            
        });

        const filedNames: string[] = ['documentTypeId', 'fileName', 'description', 'notes'];
        filedNames.forEach((field) => {
            if (this.documentsFilingPM[field] !== this.orginalDocumentsFilingPM[field])
                changes += `Field: ${field} - Old Value: ${this.orginalDocumentsFilingPM[field]} - New Value: ${this.documentsFilingPM[field]}, `;
        });

        return changes;
    }
}
