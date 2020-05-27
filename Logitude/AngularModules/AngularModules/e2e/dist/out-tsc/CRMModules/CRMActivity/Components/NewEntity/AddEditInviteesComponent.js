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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityPartner_1 = require("../../../../Infrastructure/DataContracts/EntityPartner");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var ComponentArgs_1 = require("../../../../Infrastructure/DataContracts/ComponentArgs");
var ParameterComponentArgs_1 = require("../../../../Infrastructure/DataContracts/ParameterComponentArgs");
var ActivityInviteePM_1 = require("../../../../CRM/EntityPMs/ActivityInviteePM");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var AddEditInviteesComponent = /** @class */ (function (_super) {
    __extends(AddEditInviteesComponent, _super);
    function AddEditInviteesComponent(_entityListService, cd) {
        var _this = _super.call(this) || this;
        _this._entityListService = _entityListService;
        _this.cd = cd;
        _this.DataContext = _this;
        _this.items = [];
        _this.ObjectTableName = "Contact";
        _this.RequiredList = [];
        _this.OptionalList = [];
        _this.ValidationErrorsList = [];
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.SearchFieldchangeevent = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DataSource = {
            pageSize: 20,
            rowCount: null,
            sortingDir: "Descending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.requiredEmailBoxText = "";
        _this.optionalEmailBoxText = "";
        // Search 
        _this.searchFields = "";
        _this.errors = [];
        _this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (res.Name == "InviteeCheckBoxComponent") {
                _this.RefreshEmailList(res.select);
            }
        });
        return _this;
    }
    AddEditInviteesComponent.prototype.SetWindowArgs = function (args) {
        if (!this.CurrentSession.Sessionkey) {
            this.CurrentSession.Sessionkey = Guid_1.Guid.newGuid();
        }
        this.InitializeLists();
        this.entityPM = args.Entity;
        this.RequiredList = this.entityPM.ActivityInvitees.filter(function (d) { return d.IsRequired == true; });
        this.OptionalList = this.entityPM.ActivityInvitees.filter(function (d) { return d.IsRequired == false; });
        window.RequiredList = this.RequiredList;
        window.OptionalList = this.OptionalList;
        this.BuildRequiredEmailList();
        this.BuildOptionalEmailList();
        this.BuildColumns();
    };
    AddEditInviteesComponent.prototype.InitializeLists = function () {
        this.PartnersObslist = [];
        this.RequiredList = [];
        this.OptionalList = [];
        window.RequiredList = [];
        window.OptionalList = [];
        this.PartnersObslist.push(new EntityPartner_1.EntityPartner("All", "", false));
        if (this.SelectedPartnerItem == null) {
            this.SelectedPartnerItem = this.PartnersObslist.filter(function (r) { return r.PartnerType == "All"; })[0];
        }
        ComponentArgs_1.ComponentArgs.AddComponent(new ParameterComponentArgs_1.ParameterComponentArgs(this.CurrentSession.Sessionkey + "SendActivity", this));
    };
    AddEditInviteesComponent.prototype.BuildRequiredEmailList = function () {
        var _this = this;
        this.RequiredBoxText = "";
        this.RequiredEmailBoxText = "";
        this.RequiredList.forEach(function (item) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.ContactId)) {
                _this.RequiredBoxText += item.Email + ";";
            }
            else {
                _this.RequiredEmailBoxText += item.Email + ";";
            }
        });
    };
    AddEditInviteesComponent.prototype.BuildOptionalEmailList = function () {
        var _this = this;
        this.OptionalBoxText = "";
        this.OptionalEmailBoxText = "";
        this.OptionalList.forEach(function (item) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.ContactId)) {
                _this.OptionalBoxText += item.Email + ";";
            }
            else {
                _this.OptionalEmailBoxText += item.Email + ";";
            }
        });
    };
    AddEditInviteesComponent.prototype.RefreshEmailList = function (res) {
        var item = null;
        var index = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            if (ComponentArgs_1.ComponentArgs && ComponentArgs_1.ComponentArgs.ComponentLists) {
                var sessionkey = this.CurrentSession.Sessionkey + "SendActivity";
                var Component = ComponentArgs_1.ComponentArgs.ComponentLists.filter(function (d) { return d.key == sessionkey; })[0];
                if (Component) {
                    var myComponent = Component.Component;
                    if (myComponent) {
                        if (res.FieldName == "Required") {
                            if (res.IsCheck) {
                                item = myComponent.RequiredList.filter(function (d) { return d.Email == res.Email && d.ContactId == res.ContactId; })[0];
                                if (item == null) {
                                    var invitee = new ActivityInviteePM_1.ActivityInviteePM(null);
                                    invitee.IsRequired = true;
                                    invitee.Email = res.Email;
                                    invitee.ContactId = res.ContactId;
                                    invitee.ContactName = res.ContactName;
                                    myComponent.RequiredList.push(invitee);
                                }
                            }
                            else {
                                item = myComponent.RequiredList.filter(function (d) { return d.Email == res.Email && d.ContactId == res.ContactId; })[0];
                                if (item) {
                                    index = myComponent.RequiredList.indexOf(item);
                                    if (index != -1)
                                        myComponent.RequiredList.splice(index, 1);
                                }
                            }
                            window.RequiredList = myComponent.RequiredList;
                            this.RequiredList = myComponent.RequiredList;
                            this.BuildRequiredEmailList();
                            //this.RequiredList = myComponent.RequiredList;
                            //this.BuildRequiredEmailList();
                        }
                        if (res.FieldName == "Optional") {
                            if (res.IsCheck) {
                                item = myComponent.OptionalList.filter(function (d) { return d.Email == res.Email && d.ContactId == res.ContactId; })[0];
                                if (item == null) {
                                    var invitee = new ActivityInviteePM_1.ActivityInviteePM(null);
                                    invitee.IsRequired = false;
                                    invitee.Email = res.Email;
                                    invitee.ContactId = res.ContactId;
                                    invitee.ContactName = res.ContactName;
                                    myComponent.OptionalList.push(invitee);
                                }
                            }
                            else {
                                item = myComponent.OptionalList.filter(function (d) { return d.Email == res.Email && d.ContactId == res.ContactId; })[0];
                                if (item) {
                                    index = myComponent.OptionalList.indexOf(item);
                                    if (index != -1)
                                        myComponent.OptionalList.splice(index, 1);
                                }
                            }
                            window.OptionalList = myComponent.OptionalList;
                            this.OptionalList = myComponent.OptionalList;
                            this.BuildOptionalEmailList();
                        }
                        //res.IsCheck = false;
                    }
                }
            }
        }
    };
    AddEditInviteesComponent.prototype.SelectionChanged = function (item) {
        this.SelectedPartnerItem = item;
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.myPartnerId = this.SelectedPartnerItem.PartnerId;
        console.log(this.myPartnerId);
        this.onQueryChangeEvent.emit({ QueryId: "", Filters: this.filterAgrs });
    };
    AddEditInviteesComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: "Required",
            DataTypeCode: 'String',
            Display: 'Required',
            IsCustomTemplate: true,
            Styles: { width: '70px' },
            HtmlListComponentName: 'InviteeCheckBoxComponent',
            HtmlListComponentUrl: './CRMModules/CRMActivity/Components/NewEntity/InviteeCheckBoxComponent',
        });
        this.columns.push({
            FieldName: "Optional",
            DataTypeCode: 'String',
            Display: 'Optional',
            IsCustomTemplate: true,
            Styles: { width: '70px' },
            HtmlListComponentName: 'InviteeCheckBoxComponent',
            HtmlListComponentUrl: './CRMModules/CRMActivity/Components/NewEntity/InviteeCheckBoxComponent',
        });
        this.columns.push({
            FieldName: "Email",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Email',
            Styles: { width: '180px' },
        });
        this.columns.push({
            FieldName: "Name",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Name',
            Styles: { width: '180px' },
        });
        this.columns.push({
            FieldName: "Company",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Company',
            Styles: { width: '150px' },
        });
    };
    AddEditInviteesComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var rowsObjectTable = this.ObjectTableName;
        if (this.SelectedPartnerItem != null) {
            if (this.SelectedPartnerItem.PartnerType.toUpperCase() != "ALL") {
                if (this.SelectedPartnerItem.PartnerId) {
                    if (!this.SelectedPartnerItem.IsUser) {
                        this.myPartnerId = this.SelectedPartnerItem.PartnerId;
                    }
                }
            }
            else
                this.myPartnerId = null;
        }
        if (filters.AdditionalFilters.filter(function (a) { return a.FieldName == "CardId"; }).length > 0) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(function (a) { return a.FieldName != "CardId"; });
        }
        filters.addAdditionalFilter("CardId", this.myPartnerId, null, null, "Equals", true, true, true, "Text");
        return this._entityListService.getByFilters(rowsObjectTable, filters);
    };
    Object.defineProperty(AddEditInviteesComponent.prototype, "RequiredBoxText", {
        get: function () { return this.requiredBoxText; },
        set: function (value) {
            this.requiredBoxText = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInviteesComponent.prototype, "OptionalBoxText", {
        get: function () { return this.optionalBoxText; },
        set: function (value) {
            this.optionalBoxText = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInviteesComponent.prototype, "RequiredEmailBoxText", {
        get: function () { return this.requiredEmailBoxText; },
        set: function (value) {
            this.requiredEmailBoxText = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInviteesComponent.prototype, "OptionalEmailBoxText", {
        get: function () { return this.optionalEmailBoxText; },
        set: function (value) {
            this.optionalEmailBoxText = value;
        },
        enumerable: true,
        configurable: true
    });
    AddEditInviteesComponent.prototype.onSearchTextChangeEvent = function (searchtext) {
        this.searchFields = searchtext;
        this.SearchFieldchangeevent.emit(this.searchFields);
    };
    // Commands
    AddEditInviteesComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditInviteesComponent.prototype.SaveButtonClicked = function () {
        this.errors = [];
        this.CheckIsValidEmails(this.RequiredEmailBoxText);
        this.CheckIsValidEmails(this.OptionalEmailBoxText);
        this.ValidationErrorsList = this.errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditInviteesComponent.prototype.CheckIsValidEmails = function (mailsList) {
        var _this = this;
        var EMAIL_REGEXP = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var IsOk = true;
        if (mailsList) {
            var emails = mailsList.split(';');
            emails.forEach(function (item) {
                if (item) {
                    if (!EMAIL_REGEXP.test(item)) {
                        IsOk = false;
                        _this.errors.push(item + " has invalid format");
                        return;
                    }
                }
            });
        }
        return IsOk;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], AddEditInviteesComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], AddEditInviteesComponent.prototype, "SearchFieldchangeevent", void 0);
    AddEditInviteesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditInviteesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService, core_1.ChangeDetectorRef])
    ], AddEditInviteesComponent);
    return AddEditInviteesComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditInviteesComponent = AddEditInviteesComponent;
//# sourceMappingURL=AddEditInviteesComponent.js.map