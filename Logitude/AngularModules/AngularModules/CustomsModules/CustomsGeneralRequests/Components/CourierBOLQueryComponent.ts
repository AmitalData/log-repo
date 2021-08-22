import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CourierBOLQueryRequestParams } from '../../../Customs/DataContract/RequestParams/CourierBOLQueryRequestParams';
import { CourierBOLQueryResponseData } from '../../../Customs/DataContract/ResponseData/CourierBOLQueryResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection'; 
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { CustomSendOptionsArgs, SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';

@Component({
    selector: 'CourierBOLQueryComponent',
    
    templateUrl: './CourierBOLQueryComponent.html',
})

export class CourierBOLQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: CourierBOLQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();

    public CourierBOLDetailsObservableList: ObservableCollection;

    private _DeclarationId: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.CourierBOLDetailsObservableList = new ObservableCollection([]);
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
            this.RequestParams = new CourierBOLQueryRequestParams();
            this.UIProperties.SetRequired("CourierBOL", null, true);
            this.UIProperties.SetRequired("CourierVAT", null, true);
        }

        if (this.ResponseData) {
            if (this.ResponseData.CourierBOLDetailsList) {
                this.CourierBOLDetailsObservableList.InsertCollection(this.ResponseData.CourierBOLDetailsList);
            }
        }
    }

    SetMenuArg(MenuArg) {
        this.OnMassageDisplayMethod();
        this.IsFromDeclaration = true;
        this.CustomFileNo = MenuArg.CustomFileNo;
        this.CourierBOL = MenuArg.CourierBOL;
        this.CourierVAT = MenuArg.CourierVAT;
        this._DeclarationId = MenuArg.DeclarationId;

        this.UIProperties.SetEnabled("CourierBOL", null, false);
        this.UIProperties.SetEnabled("CourierVAT", null, false);

        let customSendOptionsArgs: CustomSendOptionsArgs = new CustomSendOptionsArgs();
        customSendOptionsArgs.ForcePersonalSign = false;
        customSendOptionsArgs.RequestVIA = SendRequestVIA.WebServiceInteractive;
        this.OnCustomSendOptionsButtonClick(customSendOptionsArgs);
    }

    EditButtonClicked(item) {
        this.CurrentSession.CloseCurrentWindowEmit(item.cargoIdentifierKey3);
    }

    //#region Properties
    get CustomFileNo() { return this.RequestParams ? this.RequestParams.CustomFileNo : null; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.CustomFileNo != value) {
            this.RequestParams.CustomFileNo = value;
        }
    }

    get CourierBOL() { return this.RequestParams ? this.RequestParams.CourierBOL : null; }
    set CourierBOL(value: string) {
        if (this.RequestParams.CourierBOL != value) {
            this.RequestParams.CourierBOL = value;
        }
        if (value) {
            this.UIProperties.SetRequired("CourierBOL", null, false);
        }
        else {
            this.UIProperties.SetRequired("CourierBOL", null, true);
        }
    }

    get CourierVAT() { return this.RequestParams ? this.RequestParams.CourierVAT : null; }
    set CourierVAT(value: string) {
        if (this.RequestParams.CourierVAT != value) {
            this.RequestParams.CourierVAT = value;
        }
        if (value) {
            this.UIProperties.SetRequired("CourierVAT", null, false);
        }
        else {
            this.UIProperties.SetRequired("CourierVAT", null, true);
        }
    }

    private _IsFromDeclaration: boolean = false;
    get IsFromDeclaration() { return this._IsFromDeclaration; }
    set IsFromDeclaration(value: boolean) {
        if (this._IsFromDeclaration != value) {
            this._IsFromDeclaration = value;
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
        this.CourierBOL = "";
        this.CourierVAT = "";
        this.UIProperties.SetEnabled("CourierBOL", null, true);
        this.UIProperties.SetEnabled("CourierVAT", null, true);
        this.ValidationErrorsList = [];
    }

    FetchDeclaration(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            lastFetchDeclarationList = lastFetchDeclarationList[0];
            this._DeclarationId = lastFetchDeclarationList.DeclarationId;
            this.CourierBOL = lastFetchDeclarationList.ManifestNumber;
            this.CourierVAT = lastFetchDeclarationList.SecondCargoID;

            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetEnabled("CourierBOL", null, false);
            this.UIProperties.SetEnabled("CourierVAT", null, false);
        }
        else {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
            this.ValidationErrorsList.push(msg);
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
        }
    }

    //#region Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    
    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (AppTool.IsNullOrEmpty(this.RequestParams.CourierBOL) || AppTool.IsNullOrEmpty(this.RequestParams.CourierVAT)) {
            var msg = TextCodeTranslator.Translate("Customs.CourierBOLQuery.O.QueryDataMissing");
            this.ValidationErrorsList.push(msg);
        }

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        var currRequestParams = new CourierBOLQueryRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.CourierBOL = this.RequestParams.CourierBOL;
        currRequestParams.CourierVAT = this.RequestParams.CourierVAT;
        currRequestParams.DeclarationId = this._DeclarationId;
        currRequestParams.CustomFileNo = this.CustomFileNo;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId, "שליחת שאילתא לשטרי מטען בלדר", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._IIGGeneralMessagesService.PostCourierBOLRequest(currRequestParams)
            .subscribe(() => { }
            )
            ;
    }

    //#endregion Commands
}
