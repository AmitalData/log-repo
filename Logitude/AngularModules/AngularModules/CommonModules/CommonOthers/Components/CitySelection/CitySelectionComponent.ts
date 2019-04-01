import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {CountryCityList} from '../../../../Common/EntityLists/CountryCityList';
import {CountryCityListService} from '../../../../Common/Services/StandardLists/CountryCityListService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CitySelectionArgs} from '../../../../Common/Args';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './CitySelectionComponent.html',
})

export class CitySelectionComponent {
    public ItemsSource: CountryCityList[] = [];
    public IsNoData: boolean = false;
    public SelectedCity: CountryCityList = null;
    private args: CitySelectionArgs;
    private myService: CountryCityListService;
    private ObjectTableName: string = "CountryCity";
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.myService = new CountryCityListService();
        this.ItemsSource = new Array<CountryCityList>();
    }

    SetWindowArgs(args: CitySelectionArgs) {
        this.args = args;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;
            this.LoadData();
        });
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            this.LoadData();
        }
    }

    private LoadData() {
        this.IsNoData = false;
        this.ItemsSource = [];

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 50;
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Descending";

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
        }

        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, true, false, "Boolean");

        if (!AppTool.IsNullOrEmpty(this.args.CountryId)) {
            filters.addAdditionalFilter("CountryId", this.args.CountryId, null, null, "Equals", false, true, false, "Boolean");
        }

        this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse == null) {
                this.IsNoData = true;
                this.ItemsSource = [];
            }

            else {
                this.ItemsSource = myResponse.Result;
                if (this.ItemsSource.length == 0) {
                    this.IsNoData = true;
                }
            }
        });
    }

    Selecting(item: CountryCityList) {
        this.SelectedCity = item;

        if (item == null) {
            this.args.CityName = null;
            this.args.CityLocalName = null;
            this.args.CountryId = null;
            this.args.StateId = null;
            this.args.IsCitySelected = false;
        }

        else {
            this.args.CityName = item.EnglishName;
            this.args.CityLocalName = item.LocalName;
            this.args.CountryId = item.CountryId;
            this.args.StateId = item.StateId;
            this.args.IsCitySelected = true;
        }

        this.Close();
    }

    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
