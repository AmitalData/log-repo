import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import {ClientMessagesService} from '../../../Customs/Services/WebServices/ClientMessagesService';
import {ClientSearchRequestParams} from '../../../Customs/DataContract/RequestParams/ClientSearchRequestParams';
import { ClientSearchByIDResponseData } from '../../../Customs/DataContract/ResponseData/ClientSearchByIDResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { ClientList } from '../../../Customs/EntityLists/ClientList';
import { CustomsSettingListService } from '../../../Customs/Services/StandardLists/CustomsSettingListService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'ClientSearchByIDComponent',
    
    templateUrl: './ClientSearchByIDComponent.html',
})

export class ClientSearchByIDComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {

    public DataContext: ClientSearchByIDComponent = this;
    public ObjectTableName: string = "Customs.Client";

    _ClientMessagesService: ClientMessagesService = new ClientMessagesService();

    public CustomerActivityList: ObservableCollection;
    public AuthorizedList: ObservableCollection;
    public AuthorizerList: ObservableCollection;
    public ExportRequestList: ObservableCollection;
    public IndicationPerClassificationList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    EntityResourceService: EntityResourceService=new EntityResourceService();
    constructor() {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.ClientIndication").subscribe((response: any) => {

           this.CustomerActivityList = new ObservableCollection([]);
           this.AuthorizedList = new ObservableCollection([]);
           this.AuthorizerList = new ObservableCollection([]);
           this.ExportRequestList = new ObservableCollection([]);
           this.IndicationPerClassificationList = new ObservableCollection([]);
        });
    }

    SetWindowArgs(menuArg: any) {
        this.OnMassageDisplayMethod();
        if (menuArg.Mode == "DeclarationGeneralComponent") {
            this.IsExternalId = menuArg.IsExternalId;
            this.ExternalId=menuArg.ImporterCode;
            this.IsPassport = menuArg.IsPassport;
            this.PassportNumber = menuArg.PassportNumber;
            this.PassportTypeCode = menuArg.PassportTypeCode;
            this.PassportCountryCode = menuArg.PassportCountryCode;
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

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new ClientSearchRequestParams();
            this.UseExternalId();
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.RequestParams.ExternalId)) {
                this.UseExternalId();
            }
            else {
                this.UsePassportRadio();
            }
        }

        if (this.ResponseData) {
            if (this.ResponseData.CustomerActivityList) {
                for (let item of this.ResponseData.CustomerActivityList) {
                    item.CustomerIndicationList = new ObservableCollection(item.CustomerIndicationList);
                }
                this.CustomerActivityList.InsertCollection(this.ResponseData.CustomerActivityList);
            }

            if (this.ResponseData.AuthorizedList) {
                this.AuthorizedList.InsertCollection(this.ResponseData.AuthorizedList);
            }
            if (this.ResponseData.AuthorizerList) {
                this.AuthorizerList.InsertCollection(this.ResponseData.AuthorizerList);
            }
            if (this.ResponseData.ExportRequestList) {
                this.ExportRequestList.InsertCollection(this.ResponseData.ExportRequestList);
            }
            if (this.ResponseData.IndicationPerClassificationList) {
                this.IndicationPerClassificationList.InsertCollection(this.ResponseData.IndicationPerClassificationList);
            }
        }
    }

    //#region Properties
    private isExternalId: boolean;
    get IsExternalId() { return this.isExternalId; }
    set IsExternalId(value: boolean) {
        if (this.isExternalId != value) {
            this.isExternalId = value;
        }
    }

    private isPassport: boolean;
    get IsPassport() { return this.isPassport; }
    set IsPassport(value: boolean) {
        if (this.isPassport != value) {
            this.isPassport = value;
        }
    }

    get ExternalId() { return this.RequestParams ? this.RequestParams.ExternalId : null; }
    set ExternalId(value: string) {
        if (this.RequestParams.ExternalId != value) {
            this.RequestParams.ExternalId = value;
        }
    }

    get PassportNumber() { return this.RequestParams ? this.RequestParams.PassportNumber : null; }
    set PassportNumber(value: string) {
        if (this.RequestParams.PassportNumber != value) {
            this.RequestParams.PassportNumber = value;
        }
    }

    get PassportTypeCode() { return this.RequestParams ? this.RequestParams.PassportTypeCode : null; }
    set PassportTypeCode(value: string) {
        if (this.RequestParams.PassportTypeCode != value) {
            this.RequestParams.PassportTypeCode = value;
        }
    }

    get PassportCountryCode() { return this.RequestParams ? this.RequestParams.PassportCountryCode : null; }
    set PassportCountryCode(value: string) {
        if (this.RequestParams.PassportCountryCode != value) {
            this.RequestParams.PassportCountryCode = value;
        }
    }

    //#endregion Properties

    //#region General Commands

    UseExternalId() {
        this.PassportCountryCode = null;
        this.PassportTypeCode = null;
        this.PassportNumber = null;
        this.UIProperties.SetEnabled("ExternalId", "Customs.Client", true);
        this.UIProperties.SetEnabled("PassportCountryCode", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportTypeCode", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportNumber", "Customs.Client", false);
        this.IsExternalId = true;
        this.IsPassport = false;
    }

    UsePassportRadio() {
        this.ExternalId = null;
        this.UIProperties.SetEnabled("ExternalId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("PassportCountryCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("PassportTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("PassportNumber", this.ObjectTableName, true);
        this.IsExternalId = false;
        this.IsPassport = true;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    EditCustomerIndicationCommand(item) {

        if (item.CustomerIndicationList == null || item.CustomerIndicationList.length == 0) {
            return;
        }

        var windowArgs: any = {};
        windowArgs.CustomerIndicationList = item.CustomerIndicationList;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 470;
        logitudeWindow.Height = 520;
        logitudeWindow.IsShowCloseButton = false;
        logitudeWindow.Title = TextCodeTranslator.Translate("Customs.ClientIndication.O.IndicationClient"); 
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./CustomsModules/CustomsGeneralRequests/Components/CustomerIndicationComponent');
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (this.IsExternalId && AppTool.IsNullOrEmpty(this.ExternalId)) {
            var msg = TextCodeTranslator.Translate("Customs.Client.O.CodeRequired");
            this.ValidationErrorsList.push(msg);
        }

        if (this.IsPassport && AppTool.IsNullOrEmpty(this.PassportNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.Client.O.PassportRequired");
            this.ValidationErrorsList.push(msg);
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        this.ResponseData = new ClientSearchByIDResponseData();

        var currRequestParams = new ClientSearchRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.ExternalId = this.ExternalId;
        currRequestParams.PassportNumber = this.PassportNumber;
        currRequestParams.PassportTypeCode = this.PassportTypeCode;
        currRequestParams.PassportCountryCode = this.PassportCountryCode;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליחת שאילתא לנתונים נוספים ליבואן", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._ClientMessagesService.PostClientSearchByIDRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }

    //#endregion Commands
}
 