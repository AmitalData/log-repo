import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { ImporterDeclarationRequestParams } from '../../../Customs/DataContract/RequestParams/ImporterDeclarationRequestParams';
import { ImporterDeclarationResponseData } from '../../../Customs/DataContract/ResponseData/ImporterDeclarationResponseData';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { ClientList } from '../../../Customs/EntityLists/ClientList';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomsSettingListService } from '../../../Customs/Services/StandardLists/CustomsSettingListService';
import { PartnersDomainService } from '../../../Common/Services/PartnersDomainService';

import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';

@Component({
    selector: 'ImporterDeclarationComponent',
    moduleId: module.id,
    templateUrl: './ImporterDeclarationComponent.html',
})

export class ImporterDeclarationComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {
     
    public DataContext: ImporterDeclarationComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    public _ImporterName: string;
    public _IsImporerCodeEnabled: boolean = false;
    public _CodeVisibility: boolean = true;

    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _CustomsSettingListService: CustomsSettingListService = new CustomsSettingListService();
    _PartnersDomainService: PartnersDomainService = new PartnersDomainService();

    public PeriodDeclarationList: ObservableCollection;
    public LoiDeclarationList: ObservableCollection;
    public SecurityDeclarationList: ObservableCollection;

    public DeclarationConectFilterList: CodeNameClass[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.PeriodDeclarationList = new ObservableCollection([]);
        this.LoiDeclarationList = new ObservableCollection([]);
        this.SecurityDeclarationList = new ObservableCollection([]);
        this.BuildEmployeeGroupFilterList();
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

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new ImporterDeclarationRequestParams();
            this.SetIsByExpireDate(true);
            this.UIProperties.SetRequired("ImporterCode", null, true);
            this.UIProperties.SetRequired("DeclarationExpire", null, true);
            this.UIProperties.SetRequired("DeclarationConect", null, true);
            this.UIProperties.SetRequired("Code", null, true);
            this.UIProperties.SetRequired("FromDate", null, true);
            this.UIProperties.SetRequired("ToDate", null, true);
        }
        else if (this.RequestParams != null && this.RequestParams.IsByType) {
            this.SelectedDeclarationConectFilter = this.DeclarationConectFilterList.filter(d => d.Code == this.RequestParams.DeclarationConect)[0];
            if (this.RequestParams.DeclarationConect == "2") {
                this.CodeVisibility = false;
            }
        }

        if (this.ResponseData) {
            if (this.ResponseData.PeriodDeclarationList) {
                this.PeriodDeclarationList.InsertCollection(this.ResponseData.PeriodDeclarationList);
            }

            if (this.ResponseData.LoiDeclarationList) {
                this.LoiDeclarationList.InsertCollection(this.ResponseData.LoiDeclarationList);
            }

            if (this.ResponseData.SecurityDeclarationList) {
                this.SecurityDeclarationList.InsertCollection(this.ResponseData.SecurityDeclarationList);
            }
        }
    }

    //#region Properties
    get DeclarationExpire() { return this.RequestParams.DeclarationExpire; }
    set DeclarationExpire(value: Date) {
        if (this.RequestParams.DeclarationExpire != value) {
            this.RequestParams.DeclarationExpire = value;
        }
        if (value) {
            this.UIProperties.SetRequired("DeclarationExpire", null, false);
        }
        else {
            this.UIProperties.SetRequired("DeclarationExpire", null, true);
        }
    }

    SetIsByExpireDate(newValue: boolean) {
        this.IsByExpireDate = newValue;
    }

    get IsByExpireDate() { return this.RequestParams.IsByExpireDate; }
    set IsByExpireDate(newValue: boolean) {
        if (this.RequestParams.IsByExpireDate != newValue) {
            this.RequestParams.IsByExpireDate = newValue;
            if (newValue == true) {
                this.SetIsByType(false);
            }
        }
    }

    SetIsByType(newValue: boolean) {
        this.IsByType = newValue;
    }

    get IsByType() { return this.RequestParams.IsByType; }
    set IsByType(newValue: boolean) {
        if (this.RequestParams.IsByType != newValue) {
            this.RequestParams.IsByType = newValue;
            if (newValue == true) {
                this.SetIsByExpireDate(false);
            }
        }
    }

    get CodeVisibility() { return this._CodeVisibility; }
    set CodeVisibility(newValue: boolean) {
        if (this._CodeVisibility != newValue) {
            this._CodeVisibility = newValue;
        }
    }

    get DeclarationConect() { return this.RequestParams.DeclarationConect; }
    set DeclarationConect(value: string) {
        if (this.RequestParams.DeclarationConect != value) {
            this.RequestParams.DeclarationConect = value;
        }
        this.UIProperties.SetEnabled("Code", null, true);
        this.UIProperties.SetRequired("Code", null, true);
        if (value) {
            this.UIProperties.SetRequired("DeclarationConect", null, false);
            if (value == "0") {
                this.UIProperties.SetEnabled("Code", null, false);
                this.UIProperties.SetRequired("Code", null, false);
            }
        }
        else {
            this.UIProperties.SetRequired("DeclarationConect", null, true);
        }

    }

    get Code() { return this.RequestParams.Code; }
    set Code(value: string) {
        if (this.RequestParams.Code != value) {
            this.RequestParams.Code = value;
        }
        if (value) {
            this.UIProperties.SetRequired("Code", null, false);
        }
        else {
            this.UIProperties.SetRequired("Code", null, true);
        }
    }

    get FromDate() { return this.RequestParams.FromDate; }
    set FromDate(value: Date) {
        if (this.RequestParams.FromDate != value) {
            this.RequestParams.FromDate = value;
        }
        if (value) {
            this.UIProperties.SetRequired("FromDate", null, false);
        }
        else {
            this.UIProperties.SetRequired("FromDate", null, true);
        }
    }

    get ToDate() { return this.RequestParams.ToDate; }
    set ToDate(value: Date) {
        if (this.RequestParams.ToDate != value) {
            this.RequestParams.ToDate = value;
        }
        if (value) {
            this.UIProperties.SetRequired("ToDate", null, false);
        }
        else {
            this.UIProperties.SetRequired("ToDate", null, true);
        }
    }

    get ImporterCode() { return this.RequestParams.ImporterNumber; }
    set ImporterCode(value: string) {
        if (this.RequestParams.ImporterNumber != value) {
            this.RequestParams.ImporterNumber = value;
        }
        if (value) {
            this.UIProperties.SetRequired("ImporterCode", null, false);
        }
        else {
            this.UIProperties.SetRequired("ImporterCode", null, true);
        }
    }

    get ImporterName() { return this._ImporterName; }
    set ImporterName(value: string) {
        if (this._ImporterName != value) {
            this._ImporterName = value;
        }
    }
    //#endregion

    //#region EmployeeGroup
    private BuildEmployeeGroupFilterList() {
        this.DeclarationConectFilterList = [];

        var myRecordsItem: CodeNameClass = new CodeNameClass();
        myRecordsItem.Code = "0"; // "ALL"
        myRecordsItem.Name = "הכל";
        this.DeclarationConectFilterList.push(myRecordsItem);

        var allRecordsItem: CodeNameClass = new CodeNameClass();
        allRecordsItem.Code = "1"; // "ImportDeclaration"
        allRecordsItem.Name = "הצהרת יבוא";
        this.DeclarationConectFilterList.push(allRecordsItem);

        var myRecordsItem: CodeNameClass = new CodeNameClass();
        myRecordsItem.Code = "2"; // "Vendor"
        myRecordsItem.Name = "ספק";
        this.DeclarationConectFilterList.push(myRecordsItem);

        var allRecordsItem: CodeNameClass = new CodeNameClass();
        allRecordsItem.Code = "3"; // "Declaration"
        allRecordsItem.Name = "הצהרה";
        this.DeclarationConectFilterList.push(allRecordsItem);
    }

    private selectedDeclarationConectFilter: CodeNameClass;
    get SelectedDeclarationConectFilter() {
        return this.selectedDeclarationConectFilter;
    }
    set SelectedDeclarationConectFilter(newValue: CodeNameClass) {
        if (this.selectedDeclarationConectFilter != newValue) {
            this.selectedDeclarationConectFilter = newValue;
            this.DeclarationConect = newValue.Code;
        }
        if (newValue.Code == "0") {
            this.UIProperties.SetEnabled("Code", null, false);
            this.UIProperties.SetRequired("Code", null, false);
        }
        else {
            this.UIProperties.SetEnabled("Code", null, true);
            this.UIProperties.SetRequired("Code", null, true);
        }
        if (newValue.Code == "2") {
            this.CodeVisibility = false;
        }
        else {
            this.CodeVisibility = true;
        }
    }
    //#endregion

    //#region Importer Commands
    ImporterClicked(type, client: ClientList) {
        this.ImporterCode = client.Code;
        this.ImporterName = AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
    }

    ImporterTextChanged(type, item) {

        this.ImporterName = "";
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

        if (AppTool.IsNullOrEmpty(this.ImporterCode)) {
            var msg = TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.ImporterNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (this.IsByExpireDate) {
            if (AppTool.IsNullOrEmpty(this.RequestParams.DeclarationExpire)) {
                var msg = TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.DeclarationExpireMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }

        if (this.IsByType) {
            if (AppTool.IsNullOrEmpty(this.SelectedDeclarationConectFilter.Code)) {
                var msg = TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.DeclarationConectMandatory");
                this.ValidationErrorsList.push(msg);
            }
            if (AppTool.IsNullOrEmpty(this.Code) && this.SelectedDeclarationConectFilter.Code != "0") {
                var msg = TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.DeclarationNumberMandatory");
                this.ValidationErrorsList.push(msg);
            }
            if (AppTool.IsNullOrEmpty(this.FromDate)) {
                var msg = TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.FromDateMandatory");
                this.ValidationErrorsList.push(msg);
            }
            if (AppTool.IsNullOrEmpty(this.ToDate)) {
                var msg = TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.ToDateMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }

    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.PeriodDeclarationList.Clear();
        this.LoiDeclarationList.Clear();
        this.SecurityDeclarationList.Clear();

        var currRequestParams = new ImporterDeclarationRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.ImporterNumber = this.ImporterCode;
        currRequestParams.IsByExpireDate = this.IsByExpireDate;
        currRequestParams.IsByType = this.IsByType;
        if (this.IsByExpireDate) {
            currRequestParams.DeclarationExpire = this.DeclarationExpire;
        }
        else if (this.IsByType) {
            currRequestParams.DeclarationConect = this.SelectedDeclarationConectFilter.Code;
            currRequestParams.Code = this.Code;
            currRequestParams.FromDate = this.FromDate;
            currRequestParams.ToDate = this.ToDate;
        }

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
            "שליחת שאילתא לתצהיר יבואן", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._IIGGeneralMessagesService.PostImporterDeclarationRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    //#endregion 
}


class CodeNameClass {
    public Code: string
    public Name: string
}
