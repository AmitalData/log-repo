import { Component, OnInit, AfterViewInit, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { CustomMessageWrapperComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { DeclarationMessagesService } from '../../../../Customs/Services/WebServices/DeclarationMessagesService';
import { DeclarationStatusRequestParams } from '../../../../Customs/DataContract/RequestParams/DeclarationStatusRequestParams';
import { DeclarationStatusResponseData, AvailabiltyLogDeclarationCargoQuantities, MultiDeclaration } from '../../../../Customs/DataContract/ResponseData/DeclarationStatusResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CargoIdentifireTypeListService } from '../../../../Customs/Services/StandardLists/CargoIdentifireTypeListService';
import { CargoIdentifireTypePM } from '../../../../Customs/EntityPMs/CargoIdentifireTypePM';

@Component({
    selector: 'DeclarationStatusComponent',

    templateUrl: './DeclarationStatusComponent.html',
})

export class DeclarationStatusComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, OnChanges, IRequestsSheetMassagingComponent {

    public DataContext: DeclarationStatusComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    public IsShowAvailabiltyQuantitiesList: boolean = false;

    _CargoIdentifireTypePM: CargoIdentifireTypePM = new CargoIdentifireTypePM();
    _CargoIdentifireTypeListService: CargoIdentifireTypeListService = new CargoIdentifireTypeListService();
    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _DeclarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();

    public AvailabiltyQuantitiesList: Array<AvailabiltyLogDeclarationCargoQuantities>;
    public MultiDeclaration: Array<MultiDeclaration>;
    public IsMultiDeclaration: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.AvailabiltyQuantitiesList = [];
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.setRequired();
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

    DeclarationsList: any;
    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new DeclarationStatusRequestParams();
            this.SetIsByDeclarationNumber(true);
            this.UIProperties.SetRequired("DeclarationNumber", "Customs.Declaration", true);
        }

        if (this.ResponseData == null) {
            this.ResponseData = new DeclarationStatusResponseData();
        }
        else {
            if (this.ResponseData.AvailabiltyQuantitiesList != null && this.ResponseData.AvailabiltyQuantitiesList.length > 0) {
                this.IsShowAvailabiltyQuantitiesList = true;
                this.AvailabiltyQuantitiesList = this.ResponseData.AvailabiltyQuantitiesList;
            }
            if (this.ResponseData.MultiDeclarations != null && this.ResponseData.MultiDeclarations.length > 0) {
                this.DeclarationsList = [];
                this.MultiDeclaration = this.ResponseData.MultiDeclarations;
                for (let dec of this.MultiDeclaration) {
                    this.DeclarationsList.push(dec.DeclarationID);
                }
                this.IsMultiDeclaration = true;
            }
        }
    }

    ParentDeclarationSelectionChanged(value) {
        for (var item of this.MultiDeclaration) {
            if (item.DeclarationID == value) {
                this.DeclarationVersion = item.DeclarationVersion;
                this.DeclarationStatusText = item.DeclarationStatusText;
                this.LogisticStatusText = item.LogisticStatusText;
                this.DeclarationOfficeText = item.DeclarationOfficeText;
                this.TaxationDateTime = item.TaxationDateTime;
                this.FinancialStatusText = item.FinancialStatusText;
                this.ReleaseDateTime = item.ReleaseDateTime;
                this.SubmitDateTime = item.SubmitDateTime;
                this.HandeledWroker = item.HandeledWroker;
                this.DeclarationStatusCode = item.DeclarationStatusCode;

            }
        }
    }

    isExportDeclaration:boolean = false;
    SetMenuArg(MenuArg) {
        this.OnMassageDisplayMethod();
        this.DeclarationNumber = MenuArg.DeclarationNumber;
        this.CustomFileNo = MenuArg.CustomsFile;
        if (this.CustomFileNo != null) {
            this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
                .subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.Result != null) {
                        if (myResponse.Result.Direction == 'E') {
                            this.isExportDeclaration = true;
                            this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.CustomFileNo)
                                .subscribe((Response: ServiceResponse) => {
                                    if (Response.Result != null) {
                                        this.CargoTypeCode = Response.Result[0].CargoTypeCode;
                                        this.ManifestNumber = Response.Result[0].ManifestNumber;
                                        this.SecondCargoID = Response.Result[0].SecondCargoID;
                                        this.ThirdCargoID = Response.Result[0].ThirdCargoID;
                                        this.setRequired();
                                    }
                                });
                        }
                    }
                });
        }
    }

    //#region Properties
    SetIsByDeclarationNumber(newValue: boolean) {
        this.IsByDeclarationNumber = newValue;
    }

    get IsByDeclarationNumber() { return this.RequestParams ? this.RequestParams.DeclarationRadio : null; }
    set IsByDeclarationNumber(newValue: boolean) {
        if (this.RequestParams.DeclarationRadio != newValue) {
            this.RequestParams.DeclarationRadio = newValue;
            if (newValue == true) {
                this.IsByCargo = false;
                this.IsByOldReshimon = false;
            }
        }
    }

    SetIsByCargo(newValue: boolean) {
        this.IsByCargo = newValue;
    }


    get IsByCargo() { return this.RequestParams ? this.RequestParams.CargoRadio : null; }
    set IsByCargo(newValue: boolean) {
        if (this.RequestParams.CargoRadio != newValue) {
            this.RequestParams.CargoRadio = newValue;

            if (newValue == true) {
                this.IsByDeclarationNumber = false;
                this.IsByOldReshimon = false;
            }
        }
    }

    SetIsByOldReshimon(newValue: boolean) {
        this.IsByOldReshimon = newValue;
    }

    get IsByOldReshimon() { return this.RequestParams ? this.RequestParams.OldReshimonRadio : null; }
    set IsByOldReshimon(newValue: boolean) {
        if (this.RequestParams.OldReshimonRadio != newValue) {
            this.RequestParams.OldReshimonRadio = newValue;
        }
        if (newValue == true) {
            this.IsByDeclarationNumber = false;
            this.IsByCargo = false;
        }
    }

    get CustomFileNo() { return this.RequestParams ? this.RequestParams.CustomFileNo : null; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.CustomFileNo != value) {
            this.RequestParams.CustomFileNo = value;
        }
    }

    get DeclarationNumber() { return this.RequestParams ? this.RequestParams.DeclarationNumber : null; }
    set DeclarationNumber(value: string) {
        if (this.RequestParams.DeclarationNumber != value) {
            this.RequestParams.DeclarationNumber = value;
        }
    }


    get CargoTypeCode() { return this.RequestParams ? this.RequestParams.CargoTypeCode : null; }
    set CargoTypeCode(value: string) {
        if (this.RequestParams.CargoTypeCode != value) {
            this.RequestParams.CargoTypeCode = value;
            if (this.isExportDeclaration) {
                this._CargoIdentifireTypeListService.getSingleFromCache(this.CargoTypeCode)
                    .subscribe((Response: ServiceResponse) => {
                        if (Response.Result != null) {
                            this._CargoIdentifireTypePM = Response.Result;
                            this.setRequired();
                        }
                    });
            }
        }
    }

    setRequired() {
        if (this.isExportDeclaration) {
            this.UIProperties.SetRequired("ManifestNumber", this.ObjectTableName, true);
            if (this.ManifestNumber != null) {
                this.UIProperties.SetRequired("ManifestNumber", this.ObjectTableName, false);
            }

            if (this._CargoIdentifireTypePM.IsKey2Mandatory) {
                this.UIProperties.SetRequired("SecondCargoID", this.ObjectTableName, true);
                if (this.SecondCargoID != null) {
                    this.UIProperties.SetRequired("SecondCargoID", this.ObjectTableName, false);
                }
            } else {
                this.UIProperties.SetRequired("SecondCargoID", this.ObjectTableName, false);
            }
            if (this._CargoIdentifireTypePM.IsKey3Mandatory) {
                this.UIProperties.SetRequired("ThirdCargoID", this.ObjectTableName, true);
                if (this.ThirdCargoID != null) {
                    this.UIProperties.SetRequired("ThirdCargoID", this.ObjectTableName, false);
                }
            } else {
                this.UIProperties.SetRequired("ThirdCargoID", this.ObjectTableName, false);
            }
        }
    }
    get ManifestNumber() { return this.RequestParams ? this.RequestParams.ManifestNumber : null; }
    set ManifestNumber(value: string) {
        if (this.RequestParams.ManifestNumber != value) {
            this.RequestParams.ManifestNumber = value;
        }
    }

    get SecondCargoID() { return this.RequestParams ? this.RequestParams.SecondCargoID : null; }
    set SecondCargoID(value: string) {
        if (this.RequestParams.SecondCargoID != value) {
            this.RequestParams.SecondCargoID = value;
        }
    }

    get ThirdCargoID() { return this.RequestParams ? this.RequestParams.ThirdCargoID : null; }
    set ThirdCargoID(value: string) {
        if (this.RequestParams.ThirdCargoID != value) {
            this.RequestParams.ThirdCargoID = value;
        }
    }

    get OldReshimonNumber() { return this.RequestParams ? this.RequestParams.OldReshimonNumber : null; }
    set OldReshimonNumber(value: string) {
        if (this.RequestParams.OldReshimonNumber != value) {
            this.RequestParams.OldReshimonNumber = value;
        }
    }


    get WarningMessage() { return this.ResponseData ? this.ResponseData.WarningMessage : null; }
    set WarningMessage(value: string) {
        if (this.ResponseData.WarningMessage != value) {
            this.ResponseData.WarningMessage = value;
        }
    }

    get DeclarationID() { return this.ResponseData ? this.ResponseData.DeclarationID : null; }
    set DeclarationID(value: string) {
        if (this.ResponseData.DeclarationID != value) {
            this.ResponseData.DeclarationID = value;
        }
    }

    get DeclarationVersion() { return this.ResponseData ? this.ResponseData.DeclarationVersion : null; }
    set DeclarationVersion(value: string) {
        if (this.ResponseData.DeclarationVersion != value) {
            this.ResponseData.DeclarationVersion = value;
        }
    }

    get DeclarationStatusCode() { return this.ResponseData ? this.ResponseData.DeclarationStatusCode : null; }
    set DeclarationStatusCode(value: string) {
        if (this.ResponseData.DeclarationStatusCode != value) {
            this.ResponseData.DeclarationStatusCode = value;
        }
    }

    get DeclarationStatusText() { return this.ResponseData ? this.ResponseData.DeclarationStatusText : null; }
    set DeclarationStatusText(value: string) {
        if (this.ResponseData.DeclarationStatusText != value) {
            this.ResponseData.DeclarationStatusText = value;
        }
    }

    get LogisticStatusText() { return this.ResponseData ? this.ResponseData.LogisticStatusText : null; }
    set LogisticStatusText(value: string) {
        if (this.ResponseData.LogisticStatusText != value) {
            this.ResponseData.LogisticStatusText = value;
        }
    }

    get DeclarationOfficeText() { return this.ResponseData ? this.ResponseData.DeclarationOfficeText : null; }
    set DeclarationOfficeText(value: string) {
        if (this.ResponseData.DeclarationOfficeText != value) {
            this.ResponseData.DeclarationOfficeText = value;
        }
    }

    get TaxationDateTime() { return this.ResponseData ? this.ResponseData.TaxationDateTime : null; }
    set TaxationDateTime(value: Date) {
        if (this.ResponseData.TaxationDateTime != value) {
            this.ResponseData.TaxationDateTime = value;
        }
    }

    get FinancialStatusText() { return this.ResponseData ? this.ResponseData.FinancialStatusText : null; }
    set FinancialStatusText(value: string) {
        if (this.ResponseData.FinancialStatusText != value) {
            this.ResponseData.FinancialStatusText = value;
        }
    }

    get HandeledWroker() { return this.ResponseData ? this.ResponseData.HandeledWroker : null; }
    set HandeledWroker(value: string) {
        if (this.ResponseData.HandeledWroker != value) {
            this.ResponseData.HandeledWroker = value;
        }
    }

    get ReleaseDateTime() { return this.ResponseData ? this.ResponseData.ReleaseDateTime : null; }
    set ReleaseDateTime(value: Date) {
        if (this.ResponseData.ReleaseDateTime != value) {
            this.ResponseData.ReleaseDateTime = value;
        }
    }

    get SubmitDateTime() { return this.ResponseData ? this.ResponseData.SubmitDateTime : null; }
    set SubmitDateTime(value: Date) {
        if (this.ResponseData.SubmitDateTime != value) {
            this.ResponseData.SubmitDateTime = value;
        }
    }
    //#endregion Properties


    //#region Commands
    DueChangeClearChildField(sourceIsCostomFile: boolean): void {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        } else {

            this.CustomFileNo = "";
        }
        //this.ResponseData = null;
        this.ValidationErrorsList = [];
    }

    CustomFileNoTextChanged(searchtext) {

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
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

        this.DueChangeClearChildField(false);

        this.CurrentSession.StartBusyIndicator("")
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();

                this.FetchDeclaration(myResponse, false);

            });
    }

    FetchDeclaration(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            this.DeclarationNumber = lastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = lastFetchDeclarationList.CustomFileNo;
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

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (this.IsByDeclarationNumber && AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (this.IsByCargo) {
            if (AppTool.IsNullOrEmpty(this.CargoTypeCode)) {
                var msg = TextCodeTranslator.Translate("Customs.Declaration.O.CargoTypeCodeIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
            if (AppTool.IsNullOrEmpty(this.ManifestNumber)) {
                var msg = TextCodeTranslator.Translate("Customs.Declaration.O.FirstCargoIdIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }
        if (this.IsByOldReshimon && AppTool.IsNullOrEmpty(this.OldReshimonNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.OldReshimonIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        this.CurrentSession.StartBusyIndicator("");

        var currRequestParams = new DeclarationStatusRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.RequestOrigin = "DeclarationStatusRequestViewModel";

        if (this.IsByDeclarationNumber == true) {
            currRequestParams.DeclarationNumber = this.RequestParams.DeclarationNumber;
            currRequestParams.CustomFileNo = this.RequestParams.CustomFileNo;
            currRequestParams.DeclarationRadio = true;
        }
        else if (this.IsByCargo == true) {
            currRequestParams.CargoTypeCode = this.RequestParams.CargoTypeCode;
            currRequestParams.ManifestNumber = this.RequestParams.ManifestNumber;
            currRequestParams.SecondCargoID = this.RequestParams.SecondCargoID;
            currRequestParams.ThirdCargoID = this.RequestParams.ThirdCargoID;
            currRequestParams.CargoRadio = true;
        }
        else if (this.IsByOldReshimon == true) {
            currRequestParams.OldReshimonNumber = this.OldReshimonNumber;
            currRequestParams.OldReshimonRadio = true;
        }

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
                "שליחת שאילתא לסטטוס הצהרה", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._DeclarationMessagesService.PostDeclarationStatusRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }

    //#endregion Commands
}
