import {Component, OnInit, EventEmitter}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../Infrastructure/Locators/ObjectsLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {SharedLogisticsService} from '../Services/Others/SharedLogisticsService';
import {TenantPMService} from '../../Common/Services/StandardPMs/TenantPMService';
import {TenantPM} from '../../Common/EntityPMs/TenantPM';
import {Cloner} from '../../Infrastructure/Utilities/Cloner';

@Component({
    selector: 'SharedLogisticsSetting',
    templateUrl: './SharedLogisticsSettingComponent.html',
    inputs: ['TenantPM', 'OnCloseWindowEvent'],
    providers: [SharedLogisticsService]
})

export class SharedLogisticsSettingComponent implements OnInit {
    SelectedTabCode: string;

    public get IsSharedLogisticsActivated() {

        if (this.TenantPM) {
            return this.TenantPM.IsSharedLogisticsActivated;
        }
        else {
            return false;
        }
    }

    public set IsSharedLogisticsActivated(value: boolean) {
        if (this.TenantPM != null) {
            if (this.TenantPM.IsSharedLogisticsActivated != value)
            {
                this.TenantPM.IsSharedLogisticsActivated = value;
                this.SetPropertiesEnable();
            }
        }
    }

    public get IsWebAccessActivated() {
        if (this.TenantPM) {
            return this.TenantPM.IsWebAccessActivated;
        }
        else {
            return false;
        }
    }

    public set IsWebAccessActivated(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.IsWebAccessActivated = value;
        }
    }

    public get IsCargoTrackWebAccessActivated() {
        if (this.TenantPM) {
            return this.TenantPM.IsCargoTrackWebAccessActivated;
        }
        else {
            return false;
        }
    }

    public set IsCargoTrackWebAccessActivated(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.IsCargoTrackWebAccessActivated = value;
        }
    }

    public get IsDigitalPortalAccessActivated() {
        if (this.TenantPM) {
            return this.TenantPM.IsDigitalPortalAccessActivated;
        }
        else {
            return false;
        }
    }

    public set IsDigitalPortalAccessActivated(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.IsDigitalPortalAccessActivated = value;
        }
    }

    public get IsMobileActivated() {
        if (this.TenantPM) {
            return this.TenantPM.IsMobileActivated;
        }
        else {
            return false;
        }
    }

    public set IsMobileActivated(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.IsMobileActivated = value;
        }
    }

    ShipmentCreatedFilters: any[];


    public get SharedLogisticsMessageLink() {

        if (this.TenantPM) {
            return this.TenantPM.SharedLogisticsMessageLink;
        }
        else return false;
    }
    public set SharedLogisticsMessageLink(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.SharedLogisticsMessageLink = value;
        }
    }

    public get SharedLogisticsMasterMessageLink() {

        if (this.TenantPM) {
            return this.TenantPM.SharedLogisMasterMessageLink;
        }
        else return false;
    }
    
    public set SharedLogisticsMasterMessageLink(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.SharedLogisMasterMessageLink = value;
        }
    }

    public get ShowMultiUnitsOfMeasurements() {
        if (this.TenantPM) {
            return this.TenantPM.ShowMultiUnitsOfMeasurements;
        }
        else return false;
    }
    
    public set ShowMultiUnitsOfMeasurements(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.ShowMultiUnitsOfMeasurements = value;
        }
    }

    public get ShipmentCreatedSelectedFilter() {
        if (this.TenantPM){
            return this.TenantPM.DPArchiveShipmentCreateFilter;
        }
        else{
            return 12;
        }
    }
    
    public set ShipmentCreatedSelectedFilter(value: any) {
        if (this.TenantPM) 
        {
            this.TenantPM.DPArchiveShipmentCreateFilter = value;
        }
    }
    
    public get ShipmentArrivalSelectedFilter() 
    {
        if (this.TenantPM) 
        {
            return this.TenantPM.DPArchiveShipmentArrivalFilter;
        }else{
            return 3;
        }
    }
    
    public set ShipmentArrivalSelectedFilter(value: any) {
        if (this.TenantPM) 
        {
            this.TenantPM.DPArchiveShipmentArrivalFilter = value;
        }
    }

    public get ShipmentDepartSelectedFilter() {
        if (this.TenantPM){
            return this.TenantPM.DPArchiveShipmentDepartFilter;
        }
        else {
            return 3;
        }
    }
    
    public set ShipmentDepartSelectedFilter(value: any) {
        if (this.TenantPM){
            this.TenantPM.DPArchiveShipmentDepartFilter = value;
        }
    }

    IsShowAreaColseAndCancelButton: boolean;
    IsSharedLogisticsActivatedEnable: boolean;
    IsMobileActivatedEnable: boolean;    
    SharedLogisticsMessageLinkEnable: boolean;
    SharedLogisticsMasterMessageLinkEnable: boolean = true;
    ShowMultiUnitsOfMeasurementsEnable: boolean = true;
    ShowApproveUploadedDocumentsEnabled: boolean = true;

    IsSharedLogisticsActivatedCheckboxBoxId: string;
    IsMobileActivatedCheckboxBoxId: string;
    IsWebAccessActivatedCheckboxBoxId: string;
    IsCargoTrackWebAccessActivatedCheckboxBoxId: string;
    IsDigitalPortalAccessActivatedCheckboxBoxId: string;
    SharedLogisticsMessageLinkCheckboxBoxId: string;
    SharedLogisticsMasterMessageLinkCheckboxBoxId: string;
    SharedLogisticsMultiUnitsOfMeasurementsId: string;

    IsShowActivateWebAccessArea: boolean = true;
    FullComponentsVisibility: boolean = false;

    IsShowMobileActivateArea: boolean = true;
    TenantPM: TenantPM;
    SharedTitleType: string;
    public tenantPMService: TenantPMService;
    OnCloseWindowEvent = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;

    IsShowQuotesRequestActivatedInSharedLogistics: boolean = false;

    constructor() {
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService();
        }

        var quotesRequestActivatedInSharedLogisticsfeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "QRA" && d.TenantNumber == SessionLocator.Tenant)[0];
        if (quotesRequestActivatedInSharedLogisticsfeatureToggle) {
            this.IsShowQuotesRequestActivatedInSharedLogistics = true;
        }

        this.ShipmentCreatedFilters = [
            {
                name: '1 year',
                code: '12'
            },
            {
                name: '9 months',
                code: '9'
            },
            {
                name: '6 months',
                code: '6'
            },
            {
                name: '3 months',
                code: '3'
            },
            {
                name: '2 months',
                code: '2'
            },
            {
                name: '1 month',
                code: '1'
            }
        ]
    }

    ngOnInit(

    ) {
        if (this.TenantPM) {
            this.Run();
        }

        this.FullComponentsVisibility = true;
        this.OnCloseWindowEvent.subscribe(($event: any) => {
            this.SaveButtonClicked();
        });
    }


    Run() {
        this.SelectedTabCode = "ACT";
        this.IsSharedLogisticsActivated = this.TenantPM.IsSharedLogisticsActivated;
        this.IsWebAccessActivated = this.TenantPM.IsWebAccessActivated;
        this.IsCargoTrackWebAccessActivated = this.TenantPM.IsCargoTrackWebAccessActivated;
        this.IsDigitalPortalAccessActivated = this.TenantPM.IsDigitalPortalAccessActivated;
        this.IsMobileActivated = this.TenantPM.IsMobileActivated;
        this.SharedLogisticsMessageLink = this.TenantPM.SharedLogisticsMessageLink;
        this.SharedLogisticsMasterMessageLink = this.TenantPM.SharedLogisMasterMessageLink;
        this.ShowMultiUnitsOfMeasurements = this.TenantPM.ShowMultiUnitsOfMeasurements;
        this.IsQuotesRequestActivatedInSharedLogistics = this.TenantPM.IsQuoteRequestActivateInShared;
        this.ApproveUploadedDocuments = this.TenantPM.ApproveUploadedDocuments;
        if (!FeatureLocator.HasFeaturePermession("General", "MOBILE") || this.SharedTitleType == "CargoTracking" || this.SharedTitleType == "DigitalPortal") {
            this.IsShowMobileActivateArea = false;
        }
       
        if (!FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS")) {
            this.IsShowActivateWebAccessArea = false;
        }
        else {
            this.IsShowActivateWebAccessArea = true;
        }

        if (!FeatureLocator.HasFeaturePermession("General", "MASTERSDOCUMENTSLINK")) {
            this.SharedLogisticsMasterMessageLinkEnable = false;
        }

        if (!FeatureLocator.HasFeaturePermession("General", "SupportUnitMeasurements")) {
            this.ShowMultiUnitsOfMeasurementsEnable = false;
        }

        if (!FeatureLocator.HasFeaturePermession("General", "PendingApprovalDocuments")) {
            this.ShowApproveUploadedDocumentsEnabled = false;
        }

        this.IsSharedLogisticsActivatedCheckboxBoxId = Guid.newGuid();
        this.IsMobileActivatedCheckboxBoxId = Guid.newGuid();
        this.IsWebAccessActivatedCheckboxBoxId = Guid.newGuid();
        this.IsCargoTrackWebAccessActivatedCheckboxBoxId = Guid.newGuid();
        this.IsDigitalPortalAccessActivatedCheckboxBoxId = Guid.newGuid();
        this.SharedLogisticsMessageLinkCheckboxBoxId = Guid.newGuid();
        this.SharedLogisticsMasterMessageLinkCheckboxBoxId = Guid.newGuid();
        this.SharedLogisticsMultiUnitsOfMeasurementsId = Guid.newGuid();
        this.ShipmentCreatedSelectedFilter = this.ShipmentCreatedFilters.find(a => a.code == this.TenantPM.DPArchiveShipmentCreateFilter);
        this.ShipmentArrivalSelectedFilter = this.ShipmentCreatedFilters.find(a => a.code == this.TenantPM.DPArchiveShipmentArrivalFilter);
        this.ShipmentDepartSelectedFilter = this.ShipmentCreatedFilters.find(a => a.code == this.TenantPM.DPArchiveShipmentDepartFilter);
        this.SetPropertiesEnable();

    }

    SelectedTabChange(selectedTabCode) {
        this.SelectedTabCode = selectedTabCode;
    }

    SetPropertiesEnable() {

        if (this.IsSharedLogisticsActivated) {
            this.IsSharedLogisticsActivatedEnable = false;
            this.SharedLogisticsMessageLinkEnable = true;
            if (ObjectsLocator.GlobalSetting.WorkEnvironment != "cloud" && this.TenantPM.CountryCode == "IL") {
                this.IsMobileActivatedEnable = false;
            }
            else {
                this.IsMobileActivatedEnable = true;
            }


        }
        else {

            this.IsSharedLogisticsActivatedEnable = true;
            this.IsMobileActivatedEnable = false;
            this.SharedLogisticsMessageLinkEnable = false;
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();

        if (this.TenantPM.IsDirty) {
            this.RejectChanges();
        }
        
        this.CurrentSession.CloseCurrentWindow();
    }

    OnShardLogisticChange() {
        this.TenantPM.IsSharedLogisticsActivated = !this.TenantPM.IsSharedLogisticsActivated;
        this.IsSharedLogisticsActivated = !this.IsSharedLogisticsActivated;

        if (this.IsSharedLogisticsActivated) {
            this.SetPropertiesEnable();
        }
    }

    SaveButtonClicked() {
        if (this.TenantPM.IsDirty) {
            this.CurrentSession.StartBusyIndicatorSaving();

            this.TenantPM.DPArchiveShipmentArrivalFilter = this.ShipmentArrivalSelectedFilter.code;
            this.TenantPM.DPArchiveShipmentDepartFilter = this.ShipmentDepartSelectedFilter.code;
            this.TenantPM.DPArchiveShipmentCreateFilter = this.ShipmentCreatedSelectedFilter.code;
            this.tenantPMService.update(this.TenantPM).subscribe((res:any)=> {
                SessionLocator.TenantPM.ApproveUploadedDocuments = this.TenantPM.ApproveUploadedDocuments;
                this.CloseButtonClicked();
            });
        }
        else this.CloseButtonClicked();
    }

    public get IsCargoTracking(): boolean {
        return this.SharedTitleType == "CargoTracking";
    }

    public get IsDigitalPortal() {
        return this.SharedTitleType == "DigitalPortal";
    }

    SetWindowArgs(args: any) {
        this.TenantPM = args.TenantPM;
        this.SharedTitleType = args.SharedTitleType;
        this.IsShowAreaColseAndCancelButton = true;
        this.Clone();
        // this.Run();

    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.TenantPM);
        this.myCloner.AddField('IsWebAccessActivated');
        this.myCloner.AddField('IsSharedLogisticsActivated');
        this.myCloner.AddField('IsMobileActivated');
        this.myCloner.AddField('SharedLogisticsMessageLink');
        this.myCloner.AddField('DisplayDocumentsAndEvents');
        this.myCloner.AddField('SharedLogisticsMasterMessageLink');
        this.myCloner.AddField('IsQuoteRequestActivateInShared');
        this.myCloner.AddField('IsCargoTrackWebAccessActivated');
        this.myCloner.AddField('IsDigitalPortalAccessActivated');
        this.myCloner.AddField('DPArchiveShipmentCreateFilter');
        this.myCloner.AddField('DPArchiveShipmentArrivalFilter');
        this.myCloner.AddField('DPArchiveShipmentDepartFilter');

        this.myCloner.AddEntity(this.TenantPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    public get DisplayDocumentsAndEvents() {

        if (this.TenantPM) {
            return this.TenantPM.DisplayDocumentsAndEvents;
        }
        else return false;
    }

    public set DisplayDocumentsAndEvents(value: boolean) {
        if (this.TenantPM) 
        {
            this.TenantPM.DisplayDocumentsAndEvents = value;
        }
    }

    public get ApproveUploadedDocuments() 
    {
        if (this.TenantPM) {
            return this.TenantPM.ApproveUploadedDocuments;
        }
        else return false;
    }

    public set ApproveUploadedDocuments(value: boolean) 
    {
        if (this.TenantPM) {
            this.TenantPM.ApproveUploadedDocuments = value;
        }
    }

    public get IsQuotesRequestActivatedInSharedLogistics() {
        if (!this.TenantPM) 
            return false
        return this.TenantPM.IsQuoteRequestActivateInShared;
    }

    public set IsQuotesRequestActivatedInSharedLogistics(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.IsQuoteRequestActivateInShared = value;
        }
    }

    OnSharedLogisticsMessageLink() {
        if(this.SharedTitleType == 'DigitalPortal'){
            this.DisplayDocumentsAndEvents = false;
        }
    }
}