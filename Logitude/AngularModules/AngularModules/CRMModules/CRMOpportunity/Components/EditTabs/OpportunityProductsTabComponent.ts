import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {CustomerProductPM} from '../../../../Common/EntityPMs/CustomerProductPM';
import {CustomerProductActualDataPM} from '../../../../Common/EntityPMs/CustomerProductActualDataPM';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {OpportunityPM} from '../../../../CRM/EntityPMs/OpportunityPM';
import {OpportunityProductPM} from '../../../../CRM/EntityPMs/OpportunityProductPM';
import {OpportunityProductLocationPM} from '../../../../CRM/EntityPMs/OpportunityProductLocationPM';
import {ProductTypeList} from '../../../../Common/EntityLists/ProductTypeList';
import {ProductTypeListService} from '../../../../Common/Services/StandardLists/ProductTypeListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CustomerProductLocationPM} from '../../../../Common/EntityPMs/CustomerProductLocationPM';
import {CustomerProductLocationActualDataPM} from '../../../../Common/EntityPMs/CustomerProductLocationActualDataPM';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {AppTool, ArrayTool, DateTool} from '../../../../Infrastructure/Tools';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CountryFlagPipe} from '../../../../Controls/Pipes/CountryFlagPipe';

@Component({
    selector: 'OpportunityProductsTabComponent', 
    moduleId: module.id,
    templateUrl: './OpportunityProductsTabComponent.html',
})

export class OpportunityProductsTabComponent extends BaseComponent {
    public ObjectTableName: string = "Opportunity";
    public DataContext: OpportunityProductsTabComponent = this;
    public EntityPM: OpportunityPM;
    public AccountsDataList: Array<CustomerProductPM> = [];
    public ActualDataList: Array<CustomerProductActualDataPM> =[];
    public ItemsSource: ObservableCollection;
    public HistoryObsList: ObservableCollection;
    public PotentialRevenueHeader = ""; public RevenueActualHeader = "";
    public LoadedData = false; public LoadedActualData = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public TEUActualVisibile: boolean = true;
    public ActualSelectedItem: any = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ItemsSource = new ObservableCollection([]);
        this.HistoryObsList = new ObservableCollection([]);
        this.AccountsDataList = [];
        this.ActualDataList = [];
        this.myDateTime = this.GetDefaultDate();
        this.IsShowActual = true;

        var currencyListService = new CurrencyListService();
        currencyListService.getAllFromCache().subscribe(result => {
            var myCurrencyCode: string = "";
            var list: CurrencyList = result.Result.filter(d => d.Id == (SessionLocator.TenantPM.ProfitCurrencyId))[0];
            if (list != null) {
                myCurrencyCode = list.Code;
            }
            this._entityResourceService.getEntityResourceByTableName("OpportunityProduct").subscribe((response: any) => {
                this.LoadedData = true;
                this.PotentialRevenueHeader = TextCodeTranslator.Translate("OpportunityProduct.F.Revenue") + " (" + myCurrencyCode + ")";
            });

            this._entityResourceService.getEntityResourceByTableName("CustomerProductActualData").subscribe((response: any) => {
                this.LoadedActualData = true;
                this.RevenueActualHeader = TextCodeTranslator.Translate("CustomerProductActualData.F.Revenue") + " (" + myCurrencyCode + ")";

            });

        });

        //this.BuildObsList();
    }

    private myDateTime;
    get MyDateTime() { return this.myDateTime; }
    set MyDateTime(value: Date) {
        if (this.myDateTime != value) {
            this.myDateTime = value;

            if (value == null) {
                this.myDateTime = this.GetDefaultDate();
            }
            this.SetShowActualTimer();
        }
    }

    private GetDefaultDate(): Date {
        var date;
        var now = DateTool.GetCurrentDateAsUtc();
        var myMonth = now.getUTCMonth();
        var myYear = now.getUTCFullYear();

        if (myMonth == 1) {
            myMonth = 12;
            myYear = myYear - 1;
        }
        else {
            myMonth = myMonth - 1;
        }
        date = new Date(myYear, myMonth, 1);
        return date;
    }

    //BuildObsList
    public BuildObsList() {
        var ObsList: ProductData [] = [];
        this.ItemsSource.Clear();
        this.CurrentSession.StartBusyIndicator("");
        var proeductTypeListService: ProductTypeListService = new ProductTypeListService();
        proeductTypeListService.getAllFromCache().subscribe((resp: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!resp.HasError) {
                var list: ProductTypeList[] = resp.Result.filter(d => !d.InActive).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });

                list.forEach(item => {
                    var productPM: OpportunityProductPM = this.EntityPM.OpportunityProducts.filter(d => d.OpportunityProductTypeCode == item.Code)[0];
                    if (productPM == null) {
                        var notesRightToLeft = false;
                        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
                            notesRightToLeft = true;
                        }
                        productPM = new OpportunityProductPM(null);
                        productPM.Tenant = SessionLocator.TenantPM.Id;
                        productPM.OpportunityId = this.EntityPM.Id;
                        productPM.OpportunityProductTypeCode = item.Code;
                        productPM.OpportunityProductTypeName = item.Name;
                        productPM.PrepaidCollectId = "B";
                        productPM.ChargeableWeight = 0;
                        productPM.TEU = 0;
                        productPM.NumberOfShipments = 0;
                        productPM.Revenue = 0;
                        productPM.NotesRightToLeft = notesRightToLeft;
                    }
                    ObsList.push(new ProductData(productPM, null, null, this));
                    if (this.IsShowActual) {
                        var actualRecord: CustomerProductActualDataPM = this.ActualDataList.filter(d => d.ProductTypeCode == item.Code)[0];
                        if (actualRecord == null) {
                            actualRecord = new CustomerProductActualDataPM(null);
                            actualRecord.CustomerId = this.EntityPM.CustomerId;
                            actualRecord.ProductTypeCode = item.Code;
                            actualRecord.Year = DateTool.GetCurrentDateAsUtc().getUTCFullYear();
                            actualRecord.Month = DateTool.GetCurrentDateAsUtc().getUTCMonth();
                            actualRecord.Tenant = SessionLocator.Tenant;
                            actualRecord.ChargeableWeight = 0;
                            actualRecord.NumberOfShipments = 0;
                            actualRecord.TEU = 0;
                            actualRecord.Revenue = 0;

                        }
                        ObsList.push(new ProductData(productPM, null, actualRecord, this));
                    }

                    if (this.IsShowAccount) {
                        var accountRecord: CustomerProductPM = this.AccountsDataList.filter(d => d.ProductTypeCode == item.Code)[0];
                        if (accountRecord == null) {
                            accountRecord = new CustomerProductPM(null);
                            accountRecord.CustomerId = this.EntityPM.CustomerId;
                            accountRecord.ProductTypeCode = item.Code;
                            accountRecord.Tenant = SessionLocator.Tenant;
                            accountRecord.PotentialTEU = 0;
                            accountRecord.PotentialRevenue = 0;
                            accountRecord.PotentialChargeableWeight = 0;
                            accountRecord.PotentialNumberOfShipments = 0;

                        }
                        ObsList.push(new ProductData(productPM, accountRecord, null, this));
                    }
                });
                this.ItemsSource.InsertCollection(ObsList);
                if (this.ProductsSelectedItem == null) {
                    this.ProductsSelectedItem = this.ItemsSource.Collection[0];
                }
            }
        });
    }

    //Show Actual
    private isShowActual;
    get IsShowActual() { return this.isShowActual; }
    set IsShowActual(value: boolean) {
        if (this.isShowActual != value) {
            this.isShowActual = value;
            this.SetShowActualTimer();
        }
    }

    SetShowActualTimer() {
        if (this.IsShowActual) {
            this.LoadActuals();
        }
        else {
            this.BuildObsList();
        }      
    }

    LoadActuals() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerId) && this.IsShowActual) {
            var partnersdomainService: PartnersDomainService = new PartnersDomainService();
            partnersdomainService.GetCustomerActualData(this.EntityPM.CustomerId, this.MyDateTime.getUTCFullYear(), this.MyDateTime.getUTCMonth()).subscribe((response:any) => {
                this.ActualDataList = response;
                if (this.ActualDataList == null)
                    this.ActualDataList = [];
                this.BuildObsList();
            });
        }
    }

    //Show Account
    private isShowAccount: boolean;
    get IsShowAccount() { return this.isShowAccount; }
    set IsShowAccount(value: boolean) {
        if (this.isShowAccount != value) {
            this.isShowAccount = value;
            this.SetShowAccountTimer();
        }
    }

    SetShowAccountTimer() {
        if (this.IsShowAccount) {
            this.LoadAccounts();
        }

        else {
            this.BuildObsList();
        }
    }

    LoadAccounts() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
            var partnersdomainService: PartnersDomainService = new PartnersDomainService();
            partnersdomainService.GetCustomerProducts(this.EntityPM.CustomerId).subscribe((response: any) => {
                if (!response.HasError) {
                    this.AccountsDataList = response;
                    if (this.AccountsDataList == null)
                        this.AccountsDataList = [];
                    this.BuildObsList();
                }
            });
        }
    }

    // History 
    get HistoryTitle() {
        var myResult = "Actual data history";
        if (!AppTool.IsNullOrEmpty(this.SelectedProductTypeName)) {
            myResult = this.SelectedProductTypeName + " actual data history";
        }
        return myResult;
    }

    private productsSelectedItem: ProductData;
    get ProductsSelectedItem() { return this.productsSelectedItem; }
    set ProductsSelectedItem(value: ProductData) {
        if (this.productsSelectedItem != value) {
            this.productsSelectedItem = value;
            this.SelectedProductTypeCode = value == null ? null : value.ProductCode;
            this.SelectedProductTypeName = value == null ? null : value.ProductName;
            if (value != null) {
                if (value.TransportModeId == "A") {
                    this.TEUActualVisibile = false;
                }
                else {
                    this.TEUActualVisibile = true;
                }
            }
        }
    }

    private selectedProductTypeCode: string;
    get SelectedProductTypeCode() { return this.selectedProductTypeCode; }
    set SelectedProductTypeCode(value: string) {
        if (this.selectedProductTypeCode != value) {
            this.selectedProductTypeCode = value;
            this.LoadHistory();
        }
    }

    private selectedProductTypeName;
    get SelectedProductTypeName() { return this.selectedProductTypeName; }
    set SelectedProductTypeName(value: string) {
        if (this.selectedProductTypeName != value) {
            this.selectedProductTypeName = value;

        }
    }

    LoadHistory() {
        this.HistoryObsList.Clear();
        var list = [];
        if (!AppTool.IsNullOrEmpty(this.SelectedProductTypeCode)) {
            var partnersdomainService: PartnersDomainService = new PartnersDomainService();
            partnersdomainService.GetCustomerProductHistoryActualData(this.EntityPM.CustomerId, this.SelectedProductTypeCode).subscribe(result => {
                result.Result.sort((a, b) => { return ((a.Year === b.Year) ? ((a.Month === b.Month) ? 0 : (a.Month < b.Month) ? -1 : 1) : (a.Year < b.Year ? -1 : 1)) }).reverse().forEach(item => {
                    list.push(new ProductHistoryArgs(item));
                });
                this.HistoryObsList.InsertCollection(list);
            });
        }
    }
   
    rowChanged(event) {
        this.ProductsSelectedItem = event;
    }
    actualRowSelected(event) {
        this.ActualSelectedItem = event;
    }
}
export class ProductData extends BaseComponent {
    public DataContext: ProductData = this;
    public ObjectTableName = "";
    public trigger: OpportunityProductsTabComponent;
    public isActual: boolean;
    private isAccount: boolean;
    public entityPM: OpportunityProductPM;
    private accountEntityPM: CustomerProductPM;
    public actualEntityPM: CustomerProductActualDataPM;
    private targetEntityName: string;
    private _ProductTypeList: Array<ProductTypeList> = [];
    public ProductLocations: Array<ProductLocation> = [];
    public CellProductLocations: Array<ProductLocation> = [];
    public CountriesToggleObsList: Array<CountryListViewModel> = [];
    public ProductCode = "";
    public ProductName = "";
    public ItemsSource: ObservableCollection;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(entity: OpportunityProductPM, account: CustomerProductPM = null, actual: CustomerProductActualDataPM = null, trigger: OpportunityProductsTabComponent) {
        super();

        this.entityPM = entity;
        if (this.entityPM == null) {
            this.entityPM = new OpportunityProductPM(null);
        }
        this.ItemsSource = new ObservableCollection([]);
        this.ObjectTableName = "OpportunityProduct";
        this.trigger = trigger;
        this.ProductCode = entity.OpportunityProductTypeCode;
        this.ProductName = entity.OpportunityProductTypeName;
        if (account != null) {
            this.accountEntityPM = account;
            this.isAccount = true;
        }

        if (actual != null) {
            this.actualEntityPM = actual;
            this.isActual = true;
        }

        this.IsChecked = trigger.EntityPM.OpportunityProducts.indexOf(this.entityPM) != -1 ? true : false;
        this.InitializeComponent();
        this.GetEstimatedVisibility();
        this.InitializeRightToLeft();
        this.SetProductTypeName();
    }

    private productTypeName: string;
    public get ProductTypeName() { return this.productTypeName; }
    public set ProductTypeName(value: string) {
        if (this.productTypeName != value) {
            this.productTypeName = value;
        }
    }

    SetProductTypeName() {
        var myResult: string = "";

        if (this.isAccount) {
            myResult = "Potential data";
        }

        else if (this.isActual) {
            var s = "";
            var date = DateTool.GetCurrentDateAsUtc();
            var myMonth = date.getUTCMonth();
            var myYear = date.getUTCFullYear();

            if (myMonth == 1) {
                myMonth = 12;
                myYear = myYear - 1;
            }

            else {
                myMonth = myMonth - 1;
            }

            var date = new Date(myYear, myMonth, 1);
            s = DateTool.GetDateFormats(date).MonthNameShort;
            s += "-" + myYear;
            myResult = "Actual data" + "  (" + s + ")";
        }

        else {
            var proeductTypeListService: ProductTypeListService = new ProductTypeListService();
            proeductTypeListService.getSingleFromCache(this.entityPM.OpportunityProductTypeCode).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        myResult = list.Name;
                    }
                }
            });
        }

        this.ProductTypeName = myResult;
    }

    private InitializeRightToLeft() {
        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            if (this.entityPM != null) {
                this.entityPM.NotesRightToLeft = true;
            }
        }
    }
    private InitializeComponent() {
        this.ProductLocations = [];
        this.CellProductLocations = [];
        this.CountriesToggleObsList = [];

        this.BuildProductLocations();

        if (this.TransportModeId == "A") {
            if (this.isActual) {
                this.UIProperties.SetVisibility("TEU", "CustomerProductActualData", false);
            }

            else if (this.isAccount) {
                this.UIProperties.SetVisibility("PotentialTEU", "CustomerProduct", false);
            }

            else {
                this.UIProperties.SetVisibility("TEU", "OpportunityProduct", false);
            }
        }
    }

    private ComputeOpportunityTotals() {
        var sum = 0;
        this.trigger.EntityPM.OpportunityProducts.forEach(item => {
            sum += item.NumberOfShipments;
        });
        this.trigger.EntityPM.NumberOfShipments = sum;

        var field1 = this.trigger.EntityPM.Probability == null ? 0 : this.trigger.EntityPM.Probability;
        var field2 = this.trigger.EntityPM.NumberOfShipments == null ? 0 : this.trigger.EntityPM.NumberOfShipments;
        var myValue = field1 * field2 / 100;
        this.trigger.EntityPM.ValueField = myValue;
    }

    //BuildProductLocations
    public BuildProductLocations() {
        this.ProductLocations = [];
        this.ItemsSource.Clear();

        if (this.isActual && this.actualEntityPM.ProductLocations != null && this.actualEntityPM.ProductLocations.length > 0) {
            this.actualEntityPM.ProductLocations.sort((a, b) => { return (a.CountryName === b.CountryName) ? 0 : (a.CountryName < b.CountryName) ? -1 : 1 }).forEach((item: CustomerProductLocationActualDataPM) => {
                this.ProductLocations.push(new ProductLocation(null, null, item, this, false));
            });
        }

        else if (this.isAccount && this.accountEntityPM.ProductLocations != null && this.accountEntityPM.ProductLocations.length > 0) {
            this.accountEntityPM.ProductLocations.sort((a, b) => { return (a.CountryName === b.CountryName) ? 0 : (a.CountryName < b.CountryName) ? -1 : 1 }).forEach((item: CustomerProductLocationPM) => {
                this.ProductLocations.push(new ProductLocation(null, item, null, this, false));
            });
        }

        else {
            if (this.entityPM.OpportunityProductLocations != null && this.entityPM.OpportunityProductLocations.length > 0) {
                this.entityPM.OpportunityProductLocations.sort((a, b) => { return (a.LocationName === b.LocationName) ? 0 : (a.LocationName < b.LocationName) ? -1 : 1 }).forEach((item: OpportunityProductLocationPM) => {
                    this.ProductLocations.push(new ProductLocation(item, null, null, this, false));
                });
            }
        }

        this.BuildCellProductLocations();

        var totalTEU: number = 0;
        var totalRevenue: number = 0;
        var totalChargeable: number = 0;
        var totalNumberOfShipments: number = 0;

        this.ProductLocations.forEach(s => {
            if (s.TEU != null) totalTEU += s.TEU;
            if (s.Revenue != null) totalRevenue += s.Revenue;
            if (s.ChargeableWeight != null) totalChargeable += s.ChargeableWeight;
            if (s.NumberOfShipments != null) totalNumberOfShipments += s.NumberOfShipments;
        });

        var othersItem: OpportunityProductLocationPM = new OpportunityProductLocationPM(null);
        othersItem.LocationName = "Others";
        othersItem.TEU = this.TEU - totalTEU;
        othersItem.Revenue = this.Revenue - totalRevenue;
        othersItem.ChargeableWeight = this.ChargeableWeight - totalChargeable;
        othersItem.NumberOfShipments = this.NumberOfShipments - totalNumberOfShipments;
        this.ProductLocations.push(new ProductLocation(othersItem, null, null, this, true));
        this.ItemsSource.AppendCollection(this.ProductLocations);
    }
    private BuildCellProductLocations() {
        this.CellProductLocations = [];
        this.ProductLocations.filter(f => f.IsOthers == false).sort((a, b) => { return (a.NumberOfShipments === b.NumberOfShipments) ? 0 : (a.NumberOfShipments < b.NumberOfShipments) ? -1 : 1 }).reverse().forEach(item => {
            if (this.CellProductLocations.length < 10) {
                this.CellProductLocations.push(item);
            }
            else {
                return;
            }
        });
    }

    public SetField(fieldName: string, isTotals: boolean) {
        switch (fieldName) {
            case "NumberOfShipments":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(d => {
                            if (d.NumberOfShipments != null) sum += d.NumberOfShipments;
                        });
                        this.NumberOfShipments = sum;
                        this.ComputeOpportunityTotals();
                    }

                    else {
                        var othersRecord = this.ProductLocations.filter(d => d.IsOthers)[0];
                        if (othersRecord != null) {
                            var sum = 0;
                            this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => {
                                if (d.NumberOfShipments != null) sum += d.NumberOfShipments;
                            });
                            if (this.entityPM.NumberOfShipments != null && this.entityPM.NumberOfShipments != 0) {
                                othersRecord.SetField(fieldName, this.entityPM.NumberOfShipments - sum);
                            }
                        }
                    }

                    this.BuildCellProductLocations();
                    break;
                }

            case "ChargeableWeight":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(d => {
                            if (d.ChargeableWeight != null) sum += d.ChargeableWeight;
                        });
                        //this.entityPM.ChargeableWeight = sum;
                        this.ChargeableWeight = sum;
                    }

                    else {
                        var othersRecord = this.ProductLocations.filter(d => d.IsOthers)[0];
                        if (othersRecord != null) {
                            var sum = 0;
                            this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => {
                                if (d.ChargeableWeight != null) sum += d.ChargeableWeight;
                            });
                            if (this.entityPM.NumberOfShipments != null && this.entityPM.ChargeableWeight != 0) {
                                othersRecord.SetField(fieldName, this.entityPM.ChargeableWeight - sum);
                            }
                        }
                    }
                    break;
                }

            case "Revenue":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(d => {
                            if (d.Revenue != null) sum += d.Revenue;
                        });
                        this.Revenue = sum;
                        //this.entityPM.Revenue = sum;
                    }

                    else {
                        var othersRecord = this.ProductLocations.filter(d => d.IsOthers)[0];
                        if (othersRecord != null) {
                            var sum = 0;
                            this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => { if (d.Revenue != null) sum += d.Revenue; });
                            if (this.entityPM.NumberOfShipments != null && this.entityPM.Revenue != 0) {
                                othersRecord.SetField(fieldName, this.entityPM.Revenue - sum);
                            }
                        }
                    }

                    break;
                }

            case "TEU":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(d => { if (d.TEU != null) sum += d.TEU; });
                        //this.entityPM.TEU = sum;
                        this.TEU = sum;
                    }

                    else {
                        var othersRecord = this.ProductLocations.filter(d => d.IsOthers)[0];
                        if (othersRecord != null) {
                            var sum = 0;
                            this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => {
                                if (d.TEU != null) sum += d.TEU;
                            });
                            if (this.entityPM.NumberOfShipments != null && this.entityPM.TEU != 0) {
                                othersRecord.SetField(fieldName, this.entityPM.TEU - sum);
                            }
                        }
                    }
                    break;
                }
        }
    }

    public OnLocationsChanged() {
        var totalTEU = 0;
        var totalRevenue = 0;
        var totalChargeable = 0;
        var totalNumberOfShipments = 0;

        this.ProductLocations.filter(d => d.IsOthers == false).forEach(item => {
            if (item.TEU != null) {
                totalTEU += item.TEU;
            }

            if (item.ChargeableWeight != null) {
                totalChargeable += item.ChargeableWeight;
            }

            if (item.NumberOfShipments != null) {
                totalNumberOfShipments += item.NumberOfShipments;
            }
        });
        var othersRecord = this.ProductLocations.filter(d => d.IsOthers)[0];
        if (othersRecord != null) {
            othersRecord.SetField("TEU", this.TEU - totalTEU);
            othersRecord.SetField("Revenue", this.Revenue - totalRevenue);
            othersRecord.SetField("ChargeableWeight", this.ChargeableWeight - totalChargeable);
            othersRecord.SetField("NumberOfShipments", this.NumberOfShipments - totalNumberOfShipments);
        }
    }

    private SetIsCheckedOnFieldsChanged() {
        var isLineHasValues = false;
        if (this.NumberOfShipments != null && this.NumberOfShipments != 0) {
            isLineHasValues = true;
        }
        else if (this.ChargeableWeight != null && this.ChargeableWeight != 0) {
            isLineHasValues = true;
        }
        else if (this.Revenue != null && this.Revenue != 0) {
            isLineHasValues = true;
        }
        else if (this.TEU != null && this.TEU != 0) {
            isLineHasValues = true;
        }
        this.IsChecked = isLineHasValues;
    }

    //Properties
    get DirectionId() {
        var result = "";
        if (this.entityPM.OpportunityProductTypeCode == "CI") {
            result =  "C";
        }
        else {
            result = this.entityPM.OpportunityProductTypeCode.substring(1);
        }

        return result; 
    }

    get TransportModeId() {
        var result = this.entityPM.OpportunityProductTypeCode.substring(0, 1);
        return result;
    }

    private isChecked = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
            if (value) {
                if (this.trigger.EntityPM.OpportunityProducts.indexOf(this.entityPM) == -1) {
                    this.trigger.EntityPM.AddOpportunityProduct(this.entityPM);
                }
            }

            else {
                if (this.trigger.EntityPM.OpportunityProducts.indexOf(this.entityPM) != -1) {
                    this.trigger.EntityPM.RemoveOpportunityProduct(this.entityPM);
                }
            }
            this.ComputeOpportunityTotals();
            this.FireRefreshEvent();
        }
    }

    get ActualVisibility() { return this.isActual ? true : false; }
    public EstimatedVisibility: boolean = false;
    GetEstimatedVisibility() {
        var result = this.isActual || this.isAccount ? false : true;
        this.EstimatedVisibility = result;
    }
    get GridViewCellBackground() {
        if (this.isActual || this.isAccount || this.trigger.EntityPM.IsClosed || this.trigger.EntityPM.IsCancelled) {
            return "#E6E7E8";
        }
        return "transparent";
    }
    get GridViewCellForeground() {
        if (this.isActual || this.isAccount || this.trigger.EntityPM.IsClosed || this.trigger.EntityPM.IsCancelled) {
            return "#6E7172";
        }
        return "#282E30";
    }
    get GridViewCellEditControlVisibility() {
        var result = true;
        if (this.isActual || this.isAccount || this.trigger.EntityPM.IsClosed || this.trigger.EntityPM.IsCancelled) {
            result = false;
        }
        return result;
    }
    get CheckBoxVisibility() {
        var result = true;
        if (this.isActual || this.isAccount) {
            result = false;
        }
        return result;
    }
    get NumberOfShipments() {
        var result = null;

        if (this.isActual) {
            result = this.actualEntityPM.NumberOfShipments;
        }

        else if (this.isAccount) {
            result = this.accountEntityPM.PotentialNumberOfShipments;
        }

        else {
            result = this.entityPM.NumberOfShipments;
        }

        if (result == 0) {
            result = null;
        }

        return result;
    }
    set NumberOfShipments(value: number) {
        if (this.entityPM.NumberOfShipments != value) {
            this.entityPM.NumberOfShipments = value;

            this.ComputeOpportunityTotals();
            var total = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(item => {
                total += item.NumberOfShipments;
            });

            var item = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("NumberOfShipments", value - total);
            }
            this.SetIsCheckedOnFieldsChanged();
        }
    }
    get ChargeableWeight() {
        var result = null;

        if (this.isActual) {
            result = this.actualEntityPM.ChargeableWeight;
        }

        else if (this.isAccount) {
            result = this.accountEntityPM.PotentialChargeableWeight;
        }

        else {
            result = this.entityPM.ChargeableWeight;
        }

        if (result == 0) {
            result = null;
        }

        return result;
    }
    set ChargeableWeight(value: number) {
        if (this.entityPM.ChargeableWeight != value) {
            this.entityPM.ChargeableWeight = value;
            var total = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(item => {
                total += item.ChargeableWeight;
            });
            var item = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("ChargeableWeight", value - total);
            }

            this.SetIsCheckedOnFieldsChanged();
        }
    }

    get Revenue() {
        var result = null;

        if (this.isActual) {
            result = this.actualEntityPM.Revenue;
        }

        else if (this.isAccount) {
            result = this.accountEntityPM.PotentialRevenue;
        }

        else {
            result = this.entityPM.Revenue;
        }

        if (result == 0) {
            result = null;
        }

        return result;
    }
    set Revenue(value: number) {
        if (this.entityPM.Revenue != value) {
            this.entityPM.Revenue = value;
            var total = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(item => {
                total += item.Revenue;
            });
           
            var item = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("Revenue", value - total);
            }

            this.SetIsCheckedOnFieldsChanged();
        }
    }

    get TEU() {
        var result = null;
        if (this.isActual) {
            result = this.actualEntityPM.TEU;
        }
        else if (this.isAccount) {
            result = this.accountEntityPM.PotentialTEU;
        }
        else {
            result = this.entityPM.TEU;
        }

        if (result == 0) {
            result = null;
        }
        return result;
    }
    set TEU(value: number) {
        if (this.entityPM.TEU != value) {
            this.entityPM.TEU = value;
            var total = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(item => {
                total += item.TEU;
            });
           
            var item = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("TEU", value - total);
            }
            this.SetIsCheckedOnFieldsChanged();
        }
    }

    get IsFieldsEnabled() {
        var myResult = true;

        if (this.trigger.EntityPM.IsClosed || this.trigger.EntityPM.IsCancelled) {
            myResult = false;
        }

        return myResult;
    }

    private FireRefreshEvent() {
        //RefreshScreenEvent refreshScreenEvent = trigger.eventAggregator.GetEvent<RefreshScreenEvent>();
        //refreshScreenEvent.Publish(new RefreshScreenEventArIsShowAccountgs("BuildProductToggle"));
    }

    get PrepaidCollectId() { return this.entityPM.PrepaidCollectId; }
    set PrepaidCollectId(value: string) {
        if (this.entityPM.PrepaidCollectId != value) {
            this.entityPM.PrepaidCollectId = value;
        }
    }

    //RightToLeft 
    get Notes() { return this.entityPM.Notes; }
    set Notes(value: string) {
        if (this.entityPM.Notes != value) {
            this.entityPM.Notes = value;
        }
    }

    get IsNotesRightToLeftEnabled() {
        var myResult = false;
        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            myResult = true;
        }
        return myResult;
    }

    get NotesRightToLeft() { return this.entityPM.NotesRightToLeft; }
    set NotesRightToLeft(value: boolean) {
        if (this.entityPM.NotesRightToLeft != value) {
            this.entityPM.NotesRightToLeft = value;
        }
    }

    public NotesFlowDirection: string = "ltr";
    private GetNotesFlowDirection() {
        var myResult = "ltr";
        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            myResult = "rtl";
            if (this.entityPM.NotesRightToLeft) {
                myResult = "rtl";
            }
            else {
                myResult = "ltr";
            }
        }
        this.NotesFlowDirection = myResult;
    }

    public NotesBackgroundAlignRight = "transparent";
    private NotesBackgroundAlignLeft = "transparent";
    private GetNotesBackgroundAlignRight() {
        if (!AppTool.IsNullOrEmpty(this.NotesFlowDirection)) {
            this.NotesBackgroundAlignRight = this.NotesFlowDirection == "rtl" ? "#FDD59D" : "transparent";
        }
    }
    private GetNotesBackgroundAlignLeft() {
        if (!AppTool.IsNullOrEmpty(this.NotesFlowDirection)) {
            this.NotesBackgroundAlignLeft = this.NotesFlowDirection == "ltr" ? "#FDD59D" : "transparent";
        }
    }

    public AlignNotesLeftClicked() {
        this.entityPM.NotesRightToLeft = false;
        this.GetNotesFlowDirection();
        this.RefreshNotesTextAlgimentVariables();
    }
    public AlignNotesRightClicked() {
        this.entityPM.NotesRightToLeft = true;
        this.GetNotesFlowDirection();
        this.RefreshNotesTextAlgimentVariables();
    }
    public RefreshNotesTextAlgimentVariables() {
        this.GetNotesBackgroundAlignLeft();
        this.GetNotesBackgroundAlignRight();
    }

    public SetEnabledFields() {
        var isEnabled = true;
        if (this.trigger.EntityPM.IsClosed || this.trigger.EntityPM.IsCancelled) {
            isEnabled = false;
        }
        this.UIProperties.SetEnabled("NumberOfShipments", "OpportunityProduct", isEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", "OpportunityProduct", isEnabled);
        this.UIProperties.SetEnabled("Revenue", "OpportunityProduct", isEnabled);
        this.UIProperties.SetEnabled("TEU", "OpportunityProduct", isEnabled);
        this.UIProperties.SetEnabled("Notes", "OpportunityProduct", isEnabled);
    }

    public EditProduct(item) {
        var control: string = "";
        var windowTitle = "Edit Product";
        var proeductTypeListService: ProductTypeListService = new ProductTypeListService();
        this._entityResourceService.getEntityResourceByTableName("OpportunityProductLocation", 0).subscribe(p => {
            proeductTypeListService.getAllFromCache().subscribe(result => {
                var list = result.Result.filter(d => d.Code == this.entityPM.OpportunityProductTypeCode)[0];
                if (list != null)
                    windowTitle += ": " + list.Name;

                control = "./CRMModules/CRMOpportunity/Components/EditTabs/EditProductComponent";
                var logWindow = new LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 600;
                logWindow.Title = windowTitle;
                logWindow.WindowArgs = item;
                logWindow.Show(control);
            });
        });
    }
}
export class CountryListViewModel extends BaseComponent {
    private entityList: CountryList;
    private entityPM: OpportunityProductPM;
    private trigger: ProductData;
    public src = null;
    public ImgId = ""; 

    constructor(item: CountryList, entityPM: OpportunityProductPM, trigger: ProductData) {
        super();
        this.entityList = item;
        this.entityPM = entityPM;
        this.trigger = trigger;
        this.isChecked = entityPM.OpportunityProductLocations.filter(d => d.CountryId == this.entityList.Id)[0] != null;
        var pipe = new CountryFlagPipe();
        this.src = pipe.transform(this.Code);
    }

    public get CountryId() { return this.entityList.Id; }
    public get Code() { return this.entityList.Code; }
    public get Name() { return this.entityList.EnglishName; }
    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;

            if (value) {
                var line = 0;
                if (this.entityPM.OpportunityProductLocations.length > 0) {
                    line = ArrayTool.Max(this.entityPM.OpportunityProductLocations, "LineNumber");
                }

                line += 1;

                var newItemPM: OpportunityProductLocationPM = new OpportunityProductLocationPM(null);
                newItemPM.Tenant = this.entityPM.Tenant;
                newItemPM.CountryId = this.entityList.Id;
                newItemPM.OpportunityId = this.entityPM.OpportunityId;
                newItemPM.OpportunityProductTypeCode = this.entityPM.OpportunityProductTypeCode;
                newItemPM.LineNumber = line;
                newItemPM.TEU = 0;
                newItemPM.Revenue = 0;
                newItemPM.ChargeableWeight = 0;
                newItemPM.NumberOfShipments = 0;
                newItemPM.LocationCode = this.Code;
                newItemPM.LocationName = this.Name;

                if (newItemPM != null) {
                    if (this.entityPM.OpportunityProductLocations.indexOf(newItemPM) == -1) {
                        this.entityPM.AddOpportunityProductLocation(newItemPM);
                    }
                }
            }

            else {
                var itemPM: OpportunityProductLocationPM = this.entityPM.OpportunityProductLocations.filter(d => d.CountryId == this.entityList.Id)[0];
                if (itemPM != null) {
                    if (this.entityPM.OpportunityProductLocations.indexOf(itemPM) != -1) {
                        this.entityPM.RemoveOpportunityProductLocation(itemPM);
                    }
                }
            }

            this.trigger.BuildProductLocations();
        }

    }
}
export class ProductHistoryArgs {
    private entityPM: CustomerProductActualDataPM;
    get TransportModeId() { return this.entityPM.ProductTypeCode.substring(0, 1); }
    public ProductLocations: ProductLocationCountryArgs[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    constructor(entity: CustomerProductActualDataPM) {
        this.entityPM = entity;
        this.GetProductLocations();
    }

    get MonthCode() { return this.entityPM.MonthCode; }
    get Year() { return this.entityPM.Year; }
    get NumberOfShipments() { return this.entityPM.NumberOfShipments; }
    get ChargeableWeight() { return this.entityPM.ChargeableWeight; }
    get Revenue() { return this.entityPM.Revenue; }
    get TEU() { return this.entityPM.TEU; }

    private GetProductLocations() {
        this.ProductLocations = [];
        var myResult: CustomerProductLocationActualDataPM[] = [];
        if (this.entityPM.ProductLocations.length > 0) {
            this.entityPM.ProductLocations.filter(p => p.Year == this.entityPM.Year && p.Month == this.entityPM.Month).sort((a, b) => { return (a.NumberOfShipments === b.NumberOfShipments) ? 0 : (a.NumberOfShipments < b.NumberOfShipments) ? -1 : 1 }).reverse().forEach(item => {
                if (myResult.length < 10) {
                    myResult.push(item);
                }
                else {
                    return;
                }
            });
        }
        myResult.forEach(item => {
            this.ProductLocations.push(new ProductLocationCountryArgs (item));
        });
        //this.ProductLocations = myResult;
    }

    get ViewDetailsIsEnabled() { return this.entityPM.ProductLocations.length > 0; }
    public ViewDetails() {
        this._entityResourceService.getEntityResourceByTableName("OpportunityProductLocation", 0).subscribe(p => {
            var windowTitle = "Locations Details";
            var logWindow = new LogitudeWindow();
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = this.entityPM;
            logWindow.Show("./CRMModules/CRMOpportunity/Components/EditTabs/ProductHistoryDetailsComponent");
        });
    }
}
export class ProductLocation extends BaseComponent {
    public DataContext: ProductLocation = this;
    public ObjectTableName = "";
    private isActual;
    private isAccount;
    public IsOthers;
    private trigger: ProductData;
    private entityPM: OpportunityProductLocationPM;
    private accountEntityPM: CustomerProductLocationPM;
    private actualEntityPM: CustomerProductLocationActualDataPM;
    public src = null;
    constructor(entityPM: OpportunityProductLocationPM = null, accountEntityPM: CustomerProductLocationPM = null, actualEntityPM: CustomerProductLocationActualDataPM = null, trigger: ProductData, isOthers: boolean) {
        super();
        if (entityPM != null) {
            this.entityPM = entityPM;
            this.ObjectTableName = "CustomerProductLocation";
        }
        if (accountEntityPM != null) {
            this.isAccount = true;
            this.accountEntityPM = accountEntityPM;
            this.ObjectTableName = "CustomerProductLocation";
        }
        if (actualEntityPM != null) {
            this.isActual = true;
            this.actualEntityPM = actualEntityPM;
            this.ObjectTableName = "CustomerProductLocationActualData";
        }
        this.trigger = trigger;
        this.IsOthers = isOthers;
        var pipe = new CountryFlagPipe();
        this.src = pipe.transform(this.CountryCode);
    }

    // Properties
    get CountryId() {
        var result = null;
        if (this.isActual) {
            result = this.actualEntityPM.CountryId;
        }
        else if (this.isAccount) {
            result = this.accountEntityPM.CountryId;
        }
        else {
            result = this.entityPM.CountryId;
        }
        return result;
    }

    get CountryCode() {
        var result = null;

        if (this.isActual) {
            result = this.actualEntityPM.CountryCode;
        }
        else if (this.isAccount) {
            result = this.accountEntityPM.CountryCode;
        }
        else {
            result = this.entityPM.LocationCode;
        }
        return result;
    }
    set CountryCode(value: string) {
        if (this.entityPM != null) {
            if (this.entityPM.LocationCode != value) {
                this.entityPM.LocationCode = value;
            }
        }
    }

    get CountryName() {
        var result = null;
        if (this.isActual) {
            result = this.actualEntityPM.CountryName;
        }
        else if (this.isAccount) {
            result = this.accountEntityPM.CountryName;
        }

        else {
            result = this.entityPM.LocationName;
        }
        return result;
    }
    set CountryName(value: string) {
        if (this.entityPM != null) {
            if (this.entityPM.LocationName != value) {
                this.entityPM.LocationName = value;
            }
        }
    }

    get NumberOfShipments() {
        var result = null;
        if (this.isActual) {
            result = this.actualEntityPM.NumberOfShipments;
        }
        else if (this.isAccount) {
            result = this.accountEntityPM.PotentialNumberOfShipments;
        }
        else {
            result = this.entityPM.NumberOfShipments;
        }
        if (result == 0) {
            result = null;
        }
        return result;
    }
    set NumberOfShipments(value: number) {
        if (this.entityPM != null) {
            if (this.entityPM.NumberOfShipments != value) {
                this.entityPM.NumberOfShipments = value;
                this.UpdateData("NumberOfShipments");
            }
        }
    }

    get ChargeableWeight() {
        var result = null;
        if (this.isActual) {
            result = this.actualEntityPM.ChargeableWeight;
        }
        else if (this.isAccount) {
            result = this.accountEntityPM.PotentialChargeableWeight;
        }
        else {
            result = this.entityPM.ChargeableWeight;
        }
        if (result == 0) {
            result = null;
        }
        return result;
    }
    set ChargeableWeight(value: number) {
        if (this.entityPM != null) {
            if (this.entityPM.ChargeableWeight != value) {
                this.entityPM.ChargeableWeight = value;
                this.UpdateData("ChargeableWeight");
            }
        }
    }

    get Revenue() {
        var result = null;
        if (this.isActual) {
            result = this.actualEntityPM.Revenue;
        }
        else if (this.isAccount) {
            result = this.accountEntityPM.PotentialRevenue;
        }
        else {
            result = this.entityPM.Revenue;
        }
        if (result == 0) {
            result = null;
        }
        return result;
    }
    set Revenue(value: number) {
        if (this.entityPM != null) {
            if (this.entityPM.Revenue != value) {
                this.entityPM.Revenue = value;
                this.UpdateData("Revenue");
            }
        }
    }

    get TEU() {
        var result = null;
        if (this.isActual) {
            result = this.actualEntityPM.TEU;
        }
        else if (this.isAccount) {
            result = this.accountEntityPM.PotentialTEU;
        }
        else {
            result = this.entityPM.TEU;
        }

        if (result == 0) {
            result = null;
        }

        return result;
    }
    set TEU(value: number) {
        if (this.entityPM != null) {
            if (this.entityPM.TEU != value) {
                this.entityPM.TEU = value;

                this.UpdateData("TEU");
            }
        }
    }

    private UpdateData(fieldName: string) {
        if (this.IsOthers) {
            this.trigger.SetField(fieldName, true);
        }
        else {
            this.trigger.SetField(fieldName, false);
        }
        //FirePropertyChanged(fieldName + "Foreground");
        //FirePropertyChanged("GridViewCellEditControlVisibility");
    }

    get GridViewCellEditControlVisibility() {
        var result = true;
        if (this.isActual || this.isAccount || this.trigger.trigger.EntityPM.IsClosed || this.trigger.trigger.EntityPM.IsCancelled) {
            result = false;
        }
        return result;
    }

    //Foregrounds
    get NumberOfShipmentsForeground() {
        return this.NumberOfShipments < 0 ? "#E53030" : "#282E30";
    }
    get ChargeableWeightForeground() {
        return this.ChargeableWeight < 0 ? "#E53030" : "#282E30";
    }
    get RevenueForeground() {
        return this.Revenue < 0 ? "#E53030" : "#282E30";
    }
    get TEUForeground() {
        return this.TEU < 0 ? "#E53030" : "#282E30";
    }

    public get ButtonsVisibility() { return this.IsOthers ? false : true; }
    public DeleteCountry() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this country?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                var item: CountryListViewModel = this.trigger.CountriesToggleObsList.filter(d => d.CountryId == this.entityPM.CountryId && d.IsChecked)[0];
                if (item != null) {
                    item.IsChecked = false;
                }
            }
        });
    }

    public SetField(fieldName: string, value: any) {
        if (this.entityPM != null) {
            switch (fieldName) {
                case "NumberOfShipments":
                    {
                        this.entityPM.NumberOfShipments = parseInt(value);
                        break;
                    }

                case "ChargeableWeight":
                    {
                        this.entityPM.ChargeableWeight = parseFloat(value);
                        break;
                    }

                case "Revenue":
                    {
                        this.entityPM.Revenue = parseFloat(value);
                        break;
                    }

                case "TEU":
                    {
                        this.entityPM.TEU = parseFloat(value);
                        break;
                    }
            }
        }
       
    }
}
export class ProductLocationCountryArgs extends BaseComponent {

    private entityPM: CustomerProductLocationActualDataPM;
    public src = null;
    public ImgId = "";

    constructor(entityPM: CustomerProductLocationActualDataPM) {
        super();
        this.entityPM = entityPM;
        var pipe = new CountryFlagPipe();
        this.src = pipe.transform(this.CountryCode);
    }

    ngOnInit() {

    }

    public get CountryCode() { return this.entityPM.CountryCode; }
    public get CountryName() { return this.entityPM.CountryName; }
}
