import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { PaymentOrderPM } from '../../../../Customs/EntityPMs/PaymentOrderPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { NewPaymentRequestParams } from '../../../../Customs/DataContract/RequestParams/NewPaymentRequestParams';
import { PaymentOrderReplyResponseData, ConnectedEntityData } from '../../../../Customs/DataContract/ResponseData/PaymentOrderReplyResponseData';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';


@Component({
    selector: 'PaymentOrderReplyComponent',
    moduleId: module.id,
    templateUrl: './PaymentOrderReplyComponent.html',
})

export class PaymentOrderReplyComponent extends BaseRequestsSheetMassaging
implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: PaymentOrderReplyComponent = this;
    public EntityPM: PaymentOrderPM = new PaymentOrderPM();
    public ObjectTableName: string = "Customs.PaymentOrder";

    public PaymentDetailDataList: ObservableCollection;
    public ConnectedEntityDataList: ObservableCollection;
    public TaxParagraphList: ObservableCollection;
    public PaymentMethodsList: ObservableCollection;

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = true;

    ValidationErrors: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();

        this.ValidationErrors = [];
        this.PaymentDetailDataList = new ObservableCollection([]);
        this.ConnectedEntityDataList = new ObservableCollection([]);
        this.TaxParagraphList = new ObservableCollection([]);
        this.PaymentMethodsList = new ObservableCollection([]);
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
            if (this.ResponseData.PaymentDetailData) {
                this.PaymentDetailDataList.InsertCollection(this.ResponseData.PaymentDetailData);
            }

            if (this.ResponseData.ConnectedEntityData) {
                this.ConnectedEntityDataList.InsertCollection(this.ResponseData.ConnectedEntityData);
            }

            if (this.ResponseData.TaxParagraphList) {
                this.TaxParagraphList.InsertCollection(this.ResponseData.TaxParagraphList);
            }

            if (this.ResponseData.PaymentMethodsList) {
                this.PaymentMethodsList.InsertCollection(this.ResponseData.PaymentMethodsList);
            }
        }
        else {
            this.ResponseData = new PaymentOrderReplyResponseData();
            this.ResponseData.PaymentDetailData = [];
            this.ResponseData.ConnectedEntityData = [];
            this.ResponseData.TaxParagraphList = [];
            this.ResponseData.PaymentMethodsList = [];
        }
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    public get PaymentNumberHeader() {
        if (!AppTool.IsNullOrEmpty(this.ResponseData.PaymentNumber)) {
            return "הוראת תשלום" + " " + this.ResponseData.PaymentNumber;
        }
        return "הוראת תשלום";
    }

    public get PaymentOrderTotalSumToPay() { return this.ResponseData ? this.ResponseData.PaymentOrderTotalSumToPay : null; }
    public set PaymentOrderTotalSumToPay(newValue: number) { this.ResponseData.PaymentOrderTotalSumToPay = newValue; }

    public get UserMessage() { return this.ResponseData ? this.ResponseData.UserMessage : null; }
    public set UserMessage(newValue: string) { this.ResponseData.UserMessage = newValue; }

    public get PaymentStatusName() { return this.ResponseData ? this.ResponseData.PaymentStatusName : null; }
    public set PaymentStatusName(newValue: string) { this.ResponseData.PaymentStatusName = newValue; }

    public get PaymentOrderTypeName() { return this.ResponseData ? this.ResponseData.PaymentOrderTypeName : null; }
    public set PaymentOrderTypeName(newValue: string) { this.ResponseData.PaymentOrderTypeName = newValue; }

    public get PaymentOrderPayDate() { return this.ResponseData ? this.ResponseData.PaymentOrderPayDate : null; }
    public set PaymentOrderPayDate(newValue: Date) { this.ResponseData.PaymentOrderPayDate = newValue; }

    public get PaymentDetailDataCustomerActivityTypeExternalID() {
        if (!AppTool.IsNullOrEmpty(this.ResponseData.PaymentDetailData)) {
            var text = "";
            if (this.ResponseData.PaymentDetailData.CustomerActivityType != null) {
                text = this.ResponseData.PaymentDetailData.CustomerActivityTypeName;
            }
            text = text + " - ";
            if (this.ResponseData.PaymentDetailData.ExternalID != null) {
                text = this.ResponseData.PaymentDetailData.ExternalID.toString();
            }
            return text;
        }
        return "";
    }

    public get PaymentProcessName() { return this.ResponseData ? this.ResponseData.PaymentProcessName : null; }
    public set PaymentProcessName(newValue: string) { this.ResponseData.PaymentProcessName = newValue; }

    public get CustomsHouseName() { return this.ResponseData ? this.ResponseData.CustomsHouseName : null; }
    public set CustomsHouseName(newValue: string) { this.ResponseData.CustomsHouseName = newValue; }

    public get PaymentOrderReason() { return this.ResponseData ? this.ResponseData.PaymentOrderReason : null; }
    public set PaymentOrderReason(newValue: string) { this.ResponseData.PaymentOrderReason = newValue; }

    public get EntityTypeName() { return this.ResponseData ? this.ResponseData.ConnectedEntityData.EntityTypeName : null; }
    public set EntityTypeName(newValue: string) { this.ResponseData.ConnectedEntityData.EntityTypeName = newValue; }

    public get EntityIdKey1() { return this.ResponseData ? this.ResponseData.ConnectedEntityData.EntityIdKey1 : null; }
    public set EntityIdKey1(newValue: string) { this.ResponseData.ConnectedEntityData.EntityIdKey1 = newValue; }

    public get EntityIdKey2() { return this.ResponseData ? this.ResponseData.ConnectedEntityData.EntityIdKey2 : null; }
    public set EntityIdKey2(newValue: string) { this.ResponseData.ConnectedEntityData.EntityIdKey2 = newValue; }

    public get EntityIdKey3() { return this.ResponseData ? this.ResponseData.ConnectedEntityData.EntityIdKey3 : null; }
    public set EntityIdKey3(newValue: string) { this.ResponseData.ConnectedEntityData.EntityIdKey3 = newValue; }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
    }
}
