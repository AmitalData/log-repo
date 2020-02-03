import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ConsignmentPM } from '../../../../Customs/EntityPMs/ConsignmentPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { DeclarationMessagesService } from '../../../../Customs/Services/WebServices/DeclarationMessagesService';
import { CargoSealsRequestParams, CargoSealDetails } from '../../../../Customs/DataContract/RequestParams/CargoSealsRequestParams';
import { ClientList } from '../../../../Customs/EntityLists/ClientList';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CargoSealPM } from '../../../../Customs/EntityPMs/CargoSealPM';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    selector: 'CargoSealsQueryComponent',
    templateUrl: './CargoSealsQueryComponent.html',
})


export class CargoSealsQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent, OnInit {
    public DataContext: CargoSealsQueryComponent = this;
    public ObjectTableName: string = "Customs.CargoSealIdentifier";

    _IsReady: boolean = false;

    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _DeclarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();
    _LastFetchDeclarationList: DeclarationList;
    _LastFetchConsignmentPMList: ConsignmentPM[];

    public CargoSealObslist: ObservableCollection;

    text: any;
    IsRePackingApproval: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();
        this.CargoSealObslist = new ObservableCollection([]);
        this.EntityResourceService.getEntityResourceByTableName("Customs.CargoSealIdentifier").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.CargoSeal").subscribe(response => {
                this._IsReady = true;
            });
        });
    }

    ngOnInit() {
        super.ngOnInit();
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
            this.RequestParams = new CargoSealsRequestParams();

            this.UpdateDate = DateTool.GetCurrentDateAsUtc();
            this.UIProperties.SetRequired("CargoIdentifierTypeCode", this.ObjectTableName, true);
            this.UIProperties.SetRequired("CargoIdentifierKey1", this.ObjectTableName, true);
            this.UIProperties.SetRequired("CargoIdentifierKey2", this.ObjectTableName, true);
            this.UIProperties.SetRequired("CargoIdentifierKey3", this.ObjectTableName, true);
            this.UIProperties.SetRequired("CargoRowNumber", this.ObjectTableName, true);
            this.UIProperties.SetRequired("ContainerNumber", this.ObjectTableName, true);
            this.UIProperties.SetRequired("ImporterNumber", this.ObjectTableName, true);
        }

        //if (this.ResponseData && this.ResponseData.MorningMessageList) {
        //    var myArr = this.ResponseData.MorningMessageList;
        //    var fast = true;
        //    if (!fast) {
        //        this.ResponseData.MorningMessageList.forEach((itemMess) => {
        //            this.MorningMessageObservableList.Insert(itemMess);
        //        });
        //    } else {

        //        this.MorningMessageObservableList.InsertCollection(myArr);
        //    }
        //}
    }

    get UpdateDate() { return this.RequestParams.UpdateDate; }
    set UpdateDate(value: Date) {
        if (this.RequestParams.UpdateDate != value) {
            this.RequestParams.UpdateDate = value;
        }
        if (value) {
            this.UIProperties.SetRequired("UpdateDate", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("UpdateDate", this.ObjectTableName, true);
        }
    }

    get CustomFileNo() { return this.RequestParams.CustomFileNo; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.CustomFileNo != value) {
            this.RequestParams.CustomFileNo = value;
        }
    }

    get DeclarationId() { return this.RequestParams ? this.RequestParams.DeclarationId : null }
    set DeclarationId(value: string) {
        if (this.RequestParams.DeclarationId != value) {
            this.RequestParams.DeclarationId = value;
        }
    }

    get DeclarationNumber() { return this.RequestParams.DeclarationNumber; }
    set DeclarationNumber(value: string) {
        if (this.RequestParams.DeclarationNumber != value) {
            this.RequestParams.DeclarationNumber = value;
        }
    }

    get CargoIdentifierTypeCode() { return this.RequestParams.CargoIdentifierTypeCode; }
    set CargoIdentifierTypeCode(value: string) {
        if (this.RequestParams.CargoIdentifierTypeCode != value) {
            this.RequestParams.CargoIdentifierTypeCode = value;
        }
        if (value) {
            this.UIProperties.SetRequired("CargoIdentifierTypeCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("CargoIdentifierTypeCode", this.ObjectTableName, true);
        }
    }

    get FileNumber() { return this.RequestParams.FileNumber; }
    set FileNumber(value: string) {
        if (this.RequestParams.FileNumber != value) {
            this.RequestParams.FileNumber = value;
            if (AppTool.IsNullOrEmpty(this.RequestParams.FileNumber)) {
                this.UIProperties.SetRequired("FileNumber", this.ObjectTableName, true);
            } else {
                this.UIProperties.SetRequired("FileNumber", this.ObjectTableName, false);
            }

        }
    }

    get CargoIdentifierKey1() { return this.RequestParams.CargoIdentifierKey1; }
    set CargoIdentifierKey1(value: string) {
        if (this.RequestParams.CargoIdentifierKey1 != value) {
            this.RequestParams.CargoIdentifierKey1 = value;
        }
        if (value) {
            this.UIProperties.SetRequired("CargoIdentifierKey1", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("CargoIdentifierKey1", this.ObjectTableName, true);
        }
    }

    get CargoIdentifierKey2() { return this.RequestParams.CargoIdentifierKey2; }
    set CargoIdentifierKey2(value: string) {
        if (this.RequestParams.CargoIdentifierKey2 != value) {
            this.RequestParams.CargoIdentifierKey2 = value;
        }
        if (value) {
            this.UIProperties.SetRequired("CargoIdentifierKey2", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("CargoIdentifierKey2", this.ObjectTableName, true);
        }
    }

    get CargoIdentifierKey3() { return this.RequestParams.CargoIdentifierKey3; }
    set CargoIdentifierKey3(value: string) {
        if (this.RequestParams.CargoIdentifierKey3 != value) {
            this.RequestParams.CargoIdentifierKey3 = value;
        }
        if (value) {
            this.UIProperties.SetRequired("CargoIdentifierKey3", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("CargoIdentifierKey3", this.ObjectTableName, true);
        }
    }

    get CargoRowNumber() { return this.RequestParams.CargoRowNumber; }
    set CargoRowNumber(value: string) {
        if (this.RequestParams.CargoRowNumber != value) {
            this.RequestParams.CargoRowNumber = value;
        }
        if (value) {
            this.UIProperties.SetRequired("CargoRowNumber", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("CargoRowNumber", this.ObjectTableName, true);
        }
    }

    get ContainerNumber() { return this.RequestParams.ContainerNumber; }
    set ContainerNumber(value: string) {
        if (this.RequestParams.ContainerNumber != value) {
            this.RequestParams.ContainerNumber = value;
        }
    }

    get ImporterNumber() { return this.RequestParams.ImporterNumber; }
    set ImporterNumber(value: string) {
        this.RequestParams.ImporterNumber = value;
        if (value) {
            this.UIProperties.SetRequired("ImporterNumber", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("ImporterNumber", this.ObjectTableName, true);
        }
    }

    ImporterNumberLostFocus(item: any) {
        this.ImporterNumber = item;
    }

    ImporterNumberTextChanged(item: any) {
        this.ImporterNumber = item;
    }

    ImporterNumberClicked(client: ClientList) {
        this.ImporterNumber = client != null ? client.PassportNumber : null;
    }

    FillErrors() {

        var errors: string[] = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.CargoIdentifierTypeCode)) {
            var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierTypeCode");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.CargoIdentifierKey1)) {
            var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierKey1Mandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.CargoIdentifierKey2)) {
            var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierKey2Mandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.CargoIdentifierKey3)) {
            var msg = TextCodeTranslator.Translate("Customs.CargoSealsQuery.F.CargoIdentifierKey3Mandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.CargoRowNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoRowNumbeMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.ContainerNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.CargoSealsQuery.F.ContainerNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.UpdateDate)) {
            var msg = TextCodeTranslator.Translate("Customs.CargoSealsQuery.F.UpdateDateMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.ImporterNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.ImporterNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (this.CargoSealObslist.Length == 0) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CargoSealsQuery.F.CargoSealItemsItemsMandatory"));
        }
        this.CargoSealObslist.Collection.forEach((item) => {
            if (AppTool.IsNullOrEmpty(item.SealNumber)) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CargoSealsQuery.F.SealNumberMandatory"));
            }
            if (AppTool.IsNullOrEmpty(item.SealTypeCode)) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CargoSealsQuery.F.SealTypeCodeMandatory"));
            }
            if (AppTool.IsNullOrEmpty(item.SealCompletenessStateCode)) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CargoSealsQuery.F.SealCompletenessStateCodeMandatory"));
            }
            if (AppTool.IsNullOrEmpty(item.UpdateReasonCode)) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CargoSealsQuery.F.UpdateReasonCodeMandatory"));
            }
            if (AppTool.IsNullOrEmpty(item.UpdateTypeCode)) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CargoSealsQuery.F.UpdateTypeCodeMandatory"));
            }

        });
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        var currRequestParams = new CargoSealsRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        currRequestParams.CustomFileNo = this.CustomFileNo;
        currRequestParams.DeclarationID = this.DeclarationId;
        currRequestParams.CargoIdentifierTypeCode = this.CargoIdentifierTypeCode;
        currRequestParams.CargoIdentifierKey1 = this.CargoIdentifierKey1;
        currRequestParams.CargoIdentifierKey2 = this.CargoIdentifierKey2;
        currRequestParams.CargoIdentifierKey3 = this.CargoIdentifierKey3;
        currRequestParams.ImporterNumber = this.ImporterNumber;
        currRequestParams.UpdateDate = this.UpdateDate;
        currRequestParams.CargoRowNumber = this.CargoRowNumber;
        currRequestParams.ContainerNumber = this.ContainerNumber;
        
        if (this.CargoSealObslist != null && this.CargoSealObslist.Length > 0) {
            currRequestParams.CargoSealList = [];
            this.CargoSealObslist.Collection.forEach((sealItem) => {
                var cargoSealDetails: CargoSealDetails = new CargoSealDetails();
                cargoSealDetails.SealNumber = sealItem.SealNumber;
                cargoSealDetails.SealTypeCode = sealItem.SealTypeCode;
                cargoSealDetails.SealCompletenessStateCode = sealItem.SealCompletenessStateCode;
                cargoSealDetails.UpdateReasonCode = sealItem.UpdateReasonCode;
                cargoSealDetails.UpdateTypeCode = sealItem.UpdateTypeCode;
                cargoSealDetails.Remarks = sealItem.Remarks;
                currRequestParams.CargoSealList.push(cargoSealDetails);
            });
        }
        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת מסר לעדכון סגרים", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._DeclarationMessagesService.PostSendCargoSealsRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    CustomFileNoTextChanged(searchtext) {

        if (!AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.CurrentSession.StartBusyIndicator("")
            this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
                .subscribe((myDeclarationResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();
                    this._LastFetchDeclarationList = myDeclarationResponse.Result;
                    if (AppTool.IsNullOrEmpty(this._LastFetchDeclarationList)) {
                        this.SetFieldsEnabled(true);
                    } else {
                        this.CurrentSession.StartBusyIndicator("")
                        this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.CustomFileNo)
                            .subscribe((myResponse: ServiceResponse) => {
                                this.CurrentSession.StopBusyIndicator();
                                this.FetchConsignment(myResponse, false);
                            });
                    }

                });

        }
    }

    SetFieldsEnabled(isDisplay: boolean) {

        if (isDisplay) {
            this.DeclarationId = "";
            this.DeclarationNumber = "";
            this.CargoIdentifierTypeCode = "";
            this.CargoIdentifierKey1 = "";
            this.CargoIdentifierKey2 = "";
            this.CargoIdentifierKey3 = "";
            this.ImporterNumber = null;
        }

        this.UIProperties.SetEnabled("CargoIdentifierTypeCode", this.ObjectTableName, isDisplay);
        this.UIProperties.SetEnabled("CargoIdentifierKey1", this.ObjectTableName, isDisplay);
        this.UIProperties.SetEnabled("CargoIdentifierKey2", this.ObjectTableName, isDisplay);
        this.UIProperties.SetEnabled("CargoIdentifierKey2", this.ObjectTableName, isDisplay);
        this.UIProperties.SetEnabled("ImporterNumber", this.ObjectTableName, isDisplay);
    }

    FetchConsignment(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        this._LastFetchConsignmentPMList = myResponse.Result
        if (this._LastFetchConsignmentPMList != null) {
            var pm = this._LastFetchConsignmentPMList[0]
            this.DeclarationId = pm.DeclarationId;
            this.DeclarationNumber = this._LastFetchDeclarationList.DeclarationNumber;
            this.ImporterNumber = this._LastFetchDeclarationList.ImporterCode;
            this.CargoIdentifierTypeCode = pm.CargoTypeCode;
            this.CargoIdentifierKey1 = pm.ManifestNumber;
            this.CargoIdentifierKey2 = pm.SecondCargoID;
            this.CargoIdentifierKey3 = pm.ThirdCargoID;
            this.SetFieldsEnabled(false);

        } else {
            this.SetFieldsEnabled(true);

        }
    }

    AddCargoSealCommand() {
        var newCargoSealPM = new CargoSealPM(this.EntityPM);
        //newCargoSealPM.CargoSealIdentifierId = this.EntityPM.ClaimId;
        //newCargoSealPM.Tenant = this.EntityPM.Tenant;
        this.CargoSealObslist.Insert(new CargoSealComponent(newCargoSealPM));
    }

    DeleteCargoSealCommand(item) {

        if (!AppTool.IsNullOrEmpty(item)) {
            this.CargoSealObslist.Remove(item);
        }
    }

    ShowMore(itemContent) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 1000;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = TextCodeTranslator.Translate("Customs.CargoSeal.F.Remarks");
        logitudeWindow.WindowArgs = itemContent;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCommunications/Components/Communications/LogFieldComponent');
    }

}

export class CargoSealComponent extends BaseComponent {
    public ObjectTableName = "Customs.CargoSeal";
    public DataContext: CargoSealComponent = this;

    constructor(public entityPM: CargoSealPM) {
        super();
    }

    public get SealNumber() { return this.entityPM.SealNumber; }
    public set SealNumber(newValue: string) { this.entityPM.SealNumber = newValue; }

    public get Remarks() { return this.entityPM.Remarks; }
    public set Remarks(newValue: string) { this.entityPM.Remarks = newValue; }

    public get SealCompletenessStateCode() { return this.entityPM.SealCompletenessStateCode; }
    public set SealCompletenessStateCode(newValue: string) { this.entityPM.SealCompletenessStateCode = newValue; }

    public get SealCompletenessStatename() { return this.entityPM.SealCompletenessStatename; }
    public set SealCompletenessStatename(newValue: string) { this.entityPM.SealCompletenessStatename = newValue; }

    public get SealTypeCode() { return this.entityPM.SealTypeCode; }
    public set SealTypeCode(newValue: string) { this.entityPM.SealTypeCode = newValue; }

    public get SealTypeName() { return this.entityPM.SealTypeName; }
    public set SealTypeName(newValue: string) { this.entityPM.SealTypeName = newValue; }

    public get UpdateReasonCode() { return this.entityPM.UpdateReasonCode; }
    public set UpdateReasonCode(newValue: string) { this.entityPM.UpdateReasonCode = newValue; }

    public get UpdateReasonName() { return this.entityPM.UpdateReasonName; }
    public set UpdateReasonName(newValue: string) { this.entityPM.UpdateReasonName = newValue; }

    public get UpdateTypeCode() { return this.entityPM.UpdateTypeCode; }
    public set UpdateTypeCode(newValue: string) { this.entityPM.UpdateTypeCode = newValue; }

    public get UpdateTypeName() { return this.entityPM.UpdateTypeName; }
    public set UpdateTypeName(newValue: string) { this.entityPM.UpdateTypeName = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }
}
