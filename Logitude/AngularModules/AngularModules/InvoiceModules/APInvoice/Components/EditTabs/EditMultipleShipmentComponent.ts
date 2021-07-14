import {Component}  from '@angular/core';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';
import {APInvoiceLinePM} from '../../../../Invoice/EntityPMs/APInvoiceLinePM';
import {APInvoiceMultipleShortPM} from '../../../../Invoice/EntityPMs/APInvoiceMultipleShortPM';
import {APInvoiceMultipleShipmentPM} from '../../../../Invoice/EntityPMs/APInvoiceMultipleShipmentPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {InvoiceDomainService} from '../../../../Invoice/Services/InvoiceDomainService';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ShipmentPayablePM} from '../../../../Shipment/EntityPMs/ShipmentPayablePM';
import {DateTool, AppTool, FormatTool, ArrayTool, FontTool} from '../../../../Infrastructure/Tools';
import {VatTypePercentagePM} from '../../../../Common/EntityPMs/VatTypePercentagePM';
import {UserList} from '../../../../Common/EntityLists/UserList';
import {VatTypeList} from '../../../../Common/EntityLists/VatTypeList';
import {ChargesTypeList} from '../../../../Common/EntityLists/ChargesTypeList';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
import {VatTypeListService} from '../../../../Common/Services/StandardLists/VatTypeListService';
import {ChargesTypeListService} from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {VATTypesGroupPM} from '../../../../Common/EntityPMs/VATTypesGroupPM';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { NumbersPipe } from '../../../../Infrastructure/Pipes/NumbersPipe';

@Component({
    
    templateUrl: './EditMultipleShipmentComponent.html',
})

export class EditMultipleShipmentComponent extends BaseComponent {
    public EntityPM: APInvoiceMultipleShortPM = null;
    public EntityShipmentPM: APInvoiceMultipleShipmentPM = null;
    public APInvoicePM: APInvoicePM;
    public ObjectTableName = "APInvoice";
    public DataContext = this;
    public IsEditingEnabled: boolean = false;
    public ItemsSource: ObservableCollection;
    public InvoiceCurrencyCode: string = null;
    public LocalCurrencyId: string = null;
    public LocalCurrencyCode: string = null;
    public ValidationErrorsList: string[] = [];
    public ShipmentId: string = null;
    public APInvoiceId: string = null;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");       
        this.ItemsSource = new ObservableCollection([]);
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.InitializeServices();
    }

    private myInvoiceDomainService: InvoiceDomainService;
    private myShipmentDomainService: ShipmentDomainService;
    private myCurrencyRatesService: CurrencyRatesService;
    private myCommonDomainService: CommonDomainService;
    public myUserListService: UserListService;
    public myVatTypeListService: VatTypeListService;
    public myChargesTypeListService: ChargesTypeListService;
    InitializeServices() {
        this.myInvoiceDomainService = new InvoiceDomainService();
        this.myShipmentDomainService = new ShipmentDomainService();
        this.myCurrencyRatesService = new CurrencyRatesService();
        this.myCommonDomainService = new CommonDomainService();
        this.myUserListService = new UserListService();
        this.myVatTypeListService = new VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService();
    }

    SetWindowArgs(args: any) {
        this.APInvoicePM = args['APInvoicePM'];
        this.EntityShipmentPM = args['EntityShipmentPM'];
        this.IsEditingEnabled = args['IsEditingEnabled'];
        this.ShipmentId = this.EntityShipmentPM.ShipmentId;
        this.APInvoiceId = this.APInvoicePM.Id;
        this.InvoiceCurrencyCode = this.APInvoicePM.InvoiceCurrencyCode;
        this.SetUIProperties();
        this.LoadEntity();
    }

    public VatTypeFilterIsEnabled: boolean = false;
    SetUIProperties() {
        this.SetUIProperties_VatTypeFilter();
    }
    SetUIProperties_VatTypeFilter() {
        var isFieldtEnabled = this.IsEditingEnabled;

        if (isFieldtEnabled) {
            if (AppTool.IsNullOrEmpty(this.VatTypeId)) {
                isFieldtEnabled = false;
            }
        }

        this.VatTypeFilterIsEnabled = isFieldtEnabled;
    }

    private myOpenPayables: ShipmentPayablePM[] = [];
    private LoadEntity() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.myInvoiceDomainService.GetSingleAPInvoiceShortPM(this.APInvoiceId, this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.EntityPM = myResponse.Result;

                if (this.IsEditingEnabled) {
                    this.LoadOpenPayables();
                }

                else {
                    this.BuildItemsSource();
                    this.CurrentSession.StopBusyIndicator();
                }
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
    private LoadOpenPayables() {
        this.myShipmentDomainService.GetInvoiceOpenAmountPayables(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.myOpenPayables = myResponse.Result;
            }

            var loadingDate = this.APInvoicePM.InvoiceDate;
            if (loadingDate == null) {
                loadingDate = DateTool.GetCurrentDateAsUtc();
            }

            this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.AccountingCurrencyId, loadingDate).subscribe((myResponse1: ServiceResponse) => {
                this.LastRatesList = myResponse1.Result;

                this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {
                    this.VatTypePercentagesList = myResponse2.Result;

                    this.BuildItemsSource();

                    this.CurrentSession.StopBusyIndicator();
                });
            });            
        });
    }
    private BuildItemsSource() {

        this.ItemsSource.Clear();
        this.RefreshSummery();

        this.EntityPM.InvoiceLines.forEach(line => {
            this.ItemsSource.Insert(new APInvoiceLineShortItem(line, this, false));
        });

        this.myOpenPayables = this.myOpenPayables.filter(f => f.ShipmentPayableParentId == null);

        this.myOpenPayables.forEach(item => {
            if (this.EntityPM.InvoiceLines.filter(f => f.EntityPayableId == item.Id).length == 0) {
                var line = new APInvoiceLinePM(null);
                line.APInvoiceId = this.APInvoiceId;
                line.Tenant = item.Tenant;
                line.ChargesTypeId = item.ChargesTypeId;
                line.ChargesTypeCode = item.ChargesTypeCode;
                line.ChargesTypeName = item.ChargesTypeName;
                line.EntityPayableId = item.Id;
                line.EntityId = item.ShipmentId;
                line.EntityReference = item.ShipmentNumber;
                line.VendorId = item.VendorId;
                line.VendorName = item.VendorName;
                line.ExpectedAmount = item.ExpectedAmount;
                line.OtherInvoicesAmounts = item.AccountedAmount;
                line.OpenAmount = item.OpenAmount;
                line.CorrectionAmount = item.CorrectionAmount;
                line.CorrectionNote = item.CorrectionNote;
                line.CorrectionByUserId = item.CorrectionByUserId;
                line.CorrectionDate = item.CorrectionDate;
                line.AmountTypeCode = item.ShipmentPayableAmountTypeCode;
                line.ForiegnCurrencyId = item.CurrencyId;
                line.ForiegnCurrencyCode = item.CurrencyCode;
                line.VatPercentage = this.GetVatTypePercentage(item.VatTypeId);
                line.ForiegnExchangeRate = this.GetCurrencyRate(item.CurrencyId);

                this.myChargesTypeListService.getSingleFromCache(line.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: ChargesTypeList = myResponse.Result;
                        if (list) {
                            line.Description = list.EnglishName;
                            line.LocalDescription = list.LocalName;

                            if (AppTool.IsNullOrEmpty(line.VatTypeId)) {
                                line.VatTypeId = list.VatTypeId;
                            }
                        }
                    }
                });

                if (!AppTool.IsNullOrEmpty(line.VatTypeId)) {
                    this.myVatTypeListService.getSingleFromCache(line.VatTypeId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list_VAT: VatTypeList = myResponse.Result;
                            if (list_VAT) {
                                line.VatTypeName = list_VAT.EnglishName;
                                line.VatIsMultiPercentage = list_VAT.IsMultiPercentage;

                                if (!list_VAT.IsMultiPercentage) {
                                    line.VatPercentage = this.GetVatTypePercentage(line.VatTypeId);
                                }
                            }
                        }
                    });
                }

                this.ItemsSource.Insert(new APInvoiceLineShortItem(line, this, false));
            }
        });

        this.RefreshSummery();
        this.CheckIsAllChecked();
    }

    private LastRatesList: LastRate[] = [];
    private VatTypePercentagesList: VatTypePercentagePM[] = [];
    GetCurrencyRate(myCurrencyId: string) {
        var myResult: number = null;

        if (!AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (myCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                myResult = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == myCurrencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }

        return myResult;
    }
    GetVatTypePercentage(myVatTypeId: string) {
        var myResult: number = null;

        var vatTypePercentagePM = this.VatTypePercentagesList.filter(d => d.VatTypeId == myVatTypeId)[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }

        return myResult;
    }

    get House() { return this.EntityShipmentPM.House; }
    get Master() { return this.EntityShipmentPM.Master; }
    get LongMaster() { return this.EntityShipmentPM.LongMaster; }
    get ShipmentNumber() { return this.EntityShipmentPM.ShipmentNumber; }
    get PartnerName() { return this.EntityShipmentPM.PartnerName; }
    get PartnerType() { return this.EntityShipmentPM.PartnerType; }
    get MainCarriageCarrierName() { return this.EntityShipmentPM.MainCarriageCarrierName; }
    get OpenAmount() { return this.EntityShipmentPM.OpenAmount; }

    // VAT Type Filter
    private vatTypeId: string;
    get VatTypeId() { return this.vatTypeId; }
    set VatTypeId(newValue: string) {
        if (this.vatTypeId != newValue) {
            this.vatTypeId = newValue;
            this.SetUIProperties_VatTypeFilter();
        }
    }
    VatTypeFilterClicked() {
        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {

            this.ItemsSource.Collection.forEach(item => {
                item.VatTypeId = this.VatTypeId;
            });

            this.VatTypeId = null;
            this.ComputeTotals();
        }        
    }

    // Totals
    get IsCurrencyFilterVisible() {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(this.APInvoicePM.InvoiceCurrencyId)) {
            if (this.LocalCurrencyId != this.APInvoicePM.InvoiceCurrencyId) {
                myResult = true;
            }
        }

        return myResult;
    }

    public SummeryAmount: number = 0;
    public SummeryOpenAmount: number = 0;
    public SummeryCurrencyCode: string = this.InvoiceCurrencyCode;

    private isTotalInLocalCurrency: boolean = false;
    get IsTotalInLocalCurrency() { return this.isTotalInLocalCurrency; }
    set IsTotalInLocalCurrency(value: boolean) {
        if (this.isTotalInLocalCurrency != value) {
            this.isTotalInLocalCurrency = value;
            this.RefreshSummery();
        }
    }

    RefreshSummery() {
        var mySummeryAmount = 0;
        var mySummeryOpenAmount = 0;
        var mySummeryCurrencyCode = this.InvoiceCurrencyCode;

        if (this.EntityPM) {
            if (this.IsTotalInLocalCurrency) {

                mySummeryAmount = ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount")
                mySummeryCurrencyCode = this.LocalCurrencyCode;

                this.EntityPM.InvoiceLines.forEach(item => {
                    if (!AppTool.IsNullOrEmpty(item.OpenAmount) && !AppTool.IsNullOrEmpty(item.ForiegnExchangeRate)) {
                        mySummeryOpenAmount = mySummeryOpenAmount + (item.OpenAmount * item.ForiegnExchangeRate);
                    }
                });
            }

            else {
                mySummeryAmount = ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount")
                mySummeryCurrencyCode = this.InvoiceCurrencyCode;

                this.EntityPM.InvoiceLines.forEach(item => {
                    if (!AppTool.IsNullOrEmpty(item.OpenAmount) && !AppTool.IsNullOrEmpty(item.ForiegnExchangeRate)) {
                        mySummeryOpenAmount = mySummeryOpenAmount + (item.OpenAmount * item.ForiegnExchangeRate / this.EntityPM.InvoiceCurrencyExchangeRate);
                    }
                });
            }
        }

        this.SummeryAmount = mySummeryAmount;
        this.SummeryOpenAmount = mySummeryOpenAmount;
        this.SummeryCurrencyCode = mySummeryCurrencyCode;
    }
    ComputeTotals() {
        this.RefreshSummery();

        var subInLocal: number = ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount");
        var subInInvoice: number = ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount");
        this.EntityPM.SubTotalInLocalCurrency = AppTool.Round(subInLocal, 2);
        this.EntityPM.SubTotalInInvoiceCurrency = AppTool.Round(subInInvoice, 2);
    }

    AddLineClicked() {
        var line: APInvoiceLinePM = new APInvoiceLinePM(null);
        line.Tenant = this.APInvoicePM.Tenant;
        line.APInvoiceId = this.APInvoicePM.Id;
        line.VendorId = this.APInvoicePM.VendorId;
        line.VendorName = this.APInvoicePM.VendorName;
        line.EntityId = this.EntityShipmentPM.ShipmentId;
        line.EntityReference = this.EntityShipmentPM.ShipmentNumber;
        line.AmountTypeCode = "NEXP";
        line.ForiegnCurrencyId = this.APInvoicePM.InvoiceCurrencyId;
        line.ForiegnCurrencyCode = this.APInvoicePM.InvoiceCurrencyCode;
        line.ForiegnExchangeRate = this.APInvoicePM.InvoiceCurrencyExchangeRate;
        var itemComponent = new APInvoiceLineShortItem(line, this, true);
        this.RunWindow(itemComponent, TextCodeTranslator.Translate("APInvoiceLine.O.AddInvoiceLine"));        
    }
    EditLineClicked(itemComponent: APInvoiceLineShortItem) {
        this.RunWindow(itemComponent, TextCodeTranslator.Translate("APInvoiceLine.O.EditInvoiceLine"));
    }
    RunWindow(itemComponent: APInvoiceLineShortItem, titile:string) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = titile;
        logWindow.DataContext = itemComponent;
        logWindow.Show('./InvoiceModules/APInvoice/Components/EditTabs/AddEditMultipleAPInvoiceLineComponent');
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.EntityPM.InvoiceLines.filter(f => f.EntityId == this.ShipmentId).length == 0) {
            errors.push("You sould add invoice lines");
        }

        else {
            this.EntityPM.InvoiceLines.forEach(item => {
                Validator.TryValidateObject(item, "APInvoiceLine", errors);

                if (AppTool.IsNullOrEmpty(item.VatTypeId)) {
                    errors.push("VAT Type Field is Required");
                }
            });            
        }

        this.ValidationErrorsList = errors;
        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();

            this.myInvoiceDomainService.PutSingleAPInvoiceShortPM(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }

    // IsAllChecked
    CheckIsAllChecked() {
        var myResult = false;

        if (this.ItemsSource) {
            if (this.ItemsSource.Collection.length > 0) {

                var allItems_NotChecked: APInvoiceLineShortItem[] = this.ItemsSource.Collection.filter(f => f.IsChecked == false);

                if (allItems_NotChecked.length == 0) {
                    myResult = true;
                }

                else if (allItems_NotChecked.filter(f => f.IsMatch == true).length == 0) {
                    myResult = true;
                }                
            }
        }

        this.isAllChecked = myResult;
    }

    private isAllChecked: boolean = false;
    public get IsAllChecked() { return this.isAllChecked; }
    public set IsAllChecked(value: boolean) {
        if (this.isAllChecked != value) {
            this.isAllChecked = value;

            var items: APInvoiceLineShortItem[] = this.ItemsSource.Collection.filter(f => f.IsMatch == true);

            if (items.length > 0) {
                if (value) {
                    items.filter(f => f.IsChecked == false).forEach((item: APInvoiceLineShortItem) => {
                        item.ForiegnCurrencyAmount = item.OpenAmount;
                        this.EntityPM.AddInvoiceLinePM(item.EntityPM);
                        item.isChecked = value;
                        item.SetCellColor();
                    });
                }

                else {
                    items.filter(f => f.IsChecked == true).forEach((item: APInvoiceLineShortItem) => {
                        item.ForiegnCurrencyAmount = null;
                        this.EntityPM.RemoveInvoiceLinePM(item.EntityPM);
                        item.isChecked = value;
                        item.SetCellColor();
                    });
                }

                this.ComputeTotals();
            }
        }
    }
}
export class APInvoiceLineShortItem extends BaseComponent {
    public EntityPM: APInvoiceLinePM = null;
    public APInvoicePM: APInvoicePM;
    public ObjectTableName = "APInvoiceLine";
    public DataContext = this;
    public ProfitCurrencyId: string = null;
    public InvoiceCurrencyId: string = null;
    public InvoiceCurrencyCode: string = null;
    public IsMatch: boolean = true;
    public IsEditingEnabled: boolean = false;
    public AddNewLineMode: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityPM: APInvoiceLinePM, public fatherComponent: EditMultipleShipmentComponent, isAddNewLineMode: boolean) {
        super();
        this.EntityPM = entityPM;
        this.APInvoicePM = fatherComponent.APInvoicePM;
        this.ProfitCurrencyId = this.APInvoicePM.ProfitCurrencyId;
        this.InvoiceCurrencyId = this.APInvoicePM.InvoiceCurrencyId;
        this.InvoiceCurrencyCode = this.APInvoicePM.InvoiceCurrencyCode;
        this.AddNewLineMode = isAddNewLineMode;

        if (!AppTool.IsNullOrEmpty(this.VendorId)) {
            if (this.VendorId != this.APInvoicePM.VendorId) {
                this.IsMatch = false;
            }
        }

        if (this.fatherComponent.EntityPM.InvoiceLines.indexOf(this.EntityPM) > -1) {
            this.isChecked = true;
        }

        if (this.fatherComponent.IsEditingEnabled && this.IsMatch) {
            this.IsEditingEnabled = true;
        }

        this.SetCellColor();
        this.ReadVatTypeData();
        this.SetUIProperties();
        this.GetCorrectionUserName();
    }

    private GetCorrectionUserName() {
        if (!AppTool.IsNullOrEmpty(this.CorrectionByUserId)) {
            this.fatherComponent.myUserListService.getSingleFromCache(this.CorrectionByUserId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: UserList = myResponse.Result;
                    if (list != null) {
                        this.CorrectionByUserName = list.EnglishName;
                    }
                }
            });
        }
    }

    public CellBackgroundColor: string = FontTool.CellDisabledBackground;
    public CellBackgroundColor_Editable: string = "transparent";
    SetCellColor() {
        if (this.IsChecked) {
            this.CellBackgroundColor = FontTool.CellIsCheckedBackground;
            this.CellBackgroundColor_Editable = FontTool.CellIsCheckedBackground;
        }

        else {
            this.CellBackgroundColor = FontTool.CellDisabledBackground;

            if (!this.IsEditingEnabled) {
                this.CellBackgroundColor_Editable = FontTool.CellDisabledBackground;
            }

            else {
                this.CellBackgroundColor_Editable = "transparent";
            }
        }        
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForiegnCurrencyId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ForiegnCurrencyAmount", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);

        var isOpenAmountEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.EntityPM.AmountTypeCode != "NEXP") {
                isOpenAmountEnabled = true;
            }
        }

        var isChargeEnabled = false;
        if (this.IsEditingEnabled) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.EntityPayableId)) {
                isChargeEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("OpenAmount", this.ObjectTableName, isOpenAmountEnabled);
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, isChargeEnabled);
        this.SetUIProperties_VAT();
    }
    private SetUIProperties_VAT() {
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, this.IsEditingEnabled);

        if (this.VatIsMultiPercentage) {
            this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, false);
        }

        var isVatPercentageRequired = false;

        if (AppTool.IsNullOrEmpty(this.VatPercentage)) {
            isVatPercentageRequired = true;

            if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {
                if (this.VatIsMultiPercentage) {
                    isVatPercentageRequired = false;
                }
            }
        }

        this.UIProperties.SetRequired("VatPercentage", this.ObjectTableName, isVatPercentageRequired);
    }

    public isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;

            if (value) {
                if (this.EntityPM.AmountTypeCode != "NEXP") {
                    if (AppTool.IsNullOrZero(this.ForiegnCurrencyAmount) && !AppTool.IsNullOrZero(this.OpenAmount)) {
                        this.ForiegnCurrencyAmount = this.OpenAmount;
                    }
                }

                this.fatherComponent.EntityPM.AddInvoiceLinePM(this.EntityPM);
            }

            else {
                this.InvoiceCurrencyAmount = null;
                this.fatherComponent.EntityPM.RemoveInvoiceLinePM(this.EntityPM);
            }

            this.SetCellColor();
            this.SetUIProperties();
            this.fatherComponent.ComputeTotals();
            this.fatherComponent.CheckIsAllChecked();
        }
    }

    // ChargesTypeId
    get ChargesTypeId() { return this.EntityPM.ChargesTypeId; }
    set ChargesTypeId(value: string) {
        if (this.EntityPM.ChargesTypeId != value) {
            this.EntityPM.ChargesTypeId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.ChargesTypeCode = null;
                this.ChargesTypeName = null;
                this.Description = null;
                this.LocalDescription = null;
                this.VatTypeId = null;
            }

            else {
                this.fatherComponent.myChargesTypeListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: ChargesTypeList = myResponse.Result;
                        if (list != null) {
                            this.ChargesTypeCode = list.Code;
                            this.ChargesTypeName = list.EnglishName;
                            this.Description = list.EnglishName;
                            this.LocalDescription = list.LocalName;
                            this.VatTypeId = list.VatTypeId;
                        }
                    }
                });
            }
        }
    }

    get ChargesTypeCode() { return this.EntityPM.ChargesTypeCode; }
    set ChargesTypeCode(value: string) {
        if (this.EntityPM.ChargesTypeCode != value) {
            this.EntityPM.ChargesTypeCode = value;
        }
    }

    get ChargesTypeName() { return this.EntityPM.ChargesTypeName; }
    set ChargesTypeName(value: string) {
        if (this.EntityPM.ChargesTypeName != value) {
            this.EntityPM.ChargesTypeName = value;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get LocalDescription() { return this.EntityPM.LocalDescription; }
    set LocalDescription(value: string) {
        if (this.EntityPM.LocalDescription != value) {
            this.EntityPM.LocalDescription = value;
        }
    }

    // VatTypeId
    get VatTypeId() { return this.EntityPM.VatTypeId; }
    set VatTypeId(value: string) {
        if (this.EntityPM.VatTypeId != value) {
            this.EntityPM.VatTypeId = value;

            this.VatPercentage = this.fatherComponent.GetVatTypePercentage(value);

            if (AppTool.IsNullOrEmpty(value)) {
                this.VatTypeName = null;
                this.VatPercentage = null;
                this.VatIsMultiPercentage = false;
                this.EntityPM.ExternalTAXItemId = null;
                this.ReadVatTypeData();
                this.SetUIProperties_VAT();
            }

            else {
                this.fatherComponent.myVatTypeListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VatTypeList = myResponse.Result;
                        if (list != null) {
                            this.VatTypeName = list.EnglishName;
                            this.VatIsMultiPercentage = list.IsMultiPercentage;
                            this.EntityPM.ExternalTAXItemId = list.ExternalTAXItemId;

                            if (list.IsMultiPercentage) {
                                this.VatPercentage = null;
                            }

                            else {
                                this.VatPercentage = this.fatherComponent.GetVatTypePercentage(this.VatTypeId);
                            }

                            this.ReadVatTypeData();
                            this.SetUIProperties_VAT();
                        }
                    }
                });
            }
        }
    }

    get VatTypeName() { return this.EntityPM.VatTypeName; }
    set VatTypeName(value: string) {
        if (this.EntityPM.VatTypeName != value) {
            this.EntityPM.VatTypeName = value;
            this.ReadVatTypeData();
        }
    }

    get VatPercentage() { return this.EntityPM.VatPercentage; }
    set VatPercentage(value: number) {
        if (this.EntityPM.VatPercentage != value) {
            this.EntityPM.VatPercentage = value;
            this.ReadVatTypeData();
            this.fatherComponent.ComputeTotals();
            this.SetUIProperties_VAT();
        }
    }

    get VatIsMultiPercentage() { return this.EntityPM.VatIsMultiPercentage; }
    set VatIsMultiPercentage(value: boolean) {
        if (this.EntityPM.VatIsMultiPercentage != value) {
            this.EntityPM.VatIsMultiPercentage = value;
            this.SetUIProperties_VAT();
        }
    }

    public VatTypeCell: string;
    public VatTypeCellColor: string;
    public VatTypeUpdateIsVisible: boolean = false;
    public VatTypeMultiIconVisible: boolean = false;
    public VatTypesGroups: VATTypesGroupPM[] = [];
    ReadVatTypeData() {
        var myValue: string = null;
        var myColor: string = FontTool.Black;
        var isUpdateVisible = false;
        var isMultiIconVisible = false;
        this.VatTypesGroups = [];
        var pipe = new NumbersPipe();

        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {

            if (this.VatIsMultiPercentage) {
                myValue = this.VatTypeName;
                myColor = FontTool.Black;
                isMultiIconVisible = true;
                this.VatTypesGroups = SessionLocator.AllVatTypesGroups.filter(f => f.GroupVATTypeId == this.VatTypeId);
            }

            else if (this.VatPercentage != null) {
                myValue = this.VatTypeName + " (" + pipe.transform(this.VatPercentage, "N3") + "%)";
                myColor = FontTool.Black;
            }

            else {
                myValue = TextCodeTranslator.Translate("ARInvoice.S.Details.NoVat");
                myColor = FontTool.Red;
                isUpdateVisible = true;
            }
        }

        this.VatTypeCell = myValue;
        this.VatTypeCellColor = myColor;
        this.VatTypeUpdateIsVisible = isUpdateVisible;
        this.VatTypeMultiIconVisible = isMultiIconVisible;
    }
    UpdateVatPercentageClicked() {
        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Add VAT Type Percentage";
            logWindow.WindowArgs = this.VatTypeId;
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateVATPercentage/UpdateVATPercentageComponent');

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.fatherComponent.ItemsSource.Collection.filter(f => f.VatTypeId == this.VatTypeId).forEach(item => {
                            item.SetVatPercentage(comp.Percentage);
                        });
                    }
                });
            });
        }
    }

    // Properties
    get ForiegnCurrencyId() { return this.EntityPM.ForiegnCurrencyId; }
    set ForiegnCurrencyId(value: string) {
        if (this.EntityPM.ForiegnCurrencyId != value) {
            this.EntityPM.ForiegnCurrencyId = value;
        }
    }

    get ForiegnCurrencyCode() { return this.EntityPM.ForiegnCurrencyCode; }
    set ForiegnCurrencyCode(value: string) {
        if (this.EntityPM.ForiegnCurrencyCode != value) {
            this.EntityPM.ForiegnCurrencyCode = value;
        }
    }

    get VendorId() { return this.EntityPM.VendorId; }
    set VendorId(value: string) {
        if (this.EntityPM.VendorId != value) {
            this.EntityPM.VendorId = value;
        }
    }

    get VendorName() { return this.EntityPM.VendorName; }
    set VendorName(value: string) {
        if (this.EntityPM.VendorName != value) {
            this.EntityPM.VendorName = value;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    // Amounts
    get ExpectedAmount() {
        var myResult = 0;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ExpectedAmount)) {
            myResult = this.EntityPM.ExpectedAmount;
        }

        return myResult;
    }

    get OtherInvoicesAmounts() {
        var myResult = 0;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.OtherInvoicesAmounts)) {
            myResult = this.EntityPM.OtherInvoicesAmounts;
        }

        return myResult;
    }

    get ForiegnCurrencyAmount() { return this.EntityPM.ForiegnCurrencyAmount; }
    set ForiegnCurrencyAmount(value: number) {
        if (this.EntityPM.ForiegnCurrencyAmount != value) {
            this.EntityPM.ForiegnCurrencyAmount = AppTool.Round(value, 2);

            this.ComputeOpenAmount();
            this.ComputeOtherAmounts();

            if (!this.AddNewLineMode) {
                if (AppTool.IsNullOrZero(value)) {
                    this.IsChecked = false;
                }

                else {
                    this.IsChecked = true;
                }
            }
        }
    }

    get InvoiceCurrencyAmount() { return this.EntityPM.InvoiceCurrencyAmount; }
    set InvoiceCurrencyAmount(value: number) {
        if (this.EntityPM.InvoiceCurrencyAmount != value) {
            this.EntityPM.InvoiceCurrencyAmount = AppTool.Round(value, 2);

            var valueInLocal: number = null;
            var foriegnAmount: number = null;
            if (!AppTool.IsNullOrEmpty(value)) {
                valueInLocal = value * this.APInvoicePM.InvoiceCurrencyExchangeRate;
                foriegnAmount = valueInLocal / this.EntityPM.ForiegnExchangeRate;
            }

            this.EntityPM.ForiegnCurrencyAmount = AppTool.Round(foriegnAmount, 2);

            if (!this.AddNewLineMode) {
                if (AppTool.IsNullOrZero(foriegnAmount)) {
                    this.IsChecked = false;
                }

                else {
                    this.IsChecked = true;
                }
            }
            
            this.ComputeOpenAmount();

            this.LocalCurrencyAmount = valueInLocal;

            if (this.ForiegnCurrencyId == this.ProfitCurrencyId) {
                this.ProfitCurrencyAmount = this.ForiegnCurrencyAmount;
            }

            else {
                this.ProfitCurrencyAmount = this.LocalCurrencyAmount / this.APInvoicePM.ProfitCurrencyExchangeRate;
            }

            this.fatherComponent.ComputeTotals();
        }
    }

    get LocalCurrencyAmount() { return this.EntityPM.LocalCurrencyAmount; }
    set LocalCurrencyAmount(value: number) {
        if (this.EntityPM.LocalCurrencyAmount != value) {
            this.EntityPM.LocalCurrencyAmount = AppTool.Round(value, 2);
        }
    }

    get ProfitCurrencyAmount() { return this.EntityPM.ProfitCurrencyAmount; }
    set ProfitCurrencyAmount(value: number) {
        if (this.EntityPM.ProfitCurrencyAmount != value) {
            this.EntityPM.ProfitCurrencyAmount = AppTool.Round(value, 2);
        }
    }

    get OpenAmount() { return this.EntityPM.OpenAmount; }
    set OpenAmount(value: number) {
        var xValue = (value == null) ? 0 : value;
        if (this.EntityPM.OpenAmount != xValue) {
            this.EntityPM.OpenAmount = AppTool.Round(xValue, 2);

            var expect: number = this.ExpectedAmount == null ? 0 : this.ExpectedAmount;
            var amount: number = this.ForiegnCurrencyAmount == null ? 0 : this.ForiegnCurrencyAmount;
            var others: number = this.OtherInvoicesAmounts == null ? 0 : this.OtherInvoicesAmounts;
            var corre: number = expect - others - amount - xValue;
            this.CorrectionAmount = corre;
            this.fatherComponent.ComputeTotals();
        }
    }

    get CorrectionAmount() { return this.EntityPM.CorrectionAmount; }
    set CorrectionAmount(value: number) {
        if (this.EntityPM.CorrectionAmount != value) {
            this.EntityPM.CorrectionAmount = AppTool.Round(value, 2);
            this.CorrectionByUserId = SessionLocator.LoggedUserId;
            this.CorrectionDate = DateTool.GetCurrentDateAsUtc();
        }
    }

    get CorrectionByUserId() { return this.EntityPM.CorrectionByUserId; }
    set CorrectionByUserId(value: string) {
        if (this.EntityPM.CorrectionByUserId != value) {
            this.EntityPM.CorrectionByUserId = value;
            this.GetCorrectionUserName();
        }
    }

    private correctionByUserName: string = null;
    get CorrectionByUserName() { return this.correctionByUserName; }
    set CorrectionByUserName(value: string) {
        if (this.correctionByUserName != value) {
            this.correctionByUserName = value;
        }
    }

    get CorrectionDate() { return this.EntityPM.CorrectionDate; }
    set CorrectionDate(value: Date) {
        if (this.EntityPM.CorrectionDate != value) {
            this.EntityPM.CorrectionDate = value;
        }
    }

    get CorrectionNote() { return this.EntityPM.CorrectionNote; }
    set CorrectionNote(value: string) {
        if (this.EntityPM.CorrectionNote != value) {
            this.EntityPM.CorrectionNote = value;
        }
    }

    ComputeOpenAmount() {
        if (this.EntityPM.AmountTypeCode == "NEXP") {
            this.EntityPM.OpenAmount = null;
        }

        else {
            var expect: number = this.ExpectedAmount;
            var amount: number = this.ForiegnCurrencyAmount == null ? 0 : this.ForiegnCurrencyAmount;
            var others: number = this.OtherInvoicesAmounts == null ? 0 : this.OtherInvoicesAmounts;
            var corre: number = this.CorrectionAmount == null ? 0 : this.CorrectionAmount;
            var open: number = expect - others - amount - corre;

            this.EntityPM.OpenAmount = AppTool.Round(open, 2);
        }
    }
    ComputeOtherAmounts() {
        if (AppTool.IsNullOrEmpty(this.ForiegnCurrencyAmount)) {
            this.LocalCurrencyAmount = null;
            this.ProfitCurrencyAmount = null;
            this.EntityPM.InvoiceCurrencyAmount = null;
        }

        else {
            var invoiceAmount: number = 0;
            this.LocalCurrencyAmount = this.ForiegnCurrencyAmount * this.EntityPM.ForiegnExchangeRate;

            if (this.ForiegnCurrencyId == this.ProfitCurrencyId) {
                this.ProfitCurrencyAmount = this.ForiegnCurrencyAmount;
            }

            else {
                this.ProfitCurrencyAmount = this.LocalCurrencyAmount / this.APInvoicePM.ProfitCurrencyExchangeRate;
            }

            if (this.ForiegnCurrencyId == this.InvoiceCurrencyId) {
                invoiceAmount = this.ForiegnCurrencyAmount;
            }

            else {
                invoiceAmount = this.LocalCurrencyAmount / this.APInvoicePM.InvoiceCurrencyExchangeRate;
            }

            this.EntityPM.InvoiceCurrencyAmount = AppTool.Round(invoiceAmount, 2);
        }

        this.fatherComponent.ComputeTotals();
    }
}
