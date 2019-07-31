declare var window: any;
import {Component, OnInit} from '@angular/core';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ProductTypeItemClass} from './CustomerGeneralTabComponent';
import {CustomerProductActualDataPM} from '../../../../Common/EntityPMs/CustomerProductActualDataPM';
import {QueryPM} from '../../../../Infrastructure/EntityPMs/QueryPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {CustomerProductPM} from '../../../../Common/EntityPMs/CustomerProductPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {CustomerProductLocationPM} from '../../../../Common/EntityPMs/CustomerProductLocationPM';
import {CustomerProductLocationActualDataPM} from '../../../../Common/EntityPMs/CustomerProductLocationActualDataPM';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {ProductTypeListService} from '../../../../Common/Services/StandardLists/ProductTypeListService';
import {ProductTypeList} from '../../../../Common/EntityLists/ProductTypeList';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {ProductViewModelData, ProductActualViewModelData, ProductLocationViewModel, CountryListViewModel} from './CustomerCommitmentsTabComponent';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CommonDomainService} from'../../../../Common/Services/CommonDomainService'; 

@Component({
    moduleId: module.id,
    templateUrl: './CustomerProductsTabComponent.html',
})

export class CustomerProductsTabComponent extends BaseComponent implements OnInit{
    public ItemsSource: ObservableCollection;
    public ActualObsList: ObservableCollection;
    public EntityPM: CustomerPM;
    public ObsList: Array<ProductViewModelData> = [];
    //public ActualObsList: Array<ProductActualViewModelData> = [];
    public ToggleButtonList: Array<ProductTypeItemClass> = [];
    public ObjectTableName: string = "Customer";
    private selectedItem: ProductViewModelData;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public SearchProductDropButtonId: string = "SearchProductDropButtonId";
    public SearchProductsModeId: string = "SearchProductsModeId";
    public ActualSelectedItem: any = null;
    public TEUActualVisibile: boolean = true;
    private _currencyListService: CurrencyListService;
    public LoadedActualData = false;
    public LoadedData = false;
    public RevenueHeader: string;
    public RevenueActualHeader: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.ActualObsList = new ObservableCollection([]);
        this.EntityPM = entityArgs.EntityPM;
        this.InitServices();
        this.BuildProductsObsList();
        this.BuildToggleButtonList();
        this.getToolTip();
    }
    ngOnInit() {
        this._currencyListService = new CurrencyListService();
        this._currencyListService.getAllFromCache().subscribe(result => {
            var myCurrencyCode: string = "";
            var list: CurrencyList = result.Result.filter(d => d.Id == (SessionLocator.TenantPM.ProfitCurrencyId))[0];
            if (list != null) {
                myCurrencyCode = list.Code;
            }
            this._entityResourceService.getEntityResourceByTableName("CustomerProduct").subscribe((response: any) => {
                this.LoadedData = true;
                this.RevenueHeader = TextCodeTranslator.Translate("CustomerProduct.F.PotentialRevenue") + " (" + myCurrencyCode + ")";
            });

            this._entityResourceService.getEntityResourceByTableName("CustomerProductActualData").subscribe((response: any) => {
                this.LoadedActualData = true;
                this.RevenueActualHeader = TextCodeTranslator.Translate("CustomerProductActualData.F.Revenue") + " (" + myCurrencyCode + ")";

            });

        });
    }

    private partnersDomainService: PartnersDomainService;
    private commonDomainService: CommonDomainService;
    InitServices() {
        this.partnersDomainService = new PartnersDomainService();
        this.commonDomainService = new CommonDomainService();
    }
    private LoadCutomerProducts() {

        this.partnersDomainService.GetCustomerProducts(this.EntityPM.Id).subscribe((result: ServiceResponse) => {
            if (!result.HasError)
                this.BuildProductsObsList();
        });

    }
    BuildToggleButtonList() {
        this.ToggleButtonList = [];
        var proeductTypeListService: ProductTypeListService = new ProductTypeListService();
        proeductTypeListService.getAllFromCache().subscribe(result => {
            var FullProductsList = result.Result.filter(i => i.InActive == false).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });
            FullProductsList.forEach(item => {
                this.ToggleButtonList.push(new ProductTypeItemClass(item, this.EntityPM, this, FullProductsList));

            });
        });
    }
    BuildProductsObsList() {
        var selectedItem = null;
        this.ObsList = [];
        this.EntityPM.CustomerProducts.sort((a, b) => { return (a.ProductTypeCode === b.ProductTypeCode) ? 0 : (a.ProductTypeCode < b.ProductTypeCode) ? -1 : 1 }).forEach(item => {
            if (item.ProductTypeCode == "AD" || item.ProductTypeCode == "OD" || item.ProductTypeCode == "ID") {
                // continue;
            }

            else {
                this.ObsList.push(new ProductViewModelData(this.EntityPM, item, true, "CustomerProductLocation"));
            }
        });
        if (this.ObsList.length > 0)
            this.SelectedItem = this.ObsList[0];
        else
            this.SelectedItem = null;

        this.ItemsSource.Clear();
        this.ItemsSource.InsertCollection(this.ObsList);
    }

    public get SelectedItem() { return this.selectedItem; }
    public set SelectedItem(value: ProductViewModelData) {
        if (this.selectedItem != value) {
            this.selectedItem = value;
            if (value != null) {
                if (value.TransportModeId == "A") {
                    this.TEUActualVisibile = false;
                }
                else {
                    this.TEUActualVisibile = true;
                }
            }
            this.LoadActualData();
        }
    }

    rowChanged(event) {
        this.SelectedItem = event;
    }
    actualRowSelected(event) {
        this.ActualSelectedItem = event;
    }
    setToggleButtonMenuTemp() {
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    }
    setToggleButtonMenu() {
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
    ProductsToggleButtonClicked(item: ProductTypeItemClass, i) {
        if (item.IsChecked == true && !this.EntityPM.CustomerProducts.filter(d => d.ProductTypeCode == item.Code))
            this.BuildToggleButtonList();
        this.BuildProductsObsList();

    }

    public LoadActualData() {
        this.ActualObsList.Clear();
        var list = [];
        if (this.SelectedItem != null) {
            this.partnersDomainService.GetCustomerProductHistoryActualData(this.EntityPM.Id, this.SelectedItem.ProductTypeCode).subscribe(result => {
                result.Result.filter(d => d.NumberOfShipments > 0).sort((a, b) => { return ((a.Year === b.Year) ? ((a.Month === b.Month) ? 0 : (a.Month < b.Month) ? -1 : 1) : (a.Year < b.Year ? -1 : 1)) }).reverse().forEach(item => {
                    list.push(new ProductActualViewModelData(item));
                });

                this.ActualObsList.InsertCollection(list);
            });
        }
    }

    DeleteProduct(item: ProductViewModelData) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this product?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (this.EntityPM.CustomerProducts.includes(item.entityPM)) {
                    this.EntityPM.RemoveCustomerProductPM(item.entityPM);
                    this.BuildProductsObsList();
                    this.BuildToggleButtonList();
                }
            }
        });
    }
    EditProduct(item: ProductViewModelData) {
        var control: string = "";
        var windowTitle = "Edit Product";
        var proeductTypeListService: ProductTypeListService = new ProductTypeListService();
        this._entityResourceService.getEntityResourceByTableName("CustomerProductLocation", 0).subscribe(p => {
            this.Clone(item);
            proeductTypeListService.getAllFromCache().subscribe(result => {
                var list = result.Result.filter(d => d.Code == item.ProductTypeCode)[0];
                if (list != null)
                    windowTitle += ": " + list.Name;

            });
            if (item.isPotential) {
                control = "./CommonModules/CommonCustomer/Components/EditTabs/EditProductPotentialComponent";
            }
            else {
                control = "./CommonModules/CommonCustomer/Components/EditTabs/EditProductCommitmentComponent";
            }

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = item;
            logWindow.WindowClosed.subscribe(event => {
                if (event == "Cancel") {
                    this.RejectChanges();
                }
            });
            logWindow.Show(control);

        });

    }
    private myCloner: Cloner;
    private Clone(EntityPM: ProductViewModelData) {
        this.myCloner = new Cloner(EntityPM);
        this.myCloner.AddField('PotentialNumberOfShipments');
        this.myCloner.AddField('PotentialChargeable');
        this.myCloner.AddField('PotentialRevenue');
        this.myCloner.AddField('PotentialTEU');
        this.myCloner.AddField('CommitmentNumberOfShipments');
        this.myCloner.AddField('CommitmentChargeableWeight');
        this.myCloner.AddField('CommitmentRevenue');
        this.myCloner.AddField('CommitmentTEU');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(EntityPM);
        this.myCloner.AddEntity(EntityPM.entityPM);
        EntityPM.ProductLocations.forEach(p => {
            this.myCloner.AddEntity(p);
            this.myCloner.AddEntity(p.entityPM);
            this.myCloner.AddEntity(p.actualEntityPM);
        });
    }

    ViewProductActualData(Item: ProductActualViewModelData) {
        var objectTableName = "Shipment";
        var queryCode = "CustomerShipmentActualData";


        var myProductCode = null;
        if (!AppTool.IsNullOrEmpty(Item.ProductTypeCode)) {
            myProductCode = Item.ProductTypeCode;
        }

        var actualDate: Date = DateTool.GetDateParts(new Date(Item.Year, Item.Month, 1)).DateObject;
        var filterAgrs = new ApiQueryFilters();
        filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        filterAgrs.addAdditionalFilter("ProductCode", myProductCode, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("CustomerId", Item.entityPM.CustomerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ActualDataDateYearMonth", Item.Year, Item.Month, null, "Equals", true, true, false, "Date");

        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.DisplayTitle = "Customer Actual Data";
        listArgs.BackButtonTitle = "Back";
        listArgs.ShowViews = false;
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator.Tenant).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                });
        });
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    // Watch
    public WatchToolTip = "";
    private getToolTip() {
        if (this.ActivityWatch) {
            this.WatchToolTip = "Disable Activity Watch";
        }
        else {
            this.WatchToolTip = "Enable Activity Watch";
        }
    }
    get ActivityWatch() {
        return this.EntityPM.ActivityWatch;
    }
    set ActivityWatch(value: boolean) {
        this.EntityPM.ActivityWatch = value;
        this.getToolTip();
    }

    SetActivity(value: boolean) {
        this.ActivityWatch = value;
    }

    UpdateActualData() {
        this.CurrentSession.StartBusyIndicator("Updating ..");
        this.commonDomainService.GetUpdateCustomerActualData(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!response.HasError) {
                this.LoadCutomerProducts();
            }
        });
    }
}
