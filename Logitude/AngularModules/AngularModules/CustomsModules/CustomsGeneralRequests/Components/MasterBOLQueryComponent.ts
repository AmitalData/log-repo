import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { MasterBOLQueryRequestParams } from '../../../Customs/DataContract/RequestParams/MasterBOLQueryRequestParams';
import { MasterBOLFeedBackResponseData } from '../../../Customs/DataContract/ResponseData/MasterBOLFeedBackResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs, SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    selector: 'MasterBOLQueryComponent',
    
    templateUrl: './MasterBOLQueryComponent.html',
})

export class MasterBOLQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {

    public DataContext: MasterBOLQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();

    public InternalCargosList: ObservableCollection;

    private _IsFromDeclaration: boolean = false;
    private _DeclarationId: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.InternalCargosList = new ObservableCollection([]);
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
            this.RequestParams = new MasterBOLQueryRequestParams();
            this.Date = new Date().getFullYear();
            this.ReturnAllInernalCargos = true;//task 44705 21.11.18
            this.UIProperties.SetRequired("MasterBillOfLading", null, true);
            this.UIProperties.SetRequired("InternalIdentifier", null, true);
        }

        if (this.ResponseData) {
            if (this.ResponseData.InternalCargosList) {
                this.InternalCargosList.InsertCollection(this.ResponseData.InternalCargosList);
            }
        }
    }

    EditButtonClicked(item) {
        this.CurrentSession.CloseCurrentWindowEmit(item);
    }

    SetMenuArg(MenuArg) {
        this.OnMassageDisplayMethod();
        this.IsFromDeclaration = true;
        this.CustomFileNo = MenuArg.CustomFileNo;
        this.Date = MenuArg.Date;
        this.MasterBillOfLading = MenuArg.MasterBillOfLading;
        this.InternalIdentifier = MenuArg.InternalIdentifier;
        this.ReturnAllInernalCargos = MenuArg.ReturnAllInernalCargos;
        this._DeclarationId = MenuArg.DeclarationId;

        this.UIProperties.SetEnabled("CustomFileNo", null, false);
        this.UIProperties.SetEnabled("Date", null, false);
        this.UIProperties.SetEnabled("MasterBillOfLading", null, false);
        this.UIProperties.SetEnabled("InternalIdentifier", null, false);
        this.UIProperties.SetEnabled("ReturnAllInernalCargos", null, false);

        let customSendOptionsArgs: CustomSendOptionsArgs  = new CustomSendOptionsArgs();
        customSendOptionsArgs.ForcePersonalSign = false;
        customSendOptionsArgs.RequestVIA = SendRequestVIA.WebServiceInteractive;
        this.OnCustomSendOptionsButtonClick(customSendOptionsArgs);

    }

    //#region Properties
    get Date() { return this.RequestParams.Date; }
    set Date(value: any) {
        if (this.RequestParams.Date != value) {
            if (value) {
                this.UIProperties.SetRequired("Date", null, false);
            }
            else {
                this.UIProperties.SetRequired("Date", null, true);
            }
            this.RequestParams.Date = value;
        }
    }

    get MasterBillOfLading() { return this.RequestParams.MasterBillOfLading; }
    set MasterBillOfLading(value: string) {
        if (this.RequestParams.MasterBillOfLading != value) {
            this.RequestParams.MasterBillOfLading = value;
        }
        if (value) {
            this.UIProperties.SetRequired("MasterBillOfLading", null, false);
        }
        else {
            this.UIProperties.SetRequired("MasterBillOfLading", null, true);
        }
    }

    get InternalIdentifier() { return this.RequestParams.InternalIdentifier; }
    set InternalIdentifier(value: string) {
        if (this.RequestParams.InternalIdentifier != value) {
            this.RequestParams.InternalIdentifier = value;
        }
        if (value) {
            this.UIProperties.SetRequired("InternalIdentifier", null, false);
        }
        else {
            this.UIProperties.SetRequired("InternalIdentifier", null, true);
        }
    }

    get ReturnAllInernalCargos() { return this.RequestParams.ReturnAllInernalCargos; }
    set ReturnAllInernalCargos(value: boolean) {
        if (this.RequestParams.ReturnAllInernalCargos != value) {
            this.RequestParams.ReturnAllInernalCargos = value;
        }
        if (this.RequestParams.ReturnAllInernalCargos == true) {
            this.ExactMatch = false;
        }
    }

    get ExactMatch() { return this.RequestParams.ExactMatch; }
    set ExactMatch(value: boolean) {
        if (this.RequestParams.ExactMatch != value) {
            this.RequestParams.ExactMatch = value;
        }
        if (this.RequestParams.ExactMatch == true) {
            this.ReturnAllInernalCargos = false;
        }
    }

    get IsFromDeclaration() { return this._IsFromDeclaration; }
    set IsFromDeclaration(value: boolean) {
        if (this._IsFromDeclaration != value) {
            this._IsFromDeclaration = value;
        }
    }

    get CustomFileNo() { return this.RequestParams ? this.RequestParams.CustomFileNo : null; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.CustomFileNo != value) {
            this.RequestParams.CustomFileNo = value;
        }
    }

    //#endregion Properties

    CustomFileNoTextChanged(searchtext) {
        this.DueChangeClearChildField();
        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }

        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.CustomFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchDeclaration(myResponse, true);
            });
    }

    DueChangeClearChildField(): void {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this._DeclarationId = null;
        this.Date = new Date().getFullYear();
        this.MasterBillOfLading = "";
        this.InternalIdentifier = "";
        this.ReturnAllInernalCargos = false;
        this.UIProperties.SetEnabled("Date", null, true);
        this.UIProperties.SetEnabled("MasterBillOfLading", null, true);
        this.UIProperties.SetEnabled("InternalIdentifier", null, true);
        this.UIProperties.SetEnabled("ReturnAllInernalCargos", null, true);
        this.ValidationErrorsList = [];
    }

    FetchDeclaration(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            lastFetchDeclarationList = lastFetchDeclarationList[0];
            this._DeclarationId = lastFetchDeclarationList.DeclarationId;
            this.Date = lastFetchDeclarationList.ManifestNumber;
            this.MasterBillOfLading = lastFetchDeclarationList.SecondCargoID;
            this.InternalIdentifier = lastFetchDeclarationList.ThirdCargoID;
            //this.ReturnAllInernalCargos = lastFetchDeclarationList.ThirdCargoID ? false : true;
            this.ReturnAllInernalCargos = true;//task 44705 21.11.18

            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetEnabled("Date", null, false);
            this.UIProperties.SetEnabled("MasterBillOfLading", null, false);
            this.UIProperties.SetEnabled("InternalIdentifier", null, false);
            this.UIProperties.SetEnabled("ReturnAllInernalCargos", null, false);
        }
        else {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
            this.ValidationErrorsList.push(msg);
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
        }
    }

    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;


        if (AppTool.IsNullOrEmpty(this.Date)) {
            var msg = TextCodeTranslator.Translate("Customs.MasterBOLQuery.O.YearDateIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        else {
            if (((this.Date + '').length != 4) || isNaN(this.Date)) {
                var msg = "חובה להזין שנת טיסה בפורמט של 4 תווים";
                this.ValidationErrorsList.push(msg);
            }
        }

        if (AppTool.IsNullOrEmpty(this.MasterBillOfLading)) {
            var msg = TextCodeTranslator.Translate("Customs.MasterBOLQuery.O.MasterBillOfLadingIsMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.InternalIdentifier) && this.ReturnAllInernalCargos != true) {
            var msg = TextCodeTranslator.Translate("Customs.MasterBOLQuery.O.InternalIdentifierIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new MasterBOLQueryRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.Date = this.RequestParams.Date;
        currRequestParams.MasterBillOfLading = this.RequestParams.MasterBillOfLading;
        currRequestParams.InternalIdentifier = this.RequestParams.InternalIdentifier;
        currRequestParams.ReturnAllInernalCargos = this.RequestParams.ReturnAllInernalCargos;
        currRequestParams.ExactMatch = this.RequestParams.ExactMatch;
        currRequestParams.DeclarationId = this._DeclarationId;
        currRequestParams.CustomFileNo = this.CustomFileNo;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליחת שאילתא לשטרי מטען", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._IIGGeneralMessagesService.PostMasterBOLRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }

    //#endregion Commands
}
