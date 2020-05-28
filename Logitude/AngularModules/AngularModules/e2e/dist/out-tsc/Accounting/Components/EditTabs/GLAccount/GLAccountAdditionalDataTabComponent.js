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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var GLAccountExtendedListService_1 = require("../../../Services/ExtendedLists/GLAccountExtendedListService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var GLAccountExtendedPMService_1 = require("../../../Services/ExtendedPMs/GLAccountExtendedPMService");
var GLAccountPMService_1 = require("../../../Services/StandardPMs/GLAccountPMService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var GLAccountAdditionalDataTabComponent = /** @class */ (function (_super) {
    __extends(GLAccountAdditionalDataTabComponent, _super);
    function GLAccountAdditionalDataTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "GLAccount";
        _this.ObjectTableId = window.ObjectTables.filter(function (f) { return f.Name === _this.ObjectTableName; })[0].Id;
        _this.DataContext = _this;
        _this.glAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        _this.GLAccountExtendedPMService = new GLAccountExtendedPMService_1.GLAccountExtendedPMService();
        _this.entityPM = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ConnectedGLAccounts = new ObservableCollection_1.ObservableCollection([]);
        _this.ChildrenGLAccounts = new ObservableCollection_1.ObservableCollection([]);
        _this.entityPM = _this.entityArgs.EntityPM;
        _this.BuildConnectedGLAccountsList();
        _this.BuildChildrenGLAccountsList();
        _this.ChildrenFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        return _this;
        //this.ChildrenFilterItems.addAdditionalFilter("Id", this.EntityPM.Id, null, null, "Exclude", false, false, false, "string", false, true);
        //this.ChildrenFilterItems.addAdditionalFilter("ChartOfAccountsId", this.EntityPM.ChartOfAccountsId, null, null, "Equals", false, false, false, "string", false, true);
        //this.ChildrenFilterItems.addAdditionalFilter("ParentAccountId", this.entityPM.ParentAccountId, null, null, "IsNull", false, false, false, "string", false, true);
        //if (this.IsCustomerAccount) {
        //    this.ParentsFilterItems.addAdditionalFilter("AccountTypeCode", "2", null, null, "Equals", false, false, false, "string", false, true);
        //}
    }
    GLAccountAdditionalDataTabComponent.prototype.BuildConnectedGLAccountsList = function () {
        var _this = this;
        this.GLAccountExtendedPMService.GetSplittedByCurrencyGLAccounts(this.entityPM.Id).subscribe(function (myResponse) {
            if (myResponse) {
                if (!myResponse.HasError) {
                    for (var _i = 0, _a = myResponse.Result; _i < _a.length; _i++) {
                        var item = _a[_i];
                        _this.ConnectedGLAccounts.Insert(new SplittedByCurrencyAccount(item, _this));
                    }
                }
            }
        });
    };
    GLAccountAdditionalDataTabComponent.prototype.BuildChildrenGLAccountsList = function () {
        var _this = this;
        this.ChildrenGLAccounts.Clear();
        this.glAccountExtendedListService.GetChildrenGLAccounts(this.entityPM.Id).subscribe(function (myResponse) {
            if (myResponse) {
                if (!myResponse.HasError) {
                    for (var _i = 0, _a = myResponse.Result; _i < _a.length; _i++) {
                        var item = _a[_i];
                        _this.ChildrenGLAccounts.Insert(new GLAccountChild(item, _this));
                    }
                }
            }
        });
    };
    GLAccountAdditionalDataTabComponent.prototype.Add = function () {
        var _this = this;
        if (this.entityPM.IsMultiCurrency) {
            var windowArgs = {};
            windowArgs.EntityPM = this.entityPM;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewConnectedGLAccount");
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 400;
            logWindow.Height = 300;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            windowArgs.Parent = this;
            logWindow.WindowArgs = windowArgs;
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.AddNewLine(comp);
                    }
                });
            });
            logWindow.Show('./Accounting/Components/EditTabs/GLAccount/NewConnectedGLAccountComponent');
        }
        else {
            var msg = new MessageWindow_1.MessageWindow();
            msg.RTL = true;
            msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.MultiCurrencyForSplitted"));
        }
    };
    GLAccountAdditionalDataTabComponent.prototype.AddNewLine = function (data) {
        if (data.accountPM) {
            // if (!this.entityPM.ConnectedItems) this.entityPM.ConnectedItems = "";
            this.ConnectedGLAccounts.Insert(new SplittedByCurrencyAccount(data.accountPM, this));
            //  this.entityPM.ConnectedItems = this.entityPM.ConnectedItems + data.accountPM.CurrencyCode + ",";
        }
    };
    GLAccountAdditionalDataTabComponent.prototype.Choose = function () {
        var _this = this;
        var args = {};
        args.GLAccount = this.entityPM;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Title = this.ObjectTableName + " Search";
        logitudeWindow.Show('./Accounting/Components/EditTabs/GLAccount/GLAccountSearchWindowComponent');
        logitudeWindow.ComponentLoaded.subscribe(function (comp) {
            logitudeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.OnSearchWindowClosed(comp);
                }
            });
        });
    };
    GLAccountAdditionalDataTabComponent.prototype.OnSearchWindowClosed = function (args) {
        if (args.ValidationErrorsList.length == 0) {
            this.BuildChildrenGLAccountsList();
        }
    };
    GLAccountAdditionalDataTabComponent.prototype.SetMouseHoverRow = function (item, isRowHover) {
        if (item) {
            item.IsRowHover = isRowHover;
        }
    };
    GLAccountAdditionalDataTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GLAccountAdditionalDataTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], GLAccountAdditionalDataTabComponent);
    return GLAccountAdditionalDataTabComponent;
}(BaseComponent_1.BaseComponent));
exports.GLAccountAdditionalDataTabComponent = GLAccountAdditionalDataTabComponent;
var SplittedByCurrencyAccount = /** @class */ (function (_super) {
    __extends(SplittedByCurrencyAccount, _super);
    function SplittedByCurrencyAccount(entity, Parent) {
        var _this = _super.call(this) || this;
        _this.GLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.entityPM = entity;
        _this.parent = Parent;
        return _this;
        //this.DisplayNumber = Parent.entityPM.DisplayNumber + "\\" + this.CurrencyCode;
    }
    Object.defineProperty(SplittedByCurrencyAccount.prototype, "CurrencyCode", {
        get: function () {
            return this.entityPM.CurrencyCode;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplittedByCurrencyAccount.prototype, "CurrencyId", {
        get: function () {
            return this.entityPM.CurrencyId;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplittedByCurrencyAccount.prototype, "DisplayNumber", {
        get: function () { return this.entityPM.DisplayNumber; },
        set: function (value) {
            this.entityPM.DisplayNumber = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplittedByCurrencyAccount.prototype, "Inactive", {
        get: function () {
            return this.entityPM.Inactive;
        },
        set: function (value) {
            this.entityPM.Inactive = value;
        },
        enumerable: true,
        configurable: true
    });
    SplittedByCurrencyAccount.prototype.InactiveChecked = function (checked) {
        var _this = this;
        if (checked) {
            this.entityPM.Inactive = true;
            this.entityPM.Type = "INACTIVE";
        }
        else {
            this.entityPM.Inactive = false;
            this.entityPM.Type = "ACTIVE";
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Saving"));
        this.GLAccountPMService.update(this.entityPM).subscribe(function (myResponse) {
            if (myResponse) {
                if (!myResponse.HasError) {
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    SplittedByCurrencyAccount.prototype.ConnectedGLAccountClicked = function (item) {
        this.EditEntity("GLAccount", this.entityPM.Id, null, "GAGC");
    };
    SplittedByCurrencyAccount.prototype.EditEntity = function (objectTableName, entityId, windowTitle, defaultSelectedTabCode) {
        var _this = this;
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(function (res) {
            _this.parent.BuildChildrenGLAccountsList();
        });
    };
    return SplittedByCurrencyAccount;
}(BaseComponent_1.BaseComponent));
exports.SplittedByCurrencyAccount = SplittedByCurrencyAccount;
var GLAccountChild = /** @class */ (function (_super) {
    __extends(GLAccountChild, _super);
    function GLAccountChild(entity, Parent) {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.GLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
        _this.DataContext = _this;
        _this.entityPM = entity;
        _this.parent = Parent;
        return _this;
    }
    Object.defineProperty(GLAccountChild.prototype, "DisplayNumber", {
        get: function () {
            return this.entityPM.DisplayNumber;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountChild.prototype, "LocalName", {
        get: function () {
            return this.entityPM.LocalName;
        },
        enumerable: true,
        configurable: true
    });
    GLAccountChild.prototype.GLAccountHyperlinkClicked = function (item) {
        this.EditEntity("GLAccount", this.entityPM.Id, null, "GAGC");
    };
    GLAccountChild.prototype.EditEntity = function (objectTableName, entityId, windowTitle, defaultSelectedTabCode) {
        var _this = this;
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(function (res) {
            _this.parent.BuildChildrenGLAccountsList();
        });
    };
    GLAccountChild.prototype.DisconnectClicked = function (item) {
        var _this = this;
        this.parent.glAccountExtendedListService.SetParentAccountId(this.entityPM.Id, "null," + this.entityPM.Id).subscribe(function (myResponse) {
            if (myResponse) {
                if (!myResponse.HasError) {
                    _this.parent.BuildChildrenGLAccountsList();
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    return GLAccountChild;
}(BaseComponent_1.BaseComponent));
exports.GLAccountChild = GLAccountChild;
//# sourceMappingURL=GLAccountAdditionalDataTabComponent.js.map