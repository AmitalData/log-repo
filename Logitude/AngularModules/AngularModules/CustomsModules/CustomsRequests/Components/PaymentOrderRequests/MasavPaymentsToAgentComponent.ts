import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { PaymentMessagesService } from '../../../../Customs/Services/WebServices/PaymentMessagesService';
import { MasavPaymentsToAgentRequestParams } from '../../../../Customs/DataContract/RequestParams/MasavPaymentsToAgentRequestParams';
import { MasavPaymentsToAgentResponseData } from '../../../../Customs/DataContract/ResponseData/MasavPaymentsToAgentResponseData';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ClientList } from '../../../../Customs/EntityLists/ClientList';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';

import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';

@Component({
    selector: 'MasavPaymentsToAgentComponent',
    moduleId: module.id,
    templateUrl: './MasavPaymentsToAgentComponent.html',
})

export class MasavPaymentsToAgentComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: MasavPaymentsToAgentComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _PaymentMessagesService: PaymentMessagesService = new PaymentMessagesService();

    public AgentMasavPaymentResultList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.AgentMasavPaymentResultList = new ObservableCollection([]);
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new MasavPaymentsToAgentRequestParams();
            this.UIProperties.SetRequired("PaymentDate", null, true);
        }

        if (this.ResponseData && this.ResponseData.AgentMasavPaymentResultList) {
            for (let item of this.ResponseData.AgentMasavPaymentResultList) {
                item.RelatedEntityListObs = new ObservableCollection(item.RelatedEntityList);
            }
            this.AgentMasavPaymentResultList.InsertCollection(this.ResponseData.AgentMasavPaymentResultList);
        }
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

    OnRowLoaded(myRow: any) {
        if (myRow) {
            myRow.SetExpandaple(true);
        }
    }

    //#region Properties
    get PaymentDate() { return this.RequestParams.PaymentDate; }
    set PaymentDate(value: Date) {
        if (this.RequestParams.PaymentDate != value) {
            this.RequestParams.PaymentDate = value;
        }
        if (value) {
            this.UIProperties.SetRequired("PaymentDate", null, false);
        }
        else {
            this.UIProperties.SetRequired("PaymentDate", null, true);
        }
    }
    //#endregion

    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.RequestParams.PaymentDate)) {
            var msg = TextCodeTranslator.Translate("Customs.MasavPaymentsToAgentQuery.O.PaymentDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.AgentMasavPaymentResultList.Clear();

        var currRequestParams = new MasavPaymentsToAgentRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.PaymentDate = this.PaymentDate;

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
            "שליחת שאילתא לבקשת דוח קופה לסוכן", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._PaymentMessagesService.PostMasavPaymentsToAgentRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    //#endregion 
}
