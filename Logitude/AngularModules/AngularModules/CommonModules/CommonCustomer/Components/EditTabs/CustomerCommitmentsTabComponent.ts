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
import {CommonDomainService} from'../../../../Common/Services/CommonDomainService'; 
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {CountryFlagPipe} from '../../../../Controls/Pipes/CountryFlagPipe';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    
    templateUrl: './CustomerCommitmentsTabComponent.html',
})

export class CustomerCommitmentsTabComponent extends BaseComponent {
    public imgNgStyle: any = null;
    public ItemsSource: ObservableCollection;
    public EntityPM: CustomerPM;
    public ObsList: Array<ProductViewModelData> = [];
   // public ActualObsList: Array<ProductActualViewModelData> = [];
    public ActualObsList: ObservableCollection;
    public ToggleButtonList: Array<ProductTypeItemClass> = [];
    public ObjectTableName: string = "Customer";
    private partnersDomainService: PartnersDomainService;
    private selectedItem: ProductViewModelData;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public SearchProductDropButtonId: string = "SearchProductDropButtonId" + SessionLocator.Index;
    public SearchProductsModeId: string = "SearchProductsModeId";
    public LoadedActualData: boolean = false;
    public LoadedData: boolean = false;
    public ActualSelectedItem: any = null;
    public TEUActualVisibile: boolean = true;
    public CommitmentRevenueHeader: string;
    public RevenueActualHeader: string;
    private _currencyListService: CurrencyListService;
    private commonDomainService: CommonDomainService;
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
        //this.ActualObsList = [];
        var list = [];
        if (this.SelectedItem != null) {
            this.partnersDomainService.GetCustomerProductHistoryActualData(this.EntityPM.Id, this.SelectedItem.ProductTypeCode).subscribe((result:any) => {
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
                if (this.EntityPM.CustomerProducts.indexOf(item.entityPM) != -1) {
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
        this._entityResourceService.getEntityResourceByTableName("CustomerProductLocation", 0).subscribe((response: any) => {
            this.Clone(item);
            proeductTypeListService.getAllFromCache().subscribe((result:any) => {
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
                    //this.RejectChanges();
                    item.BuildProductLocations();
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
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator.Tenant).subscribe((response: any) => {
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
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.ActualObsList = new ObservableCollection([]);
        this.EntityPM = entityArgs.EntityPM;
        this._currencyListService = new CurrencyListService();
        this._currencyListService.getAllFromCache().subscribe((result:any) => {
            var myCurrencyCode: string = "";
            var list: CurrencyList = result.Result.filter(d => d.Id == (SessionLocator.TenantPM.ProfitCurrencyId))[0];
            if (list != null) {
                myCurrencyCode = list.Code;
            }
            this._entityResourceService.getEntityResourceByTableName("CustomerProduct").subscribe((response: any) => {
                this.LoadedData = true;
                this.CommitmentRevenueHeader = TextCodeTranslator.Translate("CustomerProduct.F.CommitmentRevenue") + " (" + myCurrencyCode + ")";
            });

            this._entityResourceService.getEntityResourceByTableName("CustomerProductActualData").subscribe((response: any) => {
                this.LoadedActualData = true;
                this.RevenueActualHeader = TextCodeTranslator.Translate("CustomerProductActualData.F.Revenue") + " (" + myCurrencyCode + ")";

            });

        });

        this.InitServices();
        this.BuildProductsObsList();
        this.BuildToggleButtonList();
    }

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

        proeductTypeListService.getAllFromCache().subscribe((result:any) => {
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

            //if (item.ProductTypeCode == "AD" || item.ProductTypeCode == "OD" || item.ProductTypeCode == "ID") {
            //    // continue;
            //}

            //else {
            this.ObsList.push(new ProductViewModelData(this.EntityPM, item, false, "CustomerProductLocation"));
            //}
        });
        if (this.ObsList.length > 0)
            this.SelectedItem = this.ObsList[0];
        else
            this.SelectedItem = null;

        this.ItemsSource.Clear();
        this.ItemsSource.InsertCollection(this.ObsList);
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
export class ProductViewModelData extends BaseComponent {
    public ItemsSource: ObservableCollection;
    public isPotential: boolean;
    public customerPM: CustomerPM;
    public entityPM: CustomerProductPM;
    private targetEntityName: string;
    private _ProductTypeList: Array<ProductTypeList> = [];
    public ProductLocations: Array<ProductLocationViewModel> = [];
    public CellProductLocations: Array<ProductLocationViewModel> = [];
    public CountriesToggleObsList: Array<CountryListViewModel> = [];
    public DataContext: ProductViewModelData = this;

    constructor(customerPM: CustomerPM, product: CustomerProductPM, isPotential: boolean, public modelName: string) {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.entityPM = product;
        this.customerPM = customerPM;
        this.isPotential = isPotential;
        this.TargetEntityName = "CustomerProduct";
        var _productTypeListService: ProductTypeListService = new ProductTypeListService();
        _productTypeListService.getAllFromCache().subscribe((result:any) => {
            this._ProductTypeList = result.Result;
        });



        if (this.TransportModeId == "A") {
            this.UIProperties.SetVisibility("CommitmentTEU", this.TargetEntityName, false);
            this.UIProperties.SetVisibility("PotentialTEU", this.TargetEntityName, false);
        }

        if (AppTool.IsNullOrEmpty(this.entityPM.PrepaidCollectId)) {
            this.PrepaidCollectId = "B";
        }

        this.BuildProductLocations();
        this.InitializeRightToLeft();
        this.GetNotesFlowDirection();
    }

    private InitializeRightToLeft() {
        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            if (this.entityPM != null) {
                this.entityPM.NotesRightToLeft = true;
            }
        }
    }

    public get PrepaidCollectId() { return this.entityPM.PrepaidCollectId; }
    public set PrepaidCollectId(value: string) {
        if (this.entityPM.PrepaidCollectId != value) {
            // this.entityPM.PrepaidCollectId = value;
        }

    }
    BuildProductLocations() {

        this.ProductLocations = [];
        this.ItemsSource.Clear();
        if (this.entityPM.ProductLocations == null)
            this.entityPM.ProductLocations = [];
        this.entityPM.ProductLocations.sort((a, b) => { return (a.CountryName === b.CountryName) ? 0 : (a.CountryName < b.CountryName) ? -1 : 1 }).forEach((item: CustomerProductLocationPM) => {

            this.ProductLocations.push(new ProductLocationViewModel(item, this, false, this.modelName));
        });

        this.BuildCellProductLocations();

        var totalTEU: number = 0;
        var totalRevenue: number = 0;
        var totalChargeable: number = 0;
        var totalNumberOfShipments: number = 0;

        if (this.isPotential) {
            this.ProductLocations.forEach(s => { if (s.PotentialTEU != null) totalTEU += s.PotentialTEU });
            this.ProductLocations.forEach(s => { if (s.PotentialRevenue != null) totalRevenue += s.PotentialRevenue });
            this.ProductLocations.forEach(s => { if (s.PotentialChargeableWeight != null) totalChargeable += s.PotentialChargeableWeight });
            this.ProductLocations.forEach(s => { if (s.PotentialNumberOfShipments != null) totalNumberOfShipments += s.PotentialNumberOfShipments });

            var othersItem: CustomerProductLocationPM = new CustomerProductLocationPM(null);
            othersItem.CountryName = "Others";

            var potentialTEU = this.PotentialTEU;
            if (potentialTEU != null) {
                othersItem.PotentialTEU = potentialTEU - totalTEU;
            }

            var potentialRevenue = this.PotentialRevenue;
            if (potentialRevenue != null) {
                othersItem.PotentialRevenue = potentialRevenue - totalRevenue;
            }

            var potentialChargeableWeight = this.PotentialChargeableWeight;
            if (potentialChargeableWeight != null) {
                othersItem.PotentialChargeableWeight = potentialChargeableWeight - totalChargeable;
            }

            var potentialNumberOfShipments = this.PotentialNumberOfShipments;
            if (potentialNumberOfShipments != null) {
                othersItem.PotentialNumberOfShipments = potentialNumberOfShipments - totalNumberOfShipments;
            }
            this.ProductLocations.push(new ProductLocationViewModel(othersItem, this, true, this.modelName));
        }

        else {

            this.ProductLocations.forEach(s => { if (s.CommitmentTEU != null) totalTEU += s.CommitmentTEU });
            this.ProductLocations.forEach(s => { if (s.CommitmentRevenue != null) totalRevenue += s.CommitmentRevenue });
            this.ProductLocations.forEach(s => { if (s.CommitmentChargeableWeight != null) totalChargeable += s.CommitmentChargeableWeight });
            this.ProductLocations.forEach(s => { if (s.CommitmentNumberOfShipments != null) totalNumberOfShipments += s.CommitmentNumberOfShipments });

            var othersItem: CustomerProductLocationPM = new CustomerProductLocationPM(null);
            othersItem.CountryName = "Others";

            var commitmentTEU = this.CommitmentTEU;
            if (commitmentTEU != null) {
                othersItem.CommitmentTEU = commitmentTEU - totalTEU;
            }

            var commitmentRevenue = this.CommitmentRevenue;
            if (commitmentRevenue != null) {
                othersItem.CommitmentRevenue = commitmentRevenue - totalRevenue;
            }

            var commitmentChargeableWeight = this.CommitmentChargeableWeight;
            if (commitmentChargeableWeight != null) {
                othersItem.CommitmentChargeableWeight = commitmentChargeableWeight - totalChargeable;
            }

            var commitmentNumberOfShipments = this.CommitmentNumberOfShipments;
            if (commitmentNumberOfShipments != null) {
                othersItem.CommitmentNumberOfShipments = commitmentNumberOfShipments - totalNumberOfShipments;
            }
            this.ProductLocations.push(new ProductLocationViewModel(othersItem, this, true, this.modelName));
        }

        this.ItemsSource.AppendCollection(this.ProductLocations);
    }

    public SetField(fieldName: string, isTotals: boolean) {

        switch (fieldName) {
            case "CommitmentNumberOfShipments":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(d => { if (d.CommitmentNumberOfShipments != null) sum += d.CommitmentNumberOfShipments; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.CommitmentNumberOfShipments = sum;
                    }

                    else {
                        var othersRecord: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
                        if (othersRecord != null) {
                            var sum = 0;
                            this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => { if (d.CommitmentNumberOfShipments != null) sum += d.CommitmentNumberOfShipments; });

                            var numberOfShipments = this.entityPM.CommitmentNumberOfShipments;
                            if (numberOfShipments != null && numberOfShipments != 0) {
                                var result = numberOfShipments - sum;
                                if (result == 0) result = null;
                                othersRecord.SetField(fieldName, result);
                            }
                        }
                    }

                    this.BuildCellProductLocations();
                    break;
                }

            case "CommitmentChargeableWeight":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(d => { if (d.CommitmentChargeableWeight != null) sum += d.CommitmentChargeableWeight; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.CommitmentChargeableWeight = sum;
                    }

                    var othersRecord: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => { if (d.CommitmentChargeableWeight != null) sum += d.CommitmentChargeableWeight; });
                        var chargeableWeight = this.entityPM.CommitmentChargeableWeight;
                        if (chargeableWeight != null && chargeableWeight != 0) {
                            var result = chargeableWeight - sum;
                            if (result == 0) result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }

                    break;
                }

            case "CommitmentRevenue":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(d => { if (d.CommitmentRevenue != null) sum += d.CommitmentRevenue; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.CommitmentRevenue = sum;
                    }

                    var othersRecord: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => { if (d.CommitmentRevenue != null) sum += d.CommitmentRevenue; });
                        var revenue = this.entityPM.CommitmentRevenue;
                        if (revenue != null && revenue != 0) {
                            var result = revenue - sum;
                            if (result == 0) result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }
                    break;
                }

            case "CommitmentTEU":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(d => { if (d.CommitmentTEU != null) sum += d.CommitmentTEU; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.CommitmentTEU = sum;
                    }

                    var othersRecord: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => { if (d.CommitmentTEU != null) sum += d.CommitmentTEU; });
                        var tEU = this.entityPM.CommitmentTEU;
                        if (tEU != null && tEU != 0) {
                            var result = tEU - sum;
                            if (result == 0) result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }


                    break;
                }

            case "PotentialNumberOfShipments":
                {
                    if (isTotals) {

                        var sum = 0;
                        this.ProductLocations.forEach(d => { if (d.PotentialNumberOfShipments != null) sum += d.PotentialNumberOfShipments; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.PotentialNumberOfShipments = sum;

                    }

                    var othersRecord: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => { if (d.PotentialNumberOfShipments != null) sum += d.PotentialNumberOfShipments; });
                        var numberOfShipments = this.entityPM.PotentialNumberOfShipments;
                        if (numberOfShipments != null && numberOfShipments != 0) {
                            var result = numberOfShipments - sum;
                            if (result == 0) result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }

                    this.BuildCellProductLocations();
                    break;
                }

            case "PotentialChargeableWeight":
                {
                    if (isTotals) {

                        var sum = 0;
                        this.ProductLocations.forEach(d => { if (d.PotentialChargeableWeight != null) sum += d.PotentialChargeableWeight; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.PotentialChargeableWeight = sum;
                    }

                    var othersRecord: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => { if (d.PotentialChargeableWeight != null) sum += d.PotentialChargeableWeight; });
                        var chargeableWeight = this.entityPM.PotentialChargeableWeight;
                        if (chargeableWeight != null && chargeableWeight != 0) {
                            var result = chargeableWeight - sum;
                            if (result == 0) result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }
                    break;
                }

            case "PotentialRevenue":
                {
                    if (isTotals) {

                        var sum = 0;
                        this.ProductLocations.forEach(d => { if (d.PotentialRevenue != null) sum += d.PotentialRevenue; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.PotentialRevenue = sum;

                    }

                    var othersRecord: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => { if (d.PotentialRevenue != null) sum += d.PotentialRevenue; });
                        var revenue = this.entityPM.PotentialRevenue;
                        if (revenue != null && revenue != 0) {
                            var result = revenue - sum;
                            if (result == 0) result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }
                    break;
                }

            case "PotentialTEU":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(d => { if (d.PotentialTEU != null) sum += d.PotentialTEU; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.PotentialTEU = sum;
                    }

                    var othersRecord: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(d => d.IsOthers == false).forEach(d => { if (d.PotentialTEU != null) sum += d.PotentialTEU; });
                        var tEU = this.entityPM.PotentialTEU;
                        if (tEU != null && tEU != 0) {
                            var result = tEU - sum;
                            if (result == 0) result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }

                    break;
                }
        }
    }

    public OnLocationsChanged() {

        var totalTEU: number = 0;
        var totalRevenue: number = 0;
        var totalChargeable: number = 0;
        var totalNumberOfShipments: number = 0;

        if (this.isPotential) {
            this.ProductLocations.filter(d => d.IsOthers == false).forEach(s => { if (s.PotentialTEU != null) totalTEU += s.PotentialTEU; });
            this.ProductLocations.filter(d => d.IsOthers == false).forEach(s => { if (s.PotentialRevenue != null) totalRevenue += s.PotentialRevenue; });
            this.ProductLocations.filter(d => d.IsOthers == false).forEach(s => { if (s.PotentialChargeableWeight != null) totalChargeable += s.PotentialChargeableWeight; });
            this.ProductLocations.filter(d => d.IsOthers == false).forEach(s => { if (s.PotentialNumberOfShipments != null) totalNumberOfShipments += s.PotentialNumberOfShipments; });

            var othersRecord: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (othersRecord != null) {
                var potentialTEU = this.PotentialTEU;
                if (potentialTEU == null) {
                    potentialTEU = 0;
                }
                othersRecord.SetField("PotentialTEU", potentialTEU - totalTEU);

                var potentialRevenue = this.PotentialRevenue;
                if (potentialRevenue == null) {
                    potentialRevenue = 0;
                }
                othersRecord.SetField("PotentialRevenue", potentialRevenue  - totalRevenue);

                var potentialChargeableWeight = this.PotentialChargeableWeight;
                if (potentialChargeableWeight == null) {
                    potentialChargeableWeight = 0;
                }
                othersRecord.SetField("PotentialChargeableWeight", potentialChargeableWeight - totalChargeable);

                var potentialNumberOfShipments = this.PotentialNumberOfShipments;
                if (potentialNumberOfShipments == null) {
                    potentialNumberOfShipments = 0;
                }
                othersRecord.SetField("PotentialNumberOfShipments", potentialNumberOfShipments - totalNumberOfShipments);
            }
        }

        else {
            this.ProductLocations.filter(d => d.IsOthers == false).forEach(s => { if (s.CommitmentTEU != null) totalTEU += s.CommitmentTEU; });
            this.ProductLocations.filter(d => d.IsOthers == false).forEach(s => { if (s.CommitmentRevenue != null) totalRevenue += s.CommitmentRevenue; });
            this.ProductLocations.filter(d => d.IsOthers == false).forEach(s => { if (s.CommitmentChargeableWeight != null) totalChargeable += s.CommitmentChargeableWeight; });
            this.ProductLocations.filter(d => d.IsOthers == false).forEach(s => { if (s.CommitmentNumberOfShipments != null) totalNumberOfShipments += s.CommitmentNumberOfShipments; });


            var othersRecord: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (othersRecord != null) {
                var commitmentTEU = this.CommitmentTEU;
                if (commitmentTEU == null) {
                    commitmentTEU = 0;
                }
                othersRecord.SetField("CommitmentTEU", commitmentTEU - totalTEU);

                var commitmentRevenue = this.CommitmentRevenue;
                if (commitmentRevenue == null) {
                    commitmentRevenue = 0;
                }
                othersRecord.SetField("CommitmentRevenue", commitmentRevenue - totalRevenue);

                var commitmentChargeableWeight = this.CommitmentChargeableWeight;
                if (commitmentChargeableWeight == null) {
                    commitmentChargeableWeight = 0;
                }
                othersRecord.SetField("CommitmentChargeableWeight", commitmentChargeableWeight - totalChargeable);

                var commitmentNumberOfShipments = this.CommitmentNumberOfShipments;
                if (commitmentNumberOfShipments == null) {
                    commitmentNumberOfShipments = 0;
                }
                othersRecord.SetField("CommitmentNumberOfShipments",commitmentNumberOfShipments - totalNumberOfShipments);
            }
        }
    }

    //Region Properties
    public get DirectionId() {

        if (this.entityPM.ProductTypeCode == "CI") {
            return "C";
        }

        else {
            return this.entityPM.ProductTypeCode.substr(1, 1);
        }

    }

    public get TargetEntityName() { return this.targetEntityName; }
    public set TargetEntityName(value: string) { this.targetEntityName = value; }
    public get TransportModeId() { return this.entityPM.ProductTypeCode.substr(0, 1); }
    public get ProductTypeCode() { return this.entityPM.ProductTypeCode; }
    public get ProductTypeName() {
        var result: string = "";
        var list = this._ProductTypeList.filter(d => d.Code == this.entityPM.ProductTypeCode)[0];
        if (list != null) {
            result = list.Name;
        }
        return result;
    }

    public get PotentialNumberOfShipments() { return this.entityPM.PotentialNumberOfShipments == 0 ? null : this.entityPM.PotentialNumberOfShipments; }
    public set PotentialNumberOfShipments(value: number) {
        if (this.entityPM.PotentialNumberOfShipments != value) {
            this.entityPM.PotentialNumberOfShipments = value;

            var total: number = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(s => { if (s.PotentialNumberOfShipments != null) total += s.PotentialNumberOfShipments; });
            if (total == null) {
                total = 0;
            }

            var item: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("PotentialNumberOfShipments", value - total);
            }
            this.SetActivityWatch();
        }

    }
    public get PotentialChargeableWeight() { return this.entityPM.PotentialChargeableWeight == 0 ? null : this.entityPM.PotentialChargeableWeight; }
    public set PotentialChargeableWeight(value: number) {

        if (this.entityPM.PotentialChargeableWeight != value) {
            this.entityPM.PotentialChargeableWeight = value;

            var total: number = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(s => { if (s.PotentialChargeableWeight != null) total += s.PotentialChargeableWeight; });
            if (total == null) {
                total = 0;
            }

            var item: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("PotentialChargeableWeight", value - total);
            }
            this.SetActivityWatch();
        }

    }
    public get PotentialRevenue() { return this.entityPM.PotentialRevenue == 0 ? null : this.entityPM.PotentialRevenue; }
    public set PotentialRevenue(value: number) {

        if (this.entityPM.PotentialRevenue != value) {
            this.entityPM.PotentialRevenue = value;
            var total: number = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(s => { if (s.PotentialRevenue != null) total += s.PotentialRevenue; });
            if (total == null) {
                total = 0;
            }
            var item: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("PotentialRevenue", value - total);
            }
            this.SetActivityWatch();
        }
    }
    public get PotentialTEU() { return this.entityPM.PotentialTEU == 0 ? null : this.entityPM.PotentialTEU; }
    public set PotentialTEU(value: number) {

        if (this.entityPM.PotentialTEU != value) {
            this.entityPM.PotentialTEU = value;

            var total: number = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(s => {
                if (s.PotentialTEU != null) total += s.PotentialTEU;
            });
            if (total == null) {
                total = 0;
            }

            var item: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("PotentialTEU", value - total);
            }
            this.SetActivityWatch();
        }

    }

    public get CommitmentNumberOfShipments() { return this.entityPM.CommitmentNumberOfShipments == 0 ? null : this.entityPM.CommitmentNumberOfShipments; }
    public set CommitmentNumberOfShipments(value: number) {

        if (this.entityPM.CommitmentNumberOfShipments != value) {
            this.entityPM.CommitmentNumberOfShipments = value;

            var total: number = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(s => { if (s.CommitmentNumberOfShipments != null) total += s.CommitmentNumberOfShipments });
            if (total == null) {
                total = 0;
            }

            var item: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("CommitmentNumberOfShipments", value - total);
            }

            this.SetActivityWatch();
        }

    }

    public get CommitmentChargeableWeight() { return this.entityPM.CommitmentChargeableWeight == 0 ? null : this.entityPM.CommitmentChargeableWeight; }
    public set CommitmentChargeableWeight(value: number) {

        if (this.entityPM.CommitmentChargeableWeight != value) {
            this.entityPM.CommitmentChargeableWeight = value;

            var total: number = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(s => { if (s.CommitmentChargeableWeight != null) total += s.CommitmentChargeableWeight });
            if (total == null) {
                total = 0;
            }

            var item: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("CommitmentChargeableWeight", value - total);
            }

            this.SetActivityWatch();
        }

    }
    public get CommitmentRevenue() { return this.entityPM.CommitmentRevenue == 0 ? null : this.entityPM.CommitmentRevenue; }
    public set CommitmentRevenue(value: number) {
        if (this.entityPM.CommitmentRevenue != value) {
            this.entityPM.CommitmentRevenue = value;

            var total: number = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(s => { if (s.CommitmentRevenue != null) total += s.CommitmentRevenue; });
            if (total == null) {
                total = 0;
            }

            var item: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("CommitmentRevenue", value - total);
            }
            this.SetActivityWatch();
        }

    }

    public get CommitmentTEU() { return this.entityPM.CommitmentTEU == 0 ? null : this.entityPM.CommitmentTEU; }
    public set CommitmentTEU(value: number) {
        if (this.entityPM.CommitmentTEU != value) {
            this.entityPM.CommitmentTEU = value;

            var total: number = 0;
            this.ProductLocations.filter(d => !d.IsOthers).forEach(s => { if (s.CommitmentTEU != null) total += s.CommitmentTEU; });
            if (total == null) {
                total = 0;
            }

            var item: ProductLocationViewModel = this.ProductLocations.filter(d => d.IsOthers)[0];
            if (item != null) {
                item.SetField("CommitmentTEU", value - total);
            }

            this.SetActivityWatch();
        }

    }

     //RightToLeft 
    public get Notes() { return this.entityPM.Notes; }
    public set Notes(value: string) {

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

    public AlignNotesLeftClicked() {
        this.entityPM.NotesRightToLeft = false;
        this.GetNotesFlowDirection();
    }

    public AlignNotesRightClicked() {
        this.entityPM.NotesRightToLeft = true;
        this.GetNotesFlowDirection();
    }


    public get LastShipmentDate() { return this.entityPM.LastShipmentDate; }
    public get IsDeleteButtonEnabled() { return this.LastShipmentDate == null; }

    private SetActivityWatch() {

        if (this.customerPM.CustomerAdditionalServices.length > 0) {
            // this.customerPM.ActivityWatch = true;
        }

        if (this.CommitmentChargeableWeight != null
            || this.CommitmentNumberOfShipments != null
            || this.CommitmentRevenue != null
            || this.CommitmentTEU != null
            || this.PotentialChargeableWeight != null
            || this.PotentialNumberOfShipments != null
            || this.PotentialRevenue != null
            || this.PotentialTEU != null) {
            // this.customerPM.ActivityWatch = true;
        }
        else {
            //  this.customerPM.ActivityWatch = false;
        }

    }
    BuildCellProductLocations() {
        this.CellProductLocations = [];

        if (this.isPotential) {
            this.ProductLocations.filter(f => f.IsOthers == false).sort((a, b) => { return (a.PotentialNumberOfShipments === b.PotentialNumberOfShipments) ? 0 : (a.PotentialNumberOfShipments < b.PotentialNumberOfShipments) ? -1 : 1 }).reverse().forEach(item => {

                if (this.CellProductLocations.length < 10) {
                    this.CellProductLocations.push(item);
                }
                else {
                    return;
                }

            });

        }

        else {
            this.ProductLocations.filter(f => f.IsOthers == false).sort((a, b) => { return (a.CommitmentNumberOfShipments === b.CommitmentNumberOfShipments) ? 0 : (a.CommitmentNumberOfShipments < b.CommitmentNumberOfShipments) ? -1 : 1 }).reverse().forEach(item => {

                if (this.CellProductLocations.length < 10) {
                    this.CellProductLocations.push(item);
                }
                else {
                    return;
                }

            });

        }
    }

    public ResetCountriesItems() {
        if (this.ItemsSource != null) {
            var items: ProductLocationViewModel[] = this.ItemsSource.Collection;;
            items.forEach(item => {
                var location: CustomerProductLocationPM  = this.entityPM.ProductLocations.filter(d => d == item.entityPM)[0];
                if (location == null) {
                    if (this.entityPM.ProductLocations.indexOf(location) != -1) {
                        this.entityPM.RemoveCustomerProductLocationPM(location);
                    }
                }
                else {
                    item.CountryCode = location.CountryCode;
                    item.CountryName = location.CountryName;
                    item.CommitmentNumberOfShipments = location.CommitmentNumberOfShipments;
                    item.CommitmentRevenue = location.CommitmentRevenue;
                    item.CommitmentTEU = location.CommitmentTEU; 
                }
            });

            this.ItemsSource.Collection.forEach(item => {
                var list = this.entityPM.ProductLocations.filter(d => d == item.entityPM);
                if (list == null) {
                    this.entityPM.ProductLocations.push(item);
                }
            });      
        }
    }
}
export class ProductActualViewModelData {
    public entityPM: CustomerProductActualDataPM;
   // public ProductLocations: Array<CustomerProductLocationActualDataPM> = [];
    public ProductLocations: Array<ProductLocationCountryArgs> = [];
    
    public src = null;

    constructor(entityPM: CustomerProductActualDataPM) {
        this.entityPM = entityPM;
        this.SetProductLocations();
    }

    public get ProductTypeCode() { return this.entityPM.ProductTypeCode; }
    public get Year() { return this.entityPM.Year; }
    public get Month() { return this.entityPM.Month; }
    public get MonthCode() { return this.entityPM.MonthCode; }
    public get TEU() { return this.entityPM.TEU == 0 ? null : this.entityPM.TEU; }
    public get Revenue() { return this.entityPM.Revenue == 0 ? null : this.entityPM.Revenue; }
    public get ChargeableWeight() { return this.entityPM.ChargeableWeight == 0 ? null : this.entityPM.ChargeableWeight; }
    public get NumberOfShipments() { return this.entityPM.NumberOfShipments == 0 ? null : this.entityPM.NumberOfShipments; }
    public CellReadOnlyBackground = "rgba(230, 231, 232, 0.5)";
    public SetProductLocations() {
        var filterdProductLocations = this.entityPM.ProductLocations.filter(p => p.Year == this.entityPM.Year && p.Month == this.entityPM.Month);
        if (filterdProductLocations.length > 10) {
            filterdProductLocations = filterdProductLocations.sort((a, b) => { return (a.NumberOfShipments === b.NumberOfShipments) ? 0 : (a.NumberOfShipments < b.NumberOfShipments) ? -1 : 1 }).reverse().slice(filterdProductLocations.length - 11, filterdProductLocations.length - 1);
        }
        else {
            filterdProductLocations = filterdProductLocations.sort((a, b) => { return (a.NumberOfShipments === b.NumberOfShipments) ? 0 : (a.NumberOfShipments < b.NumberOfShipments) ? -1 : 1 }).reverse();
        }

        filterdProductLocations.forEach(item => {
            this.ProductLocations.push(new ProductLocationCountryArgs(item));
        });
    }
    public GridViewCellBackground = "#E6E7E8";
    public IsHover = false;
    private Zoom() {
        var ObjectTable = window.ObjectTables.filter(x => x.Name === "Shipment")[0];
        if (ObjectTable != null) {
            var myQueryPM: QueryPM = window.Queries.filter(x => x.ObjectTableId === ObjectTable.Id && x.Code == "CustomerShipmentActualData")[0];
            if (myQueryPM != null) {
            }
        }
    }
}
export class ProductLocationViewModel extends BaseComponent {
    private isActual: boolean;
    private isOthers: boolean;
    public get IsOthers() { return this.isOthers; }
    public set IsOthers(value: boolean) { this.isOthers = value; }

    public entityPM: CustomerProductLocationPM;
    public actualEntityPM: CustomerProductLocationActualDataPM;
    private trigger: ProductViewModelData;
    private targetEntityName: string;
    public get TargetEntityName() { return this.targetEntityName; }
    public set TargetEntityName(value: string) { this.targetEntityName = value; }
    public src = null;

    constructor(item: any, trigger: ProductViewModelData, isOthers: boolean, TargetEntityName) {
        super();
        if (TargetEntityName == "CustomerProductLocation") {

            this.entityPM = item;
            this.TargetEntityName = TargetEntityName;
            this.trigger = trigger;
            this.IsOthers = isOthers;
        }
        else if (TargetEntityName == "CustomerProductLocationActualData") {
            this.isActual = true;
            this.actualEntityPM = item;
            this.trigger = trigger;
            this.TargetEntityName = TargetEntityName;
            this.IsOthers = isOthers;
        }
        var pipe= new CountryFlagPipe();
        this.src = pipe.transform(this.CountryCode);
    }

    public get CountryId() { return this.isActual ? this.actualEntityPM.CountryId : this.entityPM.CountryId; }
    public get CountryCode() { return this.isActual ? this.actualEntityPM.CountryCode : this.entityPM.CountryCode; }
    public set CountryCode(value: string) {
        if (this.entityPM.CountryCode != value) {
            this.entityPM.CountryCode = value;
        }
    }

    public get CountryName() { return this.isActual ? this.actualEntityPM.CountryName : this.entityPM.CountryName; }
    public set CountryName(value: string) {
        if (this.entityPM.CountryName != value) {
            this.entityPM.CountryName = value;
        }
    }

    public get CommitmentNumberOfShipments() { return this.isActual ? this.actualEntityPM.NumberOfShipments : this.entityPM.CommitmentNumberOfShipments; }
    public set CommitmentNumberOfShipments(value: number) {
        if (this.entityPM.CommitmentNumberOfShipments != value) {
            this.entityPM.CommitmentNumberOfShipments = value;
            this.UpdateData("CommitmentNumberOfShipments");
        }
    }

    public get CommitmentChargeableWeight() { return this.isActual ? this.actualEntityPM.ChargeableWeight : this.entityPM.CommitmentChargeableWeight; }
    public set CommitmentChargeableWeight(value: number) {
        if (this.entityPM.CommitmentChargeableWeight != value) {
            this.entityPM.CommitmentChargeableWeight = value;
            this.UpdateData("CommitmentChargeableWeight");
        }
    }

    public get CommitmentRevenue() { return this.isActual ? this.actualEntityPM.Revenue : this.entityPM.CommitmentRevenue; }
    public set CommitmentRevenue(value: number) {
        if (this.entityPM.CommitmentRevenue != value) {
            this.entityPM.CommitmentRevenue = value;
            this.UpdateData("CommitmentRevenue");
        }
    }

    public get CommitmentTEU() { return this.isActual ? this.actualEntityPM.TEU : this.entityPM.CommitmentTEU; }
    public set CommitmentTEU(value: number) {
        if (this.entityPM.CommitmentTEU != value) {
            this.entityPM.CommitmentTEU = value;
            this.UpdateData("CommitmentTEU");
        }

    }

    public get PotentialNumberOfShipments() { return this.isActual ? this.actualEntityPM.NumberOfShipments : this.entityPM.PotentialNumberOfShipments; }
    public set PotentialNumberOfShipments(value: number) {
        if (this.entityPM.PotentialNumberOfShipments != value) {
            this.entityPM.PotentialNumberOfShipments = value;
            this.UpdateData("PotentialNumberOfShipments");
        }
    }

    public get PotentialChargeableWeight() { return this.isActual ? this.actualEntityPM.ChargeableWeight : this.entityPM.PotentialChargeableWeight; }
    public set PotentialChargeableWeight(value: number) {
        if (this.entityPM.PotentialChargeableWeight != value) {
            this.entityPM.PotentialChargeableWeight = value;
            this.UpdateData("PotentialChargeableWeight");
        }
    }

    public get PotentialRevenue() { return this.isActual ? this.actualEntityPM.Revenue : this.entityPM.PotentialRevenue; }
    public set PotentialRevenue(value: number) {
        if (this.entityPM.PotentialRevenue != value) {
            this.entityPM.PotentialRevenue = value;
            this.UpdateData("PotentialRevenue");
        }
    }
    public get PotentialTEU() { return this.isActual ? this.actualEntityPM.TEU : this.entityPM.PotentialTEU; }
    public set PotentialTEU(value: number) {
        if (this.entityPM.PotentialTEU != value) {
            this.entityPM.PotentialTEU = value;
            this.UpdateData("PotentialTEU");
        }
    }

    private UpdateData(fieldName: string) {
        if (this.IsOthers) {
            this.trigger.SetField(fieldName, true);
        }

        else {
            this.trigger.SetField(fieldName, false);
        }

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

        if (!this.isActual) {
            switch (fieldName) {
                case "CommitmentNumberOfShipments":
                    {
                        if (value == null || value == 0)
                            this.entityPM.CommitmentNumberOfShipments = null;
                        else
                            this.entityPM.CommitmentNumberOfShipments = parseInt(value);
                        break;
                    }

                case "CommitmentChargeableWeight":
                    {
                        if (value == null || value == 0)
                            this.entityPM.CommitmentChargeableWeight = null;
                        else
                            this.entityPM.CommitmentChargeableWeight = parseFloat(value);
                        break;
                    }

                case "CommitmentRevenue":
                    {
                        if (value == null || value == 0)
                            this.entityPM.CommitmentRevenue = null;
                        else
                            this.entityPM.CommitmentRevenue = parseFloat(value);
                        break;
                    }

                case "CommitmentTEU":
                    {
                        if (value == null || value == 0)
                            this.entityPM.CommitmentTEU = null;
                        else
                            this.entityPM.CommitmentTEU = parseFloat(value);
                        break;
                    }

                case "PotentialNumberOfShipments":
                    {
                        if (value == null || value == 0)
                            this.entityPM.PotentialNumberOfShipments = null;
                        else
                            this.entityPM.PotentialNumberOfShipments = parseInt(value);
                        break;
                    }

                case "PotentialChargeableWeight":
                    {
                        if (value == null || value == 0)
                            this.entityPM.PotentialChargeableWeight = null;
                        else
                            this.entityPM.PotentialChargeableWeight = parseFloat(value);
                        break;
                    }

                case "PotentialRevenue":
                    {
                        if (value == null || value == 0)
                            this.entityPM.PotentialRevenue = null;
                        else
                            this.entityPM.PotentialRevenue = parseFloat(value);
                        break;
                    }

                case "PotentialTEU":
                    {
                        if (value == null || value == 0)
                            this.entityPM.PotentialTEU = null;
                        else
                            this.entityPM.PotentialTEU = parseFloat(value);
                        break;
                    }
            }

        }

    }

    get CommitmentNumberOfShipmentsForeground() { return this.CommitmentNumberOfShipments < 0 ? "#E53030" : "#282E30"; }
    get CommitmentChargeableWeightForeground() { return this.CommitmentChargeableWeight < 0 ? "#E53030" : "#282E30"; }
    get CommitmentRevenueForeground() { return this.CommitmentRevenue < 0 ? "#E53030" : "#282E30"; }
    get CommitmentTEUForeground() { return this.CommitmentTEU < 0 ? "#E53030" : "#282E30"; }

    get PotentialNumberOfShipmentsForeground() { return this.PotentialNumberOfShipments < 0 ? "#E53030" : "#282E30"; }
    get PotentialChargeableWeightForeground() { return this.PotentialChargeableWeight < 0 ? "#E53030" : "#282E30"; }
    get PotentialRevenueForeground() { return this.PotentialRevenue < 0 ? "#E53030" : "#282E30"; }
    get PotentialTEUForeground() { return this.PotentialTEU < 0 ? "#E53030" : "#282E30"; }
}
export class CountryListViewModel extends BaseComponent implements OnInit{
    private entityList: CountryList;
    private entityPM: CustomerProductPM;
    private trigger: ProductViewModelData;
    public src = null;
    public ImgId = ""; 
       
    constructor(item: CountryList, entityPM: CustomerProductPM, trigger: ProductViewModelData) {
        super();
        this.entityList = item;
        this.entityPM = entityPM;
        this.trigger = trigger;
        this.isChecked = entityPM.ProductLocations.filter(d => d.CountryId == this.entityList.Id)[0] != null;
        var pipe = new CountryFlagPipe();
        this.src = pipe.transform(this.Code);
    }

    ngOnInit() {
        
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
                var newItemPM: CustomerProductLocationPM = new CustomerProductLocationPM(null);

                newItemPM.Tenant = this.entityPM.Tenant;
                newItemPM.CountryId = this.entityList.Id;
                newItemPM.CustomerId = this.entityPM.CustomerId;
                newItemPM.ProductTypeCode = this.entityPM.ProductTypeCode;
                newItemPM.CommitmentTEU = 0;
                newItemPM.CommitmentRevenue = 0;
                newItemPM.CommitmentChargeableWeight = 0;
                newItemPM.CommitmentNumberOfShipments = 0;
                newItemPM.PotentialTEU = 0;
                newItemPM.PotentialRevenue = 0;
                newItemPM.PotentialChargeableWeight = 0;
                newItemPM.PotentialNumberOfShipments = 0;
                newItemPM.CountryCode = this.Code;
                newItemPM.CountryName = this.Name;

                if (newItemPM != null) {
                    if (this.entityPM.ProductLocations.indexOf(newItemPM) == -1) {
                        this.entityPM.AddCustomerProductLocationPM(newItemPM);
                    }
                }
            }

            else {
                var itemPM: CustomerProductLocationPM = this.entityPM.ProductLocations.filter(d => d.CountryId == this.entityList.Id)[0];
                if (itemPM != null) {
                    //var tempProductLocations: Array<CustomerProductLocationPM> = [];
                    //if (this.entityPM.ProductLocations.includes(itemPM)) {
                    //    this.entityPM.ProductLocations.forEach(item => {
                    //        if (item != itemPM)
                    //            tempProductLocations.push(item);
                    //    });
                    //    this.entityPM.ProductLocations = tempProductLocations;
                    //    this.trigger.entityPM.ProductLocations = this.entityPM.ProductLocations;
                    //}

                    if (this.entityPM.ProductLocations.indexOf(itemPM) != -1) {
                        this.entityPM.RemoveCustomerProductLocationPM(itemPM);
                    }       
                }
            }

            this.trigger.BuildProductLocations();
            this.trigger.OnLocationsChanged();
        }

    }

    public Isvisible = false;
}
export class ProductLocationCountryArgs extends BaseComponent{

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
