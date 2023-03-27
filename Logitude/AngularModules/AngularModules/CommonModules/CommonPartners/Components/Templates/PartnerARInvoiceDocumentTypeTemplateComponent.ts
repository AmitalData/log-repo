import { Component } from '@angular/core';
import { DocumentTypeListService } from '../../../../Common/Services/StandardLists/DocumentTypeListService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
declare var window: any
@Component({

    templateUrl: './PartnerARInvoiceDocumentTypeTemplateComponent.html',
})

export class PartnerARInvoiceDocumentTypeTemplateComponent extends BaseComponent{
    public EntityPM: any;
    private documentTypeListService: DocumentTypeListService;
    public SingleInvoiceFilter: ApiQueryFilters;
    public CustomsInvoiceFilter: ApiQueryFilters;
    public ConsolidationInvoiceFilter: ApiQueryFilters;
    public ManifestInvoiceFilter: ApiQueryFilters;
    public ShowSingleInvoice: boolean;
    public ShowCustomsInvoice: boolean;
    public ShowConsolidationInvoice: boolean;
    public ShowManifestInvoice: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    constructor() {
        super();
    }
    Run(entityPM: any) {
        this.EntityPM = entityPM;
        this.Initialization();
        this.LoadARInvoiceDocumentTypes();
        this.ValidateARInvoiceDocumentTypes();
    }
    Initialization() {
        this.documentTypeListService = new DocumentTypeListService();
        this.SingleInvoiceFilter = new ApiQueryFilters();
        this.CustomsInvoiceFilter = new ApiQueryFilters();
        this.ConsolidationInvoiceFilter = new ApiQueryFilters();
        this.ManifestInvoiceFilter = new ApiQueryFilters();
    }
    ARInvoiceDocumentTypes: any[] = [];
    private LoadARInvoiceDocumentTypes() {
        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionLocator.Tenant;
        apiQueryFilters.ObjectTableName = "DocumentType";

        let ARInvoiceObjectTableId = window.ObjectTables.filter(f => f.Name == "ARInvoice")[0].Id;
        apiQueryFilters.addAdditionalFilter("ObjectTableId", ARInvoiceObjectTableId, null, null, "Equals", false, false, false, "string");
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (pmResponse.HasError || !pmResponse.Result) {
                this.CurrentSession.StopBusyIndicator();
                return;
            }
            this.ARInvoiceDocumentTypes = pmResponse.Result;
            this.SetDocumentTypeARInvoiceFields();
            this.CurrentSession.StopBusyIndicator();
        });
    }
    IsDocumentTemplateAreaReady: boolean = false;
    SetDocumentTypeARInvoiceFields() {
        this.SingleInvoiceTemplateId = this.EntityPM.Card.SingleInvoiceTemplateId;
        this.CustomsInvoiceTemplateId = this.EntityPM.Card.CustomsInvoiceTemplateId;
        this.ConsolidationInvoiceTemplateId = this.EntityPM.Card.ConsolidationInvoiceTemplateId;
        this.ManifestInvoiceTemplateId = this.EntityPM.Card.ManifestInvoiceTemplateId;
        this.InitLOVFilters();
        this.IsDocumentTemplateAreaReady = true;
    }

    GetDocumentTypeIdByCode(documentTypeCode: string) {
        return this.ARInvoiceDocumentTypes.filter(d => d.Code == documentTypeCode)[0]?.Id;
    }
    IsDoumentVailable(documentTypeCode: string) {
        if (this.GetDocumentTypeIdByCode(documentTypeCode) == undefined || this.GetDocumentTypeIdByCode(documentTypeCode) == null)
            return false;
        return true;
    }
    ValidateARInvoiceDocumentTypes() {
        this.ShowSingleInvoice = this.IsDoumentVailable("999S");
        this.ShowCustomsInvoice = this.IsDoumentVailable("999CI");
        this.ShowConsolidationInvoice = this.IsDoumentVailable("999C");
        this.ShowManifestInvoice = this.IsDoumentVailable("999M");
    }
    InitLOVFilters() {
        this.SingleInvoiceFilter.addAdditionalFilter("DocumentTypeId", this.GetDocumentTypeIdByCode("999S"), null, null, "Equals", false, false, false, "string");
        this.CustomsInvoiceFilter.addAdditionalFilter("DocumentTypeId", this.GetDocumentTypeIdByCode("999CI"), null, null, "Equals", false, false, false, "string");
        this.ConsolidationInvoiceFilter.addAdditionalFilter("DocumentTypeId", this.GetDocumentTypeIdByCode("999C"), null, null, "Equals", false, false, false, "string");
        this.ManifestInvoiceFilter.addAdditionalFilter("DocumentTypeId", this.GetDocumentTypeIdByCode("999M"), null, null, "Equals", false, false, false, "string");
    }

    CheckIfDefaultChanged(DocumentTypeCode: string, documentTypeTemplateId: string) {
        let documentType = this.ARInvoiceDocumentTypes.filter(d => d.Code == DocumentTypeCode)[0];
        if (!documentType) return false;
        return (documentType.DocumentTypeDefaultReportTemplateId != documentTypeTemplateId);
    }

    IsHaveARInvoicePrintToogleFeature() {
        return SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "ARP")[0];
    }

    get SingleInvoiceTemplateId() { return this.EntityPM.Card.SingleInvoiceTemplateId; }
    set SingleInvoiceTemplateId(newValue: string) {
        if (this.EntityPM.Card.SingleInvoiceTemplateId != newValue) {
            this.EntityPM.Card.SingleInvoiceTemplateId = newValue;
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
        }
    }
    get CustomsInvoiceTemplateId() { return this.EntityPM.Card.CustomsInvoiceTemplateId; }
    set CustomsInvoiceTemplateId(newValue: string) {
        if (this.EntityPM.Card.CustomsInvoiceTemplateId != newValue) {
            this.EntityPM.Card.CustomsInvoiceTemplateId = newValue;
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
        }
    }
    get ConsolidationInvoiceTemplateId() { return this.EntityPM.Card.ConsolidationInvoiceTemplateId; }
    set ConsolidationInvoiceTemplateId(newValue: string) {
        if (this.EntityPM.Card.ConsolidationInvoiceTemplateId != newValue) {
            this.EntityPM.Card.ConsolidationInvoiceTemplateId = newValue;
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
        }
    }
    get ManifestInvoiceTemplateId() { return this.EntityPM.Card.ManifestInvoiceTemplateId; }
    set ManifestInvoiceTemplateId(newValue: string) {
        if (this.EntityPM.Card.ManifestInvoiceTemplateId != newValue) {
            this.EntityPM.Card.ManifestInvoiceTemplateId = newValue;
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
        }
    }
}
