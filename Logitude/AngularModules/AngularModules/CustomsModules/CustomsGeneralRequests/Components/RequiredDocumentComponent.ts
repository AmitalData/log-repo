import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { PaymentOrderPM } from '../../../Customs/EntityPMs/PaymentOrderPM';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../Customs/Services/StandardLists/CustomsSettingListService';
import { NewPaymentRequestParams } from '../../../Customs/DataContract/RequestParams/NewPaymentRequestParams';
import { RequiredDocumentResponseData, DocumentConnectedEntitiesResult } from '../../../Customs/DataContract/ResponseData/RequiredDocumentResponseData';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';


@Component({
    selector: 'RequiredDocumentComponent',
    moduleId: module.id,
    templateUrl: './RequiredDocumentComponent.html',
})

export class RequiredDocumentComponent extends BaseRequestsSheetMassaging
implements AfterViewInit, IRequestsSheetMassagingComponent {

    public DataContext: RequiredDocumentComponent = this;
    public EntityPM: PaymentOrderPM = new PaymentOrderPM();

    public DocumentConnectedEntitiesList: ObservableCollection;

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = true;
    ObjectTableName: string; // html component requires this property. AOT

    ValidationErrors: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();

        this.ValidationErrors = [];
        this.DocumentConnectedEntitiesList = new ObservableCollection([]);

        this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe(response => {
            });
        });
    }

    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new NewPaymentRequestParams();
        }

        if (this.ResponseData) {
            if (this.ResponseData.DocumentConnectedEntitiesList) {
                this.DocumentConnectedEntitiesList.InsertCollection(this.ResponseData.DocumentConnectedEntitiesList);
            }
        }
        else {
            this.ResponseData = new RequiredDocumentResponseData();
            this.ResponseData.DocumentConnectedEntitiesList = [];
        }
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    public get Title() { return this.ResponseData ? this.ResponseData.Title : null; }
    public set Title(newValue: string) { this.ResponseData.Title = newValue; }

    public get VerificationDecisionType() { return this.ResponseData ? this.ResponseData.VerificationDecisionType : null; }
    public set VerificationDecisionType(newValue: string) { this.ResponseData.VerificationDecisionType = newValue; }

    public get Remarks() { return this.ResponseData ? this.ResponseData.Remarks : null; }
    public set Remarks(newValue: string) { this.ResponseData.Remarks = newValue; }

    public get DocumentTypeName() { return this.ResponseData ? this.ResponseData.DocumentTypeName : null; }
    public set DocumentTypeName(newValue: string) { this.ResponseData.DocumentTypeName = newValue; }

    public get DocumentNumber() { return this.ResponseData ? this.ResponseData.DocumentNumber : null; }
    public set DocumentNumber(newValue: string) { this.ResponseData.DocumentNumber = newValue; }

    public get DocumentWorkerName() { return this.ResponseData ? this.ResponseData.DocumentWorkerName : null; }
    public set DocumentWorkerName(newValue: string) { this.ResponseData.DocumentWorkerName = newValue; }

    public get ReplacingDocumentId() { return this.ResponseData ? this.ResponseData.ReplacingDocumentId : null; }
    public set ReplacingDocumentId(newValue: string) { this.ResponseData.ReplacingDocumentId = newValue; }

    public get DocumentTypeVisibility() { return this.ResponseData ? this.ResponseData.DocumentTypeVisibility : null; }
    public set DocumentTypeVisibility(newValue: string) { this.ResponseData.DocumentTypeVisibility = newValue; }

    public get ReplacingDocumentIdVisibility() { return this.ResponseData ? this.ResponseData.ReplacingDocumentIdVisibility : null; }
    public set ReplacingDocumentIdVisibility(newValue: string) { this.ResponseData.ReplacingDocumentIdVisibility = newValue; }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
    }
}
