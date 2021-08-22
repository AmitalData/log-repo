import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { NewEntityArgs } from '../../../../Infrastructure/Args';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { PaymentOrderPM } from '../../../../Customs/EntityPMs/PaymentOrderPM';
import { PaymentMessagesService } from '../../../../Customs/Services/WebServices/PaymentMessagesService';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { NewPaymentRequestParams } from '../../../../Customs/DataContract/RequestParams/NewPaymentRequestParams';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'NewPaymentOrderComponent',
    
    templateUrl: './NewPaymentOrderComponent.html',
})

export class NewPaymentOrderComponent 
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: NewPaymentOrderComponent = this;
    public ObjectTableName: string = "Customs.PaymentOrder";

    _PaymentMessagesService: PaymentMessagesService = new PaymentMessagesService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();

        EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response:any) => { });
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

    QueryNameText: string = "";
    SetWindowArgs(args: any) {
        if (args != null) {
            this.QueryNameText = TextCodeTranslator.Translate("Customs.General.O.NewPaymentOrder");
        }
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new NewPaymentRequestParams();
            //this.UIProperties.SetRequired("PaymentNumber", this.ObjectTableName, true);
            //this.UIProperties.SetRequired("ImporterId", this.ObjectTableName, true);
        }

    }

    //#region Properties
    get PaymentNumber() { return this.RequestParams.PaymentNumber; }
    set PaymentNumber(value: string) {
        if (this.RequestParams.PaymentNumber != value) {
            this.RequestParams.PaymentNumber = value;
        }
        if (value) {
            this.UIProperties.SetRequired("PaymentNumber", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("PaymentNumber", this.ObjectTableName, true);
        }
    }

    get ImporterId() { return this.RequestParams.ExternalId; }
    set ImporterId(value: string) {
        if (this.RequestParams.ExternalId != value) {
            this.RequestParams.ExternalId = value;
        }
        //if (value) {
        //    this.UIProperties.SetRequired("ImporterId", this.ObjectTableName, false);
        //}
        //else {
        //    this.UIProperties.SetRequired("ImporterId", this.ObjectTableName, true);
        //}
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.PaymentNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.NewPaymentOrder.O.PaymentNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (AppTool.IsNullOrEmpty(this.ImporterId)) {
            var msg = TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.ImporterNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        var currRequestParams = new NewPaymentRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.PaymentNumber = this.PaymentNumber;
        currRequestParams.ExternalId = this.ImporterId;
        currRequestParams.RequestParamsVersion = 0;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליפת הוראת תשלום", false)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._PaymentMessagesService.PostNewPaymentRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

}
