import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { DeclarationMessagesService } from '../../../../Customs/Services/WebServices/DeclarationMessagesService';
import { WarehouseBlockBalanceRequestParams } from '../../../../Customs/DataContract/RequestParams/WarehouseBlockBalanceRequestParams';
import { WarehouseBlockBalanceResponseData } from '../../../../Customs/DataContract/ResponseData/WarehouseBlockBalanceResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    selector: 'WarehouseBlockBalanceComponent',
    moduleId: module.id,
    templateUrl: './WarehouseBlockBalanceComponent.html',
})

export class WarehouseBlockBalanceComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: WarehouseBlockBalanceComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _DeclarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();

    public BlockSpecialActivitiesList: ObservableCollection;
    public ActionList: ObservableCollection;
    public StorageActionList: ObservableCollection;
    public GoodsItemByInvoiceList: ObservableCollection;

    constructor() {
        super();
        this.BlockSpecialActivitiesList = new ObservableCollection([]);
        this.ActionList = new ObservableCollection([]);
        this.StorageActionList = new ObservableCollection([]);
        this.GoodsItemByInvoiceList = new ObservableCollection([]);

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
            this.RequestParams = new WarehouseBlockBalanceRequestParams();
            this.SetIsByDeclarationNumber(true);
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
        }

        if (this.ResponseData) {
            if (this.ResponseData.BlockSpecialActivitiesList) {
                this.BlockSpecialActivitiesList.InsertCollection(this.ResponseData.BlockSpecialActivitiesList);
            }

            if (this.ResponseData.ActionList) {
                this.ActionList.InsertCollection(this.ResponseData.ActionList);
            }

            if (this.ResponseData.StorageActionList) {
                for (let item of this.ResponseData.StorageActionList) {
                    item.PackingDetailsListObs = new ObservableCollection(item.PackingDetailsList);
                }
                this.StorageActionList.InsertCollection(this.ResponseData.StorageActionList);
            }

            if (this.ResponseData.GoodsItemByInvoiceList) {
                this.GoodsItemByInvoiceList.InsertCollection(this.ResponseData.GoodsItemByInvoiceList);
            }
        }
        else {
            this.ResponseData = new WarehouseBlockBalanceResponseData();
        }
    }

    OnRowLoaded(myRow: any) {
        if (myRow) {
            myRow.SetExpandaple(true);
        }
    }

    //#region Properties
    SetIsByDeclarationNumber(newValue: boolean) {
        this.ClearOldValues();
        this.IsByDeclarationNumber = newValue;
    }

    get IsByDeclarationNumber() { return this.RequestParams ? this.RequestParams.DeclarationRadio : null; }
    set IsByDeclarationNumber(newValue: boolean) {
        if (this.RequestParams.DeclarationRadio != newValue) {
            this.RequestParams.DeclarationRadio = newValue;

            if (newValue == true) {
                this.IsByStorageSite = false;
            }
        }
    }

    SetIsByStorageSite(newValue: boolean) {
        this.ClearOldValues();
        this.IsByStorageSite = newValue;
    }

    ClearOldValues() {
        this.CustomFileNo = "";
        this.DeclarationNumber = "";
        this.StorageSiteNumber = "";
        this.WarehouseBlockNumber = "";
        this.DisplayGoodsItemByInvoice = "";
    }

    get IsByStorageSite() { return this.RequestParams ? this.RequestParams.StorageSiteRadio : null; }
    set IsByStorageSite(newValue: boolean) {
        if (this.RequestParams.StorageSiteRadio != newValue) {
            this.RequestParams.StorageSiteRadio = newValue;

            if (newValue == true) {
                this.IsByDeclarationNumber = false;
            }
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
        if (value) {
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("DeclarationNumber", null, true);
        }
    }

    get DisplayGoodsItemByInvoice() { return this.RequestParams ? this.RequestParams.DisplayGoodsItemByInvoice : null; }
    set DisplayGoodsItemByInvoice(value: string) {
        if (this.RequestParams.DisplayGoodsItemByInvoice != value) {
            this.RequestParams.DisplayGoodsItemByInvoice = value;
        }
    }

    get StorageSiteNumber() { return this.RequestParams ? this.RequestParams.StorageSiteNumber : null; }
    set StorageSiteNumber(value: string) {
        if (this.RequestParams.StorageSiteNumber != value) {
            this.RequestParams.StorageSiteNumber = value;
        }
    }

    get WarehouseBlockNumber() { return this.RequestParams ? this.RequestParams.WarehouseBlockNumber : null; }
    set WarehouseBlockNumber(value: string) {
        if (this.RequestParams.WarehouseBlockNumber != value) {
            this.RequestParams.WarehouseBlockNumber = value;
        }
    }

    //#endregion Properties


    //#endregion Response Properties
    get SiteText() { return this.ResponseData ? this.ResponseData.SiteText : null; }
    set SiteText(value: string) {
        if (this.ResponseData.SiteText != value) {
            this.ResponseData.SiteText = value;
        }
    }

    get OpeningDate() { return this.ResponseData ? this.ResponseData.OpeningDate : null; }
    set OpeningDate(value: string) {
        if (this.ResponseData.OpeningDate != value) {
            this.ResponseData.OpeningDate = value;
        }
    }

    get LogicalPackagesQuantityBalance() { return this.ResponseData ? this.ResponseData.LogicalPackagesQuantityBalance : null; }
    set LogicalPackagesQuantityBalance(value: string) {
        if (this.ResponseData.LogicalPackagesQuantityBalance != value) {
            this.ResponseData.LogicalPackagesQuantityBalance = value;
        }
    }

    get ResponseDeclarationNumber() { return this.ResponseData ? this.ResponseData.DeclarationNumber : null; }
    set ResponseDeclarationNumber(value: string) {
        if (this.ResponseData.DeclarationNumber != value) {
            this.ResponseData.DeclarationNumber = value;
        }
    }

    get OriginalOpeningDate() { return this.ResponseData ? this.ResponseData.OriginalOpeningDate : null; }
    set OriginalOpeningDate(value: string) {
        if (this.ResponseData.OriginalOpeningDate != value) {
            this.ResponseData.OriginalOpeningDate = value;
        }
    }

    get PhysicalPackagesQuantityBalance() { return this.ResponseData ? this.ResponseData.PhysicalPackagesQuantityBalance : null; }
    set PhysicalPackagesQuantityBalance(value: string) {
        if (this.ResponseData.PhysicalPackagesQuantityBalance != value) {
            this.ResponseData.PhysicalPackagesQuantityBalance = value;
        }
    }

    get ResponseWarehouseBlockNumber() { return this.ResponseData ? this.ResponseData.WarehouseBlockNumber : null; }
    set ResponseWarehouseBlockNumber(value: string) {
        if (this.ResponseData.WarehouseBlockNumber != value) {
            this.ResponseData.WarehouseBlockNumber = value;
        }
    }

    get MaxStorageDate() { return this.ResponseData ? this.ResponseData.MaxStorageDate : null; }
    set MaxStorageDate(value: string) {
        if (this.ResponseData.MaxStorageDate != value) {
            this.ResponseData.MaxStorageDate = value;
        }
    }

    get Value() { return this.ResponseData ? this.ResponseData.Value : null; }
    set Value(value: string) {
        if (this.ResponseData.Value != value) {
            this.ResponseData.Value = value;
        }
    }

    get ImporterTitle() { return this.ResponseData ? this.ResponseData.ImporterTitle : null; }
    set ImporterTitle(value: string) {
        if (this.ResponseData.ImporterTitle != value) {
            this.ResponseData.ImporterTitle = value;
        }
    }

    
    get StorageEntryPortChargeBalance() { return this.ResponseData ? this.ResponseData.StorageEntryPortChargeBalance : null; }
    set StorageEntryPortChargeBalance(value: string) {
        if (this.ResponseData.StorageEntryPortChargeBalance != value) {
            this.ResponseData.StorageEntryPortChargeBalance = value;
        }
    }

    get StorageEntryPortChargeCurrencyType() { return this.ResponseData ? this.ResponseData.StorageEntryPortChargeCurrencyType : null; }
    set StorageEntryPortChargeCurrencyType(value: string) {
        if (this.ResponseData.StorageEntryPortChargeCurrencyType != value) {
            this.ResponseData.StorageEntryPortChargeCurrencyType = value;
        }
    }

    get StorageEntryTransportBalance() { return this.ResponseData ? this.ResponseData.StorageEntryTransportBalance : null; }
    set StorageEntryTransportBalance(value: string) {
        if (this.ResponseData.StorageEntryTransportBalance != value) {
            this.ResponseData.StorageEntryTransportBalance = value;
        }
    }

    get StorageEntryTransportCurrencyType() { return this.ResponseData ? this.ResponseData.StorageEntryTransportCurrencyType : null; }
    set StorageEntryTransportCurrencyType(value: string) {
        if (this.ResponseData.StorageEntryTransportCurrencyType != value) {
            this.ResponseData.StorageEntryTransportCurrencyType = value;
        }
    }

    get StorageEntryInsuranceBalance() { return this.ResponseData ? this.ResponseData.StorageEntryInsuranceBalance : null; } 
    set StorageEntryInsuranceBalance(value: string) {
        if (this.ResponseData.StorageEntryInsuranceBalance != value) {
            this.ResponseData.StorageEntryInsuranceBalance = value;
        }
    }

    get StorageEntryInsuranceCurrencyType() { return this.ResponseData ? this.ResponseData.StorageEntryInsuranceCurrencyType : null; }
    set StorageEntryInsuranceCurrencyType(value: string) {
        if (this.ResponseData.StorageEntryInsuranceCurrencyType != value) {
            this.ResponseData.StorageEntryInsuranceCurrencyType = value;
        }
    }
    //#endregion Response Properties

    //#region Declaration Commands
    DueChangeClearChildField(sourceIsCostomFile: boolean): void {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        } else {

            this.CustomFileNo = "";
        }
        this.ValidationErrorsList = [];
    }

    CustomFileNoTextChanged(searchtext) {

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
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

        this.DueChangeClearChildField(false);

        SessionLocator.CurrentSession.StartBusyIndicator("")
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                SessionLocator.CurrentSession.StopBusyIndicator();

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
    //#endregion


    //#region General Commands
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;     

        if (this.IsByDeclarationNumber) {
            if (AppTool.IsNullOrEmpty(this.RequestParams.DeclarationNumber)) {
                var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }
        else if (this.IsByStorageSite) {
            if (AppTool.IsNullOrEmpty(this.RequestParams.StorageSiteNumber)) {
                var msg = TextCodeTranslator.Translate("Customs.General.O.StorageSiteMissing");
                this.ValidationErrorsList.push(msg);
            }
            if (AppTool.IsNullOrEmpty(this.RequestParams.WarehouseBlockNumber)) {
                var msg = TextCodeTranslator.Translate("Customs.General.O.WarehouseBlockMissing");
                this.ValidationErrorsList.push(msg);
            }
        }

    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new WarehouseBlockBalanceRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.DeclarationRadio = this.IsByDeclarationNumber;
        currRequestParams.StorageSiteRadio = this.IsByStorageSite;
        currRequestParams.DeclarationNumber = this.DeclarationNumber;
        currRequestParams.CustomFileNo = this.CustomFileNo;
        currRequestParams.StorageSiteNumber = this.StorageSiteNumber;
        currRequestParams.WarehouseBlockNumber = this.WarehouseBlockNumber;

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
            "שליחת שאילתא ליתרת מלאי בגוש", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._DeclarationMessagesService.PostWarehouseBlockBalanceRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }

    //#endregion Commands
}
