import {Component, OnInit, AfterViewInit, Output, EventEmitter} from '@angular/core';
import {AppTool, FormatTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ContactList} from '../../Common/EntityLists/ContactList';
import {ContactListService} from '../../Common/Services/StandardLists/ContactListService';
import {UserList} from '../../Common/EntityLists/UserList'; 
import {UserListService} from '../../Common/Services/StandardLists/UserListService';
import {CommonDomainService} from '../../Common/Services/CommonDomainService';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    
    templateUrl: './EmailSearchTextBox.html',
    selector: "EmailSearchTextBox",
    inputs: ['Watermark', 'EmailsText', 'IsUsersList', 'IsDisabled', 'SelectedValuePath', 'ExcludedResult', 'DontInCludeInactive', 'AllowFreeEmails','isRTL'],

})

export class EmailSearchTextBox implements OnInit, AfterViewInit {
    public ContainerId: string = null;
    public ComponentId: string = null;
    public SeparatorId: string = null;
    public InputId: string = null;
    public TextAreaId: string = null;
    public ComponentDropDownId: string = null;
    public Watermark: string = null;
    private emailText: string = null;
    public get EmailsText() { return this.emailText; }
    public set EmailsText(value: string) {
        if (this.emailText != value) {
            this.emailText = value;
            this.FillEmailSearch();
        }
    }
    public IsUsersList: boolean = false;
    public AllowFreeEmails: boolean = false;
    public isRTL: boolean = false;
    public DropDownHeight: number = 200;
    public DropDownWidth: number = 300;
    public ItemsSource: EmailSearchTextBoxItem[] = [];
    public SelectedItems: any[] = [];
    public IsFocused: boolean = false;
    public ValidationErrorsList: string[];
    Placeholder: string = null;
    public SelectedValuePath: string = 'Email';
    public IsDisabled: boolean = false;
    public ExcludedResult: string[];
    public DontInCludeInactive: boolean = false;
    
    @Output() EmailsTextChanged: EventEmitter<string> = new EventEmitter<string>();
    @Output() SelectedListChanged: EventEmitter<any> = new EventEmitter<any>();
    @Output() ValidationErrorsListChanged: EventEmitter<any> = new EventEmitter<any>();

    private contactService: ContactListService;
    private userService: UserListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        
        var idIndex = this.CurrentSession.GetNewId("EmailSearchTextBox");
        this.ComponentId = "EmailSearchTextBox_" + idIndex;
        this.SeparatorId = "EmailSearchTextBox_Separator_" + idIndex;
        this.InputId = "EmailSearchTextBox_Input_" + idIndex;
        this.TextAreaId = "EmailSearchTextBox_TextArea_" + idIndex;
        this.ComponentDropDownId = "EmailSearchTextBox_DropDown_" + idIndex;
        this.contactService = new ContactListService();
        this.userService = new UserListService();
    }
    IsLoad: boolean = false;
    IsShowRedUserInActiveNote: boolean = false;
    ngOnInit() {
        this.IsLoad = true;
        this.FillEmailSearch();        
    }

    FillEmailSearch() {

        if (this.IsLoad) {
            this.Placeholder = this.Watermark;
            if (!AppTool.IsNullOrEmpty(this.EmailsText)) {
                var myDomainService = new CommonDomainService();

                if (this.SelectedValuePath == "Id") {

                    myDomainService.GetUserListsByidsString(this.EmailsText).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            this.SelectedItems = myResponse.Result;
                            this.SelectedItems.forEach(item => {
                                if (item.InActive) {
                                    this.IsShowRedUserInActiveNote = true;
                                }
                            });


                        }

                        setTimeout(() => this.SetInputPosition(), 5);
                    });

                }
                else {
                    if (this.IsUsersList) {
                        myDomainService.GetUsersByEmails(this.EmailsText).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                this.SelectedItems = myResponse.Result;
                            }

                            setTimeout(() => this.SetInputPosition(), 5);
                        });
                    }

                    else {
                        myDomainService.GetContactsByEmails(this.EmailsText).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                this.SelectedItems = myResponse.Result;
                            }

                            var allEmails: string[] = this.EmailsText.split(';');
                            if (allEmails.length > this.SelectedItems.length) {
                                allEmails.forEach((email: string) => {
                                    if (!AppTool.IsNullOrEmpty(email)) {
                                        if (this.SelectedItems.filter(f => f.Email != null && f.Email.toLowerCase() == email.toLowerCase()).length == 0) {
                                            var newItem = new ContactList();
                                            newItem.Email = email;
                                            if (this.AllowFreeEmails) newItem.EnglishName = email.split("@")[0];
                                            else newItem.EnglishName = email;
                                            if (!AppTool.IsNullOrEmpty(email) && email != "undefined" && email != "null")
                                                this.SelectedItems.push(newItem);
                                        }
                                    }
                                });
                            }

                            setTimeout(() => this.SetInputPosition(), 5);
                        });
                    }
                }
            }
            else this.SelectedItems = [];
        }
    }
    ngAfterViewInit() {
        this.SetInputPosition();
    }

    AddItem(item: any) {
        if (item) {
            var emails = this.SelectedItems.filter(f => f.Id == item.Id);
            if (emails.length == 0) {
                this.SelectedItems.push(item);
                this.SearchText = null;

                setTimeout(() => this.SetInputPosition(), 5);

                this.BuildEmailsText();

                setTimeout(() => this.SetInputFocus(), 5);
            }
        }
    }
    RemoveItem(item: any) {
        if (item) {
            if (this.SelectedItems.filter(f => f.Id == item.Id).length > 0) {

                var index = this.SelectedItems.indexOf(item);
                this.SelectedItems.splice(index, 1);

                setTimeout(() => this.SetInputPosition(), 5);

                this.BuildEmailsText();

                setTimeout(() => this.SetInputFocus(), 5);
            }
        }
    }
    SetInputFocus() {
        this.IsFocused = true;
        document.getElementById(this.TextAreaId).focus();
    }

    OnLostFocus() {
        this.SearchText = null;
        this.IsFocused = false;
        this.IsOpened = false;
        if (AppTool.IsNullOrEmpty(this.EmailsText)) {
            this.IsOpened = false;
            this.Placeholder = this.Watermark;
            this.ItemsSource = [];
        }
        else {
            this.Placeholder = "";
        }

    }
    OnKeyDown(event: KeyboardEvent) {
        switch (event.keyCode) {
            case 8: {
                // Backspace
                if (AppTool.IsNullOrEmpty(this.SearchText)) {
                    if (this.SelectedItems.length > 0) {
                        this.RemoveItem(this.SelectedItems[this.SelectedItems.length - 1]);
                    }
                }

                break;
            }

            case 9:
            case 32:
            case 186: {
                // Tab | Space | ;
                if (!AppTool.IsNullOrEmpty(this.SearchText)) {

                    var item: EmailSearchTextBoxItem = null;
                    if (this.ItemsSource.length > 0) {
                        item = this.ItemsSource.filter(f => f.Email != null && f.Email.toLowerCase() == this.SearchText.toLowerCase())[0];
                    }

                    if (item) {
                        this.AddItem(item);
                        event.preventDefault();
                    }

                    else if (FormatTool.IsEmail(this.SearchText)) {
                        if (!this.IsUsersList) {
                            var newItem = new ContactList();
                            newItem.Email = this.SearchText;
                            newItem.EnglishName = this.SearchText;
                            this.AddItem(newItem);
                            event.preventDefault();
                        }
                    }
                }
                break;
            }

            case 38: {
            // ArrowUp

                if (this.IsOpened) {
                    if (this.ItemsSource) {
                        if (this.ItemsSource.length > 0) {
                            var myCurrentSelectedItem: EmailSearchTextBoxItem = this.ItemsSource.filter(f => f.Selected == true)[0];

                            if (myCurrentSelectedItem == null) {
                                this.ItemsSource.filter(f => f.Index == 0)[0].Selected = true;
                            }

                            else {
                                var myNewSelectedIndex = myCurrentSelectedItem.Index - 1;
                                var myNewSelectedItem: EmailSearchTextBoxItem = this.ItemsSource.filter(f => f.Index == myNewSelectedIndex)[0];

                                if (myNewSelectedItem) {
                                    myCurrentSelectedItem.Selected = false;
                                    myNewSelectedItem.Selected = true;

                                    var element = document.getElementById("EmailSearchTextBoxItem_" + myNewSelectedItem.Id);
                                    if (element) {
                                        element.scrollIntoView(true);
                                    }
                                }
                            }
                        }
                    }
                }

                break;
            }

            case 40: {
                // ArrowDown

                if (this.IsOpened) {
                    if (this.ItemsSource) {
                        if (this.ItemsSource.length > 0) {
                            var myCurrentSelectedItem: EmailSearchTextBoxItem = this.ItemsSource.filter(f => f.Selected == true)[0];

                            if (myCurrentSelectedItem == null) {
                                this.ItemsSource.filter(f => f.Index == 0)[0].Selected = true;
                            }

                            else {
                                var myNewSelectedIndex = myCurrentSelectedItem.Index + 1;
                                var myNewSelectedItem: EmailSearchTextBoxItem = this.ItemsSource.filter(f => f.Index == myNewSelectedIndex)[0];

                                if (myNewSelectedItem) {
                                    myCurrentSelectedItem.Selected = false;
                                    myNewSelectedItem.Selected = true;

                                    var element = document.getElementById("EmailSearchTextBoxItem_" + myNewSelectedItem.Id);
                                    if (element) {
                                        element.scrollIntoView(false);
                                    }
                                }
                            }
                        }
                    }
                }

                break;
            }

            case 13: {
                // Enter | NumpadEnter

                if (this.IsOpened) {
                    if (this.ItemsSource) {
                        if (this.ItemsSource.length > 0) {
                            var myCurrentSelectedItem: EmailSearchTextBoxItem = this.ItemsSource.filter(f => f.Selected == true)[0];

                            if (myCurrentSelectedItem != null) {
                                this.AddItem(myCurrentSelectedItem);
                            }
                        }
                        else if (this.ItemsSource.length == 0 && this.AllowFreeEmails) {
                            var myCurrentInsertedItem: EmailSearchTextBoxItem;
                            var myCurrentInsertedItemIndex: number;
                            if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                                myCurrentInsertedItem = new EmailSearchTextBoxItem(null, 0);
                                myCurrentInsertedItemIndex = this.SelectedItems.length;
                                myCurrentInsertedItem.Email = this.SearchText;
                                myCurrentInsertedItem.EnglishName = this.SearchText.split("@")[0];
                                myCurrentInsertedItem.Index = myCurrentInsertedItemIndex;
                                myCurrentInsertedItem.Id = "1-" + myCurrentInsertedItemIndex;
                                myCurrentInsertedItem.ElementId = "EmailSearchTextBoxItem_1-" + myCurrentInsertedItemIndex.toString();
                                myCurrentInsertedItem.Selected = true;
                                this.AddItem(myCurrentInsertedItem);
                            }
                        }
                    }
                }

                break;
            }
        }
    }
    BuildEmailsText() {
        var myResult: string = null;
        var selectedList = [];
        var errors = [];
        if (this.SelectedValuePath == "Id") {
            this.SelectedItems.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.Id)) {
                    if (AppTool.IsNullOrEmpty(myResult)) {
                        myResult = item.Id;
                        selectedList.push(item);
                    }

                    else {
                        myResult += ";" + item.Id;
                        selectedList.push(item);
                    }

                    if (!FormatTool.IsEmail(item.Email)) {
                        errors.push("\"" + item.Email + "\" email address is not recognised.");
                    }
                }
            });
        }
        else {
            this.SelectedItems.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.Email)) {
                    if (AppTool.IsNullOrEmpty(myResult)) {
                        myResult = item.Email;
                        selectedList.push(item);
                    }

                    else {
                        myResult += ";" + item.Email;
                        selectedList.push(item);
                    }

                    if (!FormatTool.IsEmail(item.Email)) {
                        errors.push("\"" + item.Email + "\" email address is not recognised.");
                    }
                }
            });

        }
     
        this.EmailsText = myResult;
        this.EmailsTextChanged.emit(myResult);
        this.SelectedListChanged.emit(selectedList);
        this.ValidationErrorsListChanged.emit(errors);

    }

    // Search
    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
            this.OnSearchTextChanged();
        }
    }

    private timerTokenSearch: any;
    private OnSearchTextChanged() {

        if (AppTool.IsNullOrEmpty(this.SearchText)) {
            this.IsOpened = false;
            this.ItemsSource = [];
        }

        else {
            this.IsOpened = true;

            if (this.timerTokenSearch) {
                clearTimeout(this.timerTokenSearch);
            }

            this.timerTokenSearch = setTimeout(() => this.RunSearch(), 400);
        }
    }

    RunSearch() {
        if (this.IsUsersList) {
            this.RunUsersSearch();
        }

        else {
            this.RunContactsSearch();
        }
       
    }
    RunUsersSearch() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Ascending";
        filters.ObjectTableName = "User";

        filters.addAdditionalFilter("HasEmail", true, null, null, "Equals", true, false, false, "boolean");

        if (this.DontInCludeInactive) {
            filters.addAdditionalFilter("InActive", false, null, null, "Equals", true, false, false, "boolean");
        }

        if (!AppTool.IsNullOrEmpty(this.EmailsText)) {
            filters.addAdditionalFilter("SearchEmailsWithout", this.EmailsText, null, null, "Equals", true, false, false, "string");
        }

     

        if (!AppTool.IsNullOrEmpty(this.SearchText) && this.SearchText.indexOf("\\") == -1) {
            filters.addAdditionalFilter("SearchFields", this.SearchText.toLocaleLowerCase(), null, null, "Contains", false, false, false, "string");
        }

        this.userService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {

            }
            else {
                this.ItemsSource = [];

                var list: UserList[] = myResponse.Result;
                if (this.ExcludedResult) {
                    list = list.filter(d => this.ExcludedResult.indexOf(d.Id) == -1);
                }
                for (var i = 0; i < list.length; i++) {
                    var item: UserList = list[i];

                    if (item) {
                        this.ItemsSource.push(new EmailSearchTextBoxItem(item, i));
                    }


                    this.CheckIfIsLostFocus();
                }
            }
        });
    }
    RunContactsSearch() {
        var search = this.SearchText.substring(0, this.SearchText.length - 1).indexOf(';')[0];
        if (search != null) {
            var contact = new ContactList();
            contact.Email = search;
            contact.EnglishName = search;
            contact.SearchFields = search;
            this.ItemsSource.push();
            this.SearchText = "";
        }

        else if (this.SearchText.indexOf(' ') != -1) {
            var contact = new ContactList();
            contact.Email = this.SearchText.indexOf(' ')[0];
            contact.EnglishName = this.SearchText.indexOf(' ')[0];
            contact.SearchFields = this.SearchText.indexOf(' ')[0];
            this.ItemsSource.push();
            this.SearchText = "";
        }
        else {
            var filters = new ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 10;
            filters.SortBy = "EnglishName";
            filters.SortDirection = "Ascending";
            filters.ObjectTableName = "Contact";

            filters.addAdditionalFilter("HasEmail", true, null, null, "Equals", true, false, false, "boolean");

            if (this.DontInCludeInactive) {
                filters.addAdditionalFilter("InActive", false, null, null, "Equals", true, false, false, "boolean");
            }

            if (!AppTool.IsNullOrEmpty(this.EmailsText)) {
                filters.addAdditionalFilter("SearchEmailsWithout", this.EmailsText, null, null, "Equals", true, false, false, "string");
            }

            if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                filters.addAdditionalFilter("SearchFields", this.SearchText.toLowerCase(), null, null, "Contains", false, false, false, "string");
            }

            this.contactService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {

                }

                else {
                    this.ItemsSource = [];

                    var list: ContactList[] = myResponse.Result;
                    for (var i = 0; i < list.length; i++) {
                        var item: ContactList = list[i];

                        if (item) {
                            if (this.SelectedItems.filter(f => f.Id == item.Id).length == 0) {
                                this.ItemsSource.push(new EmailSearchTextBoxItem(item, i));
                            }
                        }
                    }

                    if (this.ItemsSource.length > 0) {
                        this.ItemsSource[0].Selected = true;
                    }
                }
            });
        }
    }
    CheckIfIsLostFocus() {
        if (this.ItemsSource != null && this.ItemsSource.length == 0) {
            this.OnLostFocus();
        }
    }

    // Drop down Position
    private isOpened: boolean = false;
    get IsOpened() { return this.isOpened; }
    set IsOpened(value: boolean) {
        if (value != undefined) {
            if (this.isOpened != value) {
                this.isOpened = value;

                if (value) {
                    this.RunPositionTimer();
                }

                else {
                    this.StopPositionTimer();
                }
            }
        }
    }

    private timerToken: any;
    private StopPositionTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    }
    private RunPositionTimer() {
        this.StopPositionTimer();
        this.timerToken = setInterval(() => this.CalculatePosition(), 0);
    }
    private CalculatePosition() {
        var item = document.getElementById(this.ComponentId);
        if (item) {

            var itemRect = item.getBoundingClientRect();

            document.getElementById(this.ComponentDropDownId).style.width = itemRect.width + 'px';
            document.getElementById(this.ComponentDropDownId).style.top = (itemRect.top + itemRect.height + 1) + 'px';
            document.getElementById(this.ComponentDropDownId).style.left = (itemRect.left) + 'px';
        }
    }

    // Input Position
    SetInputPosition() {
        var Box = document.getElementById(this.ComponentId);
        var Separator = document.getElementById(this.SeparatorId);
        
        if (Box && Separator) {

            var itemRect_Box = Box.getBoundingClientRect();
            var itemRect_Separator = Separator.getBoundingClientRect();

            var inputTop = itemRect_Separator.top - itemRect_Box.top;
            var inputLeft = itemRect_Separator.left - itemRect_Box.left;

            if (inputTop == 1) {
                inputTop = 0;
            }

            if (inputTop == 20) {
                inputTop = 19;
            }

            if (inputTop == 39) {
                inputTop = 38;
            }

            if (inputTop == 58) {
                inputTop = 57;
            }

            document.getElementById(this.InputId).style.top = inputTop + 'px';
            document.getElementById(this.InputId).style.left = inputLeft + 'px';
        }
    }
}
export class EmailSearchTextBoxItem {
    public Id: string;
    public Email: string;
    public EnglishName: string;
    public Index: number = 0;
    public Selected: boolean = false;
    public ElementId: string;
    constructor(item: any, index: number) {
        if (item) {
            this.Index = index;
            this.Id = item['Id'];
            this.Email = item['Email'];
            this.EnglishName = item['EnglishName'];

            this.ElementId = "EmailSearchTextBoxItem_" + this.Id;
        }
    }
}


// scroll into view
// https://developer.mozilla.org/en-US/docs/Web/API/Element/scrollIntoView
// https://stackoverflow.com/questions/6215779/scroll-if-element-is-not-visible
// http://www.performantdesign.com/2009/08/26/scrollintoview-but-only-if-out-of-view/
