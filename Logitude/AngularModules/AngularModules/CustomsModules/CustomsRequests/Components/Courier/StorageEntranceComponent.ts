import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationMessagesService } from '../../../../Customs/Services/WebServices/DeclarationMessagesService';
import { StorageEntranceUnloadingRequestParams } from '../../../../Customs/DataContract/RequestParams/StorageEntranceUnloadingRequestParams';
//import { StorageEntranceUnloadingResponseData } from '../../../../Customs/DataContract/ResponseData/StorageEntranceUnloadingResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { CustomSendOptionsArgs, SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DeclarationConsAcceptancePMService } from '../../../../Customs/Services/StandardPMs/DeclarationConsAcceptancePMService';
import {DeclarationConsAcceptancePM} from '../../../../Customs/EntityPMs/DeclarationConsAcceptancePM';

@Component({
    selector: 'StorageEntranceComponent',
    moduleId: module.id,
    templateUrl: './StorageEntranceComponent.html',
})

export class StorageEntranceComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {

    public DataContext: StorageEntranceComponent = this;
    public ObjectTableName: string = "Customs.DeclarationConsAcceptance";
    
    public StorageEntranceObservableList: ObservableCollection;

    _DeclarationConsAcceptancePMService: DeclarationConsAcceptancePMService = new DeclarationConsAcceptancePMService();
    _DeclarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();
    public ResponseMessage: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.StorageEntranceObservableList = new ObservableCollection([]);
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
            this.RequestParams = new StorageEntranceUnloadingRequestParams();
        }

        //if (this.ResponseData) {
        //    if (this.ResponseData.CourierBOLDetailsList) {
        //        this.StorageEntranceObservableList.InsertCollection(this.ResponseData.CourierBOLDetailsList);
        //    }
        //}
    }

    EditButtonClicked(item) {
        this.CurrentSession.CloseCurrentWindowEmit(item.cargoIdentifierKey3);
    }


    //#region Commands
    CheckStorageEntranceOcc() {

        if (this.StorageEntranceObservableList != null && this.StorageEntranceObservableList.Length >= 1) {
            var lastOccCounter: number = this.StorageEntranceObservableList.Length - 1;
            if (this.CheckIfEnterAllDetails(this.StorageEntranceObservableList.Collection[lastOccCounter]) != true) {
                var messageWindow = new MessageWindow();
                messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                messageWindow.Width = 250;
                messageWindow.Height = 150;
                messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show("ראשית יש להזין את כל הנתונים בשורה הקודמת");
                return false;
            }
        }
        return true;
    }

    SaveAndSendStorageEntranceOcc(item: DeclarationConsignmentAcceptanceComponent, customSendOptionsArgs: CustomSendOptionsArgs) {
        this.CurrentSession.StartBusyIndicatorSaving();
        this._DeclarationConsAcceptancePMService.insert(item.entityPM).subscribe((response: ServiceResponse) => {
            var result = response.Result;
            this.CurrentSession.StopBusyIndicator();
            console.log("[Response] DeclarationConsAcceptancePMService.insert ", result);
            if (!response.HasError) {
                this.SendStorageEntrance(item, customSendOptionsArgs);
            }
            else {
                this.SubmitCompleted(response);
            }
        });

    }

    SubmitCompleted(response: ServiceResponse) {
        if (!response.HasError) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            // To Check????
            var errors = [];
            //Validator.TryValidateObject(this.DeclarationPM, "Customs.DeclarationConsAcceptance", errors);

            if (errors.length > 0) {
                this.ValidationErrorsList = errors;
            }
        }
    }

    AddStorageEntranceCommand() {
        if (this.CheckStorageEntranceOcc() == true) {
            this.StorageEntranceObservableList.Insert(new DeclarationConsignmentAcceptanceComponent());
        }
    }

    private DeleteStorageEntranceCommand(item: DeclarationConsignmentAcceptanceComponent) {
        this.StorageEntranceObservableList.Remove(item);
    }

    OnRowEnded($event) {
        if (($event) == this.StorageEntranceObservableList.Length) {
            if (this.CheckStorageEntranceOcc() == true) {
                //this.SaveStorageEntranceOcc(this.StorageEntranceObservableList.Collection[$event-1]);
                this.AddStorageEntranceCommand();
            }
        }
    }

    CheckIfEnterAllDetails(item: DeclarationConsignmentAcceptanceComponent) {

        if (AppTool.IsNullOrEmpty(item.DeclarationNumber)) {
            return false;
        }
        if (AppTool.IsNullOrEmpty(item.ConsignmentNumber)) {
            return false;
        }
        if (AppTool.IsNullOrEmpty(item.ManifestNumber)) {
            return false;
        }
        if (AppTool.IsNullOrEmpty(item.EntryDate)) {
            return false;
        }
        if (AppTool.IsNullOrEmpty(item.Quantity)) {
            return false;
        }
        if (AppTool.IsNullOrEmpty(item.GrossWeight)) {
            return false;
        }
        if (AppTool.IsNullOrEmpty(item.PackageTypeCode)) {
            return false;
        }

        return true;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors(storageEntranceItem: DeclarationConsignmentAcceptanceComponent) {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(storageEntranceItem.DeclarationNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.FromDateMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(storageEntranceItem.ConsignmentNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(storageEntranceItem.EntryDate)) {
            var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(storageEntranceItem.GrossWeight)) {
            var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(storageEntranceItem.Quantity)) {
            var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        if (this.StorageEntranceObservableList == null || this.StorageEntranceObservableList.Length == 0) {
            this.ValidationErrorsList.push("חובה להזין לפחות שורה אחת");
            return;
        }

        this.StorageEntranceObservableList.Collection.forEach((storageEntranceItem: DeclarationConsignmentAcceptanceComponent) => {

            this.FillErrors(storageEntranceItem);
            if (this.ValidationErrorsList.length > 0) {
                return;
            }
            this.SaveAndSendStorageEntranceOcc(storageEntranceItem, customSendOptionsArgs);
        });
    }

    SendStorageEntrance(storageEntranceItem: DeclarationConsignmentAcceptanceComponent, customSendOptionsArgs: CustomSendOptionsArgs) {

        var currRequestParams = new StorageEntranceUnloadingRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.DeclarationNumber = storageEntranceItem.DeclarationNumber;
        currRequestParams.ConsignmentNumber = storageEntranceItem.ConsignmentNumber.toString();
        currRequestParams.ManifestNumber = storageEntranceItem.ManifestNumber;
        currRequestParams.DeclarationId = storageEntranceItem.DeclarationId;
        currRequestParams.EntryDate = storageEntranceItem.EntryDate;
        currRequestParams.GrossWeight = storageEntranceItem.GrossWeight.toString();
        currRequestParams.Quantity = storageEntranceItem.Quantity.toString();
        currRequestParams.PackageTypeCode = storageEntranceItem.PackageTypeCode;

        CustomMessageProgressComponent
                .ShowProgressBar(currRequestParams.PBId, "שליחת מסר זמינות כניסה למחסן", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._DeclarationMessagesService.PostStorageEntranceUnloadingRequest(currRequestParams)
            .subscribe(() => { }
            );
    
    }

    //#endregion Commands
}

export class DeclarationConsignmentAcceptanceComponent extends BaseComponent {
    public DataContext: DeclarationConsignmentAcceptanceComponent = this;
    public ObjectTableName: string = "Customs.DeclarationConsAcceptance";

    public entityPM: DeclarationConsAcceptancePM = new DeclarationConsAcceptancePM();

    private declarationNumber: string;
    private manifestNumber: string;
    private packageTypeName: string;

    public _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.SetScreenFieldsEditability(false);
    }

    //#region Properties
    get DeclarationId() { return this.entityPM.DeclarationId; }
    set DeclarationId(value: string) {
        if (this.entityPM.DeclarationId != value) {
            this.entityPM.DeclarationId = value;
        }
    }

    get DeclarationNumber() { return this.declarationNumber; }
    set DeclarationNumber(value: string) {
        if (this.declarationNumber != value) {
            this.declarationNumber = value;
        }
    }

    get ConsignmentNumber() { return this.entityPM.ConsignmentNumber; }
    set ConsignmentNumber(value: number) {
        if (this.entityPM.ConsignmentNumber != value) {
            this.entityPM.ConsignmentNumber = value;
        }
    }

    get ManifestNumber() { return this.manifestNumber; }
    set ManifestNumber(value: string) {
        if (this.manifestNumber != value) {
            this.manifestNumber = value;
        }
    }

    get EntryDate() { return this.entityPM.EntryDate; }
    set EntryDate(value: Date) {
        if (this.entityPM.EntryDate != value) {
            this.entityPM.EntryDate = value;
        }
    }

    get Quantity() { return this.entityPM.Quantity; }
    set Quantity(value: number) {
        if (this.entityPM.Quantity != value) {
            this.entityPM.Quantity = value;
        }
    }

    get GrossWeight() { return this.entityPM.GrossWeight; }
    set GrossWeight(value: number) {
        if (this.entityPM.GrossWeight != value) {
            this.entityPM.GrossWeight = value;
        }
    }

    get PackageTypeCode() { return this.entityPM.PackageTypeCode; }
    set PackageTypeCode(value: string) {
        if (this.entityPM.PackageTypeCode != value) {
            this.entityPM.PackageTypeCode = value;
        }
    }

    public get PackageTypeName() { return this.packageTypeName; }
    public set PackageTypeName(newValue: string) { this.packageTypeName = newValue; }

    //#endregion Properties

    public SetScreenFieldsEditability(isDisplayOnly: boolean) {
        this.UIProperties.SetEnabled("ConsignmentNumber", this.ObjectTableName, isDisplayOnly);
        this.UIProperties.SetEnabled("EntryDate", this.ObjectTableName, isDisplayOnly);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, isDisplayOnly);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isDisplayOnly);
        this.UIProperties.SetEnabled("PackageTypecode", this.ObjectTableName, isDisplayOnly);
        this.UIProperties.SetEnabled("PackageTypeName", this.ObjectTableName, isDisplayOnly);
    }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }

    DeclarationNumberTextChanged(searchtext) {
        if (AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }

        //this.DueChangeClearChildField(false);

        this.CurrentSession.StartBusyIndicator("")
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchDeclaration(myResponse);
            });
    }

    FetchDeclaration(myResponse: ServiceResponse) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            if (lastFetchDeclarationList.IsCourierDeclaration != true) {
                //this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, "ההצהרה לא מסוג בלדר");
                var messageWindow = new MessageWindow();
                messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                messageWindow.Width = 250;
                messageWindow.Height = 150;
                messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show("ההצהרה לא מסוג בלדר");
                return;
            }
            this.CurrentSession.StartBusyIndicator("")
            this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(lastFetchDeclarationList.CustomFileNo)
                .subscribe((myResponse: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    this.FetchConsignment(myResponse, false);
                });

            this.EntryDate = DateTool.GetDateByDay(+0);
            this.SetScreenFieldsEditability(true);
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        } else {
            this.SetValidityDeclarationNumber();
        }
    }

    FetchConsignment(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        var lastFetchConsignmentPMList = myResponse.Result
        if (lastFetchConsignmentPMList != null) {
            var pm = lastFetchConsignmentPMList[0]
            this.entityPM.Tenant = pm.Tenant;
            this.entityPM.DeclarationId = pm.DeclarationId;
            this.ManifestNumber = pm.ManifestNumber;
            this.entityPM.ConsignmentNumber = pm.ConsignmentNumber;
            this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, false);
        }
    }

    SetValidityDeclarationNumber() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    }

}
