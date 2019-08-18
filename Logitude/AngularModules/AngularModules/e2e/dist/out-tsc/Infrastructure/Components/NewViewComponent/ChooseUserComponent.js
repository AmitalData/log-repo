"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var SharedUserQueryPM_1 = require("../../EntityPMs/SharedUserQueryPM");
var ChooseUserComponent = /** @class */ (function () {
    function ChooseUserComponent() {
        this.UsersItemsSource = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.savedList = [];
    }
    ChooseUserComponent.prototype.SetWindowArgs = function (args) {
        this.args = args;
        this.MyQuery = args.MyQuery;
        this.myUsersList = args.AllUsers;
        this.SaveData();
        this.BuildData();
    };
    ChooseUserComponent.prototype.SaveData = function () {
        var _this = this;
        this.MyQuery.SharedUserQueries.forEach(function (item) {
            var newItem = new SharedUserQueryPM_1.SharedUserQueryPM(null);
            newItem.Id = item.Id;
            newItem.UserId = item.UserId;
            newItem.QueryId = item.QueryId;
            _this.savedList.push(newItem);
        });
    };
    ChooseUserComponent.prototype.BuildData = function () {
        var _this = this;
        var myList = [];
        this.UsersItemsSource = [];
        if (!this.mySearchText) {
            myList = this.myUsersList;
        }
        else {
            myList = this.myUsersList.filter(function (d) { return (d.Email && d.Email.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1) || (d.EnglishName && d.EnglishName.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1); });
        }
        myList.forEach(function (item) {
            var mySharedUser = _this.MyQuery.SharedUserQueries.filter(function (d) { return d.UserId == item.Id; })[0];
            _this.UsersItemsSource.push(new UserItem(item, mySharedUser, _this));
        });
    };
    ChooseUserComponent.prototype.onSearchTextChangeEvent = function (searchText) {
        if (!searchText)
            searchText = "";
        this.mySearchText = searchText;
        this.BuildData();
    };
    ChooseUserComponent.prototype.CloseButtonClicked = function () {
        var _this = this;
        this.MyQuery.SharedUserQueries = [];
        this.savedList.forEach(function (item) {
            _this.MyQuery.AddSharedUserQueryPM(item);
        });
        this.CurrentSession.CloseCurrentWindow();
    };
    ChooseUserComponent.prototype.SaveButtonClicked = function () {
        this.ValidationErrorsList = [];
        if (this.MyQuery.SharedUserQueries.length > 0) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            this.ValidationErrorsList.push("Please choose at least one user");
        }
    };
    ChooseUserComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ChooseUserComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ChooseUserComponent);
    return ChooseUserComponent;
}());
exports.ChooseUserComponent = ChooseUserComponent;
var UserItem = /** @class */ (function (_super) {
    __extends(UserItem, _super);
    function UserItem(entity, mySharedUser, fatherCompo) {
        var _this = _super.call(this) || this;
        _this.fatherCompo = fatherCompo;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.userList = entity;
        _this.sharedUserQuery = mySharedUser;
        if (mySharedUser != null) {
            _this.isChecked = true;
        }
        return _this;
    }
    Object.defineProperty(UserItem.prototype, "Email", {
        get: function () { return this.userList.Email; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserItem.prototype, "Name", {
        get: function () { return this.userList.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserItem.prototype, "Notes", {
        get: function () { return this.userList.Notes; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserItem.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
                this.SetIsChecked();
            }
        },
        enumerable: true,
        configurable: true
    });
    UserItem.prototype.SetIsChecked = function () {
        if (this.IsChecked) {
            var newItem = new SharedUserQueryPM_1.SharedUserQueryPM(this.fatherCompo.MyQuery);
            newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
            newItem.UserId = this.userList.Id;
            if (this.fatherCompo.MyQuery.SharedUserQueries.indexOf(newItem) == -1) {
                this.fatherCompo.MyQuery.AddSharedUserQueryPM(newItem);
            }
        }
        else {
            var index = this.fatherCompo.MyQuery.SharedUserQueries.indexOf(this.sharedUserQuery);
            if (index > -1) {
                this.fatherCompo.MyQuery.RemoveSharedUserQueryPM(this.sharedUserQuery);
            }
        }
    };
    return UserItem;
}(BaseComponent_1.BaseComponent));
exports.UserItem = UserItem;
var ChooseUserArgs = /** @class */ (function () {
    function ChooseUserArgs() {
    }
    return ChooseUserArgs;
}());
exports.ChooseUserArgs = ChooseUserArgs;
//# sourceMappingURL=ChooseUserComponent.js.map