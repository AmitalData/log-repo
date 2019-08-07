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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var EntityPartner_1 = require("../../../../Infrastructure/DataContracts/EntityPartner");
var DocumentOutPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentOutPMService");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ComponentArgs_1 = require("../../../../Infrastructure/DataContracts/ComponentArgs");
var ParameterComponentArgs_1 = require("../../../../Infrastructure/DataContracts/ParameterComponentArgs");
var SendToContactsComponent = /** @class */ (function () {
    function SendToContactsComponent(_entityListService, _documentOutPMService, cd) {
        var _this = this;
        this._entityListService = _entityListService;
        this._documentOutPMService = _documentOutPMService;
        this.cd = cd;
        this.onQueryChangeEvent = new core_1.EventEmitter();
        this.ComponentName = "SendTo";
        this.OnCloseSendToContactsEvent = new core_1.EventEmitter();
        this.searchFields = "";
        this.ObjectTableName = "Contact";
        this.items = [];
        this.PartnersObslist = [];
        this.IsSearchIconVisible = true;
        this.SearchFieldchangeevent = new core_1.EventEmitter();
        this.Componentkey = "";
        this.ShowBCC = true;
        this.ShowCC = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.DataSource = {
            pageSize: 20,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        this.ToEmailLists = [];
        this.CcEmailLists = [];
        this.BccEmailLists = [];
        window.ToEmailLists = [];
        window.CcEmailLists = [];
        window.BccEmailLists = [];
        this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (res && res.IsCheck)
                _this.RefreshEmailList(res);
        });
    }
    SendToContactsComponent.prototype.ngOnInit = function () {
    };
    SendToContactsComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            this.CurrentSession.Sessionkey = Guid_1.Guid.newGuid();
        }
        this.PartnersObslist = args.PartnersObslist;
        this.OnCloseSendToContactsEvent = args.OnCloseSendToContactsEvent;
        this.ToEmail = args.ToEmail;
        this.Cc = args.Cc;
        this.Bcc = args.Bcc;
        if (args.HideBCC) {
            this.ShowBCC = false;
        }
        if (args.HideCC) {
            this.ShowCC = false;
        }
        if (this.PartnersObslist) {
            if (!this.PartnersObslist.filter(function (d) { return d.PartnerType == "All"; })[0]) {
                this.PartnersObslist.push(new EntityPartner_1.EntityPartner("All", "", false));
            }
            if (!this.PartnersObslist.filter(function (d) { return d.PartnerType == "All Users"; })[0]) {
                this.PartnersObslist.push(new EntityPartner_1.EntityPartner("All Users", "", false));
            }
            this.SelectedPartnerItem = this.PartnersObslist.filter(function (r) { return r.PartnerType == "Customer" || r.PartnerType == "Customer Contacts"; })[0];
            if (this.SelectedPartnerItem == null) {
                this.SelectedPartnerItem = this.PartnersObslist.filter(function (r) { return r.PartnerType == "Owner"; })[0];
            }
            if (this.SelectedPartnerItem == null && args.IsUserFromReport) {
                this.SelectedPartnerItem = this.PartnersObslist[0];
            }
            if (this.SelectedPartnerItem == null) {
                this.SelectedPartnerItem = this.PartnersObslist.filter(function (r) { return r.PartnerType == "All"; })[0];
            }
            if (args.ObjectTableName) {
                this.ObjectTableName = args.ObjectTableName;
            }
        }
        ComponentArgs_1.ComponentArgs.AddComponent(new ParameterComponentArgs_1.ParameterComponentArgs(this.CurrentSession.Sessionkey + "SendTo", this));
        // To  Email
        if (this.ToEmail) {
            this.ToEmail.split(';').forEach(function (item) {
                if (item) {
                    _this.ToEmailLists.push(item.toLowerCase());
                }
            });
            window.ToEmailLists = this.ToEmailLists;
        }
        // Cc  Email
        if (this.Cc) {
            this.Cc.split(';').forEach(function (item) {
                if (item) {
                    _this.CcEmailLists.push(item.toLowerCase());
                }
            });
            window.CcEmailLists = this.CcEmailLists;
        }
        // Bcc  Email
        if (this.Bcc) {
            this.Bcc.split(';').forEach(function (item) {
                if (item) {
                    _this.BccEmailLists.push(item.toLowerCase());
                }
            });
            window.BccEmailLists = this.BccEmailLists;
        }
        this.BuildColumns();
    };
    SendToContactsComponent.prototype.SelectionChanged = function (item) {
        this.SelectedPartnerItem = item;
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.myPartnerId = this.SelectedPartnerItem.PartnerId;
        this.onQueryChangeEvent.emit({ QueryId: "", Filters: this.filterAgrs });
    };
    SendToContactsComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: "To",
            DataTypeCode: 'String',
            Display: 'To',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/ToComponent',
        });
        this.columns.push({
            FieldName: "Cc",
            DataTypeCode: 'String',
            Display: 'Cc',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/ToComponent',
        });
        this.columns.push({
            FieldName: "Bcc",
            DataTypeCode: 'String',
            Display: 'Bcc',
            IsCustomTemplate: true,
            Styles: { width: '32px' },
            HtmlListComponentName: 'ToComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/ToComponent',
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
        this.columns.push({
            FieldName: "Notes",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Notes',
            Styles: { width: '140px' },
            ServerSideSortable: true,
        });
        this.columns.push({
            FieldName: "Position",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Position',
            Styles: { width: '140px' },
            ServerSideSortable: true,
        });
    };
    SendToContactsComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var rowsObjectTable = this.ObjectTableName;
        if (this.SelectedPartnerItem != null) {
            if (this.SelectedPartnerItem.PartnerType.toUpperCase() != "ALL" && this.SelectedPartnerItem.PartnerTypeCode.toUpperCase() != "ALLUSERS") {
                if (this.SelectedPartnerItem.PartnerId) {
                    if (!this.SelectedPartnerItem.IsUser) {
                        this.myPartnerId = this.SelectedPartnerItem.PartnerId;
                    }
                }
            }
            else
                this.myPartnerId = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(searchfields)) {
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, false, false, "string");
        }
        filters.addAdditionalFilter("CardId", this.myPartnerId, null, null, "Equals", true, true, true, "Text");
        filters.addAdditionalFilter("HasEmail", "", null, null, "NotEqual", true, false, false, "String");
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
        if (this.SelectedPartnerItem != null) {
            if (this.SelectedPartnerItem.PartnerTypeCode.toUpperCase() == "ALLUSERS") {
                filters.addAdditionalFilter("HasUser", "", null, null, "NotEqual", true, false, false, "String");
            }
        }
        return this._entityListService.getByFilters(rowsObjectTable, filters);
    };
    SendToContactsComponent.prototype.RefreshEmailList = function (res) {
        var item = null;
        var index = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            if (ComponentArgs_1.ComponentArgs && ComponentArgs_1.ComponentArgs.ComponentLists) {
                var sessionkey = this.CurrentSession.Sessionkey + "SendTo";
                var Component = ComponentArgs_1.ComponentArgs.ComponentLists.filter(function (d) { return d.key == sessionkey; })[0];
                if (Component) {
                    var myComponent = Component.Component;
                    if (myComponent) {
                        if (res.FieldName == "To") {
                            item = myComponent.ToEmailLists.filter(function (d) { return d.toLowerCase() == res.Email.toLowerCase(); })[0];
                            if (item) {
                                index = myComponent.ToEmailLists.indexOf(res.Email.toLowerCase());
                                if (index != -1)
                                    myComponent.ToEmailLists.splice(index, 1);
                            }
                            else
                                myComponent.ToEmailLists.push(res.Email.toLowerCase());
                            if (myComponent.ToEmailLists.length > 50) {
                                myComponent.IsShowMessageCountTo = true;
                            }
                            else
                                myComponent.IsShowMessageCountTo = false;
                            window.ToEmailLists = myComponent.ToEmailLists;
                        }
                        if (res.FieldName == "Cc") {
                            item = myComponent.CcEmailLists.filter(function (d) { return d.toLowerCase() == res.Email.toLowerCase(); })[0];
                            if (item) {
                                index = myComponent.CcEmailLists.indexOf(res.Email.toLowerCase());
                                if (index != -1)
                                    myComponent.CcEmailLists.splice(index, 1);
                            }
                            else
                                myComponent.CcEmailLists.push(res.Email.toLowerCase());
                            if (myComponent.CcEmailLists.length > 50) {
                                myComponent.IsShowMessageCountCc = true;
                            }
                            else
                                myComponent.IsShowMessageCountCc = false;
                            window.CcEmailLists = myComponent.CcEmailLists;
                        }
                        if (res.FieldName == "Bcc") {
                            item = myComponent.BccEmailLists.filter(function (d) { return d.toLowerCase() == res.Email.toLowerCase(); })[0];
                            if (item) {
                                index = myComponent.BccEmailLists.indexOf(res.Email.toLowerCase());
                                if (index != -1)
                                    myComponent.BccEmailLists.splice(index, 1);
                            }
                            else
                                myComponent.BccEmailLists.push(res.Email.toLowerCase());
                            if (myComponent.BccEmailLists.length > 50) {
                                myComponent.IsShowMessageCountBcc = true;
                            }
                            else
                                myComponent.IsShowMessageCountBcc = false;
                            window.BccEmailLists = myComponent.BccEmailLists;
                        }
                        if (myComponent.IsShowMessageCountTo || myComponent.IsShowMessageCountCc || myComponent.IsShowMessageCountBcc) {
                            myComponent.IsShowMessageCount = true;
                        }
                        else
                            myComponent.IsShowMessageCount = false;
                    }
                }
            }
        }
        res.IsCheck = false;
    };
    SendToContactsComponent.prototype.onSearchTextChangeEvent = function (searchtext) {
        this.searchFields = searchtext;
        this.SearchFieldchangeevent.emit(this.searchFields);
    };
    SendToContactsComponent.prototype.DeleteEmail = function (fieldName, email) {
        if (fieldName == "To")
            window.ToEmailLists = this.ToEmailLists = this.ToEmailLists.filter(function (d) { return d.toLowerCase() != email.toLowerCase(); });
        if (fieldName == "Cc")
            window.CcEmailLists = this.CcEmailLists = this.CcEmailLists.filter(function (d) { return d.toLowerCase() != email.toLowerCase(); });
        if (fieldName == "Bcc")
            window.BccEmailLists = this.BccEmailLists = this.BccEmailLists.filter(function (d) { return d.toLowerCase() != email.toLowerCase(); });
        this.SearchFieldchangeevent.emit(this.searchFields);
    };
    SendToContactsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SendToContactsComponent.prototype.SaveButtonClicked = function () {
        this.OnCloseSendToContactsEvent.emit(this);
        this.CloseButtonClicked();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SendToContactsComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SendToContactsComponent.prototype, "SearchFieldchangeevent", void 0);
    SendToContactsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SendToContacts',
            templateUrl: './SendToContactsComponent.html',
            providers: [EntityListService_1.EntityListService, DocumentOutPMService_1.DocumentOutPMService]
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService, DocumentOutPMService_1.DocumentOutPMService, core_1.ChangeDetectorRef])
    ], SendToContactsComponent);
    return SendToContactsComponent;
}());
exports.SendToContactsComponent = SendToContactsComponent;
//# sourceMappingURL=SendToContactsComponent.js.map