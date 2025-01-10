import { ChangeDetectorRef, Component } from "@angular/core";
import { DocumentsFilingMetaDataValuePM } from "Common/EntityPMs/DocumentsFilingMetaDataValuePM";
import { DocumentsFilingPM } from "Common/EntityPMs/DocumentsFilingPM";
import { DocumentTypeMetaDataPM } from "Common/EntityPMs/DocumentTypeMetaDataPM";
import { CommonDomainService } from "Common/Services/CommonDomainService";
import { DocumentsFilingExtendedPMService } from "Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService";
import { DocumentTypeMetaDataExtendedService } from "Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService";
import { DocumentsFilingPMService } from "Common/Services/StandardPMs/DocumentsFilingPMService";
import { LogitudeWindow, LogitudeWindowTemplateComponent } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { TraceEventTypeCodes, TraceEventExtendedPMService } from "Infrastructure/Services/ExtendedPMs/TraceEventExtendedPMService";
import { LogtuideTableDataService } from "Infrastructure/Services/logtuide-table-data.service";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { Observable } from "rxjs";

@Component({
    selector: "app-new-documents-filing",
    templateUrl: "./DocumentsFilingComponent.html",
    styleUrls: ["./DocumentsFilingComponent.scss"]
})
export class DocumentsFilingComponent {
    documentsFilingPM: DocumentsFilingPM = new DocumentsFilingPM();
    fileData: string = '';
    base64File: string = '';
    allowedExtensions: string[] = ['pdf', 'tif', 'tiff', 'jpg', 'jpeg', 'gif', 'bmp', 'png', 'xml', 'xlsx', 'xls', 'json'];
    documentTypeMetaDataList: DocumentTypeMetaDataPM[];
    documentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService();
    isNew: boolean = false;
    isEditComponent: boolean = true;
    dataReady: boolean = false;
    tableDataInit: boolean = false;
    errors: string[] = [];
    inputFileWidth: number = 400;
    inputFileHeight: number = 560;
    inSavingProcess: boolean = false;
    directionCodes: { name: string, id: string }[] = [
        { name: 'In', id: 'I' },
        { name: 'Out', id: 'O' }
    ];
    selectedSession = SessionLocator.SelectedSession;
    dataInitProcess: boolean = false;
    finishInitialCdr: boolean = false;

    constructor(entityArgs: EntityArgs, private cdr: ChangeDetectorRef) {
        this.isEditComponent = !!SessionLocator.SelectedSession.CurrentEditComponent;
        this.listner();

        if (this.isEditComponent) {
            this.inputFileHeight = 700;
            this.inputFileWidth = 580;
        } else {
            (<LogitudeWindowTemplateComponent>SessionLocator.SelectedSession.CurrentWindow?.ComponentRef.instance).logWindow.Height = 700;
            (<LogitudeWindowTemplateComponent>SessionLocator.SelectedSession.CurrentWindow?.ComponentRef.instance).SetWindowSize();
        }

        this.initObjectTable();

        if (entityArgs.EntityPM)
            this.initExistsData(entityArgs.EntityPM);
    }

    ngAfterViewInit() {
        console.log('ngAfterViewInit *********');
    }

    listner(): void {
        if (this.isEditComponent) {
            SessionLocator.SelectedSession.CurrentEditComponent.SaveStart.subscribe(() => this.inSavingProcess = true);

            SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe(async (isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.sendTraceEvent(this.documentsFilingPM.Id);
                    await this.documentTypeChanged(true);
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

    initExistsData(documentsFilingPM: DocumentsFilingPM): void {
        this.initDocument(documentsFilingPM.DocumentId);
        this.initData(documentsFilingPM);
    }

    initData(documentsFilingPM: DocumentsFilingPM): void {
        this.documentsFilingPM = documentsFilingPM;
        this.documentTypeChanged(true);
        this.documentsFilingPM.UIProperties = new UIProperties();
        this.documentsFilingPM['_isDFComponent'] = true;
        this.dataReady = true;

        setTimeout(() => {
            this.cdr.detectChanges();
            this.finishInitialCdr = true;
        }, 0);
    }

    async initDocument(documentId: string): Promise<void> {
        const documentBase64: string =
            await LogtuideTableDataService.createInstance().getDataFromService(
                new CommonDomainService().GetFilingAttachPdfReport(documentId));

        this.fileData = documentBase64;
    }

    onFileSelected(file: FileList | File): void {
        if (!file || (file instanceof FileList && file.length == 0)) return;
        if (file instanceof FileList) file = file[0];

        this.documentsFilingPM.FileName = file.name;
        this.documentsFilingPM.FileExtension = file.name.split('.').pop();

        const reader = new FileReader();
        reader.onload = (e: any) => {
            const blob = new Blob([e.target.result], { type: file.type });
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

        if (!initialSelected && this.finishInitialCdr) {
            console.log('documentTypeMetaDataList *********');
            this.documentsFilingPM.DocumentsFilingMetaDataValues = [];
        }

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
            this.documentTypeMetaDataList.find(a => a.DocumentsMetaDataTypeId == item.DocumentsMetaDataTypeId).DocumentsFilingMetaDataValuePM = value;
        });

        this.cdr.detectChanges();
        this.dataInitProcess = false;

        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    async sendToServer(): Promise<boolean> {
        SessionLocator.SelectedSession.StartBusyIndicator('');

        const serviceRequest: Observable<ServiceResponse> = this.isNew ?
            new DocumentsFilingExtendedPMService().PostDocumentAndDocumentFiling(this.documentsFilingPM, this.base64File) :
            new DocumentsFilingPMService().update(this.documentsFilingPM);

        const result = await new Promise<ServiceResponse>(res => serviceRequest.subscribe((myResult: ServiceResponse) => res(myResult)));
        if (result.HasError)
            this.errors = result.ErrorsArray;
        else 
            this.sendTraceEvent(result.Result.Id);

        SessionLocator.SelectedSession.StopBusyIndicator();

        return !result.HasError;
    }

    metadataChanged(): void {
        if (!this.dataInitProcess)
            this.documentsFilingPM.IsDirty = true;
    }

    sendTraceEvent(documentsFilingId: string): void {        
        const traceEventTypeCode = this.isNew ? TraceEventTypeCodes.CREATE : TraceEventTypeCodes.UPDATE;
        documentsFilingId = documentsFilingId.substring(0, 15);
        new TraceEventExtendedPMService().CreateTraceEvent(SessionLocator.Tenant, documentsFilingId, 'DocumentsFiling', traceEventTypeCode).subscribe();
    }
}