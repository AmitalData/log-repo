"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../../Infrastructure/Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var ContactList_1 = require("../../Common/EntityLists/ContactList");
var ContactListService_1 = require("../../Common/Services/StandardLists/ContactListService");
var UserListService_1 = require("../../Common/Services/StandardLists/UserListService");
var CommonDomainService_1 = require("../../Common/Services/CommonDomainService");
var ApiQueryFilters_1 = require("../../Infrastructure/DataContracts/ApiQueryFilters");
var EmailSearchTextBox = /** @class */ (function () {
    function EmailSearchTextBox() {
        this.ComponentId = null;
        this.SeparatorId = null;
        this.InputId = null;
        this.TextAreaId = null;
        this.ComponentDropDownId = null;
        this.Watermark = null;
        this.emailText = null;
        this.IsUsersList = false;
        this.DropDownHeight = 200;
        this.DropDownWidth = 300;
        this.ItemsSource = [];
        this.SelectedItems = [];
        this.IsFocused = false;
        this.Placeholder = null;
        this.SelectedValuePath = 'Email';
        this.IsDisabled = false;
        this.DontInCludeInactive = false;
        this.EmailsTextChanged = new core_1.EventEmitter();
        this.SelectedListChanged = new core_1.EventEmitter();
        this.ValidationErrorsListChanged = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsShowRedUserInActiveNote = false;
        // Search
        this.searchText = null;
        // Drop down Position
        this.isOpened = false;
        var idIndex = this.CurrentSession.GetNewId("EmailSearchTextBox");
        this.ComponentId = "EmailSearchTextBox_" + idIndex;
        this.SeparatorId = "EmailSearchTextBox_Separator_" + idIndex;
        this.InputId = "EmailSearchTextBox_Input_" + idIndex;
        this.TextAreaId = "EmailSearchTextBox_TextArea_" + idIndex;
        this.ComponentDropDownId = "EmailSearchTextBox_DropDown_" + idIndex;
        this.contactService = new ContactListService_1.ContactListService();
        this.userService = new UserListService_1.UserListService();
    }
    Object.defineProperty(EmailSearchTextBox.prototype, "EmailsText", {
        get: function () { return this.emailText; },
        set: function (value) { if (this.emailText != value)
            this.emailText = value; },
        enumerable: true,
        configurable: true
    });
    EmailSearchTextBox.prototype.ngOnInit = function () {
        var _this = this;
        this.Placeholder = this.Watermark;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EmailsText)) {
            var myDomainService = new CommonDomainService_1.CommonDomainService();
            if (this.SelectedValuePath == "Id") {
                myDomainService.GetUserListsByidsString(this.EmailsText).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.SelectedItems = myResponse.Result;
                        _this.SelectedItems.forEach(function (item) {
                            if (item.InActive) {
                                _this.IsShowRedUserInActiveNote = true;
                            }
                        });
                    }
                    setTimeout(function () { return _this.SetInputPosition(); }, 5);
                });
            }
            else {
                if (this.IsUsersList) {
                    myDomainService.GetUsersByEmails(this.EmailsText).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.SelectedItems = myResponse.Result;
                        }
                        setTimeout(function () { return _this.SetInputPosition(); }, 5);
                    });
                }
                else {
                    myDomainService.GetContactsByEmails(this.EmailsText).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.SelectedItems = myResponse.Result;
                        }
                        var allEmails = _this.EmailsText.split(';');
                        if (allEmails.length > _this.SelectedItems.length) {
                            allEmails.forEach(function (email) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(email)) {
                                    if (_this.SelectedItems.filter(function (f) { return f.Email != null && f.Email.toLowerCase() == email.toLowerCase(); }).length == 0) {
                                        var newItem = new ContactList_1.ContactList();
                                        newItem.Email = email;
                                        newItem.EnglishName = email;
                                        _this.SelectedItems.push(newItem);
                                    }
                                }
                            });
                        }
                        setTimeout(function () { return _this.SetInputPosition(); }, 5);
                    });
                }
            }
        }
    };
    EmailSearchTextBox.prototype.ngAfterViewInit = function () {
        this.SetInputPosition();
    };
    EmailSearchTextBox.prototype.AddItem = function (item) {
        var _this = this;
        if (item) {
            var emails = this.SelectedItems.filter(function (f) { return f.Id == item.Id; });
            if (emails.length == 0) {
                this.SelectedItems.push(item);
                this.SearchText = null;
                setTimeout(function () { return _this.SetInputPosition(); }, 5);
                this.BuildEmailsText();
                setTimeout(function () { return _this.SetInputFocus(); }, 5);
            }
        }
    };
    EmailSearchTextBox.prototype.RemoveItem = function (item) {
        var _this = this;
        if (item) {
            if (this.SelectedItems.filter(function (f) { return f.Id == item.Id; }).length > 0) {
                var index = this.SelectedItems.indexOf(item);
                this.SelectedItems.splice(index, 1);
                setTimeout(function () { return _this.SetInputPosition(); }, 5);
                this.BuildEmailsText();
                setTimeout(function () { return _this.SetInputFocus(); }, 5);
            }
        }
    };
    EmailSearchTextBox.prototype.SetInputFocus = function () {
        this.IsFocused = true;
        document.getElementById(this.TextAreaId).focus();
    };
    EmailSearchTextBox.prototype.OnLostFocus = function () {
        this.SearchText = null;
        this.IsFocused = false;
        this.IsOpened = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.EmailsText)) {
            this.IsOpened = false;
            this.Placeholder = this.Watermark;
            this.ItemsSource = [];
        }
        else {
            this.Placeholder = "";
        }
    };
    EmailSearchTextBox.prototype.OnKeyDown = function (event) {
        var _this = this;
        switch (event.keyCode) {
            case 8: {
                // Backspace
                if (Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
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
                if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                    var item = null;
                    if (this.ItemsSource.length > 0) {
                        item = this.ItemsSource.filter(function (f) { return f.Email != null && f.Email.toLowerCase() == _this.SearchText.toLowerCase(); })[0];
                    }
                    if (item) {
                        this.AddItem(item);
                        event.preventDefault();
                    }
                    else if (Tools_1.FormatTool.IsEmail(this.SearchText)) {
                        if (!this.IsUsersList) {
                            var newItem = new ContactList_1.ContactList();
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
                            var myCurrentSelectedItem = this.ItemsSource.filter(function (f) { return f.Selected == true; })[0];
                            if (myCurrentSelectedItem == null) {
                                this.ItemsSource.filter(function (f) { return f.Index == 0; })[0].Selected = true;
                            }
                            else {
                                var myNewSelectedIndex = myCurrentSelectedItem.Index - 1;
                                var myNewSelectedItem = this.ItemsSource.filter(function (f) { return f.Index == myNewSelectedIndex; })[0];
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
                            var myCurrentSelectedItem = this.ItemsSource.filter(function (f) { return f.Selected == true; })[0];
                            if (myCurrentSelectedItem == null) {
                                this.ItemsSource.filter(function (f) { return f.Index == 0; })[0].Selected = true;
                            }
                            else {
                                var myNewSelectedIndex = myCurrentSelectedItem.Index + 1;
                                var myNewSelectedItem = this.ItemsSource.filter(function (f) { return f.Index == myNewSelectedIndex; })[0];
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
                            var myCurrentSelectedItem = this.ItemsSource.filter(function (f) { return f.Selected == true; })[0];
                            if (myCurrentSelectedItem != null) {
                                this.AddItem(myCurrentSelectedItem);
                            }
                        }
                    }
                }
                break;
            }
        }
    };
    EmailSearchTextBox.prototype.BuildEmailsText = function () {
        var myResult = null;
        var selectedList = [];
        var errors = [];
        if (this.SelectedValuePath == "Id") {
            this.SelectedItems.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Id)) {
                    if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                        myResult = item.Id;
                        selectedList.push(item);
                    }
                    else {
                        myResult += ";" + item.Id;
                        selectedList.push(item);
                    }
                    if (!Tools_1.FormatTool.IsEmail(item.Email)) {
                        errors.push("\"" + item.Email + "\" email address is not recognised.");
                    }
                }
            });
        }
        else {
            this.SelectedItems.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Email)) {
                    if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                        myResult = item.Email;
                        selectedList.push(item);
                    }
                    else {
                        myResult += ";" + item.Email;
                        selectedList.push(item);
                    }
                    if (!Tools_1.FormatTool.IsEmail(item.Email)) {
                        errors.push("\"" + item.Email + "\" email address is not recognised.");
                    }
                }
            });
        }
        this.EmailsText = myResult;
        this.EmailsTextChanged.emit(myResult);
        this.SelectedListChanged.emit(selectedList);
        this.ValidationErrorsListChanged.emit(errors);
    };
    Object.defineProperty(EmailSearchTextBox.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (value) {
            if (this.searchText != value) {
                this.searchText = value;
                this.OnSearchTextChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    EmailSearchTextBox.prototype.OnSearchTextChanged = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            this.IsOpened = false;
            this.ItemsSource = [];
        }
        else {
            this.IsOpened = true;
            if (this.timerTokenSearch) {
                clearTimeout(this.timerTokenSearch);
            }
            this.timerTokenSearch = setTimeout(function () { return _this.RunSearch(); }, 400);
        }
    };
    EmailSearchTextBox.prototype.RunSearch = function () {
        if (this.IsUsersList) {
            this.RunUsersSearch();
        }
        else {
            this.RunContactsSearch();
        }
    };
    EmailSearchTextBox.prototype.RunUsersSearch = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Ascending";
        filters.ObjectTableName = "User";
        filters.addAdditionalFilter("HasEmail", true, null, null, "Equals", true, false, false, "boolean");
        if (this.DontInCludeInactive) {
            filters.addAdditionalFilter("InActive", false, null, null, "Equals", true, false, false, "boolean");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EmailsText)) {
            filters.addAdditionalFilter("SearchEmailsWithout", this.EmailsText, null, null, "Equals", true, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText) && this.SearchText.indexOf("\\") == -1) {
            filters.addAdditionalFilter("SearchFields", this.SearchText.toLocaleLowerCase(), null, null, "Contains", false, false, false, "string");
        }
        this.userService.getByFilters(filters).subscribe(function (myResponse) {
            if (myResponse.HasError) {
            }
            else {
                _this.ItemsSource = [];
                var list = myResponse.Result;
                if (_this.ExcludedResult) {
                    list = list.filter(function (d) { return _this.ExcludedResult.indexOf(d.Id) == -1; });
                }
                for (var i = 0; i < list.length; i++) {
                    var item = list[i];
                    if (item) {
                        _this.ItemsSource.push(new EmailSearchTextBoxItem(item, i));
                    }
                    _this.CheckIfIsLostFocus();
                }
            }
        });
    };
    EmailSearchTextBox.prototype.RunContactsSearch = function () {
        var _this = this;
        var search = this.SearchText.substring(0, this.SearchText.length - 1).indexOf(';')[0];
        if (search != null) {
            var contact = new ContactList_1.ContactList();
            contact.Email = search;
            contact.EnglishName = search;
            contact.SearchFields = search;
            this.ItemsSource.push();
            this.SearchText = "";
        }
        else if (this.SearchText.indexOf(' ') != -1) {
            var contact = new ContactList_1.ContactList();
            contact.Email = this.SearchText.indexOf(' ')[0];
            contact.EnglishName = this.SearchText.indexOf(' ')[0];
            contact.SearchFields = this.SearchText.indexOf(' ')[0];
            this.ItemsSource.push();
            this.SearchText = "";
        }
        else {
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 10;
            filters.SortBy = "EnglishName";
            filters.SortDirection = "Ascending";
            filters.ObjectTableName = "Contact";
            filters.addAdditionalFilter("HasEmail", true, null, null, "Equals", true, false, false, "boolean");
            if (this.DontInCludeInactive) {
                filters.addAdditionalFilter("InActive", false, null, null, "Equals", true, false, false, "boolean");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EmailsText)) {
                filters.addAdditionalFilter("SearchEmailsWithout", this.EmailsText, null, null, "Equals", true, false, false, "string");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                filters.addAdditionalFilter("SearchFields", this.SearchText.toLowerCase(), null, null, "Contains", false, false, false, "string");
            }
            this.contactService.getByFilters(filters).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                }
                else {
                    _this.ItemsSource = [];
                    var list = myResponse.Result;
                    for (var i = 0; i < list.length; i++) {
                        var item = list[i];
                        if (item) {
                            if (_this.SelectedItems.filter(function (f) { return f.Id == item.Id; }).length == 0) {
                                _this.ItemsSource.push(new EmailSearchTextBoxItem(item, i));
                            }
                        }
                    }
                    if (_this.ItemsSource.length > 0) {
                        _this.ItemsSource[0].Selected = true;
                    }
                }
            });
        }
    };
    EmailSearchTextBox.prototype.CheckIfIsLostFocus = function () {
        if (this.ItemsSource != null && this.ItemsSource.length == 0) {
            this.OnLostFocus();
        }
    };
    Object.defineProperty(EmailSearchTextBox.prototype, "IsOpened", {
        get: function () { return this.isOpened; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    EmailSearchTextBox.prototype.StopPositionTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    };
    EmailSearchTextBox.prototype.RunPositionTimer = function () {
        var _this = this;
        this.StopPositionTimer();
        this.timerToken = setInterval(function () { return _this.CalculatePosition(); }, 0);
    };
    EmailSearchTextBox.prototype.CalculatePosition = function () {
        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.ComponentDropDownId).style.width = itemRect.width + 'px';
            document.getElementById(this.ComponentDropDownId).style.top = (itemRect.top + itemRect.height + 1) + 'px';
            document.getElementById(this.ComponentDropDownId).style.left = (itemRect.left) + 'px';
        }
    };
    // Input Position
    EmailSearchTextBox.prototype.SetInputPosition = function () {
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
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EmailSearchTextBox.prototype, "EmailsTextChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EmailSearchTextBox.prototype, "SelectedListChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], EmailSearchTextBox.prototype, "ValidationErrorsListChanged", void 0);
    EmailSearchTextBox = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EmailSearchTextBox.html',
            selector: "EmailSearchTextBox",
            inputs: ['Watermark', 'EmailsText', 'IsUsersList', 'IsDisabled', 'SelectedValuePath', 'ExcludedResult', 'DontInCludeInactive'],
        }),
        __metadata("design:paramtypes", [])
    ], EmailSearchTextBox);
    return EmailSearchTextBox;
}());
exports.EmailSearchTextBox = EmailSearchTextBox;
var EmailSearchTextBoxItem = /** @class */ (function () {
    function EmailSearchTextBoxItem(item, index) {
        this.Index = 0;
        this.Selected = false;
        if (item) {
            this.Index = index;
            this.Id = item['Id'];
            this.Email = item['Email'];
            this.EnglishName = item['EnglishName'];
            this.ElementId = "EmailSearchTextBoxItem_" + this.Id;
        }
    }
    return EmailSearchTextBoxItem;
}());
exports.EmailSearchTextBoxItem = EmailSearchTextBoxItem;
// scroll into view
// https://developer.mozilla.org/en-US/docs/Web/API/Element/scrollIntoView
// https://stackoverflow.com/questions/6215779/scroll-if-element-is-not-visible
// http://www.performantdesign.com/2009/08/26/scrollintoview-but-only-if-out-of-view/
//# sourceMappingURL=EmailSearchTextBox.js.map