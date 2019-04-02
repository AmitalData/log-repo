import {Component} from '@angular/core';
import {ContactList} from '../../../../Common/EntityLists/ContactList';
import {ContactListService} from '../../../../Common/Services/StandardLists/ContactListService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ContactInputTemplate} from './ContactInputTemplate';

@Component({
    moduleId: module.id,
    templateUrl: './SearchContactsComponent.html',
})

export class SearchContactsComponent {
    public ItemsSource: ContactList[] = [];
    public MyContactsCount: number = 0;
    private myService: ContactListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myService = new ContactListService();
        this.LoadAllData();
    }

    private inputTemplate: ContactInputTemplate;
    SetWindowArgs(args: ContactInputTemplate) {
        this.inputTemplate = args;
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
        filters.PageSize = 500;
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");

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

            this.MyContactsCount = this.ItemsSource == null ? 0 : this.ItemsSource.length;
        });
    }

    Selecting(item: ContactList) {

        if (this.inputTemplate != null) {
            if (item != null) {
                this.inputTemplate.Email = item.Email;
                this.inputTemplate.EmailLostFocus(item.Email);
            }
        }

        this.Close();
    }

    CloseButtonClicked() {
        this.Close();
    }

    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
