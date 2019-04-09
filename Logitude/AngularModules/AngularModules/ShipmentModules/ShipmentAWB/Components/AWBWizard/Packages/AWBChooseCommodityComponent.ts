import {Component} from '@angular/core';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {CommodityList} from '../../../../../Common/EntityLists/CommodityList';
import {CommodityListService} from '../../../../../Common/Services/StandardLists/CommodityListService';
import {CommonDomainService} from '../../../../../Common/Services/CommonDomainService';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {EntityPMService} from '../../../../../Infrastructure/Services/EntityPMService';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,

    templateUrl: './AWBChooseCommodityComponent.html',
})

export class AWBChooseCommodityComponent {
    public EntityPM: any;
    public FieldName: string;
    public NameProperty: string;
    public ItemsSource: CommodityList[] = [];
    public ItemsSource_TenantZero: CommodityList[] = [];
    public MyCommoditiesCount: number = 0;
    public AllCommoditiesCount: number = 0;
    private myService: CommodityListService;
    private DomainService: CommonDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myService = new CommodityListService();
        this.LoadAllData();
    }

    SetWindowArgs(args : any) {
        this.EntityPM = args['EntityPM'];
        this.FieldName = args['FieldName'];
        this.NameProperty = args['NameProperty'];
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            this.LoadAllData();
        }
    }

    private LoadAllData() {
        this.LoadTenantData();
        this.LoadTenantZeroData();
    }

    private LoadTenantData() {

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.SortBy = "Code";
        filters.SortDirection = "Descending";

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
        }

        this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse == null) {
                this.ItemsSource = [];
            }

            else {
                if (!myResponse.HasError) {
                    this.ItemsSource = myResponse.Result;
                }
            }

            this.MyCommoditiesCount = this.ItemsSource == null ? 0 : this.ItemsSource.length;
        });
    }
    private LoadTenantZeroData() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.SortBy = "Code";
        filters.SortDirection = "Descending";
        filters.Tenant = 0;

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {            
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
        }

        this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse == null) {
                this.ItemsSource_TenantZero = [];
            }

            else {
                if (!myResponse.HasError) {
                    this.ItemsSource_TenantZero = myResponse.Result;
                }
            }

            this.AllCommoditiesCount = this.ItemsSource_TenantZero == null ? 0 : this.ItemsSource_TenantZero.length;
        });
    }

    Selecting(item: CommodityList) {
        this.SetField(item);

        if (item == null) {
            this.SetField(null);
        }

        else {
            this.SetField(item);
        }

        this.Close();
    }

    SelectingTenantZero(item: CommodityList) {
        this.SetField(item);

        if (item == null) {
            this.Close();
        }

        else {
            this.CopyToMyTenant(item.Id);
        }
    }

    SetField(item: CommodityList) {
        var myCommodityCode: string = null;
        var myCommodityName: string = null;

        if (item) {
            myCommodityCode = item.Code;
            myCommodityName = item.Name;
        }

        if (this.EntityPM[this.FieldName] != myCommodityCode) {
            this.EntityPM[this.FieldName] = myCommodityCode;
        }

        if (this.EntityPM[this.NameProperty] != myCommodityName) {
            this.EntityPM[this.NameProperty] = myCommodityName;
        }
    }

    private CopyToMyTenant(tenantZeroId: string) {
        if (this.DomainService == null) {
            this.DomainService = new CommonDomainService();
        }

        this.DomainService.GetCopyCommodityToTenant(tenantZeroId).subscribe(myResult => {
            this.Close();
        });
    }

    CloseButtonClicked() {
        this.Close();
    }

    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }

    AddCommodityClicked() {
        var myService: EntityPMService = new EntityPMService();
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";

        myService.getNewEntity("Commodity").then(response => {
            var args = new EntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = "Commodity";

            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate("Commodity"));

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;            
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
            logWindow.Show(componentPath);
        });
    }

    private OnNewEntityWindowClosed($event: any) {
        console.log($event);
        if ($event && $event != "event") {
            this.LoadTenantData();
        }
    }
}
