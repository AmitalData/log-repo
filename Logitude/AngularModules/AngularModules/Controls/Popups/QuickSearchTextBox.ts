
import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityListService} from '../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {PartnersDomainService} from '../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ShipmentDomainService} from '../../Shipment/Services/ShipmentDomainService'; 
import {ObjectsLocator}  from  '../../Infrastructure/Locators/ObjectsLocator';
import { IdGeneratorPipe } from '../pipes/idgeneratorpipe';

@Component({
    moduleId: module.id,
    templateUrl: './QuickSearchTextBox.html',
    selector: "QuickSearchTextBox",
    inputs: ['Watermark', 'ObjectTableName', 'Filters', 'DropDownWidth', 'ItemHeight', 'Area', 'IsDisabled', 'AWBMessagesCCSTypeCode', 'ShowViewAll', 'DisplayText', 'IsIconsVisible', 'IsItemSelected'],
})

export class QuickSearchTextBox implements OnInit {
    public Area: string = null;
    public Watermark: string = null;
    public ObjectTableName: string = null;
    public Filters: ApiQueryFilters = null;
    public ItemHeight: number = 25;
    public IsDeleteIconVisible: boolean = false;
    public DropDownHeight: number = 75;
    public DropDownWidth: number = 300;
    public IsDisabled: boolean = false;
    public AWBMessagesCCSTypeCode: string = null;
    public HideBox: boolean = false;
    public HideIcons: boolean = false;
    public IsControlFocused: boolean = false;
    public IsQuickSearch: boolean = true;
    public IsQuickSearchOpened: boolean = false;
    public IsQuickSearchLoading: boolean = false;
    public IsQuickSearchNoResult: boolean = false;
    public IsSearchIconVisible: boolean = false;
    private timerToken: any;
    private myPartnersDomainService: PartnersDomainService;
    private myShipmentDomainService: ShipmentDomainService;
    public ShowViewAll: boolean = false;
    public IsIconsVisible: boolean = true;
    public NewId: string;
    @Output() DataLoaded: EventEmitter<any[]> = new EventEmitter<any[]>();
    @Output() DataLoadedCount: EventEmitter<number> = new EventEmitter<number>();
    @Output() ViewAllClicked = new EventEmitter();
    @Output() OnTextChanged: EventEmitter<boolean> = new EventEmitter<boolean>();
    constructor(private entityListService: EntityListService) {
        this.SetControlDisplay();
    }

    ngOnInit() {
        var pipe: IdGeneratorPipe = new IdGeneratorPipe();
        this.NewId = pipe.transform(this.ObjectTableName + '_Search');
        this.SetWatermark();
        this.SetAPIFilters();        
    }

    private SetWatermark() {
        if (this.Watermark == null) {
            var myResult = "Search...";

            if (!AppTool.IsNullOrEmpty(this.ObjectTableName)) {
                var textCode = this.ObjectTableName + ".F.SearchFields";

                var translated = TextCodeTranslator.Translate(textCode);
                if (!AppTool.IsNullOrEmpty(translated)) {
                    myResult = translated;
                }
            }

            this.Watermark = myResult;
        }
    }
    private SetAPIFilters() {

        if (this.Filters == null) {
            this.Filters = new ApiQueryFilters();
        }

        this.Filters.PageIndex = 0;
        this.Filters.PageSize = 10;
        this.Filters.GetCount = true;
    }

    OnFucos() {
        this.IsControlFocused = true;
        this.SetControlDisplay();
    }
    OnLostFucos() {
        if (this.Area != "Ticket.Shipment" && this.Area != "FilingInbox" && this.Area != "FilingInboxHouses") {
            this.SearchText = null;
        }
        if (!this.IsItemSelected) {
            this.SearchText = null;
        }
        this.IsControlFocused = false;
        this.SetControlDisplay();
        
    }
    SetControlDisplay() {
        var isSearchIconVisible = false;
        var isDeleteIconVisible = false;
        var isQuickSearchOpened = false;

        if (AppTool.IsNullOrEmpty(this.SearchText)) {

            if (!this.IsControlFocused) {
                isSearchIconVisible = true;
            }

            isQuickSearchOpened = false;
        }

        else {
            isDeleteIconVisible = true;

            if (this.IsQuickSearch) {
                if (this.HideBox) {
                    isQuickSearchOpened = true;
                }

                else {
                    if (this.IsControlFocused) {
                        isQuickSearchOpened = true;
                    }

                    else {
                        isQuickSearchOpened = false;
                    }
                }
            }
        }

        this.IsSearchIconVisible = isSearchIconVisible;
        this.IsDeleteIconVisible = isDeleteIconVisible;
        this.IsQuickSearchOpened = isQuickSearchOpened;
    }
    DeleteClicked() {
        this.SearchText = null;
    }

    private displayText: string = null;
    get DisplayText() { return this.displayText; }
    set DisplayText(value: string) {
        if (this.displayText != value) {
            this.displayText = value;
            this.searchText = value;
        }
    }

    private isItemSelected: boolean = false;
    get IsItemSelected() { return this.isItemSelected; }
    set IsItemSelected(value: boolean) {
        if (this.isItemSelected != value) {
            this.isItemSelected = value;
        }
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            if (this.Area == "FilingInbox" || this.Area == "FilingInboxHouses") {
                this.IsItemSelected = false;
            }
            this.OnTextChanged.emit(false);
            this.SetControlDisplay();

            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.OnDataLoaded();
            }

            else {
                this.timerToken = setTimeout(() => this.LoadData(), 400);
            }
        }
    }

    private LoadData() {
        if (!AppTool.IsNullOrEmpty(this.Area)) {
            switch (this.Area) {
                case "CRM.Customers": {

                    this.IsQuickSearchLoading = true;
                    this.IsQuickSearchNoResult = false;

                    if (this.myPartnersDomainService == null) {
                        this.myPartnersDomainService = new PartnersDomainService();
                    }

                    this.myPartnersDomainService.GetCustomersQuickSearch(this.searchText).subscribe((myResponse: ServiceResponse) => {
                        this.OnDataLoaded(myResponse.Result);
                    });

                    break;
                }
                case "TenantManagement.Airline": {
                    this.IsQuickSearchLoading = true;
                    this.IsQuickSearchNoResult = false;

                    this.Filters.PageIndex = 0;
                    this.Filters.PageSize = 8;

                    this.Filters.addAdditionalFilter("IsAllowedInAirlinesRestriction", false, null, null, "Equals", false, false, false, "boolean");

                    if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                        this.Filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", true, true, false, "string");
                    }

                    if (this.myPartnersDomainService == null) {
                        this.myPartnersDomainService = new PartnersDomainService();
                    }

                    this.myPartnersDomainService.GetAirlinesByFiltersAndTenant(this.Filters, 0).subscribe((myResponse: ServiceResponse) => {
                        this.OnDataLoaded(myResponse.Result);
                    });

                    break;
                }
                case "FilingInbox": {
                    this.Filters = new ApiQueryFilters();
                    this.Filters.PageIndex = 0;
                    this.Filters.PageSize = 10;
                    this.Filters.GetCount = true;
                    this.IsQuickSearchLoading = true;
                    this.IsQuickSearchNoResult = false;

                    var item = this.Filters.AdditionalFilters.filter(f => f.FieldName == "SearchFields")[0];
                    if (item) {
                        var indexOfItem = this.Filters.AdditionalFilters.indexOf(item);
                        this.Filters.AdditionalFilters.splice(indexOfItem, 1);
                    }

                    if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                        this.Filters.Filter10Name = "SearchFields";
                        this.Filters.Filter10Value = this.SearchText;
                        this.Filters.Filter10Operator = "Contains";
                    }
                    if (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master") {

                        if (this.myShipmentDomainService == null) {
                            this.myShipmentDomainService = new ShipmentDomainService();
                        }

                        if (this.ObjectTableName == "Shipment" && (ObjectsLocator.GlobalSetting.LogoCode == "L.O.B")) {

                        }
                        else {
                            if (this.ObjectTableName == "Master") {
                                this.ObjectTableName = "Shipment";
                                this.Filters.addAdditionalFilter("ShipmentLevelCode", "D,C", null, null, "InList", true, true, false, "string");
                            }
                            else {
                                this.Filters.addAdditionalFilter("ShipmentLevelCode", "D,H", null, null, "InList", true, true, false, "string");
                            }
                        }


                        //this.myShipmentDomainService.GetShipmentFullTextSearch(this.Filters).subscribe((myResponse: ServiceResponse) => {
                        //    if (!myResponse.HasError) {
                        //        this.OnDataLoaded(myResponse.Result);
                        //    }
                        //});
                    }

                    var loadPromise = this.entityListService.getByFilters(this.ObjectTableName, this.Filters);
                    loadPromise.then((res: any) => {
                        res.subscribe(resp => {
                            this.OnDataLoaded(resp.Result);
                        })
                    });
                    break;
                }
                case "Ticket.Shipment": {
                    this.Filters = new ApiQueryFilters();
                    this.Filters.PageIndex = 0;
                    this.Filters.PageSize = 10;
                    this.Filters.GetCount = true;
                    this.IsQuickSearchLoading = true;
                    this.IsQuickSearchNoResult = false;

                    var item = this.Filters.AdditionalFilters.filter(f => f.FieldName == "SearchFields")[0];
                    if (item) {
                        var indexOfItem = this.Filters.AdditionalFilters.indexOf(item);
                        this.Filters.AdditionalFilters.splice(indexOfItem, 1);
                    }

                    if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                        this.Filters.Filter10Name = "SearchFields";
                        this.Filters.Filter10Value = this.SearchText;
                        this.Filters.Filter10Operator = "Contains";
                    }

                    if (this.ObjectTableName == "Shipment") {
                        if (this.myShipmentDomainService == null) {
                            this.myShipmentDomainService = new ShipmentDomainService();
                        }
                        this.myShipmentDomainService.GetShipmentFullTextSearch(this.Filters).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                this.OnDataLoaded(myResponse.Result);
                            }
                        });
                    }
                    else {
                        var loadPromise = this.entityListService.getByFilters(this.ObjectTableName, this.Filters);
                        loadPromise.then((res: any) => {
                            res.subscribe(resp => {
                                this.OnDataLoaded(resp.Result);
                            })
                        });
                    }

                    break;
                }
                default:
                    {
                        this.LoadSeachData();
                        break;
                    }
            }
        }

        else if (!AppTool.IsNullOrEmpty(this.ObjectTableName)) {
            this.LoadSeachData();
        }
            
    }
    private LoadSeachData() {
        this.IsQuickSearchLoading = true;
        this.IsQuickSearchNoResult = false;

        var item = this.Filters.AdditionalFilters.filter(f => f.FieldName == "SearchFields")[0];
        if (item) {
            var indexOfItem = this.Filters.AdditionalFilters.indexOf(item);
            this.Filters.AdditionalFilters.splice(indexOfItem, 1);
        }

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            //this.Filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
            this.Filters.Filter10Name = "SearchFields";
            this.Filters.Filter10Value = this.SearchText;
            this.Filters.Filter10Operator = "Contains";
        }

        var loadPromise = this.entityListService.getByFilters(this.ObjectTableName, this.Filters);

        loadPromise.then((res: any) => {
            res.subscribe(resp => {
                this.OnDataLoaded(resp.Result);
            })
        });
    }
    private OnDataLoaded(items: any[] = []) {
        var itemsCount = items.length;
        this.IsQuickSearchLoading = false;
        this.IsQuickSearchNoResult = itemsCount == 0 ? true : false;
        this.SetDropDownheight(itemsCount);

        if (itemsCount > 10) {
            var myItems: any[] = [];

            items.forEach(item => {
                if (myItems.length < 10) {
                    myItems.push(item);
                }
            });

            this.DataLoaded.emit(myItems);
        }

        else {
            this.DataLoaded.emit(items);
        }

        this.DataLoadedCount.emit(itemsCount);
    }
    private SetDropDownheight(itemsCount: number) {
        var myHeight = 75;

        if (itemsCount > 0) {

            if (itemsCount > 10) {
                itemsCount = 10;
            }

            var itemsHeight = ((itemsCount * this.ItemHeight) + 2);
            if (itemsHeight > myHeight) {
                myHeight = itemsHeight;
            }

            if (itemsCount > 10) {
                myHeight = myHeight + 5;
            }
        }

        if (this.ShowViewAll) {
            myHeight += 20;
        }

        this.DropDownHeight = myHeight;
    }

    OnViewAllClicked() {
        this.ViewAllClicked.emit(true);
    }
}
