import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TapagMessagesService } from '../../../../Customs/Services/WebServices/TapagMessagesService';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { DeclarationFilterRequestParams } from '../../../../Customs/DataContract/RequestParams/DeclarationFilterRequestParams';
import { DeclarationFilterResponseData } from '../../../../Customs/DataContract/ResponseData/DeclarationFilterResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';

import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
@Component({
    selector: 'DeclarationFilterComponent',
    moduleId: module.id,
    templateUrl: './DeclarationFilterComponent.html',
})

export class DeclarationFilterComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {

    public DataContext: DeclarationFilterComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _TapagMessagesService: TapagMessagesService = new TapagMessagesService();
    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();

    public ClaimObservableCollection: ObservableCollection;
    public DeficitObservableCollection: ObservableCollection;

    _MyResponseObjectToShow: any = null;
    _UserMessagehidden: boolean = true;
    _LastFetchDeclarationList: DeclarationList;

    constructor() {
        super();
        this.ClaimObservableCollection = new ObservableCollection([]);
        this.DeficitObservableCollection = new ObservableCollection([]);

        this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
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

    SetMenuArg(MenuArg) {
        this.RequestParams = MenuArg;
        this.OnMassageDisplayMethod();
        //this.CustomFileNo = MenuArg.CustomFileNo;
        //this.DeclarationNumber= MenuArg.DeclarationNumber;
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new DeclarationFilterRequestParams();
            this.UIProperties.SetRequired("GuranteeType", null, true);
            this.UIProperties.SetRequired("FileNumber", null, true);
            this.UIProperties.SetRequired("Numeral", null, true);
        }
        this._MyResponseObjectToShow = null;

        if (this.ResponseData) {
            let myDeclarationFilterResponseData: DeclarationFilterResponseData = this.ResponseData;
            if (myDeclarationFilterResponseData.ClaimList) {
                this.ClaimObservableCollection.InsertCollection(myDeclarationFilterResponseData.ClaimList);
            }
            if (myDeclarationFilterResponseData.DeficitList) {
                this.DeficitObservableCollection.InsertCollection(myDeclarationFilterResponseData.ClaimList);
            }

            try {
                this._MyResponseObjectToShow = JSON.parse(myDeclarationFilterResponseData.ResponseStatusXML)
            } catch (err) {
                console.log(err);
            }
        }
        else {
            this.ResponseData = new DeclarationFilterResponseData();
        }
    }



    DueChangeClearChildField(sourceIsCostomFile: boolean): void {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        } else {

            this.CustomFileNo = "";
        }

        this.DeclarationId = "";
        this.ResponseData = null;
        this._LastFetchDeclarationList = null;
        this.ValidationErrorsList = [];
    }

    CustomFileNoTextChanged(searchtext) {

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }

        if (this._LastFetchDeclarationList != null) {
            if (this.CustomFileNo == this._LastFetchDeclarationList.CustomFileNo) {
                return;
            }
        }
        this.DueChangeClearChildField(true);
        SessionLocator.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                this.FetchDeclaration(myResponse, true);
            });

    }


    DeclarationNumberTextChanged(DeclarationNumberText: string): void {
        if (AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }

        if (this._LastFetchDeclarationList != null) {
            if (this.DeclarationNumber == this._LastFetchDeclarationList.DeclarationNumber) {
                return;
            }
        }
        this.DueChangeClearChildField(false);

        SessionLocator.CurrentSession.StartBusyIndicator("")
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                SessionLocator.CurrentSession.StopBusyIndicator();

                this.FetchDeclaration(myResponse, false);

            });
    }


    FetchDeclaration(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        this._LastFetchDeclarationList = myResponse.Result
        if (this._LastFetchDeclarationList != null) {
            this.DeclarationId = this._LastFetchDeclarationList.Id;
            this.DeclarationNumber = this._LastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = this._LastFetchDeclarationList.CustomFileNo;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        } else {

            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            } else {
                this.SetValidityDeclarationNumber();
            }


        }
    }
    SetValidityDeclarationNumber() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    }

    SetValidityCustomFileNo() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    }

    get CustomFileNo() { return this.RequestParams ? this.RequestParams.CustomsFile : null; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.CustomsFile != value) {
            this.RequestParams.CustomsFile = value;
        }
    }

    get DeclarationNumber() { return this.RequestParams ? this.RequestParams.DeclarationNumber : null; }
    set DeclarationNumber(value: string) {
        if (this.RequestParams.DeclarationNumber != value) {
            this.RequestParams.DeclarationNumber = value;
            if (value) {
                this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
            }
        } else {
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
        }
    }
    get DeclarationId() { return this.RequestParams ? this.RequestParams.DeclarationId : null; }
    set DeclarationId(value: string) {
        if (this.RequestParams.DeclarationId != value) {
            this.RequestParams.DeclarationId = value;
        }
    }


    RefreshScreen() {
        this._MyResponseObjectToShow = null;
        this._UserMessagehidden = true;
        if (this.ResponseData == null) {
            return;
        }
        this._UserMessagehidden = !this.ResponseData.IsShowUserMessage;


        if (!AppTool.IsNullOrEmpty(this.ResponseData.ResponseStatusXML)) {
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
            try {
                this._MyResponseObjectToShow = JSON.parse(this.ResponseData.ResponseStatusXML)
            } catch (err) {
                console.log(err);
            }
        }
    }

    //#region Response Properties
    get ExternalID() { return this.ResponseData != null && this.ResponseData.GeneralDetailsData != null ? this.ResponseData.GeneralDetailsData.externalID: null; }
    set ExternalID(value: string) {
        if (this.ResponseData.externalID != value) {
            this.ResponseData.externalID = value;
        }
    }

    get CustomOfficeName() { return this.ResponseData != null && this.ResponseData.GeneralDetailsData != null ? this.ResponseData.GeneralDetailsData.customOfficeName : null; }
    set CustomOfficeName(value: string) {
        if (this.ResponseData.customOfficeName != value) {
            this.ResponseData.customOfficeName = value;
        }
    }

    get ExternalName() { return this.ResponseData != null && this.ResponseData.GeneralDetailsData != null ? this.ResponseData.GeneralDetailsData.name : null; }
    set ExternalName(value: string) {
        if (this.ResponseData.name != value) {
            this.ResponseData.name = value;
        }
    }

    get StatusName() { return this.ResponseData != null && this.ResponseData.GeneralDetailsData != null ?  this.ResponseData.GeneralDetailsData.statusName : null; }
    set StatusName(value: string) {
        if (this.ResponseData.statusName != value) {
            this.ResponseData.statusName = value;
        }
    }

    //#endregion Properties


    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (this.ValidationErrorsList.length > 0) {
            return;
        }


        this.ClaimObservableCollection.Clear();
        this.DeficitObservableCollection.Clear();

        var currRequestParams = new DeclarationFilterRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.AppicationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationNumber = this.RequestParams.DeclarationNumber;
        currRequestParams.CustomsFile = this.RequestParams.CustomsFile;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
                'שליחת שאילתא לנתוני תפ""ג עבור הצהרה', true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._TapagMessagesService.PostDeclarationFilterRequestParams(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    //#endregion Commands
}
