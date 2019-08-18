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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var GLAccountWithholdingTaxPM_1 = require("../../../EntityPMs/GLAccountWithholdingTaxPM");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var GLAccountTaxWithholdingTabComponent = /** @class */ (function (_super) {
    __extends(GLAccountTaxWithholdingTabComponent, _super);
    function GLAccountTaxWithholdingTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "GLAccount";
        _this.DataContext = _this;
        _this.Lines = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.FilterSelectedValue = 'Active';
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.InactiveFilter = null;
        _this.ValidationErrorsList = [];
        _this.EntityResourceService.getEntityResourceByTableName("GLAccountWithholdingTax").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("GLAccount").subscribe(function (response) {
                _this.visible = true;
                _this.entityPM = entityArgs.EntityPM;
                _this.InactiveFilter = false;
                _this.BuildGLAccountTaxWithholdingLinesList();
                _this.Listen();
            });
        });
        return _this;
    }
    GLAccountTaxWithholdingTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess && _this.CurrentSession.CurrentEditComponent) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildGLAccountTaxWithholdingLinesList();
                }
            }));
        }
    };
    GLAccountTaxWithholdingTabComponent.prototype.FilterItemClicked = function (filtervalue) {
        if (filtervalue == "Active") {
            this.InactiveFilter = false;
            this.BuildGLAccountTaxWithholdingLinesList();
            this.FilterSelectedValue = "Active";
        }
        else if (filtervalue == "Inactive") {
            this.InactiveFilter = true;
            this.BuildGLAccountTaxWithholdingLinesList();
            this.FilterSelectedValue = "Inactive";
        }
        else {
            this.InactiveFilter = null;
            this.BuildGLAccountTaxWithholdingLinesList();
            this.FilterSelectedValue = "All";
        }
    };
    Object.defineProperty(GLAccountTaxWithholdingTabComponent.prototype, "DeductionTypeId", {
        get: function () { return this.entityPM.DeductionTypeId; },
        set: function (value) {
            if (this.entityPM.DeductionTypeId != value) {
                this.entityPM.DeductionTypeId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTaxWithholdingTabComponent.prototype, "ConsolidationVat", {
        get: function () { return this.entityPM.ConsolidationVat; },
        set: function (value) {
            if (this.entityPM.ConsolidationVat != value) {
                this.entityPM.ConsolidationVat = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTaxWithholdingTabComponent.prototype, "DeductionFileNumber", {
        get: function () { return this.entityPM.DeductionFileNumber; },
        set: function (value) {
            if (this.entityPM.DeductionFileNumber != value) {
                this.entityPM.DeductionFileNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTaxWithholdingTabComponent.prototype, "DeductionFileTypeId", {
        get: function () { return this.entityPM.DeductionFileTypeId; },
        set: function (value) {
            if (this.entityPM.DeductionFileTypeId != value) {
                this.entityPM.DeductionFileTypeId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTaxWithholdingTabComponent.prototype, "AssessingOfficeCode", {
        get: function () { return this.entityPM.AssessingOfficeCode; },
        set: function (value) {
            if (this.entityPM.AssessingOfficeCode != value) {
                this.entityPM.AssessingOfficeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTaxWithholdingTabComponent.prototype, "Occupation", {
        get: function () { return this.entityPM.Occupation; },
        set: function (value) {
            if (this.entityPM.Occupation != value) {
                this.entityPM.Occupation = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    GLAccountTaxWithholdingTabComponent.prototype.BuildGLAccountTaxWithholdingLinesList = function () {
        var _this = this;
        var sequence = 0;
        this.Lines.Clear();
        if (this.InactiveFilter != null) {
            for (var _i = 0, _a = this.entityPM.GLAccountWithholdingTaxes.filter(function (d) { return d.Inactive == _this.InactiveFilter; }); _i < _a.length; _i++) {
                var item = _a[_i];
                sequence += 1;
                this.Lines.Insert(new GLAccountTaxLine(item, this, sequence));
            }
        }
        else {
            for (var _b = 0, _c = this.entityPM.GLAccountWithholdingTaxes; _b < _c.length; _b++) {
                var item = _c[_b];
                sequence += 1;
                this.Lines.Insert(new GLAccountTaxLine(item, this, sequence));
            }
        }
    };
    GLAccountTaxWithholdingTabComponent.prototype.Add = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var FIELD_IS_REQUIERD = null;
        FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.DeductionFileTypeId) || Tools_1.AppTool.IsNullOrEmpty(this.AssessingOfficeCode) || Tools_1.AppTool.IsNullOrEmpty(this.DeductionTypeId) || Tools_1.AppTool.IsNullOrEmpty(this.DeductionFileNumber)) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.DeductionFileTypeId)) {
                var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccount.F.DeductionFileTypeId"));
                this.ValidationErrorsList.push(s);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.AssessingOfficeCode)) {
                var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccount.F.AssessingOfficeCode"));
                this.ValidationErrorsList.push(s);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.DeductionTypeId)) {
                var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccount.F.DeductionTypeId"));
                this.ValidationErrorsList.push(s);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.DeductionFileNumber)) {
                var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccount.F.DeductionFileNumber"));
                this.ValidationErrorsList.push(s);
            }
        }
        else {
            //this.ValidationErrorsList = [];
            var activeItems = this.entityPM.GLAccountWithholdingTaxes.filter(function (d) { return !d.Inactive; });
            var items = activeItems.sort(function (a, b) { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1; });
            var item = items[this.Lines.Collection.length - 1];
            if (item) {
                this.LastLineToDate = item.ToDate;
            }
            var windowArgs = {};
            windowArgs.GLAccountWithholdinTax = new GLAccountWithholdingTaxPM_1.GLAccountWithholdingTaxPM(this.entityPM);
            windowArgs.GLAccount = this.entityPM;
            windowArgs.LastLineToDate = this.LastLineToDate;
            windowArgs.IsNew = true;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.TaxLineTitle");
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 400;
            logWindow.Height = 400;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.AddTaxLine(comp);
                    }
                });
            });
            logWindow.Show('./Accounting/Components/EditTabs/GLAccount/AddEditTaxWithholdingLineComponent');
        }
    };
    GLAccountTaxWithholdingTabComponent.prototype.AddTaxLine = function (s) {
        if (s) {
            var line = 0;
            var items = this.entityPM.GLAccountWithholdingTaxes.sort(function (a, b) { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1; });
            if (items.length == 0)
                line = 0;
            else {
                line = items[this.entityPM.GLAccountWithholdingTaxes.length - 1].LineNumber;
            }
            line += 1;
            //var item = items[this.Lines.Collection.length - 1];
            //if (item) {
            //    this.LastLineToDate = item.ToDate;
            //}
            var item = s.entity;
            item.GLAccountId = this.entityPM.Id;
            item.Tenant = this.entityPM.Tenant;
            item.LineNumber = line;
            item.CreateDate = new Date();
            item.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            item.ChangeSetOp = "Insert";
            this.LastLineToDate = s.ToDate;
            item.Inactive = false;
            this.entityPM.AddGLAccountWithholdingTax(item);
            if (this.FilterSelectedValue == "Active")
                this.InactiveFilter = false;
            else if (this.FilterSelectedValue == "Inactive")
                this.InactiveFilter = true;
            else
                this.InactiveFilter = null;
            this.BuildGLAccountTaxWithholdingLinesList();
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    GLAccountTaxWithholdingTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GLAccountTaxWithholdingTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], GLAccountTaxWithholdingTabComponent);
    return GLAccountTaxWithholdingTabComponent;
}(BaseComponent_1.BaseComponent));
exports.GLAccountTaxWithholdingTabComponent = GLAccountTaxWithholdingTabComponent;
var GLAccountTaxLine = /** @class */ (function (_super) {
    __extends(GLAccountTaxLine, _super);
    function GLAccountTaxLine(Entity, Parent, sequence) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "GLAccountWithholdingTax";
        _this.entity = Entity;
        _this.parent = Parent;
        _this.Sequence = sequence;
        _this.UIProperties.SetEnabled("Inactive", _this.ObjectTableName, false);
        return _this;
    }
    Object.defineProperty(GLAccountTaxLine.prototype, "LineNumber", {
        get: function () { return this.entity.LineNumber; },
        set: function (value) {
            if (this.entity.LineNumber != value) {
                this.entity.LineNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTaxLine.prototype, "FromDate", {
        get: function () { return this.entity.FromDate; },
        set: function (value) {
            if (this.entity.FromDate != value) {
                if (value) {
                    this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, null);
                    //var datetocompare = DateTool.GetDateParts(this.LastLineToDate).DateObject;
                    if ((this.entity.ToDate && value > this.entity.ToDate)) {
                        //var msg = new MessageWindow();
                        this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, "From date must be less than to date");
                        //msg.Show(TextCodeTranslator.Translate("GLAccounts.O.NotValidDate"));
                        this.entity.FromDate = null;
                    }
                    else {
                        this.entity.FromDate = value;
                        //  this.parent.LastLineToDate = value;
                    }
                }
                else {
                    this.entity.FromDate = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTaxLine.prototype, "ToDate", {
        get: function () { return this.entity.ToDate; },
        set: function (value) {
            if (this.entity.ToDate != value) {
                if (value) {
                    if (this.entity.FromDate && (this.entity.FromDate > value || this.entity.FromDate.valueOf() == value.valueOf())) {
                        this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, "To date must be larger than from date");
                        this.entity.ToDate = null;
                    }
                    else {
                        this.entity.ToDate = value;
                        //  this.LastLineToDate = value;
                    }
                }
                else {
                    this.entity.ToDate = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTaxLine.prototype, "Percentage", {
        get: function () { return this.entity.Percentage; },
        set: function (value) {
            if (this.entity.Percentage != value) {
                this.entity.Percentage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTaxLine.prototype, "Inactive", {
        get: function () { return this.entity.Inactive; },
        set: function (value) {
            if (this.entity.Inactive != value) {
                this.entity.Inactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    GLAccountTaxLine.prototype.EditItem = function (item) {
        var _this = this;
        var windowArgs = {};
        windowArgs.GLAccountWithholdinTax = this.entity;
        windowArgs.GLAccount = this.parent.entityPM;
        windowArgs.IsNew = false;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.EditTaxPeriod");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.EditTaxLine(comp);
                }
            });
        });
        logWindow.Show('./Accounting/Components/EditTabs/GLAccount/AddEditTaxWithholdingLineComponent');
    };
    GLAccountTaxLine.prototype.EditTaxLine = function (s) {
        if (s) {
            this.entity = s.entity;
            this.entity.ChangeSetOp = "Update";
            this.parent.BuildGLAccountTaxWithholdingLinesList();
            //   this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    return GLAccountTaxLine;
}(BaseComponent_1.BaseComponent));
exports.GLAccountTaxLine = GLAccountTaxLine;
//# sourceMappingURL=GLAccountTaxWithholdingTabComponent.js.map