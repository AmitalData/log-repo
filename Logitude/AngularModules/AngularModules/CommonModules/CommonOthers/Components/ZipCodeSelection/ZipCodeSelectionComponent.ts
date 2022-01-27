import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import { PostalCodeListService } from '../../../../Common/Services/StandardLists/PostalCodeListService';
import { PostalCodeList } from '../../../../Common/EntityLists/PostalCodeList';

@Component({
    
    templateUrl: './ZipCodeSelectionComponent.html',
})

export class ZipCodeSelectionComponent {
    public ItemsSource: PostalCodeList[] = [];
    public IsNoData: boolean = false;
    private myService: PostalCodeListService;
    private ObjectTableName: string = "PostalCode";
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.myService = new PostalCodeListService();
        this.ItemsSource = new Array<PostalCodeList>();
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;
            this.LoadData();
        });
    }

    SetWindowArgs(args: any) {
        
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
        filters.SortBy = "Code";
        filters.SortDirection = "Ascending";

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
        }

        this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            this.FillItemsSource(myResponse);
        });
    }

    private FillItemsSource(myResponse: ServiceResponse) {
        if (myResponse == null) {
            this.ItemsSource = [];
        }
        else {
            this.ItemsSource = myResponse.Result;
        }

        this.IsNoData = this.ItemsSource.length == 0;
    }

    Selecting(item: any) {
        if (item)
            this.CurrentSession.CloseCurrentWindowEmit(item.Code);
    }

    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
