import { Component } from '@angular/core';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { HarmonizeCodeList } from '../../../../../Shipment/EntityLists/HarmonizeCodeList';
import { HarmonizeCodeListService } from '../../../../../Shipment/Services/StandardLists/HarmonizeCodeListService';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,

    templateUrl: './HarmonizesComponent.html',
})

export class HarmonizesComponent {
    public Entity: any;
    public FieldName: string;
    public ItemsSource: HarmonizeCodeList[] = [];
    public HarmonizesCount: number = 0;
    private myService: HarmonizeCodeListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myService = new HarmonizeCodeListService();
        this.LoadAllData();
    }

    SetWindowArgs(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
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

            this.HarmonizesCount = this.ItemsSource == null ? 0 : this.ItemsSource.length;
        });
    }

    Selecting(item: HarmonizeCodeList) {
        this.SetField(item);

        if (item == null) {
            this.SetField(null);
        }

        else {
            this.SetField(item);
        }

        this.Close();
    }


    SetField(item: HarmonizeCodeList) {
        var iCode: string = null;

        if (item) {
            iCode = item.Code;
        }

        if (this.Entity[this.FieldName] != iCode) {
            this.Entity[this.FieldName] = iCode;
        }
    }

    CloseButtonClicked() {
        this.Close();
    }

    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
