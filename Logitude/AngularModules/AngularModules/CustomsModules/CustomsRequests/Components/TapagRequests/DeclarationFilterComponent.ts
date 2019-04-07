import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
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

    public ClaimObservableCollection: ObservableCollection;
    DeficitObservableCollection: ObservableCollection;
    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();

    _MyResponseObjectToShow: any = null;
    _UserMessagehidden: boolean = true;
    _LastFetchDeclarationList: DeclarationList;
    GeneralDetailsExternalID: any; // html component requires this property. AOT
    GeneralDetailsName: any; // html component requires this property. AOT
    GeneralDetailsCustomOfficeName: any; // html component requires this property. AOT
    GeneralDetailsStatusName: any; // html component requires this property. AOT
    private CurrentSession = SessionLocator.SelectedSession;
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
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
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

        this.CurrentSession.StartBusyIndicator("")
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();

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
    get DisplayFileNumber() { return this.ResponseData.DisplayFileNumber; }
    set DisplayFileNumber(value: string) {
        if (this.ResponseData.DisplayFileNumber != value) {
            this.ResponseData.DisplayFileNumber = value;
        }
    }

    get CustomOfficeName() { return this.ResponseData.CustomOfficeName; }
    set CustomOfficeName(value: string) {
        if (this.ResponseData.CustomOfficeName != value) {
            this.ResponseData.CustomOfficeName = value;
        }
    }

    get CreditLimit() { return this.ResponseData.CreditLimit; }
    set CreditLimit(value: string) {
        if (this.ResponseData.CreditLimit != value) {
            this.ResponseData.CreditLimit = value;
        }
    }

    get StatusName() { return this.ResponseData.StatusName; }
    set StatusName(value: string) {
        if (this.ResponseData.StatusName != value) {
            this.ResponseData.StatusName = value;
        }
    }

    get EntityTypeName() { return this.ResponseData.EntityTypeName; }
    set EntityTypeName(value: string) {
        if (this.ResponseData.EntityTypeName != value) {
            this.ResponseData.EntityTypeName = value;
        }
    }

    get CreditBalance() { return this.ResponseData.CreditBalance; }
    set CreditBalance(value: string) {
        if (this.ResponseData.CreditBalance != value) {
            this.ResponseData.CreditBalance = value;
        }
    }

    get GuaranteedName() { return this.ResponseData.GuaranteedName; }
    set GuaranteedName(value: string) {
        if (this.ResponseData.GuaranteedName != value) {
            this.ResponseData.GuaranteedName = value;
        }
    }

    get EntityNumber() { return this.ResponseData.EntityNumber; }
    set EntityNumber(value: string) {
        if (this.ResponseData.EntityNumber != value) {
            this.ResponseData.EntityNumber = value;
        }
    }

    get GuaranteeExecutedAmountAdjusted() { return this.ResponseData.GuaranteeExecutedAmountAdjusted; }
    set GuaranteeExecutedAmountAdjusted(value: string) {
        if (this.ResponseData.GuaranteeExecutedAmountAdjusted != value) {
            this.ResponseData.GuaranteeExecutedAmountAdjusted = value;
        }
    }

    get AgentName() { return this.ResponseData.AgentName; }
    set AgentName(value: string) {
        if (this.ResponseData.AgentName != value) {
            this.ResponseData.AgentName = value;
        }
    }

    get Validity() { return this.ResponseData.Validity; }
    set Validity(value: string) {
        if (this.ResponseData.Validity != value) {
            this.ResponseData.Validity = value;
        }
    }

    get GuaranteeAmount() { return this.ResponseData.GuaranteeAmount; }
    set GuaranteeAmount(value: string) {
        if (this.ResponseData.GuaranteeAmount != value) {
            this.ResponseData.GuaranteeAmount = value;
        }
    }

    //#endregion Properties


    

  

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (AppTool.IsNullOrEmpty(this.RequestParams.DeclarationId) || AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        
        this.ClaimObservableCollection.Clear();
        

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


