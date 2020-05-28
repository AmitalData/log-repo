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
var TicketClassificationListService_1 = require("../Services/StandardLists/TicketClassificationListService");
var TicketClassificationPM_1 = require("../EntityPMs/TicketClassificationPM");
var TicketClassificationPMService_1 = require("../Services/StandardPMs/TicketClassificationPMService");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var Tools_1 = require("../../Infrastructure/Tools");
var ClassificationsTree = /** @class */ (function () {
    function ClassificationsTree() {
        this.Items = [];
        this.AllClassification = [];
        this.collapseAll = false;
    }
    Object.defineProperty(ClassificationsTree.prototype, "CollapseAll", {
        get: function () {
            return this.collapseAll;
        },
        set: function (value) {
            if (this.collapseAll != value) {
                this.collapseAll = value;
                if (this.collapseAll) {
                    this.Items.forEach(function (item) {
                        item.IsExpanded = false;
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ClassificationsTree.prototype.ngOnInit = function () {
        var _this = this;
        this.Items = [];
        if (this.Item == null) {
            var service = new TicketClassificationListService_1.TicketClassificationListService();
            service.getAll().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.AllClassification = myResponse.Result;
                    var level = 0;
                    if (_this.AllClassification.length > 1) {
                        level = 1;
                    }
                    _this.AllClassification.filter(function (f) { return f.ParentId == null; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; }).forEach(function (item) {
                        _this.Items.push(new ClassificationsTreeItem(item, _this.AllClassification, _this, level, true));
                    });
                }
            });
        }
        else {
            this.Item.AllClassification.filter(function (f) { return f.ParentId == _this.Item.Id; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; }).forEach(function (item) {
                _this.Items.push(new ClassificationsTreeItem(item, _this.Item.AllClassification, _this, _this.Item.Level + 1, true));
            });
        }
    };
    ClassificationsTree.prototype.BuildTreeView = function () {
        var _this = this;
        this.Items = [];
        var service = new TicketClassificationListService_1.TicketClassificationListService();
        service.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllClassification = myResponse.Result;
                if (_this.Item == null) {
                    var level = 0;
                    if (_this.AllClassification.length > 1) {
                        level = 1;
                    }
                    _this.AllClassification.filter(function (f) { return f.ParentId == null; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; }).forEach(function (item) {
                        _this.Items.push(new ClassificationsTreeItem(item, _this.AllClassification, _this, level, true));
                    });
                }
                else {
                    _this.Item.AllClassification = _this.AllClassification;
                    _this.Item.AllClassification.filter(function (f) { return f.ParentId == _this.Item.Id; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; }).forEach(function (item) {
                        _this.Items.push(new ClassificationsTreeItem(item, _this.Item.AllClassification, _this, _this.Item.Level + 1, true));
                    });
                }
            }
        });
    };
    Object.defineProperty(ClassificationsTree.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (this.selectedItem != value) {
                this.selectedItem = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ClassificationsTree = __decorate([
        core_1.Component({
            selector: 'ClassificationsTree',
            moduleId: module.id,
            templateUrl: './ClassificationsTree.html',
            inputs: ['Item', 'CollapseAll'],
        }),
        __metadata("design:paramtypes", [])
    ], ClassificationsTree);
    return ClassificationsTree;
}());
exports.ClassificationsTree = ClassificationsTree;
var ClassificationsTreeItem = /** @class */ (function () {
    function ClassificationsTreeItem(item, AllClassification, Father, level, isExpanded) {
        var _this = this;
        this.Father = Father;
        this.Level = 0;
        this.HasItems = false;
        this.NameColor = "black";
        this.AllClassification = [];
        this.isExpanded = false;
        this.expandAllOrNot = true;
        this.isSelected = false;
        this.entity = item;
        this.AllClassification = AllClassification;
        this.Level = level;
        this.HasItems = this.AllClassification.filter(function (f) { return f.ParentId == _this.Id; }).length > 0 ? true : false;
        this.IsExpanded = isExpanded;
        if (this.Inactive) {
            this.NameColor = "gray";
        }
    }
    Object.defineProperty(ClassificationsTreeItem.prototype, "Id", {
        get: function () { return this.entity.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationsTreeItem.prototype, "Name", {
        get: function () { return this.entity.Name; },
        set: function (value) { this.entity.Name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationsTreeItem.prototype, "ParentId", {
        get: function () { return this.entity.ParentId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationsTreeItem.prototype, "Inactive", {
        get: function () { return this.entity.Inactive; },
        set: function (value) { this.entity.Inactive = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationsTreeItem.prototype, "DefaultSeverityId", {
        get: function () { return this.entity.DefaultSeverityId; },
        set: function (value) { this.entity.DefaultSeverityId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationsTreeItem.prototype, "EmployeeGroupId", {
        get: function () { return this.entity.EmployeeGroupId; },
        set: function (value) { this.entity.EmployeeGroupId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationsTreeItem.prototype, "ManagerUserId", {
        get: function () { return this.entity.ManagerUserId; },
        set: function (value) { this.entity.ManagerUserId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationsTreeItem.prototype, "IsExpanded", {
        get: function () { return this.isExpanded; },
        set: function (value) {
            this.isExpanded = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationsTreeItem.prototype, "ExpandAllOrNot", {
        get: function () { return this.expandAllOrNot; },
        set: function (value) {
            this.expandAllOrNot = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationsTreeItem.prototype, "IsSelected", {
        get: function () { return this.isSelected; },
        set: function (value) {
            this.isSelected = value;
        },
        enumerable: true,
        configurable: true
    });
    // Commands 
    ClassificationsTreeItem.prototype.AddNewClassificationChild = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = "New Classification";
        var context = new ClassificationChildArgs(this.Id, null, true, this);
        logitudeWindow.DataContext = context;
        logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
        logitudeWindow.Show('./CRM/Components/Workspaces/AddEditClassificationComponent');
    };
    ClassificationsTreeItem.prototype.EditClassificationChild = function () {
        var _this = this;
        var service = new TicketClassificationPMService_1.TicketClassificationPMService();
        service.get(this.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var editEntityPM = myResponse.Result;
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Title = "Edit Classification";
                var context = new ClassificationChildArgs(editEntityPM.ParentId, editEntityPM, false, _this);
                logitudeWindow.DataContext = context;
                logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
                logitudeWindow.Show('./CRM/Components/Workspaces/AddEditClassificationComponent');
            }
        });
    };
    ClassificationsTreeItem.prototype.OnWindowClosed = function (arg) {
        if (arg == "OK") {
            this.Father.BuildTreeView();
        }
    };
    return ClassificationsTreeItem;
}());
var ClassificationChildArgs = /** @class */ (function (_super) {
    __extends(ClassificationChildArgs, _super);
    function ClassificationChildArgs(Id, entityPM, isNew, trigger) {
        var _this = _super.call(this) || this;
        _this.trigger = trigger;
        _this.ObjectTableName = "TicketClassification";
        _this.DataContext = _this;
        _this.isNew = isNew;
        if (isNew) {
            _this.entityPM = new TicketClassificationPM_1.TicketClassificationPM();
            _this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        }
        else {
            _this.entityPM = entityPM;
            //this.FillUsersList();
        }
        _this.ParentId = Id;
        return _this;
    }
    Object.defineProperty(ClassificationChildArgs.prototype, "Name", {
        //Properties
        get: function () { return this.entityPM.Name; },
        set: function (value) {
            this.entityPM.Name = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "Inactive", {
        get: function () { return this.entityPM.Inactive; },
        set: function (value) {
            this.entityPM.Inactive = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "EmployeeGroupId", {
        get: function () { return this.entityPM.EmployeeGroupId; },
        set: function (value) {
            if (this.entityPM.EmployeeGroupId != value) {
                this.entityPM.EmployeeGroupId = value;
                this.SetUIRequiredProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "DefaultSeverityId", {
        get: function () { return this.entityPM.DefaultSeverityId; },
        set: function (value) {
            if (this.entityPM.DefaultSeverityId != value) {
                this.entityPM.DefaultSeverityId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "ManagerUserId", {
        get: function () { return this.entityPM.ManagerUserId; },
        set: function (value) {
            if (this.entityPM.ManagerUserId != value) {
                this.entityPM.ManagerUserId = value;
                this.SetUIRequiredProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "EscalationNotify", {
        get: function () { return this.entityPM.EscalationNotify; },
        set: function (value) {
            if (this.entityPM.EscalationNotify != value) {
                this.entityPM.EscalationNotify = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "ParentId", {
        get: function () { return this.entityPM.ParentId; },
        set: function (value) {
            if (this.entityPM.ParentId != value) {
                this.entityPM.ParentId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ClassificationChildArgs.prototype.SetUIRequiredProperties = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Name) && this.Name == "General") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EmployeeGroupId))
                this.UIProperties.SetRequired("EmployeeGroupId", this.ObjectTableName, true);
        }
    };
    return ClassificationChildArgs;
}(BaseComponent_1.BaseComponent));
exports.ClassificationChildArgs = ClassificationChildArgs;
//# sourceMappingURL=ClassificationsTree.js.map